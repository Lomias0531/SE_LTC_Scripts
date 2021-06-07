using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities
{
	public class MyGridGroupsHelper : IMyGridGroups
	{
		public List<IMyCubeGrid> GetGroup(IMyCubeGrid node, GridLinkTypeEnum type)
		{
			return MyCubeGridGroups.Static.GetGroups(type).GetGroupNodes((MyCubeGrid)node).Cast<IMyCubeGrid>()
				.ToList();
		}

		public bool HasConnection(IMyCubeGrid grid1, IMyCubeGrid grid2, GridLinkTypeEnum type)
		{
			return MyCubeGridGroups.Static.GetGroups(type).HasSameGroup((MyCubeGrid)grid1, (MyCubeGrid)grid2);
		}
	}
}
