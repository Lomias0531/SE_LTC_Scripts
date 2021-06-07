namespace ParallelTasks
{
	public class WorkData
	{
		public WorkPriority Priority
		{
			get;
			set;
		}

		public WorkData()
		{
			Priority = WorkPriority.Normal;
		}

		public void FlagAsFailed()
		{
		}
	}
}
