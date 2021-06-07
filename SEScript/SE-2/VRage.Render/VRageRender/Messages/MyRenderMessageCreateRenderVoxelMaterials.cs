using VRage;

namespace VRageRender.Messages
{
	public class MyRenderMessageCreateRenderVoxelMaterials : MyRenderMessageBase
	{
		public MyRenderVoxelMaterialData[] Materials;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateRenderVoxelMaterials;
	}
}
