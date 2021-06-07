using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VRage.Collections;
using VRage.Library;

namespace ParallelTasks
{
	/// <summary>
	/// A thread safe, non-blocking, object pool.
	/// </summary>
	/// <typeparam name="T">The type of item to store. Must be a class with a parameterless constructor.</typeparam>
	public class Pool<T> : Singleton<Pool<T>> where T : class, new()
	{
		private readonly ConcurrentDictionary<Thread, MyConcurrentQueue<T>> m_instances;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ParallelTasks.Pool`1" /> class.
		/// </summary>
		public Pool()
		{
			m_instances = new ConcurrentDictionary<Thread, MyConcurrentQueue<T>>(MyEnvironment.ProcessorCount, MyEnvironment.ProcessorCount);
		}

		/// <summary>
		/// Gets an instance from the pool.
		/// </summary>
		/// <returns>An instance of <typeparamref name="T" />.</returns>
		public T Get(Thread thread)
		{
			if (!m_instances.TryGetValue(thread, out MyConcurrentQueue<T> value))
			{
				value = new MyConcurrentQueue<T>();
				bool flag = m_instances.TryAdd(thread, value);
			}
			if (!value.TryDequeue(out T instance))
			{
				return new T();
			}
			return instance;
		}

		/// <summary>
		/// Returns an instance to the pool, so it is available for re-use.
		/// It is advised that the item is reset to a default state before being returned.
		/// </summary>
		/// <param name="instance">The instance to return to the pool.</param>
		public void Return(Thread thread, T instance)
		{
			MyConcurrentQueue<T> myConcurrentQueue = m_instances[thread];
			myConcurrentQueue.Enqueue(instance);
		}

		public void Clean()
		{
			foreach (KeyValuePair<Thread, MyConcurrentQueue<T>> instance2 in m_instances)
			{
				instance2.Value.Clear();
			}
		}
	}
}
