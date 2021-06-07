using VRageMath;

namespace VRage
{
	public struct MyCameraSetup
	{
		public MatrixD ViewMatrix;

		public Vector3D Position;

		public float FarPlane;

		public float FOV;

		public float NearPlane;

		public Matrix ProjectionMatrix;

		public float ProjectionOffsetX;

		public float ProjectionOffsetY;

		public float AspectRatio;
	}
}
