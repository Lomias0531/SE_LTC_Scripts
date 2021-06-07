using System.Threading;

namespace ParallelTasks
{
	/// <summary>
	/// Implements a singleton instance of type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The type of object to create a singleton instance of.</typeparam>
	public abstract class Singleton<T> where T : class, new()
	{
		private static T instance;

		/// <summary>
		/// Gets a singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					T value = new T();
					Interlocked.CompareExchange(ref instance, value, null);
				}
				return instance;
			}
		}
	}
}
