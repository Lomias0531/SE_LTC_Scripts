using VRage.Library.Utils;

namespace Sandbox.Game.Replication.History
{
	public interface IMySnapshotSync
	{
		void Destroy();

		long Update(MyTimeSpan clientTimestamp, MySnapshotSyncSetup setup);

		void Read(ref MySnapshot item, MyTimeSpan timeStamp);

		void Reset(bool reinit = false);
	}
}
