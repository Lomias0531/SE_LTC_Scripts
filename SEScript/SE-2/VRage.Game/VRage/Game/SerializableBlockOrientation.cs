using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public struct SerializableBlockOrientation
	{
		protected class VRage_Game_SerializableBlockOrientation_003C_003EForward_003C_003EAccessor : IMemberAccessor<SerializableBlockOrientation, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBlockOrientation owner, in Base6Directions.Direction value)
			{
				owner.Forward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBlockOrientation owner, out Base6Directions.Direction value)
			{
				value = owner.Forward;
			}
		}

		protected class VRage_Game_SerializableBlockOrientation_003C_003EUp_003C_003EAccessor : IMemberAccessor<SerializableBlockOrientation, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBlockOrientation owner, in Base6Directions.Direction value)
			{
				owner.Up = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBlockOrientation owner, out Base6Directions.Direction value)
			{
				value = owner.Up;
			}
		}

		private class VRage_Game_SerializableBlockOrientation_003C_003EActor : IActivator, IActivator<SerializableBlockOrientation>
		{
			private sealed override object CreateInstance()
			{
				return default(SerializableBlockOrientation);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override SerializableBlockOrientation CreateInstance()
			{
				return (SerializableBlockOrientation)(object)default(SerializableBlockOrientation);
			}

			SerializableBlockOrientation IActivator<SerializableBlockOrientation>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static readonly SerializableBlockOrientation Identity = new SerializableBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

		[ProtoMember(1)]
		[XmlAttribute]
		public Base6Directions.Direction Forward;

		[ProtoMember(4)]
		[XmlAttribute]
		public Base6Directions.Direction Up;

		public SerializableBlockOrientation(Base6Directions.Direction forward, Base6Directions.Direction up)
		{
			Forward = forward;
			Up = up;
		}

		public SerializableBlockOrientation(ref Quaternion q)
		{
			Forward = Base6Directions.GetForward(q);
			Up = Base6Directions.GetUp(q);
		}

		public static implicit operator MyBlockOrientation(SerializableBlockOrientation v)
		{
			if (Base6Directions.IsValidBlockOrientation(v.Forward, v.Up))
			{
				return new MyBlockOrientation(v.Forward, v.Up);
			}
			if (v.Up == Base6Directions.Direction.Forward)
			{
				return new MyBlockOrientation(v.Forward, Base6Directions.Direction.Up);
			}
			return MyBlockOrientation.Identity;
		}

		public static implicit operator SerializableBlockOrientation(MyBlockOrientation v)
		{
			return new SerializableBlockOrientation(v.Forward, v.Up);
		}

		public static bool operator ==(SerializableBlockOrientation a, SerializableBlockOrientation b)
		{
			if (a.Forward == b.Forward)
			{
				return a.Up == b.Up;
			}
			return false;
		}

		public static bool operator !=(SerializableBlockOrientation a, SerializableBlockOrientation b)
		{
			if (a.Forward == b.Forward)
			{
				return a.Up != b.Up;
			}
			return true;
		}
	}
}
