using Sandbox.Engine.Utils;
using Sandbox.Game.Weapons;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderCompomentDrawDrillBase : MyDebugRenderComponent
	{
		private MyDrillBase m_drillBase;

		public MyDebugRenderCompomentDrawDrillBase(MyDrillBase drillBase)
			: base(null)
		{
			m_drillBase = drillBase;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_DRILLS)
			{
				m_drillBase.DebugDraw();
			}
		}
	}
}
