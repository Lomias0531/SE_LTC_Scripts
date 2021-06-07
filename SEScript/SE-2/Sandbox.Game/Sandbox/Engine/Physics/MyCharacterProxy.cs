using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Engine.Physics
{
	public class MyCharacterProxy
	{
		public const float MAX_SPRINT_SPEED = 7f;

		private bool m_isDynamic;

		private Vector3 m_gravity;

		private bool m_jump;

		private float m_posX;

		private float m_posY;

		private Vector3 m_angularVelocity;

		private float m_speed;

		private float m_maxSpeedRelativeToShip = 7f;

		private int m_airFrameCounter;

		private float m_mass;

		private float m_maxImpulse;

		private float m_maxCharacterSpeedSq;

		private bool m_isCrouching;

		private HkCharacterProxy CharacterProxy;

		private HkSimpleShapePhantom CharacterPhantom;

		private HkShape m_characterShape = HkShape.Empty;

		private HkShape m_crouchShape = HkShape.Empty;

		private HkShape m_characterCollisionShape = HkShape.Empty;

		private MyPhysicsBody m_physicsBody;

		private bool m_flyingStateEnabled;

		private HkRigidBody m_oldRigidBody;

		public HkCharacterRigidBody CharacterRigidBody
		{
			get;
			private set;
		}

		public Vector3 LinearVelocity
		{
			get
			{
				if (m_isDynamic)
				{
					return CharacterRigidBody.LinearVelocity;
				}
				return CharacterProxy.LinearVelocity;
			}
			set
			{
				if (m_isDynamic)
				{
					CharacterRigidBody.LinearVelocity = value;
				}
				else
				{
					CharacterProxy.LinearVelocity = value;
				}
			}
		}

		public Vector3 Forward
		{
			get
			{
				if (m_isDynamic)
				{
					return CharacterRigidBody.Forward;
				}
				return CharacterProxy.Forward;
			}
		}

		public Vector3 Up
		{
			get
			{
				if (m_isDynamic)
				{
					return CharacterRigidBody.Up;
				}
				return CharacterProxy.Up;
			}
		}

		public Vector3 Gravity
		{
			get
			{
				return m_gravity;
			}
			set
			{
				m_gravity = value;
			}
		}

		public float Elevate
		{
			get
			{
				if (m_isDynamic)
				{
					return CharacterRigidBody.Elevate;
				}
				return 0f;
			}
			set
			{
				if (m_isDynamic)
				{
					CharacterRigidBody.Elevate = value;
				}
			}
		}

		public bool AtLadder
		{
			get
			{
				if (m_isDynamic)
				{
					return CharacterRigidBody.AtLadder;
				}
				return false;
			}
			set
			{
				if (m_isDynamic)
				{
					CharacterRigidBody.AtLadder = value;
				}
			}
		}

		public Vector3 ElevateVector
		{
			get
			{
				if (m_isDynamic)
				{
					return CharacterRigidBody.ElevateVector;
				}
				return Vector3.Zero;
			}
			set
			{
				if (m_isDynamic)
				{
					CharacterRigidBody.ElevateVector = value;
				}
			}
		}

		public Vector3 ElevateUpVector
		{
			get
			{
				if (m_isDynamic)
				{
					return CharacterRigidBody.ElevateUpVector;
				}
				return Vector3.Zero;
			}
			set
			{
				if (m_isDynamic)
				{
					CharacterRigidBody.ElevateUpVector = value;
				}
			}
		}

		public bool Jump
		{
			set
			{
				m_jump = value;
			}
		}

		public Vector3 Position
		{
			get
			{
				if (m_isDynamic)
				{
					return CharacterRigidBody.Position;
				}
				return CharacterProxy.Position;
			}
			set
			{
				if (m_isDynamic)
				{
					CharacterRigidBody.Position = value;
				}
				else
				{
					CharacterProxy.Position = value;
				}
			}
		}

		public float PosX
		{
			set
			{
				m_posX = MathHelper.Clamp(value, -1f, 1f);
			}
		}

		public float PosY
		{
			set
			{
				m_posY = MathHelper.Clamp(value, -1f, 1f);
			}
		}

		public Vector3 AngularVelocity
		{
			get
			{
				if (!(CharacterRigidBody != null))
				{
					return m_angularVelocity;
				}
				return CharacterRigidBody.GetAngularVelocity();
			}
			set
			{
				m_angularVelocity = value;
				if (CharacterRigidBody != null)
				{
					CharacterRigidBody.AngularVelocity = m_angularVelocity;
					CharacterRigidBody.SetAngularVelocity(m_angularVelocity);
				}
			}
		}

		public float Speed
		{
			get
			{
				return m_speed;
			}
			set
			{
				m_speed = value;
			}
		}

		public bool Supported
		{
			get;
			private set;
		}

		public Vector3 SupportNormal
		{
			get;
			private set;
		}

		public Vector3 GroundVelocity
		{
			get;
			private set;
		}

		public Vector3 GroundAngularVelocity
		{
			get;
			private set;
		}

		public bool IsCrouching => m_isCrouching;

		public bool ImmediateSetWorldTransform
		{
			get;
			set;
		}

		public bool ContactPointCallbackEnabled
		{
			get
			{
				if (CharacterRigidBody != null)
				{
					return CharacterRigidBody.ContactPointCallbackEnabled;
				}
				return false;
			}
			set
			{
				if (CharacterRigidBody != null)
				{
					CharacterRigidBody.ContactPointCallbackEnabled = value;
				}
			}
		}

		public float Mass => m_mass;

		public float MaxSpeedRelativeToShip => m_maxSpeedRelativeToShip;

		public float MaxSlope
		{
			get
			{
				if (CharacterRigidBody != null)
				{
					return CharacterRigidBody.MaxSlope;
				}
				return 0f;
			}
			set
			{
				if (CharacterRigidBody != null)
				{
					CharacterRigidBody.MaxSlope = value;
				}
			}
		}

		public event HkContactPointEventHandler ContactPointCallback;

		public static HkShape CreateCharacterShape(float height, float width, float headHeight, float headSize, float headForwardOffset, float downOffset = 0f, bool capsuleForHead = false)
		{
			HkCapsuleShape shape = new HkCapsuleShape(Vector3.Up * (height - downOffset) / 2f, Vector3.Down * height / 2f, width / 2f);
			if (headSize > 0f)
			{
				HkConvexShape childShape = (!capsuleForHead) ? ((HkConvexShape)new HkSphereShape(headSize)) : ((HkConvexShape)new HkCapsuleShape(new Vector3(0f, 0f, -0.3f), new Vector3(0f, 0f, 0.3f), headSize));
				HkShape[] obj = new HkShape[2]
				{
					shape,
					new HkConvexTranslateShape(childShape, Vector3.Up * (headHeight - downOffset) / 2f + Vector3.Forward * headForwardOffset, HkReferencePolicy.TakeOwnership)
				};
				return new HkListShape(obj, obj.Length, HkReferencePolicy.TakeOwnership);
			}
			return shape;
		}

		public MyCharacterProxy(bool isDynamic, bool isCapsule, float characterWidth, float characterHeight, float crouchHeight, float ladderHeight, float headSize, float headHeight, Vector3 position, Vector3 up, Vector3 forward, float mass, MyPhysicsBody body, bool isOnlyVertical, float maxSlope, float maxImpulse, float maxSpeedRelativeToShip, float? maxForce = null, HkRagdoll ragDoll = null)
		{
			m_isDynamic = isDynamic;
			m_physicsBody = body;
			m_mass = mass;
			m_maxImpulse = maxImpulse;
			m_maxSpeedRelativeToShip = maxSpeedRelativeToShip;
			if (isCapsule)
			{
				m_characterShape = CreateCharacterShape(characterHeight, characterWidth, characterHeight + headHeight, headSize, 0f);
				m_characterCollisionShape = CreateCharacterShape(characterHeight * 0.9f, characterWidth * 0.9f, characterHeight * 0.9f + headHeight, headSize * 0.9f, 0f);
				m_crouchShape = CreateCharacterShape(characterHeight, characterWidth, characterHeight + headHeight, headSize, 0f, 1f);
				if (!m_isDynamic)
				{
					CharacterPhantom = new HkSimpleShapePhantom(m_characterShape, 18);
				}
			}
			else
			{
				HkBoxShape shape = new HkBoxShape(new Vector3(characterWidth / 2f, characterHeight / 2f, characterWidth / 2f));
				if (!m_isDynamic)
				{
					CharacterPhantom = new HkSimpleShapePhantom(shape, 18);
				}
				m_characterShape = shape;
			}
			if (!m_isDynamic)
			{
				HkCharacterProxyCinfo hkCharacterProxyCinfo = new HkCharacterProxyCinfo
				{
					StaticFriction = 1f,
					DynamicFriction = 1f,
					ExtraDownStaticFriction = 1000f,
					MaxCharacterSpeedForSolver = 10000f,
					RefreshManifoldInCheckSupport = true,
					Up = up,
					Forward = forward,
					UserPlanes = 4,
					MaxSlope = MathHelper.ToRadians(maxSlope),
					Position = position,
					CharacterMass = mass,
					CharacterStrength = 100f,
					ShapePhantom = CharacterPhantom
				};
				CharacterProxy = new HkCharacterProxy(hkCharacterProxyCinfo);
				hkCharacterProxyCinfo.Dispose();
				return;
			}
			HkCharacterRigidBodyCinfo hkCharacterRigidBodyCinfo = new HkCharacterRigidBodyCinfo
			{
				Shape = m_characterShape,
				CrouchShape = m_crouchShape,
				Friction = 0f,
				MaxSlope = MathHelper.ToRadians(maxSlope),
				Up = up,
				Mass = mass,
				CollisionFilterInfo = 18,
				MaxLinearVelocity = 1000000f,
				MaxForce = (maxForce.HasValue ? maxForce.Value : 100000f),
				AllowedPenetrationDepth = (MyFakes.ENABLE_LIMITED_CHARACTER_BODY ? 0.3f : 0.1f),
				JumpHeight = 0.8f
			};
			float num = MyGridPhysics.ShipMaxLinearVelocity() + m_maxSpeedRelativeToShip;
			CharacterRigidBody = new HkCharacterRigidBody(hkCharacterRigidBodyCinfo, num, body);
			m_maxCharacterSpeedSq = num * num;
			CharacterRigidBody.GetRigidBody().ContactPointCallbackEnabled = true;
			CharacterRigidBody.GetRigidBody().ContactPointCallback -= RigidBody_ContactPointCallback;
			CharacterRigidBody.GetRigidBody().ContactPointCallback += RigidBody_ContactPointCallback;
			CharacterRigidBody.GetRigidBody().ContactPointCallbackDelay = 0;
			Matrix inertiaTensor = CharacterRigidBody.GetHitRigidBody().InertiaTensor;
			inertiaTensor.M11 = 1000f;
			inertiaTensor.M22 = 1000f;
			inertiaTensor.M33 = 1000f;
			CharacterRigidBody.GetHitRigidBody().InertiaTensor = inertiaTensor;
			hkCharacterRigidBodyCinfo.Dispose();
		}

		private void RigidBody_ContactPointCallback(ref HkContactPointEvent value)
		{
			if (this.ContactPointCallback != null)
			{
				this.ContactPointCallback(ref value);
			}
		}

		public void Dispose()
		{
			if (CharacterProxy != null)
			{
				CharacterProxy.Dispose();
				CharacterProxy = null;
			}
			if (CharacterPhantom != null)
			{
				CharacterPhantom.Dispose();
				CharacterPhantom = null;
			}
			if (CharacterRigidBody != null)
			{
				if (CharacterRigidBody.GetRigidBody() != null)
				{
					CharacterRigidBody.GetRigidBody().ContactPointCallback -= RigidBody_ContactPointCallback;
				}
				CharacterRigidBody.Dispose();
				CharacterRigidBody = null;
			}
			m_characterShape.RemoveReference();
			m_characterCollisionShape.RemoveReference();
			m_crouchShape.RemoveReference();
		}

		public void SetCollisionFilterInfo(uint info)
		{
			if (m_isDynamic)
			{
				CharacterRigidBody.SetCollisionFilterInfo(info);
			}
		}

		public void Activate(HkWorld world)
		{
			if (CharacterPhantom != null)
			{
				world.AddPhantom(CharacterPhantom);
			}
			if (CharacterRigidBody != null)
			{
				world.AddCharacterRigidBody(CharacterRigidBody);
				if (!float.IsInfinity(m_maxImpulse))
				{
					world.BreakOffPartsUtil.MarkEntityBreakable(CharacterRigidBody.GetRigidBody(), m_maxImpulse);
				}
			}
		}

		public void Deactivate(HkWorld world)
		{
			if (CharacterPhantom != null)
			{
				world.RemovePhantom(CharacterPhantom);
			}
			if (CharacterRigidBody != null)
			{
				world.RemoveCharacterRigidBody(CharacterRigidBody);
			}
		}

		public void SetForwardAndUp(Vector3 forward, Vector3 up)
		{
			Matrix m = GetRigidBodyTransform();
			m.Up = up;
			m.Forward = forward;
			m.Right = Vector3.Cross(forward, up);
			SetRigidBodyTransform(ref m);
		}

		public HkCharacterStateType GetState()
		{
			if (m_isDynamic)
			{
				HkCharacterStateType hkCharacterStateType = CharacterRigidBody.GetState();
				if (hkCharacterStateType != 0)
				{
					m_airFrameCounter++;
				}
				if (hkCharacterStateType == HkCharacterStateType.HK_CHARACTER_ON_GROUND)
				{
					m_airFrameCounter = 0;
				}
				if (hkCharacterStateType == HkCharacterStateType.HK_CHARACTER_IN_AIR && m_airFrameCounter < 8)
				{
					hkCharacterStateType = HkCharacterStateType.HK_CHARACTER_ON_GROUND;
				}
				if (AtLadder)
				{
					hkCharacterStateType = HkCharacterStateType.HK_CHARACTER_CLIMBING;
				}
				return hkCharacterStateType;
			}
			return CharacterProxy.GetState();
		}

		public void SetState(HkCharacterStateType state)
		{
			if (m_isDynamic)
			{
				CharacterRigidBody.SetState(state);
			}
			else
			{
				CharacterProxy.SetState(state);
			}
		}

		public void SetSupportedState(bool supported)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.SetPreviousSupportedState(supported);
			}
		}

		public void StepSimulation(float stepSizeInSeconds)
		{
			if (!AtLadder)
			{
				if (CharacterProxy != null)
				{
					CharacterProxy.PosX = m_posX;
					CharacterProxy.PosY = m_posY;
					CharacterProxy.Jump = m_jump;
					m_jump = false;
					CharacterProxy.Gravity = m_gravity;
					CharacterProxy.StepSimulation(stepSizeInSeconds);
				}
				if (CharacterRigidBody != null)
				{
					CharacterRigidBody.PosX = m_posX;
					CharacterRigidBody.PosY = m_posY;
					CharacterRigidBody.Jump = m_jump;
					m_jump = false;
					CharacterRigidBody.Gravity = m_gravity;
					CharacterRigidBody.Speed = Speed;
					CharacterRigidBody.StepSimulation(stepSizeInSeconds);
					CharacterRigidBody.Elevate = Elevate;
					Supported = CharacterRigidBody.Supported;
					SupportNormal = CharacterRigidBody.SupportNormal;
					GroundVelocity = CharacterRigidBody.GroundVelocity;
					GroundAngularVelocity = CharacterRigidBody.AngularVelocity;
				}
			}
		}

		public void UpdateSupport(float stepSizeInSeconds)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.UpdateSupport(stepSizeInSeconds);
				Supported = CharacterRigidBody.Supported;
				SupportNormal = CharacterRigidBody.SupportNormal;
				GroundVelocity = CharacterRigidBody.GroundVelocity;
			}
		}

		public void SkipSimulation(MatrixD mat)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.Position = mat.Translation;
				CharacterRigidBody.Forward = mat.Forward;
				CharacterRigidBody.Up = mat.Up;
				Supported = CharacterRigidBody.Supported;
				SupportNormal = CharacterRigidBody.SupportNormal;
				GroundVelocity = CharacterRigidBody.GroundVelocity;
			}
		}

		public void EnableFlyingState(bool enable)
		{
			float maxCharacterSpeed = MyGridPhysics.ShipMaxLinearVelocity() + m_maxSpeedRelativeToShip;
			float maxFlyingSpeed = MyGridPhysics.ShipMaxLinearVelocity() + m_maxSpeedRelativeToShip;
			float maxAcceleration = 9f;
			m_physicsBody.ShapeChangeInProgress = true;
			EnableFlyingState(enable, maxCharacterSpeed, maxFlyingSpeed, maxAcceleration);
			m_physicsBody.ShapeChangeInProgress = false;
		}

		public void EnableFlyingState(bool enable, float maxCharacterSpeed, float maxFlyingSpeed, float maxAcceleration)
		{
			if (m_flyingStateEnabled != enable)
			{
				if (CharacterRigidBody != null)
				{
					m_physicsBody.ShapeChangeInProgress = true;
					CharacterRigidBody.EnableFlyingState(enable, maxCharacterSpeed, maxFlyingSpeed, maxAcceleration);
					m_physicsBody.ShapeChangeInProgress = false;
				}
				StepSimulation(0.0166666675f);
				m_flyingStateEnabled = enable;
			}
		}

		public void EnableLadderState(bool enable)
		{
			EnableLadderState(enable, MyGridPhysics.ShipMaxLinearVelocity(), 1f);
		}

		public void EnableLadderState(bool enable, float maxCharacterSpeed, float maxAcceleration)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.EnableLadderState(enable, maxCharacterSpeed, maxAcceleration);
			}
		}

		public void SetShapeForCrouch(HkWorld world, bool enable)
		{
			if (CharacterRigidBody != null && world != null)
			{
				world.Lock();
				m_physicsBody.ShapeChangeInProgress = true;
				if (enable)
				{
					CharacterRigidBody.SetShapeForCrouch();
				}
				else
				{
					CharacterRigidBody.SetDefaultShape();
				}
				if (m_physicsBody.IsInWorld)
				{
					world.ReintegrateCharacter(CharacterRigidBody);
				}
				m_physicsBody.ShapeChangeInProgress = false;
				world.Unlock();
				m_isCrouching = enable;
			}
		}

		public void ApplyLinearImpulse(Vector3 impulse)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.ApplyLinearImpulse(impulse);
			}
		}

		public void ApplyAngularImpulse(Vector3 impulse)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.ApplyAngularImpulse(impulse);
			}
		}

		public void ApplyGravity(Vector3 gravity)
		{
			CharacterRigidBody.LinearVelocity += gravity * 0.0166666675f;
			if (CharacterRigidBody.LinearVelocity.LengthSquared() > m_maxCharacterSpeedSq)
			{
				Vector3 linearVelocity = CharacterRigidBody.LinearVelocity;
				float num = MyGridPhysics.ShipMaxLinearVelocity() + MaxSpeedRelativeToShip;
				linearVelocity.Normalize();
				linearVelocity *= num;
				CharacterRigidBody.LinearVelocity = linearVelocity;
			}
		}

		public void SetRigidBodyTransform(ref Matrix m)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.SetRigidBodyTransform(ref m);
			}
		}

		public HkShape GetShape()
		{
			return m_characterShape;
		}

		public HkShape GetCollisionShape()
		{
			return m_characterCollisionShape;
		}

		public void SetSupportDistance(float distance)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.SetSupportDistance(distance);
			}
		}

		public void SetHardSupportDistance(float distance)
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.SetHardSupportDistance(distance);
			}
		}

		public MyPhysicsBody GetPhysicsBody()
		{
			if (m_physicsBody != null)
			{
				return m_physicsBody;
			}
			return null;
		}

		public HkEntity GetRigidBody()
		{
			if (CharacterRigidBody != null)
			{
				return CharacterRigidBody.GetRigidBody();
			}
			return null;
		}

		public HkRigidBody GetHitRigidBody()
		{
			if (CharacterRigidBody != null)
			{
				return CharacterRigidBody.GetHitRigidBody();
			}
			return null;
		}

		public Matrix GetRigidBodyTransform()
		{
			if (CharacterRigidBody != null)
			{
				return CharacterRigidBody.GetRigidBodyTransform();
			}
			return Matrix.Identity;
		}

		public float CharacterFlyingMaxLinearVelocity()
		{
			return m_maxSpeedRelativeToShip + MyGridPhysics.ShipMaxLinearVelocity();
		}

		public float CharacterWalkingMaxLinearVelocity()
		{
			return m_maxSpeedRelativeToShip + MyGridPhysics.ShipMaxLinearVelocity();
		}

		public void GetSupportingEntities(List<MyEntity> outEntities)
		{
			if (!(CharacterRigidBody == null))
			{
				foreach (HkRigidBody item in CharacterRigidBody.GetSupportInfo())
				{
					MyPhysicsBody myPhysicsBody = (MyPhysicsBody)item.UserObject;
					if (myPhysicsBody != null)
					{
						MyEntity myEntity = (MyEntity)myPhysicsBody.Entity;
						if (myEntity != null && !myEntity.MarkedForClose)
						{
							outEntities.Add(myEntity);
						}
					}
				}
			}
		}

		public void Stand()
		{
			if (CharacterRigidBody != null)
			{
				CharacterRigidBody.ResetSurfaceVelocity();
			}
		}
	}
}
