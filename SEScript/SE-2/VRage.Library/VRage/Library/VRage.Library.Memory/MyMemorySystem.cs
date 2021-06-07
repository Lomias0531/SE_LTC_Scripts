using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Library.Collections;

namespace VRage.Library.Memory
{
	public class MyMemorySystem
	{
		private struct AllocationInfo
		{
			public ushort RefId;

			public long Size;

			public string DebugName;
		}

		public struct AllocationRecord : IDisposable
		{
			private int m_allocationId;

			private MyMemorySystem m_memorySystem;

			public AllocationRecord(int allocationId, MyMemorySystem memorySystem)
			{
				m_allocationId = allocationId;
				m_memorySystem = memorySystem;
			}

			public void Dispose()
			{
				int num = Interlocked.Exchange(ref m_allocationId, 0);
				if (num != 0)
				{
					m_memorySystem.FreeAllocation(num);
					m_memorySystem = null;
				}
			}
		}

		private const int ID_BITS = 24;

		private const int CHECKSUM_BITS = 8;

		private const int CHECKSUM_SHIFT = 24;

		private const int ID_MAX = 16777215;

		private const int CHECKSUM_MAX = 255;

		private readonly string m_systemName;

		private readonly MyMemorySystem m_parent;

		private List<MyMemorySystem> m_subsystems;

		private ushort m_refId;

		private readonly MyFreeList<AllocationInfo> m_allocatedMemory;

		private long m_thisSystemMemory;

		private long m_totalMemoryCache;

		private MyMemorySystem(MyMemorySystem parent, string systemName)
		{
			m_parent = parent;
			m_systemName = systemName;
			m_refId = 1;
			m_allocatedMemory = new MyFreeList<AllocationInfo>(100);
		}

		public static MyMemorySystem CreateRootMemorySystem(string systemName)
		{
			return new MyMemorySystem(null, systemName);
		}

		public MyMemorySystem RegisterSubsystem(string systemName)
		{
			MyMemorySystem myMemorySystem = new MyMemorySystem(this, systemName);
			if (m_subsystems == null)
			{
				Interlocked.CompareExchange(ref m_subsystems, new List<MyMemorySystem>(), null);
			}
			lock (m_subsystems)
			{
				m_subsystems.Add(myMemorySystem);
				return myMemorySystem;
			}
		}

		public void LogMemoryStats<TLogger>(ref TLogger logger) where TLogger : struct, MyMemoryTracker.ILogger
		{
			logger.BeginSystem(m_systemName);
			if (m_subsystems != null)
			{
				lock (m_subsystems)
				{
					foreach (MyMemorySystem subsystem in m_subsystems)
					{
						subsystem.LogMemoryStats(ref logger);
					}
				}
			}
			logger.EndSystem(GetTotalMemory(), m_allocatedMemory?.Count ?? 0);
		}

		public AllocationRecord RegisterAllocation(string debugName, long size)
		{
			lock (m_allocatedMemory)
			{
				int num = m_allocatedMemory.Allocate();
				if (num > 16777215)
				{
					m_allocatedMemory.Free(num);
					return default(AllocationRecord);
				}
				ushort num2 = m_refId++;
				if (m_refId >= 255)
				{
					m_refId = 1;
				}
				int allocationId = (num2 << 24) | num;
				m_allocatedMemory[num] = new AllocationInfo
				{
					DebugName = debugName,
					RefId = num2,
					Size = size
				};
				Interlocked.Add(ref m_thisSystemMemory, size);
				InvalidateTotalMemoryCache();
				return new AllocationRecord(allocationId, this);
			}
		}

		private void FreeAllocation(int allocationId)
		{
			int num = allocationId & 0xFFFFFF;
			int num2 = (allocationId >> 24) & 0xFF;
			lock (m_allocatedMemory)
			{
				if (num >= 0 && num < m_allocatedMemory.Capacity)
				{
					AllocationInfo allocationInfo = m_allocatedMemory[num];
					if (allocationInfo.RefId == num2)
					{
						Interlocked.Add(ref m_thisSystemMemory, -allocationInfo.Size);
						InvalidateTotalMemoryCache();
						m_allocatedMemory.Free(num);
					}
				}
			}
		}

		private void InvalidateTotalMemoryCache()
		{
			m_totalMemoryCache = -1L;
			m_parent?.InvalidateTotalMemoryCache();
		}

		private long GetTotalMemory()
		{
			long num = m_totalMemoryCache;
			if (num < 0)
			{
				Thread.MemoryBarrier();
				num = m_thisSystemMemory;
				if (m_subsystems != null)
				{
					lock (m_subsystems)
					{
						foreach (MyMemorySystem subsystem in m_subsystems)
						{
							num += subsystem.GetTotalMemory();
						}
					}
				}
				m_totalMemoryCache = num;
			}
			return num;
		}
	}
}
