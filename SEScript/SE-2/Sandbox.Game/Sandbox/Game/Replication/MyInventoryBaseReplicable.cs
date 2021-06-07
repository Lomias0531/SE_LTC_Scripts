using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Replication
{
	internal class MyInventoryBaseReplicable : MyExternalReplicableEvent<MyInventoryBase>
	{
		private readonly Action<MyEntity> m_destroyEntity;

		private long m_entityId;

		private MyStringHash m_inventoryId;

		private MyInventoryBase Inventory => base.Instance;

		public override bool IsValid
		{
			get
			{
				if (Inventory != null && Inventory.Entity != null)
				{
					return !Inventory.Entity.MarkedForClose;
				}
				return false;
			}
		}

		public override bool HasToBeChild => true;

		public MyInventoryBaseReplicable()
		{
			m_destroyEntity = delegate
			{
				RaiseDestroyed();
			};
		}

		protected override void OnHook()
		{
			base.OnHook();
			if (Inventory != null)
			{
				((MyEntity)Inventory.Entity).OnClose += m_destroyEntity;
				Inventory.BeforeRemovedFromContainer += delegate
				{
					OnRemovedFromContainer();
				};
				MyCubeBlock myCubeBlock = Inventory.Entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeBlock.SlimBlock.CubeGridChanged += OnBlockCubeGridChanged;
				}
			}
		}

		private void OnBlockCubeGridChanged(MySlimBlock slimBlock, MyCubeGrid grid)
		{
			if (Sync.IsServer)
			{
				(MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy(this);
			}
		}

		public override IMyReplicable GetParent()
		{
			if (Inventory.Entity is MyCharacter)
			{
				return MyExternalReplicable.FindByObject(Inventory.Entity);
			}
			if (Inventory.Entity is MyCubeBlock)
			{
				return MyExternalReplicable.FindByObject((Inventory.Entity as MyCubeBlock).CubeGrid);
			}
			return null;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			long value = Inventory.Entity.EntityId;
			MySerializer.Write(stream, ref value);
			MyStringHash value2 = Inventory.InventoryId;
			MySerializer.Write(stream, ref value2);
			return true;
		}

		protected override void OnLoad(BitStream stream, Action<MyInventoryBase> loadingDoneHandler)
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

		private void LoadAsync(Action<MyInventoryBase> loadingDoneHandler)
		{
			MyInventoryBase component = null;
			MyInventoryBase myInventoryBase = null;
			if (MyEntities.TryGetEntityById(m_entityId, out MyEntity entity) && entity.Components.TryGet(out component) && component is MyInventoryAggregate)
			{
				myInventoryBase = (component as MyInventoryAggregate).GetInventory(m_inventoryId);
			}
			loadingDoneHandler(myInventoryBase ?? component);
		}

		public override void OnDestroyClient()
		{
			if (Inventory != null && Inventory.Entity != null)
			{
				((MyEntity)Inventory.Entity).OnClose -= m_destroyEntity;
			}
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
		}

		private void OnRemovedFromContainer()
		{
			if (Inventory != null && Inventory.Entity != null)
			{
				((MyEntity)Inventory.Entity).OnClose -= m_destroyEntity;
				MyCubeBlock myCubeBlock = Inventory.Entity as MyCubeBlock;
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
	}
}
