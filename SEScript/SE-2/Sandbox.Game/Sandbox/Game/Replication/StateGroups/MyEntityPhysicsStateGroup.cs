using Sandbox.Game.Entities;
using Sandbox.Game.Replication.History;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
	public class MyEntityPhysicsStateGroup : MyEntityPhysicsStateGroupBase
	{
		private static readonly MyPredictedSnapshotSyncSetup m_settings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "GeneralGrid",
			ApplyPosition = true,
			ApplyRotation = true,
			ApplyPhysicsAngular = true,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			IsControlled = false,
			ApplyPhysicsLocal = false,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 1f,
			IterationsFactor = 1f
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledGrid",
			ApplyPosition = true,
			ApplyRotation = true,
			ApplyPhysicsAngular = true,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			InheritRotation = false,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 10f,
			MinAngularFactor = 1000f,
			IterationsFactor = 1f
		};

		private static readonly MyPredictedSnapshotSyncSetup m_controlledNewParentSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "ControlledGridNewParent",
			ApplyPosition = true,
			ApplyRotation = true,
			ApplyPhysicsAngular = true,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			IsControlled = true,
			ApplyPhysicsLocal = true,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 10f,
			MinAngularFactor = 1000f,
			IterationsFactor = 5f,
			InheritRotation = false
		};

		private static readonly MyPredictedSnapshotSyncSetup m_wheelSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "Wheel",
			ApplyPosition = false,
			ApplyRotation = false,
			ApplyPhysicsAngular = true,
			ApplyPhysicsLinear = false,
			ExtrapolationSmoothing = true,
			IsControlled = false,
			UpdateAlways = false,
			MaxPositionFactor = 1f,
			MaxLinearFactor = 1f,
			MaxRotationFactor = 1f,
			MaxAngularFactor = 1f,
			IterationsFactor = 1f
		};

		private static readonly MyPredictedSnapshotSyncSetup m_carSettings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "Car",
			ApplyPosition = true,
			ApplyRotation = true,
			ApplyPhysicsAngular = true,
			ApplyPhysicsLinear = false,
			ExtrapolationSmoothing = true,
			InheritRotation = false,
			ApplyPhysicsLocal = true,
			IsControlled = true,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 10f,
			MinAngularFactor = 1000f,
			IterationsFactor = 1f
		};

		public static readonly ParentingSetup GridParentingSetup = new ParentingSetup
		{
			MaxParentDistance = 100f,
			MinParentSpeed = 30f,
			MaxParentAcceleration = 6f,
			MinInsideParentSpeed = 20f,
			MaxParentDisconnectDistance = 100f,
			MinDisconnectParentSpeed = 25f,
			MaxDisconnectParentAcceleration = 30f,
			MinDisconnectInsideParentSpeed = 10f
		};

		private readonly MyPredictedSnapshotSync m_predictedSync;

		private readonly IMySnapshotSync m_animatedSync;

		private MyTimeSpan m_lastParentTime;

		private long m_lastParentId;

		private bool m_inheritRotation;

		private const float NEW_PARENT_TIMEOUT = 3f;

		private MyTimeSpan m_lastAnimatedTime;

		private const float SYNC_CHANGE_TIMEOUT = 0.1f;

		public MyEntityPhysicsStateGroup(MyEntity entity, IMyReplicable ownerReplicable)
			: base(entity, ownerReplicable, createSync: false)
		{
			m_predictedSync = new MyPredictedSnapshotSync(base.Entity);
			m_animatedSync = new MyAnimatedSnapshotSync(base.Entity);
			m_snapshotSync = m_animatedSync;
		}

		public override void ClientUpdate(MyTimeSpan clientTimestamp)
		{
			MyCubeGrid myCubeGrid = base.Entity as MyCubeGrid;
			MyPredictedSnapshotSyncSetup myPredictedSnapshotSyncSetup;
			if (myCubeGrid?.IsClientPredicted ?? false)
			{
				if (m_snapshotSync != m_predictedSync)
				{
					m_lastAnimatedTime = MySandboxGame.Static.SimulationTime;
				}
				m_snapshotSync = m_predictedSync;
				if (!m_inheritRotation)
				{
					long parentId = m_predictedSync.GetParentId();
					if (parentId != -1)
					{
						myCubeGrid.ClosestParentId = parentId;
					}
				}
				else
				{
					myCubeGrid.ClosestParentId = 0L;
				}
				bool flag = MySandboxGame.Static.SimulationTime < m_lastParentTime + MyTimeSpan.FromSeconds(3.0);
				if (MySandboxGame.Static.SimulationTime < m_lastAnimatedTime + MyTimeSpan.FromSeconds(0.10000000149011612))
				{
					m_predictedSync.Reset(reinit: true);
				}
				myPredictedSnapshotSyncSetup = (myCubeGrid.IsClientPredictedWheel ? m_wheelSettings : (myCubeGrid.IsClientPredictedCar ? m_carSettings : (flag ? m_controlledNewParentSettings : m_controlledSettings)));
			}
			else
			{
				if (m_snapshotSync != m_animatedSync)
				{
					m_animatedSync.Reset();
				}
				m_snapshotSync = m_animatedSync;
				myPredictedSnapshotSyncSetup = m_settings;
			}
			long num = m_snapshotSync.Update(clientTimestamp, myPredictedSnapshotSyncSetup);
			if (myCubeGrid != null)
			{
				if (!m_inheritRotation)
				{
					if (num != -1)
					{
						myCubeGrid.ClosestParentId = num;
					}
				}
				else
				{
					myCubeGrid.ClosestParentId = 0L;
				}
				myCubeGrid.ForceDisablePrediction = MyPredictedSnapshotSync.ForceAnimated;
			}
			base.Entity.LastSnapshotFlags = myPredictedSnapshotSyncSetup;
		}

		private bool UpdateEntitySupport()
		{
			MyCubeGrid myCubeGrid = base.Entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				if (myCubeGrid.IsClientPredicted)
				{
					myCubeGrid.ClosestParentId = UpdateParenting(GridParentingSetup, myCubeGrid.ClosestParentId);
					return false;
				}
				myCubeGrid.ClosestParentId = 0L;
			}
			return true;
		}

		public override void Serialize(BitStream stream, Endpoint forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream.Writing)
			{
				bool inheritRotation = UpdateEntitySupport();
				MySnapshot mySnapshot = new MySnapshot(base.Entity, localPhysics: false, inheritRotation);
				m_forcedWorldSnapshots = mySnapshot.SkippedParent;
				mySnapshot.Write(stream);
				stream.WriteBool(value: true);
				base.Entity.SerializeControls(stream);
				return;
			}
			MySnapshot snapshot = new MySnapshot(stream);
			m_inheritRotation = snapshot.InheritRotation;
			snapshot.GetMatrix(base.Entity, out MatrixD _);
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
				base.Entity.DeserializeControls(stream, outOfOrder: false);
			}
		}
	}
}
