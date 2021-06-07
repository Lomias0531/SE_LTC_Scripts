using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_OxygenFarmDefinition), null)]
	public class MyOxygenFarmDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyOxygenFarmDefinition_003C_003EActor : IActivator, IActivator<MyOxygenFarmDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyOxygenFarmDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyOxygenFarmDefinition CreateInstance()
			{
				return new MyOxygenFarmDefinition();
			}

			MyOxygenFarmDefinition IActivator<MyOxygenFarmDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public MyStringHash ResourceSourceGroup;

		public Vector3 PanelOrientation;

		public bool IsTwoSided;

		public float PanelOffset;

		public MyDefinitionId ProducedGas;

		public float MaxGasOutput;

		public float OperationalPowerConsumption;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_OxygenFarmDefinition myObjectBuilder_OxygenFarmDefinition = builder as MyObjectBuilder_OxygenFarmDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_OxygenFarmDefinition.ResourceSinkGroup);
			ResourceSourceGroup = MyStringHash.GetOrCompute(myObjectBuilder_OxygenFarmDefinition.ResourceSourceGroup);
			PanelOrientation = myObjectBuilder_OxygenFarmDefinition.PanelOrientation;
			IsTwoSided = myObjectBuilder_OxygenFarmDefinition.TwoSidedPanel;
			PanelOffset = myObjectBuilder_OxygenFarmDefinition.PanelOffset;
			MyDefinitionId myDefinitionId = ProducedGas = ((!myObjectBuilder_OxygenFarmDefinition.ProducedGas.Id.IsNull()) ? ((MyDefinitionId)myObjectBuilder_OxygenFarmDefinition.ProducedGas.Id) : new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen"));
			MaxGasOutput = myObjectBuilder_OxygenFarmDefinition.ProducedGas.MaxOutputPerSecond;
			OperationalPowerConsumption = myObjectBuilder_OxygenFarmDefinition.OperationalPowerConsumption;
		}
	}
}
