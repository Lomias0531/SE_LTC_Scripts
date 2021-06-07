using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Collections;
using VRage.Utils;

namespace VRage.Game.Common
{
	/// <summary>
	/// Auto-debug client.
	/// </summary>
	public class MyExtDebugClient : IDisposable
	{
		public delegate void ReceivedMsgHandler(MyExternalDebugStructures.CommonMsgHeader messageHeader, IntPtr messageData);

		public const int GameDebugPort = 13000;

		private const int MsgSizeLimit = 10240;

		private TcpClient m_client;

		private readonly byte[] m_arrayBuffer = new byte[10240];

		private IntPtr m_tempBuffer;

		private Thread m_clientThread;

		private bool m_finished;

		private readonly ConcurrentCachingList<ReceivedMsgHandler> m_receivedMsgHandlers = new ConcurrentCachingList<ReceivedMsgHandler>();

		public bool ConnectedToGame
		{
			get
			{
				if (m_client != null)
				{
					return m_client.Connected;
				}
				return false;
			}
		}

		public event ReceivedMsgHandler ReceivedMsg
		{
			add
			{
				if (!m_receivedMsgHandlers.Contains(value))
				{
					m_receivedMsgHandlers.Add(value);
					m_receivedMsgHandlers.ApplyAdditions();
				}
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

		public MyExtDebugClient()
		{
			m_tempBuffer = Marshal.AllocHGlobal(10240);
			m_finished = false;
			m_clientThread = new Thread(ClientThreadProc)
			{
				IsBackground = true
			};
			m_clientThread.Start();
		}

		public void Dispose()
		{
			m_finished = true;
			if (m_client != null)
			{
				if (m_client.Client.Connected)
				{
					m_client.Client.Disconnect(reuseSocket: false);
				}
				m_client.Close();
			}
			Marshal.FreeHGlobal(m_tempBuffer);
		}

		private void ClientThreadProc()
		{
			while (!m_finished)
			{
				if (m_client == null || m_client.Client == null || !m_client.Connected)
				{
					try
					{
						m_client = new TcpClient();
						m_client.Connect(IPAddress.Loopback, 13000);
					}
					catch (Exception)
					{
					}
					if (m_client == null || m_client.Client == null || !m_client.Connected)
					{
						Thread.Sleep(2500);
						continue;
					}
				}
				try
				{
					if (m_client.Client != null)
					{
						if (m_client.Client.Receive(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize, SocketFlags.None) == 0)
						{
							m_client.Client.Close();
							m_client.Client = null;
							m_client = null;
						}
						else
						{
							Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer, MyExternalDebugStructures.MsgHeaderSize);
							MyExternalDebugStructures.CommonMsgHeader messageHeader = (MyExternalDebugStructures.CommonMsgHeader)Marshal.PtrToStructure(m_tempBuffer, typeof(MyExternalDebugStructures.CommonMsgHeader));
							if (messageHeader.IsValid)
							{
								m_client.Client.Receive(m_arrayBuffer, messageHeader.MsgSize, SocketFlags.None);
								if (m_receivedMsgHandlers != null)
								{
									Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer, messageHeader.MsgSize);
									foreach (ReceivedMsgHandler receivedMsgHandler in m_receivedMsgHandlers)
									{
										receivedMsgHandler?.Invoke(messageHeader, m_tempBuffer);
									}
								}
							}
						}
					}
				}
				catch (SocketException)
				{
					if (m_client.Client != null)
					{
						m_client.Client.Close();
						m_client.Client = null;
						m_client = null;
					}
				}
				catch (ObjectDisposedException)
				{
					if (m_client.Client != null)
					{
						m_client.Client.Close();
						m_client.Client = null;
						m_client = null;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		public bool SendMessageToGame<TMessage>(TMessage msg) where TMessage : MyExternalDebugStructures.IExternalDebugMsg
		{
			if (m_client == null || m_client.Client == null || !m_client.Connected)
			{
				return false;
			}
			int num = Marshal.SizeOf(typeof(TMessage));
			Marshal.StructureToPtr(MyExternalDebugStructures.CommonMsgHeader.Create(msg.GetTypeStr(), num), m_tempBuffer, fDeleteOld: true);
			Marshal.Copy(m_tempBuffer, m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize);
			Marshal.StructureToPtr(msg, m_tempBuffer, fDeleteOld: true);
			Marshal.Copy(m_tempBuffer, m_arrayBuffer, MyExternalDebugStructures.MsgHeaderSize, num);
			try
			{
				m_client.Client.Send(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize + num, SocketFlags.None);
			}
			catch (SocketException)
			{
				return false;
			}
			return true;
		}
	}
}
