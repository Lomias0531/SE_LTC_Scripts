using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AnimalBotDefinition), null)]
	public class MyAnimalBotDefinition : MyAgentDefinition
	{
		private class Sandbox_Definitions_MyAnimalBotDefinition_003C_003EActor : IActivator, IActivator<MyAnimalBotDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAnimalBotDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAnimalBotDefinition CreateInstance()
			{
				return new MyAnimalBotDefinition();
			}

			MyAnimalBotDefinition IActivator<MyAnimalBotDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
