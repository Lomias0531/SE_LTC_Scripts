using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractTypeEscortDefinition), null)]
	public class MyContractTypeEscortDefinition : MyContractTypeDefinition
	{
		private class Sandbox_Definitions_MyContractTypeEscortDefinition_003C_003EActor : IActivator, IActivator<MyContractTypeEscortDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractTypeEscortDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractTypeEscortDefinition CreateInstance()
			{
				return new MyContractTypeEscortDefinition();
			}

			MyContractTypeEscortDefinition IActivator<MyContractTypeEscortDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public double RewardRadius;

		public double TriggerRadius;

		public double TravelDistanceMax;

		public double TravelDistanceMin;

		public int DroneFirstDelayInS;

		public int DroneAttackPeriodInS;

		public int InitialDelayInS;

		public int DronesPerWave;

		public float Duration_BaseTime;

		public float Duration_FlightTimeMultiplier;

		public List<string> PrefabsAttackDrones;

		public List<string> PrefabsEscortShips;

		public List<string> DroneBehaviours;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractTypeEscortDefinition myObjectBuilder_ContractTypeEscortDefinition = builder as MyObjectBuilder_ContractTypeEscortDefinition;
			if (myObjectBuilder_ContractTypeEscortDefinition != null)
			{
				RewardRadius = myObjectBuilder_ContractTypeEscortDefinition.RewardRadius;
				TriggerRadius = myObjectBuilder_ContractTypeEscortDefinition.TriggerRadius;
				TravelDistanceMax = myObjectBuilder_ContractTypeEscortDefinition.TravelDistanceMax;
				TravelDistanceMin = myObjectBuilder_ContractTypeEscortDefinition.TravelDistanceMin;
				DroneAttackPeriodInS = myObjectBuilder_ContractTypeEscortDefinition.DroneAttackPeriodInS;
				DroneFirstDelayInS = myObjectBuilder_ContractTypeEscortDefinition.DroneFirstDelayInS;
				InitialDelayInS = myObjectBuilder_ContractTypeEscortDefinition.InitialDelayInS;
				DronesPerWave = myObjectBuilder_ContractTypeEscortDefinition.DronesPerWave;
				Duration_BaseTime = myObjectBuilder_ContractTypeEscortDefinition.Duration_BaseTime;
				Duration_FlightTimeMultiplier = myObjectBuilder_ContractTypeEscortDefinition.Duration_FlightTimeMultiplier;
				PrefabsAttackDrones = myObjectBuilder_ContractTypeEscortDefinition.PrefabsAttackDrones;
				PrefabsEscortShips = myObjectBuilder_ContractTypeEscortDefinition.PrefabsEscortShips;
				DroneBehaviours = myObjectBuilder_ContractTypeEscortDefinition.DroneBehaviours;
			}
		}
	}
}
