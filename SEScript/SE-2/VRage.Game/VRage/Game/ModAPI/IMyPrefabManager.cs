using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyPrefabManager
	{
		void SpawnPrefab(List<IMyCubeGrid> resultList, string prefabName, Vector3D position, Vector3 forward, Vector3 up, Vector3 initialLinearVelocity = default(Vector3), Vector3 initialAngularVelocity = default(Vector3), string beaconName = null, SpawningOptions spawningOptions = SpawningOptions.None, bool updateSync = false, Action callback = null);

		void SpawnPrefab(List<IMyCubeGrid> resultList, string prefabName, Vector3D position, Vector3 forward, Vector3 up, Vector3 initialLinearVelocity = default(Vector3), Vector3 initialAngularVelocity = default(Vector3), string beaconName = null, SpawningOptions spawningOptions = SpawningOptions.None, long ownerId = 0L, bool updateSync = false, Action callback = null);

		bool IsPathClear(Vector3D from, Vector3D to);

		bool IsPathClear(Vector3D from, Vector3D to, double halfSize);
	}
}
