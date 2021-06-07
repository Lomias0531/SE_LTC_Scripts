using System.Collections.Generic;
using VRage.Collections;
using VRage.Generics;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
	public class MyVSStateMachineBarrierNode : MyStateMachineNode
	{
		private readonly List<bool> m_cursorsFromInEdgesReceived = new List<bool>();

		public MyVSStateMachineBarrierNode(string name)
			: base(name)
		{
		}

		protected override void ExpandInternal(MyStateMachineCursor cursor, MyConcurrentHashSet<MyStringId> enquedActions, int passThrough)
		{
			MyStateMachine stateMachine = cursor.StateMachine;
			int i;
			for (i = 0; i < InTransitions.Count && InTransitions[i].Id != cursor.LastTransitionTakenId; i++)
			{
			}
			m_cursorsFromInEdgesReceived[i] = true;
			stateMachine.DeleteCursor(cursor.Id);
			foreach (bool item in m_cursorsFromInEdgesReceived)
			{
				if (!item)
				{
					return;
				}
			}
			if (OutTransitions.Count > 0)
			{
				stateMachine.CreateCursor(OutTransitions[0].TargetNode.Name);
			}
		}

		protected override void TransitionAddedInternal(MyStateMachineTransition transition)
		{
			if (transition.TargetNode == this)
			{
				m_cursorsFromInEdgesReceived.Add(item: false);
			}
		}

		protected override void TransitionRemovedInternal(MyStateMachineTransition transition)
		{
			if (transition.TargetNode == this)
			{
				int index = InTransitions.IndexOf(transition);
				m_cursorsFromInEdgesReceived.RemoveAt(index);
			}
		}
	}
}
