using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
	[Serializable]
	public class MyStoreBuyItemResult
	{
		protected class Sandbox_Game_Entities_Blocks_MyStoreBuyItemResult_003C_003EItemId_003C_003EAccessor : IMemberAccessor<MyStoreBuyItemResult, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreBuyItemResult owner, in long value)
			{
				owner.ItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreBuyItemResult owner, out long value)
			{
				value = owner.ItemId;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreBuyItemResult_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyStoreBuyItemResult, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreBuyItemResult owner, in int value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreBuyItemResult owner, out int value)
			{
				value = owner.Amount;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreBuyItemResult_003C_003EResult_003C_003EAccessor : IMemberAccessor<MyStoreBuyItemResult, MyStoreBuyItemResults>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreBuyItemResult owner, in MyStoreBuyItemResults value)
			{
				owner.Result = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreBuyItemResult owner, out MyStoreBuyItemResults value)
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

		public MyStoreBuyItemResults Result
		{
			get;
			set;
		}
	}
}
