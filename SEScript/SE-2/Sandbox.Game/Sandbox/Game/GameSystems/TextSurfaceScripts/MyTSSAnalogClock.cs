using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_ClockAnalog", "DisplayName_TSS_ClockAnalog")]
	public class MyTSSAnalogClock : MyTSSCommon
	{
		public static float ASPECT_RATIO = 1.85f;

		public static float DECORATION_RATIO = 0.25f;

		public static readonly float INDICATOR_WIDTH = 0.012f;

		private static Vector2 HOURS_SIZE = new Vector2(0.32f, INDICATOR_WIDTH);

		private static Vector2 MINUTES_SIZE = new Vector2(0.42f, INDICATOR_WIDTH);

		private static Vector2 INDICATORS_SIZE = new Vector2(0.06f, INDICATOR_WIDTH);

		private Vector2 m_innerSize;

		private Vector2 m_clockSize;

		private Vector2 m_decorationSize;

		private Vector2 m_sizeModifier;

		private StringBuilder m_sb = new StringBuilder();

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update1000;

		public MyTSSAnalogClock(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_innerSize = new Vector2(ASPECT_RATIO, 1f);
			MyTextSurfaceScriptBase.FitRect(size, ref m_innerSize);
			m_clockSize = ((m_innerSize.X > m_innerSize.Y) ? new Vector2(m_innerSize.Y) : new Vector2(m_innerSize.X));
			m_decorationSize = new Vector2(INDICATOR_WIDTH * m_innerSize.X, DECORATION_RATIO * m_innerSize.Y);
			m_sizeModifier = new Vector2(1f, 512f / m_clockSize.X);
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				AddBackground(frame, new Color(m_foregroundColor, 0.66f));
				float num = 0f;
				float num2 = 0f;
				Vector2 zero = Vector2.Zero;
				Vector2 value = new Vector2(INDICATORS_SIZE.X * 0.5f, 0f);
				Color foregroundColor = m_foregroundColor;
				for (int i = 0; i < 12; i++)
				{
					num = MathHelper.ToRadians(30 * i);
					float x = (float)Math.Cos(num);
					num2 = (float)Math.Sin(num);
					zero = new Vector2(x, num2) * m_clockSize * 0.4f - value;
					frame.Add(new MySprite(SpriteType.TEXTURE, "SquareTapered", color: foregroundColor, position: m_halfSize + zero, size: INDICATORS_SIZE * m_clockSize * m_sizeModifier, fontId: null, alignment: TextAlignment.CENTER, rotation: num));
				}
				DateTime dateTime = DateTime.Now.ToLocalTime();
				num = MathHelper.ToRadians((float)(30 * dateTime.Hour) + 0.5f * (float)dateTime.Minute - 90f);
				float x2 = (float)Math.Cos(num);
				num2 = (float)Math.Sin(num);
				zero = new Vector2(x2, num2) * m_clockSize * 0.3f * HOURS_SIZE.X;
				frame.Add(new MySprite(SpriteType.TEXTURE, "SquareTapered", color: foregroundColor, position: m_halfSize + zero, size: HOURS_SIZE * m_clockSize * m_sizeModifier, fontId: null, alignment: TextAlignment.CENTER, rotation: num));
				num = MathHelper.ToRadians(6 * dateTime.Minute - 90);
				float x3 = (float)Math.Cos(num);
				num2 = (float)Math.Sin(num);
				zero = new Vector2(x3, num2) * m_clockSize * 0.3f * MINUTES_SIZE.X;
				frame.Add(new MySprite(SpriteType.TEXTURE, "SquareTapered", color: foregroundColor, position: m_halfSize + zero, size: MINUTES_SIZE * m_clockSize * m_sizeModifier, fontId: null, alignment: TextAlignment.CENTER, rotation: num));
				float scale = m_clockSize.Y / 256f * 0.9f;
				AddBrackets(frame, new Vector2(64f, 256f), scale);
			}
		}
	}
}
