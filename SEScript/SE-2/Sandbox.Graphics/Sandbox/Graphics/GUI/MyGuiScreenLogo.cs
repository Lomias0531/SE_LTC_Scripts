using VRage.Input;
using VRageMath;
using VRageRender;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiScreenLogo : MyGuiScreenBase
	{
		private int? m_startTime;

		private string m_textureName;

		private int m_fadeIn;

		private int m_fadeOut;

		private int m_openTime;

		private float m_scale;

		public MyGuiScreenLogo(string[] textures, uint unused)
			: this(textures[0])
		{
		}

		public MyGuiScreenLogo(string texture, float scale = 0.66f, int fadeIn = 300, int fadeOut = 300, int openTime = 1000)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, Vector2.One)
		{
			m_scale = scale;
			m_fadeIn = fadeIn;
			m_fadeOut = fadeOut;
			m_openTime = openTime;
			base.DrawMouseCursor = false;
			m_textureName = texture;
			m_closeOnEsc = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();
		}

		public override void UnloadContent()
		{
			MyRenderProxy.UnloadTexture(m_textureName);
			base.UnloadContent();
		}

		protected override void Canceling()
		{
			m_fadeOut = 0;
			base.Canceling();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewLeftMousePressed() || MyInput.Static.IsNewRightMousePressed() || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyInput.Static.IsNewKeyPressed(MyKeys.Enter))
			{
				Canceling();
			}
		}

		public override string GetFriendlyName()
		{
			return "Logo screen";
		}

		public override int GetTransitionOpeningTime()
		{
			return m_fadeIn;
		}

		public override int GetTransitionClosingTime()
		{
			return m_fadeOut;
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			if (base.State == MyGuiScreenState.OPENED && !m_startTime.HasValue)
			{
				m_startTime = MyGuiManager.TotalTimeInMilliseconds;
			}
			if (m_startTime.HasValue && MyGuiManager.TotalTimeInMilliseconds > m_startTime + m_openTime)
			{
				CloseScreen();
			}
			return true;
		}

		public override bool Draw()
		{
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", fullscreenRectangle, Color.Black);
			MyGuiManager.GetSafeAspectRatioFullScreenPictureSize(MyGuiConstants.LOADING_BACKGROUND_TEXTURE_REAL_SIZE, out Rectangle outRect);
			outRect.Inflate(-(int)((float)outRect.Width * (1f - m_scale) / 2f), -(int)((float)outRect.Height * (1f - m_scale) / 2f));
			Color color = new Color(0.95f, 0.95f, 0.95f, 1f);
			color *= m_transitionAlpha;
			MyGuiManager.DrawSpriteBatch(m_textureName, outRect, color);
			return true;
		}
	}
}
