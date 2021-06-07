using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct MyOrientation
	{
		protected class VRage_MyOrientation_003C_003EYaw_003C_003EAccessor : IMemberAccessor<MyOrientation, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyOrientation owner, in float value)
			{
				owner.Yaw = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyOrientation owner, out float value)
			{
				value = owner.Yaw;
			}
		}

		protected class VRage_MyOrientation_003C_003EPitch_003C_003EAccessor : IMemberAccessor<MyOrientation, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyOrientation owner, in float value)
			{
				owner.Pitch = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyOrientation owner, out float value)
			{
				value = owner.Pitch;
			}
		}

		protected class VRage_MyOrientation_003C_003ERoll_003C_003EAccessor : IMemberAccessor<MyOrientation, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyOrientation owner, in float value)
			{
				owner.Roll = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyOrientation owner, out float value)
			{
				value = owner.Roll;
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public float Yaw;

		[ProtoMember(4)]
		[XmlAttribute]
		public float Pitch;

		[ProtoMember(7)]
		[XmlAttribute]
		public float Roll;

		public MyOrientation(float yaw, float pitch, float roll)
		{
			Yaw = yaw;
			Pitch = pitch;
			Roll = roll;
		}

		public Quaternion ToQuaternion()
		{
			return Quaternion.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
		}

		public override bool Equals(object obj)
		{
			if (obj is MyOrientation)
			{
				MyOrientation value = (MyOrientation)obj;
				return this == value;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((((int)(Yaw * 997f) * 397) ^ (int)(Pitch * 997f)) * 397) ^ (int)(Roll * 997f);
		}

		public static bool operator ==(MyOrientation value1, MyOrientation value2)
		{
			if (value1.Yaw == value2.Yaw && value1.Pitch == value2.Pitch && value1.Roll == value2.Roll)
			{
				return true;
			}
			return false;
		}

		public static bool operator !=(MyOrientation value1, MyOrientation value2)
		{
			if (value1.Yaw != value2.Yaw || value1.Pitch != value2.Pitch || value1.Roll != value2.Roll)
			{
				return true;
			}
			return false;
		}
	}
}
