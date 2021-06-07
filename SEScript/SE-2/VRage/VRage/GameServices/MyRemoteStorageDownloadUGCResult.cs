namespace VRage.GameServices
{
	public struct MyRemoteStorageDownloadUGCResult
	{
		public uint AppID;

		public ulong FileHandle;

		public string FileName;

		public MyGameServiceCallResult Result;

		public int SizeInBytes;

		public ulong SteamIDOwner;
	}
}
