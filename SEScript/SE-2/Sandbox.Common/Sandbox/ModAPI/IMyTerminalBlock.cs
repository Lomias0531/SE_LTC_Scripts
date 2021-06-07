using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	public interface IMyTerminalBlock : VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock
	{
		event Action<IMyTerminalBlock> CustomDataChanged;

		event Action<IMyTerminalBlock> CustomNameChanged;

		event Action<IMyTerminalBlock> OwnershipChanged;

		event Action<IMyTerminalBlock> PropertiesChanged;

		event Action<IMyTerminalBlock> ShowOnHUDChanged;

		event Action<IMyTerminalBlock> VisibilityChanged;

		/// <summary>
		/// Event to append custom info.
		/// </summary>
		event Action<IMyTerminalBlock, StringBuilder> AppendingCustomInfo;

		/// <summary>
		/// Raises AppendingCustomInfo so every subscriber can append custom info.
		/// </summary>
		void RefreshCustomInfo();

		/// <summary>
		/// Determines whether this block is in the same logical group as the other, meaning they're connected
		/// either mechanically or via blocks like connectors. Be aware that using merge blocks combines grids into one, so this function
		/// will not filter out grids connected that way.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		bool IsInSameLogicalGroupAs(IMyTerminalBlock other);

		/// <summary>
		/// Determines whether this block is mechanically connected to the other. This is any block connected
		/// with rotors or pistons or other mechanical devices, but not things like connectors. This will in most
		/// cases constitute your complete construct. Be aware that using merge blocks combines grids into one, so this function
		/// will not filter out grids connected that way.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		bool IsSameConstructAs(IMyTerminalBlock other);
	}
}
