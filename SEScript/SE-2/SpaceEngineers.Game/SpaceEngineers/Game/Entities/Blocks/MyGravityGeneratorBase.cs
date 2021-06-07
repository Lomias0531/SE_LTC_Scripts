using Havok;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase)
	})]
	public abstract class MyGravityGeneratorBase : MyFunctionalBlock, IMyGizmoDrawableObject, SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase, IMyGravityProvider
	{
		protected class m_gravityAcceleration_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType gravityAcceleration;
				ISyncType result = gravityAcceleration = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyGravityGeneratorBase)P_0).m_gravityAcceleration = (Sync<float, SyncDirection.BothWays>)gravityAcceleration;
				return result;
			}
		}

		protected Color m_gizmoColor = new Vector4(0f, 1f, 0f, 0.196f);

		protected const float m_maxGizmoDrawDistance = 1000f;

		protected bool m_oldEmissiveState;

		protected readonly Sync<float, SyncDirection.BothWays> m_gravityAcceleration;

		protected MyConcurrentHashSet<VRage.ModAPI.IMyEntity> m_containedEntities = new MyConcurrentHashSet<VRage.ModAPI.IMyEntity>();

		private new MyGravityGeneratorBaseDefinition BlockDefinition => (MyGravityGeneratorBaseDefinition)base.BlockDefinition;

		public float GravityAcceleration
		{
			get
			{
				return m_gravityAcceleration;
			}
			set
			{
				if ((float)m_gravityAcceleration != value)
				{
					m_gravityAcceleration.Value = value;
				}
			}
		}

		float SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase.GravityAcceleration
		{
			get
			{
				return GravityAcceleration;
			}
			set
			{
				GravityAcceleration = MathHelper.Clamp(value, BlockDefinition.MinGravityAcceleration, BlockDefinition.MaxGravityAcceleration);
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase.Gravity => GravityAcceleration / 9.81f;

		float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase.GravityAcceleration
		{
			get
			{
				return GravityAcceleration;
			}
			set
			{
				GravityAcceleration = MathHelper.Clamp(value, BlockDefinition.MinGravityAcceleration, BlockDefinition.MaxGravityAcceleration);
			}
		}

		public abstract bool IsPositionInRange(Vector3D worldPoint);

		public abstract Vector3 GetWorldGravity(Vector3D worldPoint);

		protected abstract float CalculateRequiredPowerInput();

		protected abstract HkShape GetHkShape();

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			InitializeSinkComponent();
			base.Init(objectBuilder, cubeGrid);
			if (base.CubeGrid.CreatePhysics)
			{
				if (MyFakes.ENABLE_GRAVITY_PHANTOM)
				{
					HkBvShape shape = CreateFieldShape();
					base.Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_KINEMATIC);
					base.Physics.IsPhantom = true;
					base.Physics.CreateFromCollisionObject(shape, base.PositionComp.LocalVolume.Center, base.WorldMatrix, null, 21);
					shape.Base.RemoveReference();
					base.Physics.Enabled = base.IsWorking;
				}
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
				base.ResourceSink.Update();
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_baseIdleSound.Init("BlockGravityGen");
		}

		protected abstract void InitializeSinkComponent();

		protected void UpdateFieldShape()
		{
			if (MyFakes.ENABLE_GRAVITY_PHANTOM && base.Physics != null)
			{
				HkBvShape shape = CreateFieldShape();
				base.Physics.RigidBody.SetShape(shape);
				shape.Base.RemoveReference();
				UpdateGeneratorProxy();
			}
			base.ResourceSink.Update();
		}

		private HkBvShape CreateFieldShape()
		{
			return new HkBvShape(childShape: new HkPhantomCallbackShape(phantom_Enter, phantom_Leave), boundingVolumeShape: GetHkShape(), policy: HkReferencePolicy.TakeOwnership);
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink == null || base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected MyGravityGeneratorBase()
		{
			m_gravityAcceleration.ValueChanged += delegate
			{
				AccelerationChanged();
			};
			base.NeedsWorldMatrix = true;
		}

		private void AccelerationChanged()
		{
			base.ResourceSink.Update();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			MyGravityProviderSystem.AddGravityGenerator(this);
			if (base.ResourceSink != null)
			{
				base.ResourceSink.Update();
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			MyGravityProviderSystem.RemoveGravityGenerator(this);
			base.OnRemovedFromScene(source);
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			UpdateGeneratorProxy();
		}

		private void UpdateGeneratorProxy()
		{
			MyGridPhysics physics = base.CubeGrid.Physics;
			if (physics != null && base.InScene)
			{
				Vector3 velocity = physics.LinearVelocity;
				MyGravityProviderSystem.OnGravityGeneratorMoved(this, ref velocity);
			}
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.ResourceSink.Update();
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (base.IsWorking)
			{
				foreach (VRage.ModAPI.IMyEntity containedEntity in m_containedEntities)
				{
					MyEntity myEntity = containedEntity as MyEntity;
					MyCharacter myCharacter = myEntity as MyCharacter;
					SpaceEngineers.Game.ModAPI.IMyVirtualMass myVirtualMass = myEntity as SpaceEngineers.Game.ModAPI.IMyVirtualMass;
					float naturalGravityMultiplier = MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(myEntity.WorldMatrix.Translation);
					Vector3 value = GetWorldGravity(myEntity.WorldMatrix.Translation) * MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(naturalGravityMultiplier);
					if (myVirtualMass != null && myEntity.Physics.RigidBody.IsActive)
					{
						if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS)
						{
							MyRenderProxy.DebugDrawSphere(myEntity.WorldMatrix.Translation, 0.2f, myVirtualMass.IsWorking ? Color.Blue : Color.Red, 1f, depthRead: false);
						}
						if (myVirtualMass.IsWorking && !myVirtualMass.CubeGrid.IsStatic && !myVirtualMass.CubeGrid.Physics.IsStatic)
						{
							myVirtualMass.CubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, value * myVirtualMass.VirtualMass, myEntity.WorldMatrix.Translation, null, null, applyImmediately: false);
						}
					}
					else if (!myEntity.Physics.IsKinematic && !myEntity.Physics.IsStatic && myEntity.Physics.RigidBody2 == null && myCharacter == null && myEntity.Physics.RigidBody != null)
					{
						if (myEntity is MyFloatingObject)
						{
							float scaleFactor = (myEntity as MyFloatingObject).HasConstraints() ? 2f : 1f;
							myEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, scaleFactor * value * myEntity.Physics.RigidBody.Mass, null, null);
						}
						else
						{
							myEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, value * myEntity.Physics.RigidBody.Mass, null, null);
						}
					}
				}
			}
			if (m_containedEntities.Count == 0)
			{
				base.NeedsUpdate = (base.HasDamageEffect ? MyEntityUpdateEnum.EACH_FRAME : MyEntityUpdateEnum.EACH_100TH_FRAME);
			}
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		protected void OnIsWorkingChanged(MyCubeBlock obj)
		{
			UpdateGenerator();
		}

		protected void Receiver_IsPoweredChanged()
		{
			UpdateGenerator();
		}

		private void UpdateGenerator()
		{
			UpdateIsWorking();
			if (base.Physics != null)
			{
				base.Physics.Enabled = base.IsWorking;
			}
			SetDetailedInfoDirty();
		}

		protected void Receiver_RequiredInputChanged(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver, float oldRequirement, float newRequirement)
		{
			SetDetailedInfoDirty();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void phantom_Enter(HkPhantomCallbackShape sender, HkRigidBody body)
		{
			VRage.ModAPI.IMyEntity entity = body.GetEntity(0u);
			if (entity != null && !(entity is MyCubeGrid) && m_containedEntities.Add(entity))
			{
				MySandboxGame.Static.Invoke(delegate
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
					MyPhysicsComponentBase physics = entity.Physics;
					if (physics != null && physics.HasRigidBody)
					{
						((MyPhysicsBody)physics).RigidBody.Activate();
					}
				}, "MyGravityGeneratorBase/Activate physics");
			}
		}

		private void phantom_Leave(HkPhantomCallbackShape sender, HkRigidBody body)
		{
			VRage.ModAPI.IMyEntity entity = body.GetEntity(0u);
			if (entity != null)
			{
				m_containedEntities.Remove(entity);
			}
		}

		public Color GetGizmoColor()
		{
			return m_gizmoColor;
		}

		public bool CanBeDrawn()
		{
			if (!MyCubeGrid.ShowGravityGizmos || !base.ShowOnHUD || !base.IsWorking || !HasLocalPlayerAccess() || GetDistanceBetweenCameraAndBoundingSphere() > 1000.0)
			{
				return false;
			}
			return true;
		}

		public MatrixD GetWorldMatrix()
		{
			return base.WorldMatrix;
		}

		public virtual BoundingBox? GetBoundingBox()
		{
			return null;
		}

		public virtual float GetRadius()
		{
			return -1f;
		}

		public Vector3 GetPositionInGrid()
		{
			return base.Position;
		}

		protected override void Closing()
		{
			base.Closing();
			if (base.CubeGrid.CreatePhysics && base.ResourceSink != null)
			{
				base.ResourceSink.IsPoweredChanged -= Receiver_IsPoweredChanged;
				base.ResourceSink.RequiredInputChanged -= Receiver_RequiredInputChanged;
			}
		}

		public bool EnableLongDrawDistance()
		{
			return false;
		}

		public float GetGravityMultiplier(Vector3D worldPoint)
		{
			if (!IsPositionInRange(worldPoint))
			{
				return 0f;
			}
			return 1f;
		}

		public abstract void GetProxyAABB(out BoundingBoxD aabb);
	}
}
