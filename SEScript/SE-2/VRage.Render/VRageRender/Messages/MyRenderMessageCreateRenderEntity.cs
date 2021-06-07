using VRageMath;
using VRageRender.Import;

namespace VRageRender.Messages
{
	public class MyRenderMessageCreateRenderEntity : MyRenderMessageBase
	{
		public uint ID;

		public string DebugName;

		public string Model;

		public MatrixD WorldMatrix;

		public MyMeshDrawTechnique Technique;

		public RenderFlags Flags;

		public int DepthBias;

		public CullingOptions CullingOptions;

		public float MaxViewDistance;

		public float Rescale = 1f;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateRenderEntity;

		public override string ToString()
		{
			return DebugName ?? (string.Empty + ", " + Model);
		}
	}
}
