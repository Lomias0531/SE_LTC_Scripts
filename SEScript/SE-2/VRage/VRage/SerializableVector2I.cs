using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableVector2I
	{
		protected class VRage_SerializableVector2I_003C_003EX_003C_003EAccessor : IMemberAccessor<SerializableVector2I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector2I owner, in int value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector2I owner, out int value)
			{
				value = owner.X;
			}
		}

		protected class VRage_SerializableVector2I_003C_003EY_003C_003EAccessor : IMemberAccessor<SerializableVector2I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector2I owner, in int value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector2I owner, out int value)
			{
				value = owner.Y;
			}
		}

		protected class VRage_SerializableVector2I_003C_003Ex_003C_003EAccessor : IMemberAccessor<SerializableVector2I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector2I owner, in int value)
			{
				owner.x = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector2I owner, out int value)
			{
				value = owner.x;
			}
		}

		protected class VRage_SerializableVector2I_003C_003Ey_003C_003EAccessor : IMemberAccessor<SerializableVector2I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector2I owner, in int value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector2I owner, out int value)
			{
				value = owner.y;
			}
		}

		public int X;

		public int Y;

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

		public bool ShouldSerializeX()
		{
			return false;
		}

		public bool ShouldSerializeY()
		{
			return false;
		}

		public SerializableVector2I(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static implicit operator Vector2I(SerializableVector2I v)
		{
			return new Vector2I(v.X, v.Y);
		}

		public static implicit operator SerializableVector2I(Vector2I v)
		{
			return new SerializableVector2I(v.X, v.Y);
		}
	}
}
