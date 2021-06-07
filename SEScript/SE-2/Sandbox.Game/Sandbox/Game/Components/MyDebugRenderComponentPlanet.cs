using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.World;
using VRage;
using VRage.Game.Models;
using VRage.Game.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentPlanet : MyDebugRenderComponent
	{
		private MyPlanet m_planet;

		public MyDebugRenderComponentPlanet(MyPlanet voxelMap)
			: base(voxelMap)
		{
			m_planet = voxelMap;
		}

		public override void DebugDraw()
		{
			Vector3D positionLeftBottomCorner = m_planet.PositionLeftBottomCorner;
			if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MAP_AABB)
			{
				m_planet.Components.Get<MyPlanetEnvironmentComponent>().DebugDraw();
				m_planet.DebugDrawPhysics();
				MyRenderProxy.DebugDrawAABB(m_planet.PositionComp.WorldAABB, Color.White);
				MyRenderProxy.DebugDrawLine3D(positionLeftBottomCorner, positionLeftBottomCorner + new Vector3(1f, 0f, 0f), Color.Red, Color.Red, depthRead: true);
				MyRenderProxy.DebugDrawLine3D(positionLeftBottomCorner, positionLeftBottomCorner + new Vector3(0f, 1f, 0f), Color.Green, Color.Green, depthRead: true);
				MyRenderProxy.DebugDrawLine3D(positionLeftBottomCorner, positionLeftBottomCorner + new Vector3(0f, 0f, 1f), Color.Blue, Color.Blue, depthRead: true);
				MyRenderProxy.DebugDrawAxis(m_planet.PositionComp.WorldMatrix, 2f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(m_planet.PositionComp.GetPosition(), 1f, Color.OrangeRed, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_GEOMETRY_CELL)
			{
				MyCamera mainCamera = MySector.MainCamera;
				LineD line = new LineD(mainCamera.Position, mainCamera.Position + 25f * mainCamera.ForwardVector);
				if (m_planet.GetIntersectionWithLine(ref line, out MyIntersectionResultLineTriangleEx? t))
				{
					MyTriangle_Vertices inputTriangle = t.Value.Triangle.InputTriangle;
					MyRenderProxy.DebugDrawTriangle(inputTriangle.Vertex0 + positionLeftBottomCorner, inputTriangle.Vertex1 + positionLeftBottomCorner, inputTriangle.Vertex2 + positionLeftBottomCorner, Color.Red, smooth: true, depthRead: false);
					Vector3D worldPosition = t.Value.IntersectionPointInWorldSpace;
					MyVoxelCoordSystems.WorldPositionToVoxelCoord(positionLeftBottomCorner, ref worldPosition, out Vector3I voxelCoord);
					MyVoxelCoordSystems.VoxelCoordToWorldAABB(positionLeftBottomCorner, ref voxelCoord, out BoundingBoxD worldAABB);
					MyRenderProxy.DebugDrawAABB(worldAABB, Vector3.UnitY);
					MyVoxelCoordSystems.WorldPositionToGeometryCellCoord(positionLeftBottomCorner, ref worldPosition, out Vector3I geometryCellCoord);
					MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(positionLeftBottomCorner, ref geometryCellCoord, out worldAABB);
					MyRenderProxy.DebugDrawAABB(worldAABB, Vector3.UnitZ);
				}
			}
		}
	}
}
