using System;

namespace VRage.Profiler
{
	public class MyDrawArea
	{
		public readonly float XStart;

		public readonly float YStart;

		public readonly float XScale;

		public readonly float YScale;

		public float YRange
		{
			get;
			private set;
		}

		public float YLegendMsIncrement
		{
			get;
			private set;
		}

		public int YLegendMsCount
		{
			get;
			private set;
		}

		public float YLegendIncrement
		{
			get;
			private set;
		}

		/// <summary>
		/// Index 0, 1, 2, 3, 4, 5...
		/// Makes range 1, 1.5, 2, 3, 4, 6, 8, 12, 24, 32, 48, 64...
		/// Negative index is supported as well.
		/// </summary>
		public int Index
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes draw area.
		/// </summary>
		/// <param name="yScale"></param>
		/// <param name="yRange">Range of y axis, will be rounded to 2^n or 2^n * 1.5</param>
		/// <param name="xStart"></param>
		/// <param name="yStart"></param>
		/// <param name="xScale"></param>
		public MyDrawArea(float xStart, float yStart, float xScale, float yScale, float yRange)
		{
			Index = (int)Math.Round(Math.Log(yRange, 2.0) * 2.0);
			XStart = xStart;
			YStart = yStart;
			XScale = xScale;
			YScale = yScale;
			UpdateRange();
		}

		public void IncreaseYRange()
		{
			Index++;
			UpdateRange();
		}

		public void DecreaseYRange()
		{
			Index--;
			UpdateRange();
		}

		public float GetYRange(int index)
		{
			return (float)Math.Pow(2.0, index / 2) * (1f + (float)(index % 2) * ((index < 0) ? 0.25f : 0.5f));
		}

		private void UpdateRange()
		{
			YRange = GetYRange(Index);
			YLegendMsCount = ((Index % 2 == 0) ? 8 : 12);
			YLegendMsIncrement = YRange / (float)YLegendMsCount;
			YLegendIncrement = YScale / YRange * YLegendMsIncrement;
		}
	}
}
