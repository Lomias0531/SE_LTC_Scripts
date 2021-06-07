using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenRenderModules : MyGuiScreenDebugBase
	{
		public MyGuiScreenRenderModules()
		{
			m_closeOnEsc = true;
			m_drawEvenWithoutFocus = true;
			m_isTopMostScreen = false;
			base.CanHaveFocus = false;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption("Render modules", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_scale = 0.7f;
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenRenderModules";
		}
	}
}
