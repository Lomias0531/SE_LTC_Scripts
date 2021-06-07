using VRage;
using VRage.Library.Collections;

namespace VRageRender.Voxels
{
	public interface IMyVoxelRenderDataProcessor
	{
		unsafe void AddPart(MyList<MyVertexFormatVoxelSingleData> vertices, ushort* indices, int indicesCount, MyVoxelMaterialTriple material);

		void GetDataAndDispose(ref MyVoxelRenderCellData data);
	}
}
