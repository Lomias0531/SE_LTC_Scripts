namespace VRageRender.Voxels
{
	public interface IMyVoxelRenderDataProcessorProvider
	{
		IMyVoxelRenderDataProcessor GetRenderDataProcessor(int vertexCount, int indexCount, int partsCount);
	}
}
