using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct RGBColor
	{
		protected class VRage_Game_RGBColor_003C_003ER_003C_003EAccessor : IMemberAccessor<RGBColor, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RGBColor owner, in int value)
			{
				owner.R = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RGBColor owner, out int value)
			{
				value = owner.R;
			}
		}

		protected class VRage_Game_RGBColor_003C_003EG_003C_003EAccessor : IMemberAccessor<RGBColor, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RGBColor owner, in int value)
			{
				owner.G = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RGBColor owner, out int value)
			{
				value = owner.G;
			}
		}

		protected class VRage_Game_RGBColor_003C_003EB_003C_003EAccessor : IMemberAccessor<RGBColor, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RGBColor owner, in int value)
			{
				owner.B = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RGBColor owner, out int value)
			{
				value = owner.B;
			}
		}

		private class VRage_Game_RGBColor_003C_003EActor : IActivator, IActivator<RGBColor>
		{
			private sealed override object CreateInstance()
			{
				return default(RGBColor);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override RGBColor CreateInstance()
			{
				return (RGBColor)(object)default(RGBColor);
			}

			RGBColor IActivator<RGBColor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(16)]
		public int R;

		[ProtoMember(19)]
		public int G;

		[ProtoMember(22)]
		public int B;
	}
}
