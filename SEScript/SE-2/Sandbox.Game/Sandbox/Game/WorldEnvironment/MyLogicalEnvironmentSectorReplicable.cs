using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	internal class MyLogicalEnvironmentSectorReplicable : MyExternalReplicableEvent<MyLogicalEnvironmentSectorBase>
	{
		private static readonly MySerializeInfo serialInfo = new MySerializeInfo(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, MyPrimitiveFlags.None, 0, MyObjectBuilderSerializer.SerializeDynamic, null, null);

		private long m_planetEntityId;

		private long m_packedSectorId;

		private MyObjectBuilder_EnvironmentSector m_ob;

		public override bool IncludeInIslands => false;

		public override bool IsValid
		{
			get
			{
				if (m_parent != null)
				{
					return m_parent.IsValid;
				}
				return false;
			}
		}

		public override bool HasToBeChild => false;

		public override bool IsSpatial => true;

		public override IMyReplicable GetParent()
		{
			return m_parent;
		}

		public override bool ShouldReplicate(MyClientInfo client)
		{
			MyClientState myClientState = client.State as MyClientState;
			if (base.Instance.Owner.Entity == null)
			{
				return false;
			}
			long entityId = base.Instance.Owner.Entity.EntityId;
			if (myClientState.KnownSectors.TryGetValue(entityId, out HashSet<long> value) && value.Contains(base.Instance.Id))
			{
				return true;
			}
			return false;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			stream.WriteInt64(base.Instance.Owner.Entity.EntityId);
			stream.WriteInt64(base.Instance.Id);
			MyObjectBuilder_EnvironmentSector value = base.Instance.GetObjectBuilder();
			MySerializer.Write(stream, ref value, serialInfo);
			return true;
		}

		public override void OnDestroyClient()
		{
			if (base.Instance != null)
			{
				base.Instance.ServerOwned = false;
			}
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
		}

		protected override void OnLoad(BitStream stream, Action<MyLogicalEnvironmentSectorBase> loadingDoneHandler)
		{
			if (stream != null)
			{
				m_planetEntityId = stream.ReadInt64();
				m_packedSectorId = stream.ReadInt64();
				m_ob = MySerializer.CreateAndRead<MyObjectBuilder_EnvironmentSector>(stream, serialInfo);
			}
			MyPlanet myPlanet = MyEntities.GetEntityById(m_planetEntityId) as MyPlanet;
			if (myPlanet == null)
			{
				loadingDoneHandler(null);
				return;
			}
			MyLogicalEnvironmentSectorBase logicalSector = myPlanet.Components.Get<MyPlanetEnvironmentComponent>().GetLogicalSector(m_packedSectorId);
			bool flag = MyExternalReplicable.FindByObject(myPlanet) != null;
			if (logicalSector != null && flag)
			{
				logicalSector.Init(m_ob);
			}
			loadingDoneHandler(((logicalSector != null && logicalSector.ServerOwned) || !flag) ? null : logicalSector);
		}

		protected override void OnHook()
		{
			base.OnHook();
			if (Sync.IsServer)
			{
				base.Instance.OnClose += Sector_OnClose;
			}
			else
			{
				base.Instance.ServerOwned = true;
			}
			m_parent = MyExternalReplicable.FindByObject(base.Instance.Owner.Entity);
		}

		private void Sector_OnClose()
		{
			base.Instance.OnClose -= Sector_OnClose;
			base.Instance.ServerOwned = false;
			RaiseDestroyed();
		}

		public override BoundingBoxD GetAABB()
		{
			BoundingBoxD result = BoundingBoxD.CreateInvalid();
			Vector3D[] bounds = base.Instance.Bounds;
			foreach (Vector3D point in bounds)
			{
				result = result.Include(point);
			}
			return result;
		}
	}
}
