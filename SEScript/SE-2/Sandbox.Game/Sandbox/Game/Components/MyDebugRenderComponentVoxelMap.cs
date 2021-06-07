using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentVoxelMap : MyDebugRenderComponent
	{
		private MyVoxelBase m_voxelMap;

		public MyDebugRenderComponentVoxelMap(MyVoxelBase voxelMap)
			: base(voxelMap)
		{
			m_voxelMap = voxelMap;
		}

		public override void DebugDraw()
		{
			Vector3D positionLeftBottomCorner = m_voxelMap.PositionLeftBottomCorner;
			if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MAP_AABB)
			{
				MyRenderProxy.DebugDrawAABB(m_voxelMap.PositionComp.WorldAABB, Color.White, 0.2f);
				MyRenderProxy.DebugDrawLine3D(positionLeftBottomCorner, positionLeftBottomCorner + new Vector3(1f, 0f, 0f), Color.Red, Color.Red, depthRead: true);
				MyRenderProxy.DebugDrawLine3D(positionLeftBottomCorner, positionLeftBottomCorner + new Vector3(0f, 1f, 0f), Color.Green, Color.Green, depthRead: true);
				MyRenderProxy.DebugDrawLine3D(positionLeftBottomCorner, positionLeftBottomCorner + new Vector3(0f, 0f, 1f), Color.Blue, Color.Blue, depthRead: true);
				MyRenderProxy.DebugDrawAxis(m_voxelMap.PositionComp.WorldMatrix, 2f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(m_voxelMap.PositionComp.GetPosition(), 1f, Color.OrangeRed, 1f, depthRead: false);
			}
		}
	}
}
