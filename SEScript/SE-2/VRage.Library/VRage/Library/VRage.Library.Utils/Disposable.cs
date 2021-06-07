#define TRACE
using System;
using System.Diagnostics;

namespace VRage.Library.Utils
{
	public class Disposable : IDisposable
	{
		public Disposable(bool collectStack = false)
		{
		}

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		~Disposable()
		{
			string message = "Dispose not called!";
			string detailMessage = $"Dispose was not called for '{GetType().FullName}'";
			System.Diagnostics.Trace.Fail(message, detailMessage);
		}
	}
}
