using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct MyPositionAndOrientation
	{
		protected class VRage_MyPositionAndOrientation_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyPositionAndOrientation, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPositionAndOrientation owner, in SerializableVector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPositionAndOrientation owner, out SerializableVector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_MyPositionAndOrientation_003C_003EForward_003C_003EAccessor : IMemberAccessor<MyPositionAndOrientation, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPositionAndOrientation owner, in SerializableVector3 value)
			{
				owner.Forward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPositionAndOrientation owner, out SerializableVector3 value)
			{
				value = owner.Forward;
			}
		}

		protected class VRage_MyPositionAndOrientation_003C_003EUp_003C_003EAccessor : IMemberAccessor<MyPositionAndOrientation, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPositionAndOrientation owner, in SerializableVector3 value)
			{
				owner.Up = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPositionAndOrientation owner, out SerializableVector3 value)
			{
				value = owner.Up;
			}
		}

		protected class VRage_MyPositionAndOrientation_003C_003EOrientation_003C_003EAccessor : IMemberAccessor<MyPositionAndOrientation, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPositionAndOrientation owner, in Quaternion value)
			{
				owner.Orientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPositionAndOrientation owner, out Quaternion value)
			{
				value = owner.Orientation;
			}
		}

		[ProtoMember(1)]
		[XmlElement("Position")]
		public SerializableVector3D Position;

		[ProtoMember(4)]
		[XmlElement("Forward")]
		[NoSerialize]
		public SerializableVector3 Forward;

		[ProtoMember(7)]
		[XmlElement("Up")]
		[NoSerialize]
		public SerializableVector3 Up;

		public static readonly MyPositionAndOrientation Default = new MyPositionAndOrientation(Vector3.Zero, Vector3.Forward, Vector3.Up);

		[Serialize(MyPrimitiveFlags.Normalized)]
		public Quaternion Orientation
		{
			get
			{
				return Quaternion.CreateFromRotationMatrix(GetMatrix());
			}
			set
			{
				Matrix matrix = Matrix.CreateFromQuaternion(value);
				Forward = matrix.Forward;
				Up = matrix.Up;
			}
		}

		public MyPositionAndOrientation(Vector3D position, Vector3 forward, Vector3 up)
		{
			Position = position;
			Forward = forward;
			Up = up;
		}

		public MyPositionAndOrientation(ref MatrixD matrix)
		{
			Position = matrix.Translation;
			Forward = (Vector3)matrix.Forward;
			Up = (Vector3)matrix.Up;
		}

		public MyPositionAndOrientation(MatrixD matrix)
			: this(matrix.Translation, matrix.Forward, matrix.Up)
		{
		}

		public MatrixD GetMatrix()
		{
			return MatrixD.CreateWorld(Position, Forward, Up);
		}

		public override string ToString()
		{
			return Position.ToString() + "; " + Forward.ToString() + "; " + Up;
		}
	}
}
