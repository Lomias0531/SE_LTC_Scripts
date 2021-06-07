using System.Diagnostics;

namespace VRageRender
{
	[DebuggerDisplay("{Width}x{Height}@{RefreshRate}Hz")]
	public struct MyDisplayMode
	{
		public int Width;

		public int Height;

		public int RefreshRate;

		public int? RefreshRateDenominator;

		public float RefreshRateF
		{
			get
			{
				if (!RefreshRateDenominator.HasValue)
				{
					return RefreshRate;
				}
				return (float)RefreshRate / (float)RefreshRateDenominator.Value;
			}
		}

		public MyDisplayMode(int width, int height, int refreshRate, int? refreshRateDenominator = null)
		{
			Width = width;
			Height = height;
			RefreshRate = refreshRate;
			RefreshRateDenominator = refreshRateDenominator;
		}

		public override string ToString()
		{
			if (RefreshRateDenominator.HasValue)
			{
				return $"{Width}x{Height}@{(float)RefreshRate / (float)RefreshRateDenominator.Value}Hz";
			}
			return $"{Width}x{Height}@{RefreshRate}Hz";
		}
	}
}
