using Sandbox.Engine.Physics;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public interface IMyPlacementProvider
	{
		Vector3D RayStart
		{
			get;
		}

		Vector3D RayDirection
		{
			get;
		}

		MyPhysics.HitInfo? HitInfo
		{
			get;
		}

		MyCubeGrid ClosestGrid
		{
			get;
		}

		MyVoxelBase ClosestVoxelMap
		{
			get;
		}

		bool CanChangePlacementObjectSize
		{
			get;
		}

		float IntersectionDistance
		{
			get;
			set;
		}

		void RayCastGridCells(MyCubeGrid grid, List<Vector3I> outHitPositions, Vector3I gridSizeInflate, float maxDist);

		void UpdatePlacement();
	}
}
