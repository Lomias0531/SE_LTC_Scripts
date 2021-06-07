using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentMotorStator : MyDebugRenderComponent
	{
		private MyMotorStator m_motor;

		public MyDebugRenderComponentMotorStator(MyMotorStator motor)
			: base(motor)
		{
			m_motor = motor;
		}

		public override void DebugDraw()
		{
			if (m_motor.CanDebugDraw() && MyDebugDrawSettings.DEBUG_DRAW_ROTORS)
			{
				MatrixD worldMatrix = m_motor.PositionComp.WorldMatrix;
				MatrixD worldMatrix2 = m_motor.Rotor.WorldMatrix;
				Vector3 vector = Vector3.Lerp(worldMatrix.Translation, worldMatrix2.Translation, 0.5f);
				Vector3 value = Vector3.Normalize(worldMatrix.Up);
				MyRenderProxy.DebugDrawLine3D(vector, vector + value, Color.Yellow, Color.Yellow, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, worldMatrix2.Translation, Color.Red, Color.Green, depthRead: false);
			}
		}
	}
}
