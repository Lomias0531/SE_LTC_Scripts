using Sandbox.Game.EntityComponents;
using VRage.ModAPI;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderComponentDrawPowerSource : MyDebugRenderComponent
	{
		private readonly MyResourceSourceComponent m_source;

		private IMyEntity m_entity;

		public MyDebugRenderComponentDrawPowerSource(MyResourceSourceComponent source, IMyEntity entity)
			: base(null)
		{
			m_source = source;
			m_entity = entity;
		}

		public override void DebugDraw()
		{
			m_source.DebugDraw(m_entity.PositionComp.WorldMatrix);
		}
	}
}
