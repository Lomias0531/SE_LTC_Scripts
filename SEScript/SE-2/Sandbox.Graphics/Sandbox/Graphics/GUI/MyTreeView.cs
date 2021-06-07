using System;
using System.Text;
using VRage.Audio;
using VRage.Input;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyTreeView
	{
		private MyGuiControlTreeView m_control;

		private Vector2 m_position;

		private Vector2 m_size;

		private MyTreeViewBody m_body;

		private MyHScrollbar m_hScrollbar;

		private MyVScrollbar m_vScrollbar;

		private Vector2 m_scrollbarSize;

		public MyTreeViewItem FocusedItem;

		public MyTreeViewItem HooveredItem;

		public MyTreeView(MyGuiControlTreeView control, Vector2 position, Vector2 size)
		{
			m_control = control;
			m_position = position;
			m_size = size;
			m_body = new MyTreeViewBody(this, position, size);
			m_vScrollbar = new MyVScrollbar(control);
			m_hScrollbar = new MyHScrollbar(control);
			m_scrollbarSize = new Vector2(MyGuiConstants.TREEVIEW_VSCROLLBAR_SIZE.X, MyGuiConstants.TREEVIEW_HSCROLLBAR_SIZE.Y);
		}

		public void Layout()
		{
			m_body.Layout(Vector2.Zero);
			Vector2 realSize = m_body.GetRealSize();
			bool num = m_size.Y - m_scrollbarSize.Y < realSize.Y && m_size.X - m_scrollbarSize.X < realSize.X;
			bool flag = num || m_size.Y < realSize.Y;
			bool flag2 = num || m_size.X < realSize.X;
			Vector2 size = new Vector2(flag ? (m_size.X - m_scrollbarSize.X) : m_size.X, flag2 ? (m_size.Y - m_scrollbarSize.Y) : m_size.Y);
			m_vScrollbar.Visible = flag;
			m_vScrollbar.Init(realSize.Y, size.Y);
			m_hScrollbar.Visible = flag2;
			m_hScrollbar.Init(realSize.X, size.X);
			m_body.SetSize(size);
			m_body.Layout(new Vector2(m_hScrollbar.Value, m_vScrollbar.Value));
		}

		private void TraverseVisible(ITreeView iTreeView, Action<MyTreeViewItem> action)
		{
			for (int i = 0; i < iTreeView.GetItemCount(); i++)
			{
				MyTreeViewItem item = iTreeView.GetItem(i);
				if (item.Visible)
				{
					action(item);
					if (item.IsExpanded)
					{
						TraverseVisible(item, action);
					}
				}
			}
		}

		private MyTreeViewItem NextVisible(ITreeView iTreeView, MyTreeViewItem focused)
		{
			bool found = false;
			TraverseVisible(m_body, delegate(MyTreeViewItem a)
			{
				if (a == focused)
				{
					found = true;
				}
				else if (found)
				{
					focused = a;
					found = false;
				}
			});
			return focused;
		}

		private MyTreeViewItem PrevVisible(ITreeView iTreeView, MyTreeViewItem focused)
		{
			TraverseVisible(m_body, delegate(MyTreeViewItem a)
			{
				if (a == focused)
				{
					focused = focused;
				}
				else
				{
					focused = a;
				}
			});
			return focused;
		}

		public bool HandleInput()
		{
			MyTreeViewItem hooveredItem = HooveredItem;
			HooveredItem = null;
			bool flag = m_body.HandleInput(m_control.HasFocus) || m_vScrollbar.HandleInput() || m_hScrollbar.HandleInput();
			if (m_control.HasFocus)
			{
				if (FocusedItem == null && m_body.GetItemCount() > 0 && (MyInput.Static.IsNewKeyPressed(MyKeys.Up) || MyInput.Static.IsNewKeyPressed(MyKeys.Down) || MyInput.Static.IsNewKeyPressed(MyKeys.Left) || MyInput.Static.IsNewKeyPressed(MyKeys.Right) || MyInput.Static.DeltaMouseScrollWheelValue() != 0))
				{
					FocusItem(m_body[0]);
				}
				else if (FocusedItem != null)
				{
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Down) || (MyInput.Static.DeltaMouseScrollWheelValue() < 0 && Contains(MyGuiManager.MouseCursorPosition.X, MyGuiManager.MouseCursorPosition.Y)))
					{
						FocusItem(NextVisible(m_body, FocusedItem));
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Up) || (MyInput.Static.DeltaMouseScrollWheelValue() > 0 && Contains(MyGuiManager.MouseCursorPosition.X, MyGuiManager.MouseCursorPosition.Y)))
					{
						FocusItem(PrevVisible(m_body, FocusedItem));
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Right) && FocusedItem.GetItemCount() > 0)
					{
						if (!FocusedItem.IsExpanded)
						{
							FocusedItem.IsExpanded = true;
						}
						else
						{
							MyTreeViewItem item = NextVisible(FocusedItem, FocusedItem);
							FocusItem(item);
						}
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Left))
					{
						if (FocusedItem.GetItemCount() > 0 && FocusedItem.IsExpanded)
						{
							FocusedItem.IsExpanded = false;
						}
						else if (FocusedItem.Parent is MyTreeViewItem)
						{
							FocusItem(FocusedItem.Parent as MyTreeViewItem);
						}
					}
					if (FocusedItem.GetItemCount() > 0)
					{
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Add))
						{
							FocusedItem.IsExpanded = true;
						}
						if (MyInput.Static.IsNewKeyPressed(MyKeys.Subtract))
						{
							FocusedItem.IsExpanded = false;
						}
					}
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.PageDown))
				{
					m_vScrollbar.PageDown();
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.PageUp))
				{
					m_vScrollbar.PageUp();
				}
				flag = (flag || MyInput.Static.IsNewKeyPressed(MyKeys.PageDown) || MyInput.Static.IsNewKeyPressed(MyKeys.PageUp) || MyInput.Static.IsNewKeyPressed(MyKeys.Down) || MyInput.Static.IsNewKeyPressed(MyKeys.Up) || MyInput.Static.IsNewKeyPressed(MyKeys.Left) || MyInput.Static.IsNewKeyPressed(MyKeys.Right) || MyInput.Static.IsNewKeyPressed(MyKeys.Add) || MyInput.Static.IsNewKeyPressed(MyKeys.Subtract) || MyInput.Static.DeltaMouseScrollWheelValue() != 0);
			}
			if (HooveredItem != hooveredItem)
			{
				m_control.ShowToolTip((HooveredItem == null) ? null : HooveredItem.ToolTip);
				MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
			}
			return flag;
		}

		public MyTreeViewItem AddItem(StringBuilder text, string icon, Vector2 iconSize, string expandIcon, string collapseIcon, Vector2 expandIconSize)
		{
			return m_body.AddItem(text, icon, iconSize, expandIcon, collapseIcon, expandIconSize);
		}

		public void DeleteItem(MyTreeViewItem item)
		{
			if (item == FocusedItem)
			{
				int index = item.GetIndex();
				if (index + 1 < GetItemCount())
				{
					FocusedItem = GetItem(index + 1);
				}
				else if (index - 1 >= 0)
				{
					FocusedItem = GetItem(index - 1);
				}
				else
				{
					FocusedItem = (FocusedItem.Parent as MyTreeViewItem);
				}
			}
			m_body.DeleteItem(item);
		}

		public void ClearItems()
		{
			m_body.ClearItems();
		}

		public void Draw(float transitionAlpha)
		{
			RectangleF normalizedRectangle = new RectangleF(m_body.GetPosition(), m_body.GetSize());
			using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
			{
				m_body.Draw(transitionAlpha);
			}
			Color color = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.TREEVIEW_VERTICAL_LINE_COLOR, enabled: true, transitionAlpha);
			MyGUIHelper.OutsideBorder(m_position, m_size, 2, color);
			m_vScrollbar.Draw(Color.White);
			m_hScrollbar.Draw(Color.White);
		}

		public bool Contains(Vector2 position, Vector2 size)
		{
			return MyGUIHelper.Intersects(m_body.GetPosition(), m_body.GetSize(), position, size);
		}

		public bool Contains(float x, float y)
		{
			return MyGUIHelper.Contains(m_body.GetPosition(), m_body.GetSize(), x, y);
		}

		public void FocusItem(MyTreeViewItem item)
		{
			if (item != null)
			{
				Vector2 offset = MyGUIHelper.GetOffset(m_body.GetPosition(), m_body.GetSize(), item.GetPosition(), item.GetSize());
				m_vScrollbar.ChangeValue(0f - offset.Y);
				m_hScrollbar.ChangeValue(0f - offset.X);
			}
			FocusedItem = item;
		}

		public Vector2 GetPosition()
		{
			return m_body.GetPosition();
		}

		public Vector2 GetBodySize()
		{
			return m_body.GetSize();
		}

		public Color GetColor(Vector4 color, float transitionAlpha)
		{
			return MyGuiControlBase.ApplyColorMaskModifiers(color, enabled: true, transitionAlpha);
		}

		public bool WholeRowHighlight()
		{
			return m_control.WholeRowHighlight;
		}

		public MyTreeViewItem GetItem(int index)
		{
			return m_body[index];
		}

		public MyTreeViewItem GetItem(StringBuilder name)
		{
			return m_body.GetItem(name);
		}

		public int GetItemCount()
		{
			return m_body.GetItemCount();
		}

		public void SetPosition(Vector2 position)
		{
			m_position = position;
			m_body.SetPosition(position);
		}

		public void SetSize(Vector2 size)
		{
			m_size = size;
			m_body.SetSize(size);
		}

		public static bool FilterTree(ITreeView treeView, Predicate<MyTreeViewItem> itemFilter)
		{
			int num = 0;
			for (int i = 0; i < treeView.GetItemCount(); i++)
			{
				MyTreeViewItem item = treeView.GetItem(i);
				if (FilterTree(item, itemFilter) || (item.GetItemCount() == 0 && itemFilter(item)))
				{
					item.Visible = true;
					num++;
				}
				else
				{
					item.Visible = false;
				}
			}
			return num > 0;
		}
	}
}
