using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.History;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.StateGroups
{
	public abstract class MyEntityPhysicsStateGroupBase : IMyStateGroup, IMyNetObject, IMyEventOwner
	{
		public class ParentingSetup
		{
			public float MaxParentDistance;

			public float MinParentSpeed;

			public float MaxParentAcceleration;

			public float MinInsideParentSpeed;

			public float MaxParentDisconnectDistance;

			public float MinDisconnectParentSpeed;

			public float MaxDisconnectParentAcceleration;

			public float MinDisconnectInsideParentSpeed;
		}

		protected IMySnapshotSync m_snapshotSync;

		protected bool m_forcedWorldSnapshots;

		private bool m_physicsActive;

		private const float MIN_SIZE = 10f;

		private const float MIN_ACCELERATION = 5f;

		private readonly List<MyEntity> m_tmpEntityResults = new List<MyEntity>();

		protected bool m_supportInited;

		protected long m_lastSupportId;

		public IMyReplicable Owner
		{
			get;
			private set;
		}

		public MyEntity Entity
		{
			get;
			private set;
		}

		public bool IsHighPriority
		{
			get
			{
				if (!m_forcedWorldSnapshots)
				{
					if (Entity.PositionComp.LocalAABB.Size.LengthSquared() > 100f && Entity.Physics != null)
					{
						return Entity.Physics.LinearAcceleration.LengthSquared() > 25f;
					}
					return false;
				}
				return true;
			}
		}

		public bool IsStreaming => false;

		public bool NeedsUpdate => true;

		protected bool IsControlled => Sync.Players.GetControllingPlayer(Entity) != null;

		protected bool IsControlledLocally => MySession.Static.TopMostControlledEntity == Entity;

		public bool IsValid => !Entity.MarkedForClose;

		public MyEntityPhysicsStateGroupBase(MyEntity entity, IMyReplicable ownerReplicable, bool createSync = true)
		{
			Entity = entity;
			Owner = ownerReplicable;
			if (!Sync.IsServer && createSync)
			{
				m_snapshotSync = new MyAnimatedSnapshotSync(Entity);
			}
			if (Sync.IsServer)
			{
				Entity.AddedToScene += RegisterPhysics;
			}
		}

		private void OnPhysicsComponentChanged(MyPhysicsComponentBase oldComponent, MyPhysicsComponentBase newComponent)
		{
			if (oldComponent != null)
			{
				oldComponent.OnBodyActiveStateChanged -= ActiveStateChanged;
			}
			if (newComponent != null)
			{
				m_physicsActive = newComponent.IsActive;
				newComponent.OnBodyActiveStateChanged += ActiveStateChanged;
			}
		}

		private void RegisterPhysics(MyEntity obj)
		{
			Entity.AddedToScene -= RegisterPhysics;
			if (Entity.Physics != null)
			{
				m_physicsActive = Entity.Physics.IsActive;
				Entity.Physics.OnBodyActiveStateChanged += ActiveStateChanged;
				Entity.OnPhysicsComponentChanged += OnPhysicsComponentChanged;
			}
		}

		private void ActiveStateChanged(MyPhysicsComponentBase physics, bool active)
		{
			m_physicsActive = active;
			if (active)
			{
				MyMultiplayer.GetReplicationServer().AddToDirtyGroups(this);
			}
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
		}

		public abstract void ClientUpdate(MyTimeSpan clientTimestamp);

		public virtual void Destroy()
		{
			if (Entity.Physics != null)
			{
				Entity.Physics.OnBodyActiveStateChanged -= ActiveStateChanged;
			}
			if (!Sync.IsServer)
			{
				m_snapshotSync.Destroy();
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
			m_snapshotSync.Reset(reinit);
			if (reinit)
			{
				ClientUpdate(clientTimestamp);
				m_snapshotSync.Reset(reinit);
			}
		}

		public virtual void Serialize(BitStream stream, Endpoint forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream.Writing)
			{
				new MySnapshot(Entity).Write(stream);
				bool isControlled = IsControlled;
				stream.WriteBool(isControlled);
				if (isControlled)
				{
					Entity.SerializeControls(stream);
				}
			}
			else
			{
				MySnapshot item = new MySnapshot(stream);
				m_snapshotSync.Read(ref item, serverTimestamp);
				if (stream.ReadBool())
				{
					Entity.DeserializeControls(stream, outOfOrder: false);
				}
			}
		}

		public virtual bool IsStillDirty(Endpoint forClient)
		{
			return m_physicsActive;
		}

		public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
		{
			return MyStreamProcessingState.None;
		}

		protected long UpdateParenting(ParentingSetup parentingSetup, long currentParentId)
		{
			List<MyEntity> tmpEntityResults = m_tmpEntityResults;
			MyCubeGrid myCubeGrid = null;
			BoundingBoxD boundingBox = Entity.PositionComp.WorldAABB.Inflate(parentingSetup.MaxParentDisconnectDistance);
			MyEntities.GetTopMostEntitiesInBox(ref boundingBox, tmpEntityResults, MyEntityQueryType.Dynamic);
			bool flag = false;
			float num = float.MaxValue;
			float num2 = float.MaxValue;
			float num3 = Entity.PositionComp.LocalAABB.Size.LengthSquared();
			float num4 = Entity.Physics.LinearVelocity.LengthSquared();
			foreach (MyEntity item in tmpEntityResults)
			{
				if (item.EntityId != Entity.EntityId)
				{
					MyCubeGrid myCubeGrid2 = item as MyCubeGrid;
					if (myCubeGrid2 != null && myCubeGrid2.Physics != null && myCubeGrid2.BlocksCount > 1)
					{
						float num5 = myCubeGrid2.PositionComp.LocalAABB.Size.LengthSquared();
						if (!(num5 <= num3))
						{
							bool flag2 = currentParentId == myCubeGrid2.EntityId;
							if (myCubeGrid2.PositionComp.WorldAABB.Contains(Entity.PositionComp.WorldAABB) == ContainmentType.Contains)
							{
								if (!flag)
								{
									myCubeGrid = null;
									flag = true;
								}
								float num6 = flag2 ? parentingSetup.MinDisconnectInsideParentSpeed : parentingSetup.MinInsideParentSpeed;
								if (!(myCubeGrid2.Physics.GetVelocityAtPoint(Entity.PositionComp.GetPosition()).LengthSquared() < num6 * num6) && num5 < num)
								{
									num = num5;
									myCubeGrid = myCubeGrid2;
								}
							}
							else if (!flag)
							{
								float num7 = flag2 ? parentingSetup.MinDisconnectParentSpeed : parentingSetup.MinParentSpeed;
								float num8 = num7 / 2f;
								if (!(num4 < num8 * num8))
								{
									float num9 = flag2 ? parentingSetup.MaxParentDisconnectDistance : parentingSetup.MaxParentDistance;
									float num10 = flag2 ? parentingSetup.MaxDisconnectParentAcceleration : parentingSetup.MaxParentAcceleration;
									float num11 = (float)myCubeGrid2.PositionComp.WorldAABB.DistanceSquared(Entity.PositionComp.GetPosition());
									if (!(num11 > num9 * num9) && !(myCubeGrid2.Physics.GetVelocityAtPoint(Entity.PositionComp.GetPosition()).LengthSquared() < num7 * num7) && !(myCubeGrid2.Physics.LinearAcceleration.LengthSquared() > num10 * num10) && num11 < num2)
									{
										num2 = num11;
										myCubeGrid = myCubeGrid2;
									}
								}
							}
						}
					}
				}
			}
			tmpEntityResults.Clear();
			long num12 = myCubeGrid?.EntityId ?? 0;
			if (!m_supportInited || m_lastSupportId == num12 || num12 == 0L)
			{
				currentParentId = num12;
			}
			m_lastSupportId = num12;
			m_supportInited = true;
			return currentParentId;
		}
	}
}
