using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_MotorStator))]
	public class MyMotorStator : MyMotorBase, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyMotorStator, Sandbox.ModAPI.Ingame.IMyMotorStator, Sandbox.ModAPI.Ingame.IMyMotorBase, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyMotorBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		protected class Torque_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType torque;
				ISyncType result = torque = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorStator)P_0).Torque = (Sync<float, SyncDirection.BothWays>)torque;
				return result;
			}
		}

		protected class BrakingTorque_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType brakingTorque;
				ISyncType result = brakingTorque = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorStator)P_0).BrakingTorque = (Sync<float, SyncDirection.BothWays>)brakingTorque;
				return result;
			}
		}

		protected class TargetVelocity_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetVelocity;
				ISyncType result = targetVelocity = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorStator)P_0).TargetVelocity = (Sync<float, SyncDirection.BothWays>)targetVelocity;
				return result;
			}
		}

		protected class m_minAngle_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType minAngle;
				ISyncType result = minAngle = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorStator)P_0).m_minAngle = (Sync<float, SyncDirection.BothWays>)minAngle;
				return result;
			}
		}

		protected class m_maxAngle_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType maxAngle;
				ISyncType result = maxAngle = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorStator)P_0).m_maxAngle = (Sync<float, SyncDirection.BothWays>)maxAngle;
				return result;
			}
		}

		protected class m_rotorLock_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType rotorLock;
				ISyncType result = rotorLock = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMotorStator)P_0).m_rotorLock = (Sync<bool, SyncDirection.BothWays>)rotorLock;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyMotorStator_003C_003EActor : IActivator, IActivator<MyMotorStator>
		{
			private sealed override object CreateInstance()
			{
				return new MyMotorStator();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMotorStator CreateInstance()
			{
				return new MyMotorStator();
			}

			MyMotorStator IActivator<MyMotorStator>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float NormalizedToRadians = MathF.PI * 2f;

		private const float DegreeToRadians = MathF.PI / 180f;

		private const float RAD361 = 6.30063868f;

		private static readonly float MIN_LOWER_LIMIT = MathF.PI * -2f - MathHelper.ToRadians(0.5f);

		private static readonly float MAX_UPPER_LIMIT = MathF.PI * 2f + MathHelper.ToRadians(0.5f);

		public readonly Sync<float, SyncDirection.BothWays> Torque;

		public readonly Sync<float, SyncDirection.BothWays> BrakingTorque;

		public readonly Sync<float, SyncDirection.BothWays> TargetVelocity;

		private readonly Sync<float, SyncDirection.BothWays> m_minAngle;

		private readonly Sync<float, SyncDirection.BothWays> m_maxAngle;

		private readonly Sync<bool, SyncDirection.BothWays> m_rotorLock;

		private HkVelocityConstraintMotor m_motor;

		private bool m_limitsActive;

		private float m_lockAngle;

		private bool m_isLockActive;

		private bool m_canLock;

		private bool m_delayedLock;

		protected bool m_canBeDetached;

		private float m_currentAngle;

		protected MyAttachableConveyorEndpoint m_conveyorEndpoint;

		private float m_currentAngleComputed;

		private bool m_resetDetailedInfo = true;

		private bool m_lastIsOpenedInTerminal;

		public bool IsLocked
		{
			get
			{
				return m_rotorLock;
			}
			set
			{
				m_rotorLock.Value = value;
			}
		}

		public float TargetVelocityRPM
		{
			get
			{
				return (float)TargetVelocity * (30f / MathF.PI);
			}
			set
			{
				TargetVelocity.Value = value * (MathF.PI / 30f);
			}
		}

		public float MinAngle
		{
			get
			{
				return (float)m_minAngle / (MathF.PI / 180f);
			}
			set
			{
				SetSafeAngles(lowerIsFixed: false, value * (MathF.PI / 180f), m_maxAngle);
			}
		}

		public float MaxAngle
		{
			get
			{
				return (float)m_maxAngle / (MathF.PI / 180f);
			}
			set
			{
				SetSafeAngles(lowerIsFixed: true, m_minAngle, value * (MathF.PI / 180f));
			}
		}

		protected override float ModelDummyDisplacement => base.MotorDefinition.RotorDisplacementInModel;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		float Sandbox.ModAPI.Ingame.IMyMotorStator.Angle => m_currentAngle;

		float Sandbox.ModAPI.Ingame.IMyMotorStator.Torque
		{
			get
			{
				return Torque;
			}
			set
			{
				float maxForceMagnitude = base.MotorDefinition.MaxForceMagnitude;
				Torque.Value = MathHelper.Clamp(value, 0f - maxForceMagnitude, maxForceMagnitude);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorStator.BrakingTorque
		{
			get
			{
				return BrakingTorque;
			}
			set
			{
				float maxForceMagnitude = base.MotorDefinition.MaxForceMagnitude;
				BrakingTorque.Value = MathHelper.Clamp(value, 0f - maxForceMagnitude, maxForceMagnitude);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorStator.TargetVelocityRad
		{
			get
			{
				return TargetVelocity;
			}
			set
			{
				float maxRotorAngularVelocity = base.MaxRotorAngularVelocity;
				TargetVelocity.Value = MathHelper.Clamp(value, 0f - maxRotorAngularVelocity, maxRotorAngularVelocity);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorStator.TargetVelocityRPM
		{
			get
			{
				return (float)TargetVelocity * (30f / MathF.PI);
			}
			set
			{
				float maxRotorAngularVelocity = base.MaxRotorAngularVelocity;
				TargetVelocity.Value = MathHelper.Clamp(value * (MathF.PI / 30f), 0f - maxRotorAngularVelocity, maxRotorAngularVelocity);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorStator.LowerLimitRad
		{
			get
			{
				if ((float)m_minAngle <= -6.30063868f)
				{
					return float.MinValue;
				}
				return m_minAngle;
			}
			set
			{
				SetSafeAngles(lowerIsFixed: false, value, m_maxAngle);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorStator.LowerLimitDeg
		{
			get
			{
				if ((float)m_minAngle <= -6.30063868f)
				{
					return float.MinValue;
				}
				return MathHelper.ToDegrees(m_minAngle);
			}
			set
			{
				SetSafeAngles(lowerIsFixed: false, MathHelper.ToRadians(value), m_maxAngle);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorStator.UpperLimitRad
		{
			get
			{
				if ((float)m_maxAngle >= 6.30063868f)
				{
					return float.MaxValue;
				}
				return m_maxAngle;
			}
			set
			{
				SetSafeAngles(lowerIsFixed: true, m_minAngle, value);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorStator.UpperLimitDeg
		{
			get
			{
				if ((float)m_maxAngle >= 6.30063868f)
				{
					return float.MaxValue;
				}
				return MathHelper.ToDegrees(m_maxAngle);
			}
			set
			{
				SetSafeAngles(lowerIsFixed: true, m_minAngle, MathHelper.ToRadians(value));
			}
		}

		float Sandbox.ModAPI.Ingame.IMyMotorStator.Displacement
		{
			get
			{
				return m_dummyDisplacement;
			}
			set
			{
				MyMotorStatorDefinition motorDefinition = base.MotorDefinition;
				m_dummyDisplacement.Value = MyMath.Clamp(value, motorDefinition.RotorDisplacementMin, motorDefinition.RotorDisplacementMax);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyMotorStator.RotorLock
		{
			get
			{
				return IsLocked;
			}
			set
			{
				IsLocked = value;
			}
		}

		private event Action<bool> LimitReached;

		event Action<bool> Sandbox.ModAPI.IMyMotorStator.LimitReached
		{
			add
			{
				LimitReached += value;
			}
			remove
			{
				LimitReached -= value;
			}
		}

		public MyMotorStator()
		{
			CreateTerminalControls();
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_canBeDetached = true;
			base.SyncType.PropertyChanged += SyncType_PropertyChanged;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyMotorStator>())
			{
				base.CreateTerminalControls();
				MyTerminalControlButton<MyMotorStator> obj = new MyTerminalControlButton<MyMotorStator>("Add Top Part", MySpaceTexts.BlockActionTitle_AddRotorHead, MySpaceTexts.BlockActionTooltip_AddRotorHead, delegate(MyMotorStator b)
				{
					b.RecreateTop();
				})
				{
					Enabled = ((MyMotorStator b) => b.TopBlock == null)
				};
				obj.EnableAction(MyTerminalActionIcons.STATION_ON);
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlButton<MyMotorStator> obj2 = new MyTerminalControlButton<MyMotorStator>("Add Small Top Part", MySpaceTexts.BlockActionTitle_AddSmallRotorHead, MySpaceTexts.BlockActionTooltip_AddSmallRotorHead, delegate(MyMotorStator b)
				{
					b.RecreateTop(null, smallToLarge: true);
				})
				{
					Enabled = ((MyMotorStator b) => b.TopBlock == null),
					Visible = ((MyMotorStator b) => b.CubeGrid.GridSizeEnum == MyCubeSize.Large)
				};
				obj2.EnableAction(MyTerminalActionIcons.STATION_ON);
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlButton<MyMotorStator> myTerminalControlButton = new MyTerminalControlButton<MyMotorStator>("Reverse", MySpaceTexts.BlockActionTitle_Reverse, MySpaceTexts.Blank, delegate(MyMotorStator b)
				{
					b.TargetVelocityRPM = 0f - b.TargetVelocityRPM;
				});
				myTerminalControlButton.EnableAction(MyTerminalActionIcons.REVERSE);
				MyTerminalControlFactory.AddControl(myTerminalControlButton);
				MyTerminalControlButton<MyMotorStator> obj3 = new MyTerminalControlButton<MyMotorStator>("Detach", MySpaceTexts.BlockActionTitle_Detach, MySpaceTexts.Blank, delegate(MyMotorStator b)
				{
					b.CallDetach();
				})
				{
					Enabled = ((MyMotorStator b) => b.m_connectionState.Value.TopBlockId.HasValue && !b.m_isWelding && !b.m_welded),
					Visible = ((MyMotorStator b) => b.m_canBeDetached)
				};
				obj3.EnableAction(MyTerminalActionIcons.NONE).Enabled = ((MyMotorStator b) => b.m_canBeDetached);
				MyTerminalControlFactory.AddControl(obj3);
				MyTerminalControlButton<MyMotorStator> obj4 = new MyTerminalControlButton<MyMotorStator>("Attach", MySpaceTexts.BlockActionTitle_Attach, MySpaceTexts.Blank, delegate(MyMotorStator b)
				{
					b.CallAttach();
				})
				{
					Enabled = ((MyMotorStator b) => !b.m_connectionState.Value.TopBlockId.HasValue),
					Visible = ((MyMotorStator b) => b.m_canBeDetached)
				};
				obj4.EnableAction(MyTerminalActionIcons.NONE).Enabled = ((MyMotorStator b) => b.m_canBeDetached);
				MyTerminalControlFactory.AddControl(obj4);
				MyTerminalControlCheckbox<MyMotorStator> obj5 = new MyTerminalControlCheckbox<MyMotorStator>("RotorLock", MySpaceTexts.BlockPropertyTitle_MotorLock, MySpaceTexts.BlockPropertyDescription_MotorLock)
				{
					Getter = ((MyMotorStator x) => x.IsLocked),
					Setter = delegate(MyMotorStator x, bool v)
					{
						x.IsLocked = v;
					}
				};
				obj5.EnableAction();
				MyTerminalControlFactory.AddControl(obj5);
				MyTerminalControlSlider<MyMotorStator> obj6 = new MyTerminalControlSlider<MyMotorStator>("Torque", MySpaceTexts.BlockPropertyTitle_MotorTorque, MySpaceTexts.BlockPropertyDescription_MotorTorque)
				{
					Getter = ((MyMotorStator x) => x.Torque),
					Setter = delegate(MyMotorStator x, float v)
					{
						x.Torque.Value = v;
					},
					DefaultValueGetter = ((MyMotorStator x) => x.MotorDefinition.MaxForceMagnitude),
					Writer = delegate(MyMotorStator x, StringBuilder result)
					{
						MyValueFormatter.AppendTorqueInBestUnit(x.Torque, result);
					},
					AdvancedWriter = delegate(MyMotorStator x, MyGuiControlBlockProperty control, StringBuilder res)
					{
						TorqueWriter(x, control, res, braking: false);
					}
				};
				obj6.EnableActions();
				obj6.Denormalizer = ((MyMotorStator x, float v) => x.DenormalizeTorque(v));
				obj6.Normalizer = ((MyMotorStator x, float v) => x.NormalizeTorque(v));
				MyTerminalControlFactory.AddControl(obj6);
				MyTerminalControlSlider<MyMotorStator> obj7 = new MyTerminalControlSlider<MyMotorStator>("BrakingTorque", MySpaceTexts.BlockPropertyTitle_MotorBrakingTorque, MySpaceTexts.BlockPropertyDescription_MotorBrakingTorque)
				{
					Getter = ((MyMotorStator x) => x.BrakingTorque),
					Setter = delegate(MyMotorStator x, float v)
					{
						x.BrakingTorque.Value = v;
					},
					DefaultValue = 0f,
					Writer = delegate(MyMotorStator x, StringBuilder result)
					{
						MyValueFormatter.AppendTorqueInBestUnit(x.BrakingTorque, result);
					},
					AdvancedWriter = delegate(MyMotorStator x, MyGuiControlBlockProperty control, StringBuilder res)
					{
						TorqueWriter(x, control, res, braking: true);
					}
				};
				obj7.EnableActions();
				obj7.Denormalizer = ((MyMotorStator x, float v) => x.DenormalizeTorque(v));
				obj7.Normalizer = ((MyMotorStator x, float v) => x.NormalizeTorque(v));
				MyTerminalControlFactory.AddControl(obj7);
				MyTerminalControlSlider<MyMotorStator> obj8 = new MyTerminalControlSlider<MyMotorStator>("Velocity", MySpaceTexts.BlockPropertyTitle_MotorTargetVelocity, MySpaceTexts.BlockPropertyDescription_MotorVelocity)
				{
					Getter = ((MyMotorStator x) => x.TargetVelocityRPM),
					Setter = delegate(MyMotorStator x, float v)
					{
						x.TargetVelocityRPM = v;
					},
					DefaultValue = 0f,
					Writer = delegate(MyMotorStator x, StringBuilder result)
					{
						result.Concat(x.TargetVelocityRPM, 2u).Append(" rpm");
					}
				};
				obj8.EnableActionsWithReset();
				obj8.Denormalizer = ((MyMotorStator x, float v) => x.DenormalizeRPM(v));
				obj8.Normalizer = ((MyMotorStator x, float v) => x.NormalizeRPM(v));
				MyTerminalControlFactory.AddControl(obj8);
				MyTerminalControlSlider<MyMotorStator> myTerminalControlSlider = new MyTerminalControlSlider<MyMotorStator>("LowerLimit", MySpaceTexts.BlockPropertyTitle_MotorMinAngle, MySpaceTexts.BlockPropertyDescription_MotorLowerLimit);
				myTerminalControlSlider.Getter = ((MyMotorStator x) => x.MinAngle);
				myTerminalControlSlider.Setter = delegate(MyMotorStator x, float v)
				{
					x.MinAngle = v;
				};
				myTerminalControlSlider.DefaultValue = -361f;
				myTerminalControlSlider.SetLimits(-361f, 360f);
				myTerminalControlSlider.Writer = delegate(MyMotorStator x, StringBuilder result)
				{
					WriteAngle(x.m_minAngle, result);
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MyMotorStator> myTerminalControlSlider2 = new MyTerminalControlSlider<MyMotorStator>("UpperLimit", MySpaceTexts.BlockPropertyTitle_MotorMaxAngle, MySpaceTexts.BlockPropertyDescription_MotorUpperLimit);
				myTerminalControlSlider2.Getter = ((MyMotorStator x) => x.MaxAngle);
				myTerminalControlSlider2.Setter = delegate(MyMotorStator x, float v)
				{
					x.MaxAngle = v;
				};
				myTerminalControlSlider2.DefaultValue = 361f;
				myTerminalControlSlider2.SetLimits(-360f, 361f);
				myTerminalControlSlider2.Writer = delegate(MyMotorStator x, StringBuilder result)
				{
					WriteAngle(x.m_maxAngle, result);
				};
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MyMotorStator> myTerminalControlSlider3 = new MyTerminalControlSlider<MyMotorStator>("Displacement", MySpaceTexts.BlockPropertyTitle_MotorRotorDisplacement, MySpaceTexts.BlockPropertyDescription_MotorRotorDisplacement);
				myTerminalControlSlider3.Getter = ((MyMotorStator x) => x.DummyDisplacement);
				myTerminalControlSlider3.Setter = delegate(MyMotorStator x, float v)
				{
					x.DummyDisplacement = v;
				};
				myTerminalControlSlider3.DefaultValueGetter = ((MyMotorStator x) => 0f);
				myTerminalControlSlider3.SetLimits((MyMotorStator x) => (x.TopGrid == null || x.TopGrid.GridSizeEnum != 0) ? x.MotorDefinition.RotorDisplacementMinSmall : x.MotorDefinition.RotorDisplacementMin, (MyMotorStator x) => (x.TopGrid == null || x.TopGrid.GridSizeEnum != 0) ? x.MotorDefinition.RotorDisplacementMaxSmall : x.MotorDefinition.RotorDisplacementMax);
				myTerminalControlSlider3.Writer = delegate(MyMotorStator x, StringBuilder result)
				{
					MyValueFormatter.AppendDistanceInBestUnit(x.DummyDisplacement, result);
				};
				myTerminalControlSlider3.Enabled = ((MyMotorStator b) => b.m_isAttached);
				myTerminalControlSlider3.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
			}
		}

		private static void TorqueWriter(MyMotorStator block, MyGuiControlBlockProperty control, StringBuilder output, bool braking)
		{
			Vector4 colorMask = control.ColorMask;
			float num = braking ? block.BrakingTorque.Value : block.Torque.Value;
			if (num > block.MotorDefinition.UnsafeTorqueThreshold)
			{
				colorMask = Color.Red.ToVector4();
			}
			MyValueFormatter.AppendTorqueInBestUnit(num, output);
			control.TitleLabel.ColorMask = colorMask;
			control.ExtraInfoLabel.ColorMask = colorMask;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_MotorStator myObjectBuilder_MotorStator = (MyObjectBuilder_MotorStator)objectBuilder;
			if (!myObjectBuilder_MotorStator.Torque.HasValue)
			{
				float max = (base.CubeGrid.GridSizeEnum == MyCubeSize.Large) ? 3.36E+07f : 448000f;
				myObjectBuilder_MotorStator.Torque = MathHelper.Clamp(DenormalizeTorque(myObjectBuilder_MotorStator.Force), 0f, max);
				myObjectBuilder_MotorStator.BrakingTorque = MathHelper.Clamp(DenormalizeTorque(myObjectBuilder_MotorStator.Friction), 0f, max);
			}
			IsLocked = (myObjectBuilder_MotorStator.RotorLock || myObjectBuilder_MotorStator.ForceWeld);
			Torque.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorStator.Torque.Value, 0f, base.MotorDefinition.MaxForceMagnitude));
			BrakingTorque.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorStator.BrakingTorque.Value, 0f, base.MotorDefinition.MaxForceMagnitude));
			TargetVelocity.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorStator.TargetVelocity * base.MaxRotorAngularVelocity, 0f - base.MaxRotorAngularVelocity, base.MaxRotorAngularVelocity));
			m_weldSpeed.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MotorStator.WeldSpeed, 0f, MyGridPhysics.SmallShipMaxLinearVelocity()));
			m_limitsActive = myObjectBuilder_MotorStator.LimitsActive;
			m_currentAngle = myObjectBuilder_MotorStator.CurrentAngle;
			if (Sync.IsServer)
			{
				SetSafeAngles(lowerIsFixed: true, myObjectBuilder_MotorStator.MinAngle ?? float.NegativeInfinity, myObjectBuilder_MotorStator.MaxAngle ?? float.PositiveInfinity);
			}
			else
			{
				m_minAngle.SetLocalValue(myObjectBuilder_MotorStator.MinAngle ?? float.NegativeInfinity);
				m_maxAngle.SetLocalValue(myObjectBuilder_MotorStator.MaxAngle ?? float.PositiveInfinity);
			}
			m_dummyDisplacement.SetLocalValue(myObjectBuilder_MotorStator.DummyDisplacement);
			AddDebugRenderComponent(new MyDebugRenderComponentMotorStator(this));
			m_canLock = false;
			m_delayedLock = IsLocked;
			if (m_delayedLock)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_MotorStator obj = (MyObjectBuilder_MotorStator)base.GetObjectBuilderCubeBlock(copy);
			obj.Torque = Torque;
			obj.BrakingTorque = BrakingTorque;
			obj.TargetVelocity = (float)TargetVelocity / base.MaxRotorAngularVelocity;
			obj.MinAngle = (float.IsNegativeInfinity(m_minAngle) ? null : new float?(m_minAngle));
			obj.MaxAngle = (float.IsPositiveInfinity(m_maxAngle) ? null : new float?(m_maxAngle));
			obj.CurrentAngle = ((base.SafeConstraint != null) ? HkLimitedHingeConstraintData.GetCurrentAngle(base.SafeConstraint) : m_currentAngle);
			obj.LimitsActive = m_limitsActive;
			obj.DummyDisplacement = m_dummyDisplacement;
			obj.ForceWeld = false;
			obj.RotorLock = IsLocked;
			return obj;
		}

		private void SyncType_PropertyChanged(SyncBase obj)
		{
			if (obj == m_dummyDisplacement && MyPhysicsBody.IsConstraintValid(m_constraint))
			{
				SetConstraintPosition(base.TopBlock, (HkLimitedHingeConstraintData)m_constraint.ConstraintData);
			}
			if (obj == BrakingTorque || obj == Torque)
			{
				OnUnsafeSettingsChanged();
			}
		}

		private float NormalizeRPM(float v)
		{
			return v / (base.MaxRotorAngularVelocity * (30f / MathF.PI)) / 2f + 0.5f;
		}

		private float DenormalizeRPM(float v)
		{
			return (v - 0.5f) * 2f * (base.MaxRotorAngularVelocity * (30f / MathF.PI));
		}

		public static void WriteAngle(float angleRad, StringBuilder result)
		{
			if (float.IsInfinity(angleRad))
			{
				result.Append((object)MyTexts.Get(MySpaceTexts.BlockPropertyValue_MotorAngleUnlimited));
			}
			else
			{
				result.Concat(MathHelper.ToDegrees(angleRad), 0u).Append("°");
			}
		}

		private float NormalizeTorque(float value)
		{
			if (value == 0f)
			{
				return 0f;
			}
			bool flag = false;
			flag = ((!Sync.IsServer) ? MyMultiplayer.Static.IsServerExperimental : MySandboxGame.Config.ExperimentalMode);
			return MathHelper.InterpLogInv(value, 1f, flag ? base.MotorDefinition.MaxForceMagnitude : base.MotorDefinition.UnsafeTorqueThreshold);
		}

		private float DenormalizeTorque(float value)
		{
			if (value == 0f)
			{
				return 0f;
			}
			bool flag = false;
			flag = ((!Sync.IsServer) ? MyMultiplayer.Static.IsServerExperimental : MySandboxGame.Config.ExperimentalMode);
			return MathHelper.InterpLog(value, 1f, flag ? base.MotorDefinition.MaxForceMagnitude : base.MotorDefinition.UnsafeTorqueThreshold);
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			if (!base.IsOpenedInTerminal && !m_resetDetailedInfo)
			{
				return;
			}
			float num = 0f;
			num = ((!(base.SafeConstraint != null) || base.SafeConstraint.Enabled) ? m_currentAngle : m_currentAngleComputed);
			detailedInfo.AppendStringBuilder(MyTexts.Get(GetAttachState())).AppendLine();
			if (base.SafeConstraint != null)
			{
				float num2 = m_limitsActive ? MyMath.Clamp(num, m_minAngle, m_maxAngle) : num;
				num2 = MoveDown(MoveUp(num, MathF.PI * -2f, MathF.PI * 2f), MathF.PI * 2f, MathF.PI * 2f);
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MotorCurrentAngle)).AppendDecimal(MathHelper.ToDegrees(num2), 0).Append("°");
				if (IsLocked)
				{
					if (!m_isLockActive)
					{
						detailedInfo.AppendLine();
						detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MotoLockOverrideDisabled));
					}
				}
				else if (!m_limitsActive && (!float.IsNegativeInfinity(m_minAngle) || !float.IsPositiveInfinity(m_maxAngle)))
				{
					detailedInfo.AppendLine();
					detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MotorLimitsDisabled));
				}
			}
			RaisePropertiesChanged();
		}

		private void ScaleDown(float limit)
		{
			while (m_currentAngle > limit)
			{
				m_currentAngle -= MathF.PI * 2f;
			}
			SetAngleToPhysics();
		}

		private void ScaleUp(float limit)
		{
			while (m_currentAngle < limit)
			{
				m_currentAngle += MathF.PI * 2f;
			}
			SetAngleToPhysics();
		}

		private void SetAngleToPhysics()
		{
			if (base.SafeConstraint != null)
			{
				HkLimitedHingeConstraintData.SetCurrentAngle(base.SafeConstraint, m_currentAngle);
			}
		}

		private void SetSafeAngles(bool lowerIsFixed, float lowerLimit, float upperLimit)
		{
			lowerLimit = MathHelper.Clamp(lowerLimit, -6.30063868f, MathF.PI * 2f);
			upperLimit = MathHelper.Clamp(upperLimit, MathF.PI * -2f, 6.30063868f);
			if (m_currentAngle < lowerLimit)
			{
				ScaleUp(MIN_LOWER_LIMIT);
			}
			if (m_currentAngle > upperLimit)
			{
				ScaleDown(MAX_UPPER_LIMIT);
			}
			if (upperLimit < lowerLimit)
			{
				if (lowerIsFixed)
				{
					upperLimit = lowerLimit;
				}
				else
				{
					lowerLimit = upperLimit;
				}
			}
			if (lowerLimit < MIN_LOWER_LIMIT)
			{
				lowerLimit = float.NegativeInfinity;
			}
			if (upperLimit > MAX_UPPER_LIMIT)
			{
				upperLimit = float.PositiveInfinity;
			}
			m_minAngle.Value = lowerLimit;
			m_maxAngle.Value = upperLimit;
			m_limitsActive = false;
			TryActivateLimits();
			if (base.SafeConstraint != null)
			{
				m_currentAngle = HkLimitedHingeConstraintData.GetCurrentAngle(base.SafeConstraint);
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private float MoveUp(float numberToMove, float minimum, float moveByMultipleOf)
		{
			while (numberToMove < minimum)
			{
				numberToMove += moveByMultipleOf;
			}
			return numberToMove;
		}

		private float MoveDown(float numberToMove, float maximum, float moveByMultipleOf)
		{
			while (numberToMove > maximum)
			{
				numberToMove -= moveByMultipleOf;
			}
			return numberToMove;
		}

		private void TryActivateLimits(bool allowLock = false)
		{
			if (IsLocked)
			{
				if (!m_isLockActive && allowLock)
				{
					m_currentAngle = MoveUp(m_currentAngle, 0f, MathF.PI * 2f);
					m_currentAngle = MoveDown(m_currentAngle, MathF.PI * 2f, MathF.PI * 2f);
					SetAngleToPhysics();
					m_isLockActive = true;
					m_limitsActive = false;
					m_lockAngle = m_currentAngle;
				}
				return;
			}
			m_isLockActive = false;
			if (float.IsNegativeInfinity(m_minAngle) && float.IsPositiveInfinity(m_maxAngle))
			{
				m_currentAngle = MoveUp(m_currentAngle, 0f, MathF.PI * 2f);
				m_currentAngle = MoveDown(m_currentAngle, MathF.PI * 2f, MathF.PI * 2f);
				SetAngleToPhysics();
				m_limitsActive = false;
			}
			else if (!m_limitsActive)
			{
				float num = (float)m_minAngle - MathHelper.ToRadians(5f);
				float num2 = (float)m_maxAngle + MathHelper.ToRadians(5f);
				float num3 = m_currentAngle;
				if (num3 < num)
				{
					num3 = MoveUp(num3, num, MathF.PI * 2f);
				}
				else if (num3 > num2)
				{
					num3 = MoveDown(num3, num2, MathF.PI * 2f);
				}
				if (num3 >= num && num3 <= num2)
				{
					m_limitsActive = true;
					m_currentAngle = num3;
					SetAngleToPhysics();
				}
			}
		}

		private float GetAngle(Quaternion q, Vector3 axis)
		{
			float num = 2f * (float)Math.Atan2(new Vector3(q.X, q.Y, q.Z).Length(), q.W);
			Vector3 vector = new Vector3(q.X, q.Y, q.Z) / (float)Math.Sin(num / 2f);
			vector = ((num == 0f) ? Vector3.Zero : vector);
			return num * Vector3.Dot(vector, axis);
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (m_delayedLock)
			{
				m_canLock = true;
				m_delayedLock = false;
				m_resetDetailedInfo = true;
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!m_lastIsOpenedInTerminal && base.IsOpenedInTerminal)
			{
				m_resetDetailedInfo = true;
			}
			m_lastIsOpenedInTerminal = base.IsOpenedInTerminal;
			if (base.m_welded)
			{
				return;
			}
			HkConstraint safeConstraint = base.SafeConstraint;
			if (base.TopGrid == null || safeConstraint == null)
			{
				return;
			}
			if (safeConstraint.RigidBodyA == safeConstraint.RigidBodyB)
			{
				base.SafeConstraint.Enabled = false;
				return;
			}
			float num = 0f;
			bool flag = false;
			if (safeConstraint.Enabled)
			{
				num = m_currentAngle;
				m_currentAngle = HkLimitedHingeConstraintData.GetCurrentAngle(safeConstraint);
				flag = m_canLock;
				m_canLock = true;
				bool flag2 = !num.IsEqual(m_currentAngle);
				if (flag2 || !safeConstraint.Enabled || m_resetDetailedInfo)
				{
					SetDetailedInfoDirty();
				}
				if (flag && flag2)
				{
					flag = (0.17453292f > Math.Abs(num - m_currentAngle) / 0.0166666675f);
					MyGridPhysicalGroupData.InvalidateSharedMassPropertiesCache(base.CubeGrid);
				}
			}
			else if (NeedsComputedAngle() || m_resetDetailedInfo)
			{
				num = m_currentAngleComputed;
				m_currentAngleComputed = ComputeCurrentAngle();
				flag = m_canLock;
				m_canLock = true;
				bool flag3 = !num.IsEqual(m_currentAngleComputed);
				SetDetailedInfoDirty();
				if (flag && flag3)
				{
					flag = (0.08726646f > Math.Abs(num - m_currentAngleComputed) / 0.0166666675f);
				}
			}
			m_resetDetailedInfo = false;
			HkLimitedHingeConstraintData hkLimitedHingeConstraintData = (HkLimitedHingeConstraintData)safeConstraint.ConstraintData;
			hkLimitedHingeConstraintData.MaxFrictionTorque = BrakingTorque;
			TryActivateLimits(flag);
			if (!m_limitsActive && !m_isLockActive)
			{
				hkLimitedHingeConstraintData.DisableLimits();
			}
			else
			{
				float num2 = m_minAngle.Value;
				float num3 = m_maxAngle.Value;
				if (m_isLockActive)
				{
					num2 = m_lockAngle;
					num3 = m_lockAngle;
				}
				if (!hkLimitedHingeConstraintData.MinAngularLimit.IsEqual(num2) || !hkLimitedHingeConstraintData.MaxAngularLimit.IsEqual(num3))
				{
					hkLimitedHingeConstraintData.MinAngularLimit = num2;
					hkLimitedHingeConstraintData.MaxAngularLimit = num3;
					base.CubeGrid.Physics.RigidBody.Activate();
					base.TopGrid.Physics.RigidBody.Activate();
				}
			}
			bool flag4 = m_limitsActive;
			if (m_limitsActive)
			{
				flag4 = ((m_motor.VelocityTarget != 0f && m_motor.VelocityTarget > 0f) ? (num > (float)m_maxAngle - 0.0001f) : (num < (float)m_minAngle + 0.0001f));
				Action<bool> limitReached = this.LimitReached;
				if (limitReached != null)
				{
					if (num > hkLimitedHingeConstraintData.MinAngularLimit && m_currentAngle <= hkLimitedHingeConstraintData.MinAngularLimit)
					{
						limitReached(obj: false);
					}
					if (num < hkLimitedHingeConstraintData.MaxAngularLimit && m_currentAngle >= hkLimitedHingeConstraintData.MaxAngularLimit)
					{
						limitReached(obj: true);
					}
				}
			}
			if (m_limitsActive || m_isLockActive)
			{
				float num4 = MathF.PI * 2f + (float.IsPositiveInfinity(m_maxAngle) ? (MathF.PI * 2f) : 0f);
				if (m_currentAngle > num4 + 28.64789f)
				{
					ScaleDown(num4);
				}
				float num5 = MathF.PI * -2f - (float.IsNegativeInfinity(m_minAngle) ? (MathF.PI * 2f) : 0f);
				if (m_currentAngle < num5 - 28.64789f)
				{
					ScaleUp(num5);
				}
			}
			float num6 = Math.Min(Torque, (base.TopGrid.Physics.Mass > 0f) ? (base.TopGrid.Physics.Mass * base.TopGrid.Physics.Mass) : ((float)Torque));
			m_motor.MaxForce = num6;
			m_motor.MinForce = 0f - num6;
			m_motor.VelocityTarget = TargetVelocity;
			bool isWorking = base.IsWorking;
			if (hkLimitedHingeConstraintData.MotorEnabled != isWorking)
			{
				hkLimitedHingeConstraintData.SetMotorEnabled(m_constraint, isWorking);
			}
			if (isWorking && base.TopGrid != null && !m_motor.VelocityTarget.IsZero() && !flag4)
			{
				base.CubeGrid.Physics.RigidBody.Activate();
				base.TopGrid.Physics.RigidBody.Activate();
			}
		}

		private bool NeedsComputedAngle()
		{
			return base.IsOpenedInTerminal;
		}

		private float ComputeCurrentAngle()
		{
			float num = 0f;
			MatrixD worldMatrix = base.PositionComp.WorldMatrix;
			MatrixD worldMatrix2 = base.TopBlock.PositionComp.WorldMatrix;
			double num2 = Vector3D.Dot(worldMatrix2.Right, worldMatrix.Right);
			double num3 = Vector3D.Dot(worldMatrix2.Right, worldMatrix.Backward);
			float num4 = (float)Math.Acos(MathHelper.Clamp((float)num2, -1f, 1f));
			if (num3 >= 0.0)
			{
				return num4;
			}
			return MathF.PI * 2f - num4;
		}

		private void SetConstraintPosition(MyAttachableTopBlockBase rotor, HkLimitedHingeConstraintData data)
		{
			Vector3 dummyPosition = base.DummyPosition;
			Vector3 posB = rotor.Position * rotor.CubeGrid.GridSize;
			Vector3 up = base.PositionComp.LocalMatrix.Up;
			Vector3 forward = base.PositionComp.LocalMatrix.Forward;
			Vector3 up2 = rotor.PositionComp.LocalMatrix.Up;
			Vector3 forward2 = rotor.PositionComp.LocalMatrix.Forward;
			data.SetInBodySpace(dummyPosition, posB, up, up2, forward, forward2, base.CubeGrid.Physics, base.TopGrid.Physics);
		}

		protected override bool Attach(MyAttachableTopBlockBase rotor, bool updateGroup = true)
		{
			if (rotor is MyMotorRotor && base.Attach(rotor, updateGroup))
			{
				CheckDisplacementLimits();
				CreateConstraint(rotor);
				SetDetailedInfoDirty();
				return true;
			}
			return false;
		}

		protected override bool CreateConstraint(MyAttachableTopBlockBase rotor)
		{
			if (!base.CreateConstraint(rotor))
			{
				return false;
			}
			m_resetDetailedInfo = true;
			HkRigidBody rigidBody = base.TopBlock.CubeGrid.Physics.RigidBody;
			HkLimitedHingeConstraintData hkLimitedHingeConstraintData = new HkLimitedHingeConstraintData();
			m_motor = new HkVelocityConstraintMotor(1f, 1000000f);
			hkLimitedHingeConstraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
			hkLimitedHingeConstraintData.Motor = m_motor;
			hkLimitedHingeConstraintData.DisableLimits();
			SetConstraintPosition(rotor, hkLimitedHingeConstraintData);
			m_constraint = new HkConstraint(base.CubeGrid.Physics.RigidBody, rigidBody, hkLimitedHingeConstraintData);
			m_constraint.WantRuntime = true;
			base.CubeGrid.Physics.AddConstraint(m_constraint);
			if (!m_constraint.InWorld)
			{
				base.CubeGrid.Physics.RemoveConstraint(m_constraint);
				m_constraint.Dispose();
				m_constraint = null;
				return false;
			}
			m_constraint.Enabled = true;
			SetAngleToPhysics();
			if (base.CubeGrid.Physics != null)
			{
				base.CubeGrid.Physics.ForceActivate();
			}
			if (base.TopGrid.Physics != null)
			{
				base.TopGrid.Physics.ForceActivate();
			}
			HkConstraint constraint = m_constraint;
			constraint.OnAddedToWorldCallback = (Action)Delegate.Combine(constraint.OnAddedToWorldCallback, (Action)delegate
			{
				SetAngleToPhysics();
			});
			return true;
		}

		protected override void DisposeConstraint(MyCubeGrid topGrid)
		{
			base.DisposeConstraint(topGrid);
			if (m_motor != null)
			{
				m_motor.Dispose();
				m_motor = null;
			}
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyAttachableConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
		}

		public bool CanDebugDraw()
		{
			if (base.TopGrid != null)
			{
				return base.TopGrid.Physics != null;
			}
			return false;
		}

		public override Vector3? GetConstraintPosition(MyCubeGrid grid, bool opposite = false)
		{
			if (m_constraint == null)
			{
				return null;
			}
			HkLimitedHingeConstraintData hkLimitedHingeConstraintData = m_constraint.ConstraintData as HkLimitedHingeConstraintData;
			if (hkLimitedHingeConstraintData == null)
			{
				return null;
			}
			if (grid == base.CubeGrid)
			{
				return opposite ? hkLimitedHingeConstraintData.BodyBPos : hkLimitedHingeConstraintData.BodyAPos;
			}
			if (grid == base.TopGrid)
			{
				return opposite ? hkLimitedHingeConstraintData.BodyAPos : hkLimitedHingeConstraintData.BodyBPos;
			}
			return null;
		}

		protected override bool HasUnsafeSettingsCollector()
		{
			float unsafeTorqueThreshold = base.MotorDefinition.UnsafeTorqueThreshold;
			if (!((float)Torque > unsafeTorqueThreshold) && !((float)BrakingTorque > unsafeTorqueThreshold))
			{
				return base.HasUnsafeSettingsCollector();
			}
			return true;
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
