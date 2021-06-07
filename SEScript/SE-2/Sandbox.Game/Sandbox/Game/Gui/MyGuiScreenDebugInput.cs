using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenDebugInput : MyGuiScreenDebugBase
	{
		private static StringBuilder m_debugText = new StringBuilder(1000);

		public MyGuiScreenDebugInput()
			: base(new Vector2(0.5f, 0.5f), default(Vector2), null, isTopMostScreen: true)
		{
			m_isTopMostScreen = true;
			m_drawEvenWithoutFocus = true;
			base.CanHaveFocus = false;
		}

		public override string GetFriendlyName()
		{
			return "DebugInputScreen";
		}

		public Vector2 GetScreenLeftTopPosition()
		{
			float num = 25f * MyGuiManager.GetSafeScreenScale();
			MyGuiManager.GetSafeFullscreenRectangle();
			return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2(num, num));
		}

		public void SetTexts()
		{
			m_debugText.Clear();
			MyInput.Static.GetActualJoystickState(m_debugText);
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			SetTexts();
			float dEBUG_STATISTICS_TEXT_SCALE = MyGuiConstants.DEBUG_STATISTICS_TEXT_SCALE;
			Vector2 screenLeftTopPosition = GetScreenLeftTopPosition();
			MyGuiManager.DrawString("White", m_debugText, screenLeftTopPosition, dEBUG_STATISTICS_TEXT_SCALE, Color.Yellow);
			return true;
		}
	}
}
