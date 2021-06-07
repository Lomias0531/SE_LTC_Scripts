using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace VRage.Library.Threading
{
	/// <summary>
	/// Collection of utilities for advanced process and thread management.
	/// </summary>
	public static class ProcessUtil
	{
		/// <summary>
		/// Sets the processor affinity of the current thread.
		/// </summary>
		/// <param name="cpus">A list of CPU numbers. The values should be
		/// between 0 and <see cref="P:System.Environment.ProcessorCount" />.</param>
		public static void SetThreadProcessorAffinity(params int[] cpus)
		{
			if (cpus == null)
			{
				throw new ArgumentNullException("cpus");
			}
			if (cpus.Length == 0)
			{
				throw new ArgumentException("You must specify at least one CPU.", "cpus");
			}
			long num = 0L;
			foreach (int num2 in cpus)
			{
				if (num2 < 0 || num2 >= Environment.ProcessorCount)
				{
					throw new ArgumentException("Invalid CPU number.");
				}
				num |= 1L << num2;
			}
			Thread.BeginThreadAffinity();
			int osThreadId = AppDomain.GetCurrentThreadId();
			ProcessThread processThread = Process.GetCurrentProcess().Threads.Cast<ProcessThread>().Single((ProcessThread t) => t.Id == osThreadId);
			processThread.ProcessorAffinity = new IntPtr(num);
		}
	}
}
