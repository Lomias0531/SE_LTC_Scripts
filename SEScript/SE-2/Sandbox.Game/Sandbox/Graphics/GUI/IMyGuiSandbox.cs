using System;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal interface IMyGuiSandbox
	{
		Action<float, Vector2> DrawGameLogoHandler
		{
			get;
			set;
		}

		Vector2 MouseCursorPosition
		{
			get;
		}

		void AddScreen(MyGuiScreenBase screen);

		void InsertScreen(MyGuiScreenBase screen, int index);

		void BackToIntroLogos(Action afterLogosAction);

		void BackToMainMenu();

		void Draw();

		void DrawGameLogo(float transitionAlpha, Vector2 position);

		void DrawBadge(string texture, float transitionAlpha, Vector2 position, Vector2 size);

		float GetDefaultTextScaleWithLanguage();

		void HandleInput();

		void HandleInputAfterSimulation();

		void HandleRenderProfilerInput();

		bool IsDebugScreenEnabled();

		void LoadContent();

		void LoadData();

		bool OpenSteamOverlay(string url);

		void RemoveScreen(MyGuiScreenBase screen);

		void SetMouseCursorVisibility(bool visible, bool changePosition = true);

		void SwitchDebugScreensEnabled();

		void ShowModErrors();

		void TakeScreenshot(int width, int height, string saveToPath = null, bool ignoreSprites = false, bool showNotification = true);

		void TakeScreenshot(string saveToPath = null, bool ignoreSprites = false, Vector2? sizeMultiplier = null, bool showNotification = true);

		void UnloadContent();

		void Update(int totalTimeInMS);
	}
}
