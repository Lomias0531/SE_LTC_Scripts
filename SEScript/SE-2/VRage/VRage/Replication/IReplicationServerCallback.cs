using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;

namespace VRage.Replication
{
	public interface IReplicationServerCallback
	{
		void SendServerData(IPacketData data, Endpoint endpoint);

		void SendReplicationCreate(IPacketData data, Endpoint endpoint);

		void SendReplicationCreateStreamed(IPacketData data, Endpoint endpoint);

		void SendReplicationDestroy(IPacketData data, List<EndpointId> endpoints);

		void SendReplicationIslandDone(IPacketData data, Endpoint endpoint);

		void SendStateSync(IPacketData data, Endpoint endpoint, bool reliable);

		void SendJoinResult(IPacketData data, EndpointId endpoint);

		void SendWorldData(IPacketData data, List<EndpointId> endpoints);

		void SendWorld(IPacketData data, EndpointId endpoint);

		void SendPlayerData(IPacketData data, List<EndpointId> endpoints);

		void WriteCustomState(BitStream stream);

		void SentClientJoined(IPacketData data, EndpointId endpoint);

		void SendEvent(IPacketData data, bool reliable, List<EndpointId> endpoints);

		int GetMTUSize(Endpoint clientId);

		int GetMTRSize(Endpoint clientId);

		IMyReplicable GetReplicableByEntityId(long entityId);

		void DisconnectClient(ulong clientId);

		void ValidationFailed(ulong clientId, bool kick = true, string additionalInfo = null, bool stackTrace = true);

		MyTimeSpan GetUpdateTime();

		MyPacketDataBitStreamBase GetBitStreamPacketData();

		void SendPendingReplicablesDone(Endpoint endpoint);

		void SendVoxelCacheInvalidated(string storageName, EndpointId endpoint);
	}
}
