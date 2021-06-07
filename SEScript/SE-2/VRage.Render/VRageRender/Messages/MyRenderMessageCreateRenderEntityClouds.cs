namespace VRageRender.Messages
{
	public class MyRenderMessageCreateRenderEntityClouds : MyRenderMessageBase
	{
		public MyCloudLayerSettingsRender Settings;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateRenderEntityClouds;

		public override string ToString()
		{
			return Settings.DebugName ?? (string.Empty + ", " + Settings.Model);
		}
	}
}
