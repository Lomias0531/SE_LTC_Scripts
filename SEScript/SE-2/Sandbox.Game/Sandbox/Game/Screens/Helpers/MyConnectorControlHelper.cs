using Sandbox.Game.Entities;
using Sandbox.Game.Localization;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyConnectorControlHelper : MyControllableEntityControlHelper
	{
		private MyShipController ShipController => m_entity as MyShipController;

		public override bool Enabled => ShipController.CubeGrid.GridSystems.ConveyorSystem.IsInteractionPossible;

		public MyConnectorControlHelper()
			: base(MyControlsSpace.LANDING_GEAR, delegate(IMyControllableEntity x)
			{
				x.SwitchLandingGears();
			}, (IMyControllableEntity x) => GetConnectorStatus(x), MySpaceTexts.ControlMenuItemLabel_Connectors)
		{
		}

		public new void SetEntity(IMyControllableEntity entity)
		{
			m_entity = (entity as MyShipController);
		}

		private static bool GetConnectorStatus(IMyControllableEntity shipController)
		{
			return (shipController as MyShipController).CubeGrid.GridSystems.ConveyorSystem.Connected;
		}
	}
}
