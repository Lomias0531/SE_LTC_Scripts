using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyLandingGearControlHelper : MyControllableEntityControlHelper
	{
		private MyShipController ShipController => m_entity as MyShipController;

		public override bool Enabled => ShipController.CubeGrid.GridSystems.LandingSystem.Locked != MyMultipleEnabledEnum.NoObjects;

		public MyLandingGearControlHelper()
			: base(MyControlsSpace.LANDING_GEAR, delegate(IMyControllableEntity x)
			{
				x.SwitchLandingGears();
			}, (IMyControllableEntity x) => x.EnabledLeadingGears, MySpaceTexts.ControlMenuItemLabel_LandingGear)
		{
		}

		public new void SetEntity(IMyControllableEntity entity)
		{
			m_entity = (entity as MyShipController);
		}
	}
}
