using System;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public struct MyGuiBorderThickness
	{
		public float Left;

		public float Right;

		public float Top;

		public float Bottom;

		public float HorizontalSum => Left + Right;

		public float VerticalSum => Top + Bottom;

		public Vector2 TopLeftOffset => new Vector2(Left, Top);

		public Vector2 TopRightOffset => new Vector2(0f - Right, Top);

		public Vector2 BottomLeftOffset => new Vector2(Left, 0f - Bottom);

		public Vector2 BottomRightOffset => new Vector2(0f - Right, 0f - Bottom);

		public Vector2 SizeChange => new Vector2(HorizontalSum, VerticalSum);

		public Vector2 MarginStep => new Vector2(Math.Max(Left, Right), Math.Max(Top, Bottom));

		public MyGuiBorderThickness(float val = 0f)
		{
			Left = (Right = (Top = (Bottom = val)));
		}

		public MyGuiBorderThickness(float horizontal, float vertical)
		{
			Left = (Right = horizontal);
			Top = (Bottom = vertical);
		}

		public MyGuiBorderThickness(float left, float right, float top, float bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}
	}
}
