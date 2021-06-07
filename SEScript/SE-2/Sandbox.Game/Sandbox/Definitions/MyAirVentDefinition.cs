using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AirVentDefinition), null)]
	public class MyAirVentDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyAirVentDefinition_003C_003EActor : IActivator, IActivator<MyAirVentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAirVentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAirVentDefinition CreateInstance()
			{
				return new MyAirVentDefinition();
			}

			MyAirVentDefinition IActivator<MyAirVentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public MyStringHash ResourceSourceGroup;

		public float StandbyPowerConsumption;

		public float OperationalPowerConsumption;

		public float VentilationCapacityPerSecond;

		public MySoundPair PressurizeSound;

		public MySoundPair DepressurizeSound;

		public MySoundPair IdleSound;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AirVentDefinition myObjectBuilder_AirVentDefinition = builder as MyObjectBuilder_AirVentDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_AirVentDefinition.ResourceSinkGroup);
			ResourceSourceGroup = MyStringHash.GetOrCompute(myObjectBuilder_AirVentDefinition.ResourceSourceGroup);
			StandbyPowerConsumption = myObjectBuilder_AirVentDefinition.StandbyPowerConsumption;
			OperationalPowerConsumption = myObjectBuilder_AirVentDefinition.OperationalPowerConsumption;
			VentilationCapacityPerSecond = myObjectBuilder_AirVentDefinition.VentilationCapacityPerSecond;
			PressurizeSound = new MySoundPair(myObjectBuilder_AirVentDefinition.PressurizeSound);
			DepressurizeSound = new MySoundPair(myObjectBuilder_AirVentDefinition.DepressurizeSound);
			IdleSound = new MySoundPair(myObjectBuilder_AirVentDefinition.IdleSound);
		}
	}
}
