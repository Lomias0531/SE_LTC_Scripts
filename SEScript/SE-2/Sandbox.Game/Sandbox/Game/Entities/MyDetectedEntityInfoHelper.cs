using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public static class MyDetectedEntityInfoHelper
	{
		public static MyDetectedEntityInfo Create(MyEntity entity, long sensorOwner, Vector3D? hitPosition = null)
		{
			if (entity == null)
			{
				return default(MyDetectedEntityInfo);
			}
			MatrixD orientation = MatrixD.Zero;
			Vector3 velocity = Vector3D.Zero;
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			BoundingBoxD worldAABB = entity.PositionComp.WorldAABB;
			if (entity.Physics != null)
			{
				orientation = entity.Physics.GetWorldMatrix().GetOrientation();
				velocity = entity.Physics.LinearVelocity;
			}
			MyCubeGrid myCubeGrid = entity.GetTopMostParent() as MyCubeGrid;
			MyRelationsBetweenPlayerAndBlock myRelationsBetweenPlayerAndBlock;
			if (myCubeGrid != null)
			{
				MyDetectedEntityType type = (myCubeGrid.GridSizeEnum != MyCubeSize.Small) ? MyDetectedEntityType.LargeGrid : MyDetectedEntityType.SmallGrid;
				myRelationsBetweenPlayerAndBlock = ((myCubeGrid.BigOwners.Count != 0) ? MyIDModule.GetRelationPlayerBlock(sensorOwner, myCubeGrid.BigOwners[0], MyOwnershipShareModeEnum.Faction) : MyRelationsBetweenPlayerAndBlock.NoOwnership);
				string name = (myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.Owner || myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.FactionShare || myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.Friends) ? myCubeGrid.DisplayName : ((myCubeGrid.GridSizeEnum != MyCubeSize.Small) ? MyTexts.GetString(MySpaceTexts.DetectedEntity_LargeGrid) : MyTexts.GetString(MySpaceTexts.DetectedEntity_SmallGrid));
				orientation = myCubeGrid.WorldMatrix.GetOrientation();
				velocity = myCubeGrid.Physics.LinearVelocity;
				return new MyDetectedEntityInfo(myCubeGrid.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, worldAABB, totalGamePlayTimeInMilliseconds);
			}
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				MyDetectedEntityType type = (!myCharacter.IsPlayer) ? MyDetectedEntityType.CharacterOther : MyDetectedEntityType.CharacterHuman;
				myRelationsBetweenPlayerAndBlock = MyIDModule.GetRelationPlayerBlock(sensorOwner, myCharacter.GetPlayerIdentityId(), MyOwnershipShareModeEnum.Faction);
				string name = (myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.Owner || myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.FactionShare || myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.Friends) ? myCharacter.DisplayNameText : ((!myCharacter.IsPlayer) ? MyTexts.GetString(MySpaceTexts.DetectedEntity_CharacterOther) : MyTexts.GetString(MySpaceTexts.DetectedEntity_CharacterHuman));
				BoundingBoxD boundingBox = myCharacter.Model.BoundingBox.Transform(myCharacter.WorldMatrix);
				return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, boundingBox, totalGamePlayTimeInMilliseconds);
			}
			myRelationsBetweenPlayerAndBlock = MyRelationsBetweenPlayerAndBlock.Neutral;
			MyFloatingObject myFloatingObject = entity as MyFloatingObject;
			if (myFloatingObject != null)
			{
				MyDetectedEntityType type = MyDetectedEntityType.FloatingObject;
				string name = myFloatingObject.Item.Content.SubtypeName;
				return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, worldAABB, totalGamePlayTimeInMilliseconds);
			}
			MyInventoryBagEntity myInventoryBagEntity = entity as MyInventoryBagEntity;
			if (myInventoryBagEntity != null)
			{
				MyDetectedEntityType type = MyDetectedEntityType.FloatingObject;
				string name = myInventoryBagEntity.DisplayName;
				return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, worldAABB, totalGamePlayTimeInMilliseconds);
			}
			MyPlanet myPlanet = entity as MyPlanet;
			if (myPlanet != null)
			{
				MyDetectedEntityType type = MyDetectedEntityType.Planet;
				string name = MyTexts.GetString(MySpaceTexts.DetectedEntity_Planet);
				worldAABB = BoundingBoxD.CreateFromSphere(new BoundingSphereD(myPlanet.PositionComp.GetPosition(), myPlanet.MaximumRadius));
				return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, worldAABB, totalGamePlayTimeInMilliseconds);
			}
			MyVoxelPhysics myVoxelPhysics = entity as MyVoxelPhysics;
			if (myVoxelPhysics != null)
			{
				MyDetectedEntityType type = MyDetectedEntityType.Planet;
				string name = MyTexts.GetString(MySpaceTexts.DetectedEntity_Planet);
				worldAABB = BoundingBoxD.CreateFromSphere(new BoundingSphereD(myVoxelPhysics.Parent.PositionComp.GetPosition(), myVoxelPhysics.Parent.MaximumRadius));
				return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, worldAABB, totalGamePlayTimeInMilliseconds);
			}
			if (entity is MyVoxelMap)
			{
				MyDetectedEntityType type = MyDetectedEntityType.Asteroid;
				string name = MyTexts.GetString(MySpaceTexts.DetectedEntity_Asteroid);
				return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, worldAABB, totalGamePlayTimeInMilliseconds);
			}
			if (entity is MyMeteor)
			{
				MyDetectedEntityType type = MyDetectedEntityType.Meteor;
				string name = MyTexts.GetString(MySpaceTexts.DetectedEntity_Meteor);
				return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, worldAABB, totalGamePlayTimeInMilliseconds);
			}
			if (entity is MyMissile)
			{
				MyDetectedEntityType type = MyDetectedEntityType.Missile;
				string name = entity.DisplayName;
				return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation, velocity, myRelationsBetweenPlayerAndBlock, worldAABB, totalGamePlayTimeInMilliseconds);
			}
			return new MyDetectedEntityInfo(0L, string.Empty, MyDetectedEntityType.Unknown, null, default(MatrixD), default(Vector3), MyRelationsBetweenPlayerAndBlock.NoOwnership, default(BoundingBoxD), MySandboxGame.TotalGamePlayTimeInMilliseconds);
		}
	}
}
