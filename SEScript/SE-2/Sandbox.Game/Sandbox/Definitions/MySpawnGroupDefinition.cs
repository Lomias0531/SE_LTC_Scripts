using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SpawnGroupDefinition), null)]
	public class MySpawnGroupDefinition : MyDefinitionBase
	{
		public struct SpawnGroupPrefab
		{
			public Vector3 Position;

			public string SubtypeId;

			public string BeaconText;

			public float Speed;

			public bool ResetOwnership;

			public bool PlaceToGridOrigin;

			public string Behaviour;

			public float BehaviourActivationDistance;
		}

		public struct SpawnGroupVoxel
		{
			public Vector3 Offset;

			public bool CenterOffset;

			public string StorageName;
		}

		private class Sandbox_Definitions_MySpawnGroupDefinition_003C_003EActor : IActivator, IActivator<MySpawnGroupDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySpawnGroupDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySpawnGroupDefinition CreateInstance()
			{
				return new MySpawnGroupDefinition();
			}

			MySpawnGroupDefinition IActivator<MySpawnGroupDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Frequency;

		private float m_spawnRadius;

		private bool m_initialized;

		public bool IsPirate;

		public bool IsEncounter;

		public bool IsCargoShip;

		public bool ReactorsOn;

		public List<SpawnGroupPrefab> Prefabs = new List<SpawnGroupPrefab>();

		public List<SpawnGroupVoxel> Voxels = new List<SpawnGroupVoxel>();

		public float SpawnRadius
		{
			get
			{
				if (!m_initialized)
				{
					ReloadPrefabs();
				}
				return m_spawnRadius;
			}
			private set
			{
				m_spawnRadius = value;
			}
		}

		public bool IsValid
		{
			get
			{
				if (Frequency != 0f && m_spawnRadius != 0f)
				{
					return Prefabs.Count != 0;
				}
				return false;
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase baseBuilder)
		{
			base.Init(baseBuilder);
			MyObjectBuilder_SpawnGroupDefinition myObjectBuilder_SpawnGroupDefinition = baseBuilder as MyObjectBuilder_SpawnGroupDefinition;
			Frequency = myObjectBuilder_SpawnGroupDefinition.Frequency;
			if (Frequency == 0f)
			{
				MySandboxGame.Log.WriteLine("Spawn group initialization: spawn group has zero frequency");
				return;
			}
			SpawnRadius = 0f;
			BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, float.MinValue);
			Prefabs.Clear();
			MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab[] prefabs = myObjectBuilder_SpawnGroupDefinition.Prefabs;
			foreach (MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab spawnGroupPrefab in prefabs)
			{
				SpawnGroupPrefab item = default(SpawnGroupPrefab);
				item.Position = spawnGroupPrefab.Position;
				item.SubtypeId = spawnGroupPrefab.SubtypeId;
				item.BeaconText = spawnGroupPrefab.BeaconText;
				item.Speed = spawnGroupPrefab.Speed;
				item.ResetOwnership = spawnGroupPrefab.ResetOwnership;
				item.PlaceToGridOrigin = spawnGroupPrefab.PlaceToGridOrigin;
				item.Behaviour = spawnGroupPrefab.Behaviour;
				item.BehaviourActivationDistance = spawnGroupPrefab.BehaviourActivationDistance;
				if (MyDefinitionManager.Static.GetPrefabDefinition(item.SubtypeId) == null)
				{
					MySandboxGame.Log.WriteLine("Spawn group initialization: Could not get prefab " + item.SubtypeId);
					return;
				}
				Prefabs.Add(item);
			}
			Voxels.Clear();
			if (myObjectBuilder_SpawnGroupDefinition.Voxels != null)
			{
				MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel[] voxels = myObjectBuilder_SpawnGroupDefinition.Voxels;
				foreach (MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel spawnGroupVoxel in voxels)
				{
					SpawnGroupVoxel item2 = default(SpawnGroupVoxel);
					item2.Offset = spawnGroupVoxel.Offset;
					item2.StorageName = spawnGroupVoxel.StorageName;
					item2.CenterOffset = spawnGroupVoxel.CenterOffset;
					Voxels.Add(item2);
				}
			}
			SpawnRadius = boundingSphere.Radius + 5f;
			IsEncounter = myObjectBuilder_SpawnGroupDefinition.IsEncounter;
			IsCargoShip = myObjectBuilder_SpawnGroupDefinition.IsCargoShip;
			IsPirate = myObjectBuilder_SpawnGroupDefinition.IsPirate;
			ReactorsOn = myObjectBuilder_SpawnGroupDefinition.ReactorsOn;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_SpawnGroupDefinition myObjectBuilder_SpawnGroupDefinition = base.GetObjectBuilder() as MyObjectBuilder_SpawnGroupDefinition;
			myObjectBuilder_SpawnGroupDefinition.Frequency = Frequency;
			myObjectBuilder_SpawnGroupDefinition.Prefabs = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab[Prefabs.Count];
			int num = 0;
			foreach (SpawnGroupPrefab prefab in Prefabs)
			{
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num] = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab();
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num].BeaconText = prefab.BeaconText;
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num].SubtypeId = prefab.SubtypeId;
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num].Position = prefab.Position;
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num].Speed = prefab.Speed;
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num].ResetOwnership = prefab.ResetOwnership;
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num].PlaceToGridOrigin = prefab.PlaceToGridOrigin;
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num].Behaviour = prefab.Behaviour;
				myObjectBuilder_SpawnGroupDefinition.Prefabs[num].BehaviourActivationDistance = prefab.BehaviourActivationDistance;
				num++;
			}
			myObjectBuilder_SpawnGroupDefinition.Voxels = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel[Voxels.Count];
			num = 0;
			foreach (SpawnGroupVoxel voxel in Voxels)
			{
				myObjectBuilder_SpawnGroupDefinition.Voxels[num] = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel();
				myObjectBuilder_SpawnGroupDefinition.Voxels[num].Offset = voxel.Offset;
				myObjectBuilder_SpawnGroupDefinition.Voxels[num].CenterOffset = voxel.CenterOffset;
				myObjectBuilder_SpawnGroupDefinition.Voxels[num].StorageName = voxel.StorageName;
				num++;
			}
			myObjectBuilder_SpawnGroupDefinition.IsCargoShip = IsCargoShip;
			myObjectBuilder_SpawnGroupDefinition.IsEncounter = IsEncounter;
			myObjectBuilder_SpawnGroupDefinition.IsPirate = IsPirate;
			myObjectBuilder_SpawnGroupDefinition.ReactorsOn = ReactorsOn;
			return myObjectBuilder_SpawnGroupDefinition;
		}

		public void ReloadPrefabs()
		{
			BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, float.MinValue);
			float num = 0f;
			foreach (SpawnGroupPrefab prefab in Prefabs)
			{
				MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefab.SubtypeId);
				if (prefabDefinition == null)
				{
					MySandboxGame.Log.WriteLine("Spawn group initialization: Could not get prefab " + prefab.SubtypeId);
					return;
				}
				BoundingSphere boundingSphere2 = prefabDefinition.BoundingSphere;
				boundingSphere2.Center += prefab.Position;
				boundingSphere.Include(boundingSphere2);
				if (prefabDefinition.CubeGrids != null)
				{
					MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
					foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in cubeGrids)
					{
						float cubeSize = MyDefinitionManager.Static.GetCubeSize(myObjectBuilder_CubeGrid.GridSizeEnum);
						num = Math.Max(num, 2f * cubeSize);
					}
				}
			}
			SpawnRadius = boundingSphere.Radius + num;
			m_initialized = true;
		}
	}
}
