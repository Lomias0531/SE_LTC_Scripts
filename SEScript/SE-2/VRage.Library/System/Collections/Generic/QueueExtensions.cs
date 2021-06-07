using System.Collections.Concurrent;
using System.Reflection;

namespace System.Collections.Generic
{
	public static class QueueExtensions
	{
		private static class QueueReflector<T>
		{
			public static Func<ConcurrentQueue<T>, List<T>> ToList;

			static QueueReflector()
			{
				Type typeFromHandle = typeof(ConcurrentQueue<T>);
				MethodInfo method = typeFromHandle.GetMethod("ToList", BindingFlags.Instance | BindingFlags.NonPublic);
				ToList = (Func<ConcurrentQueue<T>, List<T>>)Delegate.CreateDelegate(typeof(Func<ConcurrentQueue<T>, List<T>>), method);
			}
		}

		public static bool TryDequeue<T>(this Queue<T> queue, out T result)
		{
			if (queue.Count > 0)
			{
				result = queue.Dequeue();
				return true;
			}
			result = default(T);
			return false;
		}

		public static bool TryDequeueSync<T>(this Queue<T> queue, out T result)
		{
			lock (((ICollection)queue).SyncRoot)
			{
				return TryDequeue(queue, out result);
			}
		}

		public static List<T> ToList<T>(this ConcurrentQueue<T> queue)
		{
			return QueueReflector<T>.ToList(queue);
		}
	}
}
