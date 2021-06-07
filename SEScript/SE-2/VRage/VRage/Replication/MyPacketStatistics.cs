using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Replication
{
	public struct MyPacketStatistics
	{
		public int Duplicates;

		public int Drops;

		public int OutOfOrder;

		public int Tamperred;

		public int OutgoingData;

		public int IncomingData;

		public float TimeInterval;

		public byte PendingPackets;

		public float GCMemory;

		public float ProcessMemory;

		private MyTimeSpan m_nextTime;

		private static readonly MyTimeSpan SEND_TIMEOUT = MyTimeSpan.FromSeconds(0.10000000149011612);

		public void Reset()
		{
			Duplicates = (OutOfOrder = (Drops = (OutgoingData = (IncomingData = (Tamperred = 0)))));
			TimeInterval = 0f;
		}

		public void UpdateData(int outgoing, int incoming, int incomingTamperred, float gcMemory, float processMemory)
		{
			OutgoingData += outgoing;
			IncomingData += incoming;
			Tamperred += incomingTamperred;
			GCMemory = gcMemory;
			ProcessMemory = processMemory;
		}

		public void Update(MyPacketTracker.OrderType type)
		{
			switch (type)
			{
			case MyPacketTracker.OrderType.InOrder:
				break;
			case MyPacketTracker.OrderType.Duplicate:
				Duplicates++;
				break;
			case MyPacketTracker.OrderType.OutOfOrder:
				OutOfOrder++;
				break;
			default:
				Drops += (int)(type - 3 + 1);
				break;
			}
		}

		public void Write(BitStream sendStream, MyTimeSpan currentTime)
		{
			if (currentTime > m_nextTime)
			{
				sendStream.WriteBool(value: true);
				sendStream.WriteByte((byte)Duplicates);
				sendStream.WriteByte((byte)OutOfOrder);
				sendStream.WriteByte((byte)Drops);
				sendStream.WriteByte((byte)Tamperred);
				sendStream.WriteInt32(OutgoingData);
				sendStream.WriteInt32(IncomingData);
				sendStream.WriteFloat((float)(currentTime - m_nextTime + SEND_TIMEOUT).Seconds);
				sendStream.WriteByte(PendingPackets);
				sendStream.WriteFloat(GCMemory);
				sendStream.WriteFloat(ProcessMemory);
				Reset();
				m_nextTime = currentTime + SEND_TIMEOUT;
			}
			else
			{
				sendStream.WriteBool(value: false);
			}
		}

		public void Read(BitStream receiveStream)
		{
			if (receiveStream.ReadBool())
			{
				Duplicates = receiveStream.ReadByte();
				OutOfOrder = receiveStream.ReadByte();
				Drops = receiveStream.ReadByte();
				Tamperred = receiveStream.ReadByte();
				OutgoingData = receiveStream.ReadInt32();
				IncomingData = receiveStream.ReadInt32();
				TimeInterval = receiveStream.ReadFloat();
				PendingPackets = receiveStream.ReadByte();
				GCMemory = receiveStream.ReadFloat();
				ProcessMemory = receiveStream.ReadFloat();
			}
		}

		public void Add(MyPacketStatistics statistics)
		{
			Duplicates += statistics.Duplicates;
			OutOfOrder += statistics.OutOfOrder;
			Drops += statistics.Drops;
			Tamperred += statistics.Tamperred;
			OutgoingData += statistics.OutgoingData;
			IncomingData += statistics.IncomingData;
			TimeInterval += statistics.TimeInterval;
			PendingPackets = statistics.PendingPackets;
			GCMemory = statistics.GCMemory;
			ProcessMemory = statistics.ProcessMemory;
		}
	}
}
