using VRageMath;

namespace VRage.Voxels
{
	/// <summary>
	/// Functions for transforming to and from various coordinate systems in voxel maps and for computing bounding boxes of various types of cells.
	/// Note that local and world positions are (and should be) always in the min-corner!
	/// </summary>
	///
	/// TODO: Review this whole class
	/// These are half wrong/ don't consider world matrix, beware
	public static class MyVoxelCoordSystems
	{
		public static void WorldPositionToLocalPosition(Vector3D worldPosition, MatrixD worldMatrix, MatrixD worldMatrixInv, Vector3 halfSize, out Vector3 localPosition)
		{
			localPosition = Vector3D.Transform(worldPosition + Vector3D.TransformNormal(halfSize, worldMatrix), worldMatrixInv);
		}

		public static void WorldPositionToLocalPosition(Vector3D referenceVoxelMapPosition, ref Vector3D worldPosition, out Vector3 localPosition)
		{
			localPosition = worldPosition - referenceVoxelMapPosition;
		}

		public static void LocalPositionToWorldPosition(Vector3D referenceVoxelMapPosition, ref Vector3 localPosition, out Vector3D worldPosition)
		{
			worldPosition = (Vector3D)localPosition + referenceVoxelMapPosition;
		}

		public static void LocalPositionToVoxelCoord(ref Vector3 localPosition, out Vector3I voxelCoord)
		{
			Vector3 v = localPosition / 1f;
			Vector3I.Floor(ref v, out voxelCoord);
		}

		public static void LocalPositionToVoxelCoord(ref Vector3 localPosition, out Vector3D voxelCoord)
		{
			voxelCoord = localPosition / 1f;
		}

		public static void LocalPositionToGeometryCellCoord(ref Vector3 localPosition, out Vector3I geometryCellCoord)
		{
			Vector3D v = localPosition / 8f;
			Vector3I.Floor(ref v, out geometryCellCoord);
		}

		public static void WorldPositionToVoxelCoord(ref Vector3D worldPosition, MatrixD worldMatrix, MatrixD worldMatrixInv, Vector3 halfSize, out Vector3I voxelCoord)
		{
			WorldPositionToLocalPosition(worldPosition, worldMatrix, worldMatrixInv, halfSize, out Vector3 localPosition);
			LocalPositionToVoxelCoord(ref localPosition, out voxelCoord);
		}

		public static void WorldPositionToVoxelCoord(Vector3D referenceVoxelMapPosition, ref Vector3D worldPosition, out Vector3I voxelCoord)
		{
			WorldPositionToLocalPosition(referenceVoxelMapPosition, ref worldPosition, out Vector3 localPosition);
			LocalPositionToVoxelCoord(ref localPosition, out voxelCoord);
		}

		public static void WorldPositionToGeometryCellCoord(Vector3D referenceVoxelMapPosition, ref Vector3D worldPosition, out Vector3I geometryCellCoord)
		{
			WorldPositionToLocalPosition(referenceVoxelMapPosition, ref worldPosition, out Vector3 localPosition);
			LocalPositionToGeometryCellCoord(ref localPosition, out geometryCellCoord);
		}

		public static void VoxelCoordToLocalPosition(ref Vector3I voxelCoord, out Vector3 localPosition)
		{
			localPosition = voxelCoord * 1f;
		}

		public static void GeometryCellCoordToLocalPosition(ref MyCellCoord geometryCellCoord, out Vector3 localPosition)
		{
			localPosition = geometryCellCoord.CoordInLod * 8f * (1 << geometryCellCoord.Lod);
		}

		public static void GeometryCellCoordToLocalPosition(ref Vector3I geometryCellCoord, out Vector3 localPosition)
		{
			localPosition = geometryCellCoord * 8f;
		}

		public static void VoxelCoordToWorldPosition(Vector3D referenceVoxelMapPosition, ref Vector3I voxelCoord, out Vector3D worldPosition)
		{
			VoxelCoordToLocalPosition(ref voxelCoord, out Vector3 localPosition);
			LocalPositionToWorldPosition(referenceVoxelMapPosition, ref localPosition, out worldPosition);
		}

		public static void GeometryCellCoordToLocalAABB(ref Vector3I geometryCellCoord, out BoundingBox localAABB)
		{
			GeometryCellCoordToLocalPosition(ref geometryCellCoord, out Vector3 localPosition);
			localAABB = new BoundingBox(localPosition, localPosition + 8f);
		}

		public static void VoxelCoordToWorldAABB(Vector3D referenceVoxelMapPosition, ref Vector3I voxelCoord, out BoundingBoxD worldAABB)
		{
			VoxelCoordToWorldPosition(referenceVoxelMapPosition, ref voxelCoord, out Vector3D worldPosition);
			worldAABB = new BoundingBoxD(worldPosition, worldPosition + 1f);
		}

		public static void GeometryCellCoordToWorldAABB(Vector3D referenceVoxelMapPosition, ref Vector3I geometryCellCoord, out BoundingBoxD worldAABB)
		{
			GeometryCellCoordToLocalPosition(ref geometryCellCoord, out Vector3 localPosition);
			Vector3D worldPosition = localPosition;
			LocalPositionToWorldPosition(referenceVoxelMapPosition, ref localPosition, out worldPosition);
			worldAABB = new BoundingBoxD(worldPosition, worldPosition + 8f);
		}

		public static void GeometryCellCoordToWorldAABB(Vector3D referenceVoxelMapPosition, ref MyCellCoord geometryCellCoord, out BoundingBoxD worldAABB)
		{
			GeometryCellCoordToLocalPosition(ref geometryCellCoord, out Vector3 localPosition);
			Vector3D worldPosition = localPosition;
			LocalPositionToWorldPosition(referenceVoxelMapPosition, ref localPosition, out worldPosition);
			worldAABB = new BoundingBoxD(worldPosition, worldPosition + 8f * (float)(1 << geometryCellCoord.Lod));
		}

		public static void GeometryCellCenterCoordToWorldPos(Vector3D referenceVoxelMapPosition, ref Vector3I geometryCellCoord, out Vector3D worldPos)
		{
			GeometryCellCoordToLocalPosition(ref geometryCellCoord, out Vector3 localPosition);
			Vector3D worldPosition = localPosition;
			LocalPositionToWorldPosition(referenceVoxelMapPosition, ref localPosition, out worldPosition);
			worldPos = worldPosition + 4.0;
		}

		public static void VoxelCoordToGeometryCellCoord(ref Vector3I voxelCoord, out Vector3I geometryCellCoord)
		{
			geometryCellCoord = voxelCoord >> 3;
		}

		public static void LocalPositionToVertexCell(int lod, ref Vector3 localPosition, out Vector3I vertexCell)
		{
			float divider = 1f * (float)(1 << lod);
			vertexCell = Vector3I.Floor(localPosition / divider);
		}

		public static void VertexCellToLocalPosition(int lod, ref Vector3I vertexCell, out Vector3 localPosition)
		{
			float num = 1f * (float)(1 << lod);
			localPosition = vertexCell * num;
		}

		public static void VertexCellToLocalAABB(int lod, ref Vector3I vertexCell, out BoundingBoxD localAABB)
		{
			float num = 1f * (float)(1 << lod);
			Vector3 vector = vertexCell * num;
			localAABB = new BoundingBoxD(vector, vector + num);
		}

		public static Vector3I FindBestOctreeSize(float radius)
		{
			int num = 32;
			while ((float)num < radius)
			{
				num *= 2;
			}
			return new Vector3I(num, num, num);
		}
	}
}
