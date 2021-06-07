using System.Runtime.InteropServices;
using VRageMath.PackedVector;

namespace VRage.Game.Models
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MyCompressedBoneIndicesWeights
	{
		public HalfVector4 Weights;

		public Byte4 Indices;
	}
}
