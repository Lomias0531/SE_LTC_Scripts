namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes what permissions a user has
	/// </summary>
	public enum MyPromoteLevel
	{
		/// <summary>
		/// Normal players
		/// </summary>
		None,
		/// <summary>
		/// Can edit scripts when the scripter role is enabled
		/// </summary>
		Scripter,
		/// <summary>
		/// Can kick and ban players, has access to 'Show All Players' option in Admin Tools menu
		/// </summary>
		Moderator,
		/// <summary>
		/// Has access to Space Master tools
		/// </summary>
		SpaceMaster,
		/// <summary>
		/// Has access to Admin tools
		/// </summary>
		Admin,
		/// <summary>
		/// Admins listed in server config, cannot be demoted
		/// </summary>
		Owner
	}
}
