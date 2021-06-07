using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.RenderDirect.ActorComponents;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentThrust : MyDebugRenderComponent
	{
		private MyThrust m_thrust;

		public MyDebugRenderComponentThrust(MyThrust thrust)
			: base(thrust)
		{
			m_thrust = thrust;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_THRUSTER_DAMAGE)
			{
				DebugDrawDamageArea();
			}
		}

		private void DebugDrawDamageArea()
		{
			if (m_thrust.CurrentStrength != 0f || MyFakes.INACTIVE_THRUSTER_DMG)
			{
				foreach (MyThrustFlameAnimator.FlameInfo flame in m_thrust.Flames)
				{
					MatrixD matrixWorld = m_thrust.WorldMatrix;
					LineD damageCapsuleLine = m_thrust.GetDamageCapsuleLine(flame, ref matrixWorld);
					MyRenderProxy.DebugDrawCapsule(damageCapsuleLine.From, damageCapsuleLine.To, flame.Radius * m_thrust.FlameDamageLengthScale, Color.Red, depthRead: false);
				}
			}
		}
	}
}
