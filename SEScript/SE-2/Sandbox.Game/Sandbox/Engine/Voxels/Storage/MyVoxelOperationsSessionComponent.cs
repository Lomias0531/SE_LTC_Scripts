using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage.Collections;
using VRage.Game.Components;

namespace Sandbox.Engine.Voxels.Storage
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyVoxelOperationsSessionComponent : MySessionComponentBase
	{
		private class StorageData : WorkData, IEquatable<StorageData>
		{
			public readonly MyStorageBase Storage;

			public bool Scheduled;

			public StorageData(MyStorageBase storage)
			{
				Storage = storage;
			}

			public bool Equals(StorageData other)
			{
				return Storage == other.Storage;
			}

			public override int GetHashCode()
			{
				return Storage.GetHashCode();
			}
		}

		private const int WaitForLazy = 300;

		public static MyVoxelOperationsSessionComponent Static;

		public bool ShouldWrite = true;

		public bool ShouldFlush = true;

		public static bool EnableCache;

		private volatile int m_scheduledCount;

		private int m_waitForFlush;

		private int m_waitForWrite;

		private readonly MyConcurrentHashSet<StorageData> m_storagesWithCache = new MyConcurrentHashSet<StorageData>();

		private Action<WorkData> m_writePendingCallback;

		private Action<WorkData> m_flushCachesCallback;

		public IEnumerable<MyStorageBase> QueuedStorages => m_storagesWithCache.Select((StorageData x) => x.Storage);

		public MyVoxelOperationsSessionComponent()
		{
			m_flushCachesCallback = FlushCaches;
			m_writePendingCallback = WritePending;
		}

		public override void BeforeStart()
		{
			Static = this;
		}

		public override void UpdateAfterSimulation()
		{
			if (m_storagesWithCache.Count != m_scheduledCount)
			{
				m_waitForWrite++;
				if (m_waitForWrite > 10)
				{
					m_waitForWrite = 0;
				}
				m_waitForFlush++;
				if (m_waitForFlush >= 300 && ShouldFlush)
				{
					m_waitForFlush = 0;
					foreach (StorageData item in m_storagesWithCache)
					{
						if (!item.Scheduled && item.Storage.HasCachedChunks)
						{
							Interlocked.Increment(ref m_scheduledCount);
							item.Scheduled = true;
							Parallel.Start(m_flushCachesCallback, null, item);
						}
					}
				}
				else if (m_waitForWrite == 0 && ShouldWrite)
				{
					foreach (StorageData item2 in m_storagesWithCache)
					{
						if (!item2.Scheduled && item2.Storage.HasPendingWrites)
						{
							Interlocked.Increment(ref m_scheduledCount);
							item2.Scheduled = true;
							Parallel.Start(m_writePendingCallback, null, item2);
						}
					}
				}
			}
		}

		public void Add(MyStorageBase storage)
		{
			StorageData instance = new StorageData(storage);
			m_storagesWithCache.Add(instance);
		}

		public void WritePending(WorkData data)
		{
			StorageData obj = (StorageData)data;
			obj.Storage.WritePending();
			obj.Scheduled = false;
			Interlocked.Decrement(ref m_scheduledCount);
		}

		public void FlushCaches(WorkData data)
		{
			StorageData obj = (StorageData)data;
			obj.Storage.CleanCachedChunks();
			obj.Scheduled = false;
			Interlocked.Decrement(ref m_scheduledCount);
		}

		public void Remove(MyStorageBase storage)
		{
			m_storagesWithCache.Remove(new StorageData(storage));
		}
	}
}
