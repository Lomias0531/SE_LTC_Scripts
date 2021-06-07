using System;

namespace VRage.GameServices
{
	public interface IMyPeer2Peer
	{
		event Action<ulong> SessionRequest;

		event Action<ulong, string> ConnectionFailed;

		bool AcceptSession(ulong remotePeerId);

		bool CloseSession(ulong remotePeerId);

		bool SendPacket(ulong remoteUser, byte[] data, int byteCount, MyP2PMessageEnum msgType, int channel);

		bool ReadPacket(byte[] buffer, ref uint dataSize, out ulong remoteUser, int channel);

		bool IsPacketAvailable(out uint msgSize, int channel);

		bool GetSessionState(ulong remoteUser, ref MyP2PSessionState state);

		void BeginFrameProcessing();

		void EndFrameProcessing();

		void SetServer(bool server);
	}
}
