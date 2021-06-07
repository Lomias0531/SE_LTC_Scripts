namespace VRageRender.Messages
{
	public abstract class MyDebugRenderMessage : MyRenderMessageBase
	{
		public bool Persistent
		{
			get;
			set;
		}

		public override bool IsPersistent => Persistent;

		public override MyRenderMessageType MessageClass
		{
			get
			{
				if (!IsPersistent)
				{
					return MyRenderMessageType.DebugDraw;
				}
				return MyRenderMessageType.StateChangeOnce;
			}
		}
	}
}
