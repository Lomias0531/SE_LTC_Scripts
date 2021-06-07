#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using VRage.FileSystem;
using VRage.Library;

namespace VRage.Utils
{
	public class MyLog
	{
		public struct IndentToken : IDisposable
		{
			private MyLog m_log;

			private LoggingOptions m_options;

			internal IndentToken(MyLog log, LoggingOptions options)
			{
				m_log = log;
				m_options = options;
				m_log.IncreaseIndent(options);
			}

			public void Dispose()
			{
				if (m_log != null)
				{
					m_log.DecreaseIndent(m_options);
					m_log = null;
				}
			}
		}

		private struct MyLogIndentKey
		{
			public int ThreadId;

			public int Indent;

			public MyLogIndentKey(int threadId, int indent)
			{
				ThreadId = threadId;
				Indent = indent;
			}
		}

		private struct MyLogIndentValue
		{
			public long LastGcTotalMemory;

			public long LastWorkingSet;

			public DateTimeOffset LastDateTimeOffset;

			public MyLogIndentValue(long lastGcTotalMemory, long lastWorkingSet, DateTimeOffset lastDateTimeOffset)
			{
				LastGcTotalMemory = lastGcTotalMemory;
				LastWorkingSet = lastWorkingSet;
				LastDateTimeOffset = lastDateTimeOffset;
			}
		}

		private bool m_alwaysFlush;

		public static MyLogSeverity AssertLevel = (MyLogSeverity)255;

		private bool LogForMemoryProfiler;

		private bool m_enabled;

		private Stream m_stream;

		private StreamWriter m_streamWriter;

		private readonly FastResourceLock m_lock = new FastResourceLock();

		private Dictionary<int, int> m_indentsByThread;

		private Dictionary<MyLogIndentKey, MyLogIndentValue> m_indents;

		private string m_filepath;

		private StringBuilder m_stringBuilder = new StringBuilder(2048);

		private char[] m_tmpWrite = new char[2048];

		private LoggingOptions m_loggingOptions = LoggingOptions.NONE | LoggingOptions.ENUM_CHECKING | LoggingOptions.LOADING_TEXTURES | LoggingOptions.LOADING_CUSTOM_ASSETS | LoggingOptions.LOADING_SPRITE_VIDEO | LoggingOptions.VALIDATING_CUE_PARAMS | LoggingOptions.CONFIG_ACCESS | LoggingOptions.SIMPLE_NETWORKING | LoggingOptions.VOXEL_MAPS | LoggingOptions.MISC_RENDER_ASSETS | LoggingOptions.AUDIO | LoggingOptions.TRAILERS | LoggingOptions.SESSION_SETTINGS;

		private Action<string> m_normalWriter;

		private Action<string> m_closedLogWriter;

		private static MyLog m_default;

		private readonly FastResourceLock m_consoleStringBuilderLock = new FastResourceLock();

		private StringBuilder m_consoleStringBuilder = new StringBuilder();

		public static MyLog Default
		{
			get
			{
				return m_default;
			}
			set
			{
				m_default = value;
			}
		}

		public LoggingOptions Options
		{
			get
			{
				return m_loggingOptions;
			}
			set
			{
				value = m_loggingOptions;
			}
		}

		public bool LogEnabled => m_enabled;

		public MyLog(bool alwaysFlush = false)
		{
			m_alwaysFlush = alwaysFlush;
		}

		public void Init(string logFileName, StringBuilder appVersionString)
		{
			int num;
			using (m_lock.AcquireExclusiveUsing())
			{
				try
				{
					m_filepath = (Path.IsPathRooted(logFileName) ? logFileName : Path.Combine(MyFileSystem.UserDataPath, logFileName));
					m_stream = MyFileSystem.OpenWrite(m_filepath);
					m_streamWriter = new StreamWriter(m_stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false));
					m_normalWriter = WriteLine;
					m_closedLogWriter = delegate(string s)
					{
						File.AppendAllText(m_filepath, s + MyEnvironment.NewLine);
					};
					m_enabled = true;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Trace.Fail("Cannot create log file: " + ex.ToString());
				}
				m_indentsByThread = new Dictionary<int, int>();
				m_indents = new Dictionary<MyLogIndentKey, MyLogIndentValue>();
				num = (int)Math.Round((DateTime.Now - DateTime.UtcNow).TotalHours);
			}
			WriteLine("Log Started");
			WriteLine($"Timezone (local - UTC): {num}h");
			WriteLineAndConsole("App Version: " + appVersionString);
		}

