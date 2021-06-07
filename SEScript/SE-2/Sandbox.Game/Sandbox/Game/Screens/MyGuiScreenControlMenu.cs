using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenControlMenu : MyGuiScreenBase
	{
		private enum ItemUpdateType
		{
			Activate,
			Next,
			Previous
		}

		private class MyGuiControlItem : MyGuiControlParent
		{
			private MyAbstractControlMenuItem m_item;

			private MyGuiControlLabel m_label;

			private MyGuiControlLabel m_value;

			public bool IsItemEnabled => m_item.Enabled;

			public MyGuiControlItem(MyAbstractControlMenuItem item, Vector2? size = null)
				: base(null, size)
			{
				m_item = item;
				m_item.UpdateValue();
				m_label = new MyGuiControlLabel(null, null, item.ControlLabel);
				m_value = new MyGuiControlLabel(null, null, item.CurrentValue);
				new MyLayoutVertical(this, 28f).Add(m_label, m_value);
			}

			public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
			{
				if (base.HasFocus)
				{
					return base.Owner.GetNextFocusControl(this, direction, page);
				}
				return this;
			}

			public override void Update()
			{
				base.Update();
				RefreshValueLabel();
				if (IsItemEnabled)
				{
					m_label.Enabled = true;
					m_value.Enabled = true;
				}
				else
				{
					m_label.Enabled = false;
					m_value.Enabled = false;
				}
			}

			private void RefreshValueLabel()
			{
				m_item.UpdateValue();
				m_value.Text = m_item.CurrentValue;
			}

			internal void UpdateItem(ItemUpdateType updateType)
			{
				switch (updateType)
				{
				case ItemUpdateType.Next:
					m_item.Next();
					break;
				case ItemUpdateType.Previous:
					m_item.Previous();
					break;
				case ItemUpdateType.Activate:
					if (m_item.Enabled)
					{
						m_item.Activate();
					}
					break;
				}
				RefreshValueLabel();
			}
		}

		private const float ITEM_SIZE = 0.03f;

		private MyGuiControlScrollablePanel m_scrollPanel;

		private List<MyGuiControlItem> m_items;

		private int m_selectedItem;

		private RectangleF m_itemsRect;

		public MyGuiScreenControlMenu()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.4f, 0.7f))
		{
			base.DrawMouseCursor = false;
			base.CanHideOthers = false;
			m_items = new List<MyGuiControlItem>();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenControlMenu_Title, null, null, 1.3f);
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent(null, new Vector2(base.Size.Value.X - 0.05f, (float)m_items.Count * 0.03f));
			m_scrollPanel = new MyGuiControlScrollablePanel(myGuiControlParent);
			m_scrollPanel.ScrollbarVEnabled = true;
			m_scrollPanel.ScrollBarVScale = 1f;
			m_scrollPanel.Size = new Vector2(base.Size.Value.X - 0.05f, base.Size.Value.Y - 0.1f);
			m_scrollPanel.Position = new Vector2(0f, 0.05f);
			MyLayoutVertical myLayoutVertical = new MyLayoutVertical(myGuiControlParent, 20f);
			foreach (MyGuiControlItem item in m_items)
			{
				myLayoutVertical.Add(item, MyAlignH.Left);
			}
			m_itemsRect.Position = m_scrollPanel.GetPositionAbsoluteTopLeft();
			m_itemsRect.Size = new Vector2(base.Size.Value.X - 0.05f, base.Size.Value.Y - 0.1f);
			base.FocusedControl = myGuiControlParent;
			m_selectedItem = ((m_items.Count == 0) ? (-1) : 0);
			Controls.Add(m_scrollPanel);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Up) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_UP) || MyInput.Static.DeltaMouseScrollWheelValue() > 0)
			{
				UpdateSelectedItem(up: true);
				UpdateScroll();
			}
			else if (MyInput.Static.IsNewKeyPressed(MyKeys.Down) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_DOWN) || MyInput.Static.DeltaMouseScrollWheelValue() < 0)
			{
				UpdateSelectedItem(up: false);
				UpdateScroll();
			}
			else if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsSpace.CONTROL_MENU) || MyInput.Static.IsNewRightMousePressed())
			{
				Canceling();
			}
			if (m_selectedItem != -1)
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Right) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_RIGHT))
				{
					m_items[m_selectedItem].UpdateItem(ItemUpdateType.Next);
				}
				else if (MyInput.Static.IsNewKeyPressed(MyKeys.Left) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_LEFT))
				{
					m_items[m_selectedItem].UpdateItem(ItemUpdateType.Previous);
				}
				else if (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT) || MyInput.Static.IsNewLeftMousePressed())
				{
					m_items[m_selectedItem].UpdateItem(ItemUpdateType.Activate);
				}
			}
		}

		public override bool Draw()
		{
			base.Draw();
			if (m_selectedItem == -1)
			{
				return true;
			}
			MyGuiControlItem myGuiControlItem = m_items[m_selectedItem];
			if (myGuiControlItem != null)
			{
				m_itemsRect.Position = m_scrollPanel.GetPositionAbsoluteTopLeft();
				using (MyGuiManager.UsingScissorRectangle(ref m_itemsRect))
				{
					Vector2 positionAbsoluteTopLeft = myGuiControlItem.GetPositionAbsoluteTopLeft();
					MyGuiManager.DrawSpriteBatch(MyGuiConstants.TEXTURE_HIGHLIGHT_DARK.Center.Texture, positionAbsoluteTopLeft, myGuiControlItem.Size, new Color(1f, 1f, 1f, 0.8f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				}
			}
			return true;
		}

		private void UpdateSelectedItem(bool up)
		{
			bool flag = false;
			if (up)
			{
				for (int i = 0; i < m_items.Count; i++)
				{
					m_selectedItem--;
					if (m_selectedItem < 0)
					{
						m_selectedItem = m_items.Count - 1;
					}
					if (m_items[m_selectedItem].IsItemEnabled)
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < m_items.Count; j++)
				{
					m_selectedItem = (m_selectedItem + 1) % m_items.Count;
					if (m_items[m_selectedItem].IsItemEnabled)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				m_selectedItem = -1;
			}
		}

		private void UpdateScroll()
		{
			if (m_selectedItem != -1)
			{
				MyGuiControlItem myGuiControlItem = m_items[m_selectedItem];
				MyGuiControlItem myGuiControlItem2 = m_items[m_items.Count - 1];
				Vector2 positionAbsoluteTopLeft = myGuiControlItem.GetPositionAbsoluteTopLeft();
				Vector2 vector = myGuiControlItem2.GetPositionAbsoluteTopLeft() + myGuiControlItem2.Size;
				float y = m_scrollPanel.GetPositionAbsoluteTopLeft().Y;
				positionAbsoluteTopLeft.Y -= y;
				vector.Y -= y;
				float y2 = positionAbsoluteTopLeft.Y;
				float num = positionAbsoluteTopLeft.Y + myGuiControlItem.Size.Y;
				float num2 = y2 / vector.Y * m_scrollPanel.ScrolledAreaSize.Y;
				float num3 = num / vector.Y * m_scrollPanel.ScrolledAreaSize.Y;
				if (num2 < m_scrollPanel.ScrollbarVPosition)
				{
					m_scrollPanel.ScrollbarVPosition = num2;
				}
				if (num3 > m_scrollPanel.ScrollbarVPosition)
				{
					m_scrollPanel.ScrollbarVPosition = num3;
				}
			}
		}

		public void AddItem(MyAbstractControlMenuItem item)
		{
			m_items.Add(new MyGuiControlItem(item, new Vector2(base.Size.Value.X - 0.1f, 0.03f)));
		}

		public void AddItems(params MyAbstractControlMenuItem[] items)
		{
			foreach (MyAbstractControlMenuItem item in items)
			{
				AddItem(item);
			}
		}

		public void ClearItems()
		{
			m_items.Clear();
		}

		protected override void OnClosed()
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		public override string GetFriendlyName()
		{
			return "Control menu screen";
		}
	}
}
