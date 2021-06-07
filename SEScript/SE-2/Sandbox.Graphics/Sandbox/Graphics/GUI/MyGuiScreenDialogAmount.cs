using System;
using System.Globalization;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiScreenDialogAmount : MyGuiScreenBase
	{
		private MyGuiControlTextbox m_amountTextbox;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlButton m_increaseButton;

		private MyGuiControlButton m_decreaseButton;

		private MyGuiControlLabel m_errorLabel;

		private StringBuilder m_textBuffer;

		private MyStringId m_caption;

		private bool m_parseAsInteger;

		private float m_amountMin;

		private float m_amountMax;

		private float m_amount;

		public event Action<float> OnConfirmed;

		public MyGuiScreenDialogAmount(float min, float max, MyStringId caption, int minMaxDecimalDigits = 3, bool parseAsInteger = false, float? defaultAmount = null, float backgroundTransition = 0f, float guiTransition = 0f)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(87f / 175f, 147f / 524f), isTopMostScreen: false, null, backgroundTransition, guiTransition)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			m_textBuffer = new StringBuilder();
			m_amountMin = min;
			m_amountMax = max;
			m_amount = (defaultAmount.HasValue ? defaultAmount.Value : max);
			m_parseAsInteger = parseAsInteger;
			m_caption = caption;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDialogAmount";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			string path = MyGuiScreenBase.MakeScreenFilepath("DialogAmount");
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, path), out MyObjectBuilder_GuiScreen objectBuilder);
			Init(objectBuilder);
			AddCaption(m_caption, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, confirmButton_OnButtonClick);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, cancelButton_OnButtonClick);
			Vector2 value = new Vector2(0.002f, m_size.Value.Y / 2f - 0.071f);
			Vector2 value2 = new Vector2(0.018f, 0f);
			m_okButton.Position = value - value2;
			m_cancelButton.Position = value + value2;
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			m_amountTextbox = (MyGuiControlTextbox)Controls.GetControlByName("AmountTextbox");
			m_increaseButton = (MyGuiControlButton)Controls.GetControlByName("IncreaseButton");
			m_decreaseButton = (MyGuiControlButton)Controls.GetControlByName("DecreaseButton");
			m_errorLabel = (MyGuiControlLabel)Controls.GetControlByName("ErrorLabel");
			m_errorLabel.TextScale = 0.68f;
			m_errorLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_errorLabel.Position = new Vector2(-0.19f, 0.008f);
			m_errorLabel.Visible = false;
			m_amountTextbox.TextChanged += amountTextbox_TextChanged;
			m_increaseButton.ButtonClicked += increaseButton_OnButtonClick;
			m_decreaseButton.ButtonClicked += decreaseButton_OnButtonClick;
			RefreshAmountTextbox();
			m_amountTextbox.SelectAll();
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))
			{
				confirmButton_OnButtonClick(m_okButton);
			}
		}

		private void RefreshAmountTextbox()
		{
			m_textBuffer.Clear();
			if (m_parseAsInteger)
			{
				m_textBuffer.AppendInt32((int)m_amount);
			}
			else
			{
				m_textBuffer.AppendDecimalDigit(m_amount, 4);
			}
			m_amountTextbox.TextChanged -= amountTextbox_TextChanged;
			m_amountTextbox.Text = m_textBuffer.ToString();
			m_amountTextbox.TextChanged += amountTextbox_TextChanged;
			m_amountTextbox.ColorMask = Vector4.One;
		}

		private bool TryParseAndStoreAmount(string text)
		{
			if (text.TryParseWithSuffix(NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out float value))
			{
				m_amount = (m_parseAsInteger ? ((float)Math.Floor(value)) : value);
				return true;
			}
			return false;
		}

		private void amountTextbox_TextChanged(MyGuiControlTextbox obj)
		{
			m_amountTextbox.ColorMask = Vector4.One;
			m_errorLabel.Visible = false;
		}

		private void increaseButton_OnButtonClick(MyGuiControlButton sender)
		{
			if (!TryParseAndStoreAmount(m_amountTextbox.Text))
			{
				m_errorLabel.Text = MyTexts.GetString(MyCommonTexts.DialogAmount_ParsingError);
				m_errorLabel.Visible = true;
				m_amountTextbox.ColorMask = Color.Red.ToVector4();
			}
			else
			{
				m_amount += 1f;
				m_amount = MathHelper.Clamp(m_amount, m_amountMin, m_amountMax);
				RefreshAmountTextbox();
			}
		}

		private void decreaseButton_OnButtonClick(MyGuiControlButton sender)
		{
			if (!TryParseAndStoreAmount(m_amountTextbox.Text))
			{
				m_errorLabel.Text = MyTexts.GetString(MyCommonTexts.DialogAmount_ParsingError);
				m_errorLabel.Visible = true;
				m_amountTextbox.ColorMask = Color.Red.ToVector4();
			}
			else
			{
				m_amount -= 1f;
				m_amount = MathHelper.Clamp(m_amount, m_amountMin, m_amountMax);
				RefreshAmountTextbox();
			}
		}

		private void confirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			if (!TryParseAndStoreAmount(m_amountTextbox.Text))
			{
				m_errorLabel.Text = MyTexts.GetString(MyCommonTexts.DialogAmount_ParsingError);
				m_errorLabel.Visible = true;
				m_amountTextbox.ColorMask = Color.Red.ToVector4();
				return;
			}
			if (m_amount > m_amountMax || m_amount < m_amountMin)
			{
				m_errorLabel.Text = string.Format(MyTexts.GetString(MyCommonTexts.DialogAmount_RangeError), m_amountMin, m_amountMax);
				m_errorLabel.Visible = true;
				m_amountTextbox.ColorMask = Color.Red.ToVector4();
				return;
			}
			if (this.OnConfirmed != null)
			{
				this.OnConfirmed(m_amount);
			}
			CloseScreen();
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
