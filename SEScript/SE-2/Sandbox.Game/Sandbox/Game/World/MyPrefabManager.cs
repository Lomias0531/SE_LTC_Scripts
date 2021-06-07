using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MyPrefabManager : IMyPrefabManager
	{
		public class CreateGridsData : WorkData
		{
			private List<MyCubeGrid> m_results;

			private string m_prefabName;

			private MatrixD m_worldMatrix;

			private SpawningOptions m_spawnOptions;

			private bool m_ignoreMemoryLimits;

			private long m_ownerId;

			private Stack<Action> m_callbacks;

			private List<IMyEntity> m_resultIDs;

			private Action m_spawnFailedCallback;

			public CreateGridsData(List<MyCubeGrid> results, string prefabName, MatrixD worldMatrix, SpawningOptions spawnOptions, bool ignoreMemoryLimits = true, long ownerId = 0L, Stack<Action> callbacks = null, Action spawnFailedCallback = null)
			{
				m_results = results;
				m_ownerId = ownerId;
				m_prefabName = prefabName;
				m_worldMatrix = worldMatrix;
				m_spawnOptions = spawnOptions;
				m_ignoreMemoryLimits = ignoreMemoryLimits;
				m_callbacks = (callbacks ?? new Stack<Action>());
				m_spawnFailedCallback = spawnFailedCallback;
				if (spawnOptions.HasFlag(SpawningOptions.SetNeutralOwner))
				{
					List<MyFaction> npcFactions = MySession.Static.Factions.GetNpcFactions();
					int index = MyRandom.Instance.Next(0, npcFactions.Count - 1);
					m_ownerId = npcFactions[index].FounderId;
				}
			}

			public void CallCreateGridsFromPrefab(WorkData workData)
			{
				try
				{
					MyEntityIdentifier.InEntityCreationBlock = true;
					MyEntityIdentifier.LazyInitPerThreadStorage(2048);
					Static.CreateGridsFromPrefab(m_results, m_prefabName, m_worldMatrix, m_spawnOptions, m_ignoreMemoryLimits, m_ownerId, m_callbacks);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine("Could not spawn prefab " + m_prefabName + ".");
					MyLog.Default.WriteLine(ex);
					throw;
				}
				finally
				{
					m_resultIDs = new List<IMyEntity>();
					MyEntityIdentifier.GetPerThreadEntities(m_resultIDs);
					MyEntityIdentifier.ClearPerThreadEntities();
					MyEntityIdentifier.InEntityCreationBlock = false;
					Interlocked.Decrement(ref PendingGrids);
					if (PendingGrids <= 0)
					{
						FinishedProcessingGrids.Set();
					}
				}
			}

			public void OnGridsCreated(WorkData workData)
			{
				if (m_resultIDs == null || m_results == null)
				{
					m_spawnFailedCallback.InvokeIfNotNull();
					return;
				}
				foreach (IMyEntity resultID in m_resultIDs)
				{
					MyEntityIdentifier.TryGetEntity(resultID.EntityId, out IMyEntity entity);
					if (entity == null)
					{
						MyEntityIdentifier.AddEntityWithId(resultID);
					}
				}
				foreach (MyCubeGrid result in m_results)
				{
					MyEntities.Add(result);
				}
				List<MyCubeGrid> results = m_results;
				if (results != null && results.Count == 0)
				{
					m_spawnFailedCallback.InvokeIfNotNull();
				}
				else if (m_callbacks != null)
				{
					while (m_callbacks.Count > 0)
					{
						m_callbacks.Pop().InvokeIfNotNull();
					}
				}
			}
		}

		private static FastResourceLock m_builderLock;

		private readonly Vector3 MASK_COLOR = new Vector3(1f, 0.2f, 0.55f);

		public static EventWaitHandle FinishedProcessingGrids;

		public static int PendingGrids;

		public static readonly MyPrefabManager Static;

		private static List<MyPhysics.HitInfo> m_raycastHits;

		static MyPrefabManager()
		{
			m_builderLock = new FastResourceLock();
			FinishedProcessingGrids = new AutoResetEvent(initialState: false);
			m_raycastHits = new List<MyPhysics.HitInfo>();
			Static = new MyPrefabManager();
		}

		public static void SavePrefab(string prefabName, MyObjectBuilder_EntityBase entity)
		{
			string path = Path.Combine(MyFileSystem.ContentPath, Path.Combine("Data", "Prefabs", prefabName + ".sbc"));
			MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PrefabDefinition>();
			myObjectBuilder_PrefabDefinition.Id = new MyDefinitionId(new MyObjectBuilderType(typeof(MyObjectBuilder_PrefabDefinition)), prefabName);
			myObjectBuilder_PrefabDefinition.CubeGrid = (MyObjectBuilder_CubeGrid)entity;
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
			myObjectBuilder_Definitions.Prefabs = new MyObjectBuilder_PrefabDefinition[1];
			myObjectBuilder_Definitions.Prefabs[0] = myObjectBuilder_PrefabDefinition;
			MyObjectBuilderSerializer.SerializeXML(path, compress: false, myObjectBuilder_Definitions);
		}

		public static MyObjectBuilder_PrefabDefinition SavePrefab(string prefabName, List<MyObjectBuilder_CubeGrid> copiedPrefab)
		{
			string path = Path.Combine(MyFileSystem.ContentPath, Path.Combine("Data", "Prefabs", prefabName + ".sbc"));
			return SavePrefabToPath(prefabName, path, copiedPrefab);
		}

		public static MyObjectBuilder_PrefabDefinition SavePrefabToPath(string prefabName, string path, List<MyObjectBuilder_CubeGrid> copiedPrefab)
		{
			MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PrefabDefinition>();
			myObjectBuilder_PrefabDefinition.Id = new MyDefinitionId(new MyObjectBuilderType(typeof(MyObjectBuilder_PrefabDefinition)), prefabName);
			myObjectBuilder_PrefabDefinition.CubeGrids = copiedPrefab.Select((MyObjectBuilder_CubeGrid x) => (MyObjectBuilder_CubeGrid)x.Clone()).ToArray();
			MyObjectBuilder_CubeGrid[] cubeGrids = myObjectBuilder_PrefabDefinition.CubeGrids;
			for (int i = 0; i < cubeGrids.Length; i++)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrids[i].CubeBlocks)
				{
					cubeBlock.Owner = 0L;
					cubeBlock.BuiltBy = 0L;
				}
			}
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
			myObjectBuilder_Definitions.Prefabs = new MyObjectBuilder_PrefabDefinition[1];
			myObjectBuilder_Definitions.Prefabs[0] = myObjectBuilder_PrefabDefinition;
			MyObjectBuilderSerializer.SerializeXML(path, compress: false, myObjectBuilder_Definitions);
			return myObjectBuilder_PrefabDefinition;
		}

		public MyObjectBuilder_CubeGrid[] GetGridPrefab(string prefabName)
		{
			MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefabName);
			if (prefabDefinition == null)
			{
				return null;
			}
			MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
			MyEntities.RemapObjectBuilderCollection(cubeGrids);
			return cubeGrids;
		}

		public void AddShipPrefab(string prefabName, Matrix? worldMatrix = null, long ownerId = 0L, bool spawnAtOrigin = false)
		{
			CreateGridsData createGridsData = new CreateGridsData(new List<MyCubeGrid>(), prefabName, worldMatrix ?? Matrix.Identity, ownerId: ownerId, spawnOptions: spawnAtOrigin ? SpawningOptions.UseGridOrigin : SpawningOptions.None);
			Interlocked.Increment(ref PendingGrids);
			Parallel.Start(createGridsData.CallCreateGridsFromPrefab, createGridsData.OnGridsCreated, createGridsData);
		}

		public void AddShipPrefabRandomPosition(string prefabName, Vector3D position, float distance, long ownerId = 0L, bool spawnAtOrigin = false)
		{
			MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefabName);
			if (prefabDefinition == null)
			{
				return;
			}
			BoundingSphereD sphere = new BoundingSphereD(Vector3D.Zero, prefabDefinition.BoundingSphere.Radius);
			int num = 0;
			Vector3 vector;
			MyEntity intersectionWithSphere;
			do
			{
				vector = position + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(0.5f, 1f) * distance;
				sphere.Center = vector;
				intersectionWithSphere = MyEntities.GetIntersectionWithSphere(ref sphere);
				num++;
				if (num % 8 == 0)
				{
					distance += (float)sphere.Radius / 2f;
				}
			}
			while (intersectionWithSphere != null);
			CreateGridsData createGridsData = new CreateGridsData(new List<MyCubeGrid>(), prefabName, Matrix.CreateWorld(vector, Vector3.Forward, Vector3.Up), ownerId: ownerId, spawnOptions: spawnAtOrigin ? SpawningOptions.UseGridOrigin : SpawningOptions.None);
			Interlocked.Increment(ref PendingGrids);
			Parallel.Start(createGridsData.CallCreateGridsFromPrefab, createGridsData.OnGridsCreated, createGridsData);
		}

		private void CreateGridsFromPrefab(List<MyCubeGrid> results, string prefabName, MatrixD worldMatrix, SpawningOptions spawningOptions, bool ignoreMemoryLimits, long ownerId, Stack<Action> callbacks)
		{
			MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefabName);
			if (prefabDefinition == null)
			{
				return;
			}
			MyObjectBuilder_CubeGrid[] array = new MyObjectBuilder_CubeGrid[prefabDefinition.CubeGrids.Length];
			if (array.Length == 0)
			{
				return;
			}
			int num = 0;
			MatrixD matrix = MatrixD.Identity;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (MyObjectBuilder_CubeGrid)prefabDefinition.CubeGrids[i].Clone();
				if (array[i].CubeBlocks.Count > num)
				{
					num = array[i].CubeBlocks.Count;
					matrix = (array[i].PositionAndOrientation.HasValue ? array[i].PositionAndOrientation.Value.GetMatrix() : MatrixD.Identity);
				}
			}
			MyEntities.RemapObjectBuilderCollection(array);
			MatrixD matrix2;
			if (spawningOptions.HasFlag(SpawningOptions.UseGridOrigin))
			{
				Vector3D value = Vector3D.Zero;
				if (prefabDefinition.CubeGrids[0].PositionAndOrientation.HasValue)
				{
					value = prefabDefinition.CubeGrids[0].PositionAndOrientation.Value.Position;
				}
				matrix2 = MatrixD.CreateWorld(-value, Vector3D.Forward, Vector3D.Up);
			}
			else
			{
				matrix2 = MatrixD.CreateWorld(-prefabDefinition.BoundingSphere.Center, Vector3D.Forward, Vector3D.Up);
			}
			bool ignoreMemoryLimits2 = MyEntities.IgnoreMemoryLimits;
			MyEntities.IgnoreMemoryLimits = ignoreMemoryLimits;
			bool flag = spawningOptions.HasFlag(SpawningOptions.SetAuthorship);
			for (int j = 0; j < array.Length; j++)
			{
				if (ownerId != 0L)
				{
					foreach (MyObjectBuilder_CubeBlock cubeBlock in array[j].CubeBlocks)
					{
						cubeBlock.Owner = ownerId;
						cubeBlock.ShareMode = MyOwnershipShareModeEnum.Faction;
						if (flag)
						{
							cubeBlock.BuiltBy = ownerId;
						}
					}
				}
				MatrixD newWorldMatrix;
				if (spawningOptions.HasFlag(SpawningOptions.UseOnlyWorldMatrix))
				{
					if (array[j].PositionAndOrientation.HasValue)
					{
						MatrixD matrix3 = array[j].PositionAndOrientation.Value.GetMatrix() * MatrixD.Invert(matrix);
						newWorldMatrix = matrix3 * worldMatrix;
						array[j].PositionAndOrientation = new MyPositionAndOrientation(newWorldMatrix);
					}
					else
					{
						newWorldMatrix = worldMatrix;
						array[j].PositionAndOrientation = new MyPositionAndOrientation(worldMatrix);
					}
				}
				else
				{
					MatrixD matrix4 = array[j].PositionAndOrientation.HasValue ? array[j].PositionAndOrientation.Value.GetMatrix() : MatrixD.Identity;
					newWorldMatrix = MatrixD.Multiply(matrix4, MatrixD.Multiply(matrix2, worldMatrix));
					array[j].PositionAndOrientation = new MyPositionAndOrientation(newWorldMatrix);
				}
				MyEntity entity = MyEntities.CreateFromObjectBuilder(array[j], fadeIn: false);
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (myCubeGrid != null && myCubeGrid.CubeBlocks.Count > 0)
				{
					results.Add(myCubeGrid);
					callbacks.Push(delegate
					{
						SetPrefabPosition(entity, newWorldMatrix);
					});
				}
			}
			MyEntities.IgnoreMemoryLimits = ignoreMemoryLimits2;
		}

		private void SetPrefabPosition(MyEntity entity, MatrixD newWorldMatrix)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.PositionComp.SetWorldMatrix(newWorldMatrix, null, forceUpdate: true);
				if (MyPerGameSettings.Destruction && myCubeGrid.IsStatic && myCubeGrid.Physics != null && myCubeGrid.Physics.Shape != null)
				{
					myCubeGrid.Physics.Shape.RecalculateConnectionsToWorld(myCubeGrid.GetBlocks());
				}
			}
		}

		public void SpawnPrefab(string prefabName, Vector3D position, Vector3 forward, Vector3 up, Vector3 initialLinearVelocity = default(Vector3), Vector3 initialAngularVelocity = default(Vector3), string beaconName = null, string entityName = null, SpawningOptions spawningOptions = SpawningOptions.None, long ownerId = 0L, bool updateSync = false, Stack<Action> callbacks = null)
		{
			if (callbacks == null)
			{
				callbacks = new Stack<Action>();
			}
			SpawnPrefabInternal(new List<MyCubeGrid>(), prefabName, position, forward, up, initialLinearVelocity, initialAngularVelocity, beaconName, entityName, spawningOptions, ownerId, updateSync, callbacks);
		}

		public void SpawnPrefab(List<MyCubeGrid> resultList, string prefabName, Vector3D position, Vector3 forward, Vector3 up, Vector3 initialLinearVelocity = default(Vector3), Vector3 initialAngularVelocity = default(Vector3), string beaconName = null, string entityName = null, SpawningOptions spawningOptions = SpawningOptions.None, long ownerId = 0L, bool updateSync = false, Stack<Action> callbacks = null)
		{
			if (callbacks == null)
			{
				callbacks = new Stack<Action>();
			}
			SpawnPrefabInternal(resultList, prefabName, position, forward, up, initialLinearVelocity, initialAngularVelocity, beaconName, entityName, spawningOptions, ownerId, updateSync, callbacks);
		}

		void IMyPrefabManager.SpawnPrefab(List<IMyCubeGrid> resultList, string prefabName, Vector3D position, Vector3 forward, Vector3 up, Vector3 initialLinearVelocity, Vector3 initialAngularVelocity, string beaconName, SpawningOptions spawningOptions, bool updateSync, Action callback)
		{
			((IMyPrefabManager)this).SpawnPrefab(resultList, prefabName, position, forward, up, initialAngularVelocity, initialAngularVelocity, beaconName, spawningOptions, 0L, updateSync, callback);
		}

		void IMyPrefabManager.SpawnPrefab(List<IMyCubeGrid> resultList, string prefabName, Vector3D position, Vector3 forward, Vector3 up, Vector3 initialLinearVelocity, Vector3 initialAngularVelocity, string beaconName, SpawningOptions spawningOptions, long ownerId, bool updateSync, Action callback)
		{
			Stack<Action> stack = new Stack<Action>();
			if (callback != null)
			{
				stack.Push(callback);
			}
			List<MyCubeGrid> results = new List<MyCubeGrid>();
			SpawnPrefab(results, prefabName, position, forward, up, initialLinearVelocity, initialAngularVelocity, beaconName, null, spawningOptions, ownerId, updateSync, stack);
			stack.Push(delegate
			{
				foreach (MyCubeGrid item in results)
				{
					resultList.Add(item);
				}
			});
		}

		internal void SpawnPrefabInternal(List<MyCubeGrid> resultList, string prefabName, Vector3D position, Vector3 forward, Vector3 up, Vector3 initialLinearVelocity, Vector3 initialAngularVelocity, string beaconName, string entityName, SpawningOptions spawningOptions, long ownerId, bool updateSync, Stack<Action> callbacks)
		{
			MySpawnPrefabProperties spawnPrefabProperties = new MySpawnPrefabProperties
			{
				ResultList = resultList,
				PrefabName = prefabName,
				Position = position,
				Forward = forward,
				Up = up,
				InitialAngularVelocity = initialAngularVelocity,
				InitialLinearVelocity = initialLinearVelocity,
				BeaconName = beaconName,
				EntityName = entityName,
				SpawningOptions = spawningOptions,
				OwnerId = ownerId,
				UpdateSync = updateSync
			};
			SpawnPrefabInternal(spawnPrefabProperties, callbacks);
		}

		internal void SpawnPrefabInternal(MySpawnPrefabProperties spawnPrefabProperties, Action callback, Action spawnFailedCallback = null)
		{
			Stack<Action> stack = new Stack<Action>();
			if (callback != null)
			{
				stack.Push(callback);
			}
			SpawnPrefabInternal(spawnPrefabProperties, stack, spawnFailedCallback);
		}

		internal void SpawnPrefabInternal(MySpawnPrefabProperties spawnPrefabProperties, Stack<Action> callbacks, Action spawnFailedCallback = null)
		{
			if (spawnPrefabProperties == null)
			{
				throw new ArgumentNullException("spawnPrefabProperties");
			}
			if (callbacks == null)
			{
				throw new ArgumentNullException("callbacks");
			}
			if (spawnPrefabProperties.ResultList == null)
			{
				spawnPrefabProperties.ResultList = new List<MyCubeGrid>();
			}
			Vector3 forward = spawnPrefabProperties.Forward;
			Vector3 up = spawnPrefabProperties.Up;
			MatrixD worldMatrix = MatrixD.CreateWorld(spawnPrefabProperties.Position, forward, up);
			CreateGridsData createGridsData = new CreateGridsData(spawnPrefabProperties.ResultList, spawnPrefabProperties.PrefabName, worldMatrix, spawnPrefabProperties.SpawningOptions, ignoreMemoryLimits: true, spawnPrefabProperties.OwnerId, callbacks, spawnFailedCallback);
			Interlocked.Increment(ref PendingGrids);
			callbacks.Push(delegate
			{
				SpawnPrefabInternalSetProperties(spawnPrefabProperties);
			});
			callbacks.Push(delegate
			{
				if (spawnPrefabProperties.ResultList.Count > 0)
				{
					MyVisualScriptLogicProvider.PrefabSpawnedDetailed?.Invoke(spawnPrefabProperties.ResultList[0].EntityId, spawnPrefabProperties.PrefabName);
				}
			});
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.PrepareSwapData();
				MyEntityIdentifier.SwapPerThreadData();
			}
			Parallel.Start(createGridsData.CallCreateGridsFromPrefab, createGridsData.OnGridsCreated, createGridsData);
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.ClearSwapDataAndRestore();
			}
		}

		private void SpawnPrefabInternalSetProperties(MySpawnPrefabProperties spawnPrefabProperties)
		{
			int num = 0;
			using (spawnPrefabProperties.UpdateSync ? MyRandom.Instance.PushSeed(num = MyRandom.Instance.CreateRandomSeed()) : default(MyRandom.StateToken))
			{
				SpawningOptions spawningOptions = spawnPrefabProperties.SpawningOptions;
				bool flag = spawningOptions.HasFlag(SpawningOptions.RotateFirstCockpitTowardsDirection);
				bool flag2 = spawningOptions.HasFlag(SpawningOptions.SpawnRandomCargo);
				bool flag3 = spawningOptions.HasFlag(SpawningOptions.SetNeutralOwner);
				bool flag4 = spawningOptions.HasFlag(SpawningOptions.ReplaceColor);
				string beaconName = spawnPrefabProperties.BeaconName;
				string entityName = spawnPrefabProperties.EntityName;
				bool flag5 = flag2 || flag || flag3 || beaconName != null || !string.IsNullOrEmpty(entityName) || flag4;
				List<MyCockpit> list = new List<MyCockpit>();
				List<MyRemoteControl> list2 = new List<MyRemoteControl>();
				bool flag6 = spawningOptions.HasFlag(SpawningOptions.TurnOffReactors);
				foreach (MyCubeGrid result in spawnPrefabProperties.ResultList)
				{
					result.ClearSymmetries();
					if (spawningOptions.HasFlag(SpawningOptions.DisableDampeners))
					{
						MyEntityThrustComponent myEntityThrustComponent = result.Components.Get<MyEntityThrustComponent>();
						if (myEntityThrustComponent != null)
						{
							myEntityThrustComponent.DampenersEnabled = false;
						}
					}
					if (spawningOptions.HasFlag(SpawningOptions.DisableSave))
					{
						result.Save = false;
					}
					if (flag5 || flag6)
					{
						bool flag7 = false;
						foreach (MySlimBlock block in result.GetBlocks())
						{
							if (block.ColorMaskHSV.Equals(MASK_COLOR, 0.01f))
							{
								block.ColorMaskHSV = spawnPrefabProperties.Color;
								block.UpdateVisual(updatePhysics: false);
							}
							if (block.FatBlock is MyCockpit && block.FatBlock.IsFunctional)
							{
								list.Add((MyCockpit)block.FatBlock);
							}
							else if (block.FatBlock is MyCargoContainer && flag2)
							{
								(block.FatBlock as MyCargoContainer).SpawnRandomCargo();
							}
							else if (block.FatBlock is MyBeacon && beaconName != null)
							{
								(block.FatBlock as MyBeacon).SetCustomName(beaconName);
							}
							else if (flag6 && block.FatBlock != null && block.FatBlock.Components.Contains(typeof(MyResourceSourceComponent)))
							{
								MyResourceSourceComponent myResourceSourceComponent = block.FatBlock.Components.Get<MyResourceSourceComponent>();
								if (myResourceSourceComponent != null && myResourceSourceComponent.ResourceTypes.Contains(MyResourceDistributorComponent.ElectricityId))
								{
									myResourceSourceComponent.Enabled = false;
								}
							}
							else if (block.FatBlock is MyRemoteControl)
							{
								list2.Add(block.FatBlock as MyRemoteControl);
								flag7 = true;
							}
						}
						if (flag7 && !string.IsNullOrEmpty(entityName))
						{
							result.Name = entityName;
							MyEntities.SetEntityName(result, possibleRename: false);
						}
					}
				}
				if (list.Count > 1)
				{
					list.Sort(delegate(MyCockpit cockpitA, MyCockpit cockpitB)
					{
						int num2 = cockpitB.IsMainCockpit.CompareTo(cockpitA.IsMainCockpit);
						return (num2 != 0) ? num2 : cockpitB.EnableShipControl.CompareTo(cockpitA.EnableShipControl);
					});
				}
				MyCubeBlock myCubeBlock = null;
				if (list.Count > 0 && (list[0].EnableShipControl || list2.Count <= 0))
				{
					myCubeBlock = list[0];
				}
				if (myCubeBlock == null && list2.Count > 0)
				{
					if (list2.Count > 1)
					{
						list2.Sort((MyRemoteControl remoteA, MyRemoteControl remoteB) => remoteB.IsMainRemoteControl.CompareTo(remoteA.IsMainCockpit));
					}
					myCubeBlock = list2[0];
				}
				Matrix matrix = Matrix.Identity;
				if (flag && myCubeBlock != null)
				{
					matrix = Matrix.Multiply(Matrix.Invert(myCubeBlock.WorldMatrix), Matrix.CreateWorld(myCubeBlock.WorldMatrix.Translation, spawnPrefabProperties.Forward, spawnPrefabProperties.Up));
				}
				foreach (MyCubeGrid result2 in spawnPrefabProperties.ResultList)
				{
					if (myCubeBlock != null && flag)
					{
						result2.WorldMatrix *= matrix;
					}
					if (result2.Physics != null)
					{
						result2.Physics.LinearVelocity = spawnPrefabProperties.InitialLinearVelocity;
						result2.Physics.AngularVelocity = spawnPrefabProperties.InitialAngularVelocity;
					}
					SingleKeyEntityNameEvent prefabSpawned = MyVisualScriptLogicProvider.PrefabSpawned;
					if (prefabSpawned != null)
					{
						string text = result2.Name;
						if (string.IsNullOrWhiteSpace(text))
						{
							text = result2.EntityId.ToString();
						}
						prefabSpawned(text);
					}
				}
			}
		}

		bool IMyPrefabManager.IsPathClear(Vector3D from, Vector3D to)
		{
			MyPhysics.CastRay(from, to, m_raycastHits, 24);
			m_raycastHits.Clear();
			return m_raycastHits.Count == 0;
		}

		bool IMyPrefabManager.IsPathClear(Vector3D from, Vector3D to, double halfSize)
		{
			Vector3D vector3D = default(Vector3D);
			vector3D.X = 1.0;
			Vector3D vector = to - from;
			vector.Normalize();
			if (Vector3D.Dot(vector, vector3D) > 0.89999997615814209 || Vector3D.Dot(vector, vector3D) < -0.89999997615814209)
			{
				vector3D.X = 0.0;
				vector3D.Y = 1.0;
			}
			vector3D = Vector3D.Cross(vector, vector3D);
			vector3D.Normalize();
			vector3D *= halfSize;
			MyPhysics.CastRay(from + vector3D, to + vector3D, m_raycastHits, 24);
			if (m_raycastHits.Count > 0)
			{
				m_raycastHits.Clear();
				return false;
			}
			vector3D *= -1.0;
			MyPhysics.CastRay(from + vector3D, to + vector3D, m_raycastHits, 24);
			if (m_raycastHits.Count > 0)
			{
				m_raycastHits.Clear();
				return false;
			}
			vector3D = Vector3D.Cross(vector, vector3D);
			MyPhysics.CastRay(from + vector3D, to + vector3D, m_raycastHits, 24);
			if (m_raycastHits.Count > 0)
			{
				m_raycastHits.Clear();
				return false;
			}
			vector3D *= -1.0;
			MyPhysics.CastRay(from + vector3D, to + vector3D, m_raycastHits, 24);
			if (m_raycastHits.Count > 0)
			{
				m_raycastHits.Clear();
				return false;
			}
			return true;
		}
	}
}
