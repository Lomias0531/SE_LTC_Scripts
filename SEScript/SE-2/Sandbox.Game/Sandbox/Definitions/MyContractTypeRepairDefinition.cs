using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractTypeRepairDefinition), null)]
	internal class MyContractTypeRepairDefinition : MyContractTypeDefinition
	{
		private class Sandbox_Definitions_MyContractTypeRepairDefinition_003C_003EActor : IActivator, IActivator<MyContractTypeRepairDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractTypeRepairDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractTypeRepairDefinition CreateInstance()
			{
				return new MyContractTypeRepairDefinition();
			}

			MyContractTypeRepairDefinition IActivator<MyContractTypeRepairDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public double MaxGridDistance;

		public double MinGridDistance;

		public double Duration_BaseTime;

		public double Duration_TimePerMeter;

		public float PriceToRewardCoeficient;

		public float PriceSpread;

		public float TimeToPriceDenominator = 60f;

		public List<string> PrefabNames;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractTypeRepairDefinition myObjectBuilder_ContractTypeRepairDefinition = builder as MyObjectBuilder_ContractTypeRepairDefinition;
			if (myObjectBuilder_ContractTypeRepairDefinition != null)
			{
				MaxGridDistance = myObjectBuilder_ContractTypeRepairDefinition.MaxGridDistance;
				MinGridDistance = myObjectBuilder_ContractTypeRepairDefinition.MinGridDistance;
				Duration_BaseTime = myObjectBuilder_ContractTypeRepairDefinition.Duration_BaseTime;
				Duration_TimePerMeter = myObjectBuilder_ContractTypeRepairDefinition.Duration_TimePerMeter;
				PrefabNames = myObjectBuilder_ContractTypeRepairDefinition.PrefabNames;
				PriceToRewardCoeficient = myObjectBuilder_ContractTypeRepairDefinition.PriceToRewardCoeficient;
				PriceSpread = myObjectBuilder_ContractTypeRepairDefinition.PriceSpread;
				TimeToPriceDenominator = myObjectBuilder_ContractTypeRepairDefinition.TimeToPriceDenominator;
			}
		}
	}
}
