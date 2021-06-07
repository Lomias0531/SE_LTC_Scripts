using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableQuaternion
	{
		protected class VRage_SerializableQuaternion_003C_003EX_003C_003EAccessor : IMemberAccessor<SerializableQuaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableQuaternion owner, in float value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableQuaternion owner, out float value)
			{
				value = owner.X;
			}
		}

		protected class VRage_SerializableQuaternion_003C_003EY_003C_003EAccessor : IMemberAccessor<SerializableQuaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableQuaternion owner, in float value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableQuaternion owner, out float value)
			{
				value = owner.Y;
			}
		}

		protected class VRage_SerializableQuaternion_003C_003EZ_003C_003EAccessor : IMemberAccessor<SerializableQuaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableQuaternion owner, in float value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableQuaternion owner, out float value)
			{
				value = owner.Z;
			}
		}

		protected class VRage_SerializableQuaternion_003C_003EW_003C_003EAccessor : IMemberAccessor<SerializableQuaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableQuaternion owner, in float value)
			{
				owner.W = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableQuaternion owner, out float value)
			{
				value = owner.W;
			}
		}

		protected class VRage_SerializableQuaternion_003C_003Ex_003C_003EAccessor : IMemberAccessor<SerializableQuaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableQuaternion owner, in float value)
			{
				owner.x = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableQuaternion owner, out float value)
			{
				value = owner.x;
			}
		}

		protected class VRage_SerializableQuaternion_003C_003Ey_003C_003EAccessor : IMemberAccessor<SerializableQuaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableQuaternion owner, in float value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableQuaternion owner, out float value)
			{
				value = owner.y;
			}
		}

		protected class VRage_SerializableQuaternion_003C_003Ez_003C_003EAccessor : IMemberAccessor<SerializableQuaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableQuaternion owner, in float value)
			{
				owner.z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableQuaternion owner, out float value)
			{
				value = owner.z;
			}
		}

		protected class VRage_SerializableQuaternion_003C_003Ew_003C_003EAccessor : IMemberAccessor<SerializableQuaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableQuaternion owner, in float value)
			{
				owner.w = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableQuaternion owner, out float value)
			{
				value = owner.w;
			}
		}

		public float X;

		public float Y;

		public float Z;

		public float W;

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

		[ProtoMember(7)]
		[XmlAttribute]
		[NoSerialize]
		public float z
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

		[ProtoMember(10)]
		[XmlAttribute]
		[NoSerialize]
		public float w
		{
			get
			{
				return W;
			}
			set
			{
				W = value;
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

		public bool ShouldSerializeW()
		{
			return false;
		}

		public SerializableQuaternion(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static implicit operator Quaternion(SerializableQuaternion q)
		{
			return new Quaternion(q.X, q.Y, q.Z, q.W);
		}

		public static implicit operator SerializableQuaternion(Quaternion q)
		{
			return new SerializableQuaternion(q.X, q.Y, q.Z, q.W);
		}
	}
}
