using System.Collections.Generic;

namespace VRage.Game
{
	public struct MyFactionMember
	{
		public class FactionComparerType : IEqualityComparer<MyFactionMember>
		{
			public bool Equals(MyFactionMember x, MyFactionMember y)
			{
				return x.PlayerId != y.PlayerId;
			}

			public int GetHashCode(MyFactionMember obj)
			{
				return (int)(obj.PlayerId >> 32) ^ (int)obj.PlayerId;
			}
		}

		public long PlayerId;

		public bool IsLeader;

		public bool IsFounder;

		public static readonly FactionComparerType Comparer = new FactionComparerType();

		public MyFactionMember(long id, bool isLeader, bool isFounder = false)
		{
			PlayerId = id;
			IsLeader = isLeader;
			IsFounder = isFounder;
		}

		public static implicit operator MyFactionMember(MyObjectBuilder_FactionMember v)
		{
			return new MyFactionMember(v.PlayerId, v.IsLeader, v.IsFounder);
		}

		public static implicit operator MyObjectBuilder_FactionMember(MyFactionMember v)
		{
			MyObjectBuilder_FactionMember result = default(MyObjectBuilder_FactionMember);
			result.PlayerId = v.PlayerId;
			result.IsLeader = v.IsLeader;
			result.IsFounder = v.IsFounder;
			return result;
		}
	}
}
