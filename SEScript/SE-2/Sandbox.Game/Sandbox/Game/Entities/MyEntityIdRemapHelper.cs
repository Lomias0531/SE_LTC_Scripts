using System.Collections.Generic;
using VRage;
using VRage.Library.Utils;
using VRage.ModAPI;

namespace Sandbox.Game.Entities
{
	internal class MyEntityIdRemapHelper : IMyRemapHelper
	{
		private static int DEFAULT_REMAPPER_SIZE = 512;

		private Dictionary<long, long> m_oldToNewMap = new Dictionary<long, long>(DEFAULT_REMAPPER_SIZE);

		private Dictionary<string, Dictionary<int, int>> m_groupMap = new Dictionary<string, Dictionary<int, int>>();

		public long RemapEntityId(long oldEntityId)
		{
			if (!m_oldToNewMap.TryGetValue(oldEntityId, out long value))
			{
				value = MyEntityIdentifier.AllocateId();
				m_oldToNewMap.Add(oldEntityId, value);
			}
			return value;
		}

		public int RemapGroupId(string group, int oldValue)
		{
			if (!m_groupMap.TryGetValue(group, out Dictionary<int, int> value))
			{
				value = new Dictionary<int, int>();
				m_groupMap.Add(group, value);
			}
			if (!value.TryGetValue(oldValue, out int value2))
			{
				value2 = MyRandom.Instance.Next();
				value.Add(oldValue, value2);
			}
			return value2;
		}

		public void Clear()
		{
			m_oldToNewMap.Clear();
			m_groupMap.Clear();
		}
	}
}
