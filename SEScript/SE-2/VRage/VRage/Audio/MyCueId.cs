using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Utils;

namespace VRage.Audio
{
	[ProtoContract]
	public struct MyCueId
	{
		public class ComparerType : IEqualityComparer<MyCueId>
		{
			bool IEqualityComparer<MyCueId>.Equals(MyCueId x, MyCueId y)
			{
				return x.Hash == y.Hash;
			}

			int IEqualityComparer<MyCueId>.GetHashCode(MyCueId obj)
			{
				return obj.Hash.GetHashCode();
			}
		}

		protected class VRage_Audio_MyCueId_003C_003EHash_003C_003EAccessor : IMemberAccessor<MyCueId, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCueId owner, in MyStringHash value)
			{
				owner.Hash = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCueId owner, out MyStringHash value)
			{
				value = owner.Hash;
			}
		}

		[ProtoMember(1)]
		public MyStringHash Hash;

		public static readonly ComparerType Comparer = new ComparerType();

		public bool IsNull => Hash == MyStringHash.NullOrEmpty;

		public MyCueId(MyStringHash hash)
		{
			Hash = hash;
		}

		public static bool operator ==(MyCueId r, MyCueId l)
		{
			return r.Hash == l.Hash;
		}

		public static bool operator !=(MyCueId r, MyCueId l)
		{
			return r.Hash != l.Hash;
		}

		public bool Equals(MyCueId obj)
		{
			return obj.Hash.Equals(Hash);
		}

		public override bool Equals(object obj)
		{
			if (obj is MyCueId)
			{
				return ((MyCueId)obj).Hash.Equals(Hash);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Hash.GetHashCode();
		}

		public override string ToString()
		{
			return Hash.ToString();
		}
	}
}
