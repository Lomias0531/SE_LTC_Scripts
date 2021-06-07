using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentCockpit : MyDebugRenderComponent
	{
		private MyCockpit m_cockpit;

		public MyDebugRenderComponentCockpit(MyCockpit cockpit)
			: base(cockpit)
		{
			m_cockpit = cockpit;
		}

		public override void DebugDraw()
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_COCKPIT)
			{
				return;
			}
			if (m_cockpit.AiPilot != null)
			{
				m_cockpit.AiPilot.DebugDraw();
			}
			MyRenderProxy.DebugDrawText3D(m_cockpit.PositionComp.WorldMatrix.Translation, m_cockpit.IsShooting() ? "PEW!" : "", Color.Red, 2f, depthRead: false);
			if (m_cockpit.Pilot == null)
			{
				return;
			}
			Vector3I[] neighbourPositions = m_cockpit.NeighbourPositions;
			foreach (Vector3I neighbourOffsetI in neighbourPositions)
			{
				if (m_cockpit.IsNeighbourPositionFree(neighbourOffsetI, out Vector3D translation))
				{
					MyRenderProxy.DebugDrawSphere(translation, 0.3f, Color.Green, 1f, depthRead: false);
				}
				else
				{
					MyRenderProxy.DebugDrawSphere(translation, 0.3f, Color.Red, 1f, depthRead: false);
				}
			}
		}
	}
}
