namespace Sandbox.Game.GameSystems.Conveyors
{
	public interface IMyConveyorSegmentBlock
	{
		MyConveyorSegment ConveyorSegment
		{
			get;
		}

		void InitializeConveyorSegment();
	}
}
