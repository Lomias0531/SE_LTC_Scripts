using VRage.Library;
using VRageMath;

namespace VRage.Voxels
{
	public static class MyVoxelConstants
	{
		public const string FILE_EXTENSION = ".vx2";

		public const float VOXEL_SIZE_IN_METRES = 1f;

		public const float VOXEL_VOLUME_IN_METERS = 1f;

		public const float VOXEL_SIZE_IN_METRES_HALF = 0.5f;

		public static readonly Vector3 VOXEL_SIZE_VECTOR = new Vector3(1f, 1f, 1f);

		public static readonly Vector3 VOXEL_SIZE_VECTOR_HALF = VOXEL_SIZE_VECTOR / 2f;

		public static readonly float VOXEL_RADIUS = VOXEL_SIZE_VECTOR_HALF.Length();

		public const int DATA_CELL_SIZE_IN_VOXELS_BITS = 3;

		public const int DATA_CELL_SIZE_IN_VOXELS = 8;

		public const int DATA_CELL_SIZE_IN_VOXELS_MASK = 7;

		public const int DATA_CELL_SIZE_IN_VOXELS_TOTAL = 512;

		public const int DATA_CELL_CONTENT_SUM_TOTAL = 130560;

		public const float DATA_CELL_SIZE_IN_METRES = 8f;

		public const int GEOMETRY_CELL_SIZE_IN_VOXELS_BITS = 3;

		public const int GEOMETRY_CELL_SIZE_IN_VOXELS = 8;

		public const int GEOMETRY_CELL_SIZE_IN_VOXELS_TOTAL = 512;

		public const int GEOMETRY_CELL_MAX_TRIANGLES_COUNT = 2560;

		public const float GEOMETRY_CELL_SIZE_IN_METRES = 8f;

		public const float GEOMETRY_CELL_SIZE_IN_METRES_HALF = 4f;

		public static readonly Vector3 GEOMETRY_CELL_SIZE_VECTOR_IN_METRES = new Vector3(8f);

		public static readonly int GEOMETRY_CELL_CACHE_SIZE = MyEnvironment.Is64BitProcess ? 262144 : 78643;

		public const float DEFAULT_WRINKLE_WEIGHT_ADD = 0.5f;

		public const float DEFAULT_WRINKLE_WEIGHT_REMOVE = 0.45f;

		public const int VOXEL_GENERATOR_VERSION = 4;

		public const int VOXEL_GENERATOR_MIN_ICE_VERSION = 1;

		public const int PRIORITY_IGNORE_EXTRACTION = -1;

		public const int PRIORITY_PLANET = 0;

		public const int PRIORITY_NORMAL = 1;

		public const int RenderCellBits = 3;

		public const int RenderCellSize = 8;

		public const byte VOXEL_ISO_LEVEL = 127;

		public const byte VOXEL_CONTENT_EMPTY = 0;

		public const byte VOXEL_CONTENT_FULL = byte.MaxValue;

		public const float VOXEL_CONTENT_FULL_FLOAT = 255f;

		public const byte NULL_MATERIAL = byte.MaxValue;

		private static readonly byte[] Defaults = new byte[3]
		{
			0,
			255,
			0
		};

		public static byte DefaultValue(MyStorageDataTypeEnum type)
		{
			return Defaults[(int)type];
		}
	}
}
