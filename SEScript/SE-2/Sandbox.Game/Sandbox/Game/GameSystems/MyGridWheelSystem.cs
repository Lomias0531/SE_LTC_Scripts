using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	[StaticEventOwner]
	public class MyGridWheelSystem
	{
		public enum WheelJumpSate
		{
			Idle,
			Charging,
			Pushing,
			Restore
		}

		protected sealed class InvokeJumpInternal_003C_003ESystem_Int64_0023System_Boolean : ICallSite<IMyEventOwner, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridId, in bool initiate, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				InvokeJumpInternal(gridId, initiate);
			}
		}

		public Vector3 AngularVelocity;

		private const int JUMP_FULL_CHARGE_TIME = 600;

		private bool m_wheelsChanged;

		private float m_maxRequiredPowerInput;

		private readonly MyCubeGrid m_grid;

		private readonly HashSet<MyMotorSuspension> m_wheels;

		private readonly HashSet<MyMotorSuspension> m_wheelsNeedingUpdate;

		private readonly MyResourceSinkComponent m_sinkComp;

		private bool m_handbrake;

		private bool m_brake;

		private ulong m_jumpStartTime;

		private bool m_lastJumpInput;

		public WheelJumpSate m_jumpState;

		private int m_consecutiveCorrectionFrames;

		private Vector3 m_lastPhysicsAngularVelocityLateral;

		public HashSet<MyMotorSuspension> Wheels => m_wheels;

		public int WheelCount => m_wheels.Count;

		public bool HandBrake
		{
			get
			{
				return m_handbrake;
			}
			set
			{
				if (m_handbrake != value)
				{
					m_handbrake = value;
					if (Sync.IsServer)
					{
						UpdateBrake();
					}
				}
			}
		}

		public bool Brake
		{
			get
			{
				return m_brake;
			}
			set
			{
				if (m_brake != value)
				{
					m_brake = value;
					UpdateBrake();
				}
			}
		}

		public bool LastJumpInput => m_lastJumpInput;

		public bool NeedsPerFrameUpdate
		{
			get
			{
				if (!m_wheelsChanged && m_wheelsNeedingUpdate.Count <= 0)
				{
					if (m_grid.Physics != null)
					{
						return m_grid.Physics.LinearVelocity.LengthSquared() > 0.1f;
					}
					return false;
				}
				return true;
			}
		}

		public event Action<MyCubeGrid> OnMotorUnregister;

		public MyGridWheelSystem(MyCubeGrid grid)
		{
			m_wheels = new HashSet<MyMotorSuspension>();
			m_wheelsNeedingUpdate = new HashSet<MyMotorSuspension>();
			m_wheelsChanged = false;
			m_grid = grid;
			m_sinkComp = new MyResourceSinkComponent();
			m_sinkComp.Init(MyStringHash.GetOrCompute("Utility"), m_maxRequiredPowerInput, () => m_maxRequiredPowerInput);
			m_sinkComp.IsPoweredChanged += ReceiverIsPoweredChanged;
			grid.OnPhysicsChanged += OnGridPhysicsChanged;
		}

		public void Register(MyMotorSuspension motor)
		{
			m_wheels.Add(motor);
			OnBlockNeedsUpdateChanged(motor);
			m_wheelsChanged = true;
			motor.EnabledChanged += MotorEnabledChanged;
			if (Sync.IsServer)
			{
				motor.Brake = m_handbrake;
			}
			m_grid.MarkForUpdate();
		}

		public void Unregister(MyMotorSuspension motor)
		{
			if (motor != null)
			{
				if (motor.RotorGrid != null && this.OnMotorUnregister != null)
				{
					this.OnMotorUnregister(motor.RotorGrid);
				}
				m_wheels.Remove(motor);
				m_wheelsNeedingUpdate.Remove(motor);
				m_wheelsChanged = true;
				motor.EnabledChanged -= MotorEnabledChanged;
				m_grid.MarkForUpdate();
			}
		}

		public void OnBlockNeedsUpdateChanged(MyMotorSuspension motor)
		{
			if (motor.NeedsPerFrameUpdate)
			{
				if (m_wheelsNeedingUpdate.Count == 0)
				{
					m_grid.MarkForUpdate();
				}
				m_wheelsNeedingUpdate.Add(motor);
			}
			else
			{
				m_wheelsNeedingUpdate.Remove(motor);
			}
		}

		public void UpdateBeforeSimulation()
		{
			if (m_wheelsChanged)
			{
				RecomputeWheelParameters();
			}
			if (m_jumpState == WheelJumpSate.Pushing || m_jumpState == WheelJumpSate.Restore)
			{
				MyWheel.WheelExplosionLog(m_grid, null, "JumpUpdate: " + m_jumpState);
				foreach (MyMotorSuspension wheel in Wheels)
				{
					wheel.OnSuspensionJumpStateUpdated();
				}
				switch (m_jumpState)
				{
				case WheelJumpSate.Pushing:
					m_jumpState = WheelJumpSate.Restore;
					break;
				case WheelJumpSate.Restore:
					m_jumpState = WheelJumpSate.Idle;
					break;
				}
			}
			MyGridPhysics physics = m_grid.Physics;
			if (physics == null)
			{
				return;
			}
			Vector3 linVelocityNormal = physics.LinearVelocity;
			float num = linVelocityNormal.Normalize() / MyGridPhysics.GetShipMaxLinearVelocity(m_grid.GridSizeEnum);
			if (num > 1f)
			{
				num = 1f;
			}
			bool flag = AngularVelocity.Z < 0f;
			bool flag2 = AngularVelocity.Z > 0f;
			bool flag3 = flag || flag2;
			if (m_wheels.Count <= 0)
			{
				return;
			}
			int num2 = 0;
			bool flag4 = !m_grid.GridSystems.GyroSystem.HasOverrideInput;
			Vector3 suspensionNormal = Vector3.Zero;
			Vector3 groundNormal = Vector3.Zero;
			foreach (MyMotorSuspension wheel2 in m_wheels)
			{
				bool flag5 = wheel2.IsWorking && wheel2.Propulsion;
				wheel2.AxleFrictionLogic(num, flag3 && flag5);
				wheel2.Update();
				if (flag4 && wheel2.LateralCorrectionLogicInfo(ref groundNormal, ref suspensionNormal))
				{
					num2++;
				}
				if (wheel2.IsWorking)
				{
					if (wheel2.Steering)
					{
						wheel2.Steer(AngularVelocity.X, num);
					}
					if (wheel2.Propulsion)
					{
						wheel2.UpdatePropulsion(flag, flag2);
					}
				}
			}
			flag4 &= (num2 != 0 && !Vector3.IsZero(ref suspensionNormal) && !Vector3.IsZero(ref groundNormal));
			bool flag6 = false;
			if (flag4)
			{
				flag6 = LateralCorrectionLogic(ref suspensionNormal, ref groundNormal, ref linVelocityNormal);
			}
			if (flag6)
			{
				if (m_consecutiveCorrectionFrames < 10)
				{
					m_consecutiveCorrectionFrames++;
				}
			}
			else
			{
				m_consecutiveCorrectionFrames = (int)((float)m_consecutiveCorrectionFrames * 0.8f);
			}
		}

		private bool LateralCorrectionLogic(ref Vector3 gridDownNormal, ref Vector3 lateralCorrectionNormal, ref Vector3 linVelocityNormal)
		{
			if (!Sync.IsServer && !m_grid.IsClientPredicted)
			{
				return false;
			}
			MyGridPhysics physics = m_grid.Physics;
			bool result = false;
			MatrixD matrix = m_grid.WorldMatrix;
			Vector3.TransformNormal(ref gridDownNormal, ref matrix, out gridDownNormal);
			gridDownNormal = Vector3.ProjectOnPlane(ref gridDownNormal, ref linVelocityNormal);
			lateralCorrectionNormal = Vector3.ProjectOnPlane(ref lateralCorrectionNormal, ref linVelocityNormal);
			Vector3 vector = Vector3.Cross(gridDownNormal, linVelocityNormal);
			gridDownNormal.Normalize();
			lateralCorrectionNormal.Normalize();
			Vector3 vec = physics.AngularVelocity;
			vec = Vector3.ProjectOnVector(ref vec, ref linVelocityNormal);
			float num = vec.Length();
			float num2 = m_lastPhysicsAngularVelocityLateral.Length();
			if (num > num2)
			{
				result = true;
			}
			float num3 = Vector3.Dot(lateralCorrectionNormal, vector) * (float)Math.Max(1, m_consecutiveCorrectionFrames);
			float value = num3 * num * num;
			if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)
			{
				Vector3D translation = matrix.Translation;
				MyRenderProxy.DebugDrawArrow3DDir(translation, lateralCorrectionNormal * 5f, Color.Yellow);
				MyRenderProxy.DebugDrawArrow3DDir(translation, gridDownNormal * 5f, Color.Pink);
				MyRenderProxy.DebugDrawArrow3DDir(translation, vec * 5f, Color.Red);
			}
			m_lastPhysicsAngularVelocityLateral = vec;
			if (Math.Abs(value) > 0.02f)
			{
				Vector3 vector2 = linVelocityNormal * num3;
				if (Vector3.Dot(vector2, vec) > 0f)
				{
					vector2 = Vector3.ClampToSphere(vector2, vec.Length());
					if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)
					{
						MyRenderProxy.DebugDrawArrow3DDir(matrix.Translation - gridDownNormal * 5f, vector2 * 100f, Color.Red);
					}
					physics.AngularVelocity -= vector2;
					result = true;
				}
			}
			return result;
		}

		public bool HasWorkingWheels(bool propulsion)
		{
			foreach (MyMotorSuspension wheel in m_wheels)
			{
				if (wheel.IsWorking)
				{
					if (!propulsion)
					{
						return true;
					}
					if (wheel.RotorGrid != null && wheel.RotorAngularVelocity.LengthSquared() > 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		internal void InitControl(MyEntity controller)
		{
			foreach (MyMotorSuspension wheel in m_wheels)
			{
				wheel.InitControl(controller);
			}
		}

		internal void ReleaseControl(MyEntity controller)
		{
			foreach (MyMotorSuspension wheel in m_wheels)
			{
				wheel.ReleaseControl(controller);
			}
			UpdateJumpControlState(isCharging: false, sync: false);
		}

		private void UpdateBrake()
		{
			foreach (MyMotorSuspension wheel in m_wheels)
			{
				wheel.Brake = (m_brake | m_handbrake);
			}
		}

		private void OnGridPhysicsChanged(MyEntity obj)
		{
			if (m_grid.GridSystems != null && m_grid.GridSystems.ControlSystem != null)
			{
				MyShipController shipController = m_grid.GridSystems.ControlSystem.GetShipController();
				if (shipController != null)
				{
					InitControl(shipController);
				}
			}
		}

		private void RecomputeWheelParameters()
		{
			m_wheelsChanged = false;
			m_maxRequiredPowerInput = 0f;
			foreach (MyMotorSuspension wheel in m_wheels)
			{
				if (IsUsed(wheel))
				{
					m_maxRequiredPowerInput += wheel.RequiredPowerInput;
				}
			}
			m_sinkComp.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, m_maxRequiredPowerInput);
			m_sinkComp.Update();
		}

		private static bool IsUsed(MyMotorSuspension motor)
		{
			if (motor.Enabled)
			{
				return motor.IsFunctional;
			}
			return false;
		}

		private void MotorEnabledChanged(MyTerminalBlock obj)
		{
			m_wheelsChanged = true;
		}

		private void ReceiverIsPoweredChanged()
		{
			foreach (MyMotorSuspension wheel in m_wheels)
			{
				wheel.UpdateIsWorking();
			}
		}

		public void UpdateJumpControlState(bool isCharging, bool sync)
		{
			if (isCharging && m_grid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != 0)
			{
				isCharging = false;
			}
			bool lastJumpInput = m_lastJumpInput;
			if ((sync || Sync.IsServer) && lastJumpInput != isCharging)
			{
				bool arg = !lastJumpInput;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => InvokeJumpInternal, m_grid.EntityId, arg);
			}
			m_lastJumpInput = isCharging;
		}

		[Event(null, 426)]
		[Reliable]
		[Server]
		[Broadcast]
		public static void InvokeJumpInternal(long gridId, bool initiate)
		{
			MyCubeGrid myCubeGrid = (MyCubeGrid)MyEntities.GetEntityById(gridId);
			if (myCubeGrid == null)
			{
				return;
			}
			if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked)
			{
				MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(myCubeGrid);
				if ((controllingPlayer == null || controllingPlayer.Client.SteamUserId != MyEventContext.Current.Sender.Value) && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
				{
					controllingPlayer = MySession.Static.Players.GetPreviousControllingPlayer(myCubeGrid);
					bool kick = (controllingPlayer == null || controllingPlayer.Client.SteamUserId != MyEventContext.Current.Sender.Value) && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value);
					(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, kick);
					MyEventContext.ValidationFailed();
					return;
				}
			}
			MyWheel.WheelExplosionLog(myCubeGrid, null, "InvokeJump" + initiate);
			MyGridWheelSystem wheelSystem = myCubeGrid.GridSystems.WheelSystem;
			if (initiate)
			{
				wheelSystem.m_jumpState = WheelJumpSate.Charging;
				wheelSystem.m_jumpStartTime = MySandboxGame.Static.SimulationFrameCounter;
			}
			else
			{
				wheelSystem.m_jumpState = WheelJumpSate.Pushing;
			}
			foreach (MyMotorSuspension wheel in wheelSystem.Wheels)
			{
				wheel.OnSuspensionJumpStateUpdated();
			}
		}

		public void SetWheelJumpStrengthRatioIfJumpEngaged(ref float strength, float defaultStrength)
		{
			switch (m_jumpState)
			{
			case WheelJumpSate.Charging:
				strength = 0.0001f;
				break;
			case WheelJumpSate.Pushing:
			{
				ulong num = MySandboxGame.Static.SimulationFrameCounter - m_jumpStartTime;
				float num2 = Math.Min(1f, (float)num / 600f);
				strength = defaultStrength + (1f - defaultStrength) * num2;
				break;
			}
			}
		}
	}
}
