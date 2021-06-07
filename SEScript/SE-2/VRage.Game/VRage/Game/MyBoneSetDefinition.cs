using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyBoneSetDefinition
	{
		protected class VRage_Game_MyBoneSetDefinition_003C_003EName_003C_003EAccessor : IMemberAccessor<MyBoneSetDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBoneSetDefinition owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBoneSetDefinition owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyBoneSetDefinition_003C_003EBones_003C_003EAccessor : IMemberAccessor<MyBoneSetDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBoneSetDefinition owner, in string value)
			{
				owner.Bones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBoneSetDefinition owner, out string value)
			{
				value = owner.Bones;
			}
		}

		private class VRage_Game_MyBoneSetDefinition_003C_003EActor : IActivator, IActivator<MyBoneSetDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBoneSetDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBoneSetDefinition CreateInstance()
			{
				return new MyBoneSetDefinition();
			}

			MyBoneSetDefinition IActivator<MyBoneSetDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(9)]
		public string Name;

		[ProtoMember(10)]
		public string Bones;
	}
}
