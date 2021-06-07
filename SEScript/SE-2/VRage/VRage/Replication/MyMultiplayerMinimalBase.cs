namespace VRage.Replication
{
	/// <summary>
	/// This stub should be merged with MyMultiplayerBase as soon as possible and the result should be put in planned library VRage.Multiplayer.
	/// </summary>
	public abstract class MyMultiplayerMinimalBase
	{
		public static MyMultiplayerMinimalBase Instance;

		public readonly bool IsServer;

		protected abstract bool IsServerInternal
		{
			get;
		}

		protected MyMultiplayerMinimalBase()
		{
			IsServer = IsServerInternal;
		}
	}
}
