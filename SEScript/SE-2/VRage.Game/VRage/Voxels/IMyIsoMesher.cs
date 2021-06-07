using VRage.Game.Voxels;
using VRageMath;

namespace VRage.Voxels
{
	public interface IMyIsoMesher
	{
		int InvalidatedRangeInflate
		{
			get;
		}

		MyIsoMesh Precalc(IMyStorage storage, int lod, Vector3I lodVoxelMin, Vector3I lodVoxelMax, MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial, MyVoxelRequestFlags flags = (MyVoxelRequestFlags)0);
	}
}
