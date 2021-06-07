using System.Collections.Generic;

namespace VRage.GameServices
{
	public struct MyRemoteStorageEnumerateUserSubscribedFilesResult
	{
		public MyGameServiceCallResult Result;

		public int ResultsReturned;

		public int TotalResultCount;

		public List<ulong> FileIds;

		public ulong this[int i] => FileIds[i];
	}
}
