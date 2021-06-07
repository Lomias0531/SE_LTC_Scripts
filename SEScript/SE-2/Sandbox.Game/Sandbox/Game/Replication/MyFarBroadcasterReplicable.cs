using Sandbox.Game.Entities;
using Sandbox.Game.Replication.StateGroups;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
	internal class MyFarBroadcasterReplicable : MyExternalReplicableEvent<MyDataBroadcaster>
	{
		private MyEntityPositionStateGroup m_positionStateGroup;

		private MyProxyAntenna m_proxyAntenna;

		public override bool IsValid
		{
			get
			{
				if (base.Instance != null && base.Instance.Entity != null)
				{
					return !base.Instance.Entity.MarkedForClose;
				}
				return false;
			}
		}

		public override bool PriorityUpdate => false;

		public override bool HasToBeChild => true;

		protected override void OnHook()
		{
			base.OnHook();
			m_positionStateGroup = new MyEntityPositionStateGroup(this, base.Instance.Entity);
			base.Instance.BeforeRemovedFromContainer += delegate
			{
				OnRemovedFromContainer();
			};
		}

		private void OnRemovedFromContainer()
		{
			RaiseDestroyed();
		}

		public override IMyReplicable GetParent()
		{
			return null;
		}

		protected override void OnLoad(BitStream stream, Action<MyDataBroadcaster> loadingDoneHandler)
		{
			MySerializer.CreateAndRead(stream, out MyObjectBuilder_ProxyAntenna value, MyObjectBuilderSerializer.Dynamic);
			m_proxyAntenna = (MyEntities.CreateFromObjectBuilderAndAdd(value, fadeIn: false) as MyProxyAntenna);
			loadingDoneHandler(m_proxyAntenna.Broadcaster);
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			MyObjectBuilder_ProxyAntenna value = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ProxyAntenna>();
			base.Instance.InitProxyObjectBuilder(value);
			MySerializer.Write(stream, ref value, MyObjectBuilderSerializer.Dynamic);
			return true;
		}

		public override void OnDestroyClient()
		{
			if (m_proxyAntenna != null)
			{
				m_proxyAntenna.Close();
			}
			m_proxyAntenna = null;
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			resultList.Add(m_positionStateGroup);
		}

		public override BoundingBoxD GetAABB()
		{
			_ = base.Instance;
			if (base.Instance != null && base.Instance.Entity != null)
			{
				return base.Instance.Entity.WorldAABB;
			}
			return BoundingBoxD.CreateInvalid();
		}
	}
}
