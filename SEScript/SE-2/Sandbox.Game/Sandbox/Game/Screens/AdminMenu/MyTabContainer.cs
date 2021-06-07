using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
	internal abstract class MyTabContainer
	{
		protected const float TEXT_SCALE = 0.8f;

		protected const float HIDDEN_PART_RIGHT = 0.04f;

		protected static readonly Vector4 DEFAULT_COLOR = Color.Yellow.ToVector4();

		protected static readonly Vector4 LABEL_COLOR = Color.White.ToVector4();

		protected MyGuiScreenBase m_parentScreen;

		internal MyGuiControlParent Control
		{
			get;
		}

		public MyTabContainer(MyGuiScreenBase parentScreen)
		{
			m_parentScreen = parentScreen;
			Control = new MyGuiControlParent();
			Control.Position = Vector2.Zero;
			Control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_parentScreen.Controls.Add(Control);
		}

		internal abstract bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings settings);

		protected MyGuiControlTextbox AddTextbox(ref Vector2 currentPosition, string value, Action<MyGuiControlTextbox> onTextChanged, Vector4? color = null, float scale = 1f, MyGuiControlTextboxType type = MyGuiControlTextboxType.Normal, List<MyGuiControlBase> controlGroup = null, string font = "Debug", MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, bool addToControls = true)
		{
			MyGuiControlTextbox myGuiControlTextbox = new MyGuiControlTextbox(currentPosition, value, 6, color, scale, type);
			myGuiControlTextbox.OriginAlign = originAlign;
			if (onTextChanged != null)
			{
				myGuiControlTextbox.EnterPressed += onTextChanged;
			}
			if (addToControls)
			{
				m_parentScreen.Controls.Add(myGuiControlTextbox);
			}
			controlGroup?.Add(myGuiControlTextbox);
			return myGuiControlTextbox;
		}

		public MyGuiControlButton AddButton(ref Vector2 currentPosition, string text, Action<MyGuiControlButton> onClick, List<MyGuiControlBase> controlGroup = null, Vector4? textColor = null, Vector2? size = null)
		{
			return AddButton(ref currentPosition, new StringBuilder(text), onClick, controlGroup, textColor, size);
		}

		public MyGuiControlButton AddButton(ref Vector2 currentPosition, StringBuilder text, Action<MyGuiControlButton> onClick, List<MyGuiControlBase> controlGroup = null, Vector4? textColor = null, Vector2? size = null, bool increaseSpacing = true, bool addToControls = true)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(currentPosition, MyGuiControlButtonStyleEnum.Debug, null, DEFAULT_COLOR, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, text, 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			if (addToControls)
			{
				m_parentScreen.Controls.Add(myGuiControlButton);
			}
			if (increaseSpacing)
			{
				currentPosition.Y += myGuiControlButton.Size.Y + 0.01f;
			}
			controlGroup?.Add(myGuiControlButton);
			return myGuiControlButton;
		}

		protected MyGuiControlButton CreateDebugButton(ref Vector2 currentPosition, float usableWidth, MyStringId text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, bool increaseSpacing = true, bool addToControls = true)
		{
			MyGuiControlButton myGuiControlButton = AddButton(ref currentPosition, MyTexts.Get(text), onClick, null, null, null, increaseSpacing, addToControls);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = 0.8f;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Enabled = enabled;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}
	}
}
