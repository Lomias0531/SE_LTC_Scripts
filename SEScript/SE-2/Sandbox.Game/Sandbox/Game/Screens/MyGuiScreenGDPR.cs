using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenGDPR : MyGuiScreenBase
	{
		private MyGuiControlButton m_yesBtn;

		private MyGuiControlButton m_noBtn;

		private MyGuiControlButton m_linkBtn;

		public MyGuiScreenGDPR()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.5264286f, 225f / 524f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenGDPR.ctor START");
			base.EnabledBackgroundFade = true;
			m_closeOnEsc = true;
			base.CloseButtonEnabled = true;
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
			return "MyGuiScreenGDPR";
		}

		protected void BuildControls()
		{
			AddCaption(MyTexts.GetString(MySpaceTexts.GDPR_Caption), null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.79f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.79f);
			Controls.Add(myGuiControlSeparatorList2);
			float num = 0.095f;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(0.015f, 0.005f + num), new Vector2(0.44f, 0.45f), null, "Blue", 0.76f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlMultilineText.AppendText(MyTexts.GetString(MySpaceTexts.GDPR_Text1), "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendText("\n\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendText(MyTexts.GetString(MySpaceTexts.GDPR_Text2), "Blue", 0.76f, Color.White);
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			m_linkBtn = new MyGuiControlButton(new Vector2(-0f, 0.068f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Yes), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnLinkButtonClick);
			m_linkBtn.Enabled = true;
			m_linkBtn.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
			m_linkBtn.Text = MyTexts.GetString(MySpaceTexts.GDPR_PrivacyPolicy);
			Controls.Add(myGuiControlMultilineText);
			Controls.Add(m_linkBtn);
			m_yesBtn = new MyGuiControlButton(new Vector2(-0.1f, 0.17f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Yes), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnYesButtonClick);
			m_yesBtn.Enabled = true;
			m_yesBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			Controls.Add(m_yesBtn);
			m_noBtn = new MyGuiControlButton(new Vector2(0.1f, 0.17f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.No), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnNoButtonClick);
			m_noBtn.Enabled = true;
			m_noBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Close));
			Controls.Add(m_noBtn);
		}

		private void OnLinkButtonClick(object sender)
		{
			MyGuiSandbox.OpenUrl("http://mirror.keenswh.com/policy/KSWH_Privacy_Policy.pdf", UrlOpenMode.ExternalBrowser);
		}

		private void OnYesButtonClick(object sender)
		{
			MySandboxGame.Config.GDPRConsent = true;
			MySandboxGame.Config.Save();
			ConsentSenderGDPR.TrySendConsent();
			CloseScreen();
		}

		private void OnNoButtonClick(object sender)
		{
			MySandboxGame.Config.GDPRConsent = false;
			MySandboxGame.Config.Save();
			ConsentSenderGDPR.TrySendConsent();
			CloseScreen();
		}
	}
}
