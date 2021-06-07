namespace VRage.Network
{
	public enum JoinResult
	{
		OK,
		AlreadyJoined,
		TicketInvalid,
		SteamServersOffline,
		NotInGroup,
		GroupIdInvalid,
		ServerFull,
		BannedByAdmins,
		KickedRecently,
		TicketCanceled,
		TicketAlreadyUsed,
		LoggedInElseWhere,
		NoLicenseOrExpired,
		UserNotConnected,
		VACBanned,
		VACCheckTimedOut,
		PasswordRequired,
		WrongPassword,
		ExperimentalMode,
		ProfilingNotAllowed
	}
}
