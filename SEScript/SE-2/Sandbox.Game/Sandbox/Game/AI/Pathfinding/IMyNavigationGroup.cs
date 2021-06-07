using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public interface IMyNavigationGroup
	{
		MyHighLevelGroup HighLevelGroup
		{
			get;
		}

		int GetExternalNeighborCount(MyNavigationPrimitive primitive);

		MyNavigationPrimitive GetExternalNeighbor(MyNavigationPrimitive primitive, int index);

		IMyPathEdge<MyNavigationPrimitive> GetExternalEdge(MyNavigationPrimitive primitive, int index);

		void RefinePath(MyPath<MyNavigationPrimitive> path, List<Vector4D> output, ref Vector3 startPoint, ref Vector3 endPoint, int begin, int end);

		Vector3 GlobalToLocal(Vector3D globalPos);

		Vector3D LocalToGlobal(Vector3 localPos);

		MyHighLevelPrimitive GetHighLevelPrimitive(MyNavigationPrimitive myNavigationTriangle);

		IMyHighLevelComponent GetComponent(MyHighLevelPrimitive highLevelPrimitive);

		MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, ref double closestDistanceSq);
	}
}
