using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.ScriptBuilder;
using VRage.Generics;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
	public class MyVSStateMachine : MyStateMachine
	{
		private MyObjectBuilder_ScriptSM m_objectBuilder;

		private readonly MyConcurrentHashSet<MyStringId> m_cachedActions = new MyConcurrentHashSet<MyStringId>();

		private readonly List<MyStateMachineCursor> m_cursorsToInit = new List<MyStateMachineCursor>();

		private readonly List<MyStateMachineCursor> m_cursorsToDeserialize = new List<MyStateMachineCursor>();

		private long m_ownerId;

		public int ActiveCursorCount => m_activeCursors.Count;

		public long OwnerId
		{
			get
			{
				return m_ownerId;
			}
			set
			{
				foreach (MyStateMachineNode value2 in m_nodes.Values)
				{
					MyVSStateMachineNode myVSStateMachineNode = value2 as MyVSStateMachineNode;
					if (myVSStateMachineNode != null && myVSStateMachineNode.ScriptInstance != null)
					{
						myVSStateMachineNode.ScriptInstance.OwnerId = value;
					}
				}
				m_ownerId = value;
			}
		}

		public event Action<MyVSStateMachineNode, MyVSStateMachineNode> CursorStateChanged;

		public MyStateMachineCursor RestoreCursor(string nodeName)
		{
			foreach (MyStateMachineCursor value in m_activeCursorsById.Values)
			{
				if (value.Node.Name == nodeName)
				{
					return null;
				}
			}
			MyStateMachineCursor myStateMachineCursor = base.CreateCursor(nodeName);
			if (myStateMachineCursor != null)
			{
				myStateMachineCursor.OnCursorStateChanged += OnCursorStateChanged;
				if (myStateMachineCursor.Node is MyVSStateMachineNode)
				{
					m_cursorsToDeserialize.Add(myStateMachineCursor);
				}
			}
			return myStateMachineCursor;
		}

		public override MyStateMachineCursor CreateCursor(string nodeName)
		{
			foreach (MyStateMachineCursor value in m_activeCursorsById.Values)
			{
				if (value.Node.Name == nodeName)
				{
					return null;
				}
			}
			MyStateMachineCursor myStateMachineCursor = base.CreateCursor(nodeName);
			if (myStateMachineCursor != null)
			{
				myStateMachineCursor.OnCursorStateChanged += OnCursorStateChanged;
				if (myStateMachineCursor.Node is MyVSStateMachineNode)
				{
					m_cursorsToInit.Add(myStateMachineCursor);
				}
			}
			return myStateMachineCursor;
		}

		private void OnCursorStateChanged(int transitionId, MyStringId action, MyStateMachineNode node, MyStateMachine stateMachine)
		{
			MyVSStateMachineNode myVSStateMachineNode = FindTransitionWithStart(transitionId).StartNode as MyVSStateMachineNode;
			myVSStateMachineNode?.DisposeScript();
			MyVSStateMachineNode myVSStateMachineNode2 = node as MyVSStateMachineNode;
			myVSStateMachineNode2?.ActivateScript();
			NotifyStateChanged(myVSStateMachineNode, myVSStateMachineNode2);
		}

		public override void Update()
		{
			m_activeCursors.ApplyChanges();
			foreach (MyStateMachineCursor item in m_cursorsToDeserialize)
			{
				(item.Node as MyVSStateMachineNode)?.ActivateScript(restored: true);
			}
			m_cursorsToDeserialize.Clear();
			foreach (MyStateMachineCursor item2 in m_cursorsToInit)
			{
				(item2.Node as MyVSStateMachineNode)?.ActivateScript();
			}
			m_cursorsToInit.Clear();
			foreach (MyStringId cachedAction in m_cachedActions)
			{
				m_enqueuedActions.Add(cachedAction);
			}
			m_cachedActions.Clear();
			base.Update();
		}

		public void Init(MyObjectBuilder_ScriptSM ob, long? ownerId = null)
		{
			m_objectBuilder = ob;
			base.Name = ob.Name;
			if (ob.Nodes != null)
			{
				MyObjectBuilder_ScriptSMNode[] nodes = ob.Nodes;
				foreach (MyObjectBuilder_ScriptSMNode myObjectBuilder_ScriptSMNode in nodes)
				{
					MyStateMachineNode newNode;
					if (myObjectBuilder_ScriptSMNode is MyObjectBuilder_ScriptSMFinalNode)
					{
						newNode = new MyVSStateMachineFinalNode(myObjectBuilder_ScriptSMNode.Name);
					}
					else if (myObjectBuilder_ScriptSMNode is MyObjectBuilder_ScriptSMSpreadNode)
					{
						newNode = new MyVSStateMachineSpreadNode(myObjectBuilder_ScriptSMNode.Name);
					}
					else if (myObjectBuilder_ScriptSMNode is MyObjectBuilder_ScriptSMBarrierNode)
					{
						newNode = new MyVSStateMachineBarrierNode(myObjectBuilder_ScriptSMNode.Name);
					}
					else
					{
						Type type = MyVSAssemblyProvider.GetType("VisualScripting.CustomScripts." + myObjectBuilder_ScriptSMNode.ScriptClassName);
						MyVSStateMachineNode myVSStateMachineNode = new MyVSStateMachineNode(myObjectBuilder_ScriptSMNode.Name, type);
						if (myVSStateMachineNode.ScriptInstance != null)
						{
							if (!ownerId.HasValue)
							{
								myVSStateMachineNode.ScriptInstance.OwnerId = ob.OwnerId;
							}
							else
							{
								myVSStateMachineNode.ScriptInstance.OwnerId = ownerId.Value;
							}
						}
						newNode = myVSStateMachineNode;
					}
					AddNode(newNode);
				}
			}
			if (ob.Transitions != null)
			{
				MyObjectBuilder_ScriptSMTransition[] transitions = ob.Transitions;
				foreach (MyObjectBuilder_ScriptSMTransition myObjectBuilder_ScriptSMTransition in transitions)
				{
					AddTransition(myObjectBuilder_ScriptSMTransition.From, myObjectBuilder_ScriptSMTransition.To, null, myObjectBuilder_ScriptSMTransition.Name);
				}
			}
			if (ob.Cursors != null)
			{
				MyObjectBuilder_ScriptSMCursor[] cursors = ob.Cursors;
				foreach (MyObjectBuilder_ScriptSMCursor myObjectBuilder_ScriptSMCursor in cursors)
				{
					CreateCursor(myObjectBuilder_ScriptSMCursor.NodeName);
				}
			}
		}

		public MyObjectBuilder_ScriptSM GetObjectBuilder()
		{
			IReadOnlyList<MyStateMachineCursor> readOnlyList = m_activeCursors;
			if (m_activeCursors.HasChanges)
			{
				readOnlyList = m_activeCursors.CopyWithChanges();
			}
			m_objectBuilder.Cursors = new MyObjectBuilder_ScriptSMCursor[readOnlyList.Count];
			m_objectBuilder.OwnerId = m_ownerId;
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				m_objectBuilder.Cursors[i] = new MyObjectBuilder_ScriptSMCursor
				{
					NodeName = readOnlyList[i].Node.Name
				};
			}
			return m_objectBuilder;
		}

		public void Dispose()
		{
			m_activeCursors.ApplyChanges();
			for (int i = 0; i < m_activeCursors.Count; i++)
			{
				(m_activeCursors[i].Node as MyVSStateMachineNode)?.DisposeScript();
				DeleteCursor(m_activeCursors[i].Id);
			}
			m_activeCursors.ApplyChanges();
			m_activeCursors.Clear();
		}

		public void TriggerCachedAction(MyStringId actionName)
		{
			m_cachedActions.Add(actionName);
		}

		private void NotifyStateChanged(MyVSStateMachineNode from, MyVSStateMachineNode to)
		{
			if (this.CursorStateChanged != null)
			{
				this.CursorStateChanged(from, to);
			}
		}
	}
}
