using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct AnimationItem
	{
		protected class VRage_Game_AnimationItem_003C_003ERatio_003C_003EAccessor : IMemberAccessor<AnimationItem, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationItem owner, in float value)
			{
				owner.Ratio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationItem owner, out float value)
			{
				value = owner.Ratio;
			}
		}

		protected class VRage_Game_AnimationItem_003C_003EAnimation_003C_003EAccessor : IMemberAccessor<AnimationItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationItem owner, in string value)
			{
				owner.Animation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationItem owner, out string value)
			{
				value = owner.Animation;
			}
		}

		private class VRage_Game_AnimationItem_003C_003EActor : IActivator, IActivator<AnimationItem>
		{
			private sealed override object CreateInstance()
			{
				return default(AnimationItem);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override AnimationItem CreateInstance()
			{
				return (AnimationItem)(object)default(AnimationItem);
			}

			AnimationItem IActivator<AnimationItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float Ratio;

		[ProtoMember(4)]
		public string Animation;
	}
}
