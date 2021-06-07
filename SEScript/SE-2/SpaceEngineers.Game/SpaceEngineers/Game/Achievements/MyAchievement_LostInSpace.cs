using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_LostInSpace : MySteamAchievementBase
	{
		public const int CHECK_INTERVAL_MS = 3000;

		public const int MAXIMUM_TIME_S = 3600;

		private int m_startedS;

		private double m_lastTimeChecked;

		private bool m_conditionsValid;

		private List<MyPhysics.HitInfo> m_hitInfoResults;

		public override bool NeedsUpdate => m_conditionsValid;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_LostInSpace", "LostInSpace_LostInSpaceStartedS", 3600f);
		}

		protected override void LoadStatValue()
		{
			m_startedS = m_remoteAchievement.StatValueInt;
		}

		public override void SessionLoad()
		{
			m_conditionsValid = (MyMultiplayer.Static != null);
			m_lastTimeChecked = 0.0;
		}

		public override void SessionUpdate()
		{
			if (!m_conditionsValid)
			{
				return;
			}
			double totalMilliseconds = MySession.Static.ElapsedPlayTime.TotalMilliseconds;
			double num = totalMilliseconds - m_lastTimeChecked;
			if (!(num > 3000.0))
			{
				return;
			}
			m_lastTimeChecked = totalMilliseconds;
			m_startedS += (int)(num / 1000.0);
			if (MySession.Static.Players.GetOnlinePlayerCount() == 1)
			{
				LoadStatValue();
				return;
			}
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				if (onlinePlayer != MySession.Static.LocalHumanPlayer && IsThePlayerInSight(onlinePlayer))
				{
					LoadStatValue();
					return;
				}
			}
			m_remoteAchievement.StatValueInt = m_startedS;
			if (m_startedS >= 3600)
			{
				NotifyAchieved();
			}
		}

		private bool IsThePlayerInSight(MyPlayer player)
		{
			if (player.Character == null)
			{
				return false;
			}
			if (MySession.Static.LocalCharacter == null)
			{
				return false;
			}
			Vector3D position = player.Character.PositionComp.GetPosition();
			Vector3D position2 = MySession.Static.LocalCharacter.PositionComp.GetPosition();
			if (Vector3D.DistanceSquared(position, position2) > 4000000.0)
			{
				return false;
			}
			using (MyUtils.ReuseCollection(ref m_hitInfoResults))
			{
				MyPhysics.CastRay(position, position2, m_hitInfoResults);
				foreach (MyPhysics.HitInfo hitInfoResult in m_hitInfoResults)
				{
					if (!(hitInfoResult.HkHitInfo.GetHitEntity() is MyCharacter))
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}
