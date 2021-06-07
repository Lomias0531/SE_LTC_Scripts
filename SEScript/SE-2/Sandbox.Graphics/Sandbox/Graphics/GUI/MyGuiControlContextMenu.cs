using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlContextMenu : MyGuiControlBase, IMyImeCandidateList, IVRageGuiControl
	{
		public struct EventArgs
		{
			public int ItemIndex;

			public object UserData;
		}

		private enum MyContextMenuKeys
		{
			UP,
			DOWN,
			ENTER
		}

		private class MyContextMenuKeyTimerController
		{
			public MyKeys Key;

			public int LastKeyPressTime;

			public MyContextMenuKeyTimerController(MyKeys key)
			{
				Key = key;
				LastKeyPressTime = -60000;
			}
		}

		private const int NUM_VISIBLE_ITEMS = 20;

		private int m_numItems;

		private MyGuiControlListbox m_itemsList;

		private MyContextMenuKeyTimerController[] m_keys;

		private bool m_allowKeyboardNavigation;

		public List<MyGuiControlListbox.Item> Items => m_itemsList.Items.ToList();

		public bool AllowKeyboardNavigation
		{
			get
			{
				return m_allowKeyboardNavigation;
			}
			set
			{
				if (m_allowKeyboardNavigation != value)
				{
					m_allowKeyboardNavigation = value;
				}
			}
		}

		public event Action<MyGuiControlContextMenu, EventArgs> ItemClicked;

		private event Action<IMyImeCandidateList, int> ItemClickedIndex;

		event Action<IMyImeCandidateList, int> IMyImeCandidateList.ItemClicked
		{
			add
			{
				ItemClickedIndex += value;
			}
			remove
			{
				ItemClickedIndex -= value;
			}
		}

		public event Action OnDeactivated;

		public bool IsGuiControlEqual(IVRageGuiControl control)
		{
			return m_itemsList == control;
		}

		public MyGuiControlContextMenu()
		{
			m_itemsList = new MyGuiControlListbox();
			m_itemsList.Name = "ContextMenuListbox";
			m_itemsList.VisibleRowsCount = 20;
			base.Enabled = false;
			m_keys = new MyContextMenuKeyTimerController[3];
			m_keys[0] = new MyContextMenuKeyTimerController(MyKeys.Up);
			m_keys[1] = new MyContextMenuKeyTimerController(MyKeys.Down);
			m_keys[2] = new MyContextMenuKeyTimerController(MyKeys.Enter);
			base.Name = "ContextMenu";
			Elements.Add(m_itemsList);
			base.CanFocusChildren = true;
		}

		public void CreateNewContextMenu()
		{
			Clear();
			Deactivate();
			CreateContextMenu();
		}

		public Vector2 GetListBoxSize()
		{
			return m_itemsList.Size;
		}

		private void CreateContextMenu()
		{
			m_itemsList = new MyGuiControlListbox(null, MyGuiControlListboxStyleEnum.ContextMenu);
			m_itemsList.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			m_itemsList.Enabled = true;
			m_itemsList.ItemClicked += list_ItemClicked;
			m_itemsList.MultiSelect = false;
		}

		public new void Clear()
		{
			m_itemsList.Items.Clear();
			m_numItems = 0;
		}

		public void AddItem(StringBuilder text, string tooltip = "", string icon = "", object userData = null)
		{
			MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text, null, icon, userData);
			m_itemsList.Add(item);
			m_itemsList.VisibleRowsCount = Math.Min(20, m_numItems++) + 1;
		}

		private void list_ItemClicked(MyGuiControlListbox sender)
		{
			if (base.Visible)
			{
				int num = -1;
				object userData = null;
				using (List<MyGuiControlListbox.Item>.Enumerator enumerator = sender.SelectedItems.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						MyGuiControlListbox.Item current = enumerator.Current;
						num = sender.Items.IndexOf(current);
						userData = current.UserData;
					}
				}
				if (this.ItemClicked != null)
				{
					this.ItemClicked(this, new EventArgs
					{
						ItemIndex = num,
						UserData = userData
					});
				}
				this.ItemClickedIndex?.Invoke(this, num);
				if (!m_itemsList.IsOverScrollBar())
				{
					Deactivate();
				}
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			if ((MyInput.Static.IsNewMousePressed(MyMouseButtonsEnum.Left) || MyInput.Static.IsNewMousePressed(MyMouseButtonsEnum.Right)) && base.Visible && !base.IsMouseOver)
			{
				Deactivate();
			}
			if ((MyInput.Static.IsKeyPress(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL)) && base.Visible)
			{
				Deactivate();
				return this;
			}
			if (AllowKeyboardNavigation)
			{
				Vector2 mouseCursorPosition = MyGuiManager.MouseCursorPosition;
				if (mouseCursorPosition.X >= m_itemsList.Position.X && mouseCursorPosition.X <= m_itemsList.Position.X + m_itemsList.Size.X && mouseCursorPosition.Y >= m_itemsList.Position.Y && mouseCursorPosition.Y <= m_itemsList.Position.Y + m_itemsList.Size.Y)
				{
					m_itemsList.SelectedItems.Clear();
				}
				else
				{
					if (MyInput.Static.IsKeyPress(MyKeys.Up) && IsEnoughDelay(MyContextMenuKeys.UP, 100))
					{
						UpdateLastKeyPressTimes(MyContextMenuKeys.UP);
						SelectPrevious();
						return this;
					}
					if (MyInput.Static.IsKeyPress(MyKeys.Down) && IsEnoughDelay(MyContextMenuKeys.DOWN, 100))
					{
						UpdateLastKeyPressTimes(MyContextMenuKeys.DOWN);
						SelectNext();
						return this;
					}
					if (MyInput.Static.IsKeyPress(MyKeys.Enter) && IsEnoughDelay(MyContextMenuKeys.ENTER, 100))
					{
						UpdateLastKeyPressTimes(MyContextMenuKeys.ENTER);
						if (m_itemsList.SelectedItems.Count > 0)
						{
							EventArgs arg = default(EventArgs);
							arg.ItemIndex = m_itemsList.Items.IndexOf(m_itemsList.SelectedItems[0]);
							arg.UserData = m_itemsList.SelectedItems[0].UserData;
							this.ItemClicked(this, arg);
							Deactivate();
							return this;
						}
					}
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT) && m_itemsList.SelectedItems.Count > 0)
			{
				EventArgs arg2 = default(EventArgs);
				arg2.ItemIndex = m_itemsList.Items.IndexOf(m_itemsList.SelectedItems[0]);
				arg2.UserData = m_itemsList.SelectedItems[0].UserData;
				this.ItemClicked(this, arg2);
				Deactivate();
				return this;
			}
			return m_itemsList.HandleInput();
		}

		private void SelectPrevious()
		{
			int num = -1;
			int num2 = 0;
			int count = m_itemsList.Items.Count;
			foreach (MyGuiControlListbox.Item item in m_itemsList.Items)
			{
				if (m_itemsList.SelectedItems.Contains(item))
				{
					num = num2;
				}
				num2++;
			}
			m_itemsList.SelectedItems.Clear();
			if (num >= 0)
			{
				m_itemsList.SelectedItems.Add(m_itemsList.Items[((num - 1) % count + count) % count]);
			}
			else if (m_itemsList.Items.Count > 0)
			{
				m_itemsList.SelectedItems.Add(m_itemsList.Items[0]);
			}
		}

		private void SelectNext()
		{
			int num = 0;
			int num2 = -1;
			int count = m_itemsList.Items.Count;
			foreach (MyGuiControlListbox.Item item in m_itemsList.Items)
			{
				if (m_itemsList.SelectedItems.Contains(item))
				{
					num2 = num;
				}
				num++;
			}
			m_itemsList.SelectedItems.Clear();
			if (num2 >= 0)
			{
				m_itemsList.SelectedItems.Add(m_itemsList.Items[((num2 + 1) % count + count) % count]);
			}
			else if (m_itemsList.Items.Count > 0)
			{
				m_itemsList.SelectedItems.Add(m_itemsList.Items[0]);
			}
		}

		public void Deactivate()
		{
			m_itemsList.IsActiveControl = false;
			m_itemsList.Visible = false;
			IsActiveControl = false;
			base.Visible = false;
			this.OnDeactivated.InvokeIfNotNull();
		}

		public void Activate(bool autoPositionOnMouseTip = true)
		{
			if (autoPositionOnMouseTip)
			{
				m_itemsList.Position = MyGuiManager.MouseCursorPosition;
				m_itemsList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				FitContextMenuToScreen();
			}
			else
			{
				m_itemsList.Position = base.Position;
				m_itemsList.OriginAlign = base.OriginAlign;
			}
			m_itemsList.Visible = true;
			m_itemsList.IsActiveControl = true;
			base.Visible = true;
			IsActiveControl = true;
		}

		private void FitContextMenuToScreen()
		{
			if (m_itemsList.Position.X < 0f)
			{
				m_itemsList.Position = new Vector2(0f, m_itemsList.Position.Y);
			}
			if (m_itemsList.Position.X + m_itemsList.Size.X >= 1f)
			{
				m_itemsList.Position = new Vector2(1f - m_itemsList.Size.X, m_itemsList.Position.Y);
			}
			if (m_itemsList.Position.Y < 0f)
			{
				m_itemsList.Position = new Vector2(m_itemsList.Position.X, 0f);
			}
			if (m_itemsList.Position.Y + m_itemsList.Size.Y >= 1f)
			{
				m_itemsList.Position = new Vector2(m_itemsList.Position.X, 1f - m_itemsList.Size.Y);
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_itemsList.Draw(transitionAlpha * m_itemsList.Alpha, backgroundTransitionAlpha * m_itemsList.Alpha);
		}

		private bool IsEnoughDelay(MyContextMenuKeys key, int forcedDelay)
		{
			MyContextMenuKeyTimerController myContextMenuKeyTimerController = m_keys[(int)key];
			if (myContextMenuKeyTimerController == null)
			{
				return true;
			}
			return MyGuiManager.TotalTimeInMilliseconds - myContextMenuKeyTimerController.LastKeyPressTime > forcedDelay;
		}

		private void UpdateLastKeyPressTimes(MyContextMenuKeys key)
		{
			MyContextMenuKeyTimerController myContextMenuKeyTimerController = m_keys[(int)key];
			if (myContextMenuKeyTimerController != null)
			{
				myContextMenuKeyTimerController.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
			}
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			if (currentFocusControl == this)
			{
				if (page || Items.Count == 0)
				{
					return null;
				}
				switch (direction)
				{
				case MyDirection.Down:
					SelectNext();
					break;
				case MyDirection.Up:
					SelectPrevious();
					break;
				case MyDirection.Right:
					return null;
				case MyDirection.Left:
					return null;
				}
			}
			return this;
		}
	}
}
