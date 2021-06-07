using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.AI.BehaviorTree
{
	[MyBehaviorTreeNodeType(typeof(MyObjectBuilder_BehaviorTreeSelectorNode), typeof(MyBehaviorTreeControlNodeMemory))]
	internal class MyBehaviorTreeSelectorNode : MyBehaviorTreeControlBaseNode
	{
		private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeSelectorNode_003C_003EActor : IActivator, IActivator<MyBehaviorTreeSelectorNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyBehaviorTreeSelectorNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBehaviorTreeSelectorNode CreateInstance()
			{
				return new MyBehaviorTreeSelectorNode();
			}

			MyBehaviorTreeSelectorNode IActivator<MyBehaviorTreeSelectorNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override MyBehaviorTreeState SearchedValue => MyBehaviorTreeState.SUCCESS;

		public override MyBehaviorTreeState FinalValue => MyBehaviorTreeState.FAILURE;

		public override string DebugSign => "?";

		public override string ToString()
		{
			return "SEL: " + base.ToString();
		}
	}
}
