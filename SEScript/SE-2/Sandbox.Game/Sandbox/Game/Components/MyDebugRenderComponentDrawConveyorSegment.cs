using Sandbox.Game.GameSystems.Conveyors;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentDrawConveyorSegment : MyDebugRenderComponent
	{
		private MyConveyorSegment m_conveyorSegment;

		public MyDebugRenderComponentDrawConveyorSegment(MyConveyorSegment conveyorSegment)
			: base(null)
		{
			m_conveyorSegment = conveyorSegment;
		}

		public override void DebugDraw()
		{
			m_conveyorSegment.DebugDraw();
		}
	}
}
