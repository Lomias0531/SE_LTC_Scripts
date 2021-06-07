using Sandbox.Definitions.GUI;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System.Collections.Generic;
using VRage.Game.Gui;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyHudCrosshair
	{
		private struct SpriteInfo
		{
			public MyHudTexturesEnum SpriteEnum;

			public Color Color;

			public Vector2 HalfSize;

			public MyStringId SpriteId;

			public int FadeoutTime;

			public int TimeRemaining;

			public bool Visible;
		}

		private Vector2 m_rightVector;

		private Vector2 m_position;

		private List<SpriteInfo> m_sprites;

		private int m_lastGameplayTimeInMs;

		protected MyObjectBuilder_CrosshairStyle m_style;

		protected MyStatControls m_statControls;

		private static MyStringId m_defaultSpriteId;

		public Vector2 Position => m_position;

		public static Vector2 ScreenCenter => new Vector2(0.5f, MyGuiManager.GetHudSizeHalf().Y);

		public bool Visible
		{
			get;
			private set;
		}

		static MyHudCrosshair()
		{
			m_defaultSpriteId = MyStringId.GetOrCompute("Default");
		}

		public MyHudCrosshair()
		{
			m_sprites = new List<SpriteInfo>();
			m_lastGameplayTimeInMs = 0;
			ResetToDefault();
		}

		public void ResetToDefault(bool clear = true)
		{
			SetDefaults(clear);
		}

		public void HideDefaultSprite()
		{
			for (int i = 0; i < m_sprites.Count; i++)
			{
				SpriteInfo value = m_sprites[i];
				if (!(value.SpriteId != m_defaultSpriteId))
				{
					value.Visible = false;
					m_sprites[i] = value;
				}
			}
		}

		public void Recenter()
		{
			m_position = ScreenCenter;
		}

		public void ChangePosition(Vector2 newPosition)
		{
			m_position = newPosition;
		}

		public void ChangeDefaultSprite(MyHudTexturesEnum newSprite, float size = 0f)
		{
			for (int i = 0; i < m_sprites.Count; i++)
			{
				SpriteInfo value = m_sprites[i];
				if (!(value.SpriteId != m_defaultSpriteId))
				{
					if (size != 0f)
					{
						value.HalfSize = Vector2.One * size;
					}
					value.SpriteEnum = newSprite;
					m_sprites[i] = value;
				}
			}
		}

		public void AddTemporarySprite(MyHudTexturesEnum spriteEnum, MyStringId spriteId, int timeout = 2000, int fadeTime = 1000, Color? color = null, float size = 0.02f)
		{
			SpriteInfo spriteInfo = default(SpriteInfo);
			spriteInfo.Color = (color.HasValue ? color.Value : MyHudConstants.HUD_COLOR_LIGHT);
			spriteInfo.FadeoutTime = fadeTime;
			spriteInfo.HalfSize = Vector2.One * size;
			spriteInfo.SpriteId = spriteId;
			spriteInfo.SpriteEnum = spriteEnum;
			spriteInfo.TimeRemaining = timeout;
			spriteInfo.Visible = true;
			for (int i = 0; i < m_sprites.Count; i++)
			{
				if (m_sprites[i].SpriteId == spriteId)
				{
					m_sprites[i] = spriteInfo;
					return;
				}
			}
			m_sprites.Add(spriteInfo);
		}

		private void SetDefaults(bool clear)
		{
			if (clear)
			{
				m_sprites.Clear();
			}
			CreateDefaultSprite();
			m_rightVector = Vector2.UnitX;
		}

		private void CreateDefaultSprite()
		{
			SpriteInfo spriteInfo = default(SpriteInfo);
			spriteInfo.Color = MyHudConstants.HUD_COLOR_LIGHT;
			spriteInfo.FadeoutTime = 0;
			spriteInfo.HalfSize = Vector2.One * 0.02f;
			spriteInfo.SpriteId = m_defaultSpriteId;
			spriteInfo.SpriteEnum = MyHudTexturesEnum.crosshair;
			spriteInfo.TimeRemaining = 0;
			spriteInfo.Visible = true;
			bool flag = false;
			for (int i = 0; i < m_sprites.Count; i++)
			{
				if (m_sprites[i].SpriteId == m_defaultSpriteId)
				{
					m_sprites[i] = spriteInfo;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				m_sprites.Add(spriteInfo);
			}
		}

		public static bool GetTarget(Vector3D from, Vector3D to, ref Vector3D target)
		{
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(from, to, 15);
			if (hitInfo.HasValue)
			{
				target = hitInfo.Value.Position;
			}
			return hitInfo.HasValue;
		}

		public static bool GetProjectedTarget(Vector3D from, Vector3D to, ref Vector2 target)
		{
			Vector3D target2 = Vector3D.Zero;
			if (GetTarget(from, to, ref target2))
			{
				return GetProjectedVector(target2, ref target);
			}
			return false;
		}

		public void Update()
		{
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (m_lastGameplayTimeInMs == 0)
			{
				m_lastGameplayTimeInMs = totalGamePlayTimeInMilliseconds;
				return;
			}
			int num = totalGamePlayTimeInMilliseconds - m_lastGameplayTimeInMs;
			m_lastGameplayTimeInMs = totalGamePlayTimeInMilliseconds;
			for (int i = 0; i < m_sprites.Count; i++)
			{
				SpriteInfo value = m_sprites[i];
				if (!(value.SpriteId == m_defaultSpriteId))
				{
					value.TimeRemaining -= num;
					if (value.TimeRemaining <= 0)
					{
						m_sprites.RemoveAt(i);
						i--;
					}
					else
					{
						m_sprites[i] = value;
					}
				}
			}
		}

		public void Draw(string atlas, MyAtlasTextureCoordinate[] atlasCoords)
		{
			float x = (float)MyGuiManager.GetSafeFullscreenRectangle().Width / MyGuiManager.GetHudSize().X;
			float y = (float)MyGuiManager.GetSafeFullscreenRectangle().Height / MyGuiManager.GetHudSize().Y;
			Vector2 position = m_position;
			if (MyVideoSettingsManager.IsTripleHead())
			{
				position.X += 1f;
			}
			foreach (SpriteInfo sprite in m_sprites)
			{
				if (sprite.Visible)
				{
					int spriteEnum = (int)sprite.SpriteEnum;
					if (spriteEnum < atlasCoords.Length)
					{
						MyAtlasTextureCoordinate myAtlasTextureCoordinate = atlasCoords[spriteEnum];
						Color color = sprite.Color;
						if (sprite.TimeRemaining < sprite.FadeoutTime)
						{
							color.A = (byte)(color.A * sprite.TimeRemaining / sprite.FadeoutTime);
						}
						MyRenderProxy.DrawSpriteAtlas(atlas, position, myAtlasTextureCoordinate.Offset, myAtlasTextureCoordinate.Size, m_rightVector, new Vector2(x, y), color, sprite.HalfSize);
					}
				}
			}
			if (m_statControls != null && m_style != null)
			{
				Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
				Vector2 vector = new Vector2(fullscreenRectangle.Width, fullscreenRectangle.Height);
				Vector2 coordScreen = m_style.Position * vector;
				m_statControls.Position = (Position - ScreenCenter) / MyGuiManager.GetHudSize() * vector + MyUtils.AlignCoord(coordScreen, vector, m_style.OriginAlign);
				m_statControls.Draw(1f, 1f);
			}
		}

		public static bool GetProjectedVector(Vector3D worldPosition, ref Vector2 target)
		{
			Vector3D v = Vector3D.Transform(worldPosition, MySector.MainCamera.ViewMatrix);
			Vector4 vector = Vector4.Transform(v, MySector.MainCamera.ProjectionMatrix);
			if (v.Z > 0.0)
			{
				return false;
			}
			if (vector.W == 0f)
			{
				return false;
			}
			target = new Vector2(vector.X / vector.W / 2f + 0.5f, (0f - vector.Y) / vector.W / 2f + 0.5f);
			if (MyVideoSettingsManager.IsTripleHead())
			{
				target.X = (target.X - 0.333333343f) / 0.333333343f;
			}
			target.Y *= MyGuiManager.GetHudSize().Y;
			return true;
		}

		public void RecreateControls(bool constructor)
		{
			MyHudDefinition hudDefinition = MyHud.HudDefinition;
			m_style = hudDefinition.Crosshair;
			if (m_style != null)
			{
				InitStatControls();
			}
		}

		private void InitStatControls()
		{
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			Vector2 vector = new Vector2(fullscreenRectangle.Width, fullscreenRectangle.Height);
			float uiScale = MyGuiManager.GetSafeScreenScale() * MyHud.HudElementsScaleMultiplier;
			m_statControls = new MyStatControls(m_style, uiScale);
			Vector2 coordScreen = m_style.Position * vector;
			m_statControls.Position = (Position - ScreenCenter) / MyGuiManager.GetHudSize() * vector + MyUtils.AlignCoord(coordScreen, vector, m_style.OriginAlign);
		}
	}
}
