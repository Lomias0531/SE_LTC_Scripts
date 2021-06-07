using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Utils;

namespace Sandbox.Game.Multiplayer
{
	public class MyClientCollection
	{
		private readonly Dictionary<ulong, MyNetworkClient> m_clients = new Dictionary<ulong, MyNetworkClient>();

		private HashSet<ulong> m_disconnectedClients = new HashSet<ulong>();

		private ulong m_localSteamId;

		public Action<ulong> ClientAdded;

		public Action<ulong> ClientRemoved;

		public int Count => m_clients.Count;

		public MyNetworkClient LocalClient
		{
			get
			{
				MyNetworkClient value = null;
				m_clients.TryGetValue(m_localSteamId, out value);
				return value;
			}
		}

		public void SetLocalSteamId(ulong localSteamId, bool createLocalClient = false)
		{
			m_localSteamId = localSteamId;
			if (createLocalClient && !m_clients.ContainsKey(m_localSteamId))
			{
				AddClient(m_localSteamId);
			}
		}

		public void Clear()
		{
			m_clients.Clear();
			m_disconnectedClients.Clear();
		}

		public bool TryGetClient(ulong steamId, out MyNetworkClient client)
		{
			client = null;
			return m_clients.TryGetValue(steamId, out client);
		}

		public bool HasClient(ulong steamId)
		{
			return m_clients.ContainsKey(steamId);
		}

		public MyNetworkClient AddClient(ulong steamId)
		{
			if (m_clients.ContainsKey(steamId))
			{
				MyLog.Default.WriteLine("ERROR: Added client already present: " + m_clients[steamId].DisplayName);
				return m_clients[steamId];
			}
			MyNetworkClient myNetworkClient = new MyNetworkClient(steamId);
			m_clients.Add(steamId, myNetworkClient);
			m_disconnectedClients.Remove(steamId);
			RaiseClientAdded(steamId);
			return myNetworkClient;
		}

		public void RemoveClient(ulong steamId)
		{
			m_clients.TryGetValue(steamId, out MyNetworkClient value);
			if (value == null)
			{
				if (!m_disconnectedClients.Contains(steamId))
				{
					MyLog.Default.WriteLine("ERROR: Removed client not present: " + steamId);
				}
			}
			else
			{
				m_clients.Remove(steamId);
				m_disconnectedClients.Add(steamId);
				RaiseClientRemoved(steamId);
			}
		}

		private void RaiseClientAdded(ulong steamId)
		{
			ClientAdded?.Invoke(steamId);
		}

		private void RaiseClientRemoved(ulong steamId)
		{
			ClientRemoved?.Invoke(steamId);
		}

		public Dictionary<ulong, MyNetworkClient>.ValueCollection GetClients()
		{
			return m_clients.Values;
		}
	}
}
