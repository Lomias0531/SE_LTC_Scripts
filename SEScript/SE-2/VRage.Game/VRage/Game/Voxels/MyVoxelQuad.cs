using System;
using System.Runtime.InteropServices;

namespace VRage.Game.Voxels
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MyVoxelQuad
	{
		public ushort V0;

		public ushort V1;

		public ushort V2;

		public ushort V3;

		public ushort this[int i]
		{
			get
			{
				switch (i)
				{
				case 0:
					return V0;
				case 1:
					return V1;
				case 2:
					return V2;
				case 3:
					return V3;
				default:
					throw new IndexOutOfRangeException();
				}
			}
			set
			{
				switch (i)
				{
				case 0:
					V0 = value;
					break;
				case 1:
					V1 = value;
					break;
				case 2:
					V2 = value;
					break;
				case 3:
					V3 = value;
					break;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		public MyVoxelQuad(ushort v0, ushort v1, ushort v2, ushort v3)
		{
			V0 = v0;
			V1 = v1;
			V2 = v2;
			V3 = v3;
		}

		public int IndexOf(int vx)
		{
			if (vx == V0)
			{
				return 0;
			}
			if (vx == V1)
			{
				return 1;
			}
			if (vx == V2)
			{
				return 2;
			}
			if (vx == V3)
			{
				return 3;
			}
			return -1;
		}

		public override string ToString()
		{
			return "{" + V0 + ", " + V1 + ", " + V2 + ", " + V3 + "}";
		}
	}
}
