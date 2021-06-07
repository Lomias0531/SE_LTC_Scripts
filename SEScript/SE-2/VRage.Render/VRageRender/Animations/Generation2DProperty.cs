using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageRender.Animations
{
	[ProtoContract]
	public class Generation2DProperty
	{
		protected class VRageRender_Animations_Generation2DProperty_003C_003EKeys_003C_003EAccessor : IMemberAccessor<Generation2DProperty, List<AnimationKey>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Generation2DProperty owner, in List<AnimationKey> value)
			{
				owner.Keys = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Generation2DProperty owner, out List<AnimationKey> value)
			{
				value = owner.Keys;
			}
		}

		[ProtoMember(31)]
		public List<AnimationKey> Keys;
	}
}
