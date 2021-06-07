using VRage.Voxels.Sewing;

namespace VRage.Voxels.Mesh
{
	public static class MyStitchOperations
	{
		public static VrSewOperation GetInstruction(bool x = false, bool y = false, bool z = false, bool xy = false, bool xz = false, bool yz = false, bool xyz = false)
		{
			return (VrSewOperation)(0 | (x ? 2 : 0) | (y ? 4 : 0) | (z ? 6 : 0) | (xy ? 8 : 0) | (xz ? 10 : 0) | (yz ? 12 : 0) | (xyz ? 14 : 0));
		}

		public static bool Contains(this VrSewOperation self, VrSewOperation flags)
		{
			return (self & flags) == flags;
		}

		public static VrSewOperation Without(this VrSewOperation self, VrSewOperation flags)
		{
			return (VrSewOperation)((int)self & (int)(byte)(~(uint)flags));
		}

		public static VrSewOperation With(this VrSewOperation self, VrSewOperation flags)
		{
			return self | flags;
		}
	}
}
