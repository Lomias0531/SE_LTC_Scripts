using System.Diagnostics;

namespace Sandbox.Graphics.GUI
{
	[DebuggerDisplay("MinSize = { MinSize }")]
	public class ShadowTexture
	{
		public string Texture
		{
			get;
			private set;
		}

		public float MinWidth
		{
			get;
			private set;
		}

		public float GrowFactorWidth
		{
			get;
			private set;
		}

		public float GrowFactorHeight
		{
			get;
			private set;
		}

		public float DefaultAlpha
		{
			get;
			private set;
		}

		public ShadowTexture(string texture, float minwidth, float growFactorWidth, float growFactorHeight, float defaultAlpha)
		{
			Texture = texture;
			MinWidth = minwidth;
			GrowFactorWidth = growFactorWidth;
			GrowFactorHeight = growFactorHeight;
			DefaultAlpha = defaultAlpha;
		}
	}
}
