using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	[PreloadRequired]
	public class MyClientDebugCommands
	{
		private static readonly char[] m_separators;

		private static readonly Dictionary<string, Action<string[]>> m_commands;

		private static ulong m_commandAuthor;

		static MyClientDebugCommands()
		{
			m_separators = new char[3]
			{
				' ',
				'\r',
				'\n'
			};
			m_commands = new Dictionary<string, Action<string[]>>(StringComparer.InvariantCultureIgnoreCase);
			MethodInfo[] methods = typeof(MyClientDebugCommands).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				DisplayNameAttribute customAttribute = methodInfo.GetCustomAttribute<DisplayNameAttribute>();
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (customAttribute != null && methodInfo.ReturnType == typeof(void) && parameters.Length == 1 && parameters[0].ParameterType == typeof(string[]))
				{
					m_commands[customAttribute.DisplayName] = methodInfo.CreateDelegate<Action<string[]>>();
				}
			}
		}

		public static bool Process(string msg, ulong author)
		{
			m_commandAuthor = author;
			string[] array = msg.Split(m_separators, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length != 0 && m_commands.TryGetValue(array[0], out Action<string[]> value))
			{
				value(array.Skip(1).ToArray());
				return true;
			}
			return false;
		}

		[DisplayName("+stress")]
		private static void StressTest(string[] args)
		{
			if (args.Length > 1)
			{
				if (args[0] == MySession.Static.LocalHumanPlayer.DisplayName || args[0] == "all" || args[0] == "clients")
				{
					if (args.Length > 3)
					{
						MyReplicationClient.StressSleep.X = Convert.ToInt32(args[1]);
						MyReplicationClient.StressSleep.Y = Convert.ToInt32(args[2]);
						MyReplicationClient.StressSleep.Z = Convert.ToInt32(args[3]);
					}
					else if (args.Length > 2)
					{
						MyReplicationClient.StressSleep.X = Convert.ToInt32(args[1]);
						MyReplicationClient.StressSleep.Y = Convert.ToInt32(args[2]);
						MyReplicationClient.StressSleep.Z = 0;
					}
					else
					{
						MyReplicationClient.StressSleep.Y = Convert.ToInt32(args[1]);
						MyReplicationClient.StressSleep.X = MyReplicationClient.StressSleep.Y;
						MyReplicationClient.StressSleep.Z = 0;
					}
				}
			}
			else
			{
				MyReplicationClient.StressSleep.X = 0;
				MyReplicationClient.StressSleep.Y = 0;
			}
		}

		[DisplayName("+vcadd")]
		private static void VirtualClientAdd(string[] args)
		{
			int num = 1;
			if (args.Length == 1)
			{
				num = int.Parse(args[0]);
			}
			int num2 = 0;
			while (num > 0)
			{
				MySession.Static.VirtualClients.Add(num2);
				num--;
				num2++;
			}
		}
	}
}
