using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MySessionCompatHelper
	{
		public virtual void FixSessionComponentObjectBuilders(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
		{
		}

		public virtual void FixSessionObjectBuilders(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
		{
		}

		public virtual void AfterEntitiesLoad(int saveVersion)
		{
		}

		public virtual void CheckAndFixPrefab(MyObjectBuilder_Definitions prefab)
		{
		}

		protected MyObjectBuilder_EntityBase ConvertBuilderToEntityBase(MyObjectBuilder_EntityBase origEntity, string subTypeNamePrefix)
		{
			string text = (!string.IsNullOrEmpty(origEntity.SubtypeName)) ? origEntity.SubtypeName : (origEntity.EntityDefinitionId.HasValue ? origEntity.EntityDefinitionId.Value.SubtypeName : null);
			if (text == null)
			{
				return null;
			}
			string subtypeName = (subTypeNamePrefix != null) ? subTypeNamePrefix : (text ?? "");
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyObjectBuilderSerializer.CreateNewObject(typeof(MyObjectBuilder_EntityBase), subtypeName) as MyObjectBuilder_EntityBase;
			myObjectBuilder_EntityBase.EntityId = origEntity.EntityId;
			myObjectBuilder_EntityBase.PersistentFlags = origEntity.PersistentFlags;
			myObjectBuilder_EntityBase.Name = origEntity.Name;
			myObjectBuilder_EntityBase.PositionAndOrientation = origEntity.PositionAndOrientation;
			myObjectBuilder_EntityBase.ComponentContainer = origEntity.ComponentContainer;
			if (myObjectBuilder_EntityBase.ComponentContainer != null && myObjectBuilder_EntityBase.ComponentContainer.Components.Count > 0)
			{
				foreach (MyObjectBuilder_ComponentContainer.ComponentData component in myObjectBuilder_EntityBase.ComponentContainer.Components)
				{
					if (!string.IsNullOrEmpty(component.Component.SubtypeName) && component.Component.SubtypeName == text)
					{
						component.Component.SubtypeName = subtypeName;
					}
				}
				return myObjectBuilder_EntityBase;
			}
			return myObjectBuilder_EntityBase;
		}

		private MyObjectBuilder_EntityBase ConvertInventoryBagToEntityBase(MyObjectBuilder_EntityBase oldBagBuilder, Vector3 linearVelocity, Vector3 angularVelocity)
		{
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = ConvertBuilderToEntityBase(oldBagBuilder, null);
			if (myObjectBuilder_EntityBase == null)
			{
				return null;
			}
			if (myObjectBuilder_EntityBase.ComponentContainer == null)
			{
				myObjectBuilder_EntityBase.ComponentContainer = (MyObjectBuilderSerializer.CreateNewObject(typeof(MyObjectBuilder_ComponentContainer), myObjectBuilder_EntityBase.SubtypeName) as MyObjectBuilder_ComponentContainer);
			}
			foreach (MyObjectBuilder_ComponentContainer.ComponentData component in myObjectBuilder_EntityBase.ComponentContainer.Components)
			{
				if (component.Component is MyObjectBuilder_PhysicsComponentBase)
				{
					return myObjectBuilder_EntityBase;
				}
			}
			MyObjectBuilder_PhysicsComponentBase myObjectBuilder_PhysicsComponentBase = MyObjectBuilderSerializer.CreateNewObject(typeof(MyObjectBuilder_PhysicsBodyComponent), myObjectBuilder_EntityBase.SubtypeName) as MyObjectBuilder_PhysicsComponentBase;
			myObjectBuilder_EntityBase.ComponentContainer.Components.Add(new MyObjectBuilder_ComponentContainer.ComponentData
			{
				Component = myObjectBuilder_PhysicsComponentBase,
				TypeId = typeof(MyPhysicsComponentBase).Name
			});
			myObjectBuilder_PhysicsComponentBase.LinearVelocity = linearVelocity;
			myObjectBuilder_PhysicsComponentBase.AngularVelocity = angularVelocity;
			return myObjectBuilder_EntityBase;
		}

		protected MyObjectBuilder_EntityBase ConvertInventoryBagToEntityBase(MyObjectBuilder_EntityBase oldBuilder)
		{
			MyObjectBuilder_ReplicableEntity myObjectBuilder_ReplicableEntity = oldBuilder as MyObjectBuilder_ReplicableEntity;
			if (myObjectBuilder_ReplicableEntity != null)
			{
				return ConvertInventoryBagToEntityBase(myObjectBuilder_ReplicableEntity, myObjectBuilder_ReplicableEntity.LinearVelocity, myObjectBuilder_ReplicableEntity.AngularVelocity);
			}
			MyObjectBuilder_InventoryBagEntity myObjectBuilder_InventoryBagEntity = oldBuilder as MyObjectBuilder_InventoryBagEntity;
			if (myObjectBuilder_InventoryBagEntity != null)
			{
				return ConvertInventoryBagToEntityBase(myObjectBuilder_InventoryBagEntity, myObjectBuilder_InventoryBagEntity.LinearVelocity, myObjectBuilder_InventoryBagEntity.AngularVelocity);
			}
			return null;
		}
	}
}
