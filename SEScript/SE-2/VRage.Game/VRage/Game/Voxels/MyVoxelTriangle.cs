using System;
using System.Runtime.InteropServices;

namespace VRage.Game.Voxels
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MyVoxelTriangle
	{
		public ushort V0;

		public ushort V1;

		public ushort V2;

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
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		public MyVoxelTriangle(ushort v0, ushort v1, ushort v2)
		{
			V0 = v0;
			V1 = v1;
			V2 = v2;
		}

		public override string ToString()
		{
			return "{" + V0 + ", " + V1 + ", " + V2 + "}";
		}
	}
}
