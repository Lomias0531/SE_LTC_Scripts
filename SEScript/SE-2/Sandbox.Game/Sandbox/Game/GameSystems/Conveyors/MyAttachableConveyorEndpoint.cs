using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Algorithms;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public class MyAttachableConveyorEndpoint : MyMultilineConveyorEndpoint
	{
		private class MyAttachableLine : IMyPathEdge<IMyConveyorEndpoint>
		{
			private MyAttachableConveyorEndpoint m_endpoint1;

			private MyAttachableConveyorEndpoint m_endpoint2;

			public MyAttachableLine(MyAttachableConveyorEndpoint endpoint1, MyAttachableConveyorEndpoint endpoint2)
			{
				m_endpoint1 = endpoint1;
				m_endpoint2 = endpoint2;
			}

			public float GetWeight()
			{
				return 2f;
			}

			public IMyConveyorEndpoint GetOtherVertex(IMyConveyorEndpoint vertex1)
			{
				if (vertex1 == m_endpoint1)
				{
					return m_endpoint2;
				}
				return m_endpoint1;
			}

			public bool Contains(MyAttachableConveyorEndpoint endpoint)
			{
				if (endpoint != m_endpoint1)
				{
					return endpoint == m_endpoint2;
				}
				return true;
			}
		}

		private List<MyAttachableLine> m_lines;

		public MyAttachableConveyorEndpoint(MyCubeBlock block)
			: base(block)
		{
			m_lines = new List<MyAttachableLine>();
		}

		public void Attach(MyAttachableConveyorEndpoint other)
		{
			MyAttachableLine line = new MyAttachableLine(this, other);
			AddAttachableLine(line);
			other.AddAttachableLine(line);
		}

		public void Detach(MyAttachableConveyorEndpoint other)
		{
			int num = 0;
			MyAttachableLine myAttachableLine;
			while (true)
			{
				if (num < m_lines.Count)
				{
					myAttachableLine = m_lines[num];
					if (myAttachableLine.Contains(other))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			RemoveAttachableLine(myAttachableLine);
			other.RemoveAttachableLine(myAttachableLine);
		}

		public void DetachAll()
		{
			for (int i = 0; i < m_lines.Count; i++)
			{
				MyAttachableLine myAttachableLine = m_lines[i];
				(myAttachableLine.GetOtherVertex(this) as MyAttachableConveyorEndpoint).RemoveAttachableLine(myAttachableLine);
			}
			m_lines.Clear();
		}

		private void AddAttachableLine(MyAttachableLine line)
		{
			m_lines.Add(line);
		}

		private void RemoveAttachableLine(MyAttachableLine line)
		{
			m_lines.Remove(line);
		}

		public bool AlreadyAttachedTo(MyAttachableConveyorEndpoint other)
		{
			foreach (MyAttachableLine line in m_lines)
			{
				if (line.GetOtherVertex(this) == other)
				{
					return true;
				}
			}
			return false;
		}

		public bool AlreadyAttached()
		{
			return m_lines.Count != 0;
		}

		protected override int GetNeighborCount()
		{
			return base.GetNeighborCount() + m_lines.Count;
		}

		protected override IMyPathVertex<IMyConveyorEndpoint> GetNeighbor(int index)
		{
			int neighborCount = base.GetNeighborCount();
			if (index < neighborCount)
			{
				return base.GetNeighbor(index);
			}
			return m_lines[index - neighborCount].GetOtherVertex(this);
		}

		protected override IMyPathEdge<IMyConveyorEndpoint> GetEdge(int index)
		{
			int neighborCount = base.GetNeighborCount();
			if (index < neighborCount)
			{
				return base.GetEdge(index);
			}
			return m_lines[index - neighborCount];
		}
	}
}
