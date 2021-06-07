using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public class MyVoxelMaps : IMyVoxelMaps
	{
		private readonly Dictionary<uint, MyRenderComponentVoxelMap> m_renderComponentsByClipmapId = new Dictionary<uint, MyRenderComponentVoxelMap>();

		private readonly Dictionary<long, MyVoxelBase> m_voxelMapsByEntityId = new Dictionary<long, MyVoxelBase>();

		private readonly List<MyVoxelBase> m_tmpVoxelMapsList = new List<MyVoxelBase>();

		private static MyShapeBox m_boxVoxelShape = new MyShapeBox();

		private static MyShapeCapsule m_capsuleShape = new MyShapeCapsule();

		private static MyShapeSphere m_sphereShape = new MyShapeSphere();

		private static MyShapeRamp m_rampShape = new MyShapeRamp();

		private static readonly List<MyVoxelBase> m_voxelsTmpStorage = new List<MyVoxelBase>();

		public DictionaryValuesReader<long, MyVoxelBase> Instances => m_voxelMapsByEntityId;

		int IMyVoxelMaps.VoxelMaterialCount => MyDefinitionManager.Static.VoxelMaterialCount;

		public void Clear()
		{
			foreach (KeyValuePair<long, MyVoxelBase> item in m_voxelMapsByEntityId)
			{
				item.Value.Close();
			}
			MyStorageBase.ResetCache();
			m_voxelMapsByEntityId.Clear();
			m_renderComponentsByClipmapId.Clear();
		}

		public bool Exist(MyVoxelBase voxelMap)
		{
			return m_voxelMapsByEntityId.ContainsKey(voxelMap.EntityId);
		}

		public void RemoveVoxelMap(MyVoxelBase voxelMap)
		{
			if (m_voxelMapsByEntityId.Remove(voxelMap.EntityId))
			{
				MyRenderComponentBase render = voxelMap.Render;
				if (render is MyRenderComponentVoxelMap)
				{
					uint clipmapId = (render as MyRenderComponentVoxelMap).ClipmapId;
					m_renderComponentsByClipmapId.Remove(clipmapId);
				}
			}
		}

		public MyVoxelBase GetOverlappingWithSphere(ref BoundingSphereD sphere)
		{
			MyVoxelBase result = null;
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, m_tmpVoxelMapsList);
			foreach (MyVoxelBase tmpVoxelMaps in m_tmpVoxelMapsList)
			{
				if (tmpVoxelMaps.DoOverlapSphereTest((float)sphere.Radius, sphere.Center))
				{
					result = tmpVoxelMaps;
					break;
				}
			}
			m_tmpVoxelMapsList.Clear();
			return result;
		}

		public void GetAllOverlappingWithSphere(ref BoundingSphereD sphere, List<MyVoxelBase> voxels)
		{
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, voxels);
		}

		public List<MyVoxelBase> GetAllOverlappingWithSphere(ref BoundingSphereD sphere)
		{
			List<MyVoxelBase> result = new List<MyVoxelBase>();
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, result);
			return result;
		}

		public void Add(MyVoxelBase voxelMap)
		{
			if (!Exist(voxelMap))
			{
				m_voxelMapsByEntityId.Add(voxelMap.EntityId, voxelMap);
				MyRenderComponentBase render = voxelMap.Render;
				if (render is MyRenderComponentVoxelMap)
				{
					uint clipmapId = (render as MyRenderComponentVoxelMap).ClipmapId;
					m_renderComponentsByClipmapId[clipmapId] = (render as MyRenderComponentVoxelMap);
				}
			}
		}

		internal bool TryGetRenderComponent(uint clipmapId, out MyRenderComponentVoxelMap render)
		{
			return m_renderComponentsByClipmapId.TryGetValue(clipmapId, out render);
		}

		public MyVoxelBase GetVoxelMapWhoseBoundingBoxIntersectsBox(ref BoundingBoxD boundingBox, MyVoxelBase ignoreVoxelMap)
		{
			MyVoxelBase result = null;
			double num = double.MaxValue;
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (!value.MarkedForClose && !value.Closed && value != ignoreVoxelMap && value.IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref boundingBox))
				{
					double num2 = Vector3D.DistanceSquared(value.PositionComp.WorldAABB.Center, boundingBox.Center);
					if (num2 < num)
					{
						num = num2;
						result = value;
					}
				}
			}
			return result;
		}

		public bool GetVoxelMapsWhoseBoundingBoxesIntersectBox(ref BoundingBoxD boundingBox, MyVoxelBase ignoreVoxelMap, List<MyVoxelBase> voxelList)
		{
			int num = 0;
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (!value.MarkedForClose && !value.Closed && value != ignoreVoxelMap && value.IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref boundingBox))
				{
					voxelList.Add(value);
					num++;
				}
			}
			return num > 0;
		}

		public MyVoxelBase TryGetVoxelMapByNameStart(string name)
		{
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (value.StorageName != null && value.StorageName.StartsWith(name))
				{
					return value;
				}
			}
			return null;
		}

		public MyVoxelBase TryGetVoxelMapByName(string name)
		{
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (value.StorageName == name)
				{
					return value;
				}
			}
			return null;
		}

		public MyVoxelBase TryGetVoxelBaseById(long id)
		{
			if (!m_voxelMapsByEntityId.ContainsKey(id))
			{
				return null;
			}
			return m_voxelMapsByEntityId[id];
		}

		public Dictionary<string, byte[]> GetVoxelMapsArray(bool includeChanged)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (value.Storage != null && (includeChanged || (!value.ContentChanged && !value.BeforeContentChanged)) && value.Save && !dictionary.ContainsKey(value.StorageName))
				{
					byte[] outCompressedData = null;
					value.Storage.Save(out outCompressedData);
					dictionary.Add(value.StorageName, outCompressedData);
				}
			}
			return dictionary;
		}

		public Dictionary<string, byte[]> GetVoxelMapsData(bool includeChanged, bool cached, Dictionary<string, VRage.Game.Voxels.IMyStorage> voxelStorageNameCache = null)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (value.Storage != null && (includeChanged || (!value.ContentChanged && !value.BeforeContentChanged)) && value.Save && !dictionary.ContainsKey(value.StorageName) && value.Storage.AreCompressedDataCached == cached)
				{
					byte[] outCompressedData = null;
					if (cached)
					{
						value.Storage.Save(out outCompressedData);
					}
					else
					{
						outCompressedData = value.Storage.GetVoxelData();
					}
					dictionary.Add(value.StorageName, outCompressedData);
					voxelStorageNameCache?.Add(value.StorageName, value.Storage);
				}
			}
			return dictionary;
		}

		public void DebugDraw(MyVoxelDebugDrawMode drawMode)
		{
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (!(value is MyVoxelPhysics))
				{
					MatrixD worldMatrix = value.WorldMatrix;
					worldMatrix.Translation = value.PositionLeftBottomCorner;
					value.Storage.DebugDraw(ref worldMatrix, drawMode);
				}
			}
		}

		public void GetCacheStats(out int cachedChuncks, out int pendingCachedChuncks)
		{
			cachedChuncks = (pendingCachedChuncks = 0);
			foreach (KeyValuePair<long, MyVoxelBase> item in m_voxelMapsByEntityId)
			{
				if (!(item.Value is MyVoxelPhysics))
				{
					MyOctreeStorage myOctreeStorage = item.Value.Storage as MyOctreeStorage;
					if (myOctreeStorage != null)
					{
						cachedChuncks += myOctreeStorage.CachedChunksCount;
						pendingCachedChuncks += myOctreeStorage.PendingCachedChunksCount;
					}
				}
			}
		}

		internal void GetAllIds(ref List<long> list)
		{
			foreach (long key in m_voxelMapsByEntityId.Keys)
			{
				list.Add(key);
			}
		}

		void IMyVoxelMaps.Clear()
		{
			Clear();
		}

		bool IMyVoxelMaps.Exist(IMyVoxelBase voxelMap)
		{
			return Exist(voxelMap as MyVoxelBase);
		}

		IMyVoxelBase IMyVoxelMaps.GetOverlappingWithSphere(ref BoundingSphereD sphere)
		{
			m_voxelsTmpStorage.Clear();
			GetAllOverlappingWithSphere(ref sphere, m_voxelsTmpStorage);
			if (m_voxelsTmpStorage.Count == 0)
			{
				return null;
			}
			return m_voxelsTmpStorage[0];
		}

		IMyVoxelBase IMyVoxelMaps.GetVoxelMapWhoseBoundingBoxIntersectsBox(ref BoundingBoxD boundingBox, IMyVoxelBase ignoreVoxelMap)
		{
			return GetVoxelMapWhoseBoundingBoxIntersectsBox(ref boundingBox, ignoreVoxelMap as MyVoxelBase);
		}

		void IMyVoxelMaps.GetInstances(List<IMyVoxelBase> voxelMaps, Func<IMyVoxelBase, bool> collect)
		{
			foreach (MyVoxelBase instance in Instances)
			{
				if (collect == null || collect(instance))
				{
					voxelMaps.Add(instance);
				}
			}
		}

		VRage.ModAPI.IMyStorage IMyVoxelMaps.CreateStorage(Vector3I size)
		{
			return new MyOctreeStorage(null, size);
		}

		IMyVoxelMap IMyVoxelMaps.CreateVoxelMap(string storageName, VRage.ModAPI.IMyStorage storage, Vector3D position, long voxelMapId)
		{
			MyVoxelMap myVoxelMap = new MyVoxelMap();
			myVoxelMap.EntityId = voxelMapId;
			myVoxelMap.Init(storageName, storage as VRage.Game.Voxels.IMyStorage, position);
			MyEntities.Add(myVoxelMap);
			return myVoxelMap;
		}

		IMyVoxelMap IMyVoxelMaps.CreateVoxelMapFromStorageName(string storageName, string prefabVoxelMapName, Vector3D position)
		{
			MyStorageBase myStorageBase = MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(prefabVoxelMapName));
			if (myStorageBase == null)
			{
				return null;
			}
			myStorageBase.DataProvider = MyCompositeShapeProvider.CreateAsteroidShape(0, (float)myStorageBase.Size.AbsMax() * 1f);
			return MyWorldGenerator.AddVoxelMap(storageName, myStorageBase, position, 0L);
		}

		VRage.ModAPI.IMyStorage IMyVoxelMaps.CreateStorage(byte[] data)
		{
			return MyStorageBase.Load(data);
		}

		IMyVoxelShapeBox IMyVoxelMaps.GetBoxVoxelHand()
		{
			return m_boxVoxelShape;
		}

		IMyVoxelShapeCapsule IMyVoxelMaps.GetCapsuleVoxelHand()
		{
			return m_capsuleShape;
		}

		IMyVoxelShapeSphere IMyVoxelMaps.GetSphereVoxelHand()
		{
			return m_sphereShape;
		}

		IMyVoxelShapeRamp IMyVoxelMaps.GetRampVoxelHand()
		{
			return m_rampShape;
		}

		void IMyVoxelMaps.PaintInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx)
		{
			MyVoxelGenerator.RequestPaintInShape(voxelMap, voxelShape, materialIdx);
		}

		void IMyVoxelMaps.CutOutShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape)
		{
			MyVoxelGenerator.RequestCutOutShape(voxelMap, voxelShape);
		}

		void IMyVoxelMaps.FillInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx)
		{
			MyVoxelGenerator.RequestFillInShape(voxelMap, voxelShape, materialIdx);
		}

		void IMyVoxelMaps.RevertShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape)
		{
			MyVoxelGenerator.RequestRevertShape(voxelMap, voxelShape);
		}

		void IMyVoxelMaps.MakeCrater(IMyVoxelBase voxelMap, BoundingSphereD sphere, Vector3 direction, byte materialIdx)
		{
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialIdx);
			MyVoxelGenerator.MakeCrater((MyVoxelBase)voxelMap, sphere, direction, voxelMaterialDefinition);
		}
	}
}
