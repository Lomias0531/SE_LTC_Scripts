using Sandbox.Game;
using Sandbox.Game.SessionComponents;

namespace SpaceEngineers.Game.Achievements
{
	internal class MyAchievement_ToTheStars : MySteamAchievementBase
	{
		public override bool NeedsUpdate => false;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_ToTheStars", null, 0f);
		}

		public override void SessionBeforeStart()
		{
			if (!base.IsAchieved)
			{
				MyCampaignManager.Static.OnCampaignFinished += Static_OnCampaignFinished;
			}
		}

		private void Static_OnCampaignFinished()
		{
			if (MyCampaignManager.Static.ActiveCampaign.IsVanilla)
			{
				NotifyAchieved();
				MyCampaignManager.Static.OnCampaignFinished -= Static_OnCampaignFinished;
			}
		}
	}
}
