using VRage.Sync;

namespace VRage.Network
{
	/// <summary>
	/// Entity containing sync properties.
	/// </summary>
	public interface IMySyncedEntity
	{
		SyncType SyncType
		{
			get;
			set;
		}
	}
}
