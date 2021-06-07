using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_MotorSuspension))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyMotorSuspension),
		typeof(Sandbox.ModAPI.Ingame.IMyMotorSuspension)
	})]
	public class MyMotorSuspension : MyMotorBase, Sandbox.ModAPI.IMyMotorSuspension, Sandbox.ModAPI.Ingame.IMyMotorSuspension, Sandbox.ModAPI.Ingame.IMyMotorBase, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyMotorBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		private struct MyWheelInversions
		{
			public bool SteerInvert;

			public bool RevolveInvert;
		}

		protected class m_brake_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType brake;
				ISyncType result = brake = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_brake = (Sync<bool, SyncDirection.BothWays>)brake;
				return result;
			}
		}

		protected class m_strenth_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType strenth;
				ISyncType result = strenth = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_strenth = (Sync<float, SyncDirection.BothWays>)strenth;
				return result;
			}
		}

		protected class m_height_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType height;
				ISyncType result = height = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_height = (Sync<float, SyncDirection.BothWays>)height;
				return result;
			}
		}

		protected class m_breakingConstraint_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType breakingConstraint;
				ISyncType result = breakingConstraint = new Sync<bool, SyncDirection.FromServer>(P_1, P_2);
				((MyMotorSuspension)P_0).m_breakingConstraint = (Sync<bool, SyncDirection.FromServer>)breakingConstraint;
				return result;
			}
		}

		protected class m_speedLimit_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType speedLimit;
				ISyncType result = speedLimit = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_speedLimit = (Sync<float, SyncDirection.BothWays>)speedLimit;
				return result;
			}
		}

		protected class m_friction_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType friction;
				ISyncType result = friction = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_friction = (Sync<float, SyncDirection.BothWays>)friction;
				return result;
			}
		}

		protected class m_maxSteerAngle_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType maxSteerAngle;
				ISyncType result = maxSteerAngle = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_maxSteerAngle = (Sync<float, SyncDirection.BothWays>)maxSteerAngle;
				return result;
			}
		}

		protected class m_invertSteer_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType invertSteer;
				ISyncType result = invertSteer = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_invertSteer = (Sync<bool, SyncDirection.BothWays>)invertSteer;
				return result;
			}
		}

		protected class m_invertPropulsion_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType invertPropulsion;
				ISyncType result = invertPropulsion = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_invertPropulsion = (Sync<bool, SyncDirection.BothWays>)invertPropulsion;
				return result;
			}
		}

		protected class m_power_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType power;
				ISyncType result = power = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_power = (Sync<float, SyncDirection.BothWays>)power;
				return result;
			}
		}

		protected class m_steering_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType steering;
				ISyncType result = steering = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_steering = (Sync<bool, SyncDirection.BothWays>)steering;
				return result;
			}
		}

		protected class m_propulsion_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType propulsion;
				ISyncType result = propulsion = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_propulsion = (Sync<bool, SyncDirection.BothWays>)propulsion;
				return result;
			}
		}

		protected class m_airShockEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType airShockEnabled;
				ISyncType result = airShockEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_airShockEnabled = (Sync<bool, SyncDirection.BothWays>)airShockEnabled;
				return result;
			}
		}

		protected class m_brakingEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType brakingEnabled;
				ISyncType result = brakingEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_brakingEnabled = (Sync<bool, SyncDirection.BothWays>)brakingEnabled;
				return result;
			}
		}

		protected class m_propulsionOverride_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType propulsionOverride;
				ISyncType result = propulsionOverride = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_propulsionOverride = (Sync<float, SyncDirection.BothWays>)propulsionOverride;
				return result;
			}
		}

		protected class m_steeringOverride_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType steeringOverride;
				ISyncType result = steeringOverride = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorSuspension)P_0).m_steeringOverride = (Sync<float, SyncDirection.BothWays>)steeringOverride;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyMotorSuspension_003C_003EActor : IActivator, IActivator<MyMotorSuspension>
		{
			private sealed override object CreateInstance()
			{
				return new MyMotorSuspension();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMotorSuspension CreateInstance()
			{
				return new MyMotorSuspension();
			}

			MyMotorSuspension IActivator<MyMotorSuspension>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float MaxSpeedLimit = 360f;

		private const float LOCK_SPEED_SQ = 1f;

		private float m_steerAngle;

		private bool m_wasBreaking;

		private MyWheelInversions m_wheelInversions;

		private readonly Sync<bool, SyncDirection.BothWays> m_brake;

		private readonly Sync<float, SyncDirection.BothWays> m_strenth;

		private readonly Sync<float, SyncDirection.BothWays> m_height;

		private readonly Sync<bool, SyncDirection.FromServer> m_breakingConstraint;

		private static readonly List<HkBodyCollision> m_tmpList = new List<HkBodyCollision>();

		private bool m_wasAccelerating;

		private bool m_updateBrakeNeeded;

		private bool m_updateFrictionNeeded;

		private bool m_updateDampingNeeded;

		private bool m_updateStrengthNeeded;

		private bool m_steeringChanged;

		private Vector3? m_constraintPositionA;

		private Vector3? m_constraintPositionB;

		private HkCustomWheelConstraintData m_wheelConstraintData;

		private bool m_needsUpdate;

		private readonly Sync<float, SyncDirection.BothWays> m_speedLimit;

		private readonly Sync<float, SyncDirection.BothWays> m_friction;

		private readonly Sync<float, SyncDirection.BothWays> m_maxSteerAngle;

		private readonly Sync<bool, SyncDirection.BothWays> m_invertSteer;

		private readonly Sync<bool, SyncDirection.BothWays> m_invertPropulsion;

		private readonly Sync<float, SyncDirection.BothWays> m_power;

		private readonly Sync<bool, SyncDirection.BothWays> m_steering;

		private readonly Sync<bool, SyncDirection.BothWays> m_propulsion;

		private readonly Sync<bool, SyncDirection.BothWays> m_airShockEnabled;

		private readonly Sync<bool, SyncDirection.BothWays> m_brakingEnabled;

		private readonly Sync<float, SyncDirection.BothWays> m_propulsionOverride;

		private readonly Sync<float, SyncDirection.BothWays> m_steeringOverride;

		private HkWheelResponseModifierUtil m_modifier;

		private float m_angleBeforeRemove;

		private bool m_CoMVectorsCacheValid;

		private Vector3 m_adjustmentVectorCache;

		private Vector3 m_realCoMCache;

		private Vector3 m_suspensionPositionCache;

		public bool NeedsPerFrameUpdate
		{
			get
			{
				return m_needsUpdate;
			}
			set
			{
				if (m_needsUpdate != value)
				{
					m_needsUpdate = value;
					base.CubeGrid.GridSystems.WheelSystem.OnBlockNeedsUpdateChanged(this);
				}
			}
		}

		public float SpeedLimit
		{
			get
			{
				return m_speedLimit;
			}
			set
			{
				m_speedLimit.Value = value;
			}
		}

		internal float Strength
		{
			get
			{
				return (float)Math.Sqrt((float)m_strenth);
			}
			set
			{
				if ((float)m_strenth != value * value)
				{
					m_strenth.Value = value * value;
				}
			}
		}

		private HkRigidBody SafeBody
		{
			get
			{
				if (base.TopGrid == null || base.TopGrid.Physics == null)
				{
					return null;
				}
				return base.TopGrid.Physics.RigidBody;
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
				m_brake.Value = value;
			}
		}

		public float Friction
		{
			get
			{
				return m_friction;
			}
			set
			{
				if (m_friction.Value != value)
				{
					m_friction.Value = value;
				}
			}
		}

		public float Height
		{
			get
			{
				return m_height;
			}
			set
			{
				if ((float)m_height != value)
				{
					m_height.Value = value;
				}
			}
		}

		public float MaxSteerAngle
		{
			get
			{
				return m_maxSteerAngle;
			}
			set
			{
				m_steeringChanged = true;
				m_maxSteerAngle.Value = value;
				OnPerFrameUpdatePropertyChanged();
			}
		}

		public bool InvertSteer
		{
			get
			{
				return m_invertSteer;
			}
			set
			{
				m_invertSteer.Value = value;
			}
		}

		public bool InvertPropulsion
		{
			get
			{
				return m_invertPropulsion;
			}
			set
			{
				m_invertPropulsion.Value = value;
			}
		}

		public float SteerAngle
		{
			get
			{
				return m_steerAngle;
			}
			set
			{
				m_steerAngle = value;
			}
		}

		public float Power
		{
			get
			{
				return m_power;
			}
			set
			{
				m_power.Value = value;
			}
		}

		public bool Steering
		{
			get
			{
				return m_steering;
			}
			set
			{
				m_steering.Value = value;
			}
		}

		public bool Propulsion
		{
			get
			{
				return m_propulsion;
			}
			set
			{
				m_propulsion.Value = value;
			}
		}

		public bool AirShockEnabled
		{
			get
			{
				return m_airShockEnabled;
			}
			set
			{
				m_airShockEnabled.Value = value;
			}
		}

		public bool BrakingEnabled
		{
			get
			{
				return m_brakingEnabled;
			}
			set
			{
				m_brakingEnabled.Value = value;
			}
		}

		public float PropulsionOverride
		{
			get
			{
				return m_propulsionOverride;
			}
			set
			{
				m_propulsionOverride.Value = value;
				OnPerFrameUpdatePropertyChanged();
			}
		}

		public float SteeringOverride
		{
			get
			{
				return m_steeringOverride;
			}
			set
			{
				m_steeringChanged = true;
				m_steeringOverride.Value = value;
				OnPerFrameUpdatePropertyChanged();
			}
		}

		private bool ShouldBrake
		{
			get
			{
				if (Brake)
				{
					return BrakingEnabled;
				}
				return false;
			}
		}

		public float CurrentAirShock
		{
			get;
			private set;
		}

		public new MyMotorSuspensionDefinition BlockDefinition => (MyMotorSuspensionDefinition)base.BlockDefinition;

		public new float MaxRotorAngularVelocity => MathF.PI * 12f;

		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Strength
		{
			get
			{
				return GetStrengthForTerminal();
			}
			set
			{
				Strength = MathHelper.Clamp(value / 100f, 0f, 1f);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Friction
		{
			get
			{
				return Friction * 100f;
			}
			set
			{
				Friction = MathHelper.Clamp(value / 100f, 0f, 1f);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Power
		{
			get
			{
				return GetPowerForTerminal();
			}
			set
			{
				Power = MathHelper.Clamp(value / 100f, 0f, 1f);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Height
		{
			get
			{
				return GetHeightForTerminal();
			}
			set
			{
				Height = MathHelper.Clamp(value, BlockDefinition.MinHeight, BlockDefinition.MaxHeight);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.SteerAngle => m_steerAngle;

		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.MaxSteerAngle
		{
			get
			{
				return MaxSteerAngle;
			}
			set
			{
				MaxSteerAngle = MathHelper.Clamp(value, 0f, BlockDefinition.MaxSteer);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyMotorSuspension.Brake
		{
			get
			{
				return BrakingEnabled;
			}
			set
			{
				BrakingEnabled = value;
			}
		}

		[Obsolete]
		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Damping => 70f;

		[Obsolete]
		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.SteerSpeed => 0.01f;

		[Obsolete]
		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.SteerReturnSpeed => 0.01f;

		[Obsolete]
		float Sandbox.ModAPI.Ingame.IMyMotorSuspension.SuspensionTravel => 100f;

		private void PropagateFriction()
		{
			m_updateFrictionNeeded = false;
			MyWheel myWheel = base.TopBlock as MyWheel;
			if (myWheel != null)
			{
				double num = 35.0 * ((double)(MyMath.FastTanH(6f * (float)m_friction - 3f) / 2f) + 0.5);
				if (ShouldBrake)
				{
					num *= 2.0;
				}
				myWheel.Friction = (float)num;
				myWheel.CubeGrid.Physics.RigidBody.Friction = myWheel.Friction;
			}
			else
			{
				m_updateFrictionNeeded = true;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
		}

		public MyMotorSuspension()
		{
			CreateTerminalControls();
			SupportsSharedInertiaTensor = false;
			base.IsWorkingChanged += OnIsWorkingChanged;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyMotorSuspension>())
			{
				base.CreateTerminalControls();
				MyTerminalControlButton<MyMotorSuspension> obj = new MyTerminalControlButton<MyMotorSuspension>("Add Top Part", MySpaceTexts.BlockActionTitle_AddWheel, MySpaceTexts.BlockActionTooltip_AddWheel, delegate(MyMotorSuspension b)
				{
					b.RecreateTop();
				})
				{
					Enabled = ((MyMotorSuspension b) => b.TopBlock == null)
				};
				obj.EnableAction(MyTerminalActionIcons.STATION_ON);
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlCheckbox<MyMotorSuspension> obj2 = new MyTerminalControlCheckbox<MyMotorSuspension>("Steering", MySpaceTexts.BlockPropertyTitle_Motor_Steering, MySpaceTexts.BlockPropertyDescription_Motor_Steering)
				{
					Getter = ((MyMotorSuspension x) => x.Steering),
					Setter = delegate(MyMotorSuspension x, bool v)
					{
						x.Steering = v;
					}
				};
				obj2.EnableAction();
				obj2.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlCheckbox<MyMotorSuspension> obj3 = new MyTerminalControlCheckbox<MyMotorSuspension>("Propulsion", MySpaceTexts.BlockPropertyTitle_Motor_Propulsion, MySpaceTexts.BlockPropertyDescription_Motor_Propulsion)
				{
					Getter = ((MyMotorSuspension x) => x.Propulsion),
					Setter = delegate(MyMotorSuspension x, bool v)
					{
						x.Propulsion = v;
					}
				};
				obj3.EnableAction();
				obj3.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(obj3);
				MyTerminalControlCheckbox<MyMotorSuspension> obj4 = new MyTerminalControlCheckbox<MyMotorSuspension>("Braking", MySpaceTexts.BlockPropertyTitle_Suspension_Brake, MySpaceTexts.BlockPropertyDescription_Suspension_Brake)
				{
					Getter = ((MyMotorSuspension x) => x.BrakingEnabled),
					Setter = delegate(MyMotorSuspension x, bool v)
					{
						x.BrakingEnabled = v;
					}
				};
				obj4.EnableAction();
				MyTerminalControlFactory.AddControl(obj4);
				MyTerminalControlCheckbox<MyMotorSuspension> obj5 = new MyTerminalControlCheckbox<MyMotorSuspension>("AirShock", MySpaceTexts.BlockPropertyTitle_Suspension_AirShock, MySpaceTexts.BlockPropertyDescription_Suspension_AirShock)
				{
					Getter = ((MyMotorSuspension x) => x.AirShockEnabled),
					Setter = delegate(MyMotorSuspension x, bool v)
					{
						x.AirShockEnabled = v;
					}
				};
				obj5.EnableAction();
				MyTerminalControlFactory.AddControl(obj5);
				MyTerminalControlCheckbox<MyMotorSuspension> obj6 = new MyTerminalControlCheckbox<MyMotorSuspension>("InvertSteering", MySpaceTexts.BlockPropertyTitle_Motor_InvertSteer, MySpaceTexts.BlockPropertyDescription_Motor_InvertSteer)
				{
					Getter = ((MyMotorSuspension x) => x.InvertSteer),
					Setter = delegate(MyMotorSuspension x, bool v)
					{
						x.InvertSteer = v;
					}
				};
				obj6.EnableAction();
				obj6.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(obj6);
				MyTerminalControlCheckbox<MyMotorSuspension> obj7 = new MyTerminalControlCheckbox<MyMotorSuspension>("InvertPropulsion", MySpaceTexts.BlockPropertyTitle_Motor_InvertPropulsion, MySpaceTexts.BlockPropertyDescription_Motor_InvertPropulsion)
				{
					Getter = ((MyMotorSuspension x) => x.InvertPropulsion),
					Setter = delegate(MyMotorSuspension x, bool v)
					{
						x.InvertPropulsion = v;
					}
				};
				obj7.EnableAction();
				obj7.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(obj7);
				MyTerminalControlSlider<MyMotorSuspension> myTerminalControlSlider = new MyTerminalControlSlider<MyMotorSuspension>("MaxSteerAngle", MySpaceTexts.BlockPropertyTitle_Motor_MaxSteerAngle, MySpaceTexts.BlockPropertyDescription_Motor_MaxSteerAngle);
				myTerminalControlSlider.SetLimits((MyMotorSuspension x) => 0f, (MyMotorSuspension x) => MathHelper.ToDegrees(x.BlockDefinition.MaxSteer));
				myTerminalControlSlider.DefaultValue = 20f;
				myTerminalControlSlider.Getter = ((MyMotorSuspension x) => MathHelper.ToDegrees(x.MaxSteerAngle));
				myTerminalControlSlider.Setter = delegate(MyMotorSuspension x, float v)
				{
					x.MaxSteerAngle = MathHelper.ToRadians(v);
				};
				myTerminalControlSlider.Writer = delegate(MyMotorSuspension x, StringBuilder res)
				{
					MyMotorStator.WriteAngle(x.MaxSteerAngle, res);
				};
				myTerminalControlSlider.EnableActionsWithReset();
				myTerminalControlSlider.Enabled = ((MyMotorSuspension x) => x.m_constraint != null && x.Steering);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MyMotorSuspension> myTerminalControlSlider2 = new MyTerminalControlSlider<MyMotorSuspension>("Power", MySpaceTexts.BlockPropertyTitle_Motor_Power, MySpaceTexts.BlockPropertyDescription_Motor_Power);
				myTerminalControlSlider2.SetLimits(0f, 100f);
				myTerminalControlSlider2.DefaultValue = 60f;
				myTerminalControlSlider2.Getter = ((MyMotorSuspension x) => x.GetPowerForTerminal());
				myTerminalControlSlider2.Setter = delegate(MyMotorSuspension x, float v)
				{
					x.Power = v / 100f;
				};
				myTerminalControlSlider2.Writer = delegate(MyMotorSuspension x, StringBuilder res)
				{
					res.AppendInt32((int)(x.Power * 100f)).Append("%");
				};
				myTerminalControlSlider2.EnableActions();
				myTerminalControlSlider2.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MyMotorSuspension> myTerminalControlSlider3 = new MyTerminalControlSlider<MyMotorSuspension>("Strength", MySpaceTexts.BlockPropertyTitle_Motor_Strength, MySpaceTexts.BlockPropertyTitle_Motor_Strength);
				myTerminalControlSlider3.SetLimits(0f, 100f);
				myTerminalControlSlider3.DefaultValue = 10f;
				myTerminalControlSlider3.Getter = ((MyMotorSuspension x) => x.GetStrengthForTerminal());
				myTerminalControlSlider3.Setter = delegate(MyMotorSuspension x, float v)
				{
					x.Strength = v / 100f;
				};
				myTerminalControlSlider3.Writer = delegate(MyMotorSuspension x, StringBuilder res)
				{
					res.AppendInt32((int)x.GetStrengthForTerminal()).Append("%");
				};
				myTerminalControlSlider3.EnableActions();
				myTerminalControlSlider3.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				MyTerminalControlSlider<MyMotorSuspension> myTerminalControlSlider4 = new MyTerminalControlSlider<MyMotorSuspension>("Height", MySpaceTexts.BlockPropertyTitle_Motor_Height, MySpaceTexts.BlockPropertyDescription_Motor_Height);
				myTerminalControlSlider4.SetLimits((MyMotorSuspension x) => x.BlockDefinition.MinHeight, (MyMotorSuspension x) => x.BlockDefinition.MaxHeight);
				myTerminalControlSlider4.DefaultValue = 0f;
				myTerminalControlSlider4.Getter = ((MyMotorSuspension x) => x.GetHeightForTerminal());
				myTerminalControlSlider4.Setter = delegate(MyMotorSuspension x, float v)
				{
					x.Height = v;
				};
				myTerminalControlSlider4.Writer = delegate(MyMotorSuspension x, StringBuilder res)
				{
					MyValueFormatter.AppendDistanceInBestUnit(x.Height, res);
				};
				myTerminalControlSlider4.EnableActionsWithReset();
				myTerminalControlSlider4.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
				MyTerminalControlSlider<MyMotorSuspension> myTerminalControlSlider5 = new MyTerminalControlSlider<MyMotorSuspension>("Friction", MySpaceTexts.BlockPropertyTitle_Motor_Friction, MySpaceTexts.BlockPropertyDescription_Motor_Friction);
				myTerminalControlSlider5.SetLimits(0f, 100f);
				myTerminalControlSlider5.DefaultValue = 50f;
				myTerminalControlSlider5.Getter = ((MyMotorSuspension x) => x.Friction * 100f);
				myTerminalControlSlider5.Setter = delegate(MyMotorSuspension x, float v)
				{
					x.Friction = v / 100f;
				};
				myTerminalControlSlider5.Writer = delegate(MyMotorSuspension x, StringBuilder res)
				{
					res.AppendDecimal(x.Friction * 100f, 2).Append("%");
				};
				myTerminalControlSlider5.EnableActions();
				myTerminalControlSlider5.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider5);
				MyTerminalControlSlider<MyMotorSuspension> myTerminalControlSlider6 = new MyTerminalControlSlider<MyMotorSuspension>("Speed Limit", MySpaceTexts.BlockPropertyTitle_Motor_SuspensionSpeed, MySpaceTexts.BlockPropertyDescription_Motor_SuspensionSpeed);
				myTerminalControlSlider6.SetLimits(0f, 360f);
				myTerminalControlSlider6.DefaultValue = 180f;
				myTerminalControlSlider6.Getter = ((MyMotorSuspension x) => x.SpeedLimit);
				myTerminalControlSlider6.Setter = delegate(MyMotorSuspension x, float v)
				{
					x.SpeedLimit = v;
				};
				myTerminalControlSlider6.Writer = delegate(MyMotorSuspension x, StringBuilder res)
				{
					if (x.SpeedLimit >= 360f)
					{
						res.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_MotorAngleUnlimited));
					}
					else
					{
						res.AppendInt32((int)x.SpeedLimit).Append("km/h");
					}
				};
				myTerminalControlSlider6.EnableActionsWithReset();
				myTerminalControlSlider6.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider6);
				MyTerminalControlSlider<MyMotorSuspension> myTerminalControlSlider7 = new MyTerminalControlSlider<MyMotorSuspension>("Propulsion override", MySpaceTexts.BlockPropertyTitle_Motor_PropulsionOverride, MySpaceTexts.BlockPropertyDescription_Motor_PropulsionOverride);
				myTerminalControlSlider7.SetLimits(-1f, 1f);
				myTerminalControlSlider7.DefaultValue = 0f;
				myTerminalControlSlider7.Getter = ((MyMotorSuspension x) => x.PropulsionOverride);
				myTerminalControlSlider7.Setter = delegate(MyMotorSuspension x, float v)
				{
					x.PropulsionOverride = v;
				};
				myTerminalControlSlider7.Writer = delegate(MyMotorSuspension x, StringBuilder res)
				{
					res.AppendDecimal(x.PropulsionOverride * 100f, 2).Append("%");
				};
				myTerminalControlSlider7.EnableActionsWithReset();
				myTerminalControlSlider7.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider7);
				MyTerminalControlSlider<MyMotorSuspension> myTerminalControlSlider8 = new MyTerminalControlSlider<MyMotorSuspension>("Steer override", MySpaceTexts.BlockPropertyTitle_Motor_SteerOverride, MySpaceTexts.BlockPropertyDescription_Motor_SteerOverride);
				myTerminalControlSlider8.SetLimits(-1f, 1f);
				myTerminalControlSlider8.DefaultValue = 0f;
				myTerminalControlSlider8.Getter = ((MyMotorSuspension x) => x.SteeringOverride);
				myTerminalControlSlider8.Setter = delegate(MyMotorSuspension x, float v)
				{
					x.SteeringOverride = v;
				};
				myTerminalControlSlider8.Writer = delegate(MyMotorSuspension x, StringBuilder res)
				{
					res.AppendDecimal(x.SteeringOverride * 100f, 2).Append("%");
				};
				myTerminalControlSlider8.EnableActionsWithReset();
				myTerminalControlSlider8.Enabled = ((MyMotorSuspension x) => x.m_constraint != null);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider8);
			}
		}

		private void FrictionChanged()
		{
			PropagateFriction();
		}

		private void DampingChanged()
		{
			m_updateDampingNeeded = false;
			if (base.SafeConstraint != null && base.TopBlock != null)
			{
				UpdateConstraintData();
				return;
			}
			m_updateDampingNeeded = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
		}

		private void StrengthChanged()
		{
			m_updateStrengthNeeded = false;
			if (base.SafeConstraint != null && base.TopBlock != null)
			{
				UpdateConstraintData();
				return;
			}
			m_updateStrengthNeeded = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
		}

		private void HeightChanged()
		{
			if (base.SafeConstraint != null && base.TopBlock != null)
			{
				UpdateConstraintData();
			}
		}

		private void BreakingConstraintChanged()
		{
			if (!(m_wheelConstraintData == null))
			{
				if ((bool)m_breakingConstraint)
				{
					float currentAngle = HkCustomWheelConstraintData.GetCurrentAngle(m_constraint);
					MyWheel.WheelExplosionLog(base.CubeGrid, this, "OnBrakeAngleLimit: " + currentAngle);
					m_wheelConstraintData.SetAngleLimits(currentAngle, currentAngle);
					ActivatePhysics();
				}
				else
				{
					MyWheel.WheelExplosionLog(base.CubeGrid, this, "OnBrakeAngleLimitReleased");
					m_wheelConstraintData.DisableLimits();
				}
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_MotorSuspension myObjectBuilder_MotorSuspension = objectBuilder as MyObjectBuilder_MotorSuspension;
			m_steerAngle = MathHelper.Clamp(myObjectBuilder_MotorSuspension.SteerAngle, 0f - BlockDefinition.MaxSteer, BlockDefinition.MaxSteer);
			float num = MathHelper.Clamp(Math.Min(myObjectBuilder_MotorSuspension.Strength, myObjectBuilder_MotorSuspension.StrengthNew), 0f, 1f);
			m_strenth.SetLocalValue(num * num);
			m_steering.SetLocalValue(myObjectBuilder_MotorSuspension.Steering);
			m_propulsion.SetLocalValue(myObjectBuilder_MotorSuspension.Propulsion);
			m_power.SetLocalValue(MathHelper.Clamp(Math.Max(myObjectBuilder_MotorSuspension.Power, myObjectBuilder_MotorSuspension.PowerNew), 0f, 1f));
			m_height.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorSuspension.Height, BlockDefinition.MinHeight, BlockDefinition.MaxHeight));
			m_maxSteerAngle.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorSuspension.MaxSteerAngle, 0f, BlockDefinition.MaxSteer));
			m_invertSteer.SetLocalValue(myObjectBuilder_MotorSuspension.InvertSteer);
			m_invertPropulsion.SetLocalValue(myObjectBuilder_MotorSuspension.InvertPropulsion);
			m_speedLimit.SetLocalValue(myObjectBuilder_MotorSuspension.SpeedLimit);
			m_friction.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorSuspension.FrictionNew ?? (((double)(myObjectBuilder_MotorSuspension.Friction / 4f) < 0.2) ? 0.05f : 0.5f), 0f, 1f));
			m_airShockEnabled.SetLocalValue(myObjectBuilder_MotorSuspension.AirShockEnabled);
			m_brakingEnabled.SetLocalValue(myObjectBuilder_MotorSuspension.BrakingEnabled);
			m_steeringOverride.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorSuspension.SteeringOverride, -1f, 1f));
			m_propulsionOverride.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorSuspension.PropulsionOverride, -1f, 1f));
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_OnHavokSystemIDChanged;
			base.CubeGrid.OnMassPropertiesChanged += CubeGrid_OnMassPropertiesChanged;
			m_brake.ValueChanged += delegate
			{
				UpdateBrake();
			};
			m_brakingEnabled.ValueChanged += delegate
			{
				UpdateBrake();
			};
			m_friction.ValueChanged += delegate
			{
				FrictionChanged();
			};
			m_strenth.ValueChanged += delegate
			{
				StrengthChanged();
			};
			m_height.ValueChanged += delegate
			{
				HeightChanged();
			};
			m_breakingConstraint.ValueChanged += delegate
			{
				BreakingConstraintChanged();
			};
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			if (Sync.IsServer)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
			AddDebugRenderComponent(new MyDebugRenderComponentMotorSuspension(this));
		}

		public override void OnRemovedFromScene(object source)
		{
			MyWheel.DumpActivityLog();
			base.OnRemovedFromScene(source);
			base.CubeGrid.OnMassPropertiesChanged -= CubeGrid_OnMassPropertiesChanged;
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			MyWheel.WheelExplosionLog(oldGrid, this, "OnSuspensionMoved " + base.CubeGrid.DisplayName);
			base.OnCubeGridChanged(oldGrid);
			oldGrid.OnMassPropertiesChanged -= CubeGrid_OnMassPropertiesChanged;
			base.CubeGrid.OnMassPropertiesChanged += CubeGrid_OnMassPropertiesChanged;
		}

		private void CubeGrid_OnMassPropertiesChanged(MyCubeGrid obj)
		{
			if (!(base.SafeConstraint == null))
			{
				m_CoMVectorsCacheValid = false;
			}
		}

		private void CubeGrid_OnHavokSystemIDChanged(int obj)
		{
			CubeGrid_OnPhysicsChanged(base.CubeGrid);
		}

		private void CubeGrid_OnPhysicsChanged(MyEntity obj)
		{
			if (base.CubeGrid.Physics != null && base.TopGrid != null && base.TopGrid.Physics != null)
			{
				HkRigidBody rigidBody = base.TopGrid.Physics.RigidBody;
				if (!(rigidBody == null))
				{
					uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(rigidBody.Layer, base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, 1, 1);
					rigidBody.SetCollisionFilterInfo(collisionFilterInfo);
				}
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateBrake();
		}

		public override void UpdateBeforeSimulation()
		{
			base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			base.UpdateBeforeSimulation();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_MotorSuspension obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_MotorSuspension;
			obj.FrictionNew = Friction;
			obj.SteerAngle = m_steerAngle;
			obj.Steering = Steering;
			obj.Strength = float.MaxValue;
			obj.StrengthNew = Strength;
			obj.Propulsion = Propulsion;
			obj.Height = Height;
			obj.MaxSteerAngle = MaxSteerAngle;
			obj.InvertSteer = InvertSteer;
			obj.InvertPropulsion = InvertPropulsion;
			obj.SpeedLimit = SpeedLimit;
			obj.PowerNew = Power;
			obj.Power = 0f;
			obj.AirShockEnabled = AirShockEnabled;
			obj.BrakingEnabled = BrakingEnabled;
			obj.SteeringOverride = SteeringOverride;
			obj.PropulsionOverride = PropulsionOverride;
			return obj;
		}

		protected override bool CheckIsWorking()
		{
			return base.CheckIsWorking() & (base.TopBlock != null && base.TopBlock.IsWorking);
		}

		protected override float ComputeRequiredPowerInput()
		{
			if (base.TopBlock == null)
			{
				return 0f;
			}
			float num = base.ComputeRequiredPowerInput();
			if (num > 0f)
			{
				float requiredIdlePowerInput = BlockDefinition.RequiredIdlePowerInput;
				float amount = Power * ((PropulsionOverride > 0f) ? PropulsionOverride : 1f);
				float num2 = MathHelper.Lerp(requiredIdlePowerInput, num, amount);
				float num3 = (num2 - requiredIdlePowerInput) / (float)(m_wasAccelerating ? 15 : (-50));
				num = MathHelper.Clamp(base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) + num3, requiredIdlePowerInput, num2);
				OnPerFrameUpdatePropertyChanged();
			}
			return num;
		}

		public void UpdateBrake()
		{
			m_updateBrakeNeeded = false;
			if (SafeBody != null)
			{
				PropagateFriction();
				if (ShouldBrake)
				{
					SafeBody.AngularDamping = BlockDefinition.PropulsionForce;
					return;
				}
				SafeBody.AngularDamping = base.CubeGrid.Physics.AngularDamping;
				if (Sync.IsServer && m_constraint != null && (bool)m_breakingConstraint)
				{
					m_breakingConstraint.Value = false;
				}
			}
			else
			{
				m_updateBrakeNeeded = true;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public void InitControl(MyEntity controller)
		{
			MatrixD matrixD = controller.WorldMatrix * base.PositionComp.WorldMatrixNormalizedInv;
			Vector3 value = (Base6Directions.GetClosestDirection(matrixD.Forward) != Base6Directions.Direction.Up && Base6Directions.GetClosestDirection(matrixD.Forward) != Base6Directions.Direction.Down) ? ((Base6Directions.GetClosestDirection(matrixD.Up) != Base6Directions.Direction.Up && Base6Directions.GetClosestDirection(matrixD.Up) != Base6Directions.Direction.Down) ? ((Vector3)controller.WorldMatrix.Right) : ((Vector3)controller.WorldMatrix.Up)) : ((Vector3)controller.WorldMatrix.Forward);
			Vector3 vector = (Base6Directions.GetClosestDirection(matrixD.Forward) != 0 && Base6Directions.GetClosestDirection(matrixD.Forward) != Base6Directions.Direction.Backward) ? ((Base6Directions.GetClosestDirection(matrixD.Up) != 0 && Base6Directions.GetClosestDirection(matrixD.Up) != Base6Directions.Direction.Backward) ? ((Vector3)controller.WorldMatrix.Right) : ((Vector3)controller.WorldMatrix.Up)) : ((Vector3)controller.WorldMatrix.Forward);
			if (base.CubeGrid.Physics != null)
			{
				double num = (double)Vector3.Dot(controller.WorldMatrix.Forward, base.WorldMatrix.Translation - base.CubeGrid.Physics.CenterOfMassWorld) - 0.0001;
				float num2 = Vector3.Dot(base.WorldMatrix.Forward, vector);
				m_wheelInversions = new MyWheelInversions
				{
					SteerInvert = (num * (double)num2 < 0.0),
					RevolveInvert = ((base.WorldMatrix.Up - value).Length() > 0.10000000149011612)
				};
			}
		}

		public void ReleaseControl(MyEntity controller)
		{
		}

		protected override bool Attach(MyAttachableTopBlockBase rotor, bool updateGroup = true)
		{
			if (rotor is MyMotorRotor && base.Attach(rotor, updateGroup))
			{
				MyWheel.WheelExplosionLog(base.CubeGrid, this, "OnAttach" + Environment.NewLine + Environment.StackTrace);
				CreateConstraint(rotor);
				PropagateFriction();
				UpdateIsWorking();
				if (m_updateBrakeNeeded)
				{
					UpdateBrake();
				}
				return true;
			}
			return false;
		}

		protected override void Detach(bool updateGroups = true)
		{
			base.Detach(updateGroups);
			if (m_modifier != null)
			{
				m_modifier.Dispose();
				m_modifier = null;
			}
			MyWheel.WheelExplosionLog(base.CubeGrid, this, "OnDetach" + Environment.NewLine + Environment.StackTrace);
			MyWheel.DumpActivityLog();
		}

		protected override bool CreateConstraint(MyAttachableTopBlockBase rotor)
		{
			if (!base.CreateConstraint(rotor))
			{
				return false;
			}
			MyCubeGrid topGrid = base.TopGrid;
			HkRigidBody rigidBody = base.TopGrid.Physics.RigidBody;
			if (m_modifier != null)
			{
				m_modifier.Dispose();
			}
			m_modifier = HkWheelResponseModifierUtil.Create(rigidBody, () => MyPhysicsConfig.WheelSoftnessRatio, () => MyPhysicsConfig.WheelSoftnessVelocity);
			rigidBody.MaxAngularVelocity = float.MaxValue;
			rigidBody.Restitution = 0.5f;
			topGrid.OnPhysicsChanged += OnRotorPhysicsChanged;
			OnRotorPhysicsChanged(topGrid);
			if (MyFakes.WHEEL_SOFTNESS)
			{
				HkUtils.SetSoftContact(rigidBody, null, MyPhysicsConfig.WheelSoftnessRatio, MyPhysicsConfig.WheelSoftnessVelocity);
			}
			uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(rigidBody.Layer, base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, 1, 1);
			rigidBody.SetCollisionFilterInfo(collisionFilterInfo);
			base.CubeGrid.GetPhysicsBody().HavokWorld.RefreshCollisionFilterOnEntity(rigidBody);
			m_wheelConstraintData = new HkCustomWheelConstraintData();
			FillConstraintData(rotor, out Vector3 posA, out Vector3 posB);
			m_constraint = new HkConstraint(rigidBody, base.CubeGrid.Physics.RigidBody, m_wheelConstraintData);
			m_constraint.WantRuntime = true;
			base.CubeGrid.Physics.AddConstraint(m_constraint);
			if (!m_constraint.InWorld)
			{
				base.CubeGrid.Physics.RemoveConstraint(m_constraint);
				m_constraint = null;
				return false;
			}
			m_constraint.Enabled = true;
			if (Sync.IsServer)
			{
				m_breakingConstraint.Value = false;
			}
			BreakingConstraintChanged();
			ActivatePhysics();
			if (posA == Vector3.Zero)
			{
				m_constraintPositionA = null;
			}
			else
			{
				m_constraintPositionA = posA;
			}
			if (posB == Vector3.Zero)
			{
				m_constraintPositionB = null;
			}
			else
			{
				m_constraintPositionB = posB;
			}
			HkConstraint constraint = m_constraint;
			constraint.OnRemovedFromWorldCallback = (Action)Delegate.Combine(constraint.OnRemovedFromWorldCallback, (Action)delegate
			{
				m_angleBeforeRemove = HkCustomWheelConstraintData.GetCurrentAngle(m_constraint);
			});
			HkConstraint constraint2 = m_constraint;
			constraint2.OnAddedToWorldCallback = (Action)Delegate.Combine(constraint2.OnAddedToWorldCallback, (Action)delegate
			{
				HkCustomWheelConstraintData.SetCurrentAngle(m_constraint, m_angleBeforeRemove);
			});
			return true;
		}

		protected override void DisposeConstraint(MyCubeGrid topGrid)
		{
			if (topGrid != null)
			{
				topGrid.OnPhysicsChanged -= OnRotorPhysicsChanged;
				base.DisposeConstraint(topGrid);
				m_wheelConstraintData = null;
			}
		}

		protected override MatrixD GetTopGridMatrix()
		{
			Vector3 forward = base.PositionComp.LocalMatrix.Forward;
			return MatrixD.CreateWorld(Vector3D.Transform(base.DummyPosition + forward * m_height, base.CubeGrid.WorldMatrix), base.WorldMatrix.Forward, base.WorldMatrix.Up);
		}

		private void UpdateConstraintData()
		{
			MyAttachableTopBlockBase topBlock = base.TopBlock;
			if (topBlock != null && !(m_wheelConstraintData == null))
			{
				FillConstraintData(topBlock, out Vector3 _, out Vector3 _);
			}
		}

		private void OnRotorPhysicsChanged(MyEntity rotorGrid)
		{
			if (rotorGrid != base.TopGrid)
			{
				rotorGrid.OnPhysicsChanged -= OnRotorPhysicsChanged;
				return;
			}
			MyGridPhysics physics = ((MyCubeGrid)rotorGrid).Physics;
			physics?.Shape.UnmarkBreakable(physics.RigidBody);
		}

		private void FillConstraintData(MyAttachableTopBlockBase rotor, out Vector3 posA, out Vector3 posB)
		{
			float currentAirShock = CurrentAirShock;
			float suspensionDamping = MathHelper.Lerp(0.7f, 1f, currentAirShock);
			float strength = MathHelper.Lerp(m_strenth, 1f, currentAirShock);
			if (base.IsWorking)
			{
				base.CubeGrid.GridSystems.WheelSystem.SetWheelJumpStrengthRatioIfJumpEngaged(ref strength, m_strenth);
			}
			float num = MathHelper.Lerp(m_height, BlockDefinition.MinHeight, currentAirShock);
			MyWheel.WheelExplosionLog(base.CubeGrid, this, string.Concat("FillConstraint: ", base.CubeGrid.GridSystems.WheelSystem.m_jumpState, " ", strength, " ", num, " ", currentAirShock));
			Vector3 forward = base.PositionComp.LocalMatrix.Forward;
			posA = base.DummyPosition + forward * num;
			posB = (rotor as MyMotorRotor).WheelDummy;
			Vector3 up = base.PositionComp.LocalMatrix.Up;
			Vector3 up2 = rotor.PositionComp.LocalMatrix.Up;
			m_wheelConstraintData.SetInBodySpace(posB, posA, up2, up, forward, forward, base.RotorGrid.Physics, base.CubeGrid.Physics);
			m_wheelConstraintData.SetSuspensionDamping(suspensionDamping);
			m_wheelConstraintData.SetSuspensionStrength(strength);
			m_wheelConstraintData.SuspensionMinLimit = BlockDefinition.MinHeight - num;
			m_wheelConstraintData.SuspensionMaxLimit = BlockDefinition.MaxHeight - num;
			m_wheelConstraintData.FrictionEnabled = true;
			m_wheelConstraintData.MaxFrictionTorque = BlockDefinition.AxleFriction;
			ActivatePhysics();
		}

		public void GetCoMVectors(out Vector3 adjustmentVector)
		{
			GetCoMVectors(out adjustmentVector, out Vector3 _, out Vector3 _);
		}

		public void GetCoMVectors(out Vector3 adjustmentVector, out Vector3 realCoM, out Vector3 suspensionPosition)
		{
			if (m_CoMVectorsCacheValid)
			{
				realCoM = m_realCoMCache;
				adjustmentVector = m_adjustmentVectorCache;
				suspensionPosition = m_suspensionPositionCache;
				return;
			}
			realCoM = base.CubeGrid.Physics.RigidBody.CenterOfMassLocal;
			suspensionPosition = base.Position * base.CubeGrid.GridSize;
			Vector3 projectedOntoVector = -Base6Directions.GetVector(base.Orientation.Forward);
			adjustmentVector = projectedOntoVector.Project(suspensionPosition - realCoM);
			adjustmentVector *= 0.9f;
			m_CoMVectorsCacheValid = true;
			m_realCoMCache = realCoM;
			m_adjustmentVectorCache = adjustmentVector;
			m_suspensionPositionCache = suspensionPosition;
		}

		public void DebugDrawConstraint()
		{
			if (m_constraint != null)
			{
				m_constraint.GetPivotsInWorld(out Vector3 pivotA, out Vector3 pivotB);
				Vector3D value = base.CubeGrid.Physics.ClusterToWorld(pivotA);
				Vector3D vector3D = base.CubeGrid.Physics.ClusterToWorld(pivotB);
				Vector3D up = base.WorldMatrix.Up;
				Vector3D forward = base.WorldMatrix.Forward;
				Vector3D vector3D2 = vector3D + up;
				Vector3D vector3D3 = vector3D + forward * m_wheelConstraintData.SuspensionMaxLimit;
				Vector3D vector3D4 = vector3D + forward * m_wheelConstraintData.SuspensionMinLimit;
				MyRenderProxy.DebugDrawSphere(vector3D, 0.05f, Color.Red, 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(vector3D2, 0.05f, Color.Red, 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(value + up, 0.05f, Color.Red, 1f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D, vector3D2, Color.Red, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D, value + up, Color.Red, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D3, vector3D3 + up, Color.Blue, Color.Blue, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D4, vector3D4 + up, Color.Blue, Color.Blue, depthRead: false);
			}
		}

		public override void ComputeTopQueryBox(out Vector3D pos, out Vector3 halfExtents, out Quaternion orientation)
		{
			MatrixD worldMatrix = base.WorldMatrix;
			orientation = Quaternion.CreateFromRotationMatrix(worldMatrix);
			halfExtents = Vector3.One * base.CubeGrid.GridSize * 0.35f;
			halfExtents.Y = base.CubeGrid.GridSize;
			pos = worldMatrix.Translation + 0.35f * base.CubeGrid.GridSize * base.WorldMatrix.Up;
		}

		protected override bool CanPlaceTop(MyAttachableTopBlockBase topBlock, long builtBy)
		{
			return CanPlaceRotor(topBlock, builtBy);
		}

		protected override bool CanPlaceRotor(MyAttachableTopBlockBase rotorBlock, long builtBy)
		{
			Vector3 forward = base.PositionComp.LocalMatrix.Forward;
			Vector3D translation = Vector3D.Transform(base.DummyPosition + forward * m_height, base.CubeGrid.WorldMatrix);
			Matrix localMatrix = rotorBlock.PositionComp.LocalMatrix;
			Vector3 vector = -Vector3.TransformNormal(Vector3.Transform(rotorBlock.WheelDummy, Matrix.Invert(localMatrix)), rotorBlock.WorldMatrix);
			translation += vector;
			float num = rotorBlock.ModelCollision.HavokCollisionShapes[0].ConvexRadius * 0.9f;
			BoundingSphereD sphere = rotorBlock.Model.BoundingSphere;
			sphere.Center = translation;
			sphere.Radius = num;
			base.CubeGrid.GetBlocksInsideSphere(ref sphere, MyMechanicalConnectionBlockBase.m_tmpSet);
			if (MyMechanicalConnectionBlockBase.m_tmpSet.Count > 1)
			{
				MyMechanicalConnectionBlockBase.m_tmpSet.Clear();
				if (builtBy == MySession.Static.LocalPlayerId)
				{
					MyHud.Notifications.Add(MyNotificationSingletons.WheelNotPlaced);
				}
				return false;
			}
			MyMechanicalConnectionBlockBase.m_tmpSet.Clear();
			MyPhysics.GetPenetrationsShape(rotorBlock.ModelCollision.HavokCollisionShapes[0], ref translation, ref Quaternion.Identity, m_tmpList, 15);
			for (int i = 0; i < m_tmpList.Count; i++)
			{
				VRage.ModAPI.IMyEntity collisionEntity = m_tmpList[i].GetCollisionEntity();
				if (collisionEntity.Physics.IsPhantom)
				{
					continue;
				}
				MyCubeGrid myCubeGrid = collisionEntity.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid == null || myCubeGrid != base.CubeGrid)
				{
					m_tmpList.Clear();
					if (builtBy == MySession.Static.LocalPlayerId)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.WheelNotPlaced);
					}
					return false;
				}
			}
			m_tmpList.Clear();
			return true;
		}

		internal void AxleFrictionLogic(float maxSpeedRatio, bool anyPropulsion)
		{
			if (!MyPhysicsConfig.OverrideWheelAxleFriction && !(m_wheelConstraintData == null))
			{
				bool flag = !anyPropulsion;
				m_wheelConstraintData.FrictionEnabled = flag;
				if (flag)
				{
					float num = Math.Max(0.1f, maxSpeedRatio);
					m_wheelConstraintData.MaxFrictionTorque = BlockDefinition.AxleFriction * num;
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)
				{
					MyRenderProxy.DebugDrawText3D(text: "F: " + (flag ? m_wheelConstraintData.MaxFrictionTorque : 0f).ToString("F1"), worldCoord: base.WorldMatrix.Translation, color: Color.Red, scale: 0.5f, depthRead: false);
				}
			}
		}

		internal void UpdatePropulsion(bool forward, bool backwards)
		{
			float propulsionOverride = PropulsionOverride;
			if (propulsionOverride != 0f)
			{
				bool flag = propulsionOverride > 0f;
				if (InvertPropulsion)
				{
					flag = !flag;
				}
				Accelerate(Math.Abs(propulsionOverride) * BlockDefinition.PropulsionForce, flag);
			}
			else if (forward)
			{
				Accelerate(BlockDefinition.PropulsionForce * Power, !InvertPropulsion);
			}
			else if (backwards)
			{
				Accelerate(BlockDefinition.PropulsionForce * Power, InvertPropulsion);
			}
		}

		private void Accelerate(float force, bool forward)
		{
			if (!base.IsWorking)
			{
				return;
			}
			MyCubeGrid topGrid = base.TopGrid;
			if (topGrid == null)
			{
				return;
			}
			MatrixD worldMatrix = topGrid.WorldMatrix;
			MyGridPhysics physics = base.TopGrid.Physics;
			MyGridPhysics physics2 = base.CubeGrid.Physics;
			if (physics == null)
			{
				return;
			}
			float num = physics2.LinearVelocity.Length();
			bool flag = (PropulsionOverride == 0f) ? (m_wheelInversions.RevolveInvert == forward) : forward;
			float num2 = Vector3.Dot(physics.AngularVelocity, flag ? ((Vector3)worldMatrix.Up) : ((Vector3)worldMatrix.Down));
			if (num > SpeedLimit * (5f / 18f) && num2 > 0f)
			{
				return;
			}
			float num3 = (float)base.TopBlock.BlockDefinition.Size.X * topGrid.GridSizeHalf;
			float num4 = physics.AngularVelocity.LengthSquared();
			float num5 = SpeedLimit * (5f / 18f) / num3;
			num5 *= num5;
			if (num4 > num5 && num2 > 0f)
			{
				return;
			}
			float num6 = 1f;
			if (MyFakes.SUSPENSION_POWER_RATIO)
			{
				float num7 = 1f;
				if (MyDebugDrawSettings.DEBUG_DRAW_SUSPENSION_POWER)
				{
					for (int i = 2; i < 20; i++)
					{
						num7 = (i - 1) * 10;
						num6 = 1f - (num7 - 10f) / (physics2.RigidBody.MaxLinearVelocity - 20f);
						float num8 = Math.Min(1f, num6);
						num7 = i * 10;
						num6 = 1f - (num7 - 10f) / (physics2.RigidBody.MaxLinearVelocity - 20f);
						float num9 = Math.Min(1f, num6);
						MyRenderProxy.DebugDrawLine2D(new Vector2(300 + i * 20, 400f - num9 * 200f), new Vector2(300 + (i - 1) * 20, 400f - num8 * 200f), Color.Yellow, Color.Yellow);
						MyRenderProxy.DebugDrawText2D(new Vector2(300 + (i - 1) * 20, 400f), ((i - 1) * 10).ToString(), Color.Yellow, 0.35f);
					}
				}
				num7 = physics.AngularVelocity.Length() * num3;
				num6 = 1f - (num7 - 10f) / (physics2.RigidBody.MaxLinearVelocity - 20f);
				num6 = MathHelper.Clamp(num6, 0f, 1f);
				if (MyDebugDrawSettings.DEBUG_DRAW_SUSPENSION_POWER)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(300f + num7 * 2f, 400f - num6 * 200f), "I", Color.Red, 0.3f);
					MyRenderProxy.DebugDrawText2D(new Vector2(290f, 400f - num6 * 200f), num6.ToString(), Color.Yellow, 0.35f);
				}
			}
			num6 /= MathHelper.Lerp(1f, 2f, Math.Min(10f, num) / 10f);
			force *= num6;
			HkRigidBody rigidBody = physics.RigidBody;
			if (flag)
			{
				rigidBody.ApplyAngularImpulse(rigidBody.GetRigidBodyMatrix().Up * force);
			}
			else
			{
				rigidBody.ApplyAngularImpulse((Vector3)worldMatrix.Down * force);
			}
			m_wasAccelerating = true;
		}

		public void Steer(float destIndicator, float maxSpeedRatio)
		{
			if (SteeringOverride != 0f)
			{
				destIndicator = SteeringOverride;
			}
			else if (m_wheelInversions.SteerInvert)
			{
				destIndicator = 0f - destIndicator;
			}
			if (!InvertSteer)
			{
				destIndicator = 0f - destIndicator;
			}
			destIndicator = MathHelper.Clamp(destIndicator, -1f, 1f);
			float num = 1f - maxSpeedRatio;
			num = num * num * num;
			float maxSteerAngle = MaxSteerAngle;
			float value = maxSteerAngle / 10f;
			float num2 = MathHelper.Lerp(maxSteerAngle, value, 1f - num) * destIndicator;
			bool num3 = m_steerAngle < num2;
			float num4 = BlockDefinition.SteeringSpeed;
			if ((double)maxSpeedRatio < 0.01)
			{
				num4 *= 2f;
			}
			float num5 = num4 * (0.2f + 0.8f * num);
			float num6 = num2 - m_steerAngle;
			if (float.IsNaN(num6))
			{
				num6 = 0f;
			}
			m_steerAngle += num5 * (float)Math.Sign(num6);
			bool flag = m_steerAngle < num2;
			bool flag2 = num3 == flag && num6 != 0f;
			if (m_steeringChanged != flag2)
			{
				m_steeringChanged = flag2;
				OnPerFrameUpdatePropertyChanged();
			}
			if (flag2 && m_steerAngle != num2 && base.SafeConstraint != null && Steering)
			{
				ActivatePhysics();
				m_wheelConstraintData.SetSteeringAngle(m_steerAngle);
			}
		}

		public void Update()
		{
			AirShockLogic();
			ArtificialBreakingLogic();
			if (MyPhysicsConfig.OverrideWheelAxleFriction && m_wheelConstraintData != null)
			{
				m_wheelConstraintData.FrictionEnabled = true;
				m_wheelConstraintData.MaxFrictionTorque = MyPhysicsConfig.WheelAxleFriction;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
			{
				GetCoMVectors(out Vector3 adjustmentVector, out Vector3 realCoM, out Vector3 suspensionPosition);
				MatrixD worldMatrix = base.CubeGrid.WorldMatrix;
				MyRenderProxy.DebugDrawSphere(Vector3D.Transform(realCoM, worldMatrix), 0.1f, Color.Red, 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(Vector3D.Transform(suspensionPosition, worldMatrix), 0.1f, Color.Green, 1f, depthRead: false);
				MyRenderProxy.DebugDrawArrow3DDir(Vector3D.Transform(realCoM, worldMatrix), Vector3D.TransformNormal(adjustmentVector, worldMatrix), Color.Yellow);
				MyRenderProxy.DebugDrawArrow3DDir(Vector3D.Transform(suspensionPosition, worldMatrix), Vector3D.TransformNormal(adjustmentVector, worldMatrix), Color.Blue);
			}
			if (Sync.IsServer && ShouldBrake && m_constraint != null && !m_breakingConstraint && base.TopGrid.Physics.LinearVelocity.LengthSquared() < 1f)
			{
				m_breakingConstraint.Value = true;
			}
			UpdateSoundState();
			base.ResourceSink.Update();
			m_wasAccelerating = false;
		}

		protected override void UpdateSoundState()
		{
			if (!MySandboxGame.IsGameReady || m_soundEmitter == null)
			{
				return;
			}
			if (base.TopGrid == null || base.TopGrid.Physics == null)
			{
				m_soundEmitter.StopSound(forced: true);
				return;
			}
			if (base.IsWorking && Math.Abs(base.TopGrid.Physics.RigidBody.DeltaAngle.W - base.CubeGrid.Physics.RigidBody.DeltaAngle.W) > 0.0025f)
			{
				m_soundEmitter.PlaySingleSound(BlockDefinition.PrimarySound, stopPrevious: true);
			}
			else if (m_soundEmitter != null && m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: false);
			}
			if (m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying)
			{
				float semitones = 4f * (Math.Abs(base.RotorAngularVelocity.Length()) - 0.5f * MaxRotorAngularVelocity) / MaxRotorAngularVelocity;
				m_soundEmitter.Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio(semitones) * (m_wasAccelerating ? 1f : 0.95f);
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
			if (m_updateBrakeNeeded)
			{
				UpdateBrake();
			}
			if (m_updateFrictionNeeded)
			{
				FrictionChanged();
			}
			if (m_updateDampingNeeded)
			{
				DampingChanged();
			}
			if (m_updateStrengthNeeded)
			{
				StrengthChanged();
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			CheckSafetyDetach();
		}

		public float GetStrengthForTerminal()
		{
			return Strength * 100f;
		}

		public float GetPowerForTerminal()
		{
			return Power * 100f;
		}

		public float GetHeightForTerminal()
		{
			return Height;
		}

		public override Vector3? GetConstraintPosition(MyCubeGrid grid, bool opposite = false)
		{
			if (m_constraint == null)
			{
				return null;
			}
			if (grid == base.CubeGrid)
			{
				if (!opposite)
				{
					return m_constraintPositionA;
				}
				return m_constraintPositionB;
			}
			if (grid == base.TopGrid)
			{
				if (!opposite)
				{
					return m_constraintPositionB;
				}
				return m_constraintPositionA;
			}
			return null;
		}

		protected override float GetConstraintDisplacementSq()
		{
			Vector3 guideVector = base.WorldMatrix.Forward;
			m_constraint.GetPivotsInWorld(out Vector3 pivotA, out Vector3 pivotB);
			Vector3 vec = pivotB - pivotA;
			Vector3 value = Vector3.ProjectOnVector(ref vec, ref guideVector);
			return (vec - value).LengthSquared();
		}

		private void AirShockLogic()
		{
			float num = CalculateCurrentAirShockRatio();
			if (num != CurrentAirShock)
			{
				if (num < CurrentAirShock)
				{
					num = Math.Max(num, CurrentAirShock - 0.05f);
				}
				CurrentAirShock = num;
				UpdateConstraintData();
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS && CurrentAirShock > 0f)
			{
				Vector3D up = base.WorldMatrix.Up;
				MyRenderProxy.DebugDrawText3D(base.WorldMatrix.Translation + up, "AirShock " + CurrentAirShock.ToString("F2"), Color.Red, 0.5f, depthRead: false);
			}
		}

		private float CalculateCurrentAirShockRatio()
		{
			if (!AirShockEnabled)
			{
				return 0f;
			}
			if (base.CubeGrid.GridSystems.WheelSystem.HandBrake)
			{
				return 0f;
			}
			if (!GetWheelAndLinearVelocity(out MyWheel wheel, out Vector3 linearVelocity))
			{
				return 0f;
			}
			MyMotorSuspensionDefinition blockDefinition = BlockDefinition;
			if (wheel.FramesSinceLastContact < (ulong)blockDefinition.AirShockActivationDelay)
			{
				return 0f;
			}
			float airShockMinSpeed = blockDefinition.AirShockMinSpeed;
			float airShockMaxSpeed = blockDefinition.AirShockMaxSpeed;
			Vector3 guideVector = MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.WorldMatrix.Translation);
			float num = Vector3.ProjectOnVector(ref linearVelocity, ref guideVector).LengthSquared();
			if (num < airShockMinSpeed * airShockMinSpeed)
			{
				return 0f;
			}
			float num2 = (float)Math.Sqrt(num) - airShockMinSpeed;
			return Math.Min(1f, num2 / (airShockMaxSpeed - airShockMinSpeed));
		}

		private void ArtificialBreakingLogic()
		{
			if (ShouldBrake && GetWheelAndLinearVelocity(out MyWheel wheel, out Vector3 linearVelocity) && !(linearVelocity.LengthSquared() < 1f) && wheel.IsConsideredInContactWithStaticSurface)
			{
				MatrixD worldMatrix = base.WorldMatrix;
				Vector3 guideVector = worldMatrix.Left;
				Vector3 value = Vector3.ProjectOnVector(ref linearVelocity, ref guideVector);
				float num = value.Length();
				Vector3 value2 = value / num;
				float shipMaxLinearVelocity = MyGridPhysics.GetShipMaxLinearVelocity(base.CubeGrid.GridSizeEnum);
				float num2 = Math.Min(1f, num / shipMaxLinearVelocity);
				if (!((double)num2 < 0.01))
				{
					Vector3 dir = base.CubeGrid.Physics.Mass * 0.5f * MyPhysicsConfig.ArtificialBrakingMultiplier * num2 * -value2;
					GetCoMVectors(out Vector3 adjustmentVector);
					Vector3 value3 = -Vector3.TransformNormal(adjustmentVector, base.CubeGrid.WorldMatrix);
					value3 *= MyPhysicsConfig.ArtificialBrakingCoMStabilization;
					Vector3D pos = worldMatrix.Translation + value3;
					base.CubeGrid.Physics.ApplyImpulse(dir, pos);
				}
			}
		}

		public bool LateralCorrectionLogicInfo(ref Vector3 groundNormal, ref Vector3 suspensionNormal)
		{
			if (GetWheelAndLinearVelocity(out MyWheel wheel, out Vector3 linearVelocity) && wheel.IsConsideredInContactWithStaticSurface && linearVelocity.LengthSquared() > 25f)
			{
				suspensionNormal += -Base6Directions.GetVector(base.Orientation.Forward);
				groundNormal += wheel.LastUsedGroundNormal;
				return true;
			}
			return false;
		}

		private bool GetWheelAndLinearVelocity(out MyWheel wheel, out Vector3 linearVelocity)
		{
			wheel = (base.Rotor as MyWheel);
			linearVelocity = Vector3.Zero;
			if (wheel == null)
			{
				return false;
			}
			MyGridPhysics physics = base.CubeGrid.Physics;
			if (physics == null)
			{
				return false;
			}
			linearVelocity = physics.LinearVelocity;
			return true;
		}

		public void OnSuspensionJumpStateUpdated()
		{
			StrengthChanged();
		}

		private void OnIsWorkingChanged(MyCubeBlock myCubeBlock)
		{
			if (!base.IsWorking)
			{
				m_steeringChanged = false;
				OnPerFrameUpdatePropertyChanged();
			}
		}

		private void OnPerFrameUpdatePropertyChanged()
		{
			NeedsPerFrameUpdate = (m_steeringChanged || PropulsionOverride != 0f || base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) > BlockDefinition.RequiredIdlePowerInput);
		}

		private void ActivatePhysics()
		{
			base.TopGrid.Physics.ActivateIfNeeded();
			base.CubeGrid.Physics.ActivateIfNeeded();
		}
	}
}
