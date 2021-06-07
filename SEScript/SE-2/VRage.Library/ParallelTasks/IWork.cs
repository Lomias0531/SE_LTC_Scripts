namespace ParallelTasks
{
	/// <summary>
	/// An interface for a piece of work which can be executed by ParallelTasks.
	/// </summary>
	public interface IWork
	{
		/// <summary>
		/// Gets options specifying how this work may be executed.
		/// </summary>
		WorkOptions Options
		{
			get;
		}

		/// <summary>
		/// Executes the work.
		/// </summary>
		void DoWork(WorkData workData = null);
	}
}
