using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentMotorSuspension : MyDebugRenderComponent
	{
		private MyMotorSuspension m_motor;

		public MyDebugRenderComponentMotorSuspension(MyMotorSuspension motor)
			: base(motor)
		{
			m_motor = motor;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS && (MySector.MainCamera.Position - m_motor.PositionComp.GetPosition()).LengthSquared() < 10000.0)
			{
				m_motor.DebugDrawConstraint();
			}
		}
	}
}
