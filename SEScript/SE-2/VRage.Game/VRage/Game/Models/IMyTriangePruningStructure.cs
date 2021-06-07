using System.Collections.Generic;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
	public interface IMyTriangePruningStructure
	{
		int Size
		{
			get;
		}

		MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(IMyEntity entity, ref LineD line, IntersectionFlags flags = IntersectionFlags.DIRECT_TRIANGLES);

		MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(IMyEntity entity, ref LineD line, ref MatrixD customInvMatrix, IntersectionFlags flags = IntersectionFlags.DIRECT_TRIANGLES);

		void GetTrianglesIntersectingLine(IMyEntity entity, ref LineD line, ref MatrixD customInvMatrix, IntersectionFlags flags, List<MyIntersectionResultLineTriangleEx> result);

		void GetTrianglesIntersectingLine(IMyEntity entity, ref LineD line, IntersectionFlags flags, List<MyIntersectionResultLineTriangleEx> result);

		void GetTrianglesIntersectingSphere(ref BoundingSphere sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles);

		bool GetIntersectionWithSphere(IMyEntity physObject, ref BoundingSphereD sphere);

		bool GetIntersectionWithSphere(ref BoundingSphere localSphere);

		bool GetIntersectionWithAABB(IMyEntity physObject, ref BoundingBoxD aabb);

		void GetTrianglesIntersectingAABB(ref BoundingBox sphere, List<MyTriangle_Vertex_Normal> retTriangles, int maxNeighbourTriangles);

		void Close();
	}
}
