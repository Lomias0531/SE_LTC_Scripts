using VRageMath;

namespace VRageRender
{
	public static class MyRenderProxyUtils
	{
		public static Vector2I GetFixedWindowResolution(Vector2I inResolution, MyAdapterInfo adapterInfo)
		{
			Vector2I right = new Vector2I(150);
			Vector2I v = new Vector2I(adapterInfo.DesktopBounds.Width, adapterInfo.DesktopBounds.Height) - right;
			v = Vector2I.Min(inResolution, v);
			if (inResolution == v)
			{
				return v;
			}
			Vector2I vector2I = Vector2I.Zero;
			MyDisplayMode[] supportedDisplayModes = adapterInfo.SupportedDisplayModes;
			for (int i = 0; i < supportedDisplayModes.Length; i++)
			{
				MyDisplayMode myDisplayMode = supportedDisplayModes[i];
				Vector2I vector2I2 = new Vector2I(myDisplayMode.Width, myDisplayMode.Height);
				if (vector2I2.X <= v.X && vector2I2.Y <= v.Y)
				{
					float num = vector2I2.X * vector2I2.Y;
					float num2 = vector2I.X * vector2I.Y;
					if (num > num2)
					{
						vector2I = vector2I2;
					}
				}
			}
			if (vector2I != Vector2I.Zero)
			{
				v = vector2I;
			}
			return v;
		}
	}
}
