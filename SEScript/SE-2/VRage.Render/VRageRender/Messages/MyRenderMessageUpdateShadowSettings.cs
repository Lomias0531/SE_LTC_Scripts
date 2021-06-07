namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateShadowSettings : MyRenderMessageBase
	{
		public MyShadowsSettings Settings
		{
			get;
			private set;
		}

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateShadowSettings;

		public MyRenderMessageUpdateShadowSettings()
		{
			Settings = new MyShadowsSettings();
		}
	}
}
