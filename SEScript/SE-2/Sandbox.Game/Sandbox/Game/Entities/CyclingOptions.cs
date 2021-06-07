using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Game.Entities
{
	[Serializable]
	public struct CyclingOptions
	{
		protected class Sandbox_Game_Entities_CyclingOptions_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<CyclingOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CyclingOptions owner, in bool value)
			{
				owner.Enabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CyclingOptions owner, out bool value)
			{
				value = owner.Enabled;
			}
		}

		protected class Sandbox_Game_Entities_CyclingOptions_003C_003EOnlySmallGrids_003C_003EAccessor : IMemberAccessor<CyclingOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CyclingOptions owner, in bool value)
			{
				owner.OnlySmallGrids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CyclingOptions owner, out bool value)
			{
				value = owner.OnlySmallGrids;
			}
		}

		protected class Sandbox_Game_Entities_CyclingOptions_003C_003EOnlyLargeGrids_003C_003EAccessor : IMemberAccessor<CyclingOptions, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CyclingOptions owner, in bool value)
			{
				owner.OnlyLargeGrids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CyclingOptions owner, out bool value)
			{
				value = owner.OnlyLargeGrids;
			}
		}

		public bool Enabled;

		public bool OnlySmallGrids;

		public bool OnlyLargeGrids;
	}
}
