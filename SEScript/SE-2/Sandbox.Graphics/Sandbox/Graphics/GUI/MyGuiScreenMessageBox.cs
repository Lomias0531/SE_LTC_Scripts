using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiScreenMessageBox : MyGuiScreenBase
	{
		public class Style
		{
			public MyGuiPaddedTexture BackgroundTexture;

			public string CaptionFont;

			public string TextFont;

			public MyGuiControlButtonStyleEnum ButtonStyle;
		}

		public enum ResultEnum
		{
			YES,
			NO,
			CANCEL
		}

		private static readonly Style[] m_styles;

		public Action<ResultEnum> ResultCallback;

		private MyStringId m_yesButtonText;

		private MyStringId m_noButtonText;

		private MyStringId m_okButtonText;

		private MyStringId m_cancelButtonText;

		private MyMessageBoxButtonsType m_buttonType;

		private MyMessageBoxStyleEnum m_type;

		private int m_timeoutInMiliseconds;

		private int m_timeoutStartedTimeInMiliseconds;

		private MyGuiControlMultilineText m_messageBoxText;

		private MyGuiControlCheckbox m_showAgainCheckBox;

		private string m_formatText;

		private StringBuilder m_formattedCache;

		private Style m_style;

		private StringBuilder m_messageText;

		private StringBuilder m_messageCaption;

		private ResultEnum m_focusedResult;

		public bool CloseBeforeCallback
		{
			get;
			set;
		}

		public bool InstantClose
		{
			get;
			set;
		}

		public new bool CanHideOthers
		{
			get
			{
				return base.CanHideOthers;
			}
			set
			{
				base.CanHideOthers = value;
			}
		}

		public StringBuilder MessageText
		{
			get
			{
				return m_messageBoxText.Text;
			}
			set
			{
				m_messageBoxText.Text = value;
			}
		}

		static MyGuiScreenMessageBox()
		{
			m_styles = new Style[MyUtils.GetMaxValueFromEnum<MyMessageBoxStyleEnum>() + 1];
			m_styles[0] = new Style
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_MESSAGEBOX_BACKGROUND_INFO,
				CaptionFont = "InfoMessageBoxCaption",
				TextFont = "InfoMessageBoxText",
				ButtonStyle = MyGuiControlButtonStyleEnum.Default
			};
			m_styles[1] = new Style
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_MESSAGEBOX_BACKGROUND_INFO,
				CaptionFont = "InfoMessageBoxCaption",
				TextFont = "InfoMessageBoxText",
				ButtonStyle = MyGuiControlButtonStyleEnum.Default
			};
		}

		public MyGuiScreenMessageBox(MyMessageBoxStyleEnum styleEnum, MyMessageBoxButtonsType buttonType, StringBuilder messageText, StringBuilder messageCaption, MyStringId okButtonText, MyStringId cancelButtonText, MyStringId yesButtonText, MyStringId noButtonText, Action<ResultEnum> callback, int timeoutInMiliseconds, ResultEnum focusedResult, bool canHideOthers, Vector2? size, float backgroundTransition = 0f, float guiTransition = 0f)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, null, isTopMostScreen: true, null, backgroundTransition, guiTransition)
		{
			InstantClose = true;
			m_style = m_styles[(int)styleEnum];
			m_focusedResult = focusedResult;
			m_backgroundColor = new Vector4(1f, 1f, 1f, 0.95f);
			m_backgroundTexture = m_style.BackgroundTexture.Texture;
			base.EnabledBackgroundFade = true;
			m_buttonType = buttonType;
			m_okButtonText = okButtonText;
			m_cancelButtonText = cancelButtonText;
			m_yesButtonText = yesButtonText;
			m_noButtonText = noButtonText;
			ResultCallback = callback;
			m_drawEvenWithoutFocus = true;
			base.CanBeHidden = false;
			CanHideOthers = canHideOthers;
			if (size.HasValue)
			{
				m_size = size;
			}
			else
			{
				m_size = m_style.BackgroundTexture.SizeGui;
			}
			m_messageText = messageText;
			m_messageCaption = (messageCaption ?? new StringBuilder());
			RecreateControls(constructor: true);
			if (buttonType == MyMessageBoxButtonsType.YES_NO_TIMEOUT || buttonType == MyMessageBoxButtonsType.NONE_TIMEOUT)
			{
				m_timeoutStartedTimeInMiliseconds = MyGuiManager.TotalTimeInMilliseconds;
				m_timeoutInMiliseconds = timeoutInMiliseconds;
				m_formatText = messageText.ToString();
				m_formattedCache = new StringBuilder(m_formatText.Length);
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(m_style.ButtonStyle).NormalTexture.MinSizeGui;
			Vector2 vector = MyGuiManager.MeasureString(m_style.CaptionFont, m_messageCaption, 0.8f);
			Vector2 paddingSizeGui = m_style.BackgroundTexture.PaddingSizeGui;
			MyGuiControlLabel control = new MyGuiControlLabel(new Vector2(0f, -0.5f * m_size.Value.Y + paddingSizeGui.Y + 0.019f), null, m_messageCaption.ToString(), null, 0.8f, m_style.CaptionFont, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			Controls.Add(control);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			if (m_style.ButtonStyle == MyGuiControlButtonStyleEnum.Error)
			{
				myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.691f / 2f, m_size.Value.Y / 2f - 0.072f), m_size.Value.X * 0.691f, 0f, new Vector4(0.57f, 0.39f, 0.37f, 1f));
			}
			else
			{
				myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.691f / 2f, m_size.Value.Y / 2f - 0.072f), m_size.Value.X * 0.691f);
			}
			Controls.Add(myGuiControlSeparatorList);
			m_messageBoxText = new MyGuiControlMultilineText(Vector2.Zero, new Vector2(m_size.Value.X - 2f * paddingSizeGui.X, m_size.Value.Y - (2f * paddingSizeGui.Y + vector.Y + minSizeGui.Y)), Vector4.One, contents: m_messageText, font: m_style.TextFont, textScale: 0.8f, textAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_messageBoxText.PositionY -= 0.013f;
			Controls.Add(m_messageBoxText);
			float y = 0.5f * m_size.Value.Y - paddingSizeGui.Y - 0.013f;
			float num = 0.01275f;
			MyGuiControlBase myGuiControlBase = null;
			MyGuiControlBase myGuiControlBase2 = null;
			MyGuiControlBase myGuiControlBase3 = null;
			switch (m_buttonType)
			{
			case MyMessageBoxButtonsType.OK:
				Controls.Add(myGuiControlBase = MakeButton(new Vector2(0f, y), m_style, m_okButtonText, OnYesClick, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM));
				break;
			case MyMessageBoxButtonsType.YES_NO:
			case MyMessageBoxButtonsType.YES_NO_TIMEOUT:
				Controls.Add(myGuiControlBase = MakeButton(new Vector2(0f - num, y), m_style, m_yesButtonText, OnYesClick, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM));
				Controls.Add(myGuiControlBase2 = MakeButton(new Vector2(num, y), m_style, m_noButtonText, OnNoClick, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM));
				break;
			case MyMessageBoxButtonsType.YES_NO_CANCEL:
			{
				num = 0.02f;
				Controls.Add(myGuiControlBase = MakeButton(new Vector2(0f - (num + minSizeGui.X * 0.5f), y), m_style, m_yesButtonText, OnYesClick, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM));
				Controls.Add(myGuiControlBase2 = MakeButton(new Vector2(0f, y), m_style, m_noButtonText, OnNoClick, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM));
				Controls.Add(myGuiControlBase3 = MakeButton(new Vector2(num + minSizeGui.X * 0.5f, y), m_style, m_cancelButtonText, OnCancelClick, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM));
				float num2 = 0.003f;
				myGuiControlBase.PositionX += num2;
				myGuiControlBase2.PositionX += num2;
				myGuiControlBase3.PositionX += num2;
				break;
			}
			default:
				throw new InvalidBranchException();
			case MyMessageBoxButtonsType.NONE:
			case MyMessageBoxButtonsType.NONE_TIMEOUT:
				break;
			}
			switch (m_focusedResult)
			{
			case ResultEnum.YES:
				base.FocusedControl = myGuiControlBase;
				break;
			case ResultEnum.NO:
				base.FocusedControl = myGuiControlBase2;
				break;
			case ResultEnum.CANCEL:
				base.FocusedControl = myGuiControlBase3;
				break;
			}
		}

		private MyGuiControlButton MakeButton(Vector2 position, Style config, MyStringId text, Action<MyGuiControlButton> onClick, MyGuiDrawAlignEnum align)
		{
			Vector2? position2 = position;
			StringBuilder text2 = MyTexts.Get(text);
			return new MyGuiControlButton(position2, config.ButtonStyle, null, null, align, null, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenMessageBox";
		}

		public void OnYesClick(MyGuiControlButton sender)
		{
			OnClick(ResultEnum.YES);
		}

		public void OnNoClick(MyGuiControlButton sender)
		{
			OnClick(ResultEnum.NO);
		}

		public void OnCancelClick(MyGuiControlButton sender)
		{
			OnClick(ResultEnum.CANCEL);
		}

		private void OnClick(ResultEnum result)
		{
			if (CloseBeforeCallback)
			{
				CloseInternal();
				CallResultCallback(result);
			}
			else
			{
				CallResultCallback(result);
				CloseInternal();
			}
		}

		private void CloseInternal()
		{
			if (InstantClose)
			{
				CloseScreenNow();
			}
			else
			{
				CloseScreen();
			}
		}

		private void CallResultCallback(ResultEnum val)
		{
			if (ResultCallback != null)
			{
				ResultCallback(val);
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			if (m_buttonType == MyMessageBoxButtonsType.YES_NO_TIMEOUT || m_buttonType == MyMessageBoxButtonsType.NONE_TIMEOUT)
			{
				int num = MyGuiManager.TotalTimeInMilliseconds - m_timeoutStartedTimeInMiliseconds;
				if (num >= m_timeoutInMiliseconds)
				{
					OnNoClick(null);
				}
				int num2 = MathHelper.Clamp((m_timeoutInMiliseconds - num) / 1000, 0, m_timeoutInMiliseconds / 1000);
				m_messageBoxText.Text = m_formattedCache.Clear().AppendFormat(m_formatText, num2.ToString());
			}
			return true;
		}

		protected override void Canceling()
		{
			base.Canceling();
			switch (m_buttonType)
			{
			case MyMessageBoxButtonsType.NONE:
			case MyMessageBoxButtonsType.NONE_TIMEOUT:
				break;
			case MyMessageBoxButtonsType.OK:
				CallResultCallback(ResultEnum.YES);
				break;
			case MyMessageBoxButtonsType.YES_NO:
			case MyMessageBoxButtonsType.YES_NO_TIMEOUT:
				CallResultCallback(ResultEnum.NO);
				break;
			case MyMessageBoxButtonsType.YES_NO_CANCEL:
				CallResultCallback(ResultEnum.CANCEL);
				break;
			}
		}
	}
}
