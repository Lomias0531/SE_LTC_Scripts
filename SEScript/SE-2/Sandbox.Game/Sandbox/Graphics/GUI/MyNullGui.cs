using System;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyNullGui : IMyGuiSandbox
	{
		public Action<float, Vector2> DrawGameLogoHandler
		{
			get;
			set;
		}

		public Vector2 MouseCursorPosition
		{
			get
			{
				return Vector2.Zero;
			}
			set
			{
			}
		}

		public void SetMouseCursorVisibility(bool visible, bool changePosition = true)
		{
		}

		public void LoadData()
		{
		}

		public void LoadContent()
		{
		}

		public bool OpenSteamOverlay(string url)
		{
			return false;
		}

		public void UnloadContent()
		{
		}

		public void SwitchDebugScreensEnabled()
		{
		}

		public void ShowModErrors()
		{
		}

		public void HandleRenderProfilerInput()
		{
		}

		public void AddScreen(MyGuiScreenBase screen)
		{
		}

		public void InsertScreen(MyGuiScreenBase screen, int index)
		{
		}

		public void RemoveScreen(MyGuiScreenBase screen)
		{
		}

		public void HandleInput()
		{
		}

		public void HandleInputAfterSimulation()
		{
		}

		public bool IsDebugScreenEnabled()
		{
			return false;
		}

		public void Update(int totalTimeInMS)
		{
		}

		public void Draw()
		{
		}

		public void BackToIntroLogos(Action afterLogosAction)
		{
		}

		public void BackToMainMenu()
		{
		}

		public float GetDefaultTextScaleWithLanguage()
		{
			return 0f;
		}

		public void TakeScreenshot(int width, int height, string saveToPath = null, bool ignoreSprites = false, bool showNotification = true)
		{
		}

		public void TakeScreenshot(string saveToPath = null, bool ignoreSprites = false, Vector2? sizeMultiplier = null, bool showNotification = true)
		{
		}

		public static Vector2 GetNormalizedCoordsAndPreserveOriginalSize(int width, int height)
		{
			return Vector2.Zero;
		}

		public void DrawGameLogo(float transitionAlpha, Vector2 position)
		{
		}

		public void DrawBadge(string texture, float transitionAlpha, Vector2 position, Vector2 size)
		{
		}
	}
}
