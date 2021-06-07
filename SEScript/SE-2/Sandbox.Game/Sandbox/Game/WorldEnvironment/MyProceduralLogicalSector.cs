using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.WorldEnvironment
{
	public class MyProceduralLogicalSector : MyLogicalEnvironmentSectorBase
	{
		private class ModuleData
		{
			public readonly Dictionary<short, MyLodEnvironmentItemSet> ItemsPerDefinition = new Dictionary<short, MyLodEnvironmentItemSet>();

			public readonly IMyEnvironmentModule Module;

			public readonly MyDefinitionId Definition;

			public ModuleData(Type type, MyDefinitionId definition)
			{
				Module = (IMyEnvironmentModule)Activator.CreateInstance(type);
				Definition = definition;
			}
		}

		private class ProgressiveScanHelper
		{
			private readonly int m_itemsTotal;

			private readonly int m_offset;

			private const bool EXAGERATE = true;

			private readonly double m_base;

			private readonly double m_logMaxLodRecip;

			public ProgressiveScanHelper(int finalCount, int offset)
			{
				m_itemsTotal = finalCount;
				int num = 4;
				m_logMaxLodRecip = 1.0 / Math.Log(num);
				m_base = Math.Log(10.0) * m_logMaxLodRecip;
				m_offset = offset;
			}

			private double F(double x)
			{
				return (0.0 - Math.Pow(m_base, 0.0 - x)) * m_logMaxLodRecip;
			}

			public int GetItemsForLod(int lod)
			{
				lod += m_offset;
				return (int)((double)m_itemsTotal * (F(lod + 1) - F(lod)));
			}
		}

		protected sealed class HandleItemEventServer_003C_003ESystem_Int32_0023VRage_ObjectBuilders_SerializableDefinitionId_0023System_Object : ICallSite<MyProceduralLogicalSector, int, SerializableDefinitionId, object, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProceduralLogicalSector @this, in int logicalItem, in SerializableDefinitionId def, in object data, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.HandleItemEventServer(logicalItem, def, data);
			}
		}

		protected sealed class HandleItemEventClient_003C_003ESystem_Int32_0023VRage_ObjectBuilders_SerializableDefinitionId_0023System_Object : ICallSite<MyProceduralLogicalSector, int, SerializableDefinitionId, object, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProceduralLogicalSector @this, in int logicalItem, in SerializableDefinitionId def, in object data, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.HandleItemEventClient(logicalItem, def, data);
			}
		}

		private readonly Dictionary<Type, MyObjectBuilder_EnvironmentModuleBase> m_moduleData = new Dictionary<Type, MyObjectBuilder_EnvironmentModuleBase>();

		private bool m_scanning;

		private bool m_serverOwned;

		private int m_itemOffset;

		private int m_locationOffset;

		private readonly int m_x;

		private readonly int m_y;

		private readonly int m_lod;

		private readonly int[] m_itemCountForLod = new int[16];

		private readonly MyProceduralEnvironmentProvider m_provider;

		private readonly HashSet<MyProceduralDataView> m_viewers = new HashSet<MyProceduralDataView>();

		private readonly Vector3 m_basisX;

		private readonly Vector3 m_basisY;

		internal bool Replicable;

		private readonly FastResourceLock m_lock = new FastResourceLock();

		private readonly MyList<ItemInfo> m_items;

		private readonly int m_itemCountTotal;

		private readonly MyProceduralEnvironmentDefinition m_environment;

		private int m_minimumScannedLod = 16;

		private int m_totalSpawned;

		private readonly Dictionary<Type, ModuleData> m_modules = new Dictionary<Type, ModuleData>();

		private readonly int m_seed;

		private readonly MyRandom m_itemPositionRng;

		private readonly List<MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>> m_candidates = new List<MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>>();

		private readonly ProgressiveScanHelper m_scanHelper;

		public override bool ServerOwned
		{
			get
			{
				return m_serverOwned;
			}
			internal set
			{
				m_serverOwned = value;
				if (!Sync.IsServer && m_viewers.Count == 0 && this.OnViewerEmpty != null)
				{
					this.OnViewerEmpty(this);
				}
			}
		}

		public override string DebugData => $"x:{m_x} y:{m_y} highLod:{m_lod} localLod:{m_minimumScannedLod} seed:{m_seed:X} count:{m_items.Count} ";

		public event Action<MyProceduralLogicalSector> OnViewerEmpty;

		public MyProceduralLogicalSector(MyProceduralEnvironmentProvider provider, int x, int y, int localLod, MyObjectBuilder_ProceduralEnvironmentSector moduleData)
		{
			m_provider = provider;
			base.Owner = provider.Owner;
			m_x = x;
			m_y = y;
			m_lod = localLod;
			provider.GeSectorWorldParameters(x, y, localLod * provider.LodFactor, out WorldPos, out m_basisX, out m_basisY);
			m_environment = (MyProceduralEnvironmentDefinition)provider.Owner.EnvironmentDefinition;
			m_seed = (provider.GetSeed() ^ ((x * 377 + y) * 377 + m_lod));
			m_itemPositionRng = new MyRandom(m_seed);
			float num = Vector3.Cross(m_basisX, m_basisY).Length() * 4f;
			m_itemCountTotal = (int)((double)num * m_environment.ItemDensity);
			m_scanHelper = new ProgressiveScanHelper(m_itemCountTotal, localLod * provider.LodFactor);
			Bounds = base.Owner.GetBoundingShape(ref WorldPos, ref m_basisX, ref m_basisY);
			m_items = new MyList<ItemInfo>();
			m_totalSpawned = 0;
			UpdateModuleBuilders(moduleData);
		}

		private void UpdateModuleBuilders(MyObjectBuilder_ProceduralEnvironmentSector moduleData)
		{
			m_moduleData.Clear();
			if (moduleData == null)
			{
				return;
			}
			for (int i = 0; i < moduleData.SavedModules.Length; i++)
			{
				MyObjectBuilder_ProceduralEnvironmentSector.Module module = moduleData.SavedModules[i];
				MyProceduralEnvironmentModuleDefinition definition = MyDefinitionManager.Static.GetDefinition<MyProceduralEnvironmentModuleDefinition>(module.ModuleId);
				if (definition != null)
				{
					m_moduleData.Add(definition.ModuleType, module.Builder);
					if (m_modules.TryGetValue(definition.ModuleType, out ModuleData value))
					{
						value.Module.Init(this, module.Builder);
					}
				}
			}
		}

		public override void EnableItem(int itemId, bool enabled)
		{
			short definitionIndex = m_items[itemId].DefinitionIndex;
			if (definitionIndex != -1)
			{
				GetItemDefinition((ushort)definitionIndex, out MyRuntimeEnvironmentItemInfo def);
				GetModuleForDefinition(def)?.OnItemEnable(itemId, enabled);
			}
		}

		public override void UpdateItemModel(int itemId, short modelId)
		{
			if (itemId < m_items.Count)
			{
				if (!m_scanning)
				{
					foreach (MyProceduralDataView viewer in m_viewers)
					{
						if (viewer.Listener != null)
						{
							int sectorIndex = viewer.GetSectorIndex(m_x, m_y);
							int num = viewer.SectorOffsets[sectorIndex];
							if (itemId < m_itemCountForLod[viewer.Lod])
							{
								viewer.Listener.OnItemChange(itemId + num, modelId);
							}
						}
					}
				}
				ItemInfo value = m_items[itemId];
				value.ModelIndex = modelId;
				m_items[itemId] = value;
			}
		}

		public override void UpdateItemModelBatch(List<int> itemIds, short newModelId)
		{
			int count = itemIds.Count;
			if (!m_scanning)
			{
				foreach (MyProceduralDataView viewer in m_viewers)
				{
					if (viewer.Listener != null)
					{
						int sectorIndex = viewer.GetSectorIndex(m_x, m_y);
						viewer.Listener.OnItemsChange(sectorIndex, itemIds, newModelId);
					}
				}
			}
			for (int i = 0; i < count; i++)
			{
				ItemInfo value = m_items[itemIds[i]];
				value.ModelIndex = newModelId;
				m_items[itemIds[i]] = value;
			}
		}

		private IMyEnvironmentModule GetModuleForDefinition(MyRuntimeEnvironmentItemInfo def)
		{
			if (def.Type.StorageModule.Type == null)
			{
				return null;
			}
			if (m_modules.TryGetValue(def.Type.StorageModule.Type, out ModuleData value))
			{
				return value.Module;
			}
			return null;
		}

		private Vector3 ComputeRandomItemPosition()
		{
			return m_basisX * m_itemPositionRng.NextFloat(-1f, 1f) + m_basisY * m_itemPositionRng.NextFloat(-1f, 1f);
		}

		private static Vector3 GetRandomPerpendicularVector(ref Vector3 axis, int seed)
		{
			Vector3 vector = Vector3.CalculatePerpendicularVector(axis);
			Vector3.Cross(ref axis, ref vector, out Vector3 result);
			double num = MyHashRandomUtils.UniformFloatFromSeed(seed) * 2f * 3.141593f;
			return (float)Math.Cos(num) * vector + (float)Math.Sin(num) * result;
		}

		private ModuleData GetModule(MyRuntimeEnvironmentItemInfo info)
		{
			Type type = info.Type.StorageModule.Type;
			if (type == null)
			{
				return null;
			}
			if (!m_modules.TryGetValue(type, out ModuleData value))
			{
				value = new ModuleData(type, info.Type.StorageModule.Definition);
				if (m_moduleData != null && m_moduleData.ContainsKey(type))
				{
					value.Module.Init(this, m_moduleData[type]);
				}
				else
				{
					value.Module.Init(this, null);
				}
				m_modules[type] = value;
			}
			return value;
		}

		private MyRuntimeEnvironmentItemInfo GetItemForPosition(ref MySurfaceParams surface, int lod)
		{
			MyBiomeMaterial key = new MyBiomeMaterial(surface.Biome, surface.Material);
			m_candidates.Clear();
			if (m_environment.MaterialEnvironmentMappings.TryGetValue(key, out List<MyEnvironmentItemMapping> value))
			{
				foreach (MyEnvironmentItemMapping item in value)
				{
					MyDiscreteSampler<MyRuntimeEnvironmentItemInfo> myDiscreteSampler = item.Sampler(lod);
					if (myDiscreteSampler != null && item.Rule.Check(surface.HeightRatio, surface.Latitude, surface.Longitude, surface.Normal.Z))
					{
						m_candidates.Add(myDiscreteSampler);
					}
				}
			}
			int hashCode = surface.Position.GetHashCode();
			float sample = MyHashRandomUtils.UniformFloatFromSeed(hashCode);
			switch (m_candidates.Count)
			{
			case 0:
				return null;
			case 1:
				return m_candidates[0].Sample(sample);
			default:
				return m_candidates[(int)(MyHashRandomUtils.UniformFloatFromSeed(~hashCode) * (float)m_candidates.Count)].Sample(sample);
			}
		}

		private void ScanItems(int targetLod)
		{
			int num = m_minimumScannedLod - targetLod;
			if (num < 1)
			{
				return;
			}
			int num2 = m_minimumScannedLod - 1;
			int num3 = 0;
			int[] array = new int[num];
			for (int num4 = num2; num4 >= targetLod; num4--)
			{
				int itemsForLod = m_scanHelper.GetItemsForLod(num4);
				array[num4 - targetLod] = num3 + m_totalSpawned;
				num3 += itemsForLod;
			}
			List<Vector3> list = new List<Vector3>(num3);
			for (int i = 0; i < num3; i++)
			{
				list.Add(ComputeRandomItemPosition());
			}
			BoundingBoxD queryBounds = BoundingBoxD.CreateFromPoints(Bounds);
			MyList<MySurfaceParams> myList = new MyList<MySurfaceParams>(num3);
			m_provider.Owner.QuerySurfaceParameters(WorldPos, ref queryBounds, list, myList);
			m_items.Capacity = m_items.Count + list.Count;
			int num5 = 0;
			for (int num6 = num2; num6 >= targetLod; num6--)
			{
				int num7 = num6 - targetLod;
				int num8 = (num6 > targetLod) ? (array[num7 - 1] - array[num7]) : (num3 - array[num7]);
				for (int j = 0; j < num8; j++)
				{
					MySurfaceParams surface = myList[num5];
					MyRuntimeEnvironmentItemInfo itemForPosition = GetItemForPosition(ref surface, num6);
					if (itemForPosition != null && num6 >= itemForPosition.Type.LodTo)
					{
						ModuleData module = GetModule(itemForPosition);
						if (module != null)
						{
							if (!module.ItemsPerDefinition.TryGetValue(itemForPosition.Index, out MyLodEnvironmentItemSet value))
							{
								value = (module.ItemsPerDefinition[itemForPosition.Index] = new MyLodEnvironmentItemSet
								{
									Items = new List<int>()
								});
							}
							value.Items.Add(m_totalSpawned);
						}
						Vector3 axis = -surface.Gravity;
						ItemInfo itemInfo = default(ItemInfo);
						itemInfo.Position = surface.Position;
						itemInfo.ModelIndex = -1;
						itemInfo.Rotation = Quaternion.CreateFromForwardUp(GetRandomPerpendicularVector(ref axis, surface.Position.GetHashCode()), axis);
						itemInfo.DefinitionIndex = itemForPosition.Index;
						ItemInfo item = itemInfo;
						m_items.Add(item);
						m_totalSpawned++;
					}
					num5++;
				}
				m_itemCountForLod[num6] = m_totalSpawned;
				foreach (ModuleData value3 in m_modules.Values)
				{
					if (value3 != null)
					{
						short[] array2 = value3.ItemsPerDefinition.Keys.ToArray();
						foreach (short key in array2)
						{
							MyLodEnvironmentItemSet value2 = value3.ItemsPerDefinition[key];
							value3.ItemsPerDefinition[key] = value2;
						}
					}
				}
			}
			m_scanning = true;
			foreach (ModuleData value4 in m_modules.Values)
			{
				value4?.Module.ProcessItems(value4.ItemsPerDefinition, num2, targetLod);
			}
			m_scanning = false;
			m_minimumScannedLod = targetLod;
		}

		public override void Init(MyObjectBuilder_EnvironmentSector sectorBuilder)
		{
			UpdateModuleBuilders((MyObjectBuilder_ProceduralEnvironmentSector)sectorBuilder);
		}

		public override MyObjectBuilder_EnvironmentSector GetObjectBuilder()
		{
			List<MyObjectBuilder_ProceduralEnvironmentSector.Module> list = new List<MyObjectBuilder_ProceduralEnvironmentSector.Module>(m_modules.Count);
			foreach (ModuleData value in m_modules.Values)
			{
				MyObjectBuilder_EnvironmentModuleBase objectBuilder = value.Module.GetObjectBuilder();
				if (objectBuilder != null)
				{
					list.Add(new MyObjectBuilder_ProceduralEnvironmentSector.Module
					{
						ModuleId = value.Definition,
						Builder = objectBuilder
					});
				}
			}
			if (list.Count > 0)
			{
				list.Capacity = list.Count;
				return new MyObjectBuilder_ProceduralEnvironmentSector
				{
					SavedModules = list.ToArray(),
					SectorId = Id
				};
			}
			return null;
		}

		public override void GetItemDefinition(ushort key, out MyRuntimeEnvironmentItemInfo it)
		{
			it = m_environment.Items[key];
		}

		public override void Close()
		{
			foreach (ModuleData value in m_modules.Values)
			{
				value.Module.Close();
			}
			m_modules.Clear();
			m_items.Clear();
			base.Close();
		}

		public override void DebugDraw(int lod)
		{
			Vector3D value = WorldPos + MySector.MainCamera.UpVector * 1f;
			for (int i = 0; i < m_items.Count; i++)
			{
				ItemInfo itemInfo = m_items[i];
				Vector3D worldCoord = itemInfo.Position + value;
				base.Owner.GetDefinition((ushort)itemInfo.DefinitionIndex, out MyRuntimeEnvironmentItemInfo def);
				MyRenderProxy.DebugDrawText3D(worldCoord, $"{def.Type.Name} i{i} m{itemInfo.ModelIndex} d{itemInfo.DefinitionIndex}", Color.Purple, 0.7f, depthRead: true);
			}
			foreach (ModuleData value2 in m_modules.Values)
			{
				value2.Module.DebugDraw();
			}
		}

		public override void DisableItemsInBox(Vector3D center, ref BoundingBoxD box)
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				Vector3D point = center + m_items[i].Position;
				box.Contains(ref point, out ContainmentType result);
				if (result == ContainmentType.Contains)
				{
					EnableItem(i, enabled: false);
				}
			}
		}

		public override void GetItemsInAabb(ref BoundingBoxD aabb, List<int> itemsInBox)
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].DefinitionIndex >= 0 && aabb.Contains(m_items[i].Position) != 0)
				{
					itemsInBox.Add(i);
				}
			}
		}

		public override void GetItem(int logicalItem, out ItemInfo item)
		{
			if (logicalItem >= m_items.Count || logicalItem < 0)
			{
				item = default(ItemInfo);
			}
			else
			{
				item = m_items[logicalItem];
			}
		}

		public override void IterateItems(ItemIterator action)
		{
			ItemInfo[] internalArray = m_items.GetInternalArray();
			for (int i = 0; i < m_items.Count; i++)
			{
				action(i, ref internalArray[i]);
			}
		}

		public override void InvalidateItem(int itemId)
		{
			ItemInfo value = m_items[itemId];
			value.DefinitionIndex = -1;
			m_items[itemId] = value;
		}

		public void RaiseItemEvent<TModule>(int logicalItem, object eventData, bool fromClient = false) where TModule : IMyEnvironmentModule
		{
			MyDefinitionId modDef = m_modules[typeof(TModule)].Definition;
			RaiseItemEvent(logicalItem, ref modDef, eventData, fromClient);
		}

		public override void RaiseItemEvent<T>(int logicalItem, ref MyDefinitionId modDef, T eventData, bool fromClient)
		{
			if (fromClient)
			{
				MyMultiplayer.RaiseEvent(this, (Func<MyProceduralLogicalSector, Action<int, SerializableDefinitionId, object>>)((MyProceduralLogicalSector x) => x.HandleItemEventClient), logicalItem, (SerializableDefinitionId)modDef, (object)eventData, default(EndpointId));
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (Func<MyProceduralLogicalSector, Action<int, SerializableDefinitionId, object>>)((MyProceduralLogicalSector x) => x.HandleItemEventServer), logicalItem, (SerializableDefinitionId)modDef, (object)eventData, default(EndpointId));
			}
		}

		[Broadcast]
		[Event(null, 707)]
		[Reliable]
		private void HandleItemEventServer(int logicalItem, SerializableDefinitionId def, [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyDynamicObjectResolver))] object data)
		{
			HandleItemEvent(logicalItem, def, data, fromClient: false);
		}

		[Event(null, 716)]
		[Reliable]
		[Server]
		private void HandleItemEventClient(int logicalItem, SerializableDefinitionId def, [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyDynamicObjectResolver))] object data)
		{
			HandleItemEvent(logicalItem, def, data, fromClient: true);
		}

		private void HandleItemEvent(int logicalItem, SerializableDefinitionId def, object data, bool fromClient)
		{
			if (typeof(MyObjectBuilder_ProceduralEnvironmentModuleDefinition).IsAssignableFrom(def.TypeId))
			{
				MyProceduralEnvironmentModuleDefinition definition = MyDefinitionManager.Static.GetDefinition<MyProceduralEnvironmentModuleDefinition>(def);
				ModuleData value;
				if (definition == null)
				{
					MyLog.Default.Error("Received message about unknown logical module {0}", def);
				}
				else if (m_modules.TryGetValue(definition.ModuleType, out value))
				{
					value.Module.HandleSyncEvent(logicalItem, data, fromClient);
				}
			}
			else
			{
				MyEnvironmentModuleProxyDefinition definition2 = MyDefinitionManager.Static.GetDefinition<MyEnvironmentModuleProxyDefinition>(def);
				if (definition2 == null)
				{
					MyLog.Default.Error("Received message about unknown module proxy {0}", def);
				}
				else
				{
					foreach (MyProceduralDataView viewer in m_viewers)
					{
						if (viewer.Listener != null)
						{
							IMyEnvironmentModuleProxy module = viewer.Listener.GetModule(definition2.ModuleType);
							int sectorIndex = viewer.GetSectorIndex(m_x, m_y);
							int num = viewer.SectorOffsets[sectorIndex];
							if (logicalItem < m_itemCountForLod[viewer.Lod])
							{
								module?.HandleSyncEvent(logicalItem + num, data, fromClient);
							}
						}
					}
				}
			}
		}

		private void UpdateMinLod()
		{
			base.MinLod = int.MaxValue;
			foreach (MyProceduralDataView viewer in m_viewers)
			{
				base.MinLod = Math.Min(viewer.Lod, base.MinLod);
			}
			bool flag = base.MinLod <= m_provider.SyncLod;
			if (flag != Replicable)
			{
				if (flag)
				{
					m_provider.MarkReplicable(this);
				}
				else
				{
					m_provider.UnmarkReplicable(this);
				}
				Replicable = flag;
			}
		}

		public override string ToString()
		{
			return $"x{m_x} y{m_y} l{m_lod} : {m_items.Count}";
		}

		public void AddView(MyProceduralDataView view, Vector3D localOrigin, int logicalLod)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				ScanItems(logicalLod);
				m_viewers.Add(view);
				UpdateMinLod();
				view.AddSector(this);
				int num = m_itemCountForLod[logicalLod];
				view.Items.Capacity = view.Items.Count + m_items.Count;
				Vector3 vector = WorldPos - localOrigin;
				for (int i = 0; i < num; i++)
				{
					ItemInfo item = m_items[i];
					item.Position += vector;
					view.Items.Add(item);
				}
			}
		}

		public void RemoveView(MyProceduralDataView view)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_viewers.Remove(view);
				UpdateMinLod();
				if (m_viewers.Count == 0 && this.OnViewerEmpty != null)
				{
					this.OnViewerEmpty(this);
				}
			}
		}
	}
}
