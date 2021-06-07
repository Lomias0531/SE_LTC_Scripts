using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Linq;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenDebugErrors : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugErrors";
		}

		public MyGuiScreenDebugErrors()
			: base(new Vector2(0.5f, 0.5f), null, null, isTopMostScreen: true)
		{
			base.EnabledBackgroundFade = true;
			m_backgroundTexture = null;
			Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
			float num = (float)safeFullscreenRectangle.Width / (float)safeFullscreenRectangle.Height;
			base.Size = new Vector2(num * 3f / 4f, 1f);
			base.CanHideOthers = true;
			m_isTopScreen = true;
			m_canShareInput = false;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			AddCaption(MyCommonTexts.ScreenDebugOfficial_ErrorLogCaption, null, new Vector2(0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y * -0.5f));
			m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y;
			MyGuiControlMultilineText myGuiControlMultilineText = AddMultilineText(base.Size - new Vector2(0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y), base.Size * -0.5f, 0.7f);
			if (MyDefinitionErrors.GetErrors().Count() == 0)
			{
				myGuiControlMultilineText.AppendText(MyTexts.Get(MyCommonTexts.ScreenDebugOfficial_NoErrorText));
			}
			foreach (MyDefinitionErrors.Error error in MyDefinitionErrors.GetErrors())
			{
				myGuiControlMultilineText.AppendText(error.ToString(), myGuiControlMultilineText.Font, myGuiControlMultilineText.TextScaleWithLanguage, error.GetSeverityColor().ToVector4());
				myGuiControlMultilineText.AppendLine();
				myGuiControlMultilineText.AppendLine();
			}
		}
	}
}
