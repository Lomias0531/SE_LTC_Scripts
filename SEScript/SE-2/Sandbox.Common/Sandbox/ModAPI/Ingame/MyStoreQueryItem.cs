using VRage.ObjectBuilders;

namespace Sandbox.ModAPI.Ingame
{
	public struct MyStoreQueryItem
	{
		/// <summary>
		/// Gets definition id of the item
		/// </summary>
		public SerializableDefinitionId ItemId;

		/// <summary>
		/// Gets amount for buy/sell
		/// </summary>
		public int Amount;

		/// <summary>
		/// Gets price per unit
		/// </summary>
		public int PricePerUnit;

		/// <summary>
		/// Id of the store item. Ex. Needed for removing item from store.
		/// </summary>
		public long Id;
	}
}
