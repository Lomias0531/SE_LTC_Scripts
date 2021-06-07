using VRageMath;

namespace VRageRender.Messages
{
	public struct MyDecalTopoData
	{
		public Matrix MatrixBinding;

		public Vector3D WorldPosition;

		public Matrix MatrixCurrent;

		public Vector4UByte BoneIndices;

		public Vector4 BoneWeights;
	}
}
