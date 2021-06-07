using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct BoneInfo
	{
		protected class VRage_Game_BoneInfo_003C_003EBonePosition_003C_003EAccessor : IMemberAccessor<BoneInfo, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoneInfo owner, in SerializableVector3I value)
			{
				owner.BonePosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoneInfo owner, out SerializableVector3I value)
			{
				value = owner.BonePosition;
			}
		}

		protected class VRage_Game_BoneInfo_003C_003EBoneOffset_003C_003EAccessor : IMemberAccessor<BoneInfo, SerializableVector3UByte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoneInfo owner, in SerializableVector3UByte value)
			{
				owner.BoneOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoneInfo owner, out SerializableVector3UByte value)
			{
				value = owner.BoneOffset;
			}
		}

		private class VRage_Game_BoneInfo_003C_003EActor : IActivator, IActivator<BoneInfo>
		{
			private sealed override object CreateInstance()
			{
				return default(BoneInfo);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override BoneInfo CreateInstance()
			{
				return (BoneInfo)(object)default(BoneInfo);
			}

			BoneInfo IActivator<BoneInfo>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableVector3I BonePosition;

		[ProtoMember(2)]
		public SerializableVector3UByte BoneOffset;
	}
}
