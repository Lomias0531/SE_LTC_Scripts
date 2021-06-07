using System;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiSliderPropertiesExponential : MyGuiSliderProperties
	{
		public MyGuiSliderPropertiesExponential(float min, float max, float exponent = 10f, bool integer = false)
		{
			float maxLog = (float)(Math.Log(max) / Math.Log(exponent));
			float minLog = (float)(Math.Log(min) / Math.Log(exponent));
			FormatLabel = ((float x) => $"{x:N0}m");
			ValueToRatio = ((float x) => (float)((Math.Log(x) / Math.Log(exponent) - (double)minLog) / (double)(maxLog - minLog)));
			RatioToValue = delegate(float x)
			{
				double num = Math.Pow(exponent, x * (maxLog - minLog) + minLog);
				return (!integer) ? ((float)num) : ((float)(int)num);
			};
			RatioFilter = ((float x) => x);
		}
	}
}
