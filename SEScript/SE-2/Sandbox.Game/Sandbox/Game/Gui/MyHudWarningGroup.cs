using System.Collections.Generic;

namespace Sandbox.Game.Gui
{
	internal class MyHudWarningGroup
	{
		private List<MyHudWarning> m_hudWarnings;

		private bool m_canBeTurnedOff;

		private int m_msSinceLastCuePlayed;

		private int m_highestWarnedPriority = int.MaxValue;

		public MyHudWarningGroup(List<MyHudWarning> hudWarnings, bool canBeTurnedOff)
		{
			m_hudWarnings = new List<MyHudWarning>(hudWarnings);
			SortByPriority();
			m_canBeTurnedOff = canBeTurnedOff;
			InitLastCuePlayed();
			foreach (MyHudWarning warning in hudWarnings)
			{
				warning.CanPlay = (() => m_highestWarnedPriority > warning.WarningPriority || (m_msSinceLastCuePlayed > warning.RepeatInterval && m_highestWarnedPriority == warning.WarningPriority));
				warning.Played = delegate
				{
					m_msSinceLastCuePlayed = 0;
					m_highestWarnedPriority = warning.WarningPriority;
				};
			}
		}

		private void InitLastCuePlayed()
		{
			foreach (MyHudWarning hudWarning in m_hudWarnings)
			{
				if (hudWarning.RepeatInterval > m_msSinceLastCuePlayed)
				{
					m_msSinceLastCuePlayed = hudWarning.RepeatInterval;
				}
			}
		}

		public void Update()
		{
			if (MySandboxGame.IsGameReady)
			{
				m_msSinceLastCuePlayed += 16 * MyHudWarnings.FRAMES_BETWEEN_UPDATE;
				bool flag = false;
				foreach (MyHudWarning hudWarning in m_hudWarnings)
				{
					if (hudWarning.Update(flag))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					m_highestWarnedPriority = int.MaxValue;
				}
			}
		}

		public void Add(MyHudWarning hudWarning)
		{
			m_hudWarnings.Add(hudWarning);
			SortByPriority();
			InitLastCuePlayed();
			hudWarning.CanPlay = (() => m_highestWarnedPriority > hudWarning.WarningPriority || (m_msSinceLastCuePlayed > hudWarning.RepeatInterval && m_highestWarnedPriority == hudWarning.WarningPriority));
			hudWarning.Played = delegate
			{
				m_msSinceLastCuePlayed = 0;
				m_highestWarnedPriority = hudWarning.WarningPriority;
			};
		}

		public void Remove(MyHudWarning hudWarning)
		{
			m_hudWarnings.Remove(hudWarning);
		}

		public void Clear()
		{
			m_hudWarnings.Clear();
		}

		private void SortByPriority()
		{
			m_hudWarnings.Sort((MyHudWarning x, MyHudWarning y) => x.WarningPriority.CompareTo(y.WarningPriority));
		}
	}
}
