using System;
using System.Collections.Generic;
using System.Text;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlTabControl))]
	public class MyGuiControlTabControl : MyGuiControlParent
	{
		private Dictionary<int, MyGuiControlTabPage> m_pages = new Dictionary<int, MyGuiControlTabPage>();

		private string m_selectedTexture;

		private string m_unSelectedTexture;

		private int m_selectedPage;

		private Vector2 m_tabButtonSize;

		private float m_tabButtonScale = 1f;

		public int SelectedPage
		{
			get
			{
				return m_selectedPage;
			}
			set
			{
				if (m_pages.ContainsKey(m_selectedPage))
				{
					m_pages[m_selectedPage].Visible = false;
				}
				m_selectedPage = value;
				if (this.OnPageChanged != null)
				{
					this.OnPageChanged();
				}
				if (m_pages.ContainsKey(m_selectedPage))
				{
					m_pages[m_selectedPage].Visible = true;
				}
			}
		}

		public Vector2 TabButtonSize
		{
			get
			{
				return m_tabButtonSize;
			}
			private set
			{
				m_tabButtonSize = value;
			}
		}

		public float TabButtonScale
		{
			get
			{
				return m_tabButtonScale;
			}
			set
			{
				m_tabButtonScale = value;
				RefreshInternals();
			}
		}

		public int PagesCount => m_pages.Count;

		public DictionaryValuesReader<int, MyGuiControlTabPage> Pages => m_pages;

		public Vector2 TabPosition
		{
			get;
			private set;
		}

		public Vector2 TabSize
		{
			get;
			private set;
		}

		public Vector2 ButtonsOffset
		{
			get;
			set;
		}

		public event Action OnPageChanged;

		public MyGuiControlTabControl()
			: this(null, null, null)
		{
		}

		public MyGuiControlTabControl(Vector2? position = null, Vector2? size = null, Vector4? colorMask = null)
			: base(position, size, colorMask)
		{
			RefreshInternals();
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			_ = (MyObjectBuilder_GuiControlTabControl)builder;
			RecreatePages();
			HideTabs();
			SelectedPage = 0;
		}

		public void RecreatePages()
		{
			m_pages.Clear();
			foreach (MyGuiControlTabPage control in base.Controls)
			{
				control.Visible = false;
				m_pages.Add(control.PageKey, control);
			}
			RefreshInternals();
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			return (MyObjectBuilder_GuiControlTabControl)base.GetObjectBuilder();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			MyGuiCompositeTexture tEXTURE_BUTTON_DEFAULT_NORMAL = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL;
			MyGuiCompositeTexture tEXTURE_BUTTON_DEFAULT_HIGHLIGHT = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_HIGHLIGHT;
			_ = m_pages.Count;
			int num = 0;
			Vector2 vector = GetPositionAbsoluteTopLeft() + ButtonsOffset;
			foreach (int key in m_pages.Keys)
			{
				MyGuiControlTabPage tabSubControl = GetTabSubControl(key);
				if (tabSubControl.IsTabVisible)
				{
					bool flag = GetMouseOverTab() == key || SelectedPage == key;
					bool flag2 = base.Enabled && tabSubControl.Enabled;
					Color value = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, flag2, transitionAlpha);
					MyGuiCompositeTexture obj = (flag2 && flag) ? tEXTURE_BUTTON_DEFAULT_HIGHLIGHT : tEXTURE_BUTTON_DEFAULT_NORMAL;
					string font = (flag2 && flag) ? "White" : "Blue";
					obj.Draw(vector, TabButtonSize, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, flag2, transitionAlpha), m_tabButtonScale);
					StringBuilder text = tabSubControl.Text;
					if (text != null)
					{
						Vector2 normalizedCoord = vector + TabButtonSize / 2f;
						MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
						MyGuiManager.DrawString(font, text, normalizedCoord, tabSubControl.TextScale, value, drawAlign);
					}
					vector.X += TabButtonSize.X;
					num++;
				}
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		public override MyGuiControlBase HandleInput()
		{
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT))
			{
				int selectedPage = SelectedPage;
				int num = selectedPage;
				do
				{
					num = (num + PagesCount - 1) % PagesCount;
				}
				while (!m_pages[num].Enabled && selectedPage != num);
				if (selectedPage != num)
				{
					SelectedPage = num;
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
			{
				int selectedPage2 = SelectedPage;
				int num2 = selectedPage2;
				do
				{
					num2 = (num2 + 1) % PagesCount;
				}
				while (!m_pages[num2].Enabled && selectedPage2 != num2);
				if (selectedPage2 != num2)
				{
					SelectedPage = num2;
				}
			}
			int mouseOverTab = GetMouseOverTab();
			if (mouseOverTab != -1 && GetTabSubControl(mouseOverTab).Enabled && MyInput.Static.IsNewPrimaryButtonPressed())
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				SelectedPage = mouseOverTab;
				return this;
			}
			return base.HandleInput();
		}

		public override void ShowToolTip()
		{
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				if (page.Value.IsTabVisible)
				{
					page.Value.ShowToolTip();
				}
			}
			int mouseOverTab = GetMouseOverTab();
			foreach (KeyValuePair<int, MyGuiControlTabPage> page2 in m_pages)
			{
				if (page2.Key == mouseOverTab && page2.Value.m_toolTip != null)
				{
					page2.Value.m_toolTip.Draw(MyGuiManager.MouseCursorPosition);
					return;
				}
			}
			base.ShowToolTip();
		}

		public MyGuiControlTabPage GetTabSubControl(int key)
		{
			if (!m_pages.ContainsKey(key))
			{
				Dictionary<int, MyGuiControlTabPage> pages = m_pages;
				Vector2? position = TabPosition;
				Vector2? size = TabSize;
				Vector4? color = base.ColorMask;
				pages[key] = new MyGuiControlTabPage(key, position, size, color)
				{
					Visible = false,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
				};
				base.Controls.Add(m_pages[key]);
			}
			return m_pages[key];
		}

		private int GetMouseOverTab()
		{
			_ = m_pages.Keys.Count;
			int num = 0;
			Vector2 vector = GetPositionAbsoluteTopLeft() + ButtonsOffset;
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				if (page.Value.IsTabVisible)
				{
					int key = page.Key;
					Vector2 value = vector;
					Vector2 vector2 = value + TabButtonSize;
					if (MyGuiManager.MouseCursorPosition.X >= value.X && MyGuiManager.MouseCursorPosition.X <= vector2.X && MyGuiManager.MouseCursorPosition.Y >= value.Y && MyGuiManager.MouseCursorPosition.Y <= vector2.Y)
					{
						return key;
					}
					vector.X += TabButtonSize.X;
					num++;
				}
			}
			return -1;
		}

		private void RefreshInternals()
		{
			Vector2 minSizeGui = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui;
			minSizeGui *= m_tabButtonScale;
			TabButtonSize = new Vector2(Math.Min(base.Size.X / (float)m_pages.Count, minSizeGui.X), minSizeGui.Y);
			TabPosition = base.Size * -0.5f + new Vector2(0f, TabButtonSize.Y);
			TabSize = base.Size - new Vector2(0f, TabButtonSize.Y);
			RefreshPageParameters();
		}

		private void RefreshPageParameters()
		{
			foreach (MyGuiControlTabPage value in m_pages.Values)
			{
				value.Position = TabPosition;
				value.Size = TabSize;
				value.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			}
		}

		private void HideTabs()
		{
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				page.Value.Visible = false;
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		public void MoveToNextTab()
		{
			if (m_pages.Count != 0)
			{
				int selectedPage = SelectedPage;
				int num = int.MaxValue;
				int num2 = int.MaxValue;
				foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
				{
					num2 = Math.Min(num2, page.Key);
					if (page.Key > selectedPage && page.Key < num)
					{
						num = page.Key;
					}
				}
				SelectedPage = ((num != int.MaxValue) ? num : num2);
			}
		}

		public void MoveToPreviousTab()
		{
			if (m_pages.Count != 0)
			{
				int selectedPage = SelectedPage;
				int num = int.MinValue;
				int num2 = int.MinValue;
				foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
				{
					num2 = Math.Max(num2, page.Key);
					if (page.Key < selectedPage && page.Key > num)
					{
						num = page.Key;
					}
				}
				SelectedPage = ((num != int.MinValue) ? num : num2);
			}
		}

		public override MyGuiControlGridDragAndDrop GetDragAndDropHandlingNow()
		{
			if (m_selectedPage > -1)
			{
				return m_pages[m_selectedPage].GetDragAndDropHandlingNow();
			}
			return null;
		}
	}
}
