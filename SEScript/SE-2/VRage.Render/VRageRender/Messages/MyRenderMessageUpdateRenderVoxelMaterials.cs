using VRage;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateRenderVoxelMaterials : MyRenderMessageBase
	{
		public MyRenderVoxelMaterialData[] Materials;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateRenderVoxelMaterials;
	}
}
