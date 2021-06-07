using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FueledPowerProducerDefinition), null)]
	public class MyFueledPowerProducerDefinition : MyPowerProducerDefinition
	{
		private class Sandbox_Definitions_MyFueledPowerProducerDefinition_003C_003EActor : IActivator, IActivator<MyFueledPowerProducerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFueledPowerProducerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFueledPowerProducerDefinition CreateInstance()
			{
				return new MyFueledPowerProducerDefinition();
			}

			MyFueledPowerProducerDefinition IActivator<MyFueledPowerProducerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float FuelProductionToCapacityMultiplier;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			MyObjectBuilder_FueledPowerProducerDefinition myObjectBuilder_FueledPowerProducerDefinition = (MyObjectBuilder_FueledPowerProducerDefinition)builder;
			FuelProductionToCapacityMultiplier = myObjectBuilder_FueledPowerProducerDefinition.FuelProductionToCapacityMultiplier;
			base.Init(builder);
		}
	}
}
