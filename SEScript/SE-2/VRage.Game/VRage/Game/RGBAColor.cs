using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct RGBAColor
	{
		protected class VRage_Game_RGBAColor_003C_003ER_003C_003EAccessor : IMemberAccessor<RGBAColor, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RGBAColor owner, in int value)
			{
				owner.R = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RGBAColor owner, out int value)
			{
				value = owner.R;
			}
		}

		protected class VRage_Game_RGBAColor_003C_003EG_003C_003EAccessor : IMemberAccessor<RGBAColor, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RGBAColor owner, in int value)
			{
				owner.G = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RGBAColor owner, out int value)
			{
				value = owner.G;
			}
		}

		protected class VRage_Game_RGBAColor_003C_003EB_003C_003EAccessor : IMemberAccessor<RGBAColor, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RGBAColor owner, in int value)
			{
				owner.B = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RGBAColor owner, out int value)
			{
				value = owner.B;
			}
		}

		protected class VRage_Game_RGBAColor_003C_003EA_003C_003EAccessor : IMemberAccessor<RGBAColor, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RGBAColor owner, in int value)
			{
				owner.A = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RGBAColor owner, out int value)
			{
				value = owner.A;
			}
		}

		private class VRage_Game_RGBAColor_003C_003EActor : IActivator, IActivator<RGBAColor>
		{
			private sealed override object CreateInstance()
			{
				return default(RGBAColor);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override RGBAColor CreateInstance()
			{
				return (RGBAColor)(object)default(RGBAColor);
			}

			RGBAColor IActivator<RGBAColor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public int R;

		[ProtoMember(7)]
		public int G;

		[ProtoMember(10)]
		public int B;

		[ProtoMember(13)]
		public int A;
	}
}
