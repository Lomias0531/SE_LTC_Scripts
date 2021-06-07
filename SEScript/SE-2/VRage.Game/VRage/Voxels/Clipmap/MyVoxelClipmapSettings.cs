using System.Collections.Generic;

namespace VRage.Voxels.Clipmap
{
	/// <summary>
	/// Descriptor for the configurable parameters of a clipmap.
	/// </summary>
	public struct MyVoxelClipmapSettings
	{
		/// <summary>
		/// Log base 2 of the cell size, this is expressed as such since cells are required to be a power of two in size.
		/// </summary>
		public int CellSizeLg2;

		/// <summary>
		/// Ranges for each lod.
		///
		/// There must be exactly MyClipmap.LodCount entries in the array and they must be such that LodRanges[i] &lt;= LodRanges[i + 1].
		/// </summary>
		public int[] LodRanges;

		/// <summary>
		/// Default settings for the clipmap.
		/// </summary>
		public static MyVoxelClipmapSettings Default = Create(4, 2, 2f, 6, 16384);

		private static readonly Dictionary<string, MyVoxelClipmapSettings> m_settingsPerGroup = new Dictionary<string, MyVoxelClipmapSettings>();

		/// <summary>
		/// Whether the contents of the settinmmgs are valid.
		/// </summary>
		public bool IsValid
		{
			get
			{
				if (LodRanges == null || LodRanges.Length != 16)
				{
					return false;
				}
				for (int i = 1; i < 16; i++)
				{
					if (LodRanges[i - 1] > LodRanges[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool Equals(MyVoxelClipmapSettings other)
		{
			if (CellSizeLg2 == other.CellSizeLg2)
			{
				return Equals(LodRanges, other.LodRanges);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is MyVoxelClipmapSettings)
			{
				return Equals((MyVoxelClipmapSettings)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (CellSizeLg2 * 397) ^ ((LodRanges != null) ? LodRanges.GetHashCode() : 0);
		}

		private static bool Equals(int[] left, int[] right)
		{
			if (left == right)
			{
				return true;
			}
			if (left.Length != right.Length)
			{
				return false;
			}
			for (int i = 0; i < left.Length; i++)
			{
				if (left[i] != right[i])
				{
					return false;
				}
			}
			return true;
		}

		public static int[] MakeRanges(int lod0Cells, float higherLodCells, int cellSizeLg2, int lastLod = -1, int lastLodRange = -1)
		{
			int[] array = (int[])(object)new int[16];
			int num = 1 << cellSizeLg2;
			int num2 = array[0] = lod0Cells * num;
			for (int i = 1; i < array.Length; i++)
			{
				if (lastLod != -1 && lastLod == i)
				{
					array[i] = lastLodRange;
					continue;
				}
				float num3 = (float)array[i - 1] * higherLodCells;
				if (num3 > 2.14748365E+09f)
				{
					array[i] = int.MaxValue;
				}
				else
				{
					array[i] = (int)num3;
				}
			}
			return array;
		}

		/// <summary>
		/// Create a settings instance from simpler parameters.
		/// </summary>
		/// <param name="cellBits"></param>
		/// <param name="lod0Size"></param>
		/// <param name="lodSize"></param>
		/// <returns></returns>
		public static MyVoxelClipmapSettings Create(int cellBits, int lod0Size, float lodSize, int lastLod = -1, int lastLodRange = -1)
		{
			MyVoxelClipmapSettings result = default(MyVoxelClipmapSettings);
			result.CellSizeLg2 = cellBits;
			result.LodRanges = MakeRanges(lod0Size, lodSize, cellBits, lastLod, lastLodRange);
			return result;
		}

		/// <summary>
		/// Update or add clipmap settings for a group.
		/// </summary>
		///
		/// Settings gropups are named sets of settings used when there are different settings required for different clipmaps.
		/// Generally we use that to distinguish between 'small' and planet sized voxel maps.
		///
		/// <param name="group">Name of the group.</param>
		/// <param name="settings">the settings for the group.</param>
		public static void SetSettingsForGroup(string group, MyVoxelClipmapSettings settings)
		{
			m_settingsPerGroup[group] = settings;
		}

		/// <summary>
		/// Get settings for a named group.
		/// </summary>
		/// <param name="settingsGroup">The name of the group.</param>
		/// <returns>The group settings or default.</returns>
		public static MyVoxelClipmapSettings GetSettings(string settingsGroup)
		{
			if (m_settingsPerGroup.TryGetValue(settingsGroup, out MyVoxelClipmapSettings value))
			{
				return value;
			}
			return Default;
		}
	}
}
