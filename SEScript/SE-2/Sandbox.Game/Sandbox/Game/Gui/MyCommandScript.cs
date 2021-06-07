using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VRage;

namespace Sandbox.Game.GUI
{
	public class MyCommandScript : MyCommand
	{
		private class MyCommandMethodArgs : MyCommandArgs
		{
			public object[] Args;
		}

		private Type m_type;

		private static StringBuilder m_cache = new StringBuilder();

		public override string Prefix()
		{
			return m_type.Name;
		}

		public MyCommandScript(Type type)
		{
			m_type = type;
			int num = 0;
			MethodInfo[] methods = type.GetMethods();
			foreach (MethodInfo method in methods)
			{
				if (method.IsPublic && method.IsStatic)
				{
					MyCommandAction value = new MyCommandAction
					{
						AutocompleteHint = GetArgsString(method),
						Parser = ((List<string> x) => ParseArgs(x, method)),
						CallAction = ((MyCommandArgs x) => Invoke(x, method))
					};
					m_methods.Add($"{num++}{method.Name}", value);
				}
			}
		}

		private StringBuilder GetArgsString(MethodInfo method)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ParameterInfo[] parameters = method.GetParameters();
			foreach (ParameterInfo parameterInfo in parameters)
			{
				stringBuilder.Append($"{parameterInfo.ParameterType.Name} {parameterInfo.Name}, ");
			}
			return stringBuilder;
		}

		private StringBuilder Invoke(MyCommandArgs x, MethodInfo method)
		{
			m_cache.Clear();
			MyCommandMethodArgs myCommandMethodArgs = x as MyCommandMethodArgs;
			if (myCommandMethodArgs.Args != null)
			{
				m_cache.Append("Success. ");
				object obj = method.Invoke(null, myCommandMethodArgs.Args);
				if (obj != null)
				{
					m_cache.Append(obj.ToString());
				}
			}
			else
			{
				m_cache.Append($"Invoking {method.Name} failed");
			}
			return m_cache;
		}

		private MyCommandArgs ParseArgs(List<string> x, MethodInfo method)
		{
			MyCommandMethodArgs myCommandMethodArgs = new MyCommandMethodArgs();
			ParameterInfo[] parameters = method.GetParameters();
			List<object> list = new List<object>();
			for (int i = 0; i < parameters.Length && i < x.Count; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				MethodInfo method2 = parameterType.GetMethod("TryParse", new Type[2]
				{
					typeof(string),
					parameterType.MakeByRefType()
				});
				if (method2 != null)
				{
					object obj = Activator.CreateInstance(parameterType);
					object[] array = new object[2]
					{
						x[i],
						obj
					};
					method2.Invoke(null, array);
					list.Add(array[1]);
				}
				else
				{
					list.Add(x[i]);
				}
			}
			if (parameters.Length == list.Count)
			{
				myCommandMethodArgs.Args = list.ToArray();
			}
			return myCommandMethodArgs;
		}
	}
}
