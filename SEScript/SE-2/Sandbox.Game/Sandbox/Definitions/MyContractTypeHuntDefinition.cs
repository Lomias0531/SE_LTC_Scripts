using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractTypeHuntDefinition), null)]
	public class MyContractTypeHuntDefinition : MyContractTypeDefinition
	{
		private class Sandbox_Definitions_MyContractTypeHuntDefinition_003C_003EActor : IActivator, IActivator<MyContractTypeHuntDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractTypeHuntDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractTypeHuntDefinition CreateInstance()
			{
				return new MyContractTypeHuntDefinition();
			}

			MyContractTypeHuntDefinition IActivator<MyContractTypeHuntDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int RemarkPeriodInS;

		public float RemarkVariance;

		public double KillRange;

		public float KillRangeMultiplier;

		public int ReputationLossForTarget;

		public double RewardRadius;

		public double Duration_BaseTime;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractTypeHuntDefinition myObjectBuilder_ContractTypeHuntDefinition = builder as MyObjectBuilder_ContractTypeHuntDefinition;
			if (myObjectBuilder_ContractTypeHuntDefinition != null)
			{
				RemarkPeriodInS = myObjectBuilder_ContractTypeHuntDefinition.RemarkPeriodInS;
				RemarkVariance = myObjectBuilder_ContractTypeHuntDefinition.RemarkVariance;
				KillRange = myObjectBuilder_ContractTypeHuntDefinition.KillRange;
				KillRangeMultiplier = myObjectBuilder_ContractTypeHuntDefinition.KillRangeMultiplier;
				ReputationLossForTarget = myObjectBuilder_ContractTypeHuntDefinition.ReputationLossForTarget;
				RewardRadius = myObjectBuilder_ContractTypeHuntDefinition.RewardRadius;
				Duration_BaseTime = myObjectBuilder_ContractTypeHuntDefinition.Duration_BaseTime;
			}
		}
	}
}
