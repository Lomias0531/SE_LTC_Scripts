using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public abstract class MyNavigationPrimitive : IMyPathVertex<MyNavigationPrimitive>, IEnumerable<IMyPathEdge<MyNavigationPrimitive>>, IEnumerable
	{
		private MyPathfindingData m_pathfindingData;

		private bool m_externalNeighbors;

		public MyPathfindingData PathfindingData => m_pathfindingData;

		public bool HasExternalNeighbors
		{
			set
			{
				m_externalNeighbors = value;
			}
		}

		public abstract Vector3 Position
		{
			get;
		}

		public abstract Vector3D WorldPosition
		{
			get;
		}

		public abstract IMyNavigationGroup Group
		{
			get;
		}

		protected MyNavigationPrimitive()
		{
			m_pathfindingData = new MyPathfindingData(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		float IMyPathVertex<MyNavigationPrimitive>.EstimateDistanceTo(IMyPathVertex<MyNavigationPrimitive> other)
		{
			MyNavigationPrimitive myNavigationPrimitive = other as MyNavigationPrimitive;
			if (Group == myNavigationPrimitive.Group)
			{
				return Vector3.Distance(Position, myNavigationPrimitive.Position);
			}
			return (float)Vector3D.Distance(WorldPosition, myNavigationPrimitive.WorldPosition);
		}

		int IMyPathVertex<MyNavigationPrimitive>.GetNeighborCount()
		{
			int ownNeighborCount = GetOwnNeighborCount();
			if (!m_externalNeighbors)
			{
				return ownNeighborCount;
			}
			return ownNeighborCount + Group.GetExternalNeighborCount(this);
		}

		IMyPathVertex<MyNavigationPrimitive> IMyPathVertex<MyNavigationPrimitive>.GetNeighbor(int index)
		{
			int ownNeighborCount = GetOwnNeighborCount();
			IMyPathVertex<MyNavigationPrimitive> myPathVertex = null;
			if (index < ownNeighborCount)
			{
				return GetOwnNeighbor(index);
			}
			return Group.GetExternalNeighbor(this, index - ownNeighborCount);
		}

		IMyPathEdge<MyNavigationPrimitive> IMyPathVertex<MyNavigationPrimitive>.GetEdge(int index)
		{
			int ownNeighborCount = GetOwnNeighborCount();
			IMyPathEdge<MyNavigationPrimitive> myPathEdge = null;
			if (index < ownNeighborCount)
			{
				return GetOwnEdge(index);
			}
			return Group.GetExternalEdge(this, index - ownNeighborCount);
		}

		public virtual Vector3 ProjectLocalPoint(Vector3 point)
		{
			return Position;
		}

		public abstract int GetOwnNeighborCount();

		public abstract IMyPathVertex<MyNavigationPrimitive> GetOwnNeighbor(int index);

		public abstract IMyPathEdge<MyNavigationPrimitive> GetOwnEdge(int index);

		public abstract MyHighLevelPrimitive GetHighLevelPrimitive();

		public IEnumerator<IMyPathEdge<MyNavigationPrimitive>> GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
