using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
	internal static class MyNetworkReader
	{
		private class ChannelInfo
		{
			public MyReceiveQueue Queue;

			public NetworkMessageDelegate Handler;
		}

		private static int m_byteCountReceived;

		private static int m_tamperred;

		private static readonly ConcurrentDictionary<int, ChannelInfo> m_channels = new ConcurrentDictionary<int, ChannelInfo>();

		public static void SetHandler(int channel, NetworkMessageDelegate handler, Action<ulong> disconnectPeerOnError)
		{
			if (m_channels.TryGetValue(channel, out ChannelInfo value))
			{
				value.Queue.Dispose();
			}
			value = new ChannelInfo
			{
				Handler = handler,
				Queue = new MyReceiveQueue(channel, disconnectPeerOnError)
			};
			m_channels[channel] = value;
		}

		public static void ClearHandler(int channel)
		{
			if (m_channels.TryGetValue(channel, out ChannelInfo value))
			{
				value.Queue.Dispose();
			}
			m_channels.Remove(channel);
		}

		public static void Clear()
		{
			foreach (KeyValuePair<int, ChannelInfo> channel in m_channels)
			{
				channel.Value.Queue.Dispose();
			}
			m_channels.Clear();
			MyLog.Default.WriteLine("Network readers disposed");
		}

		public static void Process()
		{
			foreach (KeyValuePair<int, ChannelInfo> channel in m_channels)
			{
				channel.Value.Queue.Process(channel.Value.Handler);
			}
		}

		public static void GetAndClearStats(out int received, out int tamperred)
		{
			received = Interlocked.Exchange(ref m_byteCountReceived, 0);
			tamperred = Interlocked.Exchange(ref m_tamperred, 0);
		}

		public static void ReceiveAll()
		{
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<int, ChannelInfo> channel in m_channels)
			{
				while (true)
				{
					uint length;
					MyReceiveQueue.ReceiveStatus receiveStatus = channel.Value.Queue.ReceiveOne(out length);
					if (receiveStatus == MyReceiveQueue.ReceiveStatus.None)
					{
						break;
					}
					num += (int)length;
					if (receiveStatus == MyReceiveQueue.ReceiveStatus.TamperredPacket)
					{
						num2++;
					}
				}
			}
			Interlocked.Add(ref m_byteCountReceived, num);
			Interlocked.Add(ref m_tamperred, num2);
		}
	}
}
