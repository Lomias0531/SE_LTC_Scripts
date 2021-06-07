using System;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyBasicObstacle : IMyObstacle
	{
		public MatrixD m_worldInv;

		public Vector3D m_halfExtents;

		private MyEntity m_entity;

		private bool m_valid;

		public bool Valid => m_valid;

		public MyBasicObstacle(MyEntity entity)
		{
			m_entity = entity;
			m_entity.OnClosing += OnEntityClosing;
			Update();
			m_valid = true;
		}

		public bool Contains(ref Vector3D point)
		{
			Vector3D.Transform(ref point, ref m_worldInv, out Vector3D result);
			if (Math.Abs(result.X) < m_halfExtents.X && Math.Abs(result.Y) < m_halfExtents.Y)
			{
				return Math.Abs(result.Z) < m_halfExtents.Z;
			}
			return false;
		}

		public void Update()
		{
			m_worldInv = m_entity.PositionComp.WorldMatrixNormalizedInv;
			m_halfExtents = m_entity.PositionComp.LocalAABB.Extents;
		}

		public void DebugDraw()
		{
			MatrixD matrix = MatrixD.Invert(m_worldInv);
			MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(MatrixD.CreateScale(m_halfExtents) * matrix), Color.Red, 0.3f, depthRead: false, smooth: false);
		}

		private void OnEntityClosing(MyEntity entity)
		{
			m_valid = false;
			m_entity = null;
		}
	}
}
