using System;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.Network;

namespace Sandbox.ModAPI.Ingame
{
	[Serializable]
	public struct MyInventoryItemFilter
	{
		protected class Sandbox_ModAPI_Ingame_MyInventoryItemFilter_003C_003EAllSubTypes_003C_003EAccessor : IMemberAccessor<MyInventoryItemFilter, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInventoryItemFilter owner, in bool value)
			{
				owner.AllSubTypes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInventoryItemFilter owner, out bool value)
			{
				value = owner.AllSubTypes;
			}
		}

		protected class Sandbox_ModAPI_Ingame_MyInventoryItemFilter_003C_003EItemId_003C_003EAccessor : IMemberAccessor<MyInventoryItemFilter, MyDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInventoryItemFilter owner, in MyDefinitionId value)
			{
				owner.ItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInventoryItemFilter owner, out MyDefinitionId value)
			{
				value = owner.ItemId;
			}
		}

		/// <summary>
		/// Determines whether all subtypes of the given item ID should pass this filter check.
		/// </summary>
		public readonly bool AllSubTypes;

		/// <summary>
		/// Specifies an item to filter. Set <see cref="F:Sandbox.ModAPI.Ingame.MyInventoryItemFilter.AllSubTypes" /> to true to only check the main type part of this ID.
		/// </summary>
		public readonly MyDefinitionId ItemId;

		public MyItemType ItemType => ItemId;

		public static implicit operator MyInventoryItemFilter(MyItemType itemType)
		{
			return new MyInventoryItemFilter(itemType);
		}

		public static implicit operator MyInventoryItemFilter(MyDefinitionId definitionId)
		{
			return new MyInventoryItemFilter(definitionId);
		}

		public MyInventoryItemFilter(string itemId, bool allSubTypes = false)
		{
			this = default(MyInventoryItemFilter);
			ItemId = MyDefinitionId.Parse(itemId);
			AllSubTypes = allSubTypes;
		}

		public MyInventoryItemFilter(MyDefinitionId itemId, bool allSubTypes = false)
		{
			this = default(MyInventoryItemFilter);
			ItemId = itemId;
			AllSubTypes = allSubTypes;
		}
	}
}
