using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[Serializable]
	public struct MyAirVentBlockRoomInfo : IEquatable<MyAirVentBlockRoomInfo>
	{
		protected class SpaceEngineers_Game_Entities_Blocks_MyAirVentBlockRoomInfo_003C_003EIsRoomAirtight_003C_003EAccessor : IMemberAccessor<MyAirVentBlockRoomInfo, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAirVentBlockRoomInfo owner, in bool value)
			{
				owner.IsRoomAirtight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAirVentBlockRoomInfo owner, out bool value)
			{
				value = owner.IsRoomAirtight;
			}
		}

		protected class SpaceEngineers_Game_Entities_Blocks_MyAirVentBlockRoomInfo_003C_003EOxygenLevel_003C_003EAccessor : IMemberAccessor<MyAirVentBlockRoomInfo, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAirVentBlockRoomInfo owner, in float value)
			{
				owner.OxygenLevel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAirVentBlockRoomInfo owner, out float value)
			{
				value = owner.OxygenLevel;
			}
		}

		protected class SpaceEngineers_Game_Entities_Blocks_MyAirVentBlockRoomInfo_003C_003ERoomEnvironmentOxygen_003C_003EAccessor : IMemberAccessor<MyAirVentBlockRoomInfo, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAirVentBlockRoomInfo owner, in float value)
			{
				owner.RoomEnvironmentOxygen = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAirVentBlockRoomInfo owner, out float value)
			{
				value = owner.RoomEnvironmentOxygen;
			}
		}

		public bool IsRoomAirtight;

		public float OxygenLevel;

		public float RoomEnvironmentOxygen;

		public MyAirVentBlockRoomInfo(bool isRoomAirtight, float oxygenLevel, float roomEnvironmentOxygen)
		{
			IsRoomAirtight = isRoomAirtight;
			OxygenLevel = oxygenLevel;
			RoomEnvironmentOxygen = roomEnvironmentOxygen;
		}

		public bool Equals(MyAirVentBlockRoomInfo other)
		{
			if (IsRoomAirtight == other.IsRoomAirtight && MathHelper.IsEqual(OxygenLevel, other.OxygenLevel))
			{
				return MathHelper.IsEqual(RoomEnvironmentOxygen, other.RoomEnvironmentOxygen);
			}
			return false;
		}
	}
}
