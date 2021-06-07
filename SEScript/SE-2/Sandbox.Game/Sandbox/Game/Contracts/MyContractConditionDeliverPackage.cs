using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Contracts
{
	[MyContractConditionDescriptor(typeof(MyObjectBuilder_ContractConditionDeliverPackage))]
	internal class MyContractConditionDeliverPackage : MyContractCondition
	{
		public override void Init(MyObjectBuilder_ContractCondition builder)
		{
			base.Init(builder);
			_ = (builder is MyObjectBuilder_ContractConditionDeliverPackage);
		}

		public override MyObjectBuilder_ContractCondition GetObjectBuilder()
		{
			return base.GetObjectBuilder();
		}

		public override string ToDebugString()
		{
			return $"  Deliver Package";
		}
	}
}
