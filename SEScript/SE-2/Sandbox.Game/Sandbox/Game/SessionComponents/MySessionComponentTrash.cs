using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000)]
	public class MySessionComponentTrash : MySessionComponentBase
	{
		protected sealed class SetPlayerAFKTimeout_Server_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int min, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetPlayerAFKTimeout_Server(min);
			}
		}

		protected sealed class SetPlayerAFKTimeout_Broadcast_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int min, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetPlayerAFKTimeout_Broadcast(min);
			}
		}

		protected sealed class AFKKickRequest_Server_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AFKKickRequest_Server();
			}
		}

		private static MyDistributedUpdater<CachingList<MyCubeGrid>, MyCubeGrid> m_gridsToCheck = new MyDistributedUpdater<CachingList<MyCubeGrid>, MyCubeGrid>(100);

		private static float m_playerDistanceHysteresis = 0f;

		private static HashSet<long> m_entitiesInCenter = new HashSet<long>();

		private static bool m_worldHasPlanets;

		private int m_identityCheckIndex;

		private List<MyIdentity> m_allIdentities = new List<MyIdentity>();

		private int m_trashedGridsCount;

		private bool m_voxelTrash_StartFromBegining = true;

		private List<long> m_voxel_BaseIds = new List<long>();

		private int m_voxel_BaseCurrentIndex;

		private MyVoxelBase m_voxel_CurrentBase;

		private MyStorageBase m_voxel_CurrentStorage;

		private IEnumerator<KeyValuePair<Vector3I, MyTimeSpan>> m_voxel_CurrentAccessEnumerator;

		private KeyValuePair<Vector3I, MyTimeSpan>? m_voxel_CurrentChunk;

		private int m_voxel_Timer;

		private static int CONST_VOXEL_WAIT_CYCLE = 600;

		private static int CONST_VOXEL_WAIT_CHUNK = 10;

		private TimeSpan m_afkTimer;

		private bool m_afkTimerInitialized;

		private bool m_kicked;

		private double m_lastSecondsLeft = double.MaxValue;

		private MyHudNotification m_kickNotification;

		private TimeSpan m_stopGridsTimer;

		private static MySessionComponentTrash m_static;

		private List<MyEntity> m_entityQueryCache;

		private static Dictionary<MyTrashRemovalFlags, MyStringId> m_names = new Dictionary<MyTrashRemovalFlags, MyStringId>
		{
			{
				MyTrashRemovalFlags.Fixed,
				MySpaceTexts.ScreenDebugAdminMenu_Stations
			},
			{
				MyTrashRemovalFlags.Stationary,
				MySpaceTexts.ScreenDebugAdminMenu_Stationary
			},
			{
				MyTrashRemovalFlags.Linear,
				MyCommonTexts.ScreenDebugAdminMenu_Linear
			},
			{
				MyTrashRemovalFlags.Accelerating,
				MyCommonTexts.ScreenDebugAdminMenu_Accelerating
			},
			{
				MyTrashRemovalFlags.Powered,
				MySpaceTexts.ScreenDebugAdminMenu_Powered
			},
			{
				MyTrashRemovalFlags.Controlled,
				MySpaceTexts.ScreenDebugAdminMenu_Controlled
			},
			{
				MyTrashRemovalFlags.WithProduction,
				MySpaceTexts.ScreenDebugAdminMenu_WithProduction
			},
			{
				MyTrashRemovalFlags.WithMedBay,
				MySpaceTexts.ScreenDebugAdminMenu_WithMedBay
			},
			{
				MyTrashRemovalFlags.WithBlockCount,
				MyCommonTexts.ScreenDebugAdminMenu_WithBlockCount
			},
			{
				MyTrashRemovalFlags.DistanceFromPlayer,
				MyCommonTexts.ScreenDebugAdminMenu_DistanceFromPlayer
			},
			{
				MyTrashRemovalFlags.RevertMaterials,
				MyCommonTexts.ScreenDebugAdminMenu_RevertMaterials
			},
			{
				MyTrashRemovalFlags.RevertAsteroids,
				MyCommonTexts.ScreenDebugAdminMenu_RevertAsteroids
			},
			{
				MyTrashRemovalFlags.RevertWithFloatingsPresent,
				MyCommonTexts.ScreenDebugAdminMenu_RevertWithFloatingsPresent
			}
		};

		public static float PlayerDistanceHysteresis => m_playerDistanceHysteresis;

		public override void LoadData()
		{
			m_static = this;
			if (!Sync.IsServer)
			{
				m_kickNotification = new MyHudNotification(MySpaceTexts.Trash_KickAFKWarning, 10000);
				return;
			}
			MyEntities.OnEntityAdd += MyEntities_OnEntityAdd;
			MyEntities.OnEntityRemove += MyEntities_OnEntityRemove;
			MySession.Static.Players.IdentitiesChanged += Players_IdentitiesChanged;
			m_trashedGridsCount = 0;
		}

		protected override void UnloadData()
		{
			m_static = null;
			if (Sync.IsServer)
			{
				MyEntities.OnEntityAdd -= MyEntities_OnEntityAdd;
				MyEntities.OnEntityRemove -= MyEntities_OnEntityRemove;
				MySession.Static.Players.IdentitiesChanged -= Players_IdentitiesChanged;
				m_trashedGridsCount = 0;
				m_entitiesInCenter.Clear();
				m_worldHasPlanets = false;
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			m_stopGridsTimer = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes(MySession.Static.Settings.StopGridsPeriodMin);
			m_worldHasPlanets = (MyPlanets.GetPlanets().Count > 0);
		}

		private void Players_IdentitiesChanged()
		{
			m_allIdentities = MySession.Static.Players.GetAllIdentities().ToList();
		}

		private void MyEntities_OnEntityAdd(MyEntity entity)
		{
			if (entity is MyCubeGrid)
			{
				m_gridsToCheck.List.Add(entity as MyCubeGrid);
			}
		}

		private void MyEntities_OnEntityRemove(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				m_gridsToCheck.List.Remove(myCubeGrid);
				if (myCubeGrid.MarkedAsTrash)
				{
					m_trashedGridsCount--;
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			if (!Sync.IsServer)
			{
				if (MySession.Static.Ready && MySession.Static.Settings.AFKTimeountMin > 0 && !m_kicked)
				{
					if (!m_afkTimerInitialized)
					{
						m_afkTimerInitialized = true;
						m_afkTimer = MySession.Static.ElapsedPlayTime + TimeSpan.FromMinutes(MySession.Static.Settings.AFKTimeountMin);
					}
					TimeSpan timeSpan = m_afkTimer - MySession.Static.ElapsedPlayTime;
					if (timeSpan.TotalSeconds <= 60.0 && m_lastSecondsLeft > 60.0)
					{
						MyHud.Notifications.Add(m_kickNotification);
					}
					if (timeSpan.Ticks <= 0)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AFKKickRequest_Server);
						m_kicked = true;
					}
					m_lastSecondsLeft = timeSpan.TotalSeconds;
				}
				return;
			}
			m_gridsToCheck.List.ApplyChanges();
			if (MySession.Static.Settings.TrashRemovalEnabled && (Sync.IsDedicated || !MySession.Static.IsCameraUserAnySpectator()))
			{
				bool trashFound = false;
				m_gridsToCheck.Update();
				m_gridsToCheck.Iterate(delegate(MyCubeGrid x)
				{
					if (x != null)
					{
						trashFound |= UpdateTrash(x);
					}
				});
				if (MySession.Static.Settings.OptimalGridCount > 0 && !trashFound)
				{
					if (m_gridsToCheck.List.Count - m_trashedGridsCount > MySession.Static.Settings.OptimalGridCount)
					{
						m_playerDistanceHysteresis -= 1f;
					}
					else if (m_gridsToCheck.List.Count - m_trashedGridsCount < MySession.Static.Settings.OptimalGridCount && m_playerDistanceHysteresis < 0f)
					{
						m_playerDistanceHysteresis += 1f;
					}
					m_playerDistanceHysteresis = MathHelper.Clamp(m_playerDistanceHysteresis, 0f - MySession.Static.Settings.PlayerDistanceThreshold, 0f);
				}
				CheckIdentitiesTrash();
			}
			VoxelRevertor_Update();
		}

		private void CheckIdentitiesTrash()
		{
			int index = -1;
			if (m_identityCheckIndex < m_allIdentities.Count)
			{
				index = m_identityCheckIndex++;
			}
			else
			{
				m_identityCheckIndex = 0;
			}
			CheckIdentity(index);
		}

		private void CheckIdentity(int index)
		{
			if (index >= m_allIdentities.Count || index < 0)
			{
				return;
			}
			MyIdentity myIdentity = m_allIdentities[index];
			if (MySession.Static.Players.TryGetPlayerId(myIdentity.IdentityId, out MyPlayer.PlayerId result))
			{
				if (!MySession.Static.Players.TryGetPlayerById(result, out MyPlayer _))
				{
					int num = (int)(MySession.GetIdentityLogoutTimeSeconds(myIdentity.IdentityId) / 60f);
					bool flag = false;
					if (MySession.Static.Settings.RemoveOldIdentitiesH > 0 && num >= MySession.Static.Settings.RemoveOldIdentitiesH * 60)
					{
						flag = TryRemoveAbandonedIdentity(myIdentity, result);
					}
					int playerCharacterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
					if (flag || (playerCharacterRemovalThreshold != 0 && num >= playerCharacterRemovalThreshold))
					{
						TryRemoveAbandonedCharacter(myIdentity);
					}
				}
			}
			else
			{
				CloseAbandonedRespawnShip(myIdentity);
			}
		}

		private void TryRemoveAbandonedCharacter(MyIdentity identity)
		{
			if (identity.Character != null)
			{
				if (RemoveCharacter(identity.Character))
				{
					CloseAbandonedRespawnShip(identity);
				}
			}
			else
			{
				CloseAbandonedRespawnShip(identity);
			}
			foreach (long savedCharacter in identity.SavedCharacters)
			{
				if (MyEntities.TryGetEntityById(savedCharacter, out MyCharacter entity, allowClosed: true) && (!entity.Closed || entity.MarkedForClose))
				{
					RemoveCharacter(entity);
				}
			}
		}

		private bool TryRemoveAbandonedIdentity(MyIdentity identity, MyPlayer.PlayerId playerId)
		{
			if (identity.BlockLimits.BlocksBuilt == 0)
			{
				TryRemoveAbandonedFaction(identity);
				MySession.Static.Players.RemoveIdentity(identity.IdentityId, playerId);
				m_allIdentities.Remove(identity);
				m_identityCheckIndex--;
				return true;
			}
			return false;
		}

		private void TryRemoveAbandonedFaction(MyIdentity identity)
		{
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(identity.IdentityId);
			if (playerFaction != null)
			{
				MyFactionCollection.KickMember(playerFaction.FactionId, identity.IdentityId);
			}
		}

		public static void CloseAbandonedRespawnShip(MyIdentity identity)
		{
			if (MySession.Static.Settings.RespawnShipDelete)
			{
				CloseRespawnShip(identity);
			}
		}

		private static void CloseRespawnShip(MyIdentity identity)
		{
			if (identity.RespawnShips != null)
			{
				foreach (long respawnShip in identity.RespawnShips)
				{
					if (MyEntities.TryGetEntityById(respawnShip, out MyCubeGrid entity))
					{
						foreach (MySlimBlock block in entity.GetBlocks())
						{
							MyCockpit myCockpit = block.FatBlock as MyCockpit;
							if (myCockpit != null && myCockpit.Pilot != null)
							{
								myCockpit.Use();
							}
						}
						MyLog.Default.Info($"CloseRespawnShip removed entity '{entity.Name}:{entity.DisplayName}' with entity id '{entity.EntityId}' Block Count: '{entity.BlockCounter}'");
						MyEntities.SendCloseRequest(entity);
					}
				}
				identity.RespawnShips.Clear();
			}
		}

		public static void CloseRespawnShip(MyPlayer player)
		{
			if (player.RespawnShip != null)
			{
				CloseRespawnShip(player.Identity);
			}
		}

		private bool RemoveCharacter(MyCharacter character)
		{
			if (character.IsUsing is MyCryoChamber)
			{
				return false;
			}
			if (character.IsUsing is MyCockpit)
			{
				(character.IsUsing as MyCockpit).RemovePilot();
			}
			long playerIdentityId = character.GetPlayerIdentityId();
			MyLog.Default.Info($"Trash collector removed character '{playerIdentityId}' with entity id '{character.EntityId}'");
			character.Close();
			return true;
		}

		private bool UpdateTrash(MyCubeGrid grid)
		{
			if (grid == null)
			{
				return false;
			}
			if (MySession.Static == null)
			{
				return false;
			}
			if (grid.MarkedForClose)
			{
				return false;
			}
			if (grid.MarkedAsTrash)
			{
				return false;
			}
			if (grid.IsPreview)
			{
				return false;
			}
			if (grid.Projector != null)
			{
				return false;
			}
			if (MySession.Static != null && MySession.Static.Factions != null && MySession.Static.Factions.GetStationByGridId(grid.EntityId) != null)
			{
				return false;
			}
			if (!MyEntities.IsInsideWorld(grid.PositionComp.GetPosition()))
			{
				RemoveGrid(grid);
				return true;
			}
			if (MyEncounterGenerator.Static != null && MyEncounterGenerator.Static.IsEncounter(grid))
			{
				return false;
			}
			MyNeutralShipSpawner myNeutralShipSpawner = (MySession.Static != null) ? MySession.Static.GetComponent<MyNeutralShipSpawner>() : null;
			if (myNeutralShipSpawner != null && myNeutralShipSpawner.IsEncounter(grid.EntityId))
			{
				return false;
			}
			if (m_worldHasPlanets && grid.PositionComp.GetPosition().LengthSquared() < 100.0 && !m_entitiesInCenter.Contains(grid.EntityId))
			{
				m_entitiesInCenter.Add(grid.EntityId);
				MyLog.Default.Info($"Trash cleaner reports that '{grid.Name}:{grid.DisplayName}' with entity id '{grid.EntityId}' Landed in the center of the UNIVERSE!'");
			}
			MyTrashRemovalFlags trashState = GetTrashState(grid);
			if (trashState == MyTrashRemovalFlags.None)
			{
				if (PlayerDistanceHysteresis == 0f)
				{
					RemoveGrid(grid);
				}
				else
				{
					grid.MarkAsTrash();
					m_trashedGridsCount++;
				}
				return true;
			}
			double num = m_stopGridsTimer.TotalMilliseconds - (double)MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (MySession.Static != null && MySession.Static.Settings.StopGridsPeriodMin > 0)
			{
				if (num > (double)(MySession.Static.Settings.StopGridsPeriodMin * 60 * 1000))
				{
					m_stopGridsTimer = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes(MySession.Static.Settings.StopGridsPeriodMin);
				}
				if (!grid.IsStatic && grid.Physics != null && grid.Physics.IsMoving && !trashState.HasFlag(MyTrashRemovalFlags.DistanceFromPlayer) && num <= 0.0)
				{
					m_stopGridsTimer = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes(MySession.Static.Settings.StopGridsPeriodMin);
					MyEntityList.ProceedEntityAction(grid, MyEntityList.EntityListAction.Stop);
				}
			}
			else if (num <= 0.0)
			{
				m_stopGridsTimer = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			}
			return false;
		}

		public static string GetName(MyTrashRemovalFlags flag)
		{
			if (m_names.TryGetValue(flag, out MyStringId value))
			{
				return MyTexts.GetString(value);
			}
			return MyEnum<MyTrashRemovalFlags>.GetName(flag);
		}

		private void VoxelRevertor_Update()
		{
			if (!MySession.Static.Settings.VoxelTrashRemovalEnabled)
			{
				return;
			}
			if (m_voxel_Timer >= 0)
			{
				m_voxel_Timer--;
				return;
			}
			if (m_voxelTrash_StartFromBegining)
			{
				m_voxelTrash_StartFromBegining = false;
				m_voxel_BaseIds.Clear();
				MySession.Static.VoxelMaps.GetAllIds(ref m_voxel_BaseIds);
				m_voxel_BaseCurrentIndex = -1;
				m_voxel_CurrentBase = null;
				m_voxel_CurrentStorage = null;
				m_voxel_CurrentAccessEnumerator = null;
			}
			if (!VoxelRevertor_AdvanceToNext())
			{
				m_voxelTrash_StartFromBegining = true;
			}
			else if (m_voxel_CurrentChunk.HasValue && VoxelRevertor_CanRevertCurrent())
			{
				Vector3I coord = m_voxel_CurrentChunk.Value.Key;
				MyStorageDataTypeFlags myStorageDataTypeFlags = ((MySession.Static.Settings.TrashFlags & MyTrashRemovalFlags.RevertMaterials) == 0) ? MyStorageDataTypeFlags.Content : MyStorageDataTypeFlags.ContentAndMaterial;
				m_voxel_CurrentStorage.AccessDelete(ref coord, myStorageDataTypeFlags);
				MyMultiplayer.RaiseEvent(m_voxel_CurrentBase.RootVoxel, (MyVoxelBase x) => x.RevertVoxelAccess, coord, myStorageDataTypeFlags);
			}
		}

		private bool VoxelRevertor_CanRevertCurrent()
		{
			Vector3I coord = m_voxel_CurrentChunk.Value.Key;
			MyTimeSpan value = m_voxel_CurrentChunk.Value.Value;
			MyObjectBuilder_SessionSettings settings = MySession.Static.Settings;
			int num = settings.VoxelAgeThreshold;
			if (MySession.Static.LowMemoryState)
			{
				num /= 10;
			}
			if (MyTimeSpan.FromTicks(Stopwatch.GetTimestamp() - value.Ticks).Minutes < (double)num)
			{
				return false;
			}
			m_voxel_CurrentStorage.ConvertAccessCoordinates(ref coord, out BoundingBoxD bb);
			bb.Translate(m_voxel_CurrentBase.PositionLeftBottomCorner);
			if (m_voxel_CurrentBase.RootVoxel != m_voxel_CurrentBase)
			{
				bb.Translate(-m_voxel_CurrentBase.PositionLeftBottomCorner);
			}
			using (MyUtils.ReuseCollection(ref m_entityQueryCache))
			{
				float num2 = settings.VoxelGridDistanceThreshold;
				float num3 = settings.VoxelPlayerDistanceThreshold;
				if (MySession.Static.LowMemoryState)
				{
					num2 /= 2f;
					num3 /= 2f;
				}
				BoundingBoxD box = bb.GetInflated(Math.Max(num2, num3));
				MyGamePruningStructure.GetAllEntitiesInBox(ref box, m_entityQueryCache);
				foreach (MyEntity item in m_entityQueryCache)
				{
					if (item is MyCubeGrid)
					{
						if (item.PositionComp.WorldAABB.DistanceSquared(ref bb) < (double)(num2 * num2))
						{
							return false;
						}
					}
					else if (item is MyCharacter)
					{
						if (bb.DistanceSquared(item.PositionComp.WorldMatrix.Translation) < (double)(num3 * num3))
						{
							return false;
						}
					}
					else if ((settings.TrashFlags & MyTrashRemovalFlags.RevertWithFloatingsPresent) == 0 && (item is MyFloatingObject || item is MyInventoryBagEntity))
					{
						Vector3D point = item.PositionComp.WorldMatrix.Translation;
						bb.Contains(ref point, out ContainmentType result);
						if (result != 0)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		private bool VoxelRevertor_AdvanceToNext()
		{
			try
			{
				while (true)
				{
					if (m_voxel_CurrentBase == null || m_voxel_CurrentStorage == null)
					{
						m_voxel_BaseCurrentIndex++;
						if (m_voxel_BaseCurrentIndex >= m_voxel_BaseIds.Count)
						{
							m_voxel_Timer = CONST_VOXEL_WAIT_CYCLE;
							return false;
						}
						MyVoxelBase myVoxelBase = MySession.Static.VoxelMaps.TryGetVoxelBaseById(m_voxel_BaseIds[m_voxel_BaseCurrentIndex]);
						MyStorageBase myStorageBase;
						if ((myStorageBase = (myVoxelBase?.Storage as MyStorageBase)) == null || !VoxelsAreSuitableForReversion(myVoxelBase, myStorageBase))
						{
							continue;
						}
						m_voxel_CurrentBase = myVoxelBase;
						m_voxel_CurrentStorage = myStorageBase;
						m_voxel_CurrentAccessEnumerator = null;
						m_voxel_CurrentChunk = null;
					}
					if (m_voxel_CurrentBase != null && m_voxel_CurrentStorage != null)
					{
						if (m_voxel_CurrentAccessEnumerator == null)
						{
							m_voxel_CurrentAccessEnumerator = m_voxel_CurrentStorage.AccessEnumerator;
						}
						if (m_voxel_CurrentAccessEnumerator.MoveNext())
						{
							break;
						}
						m_voxel_CurrentBase = null;
						m_voxel_CurrentStorage = null;
						m_voxel_CurrentChunk = null;
					}
				}
				m_voxel_CurrentChunk = m_voxel_CurrentAccessEnumerator.Current;
				m_voxel_Timer = CONST_VOXEL_WAIT_CHUNK;
				return true;
			}
			finally
			{
			}
		}

		private bool VoxelsAreSuitableForReversion(MyVoxelBase vox, MyStorageBase storage)
		{
			if (vox.Closed || storage.DataProvider == null)
			{
				return false;
			}
			if (vox.RootVoxel != vox)
			{
				return false;
			}
			if (storage.DataProvider is MyCompositeShapeProvider && (MySession.Static.Settings.TrashFlags & MyTrashRemovalFlags.RevertAsteroids) == 0)
			{
				return false;
			}
			return true;
		}

		public static void RemoveGrid(MyCubeGrid grid)
		{
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(grid);
			if (group != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
				{
					node.NodeData.DismountAllCockpits();
					node.NodeData.Close();
				}
			}
			MyLog.Default.Info($"Trash collector removed grid '{grid.Name}:{grid.DisplayName}' with entity id '{grid.EntityId}'");
			grid.Close();
			if (grid.BigOwners.Count > 0)
			{
				long identityId = grid.BigOwners[0];
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
				if (MySession.Static.Players.TryGetPlayerId(identityId, out MyPlayer.PlayerId result) && MySession.Static.Players.TryGetPlayerById(result, out MyPlayer player) && !MySession.Static.Players.GetOnlinePlayers().Contains(player) && myIdentity != null && myIdentity.BlockLimits.BlocksBuilt == 0)
				{
					MySession.Static.Players.RemoveIdentity(identityId);
				}
			}
		}

		public static MyTrashRemovalFlags GetTrashState(MyCubeGrid grid)
		{
			float metric;
			return GetTrashState(grid, out metric, checkGroup: true);
		}

		private static MyTrashRemovalFlags GetTrashState(MyCubeGrid grid, out float metric, bool checkGroup = false)
		{
			metric = -1f;
			if (grid.GridGeneralDamageModifier == 0f)
			{
				return MyTrashRemovalFlags.Indestructible;
			}
			float num = MySession.GetOwnerLogoutTimeSeconds(grid) / 3600f;
			if (num > 0f && MySession.Static.Settings.PlayerInactivityThreshold > 0f && num > MySession.Static.Settings.PlayerInactivityThreshold)
			{
				return MyTrashRemovalFlags.None;
			}
			MyTrashRemovalFlags myTrashRemovalFlags = MyTrashRemovalFlags.None;
			if (IsCloseToPlayerOrCamera(grid, MySession.Static.Settings.PlayerDistanceThreshold + PlayerDistanceHysteresis))
			{
				metric = MySession.Static.Settings.PlayerDistanceThreshold;
				myTrashRemovalFlags = MyTrashRemovalFlags.DistanceFromPlayer;
			}
			if (PlayerDistanceHysteresis == 0f)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = true;
				if (grid.Physics == null)
				{
					return MyTrashRemovalFlags.Default | myTrashRemovalFlags;
				}
				flag3 = (grid.Physics.AngularVelocity.AbsMax() < 0.05f && grid.Physics.LinearVelocity.AbsMax() < 0.05f);
				flag = (!flag3 && (grid.Physics.AngularAcceleration.AbsMax() > 0.05f || grid.Physics.LinearAcceleration.AbsMax() > 0.05f));
				flag2 = (!flag && !flag3);
				if (!MySession.Static.Settings.TrashFlags.HasFlag(MyTrashRemovalFlags.Stationary) && flag3)
				{
					return MyTrashRemovalFlags.Stationary | myTrashRemovalFlags;
				}
				if (!MySession.Static.Settings.TrashFlags.HasFlag(MyTrashRemovalFlags.Linear) && flag2)
				{
					return MyTrashRemovalFlags.Linear | myTrashRemovalFlags;
				}
				if (!MySession.Static.Settings.TrashFlags.HasFlag(MyTrashRemovalFlags.Accelerating) && flag)
				{
					return MyTrashRemovalFlags.Accelerating | myTrashRemovalFlags;
				}
			}
			HashSet<MySlimBlock> blocks = grid.GetBlocks();
			if (PlayerDistanceHysteresis == 0f)
			{
				if (blocks != null && blocks.Count >= MySession.Static.Settings.BlockCountThreshold)
				{
					metric = MySession.Static.Settings.BlockCountThreshold;
					return MyTrashRemovalFlags.WithBlockCount | myTrashRemovalFlags;
				}
				if (!MySession.Static.Settings.TrashFlags.HasFlag(MyTrashRemovalFlags.Fixed) && grid.IsStatic)
				{
					return MyTrashRemovalFlags.Fixed | myTrashRemovalFlags;
				}
				if (grid.GridSystems != null)
				{
					bool flag4 = grid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != MyResourceStateEnum.NoPower;
					if (!MySession.Static.Settings.TrashFlags.HasFlag(MyTrashRemovalFlags.Powered) && flag4)
					{
						bool flag5 = true;
						long piratesId = MyPirateAntennas.GetPiratesId();
						MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(piratesId);
						if (myIdentity != null && !myIdentity.BlockLimits.HasRemainingPCU && grid.BigOwners.Contains(piratesId) && grid.Save)
						{
							bool flag6 = false;
							foreach (long smallOwner in grid.SmallOwners)
							{
								if (!MySession.Static.Players.IdentityIsNpc(smallOwner))
								{
									flag6 = true;
									break;
								}
							}
							if (!flag6)
							{
								flag5 = false;
							}
						}
						if (flag5)
						{
							return MyTrashRemovalFlags.Powered | myTrashRemovalFlags;
						}
					}
				}
			}
			if (grid.GridSystems != null)
			{
				if (!MySession.Static.Settings.TrashFlags.HasFlag(MyTrashRemovalFlags.Controlled) && grid.GridSystems.ControlSystem != null && grid.GridSystems.ControlSystem.IsControlled)
				{
					return MyTrashRemovalFlags.Controlled | myTrashRemovalFlags;
				}
				if (!MySession.Static.Settings.TrashFlags.HasFlag(MyTrashRemovalFlags.WithProduction) && (grid.BlocksCounters.GetValueOrDefault(typeof(MyObjectBuilder_ProductionBlock)) > 0 || grid.BlocksCounters.GetValueOrDefault(typeof(MyObjectBuilder_Assembler)) > 0 || grid.BlocksCounters.GetValueOrDefault(typeof(MyObjectBuilder_Refinery)) > 0))
				{
					return MyTrashRemovalFlags.WithProduction | myTrashRemovalFlags;
				}
			}
			if (!MySession.Static.Settings.TrashFlags.HasFlag(MyTrashRemovalFlags.WithMedBay) && grid.BlocksCounters.GetValueOrDefault(typeof(MyObjectBuilder_MedicalRoom)) > 0)
			{
				return MyTrashRemovalFlags.WithMedBay;
			}
			if (checkGroup && MyCubeGridGroups.Static.Physical != null)
			{
				MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(grid);
				if (group != null)
				{
					foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
					{
						if (node.NodeData != null && node.NodeData.Physics != null && node.NodeData.Physics.Shape != null && node.NodeData != grid)
						{
							MyTrashRemovalFlags trashState = GetTrashState(node.NodeData, out metric);
							if (trashState != 0)
							{
								return trashState | myTrashRemovalFlags;
							}
						}
					}
					return myTrashRemovalFlags;
				}
			}
			return myTrashRemovalFlags;
		}

		public static bool IsCloseToPlayerOrCamera(MyEntity entity, float distanceThreshold)
		{
			Vector3D translation = entity.WorldMatrix.Translation;
			float num = distanceThreshold * distanceThreshold;
			if (Sync.Players.GetOnlinePlayers().Count > 0)
			{
				int num2 = 0;
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
					if (controlledEntity != null)
					{
						num2++;
						if (Vector3D.DistanceSquared(controlledEntity.Entity.WorldMatrix.Translation, translation) < (double)num)
						{
							return true;
						}
					}
				}
				if (num2 > 0)
				{
					return false;
				}
			}
			return true;
		}

		public void SetPlayerAFKTimeout(int min)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetPlayerAFKTimeout_Server, min);
		}

		[Event(null, 988)]
		[Reliable]
		[Server]
		public static void SetPlayerAFKTimeout_Server(int min)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				MyEventContext.ValidationFailed();
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			if (MySession.Static.Settings.AFKTimeountMin != min)
			{
				MyLog.Default.Info($"Trash AFK Timeount changed by {MyEventContext.Current.Sender.Value} to {min}");
			}
			MySession.Static.Settings.AFKTimeountMin = min;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetPlayerAFKTimeout_Broadcast, min);
		}

		[Event(null, 1008)]
		[Reliable]
		[Broadcast]
		public static void SetPlayerAFKTimeout_Broadcast(int min)
		{
			MySession.Static.Settings.AFKTimeountMin = min;
			if (!Sync.IsServer)
			{
				if (min > 0)
				{
					m_static.m_afkTimer = MySession.Static.ElapsedPlayTime + TimeSpan.FromMinutes(MySession.Static.Settings.AFKTimeountMin);
				}
				else
				{
					m_static.m_afkTimer = TimeSpan.MaxValue;
				}
			}
		}

		[Event(null, 1026)]
		[Reliable]
		[Server]
		private static void AFKKickRequest_Server()
		{
			MyMultiplayer.Static.KickClient(MyEventContext.Current.Sender.Value, kicked: true, add: false);
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (!Sync.IsServer && MySession.Static.Settings.AFKTimeountMin > 0 && (MyInput.Static.IsAnyKeyPress() || MyInput.Static.IsAnyNewMouseOrJoystickPressed() || MyInput.Static.GetMouseX() != 0 || MyInput.Static.GetMouseY() != 0 || MyInput.Static.IsJoystickLastUsed))
			{
				m_afkTimer = MySession.Static.ElapsedPlayTime + TimeSpan.FromMinutes(MySession.Static.Settings.AFKTimeountMin);
				MyHud.Notifications.Remove(m_kickNotification);
			}
		}
	}
}
