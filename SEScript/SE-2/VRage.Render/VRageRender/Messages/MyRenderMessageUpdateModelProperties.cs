using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateModelProperties : MyRenderMessageBase
	{
		public uint ID;

		public string MaterialName;

		public RenderFlagsChange? FlagsChange;

		public Color? DiffuseColor;

		public float? Emissivity;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateModelProperties;
	}
}
