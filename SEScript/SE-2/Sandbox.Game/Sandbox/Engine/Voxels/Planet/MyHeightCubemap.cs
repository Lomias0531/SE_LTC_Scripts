using VRage.Voxels;

namespace Sandbox.Engine.Voxels.Planet
{
	public class MyHeightCubemap : MyWrappedCubemap<MyHeightmapFace>
	{
		public MyHeightCubemap(string name, MyHeightmapFace[] faces, int resolution)
			: base(name, resolution, faces)
		{
		}

		public unsafe VrPlanetShape.Mapset GetMapset()
		{
			VrPlanetShape.Mapset result = default(VrPlanetShape.Mapset);
			result.Front = m_faces[0].Data;
			result.Back = m_faces[1].Data;
			result.Left = m_faces[2].Data;
			result.Right = m_faces[3].Data;
			result.Up = m_faces[4].Data;
			result.Down = m_faces[5].Data;
			result.Resolution = base.Resolution;
			return result;
		}
	}
}
