using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentMotorBase : MyDebugRenderComponent
	{
		private MyMotorBase m_motor;

		public MyDebugRenderComponentMotorBase(MyMotorBase motor)
			: base(motor)
		{
			m_motor = motor;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_ROTORS)
			{
				m_motor.ComputeTopQueryBox(out Vector3D pos, out Vector3 halfExtents, out Quaternion orientation);
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(pos, halfExtents, orientation), Color.Green.ToVector3(), 1f, depthRead: false, smooth: false);
				if (m_motor.Rotor != null)
				{
					MyRenderProxy.DebugDrawSphere(Vector3D.Transform(m_motor.DummyPosition, m_motor.CubeGrid.WorldMatrix) + (Vector3D.Transform((m_motor.Rotor as MyMotorRotor).WheelDummy, m_motor.RotorGrid.WorldMatrix) - m_motor.RotorGrid.WorldMatrix.Translation), 0.1f, Color.Green, 1f, depthRead: false);
					BoundingSphere boundingSphere = m_motor.Rotor.Model.BoundingSphere;
					boundingSphere.Center = Vector3D.Transform(boundingSphere.Center, m_motor.Rotor.WorldMatrix);
					MyRenderProxy.DebugDrawSphere(boundingSphere.Center, boundingSphere.Radius, Color.Red, 1f, depthRead: false);
				}
			}
		}
	}
}
