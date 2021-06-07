namespace VRage.Game.ModAPI
{
	public interface IMyGamePaths
	{
		string ContentPath
		{
			get;
		}

		string ModsPath
		{
			get;
		}

		string UserDataPath
		{
			get;
		}

		string SavesPath
		{
			get;
		}

		/// <summary>
		/// Gets the calling mod's assembly ScopeName. This name is used in storage paths (eg. 1234567.sbm_TypeName).
		/// </summary>
		string ModScopeName
		{
			get;
		}
	}
}
