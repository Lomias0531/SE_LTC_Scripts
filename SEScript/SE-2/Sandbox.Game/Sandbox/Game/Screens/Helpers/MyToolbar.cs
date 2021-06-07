using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyToolbar
	{
		public interface IMyToolbarExtension
		{
			void Update();

			void AddedToToolbar(MyToolbar toolbar);
		}

		public struct SlotArgs
		{
			public int? SlotNumber;
		}

		public struct IndexArgs
		{
			public int ItemIndex;
		}

		public struct PageChangeArgs
		{
			public int PageIndex;
		}

		public static readonly string[] ADD_ITEM_ICON = new string[1]
		{
			"Textures\\GUI\\Icons\\HUD 2017\\plus.png"
		};

		public const int DEF_SLOT_COUNT = 9;

		public const int DEF_PAGE_COUNT = 9;

		public const int DEF_SLOT_COUNT_GAMEPAD = 4;

		public int SlotCount;

		public int PageCount;

		private MyToolbarItem[] m_items;

		private List<MyToolbarItem> m_itemsGamepad;

		private CachingDictionary<Type, IMyToolbarExtension> m_extensions;

		private MyToolbarType m_toolbarType;

		private MyEntity m_owner;

		private bool? m_enabledOverride;

		private int? m_selectedSlot;

		private int? m_stagedSelectedSlot;

		private bool m_activateSelectedItem;

		private int m_currentPage;

		private int m_currentPageGamepad;

		private bool m_toolbarEdited;

		private bool m_toolbarEditedGamepad;

		public bool DrawNumbers = true;

		public Func<int, ColoredIcon> GetSymbol = (int x) => default(ColoredIcon);

		public int ItemCount => SlotCount * PageCount;

		public MyToolbarType ToolbarType
		{
			get
			{
				return m_toolbarType;
			}
			private set
			{
				m_toolbarType = value;
			}
		}

		public MyEntity Owner
		{
			get
			{
				return m_owner;
			}
			private set
			{
				m_owner = value;
			}
		}

		public bool ShowHolsterSlot
		{
			get
			{
				if (m_toolbarType != 0 && m_toolbarType != MyToolbarType.BuildCockpit)
				{
					return true;
				}
				return true;
			}
		}

		public int? SelectedSlot
		{
			get
			{
				return m_selectedSlot;
			}
			private set
			{
				if (m_selectedSlot != value)
				{
					m_selectedSlot = value;
				}
			}
		}

		public int? StagedSelectedSlot
		{
			get
			{
				return m_stagedSelectedSlot;
			}
			private set
			{
				m_stagedSelectedSlot = value;
				m_activateSelectedItem = false;
			}
		}

		public bool ShouldActivateSlot => m_activateSelectedItem;

		public int CurrentPage => m_currentPage;

		public int CurrentPageGamepad => m_currentPageGamepad;

		public int PageCountGamepad
		{
			get
			{
				if (m_itemsGamepad.Count % 4 != 0)
				{
					return m_itemsGamepad.Count / 4 + 1;
				}
				return m_itemsGamepad.Count / 4;
			}
		}

		public MyToolbarItem SelectedItem
		{
			get
			{
				if (!SelectedSlot.HasValue)
				{
					return null;
				}
				return GetSlotItem(SelectedSlot.Value);
			}
		}

		public MyToolbarItem this[int i] => m_items[i];

		public bool? EnabledOverride
		{
			get
			{
				return m_enabledOverride;
			}
			private set
			{
				if (value != m_enabledOverride)
				{
					m_enabledOverride = value;
					if (this.ItemEnabledChanged != null)
					{
						this.ItemEnabledChanged(this, default(SlotArgs));
					}
				}
			}
		}

		public event Action<MyToolbar, IndexArgs> ItemChanged;

		public event Action<MyToolbar, IndexArgs, MyToolbarItem.ChangeInfo> ItemUpdated;

		public event Action<MyToolbar, SlotArgs> SelectedSlotChanged;

		public event Action<MyToolbar, SlotArgs, bool> SlotActivated;

		public event Action<MyToolbar, SlotArgs> ItemEnabledChanged;

		public event Action<MyToolbar, PageChangeArgs> CurrentPageChanged;

		public event Action<MyToolbar, PageChangeArgs> CurrentPageChangedGamepad;

		public event Action<MyToolbar> Unselected;

		public int SlotToIndex(int i)
		{
			return SlotCount * m_currentPage + i;
		}

		public int IndexToSlot(int i)
		{
			if (i / SlotCount != m_currentPage)
			{
				return -1;
			}
			return MyMath.Mod(i, SlotCount);
		}

		public int SlotToIndexGamepad(int i)
		{
			return 4 * m_currentPageGamepad + i;
		}

		public int IndexToSlotGamepad(int i)
		{
			if (i / 4 != m_currentPageGamepad)
			{
				return -1;
			}
			return i % 4;
		}

		public MyToolbarItem GetSlotItem(int slot)
		{
			if (!IsValidSlot(slot))
			{
				return null;
			}
			int num = SlotToIndex(slot);
			if (!IsValidIndex(num))
			{
				return null;
			}
			return this[num];
		}

		public MyToolbarItem GetItemAtIndex(int index)
		{
			if (!IsValidIndex(index))
			{
				return null;
			}
			return this[index];
		}

		public MyToolbarItem GetItemAtSlot(int slot)
		{
			if (!IsValidSlot(slot) && !IsHolsterSlot(slot))
			{
				return null;
			}
			if (IsValidSlot(slot))
			{
				return m_items[SlotToIndex(slot)];
			}
			return null;
		}

		public int GetItemIndex(MyToolbarItem item)
		{
			for (int i = 0; i < m_items.Length; i++)
			{
				if (m_items[i] == item)
				{
					return i;
				}
			}
			return -1;
		}

		public MyToolbar(MyToolbarType type, int slotCount = 9, int pageCount = 9)
		{
			SlotCount = slotCount;
			PageCount = pageCount;
			m_items = new MyToolbarItem[SlotCount * PageCount];
			m_itemsGamepad = new List<MyToolbarItem>();
			m_toolbarType = type;
			Owner = null;
			SetDefaults();
		}

		public void Init(MyObjectBuilder_Toolbar builder, MyEntity owner, bool skipAssert = false)
		{
			Owner = owner;
			if (builder == null)
			{
				ClearGamepad(4);
				return;
			}
			if (builder.Slots != null)
			{
				Clear();
				foreach (MyObjectBuilder_Toolbar.Slot slot in builder.Slots)
				{
					SetItemAtSerialized(slot.Index, slot.Item, slot.Data);
				}
			}
			if (builder.SlotsGamepad != null)
			{
				ClearGamepad(builder.SlotsGamepad.Count);
				foreach (MyObjectBuilder_Toolbar.Slot item in builder.SlotsGamepad)
				{
					SetItemAtSerialized(item.Index, item.Item, item.Data, gamepad: true);
				}
			}
			StagedSelectedSlot = builder.SelectedSlot;
			MyCockpit myCockpit = Owner as MyCockpit;
			if (myCockpit != null && myCockpit.CubeGrid != null)
			{
				myCockpit.CubeGrid.OnFatBlockClosed += OnFatBlockClosed;
			}
		}

		public MyObjectBuilder_Toolbar GetObjectBuilder()
		{
			MyObjectBuilder_Toolbar myObjectBuilder_Toolbar = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Toolbar>();
			if (myObjectBuilder_Toolbar.Slots == null)
			{
				myObjectBuilder_Toolbar.Slots = new List<MyObjectBuilder_Toolbar.Slot>(m_items.Length);
			}
			if (myObjectBuilder_Toolbar.SlotsGamepad == null)
			{
				myObjectBuilder_Toolbar.SlotsGamepad = new List<MyObjectBuilder_Toolbar.Slot>(m_itemsGamepad.Count);
			}
			myObjectBuilder_Toolbar.SelectedSlot = SelectedSlot;
			myObjectBuilder_Toolbar.Slots.Clear();
			MyObjectBuilder_Toolbar.Slot item;
			for (int i = 0; i < m_items.Length; i++)
			{
				if (m_items[i] != null)
				{
					m_items[i].GetObjectBuilder();
					MyObjectBuilder_ToolbarItem objectBuilder = m_items[i].GetObjectBuilder();
					if (objectBuilder != null)
					{
						List<MyObjectBuilder_Toolbar.Slot> slots = myObjectBuilder_Toolbar.Slots;
						item = new MyObjectBuilder_Toolbar.Slot
						{
							Index = i,
							Item = "",
							Data = objectBuilder
						};
						slots.Add(item);
					}
				}
			}
			myObjectBuilder_Toolbar.SlotsGamepad.Clear();
			for (int j = 0; j < m_itemsGamepad.Count; j++)
			{
				if (m_itemsGamepad[j] != null)
				{
					m_itemsGamepad[j].GetObjectBuilder();
					MyObjectBuilder_ToolbarItem objectBuilder2 = m_itemsGamepad[j].GetObjectBuilder();
					if (objectBuilder2 != null)
					{
						List<MyObjectBuilder_Toolbar.Slot> slotsGamepad = myObjectBuilder_Toolbar.SlotsGamepad;
						item = new MyObjectBuilder_Toolbar.Slot
						{
							Index = j,
							Item = "",
							Data = objectBuilder2
						};
						slotsGamepad.Add(item);
					}
				}
			}
			return myObjectBuilder_Toolbar;
		}

		public void PageUp()
		{
			if (PageCount > 0)
			{
				m_currentPage = MyMath.Mod(m_currentPage + 1, PageCount);
				if (this.CurrentPageChanged != null)
				{
					this.CurrentPageChanged(this, new PageChangeArgs
					{
						PageIndex = m_currentPage
					});
				}
			}
		}

		public void PageDown()
		{
			if (PageCount > 0)
			{
				m_currentPage = MyMath.Mod(m_currentPage - 1, PageCount);
				if (this.CurrentPageChanged != null)
				{
					this.CurrentPageChanged(this, new PageChangeArgs
					{
						PageIndex = m_currentPage
					});
				}
			}
		}

		public bool IsLastGamepadPageEmpty()
		{
			if (m_itemsGamepad.Count % 4 != 0)
			{
				FillGamepadPages();
			}
			if (m_itemsGamepad.Count < 4)
			{
				return false;
			}
			for (int i = m_itemsGamepad.Count - 4; i < m_itemsGamepad.Count; i++)
			{
				if (m_itemsGamepad[i] != null)
				{
					return false;
				}
			}
			return true;
		}

		private void FillGamepadPagesToIndex(int idx)
		{
			if (idx >= m_itemsGamepad.Count)
			{
				int num = (idx / 4 + 1) * 4;
				while (m_itemsGamepad.Count < num)
				{
					m_itemsGamepad.Add(null);
				}
			}
		}

		private void FillGamepadPages()
		{
			if (m_itemsGamepad.Count % 4 != 0)
			{
				for (int i = 0; i < 4 - m_itemsGamepad.Count % 4; i++)
				{
					m_itemsGamepad.Add(null);
				}
			}
		}

		private void AddEmptyGamepadPage()
		{
			for (int i = 0; i < 4; i++)
			{
				m_itemsGamepad.Add(null);
			}
		}

		public void PageUpGamepad()
		{
			if (m_currentPageGamepad < PageCountGamepad - 1)
			{
				m_currentPageGamepad++;
			}
			else
			{
				if (IsLastGamepadPageEmpty())
				{
					return;
				}
				AddEmptyGamepadPage();
				m_currentPageGamepad = PageCountGamepad - 1;
			}
			if (this.CurrentPageChangedGamepad != null)
			{
				this.CurrentPageChangedGamepad(this, new PageChangeArgs
				{
					PageIndex = m_currentPageGamepad
				});
			}
		}

		public void PageDownGamepad()
		{
			if (PageCountGamepad > 1 && m_currentPageGamepad > 0)
			{
				m_currentPageGamepad--;
				if (this.CurrentPageChanged != null)
				{
					this.CurrentPageChanged(this, new PageChangeArgs
					{
						PageIndex = m_currentPage
					});
				}
			}
		}

		public void SwitchToPage(int page)
		{
			if (page >= 0 && page < PageCount && m_currentPage != page)
			{
				m_currentPage = page;
				if (this.CurrentPageChanged != null)
				{
					this.CurrentPageChanged(this, new PageChangeArgs
					{
						PageIndex = m_currentPage
					});
				}
			}
		}

		public void SetItemAtIndex(int i, MyDefinitionId defId, MyObjectBuilder_ToolbarItem data, bool gamepad = false)
		{
			if ((gamepad || m_items.IsValidIndex(i)) && MyDefinitionManager.Static.TryGetDefinition(defId, out MyDefinitionBase _))
			{
				SetItemAtIndex(i, MyToolbarItemFactory.CreateToolbarItem(data), gamepad);
			}
		}

		public void SetItemAtSlot(int slot, MyToolbarItem item)
		{
			SetItemAtIndex(SlotToIndex(slot), item);
		}

		public void SetItemAtIndex(int i, MyToolbarItem item, bool gamepad = false)
		{
			SetItemAtIndexInternal(i, item, initialization: false, gamepad);
		}

		private void SetItemAtIndexInternal(int i, MyToolbarItem item, bool initialization = false, bool gamepad = false)
		{
			IndexArgs arg;
			if (gamepad)
			{
				if (i >= m_itemsGamepad.Count)
				{
					FillGamepadPagesToIndex(i);
				}
				MyToolbarItemDefinition myToolbarItemDefinition = item as MyToolbarItemDefinition;
				if ((myToolbarItemDefinition != null && myToolbarItemDefinition.Definition != null && !myToolbarItemDefinition.Definition.AvailableInSurvival && MySession.Static.SurvivalMode) || (item != null && !item.AllowedInToolbarType(m_toolbarType)))
				{
					return;
				}
				if (m_itemsGamepad[i] != null)
				{
					_ = m_itemsGamepad[i].Enabled;
					m_itemsGamepad[i].OnRemovedFromToolbar(this);
				}
				m_itemsGamepad[i] = item;
				if (item != null)
				{
					m_toolbarEditedGamepad = true;
					item.OnAddedToToolbar(this);
					if (MyVisualScriptLogicProvider.ToolbarItemChanged != null)
					{
						MyObjectBuilder_ToolbarItem objectBuilder = item.GetObjectBuilder();
						string typeId = objectBuilder.TypeId.ToString();
						string subtypeId = objectBuilder.SubtypeId.ToString();
						MyObjectBuilder_ToolbarItemDefinition myObjectBuilder_ToolbarItemDefinition = objectBuilder as MyObjectBuilder_ToolbarItemDefinition;
						if (myObjectBuilder_ToolbarItemDefinition != null)
						{
							typeId = myObjectBuilder_ToolbarItemDefinition.DefinitionId.TypeId.ToString();
							subtypeId = myObjectBuilder_ToolbarItemDefinition.DefinitionId.SubtypeId;
						}
						MyVisualScriptLogicProvider.ToolbarItemChanged((Owner != null) ? Owner.EntityId : 0, typeId, subtypeId, m_currentPageGamepad, i % 4);
					}
				}
				if (!initialization)
				{
					UpdateItemGamepad(i);
					if (this.ItemChanged != null)
					{
						Action<MyToolbar, IndexArgs> itemChanged = this.ItemChanged;
						arg = new IndexArgs
						{
							ItemIndex = i
						};
						itemChanged(this, arg);
					}
				}
			}
			else
			{
				if (!m_items.IsValidIndex(i))
				{
					return;
				}
				MyToolbarItemDefinition myToolbarItemDefinition2 = item as MyToolbarItemDefinition;
				if ((myToolbarItemDefinition2 != null && myToolbarItemDefinition2.Definition != null && !myToolbarItemDefinition2.Definition.AvailableInSurvival && MySession.Static.SurvivalMode) || (item != null && !item.AllowedInToolbarType(m_toolbarType)))
				{
					return;
				}
				bool flag = true;
				bool flag2 = true;
				if (m_items[i] != null)
				{
					flag = m_items[i].Enabled;
					m_items[i].OnRemovedFromToolbar(this);
				}
				m_items[i] = item;
				if (item != null)
				{
					m_toolbarEdited = true;
					item.OnAddedToToolbar(this);
					flag2 = true;
					if (MyVisualScriptLogicProvider.ToolbarItemChanged != null)
					{
						MyObjectBuilder_ToolbarItem objectBuilder2 = item.GetObjectBuilder();
						string typeId2 = objectBuilder2.TypeId.ToString();
						string subtypeId2 = objectBuilder2.SubtypeId.ToString();
						MyObjectBuilder_ToolbarItemDefinition myObjectBuilder_ToolbarItemDefinition2 = objectBuilder2 as MyObjectBuilder_ToolbarItemDefinition;
						if (myObjectBuilder_ToolbarItemDefinition2 != null)
						{
							typeId2 = myObjectBuilder_ToolbarItemDefinition2.DefinitionId.TypeId.ToString();
							subtypeId2 = myObjectBuilder_ToolbarItemDefinition2.DefinitionId.SubtypeId;
						}
						MyVisualScriptLogicProvider.ToolbarItemChanged((Owner != null) ? Owner.EntityId : 0, typeId2, subtypeId2, m_currentPage, MyMath.Mod(i, SlotCount));
					}
				}
				if (initialization)
				{
					return;
				}
				UpdateItem(i);
				if (this.ItemChanged != null)
				{
					Action<MyToolbar, IndexArgs> itemChanged2 = this.ItemChanged;
					arg = new IndexArgs
					{
						ItemIndex = i
					};
					itemChanged2(this, arg);
				}
				if (flag != flag2)
				{
					int num = IndexToSlot(i);
					if (IsValidSlot(num))
					{
						SlotEnabledChanged(num);
					}
				}
			}
		}

		public void AddExtension(IMyToolbarExtension newExtension)
		{
			if (m_extensions == null)
			{
				m_extensions = new CachingDictionary<Type, IMyToolbarExtension>();
			}
			m_extensions.Add(newExtension.GetType(), newExtension);
			newExtension.AddedToToolbar(this);
		}

		public bool TryGetExtension<T>(out T extension) where T : class, IMyToolbarExtension
		{
			extension = null;
			if (m_extensions == null)
			{
				return false;
			}
			IMyToolbarExtension value = null;
			if (m_extensions.TryGetValue(typeof(T), out value))
			{
				extension = (value as T);
			}
			return extension != null;
		}

		public void RemoveExtension(IMyToolbarExtension toRemove)
		{
			m_extensions.Remove(toRemove.GetType());
		}

		private void ToolbarItemUpdated(int index, MyToolbarItem.ChangeInfo changed)
		{
			if (m_items.IsValidIndex(index) && this.ItemUpdated != null)
			{
				this.ItemUpdated(this, new IndexArgs
				{
					ItemIndex = index
				}, changed);
			}
		}

		private void ToolbarItem_EnabledChanged(MyToolbarItem obj)
		{
			if (EnabledOverride.HasValue)
			{
				return;
			}
			int num = Array.IndexOf(m_items, obj);
			if (this.ItemEnabledChanged != null && num != -1)
			{
				int num2 = IndexToSlot(num);
				if (IsValidSlot(num2))
				{
					this.ItemEnabledChanged(this, new SlotArgs
					{
						SlotNumber = num2
					});
				}
			}
		}

		private void SlotEnabledChanged(int slotIndex)
		{
			if (!EnabledOverride.HasValue && this.ItemEnabledChanged != null)
			{
				this.ItemEnabledChanged(this, new SlotArgs
				{
					SlotNumber = slotIndex
				});
			}
		}

		public void CharacterInventory_OnContentsChanged(MyInventoryBase inventory)
		{
			Update();
		}

		private void OnFatBlockClosed(MyCubeBlock block)
		{
			if (Owner != null && Owner.EntityId == block.EntityId)
			{
				for (int i = 0; i < m_items.Length; i++)
				{
					if (m_items[i] != null)
					{
						m_items[i].OnRemovedFromToolbar(this);
						m_items[i] = null;
					}
				}
				return;
			}
			for (int j = 0; j < m_items.Length; j++)
			{
				if (m_items[j] != null && m_items[j] is IMyToolbarItemEntity && ((IMyToolbarItemEntity)m_items[j]).CompareEntityIds(block.EntityId))
				{
					m_items[j].SetEnabled(newEnabled: false);
				}
			}
		}

		public void SetDefaults(bool sendEvent = true)
		{
			if (m_toolbarType == MyToolbarType.Character)
			{
				MyDefinitionId defId = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");
				MyDefinitionId defId2 = new MyDefinitionId(typeof(MyObjectBuilder_Cockpit), "LargeBlockCockpit");
				MyDefinitionId defId3 = new MyDefinitionId(typeof(MyObjectBuilder_Reactor), "LargeBlockSmallGenerator");
				MyDefinitionId defId4 = new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "LargeBlockSmallThrust");
				MyDefinitionId defId5 = new MyDefinitionId(typeof(MyObjectBuilder_Gyro), "LargeBlockGyro");
				int num = 0;
				if (MyDefinitionManager.Static.TryGetDefinition(defId, out MyDefinitionBase definition))
				{
					SetItemAtIndex(num++, defId, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition));
				}
				if (MyDefinitionManager.Static.TryGetDefinition(defId2, out MyDefinitionBase definition2))
				{
					SetItemAtIndex(num++, defId, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition2));
				}
				if (MyDefinitionManager.Static.TryGetDefinition(defId3, out MyDefinitionBase definition3))
				{
					SetItemAtIndex(num++, defId, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition3));
				}
				if (MyDefinitionManager.Static.TryGetDefinition(defId4, out MyDefinitionBase definition4))
				{
					SetItemAtIndex(num++, defId, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition4));
				}
				if (MyDefinitionManager.Static.TryGetDefinition(defId5, out MyDefinitionBase definition5))
				{
					SetItemAtIndex(num++, defId, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition5));
				}
				for (int i = num; i < m_items.Length; i++)
				{
					SetItemAtIndex(i, null);
				}
			}
		}

		public void Clear()
		{
			for (int i = 0; i < m_items.Length; i++)
			{
				SetItemAtIndex(i, null);
			}
		}

		public void ClearGamepad(int size = 0)
		{
			m_itemsGamepad = new List<MyToolbarItem>(size);
			for (int i = 0; i < size; i++)
			{
				m_itemsGamepad.Add(null);
			}
		}

		public void ActivateItemAtSlot(int slot, bool checkIfWantsToBeActivated = false, bool playActivationSound = true, bool userActivated = true)
		{
			if (!IsValidSlot(slot) && !IsHolsterSlot(slot))
			{
				return;
			}
			if (IsValidSlot(slot))
			{
				if (ActivateItemAtIndex(SlotToIndex(slot), checkIfWantsToBeActivated))
				{
					if (playActivationSound)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
					}
					if (this.SlotActivated != null)
					{
						this.SlotActivated(this, new SlotArgs
						{
							SlotNumber = slot
						}, userActivated);
					}
				}
			}
			else
			{
				Unselect();
			}
		}

		public void SelectNextSlot()
		{
			if (m_selectedSlot.HasValue && IsValidSlot(m_selectedSlot.Value))
			{
				if (MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode == MyCubeSize.Large && MyCubeBuilder.Static.CubeBuilderState.HasComplementBlock())
				{
					ActivateItemAtSlot(m_selectedSlot.Value);
					return;
				}
				MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(MyCubeSize.Large);
			}
			int nextValidSlotWithItem = GetNextValidSlotWithItem(m_selectedSlot.HasValue ? m_selectedSlot.Value : (-1));
			if (nextValidSlotWithItem != -1)
			{
				ActivateItemAtSlot(nextValidSlotWithItem);
			}
			else
			{
				Unselect();
			}
		}

		public void SelectPreviousSlot()
		{
			if (m_selectedSlot.HasValue && IsValidSlot(m_selectedSlot.Value))
			{
				if (MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode == MyCubeSize.Large && MyCubeBuilder.Static.CubeBuilderState.HasComplementBlock())
				{
					ActivateItemAtSlot(m_selectedSlot.Value);
					return;
				}
				MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(MyCubeSize.Large);
			}
			int previousValidSlotWithItem = GetPreviousValidSlotWithItem(m_selectedSlot.HasValue ? m_selectedSlot.Value : SlotCount);
			if (previousValidSlotWithItem != -1)
			{
				ActivateItemAtSlot(previousValidSlotWithItem);
			}
			else
			{
				Unselect();
			}
		}

		public int GetNextValidSlotWithItem(int startSlot)
		{
			for (int i = startSlot + 1; i != SlotCount; i++)
			{
				if (m_items[SlotToIndex(i)] != null)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetPreviousValidSlotWithItem(int startSlot)
		{
			for (int num = startSlot - 1; num >= 0; num--)
			{
				if (m_items[SlotToIndex(num)] != null)
				{
					return num;
				}
			}
			return -1;
		}

		public int GetNextValidSlot(int startSlot)
		{
			int num = startSlot + 1;
			if (IsHolsterSlot(num))
			{
				return SlotCount;
			}
			return num;
		}

		public int GetPreviousValidSlot(int startSlot)
		{
			int num = startSlot - 1;
			if (num < 0)
			{
				return SlotCount;
			}
			return num;
		}

		public void ActivateStagedSelectedItem()
		{
			ActivateItemAtSlot(m_stagedSelectedSlot.Value);
		}

		public bool ActivateGamepadItemAtIndex(int index, bool checkIfWantsToBeActivated = false)
		{
			if (index >= m_itemsGamepad.Count)
			{
				return false;
			}
			MyToolbarItem myToolbarItem = m_itemsGamepad[index];
			if (StagedSelectedSlot.HasValue && SlotToIndex(StagedSelectedSlot.Value) != index)
			{
				StagedSelectedSlot = null;
			}
			if (myToolbarItem != null && myToolbarItem.Enabled)
			{
				if (checkIfWantsToBeActivated && !myToolbarItem.WantsToBeActivated)
				{
					return false;
				}
				if ((myToolbarItem.WantsToBeActivated || MyCubeBuilder.Static.IsActivated) && SelectedItem != myToolbarItem)
				{
					Unselect(unselectSound: false);
				}
				return myToolbarItem.Activate();
			}
			return false;
		}

		public void ShareToolbarItems()
		{
			if (m_toolbarEdited != m_toolbarEditedGamepad)
			{
				if (m_toolbarEdited)
				{
					ToolbarShareNormalToGamepad();
				}
				else
				{
					ToolbarShareGamepadToNormal();
				}
			}
		}

		private void ToolbarShareNormalToGamepad()
		{
			int num = 0;
			bool flag = true;
			for (int i = 0; i < m_items.Length; i++)
			{
				if (m_items[i] == null)
				{
					if (!flag)
					{
						if (num % 4 == 0)
						{
							flag = true;
							continue;
						}
						num += 4 - num % 4;
						flag = true;
					}
				}
				else
				{
					SetItemAtIndex(num, MyToolbarItemFactory.CreateToolbarItem(m_items[i].GetObjectBuilder()), gamepad: true);
					num++;
					flag = ((num % 4 == 0) ? true : false);
				}
			}
		}

		private void ToolbarShareGamepadToNormal()
		{
			int num = 0;
			bool flag = true;
			for (int i = 0; i < m_itemsGamepad.Count; i += 4)
			{
				if (num >= 9)
				{
					break;
				}
				if (flag)
				{
					for (int j = 0; j < 4; j++)
					{
						if (i + j < m_itemsGamepad.Count && m_itemsGamepad[i + j] != null)
						{
							SetItemAtIndex(num * 9 + j, MyToolbarItemFactory.CreateToolbarItem(m_itemsGamepad[i + j].GetObjectBuilder()));
						}
					}
				}
				else
				{
					for (int k = 0; k < 4; k++)
					{
						if (i + k < m_itemsGamepad.Count && m_itemsGamepad[i + k] != null)
						{
							SetItemAtIndex(num * 9 + k + 5, MyToolbarItemFactory.CreateToolbarItem(m_itemsGamepad[i + k].GetObjectBuilder()));
						}
					}
					num++;
				}
				flag = !flag;
			}
		}

		public bool ActivateItemAtIndex(int index, bool checkIfWantsToBeActivated = false)
		{
			MyToolbarItem myToolbarItem = m_items[index];
			if (StagedSelectedSlot.HasValue && SlotToIndex(StagedSelectedSlot.Value) != index)
			{
				StagedSelectedSlot = null;
			}
			if (myToolbarItem != null && myToolbarItem.Enabled)
			{
				if (checkIfWantsToBeActivated && !myToolbarItem.WantsToBeActivated)
				{
					return false;
				}
				if ((myToolbarItem.WantsToBeActivated || MyCubeBuilder.Static.IsActivated) && SelectedItem != myToolbarItem)
				{
					Unselect(unselectSound: false);
				}
				return myToolbarItem.Activate();
			}
			if (myToolbarItem == null)
			{
				Unselect();
			}
			return false;
		}

		public void Unselect(bool unselectSound = true)
		{
			if (MyToolbarComponent.CurrentToolbar == this)
			{
				if (SelectedItem != null && unselectSound)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				}
				if (unselectSound)
				{
					MySession.Static.GameFocusManager.Clear();
				}
				MySession.Static.ControlledEntity?.SwitchToWeapon(null);
				if (this.Unselected != null)
				{
					this.Unselected(this);
				}
			}
		}

		public bool IsValidIndex(int idx)
		{
			return m_items.IsValidIndex(idx);
		}

		public bool IsValidSlot(int slot)
		{
			if (slot >= 0)
			{
				return slot < SlotCount;
			}
			return false;
		}

		public bool IsEnabled(int idx)
		{
			if (EnabledOverride.HasValue)
			{
				return EnabledOverride.Value;
			}
			if (idx == SlotCount && ShowHolsterSlot)
			{
				return true;
			}
			if (!IsValidIndex(idx))
			{
				return false;
			}
			if (m_items[idx] != null)
			{
				return m_items[idx].Enabled;
			}
			return true;
		}

		public string[] GetItemIcons(int idx)
		{
			if (!IsValidIndex(idx))
			{
				return null;
			}
			if (m_items[idx] != null)
			{
				return m_items[idx].Icons;
			}
			return null;
		}

		public string[] GetItemIconsGamepad(int idx)
		{
			if (idx < 0 || idx >= 4)
			{
				return null;
			}
			int num = CurrentPageGamepad * 4 + idx;
			if (num >= m_itemsGamepad.Count)
			{
				return null;
			}
			if (m_itemsGamepad[num] == null)
			{
				return ADD_ITEM_ICON;
			}
			return m_itemsGamepad[num].Icons;
		}

		public Vector4 GetItemIconsColormaskGamepad(int idx)
		{
			if (idx < 0 || idx >= 4)
			{
				return Vector4.Zero;
			}
			int num = CurrentPageGamepad * 4 + idx;
			if (num >= m_itemsGamepad.Count)
			{
				return Vector4.Zero;
			}
			if (m_itemsGamepad[num] == null)
			{
				return new Vector4(0.5f);
			}
			if (!m_itemsGamepad[num].Enabled)
			{
				return new Vector4(0.8f);
			}
			return Vector4.One;
		}

		public string GetItemSubiconGamepad(int idx)
		{
			if (idx < 0 || idx >= 4)
			{
				return null;
			}
			int num = CurrentPageGamepad * 4 + idx;
			if (num >= m_itemsGamepad.Count)
			{
				return null;
			}
			if (m_itemsGamepad[num] == null)
			{
				return null;
			}
			return m_itemsGamepad[num].SubIcon;
		}

		public string GetItemNameGamepad(int idx)
		{
			if (idx < 0 || idx >= 4)
			{
				return null;
			}
			int num = CurrentPageGamepad * 4 + idx;
			if (num >= m_itemsGamepad.Count)
			{
				return null;
			}
			if (m_itemsGamepad[num] == null)
			{
				return null;
			}
			return m_itemsGamepad[num].DisplayName.ToString();
		}

		public MyToolbarItem GetItemAtIndexGamepad(int idx)
		{
			if (idx < 0 || idx >= 4)
			{
				return null;
			}
			int num = CurrentPageGamepad * 4 + idx;
			if (num >= m_itemsGamepad.Count)
			{
				return null;
			}
			if (m_itemsGamepad[num] == null)
			{
				return null;
			}
			return m_itemsGamepad[num];
		}

		public string GetItemAction(int idx)
		{
			if (idx < 0 || idx >= 4)
			{
				return null;
			}
			int num = CurrentPageGamepad * 4 + idx;
			if (num >= m_itemsGamepad.Count)
			{
				return null;
			}
			if (m_itemsGamepad[num] == null)
			{
				return null;
			}
			return m_itemsGamepad[num].IconText.ToString();
		}

		public long GetControllerPlayerID()
		{
			MyCockpit myCockpit = Owner as MyCockpit;
			if (myCockpit != null)
			{
				MyEntityController controller = myCockpit.ControllerInfo.Controller;
				if (controller != null)
				{
					return controller.Player.Identity.IdentityId;
				}
			}
			IMyComponentOwner<MyIDModule> myComponentOwner = Owner as IMyComponentOwner<MyIDModule>;
			if (myComponentOwner != null && myComponentOwner.GetComponent(out MyIDModule component))
			{
				return component.Owner;
			}
			return 0L;
		}

		public void Update()
		{
			if (MySession.Static == null)
			{
				return;
			}
			long controllerPlayerID = GetControllerPlayerID();
			for (int i = 0; i < m_items.Length; i++)
			{
				if (m_items[i] != null)
				{
					MyToolbarItem.ChangeInfo changeInfo = m_items[i].Update(Owner, controllerPlayerID);
					if (changeInfo != 0)
					{
						ToolbarItemUpdated(i, changeInfo);
					}
				}
			}
			for (int j = 0; j < m_itemsGamepad.Count; j++)
			{
				if (m_itemsGamepad[j] != null)
				{
					m_itemsGamepad[j].Update(Owner, controllerPlayerID);
				}
			}
			int? selectedSlot = m_selectedSlot;
			if (!StagedSelectedSlot.HasValue)
			{
				m_selectedSlot = null;
				for (int k = 0; k < SlotCount; k++)
				{
					if (m_items[SlotToIndex(k)] != null && m_items[SlotToIndex(k)].WantsToBeSelected)
					{
						m_selectedSlot = k;
					}
				}
			}
			else if (!m_selectedSlot.HasValue || m_selectedSlot.Value != StagedSelectedSlot.Value)
			{
				m_selectedSlot = StagedSelectedSlot;
				MyToolbarItem myToolbarItem = m_items[SlotToIndex(m_selectedSlot.Value)];
				if (myToolbarItem != null && !myToolbarItem.ActivateOnClick)
				{
					ActivateItemAtSlot(m_selectedSlot.Value);
					m_activateSelectedItem = false;
				}
				else
				{
					m_activateSelectedItem = true;
					Unselect();
				}
			}
			if (selectedSlot != m_selectedSlot && this.SelectedSlotChanged != null)
			{
				this.SelectedSlotChanged(this, new SlotArgs
				{
					SlotNumber = m_selectedSlot
				});
			}
			EnabledOverride = null;
			if (m_extensions != null)
			{
				foreach (IMyToolbarExtension value in m_extensions.Values)
				{
					value.Update();
				}
				m_extensions.ApplyChanges();
			}
		}

		public void UpdateItem(int index)
		{
			if (MySession.Static != null && m_items[index] != null)
			{
				m_items[index].Update(Owner, GetControllerPlayerID());
			}
		}

		public void UpdateItemGamepad(int index)
		{
			if (MySession.Static != null && m_itemsGamepad[index] != null)
			{
				m_itemsGamepad[index].Update(Owner, GetControllerPlayerID());
			}
		}

		private void SetItemAtSerialized(int i, string serializedItem, MyObjectBuilder_ToolbarItem data, bool gamepad = false)
		{
			if (gamepad)
			{
				if (data != null)
				{
					SetItemAtIndexInternal(i, MyToolbarItemFactory.CreateToolbarItem(data), initialization: true, gamepad);
				}
			}
			else
			{
				if (!m_items.IsValidIndex(i))
				{
					return;
				}
				if (data == null)
				{
					if (!string.IsNullOrEmpty(serializedItem))
					{
						string[] array = serializedItem.Split(new char[1]
						{
							':'
						});
						if (MyObjectBuilderType.TryParse(array[0], out MyObjectBuilderType result))
						{
							string subtypeName = (array.Length == 2) ? array[1] : null;
							MyDefinitionId defId = new MyDefinitionId(result, subtypeName);
							SetItemAtSerializedCompat(i, defId);
						}
					}
				}
				else
				{
					SetItemAtIndexInternal(i, MyToolbarItemFactory.CreateToolbarItem(data), initialization: true);
				}
			}
		}

		public void SetItemAtSerializedCompat(int i, MyDefinitionId defId)
		{
			if (m_items.IsValidIndex(i) && MyDefinitionManager.Static.TryGetDefinition(defId, out MyDefinitionBase definition))
			{
				MyObjectBuilder_ToolbarItem data = MyToolbarItemFactory.ObjectBuilderFromDefinition(definition);
				SetItemAtIndexInternal(i, MyToolbarItemFactory.CreateToolbarItem(data), initialization: true);
			}
		}

		private bool IsHolsterSlot(int idx)
		{
			if (idx == SlotCount)
			{
				return ShowHolsterSlot;
			}
			return false;
		}
	}
}
