using ParallelTasks;
using Sandbox;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.Entities.Blocks;
using SpaceEngineers.Game.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.World
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MySpaceRespawnComponent : MyRespawnComponentBase
	{
		[Serializable]
		private struct RespawnCooldownEntry
		{
			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003ERespawnCooldownEntry_003C_003EControllerId_003C_003EAccessor : IMemberAccessor<RespawnCooldownEntry, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnCooldownEntry owner, in int value)
				{
					owner.ControllerId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnCooldownEntry owner, out int value)
				{
					value = owner.ControllerId;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003ERespawnCooldownEntry_003C_003EShipId_003C_003EAccessor : IMemberAccessor<RespawnCooldownEntry, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnCooldownEntry owner, in string value)
				{
					owner.ShipId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnCooldownEntry owner, out string value)
				{
					value = owner.ShipId;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003ERespawnCooldownEntry_003C_003ERelativeRespawnTime_003C_003EAccessor : IMemberAccessor<RespawnCooldownEntry, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnCooldownEntry owner, in int value)
				{
					owner.RelativeRespawnTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnCooldownEntry owner, out int value)
				{
					value = owner.RelativeRespawnTime;
				}
			}

			public int ControllerId;

			public string ShipId;

			public int RelativeRespawnTime;
		}

		private struct RespawnKey : IEquatable<RespawnKey>
		{
			public MyPlayer.PlayerId ControllerId;

			public string RespawnShipId;

			public bool Equals(RespawnKey other)
			{
				if (ControllerId == other.ControllerId)
				{
					return RespawnShipId == other.RespawnShipId;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return ControllerId.GetHashCode() ^ ((RespawnShipId != null) ? RespawnShipId.GetHashCode() : 0);
			}
		}

		[Serializable]
		public class MyRespawnPointInfo
		{
			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EMedicalRoomId_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in long value)
				{
					owner.MedicalRoomId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out long value)
				{
					value = owner.MedicalRoomId;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EMedicalRoomGridId_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in long value)
				{
					owner.MedicalRoomGridId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out long value)
				{
					value = owner.MedicalRoomGridId;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EMedicalRoomName_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in string value)
				{
					owner.MedicalRoomName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out string value)
				{
					value = owner.MedicalRoomName;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EOxygenLevel_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in float value)
				{
					owner.OxygenLevel = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out float value)
				{
					value = owner.OxygenLevel;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EOwnerId_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in long value)
				{
					owner.OwnerId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out long value)
				{
					value = owner.OwnerId;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EPrefferedCameraPosition_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in Vector3D value)
				{
					owner.PrefferedCameraPosition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out Vector3D value)
				{
					value = owner.PrefferedCameraPosition;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EMedicalRoomPosition_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in Vector3D value)
				{
					owner.MedicalRoomPosition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out Vector3D value)
				{
					value = owner.MedicalRoomPosition;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EMedicalRoomUp_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in Vector3D value)
				{
					owner.MedicalRoomUp = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out Vector3D value)
				{
					value = owner.MedicalRoomUp;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003C_003EMedicalRoomVelocity_003C_003EAccessor : IMemberAccessor<MyRespawnPointInfo, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyRespawnPointInfo owner, in Vector3 value)
				{
					owner.MedicalRoomVelocity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyRespawnPointInfo owner, out Vector3 value)
				{
					value = owner.MedicalRoomVelocity;
				}
			}

			public long MedicalRoomId;

			public long MedicalRoomGridId;

			public string MedicalRoomName;

			public float OxygenLevel;

			public long OwnerId;

			public Vector3D PrefferedCameraPosition;

			public Vector3D MedicalRoomPosition;

			public Vector3D MedicalRoomUp;

			public Vector3 MedicalRoomVelocity;
		}

		private struct SpawnInfo
		{
			/// <summary>
			/// Identity id the algorithm should look friends for
			/// </summary>
			public long IdentityId;

			/// <summary>
			/// Planet to spawn player on
			/// </summary>
			public MyPlanet Planet;

			/// <summary>
			/// Minimal clearance around the spawn area
			/// </summary>
			public float CollisionRadius;

			/// <summary>
			/// Suggested altitude above the planet (specified above) surface
			/// </summary>
			public float PlanetDeployAltitude;

			/// <summary>
			/// Minimal air density required at landing spot
			/// </summary>
			public float MinimalAirDensity;

			public bool SpawnNearPlayers;
		}

		[Serializable]
		private struct MOTDData
		{
			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMOTDData_003C_003EUrl_003C_003EAccessor : IMemberAccessor<MOTDData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MOTDData owner, in string value)
				{
					owner.Url = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MOTDData owner, out string value)
				{
					value = owner.Url;
				}
			}

			protected class SpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMOTDData_003C_003EMessage_003C_003EAccessor : IMemberAccessor<MOTDData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MOTDData owner, in string value)
				{
					owner.Message = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MOTDData owner, out string value)
				{
					value = owner.Message;
				}
			}

			public string Url;

			public string Message;

			public bool HasMessage => !string.IsNullOrEmpty(Message);

			public bool HasUrl
			{
				get
				{
					if (string.IsNullOrEmpty(Url))
					{
						return MyGuiSandbox.IsUrlValid(Url);
					}
					return false;
				}
			}

			public bool HasAnything()
			{
				if (!HasMessage)
				{
					return HasUrl;
				}
				return true;
			}

			public MOTDData(string url, string message)
			{
				Url = url;
				Message = message;
			}

			public StringBuilder GetMessage()
			{
				StringBuilder stringBuilder = new StringBuilder(Message);
				if (MySession.Static.LocalHumanPlayer != null)
				{
					stringBuilder.Replace(MyPerGameSettings.MotDCurrentPlayerVariable, MySession.Static.LocalHumanPlayer.DisplayName);
				}
				return stringBuilder;
			}

			public static MOTDData Construct()
			{
				if (!Sync.IsDedicated)
				{
					return new MOTDData(string.Empty, MySession.Static.Description ?? string.Empty);
				}
				string messageOfTheDayUrl = MySandboxGame.ConfigDedicated.MessageOfTheDayUrl;
				string text = MySandboxGame.ConfigDedicated.MessageOfTheDay;
				if (!string.IsNullOrEmpty(text))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(text);
					stringBuilder.Replace(MyPerGameSettings.MotDServerNameVariable, MySandboxGame.ConfigDedicated.ServerName);
					stringBuilder.Replace(MyPerGameSettings.MotDWorldNameVariable, MySandboxGame.ConfigDedicated.WorldName);
					stringBuilder.Replace(MyPerGameSettings.MotDServerDescriptionVariable, MySandboxGame.ConfigDedicated.ServerDescription);
					stringBuilder.Replace(MyPerGameSettings.MotDPlayerCountVariable, Sync.Players.GetOnlinePlayerCount().ToString());
					text = stringBuilder.ToString();
				}
				return new MOTDData(messageOfTheDayUrl, text);
			}
		}

		protected sealed class OnSyncCooldownRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSyncCooldownRequest();
			}
		}

		protected sealed class OnSyncCooldownResponse_003C_003ESystem_Collections_Generic_List_00601_003CSpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003ERespawnCooldownEntry_003E : ICallSite<IMyEventOwner, List<RespawnCooldownEntry>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<RespawnCooldownEntry> entries, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSyncCooldownResponse(entries);
			}
		}

		protected sealed class RespawnRequest_Implementation_003C_003ESystem_UInt64_0023System_Int32 : ICallSite<IMyEventOwner, ulong, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong steamPlayerId, in int serialId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RespawnRequest_Implementation(steamPlayerId, serialId);
			}
		}

		protected sealed class RequestRespawnAtSpawnPoint_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long spawnPointId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestRespawnAtSpawnPoint(spawnPointId);
			}
		}

		protected sealed class ShowMedicalScreen_Implementation_003C_003ESpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMOTDData_0023System_Int64 : ICallSite<IMyEventOwner, MOTDData, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MOTDData motd, in long restrictedRespawn, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowMedicalScreen_Implementation(motd, restrictedRespawn);
			}
		}

		protected sealed class ShowMotD_003C_003ESpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMOTDData : ICallSite<IMyEventOwner, MOTDData, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MOTDData motd, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowMotD(motd);
			}
		}

		private int m_lastUpdate;

		private bool m_updatingStopped;

		private int m_updateCtr;

		private bool m_synced;

		private readonly List<RespawnCooldownEntry> m_tmpRespawnTimes = new List<RespawnCooldownEntry>();

		private const int REPEATED_DEATH_TIME_SECONDS = 10;

		private readonly CachingDictionary<RespawnKey, int> m_globalRespawnTimesMs = new CachingDictionary<RespawnKey, int>();

		private static List<MyRespawnShipDefinition> m_respawnShipsCache;

		private static readonly List<MyRespawnPointInfo> m_respanwPointsCache;

		private static readonly List<Vector3D> m_playerPositionsCache;

		public bool IsSynced => m_synced;

		public static MySpaceRespawnComponent Static => Sync.Players.RespawnComponent as MySpaceRespawnComponent;

		public event Action<ulong> RespawnDoneEvent;

		static MySpaceRespawnComponent()
		{
			m_respanwPointsCache = new List<MyRespawnPointInfo>();
			m_playerPositionsCache = new List<Vector3D>();
		}

		public void RequestSync()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSyncCooldownRequest);
		}

		public override void InitFromCheckpoint(MyObjectBuilder_Checkpoint checkpoint)
		{
			List<MyObjectBuilder_Checkpoint.RespawnCooldownItem> respawnCooldowns = checkpoint.RespawnCooldowns;
			m_lastUpdate = MySandboxGame.TotalTimeInMilliseconds;
			m_globalRespawnTimesMs.Clear();
			if (respawnCooldowns != null)
			{
				foreach (MyObjectBuilder_Checkpoint.RespawnCooldownItem item in respawnCooldowns)
				{
					MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId);
					playerId.SteamId = item.PlayerSteamId;
					playerId.SerialId = item.PlayerSerialId;
					MyPlayer.PlayerId controllerId = playerId;
					RespawnKey respawnKey = default(RespawnKey);
					respawnKey.ControllerId = controllerId;
					respawnKey.RespawnShipId = item.RespawnShipId;
					RespawnKey key = respawnKey;
					m_globalRespawnTimesMs.Add(key, item.Cooldown + m_lastUpdate, immediate: true);
				}
			}
		}

		public override void SaveToCheckpoint(MyObjectBuilder_Checkpoint checkpoint)
		{
			List<MyObjectBuilder_Checkpoint.RespawnCooldownItem> respawnCooldowns = checkpoint.RespawnCooldowns;
			foreach (KeyValuePair<RespawnKey, int> globalRespawnTimesM in m_globalRespawnTimesMs)
			{
				int num = globalRespawnTimesM.Value - m_lastUpdate;
				if (num > 0)
				{
					MyObjectBuilder_Checkpoint.RespawnCooldownItem item = default(MyObjectBuilder_Checkpoint.RespawnCooldownItem);
					item.PlayerSteamId = globalRespawnTimesM.Key.ControllerId.SteamId;
					item.PlayerSerialId = globalRespawnTimesM.Key.ControllerId.SerialId;
					item.RespawnShipId = globalRespawnTimesM.Key.RespawnShipId;
					item.Cooldown = num;
					respawnCooldowns.Add(item);
				}
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			m_lastUpdate = MySandboxGame.TotalTimeInMilliseconds;
			m_updatingStopped = true;
			m_updateCtr = 0;
			if (!Sync.IsServer)
			{
				m_synced = false;
				RequestSync();
			}
			else
			{
				RequestSync();
				m_synced = true;
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			Sync.Players.RespawnComponent = this;
			Sync.Players.LocalRespawnRequested += OnLocalRespawnRequest;
			MyRespawnComponentBase.ShowPermaWarning = false;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Sync.Players.LocalRespawnRequested -= OnLocalRespawnRequest;
			Sync.Players.RespawnComponent = null;
		}

		[Event(null, 170)]
		[Reliable]
		[Server]
		private static void OnSyncCooldownRequest()
		{
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				Static.SyncCooldownToPlayer(Sync.MyId, isLocal: true);
			}
			else
			{
				Static.SyncCooldownToPlayer(MyEventContext.Current.Sender.Value, isLocal: false);
			}
		}

		[Event(null, 184)]
		[Reliable]
		[Client]
		private static void OnSyncCooldownResponse(List<RespawnCooldownEntry> entries)
		{
			Static.SyncCooldownResponse(entries);
		}

		private void SyncCooldownResponse(List<RespawnCooldownEntry> entries)
		{
			int totalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
			if (entries != null)
			{
				foreach (RespawnCooldownEntry entry in entries)
				{
					MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId);
					playerId.SteamId = Sync.MyId;
					playerId.SerialId = entry.ControllerId;
					MyPlayer.PlayerId controllerId = playerId;
					RespawnKey respawnKey = default(RespawnKey);
					respawnKey.ControllerId = controllerId;
					respawnKey.RespawnShipId = entry.ShipId;
					RespawnKey key = respawnKey;
					m_globalRespawnTimesMs.Add(key, totalTimeInMilliseconds + entry.RelativeRespawnTime, immediate: true);
				}
			}
			m_synced = true;
		}

		public void SyncCooldownToPlayer(ulong steamId, bool isLocal)
		{
			int totalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
			m_tmpRespawnTimes.Clear();
			foreach (KeyValuePair<RespawnKey, int> globalRespawnTimesM in m_globalRespawnTimesMs)
			{
				if (globalRespawnTimesM.Key.ControllerId.SteamId == steamId)
				{
					RespawnCooldownEntry item = default(RespawnCooldownEntry);
					item.ControllerId = globalRespawnTimesM.Key.ControllerId.SerialId;
					item.ShipId = globalRespawnTimesM.Key.RespawnShipId;
					item.RelativeRespawnTime = globalRespawnTimesM.Value - totalTimeInMilliseconds;
					m_tmpRespawnTimes.Add(item);
				}
			}
			if (isLocal)
			{
				OnSyncCooldownResponse(m_tmpRespawnTimes);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSyncCooldownResponse, m_tmpRespawnTimes, new EndpointId(steamId));
			}
			m_tmpRespawnTimes.Clear();
		}

		public override void UpdatingStopped()
		{
			base.UpdatingStopped();
			m_updatingStopped = true;
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			int totalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
			int delta = totalTimeInMilliseconds - m_lastUpdate;
			if (m_updatingStopped)
			{
				UpdateRespawnTimes(delta);
				m_lastUpdate = totalTimeInMilliseconds;
				m_updatingStopped = false;
				return;
			}
			m_updateCtr++;
			m_lastUpdate = totalTimeInMilliseconds;
			if (m_updateCtr % 100 == 0)
			{
				RemoveOldRespawnTimes();
			}
		}

		private void UpdateRespawnTimes(int delta)
		{
			foreach (RespawnKey key in m_globalRespawnTimesMs.Keys)
			{
				m_globalRespawnTimesMs[key] += delta;
			}
			m_globalRespawnTimesMs.ApplyAdditionsAndModifications();
		}

		private void RemoveOldRespawnTimes()
		{
			MyDefinitionManager.Static.GetRespawnShipDefinitions();
			int totalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
			foreach (RespawnKey key in m_globalRespawnTimesMs.Keys)
			{
				int num = m_globalRespawnTimesMs[key];
				if (totalTimeInMilliseconds - num >= 0)
				{
					m_globalRespawnTimesMs.Remove(key);
				}
			}
			m_globalRespawnTimesMs.ApplyRemovals();
		}

		public void ResetRespawnCooldown(MyPlayer.PlayerId controllerId)
		{
			DictionaryReader<string, MyRespawnShipDefinition> respawnShipDefinitions = MyDefinitionManager.Static.GetRespawnShipDefinitions();
			int totalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
			float spawnShipTimeMultiplier = MySession.Static.Settings.SpawnShipTimeMultiplier;
			foreach (KeyValuePair<string, MyRespawnShipDefinition> item in respawnShipDefinitions)
			{
				RespawnKey respawnKey = default(RespawnKey);
				respawnKey.ControllerId = controllerId;
				respawnKey.RespawnShipId = item.Key;
				RespawnKey key = respawnKey;
				if (spawnShipTimeMultiplier != 0f)
				{
					m_globalRespawnTimesMs.Add(key, totalTimeInMilliseconds + (int)((float)(item.Value.Cooldown * 1000) * spawnShipTimeMultiplier), immediate: true);
				}
				else
				{
					m_globalRespawnTimesMs.Remove(key);
				}
			}
		}

		public int GetRespawnCooldownSeconds(MyPlayer.PlayerId controllerId, string respawnShipId)
		{
			if (MyDefinitionManager.Static.GetRespawnShipDefinition(respawnShipId) == null)
			{
				return 0;
			}
			RespawnKey respawnKey = default(RespawnKey);
			respawnKey.ControllerId = controllerId;
			respawnKey.RespawnShipId = respawnShipId;
			RespawnKey key = respawnKey;
			int totalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
			int value = totalTimeInMilliseconds;
			m_globalRespawnTimesMs.TryGetValue(key, out value);
			return Math.Max((value - totalTimeInMilliseconds) / 1000, 0);
		}

		private void OnLocalRespawnRequest(string model, Color color)
		{
			if (MyFakes.SHOW_FACTIONS_GUI)
			{
				ulong arg = (MySession.Static.LocalHumanPlayer != null) ? MySession.Static.LocalHumanPlayer.Id.SteamId : Sync.MyId;
				int arg2 = (MySession.Static.LocalHumanPlayer != null) ? MySession.Static.LocalHumanPlayer.Id.SerialId : 0;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RespawnRequest_Implementation, arg, arg2);
			}
			else
			{
				MyPlayerCollection.RespawnRequest(MySession.Static.LocalHumanPlayer == null, newIdentity: false, 0L, null, 0, model, color);
			}
		}

		[Event(null, 348)]
		[Reliable]
		[Server]
		private static void RespawnRequest_Implementation(ulong steamPlayerId, int serialId)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && MyEventContext.Current.Sender.Value != steamPlayerId)
			{
				((MyMultiplayerServerBase)MyMultiplayer.Static).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyPlayer.PlayerId id = new MyPlayer.PlayerId(steamPlayerId, serialId);
			MyPlayer playerById = Sync.Players.GetPlayerById(id);
			if ((MyScenarioSystem.Static != null && (MyScenarioSystem.Static.GameState == MyScenarioSystem.MyState.JoinScreen || MyScenarioSystem.Static.GameState == MyScenarioSystem.MyState.WaitingForClients)) || playerById == null)
			{
				return;
			}
			if (!TryFindExistingCharacter(playerById))
			{
				bool flag = true;
				long arg = 0L;
				if (MySession.Static.Settings.EnableAutorespawn && playerById.Identity.LastDeathPosition.HasValue)
				{
					ClearToken<MyRespawnPointInfo> availableRespawnPoints = GetAvailableRespawnPoints(playerById.Identity.IdentityId, includePublicSpawns: false);
					try
					{
						if (availableRespawnPoints.List.Count > 0)
						{
							Vector3D lastPlayerPosition = playerById.Identity.LastDeathPosition.Value;
							MyRespawnPointInfo myRespawnPointInfo = availableRespawnPoints.List.MinBy((MyRespawnPointInfo x) => (float)Vector3D.Distance(x.MedicalRoomPosition, lastPlayerPosition));
							if (MySession.Static.ElapsedGameTime - playerById.Identity.LastRespawnTime < TimeSpan.FromSeconds(10.0))
							{
								arg = myRespawnPointInfo.MedicalRoomId;
							}
							else
							{
								MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RequestRespawnAtSpawnPoint, myRespawnPointInfo.MedicalRoomId, new EndpointId(steamPlayerId));
								flag = false;
							}
						}
					}
					finally
					{
						((IDisposable)availableRespawnPoints).Dispose();
					}
				}
				if (flag)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowMedicalScreen_Implementation, MOTDData.Construct(), arg, new EndpointId(steamPlayerId));
				}
			}
			else if (!Sandbox.Engine.Platform.Game.IsDedicated && Sync.MyId == steamPlayerId)
			{
				ShowMotD(MOTDData.Construct());
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowMotD, MOTDData.Construct(), new EndpointId(steamPlayerId));
			}
			MyRespawnComponentBase.NotifyRespawnRequested(playerById);
		}

		private static bool TryFindExistingCharacter(MyPlayer player)
		{
			if (player == null)
			{
				return false;
			}
			foreach (long savedCharacter in player.Identity.SavedCharacters)
			{
				MyCharacter myCharacter = MyEntities.GetEntityById(savedCharacter) as MyCharacter;
				if (myCharacter != null && !myCharacter.IsDead)
				{
					if (myCharacter.Parent == null)
					{
						MySession.Static.Players.SetControlledEntity(player.Id, myCharacter);
						player.Identity.ChangeCharacter(myCharacter);
						MySession.SendVicinityInformation(myCharacter.EntityId, new EndpointId(player.Client.SteamUserId));
						return true;
					}
					if (myCharacter.Parent is MyCockpit)
					{
						MyCockpit myCockpit = myCharacter.Parent as MyCockpit;
						MySession.Static.Players.SetControlledEntity(player.Id, myCockpit);
						player.Identity.ChangeCharacter(myCockpit.Pilot);
						MySession.SendVicinityInformation(myCharacter.EntityId, new EndpointId(player.Client.SteamUserId));
						return true;
					}
				}
			}
			return false;
		}

		[Event(null, 450)]
		[Reliable]
		[Client]
		private static void RequestRespawnAtSpawnPoint(long spawnPointId)
		{
			string model = null;
			Color color = Color.Red;
			MyLocalCache.GetCharacterInfoFromInventoryConfig(ref model, ref color);
			MyPlayerCollection.RespawnRequest(MySession.Static.LocalCharacter == null, newIdentity: false, spawnPointId, null, 0, model, color);
		}

		[Event(null, 459)]
		[Reliable]
		[Client]
		private static void ShowMedicalScreen_Implementation(MOTDData motd, long restrictedRespawn)
		{
			MyGuiScreenMedicals myGuiScreenMedicals = new MyGuiScreenMedicals((MySession.Static.Factions.JoinableFactionsPresent || MySession.Static.Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION) && MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId) == null, restrictedRespawn);
			MyGuiSandbox.AddScreen(myGuiScreenMedicals);
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				if (motd.HasMessage)
				{
					myGuiScreenMedicals.SetMotD(motd.GetMessage().ToString());
				}
				else
				{
					myGuiScreenMedicals.HideMotdButton();
				}
				if (motd.HasUrl)
				{
					MyGuiScreenMedicals.ShowMotDUrl(motd.Url);
				}
				MySession.ShowMotD = false;
			}
		}

		[Event(null, 486)]
		[Reliable]
		[Client]
		private static void ShowMotD(MOTDData motd)
		{
			if (motd.HasMessage)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenMotD(motd.GetMessage()));
			}
			if (motd.HasUrl)
			{
				MyGuiScreenMedicals.ShowMotDUrl(motd.Url);
			}
			MySession.ShowMotD = false;
		}

		public static ClearToken<MyRespawnShipDefinition> GetRespawnShips(MyPlanet planet)
		{
			MyUtils.Init(ref m_respawnShipsCache).AssertEmpty();
			IEnumerable<MyRespawnShipDefinition> values = MyDefinitionManager.Static.GetRespawnShipDefinitions().Values;
			if (planet.HasAtmosphere)
			{
				float num = planet.Generator.Atmosphere.Density * 0.95f;
				foreach (MyRespawnShipDefinition item in values)
				{
					if (item.UseForPlanetsWithAtmosphere && num >= item.MinimalAirDensity)
					{
						m_respawnShipsCache.Add(item);
					}
				}
			}
			else
			{
				foreach (MyRespawnShipDefinition item2 in values)
				{
					if (item2.UseForPlanetsWithoutAtmosphere)
					{
						m_respawnShipsCache.Add(item2);
					}
				}
			}
			string subtypeName = planet.Generator.Id.SubtypeName;
			for (int num2 = m_respawnShipsCache.Count - 1; num2 >= 0; num2--)
			{
				bool flag = false;
				MyRespawnShipDefinition myRespawnShipDefinition = m_respawnShipsCache[num2];
				if (myRespawnShipDefinition.PlanetTypes != null && !myRespawnShipDefinition.PlanetTypes.Contains(subtypeName))
				{
					flag = true;
				}
				if (myRespawnShipDefinition.SpawnPosition.HasValue)
				{
					flag = true;
				}
				if (flag)
				{
					m_respawnShipsCache.RemoveAt(num2);
				}
			}
			return m_respawnShipsCache.GetClearToken();
		}

		public static MyRespawnShipDefinition GetRandomRespawnShip(MyPlanet planet)
		{
			ClearToken<MyRespawnShipDefinition> respawnShips = GetRespawnShips(planet);
			try
			{
				List<MyRespawnShipDefinition> list = respawnShips.List;
				if (list.Count == 0)
				{
					return null;
				}
				return list[MyUtils.GetRandomInt(list.Count)];
			}
			finally
			{
				((IDisposable)respawnShips).Dispose();
			}
		}

		public override bool HandleRespawnRequest(bool joinGame, bool newIdentity, long respawnEntityId, string respawnShipId, MyPlayer.PlayerId playerId, Vector3D? spawnPosition, SerializableDefinitionId? botDefinitionId, bool realPlayer, string modelName, Color color)
		{
			MyPlayer myPlayer = Sync.Players.GetPlayerById(playerId);
			bool flag = newIdentity || myPlayer == null;
			if (!MySessionComponentMissionTriggers.CanRespawn(playerId))
			{
				return false;
			}
			Vector3D vector3D = Vector3D.Zero;
			if (myPlayer != null && myPlayer.Character != null)
			{
				vector3D = myPlayer.Character.PositionComp.GetPosition();
			}
			if (TryFindExistingCharacter(myPlayer))
			{
				return true;
			}
			MyBotDefinition botDefinition = null;
			if (botDefinitionId.HasValue)
			{
				MyDefinitionManager.Static.TryGetBotDefinition(botDefinitionId.Value, out botDefinition);
			}
			long? planetId = null;
			MyPlanet myPlanet = MyEntities.GetEntityById(respawnEntityId) as MyPlanet;
			if (myPlanet != null)
			{
				planetId = respawnEntityId;
				if (string.IsNullOrEmpty(respawnShipId))
				{
					MyRespawnShipDefinition randomRespawnShip = GetRandomRespawnShip(myPlanet);
					if (randomRespawnShip != null)
					{
						respawnShipId = randomRespawnShip.Id.SubtypeName;
					}
				}
			}
			if (!flag)
			{
				if (respawnShipId != null)
				{
					SpawnAtShip(myPlayer, respawnShipId, botDefinition, modelName, color, planetId);
					return true;
				}
				if (spawnPosition.HasValue)
				{
					Vector3D value = MyGravityProviderSystem.CalculateTotalGravityInPoint(spawnPosition.Value);
					if (Vector3D.IsZero(value))
					{
						value = Vector3D.Down;
					}
					else
					{
						value.Normalize();
					}
					value.CalculatePerpendicularVector(out Vector3D result);
					myPlayer.SpawnAt(MatrixD.CreateWorld(spawnPosition.Value, result, -value), Vector3.Zero, null, botDefinition, findFreePlace: true, modelName, color);
					return true;
				}
				MyRespawnComponent myRespawnComponent = null;
				if (respawnEntityId == 0L || !MyFakes.SHOW_FACTIONS_GUI)
				{
					ClearToken<MyRespawnPointInfo> availableRespawnPoints = GetAvailableRespawnPoints(MySession.Static.CreativeMode ? null : new long?(myPlayer.Identity.IdentityId), includePublicSpawns: false);
					try
					{
						List<MyRespawnPointInfo> list = availableRespawnPoints.List;
						if (joinGame && list.Count > 0)
						{
							myRespawnComponent = FindRespawnById(list[MyRandom.Instance.Next(0, list.Count)].MedicalRoomId, null);
						}
					}
					finally
					{
						((IDisposable)availableRespawnPoints).Dispose();
					}
				}
				else
				{
					myRespawnComponent = FindRespawnById(respawnEntityId, myPlayer);
					if (myRespawnComponent == null)
					{
						return false;
					}
				}
				if (myRespawnComponent != null)
				{
					SpawnInRespawn(myPlayer, myRespawnComponent, botDefinition, modelName, color);
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				bool resetIdentity = false;
				if (MySession.Static.Settings.PermanentDeath.Value)
				{
					MyIdentity myIdentity = Sync.Players.TryGetPlayerIdentity(playerId);
					if (myIdentity != null)
					{
						resetIdentity = myIdentity.FirstSpawnDone;
					}
				}
				if (myPlayer == null)
				{
					MyIdentity identity = Sync.Players.CreateNewIdentity(playerId.SteamId.ToString());
					myPlayer = Sync.Players.CreateNewPlayer(identity, playerId, playerId.SteamId.ToString(), realPlayer, initialPlayer: false, newIdentity: false);
					resetIdentity = false;
				}
				if (MySession.Static.CreativeMode)
				{
					Vector3D? vector3D2 = MyEntities.FindFreePlace(vector3D, 2f, 200);
					if (vector3D2.HasValue)
					{
						vector3D = vector3D2.Value;
					}
					myPlayer.SpawnAt(Matrix.CreateTranslation(vector3D), Vector3.Zero, null, botDefinition, findFreePlace: true, modelName, color);
				}
				else
				{
					SpawnAsNewPlayer(myPlayer, vector3D, respawnShipId, planetId, resetIdentity, botDefinition, modelName, color);
				}
			}
			return true;
		}

		private void SpawnInRespawn(MyPlayer player, MyRespawnComponent respawn, MyBotDefinition botDefinition, string modelName, Color color)
		{
			if (respawn.Entity == null)
			{
				SpawnInSuit(player, null, botDefinition, modelName, color);
				return;
			}
			MyEntity topMostParent = respawn.Entity.GetTopMostParent();
			if (topMostParent.Physics == null)
			{
				SpawnInSuit(player, topMostParent, botDefinition, modelName, color);
				return;
			}
			MyCockpit myCockpit = respawn.Entity as MyCockpit;
			if (myCockpit != null)
			{
				SpawnInCockpit(player, myCockpit, botDefinition, myCockpit.WorldMatrix, modelName, color, spawnWithDefaultItems: true);
				return;
			}
			MatrixD spawnPosition = respawn.GetSpawnPosition();
			Vector3 velocityAtPoint = topMostParent.Physics.GetVelocityAtPoint(spawnPosition.Translation);
			player.SpawnAt(spawnPosition, velocityAtPoint, topMostParent, botDefinition, findFreePlace: true, modelName, color);
			MyMedicalRoom myMedicalRoom = respawn.Entity as MyMedicalRoom;
			if (myMedicalRoom == null)
			{
				return;
			}
			myMedicalRoom.TryTakeSpawneeOwnership(player);
			myMedicalRoom.TrySetFaction(player);
			if (myMedicalRoom.ForceSuitChangeOnRespawn)
			{
				player.Character.ChangeModelAndColor(myMedicalRoom.RespawnSuitName, player.Character.ColorMask, resetToDefault: false, 0L);
				if (MySession.Static.Settings.EnableOxygen && player.Character.OxygenComponent != null && player.Character.OxygenComponent.NeedsOxygenFromSuit)
				{
					player.Character.OxygenComponent.SwitchHelmet();
				}
			}
		}

		private MyRespawnComponent FindRespawnById(long respawnBlockId, MyPlayer player)
		{
			MyCubeBlock entity = null;
			if (!MyEntities.TryGetEntityById(respawnBlockId, out entity))
			{
				return null;
			}
			if (!entity.IsWorking)
			{
				return null;
			}
			MyRespawnComponent myRespawnComponent = (MyRespawnComponent)entity.Components.Get<MyEntityRespawnComponentBase>();
			if (myRespawnComponent == null)
			{
				return null;
			}
			if (!myRespawnComponent.SpawnWithoutOxygen && myRespawnComponent.GetOxygenLevel() == 0f)
			{
				return null;
			}
			if (player != null && !myRespawnComponent.CanPlayerSpawn(player.Identity.IdentityId, acceptPublicRespawn: true))
			{
				return null;
			}
			return myRespawnComponent;
		}

		public static ClearToken<MyRespawnPointInfo> GetAvailableRespawnPoints(long? identityId, bool includePublicSpawns)
		{
			m_respanwPointsCache.AssertEmpty();
			foreach (MyRespawnComponent allRespawn in MyRespawnComponent.GetAllRespawns())
			{
				MyTerminalBlock entity = allRespawn.Entity;
				if (entity != null && !entity.Closed)
				{
					if (Sync.IsServer)
					{
						entity.CubeGrid?.GridSystems?.UpdatePower();
						entity.UpdateIsWorking();
					}
					if (entity.IsWorking && (!identityId.HasValue || allRespawn.CanPlayerSpawn(identityId.Value, includePublicSpawns)))
					{
						MyRespawnPointInfo myRespawnPointInfo = new MyRespawnPointInfo();
						IMySpawnBlock mySpawnBlock = (IMySpawnBlock)entity;
						myRespawnPointInfo.MedicalRoomId = entity.EntityId;
						myRespawnPointInfo.MedicalRoomGridId = entity.CubeGrid.EntityId;
						myRespawnPointInfo.MedicalRoomName = ((!string.IsNullOrEmpty(mySpawnBlock.SpawnName)) ? mySpawnBlock.SpawnName : ((entity.CustomName != null) ? entity.CustomName.ToString() : ((entity.Name != null) ? entity.Name : entity.ToString())));
						myRespawnPointInfo.OxygenLevel = allRespawn.GetOxygenLevel();
						myRespawnPointInfo.OwnerId = entity.IDModule.Owner;
						MatrixD worldMatrix = entity.WorldMatrix;
						Vector3D translation = worldMatrix.Translation;
						Vector3D vector3D = translation + worldMatrix.Up * 20.0 + entity.WorldMatrix.Right * 20.0 + worldMatrix.Forward * 20.0;
						Vector3D? vector3D2 = MyEntities.FindFreePlace(vector3D, 1f);
						if (!vector3D2.HasValue)
						{
							vector3D2 = vector3D;
						}
						myRespawnPointInfo.PrefferedCameraPosition = vector3D2.Value;
						myRespawnPointInfo.MedicalRoomPosition = translation;
						myRespawnPointInfo.MedicalRoomUp = worldMatrix.Up;
						if (entity.CubeGrid.Physics != null)
						{
							myRespawnPointInfo.MedicalRoomVelocity = entity.CubeGrid.Physics.LinearVelocity;
						}
						m_respanwPointsCache.Add(myRespawnPointInfo);
					}
				}
			}
			ClearToken<MyRespawnPointInfo> result = default(ClearToken<MyRespawnPointInfo>);
			result.List = m_respanwPointsCache;
			return result;
		}

		public void SpawnAsNewPlayer(MyPlayer player, Vector3D currentPosition, string respawnShipId, long? planetId, bool resetIdentity, MyBotDefinition botDefinition, string modelName, Color color)
		{
			if (Sync.IsServer && player != null && player.Identity != null)
			{
				if (resetIdentity)
				{
					ResetPlayerIdentity(player, modelName, color);
				}
				if (respawnShipId != null)
				{
					SpawnAtShip(player, respawnShipId, botDefinition, modelName, color, planetId);
				}
				else
				{
					SpawnInSuit(player, null, botDefinition, modelName, color);
				}
				if (MySession.Static != null && player.Character != null && MySession.Static.Settings.EnableOxygen && player.Character.OxygenComponent != null && player.Character.OxygenComponent.NeedsOxygenFromSuit)
				{
					player.Character.OxygenComponent.SwitchHelmet();
				}
			}
		}

		public void SpawnAtShip(MyPlayer player, string respawnShipId, MyBotDefinition botDefinition, string modelName, Color? color, long? planetId = null)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			ResetRespawnCooldown(player.Id);
			if (Sync.MultiplayerActive)
			{
				SyncCooldownToPlayer(player.Id.SteamId, player.Id.SteamId == Sync.MyId);
			}
			MyRespawnShipDefinition respawnShip = MyDefinitionManager.Static.GetRespawnShipDefinition(respawnShipId);
			if (respawnShip == null)
			{
				return;
			}
			SpawnInfo spawnInfo = new SpawnInfo
			{
				SpawnNearPlayers = true,
				IdentityId = player.Identity.IdentityId,
				PlanetDeployAltitude = respawnShip.PlanetDeployAltitude,
				CollisionRadius = respawnShip.Prefab.BoundingSphere.Radius,
				MinimalAirDensity = (respawnShip.UseForPlanetsWithoutAtmosphere ? 0f : respawnShip.MinimalAirDensity)
			};
			Vector3 up = Vector3.Zero;
			Vector3 forward = Vector3.Zero;
			Vector3D position = Vector3D.Zero;
			if (!planetId.HasValue && respawnShip.PlanetTypes != null)
			{
				planetId = MyPlanets.GetPlanets().FirstOrDefault((MyPlanet x) => respawnShip.PlanetTypes.Contains(x.Generator.Id.SubtypeName))?.EntityId;
			}
			if (planetId.HasValue)
			{
				MyPlanet myPlanet = MyEntities.GetEntityById(planetId.Value) as MyPlanet;
				if (myPlanet != null)
				{
					spawnInfo.Planet = myPlanet;
					GetSpawnPositionNearPlanet(in spawnInfo, out position, out forward, out up);
				}
				else
				{
					planetId = null;
				}
			}
			if (!planetId.HasValue)
			{
				if (respawnShip.SpawnPosition.HasValue)
				{
					position = respawnShip.SpawnPosition.Value;
					MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(position);
					Vector2 spawnPositionDispersion = new Vector2(respawnShip.SpawnPositionDispersionMin, respawnShip.SpawnPositionDispersionMax);
					if (closestPlanet != null && closestPlanet.PositionComp.WorldAABB.Contains(position) == ContainmentType.Contains)
					{
						spawnInfo.Planet = closestPlanet;
						FindSpawnPositionAbovePlanetInPredefinedArea(in spawnInfo, spawnPositionDispersion, ref position, out forward, out up);
					}
					else
					{
						spawnInfo.Planet = null;
						FindSpawnPositionInSpaceInPredefinedArea(in spawnInfo, respawnShip.SpawnNearProceduralAsteroids, spawnPositionDispersion, ref position, out forward, out up);
					}
				}
				else
				{
					spawnInfo.Planet = null;
					GetSpawnPositionInSpace(spawnInfo, out position, out forward, out up);
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MySession.SetSpectatorPositionFromServer, position, new EndpointId(player.Id.SteamId));
			Stack<Action> stack = new Stack<Action>();
			List<MyCubeGrid> respawnGrids = new List<MyCubeGrid>();
			if (!MyFakes.USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS)
			{
				stack.Push(delegate
				{
					if (respawnGrids.Count != 0)
					{
						MyCubeGrid myCubeGrid2 = respawnGrids[0];
						MyGps gps = new MyGps
						{
							ShowOnHud = true,
							Name = new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.GPS_Respawn_Location_Name)).Append(" - ").Append(myCubeGrid2.EntityId)
								.ToString(),
							DisplayName = MyTexts.GetString(MySpaceTexts.GPS_Respawn_Location_Name),
							DiscardAt = null,
							Coords = new Vector3(0f, 0f, 0f),
							Description = MyTexts.GetString(MySpaceTexts.GPS_Respawn_Location_Desc),
							AlwaysVisible = true,
							GPSColor = new Color(117, 201, 241),
							IsContainerGPS = true
						};
						MySession.Static.Gpss.SendAddGps(spawnInfo.IdentityId, ref gps, myCubeGrid2.EntityId, playSoundOnCreation: false);
					}
				});
			}
			if (!Vector3.IsZero(ref respawnShip.InitialAngularVelocity) || !Vector3.IsZero(ref respawnShip.InitialLinearVelocity))
			{
				stack.Push(delegate
				{
					if (respawnGrids.Count != 0)
					{
						MyCubeGrid myCubeGrid = respawnGrids[0];
						MatrixD worldMatrix = myCubeGrid.WorldMatrix;
						MyGridPhysics physics = myCubeGrid.Physics;
						physics.LinearVelocity = Vector3D.TransformNormal(respawnShip.InitialLinearVelocity, worldMatrix);
						physics.AngularVelocity = Vector3D.TransformNormal(respawnShip.InitialAngularVelocity, worldMatrix);
						for (int i = 1; i < respawnGrids.Count; i++)
						{
							MyGridPhysics physics2 = respawnGrids[i].Physics;
							physics2.AngularVelocity = physics.AngularVelocity;
							physics2.LinearVelocity = physics.GetVelocityAtPoint(physics2.CenterOfMassWorld);
						}
					}
				});
			}
			stack.Push(delegate
			{
				PutPlayerInRespawnGrid(player, respawnGrids, botDefinition, modelName, color, respawnShip.SpawnWithDefaultItems);
				if (respawnGrids.Count != 0)
				{
					MySession.SendVicinityInformation(respawnGrids[0].EntityId, new EndpointId(player.Client.SteamUserId));
				}
				this.RespawnDoneEvent.InvokeIfNotNull(player.Client.SteamUserId);
			});
			stack.Push(delegate
			{
				if (respawnGrids.Count > 0)
				{
					MyVisualScriptLogicProvider.RespawnShipSpawned?.Invoke(respawnGrids[0].EntityId, spawnInfo.IdentityId, respawnShip.Prefab.Id.SubtypeName);
				}
			});
			MyPrefabManager.Static.SpawnPrefab(respawnGrids, respawnShip.Prefab.Id.SubtypeName, position, forward, up, default(Vector3), default(Vector3), null, null, SpawningOptions.RotateFirstCockpitTowardsDirection | SpawningOptions.SetAuthorship, spawnInfo.IdentityId, updateSync: true, stack);
		}

		private void SpawnInCockpit(MyPlayer player, MyCockpit cockpit, MyBotDefinition botDefinition, MatrixD matrix, string modelName, Color? color, bool spawnWithDefaultItems)
		{
			MyCharacter myCharacter = MyCharacter.CreateCharacter(matrix, Vector3.Zero, player.Identity.DisplayName, modelName, color.HasValue ? new Vector3?(color.Value.ToVector3()) : null, botDefinition, findNearPos: true, AIMode: false, cockpit, useInventory: true, player.Identity.IdentityId, spawnWithDefaultItems);
			if (cockpit == null)
			{
				Sync.Players.SetPlayerCharacter(player, myCharacter, null);
			}
			else
			{
				cockpit.AttachPilot(myCharacter, storeOriginalPilotWorld: false, calledFromInit: false, merged: true);
				myCharacter.SetPlayer(player);
				Sync.Players.SetControlledEntity(player.Id, cockpit);
				string text = cockpit.Name;
				if (string.IsNullOrWhiteSpace(text))
				{
					text = cockpit.EntityId.ToString();
				}
				string text2 = cockpit.CubeGrid.Name;
				if (string.IsNullOrWhiteSpace(text2))
				{
					text2 = cockpit.CubeGrid.EntityId.ToString();
				}
				if (MyVisualScriptLogicProvider.PlayerEnteredCockpit != null)
				{
					MyVisualScriptLogicProvider.PlayerEnteredCockpit(text, player.Identity.IdentityId, text2);
				}
			}
			Sync.Players.RevivePlayer(player);
			if (!MySession.Static.Settings.EnableEconomy)
			{
				return;
			}
			bool flag = false;
			MyFaction faction = null;
			MyStation myStation = null;
			Vector3D position = player.GetPosition();
			BoundingSphereD area = new BoundingSphereD(position, 150000.0);
			List<MyObjectSeed> list = new List<MyObjectSeed>();
			MyProceduralWorldGenerator.Static.GetAllInSphere<MyStationCellGenerator>(area, list);
			if (list.Count <= 0)
			{
				flag = false;
			}
			else if (list.Count == 1)
			{
				myStation = (list[0].UserData as MyStation);
				if (myStation != null)
				{
					flag = true;
					faction = (MySession.Static.Factions.TryGetFactionById(myStation.FactionId) as MyFaction);
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				double num = double.MaxValue;
				int num2 = -1;
				for (int i = 0; i < list.Count; i++)
				{
					MyStation myStation2 = list[i].UserData as MyStation;
					if (myStation2 == null)
					{
						continue;
					}
					double num3 = (myStation2.Position - position).LengthSquared();
					if (num3 < num)
					{
						MyFaction myFaction = MySession.Static.Factions.TryGetFactionById(myStation2.FactionId) as MyFaction;
						if (myFaction != null && MyIDModule.GetRelationPlayerPlayer(player.Identity.IdentityId, myFaction.FounderId) != MyRelationsBetweenPlayers.Enemies)
						{
							num = num3;
							myStation = myStation2;
							faction = myFaction;
							num2 = i;
						}
					}
				}
				if (num2 >= 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				MyObjectBuilder_Base myObjectBuilder_Base = MyObjectBuilderSerializer.CreateNewObject(MySession.Static.GetComponent<MySessionComponentEconomy>().GetDatapadDefinitionId());
				MyObjectBuilder_Datapad datapad;
				if ((datapad = (myObjectBuilder_Base as MyObjectBuilder_Datapad)) != null)
				{
					MySessionComponentEconomy.PrepareDatapad(ref datapad, faction, myStation);
				}
				if (cockpit.InventoryCount > 0)
				{
					cockpit.GetInventory().AddItems(1, myObjectBuilder_Base);
				}
			}
		}

		private void PutPlayerInRespawnGrid(MyPlayer player, List<MyCubeGrid> respawnGrids, MyBotDefinition botDefinition, string modelName, Color? color, bool spawnWithDefaultItems)
		{
			List<MyCockpit> list = new List<MyCockpit>();
			foreach (MyCubeGrid respawnGrid in respawnGrids)
			{
				foreach (MyCockpit fatBlock in respawnGrid.GetFatBlocks<MyCockpit>())
				{
					if (fatBlock.IsFunctional)
					{
						list.Add(fatBlock);
					}
				}
			}
			if (list.Count > 1)
			{
				list.Sort(delegate(MyCockpit cockpitA, MyCockpit cockpitB)
				{
					int num = cockpitB.EnableShipControl.CompareTo(cockpitA.EnableShipControl);
					if (num != 0)
					{
						return num;
					}
					int num2 = cockpitB.IsMainCockpit.CompareTo(cockpitA.IsMainCockpit);
					return (num2 != 0) ? num2 : 0;
				});
			}
			MyCockpit myCockpit = null;
			if (list.Count > 0)
			{
				myCockpit = list[0];
			}
			MatrixD matrix = MatrixD.Identity;
			if (myCockpit != null)
			{
				matrix = myCockpit.WorldMatrix;
				matrix.Translation = myCockpit.WorldMatrix.Translation - Vector3.Up - Vector3.Forward;
			}
			else if (respawnGrids.Count > 0)
			{
				matrix.Translation = respawnGrids[0].PositionComp.WorldAABB.Center + respawnGrids[0].PositionComp.WorldAABB.HalfExtents;
			}
			MySessionComponentTrash.CloseRespawnShip(player);
			foreach (MyCubeGrid respawnGrid2 in respawnGrids)
			{
				respawnGrid2.ChangeGridOwnership(player.Identity.IdentityId, MyOwnershipShareModeEnum.None);
				respawnGrid2.IsRespawnGrid = true;
				respawnGrid2.m_playedTime = 0;
				player.RespawnShip.Add(respawnGrid2.EntityId);
			}
			SpawnInCockpit(player, myCockpit, botDefinition, matrix, modelName, color, spawnWithDefaultItems);
		}

		public override void AfterRemovePlayer(MyPlayer player)
		{
		}

		private static void SaveRespawnShip(MyPlayer player)
		{
			if (MySession.Static.Settings.RespawnShipDelete && player.RespawnShip != null && MyEntities.TryGetEntityById(player.RespawnShip[0], out MyCubeGrid oldHome))
			{
				ulong sizeInBytes = 0uL;
				string sessionPath = MySession.Static.CurrentPath;
				Console.WriteLine(sessionPath);
				string fileName = "RS_" + player.Client.SteamUserId + ".sbr";
				Parallel.Start(delegate
				{
					MyLocalCache.SaveRespawnShip((MyObjectBuilder_CubeGrid)oldHome.GetObjectBuilder(), sessionPath, fileName, out sizeInBytes);
				});
			}
		}

		private void SpawnInSuit(MyPlayer player, MyEntity spawnedBy, MyBotDefinition botDefinition, string modelName, Color color)
		{
			SpawnInfo info = default(SpawnInfo);
			info.CollisionRadius = 10f;
			info.SpawnNearPlayers = false;
			info.PlanetDeployAltitude = 10f;
			info.IdentityId = player.Identity.IdentityId;
			GetSpawnPositionInSpace(info, out Vector3D position, out Vector3 forward, out Vector3 up);
			MyCharacter newCharacter = MyCharacter.CreateCharacter(Matrix.CreateWorld(position, forward, up), Vector3.Zero, player.Identity.DisplayName, modelName, color.ToVector3(), botDefinition, findNearPos: true, AIMode: false, null, useInventory: true, player.Identity.IdentityId);
			Sync.Players.SetPlayerCharacter(player, newCharacter, spawnedBy);
			Sync.Players.RevivePlayer(player);
		}

		/// <summary>
		/// Returns a position adjusted for planets that should be safe to spawn at given the radius and position.
		/// </summary>
		/// <param name="position">The position the object would like to spawn at.</param>
		/// <param name="forward">(Out) The forward vector the object should spawn with.</param>
		/// <param name="up">(Out) The up vector the object should spawn with.</param>
		private static void GetSpawnPositionNearPlanet(in SpawnInfo info, out Vector3D position, out Vector3 forward, out Vector3 up)
		{
			bool flag = false;
			position = Vector3D.Zero;
			if (info.SpawnNearPlayers)
			{
				ClearToken<Vector3D> friendlyPlayerPositions = GetFriendlyPlayerPositions(info.IdentityId);
				try
				{
					List<Vector3D> list = friendlyPlayerPositions.List;
					BoundingBoxD worldAABB = info.Planet.PositionComp.WorldAABB;
					for (int num = list.Count - 1; num >= 0; num--)
					{
						if (worldAABB.Contains(list[num]) == ContainmentType.Disjoint)
						{
							list.RemoveAt(num);
						}
					}
					for (int i = 0; i < 30; i += 3)
					{
						if (flag)
						{
							break;
						}
						foreach (Vector3D item in list)
						{
							Vector3D? vector3D = FindPositionAbovePlanet(item, in info, testFreeZone: true, i, i + 3, out forward, out up);
							if (vector3D.HasValue)
							{
								position = vector3D.Value;
								flag = true;
								break;
							}
						}
					}
				}
				finally
				{
					((IDisposable)friendlyPlayerPositions).Dispose();
				}
			}
			if (!flag)
			{
				MyPlanet planet = info.Planet;
				Vector3D center = planet.PositionComp.WorldVolume.Center;
				for (int j = 0; j < 50; j++)
				{
					Vector3 value = MyUtils.GetRandomVector3Normalized();
					if (value.Dot(MySector.DirectionToSunNormalized) < 0f && j < 20)
					{
						value = -value;
					}
					position = center + value * planet.AverageRadius;
					Vector3D? vector3D2 = FindPositionAbovePlanet(position, in info, j < 20, 0, 30, out forward, out up);
					if (vector3D2.HasValue)
					{
						position = vector3D2.Value;
						if ((position - center).Dot(MySector.DirectionToSunNormalized) > 0.0)
						{
							return;
						}
					}
				}
			}
			position = (GetShipOrientationForPlanetSpawn(in info, in position, out forward, out up) ?? position);
		}

		private static Vector3D? GetShipOrientationForPlanetSpawn(in SpawnInfo info, in Vector3D landingPosition, out Vector3 forward, out Vector3 up)
		{
			Vector3 vector = MyGravityProviderSystem.CalculateNaturalGravityInPoint(landingPosition);
			if (Vector3.IsZero(vector))
			{
				vector = Vector3.Up;
			}
			Vector3D value = Vector3D.Normalize(vector);
			Vector3D value2 = -value;
			Vector3D? result = MyEntities.FindFreePlace(landingPosition + value2 * info.PlanetDeployAltitude, info.CollisionRadius);
			forward = Vector3.CalculatePerpendicularVector(-value);
			up = -value;
			return result;
		}

		private static Vector3D? FindPositionAbovePlanet(Vector3D friendPosition, in SpawnInfo info, bool testFreeZone, int distanceIteration, int maxDistanceIterations, out Vector3 forward, out Vector3 up)
		{
			MyPlanet planet = info.Planet;
			Vector3D center = planet.PositionComp.WorldAABB.Center;
			Vector3D axis = Vector3D.Normalize(friendPosition - center);
			float optimalSpawnDistance = MySession.Static.Settings.OptimalSpawnDistance;
			float minimalClearance = (optimalSpawnDistance - optimalSpawnDistance * 0.5f) * 0.9f;
			for (int i = 0; i < 20; i++)
			{
				Vector3D randomPerpendicularVector = MyUtils.GetRandomPerpendicularVector(ref axis);
				float num = optimalSpawnDistance * (MyUtils.GetRandomFloat(0.549999952f, 1.65f) + (float)distanceIteration * 0.05f);
				Vector3D globalPos = friendPosition + randomPerpendicularVector * num;
				globalPos = planet.GetClosestSurfacePointGlobal(ref globalPos);
				if (!TestPlanetLandingPosition(in info, planet, globalPos, testFreeZone, minimalClearance, ref distanceIteration))
				{
					if (distanceIteration > maxDistanceIterations)
					{
						break;
					}
					continue;
				}
				Vector3D? shipOrientationForPlanetSpawn = GetShipOrientationForPlanetSpawn(in info, in globalPos, out forward, out up);
				if (shipOrientationForPlanetSpawn.HasValue)
				{
					return shipOrientationForPlanetSpawn.Value;
				}
			}
			forward = default(Vector3);
			up = default(Vector3);
			return null;
		}

		private static bool TestPlanetLandingPosition(in SpawnInfo info, MyPlanet planet, Vector3D landingPosition, bool testFreeZone, float minimalClearance, ref int distanceIteration)
		{
			if (testFreeZone && info.MinimalAirDensity > 0f && planet.GetAirDensity(landingPosition) < info.MinimalAirDensity)
			{
				return false;
			}
			Vector3D center = planet.PositionComp.WorldAABB.Center;
			Vector3D gravityVector = Vector3D.Normalize(landingPosition - center);
			Vector3D deviationNormal = MyUtils.GetRandomPerpendicularVector(ref gravityVector);
			float collisionRadius = info.CollisionRadius;
			if (!IsTerrainEven())
			{
				return false;
			}
			if (testFreeZone && !IsZoneFree(new BoundingSphereD(landingPosition, minimalClearance)))
			{
				distanceIteration++;
				return false;
			}
			return true;
			bool IsTerrainEven()
			{
				Vector3 vector = (Vector3)deviationNormal * collisionRadius;
				Vector3 value = Vector3.Cross(vector, gravityVector);
				MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(landingPosition, new Vector3D(collisionRadius * 2f, Math.Min(10f, collisionRadius * 0.5f), collisionRadius * 2f), Quaternion.CreateFromForwardUp(deviationNormal, gravityVector));
				int num = -1;
				for (int i = 0; i < 4; i++)
				{
					num = -num;
					int num2 = (i <= 1) ? 1 : (-1);
					Vector3D point = planet.GetClosestSurfacePointGlobal(landingPosition + vector * num + value * num2);
					if (!myOrientedBoundingBoxD.Contains(ref point))
					{
						return false;
					}
				}
				return true;
			}
		}

		private static void FindSpawnPositionAbovePlanetInPredefinedArea(in SpawnInfo info, Vector2 spawnPositionDispersion, ref Vector3D position, out Vector3 forward, out Vector3 up)
		{
			float optimalSpawnDistance = MySession.Static.Settings.OptimalSpawnDistance;
			float minimalClearance = (optimalSpawnDistance - optimalSpawnDistance * 0.5f) * 0.9f;
			MyPlanet planet = info.Planet;
			Vector3D landingPosition = Vector3D.Zero;
			for (int i = 0; i < 100; i++)
			{
				landingPosition = position + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(spawnPositionDispersion.X, spawnPositionDispersion.Y);
				landingPosition = planet.GetClosestSurfacePointGlobal(ref landingPosition);
				int distanceIteration = 0;
				if (TestPlanetLandingPosition(in info, planet, landingPosition, i < 80, minimalClearance, ref distanceIteration))
				{
					Vector3D? shipOrientationForPlanetSpawn = GetShipOrientationForPlanetSpawn(in info, in landingPosition, out forward, out up);
					if (shipOrientationForPlanetSpawn.HasValue)
					{
						position = shipOrientationForPlanetSpawn.Value;
						return;
					}
				}
			}
			position = (GetShipOrientationForPlanetSpawn(in info, in landingPosition, out forward, out up) ?? position);
		}

		private static void FindSpawnPositionInSpaceInPredefinedArea(in SpawnInfo info, bool spawnNearAsteroid, Vector2 spawnPositionDispersion, ref Vector3D position, out Vector3 forward, out Vector3 up)
		{
			float optimalSpawnDistance = MySession.Static.Settings.OptimalSpawnDistance;
			float num = (optimalSpawnDistance - optimalSpawnDistance * 0.5f) * 0.9f;
			BoundingSphereD searchArea = new BoundingSphereD(position, spawnPositionDispersion.Y);
			BoundingSphereD value = new BoundingSphereD(position, spawnPositionDispersion.X);
			if (spawnNearAsteroid)
			{
				Vector3D? vector3D = MyProceduralWorldModule.FindFreeLocationCloseToAsteroid(searchArea, value, takeOccupiedPositions: false, sortByDistance: false, info.CollisionRadius, num, out forward, out up);
				if (vector3D.HasValue)
				{
					position = vector3D.Value;
					return;
				}
			}
			Vector3D vector3D2 = Vector3D.Zero;
			for (int i = 0; i < 100; i++)
			{
				vector3D2 = position + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(spawnPositionDispersion.X, spawnPositionDispersion.Y);
				MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(vector3D2);
				if (closestPlanet == null || closestPlanet.PositionComp.WorldAABB.Contains(vector3D2) != ContainmentType.Contains)
				{
					vector3D2 = (MyEntities.FindFreePlace(vector3D2, info.CollisionRadius) ?? vector3D2);
					if (IsZoneFree(new BoundingSphereD(vector3D2, num)))
					{
						break;
					}
				}
			}
			position = vector3D2;
			forward = MyUtils.GetRandomVector3Normalized();
			up = Vector3.CalculatePerpendicularVector(forward);
		}

		/// <summary>
		/// Returns a position that should be safe to spawn at given the radius and position.
		/// </summary>
		/// <param name="position">The position the object would like to spawn at.</param>
		/// <param name="forward">(Out) The forward vector the object should spawn with.</param>
		/// <param name="up">(Out) The up vector the object should spawn with.</param>
		private static void GetSpawnPositionInSpace(SpawnInfo info, out Vector3D position, out Vector3 forward, out Vector3 up)
		{
			float optimalSpawnDistance = MySession.Static.Settings.OptimalSpawnDistance;
			float num = (optimalSpawnDistance - optimalSpawnDistance * 0.5f) * 0.9f;
			float collisionRadius = info.CollisionRadius;
			if (info.SpawnNearPlayers)
			{
				ClearToken<Vector3D> friendlyPlayerPositions = GetFriendlyPlayerPositions(info.IdentityId);
				try
				{
					foreach (Vector3D friendPosition in friendlyPlayerPositions.List)
					{
						if (!MyPlanets.Static.GetPlanetAABBs().Any((BoundingBoxD x) => x.Contains(friendPosition) != ContainmentType.Disjoint))
						{
							Vector3D center = friendPosition + MyUtils.GetRandomVector3Normalized() * (optimalSpawnDistance * MyUtils.GetRandomFloat(0.5f, 1.5f));
							Vector3D? vector3D = MyProceduralWorldModule.FindFreeLocationCloseToAsteroid(suppressedArea: new BoundingSphereD(friendPosition, num), searchArea: new BoundingSphereD(center, 100000.0), takeOccupiedPositions: false, sortByDistance: true, collisionRadius: collisionRadius, minFreeRange: num, forward: out forward, up: out up);
							if (vector3D.HasValue)
							{
								position = vector3D.Value;
								return;
							}
						}
					}
				}
				finally
				{
					((IDisposable)friendlyPlayerPositions).Dispose();
				}
			}
			BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
			BoundingBoxD box = new BoundingBoxD(new Vector3D(-25000.0), new Vector3D(25000.0));
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (entity.Parent == null)
				{
					BoundingBoxD worldAABB = entity.PositionComp.WorldAABB;
					if (entity is MyPlanet)
					{
						if (worldAABB.Contains(Vector3D.Zero) != 0)
						{
							boundingBoxD.Include(worldAABB);
						}
					}
					else
					{
						box.Include(worldAABB);
					}
				}
			}
			box.Include(boundingBoxD.GetInflated(25000.0));
			if (MyEntities.IsWorldLimited())
			{
				Vector3D vector3D2 = new Vector3D(MyEntities.WorldSafeHalfExtent());
				box = new BoundingBoxD(Vector3D.Clamp(box.Min, -vector3D2, Vector3D.Zero), Vector3D.Clamp(box.Max, Vector3D.Zero, vector3D2));
			}
			Vector3D vector3D3 = Vector3D.Zero;
			for (int i = 0; i < 50; i++)
			{
				vector3D3 = MyUtils.GetRandomPosition(ref box);
				if (boundingBoxD.Contains(vector3D3) == ContainmentType.Disjoint)
				{
					break;
				}
			}
			BoundingSphereD searchArea = new BoundingSphereD(vector3D3, 100000.0);
			BoundingSphereD value = new BoundingSphereD(boundingBoxD.Center, Math.Max(0.0, boundingBoxD.HalfExtents.Min()));
			Vector3D? vector3D4 = MyProceduralWorldModule.FindFreeLocationCloseToAsteroid(searchArea, value, takeOccupiedPositions: true, sortByDistance: true, collisionRadius, num, out forward, out up);
			if (vector3D4.HasValue)
			{
				position = vector3D4.Value;
				return;
			}
			if (MyGamePruningStructure.GetClosestPlanet(vector3D3) != null)
			{
				GetSpawnPositionNearPlanet(in info, out position, out forward, out up);
				return;
			}
			forward = MyUtils.GetRandomVector3Normalized();
			up = Vector3.CalculatePerpendicularVector(forward);
			position = (MyEntities.FindFreePlace(vector3D3, collisionRadius) ?? vector3D3);
		}

		private static bool IsZoneFree(BoundingSphereD safeZone)
		{
			ClearToken<MyEntity> clearToken = MyEntities.GetTopMostEntitiesInSphere(ref safeZone).GetClearToken();
			try
			{
				foreach (MyEntity item in clearToken.List)
				{
					if (item is MyCubeGrid)
					{
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)clearToken).Dispose();
			}
			return true;
		}

		private static ClearToken<Vector3D> GetFriendlyPlayerPositions(long identityId)
		{
			m_playerPositionsCache.AssertEmpty();
			if (MyFakes.USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS)
			{
				if (MySession.Static.Gpss.ExistsForPlayer(identityId))
				{
					List<Vector3D> list = MySession.Static.Gpss[identityId].Values.Select((MyGps x) => x.Coords).ToList();
					list.ShuffleList();
					return list.GetClearToken();
				}
				return new List<Vector3D>().GetClearToken();
			}
			int num = 0;
			foreach (MyIdentity allIdentity in MySession.Static.Players.GetAllIdentities())
			{
				MyCharacter character = allIdentity.Character;
				if (character != null && !character.IsDead && !character.MarkedForClose)
				{
					((IMyComponentOwner<MyIDModule>)character).GetComponent(out MyIDModule component);
					MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(component.Owner, identityId, MyOwnershipShareModeEnum.Faction, MyRelationsBetweenPlayerAndBlock.Neutral, MyRelationsBetweenFactions.Neutral);
					Vector3D position = character.PositionComp.GetPosition();
					switch (relationPlayerBlock)
					{
					case MyRelationsBetweenPlayerAndBlock.Neutral:
						m_playerPositionsCache.Add(position);
						break;
					case MyRelationsBetweenPlayerAndBlock.FactionShare:
						m_playerPositionsCache.Insert(num++, position);
						break;
					}
				}
			}
			m_playerPositionsCache.ShuffleList(0, num);
			m_playerPositionsCache.ShuffleList(num);
			return m_playerPositionsCache.GetClearToken();
		}

		public override MyIdentity CreateNewIdentity(string identityName, MyPlayer.PlayerId playerId, string modelName, bool initialPlayer = false)
		{
			return Sync.Players.CreateNewIdentity(identityName, modelName, null, initialPlayer);
		}

		public override void SetupCharacterDefault(MyPlayer player, MyWorldGenerator.Args args)
		{
			string firstRespawnShip = MyDefinitionManager.Static.GetFirstRespawnShip();
			SpawnAtShip(player, firstRespawnShip, null, null, null);
		}

		public override bool IsInRespawnScreen()
		{
			if (MyGuiScreenMedicals.Static != null)
			{
				return MyGuiScreenMedicals.Static.State == MyGuiScreenState.OPENED;
			}
			return false;
		}

		public override void CloseRespawnScreen()
		{
			MyGuiScreenMedicals.Close();
		}

		public override void CloseRespawnScreenNow()
		{
			if (MyGuiScreenMedicals.Static != null)
			{
				MyGuiScreenMedicals.Static.CloseScreenNow();
			}
		}

		public override void SetNoRespawnText(StringBuilder text, int timeSec)
		{
			MyGuiScreenMedicals.SetNoRespawnText(text, timeSec);
		}

		public override void SetupCharacterFromStarts(MyPlayer player, MyWorldGeneratorStartingStateBase[] playerStarts, MyWorldGenerator.Args args)
		{
			playerStarts[MyUtils.GetRandomInt(playerStarts.Length)].SetupCharacter(args);
		}
	}
}
