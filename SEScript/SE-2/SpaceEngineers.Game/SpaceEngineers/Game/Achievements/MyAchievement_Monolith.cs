using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_Monolith : MySteamAchievementBase
	{
		private const uint UPDATE_INTERVAL_S = 3u;

		private bool m_globalConditions;

		private uint m_lastTimeUpdatedS;

		private readonly List<MyCubeGrid> m_monolithGrids = new List<MyCubeGrid>();

		public override bool NeedsUpdate
		{
			get
			{
				if (!m_globalConditions)
				{
					return false;
				}
				return (uint)MySession.Static.ElapsedPlayTime.TotalSeconds - m_lastTimeUpdatedS > 3;
			}
		}

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_Monolith", null, 0f);
		}

		public override void SessionUpdate()
		{
			if (!base.IsAchieved && MySession.Static.LocalCharacter != null)
			{
				m_lastTimeUpdatedS = (uint)MySession.Static.ElapsedPlayTime.TotalSeconds;
				if (MySession.Static.LocalCharacter != null)
				{
					Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
					foreach (MyCubeGrid monolithGrid in m_monolithGrids)
					{
						Vector3D center = monolithGrid.PositionComp.WorldVolume.Center;
						if (Vector3D.DistanceSquared(position, center) < 400.0 + monolithGrid.PositionComp.WorldVolume.Radius)
						{
							NotifyAchieved();
							break;
						}
					}
				}
			}
		}

		public override void SessionBeforeStart()
		{
			if (!base.IsAchieved)
			{
				m_globalConditions = !MySession.Static.CreativeMode;
				if (m_globalConditions)
				{
					m_lastTimeUpdatedS = 0u;
					m_monolithGrids.Clear();
					foreach (MyEntity entity in MyEntities.GetEntities())
					{
						MyCubeGrid myCubeGrid = entity as MyCubeGrid;
						if (myCubeGrid != null && myCubeGrid.BlocksCount == 1 && myCubeGrid.CubeBlocks.FirstElement().BlockDefinition.Id.SubtypeId == MyStringHash.GetOrCompute("Monolith"))
						{
							m_monolithGrids.Add(myCubeGrid);
						}
					}
				}
			}
		}
	}
}
