using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct AnimationSet
	{
		protected class VRage_Game_AnimationSet_003C_003EProbability_003C_003EAccessor : IMemberAccessor<AnimationSet, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationSet owner, in float value)
			{
				owner.Probability = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationSet owner, out float value)
			{
				value = owner.Probability;
			}
		}

		protected class VRage_Game_AnimationSet_003C_003EContinuous_003C_003EAccessor : IMemberAccessor<AnimationSet, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationSet owner, in bool value)
			{
				owner.Continuous = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationSet owner, out bool value)
			{
				value = owner.Continuous;
			}
		}

		protected class VRage_Game_AnimationSet_003C_003EAnimationItems_003C_003EAccessor : IMemberAccessor<AnimationSet, AnimationItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationSet owner, in AnimationItem[] value)
			{
				owner.AnimationItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationSet owner, out AnimationItem[] value)
			{
				value = owner.AnimationItems;
			}
		}

		private class VRage_Game_AnimationSet_003C_003EActor : IActivator, IActivator<AnimationSet>
		{
			private sealed override object CreateInstance()
			{
				return default(AnimationSet);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override AnimationSet CreateInstance()
			{
				return (AnimationSet)(object)default(AnimationSet);
			}

			AnimationSet IActivator<AnimationSet>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public float Probability;

		[ProtoMember(10)]
		public bool Continuous;

		[ProtoMember(13)]
		public AnimationItem[] AnimationItems;
	}
}
