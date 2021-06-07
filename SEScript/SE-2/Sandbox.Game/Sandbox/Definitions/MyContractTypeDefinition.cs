using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractTypeDefinition), null)]
	public class MyContractTypeDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyContractTypeDefinition_003C_003EActor : IActivator, IActivator<MyContractTypeDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractTypeDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractTypeDefinition CreateInstance()
			{
				return new MyContractTypeDefinition();
			}

			MyContractTypeDefinition IActivator<MyContractTypeDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string TitleName
		{
			get;
			set;
		}

		public string DescriptionName
		{
			get;
			set;
		}

		public int MinimumReputation
		{
			get;
			set;
		}

		public int FailReputationPrice
		{
			get;
			set;
		}

		public long MinimumMoney
		{
			get;
			set;
		}

		public long MoneyReputationCoeficient
		{
			get;
			set;
		}

		public long MinStartingDeposit
		{
			get;
			set;
		}

		public long MaxStartingDeposit
		{
			get;
			set;
		}

		public double DurationMultiplier
		{
			get;
			set;
		}

		public Dictionary<SerializableDefinitionId, float> ChancesPerFactionType
		{
			get;
			set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractTypeDefinition myObjectBuilder_ContractTypeDefinition = builder as MyObjectBuilder_ContractTypeDefinition;
			if (myObjectBuilder_ContractTypeDefinition == null)
			{
				return;
			}
			MinimumReputation = myObjectBuilder_ContractTypeDefinition.MinimumReputation;
			FailReputationPrice = myObjectBuilder_ContractTypeDefinition.FailReputationPrice;
			MinimumMoney = myObjectBuilder_ContractTypeDefinition.MinimumMoney;
			MoneyReputationCoeficient = myObjectBuilder_ContractTypeDefinition.MoneyReputationCoeficient;
			MinStartingDeposit = myObjectBuilder_ContractTypeDefinition.MinStartingDeposit;
			MaxStartingDeposit = myObjectBuilder_ContractTypeDefinition.MaxStartingDeposit;
			DurationMultiplier = myObjectBuilder_ContractTypeDefinition.DurationMultiplier;
			ChancesPerFactionType = new Dictionary<SerializableDefinitionId, float>();
			if (myObjectBuilder_ContractTypeDefinition.ChancesPerFactionType != null)
			{
				MyContractChancePair[] chancesPerFactionType = myObjectBuilder_ContractTypeDefinition.ChancesPerFactionType;
				foreach (MyContractChancePair myContractChancePair in chancesPerFactionType)
				{
					ChancesPerFactionType.Add(myContractChancePair.DefinitionId, myContractChancePair.Value);
				}
			}
		}
	}
}
