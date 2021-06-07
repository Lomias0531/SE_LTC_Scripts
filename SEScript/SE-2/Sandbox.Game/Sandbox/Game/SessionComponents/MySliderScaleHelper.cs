using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	public class MySliderScaleHelper
	{
		public static float RescaleUp(float value)
		{
			float num = value * 1.2f * 2f;
			if (num % 1f <= 0.001f)
			{
				return 0.5f * num;
			}
			return 0.5f * (num + 1f - num % 1f);
		}

		public static float RescaleDown(float value)
		{
			float num = value * 0.8f * 2f;
			return 0.5f * (num - num % 1f);
		}

		public static void ScaleSliderUp(ref MyBrushGUIPropertyNumberSlider slider)
		{
			slider.Value = MyMath.Clamp(RescaleUp(slider.Value), slider.ValueMin, slider.ValueMax);
		}

		public static void ScaleSliderDown(ref MyBrushGUIPropertyNumberSlider slider)
		{
			slider.Value = MyMath.Clamp(RescaleDown(slider.Value), slider.ValueMin, slider.ValueMax);
		}
	}
}
