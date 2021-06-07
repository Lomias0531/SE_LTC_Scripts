using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage.Game.Components;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.Game.Networking;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace VRage.Game.Entity
{
	[GenerateActivator]
	[MyEntityType(typeof(MyObjectBuilder_EntityBase), true)]
	public class MyEntity : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		public struct EntityPin : IDisposable
		{
			private MyEntity m_entity;

			public EntityPin(MyEntity entity)
			{
				m_entity = entity;
				Interlocked.Increment(ref entity.m_pins);
			}

			public void Dispose()
			{
				m_entity.Unpin();
			}
		}

		[Serializable]
		public struct ContactPointData
		{
			[Flags]
			public enum ContactPointDataTypes
			{
				None = 0x0,
				Sounds = 0x1,
				Particle_PlanetCrash = 0x2,
				Particle_Collision = 0x4,
				Particle_GridCollision = 0x8,
				Particle_Dust = 0x10,
				AnySound = 0x1,
				AnyParticle = 0x1E
			}

			protected class VRage_Game_Entity_MyEntity_003C_003EContactPointData_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<ContactPointData, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactPointData owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactPointData owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class VRage_Game_Entity_MyEntity_003C_003EContactPointData_003C_003ELocalPosition_003C_003EAccessor : IMemberAccessor<ContactPointData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactPointData owner, in Vector3 value)
				{
					owner.LocalPosition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactPointData owner, out Vector3 value)
				{
					value = owner.LocalPosition;
				}
			}

			protected class VRage_Game_Entity_MyEntity_003C_003EContactPointData_003C_003ENormal_003C_003EAccessor : IMemberAccessor<ContactPointData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactPointData owner, in Vector3 value)
				{
					owner.Normal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactPointData owner, out Vector3 value)
				{
					value = owner.Normal;
				}
			}

			protected class VRage_Game_Entity_MyEntity_003C_003EContactPointData_003C_003EContactPointType_003C_003EAccessor : IMemberAccessor<ContactPointData, ContactPointDataTypes>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactPointData owner, in ContactPointDataTypes value)
				{
					owner.ContactPointType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactPointData owner, out ContactPointDataTypes value)
				{
					value = owner.ContactPointType;
				}
			}

			protected class VRage_Game_Entity_MyEntity_003C_003EContactPointData_003C_003ESeparatingVelocity_003C_003EAccessor : IMemberAccessor<ContactPointData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactPointData owner, in Vector3 value)
				{
					owner.SeparatingVelocity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactPointData owner, out Vector3 value)
				{
					value = owner.SeparatingVelocity;
				}
			}

			protected class VRage_Game_Entity_MyEntity_003C_003EContactPointData_003C_003ESeparatingSpeed_003C_003EAccessor : IMemberAccessor<ContactPointData, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactPointData owner, in float value)
				{
					owner.SeparatingSpeed = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactPointData owner, out float value)
				{
					value = owner.SeparatingSpeed;
				}
			}

			protected class VRage_Game_Entity_MyEntity_003C_003EContactPointData_003C_003EImpulse_003C_003EAccessor : IMemberAccessor<ContactPointData, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactPointData owner, in float value)
				{
					owner.Impulse = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactPointData owner, out float value)
				{
					value = owner.Impulse;
				}
			}

			public long EntityId;

			public Vector3 LocalPosition;

			public Vector3 Normal;

			public ContactPointDataTypes ContactPointType;

			public Vector3 SeparatingVelocity;

			public float SeparatingSpeed;

			public float Impulse;
		}

		public delegate MyObjectBuilder_EntityBase MyEntityFactoryCreateObjectBuilderDelegate(MyEntity entity);

		public delegate MySyncComponentBase CreateDefaultSyncEntityDelegate(MyEntity thisEntity);

		public delegate bool MyWeldingGroupsGroupExistsDelegate(MyEntity entity);

		protected class m_contactPoint_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType contactPoint;
				ISyncType result = contactPoint = new Sync<ContactPointData, SyncDirection.FromServer>(P_1, P_2);
				((MyEntity)P_0).m_contactPoint = (Sync<ContactPointData, SyncDirection.FromServer>)contactPoint;
				return result;
			}
		}

		private class VRage_Game_Entity_MyEntity_003C_003EActor : IActivator, IActivator<MyEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntity CreateInstance()
			{
				return new MyEntity();
			}

			MyEntity IActivator<MyEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected readonly Sync<ContactPointData, SyncDirection.FromServer> m_contactPoint;

		public MyDefinitionId? DefinitionId;

		public string Name;

		public bool DebugAsyncLoading;

		private List<MyEntity> m_tmpOnPhysicsChanged = new List<MyEntity>();

		protected List<MyHudEntityParams> m_hudParams = new List<MyHudEntityParams>();

		private string m_displayNameText;

		private MyPositionComponentBase m_position;

		public bool m_positionResetFromServer;

		private MyRenderComponentBase m_render;

		private List<MyDebugRenderComponentBase> m_debugRenderers = new List<MyDebugRenderComponentBase>();

		protected MyModel m_modelCollision;

		public int GamePruningProxyId = -1;

		public int TopMostPruningProxyId = -1;

		public bool StaticForPruningStructure;

		public int TargetPruningProxyId = -1;

		private bool m_raisePhysicsCalled;

		private long m_pins;

		private MyGameLogicComponent m_gameLogic;

		private long m_entityId;

		private MySyncComponentBase m_syncObject;

		private MyModStorageComponentBase m_storage;

		private bool m_isPreview;

		private bool m_isreadyForReplication;

		public Dictionary<IMyReplicable, Action> ReadyForReplicationAction = new Dictionary<IMyReplicable, Action>();

		private MyHierarchyComponent<MyEntity> m_hierarchy;

		/// Optimized link to physics component.
		private MyPhysicsComponentBase m_physics;

		private string m_displayName;

		public static Action<MyEntity> AddToGamePruningStructureExtCallBack;

		public static Action<MyEntity> RemoveFromGamePruningStructureExtCallBack;

		public static Action<MyEntity> UpdateGamePruningStructureExtCallBack;

		public static MyEntityFactoryCreateObjectBuilderDelegate MyEntityFactoryCreateObjectBuilderExtCallback;

		public static CreateDefaultSyncEntityDelegate CreateDefaultSyncEntityExtCallback;

		public static Action<MyEntity> MyWeldingGroupsAddNodeExtCallback;

		public static Action<MyEntity> MyWeldingGroupsRemoveNodeExtCallback;

		public static Action<MyEntity, List<MyEntity>> MyWeldingGroupsGetGroupNodesExtCallback;

		public static MyWeldingGroupsGroupExistsDelegate MyWeldingGroupsGroupExistsExtCallback;

		public static Action<MyEntity> MyProceduralWorldGeneratorTrackEntityExtCallback;

		public static Action<MyEntity> CreateStandardRenderComponentsExtCallback;

		public static Action<MyComponentContainer, MyObjectBuilderType, MyStringHash, MyObjectBuilder_ComponentContainer> InitComponentsExtCallback;

		public static Func<MyObjectBuilder_EntityBase, bool, MyEntity> MyEntitiesCreateFromObjectBuilderExtCallback;

		public MyEntityComponentContainer Components
		{
			get;
			private set;
		}

		public MyPositionComponentBase PositionComp
		{
			get
			{
				return m_position;
			}
			set
			{
				Components.Add(value);
			}
		}

		public MyRenderComponentBase Render
		{
			get
			{
				return m_render;
			}
			set
			{
				Components.Add(value);
			}
		}

		public MyGameLogicComponent GameLogic
		{
			get
			{
				return m_gameLogic;
			}
			set
			{
				Components.Add(value);
			}
		}

		/// <summary>
		/// Entity id, can be set by subclasses (for example when using pool...)
		/// </summary>
		public long EntityId
		{
			get
			{
				return m_entityId;
			}
			set
			{
				if (m_entityId != 0L)
				{
					long entityId = m_entityId;
					if (value == 0L)
					{
						m_entityId = 0L;
						MyEntityIdentifier.RemoveEntity(entityId);
					}
					else
					{
						m_entityId = value;
						MyEntityIdentifier.SwapRegisteredEntityId(this, entityId, m_entityId);
					}
				}
				else if (value != 0L)
				{
					m_entityId = value;
					MyEntityIdentifier.AddEntityWithId(this);
				}
			}
		}

		public MySyncComponentBase SyncObject
		{
			get
			{
				return m_syncObject;
			}
			protected set
			{
				Components.Add(value);
			}
		}

		public MyModStorageComponentBase Storage
		{
			get
			{
				return m_storage;
			}
			set
			{
				Components.Add(value);
			}
		}

		public bool Closed
		{
			get;
			protected set;
		}

		public bool MarkedForClose
		{
			get;
			protected set;
		}

		public virtual float MaxGlassDistSq
		{
			get
			{
				IMyCamera myCamera = (MyAPIGatewayShortcuts.GetMainCamera != null) ? MyAPIGatewayShortcuts.GetMainCamera() : null;
				if (myCamera != null)
				{
					return 0.01f * myCamera.FarPlaneDistance * myCamera.FarPlaneDistance;
				}
				return 4000000f;
			}
		}

		public bool Save
		{
			get
			{
				return (Flags & EntityFlags.Save) != 0;
			}
			set
			{
				if (value)
				{
					Flags |= EntityFlags.Save;
				}
				else
				{
					Flags &= ~EntityFlags.Save;
				}
			}
		}

		public bool IsPreview
		{
			get
			{
				return m_isPreview;
			}
			set
			{
				m_isPreview = value;
			}
		}

		public bool IsReadyForReplication
		{
			get
			{
				return m_isreadyForReplication;
			}
			private set
			{
				m_isreadyForReplication = value;
				if (m_isreadyForReplication && ReadyForReplicationAction.Count > 0)
				{
					foreach (Action value2 in ReadyForReplicationAction.Values)
					{
						value2();
					}
					ReadyForReplicationAction.Clear();
				}
			}
		}

		public MyEntityUpdateEnum NeedsUpdate
		{
			get
			{
				MyEntityUpdateEnum myEntityUpdateEnum = MyEntityUpdateEnum.NONE;
				if ((Flags & EntityFlags.NeedsUpdate) != 0)
				{
					myEntityUpdateEnum |= MyEntityUpdateEnum.EACH_FRAME;
				}
				if ((Flags & EntityFlags.NeedsUpdate10) != 0)
				{
					myEntityUpdateEnum |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				}
				if ((Flags & EntityFlags.NeedsUpdate100) != 0)
				{
					myEntityUpdateEnum |= MyEntityUpdateEnum.EACH_100TH_FRAME;
				}
				if ((Flags & EntityFlags.NeedsUpdateBeforeNextFrame) != 0)
				{
					myEntityUpdateEnum |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				if ((Flags & EntityFlags.NeedsSimulate) != 0)
				{
					myEntityUpdateEnum |= MyEntityUpdateEnum.SIMULATE;
				}
				return myEntityUpdateEnum;
			}
			set
			{
				if (value != NeedsUpdate)
				{
					if (InScene)
					{
						MyEntitiesInterface.UnregisterUpdate(this, arg2: false);
					}
					Flags &= ~EntityFlags.NeedsUpdateBeforeNextFrame;
					Flags &= ~EntityFlags.NeedsUpdate;
					Flags &= ~EntityFlags.NeedsUpdate10;
					Flags &= ~EntityFlags.NeedsUpdate100;
					Flags &= ~EntityFlags.NeedsSimulate;
					if ((value & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != 0)
					{
						Flags |= EntityFlags.NeedsUpdateBeforeNextFrame;
					}
					if ((value & MyEntityUpdateEnum.EACH_FRAME) != 0)
					{
						Flags |= EntityFlags.NeedsUpdate;
					}
					if ((value & MyEntityUpdateEnum.EACH_10TH_FRAME) != 0)
					{
						Flags |= EntityFlags.NeedsUpdate10;
					}
					if ((value & MyEntityUpdateEnum.EACH_100TH_FRAME) != 0)
					{
						Flags |= EntityFlags.NeedsUpdate100;
					}
					if ((value & MyEntityUpdateEnum.SIMULATE) != 0)
					{
						Flags |= EntityFlags.NeedsSimulate;
					}
					if (InScene)
					{
						MyEntitiesInterface.RegisterUpdate(this);
					}
				}
			}
		}

		public MatrixD WorldMatrix
		{
			get
			{
				if (PositionComp == null)
				{
					return MatrixD.Zero;
				}
				return PositionComp.WorldMatrix;
			}
			set
			{
				if (PositionComp != null)
				{
					PositionComp.SetWorldMatrix(value);
				}
			}
		}

		/// <summary>
		/// Gets or sets the parent.
		/// </summary>
		/// <value>
		/// The parent.
		/// </value>
		public MyEntity Parent
		{
			get
			{
				if (m_hierarchy == null || m_hierarchy.Parent == null)
				{
					return null;
				}
				return m_hierarchy.Parent.Container.Entity as MyEntity;
			}
			private set
			{
				m_hierarchy.Parent = value.Components.Get<MyHierarchyComponentBase>();
			}
		}

		public MyHierarchyComponent<MyEntity> Hierarchy
		{
			get
			{
				return m_hierarchy;
			}
			set
			{
				Components.Add((MyHierarchyComponentBase)value);
			}
		}

		MyHierarchyComponentBase VRage.ModAPI.IMyEntity.Hierarchy
		{
			get
			{
				return m_hierarchy;
			}
			set
			{
				if (value is MyHierarchyComponent<MyEntity>)
				{
					Components.Add(value);
				}
			}
		}

		/// Implementing interface IMyEntity - get/set physics component.
		MyPhysicsComponentBase VRage.ModAPI.IMyEntity.Physics
		{
			get
			{
				return m_physics;
			}
			set
			{
				Components.Add(value);
			}
		}

		/// Gets the physic component of the entity.
		public MyPhysicsComponentBase Physics
		{
			get
			{
				return m_physics;
			}
			set
			{
				MyPhysicsComponentBase physics = m_physics;
				Components.Add(value);
				this.OnPhysicsComponentChanged.InvokeIfNotNull(physics, value);
			}
		}

		public bool InvalidateOnMove
		{
			get
			{
				return (Flags & EntityFlags.InvalidateOnMove) != 0;
			}
			set
			{
				if (value)
				{
					Flags |= EntityFlags.InvalidateOnMove;
				}
				else
				{
					Flags &= ~EntityFlags.InvalidateOnMove;
				}
			}
		}

		public bool SyncFlag
		{
			get
			{
				return (Flags & EntityFlags.Sync) != 0;
			}
			set
			{
				Flags = (value ? (Flags | EntityFlags.Sync) : (Flags & ~EntityFlags.Sync));
			}
		}

		public bool NeedsWorldMatrix
		{
			get
			{
				return (Flags & EntityFlags.NeedsWorldMatrix) != 0;
			}
			set
			{
				Flags = (value ? (Flags | EntityFlags.NeedsWorldMatrix) : (Flags & ~EntityFlags.NeedsWorldMatrix));
				if (Hierarchy != null)
				{
					Hierarchy.UpdateNeedsWorldMatrix();
				}
			}
		}

		public bool InScene
		{
			get
			{
				if (Render != null)
				{
					return (Render.PersistentFlags & MyPersistentEntityFlags2.InScene) > MyPersistentEntityFlags2.None;
				}
				return false;
			}
			set
			{
				if (Render != null)
				{
					if (value)
					{
						Render.PersistentFlags |= MyPersistentEntityFlags2.InScene;
					}
					else
					{
						Render.PersistentFlags &= ~MyPersistentEntityFlags2.InScene;
					}
				}
			}
		}

		public virtual bool IsVolumetric => false;

		public virtual Vector3D LocationForHudMarker
		{
			get
			{
				if (PositionComp == null)
				{
					return Vector3D.Zero;
				}
				return PositionComp.GetPosition();
			}
		}

		public MyModel Model => Render.GetModel();

		public MyModel ModelCollision
		{
			get
			{
				if (m_modelCollision != null)
				{
					return m_modelCollision;
				}
				return Render.GetModel();
			}
		}

		public string DisplayName
		{
			get
			{
				return m_displayName;
			}
			set
			{
				m_displayName = value;
			}
		}

		public string DebugName
		{
			get
			{
				string text = m_displayName ?? Name;
				if (text == null)
				{
					text = "";
				}
				return text + " (" + GetType().Name + ", " + EntityId + ")";
			}
		}

		public Dictionary<string, MyEntitySubpart> Subparts
		{
			get;
			private set;
		}

		public virtual bool IsCCDForProjectiles => false;

		public bool Pinned => Interlocked.Read(ref m_pins) > 0;

		/// <summary>
		/// Iterate through inventories and return their count.
		/// </summary>
		public int InventoryCount
		{
			get
			{
				MyInventoryBase component = null;
				if (Components.TryGet(out component))
				{
					return component.GetInventoryCount();
				}
				return 0;
			}
		}

		/// <summary>
		/// Returns true if this entity has got at least one inventory. 
		/// Note that one aggregate inventory can contain zero simple inventories =&gt; zero will be returned even if GetInventoryBase() != null.
		/// </summary>
		public bool HasInventory => InventoryCount > 0;

		/// <summary>
		/// Display Name for GUI etc. Override in descendant classes. Usually used to display in terminal or inventory controls.
		/// </summary>
		public virtual string DisplayNameText
		{
			get;
			set;
		}

		public MySnapshotFlags LastSnapshotFlags
		{
			get;
			set;
		}

		public EntityFlags Flags
		{
			get;
			set;
		}

		VRage.ModAPI.IMyEntity VRage.ModAPI.IMyEntity.Parent => Parent;

		string VRage.ModAPI.IMyEntity.Name
		{
			get
			{
				return Name;
			}
			set
			{
				Name = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.DebugAsyncLoading => DebugAsyncLoading;

		string VRage.ModAPI.IMyEntity.DisplayName
		{
			get
			{
				return DisplayName;
			}
			set
			{
				DisplayName = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.MarkedForClose => MarkedForClose;

		bool VRage.ModAPI.IMyEntity.Closed => Closed;

		IMyModel VRage.ModAPI.IMyEntity.Model => Model;

		MyEntityComponentBase VRage.ModAPI.IMyEntity.GameLogic
		{
			get
			{
				return GameLogic;
			}
			set
			{
				GameLogic = (MyGameLogicComponent)value;
			}
		}

		MyEntityUpdateEnum VRage.ModAPI.IMyEntity.NeedsUpdate
		{
			get
			{
				return NeedsUpdate;
			}
			set
			{
				NeedsUpdate = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.NearFlag
		{
			get
			{
				return Render.NearFlag;
			}
			set
			{
				Render.NearFlag = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.CastShadows
		{
			get
			{
				return Render.CastShadows;
			}
			set
			{
				Render.CastShadows = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.FastCastShadowResolve
		{
			get
			{
				return Render.FastCastShadowResolve;
			}
			set
			{
				Render.FastCastShadowResolve = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.NeedsResolveCastShadow
		{
			get
			{
				return Render.NeedsResolveCastShadow;
			}
			set
			{
				Render.NeedsResolveCastShadow = value;
			}
		}

		float VRage.ModAPI.IMyEntity.MaxGlassDistSq => MaxGlassDistSq;

		bool VRage.ModAPI.IMyEntity.NeedsDraw
		{
			get
			{
				return Render.NeedsDraw;
			}
			set
			{
				Render.NeedsDraw = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.NeedsDrawFromParent
		{
			get
			{
				return Render.NeedsDrawFromParent;
			}
			set
			{
				Render.NeedsDrawFromParent = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.Transparent
		{
			get
			{
				return Render.Transparency != 0f;
			}
			set
			{
				Render.Transparency = (value ? 0.25f : 0f);
			}
		}

		bool VRage.ModAPI.IMyEntity.ShadowBoxLod
		{
			get
			{
				return Render.ShadowBoxLod;
			}
			set
			{
				Render.ShadowBoxLod = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.SkipIfTooSmall
		{
			get
			{
				return Render.SkipIfTooSmall;
			}
			set
			{
				Render.SkipIfTooSmall = value;
			}
		}

		MyModStorageComponentBase VRage.ModAPI.IMyEntity.Storage
		{
			get
			{
				return Storage;
			}
			set
			{
				Storage = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.Visible
		{
			get
			{
				return Render.Visible;
			}
			set
			{
				Render.Visible = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.Save
		{
			get
			{
				return Save;
			}
			set
			{
				Save = value;
			}
		}

		MyPersistentEntityFlags2 VRage.ModAPI.IMyEntity.PersistentFlags
		{
			get
			{
				return Render.PersistentFlags;
			}
			set
			{
				Render.PersistentFlags = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.InScene
		{
			get
			{
				return InScene;
			}
			set
			{
				InScene = value;
			}
		}

		bool VRage.ModAPI.IMyEntity.InvalidateOnMove => InvalidateOnMove;

		bool VRage.ModAPI.IMyEntity.IsCCDForProjectiles => IsCCDForProjectiles;

		bool VRage.ModAPI.IMyEntity.IsVolumetric => IsVolumetric;

		BoundingBox VRage.ModAPI.IMyEntity.LocalAABB
		{
			get
			{
				return PositionComp.LocalAABB;
			}
			set
			{
				PositionComp.LocalAABB = value;
			}
		}

		BoundingBox VRage.ModAPI.IMyEntity.LocalAABBHr => PositionComp.LocalAABB;

		Matrix VRage.ModAPI.IMyEntity.LocalMatrix
		{
			get
			{
				return PositionComp.LocalMatrix;
			}
			set
			{
				PositionComp.LocalMatrix = value;
			}
		}

		BoundingSphere VRage.ModAPI.IMyEntity.LocalVolume
		{
			get
			{
				return PositionComp.LocalVolume;
			}
			set
			{
				PositionComp.LocalVolume = value;
			}
		}

		Vector3 VRage.ModAPI.IMyEntity.LocalVolumeOffset
		{
			get
			{
				return PositionComp.LocalVolumeOffset;
			}
			set
			{
				PositionComp.LocalVolumeOffset = value;
			}
		}

		Vector3D VRage.ModAPI.IMyEntity.LocationForHudMarker => LocationForHudMarker;

		bool VRage.ModAPI.IMyEntity.Synchronized
		{
			get
			{
				return !IsPreview;
			}
			set
			{
				IsPreview = !value;
			}
		}

		MatrixD VRage.ModAPI.IMyEntity.WorldMatrix
		{
			get
			{
				return PositionComp.WorldMatrix;
			}
			set
			{
				PositionComp.WorldMatrix = value;
			}
		}

		MatrixD VRage.ModAPI.IMyEntity.WorldMatrixInvScaled => PositionComp.WorldMatrixInvScaled;

		MatrixD VRage.ModAPI.IMyEntity.WorldMatrixNormalizedInv => PositionComp.WorldMatrixNormalizedInv;

		bool VRage.Game.ModAPI.Ingame.IMyEntity.HasInventory => HasInventory;

		int VRage.Game.ModAPI.Ingame.IMyEntity.InventoryCount => InventoryCount;

		string VRage.Game.ModAPI.Ingame.IMyEntity.DisplayName => DisplayName;

		string VRage.Game.ModAPI.Ingame.IMyEntity.Name => Name;

		BoundingBoxD VRage.Game.ModAPI.Ingame.IMyEntity.WorldAABB => PositionComp.WorldAABB;

		BoundingBoxD VRage.Game.ModAPI.Ingame.IMyEntity.WorldAABBHr => PositionComp.WorldAABB;

		MatrixD VRage.Game.ModAPI.Ingame.IMyEntity.WorldMatrix => PositionComp.WorldMatrix;

		BoundingSphereD VRage.Game.ModAPI.Ingame.IMyEntity.WorldVolume => PositionComp.WorldVolume;

		BoundingSphereD VRage.Game.ModAPI.Ingame.IMyEntity.WorldVolumeHr => PositionComp.WorldVolume;

		/// <summary>
		/// This event may not be invoked at all, when calling MyEntities.CloseAll, marking is bypassed
		/// </summary>
		public event Action<MyEntity> OnMarkForClose;

		public event Action<MyEntity> OnClose;

		public event Action<MyEntity> OnClosing;

		public event Action<MyEntity> OnPhysicsChanged;

		public event Action<MyPhysicsComponentBase, MyPhysicsComponentBase> OnPhysicsComponentChanged;

		public event Action<MyEntity> AddedToScene;

		public event Action<MyEntity> OnTeleport;

		event Action<VRage.ModAPI.IMyEntity> VRage.ModAPI.IMyEntity.OnClose
		{
			add
			{
				OnClose += GetDelegate(value);
			}
			remove
			{
				OnClose -= GetDelegate(value);
			}
		}

		event Action<VRage.ModAPI.IMyEntity> VRage.ModAPI.IMyEntity.OnClosing
		{
			add
			{
				OnClosing += GetDelegate(value);
			}
			remove
			{
				OnClosing -= GetDelegate(value);
			}
		}

		event Action<VRage.ModAPI.IMyEntity> VRage.ModAPI.IMyEntity.OnMarkForClose
		{
			add
			{
				OnMarkForClose += GetDelegate(value);
			}
			remove
			{
				OnMarkForClose -= GetDelegate(value);
			}
		}

		event Action<VRage.ModAPI.IMyEntity> VRage.ModAPI.IMyEntity.OnPhysicsChanged
		{
			add
			{
				OnPhysicsChanged += GetDelegate(value);
			}
			remove
			{
				OnPhysicsChanged -= GetDelegate(value);
			}
		}

		public void DebugDraw()
		{
			if (Hierarchy != null)
			{
				foreach (MyHierarchyComponentBase child in Hierarchy.Children)
				{
					child.Container.Entity.DebugDraw();
				}
			}
			foreach (MyDebugRenderComponentBase debugRenderer in m_debugRenderers)
			{
				debugRenderer.DebugDraw();
			}
		}

		public void DebugDrawInvalidTriangles()
		{
			foreach (MyDebugRenderComponentBase debugRenderer in m_debugRenderers)
			{
				debugRenderer.DebugDrawInvalidTriangles();
			}
		}

		public void AddDebugRenderComponent(MyDebugRenderComponentBase render)
		{
			m_debugRenderers.Add(render);
		}

		public bool ContainsDebugRenderComponent(Type render)
		{
			foreach (MyDebugRenderComponentBase debugRenderer in m_debugRenderers)
			{
				if (debugRenderer.GetType() == render)
				{
					return true;
				}
			}
			return false;
		}

		public void RemoveDebugRenderComponent(Type t)
		{
			int num = m_debugRenderers.Count;
			while (num > 0)
			{
				num--;
				if (m_debugRenderers[num].GetType() == t)
				{
					m_debugRenderers.RemoveAt(num);
				}
			}
		}

		public void RemoveDebugRenderComponent(MyDebugRenderComponentBase render)
		{
			m_debugRenderers.Remove(render);
		}

		public void ClearDebugRenderComponents()
		{
			m_debugRenderers.Clear();
		}

		/// <summary>
		/// Return top most parent of this entity
		/// </summary>
		/// <returns></returns>
		public MyEntity GetTopMostParent(Type type = null)
		{
			MyEntity myEntity = this;
			while (myEntity.Parent != null && (type == null || !myEntity.GetType().IsSubclassOf(type)))
			{
				myEntity = myEntity.Parent;
			}
			return myEntity;
		}

		public virtual List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			return m_hudParams;
		}

		protected virtual bool CanBeAddedToRender()
		{
			return true;
		}

		public MyEntity()
		{
			Components = new MyEntityComponentContainer(this);
			Components.ComponentAdded += Components_ComponentAdded;
			Components.ComponentRemoved += Components_ComponentRemoved;
			Flags = EntityFlags.Default;
			InitComponents();
		}

		public virtual void InitComponents()
		{
			if (Hierarchy == null)
			{
				Hierarchy = new MyHierarchyComponent<MyEntity>();
			}
			if (GameLogic == null)
			{
				GameLogic = new MyNullGameLogicComponent();
			}
			if (PositionComp == null)
			{
				PositionComp = new MyPositionComponent();
			}
			PositionComp.LocalMatrix = Matrix.Identity;
			if (Render == null)
			{
				CreateStandardRenderComponentsExtCallback(this);
			}
		}

		private void Components_ComponentAdded(Type t, MyEntityComponentBase c)
		{
			if (typeof(MyPhysicsComponentBase).IsAssignableFrom(t))
			{
				m_physics = (c as MyPhysicsComponentBase);
			}
			else if (typeof(MySyncComponentBase).IsAssignableFrom(t))
			{
				m_syncObject = (c as MySyncComponentBase);
			}
			else if (typeof(MyGameLogicComponent).IsAssignableFrom(t))
			{
				m_gameLogic = (c as MyGameLogicComponent);
			}
			else if (typeof(MyPositionComponentBase).IsAssignableFrom(t))
			{
				m_position = (c as MyPositionComponentBase);
				if (m_position == null)
				{
					PositionComp = new MyNullPositionComponent();
				}
			}
			else if (typeof(MyHierarchyComponentBase).IsAssignableFrom(t))
			{
				m_hierarchy = (c as MyHierarchyComponent<MyEntity>);
			}
			else if (typeof(MyRenderComponentBase).IsAssignableFrom(t))
			{
				m_render = (c as MyRenderComponentBase);
				if (m_render == null)
				{
					Render = new MyNullRenderComponent();
				}
			}
			else if (typeof(MyInventoryBase).IsAssignableFrom(t))
			{
				OnInventoryComponentAdded(c as MyInventoryBase);
			}
			else if (typeof(MyModStorageComponentBase).IsAssignableFrom(t))
			{
				m_storage = (c as MyModStorageComponentBase);
			}
		}

		private void Components_ComponentRemoved(Type t, MyEntityComponentBase c)
		{
			if (typeof(MyPhysicsComponentBase).IsAssignableFrom(t))
			{
				m_physics = null;
			}
			else if (typeof(MySyncComponentBase).IsAssignableFrom(t))
			{
				m_syncObject = null;
			}
			else if (typeof(MyGameLogicComponent).IsAssignableFrom(t))
			{
				m_gameLogic = null;
			}
			else if (typeof(MyPositionComponentBase).IsAssignableFrom(t))
			{
				PositionComp = new MyNullPositionComponent();
			}
			else if (typeof(MyHierarchyComponentBase).IsAssignableFrom(t))
			{
				m_hierarchy = null;
			}
			else if (typeof(MyRenderComponentBase).IsAssignableFrom(t))
			{
				Render = new MyNullRenderComponent();
			}
			else if (typeof(MyInventoryBase).IsAssignableFrom(t))
			{
				OnInventoryComponentRemoved(c as MyInventoryBase);
			}
			else if (typeof(MyModStorageComponentBase).IsAssignableFrom(t))
			{
				m_storage = null;
			}
		}

		protected virtual MySyncComponentBase OnCreateSync()
		{
			return CreateDefaultSyncEntityExtCallback(this);
		}

		public void CreateSync()
		{
			SyncObject = OnCreateSync();
		}

		public MyEntitySubpart GetSubpart(string name)
		{
			return Subparts[name];
		}

		public bool TryGetSubpart(string name, out MyEntitySubpart subpart)
		{
			return Subparts.TryGetValue(name, out subpart);
		}

		public virtual void UpdateOnceBeforeFrame()
		{
			((IMyGameLogicComponent)m_gameLogic).UpdateOnceBeforeFrame(entityUpdate: true);
		}

		public virtual void UpdateBeforeSimulation()
		{
			((IMyGameLogicComponent)m_gameLogic).UpdateBeforeSimulation(entityUpdate: true);
		}

		public virtual void Simulate()
		{
		}

		public virtual void UpdateAfterSimulation()
		{
			((IMyGameLogicComponent)m_gameLogic).UpdateAfterSimulation(entityUpdate: true);
		}

		public virtual void UpdatingStopped()
		{
		}

		/// <summary>
		/// Called each 10th frame if registered for update10
		/// </summary>
		public virtual void UpdateBeforeSimulation10()
		{
			((IMyGameLogicComponent)m_gameLogic).UpdateBeforeSimulation10(entityUpdate: true);
		}

		public virtual void UpdateAfterSimulation10()
		{
			((IMyGameLogicComponent)m_gameLogic).UpdateAfterSimulation10(entityUpdate: true);
		}

		/// <summary>
		/// Called each 100th frame if registered for update100
		/// </summary>
		public virtual void UpdateBeforeSimulation100()
		{
			((IMyGameLogicComponent)m_gameLogic).UpdateBeforeSimulation100(entityUpdate: true);
		}

		public virtual void UpdateAfterSimulation100()
		{
			((IMyGameLogicComponent)m_gameLogic).UpdateAfterSimulation100(entityUpdate: true);
		}

		public virtual string GetFriendlyName()
		{
			return string.Empty;
		}

		public virtual MatrixD GetViewMatrix()
		{
			return PositionComp.WorldMatrixNormalizedInv;
		}

		public virtual void Teleport(MatrixD worldMatrix, object source = null, bool ignoreAssert = false)
		{
			if (!Closed && Hierarchy != null)
			{
				HashSet<VRage.ModAPI.IMyEntity> hashSet = new HashSet<VRage.ModAPI.IMyEntity>();
				HashSet<VRage.ModAPI.IMyEntity> hashSet2 = new HashSet<VRage.ModAPI.IMyEntity>();
				hashSet.Add(this);
				Hierarchy.GetChildrenRecursive(hashSet);
				foreach (VRage.ModAPI.IMyEntity item in hashSet)
				{
					if (item.Physics != null)
					{
						if (item.Physics.Enabled)
						{
							item.Physics.Enabled = false;
						}
						else
						{
							hashSet2.Add(item);
						}
					}
				}
				PositionComp.SetWorldMatrix(worldMatrix, source, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true, forceUpdateAllChildren: false, ignoreAssert);
				foreach (VRage.ModAPI.IMyEntity item2 in hashSet.Reverse())
				{
					if (item2.Physics != null && !hashSet2.Contains(item2))
					{
						item2.Physics.Enabled = true;
					}
				}
				if (this.OnTeleport != null)
				{
					this.OnTeleport(this);
				}
			}
		}

		/// <summary>
		/// Draw physical representation of entity
		/// </summary>
		public virtual void DebugDrawPhysics()
		{
			foreach (MyHierarchyComponentBase child in Hierarchy.Children)
			{
				(child.Container.Entity as MyEntity).DebugDrawPhysics();
			}
			if (m_physics != null && !(GetDistanceBetweenCameraAndBoundingSphere() > 200.0))
			{
				m_physics.DebugDraw();
			}
		}

		public virtual bool GetIntersectionWithLine(ref LineD line, out Vector3D? v, bool useCollisionModel = true, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			v = null;
			MyModel myModel = Model;
			if (useCollisionModel)
			{
				myModel = ModelCollision;
			}
			if (myModel != null)
			{
				MyIntersectionResultLineTriangleEx? intersectionWithLine = myModel.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, flags);
				if (intersectionWithLine.HasValue)
				{
					v = intersectionWithLine.Value.IntersectionPointInWorldSpace;
					return true;
				}
			}
			return false;
		}

		public virtual bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? t, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			bool result = false;
			t = null;
			MyModel model = Model;
			if (model != null)
			{
				MyIntersectionResultLineTriangleEx? intersectionWithLine = model.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, flags);
				if (intersectionWithLine.HasValue)
				{
					t = intersectionWithLine.Value;
					result = true;
				}
			}
			return result;
		}

		internal virtual bool GetIntersectionsWithLine(ref LineD line, List<MyIntersectionResultLineTriangleEx> result, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			Model?.GetTrianglePruningStructure().GetTrianglesIntersectingLine(this, ref line, flags, result);
			return result.Count > 0;
		}

		public virtual Vector3D? GetIntersectionWithLineAndBoundingSphere(ref LineD line, float boundingSphereRadiusMultiplier)
		{
			if (Render.GetModel() == null)
			{
				return null;
			}
			BoundingSphereD boundingSphere = PositionComp.WorldVolume;
			boundingSphere.Radius *= boundingSphereRadiusMultiplier;
			if (!MyUtils.IsLineIntersectingBoundingSphere(ref line, ref boundingSphere))
			{
				return null;
			}
			return boundingSphere.Center;
		}

		public virtual bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			return Model?.GetTrianglePruningStructure().GetIntersectionWithSphere(this, ref sphere) ?? false;
		}

		public virtual bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
		{
			return Model?.GetTrianglePruningStructure().GetIntersectionWithAABB(this, ref aabb) ?? false;
		}

		public void GetTrianglesIntersectingSphere(ref BoundingSphere sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles)
		{
			Model?.GetTrianglePruningStructure().GetTrianglesIntersectingSphere(ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
		}

		public virtual bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos)
		{
			return false;
		}

		public double GetSmallestDistanceBetweenCameraAndBoundingSphere()
		{
			Vector3D from = MyAPIGatewayShortcuts.GetMainCamera().Position;
			BoundingSphereD sphere = PositionComp.WorldVolume;
			return MyUtils.GetSmallestDistanceToSphereAlwaysPositive(ref from, ref sphere);
		}

		public double GetLargestDistanceBetweenCameraAndBoundingSphere()
		{
			Vector3D from = MyAPIGatewayShortcuts.GetMainCamera().Position;
			BoundingSphereD sphere = PositionComp.WorldVolume;
			return MyUtils.GetLargestDistanceToSphere(ref from, ref sphere);
		}

		public double GetDistanceBetweenCameraAndBoundingSphere()
		{
			Vector3D from = MyAPIGatewayShortcuts.GetMainCamera().Position;
			BoundingSphereD sphere = PositionComp.WorldVolume;
			return MyUtils.GetSmallestDistanceToSphereAlwaysPositive(ref from, ref sphere);
		}

		public double GetDistanceBetweenPlayerPositionAndBoundingSphere()
		{
			Vector3D from = MyAPIGatewayShortcuts.GetLocalPlayerPosition();
			BoundingSphereD sphere = PositionComp.WorldVolume;
			return MyUtils.GetSmallestDistanceToSphereAlwaysPositive(ref from, ref sphere);
		}

		public double GetDistanceBetweenCameraAndPosition()
		{
			return Vector3D.Distance(MyAPIGatewayShortcuts.GetMainCamera().Position, PositionComp.GetPosition());
		}

		public virtual MyEntity GetBaseEntity()
		{
			return this;
		}

		/// <summary>
		/// Called when [activated] which for entity means that was added to scene.
		/// </summary>
		/// <param name="source">The source of activation.</param>
		public virtual void OnAddedToScene(object source)
		{
			if (!IsPreview)
			{
				SetReadyForReplication();
			}
			InScene = true;
			if (NeedsUpdate != 0)
			{
				MyEntitiesInterface.RegisterUpdate(this);
			}
			if (GameLogic != null)
			{
				((IMyGameLogicComponent)GameLogic).RegisterForUpdate();
			}
			if (Render.NeedsDraw)
			{
				MyEntitiesInterface.RegisterDraw(this);
			}
			if (m_physics != null)
			{
				m_physics.Activate();
			}
			AddToGamePruningStructure();
			Components.OnAddedToScene();
			if (Hierarchy != null)
			{
				foreach (MyHierarchyComponentBase child in Hierarchy.Children)
				{
					if (!child.Container.Entity.InScene)
					{
						child.Container.Entity.OnAddedToScene(source);
					}
				}
			}
			if ((Flags & EntityFlags.UpdateRender) > (EntityFlags)0)
			{
				Render.UpdateRenderObject(visible: true, updateChildren: false);
			}
			MyProceduralWorldGeneratorTrackEntityExtCallback(this);
			this.AddedToScene.InvokeIfNotNull(this);
		}

		private void SetReadyForReplication()
		{
			IsReadyForReplication = true;
			if (Hierarchy != null)
			{
				foreach (MyHierarchyComponentBase child in Hierarchy.Children)
				{
					((MyEntity)child.Entity).SetReadyForReplication();
				}
			}
		}

		public void SetFadeOut(bool state)
		{
			Render.FadeOut = state;
			if (Hierarchy != null)
			{
				foreach (MyHierarchyComponentBase child in Hierarchy.Children)
				{
					child.Container.Entity.Render.FadeOut = state;
				}
			}
		}

		public virtual void OnRemovedFromScene(object source)
		{
			InScene = false;
			if (Hierarchy != null)
			{
				foreach (MyHierarchyComponentBase child in Hierarchy.Children)
				{
					child.Container.Entity.OnRemovedFromScene(source);
				}
			}
			Components.OnRemovedFromScene();
			MyEntitiesInterface.UnregisterUpdate(this, arg2: false);
			MyEntitiesInterface.UnregisterDraw(this);
			if (GameLogic != null)
			{
				((IMyGameLogicComponent)GameLogic).UnregisterForUpdate();
			}
			if (m_physics != null && m_physics.Enabled)
			{
				m_physics.Deactivate();
			}
			if (Parent != null)
			{
				Render.FadeOut = Parent.Render.FadeOut;
			}
			Render.RemoveRenderObjects();
			RemoveFromGamePruningStructureExtCallBack(this);
		}

		public void AddToGamePruningStructure()
		{
			if (UsePrunning())
			{
				AddToGamePruningStructureExtCallBack(this);
			}
		}

		public void RemoveFromGamePruningStructure()
		{
			if (UsePrunning())
			{
				RemoveFromGamePruningStructureExtCallBack(this);
			}
		}

		private bool UsePrunning()
		{
			EntityFlags entityFlags = (Parent == null) ? EntityFlags.IsNotGamePrunningStructureObject : EntityFlags.IsGamePrunningStructureObject;
			if (InScene)
			{
				return (Flags & entityFlags) == 0;
			}
			return false;
		}

		public void UpdateGamePruningStructure()
		{
			if (UsePrunning())
			{
				UpdateGamePruningStructureExtCallBack(this);
			}
		}

		public void RaisePhysicsChanged()
		{
			if (!m_raisePhysicsCalled)
			{
				m_raisePhysicsCalled = true;
				if (!InScene)
				{
					this.OnPhysicsChanged?.Invoke(this);
				}
				else
				{
					MyWeldingGroupsGetGroupNodesExtCallback(this, m_tmpOnPhysicsChanged);
					foreach (MyEntity item in m_tmpOnPhysicsChanged)
					{
						item.OnPhysicsChanged?.Invoke(item);
					}
					m_tmpOnPhysicsChanged.Clear();
				}
				m_raisePhysicsCalled = false;
			}
		}

		public virtual void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			MarkedForClose = false;
			Closed = false;
			Render.PersistentFlags = MyPersistentEntityFlags2.CastShadows;
			if (objectBuilder != null)
			{
				if (objectBuilder.EntityId != 0L)
				{
					EntityId = objectBuilder.EntityId;
				}
				else
				{
					AllocateEntityID();
				}
				DefinitionId = objectBuilder.GetId();
				if (objectBuilder.EntityDefinitionId.HasValue)
				{
					DefinitionId = objectBuilder.EntityDefinitionId.Value;
				}
				if (objectBuilder.PositionAndOrientation.HasValue)
				{
					MyPositionAndOrientation value = objectBuilder.PositionAndOrientation.Value;
					if (!value.Position.x.IsValid())
					{
						value.Position.x = 0.0;
					}
					if (!value.Position.y.IsValid())
					{
						value.Position.y = 0.0;
					}
					if (!value.Position.z.IsValid())
					{
						value.Position.z = 0.0;
					}
					MatrixD worldMatrix = MatrixD.CreateWorld(value.Position, value.Forward, value.Up);
					if (!worldMatrix.IsValid())
					{
						worldMatrix = MatrixD.Identity;
					}
					PositionComp.SetWorldMatrix(worldMatrix, null, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: false, forceUpdateAllChildren: false, ignoreAssert: true);
					ClampToWorld();
				}
				Name = objectBuilder.Name;
				Render.PersistentFlags = (objectBuilder.PersistentFlags & ~MyPersistentEntityFlags2.InScene);
				InitComponentsExtCallback(Components, DefinitionId.Value.TypeId, DefinitionId.Value.SubtypeId, objectBuilder.ComponentContainer);
			}
			else
			{
				AllocateEntityID();
			}
			MyEntitiesInterface.SetEntityName(this, arg2: false);
			if (SyncFlag)
			{
				CreateSync();
			}
			GameLogic.Init(objectBuilder);
		}

		protected virtual void ClampToWorld()
		{
			Vector3D position = PositionComp.GetPosition();
			float num = 10f;
			BoundingBoxD boundingBoxD = (MyAPIGatewayShortcuts.GetWorldBoundaries != null) ? MyAPIGatewayShortcuts.GetWorldBoundaries() : default(BoundingBoxD);
			if (boundingBoxD.Max.X > boundingBoxD.Min.X && boundingBoxD.Max.Y > boundingBoxD.Min.Y && boundingBoxD.Max.Z > boundingBoxD.Min.Z)
			{
				if (position.X > boundingBoxD.Max.X)
				{
					position.X = boundingBoxD.Max.X - (double)num;
				}
				else if (position.X < boundingBoxD.Min.X)
				{
					position.X = boundingBoxD.Min.X + (double)num;
				}
				if (position.Y > boundingBoxD.Max.Y)
				{
					position.Y = boundingBoxD.Max.Y - (double)num;
				}
				else if (position.Y < boundingBoxD.Min.Y)
				{
					position.Y = boundingBoxD.Min.Y + (double)num;
				}
				if (position.Z > boundingBoxD.Max.Z)
				{
					position.Z = boundingBoxD.Max.Z - (double)num;
				}
				else if (position.Z < boundingBoxD.Min.Z)
				{
					position.Z = boundingBoxD.Min.Z + (double)num;
				}
				PositionComp.SetPosition(position);
			}
		}

		private void AllocateEntityID()
		{
			if (EntityId == 0L && !MyEntityIdentifier.AllocationSuspended)
			{
				EntityId = MyEntityIdentifier.AllocateId();
			}
		}

		public virtual void Init(StringBuilder displayName, string model, MyEntity parentObject, float? scale, string modelCollision = null)
		{
			MarkedForClose = false;
			Closed = false;
			Render.PersistentFlags = MyPersistentEntityFlags2.CastShadows;
			DisplayName = displayName?.ToString();
			RefreshModels(model, modelCollision);
			parentObject?.Hierarchy.AddChild(this, preserveWorldPos: false, insertIntoSceneIfNeeded: false);
			if (!PositionComp.Scale.HasValue)
			{
				PositionComp.Scale = scale;
			}
			AllocateEntityID();
		}

		public virtual void RefreshModels(string model, string modelCollision)
		{
			float valueOrDefault = PositionComp.Scale.GetValueOrDefault(1f);
			if (model != null)
			{
				Render.ModelStorage = MyModels.GetModelOnlyData(model);
				MyModel model2 = Render.GetModel();
				PositionComp.LocalVolumeOffset = ((model2 == null) ? Vector3.Zero : (model2.BoundingSphere.Center * valueOrDefault));
			}
			if (modelCollision != null)
			{
				m_modelCollision = MyModels.GetModelOnlyData(modelCollision);
			}
			if (Render.ModelStorage != null)
			{
				BoundingBox boundingBox = Render.GetModel().BoundingBox;
				boundingBox.Min *= valueOrDefault;
				boundingBox.Max *= valueOrDefault;
				PositionComp.LocalAABB = boundingBox;
				bool allocationSuspended = MyEntityIdentifier.AllocationSuspended;
				try
				{
					MyEntityIdentifier.AllocationSuspended = false;
					if (Subparts == null)
					{
						Subparts = new Dictionary<string, MyEntitySubpart>();
					}
					else
					{
						foreach (KeyValuePair<string, MyEntitySubpart> subpart in Subparts)
						{
							Hierarchy.RemoveChild(subpart.Value);
							subpart.Value.Close();
						}
						Subparts.Clear();
					}
					MyEntitySubpart.Data outData = default(MyEntitySubpart.Data);
					foreach (KeyValuePair<string, MyModelDummy> dummy in Render.GetModel().Dummies)
					{
						if (MyEntitySubpart.GetSubpartFromDummy(model, dummy.Key, dummy.Value, ref outData))
						{
							MyEntitySubpart myEntitySubpart = InstantiateSubpart(dummy.Value, ref outData);
							myEntitySubpart.Render.EnableColorMaskHsv = Render.EnableColorMaskHsv;
							myEntitySubpart.Render.ColorMaskHsv = Render.ColorMaskHsv;
							myEntitySubpart.Render.TextureChanges = Render.TextureChanges;
							myEntitySubpart.Render.MetalnessColorable = Render.MetalnessColorable;
							MyModel modelOnlyData = MyModels.GetModelOnlyData(outData.File);
							if (modelOnlyData != null && Model != null)
							{
								modelOnlyData.Rescale(Model.ScaleFactor);
							}
							myEntitySubpart.Init(null, outData.File, this, PositionComp.Scale);
							myEntitySubpart.Render.NeedsDrawFromParent = false;
							myEntitySubpart.Render.PersistentFlags = (Render.PersistentFlags & ~MyPersistentEntityFlags2.InScene);
							myEntitySubpart.PositionComp.LocalMatrix = outData.InitialTransform;
							Subparts[outData.Name] = myEntitySubpart;
							if (InScene)
							{
								myEntitySubpart.OnAddedToScene(this);
							}
						}
					}
				}
				finally
				{
					MyEntityIdentifier.AllocationSuspended = allocationSuspended;
				}
			}
			else
			{
				float num = 0.5f;
				PositionComp.LocalAABB = new BoundingBox(new Vector3(0f - num), new Vector3(num));
			}
		}

		/// <summary>
		/// Every object must have this method, but not every phys object must necessarily have something to cleanup
		/// <remarks>
		/// </remarks>
		/// </summary>
		public void Delete()
		{
			if (Closed)
			{
				return;
			}
			Render.RemoveRenderObjects();
			Close();
			BeforeDelete();
			if (GameLogic != null)
			{
				((IMyGameLogicComponent)GameLogic).Close();
			}
			MyHierarchyComponent<MyEntity> hierarchy = Hierarchy;
			hierarchy?.Delete();
			CallAndClearOnClosing();
			MyEntitiesInterface.RemoveName(this);
			MyEntitiesInterface.RemoveFromClosedEntities(this);
			if (m_physics != null)
			{
				m_physics.Close();
				Physics = null;
				RaisePhysicsChanged();
			}
			MyEntitiesInterface.UnregisterUpdate(this, arg2: true);
			MyEntitiesInterface.UnregisterDraw(this);
			MyEntity parent = Parent;
			if (parent == null)
			{
				MyEntitiesInterface.Remove(this);
			}
			else
			{
				parent.Hierarchy.RemoveByJN(hierarchy);
				if (parent.InScene)
				{
					OnRemovedFromScene(this);
					MyEntitiesInterface.RaiseEntityRemove(this);
				}
			}
			if (EntityId != 0L && MyEntityIdentifier.GetEntityById(EntityId, allowClosed: true) == this)
			{
				MyEntityIdentifier.RemoveEntity(EntityId);
			}
			CallAndClearOnClose();
			ClearDebugRenderComponents();
			Components.Clear();
			Closed = true;
		}

		protected virtual void BeforeDelete()
		{
		}

		protected virtual void Closing()
		{
		}

		/// <summary>
		/// This method marks this entity for close which means, that Close
		/// will be called after all entities are updated
		/// </summary>
		public void Close()
		{
			if (!MarkedForClose)
			{
				MarkedForClose = true;
				Closing();
				MyEntitiesInterface.Close(this);
				GameLogic.MarkForClose();
				this.OnMarkForClose.InvokeIfNotNull(this);
			}
		}

		private void CallAndClearOnClose()
		{
			this.OnClose.InvokeIfNotNull(this);
			this.OnClose = null;
		}

		private void CallAndClearOnClosing()
		{
			this.OnClosing.InvokeIfNotNull(this);
			this.OnClosing = null;
		}

		/// <summary>
		/// Gets object builder from object.
		/// </summary>
		/// <returns></returns>
		public virtual MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyEntityFactoryCreateObjectBuilderExtCallback(this);
			if (myObjectBuilder_EntityBase != null)
			{
				myObjectBuilder_EntityBase.PositionAndOrientation = new MyPositionAndOrientation
				{
					Position = PositionComp.GetPosition(),
					Up = (Vector3)WorldMatrix.Up,
					Forward = (Vector3)WorldMatrix.Forward
				};
				myObjectBuilder_EntityBase.EntityId = EntityId;
				myObjectBuilder_EntityBase.Name = Name;
				myObjectBuilder_EntityBase.PersistentFlags = Render.PersistentFlags;
				myObjectBuilder_EntityBase.ComponentContainer = Components.Serialize(copy);
				if (DefinitionId.HasValue)
				{
					myObjectBuilder_EntityBase.SubtypeName = DefinitionId.Value.SubtypeName;
				}
			}
			return myObjectBuilder_EntityBase;
		}

		/// <summary>
		/// Called before method GetObjectBuilder, when saving sector
		/// </summary>
		public virtual void BeforeSave()
		{
		}

		/// <summary>
		/// Method is called defacto from Update, preparation fo Draw
		/// </summary>
		public virtual void PrepareForDraw()
		{
			foreach (MyDebugRenderComponentBase debugRenderer in m_debugRenderers)
			{
				debugRenderer.PrepareForDraw();
			}
		}

		public virtual void BeforePaste()
		{
		}

		public virtual void AfterPaste()
		{
		}

		public void SetEmissiveParts(string emissiveName, Color emissivePartColor, float emissivity)
		{
			UpdateNamedEmissiveParts(Render.RenderObjectIDs[0], emissiveName, emissivePartColor, emissivity);
		}

		public void SetEmissivePartsForSubparts(string emissiveName, Color emissivePartColor, float emissivity)
		{
			if (Subparts != null)
			{
				foreach (KeyValuePair<string, MyEntitySubpart> subpart in Subparts)
				{
					subpart.Value.SetEmissiveParts(emissiveName, emissivePartColor, emissivity);
				}
			}
		}

		protected static void UpdateNamedEmissiveParts(uint renderObjectId, string emissiveName, Color emissivePartColor, float emissivity)
		{
			if (renderObjectId != uint.MaxValue)
			{
				MyRenderProxy.UpdateColorEmissivity(renderObjectId, 0, emissiveName, emissivePartColor, emissivity);
			}
		}

		protected virtual MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			return new MyEntitySubpart();
		}

		public override string ToString()
		{
			return GetType().Name + " {" + EntityId.ToString("X8") + "}";
		}

		/// <summary>
		/// Search for inventory component with maching index.
		/// </summary>
		public virtual MyInventoryBase GetInventoryBase(int index)
		{
			MyInventoryBase component = null;
			if (!Components.TryGet(out component))
			{
				return null;
			}
			return component.IterateInventory(index);
		}

		/// <summary>
		/// Simply get the MyInventoryBase component stored in this entity.
		/// </summary>
		/// <returns></returns>
		public MyInventoryBase GetInventoryBase()
		{
			MyInventoryBase component = null;
			Components.TryGet(out component);
			return component;
		}

		protected virtual void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
		}

		protected virtual void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
		}

		public virtual void SerializeControls(BitStream stream)
		{
			stream.WriteBool(value: false);
		}

		public virtual void DeserializeControls(BitStream stream, bool outOfOrder)
		{
			stream.ReadBool();
		}

		public virtual void ApplyLastControls()
		{
		}

		public virtual void ResetControls()
		{
		}

		public EntityPin Pin()
		{
			return new EntityPin(this);
		}

		public void Unpin()
		{
			Interlocked.Decrement(ref m_pins);
		}

		public void UpdateSoundContactPoint(long entityId, Vector3 localPosition, Vector3 normal, Vector3 separatingVelocity, float separatingSpeed)
		{
			ContactPointData contactPointData = default(ContactPointData);
			contactPointData.EntityId = entityId;
			contactPointData.LocalPosition = localPosition;
			contactPointData.Normal = normal;
			contactPointData.ContactPointType = ContactPointData.ContactPointDataTypes.Sounds;
			contactPointData.SeparatingVelocity = separatingVelocity;
			contactPointData.SeparatingSpeed = separatingSpeed;
			ContactPointData localValue = contactPointData;
			m_contactPoint.SetLocalValue(localValue);
		}

		VRage.ModAPI.IMyEntity VRage.ModAPI.IMyEntity.GetTopMostParent(Type type)
		{
			return GetTopMostParent(type);
		}

		void VRage.ModAPI.IMyEntity.GetChildren(List<VRage.ModAPI.IMyEntity> children, Func<VRage.ModAPI.IMyEntity, bool> collect)
		{
			foreach (VRage.ModAPI.IMyEntity child in children)
			{
				if (collect == null || collect(child))
				{
					children.Add(child);
				}
			}
		}

		private Action<MyEntity> GetDelegate(Action<VRage.ModAPI.IMyEntity> value)
		{
			return (Action<MyEntity>)Delegate.CreateDelegate(typeof(Action<MyEntity>), value.Target, value.Method);
		}

		string VRage.ModAPI.IMyEntity.GetFriendlyName()
		{
			return GetFriendlyName();
		}

		void VRage.ModAPI.IMyEntity.Close()
		{
			Close();
		}

		void VRage.ModAPI.IMyEntity.Delete()
		{
			Delete();
		}

		Vector3 VRage.ModAPI.IMyEntity.GetDiffuseColor()
		{
			return Render.GetDiffuseColor();
		}

		float VRage.ModAPI.IMyEntity.GetDistanceBetweenCameraAndBoundingSphere()
		{
			return (float)GetDistanceBetweenCameraAndBoundingSphere();
		}

		float VRage.ModAPI.IMyEntity.GetDistanceBetweenCameraAndPosition()
		{
			return (float)GetDistanceBetweenCameraAndPosition();
		}

		float VRage.ModAPI.IMyEntity.GetLargestDistanceBetweenCameraAndBoundingSphere()
		{
			return (float)GetLargestDistanceBetweenCameraAndBoundingSphere();
		}

		float VRage.ModAPI.IMyEntity.GetSmallestDistanceBetweenCameraAndBoundingSphere()
		{
			return (float)GetSmallestDistanceBetweenCameraAndBoundingSphere();
		}

		Vector3D? VRage.ModAPI.IMyEntity.GetIntersectionWithLineAndBoundingSphere(ref LineD line, float boundingSphereRadiusMultiplier)
		{
			return GetIntersectionWithLineAndBoundingSphere(ref line, boundingSphereRadiusMultiplier);
		}

		bool VRage.ModAPI.IMyEntity.GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			return GetIntersectionWithSphere(ref sphere);
		}

		VRage.Game.ModAPI.IMyInventory VRage.ModAPI.IMyEntity.GetInventory()
		{
			return GetInventoryBase(0) as VRage.Game.ModAPI.IMyInventory;
		}

		VRage.Game.ModAPI.IMyInventory VRage.ModAPI.IMyEntity.GetInventory(int index)
		{
			return GetInventoryBase(index) as VRage.Game.ModAPI.IMyInventory;
		}

		void VRage.ModAPI.IMyEntity.GetTrianglesIntersectingSphere(ref BoundingSphere sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles)
		{
			GetTrianglesIntersectingSphere(ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
		}

		bool VRage.ModAPI.IMyEntity.DoOverlapSphereTest(float sphereRadius, Vector3D spherePos)
		{
			return DoOverlapSphereTest(sphereRadius, spherePos);
		}

		MyObjectBuilder_EntityBase VRage.ModAPI.IMyEntity.GetObjectBuilder(bool copy)
		{
			return GetObjectBuilder(copy);
		}

		bool VRage.ModAPI.IMyEntity.IsVisible()
		{
			return Render.IsVisible();
		}

		MatrixD VRage.ModAPI.IMyEntity.GetViewMatrix()
		{
			return GetViewMatrix();
		}

		MatrixD VRage.ModAPI.IMyEntity.GetWorldMatrixNormalizedInv()
		{
			return PositionComp.WorldMatrixNormalizedInv;
		}

		void VRage.ModAPI.IMyEntity.SetLocalMatrix(Matrix localMatrix, object source)
		{
			PositionComp.SetLocalMatrix(ref localMatrix, source, updateWorld: true);
		}

		void VRage.ModAPI.IMyEntity.SetWorldMatrix(MatrixD worldMatrix, object source)
		{
			PositionComp.SetWorldMatrix(worldMatrix, source);
		}

		void VRage.ModAPI.IMyEntity.SetPosition(Vector3D pos)
		{
			PositionComp.SetPosition(pos);
		}

		void VRage.ModAPI.IMyEntity.EnableColorMaskForSubparts(bool value)
		{
			if (Subparts != null)
			{
				foreach (KeyValuePair<string, MyEntitySubpart> subpart in Subparts)
				{
					subpart.Value.Render.EnableColorMaskHsv = value;
				}
			}
		}

		void VRage.ModAPI.IMyEntity.SetColorMaskForSubparts(Vector3 colorMaskHsv)
		{
			if (Subparts != null)
			{
				foreach (KeyValuePair<string, MyEntitySubpart> subpart in Subparts)
				{
					subpart.Value.Render.ColorMaskHsv = colorMaskHsv;
				}
			}
		}

		void VRage.ModAPI.IMyEntity.SetTextureChangesForSubparts(Dictionary<string, MyTextureChange> textureChanges)
		{
			if (Subparts != null)
			{
				foreach (KeyValuePair<string, MyEntitySubpart> subpart in Subparts)
				{
					subpart.Value.Render.TextureChanges = textureChanges;
				}
			}
		}

		void VRage.ModAPI.IMyEntity.SetEmissiveParts(string emissiveName, Color emissivePartColor, float emissivity)
		{
			SetEmissiveParts(emissiveName, emissivePartColor, emissivity);
		}

		void VRage.ModAPI.IMyEntity.SetEmissivePartsForSubparts(string emissiveName, Color emissivePartColor, float emissivity)
		{
			SetEmissivePartsForSubparts(emissiveName, emissivePartColor, emissivity);
		}

		VRage.Game.ModAPI.Ingame.IMyInventory VRage.Game.ModAPI.Ingame.IMyEntity.GetInventory()
		{
			return GetInventoryBase(0) as VRage.Game.ModAPI.Ingame.IMyInventory;
		}

		VRage.Game.ModAPI.Ingame.IMyInventory VRage.Game.ModAPI.Ingame.IMyEntity.GetInventory(int index)
		{
			return GetInventoryBase(index) as VRage.Game.ModAPI.Ingame.IMyInventory;
		}

		Vector3D VRage.Game.ModAPI.Ingame.IMyEntity.GetPosition()
		{
			return PositionComp.GetPosition();
		}
	}
}
