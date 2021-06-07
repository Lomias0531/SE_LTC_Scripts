using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderObjectUpdateData
	{
		public MatrixD? WorldMatrix;

		public Matrix? LocalMatrix;

		public BoundingBox? LocalAABB;

		private static int m_allocated;

		public void Clean()
		{
			LocalAABB = null;
			LocalMatrix = null;
			WorldMatrix = null;
		}
	}
}
