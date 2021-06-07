using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using VRage.Game.Components;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
	public class MyDebugRenderComponentSolarPanel : MyDebugRenderComponent
	{
		private MyTerminalBlock m_solarBlock;

		private MySolarGameLogicComponent m_solarComponent;

		public MyDebugRenderComponentSolarPanel(MyTerminalBlock solarBlock)
			: base(solarBlock)
		{
			m_solarBlock = solarBlock;
			if (m_solarBlock.Components.TryGet(out MyGameLogicComponent component))
			{
				m_solarComponent = (component as MySolarGameLogicComponent);
			}
			_ = m_solarComponent;
		}

		public override void DebugDraw()
		{
		}
	}
}
