using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public static class My2DClipmapHelpers
	{
		public static readonly Vector2D[] CoordsFromIndex = new Vector2D[4]
		{
			Vector2D.Zero,
			Vector2D.UnitX,
			Vector2D.UnitY,
			Vector2D.One
		};

		public static readonly Color[] LodColors = new Color[12]
		{
			Color.Red,
			Color.Green,
			Color.Blue,
			Color.Yellow,
			Color.Magenta,
			Color.Cyan,
			new Color(1f, 0.5f, 0f),
			new Color(1f, 0f, 0.5f),
			new Color(0.5f, 0f, 1f),
			new Color(0.5f, 1f, 0f),
			new Color(0f, 1f, 0.5f),
			new Color(0f, 0.5f, 1f)
		};
	}
}
