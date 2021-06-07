using System;
using VRageMath;

namespace VRageRender
{
	public interface IMyDebugDrawBatchAabb : IDisposable
	{
		void Add(ref BoundingBoxD aabb, Color? color = null);
	}
}
