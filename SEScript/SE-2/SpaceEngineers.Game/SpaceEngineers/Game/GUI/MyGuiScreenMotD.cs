using Sandbox.Graphics.GUI;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
	internal class MyGuiScreenMotD : MyGuiScreenBase
	{
		private StringBuilder m_message;

		private MyGuiControlLabel m_caption;

		private MyGuiControlMultilineText m_messageMultiline;

		private MyGuiControlButton m_continueButton;

		public StringBuilder MessageOfTheDay
		{
			get
			{
				return m_message;
			}
			private set
			{
				m_message = value;
			}
		}

		public MyGuiScreenMotD(StringBuilder message)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.8f, 0.8f), isTopMostScreen: true)
		{
			MessageOfTheDay = message;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenMotD";
		}

		public override void RecreateControls(bool constructor)
		{
			m_caption = AddCaption(MyTexts.GetString(MyCommonTexts.MotD_Caption));
			m_messageMultiline = new MyGuiControlMultilineText
			{
				Position = new Vector2(0f, -0.3f),
				Size = new Vector2(0.7f, 0.6f),
				Font = "Blue",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_messageMultiline.Text = MyTexts.SubstituteTexts(MessageOfTheDay);
			Controls.Add(m_messageMultiline);
			m_continueButton = new MyGuiControlButton(new Vector2(0f, 0.35f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.MotD_Button), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onContinueClick);
			Controls.Add(m_continueButton);
		}

		private void onContinueClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
