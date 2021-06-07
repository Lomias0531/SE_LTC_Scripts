using Sandbox.Definitions;
using Sandbox.Game.WorldEnvironment.Definitions;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public interface IMyEnvironmentOwner
	{
		MyWorldEnvironmentDefinition EnvironmentDefinition
		{
			get;
		}

		MyEntity Entity
		{
			get;
		}

		void QuerySurfaceParameters(Vector3D localOrigin, ref BoundingBoxD queryBounds, List<Vector3> queries, MyList<MySurfaceParams> results);

		MyEnvironmentSector GetSectorForPosition(Vector3D positionWorld);

		MyEnvironmentSector GetSectorById(long packedSectorId);

		void SetSectorPinned(MyEnvironmentSector sector, bool pinned);

		int GetSeed();

		short GetModelId(MyPhysicalModelDefinition def);

		MyPhysicalModelDefinition GetModelForId(short id);

		void GetDefinition(ushort index, out MyRuntimeEnvironmentItemInfo def);

		void ProjectPointToSurface(ref Vector3D center);

		void GetSurfaceNormalForPoint(ref Vector3D point, out Vector3D normal);

		Vector3D[] GetBoundingShape(ref Vector3D worldPos, ref Vector3 basisX, ref Vector3 basisY);

		void ScheduleWork(MyEnvironmentSector sector, bool parallel);
	}
}
