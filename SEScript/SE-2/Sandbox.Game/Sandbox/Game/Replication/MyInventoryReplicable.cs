using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
	internal class MyInventoryReplicable : MyExternalReplicableEvent<MyInventory>
	{
		private MyPropertySyncStateGroup m_propertySync;

		private MyEntityInventoryStateGroup m_stateGroup;

		private long m_entityId;

		private int m_inventoryId;

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

		public override bool HasToBeChild => true;

		protected override void OnHook()
		{
			base.OnHook();
			if (base.Instance != null)
			{
				m_stateGroup = new MyEntityInventoryStateGroup(base.Instance, Sync.IsServer, this);
				base.Instance.BeforeRemovedFromContainer += delegate
				{
					OnRemovedFromContainer();
				};
				m_propertySync = new MyPropertySyncStateGroup(this, base.Instance.SyncType);
				MyCubeBlock myCubeBlock = base.Instance.Owner as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeBlock.SlimBlock.CubeGridChanged += OnBlockCubeGridChanged;
					m_parent = MyExternalReplicable.FindByObject(myCubeBlock.CubeGrid);
				}
				else
				{
					m_parent = MyExternalReplicable.FindByObject(base.Instance.Owner);
				}
			}
		}

		private void OnBlockCubeGridChanged(MySlimBlock slimBlock, MyCubeGrid grid)
		{
			m_parent = MyExternalReplicable.FindByObject((base.Instance.Owner as MyCubeBlock).CubeGrid);
			(MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy(this);
		}

		public override IMyReplicable GetParent()
		{
			if (m_parent == null)
			{
				m_parent = MyExternalReplicable.FindByObject(base.Instance.Owner);
			}
			return m_parent;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			long value = base.Instance.Owner.EntityId;
			MySerializer.Write(stream, ref value);
			int value2 = 0;
			for (int i = 0; i < base.Instance.Owner.InventoryCount; i++)
			{
				if (base.Instance == base.Instance.Owner.GetInventory(i))
				{
					value2 = i;
					break;
				}
			}
			MySerializer.Write(stream, ref value2);
			return true;
		}

		protected override void OnLoad(BitStream stream, Action<MyInventory> loadingDoneHandler)
		{
			if (stream != null)
			{
				MySerializer.CreateAndRead(stream, out m_entityId);
				MySerializer.CreateAndRead(stream, out m_inventoryId);
			}
			MyEntities.CallAsync(delegate
			{
				LoadAsync(loadingDoneHandler);
			});
		}

		private void LoadAsync(Action<MyInventory> loadingDoneHandler)
		{
			MyEntities.TryGetEntityById(m_entityId, out MyEntity entity);
			MyInventory obj = null;
			MyEntity myEntity = (entity != null && entity.HasInventory) ? entity : null;
			if (myEntity != null && !myEntity.GetTopMostParent().MarkedForClose)
			{
				obj = myEntity.GetInventory(m_inventoryId);
			}
			loadingDoneHandler(obj);
		}

		public override void OnDestroyClient()
		{
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			if (m_stateGroup != null)
			{
				resultList.Add(m_stateGroup);
			}
			resultList.Add(m_propertySync);
		}

		public override string ToString()
		{
			string str = (base.Instance == null) ? "<inventory null>" : ((base.Instance.Owner != null) ? base.Instance.Owner.EntityId.ToString() : "<owner null>");
			return string.Format("MyInventoryReplicable, Owner id: " + str);
		}

		private void OnRemovedFromContainer()
		{
			if (base.Instance != null && base.Instance.Owner != null)
			{
				MyCubeBlock myCubeBlock = base.Instance.Owner as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeBlock.SlimBlock.CubeGridChanged -= OnBlockCubeGridChanged;
				}
				RaiseDestroyed();
			}
		}

		public override BoundingBoxD GetAABB()
		{
			return BoundingBoxD.CreateInvalid();
		}

		public override ValidationResult HasRights(EndpointId endpointId, ValidationType validationFlags)
		{
			return MyExternalReplicable.FindByObject(base.Instance.Owner)?.HasRights(endpointId, validationFlags) ?? base.HasRights(endpointId, validationFlags);
		}
	}
}
