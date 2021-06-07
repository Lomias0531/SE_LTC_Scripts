using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VRage.Trace
{
	public static class MyTrace
	{
		public const string TracingSymbol = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";

		private const string WindowName = "SE";

		private static Dictionary<int, ITrace> m_traces;

		private static readonly MyNullTrace m_nullTrace = new MyNullTrace();

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void Init(InitTraceHandler handler)
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void InitWinTrace()
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		[Conditional("DEVELOP")]
		private static void InitInternal(InitTraceHandler handler)
		{
			m_traces = new Dictionary<int, ITrace>();
			string text = "SE";
			foreach (object value in Enum.GetValues(typeof(TraceWindow)))
			{
				string text2 = ((TraceWindow)value == TraceWindow.Default) ? text : (text + "_" + value.ToString());
				m_traces[(int)value] = handler(text2, text2);
			}
		}

		private static ITrace InitWintraceHandler(string traceId, string traceName)
		{
			return MyWintraceWrapper.CreateTrace(traceId, traceName);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void Watch(string name, object value)
		{
			GetTrace(TraceWindow.Default).Watch(name, value);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void Send(TraceWindow window, string msg, string comment = null)
		{
			GetTrace(window).Send(msg, comment);
		}

		public static ITrace GetTrace(TraceWindow window)
		{
			if (m_traces == null || !m_traces.TryGetValue((int)window, out ITrace value))
			{
				return m_nullTrace;
			}
			return value;
		}
	}
}
