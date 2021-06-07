using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyEncounterId : IEquatable<MyEncounterId>
	{
		protected class VRage_Game_MyEncounterId_003C_003EBoundingBox_003C_003EAccessor : IMemberAccessor<MyEncounterId, BoundingBoxD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEncounterId owner, in BoundingBoxD value)
			{
				owner.BoundingBox = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEncounterId owner, out BoundingBoxD value)
			{
				value = owner.BoundingBox;
			}
		}

		protected class VRage_Game_MyEncounterId_003C_003ESeed_003C_003EAccessor : IMemberAccessor<MyEncounterId, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEncounterId owner, in int value)
			{
				owner.Seed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEncounterId owner, out int value)
			{
				value = owner.Seed;
			}
		}

		protected class VRage_Game_MyEncounterId_003C_003EEncounterId_003C_003EAccessor : IMemberAccessor<MyEncounterId, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEncounterId owner, in int value)
			{
				owner.EncounterId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEncounterId owner, out int value)
			{
				value = owner.EncounterId;
			}
		}

		private class VRage_Game_MyEncounterId_003C_003EActor : IActivator, IActivator<MyEncounterId>
		{
			private sealed override object CreateInstance()
			{
				return default(MyEncounterId);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEncounterId CreateInstance()
			{
				return (MyEncounterId)(object)default(MyEncounterId);
			}

			MyEncounterId IActivator<MyEncounterId>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public BoundingBoxD BoundingBox;

		[ProtoMember(4)]
		public int Seed;

		[ProtoMember(7)]
		public int EncounterId;

		public MyEncounterId(BoundingBoxD box, int seed, int encounterId)
		{
			Seed = seed;
			EncounterId = encounterId;
			BoundingBox = box.Round(2);
		}

		public static bool operator ==(MyEncounterId x, MyEncounterId y)
		{
			if (x.BoundingBox.Equals(y.BoundingBox, 2.0) && x.Seed == y.Seed)
			{
				return x.EncounterId == y.EncounterId;
			}
			return false;
		}

		public static bool operator !=(MyEncounterId x, MyEncounterId y)
		{
			return !(x == y);
		}

		public override bool Equals(object o)
		{
			if (o is MyEncounterId)
			{
				return Equals((MyEncounterId)o);
			}
			return false;
		}

		public bool Equals(MyEncounterId other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return Seed;
		}

		public override string ToString()
		{
			return $"{(int)Seed}:{(int)EncounterId}_{BoundingBox}";
		}
	}
}
