using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Voxels
{
	public struct MyCellCoord : IComparable<MyCellCoord>, IEquatable<MyCellCoord>
	{
		public class EqualityComparer : IEqualityComparer<MyCellCoord>, IComparer<MyCellCoord>
		{
			public bool Equals(MyCellCoord x, MyCellCoord y)
			{
				if (x.CoordInLod.X == y.CoordInLod.X && x.CoordInLod.Y == y.CoordInLod.Y && x.CoordInLod.Z == y.CoordInLod.Z)
				{
					return x.Lod == y.Lod;
				}
				return false;
			}

			public int GetHashCode(MyCellCoord obj)
			{
				return (((((obj.CoordInLod.X * 397) ^ obj.CoordInLod.Y) * 397) ^ obj.CoordInLod.Z) * 397) ^ obj.Lod;
			}

			public int Compare(MyCellCoord x, MyCellCoord y)
			{
				return x.CompareTo(y);
			}
		}

		private const int BITS_LOD = 4;

		private const int BITS_X_32 = 10;

		private const int BITS_Y_32 = 8;

		private const int BITS_Z_32 = 10;

		private const int BITS_X_64 = 20;

		private const int BITS_Y_64 = 20;

		private const int BITS_Z_64 = 20;

		private const int SHIFT_Z_32 = 0;

		private const int SHIFT_Y_32 = 10;

		private const int SHIFT_X_32 = 18;

		private const int SHIFT_LOD_32 = 28;

		private const int SHIFT_Z_64 = 0;

		private const int SHIFT_Y_64 = 20;

		private const int SHIFT_X_64 = 40;

		private const int SHIFT_LOD_64 = 60;

		private const int MASK_LOD = 15;

		private const int MASK_X_32 = 1023;

		private const int MASK_Y_32 = 255;

		private const int MASK_Z_32 = 1023;

		private const int MASK_X_64 = 1048575;

		private const int MASK_Y_64 = 1048575;

		private const int MASK_Z_64 = 1048575;

		public const int MAX_LOD_COUNT = 16;

		/// <summary>
		/// 0 is the most detailed.
		/// </summary>
		public int Lod;

		public Vector3I CoordInLod;

		public static readonly EqualityComparer Comparer;

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is MyCellCoord)
			{
				return Equals((MyCellCoord)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (Lod * 397) ^ CoordInLod.GetHashCode();
		}

		public MyCellCoord(ulong packedId)
		{
			CoordInLod.Z = (int)(packedId & 0x3FF);
			packedId >>= 10;
			CoordInLod.Y = (int)(packedId & 0xFF);
			packedId >>= 8;
			CoordInLod.X = (int)(packedId & 0x3FF);
			packedId >>= 10;
			Lod = (int)packedId;
		}

		public MyCellCoord(int lod, Vector3I coordInLod)
			: this(lod, ref coordInLod)
		{
		}

		public MyCellCoord(int lod, ref Vector3I coordInLod)
		{
			Lod = lod;
			CoordInLod = coordInLod;
		}

		public void SetUnpack(uint id)
		{
			CoordInLod.Z = (int)(id & 0x3FF);
			id >>= 10;
			CoordInLod.Y = (int)(id & 0xFF);
			id >>= 8;
			CoordInLod.X = (int)(id & 0x3FF);
			id >>= 10;
			Lod = (int)id;
		}

		public void SetUnpack(ulong id)
		{
			CoordInLod.Z = (int)(id & 0xFFFFF);
			id >>= 20;
			CoordInLod.Y = (int)(id & 0xFFFFF);
			id >>= 20;
			CoordInLod.X = (int)(id & 0xFFFFF);
			id >>= 20;
			Lod = (int)id;
		}

		public static int UnpackLod(ulong id)
		{
			return (int)(id >> 60);
		}

		public static Vector3I UnpackCoord(ulong id)
		{
			Vector3I result = default(Vector3I);
			result.Z = (int)(id & 0xFFFFF);
			id >>= 20;
			result.Y = (int)(id & 0xFFFFF);
			id >>= 20;
			result.X = (int)(id & 0xFFFFF);
			id >>= 20;
			return result;
		}

		public static ulong PackId64Static(int lod, Vector3I coordInLod)
		{
			return (ulong)(((long)lod << 60) | ((long)coordInLod.X << 40) | ((long)coordInLod.Y << 20) | coordInLod.Z);
		}

		public uint PackId32()
		{
			return (uint)((Lod << 28) | (CoordInLod.X << 18) | (CoordInLod.Y << 10) | CoordInLod.Z);
		}

		public ulong PackId64()
		{
			return (ulong)(((long)Lod << 60) | ((long)CoordInLod.X << 40) | ((long)CoordInLod.Y << 20) | CoordInLod.Z);
		}

		public bool IsCoord64Valid()
		{
			if ((CoordInLod.X & 0xFFFFF) != CoordInLod.X)
			{
				return false;
			}
			if ((CoordInLod.Y & 0xFFFFF) != CoordInLod.Y)
			{
				return false;
			}
			if ((CoordInLod.Z & 0xFFFFF) != CoordInLod.Z)
			{
				return false;
			}
			return true;
		}

		public static ulong GetClipmapCellHash(uint clipmap, MyCellCoord cellId)
		{
			return GetClipmapCellHash(clipmap, cellId.PackId64());
		}

		public static ulong GetClipmapCellHash(uint clipmap, ulong cellId)
		{
			return (cellId * 997 * 397) ^ (clipmap * 997);
		}

		public static bool operator ==(MyCellCoord x, MyCellCoord y)
		{
			if (x.CoordInLod.X == y.CoordInLod.X && x.CoordInLod.Y == y.CoordInLod.Y && x.CoordInLod.Z == y.CoordInLod.Z)
			{
				return x.Lod == y.Lod;
			}
			return false;
		}

		public static bool operator !=(MyCellCoord x, MyCellCoord y)
		{
			if (x.CoordInLod.X == y.CoordInLod.X && x.CoordInLod.Y == y.CoordInLod.Y && x.CoordInLod.Z == y.CoordInLod.Z)
			{
				return x.Lod != y.Lod;
			}
			return true;
		}

		public bool Equals(MyCellCoord other)
		{
			return this == other;
		}

		public override string ToString()
		{
			return $"{Lod}, {CoordInLod}";
		}

		static MyCellCoord()
		{
			Comparer = new EqualityComparer();
		}

		public int CompareTo(MyCellCoord other)
		{
			int num = CoordInLod.X - other.CoordInLod.X;
			int num2 = CoordInLod.Y - other.CoordInLod.Y;
			int num3 = CoordInLod.Z - other.CoordInLod.Z;
			int result = Lod - other.Lod;
			if (num == 0)
			{
				if (num2 == 0)
				{
					if (num3 == 0)
					{
						return result;
					}
					return num3;
				}
				return num2;
			}
			return num;
		}
	}
}
