using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.World
{
	public class MyNetworkClient : IMyNetworkClient
	{
		private readonly ulong m_steamUserId;

		public ushort ClientFrameId;

		private int m_controlledPlayerSerialId;

		public ulong SteamUserId => m_steamUserId;

		public bool IsLocal
		{
			get;
			private set;
		}

		public string DisplayName
		{
			get;
			private set;
		}

		public int ControlledPlayerSerialId
		{
			private get
			{
				return m_controlledPlayerSerialId;
			}
			set
			{
				if (ControlledPlayerSerialId != value)
				{
					FirstPlayer.ReleaseControls();
					m_controlledPlayerSerialId = value;
					FirstPlayer.AcquireControls();
				}
			}
		}

		public MyPlayer FirstPlayer => GetPlayer(ControlledPlayerSerialId);

		public event Action ClientLeft;

		public MyNetworkClient(ulong steamId)
		{
			m_steamUserId = steamId;
			IsLocal = (Sync.MyId == steamId);
			DisplayName = (MyGameService.IsActive ? MyGameService.GetPersonaName(steamId) : "Client");
		}

		public MyPlayer GetPlayer(int serialId)
		{
			MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId);
			playerId.SteamId = m_steamUserId;
			playerId.SerialId = serialId;
			MyPlayer.PlayerId id = playerId;
			return Sync.Players.GetPlayerById(id);
		}
	}
}
