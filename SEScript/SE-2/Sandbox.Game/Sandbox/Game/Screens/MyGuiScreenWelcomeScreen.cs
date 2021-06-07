using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenWelcomeScreen : MyGuiScreenBase
	{
		private MyGuiControlButton m_okBtn;

		public MyGuiScreenWelcomeScreen()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.5264286f, 0.7633588f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenWelcomeScreen.ctor START");
			base.EnabledBackgroundFade = true;
			m_closeOnEsc = true;
			m_drawEvenWithoutFocus = true;
			base.CanHideOthers = true;
			base.CanBeHidden = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			BuildControls();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWelcomeScreen";
		}

		protected void BuildControls()
		{
			AddCaption(MyCommonTexts.ScreenCaptionWelcomeScreen, null, new Vector2(0f, 0.003f));
			_ = MySandboxGame.Config.NewsletterCurrentStatus;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.79f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.79f);
			Controls.Add(myGuiControlSeparatorList2);
			float num = 0.095f;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(0.015f, -0.162f + num), new Vector2(0.44f, 0.45f), null, "Blue", 0.76f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlMultilineText.AppendText(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text1), "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendText("\n\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendText(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text2), "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendText("\n\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendText(string.Format(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text3), MyGameService.Service.ServiceName), "Blue", 0.76f, Color.White);
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2(-0.08f, 0.07f + num), MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
			Controls.Add(myGuiControlPanel);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0.195f, 0.1f + num), null, MyTexts.GetString(MySpaceTexts.WelcomeScreen_SignatureTitle));
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			Controls.Add(myGuiControlLabel);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(0.195f, 0.125f + num), null, MyTexts.GetString(MySpaceTexts.WelcomeScreen_Signature));
			myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			Controls.Add(myGuiControlLabel2);
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			m_okBtn = new MyGuiControlButton(new Vector2(0f, 0.338f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCloseButtonClick);
			m_okBtn.Enabled = true;
			m_okBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			Controls.Add(myGuiControlMultilineText);
			Controls.Add(m_okBtn);
			base.CloseButtonEnabled = true;
		}

		private void OnCloseButtonClick(object sender)
		{
			MySandboxGame.Config.WelcomScreenCurrentStatus = MyConfig.WelcomeScreenStatus.AlreadySeen;
			MySandboxGame.Config.Save();
			CloseScreen();
		}
	}
}
