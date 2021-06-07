using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public struct MyStorageDataWriteOperator : IVoxelOperator
	{
		private readonly MyStorageData m_data;

		public VoxelOperatorFlags Flags => VoxelOperatorFlags.WriteAll;

		public MyStorageDataWriteOperator(MyStorageData data)
		{
			m_data = data;
		}

		public void Op(ref Vector3I position, MyStorageDataTypeEnum dataType, ref byte outData)
		{
			outData = m_data.Get(dataType, ref position);
		}
	}
}
