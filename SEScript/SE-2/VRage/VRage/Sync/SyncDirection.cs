namespace VRage.Sync
{
	/// <summary>
	/// Sync variable synchronization direction.
	/// </summary>
	public class SyncDirection
	{
		public class BothWays : SyncDirection
		{
		}

		/// Allow writing on server, send to client, deny writing on client.
		public class FromServer : SyncDirection
		{
		}
	}
}
