using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public struct AnimationData
	{
		protected class VRage_Game_ObjectBuilders_Components_AnimationData_003C_003EAnimation_003C_003EAccessor : IMemberAccessor<AnimationData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationData owner, in string value)
			{
				owner.Animation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationData owner, out string value)
			{
				value = owner.Animation;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_AnimationData_003C_003EAnimation2_003C_003EAccessor : IMemberAccessor<AnimationData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationData owner, in string value)
			{
				owner.Animation2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationData owner, out string value)
			{
				value = owner.Animation2;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_AnimationData_003C_003EActor : IActivator, IActivator<AnimationData>
		{
			private sealed override object CreateInstance()
			{
				return default(AnimationData);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override AnimationData CreateInstance()
			{
				return (AnimationData)(object)default(AnimationData);
			}

			AnimationData IActivator<AnimationData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(34)]
		public string Animation;

		[ProtoMember(35)]
		[Nullable]
		public string Animation2;
	}
}
