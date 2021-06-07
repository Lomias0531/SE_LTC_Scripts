using System;

namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// Snapshot of inventory item at the moment of query.
	/// Not updated afterwards!
	/// </summary>
	public struct MyInventoryItem : IComparable<MyInventoryItem>, IEquatable<MyInventoryItem>
	{
		/// <summary>
		/// Id of item, unique within a single inventory.
		/// </summary>
		public readonly uint ItemId;

		/// <summary>
		/// Amount of stacked items.
		/// Kg or count, based on item type.
		/// </summary>
		public readonly MyFixedPoint Amount;

		/// <summary>
		/// Type of inventory item.
		/// </summary>
		public readonly MyItemType Type;

		public MyInventoryItem(MyItemType type, uint itemId, MyFixedPoint amount)
		{
			Type = type;
			ItemId = itemId;
			Amount = amount;
		}

		public static bool operator ==(MyInventoryItem a, MyInventoryItem b)
		{
			if (a.ItemId == b.ItemId && a.Amount == b.Amount)
			{
				return a.Type == b.Type;
			}
			return false;
		}

		public static bool operator !=(MyInventoryItem a, MyInventoryItem b)
		{
			return !(a == b);
		}

		public bool Equals(MyInventoryItem other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MyInventoryItem))
			{
				return false;
			}
			return Equals((MyInventoryItem)obj);
		}

		public override int GetHashCode()
		{
			int hashCode = ItemId.GetHashCode();
			MyFixedPoint amount = Amount;
			int hashCode2 = amount.GetHashCode();
			MyItemType type = Type;
			return MyTuple.CombineHashCodes(hashCode, hashCode2, type.GetHashCode());
		}

		public int CompareTo(MyInventoryItem other)
		{
			int num = ItemId.CompareTo(other.ItemId);
			if (num != 0)
			{
				return num;
			}
			int num2 = ((double)Amount).CompareTo((double)other.Amount);
			if (num2 != 0)
			{
				return num2;
			}
			return Type.CompareTo(other.Type);
		}

		public override string ToString()
		{
			MyFixedPoint amount = Amount;
			return $"{amount.ToString()}x {((MyDefinitionId)Type).ToString()}";
		}
	}
}
