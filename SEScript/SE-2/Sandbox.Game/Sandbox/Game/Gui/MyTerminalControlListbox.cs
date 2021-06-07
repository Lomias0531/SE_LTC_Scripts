using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlListbox<TBlock> : MyTerminalControl<TBlock>, ITerminalControlSync, IMyTerminalControlTitleTooltip, IMyTerminalControlListbox, IMyTerminalControl where TBlock : MyTerminalBlock
	{
		public delegate void ListContentDelegate(TBlock block, ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems);

		public delegate void SelectItemDelegate(TBlock block, List<MyGuiControlListbox.Item> items);

		public MyStringId Title;

		public MyStringId Tooltip;

		public ListContentDelegate ListContent;

		public SelectItemDelegate ItemSelected;

		public SelectItemDelegate ItemDoubleClicked;

		private MyGuiControlListbox m_listbox;

		private bool m_enableMultiSelect;

		private int m_visibleRowsCount = 8;

		private bool m_keepScrolling = true;

		private bool KeepScrolling
		{
			get
			{
				return m_keepScrolling;
			}
			set
			{
				m_keepScrolling = value;
			}
		}

		MyStringId IMyTerminalControlTitleTooltip.Title
		{
			get
			{
				return Title;
			}
			set
			{
				Title = value;
			}
		}

		MyStringId IMyTerminalControlTitleTooltip.Tooltip
		{
			get
			{
				return Tooltip;
			}
			set
			{
				Tooltip = value;
			}
		}

		bool IMyTerminalControlListbox.Multiselect
		{
			get
			{
				return m_enableMultiSelect;
			}
			set
			{
				m_enableMultiSelect = value;
			}
		}

		int IMyTerminalControlListbox.VisibleRowsCount
		{
			get
			{
				return m_visibleRowsCount;
			}
			set
			{
				m_visibleRowsCount = value;
			}
		}

		Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>, List<MyTerminalControlListBoxItem>> IMyTerminalControlListbox.ListContent
		{
			set
			{
				ListContent = delegate(TBlock block, ICollection<MyGuiControlListbox.Item> contentList, ICollection<MyGuiControlListbox.Item> selectedList)
				{
					List<MyTerminalControlListBoxItem> list = new List<MyTerminalControlListBoxItem>();
					List<MyTerminalControlListBoxItem> list2 = new List<MyTerminalControlListBoxItem>();
					value(block, list, list2);
					foreach (MyTerminalControlListBoxItem item2 in list)
					{
						MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.Text.ToString()), item2.Tooltip.ToString(), null, item2.UserData);
						contentList.Add(item);
						if (list2.Contains(item2))
						{
							selectedList.Add(item);
						}
					}
				};
			}
		}

		Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>> IMyTerminalControlListbox.ItemSelected
		{
			set
			{
				ItemSelected = delegate(TBlock block, List<MyGuiControlListbox.Item> selectedList)
				{
					List<MyTerminalControlListBoxItem> list = new List<MyTerminalControlListBoxItem>();
					foreach (MyGuiControlListbox.Item selected in selectedList)
					{
						string str = (selected.ToolTip != null && selected.ToolTip.ToolTips.Count > 0) ? selected.ToolTip.ToolTips.First().ToString() : null;
						MyTerminalControlListBoxItem item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute(selected.Text.ToString()), MyStringId.GetOrCompute(str), selected.UserData);
						list.Add(item);
					}
					value(block, list);
				};
			}
		}

		public MyTerminalControlListbox(string id, MyStringId title, MyStringId tooltip, bool multiSelect = false, int visibleRowsCount = 8)
			: base(id)
		{
			Title = title;
			Tooltip = tooltip;
			m_enableMultiSelect = multiSelect;
			m_visibleRowsCount = visibleRowsCount;
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_listbox = new MyGuiControlListbox
			{
				VisualStyle = MyGuiControlListboxStyleEnum.Terminal,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				VisibleRowsCount = m_visibleRowsCount,
				MultiSelect = m_enableMultiSelect
			};
			m_listbox.ItemsSelected += OnItemsSelected;
			m_listbox.ItemDoubleClicked += OnItemDoubleClicked;
			return new MyGuiControlBlockProperty(MyTexts.GetString(Title), MyTexts.GetString(Tooltip), m_listbox);
		}

		private void OnItemsSelected(MyGuiControlListbox obj)
		{
			if (ItemSelected != null && obj.SelectedItems.Count > 0)
			{
				foreach (TBlock targetBlock in base.TargetBlocks)
				{
					ItemSelected(targetBlock, obj.SelectedItems);
				}
			}
		}

		private void OnItemDoubleClicked(MyGuiControlListbox obj)
		{
			if (ItemDoubleClicked != null && obj.SelectedItems.Count > 0)
			{
				foreach (TBlock targetBlock in base.TargetBlocks)
				{
					ItemDoubleClicked(targetBlock, obj.SelectedItems);
				}
			}
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock != null)
			{
				float scrollPosition = m_listbox.GetScrollPosition();
				m_listbox.Items.Clear();
				m_listbox.SelectedItems.Clear();
				if (ListContent != null)
				{
					ListContent(firstBlock, m_listbox.Items, m_listbox.SelectedItems);
				}
				if (scrollPosition <= (float)(m_listbox.Items.Count - m_listbox.VisibleRowsCount) + 1f)
				{
					m_listbox.SetScrollPosition(scrollPosition);
				}
				else
				{
					m_listbox.SetScrollPosition(0f);
				}
			}
		}

		public void Serialize(BitStream stream, MyTerminalBlock block)
		{
		}
	}
}
