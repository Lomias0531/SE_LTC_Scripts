using Sandbox.Game.Entities;
using System.Collections;
using System.Collections.Generic;
using VRage.Algorithms;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public interface IMyConveyorEndpoint : IMyPathVertex<IMyConveyorEndpoint>, IEnumerable<IMyPathEdge<IMyConveyorEndpoint>>, IEnumerable
	{
		MyCubeBlock CubeBlock
		{
			get;
		}

		MyConveyorLine GetConveyorLine(ConveyorLinePosition position);

		MyConveyorLine GetConveyorLine(int index);

		ConveyorLinePosition GetPosition(int index);

		void DebugDraw();

		void SetConveyorLine(ConveyorLinePosition position, MyConveyorLine newLine);

		int GetLineCount();
	}
}
