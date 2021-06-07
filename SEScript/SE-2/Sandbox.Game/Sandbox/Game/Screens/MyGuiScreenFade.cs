using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenFade : MyGuiScreenBase
	{
		private uint m_fadeInTimeMs;

		private uint m_fadeOutTimeMs;

		public event Action<MyGuiScreenFade> Shown;

		public override string GetFriendlyName()
		{
			return "Fade Screen";
		}

		public override int GetTransitionOpeningTime()
		{
			return (int)m_fadeInTimeMs;
		}

		public override int GetTransitionClosingTime()
		{
			return (int)m_fadeOutTimeMs;
		}

		public MyGuiScreenFade(Color fadeColor, uint fadeInTimeMs = 5000u, uint fadeOutTimeMs = 5000u)
			: base(Vector2.Zero, fadeColor, Vector2.One * 10f, isTopMostScreen: true)
		{
			m_fadeInTimeMs = fadeInTimeMs;
			m_fadeOutTimeMs = fadeOutTimeMs;
			m_backgroundFadeColor = fadeColor;
			base.EnabledBackgroundFade = true;
		}

		protected override void OnShow()
		{
			if (this.Shown != null)
			{
				this.Shown(this);
			}
		}

		public override bool CloseScreen()
		{
			return base.CloseScreen();
		}
	}
}
