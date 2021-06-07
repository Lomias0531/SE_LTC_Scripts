using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Utils;

namespace Sandbox.Game.Multiplayer
{
	public class MyPerPlayerData
	{
		private Dictionary<MyPlayer.PlayerId, Dictionary<MyStringId, object>> m_playerDataByPlayerId;

		public MyPerPlayerData()
		{
			m_playerDataByPlayerId = new Dictionary<MyPlayer.PlayerId, Dictionary<MyStringId, object>>(MyPlayer.PlayerId.Comparer);
		}

		public void SetPlayerData<T>(MyPlayer.PlayerId playerId, MyStringId dataId, T data)
		{
			GetOrAllocatePlayerDataDictionary(playerId)[dataId] = data;
		}

		public T GetPlayerData<T>(MyPlayer.PlayerId playerId, MyStringId dataId, T defaultValue)
		{
			Dictionary<MyStringId, object> value = null;
			if (!m_playerDataByPlayerId.TryGetValue(playerId, out value))
			{
				return defaultValue;
			}
			object value2 = null;
			if (!value.TryGetValue(dataId, out value2))
			{
				return defaultValue;
			}
			return (T)value2;
		}

		private Dictionary<MyStringId, object> GetOrAllocatePlayerDataDictionary(MyPlayer.PlayerId playerId)
		{
			Dictionary<MyStringId, object> value = null;
			if (!m_playerDataByPlayerId.TryGetValue(playerId, out value))
			{
				value = new Dictionary<MyStringId, object>(MyStringId.Comparer);
				m_playerDataByPlayerId[playerId] = value;
			}
			return value;
		}
	}
}
