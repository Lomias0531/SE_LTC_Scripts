using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 2000, typeof(MyObjectBuilder_MySessionComponentDLC), null)]
	public class MySessionComponentDLC : MySessionComponentBase
	{
		protected sealed class RequestUpdateClientDLC_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestUpdateClientDLC();
			}
		}

		private HashSet<uint> m_availableDLCs;

		private Dictionary<ulong, HashSet<uint>> m_clientAvailableDLCs;

		private Dictionary<MyDLCs.MyDLC, int> m_usedUnownedDLCs;

		public DictionaryReader<MyDLCs.MyDLC, int> UsedUnownedDLCs => m_usedUnownedDLCs;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_availableDLCs = new HashSet<uint>();
			m_usedUnownedDLCs = new Dictionary<MyDLCs.MyDLC, int>();
			if (!Sync.IsDedicated)
			{
				UpdateLocalPlayerDLC();
				MyGameService.OnDLCInstalled += OnDLCInstalled;
			}
			if (MyMultiplayer.Static != null && Sync.IsServer)
			{
				m_clientAvailableDLCs = new Dictionary<ulong, HashSet<uint>>();
				MyMultiplayer.Static.ClientJoined += UpdateClientDLC;
			}
		}

		protected override void UnloadData()
		{
			if (!Sync.IsDedicated)
			{
				MyGameService.OnDLCInstalled -= OnDLCInstalled;
			}
			if (MyMultiplayer.Static != null && Sync.IsServer)
			{
				MyMultiplayer.Static.ClientJoined -= UpdateClientDLC;
			}
			base.UnloadData();
		}

		private void UpdateLocalPlayerDLC()
		{
			int dLCCount = MyGameService.GetDLCCount();
			for (int i = 0; i < dLCCount; i++)
			{
				MyGameService.GetDLCDataByIndex(i, out uint dlcId, out bool _, out string _, 128);
				if (MyGameService.IsDlcInstalled(dlcId))
				{
					m_availableDLCs.Add(dlcId);
					if (dlcId == MyFakes.SWITCH_DLC_FROM)
					{
						m_availableDLCs.Add(MyFakes.SWITCH_DLC_TO);
					}
				}
			}
		}

		[Event(null, 88)]
		[Reliable]
		[Server]
		public static void RequestUpdateClientDLC()
		{
			MySession.Static.GetComponent<MySessionComponentDLC>().UpdateClientDLC(MyEventContext.Current.Sender.Value);
		}

		private void UpdateClientDLC(ulong steamId)
		{
			if (!m_clientAvailableDLCs.TryGetValue(steamId, out HashSet<uint> value))
			{
				value = new HashSet<uint>();
				m_clientAvailableDLCs.Add(steamId, value);
			}
			foreach (uint key in MyDLCs.DLCs.Keys)
			{
				if (!Sync.IsDedicated || MyGameService.GameServer.UserHasLicenseForApp(steamId, key))
				{
					value.Add(key);
					if (key == MyFakes.SWITCH_DLC_FROM)
					{
						value.Add(MyFakes.SWITCH_DLC_TO);
					}
				}
			}
		}

		public bool HasDLC(string DLCName, ulong steamId)
		{
			if (MyFakes.OWN_ALL_DLCS)
			{
				return true;
			}
			if (steamId == 0L)
			{
				return false;
			}
			if (steamId == Sync.MyId)
			{
				if (Sync.IsDedicated)
				{
					return false;
				}
				if (MyDLCs.TryGetDLC(DLCName, out MyDLCs.MyDLC dlc))
				{
					return m_availableDLCs.Contains(dlc.AppId);
				}
				return false;
			}
			if (Sync.IsServer)
			{
				if (MyDLCs.TryGetDLC(DLCName, out MyDLCs.MyDLC dlc2))
				{
					return HasClientDLC(dlc2.AppId, steamId);
				}
				return false;
			}
			return false;
		}

		private bool HasClientDLC(uint DLCId, ulong steamId)
		{
			if (!m_clientAvailableDLCs.TryGetValue(steamId, out HashSet<uint> value))
			{
				UpdateClientDLC(steamId);
				value = m_clientAvailableDLCs[steamId];
			}
			return value.Contains(DLCId);
		}

		public bool HasDefinitionDLC(MyDefinitionId definitionId, ulong steamId)
		{
			MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(definitionId);
			return HasDefinitionDLC(definition, steamId);
		}

		public bool HasDefinitionDLC(MyDefinitionBase definition, ulong steamId)
		{
			if (definition.DLCs == null)
			{
				return true;
			}
			string[] dLCs = definition.DLCs;
			foreach (string dLCName in dLCs)
			{
				if (!HasDLC(dLCName, steamId))
				{
					return false;
				}
			}
			return true;
		}

		public MyDLCs.MyDLC GetFirstMissingDefinitionDLC(MyDefinitionBase definition, ulong steamId)
		{
			if (definition.DLCs == null)
			{
				return null;
			}
			string[] dLCs = definition.DLCs;
			foreach (string text in dLCs)
			{
				if (!HasDLC(text, steamId))
				{
					MyDLCs.TryGetDLC(text, out MyDLCs.MyDLC dlc);
					return dlc;
				}
			}
			return null;
		}

		private void OnDLCInstalled(uint dlcId)
		{
			m_availableDLCs.Add(dlcId);
			if (!Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestUpdateClientDLC);
			}
		}

		public void PushUsedUnownedDLC(MyDLCs.MyDLC dlc)
		{
			if (m_usedUnownedDLCs.ContainsKey(dlc))
			{
				m_usedUnownedDLCs[dlc]++;
			}
			else
			{
				m_usedUnownedDLCs[dlc] = 1;
			}
		}

		public void PopUsedUnownedDLC(MyDLCs.MyDLC dlc)
		{
			if (m_usedUnownedDLCs.TryGetValue(dlc, out int value) && value > 0)
			{
				if (value > 1)
				{
					m_usedUnownedDLCs[dlc]--;
				}
				else
				{
					m_usedUnownedDLCs.Remove(dlc);
				}
			}
		}
	}
}
