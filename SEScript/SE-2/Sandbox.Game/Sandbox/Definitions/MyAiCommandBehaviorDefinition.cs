using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AiCommandBehaviorDefinition), null)]
	public class MyAiCommandBehaviorDefinition : MyAiCommandDefinition
	{
		private class Sandbox_Definitions_MyAiCommandBehaviorDefinition_003C_003EActor : IActivator, IActivator<MyAiCommandBehaviorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAiCommandBehaviorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAiCommandBehaviorDefinition CreateInstance()
			{
				return new MyAiCommandBehaviorDefinition();
			}

			MyAiCommandBehaviorDefinition IActivator<MyAiCommandBehaviorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string BehaviorTreeName;

		public MyAiCommandEffect CommandEffect;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AiCommandBehaviorDefinition myObjectBuilder_AiCommandBehaviorDefinition = builder as MyObjectBuilder_AiCommandBehaviorDefinition;
			BehaviorTreeName = myObjectBuilder_AiCommandBehaviorDefinition.BehaviorTreeName;
			CommandEffect = myObjectBuilder_AiCommandBehaviorDefinition.CommandEffect;
		}
	}
}
