using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Utils;

namespace VRage.Game.SessionComponents
{
	/// <summary>
	/// Communication between game and editor.
	/// </summary>
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MySessionComponentExtDebug : MySessionComponentBase
	{
		private class MyDebugClientInfo
		{
			public TcpClient TcpClient;

			public MyExternalDebugStructures.CommonMsgHeader LastHeader;
		}

		public delegate void ReceivedMsgHandler(MyExternalDebugStructures.CommonMsgHeader messageHeader, IntPtr messageData);

		public static MySessionComponentExtDebug Static;

		public static bool ForceDisable;

		public const int GameDebugPort = 13000;

		private const int MsgSizeLimit = 10240;

		private Thread m_listenerThread;

		private TcpListener m_listener;

		private ConcurrentCachingList<MyDebugClientInfo> m_clients = new ConcurrentCachingList<MyDebugClientInfo>(1);

		private bool m_active;

		private byte[] m_arrayBuffer = new byte[10240];

		private IntPtr m_tempBuffer;

		private ConcurrentCachingList<ReceivedMsgHandler> m_receivedMsgHandlers = new ConcurrentCachingList<ReceivedMsgHandler>();

		public bool HasClients => m_clients.Count > 0;

		public event ReceivedMsgHandler ReceivedMsg
		{
			add
			{
				m_receivedMsgHandlers.Add(value);
				m_receivedMsgHandlers.ApplyAdditions();
			}
			remove
			{
				if (m_receivedMsgHandlers.Contains(value))
				{
					m_receivedMsgHandlers.Remove(value);
					m_receivedMsgHandlers.ApplyRemovals();
				}
			}
		}

		public bool IsHandlerRegistered(ReceivedMsgHandler handler)
		{
			return m_receivedMsgHandlers.Contains(handler);
		}

		public override void LoadData()
		{
			if (Static != null)
			{
				m_listenerThread = Static.m_listenerThread;
				m_listener = Static.m_listener;
				m_clients = Static.m_clients;
				m_active = Static.m_active;
				m_arrayBuffer = Static.m_arrayBuffer;
				m_tempBuffer = Static.m_tempBuffer;
				m_receivedMsgHandlers = Static.m_receivedMsgHandlers;
				Static = this;
				base.LoadData();
			}
			else
			{
				Static = this;
				if (m_tempBuffer == IntPtr.Zero)
				{
					m_tempBuffer = Marshal.AllocHGlobal(10240);
				}
				if (!ForceDisable && MyVRage.Platform.IsRemoteDebuggingSupported)
				{
					StartServer();
				}
				base.LoadData();
			}
		}

		protected override void UnloadData()
		{
			m_receivedMsgHandlers.ClearImmediate();
			base.UnloadData();
		}

		public void Dispose()
		{
			m_receivedMsgHandlers.ClearList();
			if (m_active)
			{
				StopServer();
			}
			if (m_tempBuffer != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(m_tempBuffer);
			}
		}

		/// <summary>
		/// Start using this component as server (game side).
		/// </summary>
		private bool StartServer()
		{
			if (!m_active)
			{
				m_listenerThread = new Thread(ServerListenerProc)
				{
					IsBackground = true
				};
				m_listenerThread.Start();
				m_active = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Stop the server on the game side. Called automatically.
		/// </summary>
		private void StopServer()
		{
			if (m_active && m_listenerThread != null)
			{
				m_listener.Stop();
				foreach (MyDebugClientInfo client in m_clients)
				{
					if (client.TcpClient != null)
					{
						client.TcpClient.Client.Disconnect(reuseSocket: true);
						client.TcpClient.Close();
					}
				}
				m_clients.ClearImmediate();
				m_active = false;
			}
		}

		/// <summary>
		/// Parallel thread - listener.
		/// </summary>
		private void ServerListenerProc()
		{
			Thread.CurrentThread.Name = "External Debugging Listener";
			try
			{
				m_listener = new TcpListener(IPAddress.Loopback, 13000)
				{
					ExclusiveAddressUse = false
				};
				m_listener.Start();
			}
			catch (SocketException ex)
			{
				MyLog.Default.WriteLine("Cannot start debug listener.");
				MyLog.Default.WriteLine(ex);
				m_listener = null;
				m_active = false;
				return;
			}
			MyLog.Default.WriteLine("External debugger: listening...");
			while (true)
			{
				try
				{
					TcpClient tcpClient = m_listener.AcceptTcpClient();
					tcpClient.Client.Blocking = true;
					MyLog.Default.WriteLine("External debugger: accepted client.");
					m_clients.Add(new MyDebugClientInfo
					{
						TcpClient = tcpClient,
						LastHeader = MyExternalDebugStructures.CommonMsgHeader.Create("UNKNOWN")
					});
					m_clients.ApplyAdditions();
				}
				catch (SocketException ex2)
				{
					if (ex2.SocketErrorCode != SocketError.Interrupted)
					{
						if (MyLog.Default != null && MyLog.Default.LogEnabled)
						{
							MyLog.Default.WriteLine(ex2);
						}
						break;
					}
					m_listener.Stop();
					m_listener = null;
					MyLog.Default.WriteLine("External debugger: interrupted.");
					return;
				}
			}
			m_listener.Stop();
			m_listener = null;
		}

		public override void UpdateBeforeSimulation()
		{
			foreach (MyDebugClientInfo client in m_clients)
			{
				if (client == null || client.TcpClient == null || client.TcpClient.Client == null || !client.TcpClient.Connected)
				{
					if (client != null && client.TcpClient != null && client.TcpClient.Client != null && client.TcpClient.Client.Connected)
					{
						client.TcpClient.Client.Disconnect(reuseSocket: true);
						client.TcpClient.Close();
					}
					m_clients.Remove(client);
				}
				else if (client.TcpClient.Connected && client.TcpClient.Available > 0)
				{
					ReadMessagesFromClients(client);
				}
			}
			m_clients.ApplyRemovals();
		}

		private void ReadMessagesFromClients(MyDebugClientInfo clientInfo)
		{
			Socket client = clientInfo.TcpClient.Client;
			while (client.Available >= 0)
			{
				bool flag = false;
				if (!clientInfo.LastHeader.IsValid && client.Available >= MyExternalDebugStructures.MsgHeaderSize)
				{
					client.Receive(m_arrayBuffer, MyExternalDebugStructures.MsgHeaderSize, SocketFlags.None);
					Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer, MyExternalDebugStructures.MsgHeaderSize);
					clientInfo.LastHeader = (MyExternalDebugStructures.CommonMsgHeader)Marshal.PtrToStructure(m_tempBuffer, typeof(MyExternalDebugStructures.CommonMsgHeader));
					flag = true;
				}
				if (clientInfo.LastHeader.IsValid && client.Available >= clientInfo.LastHeader.MsgSize)
				{
					client.Receive(m_arrayBuffer, clientInfo.LastHeader.MsgSize, SocketFlags.None);
					if (m_receivedMsgHandlers != null && m_receivedMsgHandlers.Count > 0)
					{
						Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer, clientInfo.LastHeader.MsgSize);
						foreach (ReceivedMsgHandler receivedMsgHandler in m_receivedMsgHandlers)
						{
							receivedMsgHandler?.Invoke(clientInfo.LastHeader, m_tempBuffer);
						}
					}
					clientInfo.LastHeader = default(MyExternalDebugStructures.CommonMsgHeader);
					flag = true;
				}
				if (!flag)
				{
					break;
				}
			}
		}

		public bool SendMessageToClients<TMessage>(TMessage msg) where TMessage : struct, MyExternalDebugStructures.IExternalDebugMsg
		{
			int num = Marshal.SizeOf(typeof(TMessage));
			Marshal.StructureToPtr(MyExternalDebugStructures.CommonMsgHeader.Create(msg.GetTypeStr(), num), m_tempBuffer, fDeleteOld: true);
			Marshal.Copy(m_tempBuffer, m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize);
			Marshal.StructureToPtr(msg, m_tempBuffer, fDeleteOld: true);
			Marshal.Copy(m_tempBuffer, m_arrayBuffer, MyExternalDebugStructures.MsgHeaderSize, num);
			foreach (MyDebugClientInfo client in m_clients)
			{
				try
				{
					if (client.TcpClient.Client != null)
					{
						client.TcpClient.Client.Send(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize + num, SocketFlags.None);
					}
				}
				catch (SocketException)
				{
					client.TcpClient.Close();
				}
			}
			return true;
		}
	}
}
