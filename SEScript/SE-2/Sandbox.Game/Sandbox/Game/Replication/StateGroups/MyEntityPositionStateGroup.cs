using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
	internal class MyEntityPositionStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
	{
		private Vector3D m_position;

		private readonly IMyEntity m_entity;

		public bool IsStreaming => false;

		public bool NeedsUpdate => true;

		public bool IsHighPriority => false;

		public IMyReplicable Owner
		{
			get;
			private set;
		}

		public bool IsValid => !m_entity.MarkedForClose;

		public MyEntityPositionStateGroup(IMyReplicable ownerReplicable, IMyEntity entity)
		{
			Owner = ownerReplicable;
			m_entity = entity;
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
		}

		public void ClientUpdate(MyTimeSpan clientTimestamp)
		{
			if (!m_entity.PositionComp.GetPosition().Equals(m_position, 1.0))
			{
				m_entity.SetWorldMatrix(MatrixD.CreateTranslation(m_position));
			}
		}

		public void Destroy()
		{
			Owner = null;
		}

		public float GetGroupPriority(int frameCountWithoutSync, MyClientInfo forClient)
		{
			return 1f;
		}

		public void Serialize(BitStream stream, Endpoint forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream.Writing)
			{
				stream.Write(m_entity.PositionComp.GetPosition());
			}
			else
			{
				m_position = stream.ReadVector3D();
			}
		}

		public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
		{
		}

		public void ForceSend(MyClientStateBase clientData)
		{
		}

		public void Reset(bool reinit, MyTimeSpan clientTimestamp)
		{
		}

		public bool IsStillDirty(Endpoint forClient)
		{
			return true;
		}

		public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
		{
			return MyStreamProcessingState.None;
		}
	}
}
