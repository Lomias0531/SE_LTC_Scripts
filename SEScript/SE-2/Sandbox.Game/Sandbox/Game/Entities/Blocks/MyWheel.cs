using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Wheel))]
	public class MyWheel : MyMotorRotor, Sandbox.ModAPI.IMyWheel, Sandbox.ModAPI.IMyMotorRotor, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyMotorRotor, Sandbox.ModAPI.Ingame.IMyWheel
	{
		[Serializable]
		private struct ParticleData
		{
			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003EParticleData_003C_003EEffectName_003C_003EAccessor : IMemberAccessor<ParticleData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParticleData owner, in string value)
				{
					owner.EffectName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParticleData owner, out string value)
				{
					value = owner.EffectName;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003EParticleData_003C_003EPositionRelative_003C_003EAccessor : IMemberAccessor<ParticleData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParticleData owner, in Vector3 value)
				{
					owner.PositionRelative = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParticleData owner, out Vector3 value)
				{
					value = owner.PositionRelative;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003EParticleData_003C_003ENormal_003C_003EAccessor : IMemberAccessor<ParticleData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParticleData owner, in Vector3 value)
				{
					owner.Normal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParticleData owner, out Vector3 value)
				{
					value = owner.Normal;
				}
			}

			public string EffectName;

			public Vector3 PositionRelative;

			public Vector3 Normal;
		}

		protected class m_particleData_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType particleData;
				ISyncType result = particleData = new Sync<ParticleData, SyncDirection.FromServer>(P_1, P_2);
				((MyWheel)P_0).m_particleData = (Sync<ParticleData, SyncDirection.FromServer>)particleData;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyWheel_003C_003EActor : IActivator, IActivator<MyWheel>
		{
			private sealed override object CreateInstance()
			{
				return new MyWheel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWheel CreateInstance()
			{
				return new MyWheel();
			}

			MyWheel IActivator<MyWheel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly MyStringHash m_wheelStringHash = MyStringHash.GetOrCompute("Wheel");

		private MyWheelModelsDefinition m_cachedModelsDefinition;

		public Vector3 LastUsedGroundNormal;

		private int m_modelSwapCountUp;

		private bool m_usesAlternativeModel;

		public bool m_isSuspensionMounted;

		private int m_slipCountdown;

		private int m_staticHitCount;

		private int m_contactCountdown;

		private float m_frictionCollector;

		private Vector3 m_lastFrameImpuse;

		private ConcurrentNormalAggregator m_contactNormals = new ConcurrentNormalAggregator(10);

		private readonly Sync<ParticleData, SyncDirection.FromServer> m_particleData;

		private static Dictionary<MyCubeGrid, Queue<MyTuple<DateTime, string>>> activityLog = new Dictionary<MyCubeGrid, Queue<MyTuple<DateTime, string>>>();

		private bool m_eachUpdateCallbackRegistered;

		public float Friction
		{
			get;
			set;
		}

		public ulong LastContactFrameNumber
		{
			get;
			private set;
		}

		private new MyRenderComponentWheel Render
		{
			get
			{
				return base.Render as MyRenderComponentWheel;
			}
			set
			{
				base.Render = value;
			}
		}

		public ulong FramesSinceLastContact => MySandboxGame.Static.SimulationFrameCounter - LastContactFrameNumber;

		public DateTime LastContactTime
		{
			get;
			set;
		}

		private MyWheelModelsDefinition WheelModelsDefinition
		{
			get
			{
				if (m_cachedModelsDefinition == null)
				{
					string subtypeName = base.BlockDefinition.Id.SubtypeName;
					DictionaryReader<string, MyWheelModelsDefinition> wheelModelDefinitions = MyDefinitionManager.Static.GetWheelModelDefinitions();
					if (!wheelModelDefinitions.TryGetValue(subtypeName, out m_cachedModelsDefinition))
					{
						MyDefinitionManager.Static.AddMissingWheelModelDefinition(subtypeName);
						m_cachedModelsDefinition = wheelModelDefinitions[subtypeName];
					}
				}
				return m_cachedModelsDefinition;
			}
		}

		public bool IsConsideredInContactWithStaticSurface
		{
			get
			{
				if (m_staticHitCount > 0)
				{
					return true;
				}
				return m_contactCountdown > 0;
			}
		}

		public MyWheel()
		{
			Friction = 1.5f;
			base.IsWorkingChanged += MyWheel_IsWorkingChanged;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				Render = new MyRenderComponentWheel();
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.Init(builder, cubeGrid);
			MyObjectBuilder_Wheel myObjectBuilder_Wheel = builder as MyObjectBuilder_Wheel;
			if (myObjectBuilder_Wheel != null && !myObjectBuilder_Wheel.YieldLastComponent)
			{
				SlimBlock.DisableLastComponentYield();
			}
			if (Sync.IsServer)
			{
				m_particleData.Value = new ParticleData
				{
					EffectName = "",
					PositionRelative = Vector3.Zero,
					Normal = Vector3.Forward
				};
			}
			else
			{
				m_particleData.ValueChanged += m_particleData_ValueChanged;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void m_particleData_ValueChanged(SyncBase obj)
		{
			LastContactTime = DateTime.UtcNow;
			string effectName = m_particleData.Value.EffectName;
			Vector3D position = base.PositionComp.WorldMatrix.Translation + m_particleData.Value.PositionRelative;
			Vector3 normal = m_particleData.Value.Normal;
			if (Render != null)
			{
				Render.TrySpawnParticle(effectName, ref position, ref normal);
				Render.UpdateParticle(ref position, ref normal);
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_CubeBlock objectBuilderCubeBlock = base.GetObjectBuilderCubeBlock(copy);
			MyObjectBuilder_Wheel myObjectBuilder_Wheel = objectBuilderCubeBlock as MyObjectBuilder_Wheel;
			if (myObjectBuilder_Wheel != null)
			{
				myObjectBuilder_Wheel.YieldLastComponent = SlimBlock.YieldLastComponent;
			}
			return objectBuilderCubeBlock;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (base.CubeGrid.Physics != null)
			{
				base.CubeGrid.Physics.RigidBody.CallbackLimit = 1;
				base.CubeGrid.Physics.RigidBody.CollisionAddedCallback += RigidBody_CollisionAddedCallback;
				base.CubeGrid.Physics.RigidBody.CollisionRemovedCallback += RigidBody_CollisionRemovedCallback;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (base.CubeGrid.Physics != null)
			{
				base.CubeGrid.Physics.RigidBody.CollisionAddedCallback -= RigidBody_CollisionAddedCallback;
				base.CubeGrid.Physics.RigidBody.CollisionRemovedCallback -= RigidBody_CollisionRemovedCallback;
			}
		}

		private bool IsAcceptableContact(HkRigidBody rb)
		{
			object userObject = rb.UserObject;
			if (userObject == null)
			{
				return false;
			}
			if (userObject == base.CubeGrid.Physics)
			{
				return false;
			}
			if (userObject is MyVoxelPhysicsBody)
			{
				return true;
			}
			MyGridPhysics myGridPhysics = userObject as MyGridPhysics;
			if (myGridPhysics != null && myGridPhysics.IsStatic)
			{
				return true;
			}
			return false;
		}

		private void RigidBody_CollisionAddedCallback(ref HkCollisionEvent e)
		{
			_ = base.CubeGrid.Physics;
			if (IsAcceptableContact(e.BodyA) || IsAcceptableContact(e.BodyB))
			{
				m_contactCountdown = 30;
				Interlocked.Increment(ref m_staticHitCount);
				RegisterPerFrameUpdate();
			}
		}

		private void RigidBody_CollisionRemovedCallback(ref HkCollisionEvent e)
		{
			_ = base.CubeGrid.Physics;
			if ((IsAcceptableContact(e.BodyA) || IsAcceptableContact(e.BodyB)) && Interlocked.Decrement(ref m_staticHitCount) < 0)
			{
				Interlocked.Increment(ref m_staticHitCount);
			}
		}

		private void MyWheel_IsWorkingChanged(MyCubeBlock obj)
		{
			if (base.Stator != null)
			{
				base.Stator.UpdateIsWorking();
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Render != null)
			{
				Render.UpdatePosition();
			}
		}

		public override void ContactPointCallback(ref MyGridContactInfo value)
		{
			Vector3 normal = value.Event.ContactPoint.Normal;
			m_contactNormals.PushNext(ref normal);
			MyVoxelMaterialDefinition voxelSurfaceMaterial = value.VoxelSurfaceMaterial;
			if (voxelSurfaceMaterial != null)
			{
				m_frictionCollector = voxelSurfaceMaterial.Friction;
			}
			float num = Friction;
			if (m_isSuspensionMounted && value.CollidingEntity is MyCubeGrid && value.OtherBlock != null && value.OtherBlock.FatBlock == null)
			{
				num *= 0.07f;
				m_frictionCollector = 0.7f;
			}
			HkContactPointProperties contactProperties = value.Event.ContactProperties;
			contactProperties.Friction = num;
			contactProperties.Restitution = 0.5f;
			value.EnableParticles = false;
			value.RubberDeformation = true;
			ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
			if (simulationFrameCounter == LastContactFrameNumber)
			{
				return;
			}
			LastContactFrameNumber = simulationFrameCounter;
			Vector3D position = value.ContactPosition;
			if (m_contactNormals.GetAvgNormalCached(out Vector3 normal2))
			{
				normal = normal2;
			}
			string text = null;
			if (value.CollidingEntity is MyVoxelBase && MyFakes.ENABLE_DRIVING_PARTICLES)
			{
				if (voxelSurfaceMaterial != null)
				{
					MyStringHash materialTypeNameHash = voxelSurfaceMaterial.MaterialTypeNameHash;
					text = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, m_wheelStringHash, materialTypeNameHash);
				}
			}
			else if (value.CollidingEntity is MyCubeGrid && MyFakes.ENABLE_DRIVING_PARTICLES)
			{
				MyStringHash materialAt = (value.CollidingEntity as MyCubeGrid).Physics.GetMaterialAt(position);
				text = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, m_wheelStringHash, materialAt);
			}
			if (Render != null)
			{
				if (text != null)
				{
					Render.TrySpawnParticle(text, ref position, ref normal);
				}
				Render.UpdateParticle(ref position, ref normal);
			}
			if (text != null && Sync.IsServer && MySession.Static.Settings.OnlineMode != 0)
			{
				m_particleData.Value = new ParticleData
				{
					EffectName = text,
					PositionRelative = position - base.PositionComp.WorldMatrix.Translation,
					Normal = value.Event.ContactPoint.Normal
				};
			}
			RegisterPerFrameUpdate();
		}

		private bool SteeringLogic()
		{
			if (!base.IsFunctional)
			{
				return false;
			}
			MyGridPhysics physics = base.CubeGrid.Physics;
			if (physics == null)
			{
				return false;
			}
			if (base.Stator != null && MyFixedGrids.IsRooted(base.Stator.CubeGrid))
			{
				return false;
			}
			if (m_slipCountdown > 0)
			{
				m_slipCountdown--;
			}
			if (m_staticHitCount == 0)
			{
				if (m_contactCountdown <= 0)
				{
					return false;
				}
				m_contactCountdown--;
				if (m_contactCountdown == 0)
				{
					m_frictionCollector = 0f;
					m_contactNormals.Clear();
					return false;
				}
			}
			Vector3 value = physics.LinearVelocity;
			if (MyUtils.IsZero(ref value) || !physics.IsActive)
			{
				return false;
			}
			MatrixD worldMatrix = base.WorldMatrix;
			Vector3D centerOfMassWorld = physics.CenterOfMassWorld;
			if (!m_contactNormals.GetAvgNormal(out Vector3 normal))
			{
				return false;
			}
			LastUsedGroundNormal = normal;
			Vector3 guideVector = worldMatrix.Up;
			Vector3 guideVector2 = Vector3.Cross(normal, guideVector);
			value = Vector3.ProjectOnPlane(ref value, ref normal);
			Vector3 vector = Vector3.ProjectOnVector(ref value, ref guideVector2);
			Vector3 value2 = vector - value;
			if (MyUtils.IsZero(ref value2))
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			float num = 6f * m_frictionCollector;
			Vector3 vec = Vector3.ProjectOnVector(ref value2, ref guideVector);
			float num2 = vec.Length();
			bool flag3 = num2 > num;
			if (flag3 || m_slipCountdown != 0)
			{
				float num3 = 1f / num2;
				num3 *= num;
				vec *= num3;
				flag = true;
				vec *= 1f - MyPhysicsConfig.WheelSlipCutAwayRatio;
				if (flag3)
				{
					m_slipCountdown = MyPhysicsConfig.WheelSlipCountdown;
				}
			}
			else if ((double)num2 < 0.1)
			{
				flag2 = true;
			}
			if (!flag2)
			{
				if (vec.LengthSquared() < 0.001f)
				{
					return !flag2;
				}
				vec *= 1f - (1f - m_frictionCollector) * MyPhysicsConfig.WheelSurfaceMaterialSteerRatio;
				Vector3 vec2 = vec;
				Vector3 vector2 = Vector3.ProjectOnPlane(ref vec2, ref normal);
				MyMechanicalConnectionBlockBase stator = base.Stator;
				MyPhysicsBody myPhysicsBody = null;
				if (stator != null)
				{
					myPhysicsBody = base.Stator.CubeGrid.Physics;
				}
				vector2 *= 0.1f;
				if (myPhysicsBody == null)
				{
					vector2 *= physics.Mass;
					physics.ApplyImpulse(vector2, centerOfMassWorld);
				}
				else
				{
					Vector3D vector3D = Vector3D.Zero;
					MyMotorSuspension myMotorSuspension = stator as MyMotorSuspension;
					if (myMotorSuspension != null)
					{
						vector2 *= MyMath.Clamp(myMotorSuspension.Friction * 2f, 0f, 1f);
						myMotorSuspension.GetCoMVectors(out Vector3 adjustmentVector);
						vector3D = Vector3D.TransformNormal(-adjustmentVector, stator.CubeGrid.WorldMatrix);
					}
					Vector3D vector3D2 = centerOfMassWorld + vector3D;
					float wheelImpulseBlending = MyPhysicsConfig.WheelImpulseBlending;
					vector2 = (m_lastFrameImpuse = m_lastFrameImpuse * wheelImpulseBlending + vector2 * (1f - wheelImpulseBlending)) * myPhysicsBody.Mass;
					myPhysicsBody.ApplyImpulse(vector2, vector3D2);
					if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
					{
						MyRenderProxy.DebugDrawArrow3DDir(vector3D2, -vector3D, Color.Red);
						MyRenderProxy.DebugDrawSphere(vector3D2, 0.1f, Color.Yellow, 1f, depthRead: false);
					}
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
			{
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, value, Color.Yellow);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, vector, Color.Blue);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, vec, Color.MediumPurple);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld + value, value2, Color.Red);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld + guideVector, normal, Color.AliceBlue);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, Vector3.ProjectOnPlane(ref vec, ref normal), flag ? Color.DarkRed : Color.IndianRed);
				if (m_slipCountdown > 0)
				{
					MyRenderProxy.DebugDrawText3D(centerOfMassWorld + guideVector * 2f, "Drift", Color.Red, 1f, depthRead: false);
				}
				MyRenderProxy.DebugDrawText3D(centerOfMassWorld + guideVector * 1.2f, m_staticHitCount.ToString(), Color.Red, 1f, depthRead: false);
			}
			return !flag2;
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			SwapModelLogic();
			bool flag = SteeringLogic();
			if (!flag && m_contactCountdown == 0)
			{
				m_lastFrameImpuse = Vector3.Zero;
				if ((Render == null || (Render != null && !Render.UpdateNeeded)) && !base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
			{
				MatrixD worldMatrix = base.WorldMatrix;
				MyRenderProxy.DebugDrawCross(worldMatrix.Translation, worldMatrix.Up, worldMatrix.Forward, flag ? Color.Green : Color.Red);
			}
		}

		public override string CalculateCurrentModel(out Matrix orientation)
		{
			string result = base.CalculateCurrentModel(out orientation);
			if (base.CubeGrid.Physics == null)
			{
				return result;
			}
			if (base.Stator == null || !base.IsFunctional)
			{
				return result;
			}
			if (m_usesAlternativeModel)
			{
				return WheelModelsDefinition.AlternativeModel;
			}
			return result;
		}

		private void SwapModelLogic()
		{
			if (!MyFakes.WHEEL_ALTERNATIVE_MODELS_ENABLED || base.Stator == null || !base.IsFunctional)
			{
				if (m_usesAlternativeModel)
				{
					m_usesAlternativeModel = false;
					UpdateVisual();
				}
				return;
			}
			float angularVelocityThreshold = WheelModelsDefinition.AngularVelocityThreshold;
			float observerAngularVelocityDiff = GetObserverAngularVelocityDiff();
			bool num = m_usesAlternativeModel && observerAngularVelocityDiff + 5f < angularVelocityThreshold;
			bool flag = !m_usesAlternativeModel && observerAngularVelocityDiff - 5f > angularVelocityThreshold;
			if (num || flag)
			{
				m_modelSwapCountUp++;
				if (m_modelSwapCountUp >= 5)
				{
					m_usesAlternativeModel = !m_usesAlternativeModel;
					UpdateVisual();
				}
			}
			else
			{
				m_modelSwapCountUp = 0;
			}
		}

		private float GetObserverAngularVelocityDiff()
		{
			MyGridPhysics physics = base.CubeGrid.Physics;
			if (physics != null && physics.LinearVelocity.LengthSquared() > 16f)
			{
				IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
				if (controlledEntity != null)
				{
					MyEntity entity = controlledEntity.Entity;
					if (entity != null)
					{
						MyPhysicsComponentBase physics2 = entity.GetTopMostParent().Physics;
						if (physics2 != null)
						{
							return (physics.AngularVelocity - physics2.AngularVelocity).Length();
						}
					}
				}
			}
			return 0f;
		}

		public static void WheelExplosionLog(MyCubeGrid grid, MyTerminalBlock block, string message)
		{
		}

		public static void DumpActivityLog()
		{
			lock (activityLog)
			{
				foreach (KeyValuePair<MyCubeGrid, Queue<MyTuple<DateTime, string>>> item2 in activityLog)
				{
					MyCubeGrid key = item2.Key;
					MyLog.Default.WriteLine("GRID: " + key.DisplayName);
					foreach (MyTuple<DateTime, string> item3 in item2.Value)
					{
						MyLog @default = MyLog.Default;
						DateTime item = item3.Item1;
						@default.WriteLine("[" + item.ToString("dd/MM hh:mm:ss:FFF") + "] " + item3.Item2);
					}
					MyLog.Default.Flush();
				}
				activityLog.Clear();
			}
		}

		public override void Attach(MyMechanicalConnectionBlockBase parent)
		{
			base.Attach(parent);
			m_isSuspensionMounted = (base.Stator is MyMotorSuspension);
		}

		public override void Detach(bool isWelding)
		{
			m_isSuspensionMounted = false;
			base.Detach(isWelding);
		}

		private void RegisterPerFrameUpdate()
		{
			if ((base.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) == 0 && !m_eachUpdateCallbackRegistered)
			{
				m_eachUpdateCallbackRegistered = true;
				MySandboxGame.Static.Invoke(delegate
				{
					m_eachUpdateCallbackRegistered = false;
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}, "WheelEachUpdate");
			}
		}
	}
}
