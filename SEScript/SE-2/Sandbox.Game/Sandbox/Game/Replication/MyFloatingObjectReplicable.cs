using Sandbox.Game.Entities;
using Sandbox.Game.Replication.History;
using Sandbox.Game.Replication.StateGroups;
using System.Collections.Generic;
using VRage.Network;

namespace Sandbox.Game.Replication
{
	public class MyFloatingObjectReplicable : MyEntityReplicableBaseEvent<MyFloatingObject>
	{
		private static readonly MyPredictedSnapshotSyncSetup m_settings = new MyPredictedSnapshotSyncSetup
		{
			ProfileName = "FloatingObject",
			ApplyPosition = true,
			ApplyRotation = true,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = true,
			ExtrapolationSmoothing = true,
			MaxPositionFactor = 100f,
			MaxLinearFactor = 100f,
			MaxRotationFactor = 100f,
			MaxAngularFactor = 1f,
			IterationsFactor = 0.3f
		};

		private MyPropertySyncStateGroup m_propertySync;

		protected override IMyStateGroup CreatePhysicsGroup()
		{
			return new MySimplePhysicsStateGroup(base.Instance, this, m_settings);
		}

		protected override void OnHook()
		{
			base.OnHook();
			m_propertySync = new MyPropertySyncStateGroup(this, base.Instance.SyncType);
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			base.GetStateGroups(resultList);
			if (m_propertySync != null && m_propertySync.PropertyCount > 0)
			{
				resultList.Add(m_propertySync);
			}
		}
	}
}
