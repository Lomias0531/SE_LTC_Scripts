using VRage.Game;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyObjectSeed
	{
		public MyObjectSeedParams Params = new MyObjectSeedParams();

		public int m_proxyId = -1;

		public BoundingBoxD BoundingVolume
		{
			get;
			private set;
		}

		public float Size
		{
			get;
			private set;
		}

		public MyProceduralCell Cell
		{
			get;
			private set;
		}

		public Vector3I CellId => Cell.CellId;

		public object UserData
		{
			get;
			set;
		}

		public MyObjectSeed()
		{
		}

		public MyObjectSeed(MyProceduralCell cell, Vector3D position, double size)
		{
			Cell = cell;
			Size = (float)size;
			BoundingVolume = new BoundingBoxD(position - Size, position + Size);
		}
	}
}
