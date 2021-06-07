using System;
using System.Text;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Graphics.GUI
{
	internal class MyTreeViewItem : MyTreeViewBase
	{
		public EventHandler _Action;

		public EventHandler RightClick;

		public object Tag;

		public bool Visible;

		public bool Enabled;

		public bool IsExpanded;

		public StringBuilder Text;

		public MyToolTips ToolTip;

		public MyIconTexts IconTexts;

		public MyTreeViewBase Parent;

		private readonly float padding = 0.002f;

		private readonly float spacing = 0.01f;

		private readonly float rightBorder = 0.01f;

		private string m_icon;

		private string m_expandIcon;

		private string m_collapseIcon;

		private Vector2 m_iconSize;

		private Vector2 m_expandIconSize;

		private Vector2 m_currentOrigin;

		private Vector2 m_currentSize;

		private Vector2 m_currentTextSize;

		private float m_loadingIconRotation;

		private readonly Vector2 m_loadingTextureSize;

		public MyTreeViewItemDragAndDrop DragDrop
		{
			get;
			set;
		}

		public MyTreeViewItem(StringBuilder text, string icon, Vector2 iconSize, string expandIcon, string collapseIcon, Vector2 expandIconSize)
		{
			Visible = true;
			Enabled = true;
			Text = text;
			m_icon = icon;
			m_expandIcon = expandIcon;
			m_collapseIcon = collapseIcon;
			m_iconSize = iconSize;
			m_expandIconSize = expandIconSize;
			m_loadingTextureSize = MyRenderProxy.GetTextureSize("Textures\\GUI\\screens\\screen_loading_wheel.dds");
		}

		private float GetHeight()
		{
			return Math.Max(m_currentTextSize.Y, Math.Max(m_iconSize.Y, m_expandIconSize.Y));
		}

		public Vector2 GetIconSize()
		{
			return m_iconSize;
		}

		private Vector2 GetExpandIconPosition()
		{
			return new Vector2(padding, padding + (m_currentSize.Y - m_expandIconSize.Y) / 2f);
		}

		private Vector2 GetIconPosition()
		{
			return new Vector2(padding + m_expandIconSize.X + spacing, padding);
		}

		private Vector2 GetTextPosition()
		{
			float num = (m_icon != null) ? (m_iconSize.X + spacing) : 0f;
			return new Vector2(padding + m_expandIconSize.X + spacing + num, (m_currentSize.Y - m_currentTextSize.Y) / 2f);
		}

		public Vector2 GetOffset()
		{
			return new Vector2(padding + m_expandIconSize.X / 2f, 2f * padding + GetHeight());
		}

		public Vector2 LayoutItem(Vector2 origin)
		{
			m_currentOrigin = origin;
			if (!Visible)
			{
				m_currentSize = Vector2.Zero;
				return Vector2.Zero;
			}
			m_currentTextSize = MyGuiManager.MeasureString("Blue", Text, 0.8f);
			float num = (m_icon != null) ? (m_iconSize.X + spacing) : 0f;
			float num2 = padding + m_expandIconSize.X + spacing + num + m_currentTextSize.X + rightBorder + padding;
			float num3 = padding + GetHeight() + padding;
			m_currentSize = new Vector2(num2, num3);
			if (IsExpanded)
			{
				Vector2 offset = GetOffset();
				Vector2 vector = LayoutItems(origin + GetOffset());
				num2 = Math.Max(num2, offset.X + vector.X);
				num3 += vector.Y;
			}
			return new Vector2(num2, num3);
		}

		public void Draw(float transitionAlpha)
		{
			if (!Visible || !TreeView.Contains(m_currentOrigin, m_currentSize))
			{
				return;
			}
			bool flag = TreeView.HooveredItem == this;
			Vector2 expandIconPosition = GetExpandIconPosition();
			Vector2 iconPosition = GetIconPosition();
			Vector2 textPosition = GetTextPosition();
			Vector4 vector = Enabled ? Vector4.One : MyGuiConstants.TREEVIEW_DISABLED_ITEM_COLOR;
			if (TreeView.FocusedItem == this)
			{
				Color color = TreeView.GetColor(MyGuiConstants.TREEVIEW_SELECTED_ITEM_COLOR * vector, transitionAlpha);
				if (TreeView.WholeRowHighlight())
				{
					MyGUIHelper.FillRectangle(new Vector2(TreeView.GetPosition().X, m_currentOrigin.Y), new Vector2(TreeView.GetBodySize().X, m_currentSize.Y), color);
				}
				else
				{
					MyGUIHelper.FillRectangle(m_currentOrigin, m_currentSize, color);
				}
			}
			if (GetItemCount() > 0)
			{
				Vector4 color2 = flag ? (vector * MyGuiConstants.CONTROL_MOUSE_OVER_BACKGROUND_COLOR_MULTIPLIER) : vector;
				MyGuiManager.DrawSpriteBatch(IsExpanded ? m_collapseIcon : m_expandIcon, m_currentOrigin + expandIconPosition, m_expandIconSize, TreeView.GetColor(color2, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			if (m_icon == null)
			{
				DrawLoadingIcon(vector, iconPosition, transitionAlpha);
			}
			else
			{
				MyGuiManager.DrawSpriteBatch(m_icon, m_currentOrigin + iconPosition, m_iconSize, TreeView.GetColor(vector, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			Vector4 color3 = flag ? (MyGuiConstants.CONTROL_MOUSE_OVER_BACKGROUND_COLOR_MULTIPLIER * vector) : (MyGuiConstants.TREEVIEW_TEXT_COLOR * vector);
			MyGuiManager.DrawString("Blue", Text, m_currentOrigin + textPosition, 0.8f, TreeView.GetColor(color3, transitionAlpha));
			if (IconTexts != null)
			{
				IconTexts.Draw(m_currentOrigin + iconPosition, m_iconSize, transitionAlpha, flag);
			}
		}

		private void DrawLoadingIcon(Vector4 baseColor, Vector2 iconPosition, float transitionAlpha)
		{
			Vector2 normalizedCoord = m_currentOrigin + iconPosition + m_iconSize / 2f;
			Vector2 value = 0.5f * m_iconSize;
			Vector2 normalizedSize = MyGuiManager.GetNormalizedSize(m_loadingTextureSize * value, 1f);
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\screens\\screen_loading_wheel.dds", normalizedCoord, normalizedSize, TreeView.GetColor(baseColor, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false, null, m_loadingIconRotation);
			m_loadingIconRotation += 0.02f;
			m_loadingIconRotation %= 6.283186f;
		}

		public void DrawDraged(Vector2 position, float transitionAlpha)
		{
			if (m_icon == null && Text == null)
			{
				return;
			}
			if (m_icon != null)
			{
				if (m_icon == null)
				{
					DrawLoadingIcon(Vector4.One, GetIconPosition(), transitionAlpha);
					return;
				}
				MyGUIHelper.OutsideBorder(position + GetIconPosition(), m_iconSize, 2, MyGuiConstants.THEMED_GUI_LINE_COLOR);
				MyGuiManager.DrawSpriteBatch(m_icon, position + GetIconPosition(), m_iconSize, Color.White, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			else if (Text != null)
			{
				Vector2 vector = position + GetTextPosition();
				Vector2 vector2 = MyGuiManager.MeasureString("Blue", Text, 0.8f);
				MyGUIHelper.OutsideBorder(vector, vector2, 2, MyGuiConstants.THEMED_GUI_LINE_COLOR);
				MyGUIHelper.FillRectangle(vector, vector2, TreeView.GetColor(MyGuiConstants.TREEVIEW_SELECTED_ITEM_COLOR, transitionAlpha));
				Color color = TreeView.GetColor(MyGuiConstants.CONTROL_MOUSE_OVER_BACKGROUND_COLOR_MULTIPLIER, transitionAlpha);
				MyGuiManager.DrawString("Blue", Text, position + GetTextPosition(), 0.8f, color);
			}
		}

		public bool HandleInputEx(bool hasKeyboardActiveControl)
		{
			if (!Visible)
			{
				return false;
			}
			bool result = false;
			if (TreeView.Contains(MyGuiManager.MouseCursorPosition.X, MyGuiManager.MouseCursorPosition.Y) && MyGUIHelper.Contains(m_currentOrigin, m_currentSize, MyGuiManager.MouseCursorPosition.X, MyGuiManager.MouseCursorPosition.Y))
			{
				TreeView.HooveredItem = this;
			}
			if (Enabled && DragDrop != null)
			{
				result = DragDrop.HandleInput(this);
			}
			if (MyInput.Static.IsNewLeftMouseReleased())
			{
				if (GetItemCount() > 0 && MyGUIHelper.Contains(m_currentOrigin + GetExpandIconPosition(), m_expandIconSize, MyGuiManager.MouseCursorPosition.X, MyGuiManager.MouseCursorPosition.Y))
				{
					IsExpanded = !IsExpanded;
					result = true;
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
				else if (TreeView.HooveredItem == this)
				{
					TreeView.FocusItem(this);
					result = true;
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
			}
			if (Enabled && TreeView.HooveredItem == this)
			{
				if (_Action != null)
				{
					DoAction();
				}
				else if (GetItemCount() > 0)
				{
					IsExpanded = !IsExpanded;
				}
				result = true;
			}
			if (MyInput.Static.IsNewRightMousePressed() && TreeView.HooveredItem == this)
			{
				if (RightClick != null)
				{
					RightClick(this, EventArgs.Empty);
				}
				result = true;
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			return result;
		}

		public int GetIndex()
		{
			return Parent.GetIndex(this);
		}

		public Vector2 GetPosition()
		{
			return m_currentOrigin;
		}

		public Vector2 GetSize()
		{
			return m_currentSize;
		}

		public void DoAction()
		{
			if (_Action != null)
			{
				_Action(this, EventArgs.Empty);
			}
		}
	}
}
