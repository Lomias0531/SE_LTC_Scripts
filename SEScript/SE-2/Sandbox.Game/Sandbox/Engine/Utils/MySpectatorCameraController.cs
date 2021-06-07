using Sandbox.Engine.Physics;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public class MySpectatorCameraController : MySpectator, IMyCameraController
	{
		private const int REFLECTOR_RANGE_MULTIPLIER = 5;

		public new static MySpectatorCameraController Static;

		private float m_orbitY;

		private float m_orbitX;

		private Vector3D ThirdPersonCameraOrbit = Vector3D.UnitZ * 10.0;

		private CyclingOptions m_cycling;

		private float m_cyclingMetricValue = float.MinValue;

		private long m_entityID;

		private MyEntity m_character;

		private double m_yaw;

		private double m_pitch;

		private double m_roll;

		private Vector3D m_lastRightVec = Vector3D.Right;

		private Vector3D m_lastUpVec = Vector3D.Up;

		private MatrixD m_lastOrientation = MatrixD.Identity;

		private float m_lastOrientationWeight = 1f;

		private MyLight m_light;

		private Vector3 m_lightLocalPosition;

		private Vector3D m_velocity;

		public bool IsLightOn
		{
			get
			{
				if (m_light != null)
				{
					return m_light.LightOn;
				}
				return false;
			}
		}

		public bool AlignSpectatorToGravity
		{
			get;
			set;
		}

		public long TrackedEntity
		{
			get;
			set;
		}

		public MyEntity Entity => null;

		public Vector3D Velocity
		{
			get
			{
				return m_velocity;
			}
			set
			{
				m_velocity = value;
			}
		}

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return base.IsInFirstPersonView;
			}
			set
			{
				base.IsInFirstPersonView = value;
			}
		}

		bool IMyCameraController.ForceFirstPersonCamera
		{
			get
			{
				return base.ForceFirstPersonCamera;
			}
			set
			{
				base.ForceFirstPersonCamera = value;
			}
		}

		bool IMyCameraController.EnableFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		bool IMyCameraController.AllowCubeBuilding => true;

		public MySpectatorCameraController()
		{
			Static = this;
		}

		public override void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			UpdateVelocity();
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					base.SpeedModeAngular = Math.Min(base.SpeedModeAngular * 1.5f, 6f);
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					base.SpeedModeAngular = Math.Max(base.SpeedModeAngular / 1.5f, 0.0001f);
				}
			}
			else if (MyInput.Static.IsAnyShiftKeyPressed() || MyInput.Static.IsAnyAltKeyPressed())
			{
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					base.SpeedModeLinear = Math.Min(base.SpeedModeLinear * 1.5f, 8000f);
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					base.SpeedModeLinear = Math.Max(base.SpeedModeLinear / 1.5f, 0.0001f);
				}
			}
			switch (base.SpectatorCameraMovement)
			{
			case MySpectatorCameraMovementEnum.None:
				break;
			case MySpectatorCameraMovementEnum.FreeMouse:
				MoveAndRotate_FreeMouse(moveIndicator, rotationIndicator, rollIndicator);
				break;
			case MySpectatorCameraMovementEnum.ConstantDelta:
				MoveAndRotate_ConstantDelta(moveIndicator, rotationIndicator, rollIndicator);
				if (IsLightOn)
				{
					UpdateLightPosition();
				}
				break;
			case MySpectatorCameraMovementEnum.UserControlled:
				MoveAndRotate_UserControlled(moveIndicator, rotationIndicator, rollIndicator);
				if (IsLightOn)
				{
					UpdateLightPosition();
				}
				break;
			case MySpectatorCameraMovementEnum.Orbit:
				base.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
				break;
			}
		}

		public override void Update()
		{
			base.Update();
			base.Position += m_velocity * 0.01666666753590107;
		}

		private void UpdateVelocity()
		{
			if (!MyInput.Static.IsAnyShiftKeyPressed())
			{
				return;
			}
			if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Middle))
			{
				_ = MySector.MainCamera;
				List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
				MyPhysics.CastRay(base.Position, base.Position + base.Orientation.Forward * 1000.0, list);
				IMyEntity myEntity = (list.Count <= 0) ? null : list[0].HkHitInfo.Body.GetEntity(list[0].HkHitInfo.GetShapeKey(0));
				if (myEntity != null)
				{
					m_velocity = myEntity.Physics.LinearVelocity;
				}
				else
				{
					m_velocity = Vector3D.Zero;
				}
			}
			if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Right))
			{
				m_velocity = Vector3D.Zero;
			}
			if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
			{
				m_velocity *= 1.1000000238418579;
			}
			else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
			{
				m_velocity /= 1.1000000238418579;
			}
		}

		private void MoveAndRotate_UserControlled(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			float scaleFactor = 1.66666675f;
			float num = 0.0025f * m_speedModeAngular;
			rollIndicator = MyInput.Static.GetDeveloperRoll();
			float num2 = 0f;
			if (rollIndicator != 0f)
			{
				num2 = rollIndicator * m_speedModeAngular * 0.1f;
				num2 = MathHelper.Clamp(num2, -0.02f, 0.02f);
				MyUtils.VectorPlaneRotation(m_orientation.Up, m_orientation.Right, out Vector3D xOut, out Vector3D yOut, num2);
				m_orientation.Right = yOut;
				m_orientation.Up = xOut;
			}
			if (AlignSpectatorToGravity)
			{
				rotationIndicator.Rotate(m_roll);
				m_yaw -= rotationIndicator.Y * num;
				m_pitch -= rotationIndicator.X * num;
				m_roll -= num2;
				MathHelper.LimitRadians2PI(ref m_yaw);
				m_pitch = MathHelper.Clamp(m_pitch, -Math.PI / 2.0, Math.PI / 2.0);
				MathHelper.LimitRadians2PI(ref m_roll);
				ComputeGravityAlignedOrientation(out m_orientation);
			}
			else
			{
				if (m_lastOrientationWeight < 1f)
				{
					m_orientation = MatrixD.Orthogonalize(m_orientation);
					m_orientation.Forward = Vector3D.Cross(m_orientation.Up, m_orientation.Right);
				}
				if (rotationIndicator.Y != 0f)
				{
					MyUtils.VectorPlaneRotation(m_orientation.Right, m_orientation.Forward, out Vector3D xOut2, out Vector3D yOut2, (0f - rotationIndicator.Y) * num);
					m_orientation.Right = xOut2;
					m_orientation.Forward = yOut2;
				}
				if (rotationIndicator.X != 0f)
				{
					MyUtils.VectorPlaneRotation(m_orientation.Up, m_orientation.Forward, out Vector3D xOut3, out Vector3D yOut3, rotationIndicator.X * num);
					m_orientation.Up = xOut3;
					m_orientation.Forward = yOut3;
				}
				m_lastOrientation = m_orientation;
				m_lastOrientationWeight = 1f;
				m_roll = 0.0;
				m_pitch = 0.0;
			}
			float num3 = (MyInput.Static.IsAnyShiftKeyPressed() ? 1f : 0.35f) * (MyInput.Static.IsAnyCtrlKeyPressed() ? 0.3f : 1f);
			moveIndicator *= num3 * base.SpeedModeLinear;
			Vector3 position = moveIndicator * scaleFactor;
			base.Position += Vector3.Transform(position, m_orientation);
		}

		private void ComputeGravityAlignedOrientation(out MatrixD resultOrientationStorage)
		{
			bool flag = true;
			Vector3D vector = -MyGravityProviderSystem.CalculateTotalGravityInPoint(base.Position);
			if (vector.LengthSquared() < 9.9999997473787516E-06)
			{
				vector = m_lastUpVec;
				m_lastOrientationWeight = 1f;
				flag = false;
			}
			else
			{
				m_lastUpVec = vector;
			}
			vector.Normalize();
			Vector3D vector2 = m_lastRightVec - Vector3D.Dot(m_lastRightVec, vector) * vector;
			if (vector2.LengthSquared() < 9.9999997473787516E-06)
			{
				vector2 = m_orientation.Right - Vector3D.Dot(m_orientation.Right, vector) * vector;
				if (vector2.LengthSquared() < 9.9999997473787516E-06)
				{
					vector2 = m_orientation.Forward - Vector3D.Dot(m_orientation.Forward, vector) * vector;
				}
			}
			vector2.Normalize();
			m_lastRightVec = vector2;
			Vector3D.Cross(ref vector, ref vector2, out Vector3D result);
			resultOrientationStorage = MatrixD.Identity;
			resultOrientationStorage.Right = vector2;
			resultOrientationStorage.Up = vector;
			resultOrientationStorage.Forward = result;
			resultOrientationStorage = MatrixD.CreateFromAxisAngle(Vector3D.Right, m_pitch) * resultOrientationStorage * MatrixD.CreateFromAxisAngle(vector, m_yaw);
			vector = resultOrientationStorage.Up;
			vector2 = resultOrientationStorage.Right;
			resultOrientationStorage.Right = Math.Cos(m_roll) * vector2 + Math.Sin(m_roll) * vector;
			resultOrientationStorage.Up = (0.0 - Math.Sin(m_roll)) * vector2 + Math.Cos(m_roll) * vector;
			if (flag && m_lastOrientationWeight > 0f)
			{
				m_lastOrientationWeight = Math.Max(0f, m_lastOrientationWeight - 0.0166666675f);
				resultOrientationStorage = MatrixD.Slerp(resultOrientationStorage, m_lastOrientation, MathHelper.SmoothStepStable(m_lastOrientationWeight));
				resultOrientationStorage = MatrixD.Orthogonalize(resultOrientationStorage);
				resultOrientationStorage.Forward = Vector3D.Cross(resultOrientationStorage.Up, resultOrientationStorage.Right);
			}
			if (!flag)
			{
				m_lastOrientation = resultOrientationStorage;
			}
		}

		private void MoveAndRotate_ConstantDelta(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			m_cycling.Enabled = true;
			bool flag = false;
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TOOLBAR_UP) && MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyEntityCycling.FindNext(MyEntityCyclingOrder.Characters, ref m_cyclingMetricValue, ref m_entityID, findLarger: false, m_cycling);
				flag = true;
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TOOLBAR_DOWN) && MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyEntityCycling.FindNext(MyEntityCyclingOrder.Characters, ref m_cyclingMetricValue, ref m_entityID, findLarger: true, m_cycling);
				flag = true;
			}
			if (!MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed())
			{
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					ThirdPersonCameraOrbit /= 1.1000000238418579;
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					ThirdPersonCameraOrbit *= 1.1000000238418579;
				}
			}
			if (flag)
			{
				MyEntities.TryGetEntityById(m_entityID, out m_character);
			}
			MyEntities.TryGetEntityById(TrackedEntity, out MyEntity entity);
			if (entity != null)
			{
				Vector3D position = entity.PositionComp.GetPosition();
				if (AlignSpectatorToGravity)
				{
					m_roll = 0.0;
					m_yaw = 0.0;
					m_pitch = 0.0;
					ComputeGravityAlignedOrientation(out MatrixD resultOrientationStorage);
					base.Position = position + Vector3D.Transform(ThirdPersonCameraDelta, resultOrientationStorage);
					base.Target = position;
					m_orientation.Up = resultOrientationStorage.Up;
				}
				else
				{
					Vector3D value = Vector3D.Normalize(base.Position - base.Target) * ThirdPersonCameraDelta.Length();
					base.Position = position + value;
					base.Target = position;
				}
			}
			if (MyInput.Static.IsAnyAltKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed())
			{
				base.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
			}
		}

		private void MoveAndRotate_FreeMouse(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			if (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition != null || MySessionComponentVoxelHand.Static.Enabled || MyInput.Static.IsRightMousePressed())
			{
				MoveAndRotate_UserControlled(moveIndicator, rotationIndicator, rollIndicator);
			}
			else
			{
				MoveAndRotate_UserControlled(moveIndicator, Vector2.Zero, rollIndicator);
			}
		}

		protected override void OnChangingMode(MySpectatorCameraMovementEnum oldMode, MySpectatorCameraMovementEnum newMode)
		{
			if (newMode == MySpectatorCameraMovementEnum.UserControlled && oldMode == MySpectatorCameraMovementEnum.ConstantDelta)
			{
				ComputeGravityAlignedOrientation(out MatrixD resultOrientationStorage);
				m_orientation.Up = resultOrientationStorage.Up;
				m_orientation.Forward = Vector3D.Normalize(base.Target - base.Position);
				m_orientation.Right = Vector3D.Cross(m_orientation.Forward, m_orientation.Up);
				AlignSpectatorToGravity = false;
			}
		}

		void IMyCameraController.ControlCamera(MyCamera currentCamera)
		{
			currentCamera.SetViewMatrix(GetViewMatrix());
		}

		public void InitLight(bool isLightOn)
		{
			m_light = MyLights.AddLight();
			if (m_light != null)
			{
				m_light.Start("SpectatorCameraController");
				m_light.ReflectorOn = true;
				m_light.ReflectorTexture = "Textures\\Lights\\dual_reflector_2.dds";
				m_light.Range = 2f;
				m_light.ReflectorRange = 35f;
				m_light.ReflectorColor = MyCharacter.REFLECTOR_COLOR;
				m_light.ReflectorIntensity = MyCharacter.REFLECTOR_INTENSITY;
				m_light.ReflectorGlossFactor = MyCharacter.REFLECTOR_GLOSS_FACTOR;
				m_light.ReflectorDiffuseFactor = MyCharacter.REFLECTOR_DIFFUSE_FACTOR;
				m_light.Color = MyCharacter.POINT_COLOR;
				m_light.Intensity = MyCharacter.POINT_LIGHT_INTENSITY;
				m_light.UpdateReflectorRangeAndAngle(0.373f, 175f);
				m_light.LightOn = isLightOn;
				m_light.ReflectorOn = isLightOn;
			}
		}

		private void UpdateLightPosition()
		{
			if (m_light != null)
			{
				MatrixD matrixD = MatrixD.CreateWorld(base.Position, m_orientation.Forward, m_orientation.Up);
				m_light.ReflectorDirection = matrixD.Forward;
				m_light.ReflectorUp = matrixD.Up;
				m_light.Position = base.Position;
				m_light.UpdateLight();
			}
		}

		public void SwitchLight()
		{
			if (m_light != null)
			{
				m_light.LightOn = !m_light.LightOn;
				m_light.ReflectorOn = !m_light.ReflectorOn;
				m_light.UpdateLight();
			}
		}

		public void TurnLightOff()
		{
			if (m_light != null)
			{
				m_light.LightOn = false;
				m_light.ReflectorOn = false;
				m_light.UpdateLight();
			}
		}

		public void CleanLight()
		{
			if (m_light != null)
			{
				MyLights.RemoveLight(m_light);
				m_light = null;
			}
		}

		void IMyCameraController.Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			Rotate(rotationIndicator, rollIndicator);
		}

		void IMyCameraController.RotateStopped()
		{
			RotateStopped();
		}

		public void OnAssumeControl(IMyCameraController previousCameraController)
		{
		}

		public void OnReleaseControl(IMyCameraController newCameraController)
		{
			TurnLightOff();
		}

		void IMyCameraController.OnAssumeControl(IMyCameraController previousCameraController)
		{
			OnAssumeControl(previousCameraController);
		}

		void IMyCameraController.OnReleaseControl(IMyCameraController newCameraController)
		{
			OnReleaseControl(newCameraController);
		}

		bool IMyCameraController.HandleUse()
		{
			return false;
		}

		bool IMyCameraController.HandlePickUp()
		{
			return false;
		}
	}
}
