using Sandbox.Game.Replication.History;
using VRage.Game.Entity;
using VRage.Network;

namespace Sandbox.Game.Replication.StateGroups
{
	public class MyMissileStateGroup : MySimplePhysicsStateGroup
	{
		public MyMissileStateGroup(MyEntity entity, IMyReplicable owner, MyPredictedSnapshotSyncSetup settings)
			: base(entity, owner, settings)
		{
		}

		public override bool IsStillDirty(Endpoint forClient)
		{
			return true;
		}
	}
}
