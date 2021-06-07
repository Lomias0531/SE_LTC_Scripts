using Havok;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Physics
{
	public static class MyPhysicsExtensions
	{
		[ThreadStatic]
		private static List<IMyEntity> m_entityList;

		private static List<IMyEntity> EntityList
		{
			get
			{
				if (m_entityList == null)
				{
					m_entityList = new List<IMyEntity>();
				}
				return m_entityList;
			}
		}

		public static void SetInBodySpace(this HkWheelConstraintData data, Vector3 posA, Vector3 posB, Vector3 axisA, Vector3 axisB, Vector3 suspension, Vector3 steering, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA.IsWelded)
			{
				posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
				axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
			}
			if (bodyB.IsWelded)
			{
				posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
				axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
				suspension = Vector3.TransformNormal(suspension, bodyB.WeldInfo.Transform);
				steering = Vector3.TransformNormal(steering, bodyB.WeldInfo.Transform);
			}
			data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB, ref suspension, ref steering);
		}

		public static void SetInBodySpace(this HkCustomWheelConstraintData data, Vector3 posA, Vector3 posB, Vector3 axisA, Vector3 axisB, Vector3 suspension, Vector3 steering, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA.IsWelded)
			{
				posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
				axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
			}
			if (bodyB.IsWelded)
			{
				posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
				axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
				suspension = Vector3.TransformNormal(suspension, bodyB.WeldInfo.Transform);
				steering = Vector3.TransformNormal(steering, bodyB.WeldInfo.Transform);
			}
			data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB, ref suspension, ref steering);
		}

		public static void SetInBodySpace(this HkLimitedHingeConstraintData data, Vector3 posA, Vector3 posB, Vector3 axisA, Vector3 axisB, Vector3 axisAPerp, Vector3 axisBPerp, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA.IsWelded)
			{
				posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
				axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
				axisAPerp = Vector3.TransformNormal(axisAPerp, bodyA.WeldInfo.Transform);
			}
			if (bodyB.IsWelded)
			{
				posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
				axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
				axisBPerp = Vector3.TransformNormal(axisBPerp, bodyB.WeldInfo.Transform);
			}
			data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB, ref axisAPerp, ref axisBPerp);
		}

		public static void SetInBodySpace(this HkHingeConstraintData data, Vector3 posA, Vector3 posB, Vector3 axisA, Vector3 axisB, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA.IsWelded)
			{
				posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
				axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
			}
			if (bodyB.IsWelded)
			{
				posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
				axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
			}
			data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB);
		}

		public static void SetInBodySpace(this HkPrismaticConstraintData data, Vector3 posA, Vector3 posB, Vector3 axisA, Vector3 axisB, Vector3 axisAPerp, Vector3 axisBPerp, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA.IsWelded)
			{
				posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
				axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
				axisAPerp = Vector3.TransformNormal(axisAPerp, bodyA.WeldInfo.Transform);
			}
			if (bodyB.IsWelded)
			{
				posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
				axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
				axisBPerp = Vector3.TransformNormal(axisBPerp, bodyB.WeldInfo.Transform);
			}
			data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB, ref axisAPerp, ref axisBPerp);
		}

		public static void SetInBodySpace(this HkFixedConstraintData data, Matrix pivotA, Matrix pivotB, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA != null && bodyA.IsWelded)
			{
				pivotA *= bodyA.WeldInfo.Transform;
			}
			if (bodyB != null && bodyB.IsWelded)
			{
				pivotB *= bodyB.WeldInfo.Transform;
			}
			data.SetInBodySpaceInternal(ref pivotA, ref pivotB);
		}

		public static void SetInBodySpace(this HkRopeConstraintData data, Vector3 pivotA, Vector3 pivotB, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA.IsWelded)
			{
				pivotA = Vector3.Transform(pivotA, bodyA.WeldInfo.Transform);
			}
			if (bodyB.IsWelded)
			{
				pivotB = Vector3.Transform(pivotB, bodyB.WeldInfo.Transform);
			}
			data.SetInBodySpaceInternal(ref pivotA, ref pivotB);
		}

		public static void SetInBodySpace(this HkBallAndSocketConstraintData data, Vector3 pivotA, Vector3 pivotB, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA.IsWelded)
			{
				pivotA = Vector3.Transform(pivotA, bodyA.WeldInfo.Transform);
			}
			if (bodyB.IsWelded)
			{
				pivotB = Vector3.Transform(pivotB, bodyB.WeldInfo.Transform);
			}
			data.SetInBodySpaceInternal(ref pivotA, ref pivotB);
		}

		public static void SetInBodySpace(this HkCogWheelConstraintData data, Vector3 pivotA, Vector3 rotationA, float radius1, Vector3 pivotB, Vector3 rotationB, float radius2, MyPhysicsBody bodyA, MyPhysicsBody bodyB)
		{
			if (bodyA.IsWelded)
			{
				pivotA = Vector3.Transform(pivotA, bodyA.WeldInfo.Transform);
				rotationA = Vector3.TransformNormal(rotationA, bodyA.WeldInfo.Transform);
			}
			if (bodyB.IsWelded)
			{
				pivotB = Vector3.Transform(pivotB, bodyB.WeldInfo.Transform);
				rotationB = Vector3.TransformNormal(rotationB, bodyB.WeldInfo.Transform);
			}
			data.SetInBodySpaceInternal(ref pivotA, ref rotationA, radius1, ref pivotB, ref rotationB, radius2);
		}

		public static MyPhysicsBody GetBody(this HkEntity hkEntity)
		{
			if (!(hkEntity != null))
			{
				return null;
			}
			return hkEntity.UserObject as MyPhysicsBody;
		}

		public static IMyEntity GetEntity(this HkEntity hkEntity, uint shapeKey)
		{
			MyPhysicsBody body = hkEntity.GetBody();
			if (body != null)
			{
				if (shapeKey == 0)
				{
					return body.Entity;
				}
				if (shapeKey > body.WeldInfo.Children.Count)
				{
					return body.Entity;
				}
				HkShape shape = body.RigidBody.GetShape().GetContainer().GetShape(shapeKey);
				if (shape.IsValid)
				{
					body = HkRigidBody.FromShape(shape).GetBody();
				}
			}
			return body?.Entity;
		}

		public static List<IMyEntity> GetAllEntities(this HkEntity hkEntity)
		{
			MyPhysicsBody body = hkEntity.GetBody();
			if (body != null)
			{
				EntityList.Add(body.Entity);
				foreach (MyPhysicsBody child in body.WeldInfo.Children)
				{
					EntityList.Add(child.Entity);
				}
			}
			return EntityList;
		}

		public static IMyEntity GetSingleEntity(this HkEntity hkEntity)
		{
			return hkEntity.GetBody()?.Entity;
		}

		public static MyPhysicsBody GetOtherPhysicsBody(this HkContactPointEvent eventInfo, IMyEntity sourceEntity)
		{
			MyPhysicsBody physicsBody = eventInfo.GetPhysicsBody(0);
			MyPhysicsBody physicsBody2 = eventInfo.GetPhysicsBody(1);
			IMyEntity myEntity = physicsBody?.Entity;
			_ = physicsBody2?.Entity;
			if (sourceEntity != myEntity)
			{
				return physicsBody;
			}
			return physicsBody2;
		}

		public static IMyEntity GetOtherEntity(this HkCollisionEvent eventInfo, IMyEntity sourceEntity)
		{
			bool AisThis;
			return eventInfo.GetOtherEntity(sourceEntity, out AisThis);
		}

		public static IMyEntity GetOtherEntity(this HkContactPointEvent eventInfo, IMyEntity sourceEntity)
		{
			bool AisThis;
			return eventInfo.Base.GetOtherEntity(sourceEntity, out AisThis);
		}

		public static IMyEntity GetOtherEntity(this HkContactPointEvent eventInfo, IMyEntity sourceEntity, out bool AisThis)
		{
			return eventInfo.Base.GetOtherEntity(sourceEntity, out AisThis);
		}

		public static IMyEntity GetOtherEntity(this HkCollisionEvent eventInfo, IMyEntity sourceEntity, out bool AisThis)
		{
			MyPhysicsBody physicsBody = eventInfo.GetPhysicsBody(0);
			MyPhysicsBody physicsBody2 = eventInfo.GetPhysicsBody(1);
			IMyEntity myEntity = physicsBody?.Entity;
			IMyEntity result = physicsBody2?.Entity;
			if (sourceEntity == myEntity)
			{
				AisThis = true;
				return result;
			}
			AisThis = false;
			return myEntity;
		}

		public static MyPhysicsBody GetPhysicsBody(this HkContactPointEvent eventInfo, int index)
		{
			return eventInfo.Base.GetPhysicsBody(index);
		}

		public static MyPhysicsBody GetPhysicsBody(this HkCollisionEvent eventInfo, int index)
		{
			HkRigidBody rigidBody = eventInfo.GetRigidBody(index);
			if (rigidBody == null)
			{
				return null;
			}
			MyPhysicsBody body = rigidBody.GetBody();
			_ = body?.IsWelded;
			return body;
		}

		public static IMyEntity GetHitEntity(this HkHitInfo hitInfo)
		{
			return hitInfo.Body.GetEntity(hitInfo.GetShapeKey(0));
		}

		public static float GetConvexRadius(this HkHitInfo hitInfo)
		{
			if (hitInfo.Body == null)
			{
				return 0f;
			}
			HkShape shape = hitInfo.Body.GetShape();
			for (int i = 0; i < hitInfo.ShapeKeyCount; i++)
			{
				uint shapeKey = hitInfo.GetShapeKey(i);
				if (-1 == (int)shapeKey || !shape.IsContainer())
				{
					break;
				}
			}
			if (shape.ShapeType == HkShapeType.ConvexTransform || shape.ShapeType == HkShapeType.ConvexTranslate || shape.ShapeType == HkShapeType.Transform)
			{
				shape = shape.GetContainer().GetShape(0u);
			}
			if (shape.ShapeType == HkShapeType.Sphere || shape.ShapeType == HkShapeType.Capsule)
			{
				return 0f;
			}
			if (!shape.IsConvex)
			{
				return HkConvexShape.DefaultConvexRadius;
			}
			return shape.ConvexRadius;
		}

		public static Vector3 GetFixedPosition(this MyPhysics.HitInfo hitInfo)
		{
			Vector3 result = hitInfo.Position;
			float convexRadius = hitInfo.HkHitInfo.GetConvexRadius();
			if (convexRadius != 0f)
			{
				result += -hitInfo.HkHitInfo.Normal * convexRadius;
			}
			return result;
		}

		public static IEnumerable<HkShape> GetAllShapes(this HkShape shape)
		{
			if (shape.IsContainer())
			{
				HkShapeContainerIterator iterator = shape.GetContainer();
				while (iterator.CurrentShapeKey != uint.MaxValue)
				{
					foreach (HkShape allShape in iterator.CurrentValue.GetAllShapes())
					{
						yield return allShape;
					}
					iterator.Next();
				}
			}
			else
			{
				yield return shape;
			}
		}

		public unsafe static HkShape GetHitShape(this HkShape shape, ref HkContactPointEvent contactEvent, int bodyIndex, bool checkMissingKeys = true, bool ImNotSureThatShapeKeysAreStillValid = false)
		{
			uint* ptr = stackalloc uint[4];
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				ptr[i] = contactEvent.GetShapeKey(bodyIndex, i);
				if (ptr[i] == uint.MaxValue)
				{
					break;
				}
				num++;
			}
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				uint num3 = ptr[num2];
				if (!shape.IsContainer())
				{
					shape = HkShape.Empty;
					break;
				}
				HkShapeContainerIterator container = shape.GetContainer();
				if (!container.IsValid)
				{
					shape = HkShape.Empty;
					break;
				}
				shape = ((!ImNotSureThatShapeKeysAreStillValid || container.IsShapeKeyValid(num3)) ? container.GetShape(num3) : HkShape.Empty);
				if (shape.IsZero)
				{
					return HkShape.Empty;
				}
			}
			_ = shape.IsZero;
			return shape;
		}

		public unsafe static uint GetHitTriangleMaterial(this MyVoxelPhysicsBody voxelBody, ref HkContactPointEvent contactEvent, int bodyIndex)
		{
			uint* ptr = stackalloc uint[4];
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				ptr[i] = contactEvent.GetShapeKey(bodyIndex, i);
				if (ptr[i] == uint.MaxValue)
				{
					break;
				}
				num++;
			}
			if (num == 2)
			{
				HkShape shape = voxelBody.GetShape();
				if (!shape.IsZero && shape.ShapeType == HkShapeType.BvTree)
				{
					shape = shape.GetContainer().GetShape(ptr[1]);
					if (!shape.IsZero && shape.ShapeType == HkShapeType.BvCompressedMesh)
					{
						uint num2 = (uint)shape.UserData;
						if (num2 != uint.MaxValue)
						{
							return num2;
						}
					}
				}
			}
			HkShape hitShape = voxelBody.GetShape().GetHitShape(ref contactEvent, bodyIndex, checkMissingKeys: false, ImNotSureThatShapeKeysAreStillValid: true);
			if (hitShape.IsZero)
			{
				return uint.MaxValue;
			}
			return (uint)hitShape.UserData;
		}

		public static IEnumerable<uint> GetShapeKeys(this HkShape shape)
		{
			if (shape.IsContainer())
			{
				HkShapeContainerIterator it = shape.GetContainer();
				while (it.IsValid)
				{
					yield return it.CurrentShapeKey;
					it.Next();
				}
			}
		}

		public static IMyEntity GetCollisionEntity(this HkBodyCollision collision)
		{
			if (!(collision.Body != null))
			{
				return null;
			}
			return collision.Body.GetEntity(0u);
		}

		public static bool IsInWorldWelded(this MyPhysicsBody body)
		{
			if (!body.IsInWorld)
			{
				if (body.WeldInfo.Parent != null)
				{
					return body.WeldInfo.Parent.IsInWorld;
				}
				return false;
			}
			return true;
		}

		public static bool IsInWorldWelded(this MyPhysicsComponentBase body)
		{
			if (body != null && body is MyPhysicsBody)
			{
				return ((MyPhysicsBody)body).IsInWorldWelded();
			}
			return false;
		}

		public static bool ActivateIfNeeded(this MyPhysicsComponentBase body)
		{
			if (body.IsActive || body.IsStatic)
			{
				return false;
			}
			body.ForceActivate();
			return true;
		}

		public static MyGridContactInfo.ContactFlags GetFlags(this HkContactPointProperties cp)
		{
			return (MyGridContactInfo.ContactFlags)cp.UserData.AsUint;
		}

		public static bool HasFlag(this HkContactPointProperties cp, MyGridContactInfo.ContactFlags flag)
		{
			return ((int)cp.UserData.AsUint & (int)flag) != 0;
		}

		public static void SetFlag(this HkContactPointProperties cp, MyGridContactInfo.ContactFlags flag)
		{
			MyGridContactInfo.ContactFlags asUint = (MyGridContactInfo.ContactFlags)cp.UserData.AsUint;
			asUint |= flag;
			cp.UserData = HkContactUserData.UInt((uint)asUint);
		}
	}
}
