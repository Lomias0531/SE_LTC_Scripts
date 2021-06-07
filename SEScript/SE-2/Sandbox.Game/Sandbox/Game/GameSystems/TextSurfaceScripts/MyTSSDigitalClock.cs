using Sandbox.Graphics;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_ClockDigital", "DisplayName_TSS_ClockDigital")]
	public class MyTSSDigitalClock : MyTSSCommon
	{
		public static float ASPECT_RATIO = 2.5f;

		public static float DECORATION_RATIO = 0.25f;

		public static float TEXT_RATIO = 0.25f;

		private Vector2 m_innerSize;

		private Vector2 m_decorationSize;

		private StringBuilder m_sb = new StringBuilder();

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		public MyTSSDigitalClock(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_innerSize = new Vector2(ASPECT_RATIO, 1f);
			MyTextSurfaceScriptBase.FitRect(size, ref m_innerSize);
			m_decorationSize = new Vector2(0.012f * m_innerSize.X, DECORATION_RATIO * m_innerSize.Y);
			m_sb.Clear();
			m_sb.Append("M");
			Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, 1f);
			m_fontScale = TEXT_RATIO * m_innerSize.Y / vector.Y;
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				AddBackground(frame, new Color(m_foregroundColor, 0.66f));
				string text = DateTime.Now.ToLocalTime().ToString("HH:mm:ss");
				Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, new StringBuilder(text), m_fontScale);
				MySprite mySprite = default(MySprite);
				mySprite.Position = new Vector2(m_halfSize.X, m_halfSize.Y - vector.Y * 0.5f);
				mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
				mySprite.Type = SpriteType.TEXT;
				mySprite.FontId = m_fontId;
				mySprite.Alignment = TextAlignment.CENTER;
				mySprite.Color = m_foregroundColor;
				mySprite.RotationOrScale = m_fontScale;
				mySprite.Data = text;
				MySprite sprite = mySprite;
				frame.Add(sprite);
				float scale = m_innerSize.Y / 256f * 0.9f;
				AddBrackets(frame, new Vector2(64f, 256f), scale);
			}
		}
	}
}
