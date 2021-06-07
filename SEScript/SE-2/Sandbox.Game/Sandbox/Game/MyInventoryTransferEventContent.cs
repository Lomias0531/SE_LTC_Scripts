using VRage;
using VRage.Utils;

namespace Sandbox.Game
{
	public struct MyInventoryTransferEventContent
	{
		public MyFixedPoint Amount;

		public uint ItemId;

		public long SourceOwnerId;

		public MyStringHash SourceInventoryId;

		public long DestinationOwnerId;

		public MyStringHash DestinationInventoryId;
	}
}
