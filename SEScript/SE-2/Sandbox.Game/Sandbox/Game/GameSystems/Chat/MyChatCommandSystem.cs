using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using VRage.Utils;

namespace Sandbox.Game.GameSystems.Chat
{
	public class MyChatCommandSystem
	{
		public delegate void HandleCommandDelegate(string command, string body, List<IMyChatCommand> executableCommands);

		public Dictionary<string, IMyChatCommand> ChatCommands = new Dictionary<string, IMyChatCommand>();

		private static char[] m_separators = new char[3]
		{
			' ',
			'\r',
			'\n'
		};

		public event HandleCommandDelegate OnUnhandledCommand;

		public MyChatCommandSystem()
		{
			ScanAssemblyForCommands(Assembly.GetExecutingAssembly());
		}

		public void Init()
		{
			MyChatCommands.PreloadCommands(this);
		}

		public void Unload()
		{
			this.OnUnhandledCommand = null;
		}

		public void ScanAssemblyForCommands(Assembly assembly)
		{
			foreach (TypeInfo definedType in assembly.DefinedTypes)
			{
				if (definedType.ImplementedInterfaces.Contains(typeof(IMyChatCommand)))
				{
					if (!(definedType == typeof(MyChatCommand)))
					{
						IMyChatCommand myChatCommand = (IMyChatCommand)Activator.CreateInstance(definedType);
						ChatCommands.Add(myChatCommand.CommandText, myChatCommand);
					}
				}
				else
				{
					MethodInfo[] methods = definedType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (MethodInfo methodInfo in methods)
					{
						ChatCommandAttribute customAttribute = methodInfo.GetCustomAttribute<ChatCommandAttribute>();
						if (customAttribute != null && !customAttribute.DebugCommand)
						{
							Action<string[]> action = methodInfo.CreateDelegate<Action<string[]>>();
							if (action == null)
							{
								MyLog.Default.WriteLine("Error creating delegate from " + definedType.FullName + "." + methodInfo.Name);
							}
							else
							{
								ChatCommands.Add(customAttribute.CommandText, new MyChatCommand(customAttribute.CommandText, customAttribute.HelpText, customAttribute.HelpSimpleText, action));
							}
						}
					}
				}
			}
		}

		public bool CanHandle(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				return false;
			}
			string[] array = message.Split(m_separators, 2, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 0)
			{
				return false;
			}
			if (ChatCommands.ContainsKey(array[0]))
			{
				return true;
			}
			List<IMyChatCommand> list = new List<IMyChatCommand>();
			this.OnUnhandledCommand?.Invoke(array[0], (array.Length > 1) ? array[1] : "", list);
			if (list.Count > 0)
			{
				return true;
			}
			return false;
		}

		public void Handle(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}
			string[] array = message.Split(m_separators, 2, StringSplitOptions.RemoveEmptyEntries);
			if (!ChatCommands.TryGetValue(array[0], out IMyChatCommand value))
			{
				List<IMyChatCommand> list = new List<IMyChatCommand>();
				this.OnUnhandledCommand?.Invoke(array[0], (array.Length > 1) ? array[1] : "", list);
				if (list.Count != 0)
				{
					foreach (IMyChatCommand item in list)
					{
						string[] args = ParseCommand(item, message);
						item.Handle(args);
					}
				}
			}
			else
			{
				string[] args2 = ParseCommand(value, message);
				value.Handle(args2);
			}
		}

		public static string[] ParseCommand(IMyChatCommand command, string input)
		{
			if (input.Length > command.CommandText.Length + 1)
			{
				string input2 = input.Substring(command.CommandText.Length + 1);
				MatchCollection matchCollection = Regex.Matches(input2, "(\"[^\"]+\"|\\S+)");
				string[] array = new string[matchCollection.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = matchCollection[i].Value;
				}
				return array;
			}
			return null;
		}
	}
}
