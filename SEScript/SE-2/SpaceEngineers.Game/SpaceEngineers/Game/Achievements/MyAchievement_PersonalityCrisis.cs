using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_PersonalityCrisis : MySteamAchievementBase
	{
		private const int NUMBER_OF_CHANGES_REQUIRED = 20;

		private const int MAXIMUM_TIMER = 600;

		private uint[] m_startS;

		private int m_timerIndex;

		public override bool NeedsUpdate
		{
			get
			{
				if (m_startS != null)
				{
					return m_startS[m_timerIndex] != uint.MaxValue;
				}
				return false;
			}
		}

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_PersonalityCrisis", null, 0f);
		}

		public override void SessionLoad()
		{
			if (!base.IsAchieved)
			{
				m_startS = new uint[20];
				for (int i = 0; i < m_startS.Length; i++)
				{
					m_startS[i] = uint.MaxValue;
				}
				MyGuiScreenLoadInventory.LookChanged += MyGuiScreenWardrobeOnLookChanged;
			}
		}

		public override void SessionUnload()
		{
			base.SessionUnload();
			MyGuiScreenLoadInventory.LookChanged -= MyGuiScreenWardrobeOnLookChanged;
		}

		private void MyGuiScreenWardrobeOnLookChanged()
		{
			m_startS[m_timerIndex] = (uint)MySession.Static.ElapsedPlayTime.TotalSeconds;
			m_timerIndex++;
			m_timerIndex %= 20;
			if (m_startS[m_timerIndex] != uint.MaxValue)
			{
				MyGuiScreenLoadInventory.LookChanged -= MyGuiScreenWardrobeOnLookChanged;
				NotifyAchieved();
			}
		}

		public override void SessionUpdate()
		{
			if (base.IsAchieved)
			{
				return;
			}
			uint num = (uint)MySession.Static.ElapsedPlayTime.TotalSeconds;
			for (int i = 0; i < m_startS.Length; i++)
			{
				if (m_startS[i] != uint.MaxValue && num - m_startS[i] > 600)
				{
					m_startS[i] = uint.MaxValue;
				}
			}
		}
	}
}
