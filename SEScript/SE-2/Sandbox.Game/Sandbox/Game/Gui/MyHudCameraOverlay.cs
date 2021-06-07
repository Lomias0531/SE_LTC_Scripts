using Sandbox.Graphics;
using VRage.Game.Components;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 400)]
	internal class MyHudCameraOverlay : MySessionComponentBase
	{
		private static string m_textureName;

		private static bool m_enabled;

		public static string TextureName
		{
			get
			{
				return m_textureName;
			}
			set
			{
				m_textureName = value;
			}
		}

		public static bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value)
				{
					m_enabled = value;
				}
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Enabled = false;
		}

		public override void Draw()
		{
			base.Draw();
			if (Enabled)
			{
				DrawFullScreenSprite();
			}
		}

		private static void DrawFullScreenSprite()
		{
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			RectangleF destination = new RectangleF(fullscreenRectangle.X, fullscreenRectangle.Y, fullscreenRectangle.Width, fullscreenRectangle.Height);
			MyRenderProxy.DrawSprite(m_textureName, ref destination, null, Color.White, 0f);
		}
	}
}
