using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000, typeof(MyObjectBuilder_SessionComponentSafeZones), null)]
	public class MySessionComponentSafeZones : MySessionComponentBase
	{
		protected sealed class CreateSafeZone_Implementation_003C_003EVRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D position, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateSafeZone_Implementation(position);
			}
		}

		protected sealed class DeleteSafeZone_Implementation_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DeleteSafeZone_Implementation(entityId);
			}
		}

		protected sealed class UpdateSafeZone_Implementation_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_SafeZone : ICallSite<IMyEventOwner, MyObjectBuilder_SafeZone, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_SafeZone ob, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateSafeZone_Implementation(ob);
			}
		}

		protected sealed class UpdateSafeZone_Boradcast_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_SafeZone : ICallSite<IMyEventOwner, MyObjectBuilder_SafeZone, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_SafeZone ob, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateSafeZone_Boradcast(ob);
			}
		}

		protected sealed class UpdateSafeZone_ImplementationPlayer_003C_003ESystem_Int64_0023Sandbox_Common_ObjectBuilders_MyObjectBuilder_SafeZone : ICallSite<IMyEventOwner, long, MyObjectBuilder_SafeZone, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long safezoneBlockId, in MyObjectBuilder_SafeZone ob, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateSafeZone_ImplementationPlayer(safezoneBlockId, ob);
			}
		}

		protected sealed class UpdateSafeZoneRadius_ImplementationPlayer_003C_003ESystem_Int64_0023System_Int64_0023System_Single : ICallSite<IMyEventOwner, long, long, float, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long safezoneBlockId, in long safezoneId, in float radius, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateSafeZoneRadius_ImplementationPlayer(safezoneBlockId, safezoneId, radius);
			}
		}

		protected sealed class UpdateGlobalSafeZone_Implementation_003C_003EVRage_Game_ObjectBuilders_Components_MySafeZoneAction : ICallSite<IMyEventOwner, MySafeZoneAction, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MySafeZoneAction allowedActions, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateGlobalSafeZone_Implementation(allowedActions);
			}
		}

		private static MyConcurrentList<MySafeZone> m_safeZones = new MyConcurrentList<MySafeZone>();

		public static MySafeZoneAction AllowedActions = MySafeZoneAction.All;

		private static HashSet<MyEntity> m_entitiesToForget = new HashSet<MyEntity>();

		private static HashSet<MyEntity> m_recentlyAddedEntities = new HashSet<MyEntity>();

		private static HashSet<MyEntity> m_recentlyRemovedEntities = new HashSet<MyEntity>();

		private const int FRAMES_TO_REMOVE_RECENT = 100;

		private int m_recentCounter;

		public override bool IsRequiredByGame => true;

		public static ListReader<MySafeZone> SafeZones => m_safeZones.List;

		public static event EventHandler OnAddSafeZone;

		public static event EventHandler OnRemoveSafeZone;

		public static event Action<MySafeZone> OnSafeZoneUpdated;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			AllowedActions = (sessionComponent as MyObjectBuilder_SessionComponentSafeZones).AllowedActions;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			return new MyObjectBuilder_SessionComponentSafeZones
			{
				AllowedActions = AllowedActions
			};
		}

		public static void AddSafeZone(MySafeZone safeZone)
		{
			m_safeZones.Add(safeZone);
			if (MySessionComponentSafeZones.OnAddSafeZone != null)
			{
				MySessionComponentSafeZones.OnAddSafeZone(safeZone, null);
			}
		}

		public static void RemoveSafeZone(MySafeZone safeZone)
		{
			m_safeZones.Remove(safeZone);
			if (MySessionComponentSafeZones.OnRemoveSafeZone != null)
			{
				MySessionComponentSafeZones.OnRemoveSafeZone(safeZone, null);
			}
		}

		public static void RequestCreateSafeZone(Vector3D position)
		{
			if (MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateSafeZone_Implementation, position);
			}
		}

		[Event(null, 116)]
		[Reliable]
		[Server]
		public static void CreateSafeZone_Implementation(Vector3D position)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				CrateSafeZone(MatrixD.CreateWorld(position), MySafeZoneShape.Box, MySafeZoneAccess.Whitelist, null, null, 100f, enable: false, isVisible: true, default(Vector3), "", 0L);
			}
		}

		public static long CreateSafeZone_ImplementationPlayer(long safeZoneBlockId, float startRadius, bool activate, ulong playerSteamId)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.Error("CreateSafeZone_ImplementationPlayer can be only called by server.");
				return 0L;
			}
			_ = MyMultiplayer.Static;
			if (IsPlayerValidationFailed(playerSteamId, safeZoneBlockId, out MyIdentity playerIdentity, out MyCubeBlock beaconBlock))
			{
				return 0L;
			}
			long[] factions = null;
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(beaconBlock.SlimBlock.OwnerId);
			if (playerFaction != null)
			{
				factions = new long[1]
				{
					playerFaction.FactionId
				};
			}
			long[] array = null;
			return CrateSafeZone(players: (playerIdentity.IdentityId == beaconBlock.SlimBlock.OwnerId) ? new long[1]
			{
				playerIdentity.IdentityId
			} : new long[2]
			{
				playerIdentity.IdentityId,
				beaconBlock.SlimBlock.OwnerId
			}, transform: beaconBlock.PositionComp.WorldMatrix, safeZoneShape: MySafeZoneShape.Sphere, zoneAccess: MySafeZoneAccess.Whitelist, factions: factions, startRadius: startRadius, enable: activate, isVisible: true, color: Color.SkyBlue.ToVector3(), visualTexture: "", safeZoneBlockId: safeZoneBlockId).EntityId;
		}

		private static bool IsPlayerValidationFailed(ulong steamId, long safeZoneBlockId, out MyIdentity playerIdentity, out MyCubeBlock beaconBlock)
		{
			MyMultiplayerServerBase myMultiplayerServerBase = MyMultiplayer.Static as MyMultiplayerServerBase;
			playerIdentity = null;
			beaconBlock = null;
			if (!MyEntities.TryGetEntityById(safeZoneBlockId, out MyCubeBlock entity))
			{
				myMultiplayerServerBase?.ValidationFailed(steamId);
				return true;
			}
			beaconBlock = entity;
			long identityId = MySession.Static.Players.TryGetIdentityId(steamId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null || myIdentity.Character == null)
			{
				myMultiplayerServerBase?.ValidationFailed(steamId);
				return true;
			}
			playerIdentity = myIdentity;
			return false;
		}

		public static MyEntity CrateSafeZone(MatrixD transform, MySafeZoneShape safeZoneShape, MySafeZoneAccess zoneAccess, long[] players, long[] factions, float startRadius, bool enable, bool isVisible = true, Vector3 color = default(Vector3), string visualTexture = "", long safeZoneBlockId = 0L)
		{
			MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = new MyObjectBuilder_SafeZone();
			myObjectBuilder_SafeZone.PositionAndOrientation = new MyPositionAndOrientation(transform);
			myObjectBuilder_SafeZone.Radius = startRadius;
			myObjectBuilder_SafeZone.PersistentFlags = MyPersistentEntityFlags2.InScene;
			myObjectBuilder_SafeZone.Shape = safeZoneShape;
			myObjectBuilder_SafeZone.AccessTypePlayers = zoneAccess;
			myObjectBuilder_SafeZone.AccessTypeFactions = zoneAccess;
			myObjectBuilder_SafeZone.AccessTypeGrids = zoneAccess;
			myObjectBuilder_SafeZone.AccessTypeFloatingObjects = zoneAccess;
			myObjectBuilder_SafeZone.IsVisible = isVisible;
			myObjectBuilder_SafeZone.ModelColor = color;
			if (!string.IsNullOrEmpty(visualTexture))
			{
				myObjectBuilder_SafeZone.Texture = visualTexture;
			}
			if (players != null)
			{
				myObjectBuilder_SafeZone.Players = players;
				myObjectBuilder_SafeZone.Factions = factions;
			}
			myObjectBuilder_SafeZone.Enabled = enable;
			myObjectBuilder_SafeZone.SafeZoneBlockId = safeZoneBlockId;
			return MyEntities.CreateFromObjectBuilderAndAdd(myObjectBuilder_SafeZone, fadeIn: false);
		}

		public static void RequestDeleteSafeZone(long entityId)
		{
			if (MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => DeleteSafeZone_Implementation, entityId);
			}
		}

		[Event(null, 270)]
		[Reliable]
		[Server]
		public static void DeleteSafeZone_Implementation(long entityId)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyEntity entity = null;
			if (MyEntities.TryGetEntityById(entityId, out entity))
			{
				entity.Close();
			}
		}

		public static void DeleteSafeZone_ImplementationPlayer(long safeZoneBlockId, long safeZoneId, ulong steamId)
		{
			MyIdentity playerIdentity;
			MyCubeBlock beaconBlock;
			if (!Sync.IsServer)
			{
				MyLog.Default.Error("CreateSafeZone_ImplementationPlayer can be only called by server.");
			}
			else if (!IsPlayerValidationFailed(steamId, safeZoneBlockId, out playerIdentity, out beaconBlock))
			{
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(safeZoneId, out entity))
				{
					entity.Close();
				}
			}
		}

		public static void RequestUpdateSafeZone(MyObjectBuilder_SafeZone ob)
		{
			if (MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateSafeZone_Implementation, ob);
			}
		}

		[Event(null, 316)]
		[Reliable]
		[Server]
		[Broadcast]
		public static void UpdateSafeZone_Implementation(MyObjectBuilder_SafeZone ob)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				MyEventContext.ValidationFailed();
			}
			else
			{
				UpdateSafeZone(ob);
			}
		}

		public static void UpdateSafeZone(MyObjectBuilder_SafeZone ob, bool sync = false)
		{
			MySafeZone entity = null;
			if (MyEntities.TryGetEntityById(ob.EntityId, out entity))
			{
				if (IsSafeZoneColliding(ob.EntityId, entity.PositionComp.WorldMatrix, ob.Shape, ob.Radius, ob.Size))
				{
					return;
				}
				entity.InitInternal(ob);
				MySessionComponentSafeZones.OnSafeZoneUpdated?.Invoke(entity);
			}
			if (sync)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateSafeZone_Boradcast, ob);
			}
		}

		[Event(null, 349)]
		[Reliable]
		[Broadcast]
		private static void UpdateSafeZone_Boradcast(MyObjectBuilder_SafeZone ob)
		{
			UpdateSafeZone(ob);
		}

		public static void RequestUpdateSafeZone_Player(long safeZoneBlockId, MyObjectBuilder_SafeZone ob)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateSafeZone_ImplementationPlayer, safeZoneBlockId, ob);
		}

		[Event(null, 365)]
		[Reliable]
		[Server]
		[Broadcast]
		public static void UpdateSafeZone_ImplementationPlayer(long safezoneBlockId, MyObjectBuilder_SafeZone ob)
		{
			if (Sync.IsServer && IsPlayerValidationFailed(MyEventContext.Current.Sender.Value, safezoneBlockId, out MyIdentity _, out MyCubeBlock _))
			{
				MyEventContext.ValidationFailed();
				return;
			}
			if (Sync.IsServer && ob.Texture != "SafeZone_Texture_Default")
			{
				MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
				bool flag = true;
				if (component != null)
				{
					flag = component.HasDLC(MyDLCs.MyDLC.EconomyExpansion.Name, MyEventContext.Current.Sender.Value);
				}
				if (!flag)
				{
					MyEventContext.ValidationFailed();
					return;
				}
			}
			UpdateSafeZone(ob);
		}

		public static void RequestUpdateSafeZoneRadius_Player(long safezoneBlockId, long safezoneId, float newRadius)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateSafeZoneRadius_ImplementationPlayer, safezoneBlockId, safezoneId, newRadius);
		}

		[Event(null, 398)]
		[Reliable]
		[Server]
		[Broadcast]
		public static void UpdateSafeZoneRadius_ImplementationPlayer(long safezoneBlockId, long safezoneId, float radius)
		{
			if (Sync.IsServer && IsPlayerValidationFailed(MyEventContext.Current.Sender.Value, safezoneBlockId, out MyIdentity _, out MyCubeBlock _))
			{
				MyEventContext.ValidationFailed();
				return;
			}
			MySafeZone entity = null;
			if (MyEntities.TryGetEntityById(safezoneId, out entity) && !IsSafeZoneColliding(safezoneId, entity.PositionComp.WorldMatrix, MySafeZoneShape.Sphere, radius))
			{
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = (MyObjectBuilder_SafeZone)entity.GetObjectBuilder();
				myObjectBuilder_SafeZone.Radius = radius;
				entity.InitInternal(myObjectBuilder_SafeZone, Sync.IsServer);
				MySessionComponentSafeZones.OnSafeZoneUpdated?.Invoke(entity);
			}
		}

		public static bool IsSafeZoneColliding(long safeZoneId, MatrixD safeZoneWorld, MySafeZoneShape shape, float newRadius = 0f, Vector3 newSize = default(Vector3))
		{
			BoundingSphereD sphere = new BoundingSphereD(safeZoneWorld.Translation, newRadius);
			MatrixD matrix = MatrixD.CreateScale(newSize) * safeZoneWorld;
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(matrix);
			foreach (MySafeZone safeZone in m_safeZones)
			{
				if (safeZone.EntityId != safeZoneId)
				{
					int collisionCase = GetCollisionCase(shape, safeZone.Shape);
					MatrixD worldMatrix = safeZone.PositionComp.WorldMatrix;
					BoundingSphereD sphere2 = new BoundingSphereD(worldMatrix.Translation, safeZone.Radius);
					MatrixD matrix2 = MatrixD.CreateScale(safeZone.Size) * worldMatrix;
					MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(matrix2);
					switch (collisionCase)
					{
					case 0:
						if (sphere.Intersects(sphere2))
						{
							return true;
						}
						break;
					case 1:
						if (myOrientedBoundingBoxD.Intersects(ref other))
						{
							return true;
						}
						break;
					case 2:
						if (shape == MySafeZoneShape.Sphere)
						{
							if (other.Intersects(ref sphere))
							{
								return true;
							}
						}
						else if (myOrientedBoundingBoxD.Intersects(ref sphere2))
						{
							return true;
						}
						break;
					}
				}
			}
			return false;
		}

		private static int GetCollisionCase(MySafeZoneShape shape, MySafeZoneShape otherShape)
		{
			if (shape == MySafeZoneShape.Sphere && otherShape == MySafeZoneShape.Sphere)
			{
				return 0;
			}
			if (shape == MySafeZoneShape.Box && otherShape == MySafeZoneShape.Box)
			{
				return 1;
			}
			return 2;
		}

		public static void RequestUpdateGlobalSafeZone()
		{
			if (MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateGlobalSafeZone_Implementation, AllowedActions);
			}
			else if (!MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}

		[Event(null, 498)]
		[Reliable]
		[Server]
		[Broadcast]
		public static void UpdateGlobalSafeZone_Implementation(MySafeZoneAction allowedActions)
		{
			if (!MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				MyEventContext.ValidationFailed();
			}
			else
			{
				AllowedActions = allowedActions;
			}
		}

		public static bool IsActionAllowed(MyEntity entity, MySafeZoneAction action, long sourceEntityId = 0L, ulong user = 0uL)
		{
			MyCharacter myCharacter;
			if (user != 0L)
			{
				if (MySession.Static.IsUserAdmin(user) && MySafeZone.CheckAdminIgnoreSafezones(user))
				{
					return true;
				}
			}
			else if ((myCharacter = (entity as MyCharacter)) != null && myCharacter.ControllerInfo != null && myCharacter.ControllerInfo.Controller != null && myCharacter.ControllerInfo.Controller.Player != null)
			{
				ulong steamId = myCharacter.ControllerInfo.Controller.Player.Id.SteamId;
				if (MySession.Static.IsUserAdmin(steamId) && MySafeZone.CheckAdminIgnoreSafezones(steamId))
				{
					return true;
				}
			}
			if (!AllowedActions.HasFlag(action))
			{
				return false;
			}
			foreach (MySafeZone safeZone in m_safeZones)
			{
				if (!safeZone.IsActionAllowed(entity, action, sourceEntityId))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsActionAllowed(BoundingBoxD aabb, MySafeZoneAction action, long sourceEntityId = 0L, ulong user = 0uL)
		{
			if (user != 0L && MySession.Static.IsUserAdmin(user) && MySafeZone.CheckAdminIgnoreSafezones(user))
			{
				return true;
			}
			if (!AllowedActions.HasFlag(action))
			{
				return false;
			}
			foreach (MySafeZone safeZone in m_safeZones)
			{
				if (!safeZone.IsActionAllowed(aabb, action, sourceEntityId))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsActionAllowed(Vector3D point, MySafeZoneAction action, long sourceEntityId = 0L, ulong user = 0uL)
		{
			if (user != 0L && MySession.Static.IsUserAdmin(user) && MySafeZone.CheckAdminIgnoreSafezones(user))
			{
				return true;
			}
			if (!AllowedActions.HasFlag(action))
			{
				return false;
			}
			foreach (MySafeZone safeZone in m_safeZones)
			{
				if (!safeZone.IsActionAllowed(point, action, sourceEntityId))
				{
					return false;
				}
			}
			return true;
		}

		public override void LoadData()
		{
			base.LoadData();
			if (Sync.IsServer)
			{
				MyEntities.OnEntityAdd += MyEntities_OnEntityAdd;
				MyEntities.OnEntityRemove += MyEntities_OnEntityRemove;
				MyEntities.OnEntityDelete += MyEntities_OnEntityDelete;
			}
		}

		private void MyEntities_OnEntityAdd(MyEntity obj)
		{
			if (obj.Physics != null && obj.Physics.IsStatic)
			{
				foreach (MySafeZone safeZone in m_safeZones)
				{
					safeZone.InsertEntity(obj);
				}
			}
			m_recentlyAddedEntities.Add(obj);
			m_recentCounter = 100;
		}

		private void MyEntities_OnEntityRemove(MyEntity obj)
		{
			if (obj.Physics != null && obj.Physics.IsStatic)
			{
				foreach (MySafeZone safeZone in m_safeZones)
				{
					safeZone.RemoveEntityInternal(obj, addedOrRemoved: true);
				}
			}
			m_recentlyRemovedEntities.Add(obj);
			m_recentCounter = 100;
		}

		private void MyEntities_OnEntityDelete(MyEntity obj)
		{
			m_entitiesToForget.Add(obj);
		}

		protected override void UnloadData()
		{
			m_safeZones.Clear();
			m_entitiesToForget.Clear();
			m_recentlyAddedEntities.Clear();
			m_recentlyRemovedEntities.Clear();
			base.UnloadData();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_recentCounter > 0)
			{
				m_recentCounter--;
				if (m_recentCounter == 0)
				{
					m_entitiesToForget.Clear();
					m_recentlyAddedEntities.Clear();
					m_recentlyRemovedEntities.Clear();
				}
			}
			if (m_entitiesToForget.Count > 0)
			{
				foreach (MyEntity item in m_entitiesToForget)
				{
					m_recentlyAddedEntities.Remove(item);
					m_recentlyRemovedEntities.Remove(item);
				}
				m_entitiesToForget.Clear();
			}
		}

		public static bool IsRecentlyAddedOrRemoved(MyEntity obj)
		{
			if (!m_recentlyAddedEntities.Contains(obj))
			{
				return m_recentlyRemovedEntities.Contains(obj);
			}
			return true;
		}
	}
}
