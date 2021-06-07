using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AiCommandDefinition), null)]
	public class MyAiCommandDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyAiCommandDefinition_003C_003EActor : IActivator, IActivator<MyAiCommandDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAiCommandDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAiCommandDefinition CreateInstance()
			{
				return new MyAiCommandDefinition();
			}

			MyAiCommandDefinition IActivator<MyAiCommandDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
		}
	}
}
