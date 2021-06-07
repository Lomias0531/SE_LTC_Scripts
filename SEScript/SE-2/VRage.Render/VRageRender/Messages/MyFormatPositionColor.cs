using VRageMath;

namespace VRageRender.Messages
{
	public struct MyFormatPositionColor
	{
		public Vector3 Position;

		public Color Color;

		public MyFormatPositionColor(Vector3 position, Color color)
		{
			Position = position;
			Color = color;
		}
	}
}
