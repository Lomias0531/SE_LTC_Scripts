using Sandbox.Game.AI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Modules;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 1111, typeof(MyObjectBuilder_EnvironmentBotSpawningSystem), null)]
	public class MyEnvironmentBotSpawningSystem : MySessionComponentBase
	{
		private static readonly int DELAY_BETWEEN_TICKS_IN_MS = 120000;

		private static readonly float BOT_SPAWN_RANGE_MIN = 80f;

		private static readonly float BOT_SPAWN_RANGE_MIN_SQ = BOT_SPAWN_RANGE_MIN * BOT_SPAWN_RANGE_MIN;

		private static readonly float BOT_DESPAWN_DISTANCE = 400f;

		private static readonly float BOT_DESPAWN_DISTANCE_SQ = BOT_DESPAWN_DISTANCE * BOT_DESPAWN_DISTANCE;

		private static readonly int MAX_SPAWN_ATTEMPTS = 5;

		public static MyEnvironmentBotSpawningSystem Static;

		private MyRandom m_random = new MyRandom();

		private List<Vector3D> m_tmpPlayerPositions;

		private HashSet<MyBotSpawningEnvironmentProxy> m_activeBotSpawningProxies;

		private int m_lastSpawnEventTimeInMs;

		private int m_timeSinceLastEventInMs;

		private int m_tmpSpawnAttempts;

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyAIComponent)
		};

		public override bool IsRequiredByGame => MyPerGameSettings.EnableAi;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_EnvironmentBotSpawningSystem myObjectBuilder_EnvironmentBotSpawningSystem = sessionComponent as MyObjectBuilder_EnvironmentBotSpawningSystem;
			m_timeSinceLastEventInMs = myObjectBuilder_EnvironmentBotSpawningSystem.TimeSinceLastEventInMs;
			m_lastSpawnEventTimeInMs = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_timeSinceLastEventInMs;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_EnvironmentBotSpawningSystem obj = base.GetObjectBuilder() as MyObjectBuilder_EnvironmentBotSpawningSystem;
			obj.TimeSinceLastEventInMs = m_timeSinceLastEventInMs;
			return obj;
		}

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
			m_tmpPlayerPositions = new List<Vector3D>();
			m_activeBotSpawningProxies = new HashSet<MyBotSpawningEnvironmentProxy>();
			_ = Sync.IsServer;
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			_ = Sync.IsServer;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
			m_tmpPlayerPositions = null;
			m_activeBotSpawningProxies = null;
			_ = Sync.IsServer;
		}

		public override void Draw()
		{
			base.Draw();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Sync.IsServer)
			{
				m_timeSinceLastEventInMs = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastSpawnEventTimeInMs;
				if (m_timeSinceLastEventInMs >= DELAY_BETWEEN_TICKS_IN_MS)
				{
					RemoveDistantBots();
					MyAIComponent.Static.CleanUnusedIdentities();
					m_tmpSpawnAttempts = 0;
					SpawnTick();
					m_lastSpawnEventTimeInMs = MySandboxGame.TotalGamePlayTimeInMilliseconds;
					m_timeSinceLastEventInMs = 0;
				}
			}
		}

		public void RemoveDistantBots()
		{
			ICollection<MyPlayer> onlinePlayers = Sync.Players.GetOnlinePlayers();
			m_tmpPlayerPositions.Capacity = Math.Max(m_tmpPlayerPositions.Capacity, onlinePlayers.Count);
			m_tmpPlayerPositions.Clear();
			foreach (MyPlayer item in onlinePlayers)
			{
				if (item.Id.SerialId == 0 && item.Controller.ControlledEntity != null)
				{
					Vector3D position = item.GetPosition();
					m_tmpPlayerPositions.Add(position);
				}
			}
			foreach (MyPlayer item2 in onlinePlayers)
			{
				if (item2.Controller.ControlledEntity != null && item2.Id.SerialId != 0)
				{
					bool flag = true;
					Vector3D position2 = item2.GetPosition();
					foreach (Vector3D tmpPlayerPosition in m_tmpPlayerPositions)
					{
						if (Vector3D.DistanceSquared(position2, tmpPlayerPosition) < (double)BOT_DESPAWN_DISTANCE_SQ)
						{
							flag = false;
						}
					}
					if (flag)
					{
						MyAIComponent.Static.RemoveBot(item2.Id.SerialId, removeCharacter: true);
					}
				}
			}
		}

		public void SpawnTick()
		{
			if (m_activeBotSpawningProxies.Count != 0 && m_tmpSpawnAttempts <= MAX_SPAWN_ATTEMPTS)
			{
				m_tmpSpawnAttempts++;
				int randomInt = MyUtils.GetRandomInt(0, m_activeBotSpawningProxies.Count);
				if (!m_activeBotSpawningProxies.ElementAt(randomInt).OnSpawnTick())
				{
					SpawnTick();
				}
			}
		}

		public void RegisterBotSpawningProxy(MyBotSpawningEnvironmentProxy proxy)
		{
			m_activeBotSpawningProxies.Add(proxy);
		}

		public void UnregisterBotSpawningProxy(MyBotSpawningEnvironmentProxy proxy)
		{
			m_activeBotSpawningProxies.Remove(proxy);
		}

		public bool IsHumanPlayerWithinRange(Vector3 position)
		{
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SerialId == 0 && onlinePlayer.Controller.ControlledEntity != null && Vector3.DistanceSquared(onlinePlayer.GetPosition(), position) < BOT_SPAWN_RANGE_MIN_SQ)
				{
					return false;
				}
			}
			return true;
		}
	}
}
