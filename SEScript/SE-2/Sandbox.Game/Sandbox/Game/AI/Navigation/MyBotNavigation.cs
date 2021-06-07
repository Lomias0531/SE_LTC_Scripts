using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
	public class MyBotNavigation
	{
		private List<MySteeringBase> m_steerings;

		private MyPathSteering m_path;

		private MyBotAiming m_aiming;

		private MyEntity m_entity;

		private MyDestinationSphere m_destinationSphere;

		private Vector3 m_forwardVector;

		private Vector3 m_correction;

		private Vector3 m_upVector;

		private float m_speed;

		private bool m_wasStopped;

		private float m_rotationSpeedModifier;

		private Vector3 m_gravityDirection;

		private float? m_maximumRotationAngle;

		private MyStuckDetection m_stuckDetection;

		private MatrixD m_worldMatrix;

		private MatrixD m_invWorldMatrix;

		private MatrixD m_aimingPositionAndOrientation;

		private MatrixD m_invAimingPositionAndOrientation;

		public Vector3 ForwardVector => m_forwardVector;

		public Vector3 UpVector => m_upVector;

		public float Speed => m_speed;

		public bool Navigating => m_path.TargetSet;

		public bool Stuck => m_stuckDetection.IsStuck;

		public Vector3D TargetPoint => m_destinationSphere.GetDestination();

		public MyEntity BotEntity => m_entity;

		public float? MaximumRotationAngle
		{
			get
			{
				return m_maximumRotationAngle;
			}
			set
			{
				m_maximumRotationAngle = value;
			}
		}

		public Vector3 GravityDirection => m_gravityDirection;

		public MatrixD PositionAndOrientation
		{
			get
			{
				if (m_entity == null)
				{
					return MatrixD.Identity;
				}
				return m_worldMatrix;
			}
		}

		public MatrixD PositionAndOrientationInverted
		{
			get
			{
				if (m_entity == null)
				{
					return MatrixD.Identity;
				}
				return m_invWorldMatrix;
			}
		}

		public MatrixD AimingPositionAndOrientation
		{
			get
			{
				if (m_entity == null)
				{
					return MatrixD.Identity;
				}
				return m_aimingPositionAndOrientation;
			}
		}

		public MatrixD AimingPositionAndOrientationInverted
		{
			get
			{
				if (m_entity == null)
				{
					return MatrixD.Identity;
				}
				return m_invAimingPositionAndOrientation;
			}
		}

		public bool HasRotation(float epsilon = 0.0316f)
		{
			return m_aiming.RotationHint.LengthSquared() > epsilon * epsilon;
		}

		public bool HasXRotation(float epsilon)
		{
			return Math.Abs(m_aiming.RotationHint.Y) > epsilon;
		}

		public bool HasYRotation(float epsilon)
		{
			return Math.Abs(m_aiming.RotationHint.X) > epsilon;
		}

		public MyBotNavigation()
		{
			m_steerings = new List<MySteeringBase>();
			m_path = new MyPathSteering(this);
			m_steerings.Add(m_path);
			m_aiming = new MyBotAiming(this);
			m_stuckDetection = new MyStuckDetection(0.05f, MathHelper.ToRadians(2f));
			m_destinationSphere = new MyDestinationSphere(ref Vector3D.Zero, 0f);
			m_wasStopped = false;
		}

		public void Cleanup()
		{
			foreach (MySteeringBase steering in m_steerings)
			{
				steering.Cleanup();
			}
		}

		public void ChangeEntity(IMyControllableEntity newEntity)
		{
			m_entity = newEntity?.Entity;
			if (m_entity != null)
			{
				m_forwardVector = PositionAndOrientation.Forward;
				m_upVector = PositionAndOrientation.Up;
				m_speed = 0f;
				m_rotationSpeedModifier = 1f;
			}
		}

		public void Update(int behaviorTicks)
		{
			m_stuckDetection.SetCurrentTicks(behaviorTicks);
			if (m_entity != null)
			{
				UpdateMatrices();
				m_gravityDirection = MyGravityProviderSystem.CalculateTotalGravityInPoint(m_entity.PositionComp.WorldMatrix.Translation);
				if (!Vector3.IsZero(m_gravityDirection, 0.01f))
				{
					m_gravityDirection = Vector3D.Normalize(m_gravityDirection);
				}
				if (MyPerGameSettings.NavmeshPresumesDownwardGravity)
				{
					m_upVector = Vector3.Up;
				}
				else
				{
					m_upVector = -m_gravityDirection;
				}
				if (!m_speed.IsValid())
				{
					m_forwardVector = PositionAndOrientation.Forward;
					m_speed = 0f;
					m_rotationSpeedModifier = 1f;
				}
				foreach (MySteeringBase steering in m_steerings)
				{
					steering.Update();
				}
				m_aiming.Update();
				CorrectMovement(m_aiming.RotationHint);
				if (m_speed < 0.1f)
				{
					m_speed = 0f;
				}
				MoveCharacter();
			}
		}

		private void UpdateMatrices()
		{
			if (m_entity is MyCharacter)
			{
				MyCharacter myCharacter = m_entity as MyCharacter;
				m_worldMatrix = myCharacter.WorldMatrix;
				m_invWorldMatrix = Matrix.Invert(m_worldMatrix);
				m_aimingPositionAndOrientation = myCharacter.GetHeadMatrix(includeY: true, includeX: true, forceHeadAnim: false, forceHeadBone: true);
				m_invAimingPositionAndOrientation = MatrixD.Invert(m_aimingPositionAndOrientation);
			}
			else
			{
				m_worldMatrix = m_entity.PositionComp.WorldMatrix;
				m_invWorldMatrix = m_entity.PositionComp.WorldMatrixInvScaled;
				m_aimingPositionAndOrientation = m_worldMatrix;
				m_invAimingPositionAndOrientation = m_invWorldMatrix;
			}
		}

		private void AccumulateCorrection()
		{
			m_rotationSpeedModifier = 1f;
			float weight = 0f;
			for (int i = 0; i < m_steerings.Count; i++)
			{
				m_steerings[i].AccumulateCorrection(ref m_correction, ref weight);
			}
			if (m_maximumRotationAngle.HasValue)
			{
				double num = Math.Cos(m_maximumRotationAngle.Value);
				double num2 = Vector3D.Dot(Vector3D.Normalize(m_forwardVector - m_correction), m_forwardVector);
				if (num2 < num)
				{
					float num3 = (float)Math.Acos(MathHelper.Clamp(num2, -1.0, 1.0));
					m_rotationSpeedModifier = num3 / m_maximumRotationAngle.Value;
					m_correction /= m_rotationSpeedModifier;
				}
			}
			if (weight > 1f)
			{
				m_correction /= weight;
			}
		}

		private void CorrectMovement(Vector3 rotationHint)
		{
			m_correction = Vector3.Zero;
			if (!Navigating)
			{
				m_speed = 0f;
				return;
			}
			AccumulateCorrection();
			if (HasRotation(10f))
			{
				m_correction = Vector3.Zero;
				m_speed = 0f;
				m_stuckDetection.SetRotating(rotating: true);
			}
			else
			{
				m_stuckDetection.SetRotating(rotating: false);
			}
			Vector3 value = m_forwardVector * m_speed;
			value += m_correction;
			m_speed = value.Length();
			if (m_speed <= 0.001f)
			{
				m_speed = 0f;
				return;
			}
			m_forwardVector = value / m_speed;
			if (m_speed > 1f)
			{
				m_speed = 1f;
			}
		}

		private void MoveCharacter()
		{
			MyCharacter myCharacter = m_entity as MyCharacter;
			if (myCharacter != null)
			{
				if (m_speed != 0f)
				{
					MyCharacterJetpackComponent jetpackComp = myCharacter.JetpackComp;
					if (jetpackComp != null && !jetpackComp.TurnedOn && m_path.Flying)
					{
						jetpackComp.TurnOnJetpack(newState: true);
					}
					else if (jetpackComp != null && jetpackComp.TurnedOn && !m_path.Flying)
					{
						jetpackComp.TurnOnJetpack(newState: false);
					}
					Vector3 value = Vector3.TransformNormal(m_forwardVector, myCharacter.PositionComp.WorldMatrixNormalizedInv);
					Vector3 vector = m_aiming.RotationHint * m_rotationSpeedModifier;
					if (m_path.Flying)
					{
						if (value.Y > 0f)
						{
							myCharacter.Up();
						}
						else
						{
							myCharacter.Down();
						}
					}
					myCharacter.MoveAndRotate(value * m_speed, new Vector2(vector.Y * 30f, vector.X * 30f), 0f);
				}
				else if (m_speed == 0f)
				{
					if (HasRotation())
					{
						float num = (myCharacter.WantsWalk || myCharacter.IsCrouching) ? 1 : 2;
						Vector3 vector2 = m_aiming.RotationHint * m_rotationSpeedModifier;
						myCharacter.MoveAndRotate(Vector3.Zero, new Vector2(vector2.Y * 20f * num, vector2.X * 25f * num), 0f);
						m_wasStopped = false;
					}
					else if (m_wasStopped)
					{
						myCharacter.MoveAndRotate(Vector3.Zero, Vector2.Zero, 0f);
						m_wasStopped = true;
					}
				}
			}
			m_stuckDetection.Update(m_worldMatrix.Translation, m_aiming.RotationHint);
		}

		public void AddSteering(MySteeringBase steering)
		{
			m_steerings.Add(steering);
		}

		public void RemoveSteering(MySteeringBase steering)
		{
			m_steerings.Remove(steering);
		}

		public bool HasSteeringOfType(Type steeringType)
		{
			foreach (MySteeringBase steering in m_steerings)
			{
				if (steering.GetType() == steeringType)
				{
					return true;
				}
			}
			return false;
		}

		public void Goto(Vector3D position, float radius = 0f, MyEntity relativeEntity = null)
		{
			m_destinationSphere.Init(ref position, radius);
			Goto(m_destinationSphere, relativeEntity);
		}

		public void Goto(IMyDestinationShape destination, MyEntity relativeEntity = null)
		{
			if (MyAIComponent.Static.Pathfinding != null)
			{
				IMyPath myPath = MyAIComponent.Static.Pathfinding.FindPathGlobal(PositionAndOrientation.Translation, destination, relativeEntity);
				if (myPath != null)
				{
					m_path.SetPath(myPath);
					m_stuckDetection.Reset();
				}
			}
		}

		public void GotoNoPath(Vector3D worldPosition, float radius = 0f, MyEntity relativeEntity = null, bool resetStuckDetection = true)
		{
			m_path.SetTarget(worldPosition, radius, relativeEntity);
			if (resetStuckDetection)
			{
				m_stuckDetection.Reset();
			}
		}

		public bool CheckReachability(Vector3D worldPosition, float threshold, MyEntity relativeEntity = null)
		{
			if (MyAIComponent.Static.Pathfinding == null)
			{
				return false;
			}
			m_destinationSphere.Init(ref worldPosition, 0f);
			return MyAIComponent.Static.Pathfinding.ReachableUnderThreshold(PositionAndOrientation.Translation, m_destinationSphere, threshold);
		}

		public void Flyto(Vector3D worldPosition, MyEntity relativeEntity = null)
		{
			m_path.SetTarget(worldPosition, 1f, relativeEntity, 1f, fly: true);
			m_stuckDetection.Reset();
		}

		public void Stop()
		{
			m_path.UnsetPath();
			m_stuckDetection.Stop();
		}

		public void StopImmediate(bool forceUpdate = false)
		{
			Stop();
			m_speed = 0f;
			if (forceUpdate)
			{
				MoveCharacter();
			}
		}

		public void FollowPath(IMyPath path)
		{
			m_path.SetPath(path);
			m_stuckDetection.Reset();
		}

		public void AimAt(MyEntity entity, Vector3D? worldPosition = null)
		{
			if (worldPosition.HasValue)
			{
				if (entity != null)
				{
					MatrixD worldMatrixNormalizedInv = entity.PositionComp.WorldMatrixNormalizedInv;
					Vector3 value = Vector3D.Transform(worldPosition.Value, worldMatrixNormalizedInv);
					m_aiming.SetTarget(entity, value);
				}
				else
				{
					m_aiming.SetAbsoluteTarget(worldPosition.Value);
				}
			}
			else
			{
				m_aiming.SetTarget(entity);
			}
		}

		public void AimWithMovement()
		{
			m_aiming.FollowMovement();
		}

		public void StopAiming()
		{
			m_aiming.StopAiming();
		}

		[Conditional("DEBUG")]
		private void AssertIsValid()
		{
		}

		[Conditional("DEBUG")]
		public void DebugDraw()
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				return;
			}
			m_aiming.DebugDraw(m_aimingPositionAndOrientation);
			if (MyDebugDrawSettings.DEBUG_DRAW_BOT_STEERING)
			{
				foreach (MySteeringBase steering in m_steerings)
				{
					_ = steering;
				}
			}
			if (!MyDebugDrawSettings.DEBUG_DRAW_BOT_NAVIGATION)
			{
				return;
			}
			Vector3 vector = PositionAndOrientation.Translation;
			Vector3.Cross(m_forwardVector, UpVector);
			if (Stuck)
			{
				MyRenderProxy.DebugDrawSphere(vector, 1f, Color.Red.ToVector3(), 1f, depthRead: false);
			}
			MyRenderProxy.DebugDrawArrow3D(vector, vector + ForwardVector, Color.Blue, Color.Blue, depthRead: false, 0.1, "Nav. FW");
			MyRenderProxy.DebugDrawArrow3D(vector + ForwardVector, vector + ForwardVector + m_correction, Color.LightBlue, Color.LightBlue, depthRead: false, 0.1, "Correction");
			if (m_destinationSphere != null)
			{
				m_destinationSphere.DebugDraw();
			}
			MyCharacter myCharacter = BotEntity as MyCharacter;
			if (myCharacter != null)
			{
				MatrixD matrix = MatrixD.Invert(myCharacter.GetViewMatrix());
				MatrixD headMatrix = myCharacter.GetHeadMatrix(includeY: true);
				MyRenderProxy.DebugDrawLine3D(matrix.Translation, Vector3D.Transform(Vector3D.Forward * 50.0, matrix), Color.Yellow, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(headMatrix.Translation, Vector3D.Transform(Vector3D.Forward * 50.0, headMatrix), Color.Red, Color.Red, depthRead: false);
				if (myCharacter.CurrentWeapon != null)
				{
					Vector3 value = myCharacter.CurrentWeapon.DirectionToTarget(myCharacter.AimedPoint);
					Vector3D translation = (myCharacter.CurrentWeapon as MyEntity).WorldMatrix.Translation;
					MyRenderProxy.DebugDrawSphere(myCharacter.AimedPoint, 1f, Color.Yellow, 1f, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(translation, translation + value * 20f, Color.Purple, Color.Purple, depthRead: false);
				}
			}
		}
	}
}
