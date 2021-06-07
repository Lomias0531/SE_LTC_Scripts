using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyHighLevelPrimitive : MyNavigationPrimitive
	{
		private MyHighLevelGroup m_parent;

		private List<int> m_neighbors;

		private int m_index;

		private Vector3 m_position;

		public bool IsExpanded
		{
			get;
			set;
		}

		public int Index => m_index;

		public override Vector3 Position => m_position;

		public override Vector3D WorldPosition => m_parent.LocalToGlobal(m_position);

		public MyHighLevelGroup Parent => m_parent;

		public override IMyNavigationGroup Group => m_parent;

		public MyHighLevelPrimitive(MyHighLevelGroup parent, int index, Vector3 position)
		{
			m_parent = parent;
			m_neighbors = new List<int>(4);
			m_index = index;
			m_position = position;
			IsExpanded = false;
		}

		public override string ToString()
		{
			return "(" + m_parent.ToString() + ")[" + m_index + "]";
		}

		public void GetNeighbours(List<int> output)
		{
			output.Clear();
			output.AddRange(m_neighbors);
		}

		public void Connect(int other)
		{
			m_neighbors.Add(other);
		}

		public void Disconnect(int other)
		{
			m_neighbors.Remove(other);
		}

		public void UpdatePosition(Vector3 position)
		{
			m_position = position;
		}

		public IMyHighLevelComponent GetComponent()
		{
			return m_parent.LowLevelGroup.GetComponent(this);
		}

		public override int GetOwnNeighborCount()
		{
			return m_neighbors.Count;
		}

		public override IMyPathVertex<MyNavigationPrimitive> GetOwnNeighbor(int index)
		{
			return m_parent.GetPrimitive(m_neighbors[index]);
		}

		public override IMyPathEdge<MyNavigationPrimitive> GetOwnEdge(int index)
		{
			MyNavigationEdge.Static.Init(this, GetOwnNeighbor(index) as MyNavigationPrimitive, 0);
			return MyNavigationEdge.Static;
		}

		public override MyHighLevelPrimitive GetHighLevelPrimitive()
		{
			return null;
		}
	}
}
