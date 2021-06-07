using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI.Ingame
{
	public struct MyStoreItemDataSimple
	{
		/// <summary>
		/// Gets definition id of the item
		/// </summary>
		public MyItemType ItemId
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets amount for buy/sell
		/// </summary>
		public int Amount
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets price per unit
		/// </summary>
		public int PricePerUnit
		{
			get;
			private set;
		}

		/// <summary>
		/// Store Item constructor
		/// </summary>
		/// <param name="itemId">definition id of the item</param>
		/// <param name="amount">amount for buy/sell</param>
		/// <param name="pricePerUnit">price per unit</param>
		/// <param name="onTransactionCallback">on transaction callback</param>
		/// <param name="onCancelCallback">on cancel callback</param>
		public MyStoreItemDataSimple(SerializableDefinitionId itemId, int amount, int pricePerUnit)
		{
			ItemId = itemId;
			Amount = amount;
			PricePerUnit = pricePerUnit;
		}
	}
}
