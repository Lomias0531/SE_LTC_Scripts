using Havok;
using System;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.SessionComponents;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Components
{
	[MyComponentType(typeof(MyPhysicsComponentBase))]
	public abstract class MyPhysicsComponentBase : MyEntityComponentBase
	{
		protected Vector3 m_lastLinearVelocity;

		protected Vector3 m_lastAngularVelocity;

		private Vector3 m_linearVelocity;

		private Vector3 m_angularVelocity;

		private Vector3 m_supportNormal;

		/// <summary>
		/// Must be set before creating rigid body
		/// </summary>
		public ushort ContactPointDelay = ushort.MaxValue;

		public Action EnabledChanged;

		public RigidBodyFlag Flags;

		/// <summary>
		/// Use something from Havok to detect this
		/// </summary>
		public bool IsPhantom;

		protected bool m_enabled;

		public bool ReportAllContacts
		{
			get
			{
				return ContactPointDelay == 0;
			}
			set
			{
				ContactPointDelay = (ushort)((!value) ? ushort.MaxValue : 0);
			}
		}

		public new IMyEntity Entity
		{
			get;
			protected set;
		}

		public bool CanUpdateAccelerations
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the type of the material.
		/// </summary>
		/// <value>
		/// The type of the material.
		/// </value>
		public MyStringHash MaterialType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="!:MyGameRigidBody" /> is static.
		/// </summary>
		/// <value>
		///   <c>true</c> if static; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsStatic => (Flags & RigidBodyFlag.RBF_STATIC) == RigidBodyFlag.RBF_STATIC;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="!:MyGameRigidBody" /> is kinematic.
		/// </summary>
		/// <value>
		///   <c>true</c> if kinematic; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsKinematic
		{
			get
			{
				if ((Flags & RigidBodyFlag.RBF_KINEMATIC) != RigidBodyFlag.RBF_KINEMATIC)
				{
					return (Flags & RigidBodyFlag.RBF_DOUBLED_KINEMATIC) == RigidBodyFlag.RBF_DOUBLED_KINEMATIC;
				}
				return true;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="!:MyPhysicsBody" /> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public virtual bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled == value)
				{
					return;
				}
				m_enabled = value;
				if (EnabledChanged != null)
				{
					EnabledChanged();
				}
				if (value)
				{
					if (Entity.InScene)
					{
						Activate();
					}
				}
				else
				{
					Deactivate();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [play collision cue enabled].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [play collision cue enabled]; otherwise, <c>false</c>.
		/// </value>
		public bool PlayCollisionCueEnabled
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the mass.
		/// </summary>
		/// <value>
		/// The mass.
		/// </value>
		public abstract float Mass
		{
			get;
		}

		public Vector3 Center
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the linear velocity.
		/// </summary>
		/// <value>
		/// The linear velocity.
		/// </value>
		public virtual Vector3 LinearVelocity
		{
			get
			{
				return m_linearVelocity;
			}
			set
			{
				m_linearVelocity = value;
			}
		}

		public virtual Vector3 LinearVelocityLocal
		{
			get
			{
				return m_linearVelocity;
			}
			set
			{
				m_linearVelocity = value;
			}
		}

		public virtual Vector3 AngularVelocity
		{
			get
			{
				return m_angularVelocity;
			}
			set
			{
				m_angularVelocity = value;
			}
		}

		public virtual Vector3 AngularVelocityLocal
		{
			get
			{
				return m_angularVelocity;
			}
			set
			{
				m_angularVelocity = value;
			}
		}

		public virtual Vector3 SupportNormal
		{
			get
			{
				return m_supportNormal;
			}
			set
			{
				m_supportNormal = value;
			}
		}

		public virtual Vector3 LinearAcceleration
		{
			get;
			protected set;
		}

		public virtual Vector3 AngularAcceleration
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets the linear damping.
		/// </summary>
		/// <value>
		/// The linear damping.
		/// </value>
		public abstract float LinearDamping
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the angular damping.
		/// </summary>
		/// <value>
		/// The angular damping.
		/// </value>
		public abstract float AngularDamping
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the speed.
		/// </summary>
		/// <value>
		/// The speed.
		/// </value>
		public abstract float Speed
		{
			get;
		}

		public abstract float Friction
		{
			get;
			set;
		}

		/// <summary>
		/// Obtain/set (default) rigid body of this physics object.
		/// </summary>
		public abstract HkRigidBody RigidBody
		{
			get;
			protected set;
		}

		/// <summary>
		/// Obtain/set secondary rigid body of this physics object (not used by default, it is used sometimes on grids for kinematic layer).
		/// </summary>
		public abstract HkRigidBody RigidBody2
		{
			get;
			protected set;
		}

		public abstract HkdBreakableBody BreakableBody
		{
			get;
			set;
		}

		public abstract bool IsMoving
		{
			get;
		}

		public abstract Vector3 Gravity
		{
			get;
			set;
		}

		public MyPhysicsComponentDefinitionBase Definition
		{
			get;
			private set;
		}

		public abstract bool IsActive
		{
			get;
		}

		public abstract bool HasRigidBody
		{
			get;
		}

		public abstract Vector3 CenterOfMassLocal
		{
			get;
		}

		public abstract Vector3D CenterOfMassWorld
		{
			get;
		}

		public virtual bool IsInWorld
		{
			get;
			protected set;
		}

		public virtual bool ShapeChangeInProgress
		{
			get;
			set;
		}

		public override string ComponentTypeDebugString => "Physics";

		/// <summary>
		/// </summary>
		/// <param name="arg1">Sender</param>
		/// <param name="arg2">Is-active</param>
		public abstract event Action<MyPhysicsComponentBase, bool> OnBodyActiveStateChanged;

		public virtual MyStringHash GetMaterialAt(Vector3D worldPos)
		{
			return MaterialType;
		}

		public abstract Vector3 GetVelocityAtPoint(Vector3D worldPos);

		public abstract void GetVelocityAtPointLocal(ref Vector3D worldPos, out Vector3 linearVelocity);

		public virtual void Close()
		{
			Deactivate();
			CloseRigidBody();
		}

		protected abstract void CloseRigidBody();

		/// <summary>
		/// Applies external force to the physics object.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="force">The force.</param>
		/// <param name="position">The position.</param>
		/// <param name="torque">The torque.</param>
		/// <param name="maxSpeed">Clamp max speed</param>
		/// <param name="applyImmediately">Apply immediately or enqueue to apply right before simulation</param>
		/// <param name="activeOnly">Only apply if the object is already active</param>
		public abstract void AddForce(MyPhysicsForceType type, Vector3? force, Vector3D? position, Vector3? torque, float? maxSpeed = null, bool applyImmediately = true, bool activeOnly = false);

		/// <summary>
		/// Applies the impulse.
		/// </summary>
		/// <param name="dir">The dir.</param>
		/// <param name="pos">The pos.</param>
		public abstract void ApplyImpulse(Vector3 dir, Vector3D pos);

		/// <summary>
		/// Clears the speeds.
		/// </summary>
		public abstract void ClearSpeed();

		/// <summary>
		/// Clear all dynamic values of physics object.
		/// </summary>
		public abstract void Clear();

		public abstract void CreateCharacterCollision(Vector3 center, float characterWidth, float characterHeight, float crouchHeight, float ladderHeight, float headSize, float headHeight, MatrixD worldTransform, float mass, ushort collisionLayer, bool isOnlyVertical, float maxSlope, float maxLimit, float maxSpeedRelativeToShip, bool networkProxy, float? maxForce);

		/// <summary>
		/// Debug draw of this physics object.
		/// </summary>
		public abstract void DebugDraw();

		/// <summary>
		/// Activates this rigid body in physics.
		/// </summary>
		public abstract void Activate();

		/// <summary>
		/// Deactivates this rigid body in physics.
		/// </summary>
		public abstract void Deactivate();

		public abstract void ForceActivate();

		public void UpdateAccelerations()
		{
			Vector3 linearVelocity = LinearVelocity;
			LinearAcceleration = (linearVelocity - m_lastLinearVelocity) * 60f;
			m_lastLinearVelocity = linearVelocity;
			Vector3 angularVelocity = AngularVelocity;
			AngularAcceleration = (angularVelocity - m_lastAngularVelocity) * 60f;
			m_lastAngularVelocity = angularVelocity;
		}

		/// <summary>
		/// Set the current linear and angular velocities of this physics body.
		/// </summary>
		public void SetSpeeds(Vector3 linear, Vector3 angular)
		{
			LinearVelocity = linear;
			AngularVelocity = angular;
			ClearAccelerations();
			SetActualSpeedsAsPrevious();
		}

		private void ClearAccelerations()
		{
			LinearAcceleration = Vector3.Zero;
			AngularAcceleration = Vector3.Zero;
		}

		private void SetActualSpeedsAsPrevious()
		{
			m_lastLinearVelocity = LinearVelocity;
			m_lastAngularVelocity = AngularVelocity;
		}

		/// <summary>
		/// Converts global space position to local cluster space.
		/// </summary>
		public abstract Vector3D WorldToCluster(Vector3D worldPos);

		/// <summary>
		/// Converts local cluster position to global space.
		/// </summary>
		public abstract Vector3D ClusterToWorld(Vector3 clusterPos);

		public abstract MatrixD GetWorldMatrix();

		public abstract void UpdateFromSystem();

		/// <summary>
		/// Called when [world position changed].
		/// </summary>
		/// <param name="source">The source object that caused this event.</param>
		public abstract void OnWorldPositionChanged(object source);

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			Definition = (definition as MyPhysicsComponentDefinitionBase);
			if (Definition != null)
			{
				Flags = Definition.RigidBodyFlags;
				if (Definition.LinearDamping.HasValue)
				{
					LinearDamping = Definition.LinearDamping.Value;
				}
				if (Definition.AngularDamping.HasValue)
				{
					AngularDamping = Definition.AngularDamping.Value;
				}
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			Entity = base.Container.Entity;
			if (Definition != null && Definition.UpdateFlags != 0)
			{
				MyPhysicsComponentSystem.Static.Register(this);
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (Definition != null && Definition.UpdateFlags != 0 && MyPhysicsComponentSystem.Static != null)
			{
				MyPhysicsComponentSystem.Static.Unregister(this);
			}
		}

		public override bool IsSerialized()
		{
			if (Definition != null)
			{
				return Definition.Serialize;
			}
			return false;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_PhysicsComponentBase obj = MyComponentFactory.CreateObjectBuilder(this) as MyObjectBuilder_PhysicsComponentBase;
			obj.LinearVelocity = LinearVelocity;
			obj.AngularVelocity = AngularVelocity;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase baseBuilder)
		{
			MyObjectBuilder_PhysicsComponentBase myObjectBuilder_PhysicsComponentBase = baseBuilder as MyObjectBuilder_PhysicsComponentBase;
			LinearVelocity = myObjectBuilder_PhysicsComponentBase.LinearVelocity;
			AngularVelocity = myObjectBuilder_PhysicsComponentBase.AngularVelocity;
		}
	}
}
