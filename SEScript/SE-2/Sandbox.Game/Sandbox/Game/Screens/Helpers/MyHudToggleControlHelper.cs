using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyHudToggleControlHelper : MyAbstractControlMenuItem
	{
		private string m_value;

		public override string CurrentValue => m_value;

		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ToggleHud);

		public MyHudToggleControlHelper()
			: base(MyControlsSpace.TOGGLE_HUD)
		{
		}

		public override void Activate()
		{
			MyHud.MinimalHud = !MyHud.MinimalHud;
		}

		public override void UpdateValue()
		{
			if (!MyHud.MinimalHud)
			{
				m_value = MyTexts.GetString(MyCommonTexts.ControlMenuItemValue_On);
			}
			else
			{
				m_value = MyTexts.GetString(MyCommonTexts.ControlMenuItemValue_Off);
			}
		}

		public override void Next()
		{
			Activate();
		}

		public override void Previous()
		{
			Activate();
		}
	}
}
