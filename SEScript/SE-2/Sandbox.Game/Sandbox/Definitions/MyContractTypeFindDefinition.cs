using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractTypeFindDefinition), null)]
	public class MyContractTypeFindDefinition : MyContractTypeDefinition
	{
		private class Sandbox_Definitions_MyContractTypeFindDefinition_003C_003EActor : IActivator, IActivator<MyContractTypeFindDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractTypeFindDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractTypeFindDefinition CreateInstance()
			{
				return new MyContractTypeFindDefinition();
			}

			MyContractTypeFindDefinition IActivator<MyContractTypeFindDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public double MaxGridDistance;

		public double MinGridDistance;

		public double MaxGpsOffset;

		public double TriggerRadius;

		public double Duration_BaseTime;

		public double Duration_TimePerMeter;

		public double Duration_TimePerCubicKm;

		public List<string> PrefabsSearchableGrids;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractTypeFindDefinition myObjectBuilder_ContractTypeFindDefinition = builder as MyObjectBuilder_ContractTypeFindDefinition;
			if (myObjectBuilder_ContractTypeFindDefinition != null)
			{
				MaxGridDistance = myObjectBuilder_ContractTypeFindDefinition.MaxGridDistance;
				MinGridDistance = myObjectBuilder_ContractTypeFindDefinition.MinGridDistance;
				MaxGpsOffset = myObjectBuilder_ContractTypeFindDefinition.MaxGpsOffset;
				TriggerRadius = myObjectBuilder_ContractTypeFindDefinition.TriggerRadius;
				Duration_BaseTime = myObjectBuilder_ContractTypeFindDefinition.Duration_BaseTime;
				Duration_TimePerMeter = myObjectBuilder_ContractTypeFindDefinition.Duration_TimePerMeter;
				Duration_TimePerCubicKm = myObjectBuilder_ContractTypeFindDefinition.Duration_TimePerCubicKm;
				PrefabsSearchableGrids = myObjectBuilder_ContractTypeFindDefinition.PrefabsSearchableGrids;
			}
		}
	}
}
