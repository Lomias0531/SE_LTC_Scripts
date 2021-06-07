using VRage.Utils;
using VRageMath;

namespace VRage.Game.Graphics
{
	public class MyEmissiveColorState
	{
		public MyStringHash EmissiveColor;

		public MyStringHash DisplayColor;

		public float Emissivity;

		public MyEmissiveColorState(string emissiveColor, string displayColor, float emissivity)
		{
			EmissiveColor = MyStringHash.GetOrCompute(emissiveColor);
			DisplayColor = MyStringHash.GetOrCompute(displayColor);
			Emissivity = MyMath.Clamp(emissivity, 0f, 1f);
		}
	}
}
