using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenEditorError : MyGuiScreenBase
	{
		protected string m_errorText = "";

		public MyGuiScreenEditorError(string errorText = null)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.6f, 0.7f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_errorText = errorText;
			base.CanBeHidden = false;
			base.CanHideOthers = true;
			m_closeOnEsc = true;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenEditorError";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MySpaceTexts.ProgrammableBlock_CodeEditor_Title, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel();
			myGuiControlCompositePanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
			myGuiControlCompositePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			myGuiControlCompositePanel.Position = new Vector2(0f, -0.023f);
			myGuiControlCompositePanel.Size = new Vector2(0.5f, 0.465f);
			Controls.Add(myGuiControlCompositePanel);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(0.005f, -0.025f), new Vector2(0.485f, 0.44f));
			myGuiControlMultilineText.Text = new StringBuilder(m_errorText);
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			Controls.Add(myGuiControlMultilineText);
			MyGuiControlButton control = new MyGuiControlButton(new Vector2(0f, 0.277f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OkButtonClicked);
			Controls.Add(control);
		}

		private void OkButtonClicked(MyGuiControlButton button)
		{
			CloseScreen();
		}

		public override bool CloseScreen()
		{
			return base.CloseScreen();
		}
	}
}
