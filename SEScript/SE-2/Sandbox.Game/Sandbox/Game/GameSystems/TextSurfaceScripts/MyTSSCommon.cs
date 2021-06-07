using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	public abstract class MyTSSCommon : MyTextSurfaceScriptBase
	{
		protected string m_fontId = "Monospace";

		protected float m_fontScale = 1f;

		protected MyTSSCommon(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
		}

		public override void Run()
		{
			m_backgroundColor = m_surface.ScriptBackgroundColor;
			m_foregroundColor = m_surface.ScriptForegroundColor;
		}

		protected MySpriteDrawFrame AddBackground(MySpriteDrawFrame frame, Color? color = null)
		{
			MySprite dEFAULT_BACKGROUND = MyTextSurfaceHelper.DEFAULT_BACKGROUND;
			dEFAULT_BACKGROUND.Color = (color ?? Color.White);
			ref Vector2? position = ref dEFAULT_BACKGROUND.Position;
			position += m_surface.TextureSize / 2f;
			frame.Add(dEFAULT_BACKGROUND);
			ref Vector2? position2 = ref dEFAULT_BACKGROUND.Position;
			position2 += MyTextSurfaceHelper.BACKGROUND_SHIFT;
			frame.Add(dEFAULT_BACKGROUND);
			return frame;
		}

		protected MySpriteDrawFrame AddBrackets(MySpriteDrawFrame frame, Vector2 size, float scale)
		{
			MySprite sprite = new MySprite(SpriteType.TEXTURE, "DecorativeBracketLeft", null, null, m_foregroundColor)
			{
				Position = new Vector2(size.X * scale, m_halfSize.Y),
				Size = size * scale
			};
			frame.Add(sprite);
			sprite = new MySprite(SpriteType.TEXTURE, "DecorativeBracketRight", null, null, m_foregroundColor)
			{
				Position = new Vector2(m_size.X - size.X * scale, m_halfSize.Y),
				Size = size * scale
			};
			frame.Add(sprite);
			return frame;
		}

		protected MySpriteDrawFrame AddProgressBar(MySpriteDrawFrame frame, Vector2 position, Vector2 size, float ratio, Color barBgColor, Color barFgColor, string barBgSprite = null, string barFgSprite = null)
		{
			MySprite mySprite = new MySprite(SpriteType.TEXTURE, barBgSprite ?? "SquareSimple", null, null, barBgColor);
			mySprite.Alignment = TextAlignment.LEFT;
			mySprite.Position = position - new Vector2(size.X * 0.5f, 0f);
			mySprite.Size = size;
			MySprite sprite = mySprite;
			frame.Add(sprite);
			mySprite = new MySprite(SpriteType.TEXTURE, barFgSprite ?? "SquareSimple", null, null, barFgColor);
			mySprite.Alignment = TextAlignment.LEFT;
			mySprite.Position = position - new Vector2(size.X * 0.5f, 0f);
			mySprite.Size = new Vector2(size.X * ratio, size.Y);
			MySprite sprite2 = mySprite;
			frame.Add(sprite2);
			return frame;
		}

		protected MySpriteDrawFrame AddTextBox(MySpriteDrawFrame frame, Vector2 position, Vector2 size, string text, string font, float scale, Color bgColor, Color textColor, string bgSprite = null, float textOffset = 0f)
		{
			Vector2 vector = position + new Vector2(size.X * 0.5f, 0f);
			if (!string.IsNullOrEmpty(bgSprite))
			{
				MySprite sprite = MySprite.CreateSprite(bgSprite, vector, size);
				sprite.Color = bgColor;
				sprite.Alignment = TextAlignment.RIGHT;
				frame.Add(sprite);
			}
			MySprite sprite2 = MySprite.CreateText(text, font, textColor, scale, TextAlignment.RIGHT);
			sprite2.Position = vector + new Vector2(0f - textOffset, (0f - size.Y) * 0.5f);
			sprite2.Size = size;
			frame.Add(sprite2);
			return frame;
		}

		protected MySpriteDrawFrame AddLine(MySpriteDrawFrame frame, Vector2 startPos, Vector2 endPos, Color color, float thicknessPx)
		{
			Vector2 value = endPos - startPos;
			Vector2 size = new Vector2(thicknessPx, value.Length());
			Vector2 value2 = Vector2.Normalize(value);
			float num = Vector2.Dot(value2, Vector2.UnitX);
			if (value2.Y > 0f)
			{
				num = 0f - num;
			}
			float rotationOrScale = 0f - (float)Math.Acos(num) + MathF.E * 449f / 777f;
			MySprite sprite = MySprite.CreateSprite("SquareTapered", startPos + value * 0.5f, size);
			sprite.Color = color;
			sprite.RotationOrScale = rotationOrScale;
			frame.Add(sprite);
			return frame;
		}
	}
}
