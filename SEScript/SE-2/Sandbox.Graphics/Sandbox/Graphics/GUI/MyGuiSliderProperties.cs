using System;
using System.Globalization;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiSliderProperties
	{
		public Func<float, float> RatioToValue;

		public Func<float, float> ValueToRatio;

		public Func<float, float> RatioFilter;

		public Func<float, string> FormatLabel;

		public static MyGuiSliderProperties Default = new MyGuiSliderProperties
		{
			ValueToRatio = ((float f) => f),
			RatioToValue = ((float f) => f),
			RatioFilter = ((float f) => f),
			FormatLabel = ((float f) => f.ToString(CultureInfo.CurrentCulture))
		};
	}
}
