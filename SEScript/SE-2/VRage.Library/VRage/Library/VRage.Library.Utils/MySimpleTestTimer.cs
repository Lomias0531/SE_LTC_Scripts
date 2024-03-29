using System;
using System.Diagnostics;
using System.IO;

namespace VRage.Library.Utils
{
	internal struct MySimpleTestTimer : IDisposable
	{
		private string m_name;

		private Stopwatch m_watch;

		public MySimpleTestTimer(string name)
		{
			m_name = name;
			m_watch = new Stopwatch();
			m_watch.Start();
		}

		public void Dispose()
		{
			File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "perf.log"), $"{m_name}: {m_watch.ElapsedMilliseconds:N}ms\n");
		}
	}
}
