using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Replication.StateGroups
{
	internal class MyEntityInventoryStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
	{
		private struct InventoryDeltaInformation
		{
			public bool HasChanges;

			public uint MessageId;

			public List<uint> RemovedItems;

			public Dictionary<uint, MyFixedPoint> ChangedItems;

			public SortedDictionary<int, MyPhysicalInventoryItem> NewItems;

			public Dictionary<uint, int> SwappedItems;
		}

		private struct ClientInvetoryData
		{
			public MyPhysicalInventoryItem Item;

			public MyFixedPoint Amount;
		}

		private class InventoryClientData
		{
			public uint CurrentMessageId;

			public InventoryDeltaInformation MainSendingInfo;

			public bool Dirty;

			public readonly Dictionary<byte, InventoryDeltaInformation> SendPackets = new Dictionary<byte, InventoryDeltaInformation>();

			public readonly List<InventoryDeltaInformation> FailedIncompletePackets = new List<InventoryDeltaInformation>();

			public readonly SortedDictionary<uint, ClientInvetoryData> ClientItemsSorted = new SortedDictionary<uint, ClientInvetoryData>();

			public readonly List<ClientInvetoryData> ClientItems = new List<ClientInvetoryData>();
		}

		private readonly int m_inventoryIndex;

		private Dictionary<Endpoint, InventoryClientData> m_clientInventoryUpdate;

		private List<MyPhysicalInventoryItem> m_itemsToSend;

		private HashSet<uint> m_foundDeltaItems;

		private uint m_nextExpectedPacketId;

		private readonly SortedList<uint, InventoryDeltaInformation> m_buffer;

		private Dictionary<int, MyPhysicalInventoryItem> m_tmpSwappingList;

		public bool IsHighPriority => false;

		private MyInventory Inventory
		{
			get;
			set;
		}

		public IMyReplicable Owner
		{
			get;
			private set;
		}

		public bool IsValid
		{
			get
			{
				if (Owner != null)
				{
					return Owner.IsValid;
				}
				return false;
			}
		}

		public bool IsStreaming => false;

		public bool NeedsUpdate => false;

		public MyEntityInventoryStateGroup(MyInventory entity, bool attach, IMyReplicable owner)
		{
			Inventory = entity;
			if (attach)
			{
				Inventory.ContentsChanged += InventoryChanged;
			}
			Owner = owner;
			if (!Sync.IsServer)
			{
				m_buffer = new SortedList<uint, InventoryDeltaInformation>();
			}
		}

		private void InventoryChanged(MyInventoryBase obj)
		{
			if (m_clientInventoryUpdate != null)
			{
				foreach (KeyValuePair<Endpoint, InventoryClientData> item in m_clientInventoryUpdate)
				{
					m_clientInventoryUpdate[item.Key].Dirty = true;
				}
				MyMultiplayer.GetReplicationServer().AddToDirtyGroups(this);
			}
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
			if (m_clientInventoryUpdate == null)
			{
				m_clientInventoryUpdate = new Dictionary<Endpoint, InventoryClientData>();
			}
			if (!m_clientInventoryUpdate.TryGetValue(forClient.EndpointId, out InventoryClientData value))
			{
				m_clientInventoryUpdate[forClient.EndpointId] = new InventoryClientData();
				value = m_clientInventoryUpdate[forClient.EndpointId];
			}
			value.Dirty = false;
			foreach (MyPhysicalInventoryItem item in Inventory.GetItems())
			{
				MyFixedPoint amount = item.Amount;
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null)
				{
					amount = (MyFixedPoint)myObjectBuilder_GasContainerObject.GasLevel;
				}
				ClientInvetoryData clientInvetoryData = default(ClientInvetoryData);
				clientInvetoryData.Item = item;
				clientInvetoryData.Amount = amount;
				ClientInvetoryData clientInvetoryData2 = clientInvetoryData;
				value.ClientItemsSorted[item.ItemId] = clientInvetoryData2;
				value.ClientItems.Add(clientInvetoryData2);
			}
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
			if (m_clientInventoryUpdate != null)
			{
				m_clientInventoryUpdate.Remove(forClient.EndpointId);
			}
		}

		public void ClientUpdate(MyTimeSpan clientTimestamp)
		{
		}

		public void Destroy()
		{
		}

		public float GetGroupPriority(int frameCountWithoutSync, MyClientInfo client)
		{
			InventoryClientData inventoryClientData = m_clientInventoryUpdate[client.EndpointId];
			if (!inventoryClientData.Dirty && inventoryClientData.FailedIncompletePackets.Count == 0)
			{
				return -1f;
			}
			if (inventoryClientData.FailedIncompletePackets.Count > 0)
			{
				return 1f * (float)frameCountWithoutSync;
			}
			MyClientState myClientState = (MyClientState)client.State;
			if (Inventory.Owner is MyCharacter)
			{
				MyCharacter myCharacter = Inventory.Owner as MyCharacter;
				MyPlayer myPlayer = MyPlayer.GetPlayerFromCharacter(myCharacter);
				if (myPlayer == null && myCharacter.IsUsing != null)
				{
					MyShipController myShipController = myCharacter.IsUsing as MyShipController;
					if (myShipController != null && myShipController.ControllerInfo.Controller != null)
					{
						myPlayer = myShipController.ControllerInfo.Controller.Player;
					}
				}
				if (myPlayer != null && myPlayer.Id.SteamId == client.EndpointId.Id.Value)
				{
					return 1f * (float)frameCountWithoutSync;
				}
			}
			if (myClientState.ContextEntity is MyCharacter && myClientState.ContextEntity == Inventory.Owner)
			{
				return 1f * (float)frameCountWithoutSync;
			}
			if (myClientState.Context == MyClientState.MyContextKind.Inventory || myClientState.Context == MyClientState.MyContextKind.Building || (myClientState.Context == MyClientState.MyContextKind.Production && Inventory.Owner is MyAssembler))
			{
				return GetPriorityStateGroup(client) * (float)frameCountWithoutSync;
			}
			return 0f;
		}

		private float GetPriorityStateGroup(MyClientInfo client)
		{
			MyClientState myClientState = (MyClientState)client.State;
			if (Inventory.ForcedPriority.HasValue)
			{
				return Inventory.ForcedPriority.Value;
			}
			if (myClientState.ContextEntity != null)
			{
				if (myClientState.ContextEntity == Inventory.Owner)
				{
					return 1f;
				}
				MyCubeGrid myCubeGrid = myClientState.ContextEntity.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid != null)
				{
					foreach (MyTerminalBlock block in myCubeGrid.GridSystems.TerminalSystem.Blocks)
					{
						if (block == Inventory.Container.Entity && (myClientState.Context != MyClientState.MyContextKind.Production || block is MyAssembler))
						{
							return 1f;
						}
					}
				}
			}
			return 0f;
		}

		public void Serialize(BitStream stream, Endpoint forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream.Writing)
			{
				InventoryClientData clientData = m_clientInventoryUpdate[forClient];
				bool needsSplit = false;
				if (clientData.FailedIncompletePackets.Count > 0)
				{
					InventoryDeltaInformation packetInfo = clientData.FailedIncompletePackets[0];
					clientData.FailedIncompletePackets.RemoveAtFast(0);
					InventoryDeltaInformation sentData = WriteInventory(ref packetInfo, stream, packetId, maxBitPosition, out needsSplit);
					sentData.MessageId = packetInfo.MessageId;
					if (needsSplit)
					{
						clientData.FailedIncompletePackets.Add(CreateSplit(ref packetInfo, ref sentData));
					}
					clientData.SendPackets[packetId] = sentData;
					return;
				}
				InventoryDeltaInformation packetInfo2 = CalculateInventoryDiff(ref clientData);
				packetInfo2.MessageId = clientData.CurrentMessageId;
				clientData.MainSendingInfo = WriteInventory(ref packetInfo2, stream, packetId, maxBitPosition, out needsSplit);
				clientData.SendPackets[packetId] = clientData.MainSendingInfo;
				clientData.CurrentMessageId++;
				if (needsSplit)
				{
					InventoryDeltaInformation item = CreateSplit(ref packetInfo2, ref clientData.MainSendingInfo);
					item.MessageId = clientData.CurrentMessageId;
					clientData.FailedIncompletePackets.Add(item);
					clientData.CurrentMessageId++;
				}
				clientData.Dirty = false;
			}
			else
			{
				ReadInventory(stream);
			}
		}

		private void ReadInventory(BitStream stream)
		{
			bool flag = stream.ReadBool();
			uint num = stream.ReadUInt32();
			bool flag2 = true;
			bool flag3 = false;
			InventoryDeltaInformation value = default(InventoryDeltaInformation);
			if (num == m_nextExpectedPacketId)
			{
				m_nextExpectedPacketId++;
				if (!flag)
				{
					FlushBuffer();
					return;
				}
			}
			else if (num > m_nextExpectedPacketId && !m_buffer.ContainsKey(num))
			{
				flag3 = true;
				value.MessageId = num;
			}
			else
			{
				flag2 = false;
			}
			if (!flag)
			{
				if (flag3)
				{
					m_buffer.Add(num, value);
				}
			}
			else
			{
				if (!flag)
				{
					return;
				}
				if (stream.ReadBool())
				{
					int num2 = stream.ReadInt32();
					for (int i = 0; i < num2; i++)
					{
						uint num3 = stream.ReadUInt32();
						MyFixedPoint myFixedPoint = default(MyFixedPoint);
						myFixedPoint.RawValue = stream.ReadInt64();
						if (!flag2)
						{
							continue;
						}
						if (flag3)
						{
							if (value.ChangedItems == null)
							{
								value.ChangedItems = new Dictionary<uint, MyFixedPoint>();
							}
							value.ChangedItems.Add(num3, myFixedPoint);
						}
						else
						{
							Inventory.UpdateItemAmoutClient(num3, myFixedPoint);
						}
					}
				}
				if (stream.ReadBool())
				{
					int num4 = stream.ReadInt32();
					for (int j = 0; j < num4; j++)
					{
						uint num5 = stream.ReadUInt32();
						if (!flag2)
						{
							continue;
						}
						if (flag3)
						{
							if (value.RemovedItems == null)
							{
								value.RemovedItems = new List<uint>();
							}
							value.RemovedItems.Add(num5);
						}
						else
						{
							Inventory.RemoveItemClient(num5);
						}
					}
				}
				if (stream.ReadBool())
				{
					int num6 = stream.ReadInt32();
					for (int k = 0; k < num6; k++)
					{
						int num7 = stream.ReadInt32();
						MySerializer.CreateAndRead(stream, out MyPhysicalInventoryItem value2, MyObjectBuilderSerializer.Dynamic);
						if (!flag2)
						{
							continue;
						}
						if (flag3)
						{
							if (value.NewItems == null)
							{
								value.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
							}
							value.NewItems.Add(num7, value2);
						}
						else
						{
							Inventory.AddItemClient(num7, value2);
						}
					}
				}
				if (stream.ReadBool())
				{
					if (m_tmpSwappingList == null)
					{
						m_tmpSwappingList = new Dictionary<int, MyPhysicalInventoryItem>();
					}
					int num8 = stream.ReadInt32();
					for (int l = 0; l < num8; l++)
					{
						uint num9 = stream.ReadUInt32();
						int num10 = stream.ReadInt32();
						if (!flag2)
						{
							continue;
						}
						if (flag3)
						{
							if (value.SwappedItems == null)
							{
								value.SwappedItems = new Dictionary<uint, int>();
							}
							value.SwappedItems.Add(num9, num10);
						}
						else
						{
							MyPhysicalInventoryItem? itemByID = Inventory.GetItemByID(num9);
							if (itemByID.HasValue)
							{
								m_tmpSwappingList.Add(num10, itemByID.Value);
							}
						}
					}
					foreach (KeyValuePair<int, MyPhysicalInventoryItem> tmpSwapping in m_tmpSwappingList)
					{
						Inventory.ChangeItemClient(tmpSwapping.Value, tmpSwapping.Key);
					}
					m_tmpSwappingList.Clear();
				}
				if (flag3)
				{
					m_buffer.Add(num, value);
				}
				else if (flag2)
				{
					FlushBuffer();
				}
				Inventory.Refresh();
			}
		}

		private void FlushBuffer()
		{
			while (m_buffer.Count > 0)
			{
				InventoryDeltaInformation changes = m_buffer.Values[0];
				if (changes.MessageId == m_nextExpectedPacketId)
				{
					m_nextExpectedPacketId++;
					ApplyChangesOnClient(changes);
					m_buffer.RemoveAt(0);
					continue;
				}
				break;
			}
		}

		private void ApplyChangesOnClient(InventoryDeltaInformation changes)
		{
			if (changes.ChangedItems != null)
			{
				foreach (KeyValuePair<uint, MyFixedPoint> changedItem in changes.ChangedItems)
				{
					Inventory.UpdateItemAmoutClient(changedItem.Key, changedItem.Value);
				}
			}
			if (changes.RemovedItems != null)
			{
				foreach (uint removedItem in changes.RemovedItems)
				{
					Inventory.RemoveItemClient(removedItem);
				}
			}
			if (changes.NewItems != null)
			{
				foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in changes.NewItems)
				{
					Inventory.AddItemClient(newItem.Key, newItem.Value);
				}
			}
			if (changes.SwappedItems != null)
			{
				if (m_tmpSwappingList == null)
				{
					m_tmpSwappingList = new Dictionary<int, MyPhysicalInventoryItem>();
				}
				foreach (KeyValuePair<uint, int> swappedItem in changes.SwappedItems)
				{
					MyPhysicalInventoryItem? itemByID = Inventory.GetItemByID(swappedItem.Key);
					if (itemByID.HasValue)
					{
						m_tmpSwappingList.Add(swappedItem.Value, itemByID.Value);
					}
				}
				foreach (KeyValuePair<int, MyPhysicalInventoryItem> tmpSwapping in m_tmpSwappingList)
				{
					Inventory.ChangeItemClient(tmpSwapping.Value, tmpSwapping.Key);
				}
				m_tmpSwappingList.Clear();
			}
		}

		private InventoryDeltaInformation CalculateInventoryDiff(ref InventoryClientData clientData)
		{
			if (m_itemsToSend == null)
			{
				m_itemsToSend = new List<MyPhysicalInventoryItem>();
			}
			if (m_foundDeltaItems == null)
			{
				m_foundDeltaItems = new HashSet<uint>();
			}
			m_foundDeltaItems.Clear();
			List<MyPhysicalInventoryItem> items = Inventory.GetItems();
			CalculateAddsAndRemovals(clientData, out InventoryDeltaInformation delta, items);
			if (delta.HasChanges)
			{
				ApplyChangesToClientItems(clientData, ref delta);
			}
			for (int i = 0; i < items.Count; i++)
			{
				if (i >= clientData.ClientItems.Count)
				{
					continue;
				}
				uint itemId = clientData.ClientItems[i].Item.ItemId;
				if (itemId == items[i].ItemId)
				{
					continue;
				}
				if (delta.SwappedItems == null)
				{
					delta.SwappedItems = new Dictionary<uint, int>();
				}
				for (int j = 0; j < items.Count; j++)
				{
					if (itemId == items[j].ItemId)
					{
						delta.SwappedItems[itemId] = j;
					}
				}
			}
			clientData.ClientItemsSorted.Clear();
			clientData.ClientItems.Clear();
			foreach (MyPhysicalInventoryItem item in items)
			{
				MyFixedPoint amount = item.Amount;
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null)
				{
					amount = (MyFixedPoint)myObjectBuilder_GasContainerObject.GasLevel;
				}
				ClientInvetoryData clientInvetoryData = default(ClientInvetoryData);
				clientInvetoryData.Item = item;
				clientInvetoryData.Amount = amount;
				ClientInvetoryData clientInvetoryData2 = clientInvetoryData;
				clientData.ClientItemsSorted[item.ItemId] = clientInvetoryData2;
				clientData.ClientItems.Add(clientInvetoryData2);
			}
			return delta;
		}

		private static void ApplyChangesToClientItems(InventoryClientData clientData, ref InventoryDeltaInformation delta)
		{
			if (delta.RemovedItems != null)
			{
				foreach (uint removedItem in delta.RemovedItems)
				{
					int num = -1;
					for (int i = 0; i < clientData.ClientItems.Count; i++)
					{
						if (clientData.ClientItems[i].Item.ItemId == removedItem)
						{
							num = i;
							break;
						}
					}
					if (num != -1)
					{
						clientData.ClientItems.RemoveAt(num);
					}
				}
			}
			if (delta.NewItems != null)
			{
				foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in delta.NewItems)
				{
					ClientInvetoryData clientInvetoryData = default(ClientInvetoryData);
					clientInvetoryData.Item = newItem.Value;
					clientInvetoryData.Amount = newItem.Value.Amount;
					ClientInvetoryData item = clientInvetoryData;
					if (newItem.Key >= clientData.ClientItems.Count)
					{
						clientData.ClientItems.Add(item);
					}
					else
					{
						clientData.ClientItems.Insert(newItem.Key, item);
					}
				}
			}
		}

		private void CalculateAddsAndRemovals(InventoryClientData clientData, out InventoryDeltaInformation delta, List<MyPhysicalInventoryItem> items)
		{
			delta = new InventoryDeltaInformation
			{
				HasChanges = false
			};
			int num = 0;
			foreach (MyPhysicalInventoryItem item in items)
			{
				if (clientData.ClientItemsSorted.TryGetValue(item.ItemId, out ClientInvetoryData value))
				{
					if (value.Item.Content.TypeId == item.Content.TypeId && value.Item.Content.SubtypeId == item.Content.SubtypeId)
					{
						m_foundDeltaItems.Add(item.ItemId);
						MyFixedPoint myFixedPoint = item.Amount;
						MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
						if (myObjectBuilder_GasContainerObject != null)
						{
							myFixedPoint = (MyFixedPoint)myObjectBuilder_GasContainerObject.GasLevel;
						}
						if (value.Amount != myFixedPoint)
						{
							MyFixedPoint value2 = myFixedPoint - value.Amount;
							if (delta.ChangedItems == null)
							{
								delta.ChangedItems = new Dictionary<uint, MyFixedPoint>();
							}
							delta.ChangedItems[item.ItemId] = value2;
							delta.HasChanges = true;
						}
					}
				}
				else
				{
					if (delta.NewItems == null)
					{
						delta.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
					}
					delta.NewItems[num] = item;
					delta.HasChanges = true;
				}
				num++;
			}
			foreach (KeyValuePair<uint, ClientInvetoryData> item2 in clientData.ClientItemsSorted)
			{
				if (delta.RemovedItems == null)
				{
					delta.RemovedItems = new List<uint>();
				}
				if (!m_foundDeltaItems.Contains(item2.Key))
				{
					delta.RemovedItems.Add(item2.Key);
					delta.HasChanges = true;
				}
			}
		}

		private InventoryDeltaInformation WriteInventory(ref InventoryDeltaInformation packetInfo, BitStream stream, byte packetId, int maxBitPosition, out bool needsSplit)
		{
			InventoryDeltaInformation result = PrepareSendData(ref packetInfo, stream, maxBitPosition, out needsSplit);
			result.MessageId = packetInfo.MessageId;
			stream.WriteBool(result.HasChanges);
			stream.WriteUInt32(result.MessageId);
			if (!result.HasChanges)
			{
				return result;
			}
			stream.WriteBool(result.ChangedItems != null);
			if (result.ChangedItems != null)
			{
				stream.WriteInt32(result.ChangedItems.Count);
				foreach (KeyValuePair<uint, MyFixedPoint> changedItem in result.ChangedItems)
				{
					stream.WriteUInt32(changedItem.Key);
					stream.WriteInt64(changedItem.Value.RawValue);
				}
			}
			stream.WriteBool(result.RemovedItems != null);
			if (result.RemovedItems != null)
			{
				stream.WriteInt32(result.RemovedItems.Count);
				foreach (uint removedItem in result.RemovedItems)
				{
					stream.WriteUInt32(removedItem);
				}
			}
			stream.WriteBool(result.NewItems != null);
			if (result.NewItems != null)
			{
				stream.WriteInt32(result.NewItems.Count);
				foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in result.NewItems)
				{
					stream.WriteInt32(newItem.Key);
					MyPhysicalInventoryItem value = newItem.Value;
					MySerializer.Write(stream, ref value, MyObjectBuilderSerializer.Dynamic);
				}
			}
			stream.WriteBool(result.SwappedItems != null);
			if (result.SwappedItems != null)
			{
				stream.WriteInt32(result.SwappedItems.Count);
				{
					foreach (KeyValuePair<uint, int> swappedItem in result.SwappedItems)
					{
						stream.WriteUInt32(swappedItem.Key);
						stream.WriteInt32(swappedItem.Value);
					}
					return result;
				}
			}
			return result;
		}

		private InventoryDeltaInformation PrepareSendData(ref InventoryDeltaInformation packetInfo, BitStream stream, int maxBitPosition, out bool needsSplit)
		{
			needsSplit = false;
			int bitPosition = stream.BitPosition;
			InventoryDeltaInformation inventoryDeltaInformation = default(InventoryDeltaInformation);
			inventoryDeltaInformation.HasChanges = false;
			InventoryDeltaInformation result = inventoryDeltaInformation;
			stream.WriteBool(value: false);
			stream.WriteUInt32(packetInfo.MessageId);
			stream.WriteBool(packetInfo.ChangedItems != null);
			if (packetInfo.ChangedItems != null)
			{
				stream.WriteInt32(packetInfo.ChangedItems.Count);
				if (stream.BitPosition > maxBitPosition)
				{
					needsSplit = true;
				}
				else
				{
					result.ChangedItems = new Dictionary<uint, MyFixedPoint>();
					foreach (KeyValuePair<uint, MyFixedPoint> changedItem in packetInfo.ChangedItems)
					{
						stream.WriteUInt32(changedItem.Key);
						stream.WriteInt64(changedItem.Value.RawValue);
						if (stream.BitPosition <= maxBitPosition)
						{
							result.ChangedItems[changedItem.Key] = changedItem.Value;
							result.HasChanges = true;
						}
						else
						{
							needsSplit = true;
						}
					}
				}
			}
			stream.WriteBool(packetInfo.RemovedItems != null);
			if (packetInfo.RemovedItems != null)
			{
				stream.WriteInt32(packetInfo.RemovedItems.Count);
				if (stream.BitPosition > maxBitPosition)
				{
					needsSplit = true;
				}
				else
				{
					result.RemovedItems = new List<uint>();
					foreach (uint removedItem in packetInfo.RemovedItems)
					{
						stream.WriteUInt32(removedItem);
						if (stream.BitPosition <= maxBitPosition)
						{
							result.RemovedItems.Add(removedItem);
							result.HasChanges = true;
						}
						else
						{
							needsSplit = true;
						}
					}
				}
			}
			stream.WriteBool(packetInfo.NewItems != null);
			if (packetInfo.NewItems != null)
			{
				stream.WriteInt32(packetInfo.NewItems.Count);
				if (stream.BitPosition > maxBitPosition)
				{
					needsSplit = true;
				}
				else
				{
					result.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
					foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in packetInfo.NewItems)
					{
						MyPhysicalInventoryItem value = newItem.Value;
						stream.WriteInt32(newItem.Key);
						int bitPosition2 = stream.BitPosition;
						MySerializer.Write(stream, ref value, MyObjectBuilderSerializer.Dynamic);
						_ = stream.BitPosition;
						if (stream.BitPosition <= maxBitPosition)
						{
							result.NewItems[newItem.Key] = value;
							result.HasChanges = true;
						}
						else
						{
							needsSplit = true;
						}
					}
				}
			}
			stream.WriteBool(packetInfo.SwappedItems != null);
			if (packetInfo.SwappedItems != null)
			{
				stream.WriteInt32(packetInfo.SwappedItems.Count);
				if (stream.BitPosition > maxBitPosition)
				{
					needsSplit = true;
				}
				else
				{
					result.SwappedItems = new Dictionary<uint, int>();
					foreach (KeyValuePair<uint, int> swappedItem in packetInfo.SwappedItems)
					{
						stream.WriteUInt32(swappedItem.Key);
						stream.WriteInt32(swappedItem.Value);
						if (stream.BitPosition <= maxBitPosition)
						{
							result.SwappedItems[swappedItem.Key] = swappedItem.Value;
							result.HasChanges = true;
						}
						else
						{
							needsSplit = true;
						}
					}
				}
			}
			stream.SetBitPositionWrite(bitPosition);
			return result;
		}

		private InventoryDeltaInformation CreateSplit(ref InventoryDeltaInformation originalData, ref InventoryDeltaInformation sentData)
		{
			InventoryDeltaInformation inventoryDeltaInformation = default(InventoryDeltaInformation);
			inventoryDeltaInformation.MessageId = sentData.MessageId;
			InventoryDeltaInformation result = inventoryDeltaInformation;
			if (originalData.ChangedItems != null)
			{
				if (sentData.ChangedItems == null)
				{
					result.ChangedItems = new Dictionary<uint, MyFixedPoint>();
					foreach (KeyValuePair<uint, MyFixedPoint> changedItem in originalData.ChangedItems)
					{
						result.ChangedItems[changedItem.Key] = changedItem.Value;
					}
				}
				else if (originalData.ChangedItems.Count != sentData.ChangedItems.Count)
				{
					result.ChangedItems = new Dictionary<uint, MyFixedPoint>();
					foreach (KeyValuePair<uint, MyFixedPoint> changedItem2 in originalData.ChangedItems)
					{
						if (!sentData.ChangedItems.ContainsKey(changedItem2.Key))
						{
							result.ChangedItems[changedItem2.Key] = changedItem2.Value;
						}
					}
				}
			}
			if (originalData.RemovedItems != null)
			{
				if (sentData.RemovedItems == null)
				{
					result.RemovedItems = new List<uint>();
					foreach (uint removedItem in originalData.RemovedItems)
					{
						result.RemovedItems.Add(removedItem);
					}
				}
				else if (originalData.RemovedItems.Count != sentData.RemovedItems.Count)
				{
					result.RemovedItems = new List<uint>();
					foreach (uint removedItem2 in originalData.RemovedItems)
					{
						if (!sentData.RemovedItems.Contains(removedItem2))
						{
							result.RemovedItems.Add(removedItem2);
						}
					}
				}
			}
			if (originalData.NewItems != null)
			{
				if (sentData.NewItems == null)
				{
					result.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
					foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in originalData.NewItems)
					{
						result.NewItems[newItem.Key] = newItem.Value;
					}
				}
				else if (originalData.NewItems.Count != sentData.NewItems.Count)
				{
					result.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
					foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem2 in originalData.NewItems)
					{
						if (!sentData.NewItems.ContainsKey(newItem2.Key))
						{
							result.NewItems[newItem2.Key] = newItem2.Value;
						}
					}
				}
			}
			if (originalData.SwappedItems != null)
			{
				if (sentData.SwappedItems == null)
				{
					result.SwappedItems = new Dictionary<uint, int>();
					{
						foreach (KeyValuePair<uint, int> swappedItem in originalData.SwappedItems)
						{
							result.SwappedItems[swappedItem.Key] = swappedItem.Value;
						}
						return result;
					}
				}
				if (originalData.SwappedItems.Count != sentData.SwappedItems.Count)
				{
					result.SwappedItems = new Dictionary<uint, int>();
					{
						foreach (KeyValuePair<uint, int> swappedItem2 in originalData.SwappedItems)
						{
							if (!sentData.SwappedItems.ContainsKey(swappedItem2.Key))
							{
								result.SwappedItems[swappedItem2.Key] = swappedItem2.Value;
							}
						}
						return result;
					}
				}
			}
			return result;
		}

		public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
		{
			if (m_clientInventoryUpdate.TryGetValue(forClient.EndpointId, out InventoryClientData value) && value.SendPackets.TryGetValue(packetId, out InventoryDeltaInformation value2))
			{
				if (!delivered)
				{
					value.FailedIncompletePackets.Add(value2);
					MyMultiplayer.GetReplicationServer().AddToDirtyGroups(this);
				}
				value.SendPackets.Remove(packetId);
			}
		}

		public void ForceSend(MyClientStateBase clientData)
		{
		}

		public void Reset(bool reinit, MyTimeSpan clientTimestamp)
		{
		}

		public bool IsStillDirty(Endpoint forClient)
		{
			if (m_clientInventoryUpdate.TryGetValue(forClient, out InventoryClientData value))
			{
				if (!value.Dirty)
				{
					return value.FailedIncompletePackets.Count != 0;
				}
				return true;
			}
			return true;
		}

		public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
		{
			return MyStreamProcessingState.None;
		}
	}
}
