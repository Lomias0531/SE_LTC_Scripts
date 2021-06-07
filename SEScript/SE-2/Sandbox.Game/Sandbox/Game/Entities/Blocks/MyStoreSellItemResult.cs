using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
	[Serializable]
	public class MyStoreSellItemResult
	{
		protected class Sandbox_Game_Entities_Blocks_MyStoreSellItemResult_003C_003EItemId_003C_003EAccessor : IMemberAccessor<MyStoreSellItemResult, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreSellItemResult owner, in long value)
			{
				owner.ItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreSellItemResult owner, out long value)
			{
				value = owner.ItemId;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreSellItemResult_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyStoreSellItemResult, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreSellItemResult owner, in int value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreSellItemResult owner, out int value)
			{
				value = owner.Amount;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreSellItemResult_003C_003EResult_003C_003EAccessor : IMemberAccessor<MyStoreSellItemResult, MyStoreSellItemResults>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreSellItemResult owner, in MyStoreSellItemResults value)
			{
				owner.Result = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreSellItemResult owner, out MyStoreSellItemResults value)
			{
				value = owner.Result;
			}
		}

		public long ItemId
		{
			get;
			set;
		}

		public int Amount
		{
			get;
			set;
		}

		public MyStoreSellItemResults Result
		{
			get;
			set;
		}
	}
}
