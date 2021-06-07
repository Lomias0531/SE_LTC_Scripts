using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderComponentDrawPowerReciever : MyDebugRenderComponent
	{
		private readonly MyResourceSinkComponent m_sink;

		private IMyEntity m_entity;

		public MyDebugRenderComponentDrawPowerReciever(MyResourceSinkComponent sink, IMyEntity entity)
			: base(null)
		{
			m_sink = sink;
			m_entity = entity;
			m_sink.IsPoweredChanged += IsPoweredChanged;
		}

		private void IsPoweredChanged()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				MyHud.Notifications.Add(new MyHudNotification(MyStringId.GetOrCompute($"{m_entity} PowerChanged:{m_sink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)}"), 4000));
			}
		}

		public override void DebugDraw()
		{
			m_sink.DebugDraw(m_entity.PositionComp.WorldMatrix);
		}
	}
}
