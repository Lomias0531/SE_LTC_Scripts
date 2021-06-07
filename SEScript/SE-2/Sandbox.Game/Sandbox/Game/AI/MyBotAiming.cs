using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Navigation;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using System;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
	public class MyBotAiming
	{
		private enum AimingMode : byte
		{
			FIXATED,
			TARGET,
			FOLLOW_MOVEMENT
		}

		public const float MISSING_PROBABILITY = 0.3f;

		private MyBotNavigation m_parent;

		private AimingMode m_mode;

		private MyEntity m_aimTarget;

		private Vector3 m_rotationHint;

		private Vector3? m_relativeTarget;

		private Vector3 m_dbgDesiredForward;

		public Vector3 RotationHint => m_rotationHint;

		public MyBotAiming(MyBotNavigation parent)
		{
			m_parent = parent;
			m_mode = AimingMode.FOLLOW_MOVEMENT;
			m_rotationHint = Vector3.Zero;
		}

		public void SetTarget(MyEntity entity, Vector3? relativeTarget = null)
		{
			m_mode = AimingMode.TARGET;
			m_aimTarget = entity;
			m_relativeTarget = relativeTarget;
			Update();
		}

		public void SetAbsoluteTarget(Vector3 absoluteTarget)
		{
			m_mode = AimingMode.TARGET;
			m_aimTarget = null;
			m_relativeTarget = absoluteTarget;
			Update();
		}

		public void FollowMovement()
		{
			m_aimTarget = null;
			m_mode = AimingMode.FOLLOW_MOVEMENT;
			m_relativeTarget = null;
		}

		public void StopAiming()
		{
			m_aimTarget = null;
			m_mode = AimingMode.FIXATED;
			m_relativeTarget = null;
		}

		public void Update()
		{
			if (m_mode == AimingMode.FIXATED)
			{
				m_rotationHint = Vector3.Zero;
				return;
			}
			MyCharacter myCharacter = m_parent.BotEntity as MyCharacter;
			MatrixD parentMatrix = m_parent.AimingPositionAndOrientation;
			if (m_mode == AimingMode.FOLLOW_MOVEMENT)
			{
				_ = (Matrix)m_parent.PositionAndOrientationInverted;
				_ = (Matrix)m_parent.AimingPositionAndOrientationInverted;
				Vector3 desiredForward = m_parent.ForwardVector;
				CalculateRotationHint(ref parentMatrix, ref desiredForward);
			}
			else if (m_aimTarget != null)
			{
				if (m_aimTarget.MarkedForClose)
				{
					m_aimTarget = null;
					m_rotationHint = Vector3.Zero;
					return;
				}
				Vector3 transformedRelativeTarget = (!m_relativeTarget.HasValue) ? ((Vector3)m_aimTarget.PositionComp.WorldMatrix.Translation) : ((Vector3)Vector3D.Transform(m_relativeTarget.Value, m_aimTarget.PositionComp.WorldMatrix));
				PredictTargetPosition(ref transformedRelativeTarget, myCharacter);
				Vector3 desiredForward2 = transformedRelativeTarget - m_parent.AimingPositionAndOrientation.Translation;
				desiredForward2.Normalize();
				CalculateRotationHint(ref parentMatrix, ref desiredForward2);
				if (myCharacter != null)
				{
					myCharacter.AimedPoint = transformedRelativeTarget;
					AddErrorToAiming(myCharacter, (m_aimTarget.PositionComp != null) ? (m_aimTarget.PositionComp.LocalVolume.Radius * 1.5f) : 1f);
				}
			}
			else if (m_relativeTarget.HasValue)
			{
				Vector3 desiredForward3 = m_relativeTarget.Value - m_parent.AimingPositionAndOrientation.Translation;
				desiredForward3.Normalize();
				CalculateRotationHint(ref parentMatrix, ref desiredForward3);
				if (myCharacter != null)
				{
					myCharacter.AimedPoint = m_relativeTarget.Value;
				}
			}
			else
			{
				m_rotationHint = Vector3.Zero;
			}
		}

		private void AddErrorToAiming(MyCharacter character, float errorLenght)
		{
			if (MyUtils.GetRandomFloat() < 0.3f)
			{
				character.AimedPoint += Vector3D.Normalize(MyUtils.GetRandomVector3()) * errorLenght;
			}
		}

		private void PredictTargetPosition(ref Vector3 transformedRelativeTarget, MyCharacter bot)
		{
			if (bot != null && bot.CurrentWeapon != null)
			{
				MyGunBase myGunBase = bot.CurrentWeapon.GunBase as MyGunBase;
				if (myGunBase != null)
				{
					MyWeaponPrediction.GetPredictedTargetPosition(myGunBase, bot, m_aimTarget, out transformedRelativeTarget, out float _, 355f / (678f * MathF.PI));
				}
			}
		}

		private void CalculateRotationHint(ref MatrixD parentMatrix, ref Vector3 desiredForward)
		{
			Vector3D vector = m_parent.UpVector;
			if (desiredForward.LengthSquared() == 0f)
			{
				m_rotationHint.X = (m_rotationHint.Y = 0f);
				return;
			}
			Vector3D vector2 = Vector3D.Reject(desiredForward, parentMatrix.Up);
			Vector3D vector3 = Vector3D.Reject(desiredForward, parentMatrix.Right);
			vector2.Normalize();
			vector3.Normalize();
			m_dbgDesiredForward = desiredForward;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = Vector3D.Dot(parentMatrix.Forward, vector);
			double num4 = Vector3D.Dot(desiredForward, vector);
			num2 = Vector3D.Dot(parentMatrix.Forward, vector3);
			num2 = MathHelper.Clamp(num2, -1.0, 1.0);
			num2 = Math.Acos(num2);
			if (num4 > num3)
			{
				num2 = 0.0 - num2;
			}
			num = Vector3D.Dot(parentMatrix.Forward, vector2);
			num = MathHelper.Clamp(num, -1.0, 1.0);
			num = Math.Acos(num);
			if (Vector3D.Dot(parentMatrix.Right, vector2) < 0.0)
			{
				num = 0.0 - num;
			}
			m_rotationHint.X = MathHelper.Clamp((float)num, -3f, 3f);
			m_rotationHint.Y = MathHelper.Clamp((float)num2, -3f, 3f);
		}

		public void DebugDraw(MatrixD posAndOri)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_BOT_AIMING)
			{
				Vector3 vector = posAndOri.Translation;
				MyRenderProxy.DebugDrawArrow3D(vector, vector + posAndOri.Right, Color.Red, Color.Red, depthRead: false, 0.1, "X");
				MyRenderProxy.DebugDrawArrow3D(vector, vector + posAndOri.Up, Color.Green, Color.Green, depthRead: false, 0.1, "Y");
				MyRenderProxy.DebugDrawArrow3D(vector, vector + posAndOri.Forward, Color.Blue, Color.Blue, depthRead: false, 0.1, "-Z");
				MyRenderProxy.DebugDrawArrow3D(vector, vector + m_dbgDesiredForward, Color.Yellow, Color.Yellow, depthRead: false, 0.1, "Des.-Z");
				Vector3D vector3D = vector + posAndOri.Forward;
				MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D + m_rotationHint.X * 10f * posAndOri.Right, Color.Salmon, Color.Salmon, depthRead: false, 0.1, "Rot.X");
				MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D - m_rotationHint.Y * 10f * posAndOri.Up, Color.LimeGreen, Color.LimeGreen, depthRead: false, 0.1, "Rot.Y");
				MyCharacter myCharacter = m_parent.BotEntity as MyCharacter;
				if (myCharacter != null)
				{
					MyRenderProxy.DebugDrawSphere(myCharacter.AimedPoint, 0.2f, Color.Orange, 1f, depthRead: false);
				}
			}
		}
	}
}
