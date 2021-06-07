using ParallelTasks;
using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
	public interface IMyParallelTask
	{
		/// <summary>
		/// Default WorkOptions.
		/// DetachFromParent = false, MaximumThreads = 1
		/// </summary>
		WorkOptions DefaultOptions
		{
			get;
		}

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking, work
		/// such as I/O.
		/// </summary>
		/// <param name="work">The work to execute.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="work" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// Invalid number of maximum threads set in <see cref="P:ParallelTasks.IWork.Options" />.
		/// </exception>
		Task StartBackground(IWork work, Action completionCallback);

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking, work
		/// such as I/O.
		/// </summary>
		/// <param name="work">The work to execute.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		Task StartBackground(IWork work);

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking, work
		/// such as I/O.
		/// </summary>
		/// <param name="action">The work to execute.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		Task StartBackground(Action action);

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking, work
		/// such as I/O.
		/// </summary>
		/// <param name="action">The work to execute.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="action" /> is <see langword="null" />.
		/// </exception>
		Task StartBackground(Action action, Action completionCallback);

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking work such as I/O.
		/// </summary>
		/// <param name="action">The work to execute.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <param name="workData">Data to be passed along both the work and the completion callback.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="action" /> is <see langword="null" />.
		/// </exception>
		Task StartBackground(Action<WorkData> action, Action<WorkData> completionCallback, WorkData workData);

		/// <summary>
		/// Executes the given work items potentially in parallel with each other.
		/// This method will block until all work is completed.
		/// </summary>
		/// <param name="a">Work to execute.</param>
		/// <param name="b">Work to execute.</param>
		void Do(IWork a, IWork b);

		/// <summary>
		/// Executes the given work items potentially in parallel with each other.
		/// This method will block until all work is completed.
		/// </summary>
		/// <param name="work">The work to execute.</param>
		void Do(params IWork[] work);

		/// <summary>
		/// Executes the given work items potentially in parallel with each other.
		/// This method will block until all work is completed.
		/// </summary>
		/// <param name="action1">The work to execute.</param>
		/// <param name="action2">The work to execute.</param>
		void Do(Action action1, Action action2);

		/// <summary>
		/// Executes the given work items potentially in parallel with each other.
		/// This method will block until all work is completed.
		/// </summary>
		/// <param name="actions">The work to execute.</param>
		void Do(params Action[] actions);

		/// <summary>
		/// Executes a for loop, where each iteration can potentially occur in parallel with others.
		/// </summary>
		/// <param name="startInclusive">The index (inclusive) at which to start iterating.</param>
		/// <param name="endExclusive">The index (exclusive) at which to end iterating.</param>
		/// <param name="body">The method to execute at each iteration. The current index is supplied as the parameter.</param>
		void For(int startInclusive, int endExclusive, Action<int> body);

		/// <summary>
		/// Executes a for loop, where each iteration can potentially occur in parallel with others.
		/// </summary>
		/// <param name="startInclusive">The index (inclusive) at which to start iterating.</param>
		/// <param name="endExclusive">The index (exclusive) at which to end iterating.</param>
		/// <param name="body">The method to execute at each iteration. The current index is supplied as the parameter.</param>
		/// <param name="stride">The number of iterations that each processor takes at a time.</param>
		void For(int startInclusive, int endExclusive, Action<int> body, int stride);

		/// <summary>
		/// Executes a foreach loop, where each iteration can potentially occur in parallel with others.
		/// </summary>
		/// <typeparam name="T">The type of item to iterate over.</typeparam>
		/// <param name="collection">The enumerable data source.</param>
		/// <param name="action">The method to execute at each iteration. The item to process is supplied as the parameter.</param>
		void ForEach<T>(IEnumerable<T> collection, Action<T> action);

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="options">The work options to use with this action.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		Task Start(Action action, WorkOptions options, Action completionCallback);

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="options">The work options to use with this action.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		Task Start(Action action, WorkOptions options);

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		Task Start(Action action, Action completionCallback);

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		Task Start(Action action);

		/// <summary>
		/// Creates and schedules a task to execute the given work with the given work data.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <param name="workData">Data to be passed along both the work and the completion callback.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		Task Start(Action<WorkData> action, Action<WorkData> completionCallback, WorkData workData);

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="work">The work to execute in parallel.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="work" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// Invalid number of maximum threads set in <see cref="P:ParallelTasks.IWork.Options" />.
		/// </exception>
		Task Start(IWork work, Action completionCallback);

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="work">The work to execute in parallel.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		Task Start(IWork work);

		/// <summary>
		/// Suspends the current thread for the specified number of milliseconds.
		/// </summary>
		/// <param name="millisecondsTimeout"></param>
		void Sleep(int millisecondsTimeout);

		/// <summary>
		/// Suspends the current thread for the specified amount of time.
		/// </summary>
		/// <param name="timeout"></param>
		void Sleep(TimeSpan timeout);
	}
}
