using Sandbox.Definitions;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	[Serializable]
	public class MyStoreItem
	{
		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003Em_amount_003C_003EAccessor : IMemberAccessor<MyStoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in int value)
			{
				owner.m_amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out int value)
			{
				value = owner.m_amount;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003Em_removedAmount_003C_003EAccessor : IMemberAccessor<MyStoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in int value)
			{
				owner.m_removedAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out int value)
			{
				value = owner.m_removedAmount;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003Em_updateCount_003C_003EAccessor : IMemberAccessor<MyStoreItem, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in byte value)
			{
				owner.m_updateCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out byte value)
			{
				value = owner.m_updateCount;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EId_003C_003EAccessor : IMemberAccessor<MyStoreItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EItem_003C_003EAccessor : IMemberAccessor<MyStoreItem, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in SerializableDefinitionId? value)
			{
				owner.Item = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out SerializableDefinitionId? value)
			{
				value = owner.Item;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EItemType_003C_003EAccessor : IMemberAccessor<MyStoreItem, ItemTypes>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in ItemTypes value)
			{
				owner.ItemType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out ItemTypes value)
			{
				value = owner.ItemType;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyStoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in int value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out int value)
			{
				value = owner.Amount;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EPricePerUnit_003C_003EAccessor : IMemberAccessor<MyStoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in int value)
			{
				owner.PricePerUnit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out int value)
			{
				value = owner.PricePerUnit;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EStoreItemType_003C_003EAccessor : IMemberAccessor<MyStoreItem, StoreItemTypes>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in StoreItemTypes value)
			{
				owner.StoreItemType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out StoreItemTypes value)
			{
				value = owner.StoreItemType;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EIsActive_003C_003EAccessor : IMemberAccessor<MyStoreItem, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in bool value)
			{
				owner.IsActive = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out bool value)
			{
				value = owner.IsActive;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EPrefabName_003C_003EAccessor : IMemberAccessor<MyStoreItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in string value)
			{
				owner.PrefabName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out string value)
			{
				value = owner.PrefabName;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EPrefabTotalPcu_003C_003EAccessor : IMemberAccessor<MyStoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in int value)
			{
				owner.PrefabTotalPcu = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out int value)
			{
				value = owner.PrefabTotalPcu;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EPricePerUnitDiscount_003C_003EAccessor : IMemberAccessor<MyStoreItem, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in float value)
			{
				owner.PricePerUnitDiscount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out float value)
			{
				value = owner.PricePerUnitDiscount;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EOnTransaction_003C_003EAccessor : IMemberAccessor<MyStoreItem, Action<int, int, long, long, long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in Action<int, int, long, long, long> value)
			{
				owner.OnTransaction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out Action<int, int, long, long, long> value)
			{
				value = owner.OnTransaction;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_MyStoreItem_003C_003EOnCancel_003C_003EAccessor : IMemberAccessor<MyStoreItem, Action>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStoreItem owner, in Action value)
			{
				owner.OnCancel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStoreItem owner, out Action value)
			{
				value = owner.OnCancel;
			}
		}

		private int m_amount;

		private int m_removedAmount;

		private byte m_updateCount;

		public long Id
		{
			get;
			set;
		}

		[Nullable]
		public SerializableDefinitionId? Item
		{
			get;
			set;
		}

		public ItemTypes ItemType
		{
			get;
			set;
		}

		public int Amount
		{
			get
			{
				return m_amount;
			}
			set
			{
				if (m_amount > value)
				{
					m_removedAmount += m_amount - value;
				}
				m_amount = value;
				IsActive = (m_amount > 0);
			}
		}

		public int RemovedAmount => m_removedAmount;

		public byte UpdateCount => m_updateCount;

		public int PricePerUnit
		{
			get;
			set;
		}

		public StoreItemTypes StoreItemType
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		} = true;


		[Nullable]
		public string PrefabName
		{
			get;
			set;
		}

		public int PrefabTotalPcu
		{
			get;
			set;
		}

		public float PricePerUnitDiscount
		{
			get;
			set;
		}

		[XmlIgnore]
		public Action<int, int, long, long, long> OnTransaction
		{
			get;
			private set;
		}

		[XmlIgnore]
		public Action OnCancel
		{
			get;
			private set;
		}

		public MyStoreItem()
		{
		}

		public MyStoreItem(long id, MyDefinitionId itemId, int amount, int pricePerUnit, StoreItemTypes storeItemType, byte updateCountStart = 0)
		{
			Id = id;
			Item = itemId;
			Amount = amount;
			PricePerUnit = pricePerUnit;
			StoreItemType = storeItemType;
			ItemType = ItemTypes.PhysicalItem;
			m_updateCount = updateCountStart;
		}

		public MyStoreItem(long id, int amount, int pricePerUnit, StoreItemTypes storeItemType, ItemTypes itemType)
		{
			Id = id;
			Amount = amount;
			PricePerUnit = pricePerUnit;
			StoreItemType = storeItemType;
			ItemType = itemType;
		}

		public MyStoreItem(long id, string prefabName, int amount, int pricePerUnit, int totalPcu, StoreItemTypes storeItemType)
		{
			Id = id;
			Amount = amount;
			PricePerUnit = pricePerUnit;
			StoreItemType = storeItemType;
			ItemType = ItemTypes.Grid;
			PrefabName = prefabName;
			PrefabTotalPcu = totalPcu;
		}

		public MyStoreItem(MyObjectBuilder_StoreItem builder)
		{
			Id = builder.Id;
			Item = builder.Item;
			Amount = builder.Amount;
			PricePerUnit = builder.PricePerUnit;
			StoreItemType = builder.StoreItemType;
			m_removedAmount = builder.RemovedAmount;
			m_updateCount = builder.UpdateCount;
			ItemType = builder.ItemType;
			PrefabName = builder.PrefabName;
			PrefabTotalPcu = builder.PrefabTotalPcu;
			PricePerUnitDiscount = builder.PricePerUnitDiscount;
		}

		public MyObjectBuilder_StoreItem GetObjectBuilder()
		{
			return new MyObjectBuilder_StoreItem
			{
				Id = Id,
				Item = Item,
				Amount = Amount,
				PricePerUnit = PricePerUnit,
				StoreItemType = StoreItemType,
				RemovedAmount = m_removedAmount,
				UpdateCount = m_updateCount,
				ItemType = ItemType,
				PrefabName = PrefabName,
				PrefabTotalPcu = PrefabTotalPcu,
				PricePerUnitDiscount = PricePerUnitDiscount
			};
		}

		private void ResetRemovedAmount()
		{
			m_removedAmount = 0;
		}

		private void IncrementUpdateCount()
		{
			m_updateCount++;
		}

		private void ResetUpdateCount()
		{
			m_updateCount = 0;
		}

		internal void UpdateOfferItem(MyFactionTypeDefinition definition, int minimalPricePerUnit, int minimumOfferAmount, int maximumOfferAmount)
		{
			if (IsActive)
			{
				if (RemovedAmount == 0 && UpdateCount >= definition.OfferMaxUpdateCount)
				{
					Amount = 0;
					ResetRemovedAmount();
					return;
				}
				float num = 1f;
				float num2 = Amount + RemovedAmount;
				float num3 = (float)RemovedAmount / num2;
				if (num3 > definition.OfferPriceUpDownPoint)
				{
					float amount = (num3 - definition.OfferPriceUpDownPoint) / (1f - definition.OfferPriceUpDownPoint);
					num = MathHelper.Lerp(definition.OfferPriceUpMultiplierMin, definition.OfferPriceUpMultiplierMax, amount);
				}
				else
				{
					float amount2 = (definition.OfferPriceUpDownPoint - num3) / definition.OfferPriceUpDownPoint;
					num = MathHelper.Lerp(definition.OfferPriceDownMultiplierMin, definition.OfferPriceDownMultiplierMax, amount2);
				}
				PricePerUnit = (int)Math.Max((float)PricePerUnit * num, (float)minimalPricePerUnit * definition.OfferPriceBellowMinimumMultiplier);
				PricePerUnitDiscount = ((minimalPricePerUnit > PricePerUnit) ? (1f - (float)PricePerUnit / (float)minimalPricePerUnit) : 0f);
				ResetRemovedAmount();
				IncrementUpdateCount();
			}
			else
			{
				PricePerUnit = (int)((float)PricePerUnit * definition.OfferPriceUpMultiplierMax);
				Amount = MyRandom.Instance.Next(minimumOfferAmount, maximumOfferAmount);
				ResetRemovedAmount();
				ResetUpdateCount();
			}
		}

		internal void UpdateOrderItem(MyFactionTypeDefinition definition, int minimalPricePerUnit, int minimumOrderAmount, int maximumOrderAmount, long availableBalance)
		{
			if (!IsActive)
			{
				return;
			}
			if (RemovedAmount == 0 && UpdateCount >= definition.OrderMaxUpdateCount)
			{
				Amount = 0;
				ResetRemovedAmount();
				return;
			}
			float num = 1f;
			float num2 = Amount + RemovedAmount;
			float num3 = (float)RemovedAmount / num2;
			if (num3 > definition.OrderPriceUpDownPoint)
			{
				float amount = (num3 - definition.OrderPriceUpDownPoint) / (1f - definition.OrderPriceUpDownPoint);
				num = MathHelper.Lerp(definition.OrderPriceDownMultiplierMax, definition.OrderPriceDownMultiplierMin, amount);
			}
			else
			{
				float amount2 = (definition.OrderPriceUpDownPoint - num3) / definition.OrderPriceUpDownPoint;
				num = MathHelper.Lerp(definition.OrderPriceUpMultiplierMin, definition.OrderPriceUpMultiplierMax, amount2);
			}
			PricePerUnit = (int)Math.Min((float)PricePerUnit * num, (float)minimalPricePerUnit * definition.OrderPriceOverMinimumMultiplier);
			ResetRemovedAmount();
			IncrementUpdateCount();
		}

		internal void SetActions(Action<int, int, long, long, long> onTransaction, Action onCancel)
		{
			OnTransaction = (Action<int, int, long, long, long>)Delegate.Combine(OnTransaction, onTransaction);
			OnCancel = (Action)Delegate.Combine(OnCancel, onCancel);
		}
	}
}
