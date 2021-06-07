using System;
using VRage.Game.Entity;

namespace Sandbox.Game.Gui
{
	public class MyHudGravityIndicator
	{
		internal MyEntity Entity;

		public bool Visible
		{
			get;
			private set;
		}

		public void Show(Action<MyHudGravityIndicator> propertiesInit)
		{
			Visible = true;
			propertiesInit?.Invoke(this);
		}

		public void Clean()
		{
			Entity = null;
		}

		public void Hide()
		{
			Visible = false;
		}
	}
}
