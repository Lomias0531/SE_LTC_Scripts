using Sandbox.Game.GameSystems.Conveyors;
using System.Collections.Generic;

namespace Sandbox.Game.Entities
{
	public class MyUpgradableBlockComponent
	{
		public HashSet<ConveyorLinePosition> ConnectionPositions
		{
			get;
			private set;
		}

		public MyUpgradableBlockComponent(MyCubeBlock parent)
		{
			ConnectionPositions = new HashSet<ConveyorLinePosition>();
			Refresh(parent);
		}

		public void Refresh(MyCubeBlock parent)
		{
			if (parent.BlockDefinition.Model != null)
			{
				ConnectionPositions.Clear();
				ConveyorLinePosition[] linePositions = MyMultilineConveyorEndpoint.GetLinePositions(parent, "detector_upgrade");
				foreach (ConveyorLinePosition position in linePositions)
				{
					ConnectionPositions.Add(MyMultilineConveyorEndpoint.PositionToGridCoords(position, parent));
				}
			}
		}
	}
}
