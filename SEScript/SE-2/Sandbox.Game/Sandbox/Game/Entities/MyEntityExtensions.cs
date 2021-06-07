using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World.Generator;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	public static class MyEntityExtensions
	{
		internal static void SetCallbacks()
		{
			MyEntity.AddToGamePruningStructureExtCallBack = AddToGamePruningStructure;
			MyEntity.RemoveFromGamePruningStructureExtCallBack = RemoveFromGamePruningStructure;
			MyEntity.UpdateGamePruningStructureExtCallBack = UpdateGamePruningStructure;
			MyEntity.MyEntityFactoryCreateObjectBuilderExtCallback = EntityFactoryCreateObjectBuilder;
			MyEntity.CreateDefaultSyncEntityExtCallback = CreateDefaultSyncEntity;
			MyEntity.MyWeldingGroupsGetGroupNodesExtCallback = GetWeldingGroupNodes;
			MyEntity.MyProceduralWorldGeneratorTrackEntityExtCallback = ProceduralWorldGeneratorTrackEntity;
			MyEntity.CreateStandardRenderComponentsExtCallback = CreateStandardRenderComponents;
			MyEntity.InitComponentsExtCallback = MyComponentContainerExtension.InitComponents;
			MyEntity.MyEntitiesCreateFromObjectBuilderExtCallback = MyEntities.CreateFromObjectBuilder;
		}

		public static MyPhysicsBody GetPhysicsBody(this MyEntity thisEntity)
		{
			return thisEntity.Physics as MyPhysicsBody;
		}

		public static void UpdateGamePruningStructure(this MyEntity thisEntity)
		{
			MyGamePruningStructure.Move(thisEntity);
		}

		public static void AddToGamePruningStructure(this MyEntity thisEntity)
		{
			MyGamePruningStructure.Add(thisEntity);
		}

		public static void RemoveFromGamePruningStructure(this MyEntity thisEntity)
		{
			MyGamePruningStructure.Remove(thisEntity);
		}

		public static MyObjectBuilder_EntityBase EntityFactoryCreateObjectBuilder(this MyEntity thisEntity)
		{
			return MyEntityFactory.CreateObjectBuilder(thisEntity);
		}

		public static MySyncComponentBase CreateDefaultSyncEntity(this MyEntity thisEntity)
		{
			return new MySyncEntity(thisEntity);
		}

		public static void AddNodeToWeldingGroups(this MyEntity thisEntity)
		{
			MyWeldingGroups.Static.AddNode(thisEntity);
		}

		public static void RemoveNodeFromWeldingGroups(this MyEntity thisEntity)
		{
			MyWeldingGroups.Static.RemoveNode(thisEntity);
		}

		public static void GetWeldingGroupNodes(this MyEntity thisEntity, List<MyEntity> result)
		{
			MyWeldingGroups.Static.GetGroupNodes(thisEntity, result);
		}

		public static bool WeldingGroupExists(this MyEntity thisEntity)
		{
			return MyWeldingGroups.Static.GetGroup(thisEntity) != null;
		}

		public static void ProceduralWorldGeneratorTrackEntity(this MyEntity thisEntity)
		{
			if (MyFakes.ENABLE_ASTEROID_FIELDS && MyProceduralWorldGenerator.Static != null)
			{
				MyProceduralWorldGenerator.Static.TrackEntity(thisEntity);
			}
		}

		public static bool TryGetInventory(this MyEntity thisEntity, out MyInventoryBase inventoryBase)
		{
			inventoryBase = null;
			return thisEntity.Components.TryGet(out inventoryBase);
		}

		public static bool TryGetInventory(this MyEntity thisEntity, out MyInventory inventory)
		{
			inventory = null;
			if (thisEntity.Components.Has<MyInventoryBase>())
			{
				inventory = thisEntity.GetInventory();
			}
			return inventory != null;
		}

		public static MyInventory GetInventory(this MyEntity thisEntity, int index = 0)
		{
			return thisEntity.GetInventoryBase(index) as MyInventory;
		}

		internal static void CreateStandardRenderComponents(this MyEntity thisEntity)
		{
			thisEntity.Render = new MyRenderComponent();
			thisEntity.AddDebugRenderComponent(new MyDebugRenderComponent(thisEntity));
		}
	}
}
