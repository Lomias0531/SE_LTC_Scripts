using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentType(typeof(MyEntityInventorySpawnComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_InventorySpawnComponent), true)]
	public class MyEntityInventorySpawnComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_EntityComponents_MyEntityInventorySpawnComponent_003C_003EActor : IActivator, IActivator<MyEntityInventorySpawnComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityInventorySpawnComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityInventorySpawnComponent CreateInstance()
			{
				return new MyEntityInventorySpawnComponent();
			}

			MyEntityInventorySpawnComponent IActivator<MyEntityInventorySpawnComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyDefinitionId m_containerDefinition;

		public override string ComponentTypeDebugString => "Inventory Spawn Component";

		public bool SpawnInventoryContainer(bool spawnAboveEntity = true)
		{
			if (MySession.Static == null || !MySession.Static.Ready)
			{
				return false;
			}
			MyEntity myEntity = base.Entity as MyEntity;
			for (int i = 0; i < myEntity.InventoryCount; i++)
			{
				MyInventory inventory = myEntity.GetInventory(i);
				if (inventory == null || inventory.GetItemsCount() <= 0)
				{
					continue;
				}
				MyEntity myEntity2 = base.Entity as MyEntity;
				MatrixD worldMatrix = myEntity2.WorldMatrix;
				if (spawnAboveEntity)
				{
					Vector3 vector = -MyGravityProviderSystem.CalculateNaturalGravityInPoint(myEntity2.PositionComp.GetPosition());
					if (vector == Vector3.Zero)
					{
						vector = Vector3.Up;
					}
					vector.Normalize();
					Vector3 vector2 = Vector3.CalculatePerpendicularVector(vector);
					Vector3D translation = worldMatrix.Translation;
					BoundingBoxD box = myEntity2.PositionComp.WorldAABB;
					for (int j = 0; j < 20; j++)
					{
						Vector3D value = translation + 0.1f * (float)j * vector + 0.1f * (float)j * vector2;
						if (!new BoundingBoxD(value - 0.25 * Vector3D.One, value + 0.25 * Vector3D.One).Intersects(ref box))
						{
							worldMatrix.Translation = value + 0.25f * vector;
							break;
						}
					}
					if (worldMatrix.Translation == translation)
					{
						worldMatrix.Translation += vector + vector2;
					}
				}
				else
				{
					MyModel myModel = myEntity2.Render.ModelStorage as MyModel;
					if (myModel != null)
					{
						Vector3 v = Vector3.Transform(myModel.BoundingBox.Center, worldMatrix);
						worldMatrix.Translation = v;
					}
				}
				if (!MyComponentContainerExtension.TryGetContainerDefinition(m_containerDefinition.TypeId, m_containerDefinition.SubtypeId, out MyContainerDefinition definition))
				{
					return false;
				}
				MyEntity myEntity3 = MyEntities.CreateFromComponentContainerDefinitionAndAdd(definition.Id, fadeIn: false);
				if (myEntity3 == null)
				{
					return false;
				}
				myEntity3.PositionComp.SetWorldMatrix(worldMatrix);
				if (myEntity2.InventoryCount == 1)
				{
					myEntity2.Components.Remove<MyInventoryBase>();
				}
				else
				{
					MyInventoryAggregate myInventoryAggregate = myEntity2.GetInventoryBase() as MyInventoryAggregate;
					if (myInventoryAggregate == null)
					{
						return false;
					}
					myInventoryAggregate.RemoveComponent(inventory);
				}
				myEntity3.Components.Add((MyInventoryBase)inventory);
				inventory.RemoveEntityOnEmpty = true;
				myEntity3.Physics.LinearVelocity = Vector3.Zero;
				myEntity3.Physics.AngularVelocity = Vector3.Zero;
				if (myEntity.Physics != null)
				{
					myEntity3.Physics.LinearVelocity = myEntity.Physics.LinearVelocity;
					myEntity3.Physics.AngularVelocity = myEntity.Physics.AngularVelocity;
				}
				else if (myEntity is MyCubeBlock)
				{
					MyCubeGrid cubeGrid = (myEntity as MyCubeBlock).CubeGrid;
					if (cubeGrid.Physics != null)
					{
						myEntity3.Physics.LinearVelocity = cubeGrid.Physics.LinearVelocity;
						myEntity3.Physics.AngularVelocity = cubeGrid.Physics.AngularVelocity;
					}
				}
				return true;
			}
			return false;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (Sync.IsServer)
			{
				base.Entity.OnClosing += OnEntityClosing;
			}
		}

		private void OnEntityClosing(IMyEntity obj)
		{
			MyEntity myEntity = obj as MyEntity;
			if (myEntity.HasInventory && myEntity.InScene)
			{
				SpawnInventoryContainer();
			}
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (Sync.IsServer)
			{
				base.Entity.OnClosing -= OnEntityClosing;
			}
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			MyEntityInventorySpawnComponent_Definition myEntityInventorySpawnComponent_Definition = definition as MyEntityInventorySpawnComponent_Definition;
			m_containerDefinition = myEntityInventorySpawnComponent_Definition.ContainerDefinition;
		}
	}
}
