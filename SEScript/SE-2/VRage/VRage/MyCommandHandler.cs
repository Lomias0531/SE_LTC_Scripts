using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRage
{
	internal class MyCommandHandler
	{
		private Dictionary<string, MyCommand> m_commands;

		public MyCommandHandler()
		{
			m_commands = new Dictionary<string, MyCommand>();
		}

		public StringBuilder Handle(string input)
		{
			List<string> list = SplitArgs(input);
			if (list.Count <= 0)
			{
				return new StringBuilder("Error: Empty string");
			}
			string input2 = list[0];
			string commandKey = GetCommandKey(input2);
			if (commandKey == null)
			{
				return new StringBuilder().AppendFormat("Error: Invalid method syntax '{0}'", input);
			}
			list.RemoveAt(0);
			if (m_commands.TryGetValue(commandKey, out MyCommand value))
			{
				string commandMethod = GetCommandMethod(input2);
				if (commandMethod == null)
				{
					return new StringBuilder().AppendFormat("Error: Invalid method syntax '{0}'", input);
				}
				if (commandMethod == "")
				{
					return new StringBuilder("Error: Empty Method");
				}
				try
				{
					return new StringBuilder().Append(commandKey).Append(".").Append(commandMethod)
						.Append(": ")
						.Append((object)value.Execute(commandMethod, list));
				}
				catch (MyConsoleInvalidArgumentsException)
				{
					return new StringBuilder().AppendFormat("Error: Invalid Argument for method {0}.{1}", commandKey, commandMethod);
				}
				catch (MyConsoleMethodNotFoundException)
				{
					return new StringBuilder().AppendFormat("Error: Command {0} does not contain method {1}", commandKey, commandMethod);
				}
			}
			return new StringBuilder().AppendFormat("Error: Unknown command {0}\n", commandKey);
		}

		public List<string> SplitArgs(string input)
		{
			return input.Split(new char[1]
			{
				'"'
			}).Select((string element, int index) => (index % 2 != 0) ? new string[1]
			{
				element
			} : element.Split(new char[1]
			{
				' '
			}, StringSplitOptions.RemoveEmptyEntries)).SelectMany((string[] element) => element)
				.ToList();
		}

		public string GetCommandKey(string input)
		{
			if (!input.Contains("."))
			{
				return null;
			}
			return input.Substring(0, input.IndexOf("."));
		}

		public string GetCommandMethod(string input)
		{
			try
			{
				return input.Substring(input.IndexOf(".") + 1);
			}
			catch
			{
				return null;
			}
		}

		public void AddCommand(MyCommand command)
		{
			if (m_commands.ContainsKey(command.Prefix()))
			{
				m_commands.Remove(command.Prefix());
			}
			m_commands.Add(command.Prefix(), command);
		}

		public void RemoveAllCommands()
		{
			m_commands.Clear();
		}

		public bool ContainsCommand(string command)
		{
			return m_commands.ContainsKey(command);
		}

		public bool TryGetCommand(string commandName, out MyCommand command)
		{
			if (!m_commands.ContainsKey(commandName))
			{
				command = null;
				return false;
			}
			command = m_commands[commandName];
			return true;
		}
	}
}
