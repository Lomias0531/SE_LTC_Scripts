using System;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public interface IMyWrappedCubemapFace : IDisposable
	{
		int Resolution
		{
			get;
		}

		int ResolutionMinusOne
		{
			get;
		}

		void CopyRange(Vector2I start, Vector2I end, IMyWrappedCubemapFace other, Vector2I oStart, Vector2I oEnd);

		void FinishFace(string name);
	}
}
