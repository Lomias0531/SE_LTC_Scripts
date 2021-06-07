using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
	public class MyDebugRenderComponentGravityGenerator : MyDebugRenderComponent
	{
		private MyGravityGenerator m_gravityGenerator;

		public MyDebugRenderComponentGravityGenerator(MyGravityGenerator gravityGenerator)
			: base(gravityGenerator)
		{
			m_gravityGenerator = gravityGenerator;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS && m_gravityGenerator.IsWorking)
			{
				MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(m_gravityGenerator.FieldSize) * m_gravityGenerator.PositionComp.WorldMatrix, Color.CadetBlue, 1f, depthRead: true, smooth: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS)
			{
				MyRenderProxy.DebugDrawAxis(m_gravityGenerator.PositionComp.WorldMatrix, 2f, depthRead: false);
			}
		}
	}
}
