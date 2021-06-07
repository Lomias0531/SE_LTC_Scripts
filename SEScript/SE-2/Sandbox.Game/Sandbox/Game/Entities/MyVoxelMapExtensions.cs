using Sandbox.Engine.Voxels;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal static class MyVoxelMapExtensions
	{
		public static Vector3D GetPositionOnVoxel(this MyVoxelMap map, Vector3D position, float maxVertDistance)
		{
			Vector3D worldPosition = position;
			MyVoxelCoordSystems.WorldPositionToGeometryCellCoord(map.PositionLeftBottomCorner, ref position, out Vector3I geometryCellCoord);
			MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref geometryCellCoord, out BoundingBox localAABB);
			Vector3 center = localAABB.Center;
			Line localLine = new Line(center + Vector3D.Up * maxVertDistance, center + Vector3D.Down * maxVertDistance);
			if (map.Storage.GetGeometry().Intersect(ref localLine, out MyIntersectionResultLineTriangle result, IntersectionFlags.ALL_TRIANGLES))
			{
				Vector3 localPosition = result.InputTriangle.Vertex0;
				MyVoxelCoordSystems.LocalPositionToWorldPosition(map.PositionLeftBottomCorner - (Vector3D)map.StorageMin, ref localPosition, out worldPosition);
			}
			return worldPosition;
		}
	}
}
