using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public static class MyGuiTextShadows
	{
		public const string TEXT_SHADOW_DEFAULT = "Default";

		private static Dictionary<string, ShadowTextureSet> m_textureSets = new Dictionary<string, ShadowTextureSet>();

		public static void AddTextureSet(string name, IEnumerable<ShadowTexture> textures)
		{
			ShadowTextureSet shadowTextureSet = new ShadowTextureSet();
			shadowTextureSet.AddTextures(textures);
			m_textureSets[name] = shadowTextureSet;
		}

		public static void ClearShadowTextures()
		{
			m_textureSets.Clear();
		}

		public static void DrawShadow(ref Vector2 position, ref Vector2 textSize, string textureSet = null, float fogAlphaMultiplier = 1f, MyGuiDrawAlignEnum alignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
		{
			if (textureSet == null)
			{
				textureSet = "Default";
			}
			ShadowTexture texture;
			Vector2 shadowSize = GetShadowSize(ref textSize, textureSet, out texture);
			Vector2 normalizedCoord = AdjustPosition(position, ref textSize, ref shadowSize, alignment);
			MyGuiManager.DrawSpriteBatch(color: new Color(0, 0, 0, (byte)(255f * texture.DefaultAlpha * fogAlphaMultiplier)), texture: texture.Texture, normalizedCoord: normalizedCoord, normalizedSize: shadowSize, drawAlign: alignment);
		}

		public static Vector2 GetShadowSize(ref Vector2 size, string textureSet = null)
		{
			if (textureSet == null)
			{
				textureSet = "Default";
			}
			ShadowTexture texture;
			return GetShadowSize(ref size, textureSet, out texture);
		}

		private static Vector2 GetShadowSize(ref Vector2 size, string textureSet, out ShadowTexture texture)
		{
			Vector2 result = size;
			if (!m_textureSets.TryGetValue(textureSet, out ShadowTextureSet value) && !m_textureSets.TryGetValue("Default", out value))
			{
				throw new Exception("Missing Default shadow texture. Check ShadowTextureSets.sbc");
			}
			texture = value.GetOptimalTexture(size.X);
			result.X *= texture.GrowFactorWidth;
			result.Y *= texture.GrowFactorHeight;
			return result;
		}

		private static Vector2 AdjustPosition(Vector2 position, ref Vector2 textSize, ref Vector2 shadowSize, MyGuiDrawAlignEnum alignment)
		{
			switch (alignment)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
			{
				float num3 = shadowSize.X - textSize.X;
				float num4 = shadowSize.Y - textSize.Y;
				position.X -= num3 / 2f;
				position.Y -= num4 / 2f;
				break;
			}
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
			{
				float num = shadowSize.X - textSize.X;
				float num2 = shadowSize.Y - textSize.Y;
				position.X += num / 2f;
				position.Y -= num2 / 2f;
				break;
			}
			}
			return position;
		}
	}
}
