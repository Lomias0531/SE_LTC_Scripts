using System.Runtime.InteropServices;
using VRage.Import;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage
{
	/// <summary>
	/// Format for vertices of renderable voxel meshes
	///
	/// Before changing this format check how it is used in the renderer
	/// and be sure to change both places to match.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct MyVertexFormatVoxelSingleData
	{
		[FieldOffset(0)]
		public Vector3 Position;

		/// <summary>
		/// For multimaterial vertex only
		/// 0, 1 or 2, indicates what material is on this vertex
		/// </summary>
		[FieldOffset(12)]
		public Byte4 Material;

		[FieldOffset(16)]
		public Byte4 PackedNormal;

		[FieldOffset(20)]
		public uint PackedColorShift;

		public Vector3 Normal
		{
			get
			{
				return VF_Packer.UnpackNormal(ref PackedNormal);
			}
			set
			{
				PackedNormal.PackedValue = VF_Packer.PackNormal(ref value);
			}
		}
	}
}
