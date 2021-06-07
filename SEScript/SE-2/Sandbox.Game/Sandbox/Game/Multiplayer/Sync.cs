using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Game.World;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
	public static class Sync
	{
		private static bool? m_steamOnline;

		private static float m_serverCPULoad;

		private static float m_serverCPULoadSmooth;

		private static float m_serverThreadLoad;

		private static float m_serverThreadLoadSmooth;

		public static bool MultiplayerActive => MyMultiplayer.Static != null;

		public static bool IsServer
		{
			get
			{
				if (MultiplayerActive)
				{
					return MyMultiplayer.Static.IsServer;
				}
				return true;
			}
		}

		public static bool IsValidEventOnServer
		{
			get
			{
				if (MyMultiplayer.Static != null && MyMultiplayer.Static.IsServer)
				{
					return MyEventContext.Current.IsValid;
				}
				return false;
			}
		}

		public static bool IsDedicated => Sandbox.Engine.Platform.Game.IsDedicated;

		public static ulong ServerId
		{
			get
			{
				if (!MultiplayerActive)
				{
					return MyId;
				}
				return MyMultiplayer.Static.ServerId;
			}
		}

		public static MySyncLayer Layer
		{
			get
			{
				if (MySession.Static == null)
				{
					return null;
				}
				return MySession.Static.SyncLayer;
			}
		}

		public static ulong MyId => MyGameService.UserId;

		public static string MyName => MyGameService.UserName;

		public static float ServerSimulationRatio
		{
			get
			{
				MyMultiplayerBase @static = MyMultiplayer.Static;
				if (@static == null || @static.IsServer)
				{
					return MyPhysics.SimulationRatio;
				}
				return @static.ServerSimulationRatio;
			}
			set
			{
				if (MultiplayerActive && !IsServer)
				{
					MyMultiplayer.Static.ServerSimulationRatio = value;
				}
			}
		}

		public static float ServerCPULoad
		{
			get
			{
				MyMultiplayerBase @static = MyMultiplayer.Static;
				if (@static == null || @static.IsServer)
				{
					return MySandboxGame.Static.CPULoad;
				}
				return m_serverCPULoad;
			}
			set
			{
				m_serverCPULoad = value;
			}
		}

		public static float ServerCPULoadSmooth
		{
			get
			{
				MyMultiplayerBase @static = MyMultiplayer.Static;
				if (@static == null || @static.IsServer)
				{
					return MySandboxGame.Static.CPULoadSmooth;
				}
				return m_serverCPULoadSmooth;
			}
			set
			{
				m_serverCPULoadSmooth = MathHelper.Smooth(value, m_serverCPULoadSmooth);
			}
		}

		public static float ServerThreadLoad
		{
			get
			{
				MyMultiplayerBase @static = MyMultiplayer.Static;
				if (@static == null || @static.IsServer)
				{
					return MySandboxGame.Static.ThreadLoad;
				}
				return m_serverThreadLoad;
			}
			set
			{
				m_serverThreadLoad = value;
			}
		}

		public static float ServerThreadLoadSmooth
		{
			get
			{
				MyMultiplayerBase @static = MyMultiplayer.Static;
				if (@static == null || @static.IsServer)
				{
					return MySandboxGame.Static.ThreadLoadSmooth;
				}
				return m_serverThreadLoadSmooth;
			}
			set
			{
				m_serverThreadLoadSmooth = MathHelper.Smooth(value, m_serverThreadLoadSmooth);
			}
		}

		public static MyClientCollection Clients
		{
			get
			{
				if (Layer != null)
				{
					return Layer.Clients;
				}
				return null;
			}
		}

		public static MyPlayerCollection Players => MySession.Static.Players;

		public static bool IsProcessingBufferedMessages => Layer.TransportLayer.IsProcessingBuffer;

		public static bool IsGameServer(this MyNetworkClient client)
		{
			if (client != null)
			{
				return client.SteamUserId == ServerId;
			}
			return false;
		}

		public static void ClientConnected(ulong sender)
		{
			if (Layer != null && Layer.Clients != null && !Layer.Clients.HasClient(sender))
			{
				Layer.Clients.AddClient(sender);
			}
		}
	}
}
