using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using VRage.Groups;

namespace Sandbox.Game.Entities
{
	public class MyGridLogicalGroupData : IGroupData<MyCubeGrid>
	{
		internal readonly MyGridTerminalSystem TerminalSystem = new MyGridTerminalSystem();

		internal readonly MyGridWeaponSystem WeaponSystem = new MyGridWeaponSystem();

		internal readonly MyResourceDistributorComponent ResourceDistributor;

		public MyGridLogicalGroupData()
			: this(null)
		{
		}

		public MyGridLogicalGroupData(string debugName)
		{
			ResourceDistributor = new MyResourceDistributorComponent(debugName);
		}

		public void OnRelease()
		{
		}

		public void OnNodeAdded(MyCubeGrid entity)
		{
			entity.OnAddedToGroup(this);
		}

		public void OnNodeRemoved(MyCubeGrid entity)
		{
			entity.OnRemovedFromGroup(this);
		}

		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		internal void UpdateGridOwnership(List<MyCubeGrid> grids, long ownerID)
		{
			foreach (MyCubeGrid grid in grids)
			{
				grid.IsAccessibleForProgrammableBlock = grid.BigOwners.Contains(ownerID);
			}
		}
	}
}
