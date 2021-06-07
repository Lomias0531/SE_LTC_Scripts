using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public struct MovementData
	{
		protected class VRage_Game_ObjectBuilders_Components_MovementData_003C_003EMoveVector_003C_003EAccessor : IMemberAccessor<MovementData, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MovementData owner, in SerializableVector3 value)
			{
				owner.MoveVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MovementData owner, out SerializableVector3 value)
			{
				value = owner.MoveVector;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MovementData_003C_003ERotateVector_003C_003EAccessor : IMemberAccessor<MovementData, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MovementData owner, in SerializableVector3 value)
			{
				owner.RotateVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MovementData owner, out SerializableVector3 value)
			{
				value = owner.RotateVector;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MovementData_003C_003EMovementFlags_003C_003EAccessor : IMemberAccessor<MovementData, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MovementData owner, in byte value)
			{
				owner.MovementFlags = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MovementData owner, out byte value)
			{
				value = owner.MovementFlags;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MovementData_003C_003EActor : IActivator, IActivator<MovementData>
		{
			private sealed override object CreateInstance()
			{
				return default(MovementData);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MovementData CreateInstance()
			{
				return (MovementData)(object)default(MovementData);
			}

			MovementData IActivator<MovementData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		public SerializableVector3 MoveVector;

		[ProtoMember(13)]
		public SerializableVector3 RotateVector;

		[ProtoMember(16)]
		public byte MovementFlags;
	}
}
