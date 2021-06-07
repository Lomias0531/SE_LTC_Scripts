using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public interface IMyCompositeShape
	{
		ContainmentType Contains(ref BoundingBox queryBox, ref BoundingSphere querySphere, int lodVoxelSize);

		float SignedDistance(ref Vector3 localPos, int lodVoxelSize);

		void ComputeContent(MyStorageData storage, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax, int lodVoxelSize);

		void DebugDraw(ref MatrixD worldMatrix, Color color);

		void Close();
	}
}
