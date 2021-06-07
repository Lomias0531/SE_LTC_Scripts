using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.AI.BehaviorTree
{
	[MyBehaviorTreeNodeType(typeof(MyObjectBuilder_BehaviorTreeSequenceNode), typeof(MyBehaviorTreeControlNodeMemory))]
	internal class MyBehaviorTreeSequenceNode : MyBehaviorTreeControlBaseNode
	{
		private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeSequenceNode_003C_003EActor : IActivator, IActivator<MyBehaviorTreeSequenceNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyBehaviorTreeSequenceNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBehaviorTreeSequenceNode CreateInstance()
			{
				return new MyBehaviorTreeSequenceNode();
			}

			MyBehaviorTreeSequenceNode IActivator<MyBehaviorTreeSequenceNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override MyBehaviorTreeState SearchedValue => MyBehaviorTreeState.FAILURE;

		public override MyBehaviorTreeState FinalValue => MyBehaviorTreeState.SUCCESS;

		public override string DebugSign => "->";

		public override string ToString()
		{
			return "SEQ: " + base.ToString();
		}
	}
}
