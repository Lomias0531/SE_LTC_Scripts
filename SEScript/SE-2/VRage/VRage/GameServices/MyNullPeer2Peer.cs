using System;
using System.Text;
using VRage.Utils;

namespace VRage.GameServices
{
	public class MyNullPeer2Peer : IMyPeer2Peer
	{
		private static readonly StringBuilder m_errorBuilder = new StringBuilder("Attempted to use method from NULL Peer2Peer.");

		public event Action<ulong> SessionRequest
		{
			add
			{
				ErrorMsg();
			}
			remove
			{
				ErrorMsg();
			}
		}

		public event Action<ulong, string> ConnectionFailed
		{
			add
			{
				ErrorMsg();
			}
			remove
			{
				ErrorMsg();
			}
		}

		public bool AcceptSession(ulong remotePeerId)
		{
			ErrorMsg();
			return false;
		}

		public bool CloseSession(ulong remotePeerId)
		{
			ErrorMsg();
			return false;
		}

		public bool SendPacket(ulong remoteUser, byte[] data, int byteCount, MyP2PMessageEnum msgType, int channel)
		{
			ErrorMsg();
			return false;
		}

		public bool ReadPacket(byte[] buffer, ref uint dataSize, out ulong remoteUser, int channel)
		{
			ErrorMsg();
			dataSize = 0u;
			remoteUser = 0uL;
			return false;
		}

		public bool IsPacketAvailable(out uint msgSize, int channel)
		{
			ErrorMsg();
			msgSize = 0u;
			return false;
		}

		public bool GetSessionState(ulong remoteUser, ref MyP2PSessionState state)
		{
			state = default(MyP2PSessionState);
			return false;
		}

		public void BeginFrameProcessing()
		{
			ErrorMsg();
		}

		public void EndFrameProcessing()
		{
			ErrorMsg();
		}

		private static void ErrorMsg()
		{
			MyLog.Default.Log(MyLogSeverity.Error, m_errorBuilder.ToString());
		}

		public void SetServer(bool server)
		{
		}
	}
}
