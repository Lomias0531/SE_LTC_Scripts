using ParallelTasks;

namespace Sandbox.Game.GUI
{
	public interface IMyAsyncResult
	{
		bool IsCompleted
		{
			get;
		}

		Task Task
		{
			get;
		}
	}
}
