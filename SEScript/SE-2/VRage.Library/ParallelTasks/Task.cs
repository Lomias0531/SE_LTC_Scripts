using System;

namespace ParallelTasks
{
	/// <summary>
	/// A struct which represents a single execution of an IWork instance.
	/// </summary>
	public struct Task
	{
		public bool valid;

		internal WorkItem Item
		{
			get;
			private set;
		}

		internal int ID
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a value which indicates if this task has completed.
		/// </summary>
		public bool IsComplete
		{
			get
			{
				if (valid)
				{
					return Item.RunCount != ID;
				}
				return true;
			}
		}

		/// <summary>
		/// Gets an array containing any exceptions thrown by this task.
		/// </summary>
		public Exception[] Exceptions
		{
			get
			{
				if (valid)
				{
					return Item.GetExceptions(ID);
				}
				return null;
			}
		}

		internal Task(WorkItem item)
		{
			this = default(Task);
			ID = item.RunCount;
			Item = item;
			valid = true;
		}

		/// <summary>
		/// Waits for the task to complete.
		/// </summary>
		public void WaitOrExecute(bool blocking = false)
		{
			if (valid)
			{
				AssertNotOperatingOnItself();
				Item.WaitOrExecute(ID, blocking);
			}
		}

		public void Wait(bool blocking = false)
		{
			if (valid)
			{
				AssertNotOperatingOnItself();
				Item.Wait(ID, blocking);
			}
		}

		public void Execute()
		{
			if (valid)
			{
				AssertNotOperatingOnItself();
				Item.Execute(ID);
			}
		}

		internal void DoWork()
		{
			if (valid)
			{
				Item.DoWork(ID);
			}
		}

		private void AssertNotOperatingOnItself()
		{
			Task? currentTask = WorkItem.CurrentTask;
			if (currentTask.HasValue && currentTask.Value.Item == Item && currentTask.Value.ID == ID)
			{
				throw new InvalidOperationException("A task cannot perform this operation on itself.");
			}
		}
	}
}
