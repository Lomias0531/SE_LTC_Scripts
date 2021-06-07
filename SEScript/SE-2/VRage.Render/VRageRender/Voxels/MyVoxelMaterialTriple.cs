using System.Collections.Generic;

namespace VRageRender.Voxels
{
	public struct MyVoxelMaterialTriple
	{
		private class comp : IEqualityComparer<MyVoxelMaterialTriple>
		{
			public bool Equals(MyVoxelMaterialTriple x, MyVoxelMaterialTriple y)
			{
				if (x.I0 == y.I0 && x.I1 == y.I1)
				{
					return x.I2 == y.I2;
				}
				return false;
			}

			public int GetHashCode(MyVoxelMaterialTriple obj)
			{
				return ((obj.I0 << 16) | (obj.I1 << 8) | obj.I2).GetHashCode();
			}
		}

		public byte I0;

		public byte I1;

		public byte I2;

		public static readonly IEqualityComparer<MyVoxelMaterialTriple> Comparer = new comp();

		public bool MultiMaterial => I1 != byte.MaxValue;

		public bool SingleMaterial => I1 == byte.MaxValue;

		public MyVoxelMaterialTriple(int i0, int i1, int i2)
		{
			I0 = (byte)((i0 == -1) ? 255 : i0);
			I1 = (byte)((i1 == -1) ? 255 : i1);
			I2 = (byte)((i2 == -1) ? 255 : i2);
		}

		public MyVoxelMaterialTriple(byte i0, byte i1, byte i2)
		{
			I0 = i0;
			I1 = i1;
			I2 = i2;
		}

		internal bool IsMultimaterial()
		{
			if (I1 == byte.MaxValue)
			{
				return I2 != byte.MaxValue;
			}
			return true;
		}
	}
}
