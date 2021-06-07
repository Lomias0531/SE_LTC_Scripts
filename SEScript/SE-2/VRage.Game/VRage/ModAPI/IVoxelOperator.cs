using VRage.Voxels;
using VRageMath;

namespace VRage.ModAPI
{
	public interface IVoxelOperator
	{
		VoxelOperatorFlags Flags
		{
			get;
		}

		void Op(ref Vector3I position, MyStorageDataTypeEnum dataType, ref byte inOutContent);
	}
}
