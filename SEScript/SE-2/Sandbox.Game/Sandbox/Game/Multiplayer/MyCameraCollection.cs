using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	internal class MyCameraCollection
	{
		protected sealed class OnSaveEntityCameraSettings_003C_003ESandbox_Game_World_MyPlayer_003C_003EPlayerId_0023System_Int64_0023System_Boolean_0023System_Double_0023VRageMath_Vector2_0023System_Boolean : ICallSite<IMyEventOwner, MyPlayer.PlayerId, long, bool, double, Vector2, bool>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyPlayer.PlayerId playerId, in long entityId, in bool isFirstPerson, in double distance, in Vector2 headAngle, in bool isLocalCharacter)
			{
				OnSaveEntityCameraSettings(playerId, entityId, isFirstPerson, distance, headAngle, isLocalCharacter);
			}
		}

		private Dictionary<MyPlayer.PlayerId, Dictionary<long, MyEntityCameraSettings>> m_entityCameraSettings = new Dictionary<MyPlayer.PlayerId, Dictionary<long, MyEntityCameraSettings>>();

		private List<long> m_entitiesToRemove = new List<long>();

		private Dictionary<MyPlayer.PlayerId, MyEntityCameraSettings> m_lastCharacterSettings = new Dictionary<MyPlayer.PlayerId, MyEntityCameraSettings>();

		public void RequestSaveEntityCameraSettings(MyPlayer.PlayerId pid, long entityId, bool isFirstPerson, double distance, float headAngleX, float headAngleY, bool isLocalCharacter)
		{
			MyMultiplayer.RaiseStaticEvent<MyPlayer.PlayerId, long, bool, double, Vector2, bool>(arg6: new Vector2(headAngleX, headAngleY), action: (IMyEventOwner x) => OnSaveEntityCameraSettings, arg2: pid, arg3: entityId, arg4: isFirstPerson, arg5: distance, arg7: isLocalCharacter);
		}

		[Event(null, 44)]
		[Reliable]
		[Server]
		private static void OnSaveEntityCameraSettings(MyPlayer.PlayerId playerId, long entityId, bool isFirstPerson, double distance, Vector2 headAngle, bool isLocalCharacter)
		{
			MyPlayer.PlayerId pid = new MyPlayer.PlayerId(playerId.SteamId, playerId.SerialId);
			MySession.Static.Cameras.AddCameraData(pid, entityId, isFirstPerson, distance, headAngle, isLocalCharacter);
		}

		public bool ContainsPlayer(MyPlayer.PlayerId pid)
		{
			return m_entityCameraSettings.ContainsKey(pid);
		}

		private void AddCameraData(MyPlayer.PlayerId pid, long entityId, bool isFirstPerson, double distance, Vector2 headAngle, bool isLocalCharacter)
		{
			MyEntityCameraSettings cameraSettings = null;
			if (TryGetCameraSettings(pid, entityId, isLocalCharacter, out cameraSettings))
			{
				cameraSettings.IsFirstPerson = isFirstPerson;
				if (!isFirstPerson)
				{
					cameraSettings.Distance = distance;
					cameraSettings.HeadAngle = headAngle;
				}
				if (isLocalCharacter)
				{
					m_lastCharacterSettings[pid] = cameraSettings;
				}
			}
			else
			{
				cameraSettings = new MyEntityCameraSettings
				{
					Distance = distance,
					IsFirstPerson = isFirstPerson,
					HeadAngle = headAngle
				};
				AddCameraData(pid, entityId, isLocalCharacter, cameraSettings);
			}
		}

		private void AddCameraData(MyPlayer.PlayerId pid, long entityId, bool isLocalCharacter, MyEntityCameraSettings data)
		{
			if (!ContainsPlayer(pid))
			{
				m_entityCameraSettings[pid] = new Dictionary<long, MyEntityCameraSettings>();
			}
			if (m_entityCameraSettings[pid].ContainsKey(entityId))
			{
				m_entityCameraSettings[pid][entityId] = data;
			}
			else
			{
				m_entityCameraSettings[pid].Add(entityId, data);
			}
			if (isLocalCharacter)
			{
				m_lastCharacterSettings[pid] = data;
			}
		}

		public bool TryGetCameraSettings(MyPlayer.PlayerId pid, long entityId, bool isLocalCharacter, out MyEntityCameraSettings cameraSettings)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MySession.Static.Players.GetPlayerById(pid);
			}
			else
			{
				_ = MySession.Static.LocalHumanPlayer;
			}
			if (ContainsPlayer(pid))
			{
				if (m_entityCameraSettings[pid].ContainsKey(entityId))
				{
					return m_entityCameraSettings[pid].TryGetValue(entityId, out cameraSettings);
				}
				if (isLocalCharacter && m_lastCharacterSettings.ContainsKey(pid))
				{
					cameraSettings = m_lastCharacterSettings[pid];
					m_entityCameraSettings[pid][entityId] = cameraSettings;
					return true;
				}
			}
			cameraSettings = null;
			return false;
		}

		public void LoadCameraCollection(MyObjectBuilder_Checkpoint checkpoint)
		{
			m_entityCameraSettings = new Dictionary<MyPlayer.PlayerId, Dictionary<long, MyEntityCameraSettings>>();
			SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> allPlayersData = checkpoint.AllPlayersData;
			if (allPlayersData != null)
			{
				foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> item in allPlayersData.Dictionary)
				{
					MyPlayer.PlayerId key = new MyPlayer.PlayerId(item.Key.ClientId, item.Key.SerialId);
					m_entityCameraSettings[key] = new Dictionary<long, MyEntityCameraSettings>();
					foreach (CameraControllerSettings entityCameraDatum in item.Value.EntityCameraData)
					{
						MyEntityCameraSettings value = new MyEntityCameraSettings
						{
							Distance = entityCameraDatum.Distance,
							HeadAngle = entityCameraDatum.HeadAngle,
							IsFirstPerson = entityCameraDatum.IsFirstPerson
						};
						m_entityCameraSettings[key][entityCameraDatum.EntityId] = value;
					}
				}
			}
		}

		public void SaveCameraCollection(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (checkpoint.AllPlayersData != null)
			{
				foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> item2 in checkpoint.AllPlayersData.Dictionary)
				{
					MyPlayer.PlayerId key = new MyPlayer.PlayerId(item2.Key.ClientId, item2.Key.SerialId);
					item2.Value.EntityCameraData = new List<CameraControllerSettings>();
					if (m_entityCameraSettings.ContainsKey(key))
					{
						m_entitiesToRemove.Clear();
						foreach (KeyValuePair<long, MyEntityCameraSettings> item3 in m_entityCameraSettings[key])
						{
							if (MyEntities.EntityExists(item3.Key))
							{
								CameraControllerSettings item = new CameraControllerSettings
								{
									Distance = item3.Value.Distance,
									IsFirstPerson = item3.Value.IsFirstPerson,
									HeadAngle = item3.Value.HeadAngle,
									EntityId = item3.Key
								};
								item2.Value.EntityCameraData.Add(item);
							}
							else
							{
								m_entitiesToRemove.Add(item3.Key);
							}
						}
						foreach (long item4 in m_entitiesToRemove)
						{
							m_entityCameraSettings[key].Remove(item4);
						}
					}
				}
			}
		}

		public void SaveEntityCameraSettings(MyPlayer.PlayerId pid, long entityId, bool isFirstPerson, double distance, bool isLocalCharacter, float headAngleX, float headAngleY, bool sync = true)
		{
			if (!Sync.IsServer && sync)
			{
				RequestSaveEntityCameraSettings(pid, entityId, isFirstPerson, distance, headAngleX, headAngleY, isLocalCharacter);
			}
			Vector2 headAngle = new Vector2(headAngleX, headAngleY);
			AddCameraData(pid, entityId, isFirstPerson, distance, headAngle, isLocalCharacter);
		}
	}
}
