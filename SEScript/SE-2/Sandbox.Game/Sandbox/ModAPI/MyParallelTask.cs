using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI
{
	internal class MyParallelTask : IMyParallelTask
	{
		public static readonly MyParallelTask Static;

		WorkOptions IMyParallelTask.DefaultOptions => Parallel.DefaultOptions;

		static MyParallelTask()
		{
			Static = new MyParallelTask();
		}

		Task IMyParallelTask.StartBackground(IWork work, Action completionCallback)
		{
			return Parallel.StartBackground(work, completionCallback);
		}

		Task IMyParallelTask.StartBackground(IWork work)
		{
			return Parallel.StartBackground(work);
		}

		Task IMyParallelTask.StartBackground(Action action)
		{
			return Parallel.StartBackground(action);
		}

		Task IMyParallelTask.StartBackground(Action action, Action completionCallback)
		{
			return Parallel.StartBackground(action, completionCallback);
		}

		Task IMyParallelTask.StartBackground(Action<WorkData> action, Action<WorkData> completionCallback, WorkData workData)
		{
			return Parallel.StartBackground(action, completionCallback, workData);
		}

		void IMyParallelTask.Do(IWork a, IWork b)
		{
			Parallel.Do(a, b);
		}

		void IMyParallelTask.Do(params IWork[] work)
		{
			Parallel.Do(work);
		}

		void IMyParallelTask.Do(Action action1, Action action2)
		{
			Parallel.Do(action1, action2);
		}

		void IMyParallelTask.Do(params Action[] actions)
		{
			Parallel.Do(actions);
		}

		void IMyParallelTask.For(int startInclusive, int endExclusive, Action<int> body)
		{
			Parallel.For(startInclusive, endExclusive, body);
		}

		void IMyParallelTask.For(int startInclusive, int endExclusive, Action<int> body, int stride)
		{
			Parallel.For(startInclusive, endExclusive, body, stride);
		}

		void IMyParallelTask.ForEach<T>(IEnumerable<T> collection, Action<T> action)
		{
			Parallel.ForEach(collection, action);
		}

		Task IMyParallelTask.Start(Action action, WorkOptions options, Action completionCallback)
		{
			return Parallel.Start(action, options, completionCallback);
		}

		Task IMyParallelTask.Start(Action action, WorkOptions options)
		{
			return Parallel.Start(action, options);
		}

		Task IMyParallelTask.Start(Action action, Action completionCallback)
		{
			return Parallel.Start(action, completionCallback);
		}

		Task IMyParallelTask.Start(Action action)
		{
			return Parallel.Start(action);
		}

		Task IMyParallelTask.Start(IWork work, Action completionCallback)
		{
			return Parallel.Start(work, completionCallback);
		}

		Task IMyParallelTask.Start(IWork work)
		{
			return Parallel.Start(work);
		}

		Task IMyParallelTask.Start(Action<WorkData> action, Action<WorkData> completionCallback, WorkData workData)
		{
			return Parallel.Start(action, completionCallback, workData);
		}

		void IMyParallelTask.Sleep(int millisecondsTimeout)
		{
			if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
			{
				Thread.Sleep(millisecondsTimeout);
			}
		}

		void IMyParallelTask.Sleep(TimeSpan timeout)
		{
			if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
			{
				Thread.Sleep(timeout);
			}
		}
	}
}
