using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	public static class MyDefinitionErrors
	{
		public class Error
		{
			public string ModName;

			public string ErrorFile;

			public string Message;

			public TErrorSeverity Severity;

			private static Color[] severityColors = new Color[4]
			{
				Color.Gray,
				Color.Gray,
				Color.White,
				new Color(1f, 0.25f, 0.1f)
			};

			private static string[] severityName = new string[4]
			{
				"notice",
				"warning",
				"error",
				"critical error"
			};

			private static string[] severityNamePlural = new string[4]
			{
				"notices",
				"warnings",
				"errors",
				"critical errors"
			};

			public string ErrorId
			{
				get
				{
					if (ModName != null)
					{
						return "mod_";
					}
					return "definition_";
				}
			}

			public string ErrorSeverity
			{
				get
				{
					string text = ErrorId;
					switch (Severity)
					{
					case TErrorSeverity.Critical:
						text = (text + "critical_error").ToUpperInvariant();
						break;
					case TErrorSeverity.Error:
						text = (text + "error").ToUpperInvariant();
						break;
					case TErrorSeverity.Warning:
						text += "warning";
						break;
					case TErrorSeverity.Notice:
						text += "notice";
						break;
					}
					return text;
				}
			}

			public override string ToString()
			{
				return $"{ErrorSeverity}: {ModName ?? string.Empty}, in file: {ErrorFile}\n{Message}";
			}

			public static Color GetSeverityColor(TErrorSeverity severity)
			{
				try
				{
					return severityColors[(int)severity];
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine($"Error type does not have color assigned: message: {ex.Message}, stack:{ex.StackTrace}");
					return Color.White;
				}
			}

			public static string GetSeverityName(TErrorSeverity severity, bool plural)
			{
				try
				{
					if (plural)
					{
						return severityNamePlural[(int)severity];
					}
					return severityName[(int)severity];
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine($"Error type does not have name assigned: message: {ex.Message}, stack:{ex.StackTrace}");
					return plural ? "Errors" : "Error";
				}
			}

			public Color GetSeverityColor()
			{
				return GetSeverityColor(Severity);
			}
		}

		public class ErrorComparer : IComparer<Error>
		{
			public int Compare(Error x, Error y)
			{
				return y.Severity - x.Severity;
			}
		}

		private static readonly object m_lockObject = new object();

		private static readonly List<Error> m_errors = new List<Error>();

		private static readonly ErrorComparer m_comparer = new ErrorComparer();

		public static bool ShouldShowModErrors
		{
			get;
			set;
		}

		public static void Clear()
		{
			lock (m_lockObject)
			{
				m_errors.Clear();
			}
		}

		public static void Add(MyModContext context, string message, TErrorSeverity severity, bool writeToLog = true)
		{
			Error error = new Error
			{
				ModName = context.ModName,
				ErrorFile = context.CurrentFile,
				Message = message,
				Severity = severity
			};
			lock (m_lockObject)
			{
				m_errors.Add(error);
			}
			_ = context.ModName;
			if (writeToLog)
			{
				WriteError(error);
			}
			if (severity == TErrorSeverity.Critical)
			{
				ShouldShowModErrors = true;
			}
		}

		public static ListReader<Error> GetErrors()
		{
			lock (m_lockObject)
			{
				m_errors.Sort(m_comparer);
				return new ListReader<Error>(m_errors);
			}
		}

		public static void WriteError(Error e)
		{
			MyLog.Default.WriteLine($"{e.ErrorSeverity}: {e.ModName ?? string.Empty}");
			MyLog.Default.WriteLine("  in file: " + e.ErrorFile);
			MyLog.Default.WriteLine("  " + e.Message);
		}
	}
}
