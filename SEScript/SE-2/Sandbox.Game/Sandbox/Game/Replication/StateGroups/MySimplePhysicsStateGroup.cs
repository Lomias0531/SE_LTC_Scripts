using Sandbox.Game.Replication.History;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Game.Replication.StateGroups
{
	public class MySimplePhysicsStateGroup : MyEntityPhysicsStateGroupBase
	{
		private readonly MyPredictedSnapshotSyncSetup m_settings;

		public MySimplePhysicsStateGroup(MyEntity entity, IMyReplicable owner, MyPredictedSnapshotSyncSetup settings)
			: base(entity, owner)
		{
			m_settings = settings;
		}

		public override void ClientUpdate(MyTimeSpan clientTimestamp)
		{
			m_snapshotSync.Update(clientTimestamp, m_settings);
		}
	}
}
