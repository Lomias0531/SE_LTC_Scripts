using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System.Collections;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_Explorer : MySteamAchievementBase
	{
		private const uint CHECK_INTERVAL_S = 3u;

		private const uint PLANET_COUNT = 6u;

		private BitArray m_exploredPlanetData;

		private readonly int[] m_bitArrayConversionArray = new int[1];

		private int m_planetsDiscovered;

		private uint m_lastCheckS;

		private readonly Dictionary<MyStringHash, int> m_planetNamesToIndexes = new Dictionary<MyStringHash, int>();

		private bool m_globalConditionsMet;

		public override bool NeedsUpdate => m_globalConditionsMet;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_Explorer", "Explorer_ExplorePlanetsData", 6f);
		}

		public override void Init()
		{
			base.Init();
			if (!base.IsAchieved)
			{
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Alien"), 0);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("EarthLike"), 1);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Europa"), 2);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Mars"), 3);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Moon"), 4);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Titan"), 5);
			}
		}

		public override void SessionLoad()
		{
			m_globalConditionsMet = !MySession.Static.CreativeMode;
			m_lastCheckS = 0u;
		}

		public override void SessionUpdate()
		{
			if (MySession.Static.LocalCharacter == null || base.IsAchieved)
			{
				return;
			}
			uint num = (uint)MySession.Static.ElapsedPlayTime.TotalSeconds;
			if (num - m_lastCheckS <= 3 || MySession.Static.LocalCharacter == null)
			{
				return;
			}
			Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
			Vector3 value = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
			m_lastCheckS = num;
			if (value == Vector3.Zero)
			{
				return;
			}
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
			if (closestPlanet == null || !m_planetNamesToIndexes.TryGetValue(closestPlanet.Generator.Id.SubtypeId, out int value2) || m_exploredPlanetData[value2])
			{
				return;
			}
			m_exploredPlanetData[value2] = true;
			m_planetsDiscovered = 0;
			for (int i = 0; (long)i < 6L; i++)
			{
				if (m_exploredPlanetData[i])
				{
					m_planetsDiscovered++;
				}
			}
			StoreSteamData();
			if ((long)m_planetsDiscovered < 6L)
			{
				m_remoteAchievement.IndicateProgress((uint)m_planetsDiscovered);
			}
			else
			{
				NotifyAchieved();
			}
		}

		protected override void LoadStatValue()
		{
			int statValueConditionBitField = m_remoteAchievement.StatValueConditionBitField;
			m_exploredPlanetData = new BitArray(new int[1]
			{
				statValueConditionBitField
			});
		}

		private void StoreSteamData()
		{
			m_exploredPlanetData.CopyTo(m_bitArrayConversionArray, 0);
			m_remoteAchievement.StatValueConditionBitField = m_bitArrayConversionArray[0];
		}
	}
}
