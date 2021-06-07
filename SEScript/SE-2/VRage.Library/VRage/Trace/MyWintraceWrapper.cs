using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using VRage.FileSystem;

namespace VRage.Trace
{
	internal class MyWintraceWrapper : ITrace
	{
		private static readonly Type m_winTraceType;

		private static readonly object m_winWatches;

		private readonly object m_trace;

		private readonly Action m_clearAll;

		private readonly Action<string, object> m_send;

		private readonly Action<string, string> m_debugSend;

		static MyWintraceWrapper()
		{
			Assembly assembly = TryLoad("TraceTool.dll") ?? TryLoad(MyFileSystem.ExePath + "/../../../../../../3rd/TraceTool/TraceTool.dll") ?? TryLoad(MyFileSystem.ExePath + "/../../../3rd/TraceTool/TraceTool.dll");
			if (assembly != null)
			{
				m_winTraceType = assembly.GetType("TraceTool.WinTrace");
				Type type = assembly.GetType("TraceTool.TTrace");
				m_winWatches = type.GetProperty("Watches").GetGetMethod().Invoke(null, new object[0]);
			}
		}

		private static Assembly TryLoad(string assembly)
		{
			if (!File.Exists(assembly))
			{
				return null;
			}
			try
			{
				return Assembly.LoadFrom(assembly);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static ITrace CreateTrace(string id, string name)
		{
			if (m_winTraceType != null)
			{
				return new MyWintraceWrapper(Activator.CreateInstance(m_winTraceType, id, name));
			}
			return new MyNullTrace();
		}

		private MyWintraceWrapper(object trace)
		{
			m_trace = trace;
			m_clearAll = Expression.Lambda<Action>(Expression.Call(Expression.Constant(m_trace), trace.GetType().GetMethod("ClearAll")), Array.Empty<ParameterExpression>()).Compile();
			m_clearAll();
			ParameterExpression parameterExpression = Expression.Parameter(typeof(string));
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
			MethodCallExpression body = Expression.Call(Expression.Constant(m_winWatches), m_winWatches.GetType().GetMethod("Send"), parameterExpression, parameterExpression2);
			m_send = Expression.Lambda<Action<string, object>>(body, new ParameterExpression[2]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(string));
			ParameterExpression parameterExpression4 = Expression.Parameter(typeof(string));
			MemberExpression memberExpression = Expression.PropertyOrField(Expression.Constant(m_trace), "Debug");
			MethodCallExpression body2 = Expression.Call(memberExpression, memberExpression.Expression.Type.GetMethod("Send", new Type[2]
			{
				typeof(string),
				typeof(string)
			}), parameterExpression3, parameterExpression4);
			m_debugSend = Expression.Lambda<Action<string, string>>(body2, new ParameterExpression[2]
			{
				parameterExpression3,
				parameterExpression4
			}).Compile();
		}

		public void Send(string msg, string comment = null)
		{
			try
			{
				m_debugSend(msg, comment);
			}
			catch
			{
			}
		}

		public void Watch(string name, object value)
		{
			try
			{
				m_send(name, value);
			}
			catch
			{
			}
		}
	}
}
