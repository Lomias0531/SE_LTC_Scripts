namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Broadcast listeners scan the network for broadcasted messages with specific tag.
	/// </summary>
	public interface IMyBroadcastListener : IMyMessageProvider
	{
		/// <summary>
		/// Gets the tag this broadcast listener is listening for.
		/// </summary>
		string Tag
		{
			get;
		}

		/// <summary>
		/// Gets a value that indicates whether the broadcast listener is active.
		/// </summary>
		bool IsActive
		{
			get;
		}
	}
}
