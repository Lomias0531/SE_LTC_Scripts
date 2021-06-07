using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.History;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
	public class MyCharacterPhysicsStateGroup : MyEntityPhysicsStateGroupBase
	{
		public static MyTimeSpan ParentChangeTimeOut = MyTimeSpan.FromMilliseconds(100.0);

		public static readonly ParentingSetup JetpackParentingSetup = new ParentingSetup
		{
			MaxParentDistance = 100f,
			MinParentSpeed = 20f,
			MaxParentAcceleration = 6f,
			MinInsideParentSpeed = 20f,
			MaxParentDisconnectDistance = 100f,
			MinDisconnectParentSpeed = 15f,
			MaxDisconnectParentAcceleration = 30f,
			MinDisconnectInsideParentSpeed = 10f
		};

		private const float FallMaxParentDisconnectDistance = 10f;

		private readonly List<MyEntity> m_tmpEntityResults = new List<MyEntity>();

		private MyTimeSpan m_lastTimestamp;

		private static readonly MyPredictedSnapshotSyncSetup m_controlledJetPackSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledJetpack",
			ApplyPosition = true,
			ApplyRotation = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			InheritRotation = false,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MaxPositionFactor = 100f,
			MinPositionFactor = 100f,
			MaxLinearFactor = 1f,
			MaxRotationFactor = 1f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.3f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledJetPackMovingSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledJetpackMoving",
			ApplyPosition = true,
			ApplyRotation = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			InheritRotation = false,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 1f,
			MaxRotationFactor = 1f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.3f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledJetPackNewParentSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledJetpackNewParent",
			ApplyPosition = true,
			ApplyRotation = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			InheritRotation = false,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 1f,
			MaxRotationFactor = 1f,
			MaxAngularFactor = 1f,
			IterationsFactor = 1.5f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledCharacter",
			ApplyPosition = true,
			ApplyRotation = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = false,
			ExtrapolationSmoothing = true,
			InheritRotation = true,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MinPositionFactor = 100f,
			MaxPositionFactor = 10f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.3f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private static readonly MyPredictedSnapshotSyncSetup m_deadSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "DeadCharacter",
			ApplyPosition = false,
			ApplyRotation = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = false,
			ExtrapolationSmoothing = true,
			InheritRotation = true,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MinPositionFactor = 100f,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.3f,
			IgnoreParentId = true,
			UserTrend = false
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledAnimatedSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledAnimatedCharacter",
			ApplyPosition = true,
			ApplyRotation = false,
			ApplyPhysicsAngular = true,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			InheritRotation = true,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MinPositionFactor = 100f,
			MaxPositionFactor = 10f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.3f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledMovingSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledCharacterMoving",
			ApplyPosition = true,
			ApplyRotation = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = false,
			ExtrapolationSmoothing = true,
			InheritRotation = true,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MaxPositionFactor = 10f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.3f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledNewParentSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledCharacterNewParent",
			ApplyPosition = true,
			ApplyRotation = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = false,
			ExtrapolationSmoothing = true,
			InheritRotation = true,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 1f,
			IterationsFactor = 1.5f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private static readonly MyPredictedSnapshotSyncSetup m_settings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "GeneralCharacter",
			ApplyPosition = true,
			ApplyRotation = true,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			ApplyPhysicsLocal = true,
			IsControlled = false,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 180f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.25f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledLadderSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledLadderCharacter",
			ApplyPosition = false,
			ApplyRotation = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = false,
			ExtrapolationSmoothing = true,
			InheritRotation = true,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			AllowForceStop = true,
			MinPositionFactor = 100f,
			MaxPositionFactor = 10f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.3f,
			IgnoreParentId = true,
			UserTrend = true
		};

		private readonly MyPredictedSnapshotSync m_predictedSync;

		private readonly IMySnapshotSync m_animatedSync;

		private long m_lastParentId;

		private MyTimeSpan m_lastParentTime;

		private MyTimeSpan m_lastAnimatedTime;

		private byte m_syncLinearVelocity = 2;

		private const float NEW_PARENT_TIMEOUT = 3f;

		private const float SYNC_CHANGE_TIMEOUT = 0.1f;

		public static float EXCESSIVE_CORRECTION_THRESHOLD = 20f;

		public new MyCharacter Entity => (MyCharacter)base.Entity;

		public double AverageCorrection => m_predictedSync.AverageCorrection.Sum;

		public MyCharacterPhysicsStateGroup(MyEntity entity, IMyReplicable ownerReplicable)
			: base(entity, ownerReplicable, createSync: false)
		{
			m_predictedSync = new MyPredictedSnapshotSync(Entity);
			m_animatedSync = new MyAnimatedSnapshotSync(Entity);
			m_snapshotSync = m_animatedSync;
			if (Sync.IsServer)
			{
				Entity.Hierarchy.OnParentChanged += OnEntityParentChanged;
			}
		}

		private void OnEntityParentChanged(MyHierarchyComponentBase oldParent, MyHierarchyComponentBase newParent)
		{
			if (oldParent != null && newParent == null)
			{
				MyMultiplayer.GetReplicationServer().AddToDirtyGroups(this);
			}
		}

		public override void ClientUpdate(MyTimeSpan clientTimestamp)
		{
			bool isControlledLocally = base.IsControlledLocally;
			bool isClientPredicted = Entity.IsClientPredicted;
			bool flag = true;
			MyPredictedSnapshotSyncSetup myPredictedSnapshotSyncSetup = m_settings;
			if (isClientPredicted)
			{
				if (m_snapshotSync != m_predictedSync)
				{
					m_lastAnimatedTime = MySandboxGame.Static.SimulationTime;
				}
				m_snapshotSync = m_predictedSync;
				long parentId = m_predictedSync.GetParentId();
				if (parentId != -1)
				{
					Entity.ClosestParentId = parentId;
				}
				bool flag2 = MySandboxGame.Static.SimulationTime < m_lastParentTime + MyTimeSpan.FromSeconds(3.0);
				if (MySandboxGame.Static.SimulationTime < m_lastAnimatedTime + MyTimeSpan.FromSeconds(0.10000000149011612))
				{
					m_predictedSync.Reset(reinit: true);
				}
				flag = m_predictedSync.Inited;
				bool flag3 = Entity.MoveIndicator != Vector3.Zero;
				if (isControlledLocally)
				{
					myPredictedSnapshotSyncSetup = ((!Entity.InheritRotation) ? (flag2 ? m_controlledJetPackNewParentSettings : (flag3 ? m_controlledJetPackMovingSettings : m_controlledJetPackSettings)) : ((Entity.IsOnLadder || Entity.CurrentMovementState == MyCharacterMovementEnum.LadderOut) ? m_controlledLadderSettings : (flag2 ? m_controlledNewParentSettings : (flag3 ? m_controlledMovingSettings : m_controlledSettings))));
				}
			}
			else
			{
				if (isControlledLocally)
				{
					myPredictedSnapshotSyncSetup = m_controlledAnimatedSettings;
				}
				m_snapshotSync = m_animatedSync;
			}
			if (Entity.IsDead)
			{
				myPredictedSnapshotSyncSetup = m_deadSettings;
			}
			long num = m_snapshotSync.Update(clientTimestamp, myPredictedSnapshotSyncSetup);
			if (num != -1)
			{
				Entity.ClosestParentId = num;
			}
			Entity.AlwaysDisablePrediction = MyPredictedSnapshotSync.ForceAnimated;
			Entity.LastSnapshotFlags = myPredictedSnapshotSyncSetup;
			if (m_predictedSync.AverageCorrection.Sum > (double)EXCESSIVE_CORRECTION_THRESHOLD)
			{
				Entity.ForceDisablePrediction = true;
				m_predictedSync.AverageCorrection.Reset();
			}
			if (m_predictedSync.Inited && !flag && Entity.Physics != null && Entity.Physics.CharacterProxy != null)
			{
				Entity.Physics.CharacterProxy.SetSupportedState(supported: true);
			}
		}

		public override void Serialize(BitStream stream, Endpoint forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream.Writing)
			{
				UpdateEntitySupport();
				MySnapshot mySnapshot = new MySnapshot(Entity, localPhysics: false, Entity.InheritRotation);
				if (Entity.Parent != null)
				{
					mySnapshot.Active = false;
				}
				mySnapshot.Write(stream);
				stream.WriteBool(value: true);
				Entity.SerializeControls(stream);
				return;
			}
			MySnapshot snapshot = new MySnapshot(stream);
			if (snapshot.ParentId != m_lastParentId)
			{
				m_lastParentId = snapshot.ParentId;
				if (m_supportInited)
				{
					m_lastParentTime = MySandboxGame.Static.SimulationTime;
				}
				else
				{
					m_supportInited = true;
				}
			}
			m_animatedSync.Read(ref snapshot, serverTimestamp);
			m_predictedSync.Read(ref snapshot, lastClientTimestamp);
			if (stream.ReadBool())
			{
				Entity.DeserializeControls(stream, outOfOrder: false);
			}
		}

		private void UpdateEntitySupport()
		{
			MyTimeSpan simulationTime = MySandboxGame.Static.SimulationTime;
			if (m_lastTimestamp + ParentChangeTimeOut > simulationTime || Entity.Physics == null)
			{
				return;
			}
			if (Entity.Parent != null)
			{
				m_lastSupportId = Entity.Parent.EntityId;
				return;
			}
			m_lastTimestamp = simulationTime;
			if (!Entity.JetpackRunning && !Entity.IsDead)
			{
				bool flag = false;
				if (Entity.Physics.CharacterProxy != null && Entity.Physics.CharacterProxy.Supported)
				{
					List<MyEntity> tmpEntityResults = m_tmpEntityResults;
					Entity.Physics.CharacterProxy.GetSupportingEntities(tmpEntityResults);
					bool flag2 = false;
					foreach (MyEntity item in tmpEntityResults)
					{
						if (item is MyCubeGrid || item is MyVoxelBase)
						{
							m_supportInited = true;
							flag2 = true;
							if (item.Physics.IsStatic)
							{
								Entity.ClosestParentId = (m_lastSupportId = 0L);
							}
							else
							{
								Entity.ClosestParentId = (m_lastSupportId = item.EntityId);
								flag = true;
							}
							break;
						}
					}
					if (tmpEntityResults.Count > 0 && !flag2)
					{
						Entity.ClosestParentId = UpdateParenting(JetpackParentingSetup, Entity.ClosestParentId);
					}
					tmpEntityResults.Clear();
				}
				else if (Entity.IsOnLadder)
				{
					Entity.ClosestParentId = Entity.Ladder.CubeGrid.EntityId;
					flag = true;
				}
				if (!flag && Entity.ClosestParentId != 0L)
				{
					MyEntities.TryGetEntityById(Entity.ClosestParentId, out MyEntity entity);
					MyCubeGrid myCubeGrid = entity as MyCubeGrid;
					if (myCubeGrid != null && (float)myCubeGrid.PositionComp.WorldAABB.DistanceSquared(Entity.PositionComp.GetPosition()) > 100f)
					{
						Entity.ClosestParentId = 0L;
					}
				}
			}
			else
			{
				Entity.ClosestParentId = UpdateParenting(JetpackParentingSetup, Entity.ClosestParentId);
			}
		}

		public override bool IsStillDirty(Endpoint forClient)
		{
			return Entity.Parent == null;
		}

		public override void Destroy()
		{
			if (Sync.IsServer)
			{
				Entity.Hierarchy.OnParentChanged -= OnEntityParentChanged;
			}
			base.Destroy();
		}
	}
}
