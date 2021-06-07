using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_DeathWish : MySteamAchievementBase
	{
		private const float EndValue = 300f;

		private bool m_conditionsMet;

		private int m_lastElapsedMinutes;

		private int m_totalMinutesPlayedInArmageddonSettings;

		public override bool NeedsUpdate => true;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_DeathWish", "DeathWish_MinutesPlayed", 300f);
		}

		public override void SessionLoad()
		{
			m_conditionsMet = (MySession.Static.Settings.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM_UNREAL && !MySession.Static.CreativeMode);
			m_lastElapsedMinutes = 0;
		}

		protected override void LoadStatValue()
		{
			m_totalMinutesPlayedInArmageddonSettings = m_remoteAchievement.StatValueInt;
		}

		public override void SessionUpdate()
		{
			if (!m_conditionsMet)
			{
				return;
			}
			int num = (int)MySession.Static.ElapsedPlayTime.TotalMinutes;
			if (m_lastElapsedMinutes < num)
			{
				m_lastElapsedMinutes = num;
				m_totalMinutesPlayedInArmageddonSettings++;
				m_remoteAchievement.StatValueInt = m_totalMinutesPlayedInArmageddonSettings;
				if ((float)m_totalMinutesPlayedInArmageddonSettings > 300f)
				{
					NotifyAchieved();
				}
			}
		}
	}
}
