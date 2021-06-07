namespace VRage.GameServices
{
	public delegate void MyLobbyChatUpdated(IMyLobby lobby, ulong changedUserId, ulong makingChangeUserId, MyChatMemberStateChangeEnum stateChange, MyLobbyStatusCode reason);
}
