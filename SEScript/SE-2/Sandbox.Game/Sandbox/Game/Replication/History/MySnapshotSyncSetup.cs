using VRage.Game.Networking;

namespace Sandbox.Game.Replication.History
{
	public class MySnapshotSyncSetup : MySnapshotFlags
	{
		public string ProfileName;

		public bool ExtrapolationSmoothing;

		public bool IgnoreParentId;

		public bool UserTrend;
	}
}
