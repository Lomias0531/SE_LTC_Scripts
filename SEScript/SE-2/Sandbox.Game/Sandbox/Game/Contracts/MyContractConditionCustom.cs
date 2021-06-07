using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Contracts
{
	[MyContractConditionDescriptor(typeof(MyObjectBuilder_ContractConditionCustom))]
	public class MyContractConditionCustom : MyContractCondition
	{
		public override void Init(MyObjectBuilder_ContractCondition builder)
		{
			base.Init(builder);
		}
	}
}
