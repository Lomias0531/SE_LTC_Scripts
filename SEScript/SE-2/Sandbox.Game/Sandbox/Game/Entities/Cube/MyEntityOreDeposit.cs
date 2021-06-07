using System.Collections.Generic;
using VRage.Game;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyEntityOreDeposit
	{
		public struct Data
		{
			public MyVoxelMaterialDefinition Material;

			public Vector3 AverageLocalPosition;

			internal void ComputeWorldPosition(MyVoxelBase voxelMap, out Vector3D oreWorldPosition)
			{
				MyVoxelCoordSystems.LocalPositionToWorldPosition(voxelMap.PositionComp.GetPosition() - (Vector3D)voxelMap.StorageMin, ref AverageLocalPosition, out oreWorldPosition);
			}
		}

		public class TypeComparer : IEqualityComparer<MyEntityOreDeposit>
		{
			bool IEqualityComparer<MyEntityOreDeposit>.Equals(MyEntityOreDeposit x, MyEntityOreDeposit y)
			{
				if (x.VoxelMap.EntityId == y.VoxelMap.EntityId && x.CellCoord == y.CellCoord)
				{
					return x.DetectorId == y.DetectorId;
				}
				return false;
			}

			int IEqualityComparer<MyEntityOreDeposit>.GetHashCode(MyEntityOreDeposit obj)
			{
				return (int)(obj.VoxelMap.EntityId ^ (obj.CellCoord.GetHashCode() * obj.DetectorId));
			}
		}

		public long DetectorId;

		public MyVoxelBase VoxelMap;

		public Vector3I CellCoord;

		public readonly List<Data> Materials = new List<Data>();

		public static readonly TypeComparer Comparer = new TypeComparer();

		public MyEntityOreDeposit(MyVoxelBase voxelMap, Vector3I cellCoord, long detectorId)
		{
			VoxelMap = voxelMap;
			CellCoord = cellCoord;
			DetectorId = detectorId;
		}
	}
}
