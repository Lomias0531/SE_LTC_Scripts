using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Models;
using VRage.Game.Voxels;
using VRage.Utils;

namespace Sandbox.Engine.Physics
{
	public class MyBlockShapePool
	{
		public const int PREALLOCATE_COUNT = 50;

		private const int MAX_CLONE_PER_FRAME = 3;

		private Dictionary<MyDefinitionId, Dictionary<string, ConcurrentQueue<HkdBreakableShape>>> m_pools = new Dictionary<MyDefinitionId, Dictionary<string, ConcurrentQueue<HkdBreakableShape>>>();

		private MyWorkTracker<MyDefinitionId, MyBreakableShapeCloneJob> m_tracker = new MyWorkTracker<MyDefinitionId, MyBreakableShapeCloneJob>();

		private FastResourceLock m_poolLock = new FastResourceLock();

		private int m_missing;

		private bool m_dequeuedThisFrame;

		public void Preallocate()
		{
			MySandboxGame.Log.WriteLine("Preallocate shape pool - START");
			foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
				if (definitionGroup.Large != null && definitionGroup.Large.Public)
				{
					MyCubeBlockDefinition large = definitionGroup.Large;
					AllocateForDefinition(definitionGroup.Large.Model, large, 50);
					MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = definitionGroup.Large.BuildProgressModels;
					foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in buildProgressModels)
					{
						AllocateForDefinition(buildProgressModel.File, large, 50);
					}
				}
				if (definitionGroup.Small != null && definitionGroup.Small.Public)
				{
					AllocateForDefinition(definitionGroup.Small.Model, definitionGroup.Small, 50);
					MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = definitionGroup.Small.BuildProgressModels;
					foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel2 in buildProgressModels)
					{
						AllocateForDefinition(buildProgressModel2.File, definitionGroup.Small, 50);
					}
				}
			}
			MySandboxGame.Log.WriteLine("Preallocate shape pool - END");
		}

		public void AllocateForDefinition(string model, MyPhysicalModelDefinition definition, int count)
		{
			if (string.IsNullOrEmpty(model))
			{
				return;
			}
			MyModel modelOnlyData = MyModels.GetModelOnlyData(model);
			if (modelOnlyData.HavokBreakableShapes == null)
			{
				MyDestructionData.Static.LoadModelDestruction(model, definition, modelOnlyData.BoundingBoxSize);
			}
			if (modelOnlyData.HavokBreakableShapes == null || modelOnlyData.HavokBreakableShapes.Length == 0)
			{
				return;
			}
			ConcurrentQueue<HkdBreakableShape> concurrentQueue;
			using (m_poolLock.AcquireExclusiveUsing())
			{
				if (!m_pools.ContainsKey(definition.Id))
				{
					m_pools[definition.Id] = new Dictionary<string, ConcurrentQueue<HkdBreakableShape>>();
				}
				if (!m_pools[definition.Id].ContainsKey(model))
				{
					m_pools[definition.Id][model] = new ConcurrentQueue<HkdBreakableShape>();
				}
				concurrentQueue = m_pools[definition.Id][model];
			}
			int num = 0;
			while (true)
			{
				if (num >= count)
				{
					return;
				}
				HkdBreakableShape hkdBreakableShape = modelOnlyData.HavokBreakableShapes[0].Clone();
				concurrentQueue.Enqueue(hkdBreakableShape);
				if (num == 0)
				{
					HkMassProperties massProperties = default(HkMassProperties);
					hkdBreakableShape.BuildMassProperties(ref massProperties);
					if (!massProperties.InertiaTensor.IsValid())
					{
						break;
					}
				}
				num++;
			}
			MyLog.Default.WriteLine($"Block with wrong destruction! (q.isOk): {definition.Model}");
		}

		public void RefillPools()
		{
			if (m_missing == 0)
			{
				return;
			}
			if (m_dequeuedThisFrame && !MyFakes.CLONE_SHAPES_ON_WORKER)
			{
				m_dequeuedThisFrame = false;
				return;
			}
			int num = 0;
			if (MyFakes.CLONE_SHAPES_ON_WORKER)
			{
				StartJobs();
			}
			else
			{
				using (m_poolLock.AcquireSharedUsing())
				{
					foreach (KeyValuePair<MyDefinitionId, Dictionary<string, ConcurrentQueue<HkdBreakableShape>>> pool in m_pools)
					{
						foreach (KeyValuePair<string, ConcurrentQueue<HkdBreakableShape>> item in pool.Value)
						{
							if (pool.Value.Count < 50)
							{
								MyDefinitionManager.Static.TryGetDefinition(pool.Key, out MyCubeBlockDefinition definition);
								int num2 = Math.Min(50 - pool.Value.Count, 3 - num);
								AllocateForDefinition(item.Key, definition, num2);
								num += num2;
							}
							if (num >= 3)
							{
								break;
							}
						}
					}
				}
			}
			m_missing -= num;
			num = 0;
		}

		private void StartJobs()
		{
			using (m_poolLock.AcquireSharedUsing())
			{
				foreach (KeyValuePair<MyDefinitionId, Dictionary<string, ConcurrentQueue<HkdBreakableShape>>> pool in m_pools)
				{
					foreach (KeyValuePair<string, ConcurrentQueue<HkdBreakableShape>> item in pool.Value)
					{
						if (item.Value.Count < 50 && !m_tracker.Exists(pool.Key))
						{
							MyDefinitionManager.Static.TryGetDefinition(pool.Key, out MyPhysicalModelDefinition definition);
							MyModel modelOnlyData = MyModels.GetModelOnlyData(definition.Model);
							if (modelOnlyData.HavokBreakableShapes != null)
							{
								MyBreakableShapeCloneJob.Args args = default(MyBreakableShapeCloneJob.Args);
								args.Model = item.Key;
								args.DefId = pool.Key;
								args.ShapeToClone = modelOnlyData.HavokBreakableShapes[0];
								args.Count = 50 - pool.Value.Count;
								args.Tracker = m_tracker;
								MyBreakableShapeCloneJob.Start(args);
							}
						}
					}
				}
			}
		}

		public HkdBreakableShape GetBreakableShape(string model, MyCubeBlockDefinition block)
		{
			m_dequeuedThisFrame = true;
			if (!block.Public || MyFakes.LAZY_LOAD_DESTRUCTION)
			{
				using (m_poolLock.AcquireExclusiveUsing())
				{
					if (!m_pools.ContainsKey(block.Id))
					{
						m_pools[block.Id] = new Dictionary<string, ConcurrentQueue<HkdBreakableShape>>();
					}
					if (!m_pools[block.Id].ContainsKey(model))
					{
						m_pools[block.Id][model] = new ConcurrentQueue<HkdBreakableShape>();
					}
				}
			}
			ConcurrentQueue<HkdBreakableShape> concurrentQueue = m_pools[block.Id][model];
			if (concurrentQueue.Count == 0)
			{
				AllocateForDefinition(model, block, 1);
			}
			else
			{
				m_missing++;
			}
			concurrentQueue.TryDequeue(out HkdBreakableShape result);
			return result;
		}

		internal void Free()
		{
			HashSet<IntPtr> hashSet = new HashSet<IntPtr>();
			m_tracker.CancelAll();
			using (m_poolLock.AcquireExclusiveUsing())
			{
				foreach (Dictionary<string, ConcurrentQueue<HkdBreakableShape>> value in m_pools.Values)
				{
					foreach (ConcurrentQueue<HkdBreakableShape> value2 in value.Values)
					{
						foreach (HkdBreakableShape item in value2)
						{
							if (hashSet.Contains(item.NativeDebug))
							{
								string msg = "Shape " + item.Name + " was referenced twice in the pool!";
								MyLog.Default.WriteLine(msg);
							}
							hashSet.Add(item.NativeDebug);
						}
					}
				}
				foreach (Dictionary<string, ConcurrentQueue<HkdBreakableShape>> value3 in m_pools.Values)
				{
					foreach (ConcurrentQueue<HkdBreakableShape> value4 in value3.Values)
					{
						foreach (HkdBreakableShape item2 in value4)
						{
							_ = item2.NativeDebug;
							item2.RemoveReference();
						}
					}
				}
				m_pools.Clear();
			}
		}

		public void EnqueShapes(string model, MyDefinitionId id, List<HkdBreakableShape> shapes)
		{
			using (m_poolLock.AcquireExclusiveUsing())
			{
				if (!m_pools.ContainsKey(id))
				{
					m_pools[id] = new Dictionary<string, ConcurrentQueue<HkdBreakableShape>>();
				}
				if (!m_pools[id].ContainsKey(model))
				{
					m_pools[id][model] = new ConcurrentQueue<HkdBreakableShape>();
				}
			}
			foreach (HkdBreakableShape shape in shapes)
			{
				m_pools[id][model].Enqueue(shape);
			}
			m_missing -= shapes.Count;
		}

		public void EnqueShape(string model, MyDefinitionId id, HkdBreakableShape shape)
		{
			using (m_poolLock.AcquireExclusiveUsing())
			{
				if (!m_pools.ContainsKey(id))
				{
					m_pools[id] = new Dictionary<string, ConcurrentQueue<HkdBreakableShape>>();
				}
				if (!m_pools[id].ContainsKey(model))
				{
					m_pools[id][model] = new ConcurrentQueue<HkdBreakableShape>();
				}
			}
			m_pools[id][model].Enqueue(shape);
			m_missing--;
		}
	}
}
