using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyEnableStationRotationControlHelper : MyAbstractControlMenuItem
	{
		private IMyControllableEntity m_entity;

		public override string Label => MyTexts.GetString(MySpaceTexts.StationRotation_Static);

		public MyEnableStationRotationControlHelper()
			: base(MyControlsSpace.FREE_ROTATION)
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
		}
	}
}
