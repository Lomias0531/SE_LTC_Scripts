using VRageMath;

namespace VRage.Game
{
	public static class MyColorPickerConstants
	{
		public static readonly float SATURATION_DELTA = 0.8f;

		public static readonly float VALUE_DELTA = 0.55f;

		public static readonly float VALUE_COLORIZE_DELTA = 0.1f;

		public static Vector3 HSVOffsetToHSV(Vector3 hsvOffset)
		{
			return new Vector3(hsvOffset.X, MathHelper.Clamp(hsvOffset.Y + SATURATION_DELTA, 0f, 1f), MathHelper.Clamp(hsvOffset.Z + VALUE_DELTA - VALUE_COLORIZE_DELTA, 0f, 1f));
		}

		public static Vector3 HSVToHSVOffset(Vector3 hsv)
		{
			float y = hsv.Y - SATURATION_DELTA;
			float z = hsv.Z - VALUE_DELTA + VALUE_COLORIZE_DELTA;
			return new Vector3(hsv.X, y, z);
		}
	}
}
