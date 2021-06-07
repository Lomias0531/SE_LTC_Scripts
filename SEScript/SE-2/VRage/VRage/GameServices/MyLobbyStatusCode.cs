namespace VRage.GameServices
{
	public enum MyLobbyStatusCode
	{
		Success = 1,
		DoesntExist = 2,
		NotAllowed = 3,
		Full = 4,
		Error = 5,
		Banned = 6,
		Limited = 7,
		ClanDisabled = 8,
		CommunityBan = 9,
		MemberBlockedYou = 10,
		YouBlockedMember = 11,
		FriendsOnly = 100,
		Cancelled = 200,
		LostInternetConnection = 201,
		ServiceUnavailable = 202,
		NoDirectConnections = 203,
		VersionMismatch = 204,
		UserMultiplayerRestricted = 205,
		ConnectionProblems = 206,
		InvalidPasscode = 207,
		NoUser = 208
	}
}
