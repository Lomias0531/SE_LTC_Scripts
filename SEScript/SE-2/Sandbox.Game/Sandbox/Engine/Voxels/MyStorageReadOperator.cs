using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public struct MyStorageReadOperator : IVoxelOperator
	{
		private readonly MyStorageData m_data;

		public VoxelOperatorFlags Flags => VoxelOperatorFlags.Read;

		public MyStorageReadOperator(MyStorageData data)
		{
			m_data = data;
		}

		public void Op(ref Vector3I position, MyStorageDataTypeEnum dataType, ref byte inData)
		{
			m_data.Set(dataType, ref position, inData);
		}
	}
}
