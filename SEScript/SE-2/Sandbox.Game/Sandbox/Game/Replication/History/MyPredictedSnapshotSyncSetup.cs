using System;

namespace Sandbox.Game.Replication.History
{
	public class MyPredictedSnapshotSyncSetup : MySnapshotSyncSetup
	{
		public float MaxPositionFactor;

		public float MinPositionFactor = 1f;

		public float MaxLinearFactor;

		public float MinLinearFactor = 1f;

		public float MaxRotationFactor;

		public float MaxAngularFactor;

		public float MinAngularFactor = 1f;

		public float IterationsFactor;

		public bool UpdateAlways;

		public bool AllowForceStop;

		public bool IsControlled;

		public bool Smoothing = true;

		private MyPredictedSnapshotSyncSetup m_notSmoothed;

		public MyPredictedSnapshotSyncSetup NotSmoothed
		{
			get
			{
				if (m_notSmoothed == null)
				{
					m_notSmoothed = (MemberwiseClone() as MyPredictedSnapshotSyncSetup);
					m_notSmoothed.MaxPositionFactor = Math.Min(1f, MaxPositionFactor);
					m_notSmoothed.MaxLinearFactor = Math.Min(1f, MaxLinearFactor);
					m_notSmoothed.MaxRotationFactor = Math.Min(1f, MaxRotationFactor);
					m_notSmoothed.MaxAngularFactor = Math.Min(1f, MaxAngularFactor);
					m_notSmoothed.Smoothing = false;
				}
				return m_notSmoothed;
			}
		}
	}
}
