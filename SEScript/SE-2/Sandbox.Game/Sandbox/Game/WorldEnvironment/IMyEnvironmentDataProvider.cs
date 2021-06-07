using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public interface IMyEnvironmentDataProvider
	{
		IEnumerable<MyLogicalEnvironmentSectorBase> LogicalSectors
		{
			get;
		}

		MyEnvironmentDataView GetItemView(int lod, ref Vector2I start, ref Vector2I end, ref Vector3D localOrigin);

		MyObjectBuilder_EnvironmentDataProvider GetObjectBuilder();

		void DebugDraw();

		MyLogicalEnvironmentSectorBase GetLogicalSector(long sectorId);
	}
}
