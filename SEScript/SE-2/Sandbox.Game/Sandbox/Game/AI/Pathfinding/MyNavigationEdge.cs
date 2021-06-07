using VRage.Algorithms;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyNavigationEdge : IMyPathEdge<MyNavigationPrimitive>
	{
		public static MyNavigationEdge Static = new MyNavigationEdge();

		private MyNavigationPrimitive m_triA;

		private MyNavigationPrimitive m_triB;

		private int m_index;

		public int Index => m_index;

		public void Init(MyNavigationPrimitive triA, MyNavigationPrimitive triB, int index)
		{
			m_triA = triA;
			m_triB = triB;
			m_index = index;
		}

		public float GetWeight()
		{
			return (m_triA.Position - m_triB.Position).Length() * 1f;
		}

		public MyNavigationPrimitive GetOtherVertex(MyNavigationPrimitive vertex1)
		{
			if (vertex1 == m_triA)
			{
				return m_triB;
			}
			return m_triA;
		}
	}
}
