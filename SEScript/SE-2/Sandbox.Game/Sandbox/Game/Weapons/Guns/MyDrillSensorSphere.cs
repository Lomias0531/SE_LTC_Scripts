using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.WorldEnvironment;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons.Guns
{
	internal class MyDrillSensorSphere : MyDrillSensorBase
	{
		private float m_radius;

		private float m_centerForwardOffset;

		public MyDrillSensorSphere(float radius, float centerForwardOffset, MyDefinitionBase drillDefinition)
		{
			m_radius = radius;
			m_centerForwardOffset = centerForwardOffset;
			base.Center = centerForwardOffset * Vector3.Forward;
			base.FrontPoint = base.Center + Vector3.Forward * m_radius;
			m_drillDefinition = drillDefinition;
		}

		public override void OnWorldPositionChanged(ref MatrixD worldMatrix)
		{
			base.Center = worldMatrix.Translation + worldMatrix.Forward * m_centerForwardOffset;
			base.FrontPoint = base.Center + worldMatrix.Forward * m_radius;
		}

		protected override void ReadEntitiesInRange()
		{
			m_entitiesInRange.Clear();
			BoundingSphereD boundingSphere = new BoundingSphereD(base.Center, m_radius);
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
			bool flag = false;
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				if (item is MyEnvironmentSector)
				{
					flag = true;
				}
				if (!IgnoredEntities.Contains(item))
				{
					m_entitiesInRange[item.EntityId] = new DetectionInfo(item, base.FrontPoint);
				}
			}
			topMostEntitiesInSphere.Clear();
			if (!flag)
			{
				return;
			}
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(base.Center, base.FrontPoint, 24);
			if (!hitInfo.HasValue || !hitInfo.HasValue)
			{
				return;
			}
			IMyEntity hitEntity = hitInfo.Value.HkHitInfo.GetHitEntity();
			if (hitEntity is MyEnvironmentSector)
			{
				MyEnvironmentSector myEnvironmentSector = hitEntity as MyEnvironmentSector;
				uint shapeKey = hitInfo.Value.HkHitInfo.GetShapeKey(0);
				int itemFromShapeKey = myEnvironmentSector.GetItemFromShapeKey(shapeKey);
				if (myEnvironmentSector.DataView.Items[itemFromShapeKey].ModelIndex >= 0)
				{
					m_entitiesInRange[hitEntity.EntityId] = new DetectionInfo(myEnvironmentSector, base.FrontPoint, itemFromShapeKey);
				}
			}
		}

		public override void DebugDraw()
		{
			MyRenderProxy.DebugDrawSphere(base.Center, m_radius, Color.Yellow);
			MyRenderProxy.DebugDrawArrow3D(base.Center, base.FrontPoint, Color.Yellow, Color.Red);
		}
	}
}
