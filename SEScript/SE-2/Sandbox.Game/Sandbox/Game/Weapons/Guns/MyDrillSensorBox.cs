using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons.Guns
{
	internal class MyDrillSensorBox : MyDrillSensorBase
	{
		private Vector3 m_halfExtents;

		private float m_centerOffset;

		private Quaternion m_orientation;

		public MyDrillSensorBox(Vector3 halfExtents, float centerOffset)
		{
			m_halfExtents = halfExtents;
			m_centerOffset = centerOffset;
			base.Center = Vector3.Forward * centerOffset;
			base.FrontPoint = base.Center + Vector3.Forward * m_halfExtents.Z;
		}

		public override void OnWorldPositionChanged(ref MatrixD worldMatrix)
		{
			m_orientation = Quaternion.CreateFromRotationMatrix(worldMatrix.GetOrientation());
			base.Center = worldMatrix.Translation + worldMatrix.Forward * m_centerOffset;
			base.FrontPoint = base.Center + worldMatrix.Forward * m_halfExtents.Z;
		}

		protected override void ReadEntitiesInRange()
		{
			m_entitiesInRange.Clear();
			BoundingBox boundingBox = new MyOrientedBoundingBox(base.Center, m_halfExtents, m_orientation).GetAABB();
			List<MyEntity> entitiesInAABB = MyEntities.GetEntitiesInAABB(ref boundingBox);
			for (int i = 0; i < entitiesInAABB.Count; i++)
			{
				MyEntity topMostParent = entitiesInAABB[i].GetTopMostParent();
				if (!IgnoredEntities.Contains(topMostParent))
				{
					m_entitiesInRange[topMostParent.EntityId] = new DetectionInfo(topMostParent, base.FrontPoint);
				}
			}
			entitiesInAABB.Clear();
		}

		public override void DebugDraw()
		{
			MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(base.Center, m_halfExtents, m_orientation);
			Vector3 v = new Vector3(1f, 0f, 0f);
			MyRenderProxy.DebugDrawOBB(obb, v, 0.6f, depthRead: true, smooth: false);
		}
	}
}
