using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_MasterEngineer : MySteamAchievementBase
	{
		public const int EndValue = 9000;

		private int m_totalMinutesPlayed;

		private int m_lastLoggedMinute;

		public override bool NeedsUpdate => true;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_MasterEngineer", "MasterEngineer_MinutesPlayed", 9000f);
		}

		protected override void LoadStatValue()
		{
			m_totalMinutesPlayed = m_remoteAchievement.StatValueInt;
		}

		public override void SessionLoad()
		{
			m_lastLoggedMinute = 0;
		}

		public override void SessionUpdate()
		{
			int num = (int)MySession.Static.ElapsedPlayTime.TotalMinutes;
			if (m_lastLoggedMinute < num)
			{
				m_totalMinutesPlayed++;
				m_lastLoggedMinute = num;
				m_remoteAchievement.StatValueInt = m_totalMinutesPlayed;
				if (m_totalMinutesPlayed > 9000)
				{
					NotifyAchieved();
				}
			}
		}
	}
}
