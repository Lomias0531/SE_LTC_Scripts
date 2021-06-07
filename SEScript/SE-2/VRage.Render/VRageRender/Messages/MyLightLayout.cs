using System.Runtime.InteropServices;
using VRageMath;

namespace VRageRender.Messages
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MyLightLayout
	{
		public Vector3 Position;

		public float Range;

		public Vector3 Color;

		public float Falloff;

		public float GlossFactor;

		public float DiffuseFactor;

		public Vector2 _pad;
	}
}
