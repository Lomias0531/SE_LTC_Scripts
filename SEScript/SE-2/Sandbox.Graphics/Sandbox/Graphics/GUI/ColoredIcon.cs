using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public struct ColoredIcon
	{
		public string Icon;

		public Vector4 Color;

		public ColoredIcon(string icon, Vector4 color)
		{
			Icon = icon;
			Color = color;
		}
	}
}
