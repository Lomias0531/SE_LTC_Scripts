using Sandbox.Game.GameSystems.Conveyors;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderComponentDrawConveyorEndpoint : MyDebugRenderComponent
	{
		private IMyConveyorEndpoint ConveyorEndpoint
		{
			get;
			set;
		}

		public MyDebugRenderComponentDrawConveyorEndpoint(IMyConveyorEndpoint endpoint)
			: base(null)
		{
			ConveyorEndpoint = endpoint;
		}

		public override void DebugDraw()
		{
			ConveyorEndpoint.DebugDraw();
		}
	}
}
