using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Collections
{
	public abstract class MyConcurrentBucketPool
	{
		public static bool EnablePooling = true;

		private static List<MyConcurrentBucketPool> m_poolsForDispose = new List<MyConcurrentBucketPool>();

		public static void OnExit()
		{
			foreach (MyConcurrentBucketPool item in m_poolsForDispose)
			{
				item.DisposeInternal();
			}
			m_poolsForDispose.Clear();
		}

		protected MyConcurrentBucketPool(bool requiresDispose)
		{
			if (requiresDispose)
			{
				m_poolsForDispose.Add(this);
			}
		}

		protected abstract void DisposeInternal();
	}
	/// <summary>
	/// Simple thread-safe pool.
	/// Can store external objects by calling return.
	/// Creates new instances when empty.
	/// </summary>
	public class MyConcurrentBucketPool<T> : MyConcurrentBucketPool where T : class
	{
		private readonly IMyElementAllocator<T> m_allocator;

		private readonly ConcurrentDictionary<int, ConcurrentStack<T>> m_instances;

		private MyBufferStatistics m_statistics;

		public MyConcurrentBucketPool(string debugName, IMyElementAllocator<T> allocator)
			: base(allocator.ExplicitlyDisposeAllElements)
		{
			m_allocator = allocator;
			m_instances = new ConcurrentDictionary<int, ConcurrentStack<T>>();
			m_statistics.Name = debugName;
		}

		public T Get(int bucketId)
		{
			T result = null;
			if (m_instances.TryGetValue(bucketId, out ConcurrentStack<T> value))
			{
				value.TryPop(out result);
			}
			if (result == null)
			{
				result = m_allocator.Allocate(bucketId);
				Interlocked.Increment(ref m_statistics.TotalBuffersAllocated);
				Interlocked.Add(ref m_statistics.TotalBytesAllocated, m_allocator.GetBytes(result));
			}
			int bytes = m_allocator.GetBytes(result);
			Interlocked.Add(ref m_statistics.ActiveBytes, bytes);
			Interlocked.Increment(ref m_statistics.ActiveBuffers);
			m_allocator.Init(result);
			return result;
		}

		public void Return(T instance)
		{
			int bytes = m_allocator.GetBytes(instance);
			int bucketId = m_allocator.GetBucketId(instance);
			Interlocked.Add(ref m_statistics.ActiveBytes, -bytes);
			Interlocked.Decrement(ref m_statistics.ActiveBuffers);
			if (MyConcurrentBucketPool.EnablePooling)
			{
				if (!m_instances.TryGetValue(bucketId, out ConcurrentStack<T> value))
				{
					value = new ConcurrentStack<T>();
					value = m_instances.GetOrAdd(bucketId, value);
				}
				value.Push(instance);
			}
			else
			{
				m_allocator.Dispose(instance);
			}
		}

		public MyBufferStatistics GetReport()
		{
			return m_statistics;
		}

		public void Clear()
		{
			foreach (KeyValuePair<int, ConcurrentStack<T>> instance in m_instances)
			{
				T result;
				while (instance.Value.TryPop(out result))
				{
					m_allocator.Dispose(result);
				}
			}
			m_instances.Clear();
			m_statistics = new MyBufferStatistics
			{
				Name = m_statistics.Name
			};
		}

		protected override void DisposeInternal()
		{
			Clear();
		}
	}
	public class MyConcurrentBucketPool<TElement, TAllocator> : MyConcurrentBucketPool<TElement> where TElement : class where TAllocator : IMyElementAllocator<TElement>, new()
	{
		public MyConcurrentBucketPool(string debugName)
			: base(debugName, (IMyElementAllocator<TElement>)new TAllocator())
		{
		}
	}
}
