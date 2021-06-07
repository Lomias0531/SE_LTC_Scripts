using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderCompoonentShipConnector : MyDebugRenderComponent
	{
		private MyShipConnector m_shipConnector;

		public MyDebugRenderCompoonentShipConnector(MyShipConnector shipConnector)
			: base(shipConnector)
		{
			m_shipConnector = shipConnector;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS)
			{
				MyRenderProxy.DebugDrawSphere(m_shipConnector.ConstraintPositionWorld(), 0.05f, Color.Red, 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(m_shipConnector.PositionComp.WorldMatrix.Translation, m_shipConnector.DetectedGridCount.ToString(), Color.Red, 1f, depthRead: false);
			}
		}
	}
}
