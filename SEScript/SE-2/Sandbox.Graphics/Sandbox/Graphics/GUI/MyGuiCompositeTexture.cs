using System;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiCompositeTexture
	{
		private MyGuiSizedTexture m_leftTop;

		private MyGuiSizedTexture m_leftCenter;

		private MyGuiSizedTexture m_leftBottom;

		private MyGuiSizedTexture m_centerTop;

		private MyGuiSizedTexture m_center;

		private MyGuiSizedTexture m_centerBottom;

		private MyGuiSizedTexture m_rightTop;

		private MyGuiSizedTexture m_rightCenter;

		private MyGuiSizedTexture m_rightBottom;

		private bool m_sizeLimitsDirty;

		private Vector2 m_minSizeGui = Vector2.Zero;

		private Vector2 m_maxSizeGui = Vector2.One * float.PositiveInfinity;

		public MyGuiSizedTexture LeftTop
		{
			get
			{
				return m_leftTop;
			}
			set
			{
				m_leftTop = value;
				m_sizeLimitsDirty = true;
			}
		}

		public MyGuiSizedTexture LeftCenter
		{
			get
			{
				return m_leftCenter;
			}
			set
			{
				m_leftCenter = value;
				m_sizeLimitsDirty = true;
			}
		}

		public MyGuiSizedTexture LeftBottom
		{
			get
			{
				return m_leftBottom;
			}
			set
			{
				m_leftBottom = value;
				m_sizeLimitsDirty = true;
			}
		}

		public MyGuiSizedTexture CenterTop
		{
			get
			{
				return m_centerTop;
			}
			set
			{
				m_centerTop = value;
				m_sizeLimitsDirty = true;
			}
		}

		public MyGuiSizedTexture Center
		{
			get
			{
				return m_center;
			}
			set
			{
				m_center = value;
				m_sizeLimitsDirty = true;
			}
		}

		public MyGuiSizedTexture CenterBottom
		{
			get
			{
				return m_centerBottom;
			}
			set
			{
				m_centerBottom = value;
				m_sizeLimitsDirty = true;
			}
		}

		public MyGuiSizedTexture RightTop
		{
			get
			{
				return m_rightTop;
			}
			set
			{
				m_rightTop = value;
				m_sizeLimitsDirty = true;
			}
		}

		public MyGuiSizedTexture RightCenter
		{
			get
			{
				return m_rightCenter;
			}
			set
			{
				m_rightCenter = value;
				m_sizeLimitsDirty = true;
			}
		}

		public MyGuiSizedTexture RightBottom
		{
			get
			{
				return m_rightBottom;
			}
			set
			{
				m_rightBottom = value;
				m_sizeLimitsDirty = true;
			}
		}

		public Vector2 MinSizeGui
		{
			get
			{
				if (m_sizeLimitsDirty)
				{
					RefreshSizeLimits();
				}
				return m_minSizeGui;
			}
		}

		public Vector2 MaxSizeGui
		{
			get
			{
				if (m_sizeLimitsDirty)
				{
					RefreshSizeLimits();
				}
				return m_maxSizeGui;
			}
		}

		public void MarkDirty()
		{
			m_sizeLimitsDirty = true;
		}

		private void RefreshSizeLimits()
		{
			m_minSizeGui.X = Math.Max(m_leftTop.SizeGui.X + m_rightTop.SizeGui.X, m_leftBottom.SizeGui.X + m_rightBottom.SizeGui.X);
			m_minSizeGui.Y = Math.Max(m_leftTop.SizeGui.Y + m_leftBottom.SizeGui.Y, m_rightTop.SizeGui.Y + m_rightBottom.SizeGui.Y);
			if (m_center.Texture != null)
			{
				m_maxSizeGui.X = float.PositiveInfinity;
				m_maxSizeGui.Y = float.PositiveInfinity;
			}
			else
			{
				m_maxSizeGui.X = ((m_centerTop.Texture != null || m_centerBottom.Texture != null) ? float.PositiveInfinity : m_minSizeGui.X);
				m_maxSizeGui.Y = ((m_leftCenter.Texture != null || m_rightCenter.Texture != null) ? float.PositiveInfinity : m_minSizeGui.Y);
				if (m_leftTop.Texture == null && m_centerTop.Texture == null && m_rightTop.Texture == null && m_leftCenter.Texture == null && m_center.Texture == null && m_rightCenter.Texture == null && m_leftBottom.Texture == null && m_centerBottom.Texture == null && m_rightBottom.Texture == null)
				{
					m_maxSizeGui = Vector2.PositiveInfinity;
				}
			}
			m_sizeLimitsDirty = false;
		}

		public MyGuiCompositeTexture(string centerTexture = null)
		{
			Center = new MyGuiSizedTexture
			{
				Texture = centerTexture
			};
		}

		public void Draw(Vector2 positionTopLeft, float innerHeight, Color colorMask)
		{
			positionTopLeft = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(positionTopLeft);
			Rectangle rectangle = default(Rectangle);
			rectangle.X = (int)positionTopLeft.X;
			rectangle.Y = (int)positionTopLeft.Y;
			rectangle.Width = 0;
			rectangle.Height = 0;
			if (!string.IsNullOrEmpty(m_leftTop.Texture))
			{
				Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(m_leftTop.SizeGui);
				rectangle.Width = (int)screenSizeFromNormalizedSize.X;
				rectangle.Height = (int)screenSizeFromNormalizedSize.Y;
				MyGuiManager.DrawSprite(m_leftTop.Texture, rectangle, colorMask);
			}
			rectangle.Y += rectangle.Height;
			if (!string.IsNullOrEmpty(m_leftCenter.Texture))
			{
				Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(new Vector2(m_leftCenter.SizeGui.X, innerHeight));
				rectangle.Width = (int)screenSizeFromNormalizedSize.X;
				rectangle.Height = (int)screenSizeFromNormalizedSize.Y;
				MyGuiManager.DrawSprite(m_leftCenter.Texture, rectangle, colorMask);
			}
			rectangle.Y += rectangle.Height;
			if (!string.IsNullOrEmpty(m_leftBottom.Texture))
			{
				Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(m_leftBottom.SizeGui);
				rectangle.Width = (int)screenSizeFromNormalizedSize.X;
				rectangle.Height = (int)screenSizeFromNormalizedSize.Y;
				MyGuiManager.DrawSprite(m_leftBottom.Texture, rectangle, colorMask);
			}
		}

		public void Draw(Vector2 positionLeftTop, Vector2 size, Color colorMask, float textureScale = 1f)
		{
			size = Vector2.Clamp(size, MinSizeGui * textureScale, MaxSizeGui * textureScale);
			Vector2I screenSize2;
			Vector2I screenSize;
			Vector2I screenSize3 = screenSize2 = (screenSize = Vector2I.Zero);
			Vector2I screenSize4;
			Vector2I screenSize5 = screenSize4 = Vector2I.Zero;
			Vector2I screenSize7;
			Vector2I screenSize6;
			Vector2I screenSize8 = screenSize7 = (screenSize6 = Vector2I.Zero);
			Vector2I vector2I = new Vector2I(MyGuiManager.GetScreenSizeFromNormalizedSize(size));
			Vector2I screenPos = new Vector2I(MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(positionLeftTop));
			Vector2I screenPos2 = screenPos + vector2I;
			Vector2I screenPos3 = new Vector2I(screenPos.X, screenPos2.Y);
			Vector2I screenPos4 = new Vector2I(screenPos2.X, screenPos.Y);
			Rectangle target;
			if (!string.IsNullOrEmpty(m_leftTop.Texture))
			{
				screenSize3 = (Vector2I)(MyGuiManager.GetScreenSizeFromNormalizedSize(m_leftTop.SizeGui) * textureScale);
				SetTargetRectangle(out target, ref screenPos, ref screenSize3, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				MyGuiManager.DrawSprite(m_leftTop.Texture, target, colorMask);
			}
			if (!string.IsNullOrEmpty(m_leftBottom.Texture))
			{
				screenSize8 = (Vector2I)(MyGuiManager.GetScreenSizeFromNormalizedSize(m_leftBottom.SizeGui) * textureScale);
				SetTargetRectangle(out target, ref screenPos3, ref screenSize8, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				MyGuiManager.DrawSprite(m_leftBottom.Texture, target, colorMask);
			}
			if (!string.IsNullOrEmpty(m_rightTop.Texture))
			{
				screenSize = (Vector2I)(MyGuiManager.GetScreenSizeFromNormalizedSize(m_rightTop.SizeGui) * textureScale);
				SetTargetRectangle(out target, ref screenPos4, ref screenSize, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				MyGuiManager.DrawSprite(m_rightTop.Texture, target, colorMask);
			}
			if (!string.IsNullOrEmpty(m_rightBottom.Texture))
			{
				screenSize6 = (Vector2I)(MyGuiManager.GetScreenSizeFromNormalizedSize(m_rightBottom.SizeGui) * textureScale);
				SetTargetRectangle(out target, ref screenPos2, ref screenSize6, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				MyGuiManager.DrawSprite(m_rightBottom.Texture, target, colorMask);
			}
			if (!string.IsNullOrEmpty(m_centerTop.Texture))
			{
				screenSize2.X = vector2I.X - (screenSize3.X + screenSize.X);
				screenSize2.Y = (int)(MyGuiManager.GetScreenSizeFromNormalizedSize(m_centerTop.SizeGui).Y * textureScale);
				Vector2I screenPos5 = screenPos + new Vector2I(screenSize3.X, 0);
				SetTargetRectangle(out target, ref screenPos5, ref screenSize2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				MyGuiManager.DrawSprite(m_centerTop.Texture, target, colorMask);
			}
			if (!string.IsNullOrEmpty(m_centerBottom.Texture))
			{
				screenSize7.X = vector2I.X - (screenSize8.X + screenSize6.X);
				screenSize7.Y = (int)(MyGuiManager.GetScreenSizeFromNormalizedSize(m_centerBottom.SizeGui).Y * textureScale);
				Vector2I screenPos6 = screenPos3 + new Vector2I(screenSize8.X, 0);
				SetTargetRectangle(out target, ref screenPos6, ref screenSize7, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				MyGuiManager.DrawSprite(m_centerBottom.Texture, target, colorMask);
			}
			if (!string.IsNullOrEmpty(m_leftCenter.Texture))
			{
				screenSize5.X = (int)(MyGuiManager.GetScreenSizeFromNormalizedSize(m_leftCenter.SizeGui).X * textureScale);
				screenSize5.Y = vector2I.Y - (screenSize3.Y + screenSize8.Y);
				Vector2I screenPos7 = screenPos + new Vector2I(0, screenSize3.Y);
				SetTargetRectangle(out target, ref screenPos7, ref screenSize5, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				MyGuiManager.DrawSprite(m_leftCenter.Texture, target, colorMask);
			}
			if (!string.IsNullOrEmpty(m_rightCenter.Texture))
			{
				screenSize4.X = (int)(MyGuiManager.GetScreenSizeFromNormalizedSize(m_rightCenter.SizeGui).X * textureScale);
				screenSize4.Y = vector2I.Y - (screenSize.Y + screenSize6.Y);
				Vector2I screenPos8 = screenPos4 + new Vector2I(0, screenSize.Y);
				SetTargetRectangle(out target, ref screenPos8, ref screenSize4, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				MyGuiManager.DrawSprite(m_rightCenter.Texture, target, colorMask);
			}
			if (!string.IsNullOrEmpty(m_center.Texture))
			{
				int num = MathHelper.Max(screenSize3.X, screenSize5.X, screenSize8.X);
				int num2 = MathHelper.Max(screenSize.X, screenSize4.X, screenSize6.X);
				int num3 = MathHelper.Max(screenSize3.Y, screenSize2.Y, screenSize.Y);
				int num4 = MathHelper.Max(screenSize8.Y, screenSize7.Y, screenSize6.Y);
				Vector2I screenSize9 = vector2I - new Vector2I(num + num2, num3 + num4);
				Vector2I screenPos9 = screenPos + new Vector2I(num, num3);
				SetTargetRectangle(out target, ref screenPos9, ref screenSize9, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				MyGuiManager.DrawSprite(m_center.Texture, target, colorMask);
			}
		}

		private static void SetTargetRectangle(out Rectangle target, ref Vector2I screenPos, ref Vector2I screenSize, MyGuiDrawAlignEnum posAlign)
		{
			Vector2I coordTopLeftFromAligned = MyUtils.GetCoordTopLeftFromAligned(screenPos, screenSize, posAlign);
			target.X = coordTopLeftFromAligned.X;
			target.Y = coordTopLeftFromAligned.Y;
			target.Width = screenSize.X;
			target.Height = screenSize.Y;
		}

		public static MyGuiCompositeTexture CreateFromDefinition(MyStringHash textureKey)
		{
			MyGuiCompositeTexture myGuiCompositeTexture = new MyGuiCompositeTexture();
			MyObjectBuilder_CompositeTexture compositeTexture = MyGuiTextures.Static.GetCompositeTexture(textureKey);
			if (compositeTexture == null)
			{
				return null;
			}
			MyGuiSizedTexture myGuiSizedTexture;
			if (MyGuiTextures.Static.TryGetTexture(compositeTexture.Center, out MyObjectBuilder_GuiTexture texture))
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture.Path,
					SizePx = texture.SizePx
				};
				myGuiCompositeTexture.Center = myGuiSizedTexture;
			}
			if (MyGuiTextures.Static.TryGetTexture(compositeTexture.LeftBottom, out texture))
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture.Path,
					SizePx = texture.SizePx
				};
				myGuiCompositeTexture.LeftBottom = myGuiSizedTexture;
			}
			if (MyGuiTextures.Static.TryGetTexture(compositeTexture.LeftTop, out texture))
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture.Path,
					SizePx = texture.SizePx
				};
				myGuiCompositeTexture.LeftTop = myGuiSizedTexture;
			}
			if (MyGuiTextures.Static.TryGetTexture(compositeTexture.RightCenter, out texture))
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture.Path,
					SizePx = texture.SizePx
				};
				myGuiCompositeTexture.RightCenter = myGuiSizedTexture;
			}
			if (MyGuiTextures.Static.TryGetTexture(compositeTexture.RightBottom, out texture))
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture.Path,
					SizePx = texture.SizePx
				};
				myGuiCompositeTexture.RightBottom = myGuiSizedTexture;
			}
			if (MyGuiTextures.Static.TryGetTexture(compositeTexture.RightTop, out texture))
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture.Path,
					SizePx = texture.SizePx
				};
				myGuiCompositeTexture.RightTop = myGuiSizedTexture;
			}
			if (MyGuiTextures.Static.TryGetTexture(compositeTexture.CenterBottom, out texture))
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture.Path,
					SizePx = texture.SizePx
				};
				myGuiCompositeTexture.CenterBottom = myGuiSizedTexture;
			}
			if (MyGuiTextures.Static.TryGetTexture(compositeTexture.CenterTop, out texture))
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = texture.Path,
					SizePx = texture.SizePx
				};
				myGuiCompositeTexture.CenterTop = myGuiSizedTexture;
			}
			return myGuiCompositeTexture;
		}
	}
}
