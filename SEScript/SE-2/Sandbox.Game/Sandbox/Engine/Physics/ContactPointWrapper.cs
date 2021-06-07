using Havok;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Physics
{
	public class ContactPointWrapper
	{
		public MyPhysicsBody bodyA;

		public MyPhysicsBody bodyB;

		public Vector3 position;

		public Vector3 normal;

		public IMyEntity entityA;

		public IMyEntity entityB;

		public float separatingVelocity;

		public Vector3D WorldPosition
		{
			get;
			set;
		}

		public ContactPointWrapper(ref HkContactPointEvent e)
		{
			bodyA = e.Base.BodyA.GetBody();
			bodyB = e.Base.BodyB.GetBody();
			position = e.ContactPoint.Position;
			normal = e.ContactPoint.Normal;
			MyPhysicsBody physicsBody = e.GetPhysicsBody(0);
			MyPhysicsBody physicsBody2 = e.GetPhysicsBody(1);
			entityA = physicsBody.Entity;
			entityB = physicsBody2.Entity;
			separatingVelocity = e.SeparatingVelocity;
		}

		public bool IsValid()
		{
			if (bodyA != null && bodyB != null)
			{
				_ = position;
				_ = normal;
				if (entityA != null)
				{
					return entityB != null;
				}
			}
			return false;
		}
	}
}
