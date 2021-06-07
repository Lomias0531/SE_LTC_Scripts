using VRageMath;

namespace Sandbox.Game.Lights
{
	internal class MyDirectionalLight
	{
		public Vector3 Direction;

		public Vector4 Color;

		public Vector3 BackColor;

		public Vector3 SpecularColor = Vector3.One;

		public float Intensity;

		public float BackIntensity;

		public bool LightOn;

		public void Start()
		{
			LightOn = true;
			Intensity = 1f;
			BackIntensity = 0.1f;
		}

		public void Start(Vector3 direction, Vector4 color, Vector3 backColor)
		{
			Start();
			Direction = direction;
			Color = color;
			BackColor = backColor;
		}
	}
}
