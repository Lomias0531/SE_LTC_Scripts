using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
	internal class MyEntityTransformStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
	{
		private MatrixD m_transform = MatrixD.Identity;

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

		public MyEntityTransformStateGroup(IMyReplicable ownerReplicable, IMyEntity entity)
		{
			Owner = ownerReplicable;
			m_entity = entity;
			m_transform = m_entity.WorldMatrix;
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
		}

		public void ClientUpdate(MyTimeSpan clientTimestamp)
		{
			if (!m_entity.PositionComp.WorldMatrix.Equals(m_transform))
			{
				m_entity.SetWorldMatrix(m_transform);
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
				Quaternion q = Quaternion.CreateFromRotationMatrix(m_entity.PositionComp.WorldMatrix);
				stream.WriteQuaternion(q);
			}
			else
			{
				Vector3D translation = stream.ReadVector3D();
				Quaternion quaternion = stream.ReadQuaternion();
				m_transform = MatrixD.CreateFromQuaternion(quaternion);
				m_transform.Translation = translation;
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
