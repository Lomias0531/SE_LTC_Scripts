using System.Runtime.InteropServices;
using VRageMath;

namespace VRageRender.Messages
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MySpotLightLayout
	{
		public MyLightLayout Light;

		public Vector3 Up;

		public float ApertureCos;

		public Vector3 Direction;

		public float Resolution;
	}
}
