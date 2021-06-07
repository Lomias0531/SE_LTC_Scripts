using VRage;
using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
	public class MyPlanetSectorId
	{
		private const long CoordMask = 16777215L;

		private const int CoordBits = 24;

		private const int FaceOffset = 48;

		private const long FaceMask = 7L;

		private const int FaceBits = 3;

		private const long LodMask = 255L;

		private const int LodBits = 8;

		private const int LodOffset = 51;

		private MyPlanetSectorId()
		{
		}

		public static long MakeSectorEntityId(int x, int y, int lod, int face, long parentId)
		{
			return MyEntityIdentifier.ConstructIdFromString(MyEntityIdentifier.ID_OBJECT_TYPE.PLANET_ENVIRONMENT_SECTOR, $"P({parentId})S(x{x}, y{y}, f{face}, l{lod})");
		}

		public static long MakeSectorId(int x, int y, int face, int lod = 0)
		{
			return ((long)x & 16777215L) | (((long)y & 16777215L) << 24) | (((long)face & 7L) << 48) | (((long)lod & 255L) << 51);
		}

		public static Vector3I DecomposeSectorId(long sectorID)
		{
			return new Vector3I((int)(sectorID & 0xFFFFFF), (sectorID >> 24) & 0xFFFFFF, (sectorID >> 48) & 7);
		}

		public static int GetFace(long packedSectorId)
		{
			return (int)((packedSectorId >> 48) & 7);
		}
	}
}
