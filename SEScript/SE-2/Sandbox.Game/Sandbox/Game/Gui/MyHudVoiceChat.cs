using System;

namespace Sandbox.Game.Gui
{
	public class MyHudVoiceChat
	{
		public bool Visible
		{
			get;
			private set;
		}

		public event Action<bool> VisibilityChanged;

		public void Show()
		{
			Visible = true;
			if (this.VisibilityChanged != null)
			{
				this.VisibilityChanged(obj: true);
			}
		}

		public void Hide()
		{
			Visible = false;
			if (this.VisibilityChanged != null)
			{
				this.VisibilityChanged(obj: false);
			}
		}
	}
}
