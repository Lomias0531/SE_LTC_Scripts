using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;

namespace VRage.Generics
{
	/// <summary>
	/// Implementation of generic multistate state machine. Is able to run multiple independent cursors
	/// at once updated with every update call. Use cursors as access point to active states.
	/// </summary>
	public class MyStateMachine
	{
		private int m_transitionIdCounter;

		protected Dictionary<string, MyStateMachineNode> m_nodes = new Dictionary<string, MyStateMachineNode>();

		protected Dictionary<int, MyStateMachineTransitionWithStart> m_transitions = new Dictionary<int, MyStateMachineTransitionWithStart>();

		protected Dictionary<int, MyStateMachineCursor> m_activeCursorsById = new Dictionary<int, MyStateMachineCursor>();

		protected CachingList<MyStateMachineCursor> m_activeCursors = new CachingList<MyStateMachineCursor>();

		protected MyConcurrentHashSet<MyStringId> m_enqueuedActions = new MyConcurrentHashSet<MyStringId>();

		public DictionaryReader<string, MyStateMachineNode> AllNodes => m_nodes;

		public List<MyStateMachineCursor> ActiveCursors
		{
			get
			{
				if (m_activeCursors.HasChanges)
				{
					return m_activeCursors.CopyWithChanges();
				}
				return new List<MyStateMachineCursor>(m_activeCursors);
			}
		}

		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Creates new active cursor.
		/// </summary>
		public virtual MyStateMachineCursor CreateCursor(string nodeName)
		{
			MyStateMachineNode myStateMachineNode = FindNode(nodeName);
			if (myStateMachineNode != null)
			{
				MyStateMachineCursor myStateMachineCursor = new MyStateMachineCursor(myStateMachineNode, this);
				m_activeCursorsById.Add(myStateMachineCursor.Id, myStateMachineCursor);
				m_activeCursors.Add(myStateMachineCursor);
				return myStateMachineCursor;
			}
			return null;
		}

		public MyStateMachineCursor FindCursor(int cursorId)
		{
			m_activeCursorsById.TryGetValue(cursorId, out MyStateMachineCursor value);
			return value;
		}

		public virtual bool DeleteCursor(int id)
		{
			if (!m_activeCursorsById.ContainsKey(id))
			{
				return false;
			}
			MyStateMachineCursor entity = m_activeCursorsById[id];
			m_activeCursorsById.Remove(id);
			m_activeCursors.Remove(entity);
			return true;
		}

		public virtual bool AddNode(MyStateMachineNode newNode)
		{
			if (FindNode(newNode.Name) != null)
			{
				return false;
			}
			m_nodes.Add(newNode.Name, newNode);
			return true;
		}

		public MyStateMachineNode FindNode(string nodeName)
		{
			m_nodes.TryGetValue(nodeName, out MyStateMachineNode value);
			return value;
		}

		public virtual bool DeleteNode(string nodeName)
		{
			m_nodes.TryGetValue(nodeName, out MyStateMachineNode rtnNode);
			if (rtnNode == null)
			{
				return false;
			}
			foreach (KeyValuePair<string, MyStateMachineNode> node in m_nodes)
			{
				node.Value.OutTransitions.RemoveAll((MyStateMachineTransition x) => x.TargetNode == rtnNode);
			}
			m_nodes.Remove(nodeName);
			int num = 0;
			while (num < m_activeCursors.Count)
			{
				if (m_activeCursors[num].Node.Name == nodeName)
				{
					m_activeCursors[num].Node = null;
					m_activeCursorsById.Remove(m_activeCursors[num].Id);
					m_activeCursors.Remove(m_activeCursors[num]);
				}
			}
			return true;
		}

