using Sandbox.Game.Entities;
using Sandbox.Game.Replication.StateGroups;
using System;
using VRage.Game.ObjectBuilders;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Replication
{
	public class MyInventoryBagReplicable : MyEntityReplicableBase<MyInventoryBagEntity>
	{
		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			MyObjectBuilder_InventoryBagEntity value = (MyObjectBuilder_InventoryBagEntity)base.Instance.GetObjectBuilder();
			if (string.IsNullOrEmpty(value.SubtypeName))
			{
				return false;
			}
			if (MyInventoryBagEntity.GetPhysicsComponentBuilder(value) == null)
			{
				return false;
			}
			MySerializer.Write(stream, ref value, MyObjectBuilderSerializer.Dynamic);
			return true;
		}

		protected override void OnLoad(BitStream stream, Action<MyInventoryBagEntity> loadingDoneHandler)
		{
			MyObjectBuilder_InventoryBagEntity myObjectBuilder_InventoryBagEntity = (MyObjectBuilder_InventoryBagEntity)MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
			if (MyInventoryBagEntity.GetPhysicsComponentBuilder(myObjectBuilder_InventoryBagEntity) != null)
			{
				TryRemoveExistingEntity(myObjectBuilder_InventoryBagEntity.EntityId);
				MyInventoryBagEntity obj = (MyInventoryBagEntity)MyEntities.CreateFromObjectBuilderAndAdd(myObjectBuilder_InventoryBagEntity, fadeIn: false);
				loadingDoneHandler(obj);
			}
		}

		protected override IMyStateGroup CreatePhysicsGroup()
		{
			return new MyEntityPhysicsStateGroup(base.Instance, this);
		}
	}
}
