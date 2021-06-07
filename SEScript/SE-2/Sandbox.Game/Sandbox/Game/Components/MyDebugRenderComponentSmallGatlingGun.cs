using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentSmallGatlingGun : MyDebugRenderComponent
	{
		private MySmallGatlingGun m_gatlingGun;

		public MyDebugRenderComponentSmallGatlingGun(MySmallGatlingGun gatlingGun)
			: base(gatlingGun)
		{
			m_gatlingGun = gatlingGun;
		}

		public override void DebugDraw()
		{
			m_gatlingGun.ConveyorEndpoint.DebugDraw();
			m_gatlingGun.Components.Get<MyResourceSinkComponent>()?.DebugDraw(m_gatlingGun.PositionComp.WorldMatrix);
		}
	}
}
