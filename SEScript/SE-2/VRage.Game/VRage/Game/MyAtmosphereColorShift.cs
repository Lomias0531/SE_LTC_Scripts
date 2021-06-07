using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public class MyAtmosphereColorShift
	{
		protected class VRage_Game_MyAtmosphereColorShift_003C_003ER_003C_003EAccessor : IMemberAccessor<MyAtmosphereColorShift, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereColorShift owner, in SerializableRange value)
			{
				owner.R = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereColorShift owner, out SerializableRange value)
			{
				value = owner.R;
			}
		}

		protected class VRage_Game_MyAtmosphereColorShift_003C_003EG_003C_003EAccessor : IMemberAccessor<MyAtmosphereColorShift, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereColorShift owner, in SerializableRange value)
			{
				owner.G = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereColorShift owner, out SerializableRange value)
			{
				value = owner.G;
			}
		}

		protected class VRage_Game_MyAtmosphereColorShift_003C_003EB_003C_003EAccessor : IMemberAccessor<MyAtmosphereColorShift, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereColorShift owner, in SerializableRange value)
			{
				owner.B = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereColorShift owner, out SerializableRange value)
			{
				value = owner.B;
			}
		}

		private class VRage_Game_MyAtmosphereColorShift_003C_003EActor : IActivator, IActivator<MyAtmosphereColorShift>
		{
			private sealed override object CreateInstance()
			{
				return new MyAtmosphereColorShift();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAtmosphereColorShift CreateInstance()
			{
				return new MyAtmosphereColorShift();
			}

			MyAtmosphereColorShift IActivator<MyAtmosphereColorShift>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(69)]
		public SerializableRange R;

		[ProtoMember(70)]
		public SerializableRange G;

		[ProtoMember(71)]
		public SerializableRange B;
	}
}
