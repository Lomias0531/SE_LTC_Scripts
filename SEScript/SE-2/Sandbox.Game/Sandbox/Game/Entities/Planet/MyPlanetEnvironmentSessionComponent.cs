using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500)]
	public class MyPlanetEnvironmentSessionComponent : MySessionComponentBase
	{
		protected sealed class DisableItemsInSector_003C_003ESystem_Int64_0023System_Int64_0023System_Collections_Generic_List_00601_003CSystem_Int32_003E : ICallSite<IMyEventOwner, long, long, List<int>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long planetId, in long sectorId, in List<int> items, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DisableItemsInSector(planetId, sectorId, items);
			}
		}

		private const int TIME_TO_UPDATE = 10;

		private const int UPDATES_TO_LAZY_UPDATE = 10;

		private int m_updateInterval;

		private int m_lazyUpdateInterval;

		public static bool EnableUpdate = true;

		public static bool DebugDrawSectors = false;

		public static bool DebugDrawDynamicObjectClusters = false;

		public static bool DebugDrawEnvironmentProviders = false;

		public static bool DebugDrawActiveSectorItems = false;

		public static bool DebugDrawActiveSectorProvider = false;

		public static bool DebugDrawProxies = false;

		public static bool DebugDrawCollisionCheckers = false;

		public static float DebugDrawDistance = 150f;

		private readonly HashSet<IMyEnvironmentDataProvider> m_environmentProviders = new HashSet<IMyEnvironmentDataProvider>();

		private readonly HashSet<MyPlanetEnvironmentComponent> m_planetEnvironments = new HashSet<MyPlanetEnvironmentComponent>();

		public static MyEnvironmentSector ActiveSector;

		private const int NewEnvReleaseVersion = 1133002;

		private MyListDictionary<MyEntity, BoundingBoxD> m_cubeBlocksToWork = new MyListDictionary<MyEntity, BoundingBoxD>();

		private volatile MyListDictionary<MyEntity, BoundingBoxD> m_cubeBlocksPending = new MyListDictionary<MyEntity, BoundingBoxD>();

		private volatile bool m_itemDisableJobRunning;

		private List<MyVoxelBase> m_tmpVoxelList = new List<MyVoxelBase>();

		private List<MyEntity> m_tmpEntityList = new List<MyEntity>();

		private MyListDictionary<MyEnvironmentSector, int> m_itemsToDisable = new MyListDictionary<MyEnvironmentSector, int>();

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyCubeGrids)
		};

		public override bool UpdatedBeforeInit()
		{
			return true;
		}

		public override void UpdateBeforeSimulation()
		{
			if (!EnableUpdate)
			{
				return;
			}
			m_updateInterval++;
			if (m_updateInterval > 10)
			{
				m_updateInterval = 0;
				m_lazyUpdateInterval++;
				bool doLazy = false;
				if (m_lazyUpdateInterval > 10)
				{
					doLazy = true;
					m_lazyUpdateInterval = 0;
				}
				UpdatePlanetEnvironments(doLazy);
				if (!m_itemDisableJobRunning && m_cubeBlocksPending.Count() > 0)
				{
					Parallel.Start(GatherEnvItemsInBoxes, DisableGatheredItems);
					m_itemDisableJobRunning = true;
				}
			}
		}

		public override void Draw()
		{
			if (DebugDrawEnvironmentProviders)
			{
				foreach (IMyEnvironmentDataProvider environmentProvider in m_environmentProviders)
				{
					environmentProvider.DebugDraw();
				}
			}
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position);
			if (DebugDrawSectors && closestPlanet != null)
			{
				ActiveSector = closestPlanet.Components.Get<MyPlanetEnvironmentComponent>().GetSectorForPosition(MySector.MainCamera.Position);
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			_ = Sync.IsServer;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			_ = Sync.IsServer;
		}

		public void RegisterPlanetEnvironment(MyPlanetEnvironmentComponent env)
		{
			m_planetEnvironments.Add(env);
			IMyEnvironmentDataProvider[] providers = env.Providers;
			foreach (IMyEnvironmentDataProvider item in providers)
			{
				m_environmentProviders.Add(item);
			}
		}

		public void UnregisterPlanetEnvironment(MyPlanetEnvironmentComponent env)
		{
			m_planetEnvironments.Remove(env);
			IMyEnvironmentDataProvider[] providers = env.Providers;
			foreach (IMyEnvironmentDataProvider item in providers)
			{
				m_environmentProviders.Remove(item);
			}
		}

		private void UpdatePlanetEnvironments(bool doLazy)
		{
			foreach (MyPlanetEnvironmentComponent planetEnvironment in m_planetEnvironments)
			{
				planetEnvironment.Update(doLazy);
			}
		}

		public override void BeforeStart()
		{
			if (MySession.Static.AppVersionFromSave < 1133002)
			{
				foreach (MyPlanetEnvironmentComponent planetEnvironment in m_planetEnvironments)
				{
					planetEnvironment.InitClearAreasManagement();
				}
			}
		}

		private void OnEntityAdded(MyEntity myEntity)
		{
			_ = MySession.Static.Ready;
		}

		private void MyCubeGridsOnBlockBuilt(MyCubeGrid myCubeGrid, MySlimBlock mySlimBlock)
		{
			if (mySlimBlock == null || !myCubeGrid.IsStatic)
			{
				return;
			}
			MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(mySlimBlock.Min);
			if (cubeBlock != null)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null && mySlimBlock.FatBlock != myCompoundCubeBlock)
				{
					return;
				}
			}
			mySlimBlock.GetWorldBoundingBox(out BoundingBoxD aabb, useAABBFromBlockCubes: true);
			m_cubeBlocksPending.Add(myCubeGrid, aabb);
		}

		public void ClearEnvironmentItems(MyEntity entity, BoundingBoxD worldBBox)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.Error("This method can be used only on server.");
			}
			else
			{
				m_cubeBlocksPending.Add(entity, worldBBox);
			}
		}

		private void GatherEnvItemsInBoxes()
		{
			MyListDictionary<MyEntity, BoundingBoxD> myListDictionary = m_cubeBlocksToWork = Interlocked.Exchange(ref m_cubeBlocksPending, m_cubeBlocksToWork);
			int num = 0;
			int num2 = 0;
			foreach (List<BoundingBoxD> value in myListDictionary.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					BoundingBoxD box = value[i];
					MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_tmpVoxelList);
					num2++;
					for (int j = 0; j < m_tmpVoxelList.Count; j++)
					{
						MyPlanet myPlanet = m_tmpVoxelList[j] as MyPlanet;
						if (myPlanet != null)
						{
							myPlanet.Hierarchy.QueryAABB(ref box, m_tmpEntityList);
							for (int k = 0; k < m_tmpEntityList.Count; k++)
							{
								MyEnvironmentSector myEnvironmentSector = m_tmpEntityList[k] as MyEnvironmentSector;
								if (myEnvironmentSector == null)
								{
									return;
								}
								BoundingBoxD aabb = box;
								myEnvironmentSector.GetItemsInAabb(ref aabb, m_itemsToDisable.GetOrAdd(myEnvironmentSector));
								if (myEnvironmentSector.DataView != null && myEnvironmentSector.DataView.Items != null)
								{
									num += myEnvironmentSector.DataView.Items.Count;
								}
							}
							m_tmpEntityList.Clear();
						}
					}
					m_tmpVoxelList.Clear();
				}
			}
			myListDictionary.Clear();
		}

		public void DisableGatheredItems()
		{
			foreach (KeyValuePair<MyEnvironmentSector, List<int>> item in m_itemsToDisable)
			{
				for (int i = 0; i < item.Value.Count; i++)
				{
					item.Key.EnableItem(item.Value[i], enabled: false);
				}
				if (item.Value.Count > 0)
				{
					long entityId = item.Key.Owner.Entity.EntityId;
					long sectorId = item.Key.SectorId;
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => DisableItemsInSector, entityId, sectorId, item.Value);
				}
			}
			m_itemsToDisable.Clear();
			m_itemDisableJobRunning = false;
		}

		[Broadcast]
		[Event(null, 314)]
		[Reliable]
		public static void DisableItemsInSector(long planetId, long sectorId, List<int> items)
		{
			if (!MyEntities.TryGetEntityById(planetId, out MyPlanet entity))
			{
				return;
			}
			MyPlanetEnvironmentComponent myPlanetEnvironmentComponent = entity.Components.Get<MyPlanetEnvironmentComponent>();
			if (myPlanetEnvironmentComponent != null && myPlanetEnvironmentComponent.TryGetSector(sectorId, out MyEnvironmentSector environmentSector))
			{
				for (int i = 0; i < items.Count; i++)
				{
					environmentSector.EnableItem(items[i], enabled: false);
				}
			}
		}
	}
}
