using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.Modules
{
	public class MyVoxelMapEnvironmentProxy : IMyEnvironmentModuleProxy
	{
		protected struct VoxelMapInfo
		{
			public string Name;

			public MyDefinitionId Storage;

			public MatrixD Matrix;

			public int Item;

			public MyStringHash Modifier;

			public long EntityId;
		}

		protected MyEnvironmentSector m_sector;

		protected MyPlanet m_planet;

		protected readonly MyRandom m_random = new MyRandom();

		protected readonly MyVoxelBase.StorageChanged m_voxelMap_RangeChangedDelegate;

		protected List<int> m_items;

		protected List<VoxelMapInfo> m_voxelMapsToAdd = new List<VoxelMapInfo>();

		protected Dictionary<MyVoxelMap, int> m_voxelMaps = new Dictionary<MyVoxelMap, int>();

		private static List<MyEntity> m_entities = new List<MyEntity>();

		public MyVoxelMapEnvironmentProxy()
		{
			m_voxelMap_RangeChangedDelegate = VoxelMap_RangeChanged;
		}

		public void Init(MyEnvironmentSector sector, List<int> items)
		{
			m_sector = sector;
			MyEntityComponentBase myEntityComponentBase = (MyEntityComponentBase)m_sector.Owner;
			m_planet = (myEntityComponentBase.Entity as MyPlanet);
			m_items = items;
			LoadVoxelMapsInfo();
		}

		public void Close()
		{
			RemoveVoxelMaps();
		}

		public void CommitLodChange(int lodBefore, int lodAfter)
		{
			if (lodAfter >= 0)
			{
				AddVoxelMaps();
			}
			else if (!m_sector.HasPhysics)
			{
				RemoveVoxelMaps();
			}
		}

		public void CommitPhysicsChange(bool enabled)
		{
			if (enabled)
			{
				AddVoxelMaps();
			}
			else if (m_sector.LodLevel == -1)
			{
				RemoveVoxelMaps();
			}
		}

		public void OnItemChange(int index, short newModel)
		{
		}

		public void OnItemChangeBatch(List<int> items, int offset, short newModel)
		{
		}

		public void HandleSyncEvent(int item, object data, bool fromClient)
		{
		}

		public void DebugDraw()
		{
		}

		private void LoadVoxelMapsInfo()
		{
			m_voxelMapsToAdd.Clear();
			foreach (int item in m_items)
			{
				ItemInfo itemInfo = m_sector.DataView.Items[item];
				m_sector.Owner.GetDefinition((ushort)itemInfo.DefinitionIndex, out MyRuntimeEnvironmentItemInfo def);
				MyDefinitionId subtypeId = new MyDefinitionId(typeof(MyObjectBuilder_VoxelMapCollectionDefinition), def.Subtype);
				MyVoxelMapCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyVoxelMapCollectionDefinition>(subtypeId);
				if (definition != null)
				{
					m_sector.DataView.GetLogicalSector(item, out int logicalItem, out MyLogicalEnvironmentSectorBase sector);
					string text = $"P({m_sector.Owner.Entity.Name})S({sector.Id})A({def.Subtype}__{logicalItem})";
					MatrixD matrix = MatrixD.CreateFromQuaternion(itemInfo.Rotation);
					matrix.Translation = m_sector.SectorCenter + itemInfo.Position;
					long entityId = MyEntityIdentifier.ConstructIdFromString(MyEntityIdentifier.ID_OBJECT_TYPE.PLANET_VOXEL_DETAIL, text);
					using (m_random.PushSeed(itemInfo.Rotation.GetHashCode()))
					{
						m_voxelMapsToAdd.Add(new VoxelMapInfo
						{
							Name = text,
							Storage = definition.StorageFiles.Sample(m_random),
							Matrix = matrix,
							Item = item,
							Modifier = definition.Modifier,
							EntityId = entityId
						});
					}
				}
			}
		}

		private void AddVoxelMaps()
		{
			if (m_voxelMaps.Count <= 0)
			{
				foreach (VoxelMapInfo item2 in m_voxelMapsToAdd)
				{
					if (MyEntities.TryGetEntityById(item2.EntityId, out MyVoxelMap entity))
					{
						if (!entity.Save)
						{
							RegisterVoxelMap(item2.Item, entity);
						}
					}
					else
					{
						MyVoxelMaterialModifierDefinition definition = MyDefinitionManager.Static.GetDefinition<MyVoxelMaterialModifierDefinition>(item2.Modifier);
						Dictionary<byte, byte> modifiers = null;
						if (definition != null)
						{
							modifiers = definition.Options.Sample(MyHashRandomUtils.UniformFloatFromSeed(item2.Item + m_sector.SectorId.GetHashCode())).Changes;
						}
						int item = item2.Item;
						MyDefinitionId storage = item2.Storage;
						AddVoxelMap(item, storage.SubtypeName, item2.Matrix, item2.Name, item2.EntityId, modifiers);
					}
				}
			}
		}

		private void AddVoxelMap(int item, string prefabName, MatrixD matrix, string name, long entityId, Dictionary<byte, byte> modifiers = null)
		{
			MyStorageBase myStorageBase = MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(prefabName), modifiers);
			if (myStorageBase != null)
			{
				MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(matrix.Translation, myStorageBase.Size * 0.5f, Quaternion.CreateFromRotationMatrix(matrix));
				BoundingBoxD box = other.GetAABB();
				using (MyUtils.ReuseCollection(ref m_entities))
				{
					MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, m_entities, MyEntityQueryType.Static);
					foreach (MyEntity entity in m_entities)
					{
						if (!(entity is MyVoxelPhysics) && !(entity is MyPlanet) && !(entity is MyEnvironmentSector))
						{
							MyPositionComponentBase positionComp = entity.PositionComp;
							if (MyOrientedBoundingBoxD.Create(positionComp.LocalAABB, positionComp.WorldMatrix).Intersects(ref other))
							{
								return;
							}
						}
					}
				}
				MyVoxelMap myVoxelMap = MyWorldGenerator.AddVoxelMap(name, myStorageBase, matrix, entityId, lazyPhysics: true);
				if (myVoxelMap != null)
				{
					RegisterVoxelMap(item, myVoxelMap);
				}
			}
		}

		private void RegisterVoxelMap(int item, MyVoxelMap voxelMap)
		{
			voxelMap.Save = false;
			voxelMap.RangeChanged += m_voxelMap_RangeChangedDelegate;
			m_voxelMaps[voxelMap] = item;
			if (!voxelMap.Components.TryGet(out MyEntityReferenceComponent component))
			{
				voxelMap.Components.Add(component = new MyEntityReferenceComponent());
			}
			DisableOtherItemsInVMap(voxelMap);
			component.Ref();
		}

		private unsafe void DisableOtherItemsInVMap(MyVoxelBase voxelMap)
		{
			MyOrientedBoundingBoxD obb = MyOrientedBoundingBoxD.Create(voxelMap.PositionComp.LocalAABB, voxelMap.PositionComp.WorldMatrix);
			Vector3D center = obb.Center;
			BoundingBoxD box = voxelMap.PositionComp.WorldAABB;
			using (MyUtils.ReuseCollection(ref m_entities))
			{
				MyGamePruningStructure.GetAllEntitiesInBox(ref box, m_entities, MyEntityQueryType.Static);
				for (int j = 0; j < m_entities.Count; j++)
				{
					MyEnvironmentSector sector = m_entities[j] as MyEnvironmentSector;
					if (sector != null && sector.DataView != null)
					{
						obb.Center = center - sector.SectorCenter;
						for (int k = 0; k < sector.DataView.LogicalSectors.Count; k++)
						{
							MyLogicalEnvironmentSectorBase logicalSector = sector.DataView.LogicalSectors[k];
							logicalSector.IterateItems(delegate(int i, ref ItemInfo x)
							{
								Vector3D vector3D = x.Position + sector.SectorCenter;
								if (x.DefinitionIndex >= 0 && obb.Contains(ref x.Position) && voxelMap.CountPointsInside(&vector3D, 1) > 0 && !IsVoxelItem(sector, x.DefinitionIndex))
								{
									logicalSector.EnableItem(i, enabled: false);
								}
							});
						}
					}
				}
			}
		}

		private static bool IsVoxelItem(MyEnvironmentSector sector, short definitionIndex)
		{
			MyItemTypeDefinition.Module[] proxyModules = sector.Owner.EnvironmentDefinition.Items[definitionIndex].Type.ProxyModules;
			if (proxyModules == null)
			{
				return false;
			}
			for (int i = 0; i < proxyModules.Length; i++)
			{
				if (proxyModules[i].Type.IsSubclassOf(typeof(MyVoxelMapEnvironmentProxy)) || proxyModules[i].Type == typeof(MyVoxelMapEnvironmentProxy))
				{
					return true;
				}
			}
			return false;
		}

		private void RemoveVoxelMaps()
		{
			foreach (KeyValuePair<MyVoxelMap, int> voxelMap in m_voxelMaps)
			{
				MyVoxelMap key = voxelMap.Key;
				if (!key.Closed)
				{
					if (Sync.IsServer || !key.Save)
					{
						key.Components.Get<MyEntityReferenceComponent>().Unref();
					}
					key.RangeChanged -= m_voxelMap_RangeChangedDelegate;
				}
			}
			m_voxelMaps.Clear();
			m_voxelMapsToAdd.Clear();
		}

		private void RemoveVoxelMap(MyVoxelMap map)
		{
			map.Save = true;
			map.RangeChanged -= m_voxelMap_RangeChangedDelegate;
			if (m_voxelMaps.ContainsKey(map))
			{
				int itemId = m_voxelMaps[map];
				m_sector.EnableItem(itemId, enabled: false);
				m_voxelMaps.Remove(map);
			}
		}

		private void VoxelMap_RangeChanged(MyVoxelBase voxel, Vector3I minVoxelChanged, Vector3I maxVoxelChanged, MyStorageDataTypeFlags changedData)
		{
			RemoveVoxelMap((MyVoxelMap)voxel);
		}
	}
}
