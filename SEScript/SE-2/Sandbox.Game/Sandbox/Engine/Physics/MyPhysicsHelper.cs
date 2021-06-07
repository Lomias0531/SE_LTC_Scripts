using Havok;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Physics
{
	public static class MyPhysicsHelper
	{
		public static void InitSpherePhysics(this IMyEntity entity, MyStringHash materialType, Vector3 sphereCenter, float sphereRadius, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			mass = (((rbFlag & RigidBodyFlag.RBF_STATIC) != 0) ? 0f : mass);
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(entity, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			HkMassProperties value = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(sphereRadius, mass);
			HkSphereShape shape = new HkSphereShape(sphereRadius);
			myPhysicsBody.CreateFromCollisionObject(shape, sphereCenter, entity.PositionComp.WorldMatrix, value);
			shape.Base.RemoveReference();
			entity.Physics = myPhysicsBody;
		}

		public static void InitSpherePhysics(this IMyEntity entity, MyStringHash materialType, MyModel model, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			entity.InitSpherePhysics(materialType, model.BoundingSphere.Center, model.BoundingSphere.Radius, mass, linearDamping, angularDamping, collisionLayer, rbFlag);
		}

		public static void InitBoxPhysics(this IMyEntity entity, MyStringHash materialType, Vector3 center, Vector3 size, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			mass = (((rbFlag & RigidBodyFlag.RBF_STATIC) != 0) ? 0f : mass);
			HkMassProperties value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(size / 2f, mass);
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(entity, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			HkBoxShape shape = new HkBoxShape(size * 0.5f);
			myPhysicsBody.CreateFromCollisionObject(shape, center, entity.PositionComp.WorldMatrix, value);
			shape.Base.RemoveReference();
			entity.Physics = myPhysicsBody;
		}

		internal static void InitBoxPhysics(this IMyEntity entity, Matrix worldMatrix, MyStringHash materialType, Vector3 center, Vector3 size, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			mass = (((rbFlag & RigidBodyFlag.RBF_STATIC) != 0) ? 0f : mass);
			HkMassProperties value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(size / 2f, mass);
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(null, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			HkBoxShape shape = new HkBoxShape(size * 0.5f);
			myPhysicsBody.CreateFromCollisionObject(shape, center, worldMatrix, value);
			shape.Base.RemoveReference();
			entity.Physics = myPhysicsBody;
		}

		public static void InitBoxPhysics(this IMyEntity entity, MyStringHash materialType, MyModel model, float mass, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			Vector3 center = model.BoundingBox.Center;
			Vector3 boundingBoxSize = model.BoundingBoxSize;
			entity.InitBoxPhysics(materialType, center, boundingBoxSize, mass, 0f, angularDamping, collisionLayer, rbFlag);
		}

		public static void InitCharacterPhysics(this IMyEntity entity, MyStringHash materialType, Vector3 center, float characterWidth, float characterHeight, float crouchHeight, float ladderHeight, float headSize, float headHeight, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag, float mass, bool isOnlyVertical, float maxSlope, float maxImpulse, float maxSpeedRelativeToShip, bool networkProxy, float? maxForce)
		{
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(entity, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			myPhysicsBody.CreateCharacterCollision(center, characterWidth, characterHeight, crouchHeight, ladderHeight, headSize, headHeight, entity.PositionComp.WorldMatrix, mass, collisionLayer, isOnlyVertical, maxSlope, maxImpulse, maxSpeedRelativeToShip, networkProxy, maxForce);
			entity.Physics = myPhysicsBody;
		}

		public static void InitCapsulePhysics(this IMyEntity entity, MyStringHash materialType, Vector3 vertexA, Vector3 vertexB, float radius, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			mass = (((rbFlag & RigidBodyFlag.RBF_STATIC) != 0) ? 0f : mass);
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(entity, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			HkMassProperties value = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(radius, mass);
			myPhysicsBody.ReportAllContacts = true;
			HkCapsuleShape shape = new HkCapsuleShape(vertexA, vertexB, radius);
			myPhysicsBody.CreateFromCollisionObject(shape, (vertexA + vertexB) / 2f, entity.PositionComp.WorldMatrix, value);
			shape.Base.RemoveReference();
			entity.Physics = myPhysicsBody;
		}

		public static bool InitModelPhysics(this IMyEntity entity, RigidBodyFlag rbFlags = RigidBodyFlag.RBF_KINEMATIC, int collisionLayers = 17)
		{
			MyEntity myEntity = entity as MyEntity;
			if (myEntity.Closed)
			{
				return false;
			}
			if (myEntity.ModelCollision.HavokCollisionShapes != null && myEntity.ModelCollision.HavokCollisionShapes.Length != 0)
			{
				HkShape[] havokCollisionShapes = myEntity.ModelCollision.HavokCollisionShapes;
				HkListShape shape = new HkListShape(havokCollisionShapes, havokCollisionShapes.Length, HkReferencePolicy.None);
				myEntity.Physics = new MyPhysicsBody(myEntity, rbFlags);
				myEntity.Physics.IsPhantom = false;
				(myEntity.Physics as MyPhysicsBody).CreateFromCollisionObject(shape, Vector3D.Zero, myEntity.WorldMatrix, null, collisionLayers);
				myEntity.Physics.Enabled = true;
				shape.Base.RemoveReference();
				return true;
			}
			return false;
		}
	}
}
