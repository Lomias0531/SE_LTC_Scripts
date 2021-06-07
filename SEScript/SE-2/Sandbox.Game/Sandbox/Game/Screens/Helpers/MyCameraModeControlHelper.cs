using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyCameraModeControlHelper : MyAbstractControlMenuItem
	{
		private string m_value;

		public override bool Enabled => MyGuiScreenGamePlay.Static.CanSwitchCamera;

		public override string CurrentValue => m_value;

		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode);

		public MyCameraModeControlHelper()
			: base(MyControlsSpace.CAMERA_MODE)
		{
		}

		public override void Activate()
		{
			MyGuiScreenGamePlay.Static.SwitchCamera();
		}

		public override void UpdateValue()
		{
			if (MySession.Static.CameraController.IsInFirstPersonView)
			{
				m_value = MyTexts.GetString(MySpaceTexts.ControlMenuItemValue_FPP);
			}
			else
			{
				m_value = MyTexts.GetString(MySpaceTexts.ControlMenuItemValue_TPP);
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
