using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyBriefingMenuControlHelper : MyAbstractControlMenuItem
	{
		private IMyControllableEntity m_entity;

		public override bool Enabled
		{
			get
			{
				if (base.Enabled)
				{
					return MyFakes.ENABLE_MISSION_TRIGGERS;
				}
				return false;
			}
		}

		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ScenarioBriefing);

		public MyBriefingMenuControlHelper()
			: base(MyControlsSpace.MISSION_SETTINGS)
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiSandbox.AddScreen(new MyGuiScreenBriefing());
		}
	}
}
