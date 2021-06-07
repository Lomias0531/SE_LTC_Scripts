using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal interface IMyGridOverlapTest
	{
		void GetBlocks(Vector3I minI, Vector3I maxI, Dictionary<Vector3I, OverlapResult> outOverlappedBlocks);
	}
}
