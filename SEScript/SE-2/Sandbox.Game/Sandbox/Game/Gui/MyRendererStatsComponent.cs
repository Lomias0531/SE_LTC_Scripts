using Sandbox.Graphics;
using System.Text;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyRendererStatsComponent : MyDebugComponent
	{
		private static StringBuilder m_frameDebugText = new StringBuilder(1024);

		private static StringBuilder m_frameDebugText2 = new StringBuilder(1024);

		public override string GetName()
		{
			return "RendererStats";
		}

		public Vector2 GetScreenLeftTopPosition()
		{
			float num = 25f * MyGuiManager.GetSafeScreenScale();
			MyGuiManager.GetSafeFullscreenRectangle();
			return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2(num, num));
		}

		public override void Draw()
		{
		}
	}
}
