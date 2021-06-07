using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyGridGyroSystem
	{
		private static readonly float INV_TENSOR_MAX_LIMIT = 125000f;

		private static readonly float MAX_SLOWDOWN = MyFakes.WELD_LANDING_GEARS ? 0.8f : 0.93f;

		private static readonly float MAX_ROLL = MathF.E * 449f / 777f;

		private const float TORQUE_SQ_LEN_TH = 0.0001f;

		private Vector3 m_controlTorque;

		public bool AutopilotEnabled;

		private MyCubeGrid m_grid;

		private HashSet<MyGyro> m_gyros;

		private bool m_gyrosChanged;

		private float m_maxGyroForce;

		private float m_maxOverrideForce;

		private float m_maxRequiredPowerInput;

		private Vector3 m_overrideTargetVelocity;

		private int? m_overrideAccelerationRampFrames;

		public Vector3 SlowdownTorque;

		public Vector3 ControlTorque
		{
			get
			{
				return m_controlTorque;
			}
			set
			{
				if (m_controlTorque != value)
				{
					m_controlTorque = value;
					m_grid.MarkForUpdate();
				}
			}
		}

		public bool HasOverrideInput
		{
			get
			{
				if (Vector3.IsZero(ref m_controlTorque))
				{
					return !Vector3.IsZero(ref m_overrideTargetVelocity);
				}
				return true;
			}
		}

		public bool IsDirty => m_gyrosChanged;

		public MyResourceSinkComponent ResourceSink
		{
			get;
			private set;
		}

		public int GyroCount => m_gyros.Count;

		public HashSet<MyGyro> Gyros => m_gyros;

		public Vector3 Torque
		{
			get;
			private set;
		}

		public bool NeedsPerFrameUpdate
		{
			get
			{
				if (!(ControlTorque != Vector3.Zero) && (m_grid.Physics == null || !(m_grid.Physics.AngularVelocity.LengthSquared() > 1E-05f)) && !(Torque.LengthSquared() >= 0.0001f) && !(SlowdownTorque.LengthSquared() > 0.0001f))
				{
					return m_overrideTargetVelocity.LengthSquared() > 0.0001f;
				}
				return true;
			}
		}

		public MyGridGyroSystem(MyCubeGrid grid)
		{
			m_grid = grid;
			m_gyros = new HashSet<MyGyro>();
			m_gyrosChanged = false;
			ResourceSink = new MyResourceSinkComponent();
			ResourceSink.Init(MyStringHash.GetOrCompute("Gyro"), m_maxRequiredPowerInput, () => m_maxRequiredPowerInput);
			ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
		}

		public void Register(MyGyro gyro)
		{
			m_gyros.Add(gyro);
			m_gyrosChanged = true;
			gyro.EnabledChanged += gyro_EnabledChanged;
			gyro.SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			gyro.PropertiesChanged += gyro_PropertiesChanged;
		}

		private void gyro_PropertiesChanged(MyTerminalBlock sender)
		{
			MarkDirty();
		}

		public void Unregister(MyGyro gyro)
		{
			m_gyros.Remove(gyro);
			m_gyrosChanged = true;
			gyro.EnabledChanged -= gyro_EnabledChanged;
			gyro.SlimBlock.ComponentStack.IsFunctionalChanged -= ComponentStack_IsFunctionalChanged;
		}

		private void UpdateGyros()
		{
			SlowdownTorque = Vector3.Zero;
			MyCubeGrid grid = m_grid;
			MyGridPhysics physics = grid.Physics;
			if (physics == null || physics.IsKinematic)
			{
				return;
			}
			if (!ControlTorque.IsValid())
			{
				ControlTorque = Vector3.Zero;
			}
			if ((Vector3.IsZero(physics.AngularVelocity, 0.001f) && Vector3.IsZero(ControlTorque, 0.001f)) || !(ResourceSink.SuppliedRatio > 0f) || (!physics.Enabled && !physics.IsWelded) || physics.RigidBody.IsFixed)
			{
				return;
			}
			Matrix inverseInertiaTensor = physics.RigidBody.InverseInertiaTensor;
			inverseInertiaTensor.M44 = 1f;
			Matrix matrix = grid.PositionComp.WorldMatrixNormalizedInv.GetOrientation();
			Vector3 value = Vector3.Transform(physics.AngularVelocity, ref matrix);
			float scaleFactor = (1f - MAX_SLOWDOWN) * (1f - ResourceSink.SuppliedRatio) + MAX_SLOWDOWN;
			SlowdownTorque = -value;
			float num = (grid.GridSizeEnum == MyCubeSize.Large) ? MyFakes.SLOWDOWN_FACTOR_TORQUE_MULTIPLIER_LARGE_SHIP : MyFakes.SLOWDOWN_FACTOR_TORQUE_MULTIPLIER;
			Vector3 vector = new Vector3(m_maxGyroForce * num);
			if (physics.IsWelded)
			{
				SlowdownTorque = Vector3.TransformNormal(SlowdownTorque, grid.WorldMatrix);
				SlowdownTorque = Vector3.TransformNormal(SlowdownTorque, Matrix.Invert(physics.RigidBody.GetRigidBodyMatrix()));
			}
			if (!value.IsValid())
			{
				value = Vector3.Zero;
			}
			Vector3 value2 = Vector3.One - Vector3.IsZeroVector(Vector3.Sign(value) - Vector3.Sign(ControlTorque));
			SlowdownTorque *= num;
			SlowdownTorque /= inverseInertiaTensor.Scale;
			SlowdownTorque = Vector3.Clamp(SlowdownTorque, -vector, vector) * value2;
			if (SlowdownTorque.LengthSquared() > 0.0001f)
			{
				physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, null, SlowdownTorque * scaleFactor);
			}
			Matrix inertiaTensor = MyGridPhysicalGroupData.GetGroupSharedProperties(grid).InertiaTensor;
			float num2 = 1f / Math.Max(Math.Max(inertiaTensor.M11, inertiaTensor.M22), inertiaTensor.M33);
			float divider = Math.Max(1f, num2 * INV_TENSOR_MAX_LIMIT);
			Torque = Vector3.Clamp(ControlTorque, -Vector3.One, Vector3.One) * m_maxGyroForce / divider;
			Torque *= ResourceSink.SuppliedRatio;
			Vector3 scale = physics.RigidBody.InertiaTensor.Scale;
			scale = Vector3.Abs(scale / scale.AbsMax());
			if (Torque.LengthSquared() > 0.0001f)
			{
				Vector3 vector2 = Torque;
				if (physics.IsWelded)
				{
					vector2 = Vector3.TransformNormal(vector2, grid.WorldMatrix);
					vector2 = Vector3.TransformNormal(vector2, Matrix.Invert(physics.RigidBody.GetRigidBodyMatrix()));
				}
				physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, null, vector2 * scale);
			}
			if (ControlTorque == Vector3.Zero && physics.AngularVelocity != Vector3.Zero && physics.AngularVelocity.LengthSquared() < 9.000001E-08f && physics.RigidBody.IsActive)
			{
				physics.AngularVelocity = Vector3.Zero;
			}
		}

		private void UpdateOverriddenGyros()
		{
			if (!(ResourceSink.SuppliedRatio > 0f) || !m_grid.Physics.Enabled || m_grid.Physics.RigidBody.IsFixed)
			{
				return;
			}
			Matrix matrix = m_grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
			_ = (Matrix)m_grid.WorldMatrix.GetOrientation();
			Vector3 value = Vector3.Transform(m_grid.Physics.AngularVelocity, ref matrix);
			Torque = Vector3.Zero;
			Vector3 vector = m_overrideTargetVelocity - value;
			if (!(vector == Vector3.Zero))
			{
				UpdateOverrideAccelerationRampFrames(vector);
				Vector3 value2 = vector * (60f / (float)m_overrideAccelerationRampFrames.Value);
				Matrix inverseInertiaTensor = m_grid.Physics.RigidBody.InverseInertiaTensor;
				Vector3 value3 = new Vector3(inverseInertiaTensor.M11, inverseInertiaTensor.M22, inverseInertiaTensor.M33);
				Vector3 vector2 = value2 / value3;
				float radius = m_maxOverrideForce + m_maxGyroForce * (1f - ControlTorque.Length());
				Vector3 value4 = Vector3.ClampToSphere(vector2, radius);
				Torque = ControlTorque * m_maxGyroForce + value4;
				Torque *= ResourceSink.SuppliedRatio;
				if (!(Torque.LengthSquared() < 0.0001f))
				{
					m_grid.MarkForUpdate();
					m_grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, null, Torque);
				}
			}
		}

		private void UpdateOverrideAccelerationRampFrames(Vector3 velocityDiff)
		{
			if (!m_overrideAccelerationRampFrames.HasValue)
			{
				float num = velocityDiff.LengthSquared();
				if (num > 2.467401f)
				{
					m_overrideAccelerationRampFrames = 120;
				}
				else
				{
					m_overrideAccelerationRampFrames = (int)(num * 48.2288857f) + 1;
				}
			}
			else if (m_overrideAccelerationRampFrames > 1)
			{
				m_overrideAccelerationRampFrames--;
			}
		}

		public Vector3 GetAngularVelocity(Vector3 control)
		{
			if (ResourceSink.SuppliedRatio > 0f && m_grid.Physics != null && m_grid.Physics.Enabled && !m_grid.Physics.RigidBody.IsFixed)
			{
				Matrix matrix = m_grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
				Matrix matrix2 = m_grid.WorldMatrix.GetOrientation();
				Vector3 vector = Vector3.Transform(m_grid.Physics.AngularVelocity, ref matrix);
				Matrix inverseInertiaTensor = m_grid.Physics.RigidBody.InverseInertiaTensor;
				Vector3 vector2 = new Vector3(inverseInertiaTensor.M11, inverseInertiaTensor.M22, inverseInertiaTensor.M33);
				float num = vector2.Min();
				float divider = Math.Max(1f, num * INV_TENSOR_MAX_LIMIT);
				Vector3 value = Vector3.Zero;
				Vector3 vector3 = (m_overrideTargetVelocity - vector) * 60f;
				float num2 = m_maxOverrideForce + m_maxGyroForce * (1f - control.Length());
				vector3 *= Vector3.Normalize(vector2);
				Vector3 vector4 = vector3 / vector2;
				float num3 = vector4.Length() / num2;
				if (num3 < 0.5f && m_overrideTargetVelocity.LengthSquared() < 2.5E-05f)
				{
					return m_overrideTargetVelocity;
				}
				if (!Vector3.IsZero(vector3, 0.0001f))
				{
					float num4 = 1f - 0.8f / (float)Math.Exp(0.5f * num3);
					value = Vector3.ClampToSphere(vector4, num2) * 0.95f * num4 + vector4 * 0.05f * (1f - num4);
					if (m_grid.GridSizeEnum == MyCubeSize.Large)
					{
						value *= 2f;
					}
				}
				Torque = (control * m_maxGyroForce + value) / divider;
				Torque *= ResourceSink.SuppliedRatio;
				if (Torque.LengthSquared() > 0.0001f)
				{
					Vector3 value2 = Torque * new Vector3(num) * 0.0166666675f;
					return Vector3.Transform(vector + value2, ref matrix2);
				}
				if (control == Vector3.Zero && m_overrideTargetVelocity == Vector3.Zero && m_grid.Physics.AngularVelocity != Vector3.Zero && m_grid.Physics.AngularVelocity.LengthSquared() < 9.000001E-08f && m_grid.Physics.RigidBody.IsActive)
				{
					return Vector3.Zero;
				}
			}
			if (m_grid.Physics != null)
			{
				return m_grid.Physics.AngularVelocity;
			}
			return Vector3.Zero;
		}

		public void UpdateBeforeSimulation()
		{
			MySimpleProfiler.Begin("Gyro", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation");
			if (m_gyrosChanged)
			{
				RecomputeGyroParameters();
			}
			if (m_maxOverrideForce == 0f)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_GYROS)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "Old gyros", Color.White, 1f);
				}
				UpdateGyros();
				MySimpleProfiler.End("UpdateBeforeSimulation");
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GYROS)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "New gyros", Color.White, 1f);
			}
			if (m_grid.Physics != null)
			{
				UpdateOverriddenGyros();
			}
			MySimpleProfiler.End("UpdateBeforeSimulation");
		}

		public void DebugDraw()
		{
			double scaleFactor = 4.5 * 0.045;
			Vector3D translation = m_grid.WorldMatrix.Translation;
			Vector3D position = MySector.MainCamera.Position;
			Vector3D up = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			double val = Vector3D.Distance(translation, position);
			double num = Math.Atan(4.5 / Math.Max(val, 0.001));
			if (!(num <= 0.27000001072883606))
			{
				MyRenderProxy.DebugDrawText3D(translation, $"Grid {m_grid} Gyro System", Color.Yellow, (float)num, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				bool flag = Torque.LengthSquared() >= 0.0001f;
				bool flag2 = SlowdownTorque.LengthSquared() > 0.0001f;
				bool flag3 = m_overrideTargetVelocity.LengthSquared() > 0.0001f;
				bool flag4 = m_grid.Physics != null && m_grid.Physics.AngularVelocity.LengthSquared() > 1E-05f;
				DebugDrawText($"Gyro count: {GyroCount}", translation + -1.0 * up * scaleFactor, right, (float)num);
				DebugDrawText(string.Format("Torque [above threshold - {1}]: {0}", Torque, flag), translation + -2.0 * up * scaleFactor, right, (float)num);
				DebugDrawText(string.Format("Slowdown [above threshold - {1}]: {0}", SlowdownTorque, flag2), translation + -3.0 * up * scaleFactor, right, (float)num);
				DebugDrawText(string.Format("Override [above threshold - {1}]: {0}", m_overrideTargetVelocity, flag3), translation + -4.0 * up * scaleFactor, right, (float)num);
				DebugDrawText($"Angular velocity above threshold - {flag4}", translation + -5.0 * up * scaleFactor, right, (float)num);
				DebugDrawText($"Needs per frame update - {NeedsPerFrameUpdate}", translation + -6.0 * up * scaleFactor, right, (float)num);
				if (m_grid.Physics != null)
				{
					DebugDrawText($"Automatic deactivation enabled - {m_grid.Physics.RigidBody.EnableDeactivation}", translation + -7.0 * up * scaleFactor, right, (float)num);
				}
			}
		}

		private void DebugDrawText(string text, Vector3D origin, Vector3D rightVector, float textSize)
		{
			Vector3D value = 0.05000000074505806 * rightVector;
			Vector3D worldCoord = origin + value + rightVector * 0.014999999664723873;
			MyRenderProxy.DebugDrawLine3D(origin, origin + value, Color.White, Color.White, depthRead: false);
			MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
		}

		private void RecomputeGyroParameters()
		{
			m_gyrosChanged = false;
			_ = m_maxRequiredPowerInput;
			m_maxGyroForce = 0f;
			m_maxOverrideForce = 0f;
			m_maxRequiredPowerInput = 0f;
			m_overrideTargetVelocity = Vector3.Zero;
			m_overrideAccelerationRampFrames = null;
			foreach (MyGyro gyro in m_gyros)
			{
				if (IsUsed(gyro))
				{
					if (!gyro.GyroOverride || AutopilotEnabled)
					{
						m_maxGyroForce += gyro.MaxGyroForce;
					}
					else
					{
						m_overrideTargetVelocity += gyro.GyroOverrideVelocityGrid * gyro.MaxGyroForce;
						m_maxOverrideForce += gyro.MaxGyroForce;
					}
					m_maxRequiredPowerInput += gyro.RequiredPowerInput;
				}
			}
			if (m_maxOverrideForce != 0f)
			{
				m_overrideTargetVelocity /= m_maxOverrideForce;
			}
			ResourceSink.MaxRequiredInput = m_maxRequiredPowerInput;
			ResourceSink.Update();
			UpdateAutomaticDeactivation();
		}

		private bool IsUsed(MyGyro gyro)
		{
			if (gyro.Enabled)
			{
				return gyro.IsFunctional;
			}
			return false;
		}

		private void gyro_EnabledChanged(MyTerminalBlock obj)
		{
			MarkDirty();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			MarkDirty();
		}

		public void MarkDirty()
		{
			m_gyrosChanged = true;
			m_grid.MarkForUpdate();
		}

		private void Receiver_IsPoweredChanged()
		{
			foreach (MyGyro gyro in m_gyros)
			{
				gyro.UpdateIsWorking();
			}
		}

		private void UpdateAutomaticDeactivation()
		{
			if (m_grid.Physics != null && !m_grid.Physics.RigidBody.IsFixed)
			{
				if (!Vector3.IsZero(m_overrideTargetVelocity) && ResourceSink.IsPowered)
				{
					m_grid.Physics.RigidBody.EnableDeactivation = false;
				}
				else
				{
					m_grid.Physics.RigidBody.EnableDeactivation = true;
				}
			}
		}
	}
}
