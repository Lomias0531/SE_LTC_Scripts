using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
	public class MyDebugRenderComponentGravityGeneratorSphere : MyDebugRenderComponent
	{
		private MyGravityGeneratorSphere m_gravityGenerator;

		public MyDebugRenderComponentGravityGeneratorSphere(MyGravityGeneratorSphere gravityGenerator)
			: base(gravityGenerator)
		{
			m_gravityGenerator = gravityGenerator;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS && m_gravityGenerator.IsWorking)
			{
				MyRenderProxy.DebugDrawSphere(m_gravityGenerator.PositionComp.WorldMatrix.Translation, m_gravityGenerator.Radius, Color.CadetBlue, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS)
			{
				MyRenderProxy.DebugDrawAxis(m_gravityGenerator.PositionComp.WorldMatrix, 2f, depthRead: false);
			}
		}
	}
}
