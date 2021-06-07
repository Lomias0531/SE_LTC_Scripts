using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;

namespace SpaceEngineers.Game.Achievements
{
	internal class MyAchievement_TheHarderTheyFall : MySteamAchievementBase
	{
		private const float DESTROY_BLOCK_MASS_KG = 1000000f;

		private float m_massDestroyed;

		public override bool NeedsUpdate => false;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_TheHarderTheyFall", "TheHarderTheyFall_MassDestroyed", 1000000f);
		}

		protected override void LoadStatValue()
		{
			m_massDestroyed = m_remoteAchievement.StatValueFloat;
		}

		public override void SessionBeforeStart()
		{
			if (!base.IsAchieved)
			{
				MyCubeGrids.BlockDestroyed += MyCubeGridsOnBlockDestroyed;
			}
		}

		private void MyCubeGridsOnBlockDestroyed(MyCubeGrid myCubeGrid, MySlimBlock mySlimBlock)
		{
			if (!MySession.Static.CreativeMode)
			{
				m_massDestroyed += mySlimBlock.GetMass();
				m_remoteAchievement.StatValueFloat = m_massDestroyed;
				if (m_massDestroyed >= 1000000f)
				{
					NotifyAchieved();
					MyCubeGrids.BlockDestroyed -= MyCubeGridsOnBlockDestroyed;
				}
			}
		}
	}
}
