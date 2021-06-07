using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateRenderEntity : MyRenderMessageBase
	{
		public uint ID;

		public Color? DiffuseColor;

		public Vector3? ColorMaskHSV;

		public float? Dithering;

		public bool FadeIn;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateRenderEntity;

		public override void Close()
		{
			DiffuseColor = null;
			ColorMaskHSV = null;
			base.Close();
		}
	}
}
