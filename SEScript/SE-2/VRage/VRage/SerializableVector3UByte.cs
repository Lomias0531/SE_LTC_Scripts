using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableVector3UByte
	{
		protected class VRage_SerializableVector3UByte_003C_003EX_003C_003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3UByte owner, in byte value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3UByte owner, out byte value)
			{
				value = owner.X;
			}
		}

		protected class VRage_SerializableVector3UByte_003C_003EY_003C_003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3UByte owner, in byte value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3UByte owner, out byte value)
			{
				value = owner.Y;
			}
		}

		protected class VRage_SerializableVector3UByte_003C_003EZ_003C_003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3UByte owner, in byte value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3UByte owner, out byte value)
			{
				value = owner.Z;
			}
		}

		protected class VRage_SerializableVector3UByte_003C_003Ex_003C_003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3UByte owner, in byte value)
			{
				owner.x = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3UByte owner, out byte value)
			{
				value = owner.x;
			}
		}

		protected class VRage_SerializableVector3UByte_003C_003Ey_003C_003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3UByte owner, in byte value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3UByte owner, out byte value)
			{
				value = owner.y;
			}
		}

		protected class VRage_SerializableVector3UByte_003C_003Ez_003C_003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3UByte owner, in byte value)
			{
				owner.z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3UByte owner, out byte value)
			{
				value = owner.z;
			}
		}

		public byte X;

		public byte Y;

		public byte Z;

		[ProtoMember(1)]
		[XmlAttribute]
		[NoSerialize]
		public byte x
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
		public byte y
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
		public byte z
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

		public SerializableVector3UByte(byte x, byte y, byte z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static implicit operator Vector3UByte(SerializableVector3UByte v)
		{
			return new Vector3UByte(v.X, v.Y, v.Z);
		}

		public static implicit operator SerializableVector3UByte(Vector3UByte v)
		{
			return new SerializableVector3UByte(v.X, v.Y, v.Z);
		}
	}
}
