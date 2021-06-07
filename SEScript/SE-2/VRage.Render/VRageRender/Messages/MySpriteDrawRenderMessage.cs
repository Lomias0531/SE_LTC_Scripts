namespace VRageRender.Messages
{
	public abstract class MySpriteDrawRenderMessage : MyRenderMessageBase
	{
		public string TargetTexture
		{
			get;
			set;
		}

		/// <summary>If it has a target offscreen texture, then the message has to be processed earlier</summary>
		public override MyRenderMessageType MessageClass
		{
			get
			{
				if (TargetTexture != null)
				{
					return MyRenderMessageType.StateChangeOnce;
				}
				return MyRenderMessageType.Draw;
			}
		}
	}
}
