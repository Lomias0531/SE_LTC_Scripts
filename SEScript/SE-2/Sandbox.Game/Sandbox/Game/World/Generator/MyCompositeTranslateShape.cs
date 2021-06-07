using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyCompositeTranslateShape : IMyCompositeShape
	{
		private Vector3 m_translation;

		private readonly IMyCompositeShape m_shape;

		public MyCompositeTranslateShape(IMyCompositeShape shape, Vector3 translation)
		{
			m_shape = shape;
			m_translation = -translation;
		}

		public ContainmentType Contains(ref BoundingBox queryBox, ref BoundingSphere querySphere, int lodVoxelSize)
		{
			BoundingBox queryBox2 = queryBox;
			queryBox2.Translate(m_translation);
			BoundingSphere querySphere2 = querySphere.Translate(ref m_translation);
			return m_shape.Contains(ref queryBox2, ref querySphere2, lodVoxelSize);
		}

		public float SignedDistance(ref Vector3 localPos, int lodVoxelSize)
		{
			Vector3 localPos2 = localPos + m_translation;
			return m_shape.SignedDistance(ref localPos2, lodVoxelSize);
		}

		public void ComputeContent(MyStorageData storage, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax, int lodVoxelSize)
		{
			Vector3I b = (Vector3I)m_translation >> lodIndex;
			m_shape.ComputeContent(storage, lodIndex, lodVoxelRangeMin + b, lodVoxelRangeMax + b, lodVoxelSize);
		}

		public void DebugDraw(ref MatrixD worldMatrix, Color color)
		{
			MatrixD worldMatrix2 = MatrixD.CreateTranslation(-m_translation) * worldMatrix;
			m_shape.DebugDraw(ref worldMatrix2, color);
		}

		public void Close()
		{
			m_shape.Close();
		}
	}
}
