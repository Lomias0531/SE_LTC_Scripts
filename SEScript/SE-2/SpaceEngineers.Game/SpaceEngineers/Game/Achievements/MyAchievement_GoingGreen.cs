using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_GoingGreen : MySteamAchievementBase
	{
		private const float EndValue = 25f;

		private int m_solarPanelsBuilt;

		public override bool NeedsUpdate => false;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_GoingGreen", "GoingGreen_SolarPanelsBuilt", 25f);
		}

		protected override void LoadStatValue()
		{
			m_solarPanelsBuilt = m_remoteAchievement.StatValueInt;
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
			if (MySession.Static != null && mySlimBlock != null && mySlimBlock.FatBlock != null && !MySession.Static.CreativeMode && mySlimBlock.BuiltBy == MySession.Static.LocalPlayerId && mySlimBlock.FatBlock is MySolarPanel)
			{
				m_solarPanelsBuilt++;
				m_remoteAchievement.StatValueInt = m_solarPanelsBuilt;
				if ((float)m_solarPanelsBuilt >= 25f)
				{
					NotifyAchieved();
					MyCubeGrids.BlockBuilt -= MyCubeGridsOnBlockBuilt;
				}
			}
		}
	}
}
