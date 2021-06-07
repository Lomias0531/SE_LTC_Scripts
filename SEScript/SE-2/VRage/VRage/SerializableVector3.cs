using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableVector3
	{
		protected class VRage_SerializableVector3_003C_003EX_003C_003EAccessor : IMemberAccessor<SerializableVector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3 owner, in float value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3 owner, out float value)
			{
				value = owner.X;
			}
		}

		protected class VRage_SerializableVector3_003C_003EY_003C_003EAccessor : IMemberAccessor<SerializableVector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3 owner, in float value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3 owner, out float value)
			{
				value = owner.Y;
			}
		}

		protected class VRage_SerializableVector3_003C_003EZ_003C_003EAccessor : IMemberAccessor<SerializableVector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3 owner, in float value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3 owner, out float value)
			{
				value = owner.Z;
			}
		}

		protected class VRage_SerializableVector3_003C_003Ex_003C_003EAccessor : IMemberAccessor<SerializableVector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3 owner, in float value)
			{
				owner.x = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3 owner, out float value)
			{
				value = owner.x;
			}
		}

		protected class VRage_SerializableVector3_003C_003Ey_003C_003EAccessor : IMemberAccessor<SerializableVector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3 owner, in float value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3 owner, out float value)
			{
				value = owner.y;
			}
		}

		protected class VRage_SerializableVector3_003C_003Ez_003C_003EAccessor : IMemberAccessor<SerializableVector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3 owner, in float value)
			{
				owner.z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3 owner, out float value)
			{
				value = owner.z;
			}
		}

		public float X;

		public float Y;

		public float Z;

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

		public bool IsZero
		{
			get
			{
				if (X == 0f && Y == 0f)
				{
					return Z == 0f;
				}
				return false;
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

		public SerializableVector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static implicit operator Vector3(SerializableVector3 v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}

		public static implicit operator SerializableVector3(Vector3 v)
		{
			return new SerializableVector3(v.X, v.Y, v.Z);
		}

		public static bool operator ==(SerializableVector3 a, SerializableVector3 b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z == b.Z;
			}
			return false;
		}

		public static bool operator !=(SerializableVector3 a, SerializableVector3 b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z != b.Z;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj is SerializableVector3)
			{
				return (SerializableVector3)obj == this;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (X.GetHashCode() * 1610612741) ^ (Y.GetHashCode() * 24593) ^ Z.GetHashCode();
		}
	}
}
