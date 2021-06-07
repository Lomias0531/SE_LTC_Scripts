using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Replication.StateGroups
{
	internal class MyStreamingEntityStateGroup<T> : IMyStateGroup, IMyNetObject, IMyEventOwner where T : IMyStreamableReplicable
	{
		private class StreamPartInfo : IComparable<StreamPartInfo>
		{
			public int StartIndex;

			public int NumBits;

			public short Position;

			public int CompareTo(StreamPartInfo b)
			{
				return StartIndex.CompareTo(b.StartIndex);
			}
		}

		private class StreamClientData
		{
			public short CurrentPart;

			public short NumParts;

			public int LastPosition;

			public byte[] ObjectData;

			public bool CreatingData;

			public bool Incomplete;

			public bool Dirty;

			public int RemainingBits;

			public int UncompressedSize;

			public bool ForceSend;

			public readonly Dictionary<byte, StreamPartInfo> SendPackets = new Dictionary<byte, StreamPartInfo>();

			public readonly List<StreamPartInfo> FailedIncompletePackets = new List<StreamPartInfo>();
		}

		private int m_streamSize = 8000;

		private const int HEADER_SIZE = 97;

		private const int SAFE_VALUE = 128;

		private bool m_streamed;

		private Dictionary<Endpoint, StreamClientData> m_clientStreamData;

		private SortedList<StreamPartInfo, byte[]> m_receivedParts;

		private short m_numPartsToReceive;

		private int m_receivedBytes;

		private int m_uncompressedSize;

		private T Instance
		{
			get;
			set;
		}

		public IMyReplicable Owner
		{
			get;
			private set;
		}

		public bool NeedsUpdate => false;

		public bool IsValid
		{
			get
			{
				if (Owner != null)
				{
					return Owner.IsValid;
				}
				return false;
			}
		}

		public bool IsHighPriority => false;

		public bool IsStreaming => true;

		public MyStreamingEntityStateGroup(T obj, IMyReplicable owner)
		{
			Instance = obj;
			Owner = owner;
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
			if (m_clientStreamData == null)
			{
				m_clientStreamData = new Dictionary<Endpoint, StreamClientData>();
			}
			if (!m_clientStreamData.TryGetValue(forClient.EndpointId, out StreamClientData _))
			{
				m_clientStreamData[forClient.EndpointId] = new StreamClientData();
			}
			m_clientStreamData[forClient.EndpointId].Dirty = true;
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
			if (m_clientStreamData != null)
			{
				m_clientStreamData.Remove(forClient.EndpointId);
			}
		}

		public void ClientUpdate(MyTimeSpan clientTimestamp)
		{
		}

		public void Destroy()
		{
			if (m_receivedParts != null)
			{
				m_receivedParts.Clear();
				m_receivedParts = null;
			}
		}

		private unsafe bool ReadPart(ref BitStream stream)
		{
			m_numPartsToReceive = stream.ReadInt16();
			short startIndex = stream.ReadInt16();
			int num = stream.ReadInt32();
			int divisionCeil = MyLibraryUtils.GetDivisionCeil(num, 8);
			int num2 = stream.BitLength - stream.BitPosition;
			if (num2 < num)
			{
				MyLog.Default.WriteLine("trying to read more than there is in stream. Total num parts : " + m_numPartsToReceive + " current part : " + startIndex + " bits to read : " + num + " bits in stream : " + num2 + " replicable : " + Instance.ToString());
				return false;
			}
			if (m_receivedParts == null)
			{
				m_receivedParts = new SortedList<StreamPartInfo, byte[]>();
			}
			m_receivedBytes += divisionCeil;
			byte[] array = new byte[divisionCeil];
			fixed (byte* ptr = array)
			{
				stream.ReadMemory(ptr, num);
			}
			StreamPartInfo streamPartInfo = new StreamPartInfo();
			streamPartInfo.NumBits = num;
			streamPartInfo.StartIndex = startIndex;
			m_receivedParts[streamPartInfo] = array;
			return true;
		}

		private void ProcessRead(BitStream stream)
		{
			if (stream.BitLength == stream.BitPosition || m_streamed)
			{
				return;
			}
			if (stream.ReadBool())
			{
				m_uncompressedSize = stream.ReadInt32();
				if (!ReadPart(ref stream))
				{
					m_receivedParts = null;
					Instance.LoadCancel();
				}
				else if (m_receivedParts.Count == m_numPartsToReceive)
				{
					m_streamed = true;
					CreateReplicable(m_uncompressedSize);
				}
			}
			else
			{
				MyLog.Default.WriteLine("received empty state group");
				if (m_receivedParts != null)
				{
					m_receivedParts.Clear();
				}
				m_receivedParts = null;
				Instance.LoadCancel();
			}
		}

		private unsafe void CreateReplicable(int uncompressedSize)
		{
			byte[] array = new byte[m_receivedBytes];
			int num = 0;
			foreach (KeyValuePair<StreamPartInfo, byte[]> receivedPart in m_receivedParts)
			{
				Buffer.BlockCopy(receivedPart.Value, 0, array, num, receivedPart.Value.Length);
				num += receivedPart.Value.Length;
			}
			byte[] array2 = MemoryCompressor.Decompress(array);
			BitStream bitStream = new BitStream();
			bitStream.ResetWrite();
			fixed (byte* ptr = array2)
			{
				bitStream.SerializeMemory(ptr, uncompressedSize);
			}
			bitStream.ResetRead();
			Instance.LoadDone(bitStream);
			if (!bitStream.CheckTerminator())
			{
				MyLog.Default.WriteLine("Streaming entity: Invalid stream terminator");
			}
			bitStream.Dispose();
			if (m_receivedParts != null)
			{
				m_receivedParts.Clear();
			}
			m_receivedParts = null;
			m_receivedBytes = 0;
		}

		private void ProcessWrite(int maxBitPosition, BitStream stream, Endpoint forClient, byte packetId, HashSet<string> cachedData)
		{
			StreamClientData streamClientData = m_clientStreamData[forClient];
			if (streamClientData.FailedIncompletePackets.Count > 0)
			{
				WriteIncompletePacket(streamClientData, packetId, ref stream);
				return;
			}
			int num = 0;
			if (streamClientData.ObjectData == null)
			{
				SaveReplicable(streamClientData, cachedData, forClient);
				return;
			}
			m_streamSize = MyLibraryUtils.GetDivisionCeil(maxBitPosition - stream.BitPosition - 97 - 128, 8) * 8;
			streamClientData.NumParts = (short)MyLibraryUtils.GetDivisionCeil(streamClientData.ObjectData.Length * 8, m_streamSize);
			num = streamClientData.RemainingBits;
			if (num == 0)
			{
				streamClientData.ForceSend = false;
				streamClientData.Dirty = false;
				stream.WriteBool(value: false);
				return;
			}
			stream.WriteBool(value: true);
			stream.WriteInt32(streamClientData.UncompressedSize);
			if (num > m_streamSize || streamClientData.Incomplete)
			{
				WritePart(ref num, streamClientData, packetId, ref stream);
				streamClientData.Incomplete = (streamClientData.RemainingBits > 0);
			}
			else
			{
				WriteWhole(num, streamClientData, packetId, ref stream);
			}
			if (streamClientData.RemainingBits == 0)
			{
				streamClientData.Dirty = false;
				streamClientData.ForceSend = false;
			}
		}

		private unsafe void WriteIncompletePacket(StreamClientData clientData, byte packetId, ref BitStream stream)
		{
			if (clientData.ObjectData == null)
			{
				clientData.FailedIncompletePackets.Clear();
				return;
			}
			StreamPartInfo streamPartInfo = clientData.FailedIncompletePackets[0];
			clientData.FailedIncompletePackets.Remove(streamPartInfo);
			clientData.SendPackets[packetId] = streamPartInfo;
			stream.WriteBool(value: true);
			stream.WriteInt32(clientData.UncompressedSize);
			stream.WriteInt16(clientData.NumParts);
			stream.WriteInt16(streamPartInfo.Position);
			stream.WriteInt32(streamPartInfo.NumBits);
			fixed (byte* ptr = &clientData.ObjectData[streamPartInfo.StartIndex])
			{
				stream.WriteMemory(ptr, streamPartInfo.NumBits);
			}
		}

		private unsafe void WritePart(ref int bitsToSend, StreamClientData clientData, byte packetId, ref BitStream stream)
		{
			bitsToSend = Math.Min(m_streamSize, clientData.RemainingBits);
			StreamPartInfo streamPartInfo = new StreamPartInfo
			{
				StartIndex = clientData.LastPosition,
				NumBits = bitsToSend
			};
			clientData.LastPosition = streamPartInfo.StartIndex + MyLibraryUtils.GetDivisionCeil(m_streamSize, 8);
			clientData.SendPackets[packetId] = streamPartInfo;
			clientData.RemainingBits = Math.Max(0, clientData.RemainingBits - m_streamSize);
			stream.WriteInt16(clientData.NumParts);
			stream.WriteInt16(clientData.CurrentPart);
			streamPartInfo.Position = clientData.CurrentPart;
			clientData.CurrentPart++;
			stream.WriteInt32(bitsToSend);
			fixed (byte* ptr = &clientData.ObjectData[streamPartInfo.StartIndex])
			{
				stream.WriteMemory(ptr, bitsToSend);
			}
		}

		private unsafe void WriteWhole(int bitsToSend, StreamClientData clientData, byte packetId, ref BitStream stream)
		{
			StreamPartInfo value = new StreamPartInfo
			{
				StartIndex = 0,
				NumBits = bitsToSend,
				Position = 0
			};
			clientData.SendPackets[packetId] = value;
			clientData.RemainingBits = 0;
			clientData.Dirty = false;
			clientData.ForceSend = false;
			stream.WriteInt16(1);
			stream.WriteInt16(0);
			stream.WriteInt32(bitsToSend);
			fixed (byte* ptr = clientData.ObjectData)
			{
				stream.WriteMemory(ptr, bitsToSend);
			}
		}

		public void Serialize(BitStream stream, Endpoint forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream != null && stream.Reading)
			{
				ProcessRead(stream);
			}
			else
			{
				ProcessWrite(maxBitPosition, stream, forClient, packetId, cachedData);
			}
		}

		private void SaveReplicable(StreamClientData clientData, HashSet<string> cachedData, Endpoint forClient)
		{
			BitStream str = new BitStream();
			str.ResetWrite();
			clientData.CreatingData = true;
			Instance.Serialize(str, cachedData, forClient, delegate
			{
				WriteClientData(str, clientData);
			});
		}

		private unsafe void WriteClientData(BitStream str, StreamClientData clientData)
		{
			str.Terminate();
			str.ResetRead();
			int bitLength = str.BitLength;
			byte[] array = new byte[str.ByteLength];
			fixed (byte* ptr = array)
			{
				str.SerializeMemory(ptr, bitLength);
			}
			str.Dispose();
			clientData.CurrentPart = 0;
			clientData.ObjectData = MemoryCompressor.Compress(array);
			clientData.UncompressedSize = bitLength;
			clientData.RemainingBits = clientData.ObjectData.Length * 8;
			clientData.CreatingData = false;
		}

		public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
		{
		}

		public void ForceSend(MyClientStateBase clientData)
		{
			StreamClientData streamClientData = m_clientStreamData[clientData.EndpointId];
			streamClientData.ForceSend = true;
			SaveReplicable(streamClientData, null, clientData.EndpointId);
		}

		public void Reset(bool reinit, MyTimeSpan clientTimestamp)
		{
		}

		public bool IsStillDirty(Endpoint forClient)
		{
			return m_clientStreamData[forClient].Dirty;
		}

		public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
		{
			if (m_clientStreamData.TryGetValue(forClient, out StreamClientData value))
			{
				if (!value.CreatingData)
				{
					if (value.ObjectData == null)
					{
						return MyStreamProcessingState.None;
					}
					return MyStreamProcessingState.Finished;
				}
				return MyStreamProcessingState.Processing;
			}
			return MyStreamProcessingState.None;
		}
	}
}
