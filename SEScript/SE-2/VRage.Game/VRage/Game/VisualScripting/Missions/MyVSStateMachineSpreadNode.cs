using VRage.Collections;
using VRage.Generics;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
	public class MyVSStateMachineSpreadNode : MyStateMachineNode
	{
		public MyVSStateMachineSpreadNode(string nodeName)
			: base(nodeName)
		{
		}

		protected override void ExpandInternal(MyStateMachineCursor cursor, MyConcurrentHashSet<MyStringId> enquedActions, int passThrough)
		{
			if (OutTransitions.Count != 0)
			{
				MyStateMachine stateMachine = cursor.StateMachine;
				stateMachine.DeleteCursor(cursor.Id);
				for (int i = 0; i < OutTransitions.Count; i++)
				{
					stateMachine.CreateCursor(OutTransitions[i].TargetNode.Name);
				}
			}
		}
	}
}
