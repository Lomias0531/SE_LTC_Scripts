#define VRAGE
using Havok;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;

namespace Sandbox.Engine.Physics
{
	[MyComponentBuilder(typeof(MyObjectBuilder_PhysicsBodyComponent), true)]
	public class MyPhysicsBody : MyPhysicsComponentBase, MyClusterTree.IMyActivationHandler
	{
		public delegate void PhysicsContactHandler(ref MyPhysics.MyContactPointEvent e);

		public class MyWeldInfo
		{
			public MyPhysicsBody Parent;

			public Matrix Transform = Matrix.Identity;

			public readonly HashSet<MyPhysicsBody> Children = new HashSet<MyPhysicsBody>();

			public HkMassElement MassElement;

			internal void UpdateMassProps(HkRigidBody rb)
			{
				HkMassProperties properties = default(HkMassProperties);
				properties.InertiaTensor = rb.InertiaTensor;
				properties.Mass = rb.Mass;
				properties.CenterOfMass = rb.CenterOfMassLocal;
				MassElement = default(HkMassElement);
				MassElement.Properties = properties;
				MassElement.Tranform = Transform;
			}

			internal void SetMassProps(HkMassProperties mp)
			{
				MassElement = default(HkMassElement);
				MassElement.Properties = mp;
				MassElement.Tranform = Transform;
			}
		}

		private class Sandbox_Engine_Physics_MyPhysicsBody_003C_003EActor : IActivator, IActivator<MyPhysicsBody>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicsBody();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicsBody CreateInstance()
			{
				return new MyPhysicsBody();
			}

			MyPhysicsBody IActivator<MyPhysicsBody>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private PhysicsContactHandler m_contactPointCallbackHandler;

		private Action<MyPhysicsComponentBase, bool> m_onBodyActiveStateChangedHandler;

		private bool m_activationCallbackRegistered;

		private bool m_contactPointCallbackRegistered;

		private static MyStringHash m_character = MyStringHash.GetOrCompute("Character");

		private int m_motionCounter;

		protected float m_angularDamping;

		protected float m_linearDamping;

		private ulong m_clusterObjectID = ulong.MaxValue;

		private Vector3D m_offset = Vector3D.Zero;

		protected Matrix m_bodyMatrix;

		protected HkWorld m_world;

		private HkWorld m_lastWorld;

		private HkRigidBody m_rigidBody;

		private HkRigidBody m_rigidBody2;

		private float m_animatedClientMass;

		private readonly HashSet<HkConstraint> m_constraints = new HashSet<HkConstraint>();

		private readonly List<HkConstraint> m_constraintsAddBatch = new List<HkConstraint>();

		private readonly List<HkConstraint> m_constraintsRemoveBatch = new List<HkConstraint>();

		public HkSolverDeactivation InitialSolverDeactivation = HkSolverDeactivation.Low;

		private bool m_isInWorld;

		private bool m_shapeChangeInProgress;

		private HashSet<IMyEntity> m_batchedChildren = new HashSet<IMyEntity>();

		private List<MyPhysicsBody> m_batchedBodies = new List<MyPhysicsBody>();

		private Vector3D? m_lastComPosition;

		private Vector3 m_lastComLocal;

		private bool m_isStaticForCluster;

		private bool m_batchRequest;

		private static List<HkConstraint> m_notifyConstraints = new List<HkConstraint>();

		private HkdBreakableBody m_breakableBody;

		private List<HkdBreakableBodyInfo> m_tmpLst = new List<HkdBreakableBodyInfo>();

		protected HkRagdoll m_ragdoll;

		private bool m_ragdollDeadMode;

		private readonly MyWeldInfo m_weldInfo = new MyWeldInfo();

		private List<HkShape> m_tmpShapeList = new List<HkShape>();

		private bool NeedsActivationCallback
		{
			get
			{
				if (!(m_rigidBody2 != null))
				{
					return m_onBodyActiveStateChangedHandler != null;
				}
				return true;
			}
		}

		private bool NeedsContactPointCallback => m_contactPointCallbackHandler != null;

		protected ulong ClusterObjectID
		{
			get
			{
				return m_clusterObjectID;
			}
			set
			{
				m_clusterObjectID = value;
				if (value != ulong.MaxValue)
				{
					Offset = MyPhysics.GetObjectOffset(value);
				}
				else
				{
					Offset = Vector3D.Zero;
				}
				foreach (MyPhysicsBody child in WeldInfo.Children)
				{
					child.Offset = Offset;
				}
			}
		}

		protected Vector3D Offset
		{
			get
			{
				IMyEntity topMostParent = base.Entity.GetTopMostParent();
				if (topMostParent != base.Entity && topMostParent.Physics != null)
				{
					return ((MyPhysicsBody)topMostParent.Physics).Offset;
				}
				return m_offset;
			}
			set
			{
				m_offset = value;
			}
		}

		public new MyPhysicsBodyComponentDefinition Definition
		{
			get;
			private set;
		}

		public HkWorld HavokWorld
		{
			get
			{
				if (IsWelded)
				{
					return WeldInfo.Parent.m_world;
				}
				IMyEntity topMostParent = base.Entity.GetTopMostParent();
				if (topMostParent != base.Entity && topMostParent.Physics != null)
				{
					return ((MyPhysicsBody)topMostParent.Physics).HavokWorld;
				}
				return m_world;
			}
		}

