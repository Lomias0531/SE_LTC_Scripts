using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WindTurbineDefinition), null)]
	public class MyWindTurbineDefinition : MyPowerProducerDefinition
	{
		private class Sandbox_Definitions_MyWindTurbineDefinition_003C_003EActor : IActivator, IActivator<MyWindTurbineDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWindTurbineDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWindTurbineDefinition CreateInstance()
			{
				return new MyWindTurbineDefinition();
			}

			MyWindTurbineDefinition IActivator<MyWindTurbineDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int RaycasterSize;

		public int RaycastersCount;

		public float MinRaycasterClearance;

		public float OptimalGroundClearance;

		public float RaycastersToFullEfficiency;

		public float OptimalWindSpeed;

		public float TurbineSpinUpSpeed;

		public float TurbineSpinDownSpeed;

		public float TurbineRotationSpeed;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WindTurbineDefinition myObjectBuilder_WindTurbineDefinition = (MyObjectBuilder_WindTurbineDefinition)builder;
			RaycasterSize = myObjectBuilder_WindTurbineDefinition.RaycasterSize;
			RaycastersCount = myObjectBuilder_WindTurbineDefinition.RaycastersCount;
			MinRaycasterClearance = myObjectBuilder_WindTurbineDefinition.MinRaycasterClearance;
			RaycastersToFullEfficiency = myObjectBuilder_WindTurbineDefinition.RaycastersToFullEfficiency;
			OptimalWindSpeed = myObjectBuilder_WindTurbineDefinition.OptimalWindSpeed;
			TurbineSpinUpSpeed = myObjectBuilder_WindTurbineDefinition.TurbineSpinUpSpeed;
			TurbineSpinDownSpeed = myObjectBuilder_WindTurbineDefinition.TurbineSpinDownSpeed;
			TurbineRotationSpeed = myObjectBuilder_WindTurbineDefinition.TurbineRotationSpeed;
			OptimalGroundClearance = myObjectBuilder_WindTurbineDefinition.OptimalGroundClearance;
		}
	}
}
