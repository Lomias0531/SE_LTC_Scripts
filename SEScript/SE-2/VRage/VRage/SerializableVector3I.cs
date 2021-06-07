using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableVector3I
	{
		protected class VRage_SerializableVector3I_003C_003EX_003C_003EAccessor : IMemberAccessor<SerializableVector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3I owner, in int value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3I owner, out int value)
			{
				value = owner.X;
			}
		}

		protected class VRage_SerializableVector3I_003C_003EY_003C_003EAccessor : IMemberAccessor<SerializableVector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3I owner, in int value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3I owner, out int value)
			{
				value = owner.Y;
			}
		}

		protected class VRage_SerializableVector3I_003C_003EZ_003C_003EAccessor : IMemberAccessor<SerializableVector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3I owner, in int value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3I owner, out int value)
			{
				value = owner.Z;
			}
		}

		protected class VRage_SerializableVector3I_003C_003Ex_003C_003EAccessor : IMemberAccessor<SerializableVector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3I owner, in int value)
			{
				owner.x = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3I owner, out int value)
			{
				value = owner.x;
			}
		}

		protected class VRage_SerializableVector3I_003C_003Ey_003C_003EAccessor : IMemberAccessor<SerializableVector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3I owner, in int value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3I owner, out int value)
			{
				value = owner.y;
			}
		}

		protected class VRage_SerializableVector3I_003C_003Ez_003C_003EAccessor : IMemberAccessor<SerializableVector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3I owner, in int value)
			{
				owner.z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3I owner, out int value)
			{
				value = owner.z;
			}
		}

		public int X;

		public int Y;

		public int Z;

		[ProtoMember(1)]
		[XmlAttribute]
		[NoSerialize]
		public int x
		{
			get
			{
				return X;
			}
			set
			{
				X = value;
			}
		}

		[ProtoMember(4)]
		[XmlAttribute]
		[NoSerialize]
		public int y
		{
			get
			{
				return Y;
			}
			set
			{
				Y = value;
			}
		}

		[ProtoMember(7)]
		[XmlAttribute]
		[NoSerialize]
		public int z
		{
			get
			{
				return Z;
			}
			set
			{
				Z = value;
			}
		}

		public bool ShouldSerializeX()
		{
			return false;
		}

		public bool ShouldSerializeY()
		{
			return false;
		}

		public bool ShouldSerializeZ()
		{
			return false;
		}

		public SerializableVector3I(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static implicit operator Vector3I(SerializableVector3I v)
		{
			return new Vector3I(v.X, v.Y, v.Z);
		}

		public static implicit operator SerializableVector3I(Vector3I v)
		{
			return new SerializableVector3I(v.X, v.Y, v.Z);
		}

		public static bool operator ==(SerializableVector3I a, SerializableVector3I b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z == b.Z;
			}
			return false;
		}

		public static bool operator !=(SerializableVector3I a, SerializableVector3I b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z != b.Z;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj is SerializableVector3I)
			{
				return (SerializableVector3I)obj == this;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (X.GetHashCode() * 1610612741) ^ (Y.GetHashCode() * 24593) ^ Z.GetHashCode();
		}
	}
}
