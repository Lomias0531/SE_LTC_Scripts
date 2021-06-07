namespace VRage.GameServices
{
	public struct MyCloudFileInfo
	{
		public string Name;

		public int Size;

		public long Timestamp;

		public string LocalPath;

		public MyCloudFileInfo(string name, string localPath, int size, long timestamp)
		{
			Name = name;
			Size = size;
			Timestamp = timestamp;
			LocalPath = localPath;
		}
	}
}
