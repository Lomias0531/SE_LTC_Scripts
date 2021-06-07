using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableVector3D
	{
		protected class VRage_SerializableVector3D_003C_003EX_003C_003EAccessor : IMemberAccessor<SerializableVector3D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3D owner, in double value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3D owner, out double value)
			{
				value = owner.X;
			}
		}

		protected class VRage_SerializableVector3D_003C_003EY_003C_003EAccessor : IMemberAccessor<SerializableVector3D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3D owner, in double value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3D owner, out double value)
			{
				value = owner.Y;
			}
		}

		protected class VRage_SerializableVector3D_003C_003EZ_003C_003EAccessor : IMemberAccessor<SerializableVector3D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3D owner, in double value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3D owner, out double value)
			{
				value = owner.Z;
			}
		}

		protected class VRage_SerializableVector3D_003C_003Ex_003C_003EAccessor : IMemberAccessor<SerializableVector3D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3D owner, in double value)
			{
				owner.x = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3D owner, out double value)
			{
				value = owner.x;
			}
		}

		protected class VRage_SerializableVector3D_003C_003Ey_003C_003EAccessor : IMemberAccessor<SerializableVector3D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3D owner, in double value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3D owner, out double value)
			{
				value = owner.y;
			}
		}

		protected class VRage_SerializableVector3D_003C_003Ez_003C_003EAccessor : IMemberAccessor<SerializableVector3D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableVector3D owner, in double value)
			{
				owner.z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableVector3D owner, out double value)
			{
				value = owner.z;
			}
		}

		public double X;

		public double Y;

		public double Z;

		[ProtoMember(1)]
		[XmlAttribute]
		[NoSerialize]
		public double x
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
		public double y
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
		public double z
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
				if (X == 0.0 && Y == 0.0)
				{
					return Z == 0.0;
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

		public SerializableVector3D(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public SerializableVector3D(Vector3D v)
		{
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		public static implicit operator Vector3D(SerializableVector3D v)
		{
			return new Vector3D(v.X, v.Y, v.Z);
		}

		public static implicit operator SerializableVector3D(Vector3D v)
		{
			return new SerializableVector3D(v.X, v.Y, v.Z);
		}

		public override string ToString()
		{
			return "X: " + X + " Y: " + Y + " Z: " + Z;
		}
	}
}
