using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;

namespace SpaceEngineers.Game.Achievements
{
	internal class MyAchievement_Colorblind : MySteamAchievementBase
	{
		private const int NUMBER_OF_COLORS_TO_ACHIEV = 20;

		private bool m_isUpdating = true;

		public override bool NeedsUpdate => m_isUpdating;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievment_ColorBlind", null, 0f);
		}

		public override void SessionLoad()
		{
			base.SessionLoad();
			if (!base.IsAchieved)
			{
				m_isUpdating = true;
			}
		}

		public override void SessionUpdate()
		{
			base.SessionUpdate();
			if (m_isUpdating && MySession.Static.LocalHumanPlayer != null)
			{
				MySession.Static.LocalHumanPlayer.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged;
				m_isUpdating = false;
			}
		}

		private void Controller_ControlledEntityChanged(IMyControllableEntity oldEnt, IMyControllableEntity newEnt)
		{
			if (newEnt != null && !MyCampaignManager.Static.IsCampaignRunning && newEnt.Entity is MyCockpit)
			{
				MyCubeGrid myCubeGrid = newEnt.Entity.Parent as MyCubeGrid;
				if (myCubeGrid != null && (newEnt.Entity as MyCockpit).BuiltBy == MySession.Static.LocalHumanPlayer.Identity.IdentityId && myCubeGrid.NumberOfGridColors >= 20)
				{
					NotifyAchieved();
					MySession.Static.LocalHumanPlayer.Controller.ControlledEntityChanged -= Controller_ControlledEntityChanged;
				}
			}
		}
	}
}
