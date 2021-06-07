using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Contracts
{
	[MyContractConditionDescriptor(typeof(MyObjectBuilder_ContractConditionDeliverItems))]
	public class MyContractConditionDeliverItems : MyContractCondition
	{
		public MyDefinitionId ItemType;

		public int ItemAmount;

		public float ItemVolume;

		public bool TransferItems;

		public override void Init(MyObjectBuilder_ContractCondition builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractConditionDeliverItems myObjectBuilder_ContractConditionDeliverItems = builder as MyObjectBuilder_ContractConditionDeliverItems;
			if (myObjectBuilder_ContractConditionDeliverItems != null)
			{
				ItemType = myObjectBuilder_ContractConditionDeliverItems.ItemType;
				ItemAmount = myObjectBuilder_ContractConditionDeliverItems.ItemAmount;
				ItemVolume = myObjectBuilder_ContractConditionDeliverItems.ItemVolume;
				TransferItems = myObjectBuilder_ContractConditionDeliverItems.TransferItems;
			}
		}

		public override MyObjectBuilder_ContractCondition GetObjectBuilder()
		{
			MyObjectBuilder_ContractCondition objectBuilder = base.GetObjectBuilder();
			MyObjectBuilder_ContractConditionDeliverItems obj = objectBuilder as MyObjectBuilder_ContractConditionDeliverItems;
			obj.ItemType = ItemType;
			obj.ItemAmount = ItemAmount;
			obj.ItemVolume = ItemVolume;
			obj.TransferItems = TransferItems;
			return objectBuilder;
		}

		public override string ToDebugString()
		{
			return $"  Deliver Item\n   {ItemType}\n   {ItemAmount}";
		}
	}
}
