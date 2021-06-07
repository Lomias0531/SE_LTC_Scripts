using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public interface IMyGridConnectivityTest
	{
		void GetConnectedBlocks(Vector3I minI, Vector3I maxI, Dictionary<Vector3I, ConnectivityResult> outConnectedCubeBlocks);
	}
}
