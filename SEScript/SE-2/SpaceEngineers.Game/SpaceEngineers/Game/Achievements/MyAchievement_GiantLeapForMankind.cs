using Sandbox.Engine.Physics;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
	internal class MyAchievement_GiantLeapForMankind : MySteamAchievementBase
	{
		private const double CHECK_INTERVAL_S = 3.0;

		private const int DISTANCE_TO_BE_WALKED = 1969;

		private float m_metersWalkedOnMoon;

		private readonly List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();

		private double m_lastCheckS;

		public override bool NeedsUpdate => !base.IsAchieved;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_GiantLeapForMankind", "GiantLeapForMankind_WalkedMoon", 1969f);
		}

		protected override void LoadStatValue()
		{
			m_metersWalkedOnMoon = m_remoteAchievement.StatValueFloat;
		}

		public override void SessionBeforeStart()
		{
			m_lastCheckS = 0.0;
		}

		public override void SessionUpdate()
		{
			if (MySession.Static?.LocalCharacter?.Physics == null)
			{
				return;
			}
			double num = MySession.Static.ElapsedPlayTime.TotalSeconds - m_lastCheckS;
			if (num < 3.0)
			{
				return;
			}
			m_lastCheckS = MySession.Static.ElapsedPlayTime.TotalSeconds;
			double num2 = MySession.Static.LocalCharacter.Physics.LinearVelocity.Length();
			if ((MyCharacter.IsWalkingState(MySession.Static.LocalCharacter.GetCurrentMovementState()) || MyCharacter.IsRunningState(MySession.Static.LocalCharacter.GetCurrentMovementState())) && IsWalkingOnMoon(MySession.Static.LocalCharacter))
			{
				m_metersWalkedOnMoon += (float)(num * num2);
				m_remoteAchievement.StatValueFloat = m_metersWalkedOnMoon;
				if (m_metersWalkedOnMoon >= 1969f)
				{
					NotifyAchieved();
				}
			}
		}

		private bool IsWalkingOnMoon(MyCharacter character)
		{
			float dEFAULT_GROUND_SEARCH_DISTANCE = MyConstants.DEFAULT_GROUND_SEARCH_DISTANCE;
			Vector3D vector3D = character.PositionComp.GetPosition() + character.PositionComp.WorldMatrix.Up * 0.5;
			Vector3D to = vector3D + character.PositionComp.WorldMatrix.Down * dEFAULT_GROUND_SEARCH_DISTANCE;
			MyPhysics.CastRay(vector3D, to, m_hits, 18);
			int i;
			for (i = 0; i < m_hits.Count && (m_hits[i].HkHitInfo.Body == null || m_hits[i].HkHitInfo.GetHitEntity() == character.Components); i++)
			{
			}
			if (m_hits.Count == 0)
			{
				return false;
			}
			if (i < m_hits.Count)
			{
				MyPhysics.HitInfo hitInfo = m_hits[i];
				IMyEntity hitEntity = hitInfo.HkHitInfo.GetHitEntity();
				if (Vector3D.DistanceSquared(hitInfo.Position, vector3D) < (double)(dEFAULT_GROUND_SEARCH_DISTANCE * dEFAULT_GROUND_SEARCH_DISTANCE))
				{
					MyVoxelBase myVoxelBase = hitEntity as MyVoxelBase;
					if (myVoxelBase != null && myVoxelBase.Storage != null && myVoxelBase.Storage.DataProvider != null && myVoxelBase.Storage.DataProvider is MyPlanetStorageProvider)
					{
						MyPlanetStorageProvider myPlanetStorageProvider = myVoxelBase.Storage.DataProvider as MyPlanetStorageProvider;
						if (myPlanetStorageProvider.Generator != null && myPlanetStorageProvider.Generator.FolderName == "Moon")
						{
							m_hits.Clear();
							return true;
						}
					}
				}
			}
			m_hits.Clear();
			return false;
		}
	}
}