		public virtual int HavokCollisionSystemID
		{
			get
			{
				if (!(RigidBody != null))
				{
					return 0;
				}
				return HkGroupFilter.GetSystemGroupFromFilterInfo(RigidBody.GetCollisionFilterInfo());
			}
			protected set
			{
				if (RigidBody != null)
				{
					RigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(RigidBody.Layer, value, 1, 1));
				}
				if (RigidBody2 != null)
				{
					RigidBody2.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(RigidBody2.Layer, value, 1, 1));
				}
			}
		}

		public override HkRigidBody RigidBody
		{
			get
			{
				if (WeldInfo.Parent == null)
				{
					return m_rigidBody;
				}
				return WeldInfo.Parent.RigidBody;
			}
			protected set
			{
				if (m_rigidBody != value)
				{
					if (m_rigidBody != null && !m_rigidBody.IsDisposed)
					{
						m_rigidBody.ContactSoundCallback -= OnContactSoundCallback;
						UnregisterContactPointCallback();
						UnregisterActivationCallbacks();
					}
					m_rigidBody = value;
					m_activationCallbackRegistered = false;
					m_contactPointCallbackRegistered = false;
					if (m_rigidBody != null)
					{
						RegisterActivationCallbacksIfNeeded();
						RegisterContactPointCallbackIfNeeded();
						m_rigidBody.ContactSoundCallback += OnContactSoundCallback;
					}
				}
			}
		}

		public override HkRigidBody RigidBody2
		{
			get
			{
				if (WeldInfo.Parent == null)
				{
					return m_rigidBody2;
				}
				return WeldInfo.Parent.RigidBody2;
			}
			protected set
			{
				if (m_rigidBody2 != value)
				{
					m_rigidBody2 = value;
					if (NeedsActivationCallback)
					{
						RegisterActivationCallbacksIfNeeded();
					}
					else
					{
						UnregisterActivationCallbacks();
					}
				}
			}
		}

		public override float Mass
		{
			get
			{
				if (CharacterProxy != null)
				{
					return CharacterProxy.Mass;
				}
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer)
					{
						return m_animatedClientMass;
					}
					return RigidBody.Mass;
				}
				if (Ragdoll != null)
				{
					return Ragdoll.Mass;
				}
				return 0f;
			}
		}

		public override float Speed => LinearVelocity.Length();

		public override float Friction
		{
			get
			{
				return RigidBody.Friction;
			}
			set
			{
				RigidBody.Friction = value;
			}
		}

		public override bool IsStatic
		{
			get
			{
				if (RigidBody != null)
				{
					return RigidBody.IsFixed;
				}
				return false;
			}
		}

		public override bool IsKinematic
		{
			get
			{
				if (RigidBody != null)
				{
					if (!RigidBody.IsFixed)
					{
						return RigidBody.IsFixedOrKeyframed;
					}
					return false;
				}
				return false;
			}
		}

		public bool IsSubpart
		{
			get;
			set;
		}

		public override bool IsActive
		{
			get
			{
				if (RigidBody != null)
				{
					return RigidBody.IsActive;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.GetHitRigidBody().IsActive;
				}
				if (Ragdoll != null)
				{
					return Ragdoll.IsActive;
				}
				return false;
			}
		}

		public MyCharacterProxy CharacterProxy
		{
			get;
			set;
		}

		public int CharacterSystemGroupCollisionFilterID
		{
			get;
			private set;
		}

		public uint CharacterCollisionFilter
		{
			get;
			private set;
		}

		public override bool IsInWorld
		{
			get
			{
				return m_isInWorld;
			}
			protected set
			{
				m_isInWorld = value;
			}
		}

		public override bool ShapeChangeInProgress
		{
			get
			{
				return m_shapeChangeInProgress;
			}
			set
			{
				m_shapeChangeInProgress = value;
			}
		}

		public override Vector3 AngularVelocityLocal
		{
			get
			{
				if (!Enabled)
				{
					return Vector3.Zero;
				}
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer && IsStatic)
					{
						return base.AngularVelocity;
					}
					return RigidBody.AngularVelocity;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.AngularVelocity;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					return Ragdoll.GetRootRigidBody().AngularVelocity;
				}
				return base.AngularVelocity;
			}
		}

		public override Vector3 LinearVelocityLocal
		{
			get
			{
				if (!Enabled)
				{
					return Vector3.Zero;
				}
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer && IsStatic)
					{
						return base.LinearVelocity;
					}
					return RigidBody.LinearVelocity;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.LinearVelocity;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					return Ragdoll.GetRootRigidBody().LinearVelocity;
				}
				return base.LinearVelocity;
			}
		}

		public override Vector3 LinearVelocity
		{
			get
			{
				if (!Enabled)
				{
					return Vector3.Zero;
				}
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer)
					{
						MyCubeGrid myCubeGrid = base.Entity as MyCubeGrid;
						if (myCubeGrid != null)
						{
							MyEntity entityById = MyEntities.GetEntityById(myCubeGrid.ClosestParentId);
							if (entityById != null && entityById.Physics != null)
							{
								return entityById.Physics.LinearVelocity + RigidBody.LinearVelocity;
							}
							if (IsStatic)
							{
								return base.LinearVelocity;
							}
						}
						else if (IsStatic)
						{
							MyCharacter myCharacter = base.Entity as MyCharacter;
							if (myCharacter != null)
							{
								MyEntity entityById2 = MyEntities.GetEntityById(myCharacter.ClosestParentId);
								if (entityById2 != null && entityById2.Physics != null)
								{
									if (myCharacter.InheritRotation)
									{
										Vector3D worldPos = base.Entity.PositionComp.GetPosition();
										entityById2.Physics.GetVelocityAtPointLocal(ref worldPos, out Vector3 linearVelocity);
										return linearVelocity + base.LinearVelocity;
									}
									return entityById2.Physics.LinearVelocity + base.LinearVelocity;
								}
							}
							return base.LinearVelocity;
						}
					}
					return RigidBody.LinearVelocity;
				}
				if (CharacterProxy != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer)
					{
						MyCharacter myCharacter2 = (MyCharacter)base.Entity;
						MyEntity entityById3 = MyEntities.GetEntityById(myCharacter2.ClosestParentId);
						if (entityById3 != null && entityById3.Physics != null)
						{
							if (myCharacter2.InheritRotation)
							{
								Vector3D worldPos2 = base.Entity.PositionComp.GetPosition();
								entityById3.Physics.GetVelocityAtPointLocal(ref worldPos2, out Vector3 linearVelocity2);
								return linearVelocity2 + CharacterProxy.LinearVelocity;
							}
							return entityById3.Physics.LinearVelocity + CharacterProxy.LinearVelocity;
						}
					}
					return CharacterProxy.LinearVelocity;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					return Ragdoll.GetRootRigidBody().LinearVelocity;
				}
				return base.LinearVelocity;
			}
			set
			{
				if (RigidBody != null)
				{
					RigidBody.LinearVelocity = value;
				}
				if (CharacterProxy != null)
				{
					CharacterProxy.LinearVelocity = value;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
					{
						rigidBody.LinearVelocity = value;
					}
				}
				base.LinearVelocity = value;
			}
		}

		public override float LinearDamping
		{
			get
			{
				return RigidBody.LinearDamping;
			}
			set
			{
				if (RigidBody != null)
				{
					RigidBody.LinearDamping = value;
				}
				m_linearDamping = value;
			}
		}

		public override float AngularDamping
		{
			get
			{
				return RigidBody.AngularDamping;
			}
			set
			{
				if (RigidBody != null)
				{
					RigidBody.AngularDamping = value;
				}
				m_angularDamping = value;
			}
		}

		public override Vector3 AngularVelocity
		{
			get
			{
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer && IsStatic)
					{
						return base.AngularVelocity;
					}
					return RigidBody.AngularVelocity;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.AngularVelocity;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					return Ragdoll.GetRootRigidBody().AngularVelocity;
				}
				return base.AngularVelocity;
			}
			set
			{
				if (RigidBody != null)
				{
					RigidBody.AngularVelocity = value;
				}
				if (CharacterProxy != null)
				{
					CharacterProxy.AngularVelocity = value;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
					{
						rigidBody.AngularVelocity = value;
					}
				}
				base.AngularVelocity = value;
			}
		}

		public override Vector3 SupportNormal
		{
			get
			{
				if (CharacterProxy != null)
				{
					return CharacterProxy.SupportNormal;
				}
				return base.SupportNormal;
			}
			set
			{
				base.SupportNormal = value;
			}
		}

		public override bool IsMoving
		{
			get
			{
				if (Vector3.IsZero(LinearVelocity))
				{
					return !Vector3.IsZero(AngularVelocity);
				}
				return true;
			}
		}

		public override Vector3 Gravity
		{
			get
			{
				if (!Enabled)
				{
					return Vector3.Zero;
				}
				if (RigidBody != null)
				{
					return RigidBody.Gravity;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.Gravity;
				}
				return Vector3.Zero;
			}
			set
			{
				HkRigidBody rigidBody = RigidBody;
				if (rigidBody != null)
				{
					rigidBody.Gravity = value;
				}
				if (CharacterProxy != null)
				{
					CharacterProxy.Gravity = value;
				}
			}
		}

		public override bool HasRigidBody => RigidBody != null;

		public override Vector3 CenterOfMassLocal => RigidBody.CenterOfMassLocal;

		public override Vector3D CenterOfMassWorld => RigidBody.CenterOfMassWorld + Offset;

		public HashSetReader<HkConstraint> Constraints => m_constraints;

		public virtual bool IsStaticForCluster
		{
			get
			{
				return m_isStaticForCluster;
			}
			set
			{
				m_isStaticForCluster = value;
			}
		}

		bool MyClusterTree.IMyActivationHandler.IsStaticForCluster => IsStaticForCluster;

		public Vector3 LastLinearVelocity => m_lastLinearVelocity;

		public Vector3 LastAngularVelocity => m_lastAngularVelocity;

		public override HkdBreakableBody BreakableBody
		{
			get
			{
				return m_breakableBody;
			}
			set
			{
				m_breakableBody = value;
				RigidBody = value;
			}
		}

		public int RagdollSystemGroupCollisionFilterID
		{
			get;
			private set;
		}

		public bool IsRagdollModeActive
		{
			get
			{
				if (Ragdoll == null)
				{
					return false;
				}
				return Ragdoll.InWorld;
			}
		}

		public HkRagdoll Ragdoll
		{
			get
			{
				return m_ragdoll;
			}
			set
			{
				m_ragdoll = value;
				if (m_ragdoll != null)
				{
					HkRagdoll ragdoll = m_ragdoll;
					ragdoll.AddedToWorld = (Action<HkRagdoll>)Delegate.Combine(ragdoll.AddedToWorld, new Action<HkRagdoll>(OnRagdollAddedToWorld));
				}
			}
		}

		public bool ReactivateRagdoll
		{
			get;
			set;
		}

		public bool SwitchToRagdollModeOnActivate
		{
			get;
			set;
		}

		public bool IsWelded => WeldInfo.Parent != null;

		[Obsolete]
		public MyWeldInfo WeldInfo => m_weldInfo;

		public HkRigidBody WeldedRigidBody
		{
			get;
			protected set;
		}

		public override event Action<MyPhysicsComponentBase, bool> OnBodyActiveStateChanged
		{
			add
			{
				m_onBodyActiveStateChangedHandler = (Action<MyPhysicsComponentBase, bool>)Delegate.Combine(m_onBodyActiveStateChangedHandler, value);
				RegisterActivationCallbacksIfNeeded();
			}
			remove
			{
				m_onBodyActiveStateChangedHandler = (Action<MyPhysicsComponentBase, bool>)Delegate.Remove(m_onBodyActiveStateChangedHandler, value);
				if (!NeedsActivationCallback)
				{
					UnregisterActivationCallbacks();
				}
			}
		}

		public event PhysicsContactHandler ContactPointCallback
		{
			add
			{
				_ = IsInWorld;
				m_contactPointCallbackHandler = (PhysicsContactHandler)Delegate.Combine(m_contactPointCallbackHandler, value);
				RegisterContactPointCallbackIfNeeded();
			}
			remove
			{
				_ = IsInWorld;
				m_contactPointCallbackHandler = (PhysicsContactHandler)Delegate.Remove(m_contactPointCallbackHandler, value);
				if (!NeedsContactPointCallback)
				{
					UnregisterContactPointCallback();
				}
			}
		}

		private void OnContactPointCallback(ref HkContactPointEvent e)
		{
			if (m_contactPointCallbackHandler != null)
			{
				MyPhysics.MyContactPointEvent myContactPointEvent = default(MyPhysics.MyContactPointEvent);
				myContactPointEvent.ContactPointEvent = e;
				myContactPointEvent.Position = e.ContactPoint.Position + Offset;
				MyPhysics.MyContactPointEvent e2 = myContactPointEvent;
				m_contactPointCallbackHandler(ref e2);
			}
		}

		private void OnDynamicRigidBodyActivated(HkEntity entity)
		{
			SynchronizeKeyframedRigidBody();
			InvokeOnBodyActiveStateChanged(active: true);
		}

		private void OnDynamicRigidBodyDeactivated(HkEntity entity)
		{
			SynchronizeKeyframedRigidBody();
			InvokeOnBodyActiveStateChanged(active: false);
		}

		protected void InvokeOnBodyActiveStateChanged(bool active)
		{
			m_onBodyActiveStateChangedHandler.InvokeIfNotNull(this, active);
		}

		private void RegisterActivationCallbacksIfNeeded()
		{
			if (!m_activationCallbackRegistered && NeedsActivationCallback && !(m_rigidBody == null))
			{
				m_activationCallbackRegistered = true;
				m_rigidBody.Activated += OnDynamicRigidBodyActivated;
				m_rigidBody.Deactivated += OnDynamicRigidBodyDeactivated;
			}
		}

		private void RegisterContactPointCallbackIfNeeded()
		{
			if (!m_contactPointCallbackRegistered && NeedsContactPointCallback && !(m_rigidBody == null))
			{
				m_contactPointCallbackRegistered = true;
				m_rigidBody.ContactPointCallback += OnContactPointCallback;
			}
		}

		private void UnregisterActivationCallbacks()
		{
			if (m_activationCallbackRegistered)
			{
				m_activationCallbackRegistered = false;
				if (!(m_rigidBody == null))
				{
					m_rigidBody.Activated -= OnDynamicRigidBodyActivated;
					m_rigidBody.Deactivated -= OnDynamicRigidBodyDeactivated;
				}
			}
		}

		private void UnregisterContactPointCallback()
		{
			if (m_contactPointCallbackRegistered)
			{
				m_contactPointCallbackRegistered = false;
				if (!(m_rigidBody == null))
				{
					m_rigidBody.ContactPointCallback -= OnContactPointCallback;
				}
			}
		}

		protected override void CloseRigidBody()
		{
			if (IsWelded)
			{
				WeldInfo.Parent.Unweld(this, insertToWorld: false);
			}
			if (WeldInfo.Children.Count != 0)
			{
				MyWeldingGroups.ReplaceParent(MyWeldingGroups.Static.GetGroup((MyEntity)base.Entity), (MyEntity)base.Entity, null);
			}
			CheckRBNotInWorld();
			if (RigidBody != null)
			{
				if (!RigidBody.IsDisposed)
				{
					RigidBody.Dispose();
				}
				RigidBody = null;
			}
			if (RigidBody2 != null)
			{
				RigidBody2.Dispose();
				RigidBody2 = null;
			}
			if (BreakableBody != null)
			{
				BreakableBody.Dispose();
				BreakableBody = null;
			}
			if (WeldedRigidBody != null)
			{
				WeldedRigidBody.Dispose();
				WeldedRigidBody = null;
			}
		}

		public MyPhysicsBody()
		{
		}

		public MyPhysicsBody(IMyEntity entity, RigidBodyFlag flags)
		{
			base.Entity = entity;
			m_enabled = false;
			Flags = flags;
			IsSubpart = false;
		}

		private void OnContactSoundCallback(ref HkContactPointEvent e)
		{
			if (Sync.IsServer && MyAudioComponent.ShouldPlayContactSound(base.Entity.EntityId, e.EventType))
			{
				ContactPointWrapper wrap = new ContactPointWrapper(ref e);
				wrap.WorldPosition = ClusterToWorld(wrap.position);
				MySandboxGame.Static.Invoke(delegate
				{
					MyAudioComponent.PlayContactSound(wrap, base.Entity);
				}, "MyAudioComponent::PlayContactSound");
			}
		}

		public override void Close()
		{
			CloseRagdoll();
			base.Close();
			if (CharacterProxy != null)
			{
				CharacterProxy.Dispose();
				CharacterProxy = null;
			}
		}

		public override void AddForce(MyPhysicsForceType type, Vector3? force, Vector3D? position, Vector3? torque, float? maxSpeed = null, bool applyImmediately = true, bool activeOnly = false)
		{
			if (applyImmediately)
			{
				AddForceInternal(type, force, position, torque, maxSpeed, activeOnly);
				return;
			}
			if (!activeOnly && !IsActive)
			{
				if (RigidBody != null)
				{
					RigidBody.Activate();
				}
				else if (CharacterProxy != null)
				{
					CharacterProxy.GetHitRigidBody().Activate();
				}
				else if (Ragdoll != null)
				{
					Ragdoll.Activate();
				}
			}
			MyPhysics.QueuedForces.Enqueue(new MyPhysics.ForceInfo(this, activeOnly, maxSpeed, force, torque, position, type));
		}

		private void AddForceInternal(MyPhysicsForceType type, Vector3? force, Vector3D? position, Vector3? torque, float? maxSpeed, bool activeOnly)
		{
			if (IsStatic || (activeOnly && !IsActive))
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_FORCES)
			{
				MyPhysicsDebugDraw.DebugDrawAddForce(this, type, force, position, torque);
			}
			switch (type)
			{
			case MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE:
				if (RigidBody != null)
				{
					Matrix transform = RigidBody.GetRigidBodyMatrix();
					AddForceTorqueBody(force, torque, position, RigidBody, ref transform);
				}
				if (CharacterProxy != null && CharacterProxy.GetHitRigidBody() != null)
				{
					Matrix transform = base.Entity.WorldMatrix;
					AddForceTorqueBody(force, torque, position, CharacterProxy.GetHitRigidBody(), ref transform);
				}
				if (Ragdoll != null && Ragdoll.InWorld && !Ragdoll.IsKeyframed)
				{
					Matrix transform = base.Entity.WorldMatrix;
					ApplyForceTorqueOnRagdoll(force, torque, Ragdoll, ref transform);
				}
				break;
			case MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE:
				ApplyImplusesWorld(force, position, torque, RigidBody);
				if (CharacterProxy != null && force.HasValue)
				{
					CharacterProxy.ApplyLinearImpulse(force.Value);
				}
				if (Ragdoll != null && Ragdoll.InWorld && !Ragdoll.IsKeyframed)
				{
					ApplyImpuseOnRagdoll(force, position, torque, Ragdoll);
				}
				break;
			case MyPhysicsForceType.APPLY_WORLD_FORCE:
				ApplyForceWorld(force, position, RigidBody);
				if (CharacterProxy != null)
				{
					if (CharacterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_ON_GROUND)
					{
						CharacterProxy.ApplyLinearImpulse(force.Value / Mass * 10f);
					}
					else
					{
						CharacterProxy.ApplyLinearImpulse(force.Value / Mass);
					}
				}
				if (Ragdoll != null && Ragdoll.InWorld && !Ragdoll.IsKeyframed)
				{
					ApplyForceOnRagdoll(force, position, Ragdoll);
				}
				break;
			}
			if (LinearVelocity.LengthSquared() > maxSpeed * maxSpeed)
			{
				Vector3 linearVelocity = LinearVelocity;
				linearVelocity.Normalize();
				linearVelocity *= maxSpeed.Value;
				if (RigidBody != null && MyMultiplayer.Static != null && !Sync.IsServer && base.Entity is MyCubeGrid && MyEntities.TryGetEntityById(((MyCubeGrid)base.Entity).ClosestParentId, out MyEntity entity))
				{
					linearVelocity -= entity.Physics.LinearVelocity;
				}
				LinearVelocity = linearVelocity;
			}
		}

		private void ApplyForceWorld(Vector3? force, Vector3D? position, HkRigidBody rigidBody)
		{
			if (!(rigidBody == null) && force.HasValue && !MyUtils.IsZero(force.Value))
			{
				if (position.HasValue)
				{
					Vector3 point = position.Value - Offset;
					rigidBody.ApplyForce(0.0166666675f, force.Value, point);
				}
				else
				{
					rigidBody.ApplyForce(0.0166666675f, force.Value);
				}
			}
		}

		private void ApplyImplusesWorld(Vector3? force, Vector3D? position, Vector3? torque, HkRigidBody rigidBody)
		{
			if (!(rigidBody == null))
			{
				if (force.HasValue && position.HasValue)
				{
					rigidBody.ApplyPointImpulse(force.Value, position.Value - Offset);
				}
				if (torque.HasValue)
				{
					rigidBody.ApplyAngularImpulse(torque.Value * 0.0166666675f * MyFakes.SIMULATION_SPEED);
				}
			}
		}

		private void AddForceTorqueBody(Vector3? force, Vector3? torque, Vector3? position, HkRigidBody rigidBody, ref Matrix transform)
		{
			if (force.HasValue && !MyUtils.IsZero(force.Value))
			{
				Vector3 normal = force.Value;
				Vector3.TransformNormal(ref normal, ref transform, out normal);
				if (position.HasValue)
				{
					Vector3 position2 = position.Value;
					Vector3.Transform(ref position2, ref transform, out position2);
					ApplyForceWorld(normal, position2 + Offset, rigidBody);
				}
				else
				{
					rigidBody.ApplyLinearImpulse(normal * 0.0166666675f * MyFakes.SIMULATION_SPEED);
				}
			}
			if (torque.HasValue && !MyUtils.IsZero(torque.Value))
			{
				Vector3 value = Vector3.TransformNormal(torque.Value, transform);
				rigidBody.ApplyAngularImpulse(value * 0.0166666675f * MyFakes.SIMULATION_SPEED);
				Vector3 angularVelocity = rigidBody.AngularVelocity;
				float maxAngularVelocity = rigidBody.MaxAngularVelocity;
				if (angularVelocity.LengthSquared() > maxAngularVelocity * maxAngularVelocity)
				{
					angularVelocity.Normalize();
					angularVelocity = (rigidBody.AngularVelocity = angularVelocity * maxAngularVelocity);
				}
			}
		}

		public override void ApplyImpulse(Vector3 impulse, Vector3D pos)
		{
			AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, impulse, pos, null);
		}

		public override void ClearSpeed()
		{
			if (RigidBody != null)
			{
				RigidBody.LinearVelocity = Vector3.Zero;
				RigidBody.AngularVelocity = Vector3.Zero;
			}
			if (CharacterProxy != null)
			{
				CharacterProxy.LinearVelocity = Vector3.Zero;
				CharacterProxy.AngularVelocity = Vector3.Zero;
				CharacterProxy.PosX = 0f;
				CharacterProxy.PosY = 0f;
				CharacterProxy.Elevate = 0f;
			}
		}

		public override void Clear()
		{
			ClearSpeed();
		}

		public override void DebugDraw()
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS)
			{
				int num = 0;
				foreach (HkConstraint constraint in Constraints)
				{
					if (!constraint.IsDisposed)
					{
						Color color = Color.Green;
						if (!IsConstraintValid(constraint))
						{
							color = Color.Red;
						}
						else if (!constraint.Enabled)
						{
							color = Color.Yellow;
						}
						constraint.GetPivotsInWorld(out Vector3 pivotA, out Vector3 pivotB);
						Vector3D vector3D = ClusterToWorld(pivotA);
						Vector3D vector3D2 = ClusterToWorld(pivotB);
						MyRenderProxy.DebugDrawLine3D(vector3D, vector3D2, color, color, depthRead: false);
						MyRenderProxy.DebugDrawSphere(vector3D, 0.2f, color, 1f, depthRead: false);
						MyRenderProxy.DebugDrawText3D(vector3D, num + " A", Color.White, 0.7f, depthRead: true);
						MyRenderProxy.DebugDrawSphere(vector3D2, 0.2f, color, 1f, depthRead: false);
						MyRenderProxy.DebugDrawText3D(vector3D2, num + " B", Color.White, 0.7f, depthRead: true);
						num++;
					}
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_INERTIA_TENSORS && RigidBody != null)
			{
				Vector3D vector3D3 = ClusterToWorld(RigidBody.CenterOfMassWorld);
				MyRenderProxy.DebugDrawLine3D(vector3D3, vector3D3 + RigidBody.AngularVelocity, Color.Blue, Color.Red, depthRead: false);
				float num2 = 1f / RigidBody.Mass;
				Vector3 scale = RigidBody.InertiaTensor.Scale;
				float num3 = (scale.X - scale.Y + scale.Z) * num2 * 6f;
				float num4 = scale.X * num2 * 12f - num3;
				float num5 = scale.Z * num2 * 12f - num3;
				float scaleFactor = 0.505f;
				Vector3 vector = new Vector3(Math.Sqrt(num5), Math.Sqrt(num3), Math.Sqrt(num4)) * scaleFactor;
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(new BoundingBoxD(-vector, vector), MatrixD.Identity);
				obb.Transform(RigidBody.GetRigidBodyMatrix());
				obb.Center = CenterOfMassWorld;
				MyRenderProxy.DebugDrawOBB(obb, Color.Purple, 0.05f, depthRead: false, smooth: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_MOTION_TYPES && RigidBody != null)
			{
				MyRenderProxy.DebugDrawText3D(CenterOfMassWorld, RigidBody.GetMotionType().ToString(), Color.Purple, 0.5f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES && !IsWelded)
			{
				if (RigidBody != null && BreakableBody != null)
				{
					_ = Vector3D.Transform((Vector3D)BreakableBody.BreakableShape.CoM, RigidBody.GetRigidBodyMatrix()) + Offset;
					Color color2 = (RigidBody.GetMotionType() != HkMotionType.Box_Inertia) ? Color.Gray : (RigidBody.IsActive ? Color.Red : Color.Blue);
					MyRenderProxy.DebugDrawSphere(RigidBody.CenterOfMassWorld + Offset, 0.2f, color2, 1f, depthRead: false);
					MyRenderProxy.DebugDrawAxis(base.Entity.PositionComp.WorldMatrix, 0.2f, depthRead: false);
				}
				if (RigidBody != null)
				{
					int shapeIndex = 0;
					Matrix rigidBodyMatrix = RigidBody.GetRigidBodyMatrix();
					MatrixD worldMatrix = MatrixD.CreateWorld(rigidBodyMatrix.Translation + Offset, rigidBodyMatrix.Forward, rigidBodyMatrix.Up);
					MyPhysicsDebugDraw.DrawCollisionShape(RigidBody.GetShape(), worldMatrix, 0.3f, ref shapeIndex);
				}
				if (RigidBody2 != null)
				{
					int shapeIndex = 0;
					Matrix rigidBodyMatrix2 = RigidBody2.GetRigidBodyMatrix();
					MatrixD worldMatrix2 = MatrixD.CreateWorld(rigidBodyMatrix2.Translation + Offset, rigidBodyMatrix2.Forward, rigidBodyMatrix2.Up);
					MyPhysicsDebugDraw.DrawCollisionShape(RigidBody2.GetShape(), worldMatrix2, 0.3f, ref shapeIndex);
				}
				if (CharacterProxy != null)
				{
					int shapeIndex = 0;
					Matrix rigidBodyTransform = CharacterProxy.GetRigidBodyTransform();
					MatrixD worldMatrix3 = MatrixD.CreateWorld(rigidBodyTransform.Translation + Offset, rigidBodyTransform.Forward, rigidBodyTransform.Up);
					MyPhysicsDebugDraw.DrawCollisionShape(CharacterProxy.GetShape(), worldMatrix3, 0.3f, ref shapeIndex);
				}
			}
		}

		public virtual void CreateFromCollisionObject(HkShape shape, Vector3 center, MatrixD worldTransform, HkMassProperties? massProperties = null, int collisionFilter = 15)
		{
			CloseRigidBody();
			base.Center = center;
			base.CanUpdateAccelerations = true;
			CreateBody(ref shape, massProperties);
			RigidBody.UserObject = this;
			RigidBody.SetWorldMatrix(worldTransform);
			RigidBody.Layer = collisionFilter;
			if ((Flags & RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE) > RigidBodyFlag.RBF_DEFAULT)
			{
				RigidBody.Layer = 19;
			}
		}

		protected virtual void CreateBody(ref HkShape shape, HkMassProperties? massProperties)
		{
			HkRigidBodyCinfo hkRigidBodyCinfo = new HkRigidBodyCinfo();
			hkRigidBodyCinfo.AngularDamping = m_angularDamping;
			hkRigidBodyCinfo.LinearDamping = m_linearDamping;
			hkRigidBodyCinfo.Shape = shape;
			hkRigidBodyCinfo.SolverDeactivation = InitialSolverDeactivation;
			hkRigidBodyCinfo.ContactPointCallbackDelay = ContactPointDelay;
			if (massProperties.HasValue)
			{
				m_animatedClientMass = massProperties.Value.Mass;
				hkRigidBodyCinfo.SetMassProperties(massProperties.Value);
			}
			GetInfoFromFlags(hkRigidBodyCinfo, Flags);
			RigidBody = new HkRigidBody(hkRigidBodyCinfo);
		}

		protected static void GetInfoFromFlags(HkRigidBodyCinfo rbInfo, RigidBodyFlag flags)
		{
			if ((flags & RigidBodyFlag.RBF_STATIC) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Fixed;
				rbInfo.QualityType = HkCollidableQualityType.Fixed;
			}
			else if ((flags & RigidBodyFlag.RBF_BULLET) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Dynamic;
				rbInfo.QualityType = HkCollidableQualityType.Bullet;
			}
			else if ((flags & RigidBodyFlag.RBF_KINEMATIC) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Keyframed;
				rbInfo.QualityType = HkCollidableQualityType.Keyframed;
			}
			else if ((flags & RigidBodyFlag.RBF_DOUBLED_KINEMATIC) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Dynamic;
				rbInfo.QualityType = HkCollidableQualityType.Moving;
			}
			else if ((flags & RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Fixed;
				rbInfo.QualityType = HkCollidableQualityType.Fixed;
			}
			else if ((flags & RigidBodyFlag.RBF_DEBRIS) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Dynamic;
				rbInfo.QualityType = HkCollidableQualityType.Debris;
				rbInfo.SolverDeactivation = HkSolverDeactivation.Max;
			}
			else if ((flags & RigidBodyFlag.RBF_KEYFRAMED_REPORTING) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Keyframed;
				rbInfo.QualityType = HkCollidableQualityType.KeyframedReporting;
			}
			else
			{
				rbInfo.MotionType = HkMotionType.Dynamic;
				rbInfo.QualityType = HkCollidableQualityType.Moving;
			}
			if ((flags & RigidBodyFlag.RBF_UNLOCKED_SPEEDS) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MaxLinearVelocity = MyGridPhysics.LargeShipMaxLinearVelocity() * 10f;
				rbInfo.MaxAngularVelocity = MyGridPhysics.GetLargeShipMaxAngularVelocity() * 10f;
			}
		}

		public override void CreateCharacterCollision(Vector3 center, float characterWidth, float characterHeight, float crouchHeight, float ladderHeight, float headSize, float headHeight, MatrixD worldTransform, float mass, ushort collisionLayer, bool isOnlyVertical, float maxSlope, float maxImpulse, float maxSpeedRelativeToShip, bool networkProxy, float? maxForce = null)
		{
			base.Center = center;
			base.CanUpdateAccelerations = false;
			if (networkProxy)
			{
				float downOffset = ((MyCharacter)base.Entity).IsCrouching ? 1f : 0f;
				HkShape shape = MyCharacterProxy.CreateCharacterShape(characterHeight, characterWidth, characterHeight + headHeight, headSize, 0f, downOffset);
				HkMassProperties value = default(HkMassProperties);
				value.Mass = mass;
				value.InertiaTensor = Matrix.Identity;
				value.Volume = characterWidth * characterWidth * (characterHeight + 2f * characterWidth);
				CreateFromCollisionObject(shape, center, worldTransform, value, collisionLayer);
				base.CanUpdateAccelerations = false;
			}
			else
			{
				CharacterProxy = new MyCharacterProxy(isDynamic: true, isCapsule: true, characterWidth, characterHeight, crouchHeight, ladderHeight, headSize, headHeight, Matrix.CreateWorld(Vector3.TransformNormal(base.Center, worldTransform) + worldTransform.Translation, worldTransform.Forward, worldTransform.Up).Translation, worldTransform.Up, worldTransform.Forward, mass, this, isOnlyVertical, maxSlope, maxImpulse, maxSpeedRelativeToShip, maxForce);
				CharacterProxy.GetRigidBody().ContactPointCallbackDelay = 0;
			}
		}

		protected virtual void ActivateCollision()
		{
			((MyEntity)base.Entity).RaisePhysicsChanged();
		}

		public override void Deactivate()
		{
			if (ClusterObjectID != ulong.MaxValue)
			{
				if (IsWelded)
				{
					Unweld(insertInWorld: false);
					return;
				}
				MyPhysics.RemoveObject(ClusterObjectID);
				ClusterObjectID = ulong.MaxValue;
				CheckRBNotInWorld();
				return;
			}
			IMyEntity topMostParent = base.Entity.GetTopMostParent();
			if (topMostParent.Physics != null && IsInWorld)
			{
				if (((MyPhysicsBody)topMostParent.Physics).HavokWorld != null)
				{
					Deactivate(m_world);
					return;
				}
				RigidBody = null;
				RigidBody2 = null;
				CharacterProxy = null;
			}
		}

		private void CheckRBNotInWorld()
		{
			if (RigidBody != null && RigidBody.InWorld)
			{
				MyAnalyticsHelper.ReportActivityStart((MyEntity)base.Entity, "RigidBody in world after deactivation", "", "DevNote", "", expectActivityEnd: false);
				RigidBody.RemoveFromWorld();
			}
			if (RigidBody2 != null && RigidBody2.InWorld)
			{
				RigidBody2.RemoveFromWorld();
			}
		}

		public virtual void Deactivate(object world)
		{
			if (RigidBody != null && !RigidBody.InWorld)
			{
				return;
			}
			if (IsRagdollModeActive)
			{
				ReactivateRagdoll = true;
				CloseRagdollMode(world as HkWorld);
			}
			if (IsInWorld && RigidBody != null && !RigidBody.IsActive)
			{
				if (!RigidBody.IsFixed)
				{
					RigidBody.Activate();
				}
				else
				{
					BoundingBoxD box = base.Entity.PositionComp.WorldAABB;
					box.Inflate(0.5);
					MyPhysics.ActivateInBox(ref box);
				}
			}
			if (m_constraints.Count > 0)
			{
				m_world.LockCriticalOperations();
				foreach (HkConstraint constraint in m_constraints)
				{
					if (!constraint.IsDisposed)
					{
						m_world.RemoveConstraint(constraint);
					}
				}
				m_world.UnlockCriticalOperations();
			}
			if (BreakableBody != null && m_world.DestructionWorld != null)
			{
				m_world.DestructionWorld.RemoveBreakableBody(BreakableBody);
			}
			else if (RigidBody != null && !RigidBody.IsDisposed)
			{
				m_world.RemoveRigidBody(RigidBody);
			}
			if (RigidBody2 != null && !RigidBody2.IsDisposed)
			{
				m_world.RemoveRigidBody(RigidBody2);
			}
			if (CharacterProxy != null)
			{
				CharacterProxy.Deactivate(m_world);
			}
			CheckRBNotInWorld();
			m_world = null;
			IsInWorld = false;
		}

		private void DeactivateBatchInternal(object world)
		{
			if (m_world != null)
			{
				if (IsRagdollModeActive)
				{
					ReactivateRagdoll = true;
					CloseRagdollMode(world as HkWorld);
				}
				if (BreakableBody != null && m_world.DestructionWorld != null)
				{
					m_world.DestructionWorld.RemoveBreakableBody(BreakableBody);
				}
				else if (RigidBody != null)
				{
					m_world.RemoveRigidBodyBatch(RigidBody);
				}
				if (RigidBody2 != null)
				{
					m_world.RemoveRigidBodyBatch(RigidBody2);
				}
				if (CharacterProxy != null)
				{
					CharacterProxy.Deactivate(m_world);
				}
				foreach (HkConstraint constraint in m_constraints)
				{
					if (IsConstraintValid(constraint, checkBodiesInWorld: false))
					{
						m_constraintsRemoveBatch.Add(constraint);
					}
				}
				m_enabled = false;
				if (EnabledChanged != null)
				{
					EnabledChanged();
				}
				m_world = null;
				IsInWorld = false;
			}
		}

		public virtual void DeactivateBatch(object world)
		{
			MyHierarchyComponentBase hierarchy = base.Entity.Hierarchy;
			if (hierarchy != null)
			{
				hierarchy.GetChildrenRecursive(m_batchedChildren);
				foreach (IMyEntity batchedChild in m_batchedChildren)
				{
					if (batchedChild.Physics != null && batchedChild.Physics.Enabled)
					{
						m_batchedBodies.Add((MyPhysicsBody)batchedChild.Physics);
					}
				}
				m_batchedChildren.Clear();
			}
			foreach (MyPhysicsBody batchedBody in m_batchedBodies)
			{
				batchedBody.DeactivateBatchInternal(world);
			}
			DeactivateBatchInternal(world);
		}

		public void FinishAddBatch()
		{
			ActivateCollision();
			if (EnabledChanged != null)
			{
				EnabledChanged();
			}
			foreach (HkConstraint item in m_constraintsAddBatch)
			{
				if (IsConstraintValid(item))
				{
					m_world.AddConstraint(item);
				}
			}
			m_constraintsAddBatch.Clear();
			if (CharacterProxy != null)
			{
				HkEntity rigidBody = CharacterProxy.GetRigidBody();
				if (rigidBody != null)
				{
					m_world.RefreshCollisionFilterOnEntity(rigidBody);
				}
			}
			if (ReactivateRagdoll)
			{
				GetRigidBodyMatrix(out m_bodyMatrix, useCenterOffset: false);
				ActivateRagdoll(m_bodyMatrix);
				ReactivateRagdoll = false;
			}
		}

		public void FinishRemoveBatch(object userData)
		{
			HkWorld hkWorld = (HkWorld)userData;
			foreach (HkConstraint item in m_constraintsRemoveBatch)
			{
				if (IsConstraintValid(item, checkBodiesInWorld: false))
				{
					hkWorld.RemoveConstraint(item);
				}
			}
			if (IsRagdollModeActive)
			{
				ReactivateRagdoll = true;
				CloseRagdollMode(hkWorld);
			}
			m_constraintsRemoveBatch.Clear();
		}

		public override void ForceActivate()
		{
			if (IsInWorld && RigidBody != null)
			{
				RigidBody.ForceActivate();
				m_world.RigidBodyActivated(RigidBody);
			}
		}

		public virtual void OnMotionKinematic(HkRigidBody rbo)
		{
			if (rbo.MarkedForVelocityRecompute)
			{
				RigidBody.SetCustomVelocity(RigidBody.LinearVelocity, valid: true);
			}
		}

		public virtual void OnMotion(HkRigidBody rbo, float step, bool fromParent = false)
		{
			if (rbo == RigidBody2 || base.Entity == null || (!IsSubpart && base.Entity.Parent != null) || Flags == RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE || IsPhantom)
			{
				return;
			}
			if (base.CanUpdateAccelerations)
			{
				UpdateAccelerations();
			}
			if (RigidBody2 != null)
			{
				if (IsSubpart || m_lastWorld != HavokWorld)
				{
					Matrix rigidBodyMatrix = rbo.GetRigidBodyMatrix();
					RigidBody2.Motion.SetWorldMatrix(rigidBodyMatrix);
					RigidBody2.LinearVelocity = rbo.LinearVelocity;
					RigidBody2.AngularVelocity = rbo.AngularVelocity;
					m_lastWorld = HavokWorld;
					return;
				}
				Matrix matrix = rbo.PredictRigidBodyMatrix(0.0166666675f, HavokWorld);
				Quaternion nextOrientation = Quaternion.CreateFromRotationMatrix(matrix);
				Vector4 nextPosition = new Vector4(matrix.Translation.X, matrix.Translation.Y, matrix.Translation.Z, 0f);
				HkKeyFrameUtility.ApplyHardKeyFrame(ref nextPosition, ref nextOrientation, 59.9999962f, RigidBody2);
				RigidBody2.AngularVelocity = RigidBody2.AngularVelocity * 0.1f + rbo.AngularVelocity * 0.9f;
			}
			m_lastWorld = HavokWorld;
			int num = (base.Entity is MyFloatingObject) ? 600 : 60;
			Vector3D vector3D = GetWorldMatrix().Translation - base.Entity.PositionComp.GetPosition();
			m_motionCounter++;
			float num2 = LinearVelocity.LengthSquared();
			if (num2 > 0.0001f || m_motionCounter > num || vector3D.LengthSquared() > 9.9999999747524271E-07 || AngularVelocity.LengthSquared() > 0.0001f || fromParent)
			{
				float num3 = rbo.MaxLinearVelocity * 1.1f;
				if (num2 > num3 * num3)
				{
					MyLog.Default.WriteLine("Clamping velocity for: " + base.Entity.EntityId + " " + num2.ToString("F2") + "->" + num3.ToString("F2"));
					rbo.LinearVelocity *= 1f / (float)Math.Sqrt(num2) * num3;
				}
				MatrixD worldMatrix = GetWorldMatrix();
				base.Entity.PositionComp.SetWorldMatrix(worldMatrix, this);
				UpdateInterpolatedVelocities(rbo, moved: true);
				m_motionCounter = 0;
				m_bodyMatrix = rbo.GetRigidBodyMatrix();
				foreach (MyPhysicsBody child in WeldInfo.Children)
				{
					child.OnMotion(rbo, step, fromParent: true);
				}
				if (WeldInfo.Parent == null)
				{
					UpdateCluster();
				}
			}
			else
			{
				UpdateInterpolatedVelocities(rbo, moved: false);
			}
		}

		private void UpdateInterpolatedVelocities(HkRigidBody rb, bool moved)
		{
			if (rb.MarkedForVelocityRecompute)
			{
				Vector3D centerOfMassWorld = CenterOfMassWorld;
				if (m_lastComPosition.HasValue && m_lastComLocal == rb.CenterOfMassLocal)
				{
					Vector3 velocity = Vector3.Zero;
					if (moved)
					{
						velocity = (centerOfMassWorld - m_lastComPosition.Value) / 0.01666666753590107;
					}
					rb.SetCustomVelocity(velocity, valid: true);
				}
				rb.MarkedForVelocityRecompute = false;
				m_lastComPosition = centerOfMassWorld;
				m_lastComLocal = rb.CenterOfMassLocal;
			}
			else if (m_lastComPosition.HasValue)
			{
				m_lastComPosition = null;
				rb.SetCustomVelocity(Vector3.Zero, valid: false);
			}
		}

		public void SynchronizeKeyframedRigidBody()
		{
			if (RigidBody != null && RigidBody2 != null && RigidBody.IsActive != RigidBody2.IsActive)
			{
				if (RigidBody.IsActive)
				{
					RigidBody2.IsActive = true;
					return;
				}
				RigidBody2.LinearVelocity = Vector3.Zero;
				RigidBody2.AngularVelocity = Vector3.Zero;
				RigidBody2.IsActive = false;
			}
		}

		public override MatrixD GetWorldMatrix()
		{
			if (WeldInfo.Parent != null)
			{
				return WeldInfo.Transform * WeldInfo.Parent.GetWorldMatrix();
			}
			MatrixD matrix;
			if (RigidBody != null)
			{
				matrix = RigidBody.GetRigidBodyMatrix();
				matrix.Translation += Offset;
			}
			else if (RigidBody2 != null)
			{
				matrix = RigidBody2.GetRigidBodyMatrix();
				matrix.Translation += Offset;
			}
			else if (CharacterProxy != null)
			{
				MatrixD matrixD = CharacterProxy.GetRigidBodyTransform();
				matrixD.Translation = CharacterProxy.Position + Offset;
				matrix = matrixD;
			}
			else
			{
				if ((Ragdoll != null) & IsRagdollModeActive)
				{
					matrix = Ragdoll.WorldMatrix;
					matrix.Translation += Offset;
					return matrix;
				}
				matrix = MatrixD.Identity;
			}
			if (base.Center != Vector3.Zero)
			{
				matrix.Translation -= Vector3D.TransformNormal(base.Center, ref matrix);
			}
			return matrix;
		}

		public override Vector3 GetVelocityAtPoint(Vector3D worldPos)
		{
			Vector3 worldPos2 = WorldToCluster(worldPos);
			if (RigidBody != null)
			{
				return RigidBody.GetVelocityAtPoint(worldPos2);
			}
			return Vector3.Zero;
		}

		public override void GetVelocityAtPointLocal(ref Vector3D worldPos, out Vector3 linearVelocity)
		{
			Vector3 vector = worldPos - CenterOfMassWorld;
			linearVelocity = Vector3.Cross(AngularVelocityLocal, vector);
			linearVelocity.Add(LinearVelocity);
		}

		public override void OnWorldPositionChanged(object source)
		{
			if (!IsInWorld)
			{
				return;
			}
			Vector3 velocity = Vector3.Zero;
			IMyEntity topMostParent = base.Entity.GetTopMostParent();
			if (topMostParent.Physics != null)
			{
				velocity = topMostParent.Physics.LinearVelocity;
			}
			if (!IsWelded && ClusterObjectID != ulong.MaxValue)
			{
				MyPhysics.MoveObject(ClusterObjectID, topMostParent.WorldAABB, velocity);
			}
			GetRigidBodyMatrix(out Matrix m);
			if (m.EqualsFast(ref m_bodyMatrix) && CharacterProxy == null)
			{
				return;
			}
			m_bodyMatrix = m;
			if (RigidBody != null)
			{
				RigidBody.SetWorldMatrix(m_bodyMatrix);
			}
			if (RigidBody2 != null)
			{
				RigidBody2.SetWorldMatrix(m_bodyMatrix);
			}
			if (CharacterProxy != null)
			{
				CharacterProxy.Speed = 0f;
				CharacterProxy.SetRigidBodyTransform(ref m_bodyMatrix);
			}
			if (Ragdoll == null || !IsRagdollModeActive)
			{
				return;
			}
			bool flag = source is MyCockpit;
			bool flag2 = source == MyGridPhysicalHierarchy.Static;
			if (flag || flag2)
			{
				if (flag)
				{
					Ragdoll.ResetToRigPose();
				}
				GetRigidBodyMatrix(out Matrix m2, useCenterOffset: false);
				MyCharacter myCharacter = (MyCharacter)base.Entity;
				bool flag3 = flag2 && !myCharacter.IsClientPredicted;
				bool flag4 = myCharacter.m_positionResetFromServer || Vector3D.DistanceSquared(Ragdoll.WorldMatrix.Translation, m2.Translation) > 0.5;
				Ragdoll.SetWorldMatrix(m2, !flag4 && flag3, updateVelocities: true);
				if (flag)
				{
					SetRagdollVelocities();
				}
			}
		}

		protected void GetRigidBodyMatrix(out Matrix m, bool useCenterOffset = true)
		{
			MatrixD worldMatrix = base.Entity.WorldMatrix;
			if (base.Center != Vector3.Zero && useCenterOffset)
			{
				worldMatrix.Translation += Vector3.TransformNormal(base.Center, worldMatrix);
			}
			worldMatrix.Translation -= Offset;
			m = worldMatrix;
		}

		protected Matrix GetRigidBodyMatrix()
		{
			Vector3 v = Vector3.TransformNormal(base.Center, base.Entity.WorldMatrix);
			Vector3D objectOffset = MyPhysics.GetObjectOffset(ClusterObjectID);
			return Matrix.CreateWorld((Vector3D)v + base.Entity.GetPosition() - objectOffset, base.Entity.WorldMatrix.Forward, base.Entity.WorldMatrix.Up);
		}

		protected Matrix GetRigidBodyMatrix(MatrixD worldMatrix)
		{
			if (base.Center != Vector3.Zero)
			{
				worldMatrix.Translation += Vector3D.TransformNormal(base.Center, ref worldMatrix);
			}
			worldMatrix.Translation -= Offset;
			return worldMatrix;
		}

		public virtual void ChangeQualityType(HkCollidableQualityType quality)
		{
			RigidBody.Quality = quality;
		}

		private static bool IsConstraintValid(HkConstraint constraint, bool checkBodiesInWorld)
		{
			if (constraint == null)
			{
				return false;
			}
			if (constraint.IsDisposed)
			{
				return false;
			}
			HkRigidBody rigidBodyA = constraint.RigidBodyA;
			HkRigidBody rigidBodyB = constraint.RigidBodyB;
			if (rigidBodyA == null || rigidBodyB == null)
			{
				return false;
			}
			if (checkBodiesInWorld)
			{
				if (!rigidBodyA.InWorld || !rigidBodyB.InWorld)
				{
					return false;
				}
				if (((MyPhysicsBody)rigidBodyA.UserObject).HavokWorld != ((MyPhysicsBody)rigidBodyB.UserObject).HavokWorld)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsConstraintValid(HkConstraint constraint)
		{
			return IsConstraintValid(constraint, checkBodiesInWorld: true);
		}

		public void AddConstraint(HkConstraint constraint)
		{
			if (IsWelded)
			{
				WeldInfo.Parent.AddConstraint(constraint);
			}
			else if (HavokWorld != null && !(RigidBody == null) && IsConstraintValid(constraint))
			{
				m_constraints.Add(constraint);
				HavokWorld.AddConstraint(constraint);
				if (!MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS && !Sync.IsServer)
				{
					UpdateConstraintForceDisable(constraint);
				}
			}
		}

		public void UpdateConstraintsForceDisable()
		{
			if (!MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS && !Sync.IsServer)
			{
				foreach (HkConstraint constraint in m_constraints)
				{
					UpdateConstraintForceDisable(constraint);
				}
			}
		}

		public void UpdateConstraintForceDisable(HkConstraint constraint)
		{
			if (!(constraint.RigidBodyA.GetEntity(0u) is MyCharacter) && !(constraint.RigidBodyB.GetEntity(0u) is MyCharacter))
			{
				MyCubeGrid myCubeGrid = base.Entity as MyCubeGrid;
				if ((myCubeGrid == null || !myCubeGrid.IsClientPredicted) && !IsPhantomOrSubPart(constraint.RigidBodyA) && !IsPhantomOrSubPart(constraint.RigidBodyB))
				{
					constraint.ForceDisabled = true;
				}
				else
				{
					constraint.ForceDisabled = false;
				}
			}
		}

		private static bool IsPhantomOrSubPart(HkRigidBody rigidBody)
		{
			MyPhysicsBody myPhysicsBody = (MyPhysicsBody)rigidBody.UserObject;
			if (!myPhysicsBody.IsPhantom)
			{
				return myPhysicsBody.IsSubpart;
			}
			return true;
		}

		public void RemoveConstraint(HkConstraint constraint)
		{
			if (IsWelded)
			{
				m_constraints.Remove(constraint);
				WeldInfo.Parent.RemoveConstraint(constraint);
				return;
			}
			m_constraints.Remove(constraint);
			if (HavokWorld != null)
			{
				HavokWorld.RemoveConstraint(constraint);
			}
		}

		public override Vector3D WorldToCluster(Vector3D worldPos)
		{
			return worldPos - Offset;
		}

		public override Vector3D ClusterToWorld(Vector3 clusterPos)
		{
			return (Vector3D)clusterPos + Offset;
		}

		public void EnableBatched()
		{
			if (!m_enabled)
			{
				m_enabled = true;
				if (base.Entity.InScene)
				{
					m_batchRequest = true;
					Activate();
					m_batchRequest = false;
				}
				if (EnabledChanged != null)
				{
					EnabledChanged();
				}
			}
		}

		public override void Activate()
		{
			if (Enabled)
			{
				IMyEntity topMostParent = base.Entity.GetTopMostParent();
				if (topMostParent != base.Entity && topMostParent.Physics != null)
				{
					Activate(((MyPhysicsBody)topMostParent.Physics).HavokWorld, ulong.MaxValue);
				}
				else if (ClusterObjectID == ulong.MaxValue)
				{
					ClusterObjectID = MyPhysics.AddObject(base.Entity.WorldAABB, this, null, ((MyEntity)base.Entity).DebugName, base.Entity.EntityId, m_batchRequest);
				}
			}
		}

		public virtual void Activate(object world, ulong clusterObjectID)
		{
			m_world = (HkWorld)world;
			if (m_world == null)
			{
				return;
			}
			ClusterObjectID = clusterObjectID;
			ActivateCollision();
			IsInWorld = true;
			GetRigidBodyMatrix(out m_bodyMatrix);
			if (BreakableBody != null)
			{
				if (RigidBody != null)
				{
					RigidBody.SetWorldMatrix(m_bodyMatrix);
				}
				if (Sync.IsServer)
				{
					m_world.DestructionWorld.AddBreakableBody(BreakableBody);
				}
				else if (RigidBody != null)
				{
					m_world.AddRigidBody(RigidBody);
				}
			}
			else if (RigidBody != null)
			{
				RigidBody.SetWorldMatrix(m_bodyMatrix);
				m_world.AddRigidBody(RigidBody);
			}
			if (RigidBody2 != null)
			{
				RigidBody2.SetWorldMatrix(m_bodyMatrix);
				m_world.AddRigidBody(RigidBody2);
			}
			if (CharacterProxy != null)
			{
				RagdollSystemGroupCollisionFilterID = 0;
				CharacterSystemGroupCollisionFilterID = m_world.GetCollisionFilter().GetNewSystemGroup();
				CharacterCollisionFilter = HkGroupFilter.CalcFilterInfo(18, CharacterSystemGroupCollisionFilterID, 0, 0);
				CharacterProxy.SetCollisionFilterInfo(CharacterCollisionFilter);
				CharacterProxy.SetRigidBodyTransform(ref m_bodyMatrix);
				CharacterProxy.Activate(m_world);
			}
			if (ReactivateRagdoll)
			{
				GetRigidBodyMatrix(out m_bodyMatrix, useCenterOffset: false);
				ActivateRagdoll(m_bodyMatrix);
				ReactivateRagdoll = false;
			}
			if (SwitchToRagdollModeOnActivate)
			{
				_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
				SwitchToRagdollModeOnActivate = false;
				SwitchToRagdollMode(m_ragdollDeadMode);
			}
			m_world.LockCriticalOperations();
			foreach (HkConstraint constraint in m_constraints)
			{
				if (IsConstraintValid(constraint))
				{
					m_world.AddConstraint(constraint);
				}
			}
			m_world.UnlockCriticalOperations();
			m_enabled = true;
		}

		private void ActivateBatchInternal(object world)
		{
			m_world = (HkWorld)world;
			IsInWorld = true;
			GetRigidBodyMatrix(out m_bodyMatrix);
			if (RigidBody != null)
			{
				RigidBody.SetWorldMatrix(m_bodyMatrix);
				m_world.AddRigidBodyBatch(RigidBody);
			}
			if (RigidBody2 != null)
			{
				RigidBody2.SetWorldMatrix(m_bodyMatrix);
				m_world.AddRigidBodyBatch(RigidBody2);
			}
			if (CharacterProxy != null)
			{
				RagdollSystemGroupCollisionFilterID = 0;
				CharacterSystemGroupCollisionFilterID = m_world.GetCollisionFilter().GetNewSystemGroup();
				CharacterCollisionFilter = HkGroupFilter.CalcFilterInfo(18, CharacterSystemGroupCollisionFilterID, 1, 1);
				CharacterProxy.SetCollisionFilterInfo(CharacterCollisionFilter);
				CharacterProxy.SetRigidBodyTransform(ref m_bodyMatrix);
				CharacterProxy.Activate(m_world);
			}
			if (SwitchToRagdollModeOnActivate)
			{
				_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
				SwitchToRagdollModeOnActivate = false;
				SwitchToRagdollMode(m_ragdollDeadMode);
			}
			foreach (HkConstraint constraint in m_constraints)
			{
				m_constraintsAddBatch.Add(constraint);
			}
			m_enabled = true;
		}

		public virtual void ActivateBatch(object world, ulong clusterObjectID)
		{
			IMyEntity topMostParent = base.Entity.GetTopMostParent();
			if (topMostParent == base.Entity || topMostParent.Physics == null)
			{
				ClusterObjectID = clusterObjectID;
				foreach (MyPhysicsBody batchedBody in m_batchedBodies)
				{
					batchedBody.ActivateBatchInternal(world);
				}
				m_batchedBodies.Clear();
				ActivateBatchInternal(world);
			}
		}

		public void UpdateCluster()
		{
			if (!MyPerGameSettings.LimitedWorld && base.Entity != null && !base.Entity.Closed)
			{
				MyPhysics.MoveObject(ClusterObjectID, base.Entity.WorldAABB, LinearVelocity);
			}
		}

		[Conditional("DEBUG")]
		private void CheckUnlockedSpeeds()
		{
			if (IsPhantom || IsSubpart || base.Entity.Parent != null)
			{
				_ = (RigidBody == null);
			}
		}

		void MyClusterTree.IMyActivationHandler.Activate(object userData, ulong clusterObjectID)
		{
			Activate(userData, clusterObjectID);
		}

		void MyClusterTree.IMyActivationHandler.Deactivate(object userData)
		{
			Deactivate(userData);
		}

		void MyClusterTree.IMyActivationHandler.ActivateBatch(object userData, ulong clusterObjectID)
		{
			ActivateBatch(userData, clusterObjectID);
		}

		void MyClusterTree.IMyActivationHandler.DeactivateBatch(object userData)
		{
			DeactivateBatch(userData);
		}

		void MyClusterTree.IMyActivationHandler.FinishAddBatch()
		{
			FinishAddBatch();
		}

		void MyClusterTree.IMyActivationHandler.FinishRemoveBatch(object userData)
		{
			FinishRemoveBatch(userData);
		}

		public virtual HkShape GetShape()
		{
			if (WeldedRigidBody != null)
			{
				return WeldedRigidBody.GetShape();
			}
			HkShape shape = RigidBody.GetShape();
			if (shape.ShapeType == HkShapeType.List)
			{
				HkShapeContainerIterator container = RigidBody.GetShape().GetContainer();
				while (container.IsValid)
				{
					HkShape shape2 = container.GetShape(container.CurrentShapeKey);
					if (RigidBody.GetGcRoot() == shape2.UserData)
					{
						return shape2;
					}
					container.Next();
				}
			}
			return shape;
		}

		private static HkMassProperties? GetMassPropertiesFromDefinition(MyPhysicsBodyComponentDefinition physicsBodyComponentDefinition, MyModelComponentDefinition modelComponentDefinition)
		{
			HkMassProperties? result = null;
			MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType massPropertiesComputation = physicsBodyComponentDefinition.MassPropertiesComputation;
			if (massPropertiesComputation != 0 && massPropertiesComputation == MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType.Box)
			{
				result = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(modelComponentDefinition.Size / 2f, MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(modelComponentDefinition.Mass) : modelComponentDefinition.Mass);
			}
			return result;
		}

		private void OnModelChanged(MyEntityContainerEventExtensions.EntityEventParams eventParams)
		{
			Close();
			InitializeRigidBodyFromModel();
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			Definition = (definition as MyPhysicsBodyComponentDefinition);
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (Definition != null)
			{
				InitializeRigidBodyFromModel();
				this.RegisterForEntityEvent(MyModelComponent.ModelChanged, OnModelChanged);
			}
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			if (Definition != null)
			{
				Enabled = true;
				if (Definition.ForceActivate)
				{
					ForceActivate();
				}
			}
		}

		private void InitializeRigidBodyFromModel()
		{
			if (Definition != null && RigidBody == null && Definition.CreateFromCollisionObject && base.Container.Has<MyModelComponent>())
			{
				MyModelComponent myModelComponent = base.Container.Get<MyModelComponent>();
				if (myModelComponent.Definition != null && myModelComponent.ModelCollision != null && myModelComponent.ModelCollision.HavokCollisionShapes.Length >= 1)
				{
					HkMassProperties? massPropertiesFromDefinition = GetMassPropertiesFromDefinition(Definition, myModelComponent.Definition);
					int collisionFilter = (Definition.CollisionLayer != null) ? MyPhysics.GetCollisionLayer(Definition.CollisionLayer) : 15;
					CreateFromCollisionObject(myModelComponent.ModelCollision.HavokCollisionShapes[0], Vector3.Zero, base.Entity.WorldMatrix, massPropertiesFromDefinition, collisionFilter);
				}
			}
		}

		public override void UpdateFromSystem()
		{
			if (Definition != null && (Definition.UpdateFlags & MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags.Gravity) != 0 && (MyFakes.ENABLE_PLANETS || MyInput.Static.ENABLE_DEVELOPER_KEYS) && base.Entity != null && base.Entity.PositionComp != null && Enabled && RigidBody != null)
			{
				RigidBody.Gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.Entity.PositionComp.GetPosition());
			}
		}

		protected void NotifyConstraintsAddedToWorld()
		{
			foreach (HkConstraint notifyConstraint in m_notifyConstraints)
			{
				notifyConstraint.NotifyAddedToWorld();
			}
			m_notifyConstraints.Clear();
		}

		protected void NotifyConstraintsRemovedFromWorld()
		{
			m_notifyConstraints.AssertEmpty();
			HkRigidBody rigidBody = RigidBody;
			if (rigidBody != null)
			{
				HkConstraint.GetAttachedConstraints(rigidBody, m_notifyConstraints);
			}
			foreach (HkConstraint notifyConstraint in m_notifyConstraints)
			{
				notifyConstraint.NotifyRemovedFromWorld();
			}
		}

		public virtual void FracturedBody_AfterReplaceBody(ref HkdReplaceBodyEvent e)
		{
			if (Sync.IsServer)
			{
				e.GetNewBodies(m_tmpLst);
				if (m_tmpLst.Count != 0)
				{
					MyPhysics.RemoveDestructions(RigidBody);
					foreach (HkdBreakableBodyInfo item in m_tmpLst)
					{
						HkdBreakableBody breakableBody = MyFracturedPiecesManager.Static.GetBreakableBody(item);
						MatrixD worldMatrix = breakableBody.GetRigidBody().GetRigidBodyMatrix();
						worldMatrix.Translation = ClusterToWorld(worldMatrix.Translation);
						if (MyDestructionHelper.CreateFracturePiece(breakableBody, ref worldMatrix, (base.Entity as MyFracturedPiece).OriginalBlocks) == null)
						{
							MyFracturedPiecesManager.Static.ReturnToPool(breakableBody);
						}
					}
					m_tmpLst.Clear();
					BreakableBody.AfterReplaceBody -= FracturedBody_AfterReplaceBody;
					MyFracturedPiecesManager.Static.RemoveFracturePiece(base.Entity as MyFracturedPiece, 0f);
				}
			}
		}

		public void CloseRagdoll()
		{
			if (Ragdoll != null)
			{
				if (IsRagdollModeActive)
				{
					CloseRagdollMode(HavokWorld);
				}
				if (Ragdoll.InWorld)
				{
					HavokWorld.RemoveRagdoll(Ragdoll);
				}
				Ragdoll.Dispose();
				Ragdoll = null;
			}
		}

		public void SwitchToRagdollMode(bool deadMode = true, int firstRagdollSubID = 1)
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.SwitchToRagdollMode");
			}
			if (HavokWorld == null || !Enabled)
			{
				SwitchToRagdollModeOnActivate = true;
				m_ragdollDeadMode = deadMode;
			}
			else if (!IsRagdollModeActive)
			{
				MatrixD worldMatrix = base.Entity.WorldMatrix;
				worldMatrix.Translation = WorldToCluster(worldMatrix.Translation);
				if (RagdollSystemGroupCollisionFilterID == 0)
				{
					RagdollSystemGroupCollisionFilterID = m_world.GetCollisionFilter().GetNewSystemGroup();
				}
				Ragdoll.SetToKeyframed();
				Ragdoll.GenerateRigidBodiesCollisionFilters(deadMode ? 18 : 31, RagdollSystemGroupCollisionFilterID, firstRagdollSubID);
				Ragdoll.ResetToRigPose();
				Ragdoll.SetWorldMatrix(worldMatrix, updateOnlyKeyframed: false, updateVelocities: false);
				if (deadMode)
				{
					Ragdoll.SetToDynamic();
					SetRagdollVelocities(null, RigidBody);
				}
				else
				{
					SetRagdollVelocities();
				}
				if (CharacterProxy != null && deadMode)
				{
					CharacterProxy.Deactivate(HavokWorld);
					CharacterProxy.Dispose();
					CharacterProxy = null;
				}
				if (RigidBody != null && deadMode)
				{
					RigidBody.Deactivate();
					HavokWorld.RemoveRigidBody(RigidBody);
					RigidBody.Dispose();
					RigidBody = null;
				}
				if (RigidBody2 != null && deadMode)
				{
					RigidBody2.Deactivate();
					HavokWorld.RemoveRigidBody(RigidBody2);
					RigidBody2.Dispose();
					RigidBody2 = null;
				}
				foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
				{
					rigidBody.UserObject = this;
					rigidBody.Motion.SetDeactivationClass(deadMode ? HkSolverDeactivation.High : HkSolverDeactivation.Medium);
				}
				Ragdoll.OptimizeInertiasOfConstraintTree();
				if (!Ragdoll.InWorld)
				{
					HavokWorld.AddRagdoll(Ragdoll);
				}
				Ragdoll.EnableConstraints();
				Ragdoll.Activate();
				m_ragdollDeadMode = deadMode;
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyPhysicsBody.SwitchToRagdollMode - FINISHED");
				}
			}
		}

		private void ActivateRagdoll(Matrix worldMatrix)
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.ActivateRagdoll");
			}
			if (Ragdoll != null && HavokWorld != null && !IsRagdollModeActive)
			{
				foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
				{
					rigidBody.UserObject = this;
				}
				Ragdoll.SetWorldMatrix(worldMatrix, updateOnlyKeyframed: false, updateVelocities: false);
				HavokWorld.AddRagdoll(Ragdoll);
				if (!MyFakes.ENABLE_JETPACK_RAGDOLL_COLLISIONS)
				{
					DisableRagdollBodiesCollisions();
				}
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyPhysicsBody.ActivateRagdoll - FINISHED");
				}
			}
		}

		private void OnRagdollAddedToWorld(HkRagdoll ragdoll)
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			Ragdoll.Activate();
			Ragdoll.EnableConstraints();
			HkConstraintStabilizationUtil.StabilizeRagdollInertias(ragdoll, 1f, 0f);
		}

		public void CloseRagdollMode()
		{
			CloseRagdollMode(HavokWorld);
		}

		public void CloseRagdollMode(HkWorld world)
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			if (IsRagdollModeActive && world != null)
			{
				foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
				{
					rigidBody.UserObject = null;
				}
				Ragdoll.Deactivate();
				world.RemoveRagdoll(Ragdoll);
				_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			}
		}

		public void SetRagdollDefaults()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.SetRagdollDefaults");
			}
			bool isKeyframed = Ragdoll.IsKeyframed;
			Ragdoll.SetToDynamic();
			float num = (base.Entity as MyCharacter).Definition.Mass;
			if (num <= 1f)
			{
				num = 80f;
			}
			float num2 = 0f;
			foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
			{
				float num3 = 0f;
				rigidBody.GetShape().GetLocalAABB(0.01f, out Vector4 min, out Vector4 max);
				num3 = (max - min).Length();
				num2 += num3;
			}
			if (num2 <= 0f)
			{
				num2 = 1f;
			}
			foreach (HkRigidBody rigidBody2 in Ragdoll.RigidBodies)
			{
				rigidBody2.MaxLinearVelocity = 1000f;
				rigidBody2.MaxAngularVelocity = 1000f;
				HkShape shape = rigidBody2.GetShape();
				shape.GetLocalAABB(0.01f, out Vector4 min2, out Vector4 max2);
				float num4 = (max2 - min2).Length();
				float num5 = num / num2 * num4;
				rigidBody2.Mass = (MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(num5) : num5);
				float convexRadius = shape.ConvexRadius;
				if (shape.ShapeType == HkShapeType.Capsule)
				{
					HkCapsuleShape hkCapsuleShape = (HkCapsuleShape)shape;
					HkMassProperties hkMassProperties = HkInertiaTensorComputer.ComputeCapsuleVolumeMassProperties(hkCapsuleShape.VertexA, hkCapsuleShape.VertexB, convexRadius, rigidBody2.Mass);
					rigidBody2.InertiaTensor = hkMassProperties.InertiaTensor;
				}
				else
				{
					HkMassProperties hkMassProperties2 = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(Vector3.One * num4 * 0.5f, rigidBody2.Mass);
					rigidBody2.InertiaTensor = hkMassProperties2.InertiaTensor;
				}
				rigidBody2.AngularDamping = 0.005f;
				rigidBody2.LinearDamping = 0f;
				rigidBody2.Friction = 6f;
				rigidBody2.AllowedPenetrationDepth = 0.1f;
				rigidBody2.Restitution = 0.05f;
			}
			Ragdoll.OptimizeInertiasOfConstraintTree();
			if (isKeyframed)
			{
				Ragdoll.SetToKeyframed();
			}
			foreach (HkConstraint constraint in Ragdoll.Constraints)
			{
				if (constraint.ConstraintData is HkRagdollConstraintData)
				{
					(constraint.ConstraintData as HkRagdollConstraintData).MaxFrictionTorque = (MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(0.5f) : 3f);
				}
				else if (constraint.ConstraintData is HkFixedConstraintData)
				{
					HkFixedConstraintData obj = constraint.ConstraintData as HkFixedConstraintData;
					obj.MaximumLinearImpulse = 3.40282E+28f;
					obj.MaximumAngularImpulse = 3.40282E+28f;
				}
				else if (constraint.ConstraintData is HkHingeConstraintData)
				{
					HkHingeConstraintData obj2 = constraint.ConstraintData as HkHingeConstraintData;
					obj2.MaximumAngularImpulse = 3.40282E+28f;
					obj2.MaximumLinearImpulse = 3.40282E+28f;
				}
				else if (constraint.ConstraintData is HkLimitedHingeConstraintData)
				{
					(constraint.ConstraintData as HkLimitedHingeConstraintData).MaxFrictionTorque = (MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(0.5f) : 3f);
				}
			}
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.SetRagdollDefaults FINISHED");
			}
		}

		internal void DisableRagdollBodiesCollisions()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				_ = HavokWorld;
			}
			if (Ragdoll != null)
			{
				foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
				{
					uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(31, 0, 0, 0);
					rigidBody.SetCollisionFilterInfo(collisionFilterInfo);
					rigidBody.LinearVelocity = Vector3.Zero;
					rigidBody.AngularVelocity = Vector3.Zero;
					HavokWorld.RefreshCollisionFilterOnEntity(rigidBody);
				}
			}
		}

		private void ApplyForceTorqueOnRagdoll(Vector3? force, Vector3? torque, HkRagdoll ragdoll, ref Matrix transform)
		{
			foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
			{
				if (rigidBody != null)
				{
					Vector3 value = force.Value * rigidBody.Mass / ragdoll.Mass;
					transform = rigidBody.GetRigidBodyMatrix();
					AddForceTorqueBody(value, torque, null, rigidBody, ref transform);
				}
			}
		}

		private void ApplyImpuseOnRagdoll(Vector3? force, Vector3D? position, Vector3? torque, HkRagdoll ragdoll)
		{
			foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
			{
				Vector3 value = force.Value * rigidBody.Mass / ragdoll.Mass;
				ApplyImplusesWorld(value, position, torque, rigidBody);
			}
		}

		private void ApplyForceOnRagdoll(Vector3? force, Vector3D? position, HkRagdoll ragdoll)
		{
			foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
			{
				Vector3 value = force.Value * rigidBody.Mass / ragdoll.Mass;
				ApplyForceWorld(value, position, rigidBody);
			}
		}

		public void SetRagdollVelocities(List<int> bodiesToUpdate = null, HkRigidBody leadingBody = null)
		{
			List<HkRigidBody> rigidBodies = Ragdoll.RigidBodies;
			if (leadingBody == null && CharacterProxy != null)
			{
				HkRigidBody hitRigidBody = CharacterProxy.GetHitRigidBody();
				if (hitRigidBody != null)
				{
					leadingBody = hitRigidBody;
				}
			}
			if (leadingBody == null)
			{
				leadingBody = rigidBodies[0];
			}
			Vector3 angularVelocity = leadingBody.AngularVelocity;
			if (bodiesToUpdate != null)
			{
				foreach (int item in bodiesToUpdate)
				{
					HkRigidBody hkRigidBody = rigidBodies[item];
					hkRigidBody.AngularVelocity = angularVelocity;
					hkRigidBody.LinearVelocity = leadingBody.GetVelocityAtPoint(hkRigidBody.Position);
				}
			}
			else
			{
				foreach (HkRigidBody item2 in rigidBodies)
				{
					item2.AngularVelocity = angularVelocity;
					item2.LinearVelocity = leadingBody.GetVelocityAtPoint(item2.Position);
				}
			}
		}

		public void Weld(MyPhysicsComponentBase other, bool recreateShape = true)
		{
			Weld(other as MyPhysicsBody, recreateShape);
		}

		public void Weld(MyPhysicsBody other, bool recreateShape = true)
		{
			if (other.WeldInfo.Parent == this)
			{
				return;
			}
			if (other.IsWelded && !IsWelded)
			{
				other.Weld(this);
				return;
			}
			if (IsWelded)
			{
				WeldInfo.Parent.Weld(other);
				return;
			}
			if (other.WeldInfo.Children.Count > 0)
			{
				other.UnweldAll(insertInWorld: false);
			}
			if (WeldInfo.Children.Count == 0)
			{
				WeldedRigidBody = RigidBody;
				HkShape shape = RigidBody.GetShape();
				if (HavokWorld != null)
				{
					HavokWorld.RemoveRigidBody(WeldedRigidBody);
				}
				RigidBody = HkRigidBody.Clone(WeldedRigidBody);
				if (HavokWorld != null)
				{
					HavokWorld.AddRigidBody(RigidBody);
				}
				HkShape.SetUserData(shape, RigidBody);
				base.Entity.OnPhysicsChanged += WeldedEntity_OnPhysicsChanged;
				WeldInfo.UpdateMassProps(RigidBody);
			}
			else
			{
				GetShape();
			}
			other.Deactivate();
			MatrixD m = other.Entity.WorldMatrix * base.Entity.WorldMatrixInvScaled;
			other.WeldInfo.Transform = m;
			other.WeldInfo.UpdateMassProps(other.RigidBody);
			other.WeldedRigidBody = other.RigidBody;
			other.RigidBody = RigidBody;
			other.WeldInfo.Parent = this;
			other.ClusterObjectID = ClusterObjectID;
			WeldInfo.Children.Add(other);
			OnWelded(other);
			other.OnWelded(this);
		}

		private void Entity_OnClose(IMyEntity obj)
		{
			UnweldAll(insertInWorld: true);
		}

		private void WeldedEntity_OnPhysicsChanged(IMyEntity obj)
		{
			if (base.Entity != null && base.Entity.Physics != null)
			{
				foreach (MyPhysicsBody child in WeldInfo.Children)
				{
					if (child.Entity == null)
					{
						child.WeldInfo.Parent = null;
						WeldInfo.Children.Remove(child);
						if (obj.Physics != null)
						{
							Weld(obj.Physics as MyPhysicsBody);
						}
						break;
					}
				}
				RecreateWeldedShape(GetShape());
			}
		}

		public void RecreateWeldedShape()
		{
			if (WeldInfo.Children.Count != 0)
			{
				RecreateWeldedShape(GetShape());
			}
		}

		public void UpdateMassProps()
		{
			if (!RigidBody.IsFixedOrKeyframed)
			{
				if (WeldInfo.Parent != null)
				{
					WeldInfo.Parent.UpdateMassProps();
					return;
				}
				int num = 1 + WeldInfo.Children.Count;
				HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(num);
				array[0] = WeldInfo.MassElement;
				int num2 = 1;
				foreach (MyPhysicsBody child in WeldInfo.Children)
				{
					array[num2++] = child.WeldInfo.MassElement;
				}
				HkInertiaTensorComputer.CombineMassProperties(new Span<HkMassElement>(array, 0, num), out HkMassProperties massProperties);
				RigidBody.SetMassProperties(ref massProperties);
				ArrayPool<HkMassElement>.Shared.Return(array);
			}
		}

		private void RecreateWeldedShape(HkShape thisShape)
		{
			if (RigidBody == null || RigidBody.IsDisposed)
			{
				return;
			}
			if (WeldInfo.Children.Count == 0)
			{
				RigidBody.SetShape(thisShape);
				if (RigidBody2 != null)
				{
					RigidBody2.SetShape(thisShape);
				}
				return;
			}
			m_tmpShapeList.Add(thisShape);
			foreach (MyPhysicsBody child in WeldInfo.Children)
			{
				HkTransformShape shape = new HkTransformShape(child.WeldedRigidBody.GetShape(), ref child.WeldInfo.Transform);
				HkShape.SetUserData(shape, child.WeldedRigidBody);
				m_tmpShapeList.Add(shape);
				if (m_tmpShapeList.Count == 128)
				{
					break;
				}
			}
			HkSmartListShape shape2 = new HkSmartListShape(0);
			foreach (HkShape tmpShape in m_tmpShapeList)
			{
				shape2.AddShape(tmpShape);
			}
			RigidBody.SetShape(shape2);
			if (RigidBody2 != null)
			{
				RigidBody2.SetShape(shape2);
			}
			shape2.Base.RemoveReference();
			WeldedMarkBreakable();
			for (int i = 1; i < m_tmpShapeList.Count; i++)
			{
				m_tmpShapeList[i].RemoveReference();
			}
			m_tmpShapeList.Clear();
			UpdateMassProps();
		}

		private void WeldedMarkBreakable()
		{
			if (HavokWorld != null)
			{
				MyGridPhysics myGridPhysics = this as MyGridPhysics;
				if (myGridPhysics != null && (myGridPhysics.Entity as MyCubeGrid).BlocksDestructionEnabled)
				{
					HavokWorld.BreakOffPartsUtil.MarkPieceBreakable(RigidBody, 0u, myGridPhysics.Shape.BreakImpulse);
				}
				uint num = 1u;
				foreach (MyPhysicsBody child in WeldInfo.Children)
				{
					myGridPhysics = (child as MyGridPhysics);
					if (myGridPhysics != null && (myGridPhysics.Entity as MyCubeGrid).BlocksDestructionEnabled)
					{
						HavokWorld.BreakOffPartsUtil.MarkPieceBreakable(RigidBody, num, myGridPhysics.Shape.BreakImpulse);
					}
					num++;
				}
			}
		}

		public void UnweldAll(bool insertInWorld)
		{
			while (WeldInfo.Children.Count > 1)
			{
				Unweld(WeldInfo.Children.First(), insertInWorld, recreateShape: false);
			}
			if (WeldInfo.Children.Count > 0)
			{
				Unweld(WeldInfo.Children.First(), insertInWorld);
			}
		}

		public void Unweld(MyPhysicsBody other, bool insertToWorld = true, bool recreateShape = true)
		{
			if (IsWelded)
			{
				WeldInfo.Parent.Unweld(other, insertToWorld, recreateShape);
				return;
			}
			if (other.IsInWorld || RigidBody == null || other.WeldedRigidBody == null)
			{
				WeldInfo.Children.Remove(other);
				return;
			}
			Matrix rigidBodyMatrix = RigidBody.GetRigidBodyMatrix();
			other.WeldInfo.Parent = null;
			WeldInfo.Children.Remove(other);
			HkRigidBody rigidBody = other.RigidBody;
			other.RigidBody = other.WeldedRigidBody;
			other.WeldedRigidBody = null;
			if (!other.RigidBody.IsDisposed)
			{
				other.RigidBody.SetWorldMatrix(other.WeldInfo.Transform * rigidBodyMatrix);
				other.RigidBody.LinearVelocity = rigidBody.LinearVelocity;
				other.WeldInfo.MassElement.Tranform = Matrix.Identity;
				other.WeldInfo.Transform = Matrix.Identity;
				if (other.RigidBody2 != null)
				{
					other.OnMotion(other.RigidBody, 0f);
				}
			}
			other.ClusterObjectID = ulong.MaxValue;
			if (insertToWorld)
			{
				other.Activate();
				other.OnMotion(other.RigidBody, 0f);
			}
			if (WeldInfo.Children.Count == 0)
			{
				recreateShape = false;
				base.Entity.OnPhysicsChanged -= WeldedEntity_OnPhysicsChanged;
				base.Entity.OnClose -= Entity_OnClose;
				WeldedRigidBody.LinearVelocity = RigidBody.LinearVelocity;
				WeldedRigidBody.AngularVelocity = RigidBody.AngularVelocity;
				if (HavokWorld != null)
				{
					HavokWorld.RemoveRigidBody(RigidBody);
				}
				RigidBody.Dispose();
				RigidBody = WeldedRigidBody;
				WeldedRigidBody = null;
				RigidBody.SetWorldMatrix(rigidBodyMatrix);
				WeldInfo.Transform = Matrix.Identity;
				if (HavokWorld != null)
				{
					HavokWorld.AddRigidBody(RigidBody);
					ActivateCollision();
				}
				else if (!base.Entity.MarkedForClose)
				{
					Activate();
				}
				if (RigidBody2 != null)
				{
					RigidBody2.SetShape(RigidBody.GetShape());
				}
			}
			if (RigidBody != null && recreateShape)
			{
				RecreateWeldedShape(GetShape());
			}
			OnUnwelded(other);
			other.OnUnwelded(this);
		}

		public void Unweld(bool insertInWorld = true)
		{
			WeldInfo.Parent.Unweld(this, insertInWorld);
		}

		protected virtual void OnWelded(MyPhysicsBody other)
		{
		}

		protected virtual void OnUnwelded(MyPhysicsBody other)
		{
		}

		private void RemoveConstraints(HkRigidBody hkRigidBody)
		{
			foreach (HkConstraint constraint in m_constraints)
			{
				if (constraint.IsDisposed || constraint.RigidBodyA == hkRigidBody || constraint.RigidBodyB == hkRigidBody)
				{
					m_constraintsRemoveBatch.Add(constraint);
				}
			}
			foreach (HkConstraint item in m_constraintsRemoveBatch)
			{
				m_constraints.Remove(item);
				if (!item.IsDisposed && item.InWorld)
				{
					HavokWorld.RemoveConstraint(item);
				}
			}
			m_constraintsRemoveBatch.Clear();
		}
	}
}
