using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GasFueledPowerProducerDefinition), null)]
	public class MyGasFueledPowerProducerDefinition : MyFueledPowerProducerDefinition
	{
		public struct FuelInfo
		{
			public readonly float Ratio;

			public readonly MyDefinitionId FuelId;

			public FuelInfo(MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo fuelInfo)
			{
				FuelId = fuelInfo.Id;
				Ratio = fuelInfo.Ratio;
			}
		}

		private class Sandbox_Definitions_MyGasFueledPowerProducerDefinition_003C_003EActor : IActivator, IActivator<MyGasFueledPowerProducerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGasFueledPowerProducerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGasFueledPowerProducerDefinition CreateInstance()
			{
				return new MyGasFueledPowerProducerDefinition();
			}

			MyGasFueledPowerProducerDefinition IActivator<MyGasFueledPowerProducerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public FuelInfo Fuel;

		public float FuelCapacity;

		public MyStringHash ResourceSinkGroup;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			MyObjectBuilder_GasFueledPowerProducerDefinition myObjectBuilder_GasFueledPowerProducerDefinition = (MyObjectBuilder_GasFueledPowerProducerDefinition)builder;
			base.Init(builder);
			FuelCapacity = myObjectBuilder_GasFueledPowerProducerDefinition.FuelCapacity;
			Fuel = new FuelInfo(myObjectBuilder_GasFueledPowerProducerDefinition.FuelInfos[0]);
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_GasFueledPowerProducerDefinition.ResourceSinkGroup);
		}
	}
}
