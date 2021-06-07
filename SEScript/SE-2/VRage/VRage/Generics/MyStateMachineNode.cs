using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;

namespace VRage.Generics
{
	/// <summary>
	/// Node of the state machine.
	/// </summary>
	public class MyStateMachineNode
	{
		private readonly string m_name;

		protected internal List<MyStateMachineTransition> OutTransitions = new List<MyStateMachineTransition>();

		protected internal List<MyStateMachineTransition> InTransitions = new List<MyStateMachineTransition>();

		protected internal HashSet<MyStateMachineCursor> Cursors = new HashSet<MyStateMachineCursor>();

		public bool PassThrough;

		public string Name => m_name;

		public MyStateMachineNode(string name)
		{
			m_name = name;
		}

		internal void TransitionAdded(MyStateMachineTransition transition)
		{
			TransitionAddedInternal(transition);
		}

		/// <summary>
		/// Called after Transition is added.
		/// Override for custom behavior.
		/// </summary>
		protected virtual void TransitionAddedInternal(MyStateMachineTransition transition)
		{
		}

		internal void TransitionRemoved(MyStateMachineTransition transition)
		{
			TransitionRemovedInternal(transition);
		}

		/// <summary>
		/// Called before Transition remove.
		/// Override for custom behavior.
		/// </summary>
		protected virtual void TransitionRemovedInternal(MyStateMachineTransition transition)
		{
		}

		internal void Expand(MyStateMachineCursor cursor, MyConcurrentHashSet<MyStringId> enquedActions)
		{
			ExpandInternal(cursor, enquedActions, 100);
		}

		/// <summary>
		/// Expands current node with given cursor.
		/// First enquedAction is taking place then any valid transition.
		/// Cursor is being transitioned to result of expansion.
		/// Override this for custom behavior.
		/// </summary>
		protected virtual void ExpandInternal(MyStateMachineCursor cursor, MyConcurrentHashSet<MyStringId> enquedActions, int passThrough)
		{
			MyStateMachineTransition myStateMachineTransition;
			do
			{
				myStateMachineTransition = null;
				List<MyStateMachineTransition> outTransitions = cursor.Node.OutTransitions;
				MyStringId action = MyStringId.NullOrEmpty;
				if (enquedActions.Count > 0)
				{
					int num = int.MaxValue;
					for (int i = 0; i < outTransitions.Count; i++)
					{
						int num2 = outTransitions[i].Priority ?? int.MaxValue;
						enquedActions.Contains(outTransitions[i].Name);
						bool flag = false;
						foreach (MyStringId enquedAction in enquedActions)
						{
							if (enquedAction.String.ToLower() == outTransitions[i].Name.ToString().ToLower())
							{
								flag = true;
							}
						}
						if (flag && num2 <= num && (outTransitions[i].Conditions.Count == 0 || outTransitions[i].Evaluate()))
						{
							myStateMachineTransition = outTransitions[i];
							num = num2;
							action = outTransitions[i].Name;
						}
					}
				}
				if (myStateMachineTransition == null)
				{
					myStateMachineTransition = cursor.Node.QueryNextTransition();
					foreach (MyStringId enquedAction2 in enquedActions)
					{
						action = enquedAction2;
					}
				}
				if (myStateMachineTransition != null)
				{
					cursor.FollowTransition(myStateMachineTransition, action);
				}
			}
			while (myStateMachineTransition != null && cursor.Node.PassThrough && passThrough-- > 0);
		}

		protected virtual MyStateMachineTransition QueryNextTransition()
		{
			for (int i = 0; i < OutTransitions.Count; i++)
			{
				if (OutTransitions[i].Evaluate())
				{
					return OutTransitions[i];
				}
			}
			return null;
		}

		public virtual void OnUpdate(MyStateMachine stateMachine)
		{
		}
	}
}
