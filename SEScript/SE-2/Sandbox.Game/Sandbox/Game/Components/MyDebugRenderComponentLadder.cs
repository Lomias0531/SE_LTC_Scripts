using VRage.ModAPI;
using VRageRender;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderComponentLadder : MyDebugRenderComponent
	{
		private IMyEntity m_ladder;

		public MyDebugRenderComponentLadder(IMyEntity ladder)
			: base(ladder)
		{
			m_ladder = ladder;
		}

		public override void DebugDraw()
		{
			MyRenderProxy.DebugDrawAxis(m_ladder.PositionComp.WorldMatrix, 1f, depthRead: false);
			base.DebugDraw();
		}
	}
}
