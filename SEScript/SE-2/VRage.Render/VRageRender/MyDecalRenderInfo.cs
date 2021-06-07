using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	public struct MyDecalRenderInfo
	{
		public MyDecalFlags Flags;

		public Vector3D Position;

		public Vector3 Normal;

		public Vector4UByte BoneIndices;

		public Vector4 BoneWeights;

		public MyDecalBindingInfo? Binding;

		public uint[] RenderObjectIds;

		public MyStringHash Material;

		public MyStringHash Source;
	}
}
