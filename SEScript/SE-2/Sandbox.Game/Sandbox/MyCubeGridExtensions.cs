using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;
using VRage.Groups;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox
{
	public static class MyCubeGridExtensions
	{
		internal static bool HasSameGroupAndIsGrid<TGroupData>(this MyGroups<MyCubeGrid, TGroupData> groups, IMyEntity gridA, IMyEntity gridB) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			MyCubeGrid myCubeGrid = gridA as MyCubeGrid;
			MyCubeGrid myCubeGrid2 = gridB as MyCubeGrid;
			if (myCubeGrid != null && myCubeGrid2 != null)
			{
				return groups.HasSameGroup(myCubeGrid, myCubeGrid2);
			}
			return false;
		}

		public static BoundingSphere CalculateBoundingSphere(this MyObjectBuilder_CubeGrid grid)
		{
			return BoundingSphere.CreateFromBoundingBox(grid.CalculateBoundingBox());
		}

		public static BoundingBox CalculateBoundingBox(this MyObjectBuilder_CubeGrid grid)
		{
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(grid.GridSizeEnum);
			BoundingBox result = new BoundingBox(Vector3.MaxValue, Vector3.MinValue);
			try
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
				{
					if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(cubeBlock.GetId(), out MyCubeBlockDefinition blockDefinition))
					{
						MyBlockOrientation orientation = cubeBlock.BlockOrientation;
						Vector3 value = Vector3.TransformNormal(new Vector3(blockDefinition.Size) * cubeSize, orientation);
						value = Vector3.Abs(value);
						Vector3 vector = new Vector3(cubeBlock.Min) * cubeSize - new Vector3(cubeSize / 2f);
						Vector3 point = vector + value;
						result.Include(vector);
						result.Include(point);
					}
				}
				return result;
			}
			catch (KeyNotFoundException ex)
			{
				MySandboxGame.Log.WriteLine(ex);
				return default(BoundingBox);
			}
		}

		public static void HookMultiplayer(this MyCubeBlock cubeBlock)
		{
			if (cubeBlock != null)
			{
				MyEntities.RaiseEntityCreated(cubeBlock);
			}
		}
	}
}
