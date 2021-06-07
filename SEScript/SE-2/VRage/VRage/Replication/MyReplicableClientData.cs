namespace VRage.Replication
{
	public class MyReplicableClientData
	{
		public bool IsPending = true;

		public bool IsStreaming;

		/// <summary>
		/// Returns true when replicable is not pending and is not sleeping.
		/// </summary>
		public bool HasActiveStateSync => !IsPending;
	}
}
