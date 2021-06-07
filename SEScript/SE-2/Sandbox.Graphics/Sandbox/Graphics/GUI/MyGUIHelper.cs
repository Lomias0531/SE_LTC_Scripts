using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyGUIHelper
	{
		public static bool Contains(Vector2 position, Vector2 size, float x, float y)
		{
			if (x >= position.X && y >= position.Y && x <= position.X + size.X)
			{
				return y <= position.Y + size.Y;
			}
			return false;
		}

		public static bool Intersects(Vector2 aPosition, Vector2 aSize, Vector2 bPosition, Vector2 bSize)
		{
			if ((!(aPosition.X > bPosition.X) || !(aPosition.X > bPosition.X + bSize.X)) && (!(aPosition.X + aSize.X < bPosition.X) || !(aPosition.X + aSize.X < bPosition.X + bSize.X)) && (!(aPosition.Y > bPosition.Y) || !(aPosition.Y > bPosition.Y + bSize.Y)))
			{
				if (aPosition.Y + aSize.Y < bPosition.Y)
				{
					return !(aPosition.Y + aSize.Y < bPosition.Y + bSize.Y);
				}
				return true;
			}
			return false;
		}

		public static void FillRectangle(Vector2 position, Vector2 size, Color color)
		{
			Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(position);
			Point point = new Point((int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y);
			Vector2 screenCoordinateFromNormalizedCoordinate2 = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(position + size);
			Point point2 = new Point((int)screenCoordinateFromNormalizedCoordinate2.X, (int)screenCoordinateFromNormalizedCoordinate2.Y);
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", point.X, point.Y, point2.X - point.X, point2.Y - point.Y, color);
		}

		private static void OffsetInnerBorder(Vector2 normalizedPosition, Vector2 normalizedSize, int pixelWidth, int offset, Color color, bool top = true, bool bottom = true, bool left = true, bool right = true, Vector2? normalizedOffset = null)
		{
			Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(normalizedPosition - (normalizedOffset.HasValue ? normalizedOffset.Value : Vector2.Zero));
			Point point = new Point((int)screenCoordinateFromNormalizedCoordinate.X - offset, (int)screenCoordinateFromNormalizedCoordinate.Y - offset);
			Vector2 screenCoordinateFromNormalizedCoordinate2 = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(normalizedPosition + normalizedSize + (normalizedOffset.HasValue ? normalizedOffset.Value : Vector2.Zero));
			Point point2 = new Point((int)screenCoordinateFromNormalizedCoordinate2.X + offset, (int)screenCoordinateFromNormalizedCoordinate2.Y + offset);
			if (top)
			{
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", point.X, point.Y, point2.X - point.X, pixelWidth, color);
			}
			if (bottom)
			{
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", point.X, point2.Y - pixelWidth, point2.X - point.X, pixelWidth, color);
			}
			if (left)
			{
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", point.X, point.Y + (top ? pixelWidth : 0), pixelWidth, point2.Y - point.Y - (bottom ? pixelWidth : 0) - (top ? pixelWidth : 0), color);
			}
			if (right)
			{
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", point2.X - pixelWidth, point.Y + (top ? pixelWidth : 0), pixelWidth, point2.Y - point.Y - (bottom ? pixelWidth : 0) - (top ? pixelWidth : 0), color);
			}
		}

		public static void OutsideBorder(Vector2 normalizedPosition, Vector2 normalizedSize, int pixelWidth, Color color, bool top = true, bool bottom = true, bool left = true, bool right = true)
		{
			OffsetInnerBorder(normalizedPosition, normalizedSize, pixelWidth, pixelWidth, color, top, bottom, left, right);
		}

		public static void InsideBorder(Vector2 normalizedPosition, Vector2 normalizedSize, int pixelWidth, Color color, bool top = true, bool bottom = true, bool left = true, bool right = true)
		{
			OffsetInnerBorder(normalizedPosition, normalizedSize, pixelWidth, 0, color, top, bottom, left, right);
		}

		public static void Border(Vector2 normalizedPosition, Vector2 normalizedSize, int pixelWidth, Color color, bool top = true, bool bottom = true, bool left = true, bool right = true, Vector2? normalizedOffset = null)
		{
			OffsetInnerBorder(normalizedPosition, normalizedSize, 2 * pixelWidth, pixelWidth, color, top, bottom, left, right, normalizedOffset);
		}

		public static Vector2 GetOffset(Vector2 basePosition, Vector2 baseSize, Vector2 itemPosition, Vector2 itemSize)
		{
			float x = 0f;
			float y = 0f;
			if (baseSize.X > itemSize.X)
			{
				if (basePosition.X + baseSize.X < itemPosition.X + itemSize.X)
				{
					x = basePosition.X + baseSize.X - (itemPosition.X + itemSize.X);
				}
				if (basePosition.X > itemPosition.X)
				{
					x = basePosition.X - itemPosition.X;
				}
			}
			if (baseSize.Y > itemSize.Y)
			{
				if (basePosition.Y + baseSize.Y < itemPosition.Y + itemSize.Y)
				{
					y = basePosition.Y + baseSize.Y - (itemPosition.Y + itemSize.Y);
				}
				if (basePosition.Y > itemPosition.Y)
				{
					y = basePosition.Y - itemPosition.Y;
				}
			}
			return new Vector2(x, y);
		}
	}
}
