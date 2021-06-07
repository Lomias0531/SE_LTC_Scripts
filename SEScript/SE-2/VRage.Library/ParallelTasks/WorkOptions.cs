using VRage.Profiler;

namespace ParallelTasks
{
	/// <summary>
	/// A struct containing options about how an IWork instance can be executed.
	/// </summary>
	public struct WorkOptions
	{
		/// <summary>
		/// Gets or sets the maximum number of threads which can concurrently execute this work.
		/// </summary>
		public int MaximumThreads
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating that this work should be queued in a first in first out fashion.
		/// </summary>
		public bool QueueFIFO
		{
			get;
			set;
		}

		public string DebugName
		{
			get;
			set;
		}

		public MyProfiler.TaskType TaskType
		{
			get;
			set;
		}

		public WorkOptions WithDebugInfo(MyProfiler.TaskType taskType, string debugName = null)
		{
			WorkOptions result = this;
			result.TaskType = taskType;
			result.DebugName = debugName;
			return result;
		}
	}
}
