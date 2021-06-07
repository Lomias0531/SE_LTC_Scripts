using VRageMath;

namespace VRageRender
{
	public struct MyViewport
	{
		public float OffsetX;

		public float OffsetY;

		public float Width;

		public float Height;

		public MyViewport(float width, float height)
		{
			OffsetX = 0f;
			OffsetY = 0f;
			Width = width;
			Height = height;
		}

		public MyViewport(Vector2I resolution)
		{
			OffsetX = 0f;
			OffsetY = 0f;
			Width = resolution.X;
			Height = resolution.Y;
		}

		public MyViewport(float x, float y, float width, float height)
		{
			OffsetX = x;
			OffsetY = y;
			Width = width;
			Height = height;
		}
	}
}
