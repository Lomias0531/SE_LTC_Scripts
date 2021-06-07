using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_Velocity", "DisplayName_TSS_Velocity")]
	public class MyTSSVelocity : MyTSSCommon
	{
		public static float ASPECT_RATIO = 3f;

		public static float DECORATION_RATIO = 0.25f;

		public static float TEXT_RATIO = 0.25f;

		private Vector2 m_innerSize;

		private Vector2 m_decorationSize;

		private float m_firstLine;

		private float m_secondLine;

		private StringBuilder m_sb = new StringBuilder();

		private MyCubeGrid m_grid;

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		public MyTSSVelocity(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_innerSize = new Vector2(ASPECT_RATIO, 1f);
			MyTextSurfaceScriptBase.FitRect(size, ref m_innerSize);
			m_decorationSize = new Vector2(0.012f * m_innerSize.X, DECORATION_RATIO * m_innerSize.Y);
			m_sb.Clear();
			m_sb.Append("M");
			Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, 1f);
			m_fontScale = TEXT_RATIO * m_innerSize.Y / vector.Y;
			m_firstLine = m_halfSize.Y - m_decorationSize.Y * 0.55f;
			m_secondLine = m_halfSize.Y + m_decorationSize.Y * 0.55f;
			if (m_block != null)
			{
				m_grid = (m_block.CubeGrid as MyCubeGrid);
			}
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				AddBackground(frame, new Color(m_foregroundColor, 0.66f));
				if (m_grid != null && m_grid.Physics != null)
				{
					Color barBgColor = new Color(m_foregroundColor, 0.1f);
					float num = m_grid.Physics.LinearVelocity.Length();
					float num2 = Math.Max(MyGridPhysics.ShipMaxLinearVelocity(), 1f);
					float ratio = num / num2;
					string text = $"{num:F2} m/s";
					Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, new StringBuilder(text), m_fontScale);
					MySprite mySprite = default(MySprite);
					mySprite.Position = new Vector2(m_halfSize.X, m_firstLine - vector.Y * 0.5f);
					mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
					mySprite.Type = SpriteType.TEXT;
					mySprite.FontId = m_fontId;
					mySprite.Alignment = TextAlignment.CENTER;
					mySprite.Color = m_foregroundColor;
					mySprite.RotationOrScale = m_fontScale;
					mySprite.Data = text;
					MySprite sprite = mySprite;
					frame.Add(sprite);
					m_sb.Clear();
					m_sb.Append("[");
					Vector2 vector2 = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, 1f);
					float scale = m_decorationSize.Y / vector2.Y;
					vector2 = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, scale);
					float x = m_innerSize.X * 0.6f;
					AddProgressBar(frame, new Vector2(m_halfSize.X, m_secondLine), new Vector2(x, vector2.Y * 0.4f), ratio, barBgColor, m_foregroundColor);
					float scale2 = m_innerSize.Y / 256f * 0.9f;
					AddBrackets(frame, new Vector2(64f, 256f), scale2);
				}
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			m_grid = null;
		}
	}
}
