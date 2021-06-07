using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyNewGameScreenABTestHelper
	{
		private static MyNewGameScreenABTestHelper m_instance;

		private bool m_isActive;

		private bool m_isApplied;

		public static MyNewGameScreenABTestHelper Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyNewGameScreenABTestHelper();
				}
				return m_instance;
			}
		}

		private MyNewGameScreenABTestHelper()
		{
			Clear();
		}

		public void Clear()
		{
			m_isActive = false;
			m_isApplied = false;
		}

		public void ActivateTest()
		{
			m_isActive = true;
		}

		public bool IsActive()
		{
			return m_isActive;
		}

		public bool IsApplied()
		{
			return m_isApplied;
		}

		public bool IsA()
		{
			if (Sync.MyId % 2uL == 1)
			{
				return true;
			}
			return false;
		}

		public bool ApplyTest()
		{
			if (!m_isActive)
			{
				return false;
			}
			MySandboxGame.Config.EnableNewNewGameScreen = (IsA() && MyFakes.ENABLE_SIMPLE_NEWGAME_SCREEN);
			m_isApplied = true;
			return true;
		}
	}
}
