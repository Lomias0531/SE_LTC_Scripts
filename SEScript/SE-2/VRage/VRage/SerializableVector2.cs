using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableVector2
	{
		protected class VRage_SerializableVector2_003C_003EX_003C_003EAccessor : IMemberAccessor<SerializableVector2, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector2 owner, in float value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector2 owner, out float value)
			{
				value = owner.X;
			}
		}

		protected class VRage_SerializableVector2_003C_003EY_003C_003EAccessor : IMemberAccessor<SerializableVector2, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector2 owner, in float value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector2 owner, out float value)
			{
				value = owner.Y;
			}
		}

		protected class VRage_SerializableVector2_003C_003Ex_003C_003EAccessor : IMemberAccessor<SerializableVector2, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector2 owner, in float value)
			{
				owner.x = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector2 owner, out float value)
			{
				value = owner.x;
			}
		}

		protected class VRage_SerializableVector2_003C_003Ey_003C_003EAccessor : IMemberAccessor<SerializableVector2, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector2 owner, in float value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector2 owner, out float value)
			{
				value = owner.y;
			}
		}

		public float X;

		public float Y;

		[ProtoMember(1)]
		[XmlAttribute]
		[NoSerialize]
		public float x
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
		public float y
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

		public SerializableVector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		public static implicit operator Vector2(SerializableVector2 v)
		{
			return new Vector2(v.X, v.Y);
		}

		public static implicit operator SerializableVector2(Vector2 v)
		{
			return new SerializableVector2(v.X, v.Y);
		}
	}
}
