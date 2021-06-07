namespace VRage.GameServices
{
	/// <summary>
	/// Common interface for MyGameServerItem and IMyLobby
	/// </summary>
	public interface IMyMultiplayerGame
	{
		ulong GameID
		{
			get;
		}
	}
}
