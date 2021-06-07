using Sandbox.Engine.Utils;
using Sandbox.Game.Weapons;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentEngineerTool : MyDebugRenderComponent
	{
		private MyEngineerToolBase m_tool;

		public MyDebugRenderComponentEngineerTool(MyEngineerToolBase tool)
			: base(tool)
		{
			m_tool = tool;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC && m_tool.GetTargetGrid() != null)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), m_tool.TargetCube.ToString(), Color.White, 1f);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)
			{
				MyRenderProxy.DebugDrawSphere(m_tool.GunBase.GetMuzzleWorldPosition(), 0.01f, Color.Green, 1f, depthRead: false);
			}
		}
	}
}
