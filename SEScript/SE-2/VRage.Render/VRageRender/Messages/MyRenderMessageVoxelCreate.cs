using VRageMath;
using VRageRender.Voxels;

namespace VRageRender.Messages
{
	public class MyRenderMessageVoxelCreate : MyRenderMessageBase
	{
		public uint Id;

		public string DebugName;

		public MatrixD WorldMatrix;

		public Vector3I Size;

		public float? SpherizeRadius;

		public Vector3D SpherizePosition;

		public IMyLodController Clipmap;

		public RenderFlags RenderFlags;

		public float Dithering;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.VoxelCreate;
	}
}
