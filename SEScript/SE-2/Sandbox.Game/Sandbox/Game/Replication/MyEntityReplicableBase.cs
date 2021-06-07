using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Replication;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
	public abstract class MyEntityReplicableBase<T> : MyExternalReplicable<T>, IMyEntityReplicable where T : MyEntity
	{
		private Action<MyEntity> m_onCloseAction;

		private readonly List<IMyReplicable> m_tmpReplicables = new List<IMyReplicable>();

		private readonly HashSet<IMyReplicable> m_physicalDependencies = new HashSet<IMyReplicable>();

		private MyTimeSpan m_lastPhysicalDependencyUpdate;

		private bool m_destroyed;

		protected const double MIN_DITHER_DISTANCE_SQR = 1000000.0;

		public override bool IsValid
		{
			get
			{
				if (base.Instance != null)
				{
					return !base.Instance.MarkedForClose;
				}
				return false;
			}
		}

		public MatrixD WorldMatrix
		{
			get
			{
				if (base.Instance != null)
				{
					return base.Instance.WorldMatrix;
				}
				return MatrixD.Identity;
			}
		}

		public long EntityId
		{
			get
			{
				if (base.Instance != null)
				{
					return base.Instance.EntityId;
				}
				return 0L;
			}
		}

		public override bool HasToBeChild => false;

		protected override void OnLoad(BitStream stream, Action<T> loadingDoneHandler)
		{
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
			TryRemoveExistingEntity(myObjectBuilder_EntityBase.EntityId);
			T val = (T)MyEntities.CreateFromObjectBuilder(myObjectBuilder_EntityBase, fadeIn: false);
			if (val != null)
			{
				MyEntities.Add(val);
			}
			loadingDoneHandler(val);
		}

		protected override void OnHook()
		{
			m_physicsSync = CreatePhysicsGroup();
			m_onCloseAction = OnClose;
			base.Instance.OnClose += m_onCloseAction;
			if (Sync.IsServer)
			{
				base.Instance.PositionComp.OnPositionChanged += PositionComp_OnPositionChanged;
				base.Instance.PositionComp.OnLocalAABBChanged += PositionComp_OnPositionChanged;
			}
		}

		private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
		{
			if (base.OnAABBChanged != null)
			{
				base.OnAABBChanged(this);
			}
		}

		private void OnClose(MyEntity ent)
		{
			RaiseDestroyed();
		}

		protected virtual IMyStateGroup CreatePhysicsGroup()
		{
			return new MyEntityPhysicsStateGroup(base.Instance, this);
		}

		public override IMyReplicable GetParent()
		{
			return null;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			MyObjectBuilder_EntityBase value = base.Instance.GetObjectBuilder();
			MySerializer.Write(stream, ref value, MyObjectBuilderSerializer.Dynamic);
			return true;
		}

		public override void OnDestroyClient()
		{
			if (base.Instance != null)
			{
				if ((base.Instance.PositionComp.GetPosition() - MySector.MainCamera.Position).LengthSquared() > 1000000.0)
				{
					base.Instance.Render.FadeOut = true;
				}
				base.Instance.Close();
			}
			m_physicsSync = null;
			m_destroyed = true;
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			if (m_physicsSync != null)
			{
				resultList.Add(m_physicsSync);
			}
		}

		public override BoundingBoxD GetAABB()
		{
			if (base.Instance == null)
			{
				return BoundingBoxD.CreateInvalid();
			}
			return base.Instance.PositionComp.WorldAABB;
		}

		public override HashSet<IMyReplicable> GetPhysicalDependencies(MyTimeSpan timeStamp, MyReplicablesBase replicables)
		{
			if (m_lastPhysicalDependencyUpdate != timeStamp && IncludeInIslands && CheckConsistency())
			{
				m_lastPhysicalDependencyUpdate = timeStamp;
				m_physicalDependencies.Clear();
				bool flag = true;
				BoundingBoxD aABB = GetAABB();
				while (flag)
				{
					flag = false;
					m_tmpReplicables.Clear();
					m_physicalDependencies.Add(this);
					replicables.GetReplicablesInBox(aABB.GetInflated(2.5), m_tmpReplicables);
					foreach (IMyReplicable tmpReplicable in m_tmpReplicables)
					{
						if (tmpReplicable.CheckConsistency() && !m_physicalDependencies.Contains(tmpReplicable) && tmpReplicable.IncludeInIslands)
						{
							m_physicalDependencies.Add(tmpReplicable);
							aABB.Include(tmpReplicable.GetAABB());
							flag = true;
						}
					}
				}
			}
			return m_physicalDependencies;
		}

		protected void TryRemoveExistingEntity(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
			{
				entity.EntityId = MyEntityIdentifier.AllocateId();
				if (!entity.MarkedForClose)
				{
					entity.Close();
				}
			}
		}
	}
}
