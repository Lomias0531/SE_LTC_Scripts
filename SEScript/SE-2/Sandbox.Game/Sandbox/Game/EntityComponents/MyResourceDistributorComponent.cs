using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
	public class MyResourceDistributorComponent : MyEntityComponentBase
	{
		private struct MyPhysicalDistributionGroup
		{
			public IMyConveyorEndpoint FirstEndpoint;

			public HashSet<MyResourceSinkComponent>[] SinksByPriority;

			public HashSet<MyResourceSourceComponent>[] SourcesByPriority;

			public List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> SinkSourcePairs;

			public MySinkGroupData[] SinkDataByPriority;

			public MySourceGroupData[] SourceDataByPriority;

			public MyTuple<MySinkGroupData, MySourceGroupData> InputOutputData;

			public MyList<int> StockpilingStorage;

			public MyList<int> OtherStorage;

			public float MaxAvailableResources;

			public MyResourceStateEnum ResourceState;

			public MyPhysicalDistributionGroup(MyDefinitionId typeId, IMyConveyorEndpointBlock block)
			{
				SinksByPriority = null;
				SourcesByPriority = null;
				SinkSourcePairs = null;
				FirstEndpoint = null;
				SinkDataByPriority = null;
				SourceDataByPriority = null;
				StockpilingStorage = null;
				OtherStorage = null;
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>);
				MaxAvailableResources = 0f;
				ResourceState = MyResourceStateEnum.NoPower;
				AllocateData();
				Init(typeId, block);
			}

			public MyPhysicalDistributionGroup(MyDefinitionId typeId, MyResourceSinkComponent tempConnectedSink)
			{
				SinksByPriority = null;
				SourcesByPriority = null;
				SinkSourcePairs = null;
				FirstEndpoint = null;
				SinkDataByPriority = null;
				SourceDataByPriority = null;
				StockpilingStorage = null;
				OtherStorage = null;
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>);
				MaxAvailableResources = 0f;
				ResourceState = MyResourceStateEnum.NoPower;
				AllocateData();
				InitFromTempConnected(typeId, tempConnectedSink);
			}

			public MyPhysicalDistributionGroup(MyDefinitionId typeId, MyResourceSourceComponent tempConnectedSource)
			{
				SinksByPriority = null;
				SourcesByPriority = null;
				SinkSourcePairs = null;
				FirstEndpoint = null;
				SinkDataByPriority = null;
				SourceDataByPriority = null;
				StockpilingStorage = null;
				OtherStorage = null;
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>);
				MaxAvailableResources = 0f;
				ResourceState = MyResourceStateEnum.NoPower;
				AllocateData();
				InitFromTempConnected(typeId, tempConnectedSource);
			}

			public void Init(MyDefinitionId typeId, IMyConveyorEndpointBlock block)
			{
				FirstEndpoint = block.ConveyorEndpoint;
				ClearData();
				Add(typeId, block);
			}

			public void InitFromTempConnected(MyDefinitionId typeId, MyResourceSinkComponent tempConnectedSink)
			{
				IMyConveyorEndpointBlock myConveyorEndpointBlock = tempConnectedSink.TemporaryConnectedEntity as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock != null)
				{
					FirstEndpoint = myConveyorEndpointBlock.ConveyorEndpoint;
				}
				ClearData();
				AddTempConnected(typeId, tempConnectedSink);
			}

			public void InitFromTempConnected(MyDefinitionId typeId, MyResourceSourceComponent tempConnectedSource)
			{
				IMyConveyorEndpointBlock myConveyorEndpointBlock = tempConnectedSource.TemporaryConnectedEntity as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock != null)
				{
					FirstEndpoint = myConveyorEndpointBlock.ConveyorEndpoint;
				}
				ClearData();
				AddTempConnected(typeId, tempConnectedSource);
			}

			public void Add(MyDefinitionId typeId, IMyConveyorEndpointBlock endpoint)
			{
				if (FirstEndpoint == null)
				{
					FirstEndpoint = endpoint.ConveyorEndpoint;
				}
				MyEntityComponentContainer components = (endpoint as IMyEntity).Components;
				MyResourceSinkComponent myResourceSinkComponent = components.Get<MyResourceSinkComponent>();
				MyResourceSourceComponent myResourceSourceComponent = components.Get<MyResourceSourceComponent>();
				bool flag = myResourceSinkComponent != null && myResourceSinkComponent.AcceptedResources.Contains(typeId);
				bool flag2 = myResourceSourceComponent != null && myResourceSourceComponent.ResourceTypes.Contains(typeId);
				if (flag && flag2)
				{
					SinkSourcePairs.Add(new MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>(myResourceSinkComponent, myResourceSourceComponent));
				}
				else if (flag)
				{
					SinksByPriority[GetPriority(myResourceSinkComponent)].Add(myResourceSinkComponent);
				}
				else if (flag2)
				{
					SourcesByPriority[GetPriority(myResourceSourceComponent)].Add(myResourceSourceComponent);
				}
			}

			public void AddTempConnected(MyDefinitionId typeId, MyResourceSinkComponent tempConnectedSink)
			{
				if (tempConnectedSink != null && tempConnectedSink.AcceptedResources.Contains(typeId))
				{
					SinksByPriority[GetPriority(tempConnectedSink)].Add(tempConnectedSink);
				}
			}

			public void AddTempConnected(MyDefinitionId typeId, MyResourceSourceComponent tempConnectedSource)
			{
				if (tempConnectedSource != null && tempConnectedSource.ResourceTypes.Contains(typeId))
				{
					SourcesByPriority[GetPriority(tempConnectedSource)].Add(tempConnectedSource);
				}
			}

			private void AllocateData()
			{
				FirstEndpoint = null;
				SinksByPriority = new HashSet<MyResourceSinkComponent>[m_sinkGroupPrioritiesTotal];
				SourcesByPriority = new HashSet<MyResourceSourceComponent>[m_sourceGroupPrioritiesTotal];
				SinkSourcePairs = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
				SinkDataByPriority = new MySinkGroupData[m_sinkGroupPrioritiesTotal];
				SourceDataByPriority = new MySourceGroupData[m_sourceGroupPrioritiesTotal];
				StockpilingStorage = new MyList<int>();
				OtherStorage = new MyList<int>();
				for (int i = 0; i < m_sinkGroupPrioritiesTotal; i++)
				{
					SinksByPriority[i] = new HashSet<MyResourceSinkComponent>();
				}
				for (int j = 0; j < m_sourceGroupPrioritiesTotal; j++)
				{
					SourcesByPriority[j] = new HashSet<MyResourceSourceComponent>();
				}
			}

			private void ClearData()
			{
				HashSet<MyResourceSinkComponent>[] sinksByPriority = SinksByPriority;
				for (int i = 0; i < sinksByPriority.Length; i++)
				{
					sinksByPriority[i].Clear();
				}
				HashSet<MyResourceSourceComponent>[] sourcesByPriority = SourcesByPriority;
				for (int i = 0; i < sourcesByPriority.Length; i++)
				{
					sourcesByPriority[i].Clear();
				}
				SinkSourcePairs.Clear();
				StockpilingStorage.ClearFast();
				OtherStorage.ClearFast();
			}
		}

		private struct MySinkGroupData
		{
			public bool IsAdaptible;

			public float RequiredInput;

			public float RequiredInputCumulative;

			public float RemainingAvailableResource;

			public override string ToString()
			{
				return $"IsAdaptible: {IsAdaptible}, RequiredInput: {RequiredInput}, RemainingAvailableResource: {RemainingAvailableResource}";
			}
		}

		private struct MySourceGroupData
		{
			public float MaxAvailableResource;

			public float UsageRatio;

			public bool InfiniteCapacity;

			public int ActiveCount;

			public override string ToString()
			{
				return $"MaxAvailableResource: {MaxAvailableResource}, UsageRatio: {UsageRatio}";
			}
		}

		private class PerTypeData
		{
			private bool m_needsRecompute;

			public MyDefinitionId TypeId;

			public MySinkGroupData[] SinkDataByPriority;

			public MySourceGroupData[] SourceDataByPriority;

			public MyTuple<MySinkGroupData, MySourceGroupData> InputOutputData;

			public HashSet<MyResourceSinkComponent>[] SinksByPriority;

			public HashSet<MyResourceSourceComponent>[] SourcesByPriority;

			public List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> InputOutputList;

			public MyList<int> StockpilingStorageIndices;

			public MyList<int> OtherStorageIndices;

			public List<MyPhysicalDistributionGroup> DistributionGroups;

			public int DistributionGroupsInUse;

			public bool GroupsDirty;

			public int SourceCount;

			public float RemainingFuelTime;

			public bool RemainingFuelTimeDirty;

			public float MaxAvailableResource;

			public MyMultipleEnabledEnum SourcesEnabled;

			public bool SourcesEnabledDirty;

			public MyResourceStateEnum ResourceState;

			private bool m_gridsForUpdateValid;

			private HashSet<MyCubeGrid> m_gridsForUpdate = new HashSet<MyCubeGrid>();

			private bool m_gridUpdateScheduled;

			private Action m_UpdateGridsCallback;

			public bool NeedsRecompute
			{
				get
				{
					return m_needsRecompute;
				}
				set
				{
					if (m_needsRecompute != value)
					{
						m_needsRecompute = value;
						if (m_needsRecompute)
						{
							ScheduleGridUpdate();
						}
					}
				}
			}

			public PerTypeData()
			{
				m_UpdateGridsCallback = UpdateGrids;
			}

			public void InvalidateGridForUpdateCache()
			{
				m_gridsForUpdate.Clear();
				m_gridsForUpdateValid = false;
			}

			private void ScheduleGridUpdate()
			{
				if (!m_gridUpdateScheduled)
				{
					m_gridUpdateScheduled = true;
					MySandboxGame.Static.Invoke(m_UpdateGridsCallback, "UpdateResourcesOnGrids");
				}
			}

			private void UpdateGrids()
			{
				m_gridUpdateScheduled = false;
				if (!m_gridsForUpdateValid)
				{
					m_gridsForUpdateValid = true;
					HashSet<MyResourceSourceComponent>[] sourcesByPriority = SourcesByPriority;
					for (int i = 0; i < sourcesByPriority.Length; i++)
					{
						foreach (MyResourceSourceComponent item in sourcesByPriority[i])
						{
							AddGridForUpdate(item);
						}
					}
					HashSet<MyResourceSinkComponent>[] sinksByPriority = SinksByPriority;
					for (int i = 0; i < sinksByPriority.Length; i++)
					{
						foreach (MyResourceSinkComponent item2 in sinksByPriority[i])
						{
							AddGridForUpdate(item2);
						}
					}
				}
				foreach (MyCubeGrid item3 in m_gridsForUpdate)
				{
					item3.MarkForUpdate();
				}
			}

			private void AddGridForUpdate(MyEntityComponentBase component)
			{
				IMyEntity entity = component.Entity;
				MyCubeBlock myCubeBlock = entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					m_gridsForUpdate.Add(myCubeBlock.CubeGrid);
					return;
				}
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (myCubeGrid != null)
				{
					m_gridsForUpdate.Add(myCubeGrid);
				}
			}
		}

		private class Sandbox_Game_EntityComponents_MyResourceDistributorComponent_003C_003EActor
		{
		}

		public static readonly MyDefinitionId ElectricityId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Electricity");

		public static readonly MyDefinitionId HydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");

		public static readonly MyDefinitionId OxygenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

		private int m_typeGroupCount;

		private bool m_forceRecalculation;

		private readonly List<PerTypeData> m_dataPerType = new List<PerTypeData>();

		private readonly HashSet<MyDefinitionId> m_initializedTypes = new HashSet<MyDefinitionId>(MyDefinitionId.Comparer);

		private readonly Dictionary<MyDefinitionId, int> m_typeIdToIndex = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		private readonly Dictionary<MyDefinitionId, bool> m_typeIdToConveyorConnectionRequired = new Dictionary<MyDefinitionId, bool>(MyDefinitionId.Comparer);

		private readonly HashSet<MyDefinitionId> m_typesToRemove = new HashSet<MyDefinitionId>(MyDefinitionId.Comparer);

		private readonly MyConcurrentHashSet<MyResourceSinkComponent> m_sinksToAdd = new MyConcurrentHashSet<MyResourceSinkComponent>();

		private readonly MyConcurrentHashSet<MyTuple<MyResourceSinkComponent, bool>> m_sinksToRemove = new MyConcurrentHashSet<MyTuple<MyResourceSinkComponent, bool>>();

		private readonly MyConcurrentHashSet<MyResourceSourceComponent> m_sourcesToAdd = new MyConcurrentHashSet<MyResourceSourceComponent>();

		private readonly MyConcurrentHashSet<MyResourceSourceComponent> m_sourcesToRemove = new MyConcurrentHashSet<MyResourceSourceComponent>();

		private readonly MyConcurrentDictionary<MyDefinitionId, int> m_changedTypes = new MyConcurrentDictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		private readonly List<string> m_changesDebug = new List<string>();

		public static bool ShowTrace = false;

		public string DebugName;

		private static int m_typeGroupCountTotal = -1;

		private static int m_sinkGroupPrioritiesTotal = -1;

		private static int m_sourceGroupPrioritiesTotal = -1;

		private static readonly Dictionary<MyDefinitionId, int> m_typeIdToIndexTotal = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		private static readonly Dictionary<MyDefinitionId, bool> m_typeIdToConveyorConnectionRequiredTotal = new Dictionary<MyDefinitionId, bool>(MyDefinitionId.Comparer);

		private static readonly Dictionary<MyStringHash, int> m_sourceSubtypeToPriority = new Dictionary<MyStringHash, int>(MyStringHash.Comparer);

		private static readonly Dictionary<MyStringHash, int> m_sinkSubtypeToPriority = new Dictionary<MyStringHash, int>(MyStringHash.Comparer);

		private static readonly Dictionary<MyStringHash, bool> m_sinkSubtypeToAdaptability = new Dictionary<MyStringHash, bool>(MyStringHash.Comparer);

		public Action<bool> OnPowerGenerationChanged;

		private MyResourceStateEnum m_electricityState;

		private bool m_updateInProgress;

		private bool m_recomputeInProgress;

		public MyMultipleEnabledEnum SourcesEnabled => SourcesEnabledByType(m_typeIdToIndexTotal.Keys.First());

		public MyResourceStateEnum ResourceState => ResourceStateByType(m_typeIdToIndexTotal.Keys.First());

		public static int SinkGroupPrioritiesTotal => m_sinkGroupPrioritiesTotal;

		public static DictionaryReader<MyStringHash, int> SinkSubtypesToPriority => new DictionaryReader<MyStringHash, int>(m_sinkSubtypeToPriority);

		public bool NeedsPerFrameUpdate
		{
			get
			{
				bool flag = m_sinksToRemove.Count > 0 || m_sourcesToRemove.Count > 0 || m_sourcesToAdd.Count > 0 || m_sinksToAdd.Count > 0 || m_typesToRemove.Count > 0 || ShowTrace;
				if (!flag)
				{
					foreach (KeyValuePair<MyDefinitionId, int> item in m_typeIdToIndex)
					{
						MyDefinitionId typeId = item.Key;
						flag |= NeedsRecompute(ref typeId, item.Value);
						if (flag)
						{
							return flag;
						}
					}
					return flag;
				}
				return flag;
			}
		}

		public override string ComponentTypeDebugString => "Resource Distributor";

		internal static void InitializeMappings()
		{
			lock (m_typeIdToIndexTotal)
			{
				if (m_sinkGroupPrioritiesTotal < 0 && m_sourceGroupPrioritiesTotal < 0)
				{
					ListReader<MyResourceDistributionGroupDefinition> definitionsOfType = MyDefinitionManager.Static.GetDefinitionsOfType<MyResourceDistributionGroupDefinition>();
					IOrderedEnumerable<MyResourceDistributionGroupDefinition> orderedEnumerable = definitionsOfType.OrderBy((MyResourceDistributionGroupDefinition def) => def.Priority);
					if (definitionsOfType.Count > 0)
					{
						m_sinkGroupPrioritiesTotal = 0;
						m_sourceGroupPrioritiesTotal = 0;
					}
					foreach (MyResourceDistributionGroupDefinition item in orderedEnumerable)
					{
						if (!item.IsSource)
						{
							m_sinkSubtypeToPriority.Add(item.Id.SubtypeId, m_sinkGroupPrioritiesTotal++);
							m_sinkSubtypeToAdaptability.Add(item.Id.SubtypeId, item.IsAdaptible);
						}
						else
						{
							m_sourceSubtypeToPriority.Add(item.Id.SubtypeId, m_sourceGroupPrioritiesTotal++);
						}
					}
					m_sinkGroupPrioritiesTotal = Math.Max(m_sinkGroupPrioritiesTotal, 1);
					m_sourceGroupPrioritiesTotal = Math.Max(m_sourceGroupPrioritiesTotal, 1);
					m_sinkSubtypeToPriority.Add(MyStringHash.NullOrEmpty, m_sinkGroupPrioritiesTotal - 1);
					m_sinkSubtypeToAdaptability.Add(MyStringHash.NullOrEmpty, value: false);
					m_sourceSubtypeToPriority.Add(MyStringHash.NullOrEmpty, m_sourceGroupPrioritiesTotal - 1);
					m_typeGroupCountTotal = 0;
					m_typeIdToIndexTotal.Add(ElectricityId, m_typeGroupCountTotal++);
					m_typeIdToConveyorConnectionRequiredTotal.Add(ElectricityId, value: false);
					foreach (MyGasProperties item2 in MyDefinitionManager.Static.GetDefinitionsOfType<MyGasProperties>())
					{
						m_typeIdToIndexTotal.Add(item2.Id, m_typeGroupCountTotal++);
						m_typeIdToConveyorConnectionRequiredTotal.Add(item2.Id, value: true);
					}
				}
			}
		}

		private void InitializeNewType(ref MyDefinitionId typeId)
		{
			m_typeIdToIndex.Add(typeId, m_typeGroupCount++);
			m_typeIdToConveyorConnectionRequired.Add(typeId, IsConveyorConnectionRequiredTotal(ref typeId));
			HashSet<MyResourceSinkComponent>[] array = new HashSet<MyResourceSinkComponent>[m_sinkGroupPrioritiesTotal];
			HashSet<MyResourceSourceComponent>[] array2 = new HashSet<MyResourceSourceComponent>[m_sourceGroupPrioritiesTotal];
			List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> inputOutputList = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new HashSet<MyResourceSinkComponent>();
			}
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = new HashSet<MyResourceSourceComponent>();
			}
			List<MyPhysicalDistributionGroup> distributionGroups = null;
			int distributionGroupsInUse = 0;
			MySinkGroupData[] sinkDataByPriority = null;
			MySourceGroupData[] sourceDataByPriority = null;
			MyList<int> stockpilingStorageIndices = null;
			MyList<int> otherStorageIndices = null;
			if (IsConveyorConnectionRequired(ref typeId))
			{
				distributionGroups = new List<MyPhysicalDistributionGroup>();
			}
			else
			{
				sinkDataByPriority = new MySinkGroupData[m_sinkGroupPrioritiesTotal];
				sourceDataByPriority = new MySourceGroupData[m_sourceGroupPrioritiesTotal];
				stockpilingStorageIndices = new MyList<int>();
				otherStorageIndices = new MyList<int>();
			}
			m_dataPerType.Add(new PerTypeData
			{
				TypeId = typeId,
				SinkDataByPriority = sinkDataByPriority,
				SourceDataByPriority = sourceDataByPriority,
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>),
				SinksByPriority = array,
				SourcesByPriority = array2,
				InputOutputList = inputOutputList,
				StockpilingStorageIndices = stockpilingStorageIndices,
				OtherStorageIndices = otherStorageIndices,
				DistributionGroups = distributionGroups,
				DistributionGroupsInUse = distributionGroupsInUse,
				NeedsRecompute = false,
				GroupsDirty = true,
				SourceCount = 0,
				RemainingFuelTime = 0f,
				RemainingFuelTimeDirty = true,
				MaxAvailableResource = 0f,
				SourcesEnabled = MyMultipleEnabledEnum.NoObjects,
				SourcesEnabledDirty = true,
				ResourceState = MyResourceStateEnum.NoPower
			});
			m_initializedTypes.Add(typeId);
		}

		public MyResourceDistributorComponent(string debugName)
		{
			InitializeMappings();
			DebugName = debugName;
			m_changesDebug.Clear();
		}

		public void MarkForUpdate()
		{
			m_forceRecalculation = true;
		}

		private void RemoveTypesFromChanges(ListReader<MyDefinitionId> types)
		{
			foreach (MyDefinitionId item in types)
			{
				if (m_changedTypes.TryGetValue(item, out int value))
				{
					m_changedTypes[item] = Math.Max(0, value - 1);
				}
			}
		}

		public void AddSink(MyResourceSinkComponent sink)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				m_changesDebug.Add($"+Sink: {((sink.Entity != null) ? sink.Entity.ToString() : sink.Group.ToString())}");
			}
			MyTuple<MyResourceSinkComponent, bool> value = default(MyTuple<MyResourceSinkComponent, bool>);
			lock (m_sinksToRemove)
			{
				foreach (MyTuple<MyResourceSinkComponent, bool> item in m_sinksToRemove)
				{
					if (item.Item1 == sink)
					{
						value = item;
						break;
					}
				}
				if (value.Item1 != null)
				{
					m_sinksToRemove.Remove(value);
					RemoveTypesFromChanges(value.Item1.AcceptedResources);
					return;
				}
			}
			lock (m_sinksToAdd)
			{
				m_sinksToAdd.Add(sink);
			}
			foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
			{
				if (!m_changedTypes.TryGetValue(acceptedResource, out int value2))
				{
					m_changedTypes.Add(acceptedResource, 1);
				}
				else
				{
					m_changedTypes[acceptedResource] = value2 + 1;
				}
			}
		}

		public void RemoveSink(MyResourceSinkComponent sink, bool resetSinkInput = true, bool markedForClose = false)
		{
			if (!markedForClose)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
				{
					m_changesDebug.Add($"-Sink: {((sink.Entity != null) ? sink.Entity.ToString() : sink.Group.ToString())}");
				}
				lock (m_sinksToAdd)
				{
					if (m_sinksToAdd.Contains(sink))
					{
						m_sinksToAdd.Remove(sink);
						RemoveTypesFromChanges(sink.AcceptedResources);
						return;
					}
				}
				lock (m_sinksToRemove)
				{
					m_sinksToRemove.Add(MyTuple.Create(sink, resetSinkInput));
				}
				foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
				{
					if (!m_changedTypes.TryGetValue(acceptedResource, out int value))
					{
						m_changedTypes.Add(acceptedResource, 1);
					}
					else
					{
						m_changedTypes[acceptedResource] = value + 1;
					}
				}
			}
		}

		public void AddSource(MyResourceSourceComponent source)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				m_changesDebug.Add($"+Source: {((source.Entity != null) ? source.Entity.ToString() : source.Group.ToString())}");
			}
			lock (m_sourcesToRemove)
			{
				if (m_sourcesToRemove.Contains(source))
				{
					m_sourcesToRemove.Remove(source);
					RemoveTypesFromChanges(source.ResourceTypes);
					return;
				}
			}
			lock (m_sourcesToAdd)
			{
				m_sourcesToAdd.Add(source);
			}
			foreach (MyDefinitionId resourceType in source.ResourceTypes)
			{
				if (!m_changedTypes.TryGetValue(resourceType, out int value))
				{
					m_changedTypes.Add(resourceType, 1);
				}
				else
				{
					m_changedTypes[resourceType] = value + 1;
				}
			}
		}

		public void RemoveSource(MyResourceSourceComponent source)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				m_changesDebug.Add($"-Source: {((source.Entity != null) ? source.Entity.ToString() : source.Group.ToString())}");
			}
			lock (m_sourcesToAdd)
			{
				if (m_sourcesToAdd.Contains(source))
				{
					m_sourcesToAdd.Remove(source);
					RemoveTypesFromChanges(source.ResourceTypes);
					return;
				}
			}
			lock (m_sourcesToRemove)
			{
				m_sourcesToRemove.Add(source);
			}
			foreach (MyDefinitionId resourceType in source.ResourceTypes)
			{
				if (!m_changedTypes.TryGetValue(resourceType, out int value))
				{
					m_changedTypes.Add(resourceType, 1);
				}
				else
				{
					m_changedTypes[resourceType] = value + 1;
				}
			}
		}

		private void AddSinkLazy(MyResourceSinkComponent sink)
		{
			foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
			{
				MyDefinitionId typeId = acceptedResource;
				if (!m_initializedTypes.Contains(typeId))
				{
					InitializeNewType(ref typeId);
				}
				HashSet<MyResourceSinkComponent> sinksOfType = GetSinksOfType(ref typeId, sink.Group);
				if (sinksOfType != null)
				{
					int typeIndex = GetTypeIndex(ref typeId);
					PerTypeData perTypeData = m_dataPerType[typeIndex];
					MyResourceSourceComponent myResourceSourceComponent = null;
					if (sink.Container != null)
					{
						HashSet<MyResourceSourceComponent>[] sourcesByPriority = perTypeData.SourcesByPriority;
						foreach (HashSet<MyResourceSourceComponent> hashSet in sourcesByPriority)
						{
							foreach (MyResourceSourceComponent item in hashSet)
							{
								if (item.Container != null && item.Container.Get<MyResourceSinkComponent>() == sink)
								{
									perTypeData.InputOutputList.Add(MyTuple.Create(sink, item));
									myResourceSourceComponent = item;
									break;
								}
							}
							if (myResourceSourceComponent != null)
							{
								hashSet.Remove(myResourceSourceComponent);
								perTypeData.InvalidateGridForUpdateCache();
								break;
							}
						}
					}
					if (myResourceSourceComponent == null)
					{
						sinksOfType.Add(sink);
						perTypeData.InvalidateGridForUpdateCache();
					}
					perTypeData.NeedsRecompute = true;
					perTypeData.GroupsDirty = true;
					perTypeData.RemainingFuelTimeDirty = true;
				}
			}
			sink.RequiredInputChanged += Sink_RequiredInputChanged;
			sink.ResourceAvailable += Sink_IsResourceAvailable;
			sink.OnAddType += Sink_OnAddType;
			sink.OnRemoveType += Sink_OnRemoveType;
		}

		private void RemoveSinkLazy(MyResourceSinkComponent sink, bool resetSinkInput = true)
		{
			foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
			{
				MyDefinitionId typeId = acceptedResource;
				HashSet<MyResourceSinkComponent> sinksOfType = GetSinksOfType(ref typeId, sink.Group);
				if (sinksOfType != null)
				{
					int typeIndex = GetTypeIndex(ref typeId);
					PerTypeData perTypeData = m_dataPerType[typeIndex];
					if (!sinksOfType.Remove(sink))
					{
						int num = -1;
						for (int i = 0; i < perTypeData.InputOutputList.Count; i++)
						{
							if (perTypeData.InputOutputList[i].Item1 == sink)
							{
								num = i;
								break;
							}
						}
						if (num != -1)
						{
							MyResourceSourceComponent item = perTypeData.InputOutputList[num].Item2;
							perTypeData.InputOutputList.RemoveAtFast(num);
							perTypeData.SourcesByPriority[GetPriority(item)].Add(item);
							perTypeData.InvalidateGridForUpdateCache();
						}
					}
					else
					{
						perTypeData.InvalidateGridForUpdateCache();
					}
					perTypeData.NeedsRecompute = true;
					perTypeData.GroupsDirty = true;
					perTypeData.RemainingFuelTimeDirty = true;
					if (resetSinkInput)
					{
						sink.SetInputFromDistributor(typeId, 0f, IsAdaptible(sink));
					}
				}
			}
			sink.OnRemoveType -= Sink_OnRemoveType;
			sink.OnAddType -= Sink_OnAddType;
			sink.RequiredInputChanged -= Sink_RequiredInputChanged;
			sink.ResourceAvailable -= Sink_IsResourceAvailable;
		}

		private void AddSourceLazy(MyResourceSourceComponent source)
		{
			foreach (MyDefinitionId resourceType in source.ResourceTypes)
			{
				MyDefinitionId typeId = resourceType;
				if (!m_initializedTypes.Contains(typeId))
				{
					InitializeNewType(ref typeId);
				}
				HashSet<MyResourceSourceComponent> sourcesOfType = GetSourcesOfType(ref typeId, source.Group);
				if (sourcesOfType != null)
				{
					int typeIndex = GetTypeIndex(ref typeId);
					PerTypeData perTypeData = m_dataPerType[typeIndex];
					MyResourceSinkComponent myResourceSinkComponent = null;
					if (source.Container != null)
					{
						HashSet<MyResourceSinkComponent>[] sinksByPriority = perTypeData.SinksByPriority;
						foreach (HashSet<MyResourceSinkComponent> hashSet in sinksByPriority)
						{
							foreach (MyResourceSinkComponent item in hashSet)
							{
								if (item.Container != null && item.Container.Get<MyResourceSourceComponent>() == source)
								{
									perTypeData.InputOutputList.Add(MyTuple.Create(item, source));
									myResourceSinkComponent = item;
									break;
								}
							}
							if (myResourceSinkComponent != null)
							{
								hashSet.Remove(myResourceSinkComponent);
								perTypeData.InvalidateGridForUpdateCache();
								break;
							}
						}
					}
					if (myResourceSinkComponent == null)
					{
						sourcesOfType.Add(source);
						perTypeData.InvalidateGridForUpdateCache();
					}
					perTypeData.NeedsRecompute = true;
					perTypeData.GroupsDirty = true;
					perTypeData.SourceCount++;
					if (perTypeData.SourceCount == 1)
					{
						perTypeData.SourcesEnabled = ((!source.Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
					}
					else if ((perTypeData.SourcesEnabled == MyMultipleEnabledEnum.AllEnabled && !source.Enabled) || (perTypeData.SourcesEnabled == MyMultipleEnabledEnum.AllDisabled && source.Enabled))
					{
						perTypeData.SourcesEnabled = MyMultipleEnabledEnum.Mixed;
					}
					perTypeData.RemainingFuelTimeDirty = true;
				}
			}
			source.HasCapacityRemainingChanged += source_HasRemainingCapacityChanged;
			source.MaxOutputChanged += source_MaxOutputChanged;
			source.ProductionEnabledChanged += source_ProductionEnabledChanged;
		}

		private void RemoveSourceLazy(MyResourceSourceComponent source)
		{
			foreach (MyDefinitionId resourceType in source.ResourceTypes)
			{
				MyDefinitionId typeId = resourceType;
				HashSet<MyResourceSourceComponent> sourcesOfType = GetSourcesOfType(ref typeId, source.Group);
				if (sourcesOfType != null)
				{
					int typeIndex = GetTypeIndex(ref typeId);
					PerTypeData perTypeData = m_dataPerType[typeIndex];
					if (!sourcesOfType.Remove(source))
					{
						int num = -1;
						for (int i = 0; i < perTypeData.InputOutputList.Count; i++)
						{
							if (perTypeData.InputOutputList[i].Item2 == source)
							{
								num = i;
								break;
							}
						}
						if (num != -1)
						{
							MyResourceSinkComponent item = perTypeData.InputOutputList[num].Item1;
							perTypeData.InputOutputList.RemoveAtFast(num);
							perTypeData.SinksByPriority[GetPriority(item)].Add(item);
							perTypeData.InvalidateGridForUpdateCache();
						}
					}
					else
					{
						perTypeData.InvalidateGridForUpdateCache();
					}
					perTypeData.NeedsRecompute = true;
					perTypeData.GroupsDirty = true;
					perTypeData.SourceCount--;
					if (perTypeData.SourceCount == 0)
					{
						perTypeData.SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
					}
					else if (perTypeData.SourceCount == 1)
					{
						MyResourceSourceComponent firstSourceOfType = GetFirstSourceOfType(ref typeId);
						if (firstSourceOfType != null)
						{
							ChangeSourcesState(typeId, (!firstSourceOfType.Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled, MySession.Static.LocalPlayerId);
						}
						else
						{
							perTypeData.SourceCount--;
							perTypeData.SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
						}
					}
					else if (perTypeData.SourcesEnabled == MyMultipleEnabledEnum.Mixed)
					{
						perTypeData.SourcesEnabledDirty = true;
					}
					perTypeData.RemainingFuelTimeDirty = true;
				}
			}
			source.ProductionEnabledChanged -= source_ProductionEnabledChanged;
			source.MaxOutputChanged -= source_MaxOutputChanged;
			source.HasCapacityRemainingChanged -= source_HasRemainingCapacityChanged;
		}

		public int GetSourceCount(MyDefinitionId resourceTypeId, MyStringHash sourceGroupType)
		{
			int num = 0;
			if (!TryGetTypeIndex(ref resourceTypeId, out int typeIndex))
			{
				return 0;
			}
			int num2 = m_sourceSubtypeToPriority[sourceGroupType];
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
			{
				if (inputOutput.Item2.Group == sourceGroupType && inputOutput.Item2.CurrentOutputByType(resourceTypeId) > 0f)
				{
					num++;
				}
			}
			return m_dataPerType[typeIndex].SourceDataByPriority[num2].ActiveCount + num;
		}

		public void UpdateBeforeSimulation()
		{
			CheckDistributionSystemChanges();
			foreach (MyDefinitionId key in m_typeIdToIndex.Keys)
			{
				MyDefinitionId typeId = key;
				if (m_forceRecalculation || NeedsRecompute(ref typeId))
				{
					RecomputeResourceDistribution(ref typeId, updateChanges: false);
				}
			}
			m_forceRecalculation = false;
			foreach (MyDefinitionId item in m_typesToRemove)
			{
				MyDefinitionId typeId2 = item;
				RemoveType(ref typeId2);
			}
			_ = ShowTrace;
		}

		public void UpdateBeforeSimulation100()
		{
			MyResourceStateEnum myResourceStateEnum = ResourceStateByType(ElectricityId);
			if (m_electricityState != myResourceStateEnum)
			{
				if (PowerStateIsOk(myResourceStateEnum) != PowerStateIsOk(m_electricityState))
				{
					ConveyorSystem_OnPoweredChanged();
				}
				bool flag = PowerStateWorks(myResourceStateEnum);
				if (OnPowerGenerationChanged != null && flag != PowerStateWorks(m_electricityState))
				{
					OnPowerGenerationChanged(flag);
				}
				ConveyorSystem_OnPoweredChanged();
				m_electricityState = myResourceStateEnum;
			}
		}

		private bool PowerStateIsOk(MyResourceStateEnum state)
		{
			return state == MyResourceStateEnum.Ok;
		}

		private bool PowerStateWorks(MyResourceStateEnum state)
		{
			return state != MyResourceStateEnum.NoPower;
		}

		public void UpdateHud(MyHudSinkGroupInfo info)
		{
			bool flag = true;
			int num = 0;
			int i = 0;
			if (!TryGetTypeIndex(ElectricityId, out int typeIndex))
			{
				return;
			}
			for (; i < m_dataPerType[typeIndex].SinkDataByPriority.Length; i++)
			{
				if (flag && m_dataPerType[typeIndex].SinkDataByPriority[i].RemainingAvailableResource < m_dataPerType[typeIndex].SinkDataByPriority[i].RequiredInput && !m_dataPerType[typeIndex].SinkDataByPriority[i].IsAdaptible)
				{
					flag = false;
				}
				if (flag)
				{
					num++;
				}
				info.SetGroupDeficit(i, Math.Max(m_dataPerType[typeIndex].SinkDataByPriority[i].RequiredInput - m_dataPerType[typeIndex].SinkDataByPriority[i].RemainingAvailableResource, 0f));
			}
			info.WorkingGroupCount = num;
		}

		public void ChangeSourcesState(MyDefinitionId resourceTypeId, MyMultipleEnabledEnum state, long playerId)
		{
			if (!m_recomputeInProgress && TryGetTypeIndex(resourceTypeId, out int typeIndex) && m_dataPerType[typeIndex].SourcesEnabled != state && m_dataPerType[typeIndex].SourcesEnabled != 0)
			{
				m_recomputeInProgress = true;
				m_dataPerType[typeIndex].SourcesEnabled = state;
				bool newValue = state == MyMultipleEnabledEnum.AllEnabled;
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId);
				bool flag = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals);
				if (MySession.Static.RemoteAdminSettings.TryGetValue(MySession.Static.Players.TryGetSteamId(playerId), out AdminSettingsEnum value))
				{
					flag |= value.HasFlag(AdminSettingsEnum.UseTerminals);
				}
				HashSet<MyResourceSourceComponent>[] sourcesByPriority = m_dataPerType[typeIndex].SourcesByPriority;
				for (int i = 0; i < sourcesByPriority.Length; i++)
				{
					foreach (MyResourceSourceComponent item2 in sourcesByPriority[i])
					{
						if (!flag && playerId >= 0 && item2.Entity != null)
						{
							MyFunctionalBlock myFunctionalBlock = item2.Entity as MyFunctionalBlock;
							if (myFunctionalBlock != null && myFunctionalBlock.OwnerId != 0L && myFunctionalBlock.OwnerId != playerId)
							{
								MyOwnershipShareModeEnum shareMode = myFunctionalBlock.IDModule.ShareMode;
								IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(myFunctionalBlock.OwnerId);
								if (shareMode == MyOwnershipShareModeEnum.None || (shareMode == MyOwnershipShareModeEnum.Faction && (myFaction == null || (myFaction2 != null && myFaction.FactionId != myFaction2.FactionId))))
								{
									continue;
								}
							}
						}
						if (playerId == -2)
						{
							MyTerminalBlock myTerminalBlock = item2.Container.Entity as MyTerminalBlock;
							if (myTerminalBlock != null)
							{
								string a = myTerminalBlock.CustomName.ToString();
								if (a != "Special Content" && a != "Special Content Power")
								{
									continue;
								}
							}
						}
						item2.MaxOutputChanged -= source_MaxOutputChanged;
						item2.SetEnabled(newValue, fireEvents: false);
						item2.MaxOutputChanged += source_MaxOutputChanged;
					}
				}
				foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
				{
					MyResourceSourceComponent item = inputOutput.Item2;
					if (!flag && playerId >= 0 && item.Entity != null)
					{
						MyFunctionalBlock myFunctionalBlock2 = item.Entity as MyFunctionalBlock;
						if (myFunctionalBlock2 != null && myFunctionalBlock2.OwnerId != 0L && myFunctionalBlock2.OwnerId != playerId)
						{
							MyOwnershipShareModeEnum shareMode2 = myFunctionalBlock2.IDModule.ShareMode;
							IMyFaction myFaction3 = MySession.Static.Factions.TryGetPlayerFaction(myFunctionalBlock2.OwnerId);
							if (shareMode2 == MyOwnershipShareModeEnum.None || (shareMode2 == MyOwnershipShareModeEnum.Faction && (myFaction == null || (myFaction3 != null && myFaction.FactionId != myFaction3.FactionId))))
							{
								continue;
							}
						}
					}
					item.MaxOutputChanged -= source_MaxOutputChanged;
					item.SetEnabled(newValue, fireEvents: false);
					item.MaxOutputChanged += source_MaxOutputChanged;
				}
				m_dataPerType[typeIndex].SourcesEnabledDirty = false;
				m_dataPerType[typeIndex].NeedsRecompute = true;
				m_recomputeInProgress = false;
			}
		}

		private float ComputeRemainingFuelTime(MyDefinitionId resourceTypeId)
		{
			try
			{
				int typeIndex = GetTypeIndex(ref resourceTypeId);
				if (m_dataPerType[typeIndex].MaxAvailableResource == 0f)
				{
					return 0f;
				}
				float num = 0f;
				MySinkGroupData[] sinkDataByPriority = m_dataPerType[typeIndex].SinkDataByPriority;
				for (int i = 0; i < sinkDataByPriority.Length; i++)
				{
					MySinkGroupData mySinkGroupData = sinkDataByPriority[i];
					if (mySinkGroupData.RemainingAvailableResource >= mySinkGroupData.RequiredInput)
					{
						num += mySinkGroupData.RequiredInput;
					}
					else
					{
						if (!mySinkGroupData.IsAdaptible)
						{
							break;
						}
						num += mySinkGroupData.RemainingAvailableResource;
					}
				}
				num = ((!(m_dataPerType[typeIndex].InputOutputData.Item1.RemainingAvailableResource > m_dataPerType[typeIndex].InputOutputData.Item1.RequiredInput)) ? (num + m_dataPerType[typeIndex].InputOutputData.Item1.RemainingAvailableResource) : (num + m_dataPerType[typeIndex].InputOutputData.Item1.RequiredInput));
				bool flag = false;
				bool flag2 = false;
				float num2 = 0f;
				for (int j = 0; j < m_dataPerType[typeIndex].SourcesByPriority.Length; j++)
				{
					MySourceGroupData mySourceGroupData = m_dataPerType[typeIndex].SourceDataByPriority[j];
					if (!(mySourceGroupData.UsageRatio <= 0f))
					{
						if (mySourceGroupData.InfiniteCapacity)
						{
							flag = true;
							num -= mySourceGroupData.UsageRatio * mySourceGroupData.MaxAvailableResource;
						}
						else
						{
							foreach (MyResourceSourceComponent item in m_dataPerType[typeIndex].SourcesByPriority[j])
							{
								if (item.Enabled && item.ProductionEnabledByType(resourceTypeId))
								{
									flag2 = true;
									num2 += item.RemainingCapacityByType(resourceTypeId);
								}
							}
						}
					}
				}
				if (m_dataPerType[typeIndex].InputOutputData.Item2.UsageRatio > 0f)
				{
					foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
					{
						if (inputOutput.Item2.Enabled && inputOutput.Item2.ProductionEnabledByType(resourceTypeId))
						{
							flag2 = true;
							num2 += inputOutput.Item2.RemainingCapacityByType(resourceTypeId);
						}
					}
				}
				if (flag && !flag2)
				{
					return float.PositiveInfinity;
				}
				float result = 0f;
				if (num > 0f)
				{
					result = num2 / num;
				}
				return result;
			}
			finally
			{
			}
		}

		private void RefreshSourcesEnabled(MyDefinitionId resourceTypeId)
		{
			if (!TryGetTypeIndex(ref resourceTypeId, out int typeIndex))
			{
				return;
			}
			m_dataPerType[typeIndex].SourcesEnabledDirty = false;
			if (m_dataPerType[typeIndex].SourceCount == 0)
			{
				m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
				return;
			}
			bool flag = true;
			bool flag2 = true;
			HashSet<MyResourceSourceComponent>[] sourcesByPriority = m_dataPerType[typeIndex].SourcesByPriority;
			for (int i = 0; i < sourcesByPriority.Length; i++)
			{
				foreach (MyResourceSourceComponent item in sourcesByPriority[i])
				{
					flag = (flag && item.Enabled);
					flag2 = (flag2 && !item.Enabled);
					if (!flag && !flag2)
					{
						m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.Mixed;
						return;
					}
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
			{
				flag = (flag && inputOutput.Item2.Enabled);
				flag2 = (flag2 && !inputOutput.Item2.Enabled);
				if (!flag && !flag2)
				{
					m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.Mixed;
					return;
				}
			}
			m_dataPerType[typeIndex].SourcesEnabled = (flag2 ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
		}

		internal void CubeGrid_OnFatBlockAddedOrRemoved(MyCubeBlock fatblock)
		{
			IMyConveyorEndpointBlock obj = fatblock as IMyConveyorEndpointBlock;
			IMyConveyorSegmentBlock myConveyorSegmentBlock = fatblock as IMyConveyorSegmentBlock;
			if (obj != null || myConveyorSegmentBlock != null)
			{
				foreach (PerTypeData item in m_dataPerType)
				{
					item.GroupsDirty = true;
					item.NeedsRecompute = true;
				}
			}
		}

		private void CheckDistributionSystemChanges()
		{
			if (m_sinksToRemove.Count > 0)
			{
				lock (m_sinksToRemove)
				{
					MyTuple<MyResourceSinkComponent, bool>[] array = m_sinksToRemove.ToArray();
					for (int i = 0; i < array.Length; i++)
					{
						MyTuple<MyResourceSinkComponent, bool> myTuple = array[i];
						RemoveSinkLazy(myTuple.Item1, myTuple.Item2);
						foreach (MyDefinitionId acceptedResource in myTuple.Item1.AcceptedResources)
						{
							m_changedTypes[acceptedResource] = Math.Max(0, m_changedTypes[acceptedResource] - 1);
						}
					}
					m_sinksToRemove.Clear();
				}
			}
			if (m_sourcesToRemove.Count > 0)
			{
				lock (m_sourcesToRemove)
				{
					foreach (MyResourceSourceComponent item in m_sourcesToRemove)
					{
						RemoveSourceLazy(item);
						foreach (MyDefinitionId resourceType in item.ResourceTypes)
						{
							m_changedTypes[resourceType] = Math.Max(0, m_changedTypes[resourceType] - 1);
						}
					}
					m_sourcesToRemove.Clear();
				}
			}
			if (m_sourcesToAdd.Count > 0)
			{
				lock (m_sourcesToAdd)
				{
					foreach (MyResourceSourceComponent item2 in m_sourcesToAdd)
					{
						AddSourceLazy(item2);
						foreach (MyDefinitionId resourceType2 in item2.ResourceTypes)
						{
							m_changedTypes[resourceType2] = Math.Max(0, m_changedTypes[resourceType2] - 1);
						}
					}
					m_sourcesToAdd.Clear();
				}
			}
			if (m_sinksToAdd.Count > 0)
			{
				lock (m_sinksToAdd)
				{
					foreach (MyResourceSinkComponent item3 in m_sinksToAdd)
					{
						AddSinkLazy(item3);
						foreach (MyDefinitionId acceptedResource2 in item3.AcceptedResources)
						{
							m_changedTypes[acceptedResource2] = Math.Max(0, m_changedTypes[acceptedResource2] - 1);
						}
					}
					m_sinksToAdd.Clear();
				}
			}
		}

		private void RecomputeResourceDistribution(ref MyDefinitionId typeId, bool updateChanges = true)
		{
			if (m_recomputeInProgress)
			{
				return;
			}
			m_recomputeInProgress = true;
			if (updateChanges && !m_updateInProgress)
			{
				m_updateInProgress = true;
				CheckDistributionSystemChanges();
				m_updateInProgress = false;
			}
			int typeIndex = GetTypeIndex(ref typeId);
			if (m_dataPerType[typeIndex].SinksByPriority.Length == 0 && m_dataPerType[typeIndex].SourcesByPriority.Length == 0 && m_dataPerType[typeIndex].InputOutputList.Count == 0)
			{
				m_typesToRemove.Add(typeId);
				m_recomputeInProgress = false;
				return;
			}
			if (!IsConveyorConnectionRequired(ref typeId))
			{
				ComputeInitialDistributionData(ref typeId, m_dataPerType[typeIndex].SinkDataByPriority, m_dataPerType[typeIndex].SourceDataByPriority, ref m_dataPerType[typeIndex].InputOutputData, m_dataPerType[typeIndex].SinksByPriority, m_dataPerType[typeIndex].SourcesByPriority, m_dataPerType[typeIndex].InputOutputList, m_dataPerType[typeIndex].StockpilingStorageIndices, m_dataPerType[typeIndex].OtherStorageIndices, out m_dataPerType[typeIndex].MaxAvailableResource);
				m_dataPerType[typeIndex].ResourceState = RecomputeResourceDistributionPartial(ref typeId, 0, m_dataPerType[typeIndex].SinkDataByPriority, m_dataPerType[typeIndex].SourceDataByPriority, ref m_dataPerType[typeIndex].InputOutputData, m_dataPerType[typeIndex].SinksByPriority, m_dataPerType[typeIndex].SourcesByPriority, m_dataPerType[typeIndex].InputOutputList, m_dataPerType[typeIndex].StockpilingStorageIndices, m_dataPerType[typeIndex].OtherStorageIndices, m_dataPerType[typeIndex].MaxAvailableResource);
			}
			else
			{
				if (m_dataPerType[typeIndex].GroupsDirty)
				{
					m_dataPerType[typeIndex].GroupsDirty = false;
					m_dataPerType[typeIndex].DistributionGroupsInUse = 0;
					RecreatePhysicalDistributionGroups(ref typeId, m_dataPerType[typeIndex].SinksByPriority, m_dataPerType[typeIndex].SourcesByPriority, m_dataPerType[typeIndex].InputOutputList);
				}
				m_dataPerType[typeIndex].MaxAvailableResource = 0f;
				for (int i = 0; i < m_dataPerType[typeIndex].DistributionGroupsInUse; i++)
				{
					MyPhysicalDistributionGroup value = m_dataPerType[typeIndex].DistributionGroups[i];
					ComputeInitialDistributionData(ref typeId, value.SinkDataByPriority, value.SourceDataByPriority, ref value.InputOutputData, value.SinksByPriority, value.SourcesByPriority, value.SinkSourcePairs, value.StockpilingStorage, value.OtherStorage, out value.MaxAvailableResources);
					value.ResourceState = RecomputeResourceDistributionPartial(ref typeId, 0, value.SinkDataByPriority, value.SourceDataByPriority, ref value.InputOutputData, value.SinksByPriority, value.SourcesByPriority, value.SinkSourcePairs, value.StockpilingStorage, value.OtherStorage, value.MaxAvailableResources);
					m_dataPerType[typeIndex].MaxAvailableResource += value.MaxAvailableResources;
					m_dataPerType[typeIndex].DistributionGroups[i] = value;
				}
				MyResourceStateEnum resourceState;
				if (m_dataPerType[typeIndex].MaxAvailableResource == 0f)
				{
					resourceState = MyResourceStateEnum.NoPower;
				}
				else
				{
					resourceState = MyResourceStateEnum.Ok;
					for (int j = 0; j < m_dataPerType[typeIndex].DistributionGroupsInUse; j++)
					{
						if (m_dataPerType[typeIndex].DistributionGroups[j].ResourceState == MyResourceStateEnum.OverloadAdaptible)
						{
							resourceState = MyResourceStateEnum.OverloadAdaptible;
							break;
						}
						if (m_dataPerType[typeIndex].DistributionGroups[j].ResourceState == MyResourceStateEnum.OverloadBlackout)
						{
							resourceState = MyResourceStateEnum.OverloadAdaptible;
							break;
						}
					}
				}
				m_dataPerType[typeIndex].ResourceState = resourceState;
			}
			m_dataPerType[typeIndex].NeedsRecompute = false;
			m_recomputeInProgress = false;
		}

		private void RecreatePhysicalDistributionGroups(ref MyDefinitionId typeId, HashSet<MyResourceSinkComponent>[] allSinksByPriority, HashSet<MyResourceSourceComponent>[] allSourcesByPriority, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> allSinkSources)
		{
			for (int i = 0; i < allSinksByPriority.Length; i++)
			{
				foreach (MyResourceSinkComponent item in allSinksByPriority[i])
				{
					if (item.Entity == null)
					{
						if (item.TemporaryConnectedEntity != null)
						{
							SetEntityGroupForTempConnected(ref typeId, item);
						}
					}
					else
					{
						SetEntityGroup(ref typeId, item.Entity);
					}
				}
			}
			for (int i = 0; i < allSourcesByPriority.Length; i++)
			{
				foreach (MyResourceSourceComponent item2 in allSourcesByPriority[i])
				{
					if (item2.Entity == null)
					{
						if (item2.TemporaryConnectedEntity != null)
						{
							SetEntityGroupForTempConnected(ref typeId, item2);
						}
					}
					else
					{
						SetEntityGroup(ref typeId, item2.Entity);
					}
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> allSinkSource in allSinkSources)
			{
				if (allSinkSource.Item1.Entity != null)
				{
					SetEntityGroup(ref typeId, allSinkSource.Item1.Entity);
				}
			}
		}

		private void SetEntityGroup(ref MyDefinitionId typeId, IMyEntity entity)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
			if (myConveyorEndpointBlock == null)
			{
				return;
			}
			int typeIndex = GetTypeIndex(ref typeId);
			bool flag = false;
			for (int i = 0; i < m_dataPerType[typeIndex].DistributionGroupsInUse; i++)
			{
				if (MyGridConveyorSystem.Reachable(m_dataPerType[typeIndex].DistributionGroups[i].FirstEndpoint, myConveyorEndpointBlock.ConveyorEndpoint))
				{
					MyPhysicalDistributionGroup value = m_dataPerType[typeIndex].DistributionGroups[i];
					value.Add(typeId, myConveyorEndpointBlock);
					m_dataPerType[typeIndex].DistributionGroups[i] = value;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (++m_dataPerType[typeIndex].DistributionGroupsInUse > m_dataPerType[typeIndex].DistributionGroups.Count)
				{
					m_dataPerType[typeIndex].DistributionGroups.Add(new MyPhysicalDistributionGroup(typeId, myConveyorEndpointBlock));
					return;
				}
				MyPhysicalDistributionGroup value2 = m_dataPerType[typeIndex].DistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
				value2.Init(typeId, myConveyorEndpointBlock);
				m_dataPerType[typeIndex].DistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = value2;
			}
		}

		private void SetEntityGroupForTempConnected(ref MyDefinitionId typeId, MyResourceSinkComponent sink)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = sink.TemporaryConnectedEntity as IMyConveyorEndpointBlock;
			int typeIndex = GetTypeIndex(ref typeId);
			bool flag = false;
			int num = 0;
			while (num < m_dataPerType[typeIndex].DistributionGroupsInUse)
			{
				if (myConveyorEndpointBlock == null || !MyGridConveyorSystem.Reachable(m_dataPerType[typeIndex].DistributionGroups[num].FirstEndpoint, myConveyorEndpointBlock.ConveyorEndpoint))
				{
					bool flag2 = false;
					if (myConveyorEndpointBlock == null)
					{
						HashSet<MyResourceSourceComponent>[] sourcesByPriority = m_dataPerType[typeIndex].DistributionGroups[num].SourcesByPriority;
						for (int i = 0; i < sourcesByPriority.Length; i++)
						{
							foreach (MyResourceSourceComponent item in sourcesByPriority[i])
							{
								if (sink.TemporaryConnectedEntity == item.TemporaryConnectedEntity)
								{
									flag2 = true;
									break;
								}
							}
							if (flag2)
							{
								break;
							}
						}
					}
					if (!flag2)
					{
						num++;
						continue;
					}
				}
				MyPhysicalDistributionGroup value = m_dataPerType[typeIndex].DistributionGroups[num];
				value.AddTempConnected(typeId, sink);
				m_dataPerType[typeIndex].DistributionGroups[num] = value;
				flag = true;
				break;
			}
			if (!flag)
			{
				if (++m_dataPerType[typeIndex].DistributionGroupsInUse > m_dataPerType[typeIndex].DistributionGroups.Count)
				{
					m_dataPerType[typeIndex].DistributionGroups.Add(new MyPhysicalDistributionGroup(typeId, sink));
					return;
				}
				MyPhysicalDistributionGroup value2 = m_dataPerType[typeIndex].DistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
				value2.InitFromTempConnected(typeId, sink);
				m_dataPerType[typeIndex].DistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = value2;
			}
		}

		private void SetEntityGroupForTempConnected(ref MyDefinitionId typeId, MyResourceSourceComponent source)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = source.TemporaryConnectedEntity as IMyConveyorEndpointBlock;
			int typeIndex = GetTypeIndex(ref typeId);
			bool flag = false;
			int num = 0;
			while (num < m_dataPerType[typeIndex].DistributionGroupsInUse)
			{
				if (myConveyorEndpointBlock == null || !MyGridConveyorSystem.Reachable(m_dataPerType[typeIndex].DistributionGroups[num].FirstEndpoint, myConveyorEndpointBlock.ConveyorEndpoint))
				{
					bool flag2 = false;
					if (myConveyorEndpointBlock == null)
					{
						HashSet<MyResourceSinkComponent>[] sinksByPriority = m_dataPerType[typeIndex].DistributionGroups[num].SinksByPriority;
						for (int i = 0; i < sinksByPriority.Length; i++)
						{
							foreach (MyResourceSinkComponent item in sinksByPriority[i])
							{
								if (source.TemporaryConnectedEntity == item.TemporaryConnectedEntity)
								{
									flag2 = true;
									break;
								}
							}
							if (flag2)
							{
								break;
							}
						}
					}
					if (!flag2)
					{
						num++;
						continue;
					}
				}
				MyPhysicalDistributionGroup value = m_dataPerType[typeIndex].DistributionGroups[num];
				value.AddTempConnected(typeId, source);
				m_dataPerType[typeIndex].DistributionGroups[num] = value;
				flag = true;
				break;
			}
			if (!flag)
			{
				if (++m_dataPerType[typeIndex].DistributionGroupsInUse > m_dataPerType[typeIndex].DistributionGroups.Count)
				{
					m_dataPerType[typeIndex].DistributionGroups.Add(new MyPhysicalDistributionGroup(typeId, source));
					return;
				}
				MyPhysicalDistributionGroup value2 = m_dataPerType[typeIndex].DistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
				value2.InitFromTempConnected(typeId, source);
				m_dataPerType[typeIndex].DistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = value2;
			}
		}

		private static void ComputeInitialDistributionData(ref MyDefinitionId typeId, MySinkGroupData[] sinkDataByPriority, MySourceGroupData[] sourceDataByPriority, ref MyTuple<MySinkGroupData, MySourceGroupData> sinkSourceData, HashSet<MyResourceSinkComponent>[] sinksByPriority, HashSet<MyResourceSourceComponent>[] sourcesByPriority, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs, MyList<int> stockpilingStorageList, MyList<int> otherStorageList, out float maxAvailableResource)
		{
			maxAvailableResource = 0f;
			for (int i = 0; i < sourceDataByPriority.Length; i++)
			{
				HashSet<MyResourceSourceComponent> obj = sourcesByPriority[i];
				MySourceGroupData mySourceGroupData = sourceDataByPriority[i];
				mySourceGroupData.MaxAvailableResource = 0f;
				foreach (MyResourceSourceComponent item in obj)
				{
					if (item.Enabled && item.HasCapacityRemainingByType(typeId))
					{
						mySourceGroupData.MaxAvailableResource += item.MaxOutputByType(typeId);
						mySourceGroupData.InfiniteCapacity = item.IsInfiniteCapacity;
					}
				}
				maxAvailableResource += mySourceGroupData.MaxAvailableResource;
				sourceDataByPriority[i] = mySourceGroupData;
			}
			float num = 0f;
			for (int j = 0; j < sinksByPriority.Length; j++)
			{
				float num2 = 0f;
				bool flag = true;
				foreach (MyResourceSinkComponent item2 in sinksByPriority[j])
				{
					num2 += item2.RequiredInputByType(typeId);
					flag = (flag && IsAdaptible(item2));
				}
				sinkDataByPriority[j].RequiredInput = num2;
				sinkDataByPriority[j].IsAdaptible = flag;
				num += num2;
				sinkDataByPriority[j].RequiredInputCumulative = num;
			}
			PrepareSinkSourceData(ref typeId, ref sinkSourceData, sinkSourcePairs, stockpilingStorageList, otherStorageList);
			maxAvailableResource += sinkSourceData.Item2.MaxAvailableResource;
		}

		private static void PrepareSinkSourceData(ref MyDefinitionId typeId, ref MyTuple<MySinkGroupData, MySourceGroupData> sinkSourceData, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs, MyList<int> stockpilingStorageList, MyList<int> otherStorageList)
		{
			stockpilingStorageList.Clear();
			otherStorageList.Clear();
			sinkSourceData.Item1.IsAdaptible = true;
			sinkSourceData.Item1.RequiredInput = 0f;
			sinkSourceData.Item1.RequiredInputCumulative = 0f;
			sinkSourceData.Item2.MaxAvailableResource = 0f;
			for (int i = 0; i < sinkSourcePairs.Count; i++)
			{
				MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> myTuple = sinkSourcePairs[i];
				bool flag = myTuple.Item2.ProductionEnabledByType(typeId);
				bool num = myTuple.Item2.Enabled && !flag && myTuple.Item1.RequiredInputByType(typeId) > 0f;
				sinkSourceData.Item1.IsAdaptible = (sinkSourceData.Item1.IsAdaptible && IsAdaptible(myTuple.Item1));
				sinkSourceData.Item1.RequiredInput += myTuple.Item1.RequiredInputByType(typeId);
				if (num)
				{
					sinkSourceData.Item1.RequiredInputCumulative += myTuple.Item1.RequiredInputByType(typeId);
				}
				sinkSourceData.Item2.InfiniteCapacity = float.IsInfinity(myTuple.Item2.RemainingCapacityByType(typeId));
				if (num)
				{
					stockpilingStorageList.Add(i);
					continue;
				}
				otherStorageList.Add(i);
				if (myTuple.Item2.Enabled && flag)
				{
					sinkSourceData.Item2.MaxAvailableResource += myTuple.Item2.MaxOutputByType(typeId);
				}
			}
		}

		private static MyResourceStateEnum RecomputeResourceDistributionPartial(ref MyDefinitionId typeId, int startPriorityIdx, MySinkGroupData[] sinkDataByPriority, MySourceGroupData[] sourceDataByPriority, ref MyTuple<MySinkGroupData, MySourceGroupData> sinkSourceData, HashSet<MyResourceSinkComponent>[] sinksByPriority, HashSet<MyResourceSourceComponent>[] sourcesByPriority, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs, MyList<int> stockpilingStorageList, MyList<int> otherStorageList, float availableResource)
		{
			float num = availableResource;
			int i;
			for (i = startPriorityIdx; i < sinksByPriority.Length; i++)
			{
				sinkDataByPriority[i].RemainingAvailableResource = availableResource;
				if (sinkDataByPriority[i].RequiredInput <= availableResource)
				{
					availableResource -= sinkDataByPriority[i].RequiredInput;
					foreach (MyResourceSinkComponent item8 in sinksByPriority[i])
					{
						item8.SetInputFromDistributor(typeId, item8.RequiredInputByType(typeId), sinkDataByPriority[i].IsAdaptible);
					}
				}
				else if (sinkDataByPriority[i].IsAdaptible && availableResource > 0f)
				{
					foreach (MyResourceSinkComponent item9 in sinksByPriority[i])
					{
						item9.SetInputFromDistributor(newResourceInput: item9.RequiredInputByType(typeId) / sinkDataByPriority[i].RequiredInput * availableResource, resourceTypeId: typeId, isAdaptible: true);
					}
					availableResource = 0f;
				}
				else
				{
					foreach (MyResourceSinkComponent item10 in sinksByPriority[i])
					{
						item10.SetInputFromDistributor(typeId, 0f, sinkDataByPriority[i].IsAdaptible);
					}
					sinkDataByPriority[i].RemainingAvailableResource = availableResource;
				}
			}
			for (; i < sinkDataByPriority.Length; i++)
			{
				sinkDataByPriority[i].RemainingAvailableResource = 0f;
				foreach (MyResourceSinkComponent item11 in sinksByPriority[i])
				{
					item11.SetInputFromDistributor(typeId, 0f, sinkDataByPriority[i].IsAdaptible);
				}
			}
			float num2 = num - availableResource + ((startPriorityIdx != 0) ? (sinkDataByPriority[0].RemainingAvailableResource - sinkDataByPriority[startPriorityIdx].RemainingAvailableResource) : 0f);
			float num3 = Math.Max(num - num2, 0f);
			float num4 = num3;
			if (stockpilingStorageList.Count > 0)
			{
				float requiredInputCumulative = sinkSourceData.Item1.RequiredInputCumulative;
				if (requiredInputCumulative <= num4)
				{
					num4 -= requiredInputCumulative;
					foreach (int stockpilingStorage in stockpilingStorageList)
					{
						MyResourceSinkComponent item = sinkSourcePairs[stockpilingStorage].Item1;
						item.SetInputFromDistributor(typeId, item.RequiredInputByType(typeId), isAdaptible: true);
					}
					sinkSourceData.Item1.RemainingAvailableResource = num4;
				}
				else
				{
					foreach (int stockpilingStorage2 in stockpilingStorageList)
					{
						MyResourceSinkComponent item2 = sinkSourcePairs[stockpilingStorage2].Item1;
						item2.SetInputFromDistributor(newResourceInput: item2.RequiredInputByType(typeId) / requiredInputCumulative * num3, resourceTypeId: typeId, isAdaptible: true);
					}
					num4 = 0f;
					sinkSourceData.Item1.RemainingAvailableResource = num4;
				}
			}
			float num5 = num3 - num4;
			float num6 = Math.Max(num - (sinkSourceData.Item2.MaxAvailableResource - sinkSourceData.Item2.MaxAvailableResource * sinkSourceData.Item2.UsageRatio) - num2 - num5, 0f);
			float num7 = num6;
			if (otherStorageList.Count > 0)
			{
				float num8 = sinkSourceData.Item1.RequiredInput - sinkSourceData.Item1.RequiredInputCumulative;
				if (num8 <= num7)
				{
					num7 -= num8;
					for (int j = 0; j < otherStorageList.Count; j++)
					{
						int index = otherStorageList[j];
						MyResourceSinkComponent item3 = sinkSourcePairs[index].Item1;
						item3.SetInputFromDistributor(typeId, item3.RequiredInputByType(typeId), isAdaptible: true);
					}
					sinkSourceData.Item1.RemainingAvailableResource = num7;
				}
				else
				{
					for (int k = 0; k < otherStorageList.Count; k++)
					{
						int index2 = otherStorageList[k];
						MyResourceSinkComponent item4 = sinkSourcePairs[index2].Item1;
						item4.SetInputFromDistributor(newResourceInput: item4.RequiredInputByType(typeId) / num8 * num7, resourceTypeId: typeId, isAdaptible: true);
					}
					num7 = 0f;
					sinkSourceData.Item1.RemainingAvailableResource = num7;
				}
			}
			float num9 = num6 - num7;
			float num10 = num5 + num2;
			if (sinkSourceData.Item2.MaxAvailableResource > 0f)
			{
				float num11 = num10;
				sinkSourceData.Item2.UsageRatio = Math.Min(1f, num11 / sinkSourceData.Item2.MaxAvailableResource);
				num10 -= Math.Min(num11, sinkSourceData.Item2.MaxAvailableResource);
			}
			else
			{
				sinkSourceData.Item2.UsageRatio = 0f;
			}
			num5 = num3 - num4;
			num6 = Math.Max(num - (sinkSourceData.Item2.MaxAvailableResource - sinkSourceData.Item2.MaxAvailableResource * sinkSourceData.Item2.UsageRatio) - num2 - num5, 0f);
			num7 = num6;
			if (otherStorageList.Count > 0)
			{
				float num12 = sinkSourceData.Item1.RequiredInput - sinkSourceData.Item1.RequiredInputCumulative;
				if (num12 <= num7)
				{
					num7 -= num12;
					for (int l = 0; l < otherStorageList.Count; l++)
					{
						int index3 = otherStorageList[l];
						MyResourceSinkComponent item5 = sinkSourcePairs[index3].Item1;
						item5.SetInputFromDistributor(typeId, item5.RequiredInputByType(typeId), isAdaptible: true);
					}
					sinkSourceData.Item1.RemainingAvailableResource = num7;
				}
				else
				{
					for (int m = 0; m < otherStorageList.Count; m++)
					{
						int index4 = otherStorageList[m];
						MyResourceSinkComponent item6 = sinkSourcePairs[index4].Item1;
						item6.SetInputFromDistributor(newResourceInput: item6.RequiredInputByType(typeId) / num12 * num7, resourceTypeId: typeId, isAdaptible: true);
					}
					num7 = 0f;
					sinkSourceData.Item1.RemainingAvailableResource = num7;
				}
			}
			sinkSourceData.Item2.ActiveCount = 0;
			for (int n = 0; n < otherStorageList.Count; n++)
			{
				int index5 = otherStorageList[n];
				MyResourceSourceComponent item7 = sinkSourcePairs[index5].Item2;
				if (item7.Enabled && item7.ProductionEnabledByType(typeId) && item7.HasCapacityRemainingByType(typeId))
				{
					sinkSourceData.Item2.ActiveCount++;
					item7.SetOutputByType(typeId, sinkSourceData.Item2.UsageRatio * item7.MaxOutputByType(typeId));
				}
			}
			int num13 = 0;
			float num14 = num10 + num9;
			for (; num13 < sourcesByPriority.Length; num13++)
			{
				if (sourceDataByPriority[num13].MaxAvailableResource > 0f)
				{
					float num15 = Math.Max(num14, 0f);
					sourceDataByPriority[num13].UsageRatio = Math.Min(1f, num15 / sourceDataByPriority[num13].MaxAvailableResource);
					num14 -= Math.Min(num15, sourceDataByPriority[num13].MaxAvailableResource);
				}
				else
				{
					sourceDataByPriority[num13].UsageRatio = 0f;
				}
				sourceDataByPriority[num13].ActiveCount = 0;
				foreach (MyResourceSourceComponent item12 in sourcesByPriority[num13])
				{
					if (item12.Enabled && item12.HasCapacityRemainingByType(typeId))
					{
						sourceDataByPriority[num13].ActiveCount++;
						item12.SetOutputByType(typeId, sourceDataByPriority[num13].UsageRatio * item12.MaxOutputByType(typeId));
					}
				}
			}
			if (num == 0f)
			{
				return MyResourceStateEnum.NoPower;
			}
			if (sinkDataByPriority[m_sinkGroupPrioritiesTotal - 1].RequiredInputCumulative > num)
			{
				MySinkGroupData mySinkGroupData = sinkDataByPriority.Last();
				if (mySinkGroupData.IsAdaptible && mySinkGroupData.RemainingAvailableResource != 0f)
				{
					return MyResourceStateEnum.OverloadAdaptible;
				}
				return MyResourceStateEnum.OverloadBlackout;
			}
			return MyResourceStateEnum.Ok;
		}

		private bool MatchesAdaptability(HashSet<MyResourceSinkComponent> group, MyResourceSinkComponent referenceSink)
		{
			bool flag = IsAdaptible(referenceSink);
			foreach (MyResourceSinkComponent item in group)
			{
				if (IsAdaptible(item) != flag)
				{
					return false;
				}
			}
			return true;
		}

		private bool MatchesInfiniteCapacity(HashSet<MyResourceSourceComponent> group, MyResourceSourceComponent producer)
		{
			foreach (MyResourceSourceComponent item in group)
			{
				if (producer.IsInfiniteCapacity != item.IsInfiniteCapacity)
				{
					return false;
				}
			}
			return true;
		}

		[Conditional("DEBUG")]
		private void UpdateTrace()
		{
			for (int i = 0; i < m_typeGroupCount; i++)
			{
				for (int j = 0; j < m_dataPerType[i].DistributionGroupsInUse; j++)
				{
					MyPhysicalDistributionGroup myPhysicalDistributionGroup = m_dataPerType[i].DistributionGroups[j];
					for (int k = 0; k < myPhysicalDistributionGroup.SinkSourcePairs.Count; k++)
					{
					}
				}
			}
		}

		private HashSet<MyResourceSinkComponent> GetSinksOfType(ref MyDefinitionId typeId, MyStringHash groupType)
		{
			if (!TryGetTypeIndex(typeId, out int typeIndex) || !m_sinkSubtypeToPriority.TryGetValue(groupType, out int value))
			{
				return null;
			}
			return m_dataPerType[typeIndex].SinksByPriority[value];
		}

		private HashSet<MyResourceSourceComponent> GetSourcesOfType(ref MyDefinitionId typeId, MyStringHash groupType)
		{
			if (!TryGetTypeIndex(typeId, out int typeIndex) || !m_sourceSubtypeToPriority.TryGetValue(groupType, out int value))
			{
				return null;
			}
			return m_dataPerType[typeIndex].SourcesByPriority[value];
		}

		private MyResourceSourceComponent GetFirstSourceOfType(ref MyDefinitionId resourceTypeId)
		{
			int typeIndex = GetTypeIndex(ref resourceTypeId);
			for (int i = 0; i < m_dataPerType[typeIndex].SourcesByPriority.Length; i++)
			{
				HashSet<MyResourceSourceComponent> hashSet = m_dataPerType[typeIndex].SourcesByPriority[i];
				if (hashSet.Count > 0)
				{
					return hashSet.FirstElement();
				}
			}
			return null;
		}

		public MyMultipleEnabledEnum SourcesEnabledByType(MyDefinitionId resourceTypeId)
		{
			if (!TryGetTypeIndex(ref resourceTypeId, out int typeIndex))
			{
				return MyMultipleEnabledEnum.NoObjects;
			}
			if (m_dataPerType[typeIndex].SourcesEnabledDirty)
			{
				RefreshSourcesEnabled(resourceTypeId);
			}
			return m_dataPerType[typeIndex].SourcesEnabled;
		}

		public float RemainingFuelTimeByType(MyDefinitionId resourceTypeId)
		{
			if (!TryGetTypeIndex(ref resourceTypeId, out int typeIndex))
			{
				return 0f;
			}
			if (!m_dataPerType[typeIndex].RemainingFuelTimeDirty)
			{
				return m_dataPerType[typeIndex].RemainingFuelTime;
			}
			m_dataPerType[typeIndex].RemainingFuelTime = ComputeRemainingFuelTime(resourceTypeId);
			return m_dataPerType[typeIndex].RemainingFuelTime;
		}

		private bool NeedsRecompute(ref MyDefinitionId typeId)
		{
			if (!m_changedTypes.TryGetValue(typeId, out int value) || value <= 0)
			{
				if (TryGetTypeIndex(ref typeId, out int typeIndex))
				{
					return m_dataPerType[typeIndex].NeedsRecompute;
				}
				return false;
			}
			return true;
		}

		private bool NeedsRecompute(ref MyDefinitionId typeId, int typeIndex)
		{
			if (m_typeGroupCount <= 0 || typeIndex < 0 || m_dataPerType.Count <= typeIndex || !m_dataPerType[typeIndex].NeedsRecompute)
			{
				if (m_changedTypes.TryGetValue(typeId, out int value))
				{
					return value > 0;
				}
				return false;
			}
			return true;
		}

		public MyResourceStateEnum ResourceStateByType(MyDefinitionId typeId, bool withRecompute = true)
		{
			int typeIndex = GetTypeIndex(ref typeId);
			if (withRecompute && NeedsRecompute(ref typeId))
			{
				RecomputeResourceDistribution(ref typeId);
			}
			if (!withRecompute && (typeIndex < 0 || typeIndex >= m_dataPerType.Count))
			{
				return MyResourceStateEnum.NoPower;
			}
			return m_dataPerType[typeIndex].ResourceState;
		}

		private bool TryGetTypeIndex(MyDefinitionId typeId, out int typeIndex)
		{
			return TryGetTypeIndex(ref typeId, out typeIndex);
		}

		private bool TryGetTypeIndex(ref MyDefinitionId typeId, out int typeIndex)
		{
			typeIndex = 0;
			if (m_typeGroupCount == 0)
			{
				return false;
			}
			if (m_typeGroupCount > 1)
			{
				return m_typeIdToIndex.TryGetValue(typeId, out typeIndex);
			}
			return true;
		}

		private int GetTypeIndex(ref MyDefinitionId typeId)
		{
			int result = 0;
			if (m_typeGroupCount > 1)
			{
				result = m_typeIdToIndex[typeId];
			}
			return result;
		}

		private static int GetTypeIndexTotal(ref MyDefinitionId typeId)
		{
			int result = 0;
			if (m_typeGroupCountTotal > 1)
			{
				result = m_typeIdToIndexTotal[typeId];
			}
			return result;
		}

		public static bool IsConveyorConnectionRequiredTotal(MyDefinitionId typeId)
		{
			return IsConveyorConnectionRequiredTotal(ref typeId);
		}

		public static bool IsConveyorConnectionRequiredTotal(ref MyDefinitionId typeId)
		{
			return m_typeIdToConveyorConnectionRequiredTotal[typeId];
		}

		private bool IsConveyorConnectionRequired(ref MyDefinitionId typeId)
		{
			return m_typeIdToConveyorConnectionRequired[typeId];
		}

		internal static int GetPriority(MyResourceSinkComponent sink)
		{
			return m_sinkSubtypeToPriority[sink.Group];
		}

		internal static int GetPriority(MyResourceSourceComponent source)
		{
			return m_sourceSubtypeToPriority[source.Group];
		}

		private static bool IsAdaptible(MyResourceSinkComponent sink)
		{
			return m_sinkSubtypeToAdaptability[sink.Group];
		}

		private void RemoveType(ref MyDefinitionId typeId)
		{
			if (TryGetTypeIndex(ref typeId, out int typeIndex))
			{
				m_dataPerType.RemoveAt(typeIndex);
				m_initializedTypes.Remove(typeId);
				m_typeGroupCount--;
				m_typeIdToIndex.Remove(typeId);
				m_typeIdToConveyorConnectionRequired.Remove(typeId);
			}
		}

		private void Sink_OnAddType(MyResourceSinkComponent sink, MyDefinitionId resourceType)
		{
			RemoveSinkLazy(sink, resetSinkInput: false);
			CheckDistributionSystemChanges();
			AddSinkLazy(sink);
		}

		private void Sink_OnRemoveType(MyResourceSinkComponent sink, MyDefinitionId resourceType)
		{
			RemoveSinkLazy(sink, resetSinkInput: false);
			CheckDistributionSystemChanges();
			AddSinkLazy(sink);
		}

		private void Sink_RequiredInputChanged(MyDefinitionId changedResourceTypeId, MyResourceSinkComponent changedSink, float oldRequirement, float newRequirement)
		{
			if (m_typeIdToIndex.ContainsKey(changedResourceTypeId) && m_sinkSubtypeToPriority.ContainsKey(changedSink.Group))
			{
				int typeIndex = GetTypeIndex(ref changedResourceTypeId);
				if (TryGetTypeIndex(changedResourceTypeId, out typeIndex))
				{
					m_dataPerType[typeIndex].NeedsRecompute = true;
				}
			}
		}

		private float Sink_IsResourceAvailable(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver)
		{
			int typeIndex = GetTypeIndex(ref resourceTypeId);
			int priority = GetPriority(receiver);
			if (IsConveyorConnectionRequired(ref resourceTypeId))
			{
				IMyConveyorEndpointBlock myConveyorEndpointBlock = receiver.Entity as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock == null)
				{
					return 0f;
				}
				_ = myConveyorEndpointBlock.ConveyorEndpoint;
				int i;
				for (i = 0; i < m_dataPerType[typeIndex].DistributionGroupsInUse && !m_dataPerType[typeIndex].DistributionGroups[i].SinksByPriority[priority].Contains(receiver); i++)
				{
				}
				if (i == m_dataPerType[typeIndex].DistributionGroupsInUse)
				{
					return 0f;
				}
				return m_dataPerType[typeIndex].DistributionGroups[i].SinkDataByPriority[priority].RemainingAvailableResource - m_dataPerType[typeIndex].DistributionGroups[i].SinkDataByPriority[priority].RequiredInput;
			}
			float remainingAvailableResource = m_dataPerType[typeIndex].SinkDataByPriority[priority].RemainingAvailableResource;
			float requiredInput = m_dataPerType[typeIndex].SinkDataByPriority[priority].RequiredInput;
			return remainingAvailableResource - requiredInput;
		}

		private void source_HasRemainingCapacityChanged(MyDefinitionId changedResourceTypeId, MyResourceSourceComponent source)
		{
			int typeIndex = GetTypeIndex(ref changedResourceTypeId);
			m_dataPerType[typeIndex].NeedsRecompute = true;
			m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
		}

		public void ConveyorSystem_OnPoweredChanged()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				for (int i = 0; i < m_dataPerType.Count; i++)
				{
					m_dataPerType[i].GroupsDirty = true;
					m_dataPerType[i].NeedsRecompute = true;
					m_dataPerType[i].RemainingFuelTimeDirty = true;
					m_dataPerType[i].SourcesEnabledDirty = true;
				}
			}, "ConveyorSystem_OnPoweredChanged");
		}

		private void source_MaxOutputChanged(MyDefinitionId changedResourceTypeId, float oldOutput, MyResourceSourceComponent obj)
		{
			int typeIndex = GetTypeIndex(ref changedResourceTypeId);
			m_dataPerType[typeIndex].NeedsRecompute = true;
			m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
			m_dataPerType[typeIndex].SourcesEnabledDirty = true;
			if (m_dataPerType[typeIndex].SourceCount == 1)
			{
				RecomputeResourceDistribution(ref changedResourceTypeId);
			}
		}

		private void source_ProductionEnabledChanged(MyDefinitionId changedResourceTypeId, MyResourceSourceComponent obj)
		{
			int typeIndex = GetTypeIndex(ref changedResourceTypeId);
			m_dataPerType[typeIndex].NeedsRecompute = true;
			m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
			m_dataPerType[typeIndex].SourcesEnabledDirty = true;
			if (m_dataPerType[typeIndex].SourceCount == 1)
			{
				RecomputeResourceDistribution(ref changedResourceTypeId);
			}
		}

		public float MaxAvailableResourceByType(MyDefinitionId resourceTypeId)
		{
			if (!TryGetTypeIndex(ref resourceTypeId, out int typeIndex))
			{
				return 0f;
			}
			return m_dataPerType[typeIndex].MaxAvailableResource;
		}

		public float TotalRequiredInputByType(MyDefinitionId resourceTypeId)
		{
			if (!TryGetTypeIndex(ref resourceTypeId, out int typeIndex))
			{
				return 0f;
			}
			return m_dataPerType[typeIndex].SinkDataByPriority.Last().RequiredInputCumulative;
		}

		public void DebugDraw(MyEntity entity)
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				return;
			}
			double scaleFactor = 6.5 * 0.045;
			Vector3D vector3D = entity.WorldMatrix.Translation;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.GetPhysicalGroupAABB();
				vector3D = myCubeGrid.GetPhysicalGroupAABB().Center;
			}
			Vector3D position = MySector.MainCamera.Position;
			Vector3D up = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			double val = Vector3D.Distance(vector3D, position);
			float num = (float)Math.Atan(6.5 / Math.Max(val, 0.001));
			if (num <= 0.27f)
			{
				return;
			}
			MyRenderProxy.DebugDrawText3D(vector3D, entity.ToString(), Color.Yellow, num, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			if (m_initializedTypes != null && m_initializedTypes.Count != 0)
			{
				Vector3D vector3D2 = vector3D;
				int num2 = -1;
				foreach (MyDefinitionId initializedType in m_initializedTypes)
				{
					vector3D2 = vector3D + num2 * up * scaleFactor;
					DebugDrawResource(initializedType, vector3D2, right, num);
					num2--;
				}
				while (m_changesDebug.Count > 10)
				{
					m_changesDebug.RemoveAt(0);
				}
				num2--;
				vector3D2 = vector3D + num2 * up * scaleFactor;
				MyRenderProxy.DebugDrawText3D(vector3D2 + right * 0.064999997615814209, "Recent changes:", Color.LightYellow, num, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				num2--;
				foreach (string item in m_changesDebug)
				{
					vector3D2 = vector3D + num2 * up * scaleFactor;
					MyRenderProxy.DebugDrawText3D(vector3D2 + right * 0.064999997615814209, item, Color.White, num, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
					num2--;
				}
			}
		}

		private void DebugDrawResource(MyDefinitionId resourceId, Vector3D origin, Vector3D rightVector, float textSize)
		{
			Vector3D value = 0.05000000074505806 * rightVector;
			Vector3D worldCoord = origin + value + rightVector * 0.014999999664723873;
			int value2 = 0;
			string text = resourceId.SubtypeName;
			if (m_typeIdToIndex.TryGetValue(resourceId, out value2))
			{
				PerTypeData perTypeData = m_dataPerType[value2];
				int num = 0;
				HashSet<MyResourceSinkComponent>[] sinksByPriority = perTypeData.SinksByPriority;
				foreach (HashSet<MyResourceSinkComponent> hashSet in sinksByPriority)
				{
					num += hashSet.Count;
				}
				text = $"{resourceId.SubtypeName} Sources:{perTypeData.SourceCount} Sinks:{num} Available:{perTypeData.MaxAvailableResource} State:{perTypeData.ResourceState}";
			}
			MyRenderProxy.DebugDrawLine3D(origin, origin + value, Color.White, Color.White, depthRead: false);
			MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
		}
	}
}
