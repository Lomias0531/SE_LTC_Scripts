using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyProceduralCell
	{
		public int proxyId = -1;

		private MyDynamicAABBTreeD m_tree = new MyDynamicAABBTreeD(Vector3D.Zero);

		public Vector3I CellId
		{
			get;
			private set;
		}

		public BoundingBoxD BoundingVolume
		{
			get;
			private set;
		}

		public void AddObject(MyObjectSeed objectSeed)
		{
			BoundingBoxD aabb = objectSeed.BoundingVolume;
			objectSeed.m_proxyId = m_tree.AddProxy(ref aabb, objectSeed, 0u);
		}

		public MyProceduralCell(Vector3I cellId, double cellSize)
		{
			CellId = cellId;
			BoundingVolume = new BoundingBoxD(CellId * cellSize, (CellId + 1) * cellSize);
		}

		public void OverlapAllBoundingSphere(ref BoundingSphereD sphere, List<MyObjectSeed> list, bool clear = false)
		{
			m_tree.OverlapAllBoundingSphere(ref sphere, list, clear);
		}

		public void OverlapAllBoundingBox(ref BoundingBoxD box, List<MyObjectSeed> list, bool clear = false)
		{
			m_tree.OverlapAllBoundingBox(ref box, list, 0u, clear);
		}

		public void GetAll(List<MyObjectSeed> list, bool clear = true)
		{
			m_tree.GetAll(list, clear);
		}

		public override int GetHashCode()
		{
			return CellId.GetHashCode();
		}

		public override string ToString()
		{
			return CellId.ToString();
		}
	}
}
