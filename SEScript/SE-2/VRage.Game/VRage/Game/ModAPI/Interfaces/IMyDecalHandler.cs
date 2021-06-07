using System.Collections.Generic;
using VRageRender;

namespace VRage.Game.ModAPI.Interfaces
{
	public interface IMyDecalHandler
	{
		/// <param name="renderData">Position and normal on local coordinates for regular actors.
		/// World position on voxel maps</param>
		/// <param name="renderInfo"></param>
		/// <param name="ids"></param>
		void AddDecal(ref MyDecalRenderInfo renderInfo, List<uint> ids = null);
	}
}
