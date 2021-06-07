using System.Threading;

namespace ParallelTasks
{
	/// <summary>
	/// A semaphore class.
	/// </summary>
	public class Semaphore
	{
		private AutoResetEvent gate;

		private int free;

		private object free_lock = new object();

		/// <summary>
		/// Creates a new instance of the <see cref="T:ParallelTasks.Semaphore" /> class.
		/// </summary>
		/// <param name="maximumCount"></param>
		public Semaphore(int maximumCount)
		{
			free = maximumCount;
			gate = new AutoResetEvent(free > 0);
		}

		/// <summary>
		/// Blocks the calling thread until resources are made available, then consumes one resource.
		/// </summary>
		public void WaitOne()
		{
			gate.WaitOne();
			lock (free_lock)
			{
				free--;
				if (free > 0)
				{
					gate.Set();
				}
			}
		}

		/// <summary>
		/// Adds one resource.
		/// </summary>
		public void Release()
		{
			lock (free_lock)
			{
				free++;
				gate.Set();
			}
		}
	}
}
