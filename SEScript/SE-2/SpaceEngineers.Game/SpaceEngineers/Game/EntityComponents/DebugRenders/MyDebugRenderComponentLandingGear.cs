using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
	public class MyDebugRenderComponentLandingGear : MyDebugRenderComponent
	{
		private MyLandingGear m_langingGear;

		public MyDebugRenderComponentLandingGear(MyLandingGear landingGear)
			: base(landingGear)
		{
			m_langingGear = landingGear;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES)
			{
				Matrix[] lockPositions = m_langingGear.LockPositions;
				foreach (Matrix m in lockPositions)
				{
					m_langingGear.GetBoxFromMatrix(m, out Vector3 halfExtents, out Vector3D position, out Quaternion orientation);
					Matrix matrix = Matrix.CreateFromQuaternion(orientation);
					matrix.Translation = position;
					matrix = Matrix.CreateScale(halfExtents * 2f * new Vector3(2f, 1f, 2f)) * matrix;
					MyRenderProxy.DebugDrawOBB(matrix, Color.Yellow.ToVector3(), 1f, depthRead: false, smooth: false);
				}
			}
		}
	}
}
