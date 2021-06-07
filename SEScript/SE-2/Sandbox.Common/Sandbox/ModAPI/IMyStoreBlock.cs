using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	public interface IMyStoreBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyStoreBlock
	{
		/// <summary>
		/// Creates store item and returns its id.
		/// </summary>
		/// <param name="item">Data defining the store item.</param>
		/// <param name="id">Id of the item in the shop. (Ex. needed to remove the item)</param>
		/// <returns>Result of the creation of the store item.</returns>
		MyStoreInsertResults InsertOffer(MyStoreItemData item, out long id);

		/// <summary>
		/// Creates store item and returns its id.
		/// </summary>
		/// <param name="item">Data defining the store item.</param>
		/// <param name="id">Id of the item in the shop. (Ex. needed to remove the item)</param>
		/// <returns>Result of the creation of the store item.</returns>
		MyStoreInsertResults InsertOrder(MyStoreItemData item, out long id);
	}
}
