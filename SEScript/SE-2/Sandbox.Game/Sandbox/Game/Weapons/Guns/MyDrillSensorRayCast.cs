using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.WorldEnvironment;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons.Guns
{
	public class MyDrillSensorRayCast : MyDrillSensorBase
	{
		private static List<MyLineSegmentOverlapResult<MyEntity>> m_raycastResults = new List<MyLineSegmentOverlapResult<MyEntity>>();

		private float m_rayLength;

		private float m_originOffset;

		private Vector3D m_origin;

		private List<MyPhysics.HitInfo> m_hits;

		public MyDrillSensorRayCast(float originOffset, float rayLength, MyDefinitionBase drillDefinition)
		{
			m_rayLength = rayLength;
			m_originOffset = originOffset;
			m_hits = new List<MyPhysics.HitInfo>();
			m_drillDefinition = drillDefinition;
		}

		public override void OnWorldPositionChanged(ref MatrixD worldMatrix)
		{
			Vector3D forward = worldMatrix.Forward;
			m_origin = worldMatrix.Translation + forward * m_originOffset;
			base.FrontPoint = m_origin + m_rayLength * forward;
			base.Center = m_origin;
		}

		protected override void ReadEntitiesInRange()
		{
			m_entitiesInRange.Clear();
			m_hits.Clear();
			MyPhysics.CastRay(m_origin, base.FrontPoint, m_hits, 24);
			DetectionInfo value = default(DetectionInfo);
			bool flag = false;
			foreach (MyPhysics.HitInfo hit in m_hits)
			{
				HkHitInfo hkHitInfo = hit.HkHitInfo;
				if (!(hkHitInfo.Body == null))
				{
					IMyEntity hitEntity = hkHitInfo.GetHitEntity();
					if (hitEntity != null)
					{
						IMyEntity topMostParent = hitEntity.GetTopMostParent();
						if (!IgnoredEntities.Contains(topMostParent))
						{
							Vector3D position = hit.Position;
							MyCubeGrid myCubeGrid = topMostParent as MyCubeGrid;
							if (myCubeGrid != null)
							{
								if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
								{
									position += hit.HkHitInfo.Normal * -0.08f;
								}
								else
								{
									position += hit.HkHitInfo.Normal * -0.02f;
								}
							}
							if (m_entitiesInRange.TryGetValue(topMostParent.EntityId, out value))
							{
								float num = Vector3.DistanceSquared(value.DetectionPoint, m_origin);
								float num2 = Vector3.DistanceSquared(position, m_origin);
								if (num > num2)
								{
									m_entitiesInRange[topMostParent.EntityId] = new DetectionInfo(topMostParent as MyEntity, position);
								}
							}
							else
							{
								m_entitiesInRange[topMostParent.EntityId] = new DetectionInfo(topMostParent as MyEntity, position);
							}
							if (hitEntity is MyEnvironmentSector && !flag)
							{
								MyEnvironmentSector myEnvironmentSector = hitEntity as MyEnvironmentSector;
								uint shapeKey = hkHitInfo.GetShapeKey(0);
								int itemFromShapeKey = myEnvironmentSector.GetItemFromShapeKey(shapeKey);
								if (myEnvironmentSector.DataView.Items[itemFromShapeKey].ModelIndex >= 0)
								{
									flag = true;
									m_entitiesInRange[hitEntity.EntityId] = new DetectionInfo(myEnvironmentSector, position, itemFromShapeKey);
								}
							}
						}
					}
				}
			}
			LineD ray = new LineD(m_origin, base.FrontPoint);
			using (m_raycastResults.GetClearToken())
			{
				MyGamePruningStructure.GetAllEntitiesInRay(ref ray, m_raycastResults);
				foreach (MyLineSegmentOverlapResult<MyEntity> raycastResult in m_raycastResults)
				{
					if (raycastResult.Element != null)
					{
						MyEntity topMostParent2 = raycastResult.Element.GetTopMostParent();
						if (!IgnoredEntities.Contains(topMostParent2))
						{
							MyCubeBlock myCubeBlock = raycastResult.Element as MyCubeBlock;
							if (myCubeBlock != null)
							{
								Vector3D vector3D = default(Vector3D);
								if (!myCubeBlock.SlimBlock.BlockDefinition.HasPhysics)
								{
									MatrixD matrix = myCubeBlock.PositionComp.WorldMatrixNormalizedInv;
									Vector3D vector3D2 = Vector3D.Transform(m_origin, ref matrix);
									Vector3D value2 = Vector3D.Transform(base.FrontPoint, ref matrix);
									float? num3 = new Ray(vector3D2, Vector3.Normalize(value2 - vector3D2)).Intersects(myCubeBlock.PositionComp.LocalAABB) + 0.01f;
									if (num3.HasValue && num3 <= m_rayLength)
									{
										vector3D = m_origin + Vector3D.Normalize(base.FrontPoint - m_origin) * num3.Value;
										if (m_entitiesInRange.TryGetValue(topMostParent2.EntityId, out value))
										{
											if (Vector3.DistanceSquared(value.DetectionPoint, m_origin) > Vector3.DistanceSquared(vector3D, m_origin))
											{
												m_entitiesInRange[topMostParent2.EntityId] = new DetectionInfo(topMostParent2, vector3D);
											}
										}
										else
										{
											m_entitiesInRange[topMostParent2.EntityId] = new DetectionInfo(topMostParent2, vector3D);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public override void DebugDraw()
		{
			MyRenderProxy.DebugDrawLine3D(m_origin, base.FrontPoint, Color.Red, Color.Blue, depthRead: false);
		}
	}
}