		public string GetFilePath()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				return m_filepath;
			}
		}

		public IndentToken IndentUsing(LoggingOptions options = LoggingOptions.NONE)
		{
			return new IndentToken(this, options);
		}

		public void IncreaseIndent(LoggingOptions option)
		{
			if (LogFlag(option))
			{
				IncreaseIndent();
			}
		}

		public void IncreaseIndent()
		{
			if (m_enabled)
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					if (m_enabled)
					{
						int threadId = GetThreadId();
						m_indentsByThread[threadId] = GetIdentByThread(threadId) + 1;
						MyLogIndentKey key = new MyLogIndentKey(threadId, m_indentsByThread[threadId]);
						m_indents[key] = new MyLogIndentValue(GetManagedMemory(), GetSystemMemory(), DateTimeOffset.Now);
						if (LogForMemoryProfiler)
						{
							MyMemoryLogs.StartEvent();
						}
					}
				}
			}
		}

		public bool IsIndentKeyIncreased()
		{
			if (!m_enabled)
			{
				return false;
			}
			using (m_lock.AcquireExclusiveUsing())
			{
				if (!m_enabled)
				{
					return false;
				}
				int threadId = GetThreadId();
				MyLogIndentKey key = new MyLogIndentKey(threadId, GetIdentByThread(threadId));
				return m_indents.ContainsKey(key);
			}
		}

		public void DecreaseIndent(LoggingOptions option)
		{
			if (LogFlag(option))
			{
				DecreaseIndent();
			}
		}

		public void DecreaseIndent()
		{
			if (m_enabled)
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					if (!m_enabled)
					{
						return;
					}
					int threadId = GetThreadId();
					MyLogIndentKey key = new MyLogIndentKey(threadId, GetIdentByThread(threadId));
					MyLogIndentValue myLogIndentValue = m_indents[key];
					if (LogForMemoryProfiler)
					{
						MyMemoryLogs.EndEvent(new MyMemoryLogs.MyMemoryEvent
						{
							DeltaTime = (float)(DateTimeOffset.Now - myLogIndentValue.LastDateTimeOffset).TotalMilliseconds / 1000f,
							ManagedEndSize = GetManagedMemory(),
							ProcessEndSize = GetSystemMemory(),
							ManagedStartSize = myLogIndentValue.LastGcTotalMemory,
							ProcessStartSize = myLogIndentValue.LastWorkingSet
						});
					}
				}
				using (m_lock.AcquireExclusiveUsing())
				{
					int threadId2 = GetThreadId();
					m_indentsByThread[threadId2] = GetIdentByThread(threadId2) - 1;
				}
			}
		}

		private string GetFormatedMemorySize(long bytesCount)
		{
			return MyValueFormatter.GetFormatedFloat((float)bytesCount / 1024f / 1024f, 3) + " Mb (" + MyValueFormatter.GetFormatedLong(bytesCount) + " bytes)";
		}

		private long GetManagedMemory()
		{
			return GC.GetTotalMemory(forceFullCollection: false);
		}

		private long GetSystemMemory()
		{
			return MyEnvironment.WorkingSetForMyLog;
		}

		public void Close()
		{
			if (m_enabled)
			{
				WriteLine("Log Closed");
				using (m_lock.AcquireExclusiveUsing())
				{
					if (m_enabled)
					{
						m_streamWriter.Close();
						m_stream.Close();
						m_stream = null;
						m_streamWriter = null;
						m_enabled = false;
					}
				}
			}
		}

		public void AppendToClosedLog(string text)
		{
			if (m_enabled)
			{
				WriteLine(text);
			}
			else if (m_filepath != null)
			{
				File.AppendAllText(m_filepath, text + MyEnvironment.NewLine);
			}
		}

		public void AppendToClosedLog(Exception e)
		{
			if (m_enabled)
			{
				WriteLine(e);
			}
			else if (m_filepath != null)
			{
				WriteLine(m_closedLogWriter, e);
			}
		}

		public bool LogFlag(LoggingOptions option)
		{
			return (m_loggingOptions & option) != 0;
		}

		public void WriteLine(string message, LoggingOptions option)
		{
			if (LogFlag(option))
			{
				WriteLine(message);
			}
		}

		private static void WriteLine(Action<string> writer, Exception ex)
		{
			writer("Exception occured: " + ((ex == null) ? "null" : ex.ToString()));
			if (ex != null && ex is ReflectionTypeLoadException)
			{
				writer("LoaderExceptions: ");
				Exception[] loaderExceptions = ((ReflectionTypeLoadException)ex).LoaderExceptions;
				foreach (Exception ex2 in loaderExceptions)
				{
					WriteLine(writer, ex2);
				}
			}
			if (ex != null && ex.InnerException != null)
			{
				writer("InnerException: ");
				WriteLine(writer, ex.InnerException);
			}
		}

		public void WriteLine(Exception ex)
		{
			if (m_enabled)
			{
				WriteLine(m_normalWriter, ex);
				m_streamWriter.Flush();
			}
		}

		public void WriteLineAndConsole(string msg)
		{
			WriteLine(msg);
			WriteLineToConsole(msg);
		}

		public void WriteLineToConsole(string msg)
		{
			using (m_consoleStringBuilderLock.AcquireExclusiveUsing())
			{
				m_consoleStringBuilder.Clear();
				AppendDateAndTime(m_consoleStringBuilder);
				m_consoleStringBuilder.Append(": ");
				m_consoleStringBuilder.Append(msg);
				MyVRage.Platform?.WriteLineToConsole(m_consoleStringBuilder.ToString());
			}
		}

		public void WriteLine(string msg)
		{
			if (m_enabled)
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					if (m_enabled)
					{
						WriteDateTimeAndThreadId();
						WriteString(msg);
						m_streamWriter.WriteLine();
						if (m_alwaysFlush)
						{
							m_streamWriter.Flush();
						}
					}
				}
			}
			if (LogForMemoryProfiler)
			{
				MyMemoryLogs.AddConsoleLine(msg);
			}
		}

		public void WriteToLogAndAssert(string message)
		{
			WriteLine(message);
		}

		public TextWriter GetTextWriter()
		{
			return m_streamWriter;
		}

		private string GetGCMemoryString(string prependText = "")
		{
			return string.Format("{0}: GC Memory: {1} B", prependText, GetManagedMemory().ToString("##,#"));
		}

		public void WriteMemoryUsage(string prefixText)
		{
			WriteLine(GetGCMemoryString(prefixText));
		}

		public void LogThreadPoolInfo()
		{
			if (m_enabled)
			{
				WriteLine("LogThreadPoolInfo - START");
				IncreaseIndent();
				ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
				WriteLine("GetMaxThreads.WorkerThreads: " + workerThreads);
				WriteLine("GetMaxThreads.CompletionPortThreads: " + completionPortThreads);
				ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
				WriteLine("GetMinThreads.WorkerThreads: " + workerThreads);
				WriteLine("GetMinThreads.CompletionPortThreads: " + completionPortThreads);
				ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
				WriteLine("GetAvailableThreads.WorkerThreads: " + workerThreads);
				WriteLine("GetAvailableThreads.WompletionPortThreads: " + completionPortThreads);
				DecreaseIndent();
				WriteLine("LogThreadPoolInfo - END");
			}
		}

		private void WriteDateTimeAndThreadId()
		{
			m_stringBuilder.Clear();
			AppendDateAndTime(m_stringBuilder);
			m_stringBuilder.Append(" - ");
			m_stringBuilder.Append("Thread: ");
			m_stringBuilder.Concat(GetThreadId(), 3u, ' ');
			m_stringBuilder.Append(" ->  ");
			m_stringBuilder.Append(' ', GetIdentByThread(GetThreadId()) * 3);
			WriteStringBuilder(m_stringBuilder);
		}

		private void AppendDateAndTime(StringBuilder sb)
		{
			DateTimeOffset now = DateTimeOffset.Now;
			sb.Concat(now.Year, 4u, '0', 10u, thousandSeparation: false).Append('-');
			sb.Concat(now.Month, 2u).Append('-');
			sb.Concat(now.Day, 2u).Append(' ');
			sb.Concat(now.Hour, 2u).Append(':');
			sb.Concat(now.Minute, 2u).Append(':');
			sb.Concat(now.Second, 2u).Append('.');
			sb.Concat(now.Millisecond, 3u);
		}

		private void WriteString(string text)
		{
			if (text == null)
			{
				text = "UNKNOWN ERROR: Text is null in MyLog.WriteString()!";
			}
			try
			{
				m_streamWriter.Write(text);
			}
			catch (Exception)
			{
				m_streamWriter.Write("Error: The string is corrupted and cannot be displayed");
			}
		}

		private void WriteStringBuilder(StringBuilder sb)
		{
			if (sb != null && m_tmpWrite != null && m_streamWriter != null)
			{
				if (m_tmpWrite.Length < sb.Length)
				{
					Array.Resize(ref m_tmpWrite, Math.Max(m_tmpWrite.Length * 2, sb.Length));
				}
				sb.CopyTo(0, m_tmpWrite, 0, sb.Length);
				try
				{
					m_streamWriter.Write(m_tmpWrite, 0, sb.Length);
					Array.Clear(m_tmpWrite, 0, sb.Length);
				}
				catch (Exception)
				{
					m_streamWriter.Write("Error: The string is corrupted and cannot be written");
					Array.Clear(m_tmpWrite, 0, m_tmpWrite.Length);
				}
			}
		}

		private int GetThreadId()
		{
			return Thread.CurrentThread.ManagedThreadId;
		}

		private int GetIdentByThread(int threadId)
		{
			if (!m_indentsByThread.TryGetValue(threadId, out int value))
			{
				return 0;
			}
			return value;
		}

		public void Log(MyLogSeverity severity, string format, params object[] args)
		{
			if (m_enabled)
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					if (m_enabled)
					{
						WriteDateTimeAndThreadId();
						StringBuilder stringBuilder = m_stringBuilder;
						stringBuilder.Clear();
						stringBuilder.AppendFormat("{0}: ", severity);
						try
						{
							stringBuilder.AppendFormat(format, args);
						}
						catch
						{
							stringBuilder.Append(format).Append(' ');
							stringBuilder.Append(string.Join(";", args ?? Array.Empty<object>()));
						}
						stringBuilder.Append('\n');
						WriteStringBuilder(stringBuilder);
						if (severity >= MyLogSeverity.Warning)
						{
							MyVRage.Platform.LogToExternalDebugger(stringBuilder.ToString());
						}
						if (severity >= AssertLevel)
						{
							System.Diagnostics.Trace.Fail(stringBuilder.ToString());
						}
					}
				}
			}
		}

		public void Log(MyLogSeverity severity, StringBuilder builder)
		{
			if (m_enabled)
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					if (m_enabled)
					{
						WriteDateTimeAndThreadId();
						StringBuilder stringBuilder = m_stringBuilder;
						stringBuilder.Clear();
						stringBuilder.AppendFormat("{0}: ", severity);
						stringBuilder.AppendStringBuilder(builder);
						stringBuilder.Append('\n');
						WriteStringBuilder(stringBuilder);
						if (severity >= MyLogSeverity.Warning)
						{
							MyVRage.Platform.LogToExternalDebugger(stringBuilder.ToString());
						}
						if (severity >= AssertLevel)
						{
							System.Diagnostics.Trace.Fail(stringBuilder.ToString());
						}
					}
				}
			}
		}

		public void Flush()
		{
			m_streamWriter.Flush();
		}
	}
}
