using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
	public interface IMyGridGroups
	{
		/// <summary>
		/// Returns all grids connected to the given grid by the specified link type.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		List<IMyCubeGrid> GetGroup(IMyCubeGrid node, GridLinkTypeEnum type);

		/// <summary>
		/// Checks if two grids are connected by the given link type.
		/// </summary>
		/// <param name="grid1"></param>
		/// <param name="grid2"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		bool HasConnection(IMyCubeGrid grid1, IMyCubeGrid grid2, GridLinkTypeEnum type);
	}
}
