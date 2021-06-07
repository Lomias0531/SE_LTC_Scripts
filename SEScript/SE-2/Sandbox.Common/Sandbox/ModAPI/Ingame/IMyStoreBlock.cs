using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyStoreBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Creates store item offer and returns its id.
		/// </summary>
		/// <param name="item">Data defining the store item.</param>
		/// <param name="id">Id of the item in the shop. (Ex. needed to remove the item)</param>
		/// <returns>Result of the creation of the store item.</returns>
		MyStoreInsertResults InsertOffer(MyStoreItemDataSimple item, out long id);

		/// <summary>
		/// Creates store item  order and returns its id.
		/// </summary>
		/// <param name="item">Data defining the store item.</param>
		/// <param name="id">Id of the item in the shop. (Ex. needed to remove the item)</param>
		/// <returns>Result of the creation of the store item.</returns>
		MyStoreInsertResults InsertOrder(MyStoreItemDataSimple item, out long id);

		/// <summary>
		/// Cancels the item (either order or offer).
		/// </summary>
		/// <param name="id">Id of the item to be canceled.</param>
		/// <returns></returns>
		bool CancelStoreItem(long id);

		/// <summary>
		/// Returns player store items.
		/// </summary>
		/// <param name="storeItems">Items currently set in store.</param>
		void GetPlayerStoreItems(List<MyStoreQueryItem> storeItems);
	}
}
