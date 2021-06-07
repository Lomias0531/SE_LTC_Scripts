using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication
{
	internal static class MyReplicableRightsValidator
	{
		private static float ALLOWED_PHYSICAL_DISTANCE_SQUARED = MyConstants.DEFAULT_INTERACTIVE_DISTANCE * 3f * (MyConstants.DEFAULT_INTERACTIVE_DISTANCE * 3f);

		public static ValidationResult GetControlled(MyEntity controlledEntity, EndpointId endpointId)
		{
			if (controlledEntity == null)
			{
				return ValidationResult.Kick | ValidationResult.Controlled;
			}
			MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(controlledEntity);
			if ((controllingPlayer == null || controllingPlayer.Client.SteamUserId != endpointId.Value) && !MySession.Static.IsUserAdmin(endpointId.Value))
			{
				controllingPlayer = MySession.Static.Players.GetPreviousControllingPlayer(controlledEntity);
				if ((controllingPlayer == null || controllingPlayer.Client.SteamUserId != endpointId.Value) && !MySession.Static.IsUserAdmin(endpointId.Value))
				{
					return ValidationResult.Kick | ValidationResult.Controlled;
				}
				return ValidationResult.Controlled;
			}
			return ValidationResult.Passed;
		}

		public static bool GetBigOwner(MyCubeGrid grid, EndpointId endpointId, long identityId, bool spaceMaster)
		{
			if (grid == null)
			{
				return false;
			}
			bool flag = grid.BigOwners.Count == 0 || grid.BigOwners.Contains(identityId);
			if (spaceMaster)
			{
				flag |= MySession.Static.IsUserSpaceMaster(endpointId.Value);
			}
			return flag;
		}

		public static bool GetAccess(MyCharacterReplicable characterReplicable, Vector3D characterPosition, MyCubeGrid grid, MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group, bool physical)
		{
			if (characterReplicable == null || grid == null)
			{
				return false;
			}
			if (group != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
				{
					if (physical && node.NodeData.PositionComp.WorldAABB.DistanceSquared(characterPosition) <= (double)ALLOWED_PHYSICAL_DISTANCE_SQUARED)
					{
						return true;
					}
					if (characterReplicable.CachedParentDependencies.Contains(node.NodeData))
					{
						return true;
					}
				}
			}
			else
			{
				if (physical && grid.PositionComp.WorldAABB.DistanceSquared(characterPosition) <= (double)ALLOWED_PHYSICAL_DISTANCE_SQUARED)
				{
					return true;
				}
				if (characterReplicable.CachedParentDependencies.Contains(grid))
				{
					return true;
				}
			}
			return false;
		}
	}
}
