using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenTutorialsScreen : MyGuiScreenBase
	{
		private MyGuiControlButton m_okBtn;

		private MyGuiControlCheckbox m_dontShowAgainCheckbox;

		private Action m_okAction;

		public MyGuiScreenTutorialsScreen(Action okAction)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.5264286f, 175f / 262f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenWelcomeScreen.ctor START");
			m_okAction = okAction;
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
			return "MyGuiScreenTutorialsScreen";
		}

		protected void BuildControls()
		{
			AddCaption("Tutorials", null, new Vector2(0f, 0.003f));
			_ = MySandboxGame.Config.NewsletterCurrentStatus;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.79f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.79f);
			Controls.Add(myGuiControlSeparatorList2);
			float num = 0.145f;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(0.015f, -0.162f + num), new Vector2(0.44f, 0.45f), null, "Blue", 0.76f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlMultilineText.AppendText("Hello Engineer!\r\n\r\n            We recommend that you view these tutorial links, which contain useful information on how to get started in Space Engineers!", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendText("\n\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART1, "Introduction");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART2, "Basic Controls");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART3, "Possibilities Within The Game Modes");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART4, "Drilling, Refining, & Assembling (Survival) ");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART5, "Building Your 1st Ship (Creative)");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART10, "Survival");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART6, "Experimental Mode");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART7, "Building Your 1st Ground Vehicle (Creative)");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART8, "Steam Workshop & Blueprints");
			myGuiControlMultilineText.AppendText("\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendLink(MySteamConstants.URL_TUTORIAL_PART9, "Other Advice & Closing Thoughts");
			myGuiControlMultilineText.AppendText("\n\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.OnLinkClicked += OnLinkClicked;
			myGuiControlMultilineText.AppendText("You can always access these tutorials from the Help screen (F1 key).", "Blue", 0.76f, Color.White);
			m_dontShowAgainCheckbox = new MyGuiControlCheckbox(new Vector2(0.08f, 0.017f + num), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_dontShowAgainCheckbox);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0.195f, 0.047f + num), null, "Don't show again");
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			Controls.Add(myGuiControlLabel);
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			m_okBtn = new MyGuiControlButton(new Vector2(0f, 0.155f + num), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOKButtonClick);
			m_okBtn.Enabled = true;
			m_okBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			Controls.Add(myGuiControlMultilineText);
			Controls.Add(m_okBtn);
			base.CloseButtonEnabled = true;
		}

		private void OnOKButtonClick(object sender)
		{
			MySandboxGame.Config.FirstTimeTutorials = !m_dontShowAgainCheckbox.IsChecked;
			MySandboxGame.Config.Save();
			CloseScreen();
			m_okAction();
		}

		protected override void Canceling()
		{
			m_okAction();
			base.Canceling();
		}

		private void OnLinkClicked(MyGuiControlBase sender, string url)
		{
			MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
		}
	}
}
