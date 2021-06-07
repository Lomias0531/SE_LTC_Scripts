using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Audio;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_SafeZone), true)]
	public class MySafeZone : MyEntity, IMyEventProxy, IMyEventOwner
	{
		protected sealed class InsertEntity_Implementation_003C_003ESystem_Int64_0023System_Boolean : ICallSite<MySafeZone, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZone @this, in long entityId, in bool addedOrRemoved, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.InsertEntity_Implementation(entityId, addedOrRemoved);
			}
		}

		protected sealed class InsertEntities_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<MySafeZone, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZone @this, in List<long> list, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.InsertEntities_Implementation(list);
			}
		}

		protected sealed class RemoveEntity_Implementation_003C_003ESystem_Int64_0023System_Boolean : ICallSite<MySafeZone, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZone @this, in long entityId, in bool addedOrRemoved, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemoveEntity_Implementation(entityId, addedOrRemoved);
			}
		}

		private class Sandbox_Game_Entities_MySafeZone_003C_003EActor : IActivator, IActivator<MySafeZone>
		{
			private sealed override object CreateInstance()
			{
				return new MySafeZone();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySafeZone CreateInstance()
			{
				return new MySafeZone();
			}

			MySafeZone IActivator<MySafeZone>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const string MODEL_SPHERE = "Models\\Environment\\SafeZone\\SafeZone.mwm";

		private const string MODEL_BOX = "Models\\Environment\\SafeZone\\SafeZoneBox.mwm";

		public float Radius;

		protected MyConcurrentHashSet<long> m_containedEntities = new MyConcurrentHashSet<long>();

		private static object m_drawLock = new object();

		public List<MyFaction> Factions = new List<MyFaction>();

		public List<long> Players = new List<long>();

		public HashSet<long> Entities = new HashSet<long>();

		private long m_safezoneBlockId;

		private List<long> m_entitiesToSend = new List<long>();

		private List<long> m_entitiesToAdd = new List<long>();

		private MyHudNotification m_safezoneEnteredNotification = new MyHudNotification(MyCommonTexts.SafeZone_Entered, 2000, "White");

		private MyHudNotification m_safezoneLeftNotification = new MyHudNotification(MyCommonTexts.SafeZone_Left, 2000, "White");

		private Dictionary<MyStringHash, MyTextureChange> m_texturesDefinitions;

		private MySafeZoneSettingsDefinition m_safeZoneSettings;

		private Color m_animatedColor;

		private TimeSpan m_blendTimer;

		private bool m_isAnimating;

		private Vector3 m_size;

		public bool Enabled
		{
			get;
			set;
		}

		public long SafeZoneBlockId => m_safezoneBlockId;

		public MySafeZoneAccess AccessTypePlayers
		{
			get;
			set;
		}

		public MySafeZoneAccess AccessTypeFactions
		{
			get;
			set;
		}

		public MySafeZoneAccess AccessTypeGrids
		{
			get;
			set;
		}

		public MySafeZoneAccess AccessTypeFloatingObjects
		{
			get;
			set;
		}

		public MySafeZoneAction AllowedActions
		{
			get;
			set;
		}

		public MySafeZoneShape Shape
		{
			get;
			set;
		}

		public Color ModelColor
		{
			get;
			private set;
		}

		public MyStringHash CurrentTexture
		{
			get;
			private set;
		}

		public MyTextureChange DisabledTexture
		{
			get;
			private set;
		}

		public bool IsVisible
		{
			get;
			set;
		}

		public Vector3 Size
		{
			get
			{
				return m_size;
			}
			set
			{
				if (m_size != value)
				{
					m_size = value;
				}
			}
		}

		public bool IsActionAllowed(MyEntity entity, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			if (entity == null)
			{
				return false;
			}
			if (!m_containedEntities.Contains(entity.EntityId))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out MyEntity entity2) && !IsSafe(entity2.GetTopMostParent()))
			{
				return false;
			}
			return AllowedActions.HasFlag(action);
		}

		private bool IsOutside(BoundingBoxD aabb)
		{
			bool flag = false;
			if (Shape == MySafeZoneShape.Sphere)
			{
				return !new BoundingSphereD(base.PositionComp.GetPosition(), Radius).Intersects(aabb);
			}
			return !new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrix).Intersects(ref aabb);
		}

		private bool IsOutside(MyEntity entity)
		{
			bool flag = false;
			MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrix);
			if (Shape != 0)
			{
				return !new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrix).Intersects(ref other);
			}
			BoundingSphereD sphere = new BoundingSphereD(base.PositionComp.GetPosition(), Radius);
			return !other.Intersects(ref sphere);
		}

		public bool IsEntityInsideAlone(long entityId)
		{
			int num = 0;
			foreach (long containedEntity in m_containedEntities)
			{
				MyEntity entity = null;
				MyEntities.TryGetEntityById(containedEntity, out entity);
				if (!(entity is MyVoxelPhysics))
				{
					num++;
				}
			}
			if (num == 1)
			{
				return m_containedEntities.Contains(entityId);
			}
			return false;
		}

		public bool IsEmpty()
		{
			return m_containedEntities.Count == 0;
		}

		public bool IsActionAllowed(BoundingBoxD aabb, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			if (IsOutside(aabb))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out MyEntity entity) && !IsSafe(entity.GetTopMostParent()))
			{
				return false;
			}
			return AllowedActions.HasFlag(action);
		}

		public bool IsActionAllowed(Vector3D point, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			bool flag = false;
			if ((Shape != 0) ? (!new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrix).Contains(ref point)) : (new BoundingSphereD(base.PositionComp.GetPosition(), Radius).Contains(point) != ContainmentType.Contains))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out MyEntity entity) && !IsSafe(entity.GetTopMostParent()))
			{
				return false;
			}
			return AllowedActions.HasFlag(action);
		}

		public MySafeZone()
		{
			base.SyncFlag = true;
		}

		protected override void Closing()
		{
			MySessionComponentSafeZones.RemoveSafeZone(this);
			base.Closing();
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			if (m_texturesDefinitions == null)
			{
				IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
				if (allDefinitions == null)
				{
					MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
				}
				else
				{
					m_texturesDefinitions = new Dictionary<MyStringHash, MyTextureChange>();
					foreach (MySafeZoneTexturesDefinition item in allDefinitions)
					{
						if (item.Id.SubtypeName == "Disabled")
						{
							DisabledTexture = item.Texture;
						}
						m_texturesDefinitions.Add(item.DisplayTextId, item.Texture);
					}
					if (m_texturesDefinitions.Count == 0)
					{
						MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
					}
				}
			}
			if (m_safeZoneSettings == null)
			{
				MySafeZoneSettingsDefinition definition = MyDefinitionManager.Static.GetDefinition<MySafeZoneSettingsDefinition>("SafeZoneSettings");
				if (definition == null)
				{
					MyLog.Default.Error("Safe Zone Settings definition for safe zone are missing. Without it, safezone wont work propertly.");
					m_safeZoneSettings = new MySafeZoneSettingsDefinition();
				}
				else
				{
					m_safeZoneSettings = definition;
				}
			}
			CurrentTexture = MyStringHash.NullOrEmpty;
			MyRenderComponentSafeZone myRenderComponentSafeZone = (MyRenderComponentSafeZone)(base.Render = new MyRenderComponentSafeZone());
			base.Render.PersistentFlags &= ~MyPersistentEntityFlags2.CastShadows;
			base.Render.EnableColorMaskHsv = true;
			base.Render.FadeIn = (base.Render.FadeOut = true);
			base.Save = true;
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
			MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = (MyObjectBuilder_SafeZone)objectBuilder;
			InitInternal(myObjectBuilder_SafeZone, insertEntities: false);
			Init(null, "Models\\Environment\\SafeZone\\SafeZone.mwm", null, null);
			if (Shape == MySafeZoneShape.Sphere)
			{
				base.PositionComp.LocalAABB = new BoundingBox(new Vector3(0f - Radius), new Vector3(Radius));
			}
			else
			{
				base.PositionComp.LocalAABB = new BoundingBox(-Size / 2f, Size / 2f);
			}
			MySessionComponentSafeZones.AddSafeZone(this);
			if (base.PositionComp != null)
			{
				base.PositionComp.OnPositionChanged += PositionComp_OnPositionChanged;
			}
			base.DisplayName = myObjectBuilder_SafeZone.DisplayName;
			m_safezoneBlockId = myObjectBuilder_SafeZone.SafeZoneBlockId;
		}

		internal void InitInternal(MyObjectBuilder_SafeZone ob, bool insertEntities = true)
		{
			bool flag = Radius != ob.Radius;
			Radius = ob.Radius;
			bool enabled = Enabled;
			bool flag2 = Enabled != ob.Enabled;
			Enabled = ob.Enabled;
			AccessTypePlayers = ob.AccessTypePlayers;
			AccessTypeFactions = ob.AccessTypeFactions;
			AccessTypeGrids = ob.AccessTypeGrids;
			AccessTypeFloatingObjects = ob.AccessTypeFloatingObjects;
			AllowedActions = ob.AllowedActions;
			bool flag3 = Size != ob.Size;
			Size = ob.Size;
			bool flag4 = Shape != ob.Shape;
			Shape = ob.Shape;
			IsVisible = ob.IsVisible;
			Color color = new Color(ob.ModelColor);
			bool num = color != ModelColor;
			ModelColor = color;
			MyStringHash orCompute = MyStringHash.GetOrCompute(ob.Texture);
			bool flag5 = false;
			if (m_texturesDefinitions.TryGetValue(orCompute, out MyTextureChange _))
			{
				flag5 = (CurrentTexture != orCompute);
				CurrentTexture = orCompute;
			}
			bool flag6 = false;
			if (ob.PositionAndOrientation.HasValue)
			{
				MatrixD other = ob.PositionAndOrientation.Value.GetMatrix();
				flag6 = !base.PositionComp.WorldMatrix.EqualsFast(ref other, 0.01);
				base.PositionComp.WorldMatrix = ob.PositionAndOrientation.Value.GetMatrix();
			}
			if (ob.Factions != null)
			{
				Factions = (from x in ob.Factions.ToList().ConvertAll((long x) => (MyFaction)MySession.Static.Factions.TryGetFactionById(x))
					where x != null
					select x).ToList();
			}
			if (ob.Players != null)
			{
				Players = ob.Players.ToList();
			}
			if (ob.Entities != null)
			{
				Entities = new HashSet<long>(ob.Entities);
			}
			if (flag || flag4 || flag3 || flag6)
			{
				RecreatePhysics(insertEntities, triggerNotification: false);
				flag2 = false;
			}
			if (flag2 && insertEntities)
			{
				StartEnableAnimation(enabled);
				InsertContainingEntities(triggerNotification: false);
			}
			if (num || (flag2 && insertEntities) || flag5 || flag4)
			{
				RefreshGraphics();
			}
			if (!Sync.IsServer && ob.ContainedEntities != null)
			{
				m_entitiesToAdd.AddRange(ob.ContainedEntities);
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = (MyObjectBuilder_SafeZone)base.GetObjectBuilder(copy);
			myObjectBuilder_SafeZone.Radius = Radius;
			myObjectBuilder_SafeZone.Size = Size;
			myObjectBuilder_SafeZone.Shape = Shape;
			myObjectBuilder_SafeZone.Enabled = Enabled;
			myObjectBuilder_SafeZone.AccessTypePlayers = AccessTypePlayers;
			myObjectBuilder_SafeZone.AccessTypeFactions = AccessTypeFactions;
			myObjectBuilder_SafeZone.AccessTypeGrids = AccessTypeGrids;
			myObjectBuilder_SafeZone.AccessTypeFloatingObjects = AccessTypeFloatingObjects;
			myObjectBuilder_SafeZone.AllowedActions = AllowedActions;
			myObjectBuilder_SafeZone.DisplayName = base.DisplayName;
			myObjectBuilder_SafeZone.ModelColor = ModelColor.ToVector3();
			myObjectBuilder_SafeZone.Texture = CurrentTexture.String;
			myObjectBuilder_SafeZone.Factions = Factions.ConvertAll((MyFaction x) => x.FactionId).ToArray();
			myObjectBuilder_SafeZone.Players = Players.ToArray();
			myObjectBuilder_SafeZone.Entities = Entities.ToArray();
			myObjectBuilder_SafeZone.SafeZoneBlockId = m_safezoneBlockId;
			if (Sync.IsServer && m_containedEntities.Count > 0)
			{
				myObjectBuilder_SafeZone.ContainedEntities = m_containedEntities.ToArray();
			}
			myObjectBuilder_SafeZone.IsVisible = IsVisible;
			return myObjectBuilder_SafeZone;
		}

		public void RecreatePhysics(bool insertEntities = true, bool triggerNotification = true)
		{
			if (base.Physics != null)
			{
				base.Physics.Close();
				base.Physics = null;
			}
			if (Shape == MySafeZoneShape.Sphere)
			{
				base.PositionComp.LocalAABB = new BoundingBox(new Vector3(0f - Radius), new Vector3(Radius));
				UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(Radius));
			}
			else
			{
				base.PositionComp.LocalAABB = new BoundingBox(-Size / 2f, Size / 2f);
				UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", Size * 0.5f);
			}
			if (insertEntities)
			{
				m_containedEntities.Clear();
			}
			if (Sync.IsServer)
			{
				HkBvShape shape = CreateFieldShape();
				base.Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_KINEMATIC);
				base.Physics.IsPhantom = true;
				((MyPhysicsBody)base.Physics).CreateFromCollisionObject(shape, base.PositionComp.LocalVolume.Center, base.WorldMatrix);
				shape.Base.RemoveReference();
				base.Physics.Enabled = true;
				if (insertEntities)
				{
					InsertContainingEntities(triggerNotification);
				}
			}
			if (!Sync.IsDedicated)
			{
				RefreshGraphics();
			}
		}

		private void StartEnableAnimation(bool lastEnabled)
		{
			m_blendTimer = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMilliseconds(m_safeZoneSettings.EnableAnimationTimeMs);
			m_animatedColor = Color.Black;
			m_isAnimating = true;
			MyRenderComponentSafeZone myRenderComponentSafeZone;
			if ((myRenderComponentSafeZone = (base.Render as MyRenderComponentSafeZone)) != null)
			{
				myRenderComponentSafeZone.AddTransitionObject(GetTextureChange(lastEnabled));
				myRenderComponentSafeZone.UpdateTransitionObjColor(lastEnabled ? ModelColor : Color.White);
			}
		}

		private void UpdateRenderObject(string modelName, Vector3 scale)
		{
			MyRenderComponentSafeZone myRenderComponentSafeZone;
			if ((myRenderComponentSafeZone = (base.Render as MyRenderComponentSafeZone)) != null)
			{
				myRenderComponentSafeZone.SwitchModel(modelName);
				myRenderComponentSafeZone.ChangeScale(scale);
			}
		}

		public void RefreshGraphics()
		{
			MyRenderComponentSafeZone myRenderComponentSafeZone;
			if (!Sync.IsDedicated && (myRenderComponentSafeZone = (base.Render as MyRenderComponentSafeZone)) != null)
			{
				Color newColor = m_isAnimating ? m_animatedColor : (Enabled ? ModelColor : Color.White);
				myRenderComponentSafeZone.ChangeColor(newColor);
				myRenderComponentSafeZone.InvalidateRenderObjects();
				myRenderComponentSafeZone.TextureChanges = GetTextureChange(Enabled);
			}
		}

		private Dictionary<string, MyTextureChange> GetTextureChange(bool enabled)
		{
			if (enabled)
			{
				MyTextureChange value = m_texturesDefinitions[CurrentTexture];
				return new Dictionary<string, MyTextureChange>
				{
					{
						"SafeZoneShield_Material",
						value
					}
				};
			}
			return new Dictionary<string, MyTextureChange>
			{
				{
					"SafeZoneShield_Material",
					DisabledTexture
				}
			};
		}

		private void InsertContainingEntities(bool triggerNotification = true)
		{
			if (Sync.IsServer)
			{
				List<MyEntity> list = null;
				if (Shape == MySafeZoneShape.Sphere)
				{
					BoundingSphereD boundingSphere = new BoundingSphereD(base.PositionComp.WorldMatrix.Translation, Radius);
					list = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
				}
				else
				{
					MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrix);
					list = MyEntities.GetEntitiesInOBB(ref obb);
				}
				foreach (MyEntity item in list)
				{
					MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrix);
					MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(item.PositionComp.LocalAABB, item.PositionComp.WorldMatrix);
					if (myOrientedBoundingBoxD.Contains(ref other) == ContainmentType.Contains && InsertEntityInternal(item, addedOrRemoved: false, triggerNotification))
					{
						m_entitiesToSend.Add(item.EntityId);
					}
				}
				SendInsertedEntities(m_entitiesToSend);
				list.Clear();
				m_entitiesToSend.Clear();
			}
		}

		internal void InsertEntity(MyEntity entity)
		{
			if (Shape == MySafeZoneShape.Box)
			{
				MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrix);
				MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrix);
				if (myOrientedBoundingBoxD.Contains(ref other) != ContainmentType.Contains)
				{
					return;
				}
			}
			else
			{
				BoundingSphereD sphere = new BoundingSphereD(base.PositionComp.WorldMatrix.Translation, Radius);
				if (new MyOrientedBoundingBoxD(entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrix).Contains(ref sphere) != ContainmentType.Contains)
				{
					return;
				}
			}
			if (InsertEntityInternal(entity, addedOrRemoved: false))
			{
				SendInsertedEntity(entity.EntityId, addedOrRemoved: false);
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (Sync.IsServer)
			{
				InsertContainingEntities();
			}
			else
			{
				m_containedEntities.Clear();
				foreach (long item in m_entitiesToAdd)
				{
					InsertEntity_Implementation(item, addedOrRemoved: false);
				}
				m_entitiesToAdd.Clear();
			}
			if (!Sync.IsDedicated)
			{
				if (Shape == MySafeZoneShape.Sphere)
				{
					UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(Radius));
				}
				else
				{
					UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", Size * 0.5f);
				}
				RefreshGraphics();
			}
		}

		private HkBvShape CreateFieldShape()
		{
			return new HkBvShape(childShape: new HkPhantomCallbackShape(phantom_Enter, phantom_Leave), boundingVolumeShape: GetHkShape(), policy: HkReferencePolicy.TakeOwnership);
		}

		protected HkShape GetHkShape()
		{
			if (Shape == MySafeZoneShape.Sphere)
			{
				return new HkSphereShape(Radius);
			}
			return new HkBoxShape(Size / 2f);
		}

		private void phantom_Enter(HkPhantomCallbackShape sender, HkRigidBody body)
		{
			MyEntity myEntity = body.GetEntity(0u) as MyEntity;
			bool addedOrRemoved = MySessionComponentSafeZones.IsRecentlyAddedOrRemoved(myEntity);
			if (InsertEntityInternal(myEntity, addedOrRemoved))
			{
				SendInsertedEntity(myEntity.EntityId, addedOrRemoved);
			}
		}

		private bool InsertEntityInternal(MyEntity entity, bool addedOrRemoved, bool triggerNotification = true)
		{
			if (entity != null)
			{
				MyEntity topEntity = entity.GetTopMostParent();
				if (topEntity.Physics == null)
				{
					return false;
				}
				if (topEntity is MySafeZone)
				{
					return false;
				}
				if (topEntity.Physics.ShapeChangeInProgress)
				{
					return false;
				}
				if (!m_containedEntities.Contains(topEntity.EntityId))
				{
					m_containedEntities.Add(topEntity.EntityId);
					if (triggerNotification)
					{
						UpdatePlayerNotification(topEntity, addedOrRemoved);
					}
					MySandboxGame.Static.Invoke(delegate
					{
						if (topEntity.Physics != null && topEntity.Physics.HasRigidBody && !topEntity.Physics.IsStatic)
						{
							((MyPhysicsBody)topEntity.Physics).RigidBody.Activate();
						}
					}, "MyGravityGeneratorBase/Activate physics");
					MyCubeGrid myCubeGrid;
					if ((myCubeGrid = (entity as MyCubeGrid)) != null)
					{
						foreach (MyShipController fatBlock in myCubeGrid.GetFatBlocks<MyShipController>())
						{
							if (!(fatBlock is MyRemoteControl) && fatBlock.Pilot != null && fatBlock.Pilot.GetTopMostParent() == topEntity && InsertEntityInternal(fatBlock.Pilot, addedOrRemoved))
							{
								SendInsertedEntity(fatBlock.Pilot.EntityId, addedOrRemoved);
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		private void UpdatePlayerNotification(MyEntity topEntity, bool addedOrRemoved)
		{
			if (Enabled && MySession.Static.ControlledEntity != null && ((MyEntity)MySession.Static.ControlledEntity).GetTopMostParent() == topEntity && !addedOrRemoved)
			{
				if (!IsSafe((MyEntity)MySession.Static.ControlledEntity))
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
					return;
				}
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				MyHud.Notifications.Add(m_safezoneEnteredNotification);
			}
		}

		internal bool RemoveEntityInternal(MyEntity entity, bool addedOrRemoved)
		{
			bool num = m_containedEntities.Remove(entity.EntityId);
			if (num)
			{
				RemoveEntityLocal(entity, addedOrRemoved);
			}
			return num;
		}

		private void RemoveEntityLocal(MyEntity entity, bool addedOrRemoved)
		{
			if (Enabled && MySession.Static != null && MySession.Static.ControlledEntity != null && ((MyEntity)MySession.Static.ControlledEntity).GetTopMostParent() == entity && IsSafe(entity) && !addedOrRemoved && (!(entity is MyCharacter) || !((entity as MyCharacter).IsUsing is MyCockpit)))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				MyHud.Notifications.Add(m_safezoneLeftNotification);
			}
		}

		private void entity_OnClose(MyEntity obj)
		{
			if (base.PositionComp != null)
			{
				base.PositionComp.OnPositionChanged -= PositionComp_OnPositionChanged;
			}
			if (RemoveEntityInternal(obj, addedOrRemoved: true))
			{
				SendRemovedEntity(obj.EntityId, addedOrRemoved: true);
			}
		}

		private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
		{
			if (Shape == MySafeZoneShape.Sphere)
			{
				UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(Radius));
			}
			else
			{
				UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", Size * 0.5f);
			}
			RefreshGraphics();
		}

		private void phantom_Leave(HkPhantomCallbackShape sender, HkRigidBody body)
		{
			IMyEntity entity = body.GetEntity(0u);
			if (entity != null)
			{
				RemoveEntityPhantom(body, entity);
			}
		}

		private void RemoveEntityPhantom(HkRigidBody body, IMyEntity entity)
		{
			MyEntity topEntity = entity.GetTopMostParent() as MyEntity;
			if (topEntity.Physics != null && !topEntity.Physics.ShapeChangeInProgress && topEntity == entity)
			{
				bool addedOrRemoved = MySessionComponentSafeZones.IsRecentlyAddedOrRemoved(topEntity) || !entity.InScene;
				Vector3D position = entity.Physics.ClusterToWorld(body.Position);
				Quaternion rotation = Quaternion.CreateFromRotationMatrix(body.GetRigidBodyMatrix());
				MySandboxGame.Static.Invoke(delegate
				{
					if (base.Physics != null)
					{
						if (entity.MarkedForClose)
						{
							if (RemoveEntityInternal(topEntity, addedOrRemoved))
							{
								SendRemovedEntity(topEntity.EntityId, addedOrRemoved);
							}
						}
						else
						{
							MyCharacter myCharacter = entity as MyCharacter;
							bool flag = (myCharacter?.IsDead ?? false) || body.IsDisposed || !entity.Physics.IsInWorld;
							if (entity.Physics != null && !flag)
							{
								position = entity.Physics.ClusterToWorld(body.Position);
								rotation = Quaternion.CreateFromRotationMatrix(body.GetRigidBodyMatrix());
							}
							Vector3D translation = base.PositionComp.GetPosition();
							Quaternion rotation2 = Quaternion.CreateFromRotationMatrix(base.PositionComp.GetOrientation());
							HkShape shape = HkShape.Empty;
							if (entity.Physics != null)
							{
								MyPhysicsBody myPhysicsBody;
								if (entity.Physics.RigidBody != null)
								{
									shape = entity.Physics.RigidBody.GetShape();
								}
								else if ((myPhysicsBody = (entity.Physics as MyPhysicsBody)) != null && myCharacter != null && myPhysicsBody.CharacterProxy != null)
								{
									shape = myPhysicsBody.CharacterProxy.GetHitRigidBody().GetShape();
								}
							}
							if (flag || !shape.IsValid || !MyPhysics.IsPenetratingShapeShape(shape, ref position, ref rotation, base.Physics.RigidBody.GetShape(), ref translation, ref rotation2))
							{
								if (RemoveEntityInternal(topEntity, addedOrRemoved))
								{
									SendRemovedEntity(topEntity.EntityId, addedOrRemoved);
									MyCubeGrid myCubeGrid;
									if ((myCubeGrid = (topEntity as MyCubeGrid)) != null)
									{
										foreach (MyShipController fatBlock in myCubeGrid.GetFatBlocks<MyShipController>())
										{
											if (!(fatBlock is MyRemoteControl) && fatBlock.Pilot != null && fatBlock.Pilot != topEntity && RemoveEntityInternal(fatBlock.Pilot, addedOrRemoved))
											{
												SendRemovedEntity(fatBlock.Pilot.EntityId, addedOrRemoved);
											}
										}
									}
								}
								topEntity.OnClose -= entity_OnClose;
							}
						}
					}
				}, "Phantom leave");
			}
		}

		private void SendInsertedEntity(long entityId, bool addedOrRemoved)
		{
			if (base.IsReadyForReplication)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZone x) => x.InsertEntity_Implementation, entityId, addedOrRemoved);
			}
		}

		private void SendInsertedEntities(List<long> list)
		{
			if (base.IsReadyForReplication)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZone x) => x.InsertEntities_Implementation, list);
			}
		}

		private void SendRemovedEntity(long entityId, bool addedOrRemoved)
		{
			if (base.IsReadyForReplication)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZone x) => x.RemoveEntity_Implementation, entityId, addedOrRemoved);
			}
		}

		[Event(null, 904)]
		[Reliable]
		[BroadcastExcept]
		private void InsertEntity_Implementation(long entityId, bool addedOrRemoved)
		{
			if (!m_containedEntities.Contains(entityId))
			{
				m_containedEntities.Add(entityId);
				if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
				{
					UpdatePlayerNotification(entity, addedOrRemoved);
				}
			}
		}

		[Event(null, 919)]
		[Reliable]
		[BroadcastExcept]
		private void InsertEntities_Implementation(List<long> list)
		{
			foreach (long item in list)
			{
				InsertEntity_Implementation(item, addedOrRemoved: false);
			}
		}

		[Event(null, 928)]
		[Reliable]
		[BroadcastExcept]
		private void RemoveEntity_Implementation(long entityId, bool addedOrRemoved)
		{
			if (m_containedEntities.Contains(entityId))
			{
				m_containedEntities.Remove(entityId);
				if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
				{
					RemoveEntityLocal(entity, addedOrRemoved);
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (m_isAnimating)
			{
				TimeSpan timeSpan = m_blendTimer - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				if (timeSpan.Ticks < 0)
				{
					m_isAnimating = false;
					MyRenderComponentSafeZone myRenderComponentSafeZone;
					if ((myRenderComponentSafeZone = (base.Render as MyRenderComponentSafeZone)) != null)
					{
						myRenderComponentSafeZone.RemoveTransitionObject();
					}
				}
				else
				{
					float amount = 1f - (float)(timeSpan.TotalMilliseconds / (double)(float)m_safeZoneSettings.EnableAnimationTimeMs);
					Color value = Enabled ? ModelColor : Color.White;
					Color value2 = Enabled ? Color.White : ModelColor;
					m_animatedColor = Color.Lerp(Color.Black, value, amount);
					RefreshGraphics();
					MyRenderComponentSafeZone myRenderComponentSafeZone2;
					if ((myRenderComponentSafeZone2 = (base.Render as MyRenderComponentSafeZone)) != null)
					{
						Color color = Color.Lerp(value2, Color.Black, amount);
						myRenderComponentSafeZone2.UpdateTransitionObjColor(color);
					}
				}
			}
			if (Sync.IsServer && Enabled)
			{
				foreach (long containedEntity in m_containedEntities)
				{
					if (MyEntities.TryGetEntityById(containedEntity, out MyEntity entity) && !entity.Physics.IsKinematic && !entity.Physics.IsStatic && !IsSafe(entity))
					{
						MyAmmoBase myAmmoBase = entity as MyAmmoBase;
						MyMeteor myMeteor;
						if (myAmmoBase != null)
						{
							myAmmoBase.MarkForDestroy();
						}
						else if ((myMeteor = (entity as MyMeteor)) != null)
						{
							myMeteor.GameLogic.MarkForClose();
						}
						else
						{
							Vector3D value3 = entity.PositionComp.GetPosition() - base.PositionComp.GetPosition();
							if (value3.LengthSquared() > 0.10000000149011612)
							{
								value3.Normalize();
							}
							else
							{
								value3 = Vector3.Up;
							}
							Vector3D v = value3 * entity.Physics.Mass * 1000.0;
							entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, v, null, null);
						}
					}
				}
			}
		}

		private bool IsSafe(MyEntity entity)
		{
			MyFloatingObject obj = entity as MyFloatingObject;
			MyInventoryBagEntity myInventoryBagEntity = entity as MyInventoryBagEntity;
			if (obj != null || myInventoryBagEntity != null)
			{
				if (Entities.Contains(entity.EntityId))
				{
					return AccessTypeFloatingObjects == MySafeZoneAccess.Whitelist;
				}
				return AccessTypeFloatingObjects != MySafeZoneAccess.Whitelist;
			}
			MyEntity topMostParent = entity.GetTopMostParent();
			IMyComponentOwner<MyIDModule> myComponentOwner = topMostParent as IMyComponentOwner<MyIDModule>;
			if (myComponentOwner != null && myComponentOwner.GetComponent(out MyIDModule component))
			{
				ulong num = MySession.Static.Players.TryGetSteamId(component.Owner);
				if (num != 0L && CheckAdminIgnoreSafezones(num))
				{
					return true;
				}
				if (AccessTypePlayers == MySafeZoneAccess.Whitelist)
				{
					if (Players.Contains(component.Owner))
					{
						return true;
					}
				}
				else if (Players.Contains(component.Owner))
				{
					return false;
				}
				MyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(component.Owner) as MyFaction;
				if (myFaction != null)
				{
					if (AccessTypeFactions == MySafeZoneAccess.Whitelist)
					{
						if (Factions.Contains(myFaction))
						{
							return true;
						}
					}
					else if (Factions.Contains(myFaction))
					{
						return false;
					}
				}
				return AccessTypePlayers == MySafeZoneAccess.Blacklist;
			}
			MyCubeGrid myCubeGrid = topMostParent as MyCubeGrid;
			if (myCubeGrid != null)
			{
				if (myCubeGrid.BigOwners != null && myCubeGrid.BigOwners.Count > 0)
				{
					foreach (long bigOwner in myCubeGrid.BigOwners)
					{
						ulong num2 = MySession.Static.Players.TryGetSteamId(bigOwner);
						if (num2 != 0L && CheckAdminIgnoreSafezones(num2))
						{
							return true;
						}
					}
				}
				if (AccessTypeGrids == MySafeZoneAccess.Whitelist)
				{
					if (Entities.Contains(topMostParent.EntityId))
					{
						return true;
					}
				}
				else if (Entities.Contains(topMostParent.EntityId))
				{
					return false;
				}
				if (myCubeGrid.BigOwners.Count > 0)
				{
					foreach (long bigOwner2 in myCubeGrid.BigOwners)
					{
						MyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(bigOwner2) as MyFaction;
						if (myFaction2 != null)
						{
							if (Factions.Contains(myFaction2))
							{
								return AccessTypeFactions == MySafeZoneAccess.Whitelist;
							}
							return AccessTypeFactions != MySafeZoneAccess.Whitelist;
						}
					}
				}
				return AccessTypeGrids == MySafeZoneAccess.Blacklist;
			}
			if (entity is MyAmmoBase && !AllowedActions.HasFlag(MySafeZoneAction.Shooting))
			{
				return false;
			}
			if (entity is MyMeteor && !AllowedActions.HasFlag(MySafeZoneAction.Shooting))
			{
				return false;
			}
			return true;
		}

		public static bool CheckAdminIgnoreSafezones(ulong id)
		{
			AdminSettingsEnum adminSettingsEnum = AdminSettingsEnum.None;
			if (id == Sync.MyId)
			{
				adminSettingsEnum = MySession.Static.AdminSettings;
			}
			else if (MySession.Static.RemoteAdminSettings.ContainsKey(id))
			{
				adminSettingsEnum = MySession.Static.RemoteAdminSettings[id];
			}
			if ((adminSettingsEnum & AdminSettingsEnum.IgnoreSafeZones) != 0)
			{
				return true;
			}
			return false;
		}

		public void AddContainedToList()
		{
			foreach (long containedEntity in m_containedEntities)
			{
				if (MyEntities.TryGetEntityById(containedEntity, out MyEntity entity))
				{
					IMyComponentOwner<MyIDModule> myComponentOwner = entity as IMyComponentOwner<MyIDModule>;
					if (myComponentOwner != null && myComponentOwner.GetComponent(out MyIDModule component))
					{
						if (!Players.Contains(component.Owner))
						{
							Players.Add(component.Owner);
						}
					}
					else if (!Entities.Contains(entity.EntityId))
					{
						Entities.Add(entity.EntityId);
					}
				}
			}
		}

		public override bool GetIntersectionWithLine(ref LineD line, out Vector3D? v, bool useCollisionModel = true, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			v = null;
			RayD ray = new RayD(line.From, line.Direction);
			if (Shape == MySafeZoneShape.Sphere)
			{
				if (new BoundingSphereD(base.PositionComp.GetPosition(), Radius).IntersectRaySphere(ray, out double tmin, out double _))
				{
					v = line.From + line.Direction * tmin;
					return true;
				}
			}
			else
			{
				double? num = new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrix).Intersects(ref ray);
				if (num.HasValue)
				{
					v = line.From + line.Direction * num.Value;
					return true;
				}
			}
			return false;
		}
	}
}
