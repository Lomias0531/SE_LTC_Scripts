using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiBlueprintTextDialog : MyGuiBlueprintScreenBase
	{
		private MyGuiControlTextbox m_nameBox;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private string m_defaultName;

		private string m_caption;

		private int m_maxTextLength;

		private float m_textBoxWidth;

		private Action<string> callBack;

		private Vector2 WINDOW_SIZE = new Vector2(0.3f, 0.5f);

		public MyGuiBlueprintTextDialog(Vector2 position, Action<string> callBack, string defaultName, string caption = "", int maxLenght = 20, float textBoxWidth = 0.2f)
			: base(position, new Vector2(87f / 175f, 147f / 524f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, isTopMostScreen: true)
		{
			m_maxTextLength = maxLenght;
			m_caption = caption;
			m_textBoxWidth = textBoxWidth;
			this.callBack = callBack;
			m_defaultName = defaultName;
			RecreateControls(constructor: true);
			OnEnterCallback = ReturnOk;
			base.CanBeHidden = true;
			base.CanHideOthers = true;
			base.CloseButtonEnabled = true;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(m_caption, Color.White.ToVector4(), new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList2);
			m_nameBox = new MyGuiControlTextbox(new Vector2(0f, -0.027f), null, m_maxTextLength);
			m_nameBox.Text = m_defaultName;
			m_nameBox.Size = new Vector2(0.385f, 1f);
			Controls.Add(m_nameBox);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOk);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancel);
			Vector2 value = new Vector2(0.002f, m_size.Value.Y / 2f - 0.071f);
			Vector2 value2 = new Vector2(0.018f, 0f);
			m_okButton.Position = value - value2;
			m_cancelButton.Position = value + value2;
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
		}

		private void CallResultCallback(string val)
		{
			if (val != null)
			{
				callBack(val);
			}
		}

		private void ReturnOk()
		{
			if (m_nameBox.Text.Length > 0)
			{
				CallResultCallback(m_nameBox.Text);
				CloseScreen();
			}
		}

		private void OnOk(MyGuiControlButton button)
		{
			ReturnOk();
		}

		private void OnCancel(MyGuiControlButton button)
		{
			CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiRenameDialog";
		}
	}
}
