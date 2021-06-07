using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyGridObstacle : IMyObstacle
	{
		private List<BoundingBox> m_segments;

		private static MyVoxelSegmentation m_segmentation = new MyVoxelSegmentation();

		private MatrixD m_worldInv;

		private MyCubeGrid m_grid;

		public MyGridObstacle(MyCubeGrid grid)
		{
			m_grid = grid;
			Segment();
			Update();
		}

		private void Segment()
		{
			m_segmentation.ClearInput();
			foreach (MySlimBlock cubeBlock in m_grid.CubeBlocks)
			{
				Vector3I start = cubeBlock.Min;
				Vector3I end = cubeBlock.Max;
				Vector3I next = start;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				while (vector3I_RangeIterator.IsValid())
				{
					m_segmentation.AddInput(next);
					vector3I_RangeIterator.GetNext(out next);
				}
			}
			List<MyVoxelSegmentation.Segment> list = m_segmentation.FindSegments(MyVoxelSegmentationType.Simple2);
			m_segments = new List<BoundingBox>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				BoundingBox item = default(BoundingBox);
				item.Min = (new Vector3(list[i].Min) - Vector3.Half) * m_grid.GridSize - Vector3.Half;
				item.Max = (new Vector3(list[i].Max) + Vector3.Half) * m_grid.GridSize + Vector3.Half;
				m_segments.Add(item);
			}
			m_segmentation.ClearInput();
		}

		public bool Contains(ref Vector3D point)
		{
			Vector3D.Transform(ref point, ref m_worldInv, out Vector3D result);
			Vector3 normal = MyGravityProviderSystem.CalculateNaturalGravityInPoint(m_grid.PositionComp.WorldAABB.Center);
			normal = Vector3.TransformNormal(normal, m_worldInv);
			if (!Vector3.IsZero(normal))
			{
				normal = Vector3.Normalize(normal);
				Ray ray = new Ray(result, -normal * 2f);
				foreach (BoundingBox segment in m_segments)
				{
					if (segment.Intersects(ray).HasValue)
					{
						return true;
					}
				}
			}
			else
			{
				foreach (BoundingBox segment2 in m_segments)
				{
					if (segment2.Contains(result) == ContainmentType.Contains)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void Update()
		{
			Segment();
			m_worldInv = m_grid.PositionComp.WorldMatrixNormalizedInv;
		}

		public void DebugDraw()
		{
			MatrixD matrix = MatrixD.Invert(m_worldInv);
			Quaternion orientation = Quaternion.CreateFromRotationMatrix(matrix.GetOrientation());
			foreach (BoundingBox segment in m_segments)
			{
				Vector3D halfExtents = new Vector3D(segment.Size) * 0.51;
				Vector3D position = new Vector3D(segment.Min + segment.Max) * 0.5;
				position = Vector3D.Transform(position, matrix);
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(position, halfExtents, orientation), Color.Red, 0.5f, depthRead: false, smooth: false);
			}
		}
	}
}
