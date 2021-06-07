using VRage.Library;

namespace VRage.Network
{
	public class MyStateDataEntry : FastPriorityQueue<MyStateDataEntry>.Node
	{
		public readonly NetworkId GroupId;

		public readonly IMyStateGroup Group;

		public readonly IMyReplicable Owner;

		public MyStateDataEntry(IMyReplicable owner, NetworkId groupId, IMyStateGroup group)
		{
			Owner = owner;
			Priority = 0L;
			GroupId = groupId;
			Group = group;
		}

		public override string ToString()
		{
			return $"{Priority:0.00}, {GroupId}, {Group}";
		}
	}
}
