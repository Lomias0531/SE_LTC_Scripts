using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game
{
	[MyComponentBuilder(typeof(MyObjectBuilder_Inventory), true)]
	[StaticEventOwner]
	public class MyInventory : MyInventoryBase, VRage.Game.ModAPI.IMyInventory, VRage.Game.ModAPI.Ingame.IMyInventory
	{
		private struct ConnectionKey : IEquatable<ConnectionKey>
		{
			public long Id;

			public MyDefinitionId? ItemType;

			public ConnectionKey(long id, MyDefinitionId? itemType)
			{
				Id = id;
				ItemType = itemType;
			}

			public bool Equals(ConnectionKey other)
			{
				if (Id != other.Id)
				{
					return false;
				}
				if (ItemType.HasValue != other.ItemType.HasValue)
				{
					return false;
				}
				if (ItemType.HasValue && ItemType.Value != other.ItemType.Value)
				{
					return false;
				}
				return true;
			}

			public override bool Equals(object obj)
			{
				if (obj is ConnectionKey)
				{
					return Equals((ConnectionKey)obj);
				}
				return ((ValueType)this).Equals(obj);
			}

			public override int GetHashCode()
			{
				return MyTuple.CombineHashCodes(Id.GetHashCode(), ItemType.GetHashCode());
			}
		}

		private struct ConnectionData
		{
			public int Frame;

			public bool HasConnection;
		}

		protected sealed class PickupItem_Implementation_003C_003ESystem_Int64_0023VRage_MyFixedPoint : ICallSite<MyInventory, long, MyFixedPoint, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyInventory @this, in long entityId, in MyFixedPoint amount, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PickupItem_Implementation(entityId, amount);
			}
		}

		protected sealed class RemoveItemsAt_Request_003C_003ESystem_Int32_0023System_Nullable_00601_003CVRage_MyFixedPoint_003E_0023System_Boolean_0023System_Boolean_0023System_Nullable_00601_003CVRageMath_MatrixD_003E : ICallSite<MyInventory, int, MyFixedPoint?, bool, bool, MatrixD?, DBNull>
		{
			public sealed override void Invoke(in MyInventory @this, in int itemIndex, in MyFixedPoint? amount, in bool sendEvent, in bool spawn, in MatrixD? spawnPos, in DBNull arg6)
			{
				@this.RemoveItemsAt_Request(itemIndex, amount, sendEvent, spawn, spawnPos);
			}
		}

		protected sealed class AddEntity_Implementation_003C_003ESystem_Int64_0023System_Boolean : ICallSite<MyInventory, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyInventory @this, in long entityId, in bool blockManipulatedEntity, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AddEntity_Implementation(entityId, blockManipulatedEntity);
			}
		}

		protected sealed class ModifyDatapad_003C_003ESystem_Int64_0023System_Int32_0023System_UInt32_0023System_String_0023System_String : ICallSite<IMyEventOwner, long, int, uint, string, string, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long inventoryOwner, in int inventoryIndex, in uint itemIndex, in string name, in string data, in DBNull arg6)
			{
				ModifyDatapad(inventoryOwner, inventoryIndex, itemIndex, name, data);
			}
		}

		protected sealed class ModifyDatapad_Broadcast_003C_003ESystem_UInt32_0023System_String_0023System_String : ICallSite<MyInventory, uint, string, string, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyInventory @this, in uint itemIndex, in string name, in string data, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ModifyDatapad_Broadcast(itemIndex, name, data);
			}
		}

		protected sealed class InventoryTransferItemPlanner_Implementation_003C_003ESystem_Int64_0023System_Int32_0023VRage_ObjectBuilders_SerializableDefinitionId_0023VRage_Game_MyItemFlags_0023System_Nullable_00601_003CVRage_MyFixedPoint_003E_0023System_Boolean : ICallSite<MyInventory, long, int, SerializableDefinitionId, MyItemFlags, MyFixedPoint?, bool>
		{
			public sealed override void Invoke(in MyInventory @this, in long destinationOwnerId, in int destInventoryIndex, in SerializableDefinitionId contentId, in MyItemFlags flags, in MyFixedPoint? amount, in bool spawn)
			{
				@this.InventoryTransferItemPlanner_Implementation(destinationOwnerId, destInventoryIndex, contentId, flags, amount, spawn);
			}
		}

		protected sealed class InventoryTransferItem_Implementation_003C_003EVRage_MyFixedPoint_0023System_UInt32_0023System_Int64_0023System_Byte_0023System_Int32 : ICallSite<MyInventory, MyFixedPoint, uint, long, byte, int, DBNull>
		{
			public sealed override void Invoke(in MyInventory @this, in MyFixedPoint amount, in uint itemId, in long destinationOwnerId, in byte destInventoryIndex, in int destinationIndex, in DBNull arg6)
			{
				@this.InventoryTransferItem_Implementation(amount, itemId, destinationOwnerId, destInventoryIndex, destinationIndex);
			}
		}

		protected sealed class DebugAddItems_Implementation_003C_003EVRage_MyFixedPoint_0023VRage_ObjectBuilders_MyObjectBuilder_Base : ICallSite<MyInventory, MyFixedPoint, MyObjectBuilder_Base, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyInventory @this, in MyFixedPoint amount, in MyObjectBuilder_Base objectBuilder, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DebugAddItems_Implementation(amount, objectBuilder);
			}
		}

		protected sealed class DropItem_Implementation_003C_003EVRage_MyFixedPoint_0023System_UInt32 : ICallSite<MyInventory, MyFixedPoint, uint, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyInventory @this, in MyFixedPoint amount, in uint itemIndex, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DropItem_Implementation(amount, itemIndex);
			}
		}

		protected sealed class InventoryConsumeItem_Implementation_003C_003EVRage_MyFixedPoint_0023VRage_ObjectBuilders_SerializableDefinitionId_0023System_Int64 : ICallSite<MyInventory, MyFixedPoint, SerializableDefinitionId, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyInventory @this, in MyFixedPoint amount, in SerializableDefinitionId itemId, in long consumerEntityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.InventoryConsumeItem_Implementation(amount, itemId, consumerEntityId);
			}
		}

		protected class m_currentVolume_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType currentVolume;
				ISyncType result = currentVolume = new Sync<MyFixedPoint, SyncDirection.FromServer>(P_1, P_2);
				((MyInventory)P_0).m_currentVolume = (Sync<MyFixedPoint, SyncDirection.FromServer>)currentVolume;
				return result;
			}
		}

		protected class m_currentMass_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType currentMass;
				ISyncType result = currentMass = new Sync<MyFixedPoint, SyncDirection.FromServer>(P_1, P_2);
				((MyInventory)P_0).m_currentMass = (Sync<MyFixedPoint, SyncDirection.FromServer>)currentMass;
				return result;
			}
		}

		private class Sandbox_Game_MyInventory_003C_003EActor : IActivator, IActivator<MyInventory>
		{
			private sealed override object CreateInstance()
			{
				return new MyInventory();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyInventory CreateInstance()
			{
				return new MyInventory();
			}

			MyInventory IActivator<MyInventory>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static MyStringHash INVENTORY_CHANGED = MyStringHash.GetOrCompute("InventoryChanged");

		private static Dictionary<MyDefinitionId, int> m_tmpItemsToAdd = new Dictionary<MyDefinitionId, int>();

		private List<MyPhysicalInventoryItem> m_items = new List<MyPhysicalInventoryItem>();

		private MyFixedPoint m_maxMass = MyFixedPoint.MaxValue;

		private MyFixedPoint m_maxVolume = MyFixedPoint.MaxValue;

		private int m_maxItemCount = int.MaxValue;

		private MySoundPair dropSound = new MySoundPair("PlayDropItem");

		private readonly Sync<MyFixedPoint, SyncDirection.FromServer> m_currentVolume;

		private readonly Sync<MyFixedPoint, SyncDirection.FromServer> m_currentMass;

		private MyInventoryFlags m_flags;

		public object UserData;

		private uint m_nextItemID;

		private HashSet<uint> m_usedIds = new HashSet<uint>();

		public readonly SyncType SyncType;

		private bool m_multiplierEnabled = true;

		public MyInventoryConstraint Constraint;

		private MyObjectBuilder_InventoryDefinition myObjectBuilder_InventoryDefinition;

		private MyHudNotification m_inventoryNotEmptyNotification;

		private MyObjectBuilder_Inventory m_objectBuilder;

		private LRUCache<ConnectionKey, ConnectionData> m_connectionCache;

		public override float? ForcedPriority
		{
			get;
			set;
		}

		public bool IsConstrained
		{
			get
			{
				if (!MyPerGameSettings.ConstrainInventory())
				{
					return !IsCharacterOwner;
				}
				return true;
			}
		}

		public override MyFixedPoint MaxMass
		{
			get
			{
				if (IsConstrained)
				{
					if (m_multiplierEnabled)
					{
						if (IsCharacterOwner)
						{
							return MyFixedPoint.MultiplySafe(m_maxMass, MySession.Static.CharactersInventoryMultiplier);
						}
						return MyFixedPoint.MultiplySafe(m_maxMass, MySession.Static.BlocksInventorySizeMultiplier);
					}
					return m_maxMass;
				}
				return MyFixedPoint.MaxValue;
			}
		}

		public override MyFixedPoint MaxVolume
		{
			get
			{
				if (IsConstrained)
				{
					if (m_multiplierEnabled)
					{
						if (IsCharacterOwner)
						{
							return MyFixedPoint.MultiplySafe(m_maxVolume, MySession.Static.CharactersInventoryMultiplier);
						}
						return MyFixedPoint.MultiplySafe(m_maxVolume, MySession.Static.BlocksInventorySizeMultiplier);
					}
					return m_maxVolume;
				}
				return MyFixedPoint.MaxValue;
			}
		}

		public override int MaxItemCount
		{
			get
			{
				if (!IsConstrained)
				{
					return int.MaxValue;
				}
				if (!m_multiplierEnabled)
				{
					return m_maxItemCount;
				}
				double num = IsCharacterOwner ? MySession.Static.CharactersInventoryMultiplier : MySession.Static.BlocksInventorySizeMultiplier;
				long num2 = Math.Max(1L, (long)((double)m_maxItemCount * num));
				if (num2 > int.MaxValue)
				{
					num2 = 2147483647L;
				}
				return (int)num2;
			}
		}

		public override MyFixedPoint CurrentVolume => m_currentVolume;

		public float VolumeFillFactor
		{
			get
			{
				if (!IsConstrained)
				{
					return 0f;
				}
				return (float)CurrentVolume / (float)MaxVolume;
			}
		}

		public override MyFixedPoint CurrentMass => m_currentMass;

		public MyEntity Owner
		{
			get
			{
				if (base.Entity == null)
				{
					return null;
				}
				return base.Entity as MyEntity;
			}
		}

		public bool IsCharacterOwner
		{
			get
			{
				if (!(Owner is MyCharacter))
				{
					return Owner is MyInventoryBagEntity;
				}
				return true;
			}
		}

		public byte InventoryIdx
		{
			get
			{
				if (Owner != null)
				{
					for (byte b = 0; b < Owner.InventoryCount; b = (byte)(b + 1))
					{
						if (Owner.GetInventory(b).Equals(this))
						{
							return b;
						}
					}
				}
				return 0;
			}
		}

		public bool IsFull
		{
			get
			{
				if (!(m_currentVolume >= MaxVolume))
				{
					return m_currentMass >= MaxMass;
				}
				return true;
			}
		}

		public float CargoPercentage
		{
			get
			{
				if (!IsConstrained)
				{
					return 0f;
				}
				float num = (float)m_currentVolume.Value;
				float num2 = (float)MaxVolume;
				return MyMath.Clamp(num / num2, 0f, 1f);
			}
		}

		VRage.Game.ModAPI.Ingame.IMyEntity VRage.Game.ModAPI.Ingame.IMyInventory.Owner => Owner;

		public int ItemCount => m_items.Count;

		VRage.ModAPI.IMyEntity VRage.Game.ModAPI.IMyInventory.Owner => Owner;

		public static event Action<VRage.Game.ModAPI.Ingame.IMyInventory, VRage.Game.ModAPI.Ingame.IMyInventory, VRage.Game.ModAPI.Ingame.IMyInventoryItem, MyFixedPoint> OnTransferByUser;

		public MyInventory()
			: this(MyFixedPoint.MaxValue, MyFixedPoint.MaxValue, Vector3.Zero, (MyInventoryFlags)0)
		{
		}

		public MyInventory(float maxVolume, Vector3 size, MyInventoryFlags flags)
			: this((MyFixedPoint)maxVolume, MyFixedPoint.MaxValue, size, flags)
		{
		}

		public MyInventory(float maxVolume, float maxMass, Vector3 size, MyInventoryFlags flags)
			: this((MyFixedPoint)maxVolume, (MyFixedPoint)maxMass, size, flags)
		{
		}

		public MyInventory(MyFixedPoint maxVolume, MyFixedPoint maxMass, Vector3 size, MyInventoryFlags flags)
			: base("Inventory")
		{
			m_maxVolume = maxVolume;
			m_maxMass = maxMass;
			m_flags = flags;
			SyncType = SyncHelpers.Compose(this);
			m_currentVolume.ValueChanged += delegate
			{
				PropertiesChanged();
			};
			m_currentVolume.AlwaysReject();
			m_currentMass.ValueChanged += delegate
			{
				PropertiesChanged();
			};
			m_currentMass.AlwaysReject();
			m_inventoryNotEmptyNotification = new MyHudNotification(MyCommonTexts.NotificationInventoryNotEmpty, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2);
			Clear();
		}

		public MyInventory(MyObjectBuilder_InventoryDefinition definition, MyInventoryFlags flags)
			: this(definition.InventoryVolume, definition.InventoryMass, new Vector3(definition.InventorySizeX, definition.InventorySizeY, definition.InventorySizeZ), flags)
		{
			myObjectBuilder_InventoryDefinition = definition;
		}

		public void SetFlags(MyInventoryFlags flags)
		{
			m_flags = flags;
		}

		public MyInventoryFlags GetFlags()
		{
			return m_flags;
		}

		public bool CanItemsBeAdded(MyFixedPoint amount, MyDefinitionId contentId)
		{
			if (amount == 0)
			{
				return true;
			}
			if (base.Entity == null || base.Entity.MarkedForClose)
			{
				return false;
			}
			if (CanItemsBeAdded(amount, contentId, MaxVolume, MaxMass, m_currentVolume, m_currentMass))
			{
				return CheckConstraint(contentId);
			}
			return false;
		}

		public bool CanItemsBeAdded(MyFixedPoint amount, MyDefinitionId contentId, MyFixedPoint maxVolume, MyFixedPoint maxMass, MyFixedPoint currentVolume, MyFixedPoint currentMass)
		{
			MyInventoryItemAdapter @static = MyInventoryItemAdapter.Static;
			@static.Adapt(contentId);
			if ((IsConstrained && amount * @static.Volume + currentVolume > maxVolume) || amount * @static.Mass + currentMass > maxMass)
			{
				return false;
			}
			return true;
		}

		public static void GetItemVolumeAndMass(MyDefinitionId contentId, out float itemMass, out float itemVolume)
		{
			MyInventoryItemAdapter @static = MyInventoryItemAdapter.Static;
			if (@static.TryAdapt(contentId))
			{
				itemMass = @static.Mass;
				itemVolume = @static.Volume;
			}
			else
			{
				itemMass = 0f;
				itemVolume = 0f;
			}
		}

		public override MyFixedPoint ComputeAmountThatFits(MyDefinitionId contentId, float volumeRemoved = 0f, float massRemoved = 0f)
		{
			if (!IsConstrained)
			{
				return MyFixedPoint.MaxValue;
			}
			MyInventoryItemAdapter @static = MyInventoryItemAdapter.Static;
			@static.Adapt(contentId);
			MyFixedPoint a = MyFixedPoint.Max((MyFixedPoint)(((double)MaxVolume - Math.Max((double)m_currentVolume.Value - (double)volumeRemoved * (double)@static.Volume, 0.0)) * (1.0 / (double)@static.Volume)), 0);
			MyFixedPoint b = MyFixedPoint.Max((MyFixedPoint)(((double)MaxMass - Math.Max((double)m_currentMass.Value - (double)massRemoved * (double)@static.Mass, 0.0)) * (1.0 / (double)@static.Mass)), 0);
			MyFixedPoint myFixedPoint = MyFixedPoint.Min(a, b);
			if (MaxItemCount != int.MaxValue)
			{
				myFixedPoint = MyFixedPoint.Min(myFixedPoint, FindFreeSlotSpace(contentId, @static));
			}
			if (@static.HasIntegralAmounts)
			{
				myFixedPoint = MyFixedPoint.Floor((MyFixedPoint)(Math.Round((double)myFixedPoint * 1000.0) / 1000.0));
			}
			return myFixedPoint;
		}

		public MyFixedPoint ComputeAmountThatFits(MyBlueprintDefinitionBase blueprint)
		{
			if (!IsConstrained)
			{
				return MyFixedPoint.MaxValue;
			}
			MyFixedPoint a = (MaxVolume - m_currentVolume) * (1f / blueprint.OutputVolume);
			a = MyFixedPoint.Max(a, 0);
			if (blueprint.Atomic)
			{
				a = MyFixedPoint.Floor(a);
			}
			return a;
		}

		public bool CheckConstraint(MyDefinitionId contentId)
		{
			if (Constraint != null)
			{
				return Constraint.Check(contentId);
			}
			return true;
		}

		public bool ContainItems(MyFixedPoint amount, MyObjectBuilder_PhysicalObject ob)
		{
			if (ob == null)
			{
				return false;
			}
			return ContainItems(amount, ob.GetObjectId());
		}

		public MyFixedPoint FindFreeSlotSpace(MyDefinitionId contentId, IMyInventoryItemAdapter adapter)
		{
			MyFixedPoint myFixedPoint = 0;
			MyFixedPoint maxStackAmount = adapter.MaxStackAmount;
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].Content.CanStack(contentId.TypeId, contentId.SubtypeId, MyItemFlags.None))
				{
					myFixedPoint = MyFixedPoint.AddSafe(myFixedPoint, maxStackAmount - m_items[i].Amount);
				}
			}
			int num = MaxItemCount - m_items.Count;
			if (num > 0)
			{
				myFixedPoint = MyFixedPoint.AddSafe(myFixedPoint, maxStackAmount * num);
			}
			return myFixedPoint;
		}

		public override MyFixedPoint GetItemAmount(MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, bool substitute = false)
		{
			MyFixedPoint result = 0;
			foreach (MyPhysicalInventoryItem item in m_items)
			{
				MyDefinitionId myDefinitionId = item.Content.GetId();
				if (contentId != myDefinitionId && item.Content.TypeId == typeof(MyObjectBuilder_BlockItem))
				{
					myDefinitionId = item.Content.GetObjectId();
				}
				if (myDefinitionId == contentId && item.Content.Flags == flags)
				{
					result += item.Amount;
				}
			}
			return result;
		}

		public MyPhysicalInventoryItem? FindItem(MyDefinitionId contentId)
		{
			int? num = FindFirstPositionOfType(contentId);
			if (num.HasValue)
			{
				return m_items[num.Value];
			}
			return null;
		}

		public MyPhysicalInventoryItem? FindItem(Func<MyPhysicalInventoryItem, bool> predicate)
		{
			foreach (MyPhysicalInventoryItem item in m_items)
			{
				if (predicate(item))
				{
					return item;
				}
			}
			return null;
		}

		public MyPhysicalInventoryItem? FindUsableItem(MyDefinitionId contentId)
		{
			if (!MyFakes.ENABLE_DURABILITY_COMPONENT)
			{
				return FindItem(contentId);
			}
			int nextPosition = -1;
			while (TryFindNextPositionOfTtype(contentId, nextPosition, out nextPosition) && m_items.IsValidIndex(nextPosition))
			{
				if (m_items[nextPosition].Content == null || !m_items[nextPosition].Content.DurabilityHP.HasValue || m_items[nextPosition].Content.DurabilityHP.Value > 0f)
				{
					return m_items[nextPosition];
				}
			}
			return null;
		}

		private int? FindFirstStackablePosition(MyObjectBuilder_PhysicalObject toStack, MyFixedPoint wantedAmount)
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].Content.CanStack(toStack) && m_items[i].Amount <= wantedAmount)
				{
					return i;
				}
			}
			return null;
		}

		private int? FindFirstPositionOfType(MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None)
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				MyObjectBuilder_PhysicalObject content = m_items[i].Content;
				if (content.GetObjectId() == contentId && content.Flags == flags)
				{
					return i;
				}
			}
			return null;
		}

		private bool TryFindNextPositionOfTtype(MyDefinitionId contentId, int startPosition, out int nextPosition)
		{
			if (m_items.IsValidIndex(startPosition + 1))
			{
				for (int i = startPosition + 1; i < m_items.Count; i++)
				{
					if (m_items[i].Content.GetObjectId() == contentId)
					{
						nextPosition = i;
						return true;
					}
				}
			}
			nextPosition = -1;
			return false;
		}

		public bool ContainItems(MyFixedPoint? amount, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None)
		{
			MyFixedPoint itemAmount = GetItemAmount(contentId, flags);
			if (amount.HasValue)
			{
				MyFixedPoint value = itemAmount;
				MyFixedPoint? b = amount;
				return value >= b;
			}
			return itemAmount > 0;
		}

		public void TakeFloatingObject(MyFloatingObject obj)
		{
			MyFixedPoint myFixedPoint = obj.Item.Amount;
			if (IsConstrained)
			{
				myFixedPoint = MyFixedPoint.Min(ComputeAmountThatFits(obj.Item.Content.GetObjectId()), myFixedPoint);
			}
			if (!obj.MarkedForClose && myFixedPoint > 0 && Sync.IsServer)
			{
				MyFloatingObjects.RemoveFloatingObject(obj, myFixedPoint);
				AddItemsInternal(myFixedPoint, obj.Item.Content);
			}
		}

		public bool AddGrid(MyCubeGrid grid)
		{
			List<Vector3I> list = new List<Vector3I>();
			foreach (MySlimBlock block in grid.GetBlocks())
			{
				if (block.FatBlock is MyCompoundCubeBlock)
				{
					bool flag = false;
					foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
					{
						if (AddBlock(block2))
						{
							if (!flag)
							{
								list.Add(block.Position);
							}
							flag = true;
						}
					}
				}
				else if (AddBlock(block))
				{
					list.Add(block.Position);
				}
			}
			if (list.Count > 0)
			{
				grid.RazeBlocks(list, 0L, 0uL);
				return true;
			}
			return false;
		}

		public bool AddBlockAndRemoveFromGrid(MySlimBlock block)
		{
			bool flag = false;
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					if (AddBlock(block2))
					{
						flag = true;
					}
				}
			}
			else if (AddBlock(block))
			{
				flag = true;
			}
			if (flag)
			{
				block.CubeGrid.RazeBlock(block.Position, 0uL);
				return true;
			}
			return false;
		}

		public bool AddBlocks(MyCubeBlockDefinition blockDef, MyFixedPoint amount)
		{
			MyObjectBuilder_BlockItem myObjectBuilder_BlockItem = new MyObjectBuilder_BlockItem();
			myObjectBuilder_BlockItem.BlockDefId = blockDef.Id;
			if (ComputeAmountThatFits(myObjectBuilder_BlockItem.BlockDefId) >= amount)
			{
				AddItems(amount, myObjectBuilder_BlockItem);
				return true;
			}
			return false;
		}

		private bool AddBlock(MySlimBlock block)
		{
			if (!MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID && block.FatBlock != null && block.FatBlock.HasInventory)
			{
				return false;
			}
			MyObjectBuilder_BlockItem myObjectBuilder_BlockItem = new MyObjectBuilder_BlockItem();
			myObjectBuilder_BlockItem.BlockDefId = block.BlockDefinition.Id;
			if (ComputeAmountThatFits(myObjectBuilder_BlockItem.BlockDefId) >= 1)
			{
				AddItems(1, myObjectBuilder_BlockItem);
				return true;
			}
			return false;
		}

		public void PickupItem(MyFloatingObject obj, MyFixedPoint amount)
		{
			MyMultiplayer.RaiseEvent(this, (MyInventory x) => x.PickupItem_Implementation, obj.EntityId, amount);
		}

		[Event(null, 724)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void PickupItem_Implementation(long entityId, MyFixedPoint amount)
		{
			if (!MyEntities.TryGetEntityById(entityId, out MyFloatingObject entity) || entity == null || entity.MarkedForClose || entity.WasRemovedFromWorld)
			{
				return;
			}
			amount = MyFixedPoint.Min(amount, entity.Item.Amount);
			amount = MyFixedPoint.Min(amount, ComputeAmountThatFits(entity.Item.Content.GetObjectId()));
			if (!AddItems(amount, entity.Item.Content))
			{
				return;
			}
			if (amount >= entity.Item.Amount)
			{
				MyFloatingObjects.RemoveFloatingObject(entity, sync: true);
				MyCharacter myCharacter;
				if (MyVisualScriptLogicProvider.PlayerPickedUp != null && (myCharacter = (Owner as MyCharacter)) != null)
				{
					long controllingIdentityId = myCharacter.ControllerInfo.ControllingIdentityId;
					FloatingObjectPlayerEvent playerPickedUp = MyVisualScriptLogicProvider.PlayerPickedUp;
					MyObjectBuilderType typeId = entity.ItemDefinition.Id.TypeId;
					playerPickedUp(typeId.ToString(), entity.ItemDefinition.Id.SubtypeName, entity.Name, controllingIdentityId, amount.ToIntSafe());
				}
			}
			else
			{
				MyFloatingObjects.AddFloatingObjectAmount(entity, -amount);
			}
		}

		public void DebugAddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder)
		{
		}

		public override bool AddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder)
		{
			return AddItems(amount, objectBuilder, null);
		}

		private bool AddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder, uint? itemId, int index = -1)
		{
			if (amount == 0)
			{
				return false;
			}
			MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = objectBuilder as MyObjectBuilder_PhysicalObject;
			MyDefinitionId id = objectBuilder.GetId();
			if (MyFakes.ENABLE_COMPONENT_BLOCKS)
			{
				if (myObjectBuilder_PhysicalObject == null)
				{
					myObjectBuilder_PhysicalObject = new MyObjectBuilder_BlockItem();
					(myObjectBuilder_PhysicalObject as MyObjectBuilder_BlockItem).BlockDefId = id;
				}
				else
				{
					MyCubeBlockDefinition myCubeBlockDefinition = MyDefinitionManager.Static.TryGetComponentBlockDefinition(id);
					if (myCubeBlockDefinition != null)
					{
						myObjectBuilder_PhysicalObject = new MyObjectBuilder_BlockItem();
						(myObjectBuilder_PhysicalObject as MyObjectBuilder_BlockItem).BlockDefId = myCubeBlockDefinition.Id;
					}
				}
			}
			if (myObjectBuilder_PhysicalObject == null)
			{
				return false;
			}
			id = myObjectBuilder_PhysicalObject.GetObjectId();
			if (ComputeAmountThatFits(id) < amount)
			{
				return false;
			}
			if (Sync.IsServer)
			{
				if (IsConstrained)
				{
					AffectAddBySurvival(ref amount, myObjectBuilder_PhysicalObject);
				}
				if (amount == 0)
				{
					return false;
				}
				AddItemsInternal(amount, myObjectBuilder_PhysicalObject, itemId, index);
			}
			return true;
		}

		private void AffectAddBySurvival(ref MyFixedPoint amount, MyObjectBuilder_PhysicalObject objectBuilder)
		{
			MyFixedPoint myFixedPoint = ComputeAmountThatFits(objectBuilder.GetObjectId());
			if (myFixedPoint < amount)
			{
				if (Owner is MyCharacter)
				{
					MyCharacter c = Owner as MyCharacter;
					Matrix i = c.GetHeadMatrix(includeY: true);
					MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(amount - myFixedPoint, objectBuilder), i.Translation, i.Forward, i.Up, c.Physics, delegate(MyEntity entity)
					{
						entity.Physics.ApplyImpulse(i.Forward.Cross(i.Up), c.PositionComp.GetPosition());
					});
				}
				amount = myFixedPoint;
			}
		}

		private void AddItemsInternal(MyFixedPoint amount, MyObjectBuilder_PhysicalObject objectBuilder, uint? itemId = null, int index = -1)
		{
			OnBeforeContentsChanged();
			MyFixedPoint maxValue = MyFixedPoint.MaxValue;
			MyInventoryItemAdapter @static = MyInventoryItemAdapter.Static;
			@static.Adapt(objectBuilder.GetObjectId());
			maxValue = @static.MaxStackAmount;
			if (!objectBuilder.CanStack(objectBuilder))
			{
				maxValue = 1;
			}
			if (MyFakes.ENABLE_DURABILITY_COMPONENT)
			{
				FixDurabilityForInventoryItem(objectBuilder);
			}
			bool flag = false;
			if (index >= 0)
			{
				if (index >= m_items.Count && index < MaxItemCount)
				{
					amount = AddItemsToNewStack(amount, maxValue, objectBuilder, itemId);
					flag = true;
				}
				else if (index < m_items.Count)
				{
					if (m_items[index].Content.CanStack(objectBuilder))
					{
						amount = AddItemsToExistingStack(index, amount, maxValue);
					}
					else if (m_items.Count < MaxItemCount)
					{
						amount = AddItemsToNewStack(amount, maxValue, objectBuilder, itemId, index);
						flag = true;
					}
				}
			}
			for (int i = 0; i < MaxItemCount; i++)
			{
				if (i < m_items.Count)
				{
					MyPhysicalInventoryItem item = m_items[i];
					if (item.Content.CanStack(objectBuilder))
					{
						RaiseContentsAdded(item, amount);
						RaiseInventoryContentChanged(item, amount);
						amount = AddItemsToExistingStack(i, amount, maxValue);
					}
				}
				else
				{
					amount = AddItemsToNewStack(amount, maxValue, flag ? ((MyObjectBuilder_PhysicalObject)objectBuilder.Clone()) : objectBuilder, itemId);
					flag = true;
				}
				if (amount == 0)
				{
					break;
				}
			}
			RefreshVolumeAndMass();
			OnContentsChanged();
		}

		private MyFixedPoint AddItemsToNewStack(MyFixedPoint amount, MyFixedPoint maxStack, MyObjectBuilder_PhysicalObject objectBuilder, uint? itemId, int index = -1)
		{
			MyFixedPoint myFixedPoint = MyFixedPoint.Min(amount, maxStack);
			MyPhysicalInventoryItem myPhysicalInventoryItem = default(MyPhysicalInventoryItem);
			myPhysicalInventoryItem.Amount = myFixedPoint;
			myPhysicalInventoryItem.Scale = 1f;
			myPhysicalInventoryItem.Content = objectBuilder;
			MyPhysicalInventoryItem newItem = myPhysicalInventoryItem;
			newItem.ItemId = (itemId.HasValue ? itemId.Value : GetNextItemID());
			if (index >= 0 && index < m_items.Count)
			{
				m_items.Add(m_items[m_items.Count - 1]);
				for (int num = m_items.Count - 3; num >= index; num--)
				{
					m_items[num + 1] = m_items[num];
				}
				m_items[index] = newItem;
			}
			else
			{
				m_items.Add(newItem);
			}
			RaiseInventoryContentChanged(newItem, amount);
			m_usedIds.Add(newItem.ItemId);
			if (Sync.IsServer)
			{
				NotifyHudChangedInventoryItem(myFixedPoint, ref newItem, added: true);
			}
			return amount - myFixedPoint;
		}

		private MyFixedPoint AddItemsToExistingStack(int index, MyFixedPoint amount, MyFixedPoint maxStack)
		{
			MyPhysicalInventoryItem newItem = m_items[index];
			MyFixedPoint a = maxStack - newItem.Amount;
			if (a <= 0)
			{
				return amount;
			}
			MyFixedPoint myFixedPoint = MyFixedPoint.Min(a, amount);
			newItem.Amount += myFixedPoint;
			m_items[index] = newItem;
			if (Sync.IsServer)
			{
				NotifyHudChangedInventoryItem(myFixedPoint, ref newItem, added: true);
			}
			return amount - myFixedPoint;
		}

		private void NotifyHudChangedInventoryItem(MyFixedPoint amount, ref MyPhysicalInventoryItem newItem, bool added)
		{
			if (MyFakes.ENABLE_HUD_PICKED_UP_ITEMS && base.Entity != null && Owner is MyCharacter && MyHud.ChangedInventoryItems.Visible && (Owner as MyCharacter).GetPlayerIdentityId() == MySession.Static.LocalPlayerId)
			{
				MyHud.ChangedInventoryItems.AddChangedPhysicalInventoryItem(newItem, amount, added);
			}
		}

		private void FixDurabilityForInventoryItem(MyObjectBuilder_PhysicalObject objectBuilder)
		{
			MyPhysicalItemDefinition definition = null;
			if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(objectBuilder.GetId(), out definition))
			{
				return;
			}
			MyContainerDefinition definition2 = null;
			if (!MyComponentContainerExtension.TryGetContainerDefinition(definition.Id.TypeId, definition.Id.SubtypeId, out definition2) && objectBuilder.GetObjectId().TypeId == typeof(MyObjectBuilder_PhysicalGunObject))
			{
				MyHandItemDefinition myHandItemDefinition = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(objectBuilder.GetObjectId());
				if (myHandItemDefinition != null)
				{
					MyComponentContainerExtension.TryGetContainerDefinition(myHandItemDefinition.Id.TypeId, myHandItemDefinition.Id.SubtypeId, out definition2);
				}
			}
			if (definition2 != null && definition2.HasDefaultComponent("MyObjectBuilder_EntityDurabilityComponent") && !objectBuilder.DurabilityHP.HasValue)
			{
				objectBuilder.DurabilityHP = 100f;
			}
		}

		public bool RemoveItemsOfType(MyFixedPoint amount, MyObjectBuilder_PhysicalObject objectBuilder, bool spawn = false, bool onlyWhole = true)
		{
			return TransferOrRemove(this, amount, objectBuilder.GetObjectId(), objectBuilder.Flags, null, spawn, onlyWhole) == amount;
		}

		public override MyFixedPoint RemoveItemsOfType(MyFixedPoint amount, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, bool spawn = false)
		{
			return TransferOrRemove(this, amount, contentId, flags, null, spawn, onlyWhole: false);
		}

		public void DropItemById(uint itemId, MyFixedPoint amount)
		{
			MyMultiplayer.RaiseEvent(this, (MyInventory x) => x.DropItem_Implementation, amount, itemId);
		}

		public void DropItem(int itemIndex, MyFixedPoint amount)
		{
			uint itemId = m_items[itemIndex].ItemId;
			MyMultiplayer.RaiseEvent(this, (MyInventory x) => x.DropItem_Implementation, amount, itemId);
		}

		public void RemoveItemsAt(int itemIndex, MyFixedPoint? amount = null, bool sendEvent = true, bool spawn = false, MatrixD? spawnPos = null)
		{
			if (itemIndex >= 0 && itemIndex < m_items.Count)
			{
				if (Sync.IsServer)
				{
					RemoveItems(m_items[itemIndex].ItemId, amount, sendEvent, spawn, spawnPos);
				}
				else
				{
					MyMultiplayer.RaiseEvent(this, (MyInventory x) => x.RemoveItemsAt_Request, itemIndex, amount, sendEvent, spawn, spawnPos);
				}
			}
		}

		[Event(null, 1048)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void RemoveItemsAt_Request(int itemIndex, MyFixedPoint? amount = null, bool sendEvent = true, bool spawn = false, MatrixD? spawnPos = null)
		{
			RemoveItemsAt(itemIndex, amount, sendEvent, spawn, spawnPos);
		}

		public void RemoveItems(uint itemId, MyFixedPoint? amount = null, bool sendEvent = true, bool spawn = false, MatrixD? spawnPos = null, Action<MyDefinitionId, MyEntity> itemSpawned = null)
		{
			MyPhysicalInventoryItem? item = GetItemByID(itemId);
			MyFixedPoint amount2 = amount ?? (item.HasValue ? item.Value.Amount : ((MyFixedPoint)1));
			if (Sync.IsServer && item.HasValue && RemoveItemsInternal(itemId, amount2, sendEvent) && spawn)
			{
				if (!spawnPos.HasValue)
				{
					spawnPos = MatrixD.CreateWorld(Owner.PositionComp.GetPosition() + Owner.PositionComp.WorldMatrix.Forward + Owner.PositionComp.WorldMatrix.Up, Owner.PositionComp.WorldMatrix.Forward, Owner.PositionComp.WorldMatrix.Up);
				}
				item.Value.Spawn(amount2, spawnPos.Value, Owner, delegate(MyEntity spawned)
				{
					ItemSpawned(spawned, Owner, spawnPos, item.GetDefinitionId(), itemSpawned);
				});
			}
		}

		private void ItemSpawned(MyEntity spawned, MyEntity owner, MatrixD? spawnPos, MyDefinitionId defId, Action<MyDefinitionId, MyEntity> itemSpawned)
		{
			if (spawned == null || !spawnPos.HasValue)
			{
				return;
			}
			if (owner == MySession.Static.LocalCharacter)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.PlayDropItem);
			}
			else
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
				if (myEntity3DSoundEmitter != null)
				{
					myEntity3DSoundEmitter.SetPosition(spawnPos.Value.Translation);
					myEntity3DSoundEmitter.PlaySound(dropSound);
				}
			}
			itemSpawned?.Invoke(defId, spawned);
		}

		public bool RemoveItemsInternal(uint itemId, MyFixedPoint amount, bool sendEvent = true)
		{
			if (sendEvent)
			{
				OnBeforeContentsChanged();
			}
			bool flag = false;
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].ItemId == itemId)
				{
					MyPhysicalInventoryItem newItem = m_items[i];
					amount = MathHelper.Clamp(amount, 0, m_items[i].Amount);
					newItem.Amount -= amount;
					if (newItem.Amount == 0)
					{
						m_usedIds.Remove(m_items[i].ItemId);
						m_items.RemoveAt(i);
					}
					else
					{
						m_items[i] = newItem;
					}
					flag = true;
					this.RaiseEntityEvent(INVENTORY_CHANGED, new MyEntityContainerEventExtensions.InventoryChangedParams(newItem.ItemId, this, (float)newItem.Amount));
					if (sendEvent)
					{
						RaiseContentsRemoved(newItem, amount);
						RaiseInventoryContentChanged(newItem, -amount);
					}
					if (Sync.IsServer)
					{
						NotifyHudChangedInventoryItem(amount, ref newItem, added: false);
					}
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			RefreshVolumeAndMass();
			if (sendEvent)
			{
				OnContentsChanged();
			}
			return true;
		}

		public override List<MyPhysicalInventoryItem> GetItems()
		{
			return m_items;
		}

		public bool Empty()
		{
			if (m_items.Count == 0)
			{
				return true;
			}
			return false;
		}

		public static MyFixedPoint Transfer(MyInventory src, MyInventory dst, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, MyFixedPoint? amount = null, bool spawn = false)
		{
			return TransferOrRemove(src, amount, contentId, flags, dst);
		}

		private static MyFixedPoint TransferOrRemove(MyInventory src, MyFixedPoint? amount, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, MyInventory dst = null, bool spawn = false, bool onlyWhole = true)
		{
			MyFixedPoint result = 0;
			if (!onlyWhole)
			{
				amount = MyFixedPoint.Min(amount.Value, src.GetItemAmount(contentId, flags));
			}
			if (!onlyWhole || src.ContainItems(amount, contentId, flags))
			{
				bool flag = !amount.HasValue;
				MyFixedPoint myFixedPoint = flag ? ((MyFixedPoint)0) : amount.Value;
				if (contentId.TypeId == typeof(MyObjectBuilder_OxygenContainerObject) || contentId.TypeId == typeof(MyObjectBuilder_GasContainerObject))
				{
					int num = 0;
					while (num < src.m_items.Count && (flag || !(myFixedPoint == 0)))
					{
						MyPhysicalInventoryItem myPhysicalInventoryItem = src.m_items[num];
						MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = myPhysicalInventoryItem.Content as MyObjectBuilder_GasContainerObject;
						if (myObjectBuilder_GasContainerObject != null && myObjectBuilder_GasContainerObject.GasLevel == 1f)
						{
							num++;
						}
						else if (myPhysicalInventoryItem.Content.GetObjectId() != contentId)
						{
							num++;
						}
						else if (flag || myFixedPoint >= myPhysicalInventoryItem.Amount)
						{
							result += myPhysicalInventoryItem.Amount;
							myFixedPoint -= myPhysicalInventoryItem.Amount;
							Transfer(src, dst, myPhysicalInventoryItem.ItemId, -1, null, spawn);
						}
						else
						{
							result += myPhysicalInventoryItem.Amount;
							Transfer(src, dst, myPhysicalInventoryItem.ItemId, -1, myFixedPoint, spawn);
							myFixedPoint = 0;
						}
					}
				}
				int num2 = 0;
				while (num2 < src.m_items.Count && (flag || !(myFixedPoint == 0)))
				{
					MyPhysicalInventoryItem myPhysicalInventoryItem2 = src.m_items[num2];
					MyDefinitionId l = myPhysicalInventoryItem2.Content.GetId();
					if (l != contentId && myPhysicalInventoryItem2.Content.TypeId == typeof(MyObjectBuilder_BlockItem))
					{
						l = myPhysicalInventoryItem2.Content.GetObjectId();
					}
					if (l != contentId)
					{
						num2++;
					}
					else if (flag || myFixedPoint >= myPhysicalInventoryItem2.Amount)
					{
						result += myPhysicalInventoryItem2.Amount;
						myFixedPoint -= myPhysicalInventoryItem2.Amount;
						Transfer(src, dst, myPhysicalInventoryItem2.ItemId, -1, null, spawn);
					}
					else
					{
						result += myFixedPoint;
						Transfer(src, dst, myPhysicalInventoryItem2.ItemId, -1, myFixedPoint, spawn);
						myFixedPoint = 0;
					}
				}
			}
			return result;
		}

		public void Clear(bool sync = true)
		{
			if (!sync)
			{
				m_items.Clear();
				m_usedIds.Clear();
				RefreshVolumeAndMass();
				return;
			}
			for (int num = m_items.Count - 1; num >= 0; num--)
			{
				RemoveItems(m_items[num].ItemId);
			}
		}

		public void TransferItemFrom(MyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex = null, bool? stackIfPossible = null, MyFixedPoint? amount = null)
		{
			if (this != sourceInventory && sourceItemIndex >= 0 && sourceItemIndex < sourceInventory.m_items.Count)
			{
				Transfer(sourceInventory, this, sourceInventory.GetItems()[sourceItemIndex].ItemId, targetItemIndex.HasValue ? targetItemIndex.Value : (-1), amount);
			}
		}

		public MyPhysicalInventoryItem? GetItemByID(uint id)
		{
			foreach (MyPhysicalInventoryItem item in m_items)
			{
				if (item.ItemId == id)
				{
					return item;
				}
			}
			return null;
		}

		public MyPhysicalInventoryItem? GetItemByIndex(int id)
		{
			if (id >= 0 && id < m_items.Count)
			{
				return m_items[id];
			}
			return null;
		}

		public static void TransferByPlanner(MyInventory src, MyInventory dst, SerializableDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, MyFixedPoint? amount = null, bool spawn = false)
		{
			int arg = -1;
			for (int i = 0; i < dst.Owner.InventoryCount; i++)
			{
				if (dst.Owner.GetInventory(i) == dst)
				{
					arg = i;
					break;
				}
			}
			MyMultiplayer.RaiseEvent(src, (MyInventory x) => x.InventoryTransferItemPlanner_Implementation, dst.Owner.EntityId, arg, contentId, flags, amount, spawn);
		}

		public static void TransferByUser(MyInventory src, MyInventory dst, uint srcItemId, int dstIdx = -1, MyFixedPoint? amount = null)
		{
			if (src == null)
			{
				return;
			}
			MyPhysicalInventoryItem? itemByID = src.GetItemByID(srcItemId);
			if (!itemByID.HasValue)
			{
				return;
			}
			MyPhysicalInventoryItem value = itemByID.Value;
			if (dst != null && !dst.CheckConstraint(value.Content.GetObjectId()))
			{
				return;
			}
			MyFixedPoint myFixedPoint = amount ?? value.Amount;
			if (dst == null)
			{
				src.RemoveItems(srcItemId, amount);
				return;
			}
			byte arg = 0;
			for (byte b = 0; b < dst.Owner.InventoryCount; b = (byte)(b + 1))
			{
				if (dst.Owner.GetInventory(b).Equals(dst))
				{
					arg = b;
					break;
				}
			}
			if (MyInventory.OnTransferByUser != null)
			{
				MyInventory.OnTransferByUser(src, dst, value, myFixedPoint);
			}
			MyMultiplayer.RaiseEvent(src, (MyInventory x) => x.InventoryTransferItem_Implementation, myFixedPoint, srcItemId, dst.Owner.EntityId, arg, dstIdx);
		}

		public static void TransferAll(MyInventory src, MyInventory dst)
		{
			if (Sync.IsServer)
			{
				int num = src.m_items.Count + 1;
				while (src.m_items.Count != num && src.m_items.Count != 0)
				{
					num = src.m_items.Count;
					Transfer(src, dst, src.m_items[0].ItemId);
				}
			}
		}

		public static void Transfer(MyInventory src, MyInventory dst, uint srcItemId, int dstIdx = -1, MyFixedPoint? amount = null, bool spawn = false)
		{
			MyPhysicalInventoryItem? itemByID = src.GetItemByID(srcItemId);
			if (!itemByID.HasValue)
			{
				return;
			}
			MyPhysicalInventoryItem value = itemByID.Value;
			if ((dst == null || dst.CheckConstraint(value.Content.GetObjectId())) && Sync.IsServer)
			{
				MyFixedPoint amount2 = amount ?? value.Amount;
				if (dst == null)
				{
					src.RemoveItems(srcItemId, amount, sendEvent: true, spawn);
				}
				else
				{
					TransferItemsInternal(src, dst, srcItemId, spawn, dstIdx, amount2);
				}
			}
		}

		private static void TransferItemsInternal(MyInventory src, MyInventory dst, uint srcItemId, bool spawn, int destItemIndex, MyFixedPoint amount)
		{
			MyFixedPoint remove = amount;
			MyPhysicalInventoryItem value = default(MyPhysicalInventoryItem);
			int num = -1;
			for (int i = 0; i < src.m_items.Count; i++)
			{
				if (src.m_items[i].ItemId == srcItemId)
				{
					num = i;
					value = src.m_items[i];
					break;
				}
			}
			if (num == -1)
			{
				return;
			}
			FixTransferAmount(src, dst, value, spawn, ref remove, ref amount);
			if (!(amount != 0))
			{
				return;
			}
			if (src == dst && destItemIndex >= 0 && destItemIndex < dst.m_items.Count && !dst.m_items[destItemIndex].Content.CanStack(value.Content))
			{
				dst.SwapItems(num, destItemIndex);
				return;
			}
			dst.AddItemsInternal(amount, value.Content, (dst == src && remove == 0) ? new uint?(srcItemId) : null, destItemIndex);
			if (remove != 0)
			{
				src.RemoveItems(srcItemId, remove);
			}
		}

		private void SwapItems(int srcIndex, int dstIndex)
		{
			MyPhysicalInventoryItem value = m_items[dstIndex];
			m_items[dstIndex] = m_items[srcIndex];
			m_items[srcIndex] = value;
			OnContentsChanged();
		}

		private static void FixTransferAmount(MyInventory src, MyInventory dst, MyPhysicalInventoryItem? srcItem, bool spawn, ref MyFixedPoint remove, ref MyFixedPoint add)
		{
			if (srcItem.Value.Amount < remove)
			{
				remove = srcItem.Value.Amount;
				add = remove;
			}
			if (!dst.IsConstrained || src == dst)
			{
				return;
			}
			MyFixedPoint myFixedPoint = dst.ComputeAmountThatFits(srcItem.Value.Content.GetObjectId());
			if (myFixedPoint < remove)
			{
				if (spawn)
				{
					MyEntity owner = dst.Owner;
					Matrix matrix = owner.WorldMatrix;
					MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(remove - myFixedPoint, srcItem.Value.Content), owner.PositionComp.GetPosition() + matrix.Forward + matrix.Up, matrix.Forward, matrix.Up, owner.Physics);
				}
				else
				{
					remove = myFixedPoint;
				}
				add = myFixedPoint;
			}
		}

		public bool FilterItemsUsingConstraint()
		{
			bool flag = false;
			for (int num = m_items.Count - 1; num >= 0; num--)
			{
				if (!CheckConstraint(m_items[num].Content.GetObjectId()))
				{
					RemoveItems(m_items[num].ItemId, null, sendEvent: false);
					flag = true;
				}
			}
			if (flag)
			{
				OnContentsChanged();
			}
			return flag;
		}

		public bool IsItemAt(int position)
		{
			return m_items.IsValidIndex(position);
		}

		public override void CountItems(Dictionary<MyDefinitionId, MyFixedPoint> itemCounts)
		{
			foreach (MyPhysicalInventoryItem item in m_items)
			{
				MyDefinitionId key = item.Content.GetId();
				if (key.TypeId == typeof(MyObjectBuilder_BlockItem))
				{
					key = item.Content.GetObjectId();
				}
				if (!key.TypeId.IsNull && !(key.SubtypeId == MyStringHash.NullOrEmpty))
				{
					MyFixedPoint value = 0;
					itemCounts.TryGetValue(key, out value);
					itemCounts[key] = value + (int)item.Amount;
				}
			}
		}

		public override void ApplyChanges(List<MyComponentChange> changes)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			m_tmpItemsToAdd.Clear();
			bool flag = false;
			for (int i = 0; i < changes.Count; i++)
			{
				MyComponentChange value = changes[i];
				if (value.IsAddition())
				{
					throw new NotImplementedException();
				}
				if (value.Amount <= 0)
				{
					continue;
				}
				for (int num = m_items.Count - 1; num >= 0; num--)
				{
					MyPhysicalInventoryItem value2 = m_items[num];
					MyDefinitionId r = value2.Content.GetId();
					if (r.TypeId == typeof(MyObjectBuilder_BlockItem))
					{
						r = value2.Content.GetObjectId();
					}
					if (!(value.ToRemove != r))
					{
						MyFixedPoint myFixedPoint = value.Amount;
						if (myFixedPoint > 0)
						{
							MyFixedPoint myFixedPoint2 = MyFixedPoint.Min(myFixedPoint, value2.Amount);
							myFixedPoint -= myFixedPoint2;
							if (myFixedPoint == 0)
							{
								changes.RemoveAtFast(i);
								value.Amount = 0;
								i--;
							}
							else
							{
								value.Amount = (int)myFixedPoint;
								changes[i] = value;
							}
							if (value2.Amount - myFixedPoint2 == 0)
							{
								m_usedIds.Remove(m_items[num].ItemId);
								m_items.RemoveAt(num);
							}
							else
							{
								value2.Amount -= myFixedPoint2;
								m_items[num] = value2;
							}
							if (value.IsChange())
							{
								int value3 = 0;
								m_tmpItemsToAdd.TryGetValue(value.ToAdd, out value3);
								value3 += (int)myFixedPoint2;
								if (value3 != 0)
								{
									m_tmpItemsToAdd[value.ToAdd] = value3;
								}
							}
							flag = true;
							this.RaiseEntityEvent(INVENTORY_CHANGED, new MyEntityContainerEventExtensions.InventoryChangedParams(value2.ItemId, this, (float)value2.Amount));
						}
					}
				}
			}
			RefreshVolumeAndMass();
			foreach (KeyValuePair<MyDefinitionId, int> item in m_tmpItemsToAdd)
			{
				MyCubeBlockDefinition componentBlockDefinition = MyDefinitionManager.Static.GetComponentBlockDefinition(item.Key);
				if (componentBlockDefinition == null)
				{
					return;
				}
				AddBlocks(componentBlockDefinition, item.Value);
				flag = true;
				RefreshVolumeAndMass();
			}
			if (flag)
			{
				RefreshVolumeAndMass();
				OnContentsChanged();
			}
		}

		public void ClearItems()
		{
			m_items.Clear();
			m_usedIds.Clear();
		}

		public void AddItemClient(int position, MyPhysicalInventoryItem item)
		{
			if (!Sync.IsServer)
			{
				if (position >= m_items.Count)
				{
					m_items.Add(item);
				}
				else
				{
					m_items.Insert(position, item);
				}
				m_usedIds.Add(item.ItemId);
				RaiseContentsAdded(item, item.Amount);
				RaiseInventoryContentChanged(item, item.Amount);
				NotifyHudChangedInventoryItem(item.Amount, ref item, added: true);
			}
		}

		public void ChangeItemClient(MyPhysicalInventoryItem item, int position)
		{
			if (position >= 0 && position < m_items.Count)
			{
				m_items[position] = item;
			}
		}

		public MyObjectBuilder_Inventory GetObjectBuilder()
		{
			MyObjectBuilder_Inventory myObjectBuilder_Inventory = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Inventory>();
			myObjectBuilder_Inventory.Items.Clear();
			myObjectBuilder_Inventory.Mass = m_maxMass;
			myObjectBuilder_Inventory.Volume = m_maxVolume;
			myObjectBuilder_Inventory.MaxItemCount = m_maxItemCount;
			myObjectBuilder_Inventory.InventoryFlags = m_flags;
			myObjectBuilder_Inventory.nextItemId = m_nextItemID;
			myObjectBuilder_Inventory.RemoveEntityOnEmpty = RemoveEntityOnEmpty;
			foreach (MyPhysicalInventoryItem item in m_items)
			{
				myObjectBuilder_Inventory.Items.Add(item.GetObjectBuilder());
			}
			return myObjectBuilder_Inventory;
		}

		public void Init(MyObjectBuilder_Inventory objectBuilder)
		{
			if (objectBuilder == null)
			{
				if (myObjectBuilder_InventoryDefinition != null)
				{
					m_maxMass = (MyFixedPoint)myObjectBuilder_InventoryDefinition.InventoryMass;
					m_maxVolume = (MyFixedPoint)myObjectBuilder_InventoryDefinition.InventoryVolume;
					m_maxItemCount = myObjectBuilder_InventoryDefinition.MaxItemCount;
				}
				return;
			}
			Clear(sync: false);
			if (objectBuilder.Mass.HasValue)
			{
				m_maxMass = objectBuilder.Mass.Value;
			}
			if (objectBuilder.MaxItemCount.HasValue)
			{
				m_maxItemCount = objectBuilder.MaxItemCount.Value;
			}
			if (objectBuilder.InventoryFlags.HasValue)
			{
				m_flags = objectBuilder.InventoryFlags.Value;
			}
			RemoveEntityOnEmpty = objectBuilder.RemoveEntityOnEmpty;
			if (!Sync.IsServer || MySession.Static.Ready)
			{
				m_nextItemID = objectBuilder.nextItemId;
			}
			else
			{
				m_nextItemID = 0u;
			}
			m_objectBuilder = objectBuilder;
			if (base.Entity != null)
			{
				InitItems();
			}
		}

		public override void OnAddedToContainer()
		{
			if (m_objectBuilder != null && m_objectBuilder.Volume.HasValue)
			{
				MyFixedPoint value = m_objectBuilder.Volume.Value;
				if (value != MyFixedPoint.MaxValue || !IsConstrained)
				{
					m_maxVolume = value;
				}
			}
			InitItems();
			base.OnAddedToContainer();
		}

		private void InitItems()
		{
			if (m_objectBuilder != null)
			{
				bool flag = !Sync.IsServer || MySession.Static.Ready;
				int num = 0;
				foreach (MyObjectBuilder_InventoryItem item in m_objectBuilder.Items)
				{
					if (!(item.Amount <= 0) && item.PhysicalContent != null && MyInventoryItemAdapter.Static.TryAdapt(item.PhysicalContent.GetObjectId()))
					{
						MyDefinitionId objectId = item.PhysicalContent.GetObjectId();
						MyFixedPoint myFixedPoint = MyFixedPoint.Min(ComputeAmountThatFits(objectId), item.Amount);
						if (!(myFixedPoint == MyFixedPoint.Zero))
						{
							if (!item.PhysicalContent.CanStack(item.PhysicalContent))
							{
								MyFixedPoint a = 0;
								while (a < myFixedPoint)
								{
									AddItemsInternal(1, item.PhysicalContent, (!flag) ? null : new uint?(item.ItemId), num);
									a += (MyFixedPoint)1;
									num++;
								}
							}
							else
							{
								AddItemsInternal(myFixedPoint, item.PhysicalContent, (!flag) ? null : new uint?(item.ItemId), num);
							}
							num++;
						}
					}
				}
				m_objectBuilder = null;
			}
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			MyInventoryComponentDefinition myInventoryComponentDefinition = definition as MyInventoryComponentDefinition;
			if (myInventoryComponentDefinition != null)
			{
				m_maxVolume = (MyFixedPoint)myInventoryComponentDefinition.Volume;
				m_maxMass = (MyFixedPoint)myInventoryComponentDefinition.Mass;
				RemoveEntityOnEmpty = myInventoryComponentDefinition.RemoveEntityOnEmpty;
				m_multiplierEnabled = myInventoryComponentDefinition.MultiplierEnabled;
				m_maxItemCount = myInventoryComponentDefinition.MaxItemCount;
				Constraint = myInventoryComponentDefinition.InputConstraint;
			}
		}

		public void GenerateContent(MyContainerTypeDefinition containerDefinition)
		{
			int randomInt = MyUtils.GetRandomInt(containerDefinition.CountMin, containerDefinition.CountMax);
			for (int i = 0; i < randomInt; i++)
			{
				MyContainerTypeDefinition.ContainerTypeItem containerTypeItem = containerDefinition.SelectNextRandomItem();
				MyFixedPoint myFixedPoint = (MyFixedPoint)MyRandom.Instance.NextFloat((float)containerTypeItem.AmountMin, (float)containerTypeItem.AmountMax);
				if (ContainItems(1, containerTypeItem.DefinitionId))
				{
					MyFixedPoint itemAmount = GetItemAmount(containerTypeItem.DefinitionId);
					myFixedPoint -= itemAmount;
					if (myFixedPoint <= 0)
					{
						continue;
					}
				}
				MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(containerTypeItem.DefinitionId);
				if (physicalItemDefinition == null)
				{
					continue;
				}
				if (physicalItemDefinition.HasIntegralAmounts)
				{
					myFixedPoint = MyFixedPoint.Ceiling(myFixedPoint);
				}
				if (CheckConstraint(containerTypeItem.DefinitionId))
				{
					myFixedPoint = MyFixedPoint.Min(ComputeAmountThatFits(containerTypeItem.DefinitionId), myFixedPoint);
					if (myFixedPoint > 0)
					{
						MyObjectBuilder_PhysicalObject objectBuilder = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(containerTypeItem.DefinitionId);
						AddItems(myFixedPoint, objectBuilder);
					}
				}
			}
			containerDefinition.DeselectAll();
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			return GetObjectBuilder();
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			MyObjectBuilder_Inventory objectBuilder = builder as MyObjectBuilder_Inventory;
			Init(objectBuilder);
		}

		private void RefreshVolumeAndMass()
		{
			if (Sync.IsServer)
			{
				m_currentMass.Value = 0;
				m_currentVolume.Value = 0;
				MyFixedPoint value = 0;
				MyFixedPoint value2 = 0;
				foreach (MyPhysicalInventoryItem item in m_items)
				{
					MyInventoryItemAdapter @static = MyInventoryItemAdapter.Static;
					@static.Adapt(item);
					value += @static.Mass * item.Amount;
					value2 += @static.Volume * item.Amount;
				}
				m_currentMass.Value = value;
				m_currentVolume.Value = value2;
			}
		}

		[Conditional("DEBUG")]
		private void VerifyIntegrity()
		{
			HashSet<uint> hashSet = new HashSet<uint>();
			foreach (MyPhysicalInventoryItem item in m_items)
			{
				hashSet.Add(item.ItemId);
				item.Content.CanStack(item.Content);
			}
		}

		public void UpdateGasAmount()
		{
			RefreshVolumeAndMass();
			OnContentsChanged();
		}

		public void AddEntity(VRage.ModAPI.IMyEntity entity, bool blockManipulatedEntity = true)
		{
			MyMultiplayer.RaiseEvent(this, (MyInventory x) => x.AddEntity_Implementation, entity.EntityId, blockManipulatedEntity);
		}

		[Event(null, 1926)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void AddEntity_Implementation(long entityId, bool blockManipulatedEntity)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEntity entity) && entity != null)
			{
				AddEntityInternal(entity, blockManipulatedEntity);
			}
		}

		private void AddEntityInternal(VRage.ModAPI.IMyEntity ientity, bool blockManipulatedEntity = true)
		{
			MyEntity myEntity = ientity as MyEntity;
			if (myEntity == null)
			{
				return;
			}
			Vector3D? hitPosition = null;
			MyCharacterDetectorComponent myCharacterDetectorComponent = Owner.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent != null)
			{
				hitPosition = myCharacterDetectorComponent.HitPosition;
			}
			myEntity = TestEntityForPickup(myEntity, hitPosition, out MyDefinitionId _, blockManipulatedEntity);
			if (myEntity is MyCubeGrid)
			{
				if (!AddGrid(myEntity as MyCubeGrid))
				{
					MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
				}
			}
			else if (myEntity is MyCubeBlock)
			{
				if (!AddBlockAndRemoveFromGrid((myEntity as MyCubeBlock).SlimBlock))
				{
					MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
				}
			}
			else if (myEntity is MyFloatingObject)
			{
				TakeFloatingObject(myEntity as MyFloatingObject);
			}
		}

		public MyEntity TestEntityForPickup(MyEntity entity, Vector3D? hitPosition, out MyDefinitionId entityDefId, bool blockManipulatedEntity = true)
		{
			MyCubeBlock block;
			MyCubeGrid myCubeGrid = MyItemsCollector.TryGetAsComponent(entity, out block, blockManipulatedEntity, hitPosition);
			MyUseObjectsComponentBase component = null;
			entityDefId = new MyDefinitionId(null);
			if (myCubeGrid != null)
			{
				if (!MyCubeGrid.IsGridInCompleteState(myCubeGrid))
				{
					MyHud.Notifications.Add(MyNotificationSingletons.IncompleteGrid);
					return null;
				}
				entityDefId = new MyDefinitionId(typeof(MyObjectBuilder_CubeGrid));
				return myCubeGrid;
			}
			if (MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID && block != null && block.BlockDefinition.CubeSize == MyCubeSize.Small)
			{
				MyEntity baseEntity = block.GetBaseEntity();
				if (baseEntity != null && baseEntity.HasInventory && !baseEntity.GetInventory().Empty())
				{
					MyHud.Notifications.Add(m_inventoryNotEmptyNotification);
					return null;
				}
				entityDefId = block.BlockDefinition.Id;
				return block;
			}
			if (entity is MyFloatingObject)
			{
				MyFloatingObject myFloatingObject = entity as MyFloatingObject;
				if (MyFixedPoint.Min(myFloatingObject.Item.Amount, ComputeAmountThatFits(myFloatingObject.Item.Content.GetObjectId())) == 0)
				{
					MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
					return null;
				}
				entityDefId = myFloatingObject.Item.GetDefinitionId();
				return entity;
			}
			entity.Components.TryGet(out component);
			return null;
		}

		public override bool ItemsCanBeAdded(MyFixedPoint amount, VRage.Game.ModAPI.Ingame.IMyInventoryItem item)
		{
			if (item == null)
			{
				return false;
			}
			return CanItemsBeAdded(amount, item.GetDefinitionId());
		}

		public override bool ItemsCanBeRemoved(MyFixedPoint amount, VRage.Game.ModAPI.Ingame.IMyInventoryItem item)
		{
			if (amount == 0)
			{
				return true;
			}
			if (item == null)
			{
				return false;
			}
			MyPhysicalInventoryItem? itemByID = GetItemByID(item.ItemId);
			if (itemByID.HasValue && itemByID.Value.Amount >= amount)
			{
				return true;
			}
			return false;
		}

		public override bool Add(VRage.Game.ModAPI.Ingame.IMyInventoryItem item, MyFixedPoint amount)
		{
			uint? itemId = m_usedIds.Contains(item.ItemId) ? null : new uint?(item.ItemId);
			return AddItems(amount, item.Content, itemId);
		}

		public override bool Remove(VRage.Game.ModAPI.Ingame.IMyInventoryItem item, MyFixedPoint amount)
		{
			if (item.Content is MyObjectBuilder_PhysicalObject)
			{
				int itemIndexById = GetItemIndexById(item.ItemId);
				if (itemIndexById != -1)
				{
					RemoveItemsAt(itemIndexById, amount);
					return true;
				}
				return RemoveItemsOfType(amount, item.Content as MyObjectBuilder_PhysicalObject);
			}
			return false;
		}

		public override int GetItemsCount()
		{
			return m_items.Count;
		}

		public int GetItemIndexById(uint id)
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].ItemId == id)
				{
					return i;
				}
			}
			return -1;
		}

		[Event(null, 2088)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		public static void ModifyDatapad(long inventoryOwner, int inventoryIndex, uint itemIndex, string name, string data)
		{
			if (!MyEntities.EntityExists(inventoryOwner))
			{
				return;
			}
			MyInventory inventory = MyEntities.GetEntityById(inventoryOwner).GetInventory(inventoryIndex);
			if (inventory == null)
			{
				return;
			}
			MyPhysicalInventoryItem myPhysicalInventoryItem = default(MyPhysicalInventoryItem);
			int num = -1;
			for (int i = 0; i < inventory.m_items.Count; i++)
			{
				if (inventory.m_items[i].ItemId == itemIndex)
				{
					num = i;
					myPhysicalInventoryItem = inventory.m_items[i];
					break;
				}
			}
			MyObjectBuilder_Datapad myObjectBuilder_Datapad;
			if (num != -1 && (myObjectBuilder_Datapad = (myPhysicalInventoryItem.Content as MyObjectBuilder_Datapad)) != null)
			{
				myObjectBuilder_Datapad.Name = name;
				myObjectBuilder_Datapad.Data = data;
				MyMultiplayer.RaiseEvent(inventory, (MyInventory x) => x.ModifyDatapad_Broadcast, (uint)num, name, data);
			}
		}

		[Event(null, 2122)]
		[Reliable]
		[Broadcast]
		private void ModifyDatapad_Broadcast(uint itemIndex, string name, string data)
		{
			MyPhysicalInventoryItem myPhysicalInventoryItem = default(MyPhysicalInventoryItem);
			int num = -1;
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].ItemId == itemIndex)
				{
					num = i;
					myPhysicalInventoryItem = m_items[i];
					break;
				}
			}
			MyObjectBuilder_Datapad myObjectBuilder_Datapad;
			if (num != -1 && (myObjectBuilder_Datapad = (myPhysicalInventoryItem.Content as MyObjectBuilder_Datapad)) != null)
			{
				myObjectBuilder_Datapad.Name = name;
				myObjectBuilder_Datapad.Data = data;
			}
		}

		[Event(null, 2147)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void InventoryTransferItemPlanner_Implementation(long destinationOwnerId, int destInventoryIndex, SerializableDefinitionId contentId, MyItemFlags flags, MyFixedPoint? amount, bool spawn)
		{
			if (MyEntities.EntityExists(destinationOwnerId))
			{
				MyInventory inventory = MyEntities.GetEntityById(destinationOwnerId).GetInventory(destInventoryIndex);
				Transfer(this, inventory, contentId, flags, amount, spawn);
			}
		}

		[Event(null, 2160)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void InventoryTransferItem_Implementation(MyFixedPoint amount, uint itemId, long destinationOwnerId, byte destInventoryIndex, int destinationIndex)
		{
			if (MyEntities.EntityExists(destinationOwnerId))
			{
				MyInventory inventory = MyEntities.GetEntityById(destinationOwnerId).GetInventory(destInventoryIndex);
				TransferItemsInternal(this, inventory, itemId, spawn: false, destinationIndex, amount);
			}
		}

		[Event(null, 2171)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void DebugAddItems_Implementation(MyFixedPoint amount, [DynamicObjectBuilder(false)] MyObjectBuilder_Base objectBuilder)
		{
			MyLog.Default.WriteLine("DebugAddItems not supported on OFFICIAL builds (it's cheating)");
		}

		[Event(null, 2184)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void DropItem_Implementation(MyFixedPoint amount, uint itemIndex)
		{
			if (MyVisualScriptLogicProvider.PlayerDropped != null)
			{
				MyCharacter myCharacter = Owner as MyCharacter;
				if (myCharacter != null)
				{
					MyPhysicalInventoryItem? itemByID = GetItemByID(itemIndex);
					long controllingIdentityId = myCharacter.ControllerInfo.ControllingIdentityId;
					MyVisualScriptLogicProvider.PlayerDropped(itemByID.Value.Content.TypeId.ToString(), itemByID.Value.Content.SubtypeName, controllingIdentityId, amount.ToIntSafe());
				}
			}
			RemoveItems(itemIndex, amount, sendEvent: true, spawn: true);
		}

		public void UpdateItem(MyDefinitionId contentId, uint? itemId = null, float? amount = null, float? itemHP = null)
		{
			if (!amount.HasValue && !itemHP.HasValue)
			{
				return;
			}
			int? num = null;
			if (itemId.HasValue)
			{
				int itemIndexById = GetItemIndexById(itemId.Value);
				if (m_items.IsValidIndex(itemIndexById))
				{
					num = itemIndexById;
				}
			}
			else
			{
				num = FindFirstPositionOfType(contentId);
			}
			bool flag = false;
			if (num.HasValue && m_items.IsValidIndex(num.Value))
			{
				MyPhysicalInventoryItem value = m_items[num.Value];
				if (amount.HasValue && amount.Value != (float)value.Amount)
				{
					value.Amount = (MyFixedPoint)amount.Value;
					flag = true;
				}
				if (itemHP.HasValue && value.Content != null && (!value.Content.DurabilityHP.HasValue || value.Content.DurabilityHP.Value != itemHP.Value))
				{
					value.Content.DurabilityHP = itemHP.Value;
					flag = true;
				}
				if (flag)
				{
					m_items[num.Value] = value;
					OnContentsChanged();
				}
			}
		}

		public bool IsUniqueId(uint idToTest)
		{
			return !m_usedIds.Contains(idToTest);
		}

		private uint GetNextItemID()
		{
			while (!IsUniqueId(m_nextItemID))
			{
				if (m_nextItemID == uint.MaxValue)
				{
					m_nextItemID = 0u;
				}
				else
				{
					m_nextItemID++;
				}
			}
			return m_nextItemID++;
		}

		private void PropertiesChanged()
		{
			if (!Sync.IsServer)
			{
				OnContentsChanged();
			}
		}

		public override void OnContentsChanged()
		{
			RaiseContentsChanged();
			if (Sync.IsServer && RemoveEntityOnEmpty && GetItemsCount() == 0)
			{
				base.Container.Entity.Close();
			}
		}

		public override void OnBeforeContentsChanged()
		{
			RaiseBeforeContentsChanged();
		}

		public override void OnContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			RaiseContentsAdded(item, amount);
			RaiseInventoryContentChanged(item, amount);
		}

		public override void OnContentsRemoved(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			RaiseContentsRemoved(item, amount);
			RaiseInventoryContentChanged(item, -amount);
		}

		public override void ConsumeItem(MyDefinitionId itemId, MyFixedPoint amount, long consumerEntityId = 0L)
		{
			SerializableDefinitionId arg = itemId;
			MyMultiplayer.RaiseEvent(this, (MyInventory x) => x.InventoryConsumeItem_Implementation, amount, arg, consumerEntityId);
		}

		public override int GetInventoryCount()
		{
			return 1;
		}

		public override MyInventoryBase IterateInventory(int searchIndex, int currentIndex = 0)
		{
			if (currentIndex != searchIndex)
			{
				return null;
			}
			return this;
		}

		[Event(null, 2326)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void InventoryConsumeItem_Implementation(MyFixedPoint amount, SerializableDefinitionId itemId, long consumerEntityId)
		{
			if (consumerEntityId != 0L && !MyEntities.EntityExists(consumerEntityId))
			{
				return;
			}
			MyFixedPoint itemAmount = GetItemAmount(itemId);
			if (itemAmount < amount)
			{
				amount = itemAmount;
			}
			MyEntity myEntity = null;
			if (consumerEntityId != 0L)
			{
				myEntity = MyEntities.GetEntityById(consumerEntityId);
				if (myEntity == null)
				{
					return;
				}
			}
			if (myEntity.Components != null)
			{
				MyUsableItemDefinition myUsableItemDefinition = MyDefinitionManager.Static.GetDefinition(itemId) as MyUsableItemDefinition;
				if (myUsableItemDefinition != null)
				{
					MyCharacter myCharacter = myEntity as MyCharacter;
					myCharacter?.SoundComp.StartSecondarySound(myUsableItemDefinition.UseSound, sync: true);
					MyConsumableItemDefinition myConsumableItemDefinition = myUsableItemDefinition as MyConsumableItemDefinition;
					if (myConsumableItemDefinition != null)
					{
						myCharacter?.SuitBattery.Consume(amount, myConsumableItemDefinition);
						(myEntity.Components.Get<MyEntityStatComponent>() as MyCharacterStatComponent)?.Consume(amount, myConsumableItemDefinition);
					}
				}
			}
			if (true)
			{
				RemoveItemsOfType(amount, itemId);
			}
		}

		public void UpdateItemAmoutClient(uint itemId, MyFixedPoint amount)
		{
			if (Sync.IsServer)
			{
				return;
			}
			MyPhysicalInventoryItem? myPhysicalInventoryItem = null;
			int num = -1;
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].ItemId == itemId)
				{
					myPhysicalInventoryItem = m_items[i];
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				MyPhysicalInventoryItem newItem = myPhysicalInventoryItem.Value;
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = newItem.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null)
				{
					myObjectBuilder_GasContainerObject.GasLevel += (float)amount;
				}
				else
				{
					newItem.Amount += amount;
				}
				m_items[num] = newItem;
				RaiseContentsAdded(newItem, amount);
				RaiseInventoryContentChanged(newItem, amount);
				NotifyHudChangedInventoryItem(amount, ref newItem, amount > 0);
			}
		}

		public void RemoveItemClient(uint itemId)
		{
			if (Sync.IsServer)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].ItemId == itemId)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				MyPhysicalInventoryItem newItem = m_items[num];
				NotifyHudChangedInventoryItem(newItem.Amount, ref newItem, added: false);
				newItem.Amount = 0;
				RaiseInventoryContentChanged(newItem, -1);
				m_items.RemoveAt(num);
				m_usedIds.Remove(itemId);
			}
		}

		public void Refresh()
		{
			RefreshVolumeAndMass();
			OnContentsChanged();
		}

		public void FixInventoryVolume(float newValue)
		{
			if (m_maxVolume == MyFixedPoint.MaxValue)
			{
				m_maxVolume = (MyFixedPoint)newValue;
			}
		}

		public void ResetVolume()
		{
			m_maxVolume = MyFixedPoint.MaxValue;
		}

		MyFixedPoint VRage.Game.ModAPI.Ingame.IMyInventory.GetItemAmount(MyItemType contentId)
		{
			return GetItemAmount(contentId);
		}

		bool VRage.Game.ModAPI.Ingame.IMyInventory.ContainItems(MyFixedPoint amount, MyItemType itemType)
		{
			return ContainItems(amount, itemType);
		}

		public MyInventoryItem? GetItemAt(int index)
		{
			if (!IsItemAt(index))
			{
				return null;
			}
			return m_items[index].MakeAPIItem();
		}

		MyInventoryItem? VRage.Game.ModAPI.Ingame.IMyInventory.GetItemByID(uint id)
		{
			return GetItemByID(id).MakeAPIItem();
		}

		MyInventoryItem? VRage.Game.ModAPI.Ingame.IMyInventory.FindItem(MyItemType itemType)
		{
			return FindItem(itemType).MakeAPIItem();
		}

		bool VRage.Game.ModAPI.Ingame.IMyInventory.CanItemsBeAdded(MyFixedPoint amount, MyItemType itemType)
		{
			return CanItemsBeAdded(amount, itemType);
		}

		void VRage.Game.ModAPI.Ingame.IMyInventory.GetItems(List<MyInventoryItem> items, Func<MyInventoryItem, bool> filter = null)
		{
			foreach (MyPhysicalInventoryItem item in m_items)
			{
				MyInventoryItem myInventoryItem = item.MakeAPIItem();
				if (filter == null || filter(myInventoryItem))
				{
					items.Add(myInventoryItem);
				}
			}
		}

		bool VRage.Game.ModAPI.Ingame.IMyInventory.TransferItemTo(VRage.Game.ModAPI.Ingame.IMyInventory dstInventory, MyInventoryItem item, MyFixedPoint? amount = null)
		{
			int itemIndexById = GetItemIndexById(item.ItemId);
			if (itemIndexById < 0)
			{
				return false;
			}
			return TransferItemsTo(dstInventory, itemIndexById, null, null, amount, useConveyor: true);
		}

		bool VRage.Game.ModAPI.Ingame.IMyInventory.TransferItemFrom(VRage.Game.ModAPI.Ingame.IMyInventory sourceInventory, MyInventoryItem item, MyFixedPoint? amount = null)
		{
			if (!(sourceInventory is MyInventory))
			{
				return false;
			}
			return sourceInventory.TransferItemTo(this, item, amount);
		}

		bool VRage.Game.ModAPI.Ingame.IMyInventory.TransferItemTo(VRage.Game.ModAPI.Ingame.IMyInventory dst, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount)
		{
			return TransferItemsTo(dst, sourceItemIndex, targetItemIndex, stackIfPossible, amount, useConveyor: true);
		}

		bool VRage.Game.ModAPI.Ingame.IMyInventory.TransferItemFrom(VRage.Game.ModAPI.Ingame.IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount)
		{
			return TransferItemsFrom(sourceInventory, sourceItemIndex, targetItemIndex, stackIfPossible, amount, useConveyors: true);
		}

		bool VRage.Game.ModAPI.Ingame.IMyInventory.IsConnectedTo(VRage.Game.ModAPI.Ingame.IMyInventory dst)
		{
			MyInventory myInventory = dst as MyInventory;
			if (myInventory == null)
			{
				return false;
			}
			return CanTransferTo(myInventory, null);
		}

		bool VRage.Game.ModAPI.Ingame.IMyInventory.CanTransferItemTo(VRage.Game.ModAPI.Ingame.IMyInventory dst, MyItemType itemType)
		{
			MyInventory myInventory = dst as MyInventory;
			if (myInventory == null)
			{
				return false;
			}
			return CanTransferTo(myInventory, itemType);
		}

		void VRage.Game.ModAPI.Ingame.IMyInventory.GetAcceptedItems(List<MyItemType> items, Func<MyItemType, bool> filter)
		{
			if ((Constraint?.IsWhitelist ?? false) && Constraint.ConstrainedTypes.Count == 0)
			{
				foreach (MyDefinitionId constrainedId in Constraint.ConstrainedIds)
				{
					MyItemType arg = constrainedId;
					if (filter == null || filter(arg))
					{
						items.Add(constrainedId);
					}
				}
			}
			else
			{
				foreach (MyDefinitionBase inventoryItemDefinition in MyDefinitionManager.Static.GetInventoryItemDefinitions())
				{
					MyDefinitionId id = inventoryItemDefinition.Id;
					if (Constraint == null || Constraint.Check(id))
					{
						MyItemType myItemType = id;
						if (filter == null || filter(myItemType))
						{
							items.Add(myItemType);
						}
					}
				}
			}
		}

		void VRage.Game.ModAPI.IMyInventory.AddItems(MyFixedPoint amount, MyObjectBuilder_PhysicalObject objectBuilder, int index)
		{
			AddItems(amount, objectBuilder, null, index);
		}

		void VRage.Game.ModAPI.IMyInventory.RemoveItemsOfType(MyFixedPoint amount, MyObjectBuilder_PhysicalObject objectBuilder, bool spawn)
		{
			RemoveItemsOfType(amount, objectBuilder, spawn);
		}

		void VRage.Game.ModAPI.IMyInventory.RemoveItemsOfType(MyFixedPoint amount, SerializableDefinitionId contentId, MyItemFlags flags, bool spawn)
		{
			RemoveItemsOfType(amount, contentId, flags, spawn);
		}

		void VRage.Game.ModAPI.IMyInventory.RemoveItemsAt(int itemIndex, MyFixedPoint? amount, bool sendEvent, bool spawn)
		{
			RemoveItemsAt(itemIndex, amount, sendEvent, spawn);
		}

		void VRage.Game.ModAPI.IMyInventory.RemoveItems(uint itemId, MyFixedPoint? amount, bool sendEvent, bool spawn)
		{
			RemoveItems(itemId, amount, sendEvent, spawn);
		}

		void VRage.Game.ModAPI.IMyInventory.RemoveItemAmount(VRage.Game.ModAPI.IMyInventoryItem item, MyFixedPoint amount)
		{
			Remove(item, amount);
		}

		bool VRage.Game.ModAPI.IMyInventory.TransferItemTo(VRage.Game.ModAPI.IMyInventory dst, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount, bool checkConnection)
		{
			return TransferItemsTo(dst, sourceItemIndex, targetItemIndex, stackIfPossible, amount, checkConnection);
		}

		bool VRage.Game.ModAPI.IMyInventory.TransferItemFrom(VRage.Game.ModAPI.IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount, bool checkConnection)
		{
			return TransferItemsFrom(sourceInventory, sourceItemIndex, targetItemIndex, stackIfPossible, amount, checkConnection);
		}

		List<VRage.Game.ModAPI.IMyInventoryItem> VRage.Game.ModAPI.IMyInventory.GetItems()
		{
			return m_items.OfType<VRage.Game.ModAPI.IMyInventoryItem>().ToList();
		}

		VRage.Game.ModAPI.IMyInventoryItem VRage.Game.ModAPI.IMyInventory.GetItemByID(uint id)
		{
			MyPhysicalInventoryItem? itemByID = GetItemByID(id);
			if (itemByID.HasValue)
			{
				return itemByID.Value;
			}
			return null;
		}

		VRage.Game.ModAPI.IMyInventoryItem VRage.Game.ModAPI.IMyInventory.FindItem(SerializableDefinitionId contentId)
		{
			MyPhysicalInventoryItem? myPhysicalInventoryItem = FindItem(contentId);
			if (myPhysicalInventoryItem.HasValue)
			{
				return myPhysicalInventoryItem.Value;
			}
			return null;
		}

		bool VRage.Game.ModAPI.IMyInventory.TransferItemFrom(VRage.Game.ModAPI.IMyInventory sourceInventory, VRage.Game.ModAPI.IMyInventoryItem item, MyFixedPoint amount)
		{
			if (sourceInventory == null || item == null)
			{
				return false;
			}
			int itemIndexById = GetItemIndexById(item.ItemId);
			if (itemIndexById < 0)
			{
				return false;
			}
			return TransferItemsFrom(sourceInventory, itemIndexById, null, null, amount, useConveyors: true);
		}

		private bool CanTransferTo(MyInventory dstInventory, MyDefinitionId? itemType)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = Owner as IMyConveyorEndpointBlock;
			IMyConveyorEndpointBlock myConveyorEndpointBlock2 = dstInventory.Owner as IMyConveyorEndpointBlock;
			if (myConveyorEndpointBlock == null || myConveyorEndpointBlock2 == null)
			{
				return false;
			}
			LRUCache<ConnectionKey, ConnectionData> connectionCache = m_connectionCache;
			if (connectionCache == null)
			{
				Interlocked.CompareExchange(ref m_connectionCache, new LRUCache<ConnectionKey, ConnectionData>(25), null);
				connectionCache = m_connectionCache;
			}
			int gameplayFrameCounter = MySession.Static.GameplayFrameCounter;
			ConnectionKey key = new ConnectionKey(myConveyorEndpointBlock2.ConveyorEndpoint.CubeBlock.EntityId, itemType);
			if (connectionCache.TryPeek(key, out ConnectionData value) && value.Frame == gameplayFrameCounter)
			{
				return value.HasConnection;
			}
			bool flag = MyGridConveyorSystem.ComputeCanTransfer(myConveyorEndpointBlock, myConveyorEndpointBlock2, itemType);
			connectionCache.Write(key, new ConnectionData
			{
				Frame = gameplayFrameCounter,
				HasConnection = flag
			});
			return flag;
		}

		private bool TransferItemsTo(VRage.Game.ModAPI.Ingame.IMyInventory dst, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount, bool useConveyor)
		{
			return (dst as MyInventory)?.TransferItemsFrom(this, sourceItemIndex, targetItemIndex, stackIfPossible, amount, useConveyor) ?? false;
		}

		private bool TransferItemsFrom(VRage.Game.ModAPI.Ingame.IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount, bool useConveyors)
		{
			if (amount.HasValue && amount.Value <= 0)
			{
				return true;
			}
			MyInventory myInventory = sourceInventory as MyInventory;
			if (myInventory != null && myInventory.IsItemAt(sourceItemIndex))
			{
				MyPhysicalInventoryItem myPhysicalInventoryItem = myInventory.m_items[sourceItemIndex];
				if (!useConveyors || myInventory.CanTransferTo(this, myPhysicalInventoryItem.Content.GetObjectId()))
				{
					Transfer(myInventory, this, myPhysicalInventoryItem.ItemId, targetItemIndex ?? (-1), amount);
					return true;
				}
			}
			return false;
		}

		bool VRage.Game.ModAPI.IMyInventory.CanAddItemAmount(VRage.Game.ModAPI.IMyInventoryItem item, MyFixedPoint amount)
		{
			return ItemsCanBeAdded(amount, item);
		}
	}
}
