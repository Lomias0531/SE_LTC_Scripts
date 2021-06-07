using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyJetpackThrustDefinition
	{
		protected class VRage_Game_MyJetpackThrustDefinition_003C_003EThrustBone_003C_003EAccessor : IMemberAccessor<MyJetpackThrustDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyJetpackThrustDefinition owner, in string value)
			{
				owner.ThrustBone = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyJetpackThrustDefinition owner, out string value)
			{
				value = owner.ThrustBone;
			}
		}

		protected class VRage_Game_MyJetpackThrustDefinition_003C_003ESideFlameOffset_003C_003EAccessor : IMemberAccessor<MyJetpackThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyJetpackThrustDefinition owner, in float value)
			{
				owner.SideFlameOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyJetpackThrustDefinition owner, out float value)
			{
				value = owner.SideFlameOffset;
			}
		}

		protected class VRage_Game_MyJetpackThrustDefinition_003C_003EFrontFlameOffset_003C_003EAccessor : IMemberAccessor<MyJetpackThrustDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyJetpackThrustDefinition owner, in float value)
			{
				owner.FrontFlameOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyJetpackThrustDefinition owner, out float value)
			{
				value = owner.FrontFlameOffset;
			}
		}

		private class VRage_Game_MyJetpackThrustDefinition_003C_003EActor : IActivator, IActivator<MyJetpackThrustDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyJetpackThrustDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyJetpackThrustDefinition CreateInstance()
			{
				return new MyJetpackThrustDefinition();
			}

			MyJetpackThrustDefinition IActivator<MyJetpackThrustDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string ThrustBone;

		[ProtoMember(2)]
		public float SideFlameOffset = 0.12f;

		[ProtoMember(3)]
		public float FrontFlameOffset = 0.04f;
	}
}
