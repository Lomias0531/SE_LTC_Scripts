namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateNewLoddingSettings : MyRenderMessageBase
	{
		public MyNewLoddingSettings Settings
		{
			get;
			private set;
		}

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateNewLoddingSettings;

		public MyRenderMessageUpdateNewLoddingSettings()
		{
			Settings = new MyNewLoddingSettings();
		}
	}
}
