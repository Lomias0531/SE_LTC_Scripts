using VRage;
using VRage.Game;

namespace Sandbox.ModAPI.Ingame
{
	public struct MyProductionItem
	{
		public readonly MyFixedPoint Amount;

		public readonly MyDefinitionId BlueprintId;

		public readonly uint ItemId;

		public MyProductionItem(uint itemId, MyDefinitionId blueprintId, MyFixedPoint amount)
		{
			this = default(MyProductionItem);
			ItemId = itemId;
			BlueprintId = blueprintId;
			Amount = amount;
		}
	}
}