		public virtual MyStateMachineTransition AddTransition(string startNodeName, string endNodeName, MyStateMachineTransition existingInstance = null, string name = null)
		{
			MyStateMachineNode myStateMachineNode = FindNode(startNodeName);
			MyStateMachineNode myStateMachineNode2 = FindNode(endNodeName);
			if (myStateMachineNode == null || myStateMachineNode2 == null)
			{
				return null;
			}
			MyStateMachineTransition myStateMachineTransition;
			if (existingInstance == null)
			{
				myStateMachineTransition = new MyStateMachineTransition();
				if (name != null)
				{
					myStateMachineTransition.Name = MyStringId.GetOrCompute(name);
				}
			}
			else
			{
				myStateMachineTransition = existingInstance;
			}
			m_transitionIdCounter++;
			myStateMachineTransition._SetId(m_transitionIdCounter);
			myStateMachineTransition.TargetNode = myStateMachineNode2;
			myStateMachineNode.OutTransitions.Add(myStateMachineTransition);
			myStateMachineNode2.InTransitions.Add(myStateMachineTransition);
			m_transitions.Add(m_transitionIdCounter, new MyStateMachineTransitionWithStart(myStateMachineNode, myStateMachineTransition));
			myStateMachineNode.TransitionAdded(myStateMachineTransition);
			myStateMachineNode2.TransitionAdded(myStateMachineTransition);
			return myStateMachineTransition;
		}

		public MyStateMachineTransition FindTransition(int transitionId)
		{
			return FindTransitionWithStart(transitionId).Transition;
		}

		public MyStateMachineTransitionWithStart FindTransitionWithStart(int transitionId)
		{
			m_transitions.TryGetValue(transitionId, out MyStateMachineTransitionWithStart value);
			return value;
		}

		public virtual bool DeleteTransition(int transitionId)
		{
			if (!m_transitions.TryGetValue(transitionId, out MyStateMachineTransitionWithStart value))
			{
				return false;
			}
			value.StartNode.TransitionRemoved(value.Transition);
			value.Transition.TargetNode.TransitionRemoved(value.Transition);
			m_transitions.Remove(transitionId);
			value.StartNode.OutTransitions.Remove(value.Transition);
			value.Transition.TargetNode.InTransitions.Remove(value.Transition);
			return true;
		}

		/// <summary>
		/// Set the current state. Warning - this is not a thing that you would like to normally do, 
		/// state machine should live its own life (based on transition condition).
		/// Returns true on success.
		/// </summary>
		public virtual bool SetState(int cursorId, string nameOfNewState)
		{
			MyStateMachineNode myStateMachineNode = FindNode(nameOfNewState);
			MyStateMachineCursor myStateMachineCursor = FindCursor(cursorId);
			if (myStateMachineNode != null)
			{
				myStateMachineCursor.Node = myStateMachineNode;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Update the state machine. Transition to new states.
		/// </summary>
		public virtual void Update()
		{
			m_activeCursors.ApplyChanges();
			if (m_activeCursorsById.Count == 0)
			{
				m_enqueuedActions.Clear();
				return;
			}
			foreach (MyStateMachineCursor activeCursor in m_activeCursors)
			{
				activeCursor.Node.Expand(activeCursor, m_enqueuedActions);
				activeCursor.Node.OnUpdate(this);
			}
			m_enqueuedActions.Clear();
		}

		/// <summary>
		/// Trigger an action in this layer. 
		/// If there is a transition having given (non-null) name, it is followed immediatelly.
		/// Conditions of transition are ignored.
		/// </summary>
		public void TriggerAction(MyStringId actionName)
		{
			m_enqueuedActions.Add(actionName);
		}

		/// <summary>
		/// Sort the transitions between states according to their priorities.
		/// </summary>
		public void SortTransitions()
		{
			foreach (MyStateMachineNode value2 in m_nodes.Values)
			{
				value2.OutTransitions.Sort(delegate(MyStateMachineTransition transition1, MyStateMachineTransition transition2)
				{
					int num = transition1.Priority ?? int.MaxValue;
					int value = transition2.Priority ?? int.MaxValue;
					return num.CompareTo(value);
				});
			}
		}
	}
}
