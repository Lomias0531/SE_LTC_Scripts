using VRageMath;

namespace VRage.Voxels
{
	public struct MyVoxelDataRequest
	{
		public int Lod;

		public Vector3I MinInLod;

		public Vector3I MaxInLod;

		public Vector3I Offset;

		public MyStorageDataTypeFlags RequestedData;

		public MyVoxelRequestFlags RequestFlags;

		public MyVoxelRequestFlags Flags;

		public MyStorageData Target;

		public int SizeLinear => (MaxInLod - MinInLod + Vector3I.One).Size;

		public string ToStringShort()
		{
			return $"lod{(int)Lod}: {(int)SizeLinear}voxels";
		}
	}
}
