using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BehaviorTreeDefinition), null)]
	public class MyBehaviorDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyBehaviorDefinition_003C_003EActor : IActivator, IActivator<MyBehaviorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBehaviorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBehaviorDefinition CreateInstance()
			{
				return new MyBehaviorDefinition();
			}

			MyBehaviorDefinition IActivator<MyBehaviorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyObjectBuilder_BehaviorTreeNode FirstNode;

		public string Behavior;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BehaviorTreeDefinition myObjectBuilder_BehaviorTreeDefinition = (MyObjectBuilder_BehaviorTreeDefinition)builder;
			FirstNode = myObjectBuilder_BehaviorTreeDefinition.FirstNode;
			Behavior = myObjectBuilder_BehaviorTreeDefinition.Behavior;
		}
	}
}
