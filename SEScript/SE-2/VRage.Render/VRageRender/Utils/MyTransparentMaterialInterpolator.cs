namespace VRageRender.Utils
{
	public static class MyTransparentMaterialInterpolator
	{
		public static void Switch(ref MyTransparentMaterial val1, ref MyTransparentMaterial val2, float time, out MyTransparentMaterial value)
		{
			value = ((time < 0.5f) ? val1 : val2);
		}
	}
}
