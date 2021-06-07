using System;
using System.Collections.Generic;
using System.Linq;

namespace VRage.Utils
{
	public class MyEventArgs : EventArgs
	{
		private Dictionary<MyStringId, object> m_args = new Dictionary<MyStringId, object>(MyStringId.Comparer);

		public Dictionary<MyStringId, object>.KeyCollection ArgNames => m_args.Keys;

		public MyEventArgs()
		{
		}

		public MyEventArgs(KeyValuePair<MyStringId, object> arg)
		{
			SetArg(arg.Key, arg.Value);
		}

		public MyEventArgs(KeyValuePair<MyStringId, object>[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				KeyValuePair<MyStringId, object> keyValuePair = args[i];
				SetArg(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public object GetArg(MyStringId argName)
		{
			if (!ArgNames.Contains(argName))
			{
				return null;
			}
			return m_args[argName];
		}

		public void SetArg(MyStringId argName, object value)
		{
			m_args.Remove(argName);
			m_args.Add(argName, value);
		}
	}
}
