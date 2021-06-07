using System;
using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI
{
	public struct MyStoreItemData
	{
		/// <summary>
		/// Gets definition id of the item
		/// </summary>
		public SerializableDefinitionId ItemId
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
		/// When player makes an transaction regarding this item
		///
		/// int - amount sold
		/// int - amount remaining
		/// int - price of transaction
		/// long - owner of block
		/// long - buyer/seller
		/// </summary>
		public Action<int, int, long, long, long> OnTransaction
		{
			get;
			private set;
		}

		/// <summary>
		/// When owner of store block cancels order/offer regarding this item
		/// </summary>
		public Action OnCancel
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
		public MyStoreItemData(SerializableDefinitionId itemId, int amount, int pricePerUnit, Action<int, int, long, long, long> onTransactionCallback, Action onCancelCallback)
		{
			ItemId = itemId;
			Amount = amount;
			PricePerUnit = pricePerUnit;
			OnTransaction = onTransactionCallback;
			OnCancel = onCancelCallback;
		}
	}
}
