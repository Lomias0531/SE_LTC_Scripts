using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;

namespace SpaceEngineers.Game.Achievements
{
	internal class MyAchievement_TheBiggerTheyAre : MySteamAchievementBase
	{
		private const float BUILT_BLOCK_MASS_KG = 1000000f;

		private int m_massBuilt;

		public override bool NeedsUpdate => false;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_TheBiggerTheyAre", "TheBiggerTheyAre_MassBuilt", 1000000f);
		}

		protected override void LoadStatValue()
		{
			m_massBuilt = m_remoteAchievement.StatValueInt;
		}

		public override void SessionBeforeStart()
		{
			if (!base.IsAchieved)
			{
				MyCubeGrids.BlockBuilt += MyCubeGridsOnBlockBuilt;
			}
		}

		private void MyCubeGridsOnBlockBuilt(MyCubeGrid myCubeGrid, MySlimBlock mySlimBlock)
		{
			if (!MySession.Static.CreativeMode)
			{
				m_massBuilt += (int)mySlimBlock.GetMass();
				if ((float)m_massBuilt < 1000000f)
				{
					m_remoteAchievement.StatValueInt = m_massBuilt;
					return;
				}
				m_remoteAchievement.StatValueInt = 1000000;
				NotifyAchieved();
				MyCubeGrids.BlockBuilt -= MyCubeGridsOnBlockBuilt;
			}
		}
	}
}
