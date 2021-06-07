using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageCreateStaticGroup : MyRenderMessageBase
	{
		public uint ID;

		public string Model;

		public Vector3D Translation;

		public Matrix[] LocalMatrices;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateStaticGroup;
	}
}
