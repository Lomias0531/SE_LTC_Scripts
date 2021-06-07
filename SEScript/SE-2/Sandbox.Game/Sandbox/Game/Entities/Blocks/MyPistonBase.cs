using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_PistonBase))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyPistonBase),
		typeof(Sandbox.ModAPI.Ingame.IMyPistonBase)
	})]
	public class MyPistonBase : MyMechanicalConnectionBlockBase, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyPistonBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyPistonBase
	{
		private class PistonSubpartPhysicsComponent : MyPhysicsBody
		{
			private class Sandbox_Game_Entities_Blocks_MyPistonBase_003C_003EPistonSubpartPhysicsComponent_003C_003EActor
			{
			}

			private MyPistonBase m_piston;

			public PistonSubpartPhysicsComponent(MyPistonBase piston, VRage.ModAPI.IMyEntity entity, RigidBodyFlag flags)
				: base(entity, flags)
			{
				m_piston = piston;
			}

			public override void OnWorldPositionChanged(object source)
			{
				if (source == null || source != m_piston.CubeGrid.Physics)
				{
					base.OnWorldPositionChanged(source);
				}
				else
				{
					GetRigidBodyMatrix(out m_bodyMatrix);
				}
			}
		}

		protected class Velocity_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType velocity;
				ISyncType result = velocity = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyPistonBase)P_0).Velocity = (Sync<float, SyncDirection.BothWays>)velocity;
				return result;
			}
		}

		protected class MinLimit_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType minLimit;
				ISyncType result = minLimit = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyPistonBase)P_0).MinLimit = (Sync<float, SyncDirection.BothWays>)minLimit;
				return result;
			}
		}

		protected class MaxLimit_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType maxLimit;
				ISyncType result = maxLimit = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyPistonBase)P_0).MaxLimit = (Sync<float, SyncDirection.BothWays>)maxLimit;
				return result;
			}
		}

		protected class MaxImpulseAxis_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType maxImpulseAxis;
				ISyncType result = maxImpulseAxis = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyPistonBase)P_0).MaxImpulseAxis = (Sync<float, SyncDirection.BothWays>)maxImpulseAxis;
				return result;
			}
		}

		protected class MaxImpulseNonAxis_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType maxImpulseNonAxis;
				ISyncType result = maxImpulseNonAxis = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyPistonBase)P_0).MaxImpulseNonAxis = (Sync<float, SyncDirection.BothWays>)maxImpulseNonAxis;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyPistonBase_003C_003EActor : IActivator, IActivator<MyPistonBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyPistonBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPistonBase CreateInstance()
			{
				return new MyPistonBase();
			}

			MyPistonBase IActivator<MyPistonBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private HkConstraint m_subpartsConstraint;

		private MyPhysicsBody m_subpartPhysics;

		private MyEntitySubpart m_subpart1;

		private MyEntitySubpart m_subpart2;

		public MyEntitySubpart Subpart3;

		private Vector3 m_subpart1LocPos;

		private Vector3 m_subpart2LocPos;

		private Vector3 m_subpart3LocPos;

		private Vector3 m_constraintBasePos;

		private HkFixedConstraintData m_fixedData;

		private HkFixedConstraintData m_subpartsFixedData;

		private bool m_subPartContraintInScene;

		private MyAttachableConveyorEndpoint m_conveyorEndpoint;

		private bool m_posChanged;

		private Vector3 m_subpartsConstraintPos;

		private float m_lastPosition = float.MaxValue;

		private float m_currentPos;

		private float m_lastVelocity;

		private int m_ignoreNonAxialForcesForNMoreFrames;

		private const int IGNORE_NONAXIAL_FORCES_AFTER_VELOCITY_CHANGE_FOR_N_FRAMES = 5;

		public readonly Sync<float, SyncDirection.BothWays> Velocity;

		public readonly Sync<float, SyncDirection.BothWays> MinLimit;

		public readonly Sync<float, SyncDirection.BothWays> MaxLimit;

		public readonly Sync<float, SyncDirection.BothWays> MaxImpulseAxis;

		public readonly Sync<float, SyncDirection.BothWays> MaxImpulseNonAxis;

		private float Range => BlockDefinition.Maximum - BlockDefinition.Minimum;

		public new MyPistonBaseDefinition BlockDefinition => (MyPistonBaseDefinition)base.BlockDefinition;

		public float CurrentPosition => m_currentPos;

		public PistonStatus Status
		{
			get
			{
				if ((float)Velocity < 0f)
				{
					if (!(m_currentPos <= (float)MinLimit))
					{
						return PistonStatus.Retracting;
					}
					return PistonStatus.Retracted;
				}
				if ((float)Velocity > 0f)
				{
					if (!(m_currentPos >= (float)MaxLimit))
					{
						return PistonStatus.Extending;
					}
					return PistonStatus.Extended;
				}
				return PistonStatus.Stopped;
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public float BreakOffTreshold => (base.CubeGrid.GridSizeEnum == MyCubeSize.Large) ? 20000000 : 1000000;

		float Sandbox.ModAPI.Ingame.IMyPistonBase.Velocity
		{
			get
			{
				return Velocity;
			}
			set
			{
				value = MathHelper.Clamp(value, 0f - BlockDefinition.MaxVelocity, BlockDefinition.MaxVelocity);
				Velocity.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyPistonBase.MaxVelocity => BlockDefinition.MaxVelocity;

		float Sandbox.ModAPI.Ingame.IMyPistonBase.MinLimit
		{
			get
			{
				return MinLimit;
			}
			set
			{
				value = MathHelper.Clamp(value, BlockDefinition.Minimum, BlockDefinition.Maximum);
				MinLimit.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyPistonBase.MaxLimit
		{
			get
			{
				return MaxLimit;
			}
			set
			{
				value = MathHelper.Clamp(value, BlockDefinition.Minimum, BlockDefinition.Maximum);
				MaxLimit.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyPistonBase.LowestPosition => BlockDefinition.Minimum;

		float Sandbox.ModAPI.Ingame.IMyPistonBase.HighestPosition => BlockDefinition.Maximum;

		private event Action<bool> LimitReached;

		event Action<bool> Sandbox.ModAPI.IMyPistonBase.LimitReached
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

		event Action<Sandbox.ModAPI.IMyPistonBase> Sandbox.ModAPI.IMyPistonBase.AttachedEntityChanged
		{
			add
			{
				base.AttachedEntityChanged += GetDelegate(value);
			}
			remove
			{
				base.AttachedEntityChanged -= GetDelegate(value);
			}
		}

		public MyPistonBase()
		{
			CreateTerminalControls();
			Velocity.ValueChanged += delegate
			{
				UpdatePhysicsShape();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyPistonBase>())
			{
				base.CreateTerminalControls();
				MyTerminalControlButton<MyPistonBase> obj = new MyTerminalControlButton<MyPistonBase>("Add Top Part", MySpaceTexts.BlockActionTitle_AddPistonHead, MySpaceTexts.BlockActionTooltip_AddPistonHead, delegate(MyPistonBase b)
				{
					b.RecreateTop();
				})
				{
					Enabled = ((MyPistonBase b) => b.TopBlock == null)
				};
				obj.EnableAction(MyTerminalActionIcons.STATION_ON);
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlButton<MyPistonBase> obj2 = new MyTerminalControlButton<MyPistonBase>("Reverse", MySpaceTexts.BlockActionTitle_Reverse, MySpaceTexts.Blank, delegate(MyPistonBase x)
				{
					x.Velocity.Value = 0f - (float)x.Velocity;
				})
				{
					Enabled = ((MyPistonBase b) => b.IsFunctional)
				};
				obj2.EnableAction(MyTerminalActionIcons.REVERSE);
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyPistonBase>("Extend", MyTexts.Get(MySpaceTexts.BlockActionTitle_Extend), OnExtendApplied, null, MyTerminalActionIcons.REVERSE)
				{
					Enabled = ((MyPistonBase b) => b.IsFunctional)
				});
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyPistonBase>("Retract", MyTexts.Get(MySpaceTexts.BlockActionTitle_Retract), OnRetractApplied, null, MyTerminalActionIcons.REVERSE)
				{
					Enabled = ((MyPistonBase b) => b.IsFunctional)
				});
				MyTerminalControlSlider<MyPistonBase> myTerminalControlSlider = new MyTerminalControlSlider<MyPistonBase>("Velocity", MySpaceTexts.BlockPropertyTitle_Velocity, MySpaceTexts.Blank);
				myTerminalControlSlider.SetLimits((MyPistonBase block) => 0f - block.BlockDefinition.MaxVelocity, (MyPistonBase block) => block.BlockDefinition.MaxVelocity);
				myTerminalControlSlider.DefaultValue = -0.5f;
				myTerminalControlSlider.Getter = ((MyPistonBase x) => x.Velocity);
				myTerminalControlSlider.Setter = delegate(MyPistonBase x, float v)
				{
					x.Velocity.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyPistonBase x, StringBuilder res)
				{
					res.AppendDecimal(x.Velocity, 1).Append("m/s");
				};
				myTerminalControlSlider.EnableActionsWithReset();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MyPistonBase> myTerminalControlSlider2 = new MyTerminalControlSlider<MyPistonBase>("UpperLimit", MySpaceTexts.BlockPropertyTitle_MaximalDistance, MySpaceTexts.Blank);
				myTerminalControlSlider2.SetLimits((MyPistonBase block) => block.BlockDefinition.Minimum, (MyPistonBase block) => block.BlockDefinition.Maximum);
				myTerminalControlSlider2.DefaultValueGetter = ((MyPistonBase block) => block.BlockDefinition.Maximum);
				myTerminalControlSlider2.Getter = ((MyPistonBase x) => x.MaxLimit);
				myTerminalControlSlider2.Setter = delegate(MyPistonBase x, float v)
				{
					x.MaxLimit.Value = v;
				};
				myTerminalControlSlider2.Writer = delegate(MyPistonBase x, StringBuilder res)
				{
					res.AppendDecimal(x.MaxLimit, 1).Append("m");
				};
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MyPistonBase> myTerminalControlSlider3 = new MyTerminalControlSlider<MyPistonBase>("LowerLimit", MySpaceTexts.BlockPropertyTitle_MinimalDistance, MySpaceTexts.Blank);
				myTerminalControlSlider3.SetLimits((MyPistonBase block) => block.BlockDefinition.Minimum, (MyPistonBase block) => block.BlockDefinition.Maximum);
				myTerminalControlSlider3.DefaultValueGetter = ((MyPistonBase block) => block.BlockDefinition.Minimum);
				myTerminalControlSlider3.Getter = ((MyPistonBase x) => x.MinLimit);
				myTerminalControlSlider3.Setter = delegate(MyPistonBase x, float v)
				{
					x.MinLimit.Value = v;
				};
				myTerminalControlSlider3.Writer = delegate(MyPistonBase x, StringBuilder res)
				{
					res.AppendDecimal(x.MinLimit, 1).Append("m");
				};
				myTerminalControlSlider3.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				MyTerminalControlSlider<MyPistonBase> myTerminalControlSlider4 = new MyTerminalControlSlider<MyPistonBase>("MaxImpulseAxis", MySpaceTexts.BlockPropertyTitle_MaxImpulseAxis, MySpaceTexts.BlockPropertyTooltip_MaxImpulseAxis);
				myTerminalControlSlider4.SetLogLimits((MyPistonBase x) => 100f, delegate(MyPistonBase x)
				{
					bool flag2 = false;
					return (!((!Sync.IsServer) ? MyMultiplayer.Static.IsServerExperimental : MySandboxGame.Config.ExperimentalMode)) ? x.BlockDefinition.UnsafeImpulseThreshold : float.MaxValue;
				});
				myTerminalControlSlider4.DefaultValueGetter = ((MyPistonBase block) => block.BlockDefinition.DefaultMaxImpulseAxis);
				myTerminalControlSlider4.Getter = ((MyPistonBase x) => x.MaxImpulseAxis);
				myTerminalControlSlider4.Setter = delegate(MyPistonBase x, float v)
				{
					x.MaxImpulseAxis.Value = v;
				};
				myTerminalControlSlider4.Writer = delegate(MyPistonBase x, StringBuilder res)
				{
					WriteImpulse(res, x.MaxImpulseAxis);
				};
				myTerminalControlSlider4.AdvancedWriter = delegate(MyPistonBase x, MyGuiControlBlockProperty control, StringBuilder res)
				{
					ImpulseWriter(x, control, res, axial: true);
				};
				myTerminalControlSlider4.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
				MyTerminalControlSlider<MyPistonBase> myTerminalControlSlider5 = new MyTerminalControlSlider<MyPistonBase>("MaxImpulseNonAxis", MySpaceTexts.BlockPropertyTitle_MaxImpulseNonAxis, MySpaceTexts.BlockPropertyTooltip_MaxImpulseNonAxis);
				myTerminalControlSlider5.SetLogLimits((MyPistonBase x) => 100f, delegate(MyPistonBase x)
				{
					bool flag = false;
					return (!((!Sync.IsServer) ? MyMultiplayer.Static.IsServerExperimental : MySandboxGame.Config.ExperimentalMode)) ? x.BlockDefinition.UnsafeImpulseThreshold : float.MaxValue;
				});
				myTerminalControlSlider5.DefaultValueGetter = ((MyPistonBase block) => block.BlockDefinition.DefaultMaxImpulseNonAxis);
				myTerminalControlSlider5.Getter = ((MyPistonBase x) => x.MaxImpulseNonAxis);
				myTerminalControlSlider5.Setter = delegate(MyPistonBase x, float v)
				{
					x.MaxImpulseNonAxis.Value = v;
				};
				myTerminalControlSlider5.Writer = delegate(MyPistonBase x, StringBuilder res)
				{
					WriteImpulse(res, x.MaxImpulseNonAxis);
				};
				myTerminalControlSlider5.AdvancedWriter = delegate(MyPistonBase x, MyGuiControlBlockProperty control, StringBuilder res)
				{
					ImpulseWriter(x, control, res, axial: false);
				};
				myTerminalControlSlider5.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider5);
			}
		}

		private static void ImpulseWriter(MyPistonBase block, MyGuiControlBlockProperty control, StringBuilder output, bool axial)
		{
			Vector4 colorMask = control.ColorMask;
			float num = axial ? block.MaxImpulseAxis.Value : block.MaxImpulseNonAxis.Value;
			if (num > block.BlockDefinition.UnsafeImpulseThreshold)
			{
				colorMask = Color.Red.ToVector4();
			}
			WriteImpulse(output, num);
			control.TitleLabel.ColorMask = colorMask;
			control.ExtraInfoLabel.ColorMask = colorMask;
		}

		private static void WriteImpulse(StringBuilder sb, float impulse)
		{
			if (impulse < 1E+30f)
			{
				MyValueFormatter.AppendForceInBestUnit(impulse, sb);
				return;
			}
			impulse /= 1E+30f;
			int num = 30;
			while (impulse > 1000f)
			{
				num++;
				impulse /= 10f;
			}
			sb.AppendDecimal(impulse, 0).Append('E').Append(num)
				.Append(" N");
		}

		private static void OnExtendApplied(MyPistonBase piston)
		{
			((Sandbox.ModAPI.Ingame.IMyPistonBase)piston).Extend();
		}

		private static void OnRetractApplied(MyPistonBase piston)
		{
			((Sandbox.ModAPI.Ingame.IMyPistonBase)piston).Retract();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink.Update();
			MyObjectBuilder_PistonBase myObjectBuilder_PistonBase = objectBuilder as MyObjectBuilder_PistonBase;
			Velocity.SetLocalValue(MathHelper.Clamp(myObjectBuilder_PistonBase.Velocity, -1f, 1f) * BlockDefinition.MaxVelocity);
			MaxImpulseAxis.SetLocalValue(myObjectBuilder_PistonBase.MaxImpulseAxis ?? BlockDefinition.DefaultMaxImpulseAxis);
			MaxImpulseNonAxis.SetLocalValue(myObjectBuilder_PistonBase.MaxImpulseNonAxis ?? BlockDefinition.DefaultMaxImpulseNonAxis);
			MaxLimit.SetLocalValue(myObjectBuilder_PistonBase.MaxLimit.HasValue ? Math.Min(Math.Max(DenormalizeDistance(myObjectBuilder_PistonBase.MaxLimit.Value), BlockDefinition.Minimum), BlockDefinition.Maximum) : BlockDefinition.Maximum);
			MinLimit.SetLocalValue(myObjectBuilder_PistonBase.MinLimit.HasValue ? Math.Max(Math.Min(DenormalizeDistance(myObjectBuilder_PistonBase.MinLimit.Value), BlockDefinition.Maximum), BlockDefinition.Minimum) : BlockDefinition.Minimum);
			m_currentPos = MathHelper.Clamp(myObjectBuilder_PistonBase.CurrentPosition, BlockDefinition.Minimum, BlockDefinition.Maximum);
			m_lastVelocity = Velocity.Value;
			MaxImpulseAxis.ValueChanged += delegate
			{
				OnUnsafeSettingsChanged();
			};
			MaxImpulseNonAxis.ValueChanged += delegate
			{
				OnUnsafeSettingsChanged();
			};
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_PistonBase obj = (MyObjectBuilder_PistonBase)base.GetObjectBuilderCubeBlock(copy);
			obj.Velocity = (float)Velocity / BlockDefinition.MaxVelocity;
			obj.MaxLimit = NormalizeDistance(MaxLimit);
			obj.MinLimit = NormalizeDistance(MinLimit);
			obj.CurrentPosition = m_currentPos;
			obj.MaxImpulseAxis = MaxImpulseAxis.Value;
			obj.MaxImpulseNonAxis = MaxImpulseNonAxis.Value;
			return obj;
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			base.ResourceSink.Update();
		}

		private void OnPhysicsEnabledChanged()
		{
			if (m_subpartPhysics == null)
			{
				return;
			}
			if (base.CubeGrid.Physics.Enabled)
			{
				if (m_subPartContraintInScene)
				{
					m_subpartPhysics.Enabled = true;
				}
			}
			else
			{
				m_subpartPhysics.Enabled = false;
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			oldGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
			if (oldGrid.Physics != null)
			{
				MyGridPhysics physics = oldGrid.Physics;
				physics.EnabledChanged = (Action)Delegate.Remove(physics.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
			if (base.CubeGrid.Physics != null)
			{
				MyGridPhysics physics2 = base.CubeGrid.Physics;
				physics2.EnabledChanged = (Action)Delegate.Combine(physics2.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
		}

		private void CubeGrid_OnPhysicsChanged(MyEntity obj)
		{
			if (base.CubeGrid.Physics != null)
			{
				MyGridPhysics physics = base.CubeGrid.Physics;
				physics.EnabledChanged = (Action)Delegate.Remove(physics.EnabledChanged, new Action(OnPhysicsEnabledChanged));
				MyGridPhysics physics2 = base.CubeGrid.Physics;
				physics2.EnabledChanged = (Action)Delegate.Combine(physics2.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
			if (base.CubeGrid.Physics != null)
			{
				MyGridPhysics physics = base.CubeGrid.Physics;
				physics.EnabledChanged = (Action)Delegate.Combine(physics.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private float NormalizeDistance(float value)
		{
			return (value + BlockDefinition.Minimum) / Range;
		}

		private float DenormalizeDistance(float value)
		{
			value = MathHelper.Clamp(value, 0f, 1f);
			return value * Range + BlockDefinition.Minimum;
		}

		private void LoadSubparts()
		{
			DisposeSubpartsPhysics();
			if (!base.Closed && base.Subparts.TryGetValue("PistonSubpart1", out m_subpart1) && m_subpart1.Subparts.TryGetValue("PistonSubpart2", out m_subpart2) && m_subpart2.Subparts.TryGetValue("PistonSubpart3", out Subpart3))
			{
				if (Subpart3.Model.Dummies.TryGetValue("TopBlock", out MyModelDummy value))
				{
					m_constraintBasePos = value.Matrix.Translation;
				}
				if (base.Model.Dummies.TryGetValue("subpart_PistonSubpart1", out value))
				{
					m_subpartsConstraintPos = value.Matrix.Translation;
					m_subpart1LocPos = m_subpartsConstraintPos;
				}
				if (m_subpart1.Model.Dummies.TryGetValue("subpart_PistonSubpart2", out value))
				{
					m_subpart2LocPos = value.Matrix.Translation;
				}
				if (m_subpart2.Model.Dummies.TryGetValue("subpart_PistonSubpart3", out value))
				{
					m_subpart3LocPos = value.Matrix.Translation;
				}
				if (base.CubeGrid.CreatePhysics)
				{
					InitSubpartsPhysics();
				}
			}
		}

		private void SetSubpartBodyOffset()
		{
			Vector3D v = Vector3D.Transform(Vector3D.Transform(m_constraintBasePos, Subpart3.WorldMatrix), base.PositionComp.WorldMatrixNormalizedInv);
			v -= m_currentPos * Vector3.Up * 0.5f;
			m_subpartPhysics.Center = v;
		}

		private void InitSubpartsPhysics()
		{
			MyEntitySubpart subpart = m_subpart1;
			if (subpart != null && base.CubeGrid.Physics != null)
			{
				m_subpartPhysics = new PistonSubpartPhysicsComponent(this, this, (RigidBodyFlag)(((base.CubeGrid.GridSizeEnum == MyCubeSize.Large) ? 128 : 0) | 0x800));
				HkCylinderShape shape = new HkCylinderShape(new Vector3(0f, -2f, 0f), new Vector3(0f, 2f, 0f), base.CubeGrid.GridSize / 2f - 0.11f, 0.05f);
				HkMassProperties value = HkInertiaTensorComputer.ComputeCylinderVolumeMassProperties(new Vector3(0f, -2f, 0f), new Vector3(0f, 2f, 0f), base.CubeGrid.GridSize / 2f, 40f * base.CubeGrid.GridSize);
				value.Mass = BlockDefinition.Mass;
				m_subpartPhysics.CreateFromCollisionObject(shape, Vector3.Zero, subpart.WorldMatrix, value);
				m_subpartPhysics.RigidBody.Layer = base.CubeGrid.Physics.RigidBody.Layer;
				uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(m_subpartPhysics.RigidBody.Layer, base.CubeGrid.Physics.HavokCollisionSystemID, 1, 1);
				m_subpartPhysics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
				shape.Base.RemoveReference();
				if (m_subpartPhysics.RigidBody2 != null)
				{
					m_subpartPhysics.RigidBody2.Layer = 17;
				}
				base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_OnHavokSystemIDChanged;
				m_subpartPhysics.IsSubpart = true;
				CreateSubpartsConstraint(subpart);
				SetSubpartBodyOffset();
				base.Physics = m_subpartPhysics;
				m_posChanged = true;
			}
		}

		private void CubeGrid_OnHavokSystemIDChanged(int sysId)
		{
			if (base.CubeGrid.Physics != null && base.CubeGrid.Physics.RigidBody != null && m_subpartPhysics != null && m_subpartPhysics.RigidBody != null)
			{
				uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(base.CubeGrid.Physics.RigidBody.Layer, sysId, 1, 1);
				m_subpartPhysics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
			}
		}

		private void DisposeSubpartsPhysics()
		{
			if (m_subpartsConstraint != null)
			{
				DisposeSubpartsConstraint();
			}
			if (m_subpart1 != null && m_subpartPhysics != null)
			{
				base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
				m_subpartPhysics.Enabled = false;
				m_subpartPhysics.Close();
				m_subpartPhysics = null;
			}
		}

		private void CreateSubpartsConstraint(MyEntitySubpart subpart)
		{
			m_subpartsFixedData = new HkFixedConstraintData();
			m_subpartsFixedData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
			m_subpartsFixedData.SetInertiaStabilizationFactor(1f);
			MatrixD m = MatrixD.CreateWorld(base.Position * base.CubeGrid.GridSize + Vector3D.Transform(Vector3D.Transform(m_subpartsConstraintPos, base.WorldMatrix), base.CubeGrid.PositionComp.LocalMatrix), base.PositionComp.LocalMatrix.Forward, base.PositionComp.LocalMatrix.Up);
			m.Translation = base.CubeGrid.Physics.WorldToCluster(m.Translation);
			Matrix bodyATransform = m;
			Matrix bodyBTransform = subpart.PositionComp.LocalMatrix;
			m_subpartsFixedData.SetInWorldSpace(ref bodyATransform, ref bodyBTransform, ref bodyBTransform);
			HkConstraintData subpartsFixedData = m_subpartsFixedData;
			m_subpartsConstraint = new HkConstraint(base.CubeGrid.Physics.RigidBody, m_subpartPhysics.RigidBody, subpartsFixedData);
			uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(base.CubeGrid.Physics.RigidBody.Layer, base.CubeGrid.Physics.HavokCollisionSystemID, 1, 1);
			m_subpartPhysics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
			base.CubeGrid.Physics.HavokWorld.RefreshCollisionFilterOnEntity(m_subpartPhysics.RigidBody);
			m_subpartsConstraint.WantRuntime = true;
		}

		private void DisposeSubpartsConstraint()
		{
			if (m_subPartContraintInScene)
			{
				m_subPartContraintInScene = false;
				base.CubeGrid.Physics.RemoveConstraint(m_subpartsConstraint);
			}
			m_subpartsConstraint.Dispose();
			m_subpartsConstraint = null;
			m_subpartsFixedData = null;
		}

		private void CheckSubpartConstraint()
		{
			if (!MyPhysicsBody.IsConstraintValid(m_subpartsConstraint))
			{
				DisposeSubpartsConstraint();
				CreateSubpartsConstraint(m_subpart1);
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			LoadSubparts();
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (!base.m_welded)
			{
				SetDetailedInfoDirty();
				UpdatePhysicsShape();
			}
			UpdateSoundState();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
		}

		private void UpdatePhysicsShape()
		{
			MyEntitySubpart subpart = m_subpart1;
			if (!m_posChanged || subpart == null || m_subpartPhysics == null || m_subpartPhysics.RigidBody == null)
			{
				return;
			}
			m_posChanged = false;
			float num = Math.Abs(((float)Velocity < 0f) ? ((float)Velocity / 6f) : 0f);
			float num2 = 0f;
			Vector3 vertexA = new Vector3(0f, num2 - m_currentPos * 0.5f + num - 0.1f, 0f);
			Vector3 vertexB = new Vector3(0f, num2 + m_currentPos * 0.5f - num, 0f);
			if (vertexB.Y - vertexA.Y > 0.1f)
			{
				HkShape shape = m_subpartPhysics.RigidBody.GetShape();
				if (shape.ShapeType == HkShapeType.Cylinder)
				{
					float num3 = Math.Abs(vertexA.Y - vertexB.Y);
					HkCylinderShape hkCylinderShape = (HkCylinderShape)shape;
					float num4 = Math.Abs(hkCylinderShape.VertexA.Y - hkCylinderShape.VertexB.Y);
					if (Math.Abs(num3 - num4) < 0.001f)
					{
						return;
					}
					hkCylinderShape.VertexA = vertexA;
					hkCylinderShape.VertexB = vertexB;
					m_subpartPhysics.RigidBody.UpdateShape();
					if (m_subpartPhysics.RigidBody2 != null)
					{
						HkShape shape2 = m_subpartPhysics.RigidBody2.GetShape();
						if (shape2.ShapeType == HkShapeType.Cylinder)
						{
							HkCylinderShape hkCylinderShape2 = (HkCylinderShape)shape2;
							hkCylinderShape2.VertexA = vertexA;
							hkCylinderShape2.VertexB = vertexB;
							m_subpartPhysics.RigidBody2.UpdateShape();
						}
					}
				}
				if (!m_subpartPhysics.Enabled)
				{
					SetSubpartBodyOffset();
					m_subpartPhysics.Enabled = true;
				}
				CheckSubpartConstraint();
				UpdateSubpartFixedData();
				if (base.CubeGrid.Physics.IsInWorldWelded() && m_subpartPhysics.IsInWorldWelded() && m_subpartsConstraint != null && !m_subpartsConstraint.InWorld && !m_subPartContraintInScene)
				{
					m_subPartContraintInScene = true;
					base.CubeGrid.Physics.AddConstraint(m_subpartsConstraint);
				}
				if (m_subpartsConstraint != null && !m_subpartsConstraint.Enabled)
				{
					m_subPartContraintInScene = true;
					if (!m_subpartsConstraint.ForceDisabled)
					{
						m_subpartsConstraint.Enabled = true;
					}
				}
			}
			else
			{
				if (m_subpartsConstraint.Enabled && m_subpartsConstraint.InWorld)
				{
					m_subPartContraintInScene = false;
					m_subpartsConstraint.Enabled = false;
					base.CubeGrid.Physics.RemoveConstraint(m_subpartsConstraint);
				}
				m_subpartPhysics.Enabled = false;
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			SetDetailedInfoDirty();
			if (m_constraint != null && MyPhysicsConfig.EnablePistonImpulseDebugDraw)
			{
				GetConstraintImpulses(m_constraint, out float axial, out float nonAxial);
				MyRenderProxy.DebugDrawText3D(base.WorldMatrix.Translation + base.WorldMatrix.Up, axial.ToString("F2"), Color.Yellow, 1f, depthRead: true);
				MyRenderProxy.DebugDrawText3D(base.WorldMatrix.Translation + base.WorldMatrix.Up * 2.0, nonAxial.ToString("F2"), Color.Blue, 1f, depthRead: true);
			}
			if (base.m_welded)
			{
				return;
			}
			if (base.SafeConstraint != null && base.SafeConstraint.RigidBodyA == base.SafeConstraint.RigidBodyB)
			{
				base.SafeConstraint.Enabled = false;
				return;
			}
			UpdatePosition();
			if (m_ignoreNonAxialForcesForNMoreFrames > 0)
			{
				m_ignoreNonAxialForcesForNMoreFrames--;
			}
			if (m_subpartPhysics != null && m_subpartPhysics.RigidBody2 != null)
			{
				if (m_subpartPhysics.RigidBody.IsActive)
				{
					m_subpartPhysics.RigidBody2.LinearVelocity = m_subpartPhysics.LinearVelocity;
					m_subpartPhysics.RigidBody2.AngularVelocity = m_subpartPhysics.AngularVelocity;
				}
				else
				{
					m_subpartPhysics.RigidBody2.Deactivate();
				}
			}
			if (m_soundEmitter != null && m_soundEmitter.IsPlaying && m_lastPosition.Equals(float.MaxValue))
			{
				m_soundEmitter.StopSound(forced: true);
				m_lastPosition = m_currentPos;
			}
		}

		private float CalcHeadLinearDisplacement(bool positive)
		{
			float val = LinearDispacementOf(m_constraint);
			int num = 0;
			if (positive)
			{
				return Math.Max(val, num);
			}
			return Math.Min(val, num);
		}

		private float LinearDispacementOf(HkConstraint constraint)
		{
			if (constraint == null || !constraint.InWorld)
			{
				return 0f;
			}
			constraint.GetPivotsInWorld(out Vector3 pivotA, out Vector3 pivotB);
			Vector3D up = base.WorldMatrix.Up;
			return Vector3.Dot(pivotB - pivotA, up);
		}

		private bool IsImpulseOverThreshold(bool ignoreNonAxialImpulse)
		{
			if (!IsImpulseOverThreshold(m_constraint, ignoreNonAxialImpulse))
			{
				return IsImpulseOverThreshold(m_subpartsConstraint, ignoreNonAxialImpulse);
			}
			return true;
		}

		private bool IsImpulseOverThreshold(HkConstraint constraint, bool ignoreNonAxialImpulse)
		{
			if (!MyPhysicsConfig.EnablePistonImpulseChecking)
			{
				return false;
			}
			if (constraint == null || !constraint.InWorld)
			{
				return false;
			}
			GetConstraintImpulses(constraint, out float axial, out float nonAxial);
			if ((float)Velocity > 0f)
			{
				axial = 0f - axial;
			}
			if (axial < (float)MaxImpulseAxis)
			{
				if (!ignoreNonAxialImpulse)
				{
					return !(nonAxial < (float)MaxImpulseNonAxis);
				}
				return false;
			}
			return true;
		}

		private void GetConstraintImpulses(HkConstraint constraint, out float axial, out float nonAxial)
		{
			Vector3 vector = Vector3.TransformNormal(new Vector3(HkFixedConstraintData.GetSolverImpulseInLastStep(constraint, 0), HkFixedConstraintData.GetSolverImpulseInLastStep(constraint, 1), HkFixedConstraintData.GetSolverImpulseInLastStep(constraint, 2)), base.PositionComp.WorldMatrixNormalizedInv);
			axial = 0f - vector.Y;
			nonAxial = Math.Max(Math.Abs(vector.X), Math.Abs(vector.X));
		}

		private void UpdatePosition(bool forceUpdate = false)
		{
			if (m_subpart1 == null || (!base.IsWorking && !forceUpdate))
			{
				return;
			}
			if (Math.Sign(m_lastVelocity) != Math.Sign(Velocity))
			{
				m_lastVelocity = Velocity;
				m_ignoreNonAxialForcesForNMoreFrames = 5;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			float num = (float)Velocity / 60f;
			bool flag4 = !IsImpulseOverThreshold(m_ignoreNonAxialForcesForNMoreFrames > 0);
			MyCubeGrid topGrid = base.TopGrid;
			if (topGrid != null)
			{
				if (topGrid == base.CubeGrid)
				{
					flag4 = false;
				}
				else if (m_constraint != null && m_constraint.RigidBodyA.IsFixed && m_constraint.RigidBodyB.IsFixed)
				{
					flag4 = false;
				}
			}
			if (!forceUpdate)
			{
				if (num < 0f)
				{
					if (m_currentPos > (float)MinLimit)
					{
						flag3 = true;
						if (flag4)
						{
							MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(base.CubeGrid);
							if (!MySessionComponentSafeZones.IsActionAllowed(root.GetPhysicalGroupAABB(), (MySafeZoneAction)0, root.EntityId, 0uL))
							{
								flag2 = true;
							}
							if (!flag2)
							{
								m_currentPos = Math.Max(m_currentPos + num, MinLimit);
								flag = true;
								if (m_currentPos <= (float)MinLimit)
								{
									flag2 = true;
								}
							}
						}
					}
				}
				else if (m_currentPos < (float)MaxLimit)
				{
					flag3 = true;
					if (flag4)
					{
						MyCubeGrid root2 = MyGridPhysicalHierarchy.Static.GetRoot(base.CubeGrid);
						if (!MySessionComponentSafeZones.IsActionAllowed(root2.GetPhysicalGroupAABB(), (MySafeZoneAction)0, root2.EntityId, 0uL))
						{
							flag2 = true;
						}
						if (!flag2)
						{
							m_currentPos = Math.Min(m_currentPos + num, MaxLimit);
							flag = true;
							if (m_currentPos >= (float)MaxLimit)
							{
								flag2 = true;
							}
						}
					}
				}
			}
			if (flag2)
			{
				bool arg = num >= 0f;
				this.LimitReached.InvokeIfNotNull(arg);
				StopMovingSound();
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
			else if (!forceUpdate)
			{
				if (flag3)
				{
					if (base.TopGrid != null && base.TopGrid.Physics != null)
					{
						base.TopGrid.Physics.RigidBody.Activate();
					}
					if (base.CubeGrid != null && base.CubeGrid.Physics != null)
					{
						base.CubeGrid.Physics.RigidBody.Activate();
					}
					if (m_subpartPhysics != null)
					{
						m_subpartPhysics.RigidBody.Activate();
					}
				}
				if (flag)
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
					MyGridPhysicalGroupData.InvalidateSharedMassPropertiesCache(base.CubeGrid);
				}
				else
				{
					StopMovingSound();
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
				}
			}
			if (flag || forceUpdate)
			{
				SetDetailedInfoDirty();
				UpdateAnimation();
				m_posChanged = true;
				if (base.CubeGrid == null)
				{
					MySandboxGame.Log.WriteLine("CubeGrid is null");
				}
				if (Subpart3 == null)
				{
					MySandboxGame.Log.WriteLine("Subpart is null");
				}
				if (base.TopGrid != null && base.TopGrid.Physics != null)
				{
					FillFixedData();
				}
				UpdateSubpartFixedData();
			}
		}

		private void StopMovingSound()
		{
			if (m_soundEmitter != null && m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		private void UpdateSubpartFixedData()
		{
			Matrix topMatrixLocal = GetTopMatrixLocal();
			topMatrixLocal.Translation -= m_currentPos * base.PositionComp.LocalMatrix.Up * 0.5f;
			Matrix identity = Matrix.Identity;
			if (m_subpartsFixedData != null)
			{
				m_subpartsFixedData.SetInBodySpace(topMatrixLocal, identity, base.CubeGrid.Physics, m_subpartPhysics);
			}
			else
			{
				MySandboxGame.Log.WriteLine("m_subpartsFixedData is null");
			}
		}

		protected override MatrixD GetTopGridMatrix()
		{
			UpdateAnimation();
			return MatrixD.CreateWorld(Vector3D.Transform(m_constraintBasePos, Subpart3.WorldMatrix), base.WorldMatrix.Forward, base.WorldMatrix.Up);
		}

		private Matrix GetTopMatrixLocal()
		{
			Matrix localMatrix = base.PositionComp.LocalMatrix;
			localMatrix.Translation = Vector3D.Transform(Vector3D.Transform(m_constraintBasePos, Subpart3.WorldMatrix), base.CubeGrid.PositionComp.WorldMatrixNormalizedInv);
			return localMatrix;
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			StopMovingSound();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(GetAttachState())).AppendLine();
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_PistonCurrentPosition)).AppendDecimal(m_currentPos, 1).Append("m");
			RaisePropertiesChanged();
		}

		private void UpdateAnimation()
		{
			float currentPos = m_currentPos;
			float val = Math.Min(currentPos - 2f * Range / 3f, Range / 3f);
			val = Math.Max(0f, val);
			if (m_subpart1 != null)
			{
				Matrix localMatrix = Matrix.CreateWorld(m_subpart1LocPos + Vector3.Up * val, Vector3.Forward, Vector3.Up);
				m_subpart1.PositionComp.SetLocalMatrix(ref localMatrix, null, updateWorld: true);
			}
			_ = Sync.IsServer;
			val = Math.Min(currentPos - Range / 3f, Range / 3f);
			val = Math.Max(0f, val);
			if (m_subpart2 != null)
			{
				Matrix localMatrix2 = Matrix.CreateWorld(m_subpart2LocPos + Vector3.Up * val, Vector3.Forward, Vector3.Up);
				m_subpart2.PositionComp.SetLocalMatrix(ref localMatrix2, null, updateWorld: true);
			}
			val = Math.Min(currentPos, Range / 3f);
			val = Math.Max(0f, val);
			if (Subpart3 != null)
			{
				Matrix localMatrix3 = Matrix.CreateWorld(m_subpart3LocPos + Vector3.Up * val, Vector3.Forward, Vector3.Up);
				Subpart3.PositionComp.SetLocalMatrix(ref localMatrix3, null, updateWorld: true);
			}
		}

		protected override bool Attach(MyAttachableTopBlockBase topBlock, bool updateGroup = true)
		{
			MyPistonTop myPistonTop = topBlock as MyPistonTop;
			if (myPistonTop != null && base.Attach(topBlock, updateGroup))
			{
				UpdateAnimation();
				CreateConstraint(topBlock);
				if (updateGroup)
				{
					m_conveyorEndpoint.Attach(myPistonTop.ConveyorEndpoint as MyAttachableConveyorEndpoint);
				}
				SetDetailedInfoDirty();
				return true;
			}
			return false;
		}

		private void FillFixedData()
		{
			if (!(m_fixedData == null))
			{
				Matrix pivotB = Matrix.Identity;
				Matrix topMatrixLocal = GetTopMatrixLocal();
				MyAttachableTopBlockBase topBlock = base.TopBlock;
				if (topBlock != null)
				{
					Matrix localMatrix = topBlock.PositionComp.LocalMatrix;
					pivotB = Matrix.CreateWorld(base.TopBlock.Position * base.TopBlock.CubeGrid.GridSize, localMatrix.Forward, localMatrix.Up);
				}
				m_fixedData.SetInBodySpace(topMatrixLocal, pivotB, base.CubeGrid.Physics, base.TopGrid.Physics);
			}
		}

		protected override bool CreateConstraint(MyAttachableTopBlockBase topBlock)
		{
			if (!base.CreateConstraint(topBlock))
			{
				return false;
			}
			m_fixedData = new HkFixedConstraintData();
			m_fixedData.SetInertiaStabilizationFactor(1f);
			m_fixedData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
			UpdateAnimation();
			FillFixedData();
			m_constraint = new HkConstraint(base.CubeGrid.Physics.RigidBody, topBlock.CubeGrid.Physics.RigidBody, m_fixedData);
			m_constraint.WantRuntime = true;
			base.CubeGrid.Physics.AddConstraint(m_constraint);
			if (!m_constraint.InWorld)
			{
				base.CubeGrid.Physics.RemoveConstraint(m_constraint);
				m_constraint.Dispose();
				m_constraint = null;
				m_fixedData = null;
				return false;
			}
			m_constraint.Enabled = true;
			return true;
		}

		protected override bool CanPlaceTop(MyAttachableTopBlockBase topBlock, long builtBy)
		{
			float y = Subpart3.Model.BoundingBoxSize.Y;
			Vector3D translation = Subpart3.WorldMatrix.Translation + base.WorldMatrix.Up * y;
			float num = topBlock.ModelCollision.HavokCollisionShapes[0].ConvexRadius * 0.9f;
			BoundingSphereD sphere = topBlock.Model.BoundingSphere;
			sphere.Center = translation;
			sphere.Radius = num;
			using (MyUtils.ReuseCollection(ref MyMechanicalConnectionBlockBase.m_tmpSet))
			{
				base.CubeGrid.GetBlocksInsideSphere(ref sphere, MyMechanicalConnectionBlockBase.m_tmpSet);
				if (MyMechanicalConnectionBlockBase.m_tmpSet.Count > 1)
				{
					if (builtBy == MySession.Static.LocalPlayerId)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.HeadNotPlaced);
					}
					return false;
				}
			}
			using (MyUtils.ReuseCollection(ref MyMechanicalConnectionBlockBase.m_penetrations))
			{
				Quaternion rotation = Quaternion.Identity;
				MyPhysics.GetPenetrationsShape(topBlock.ModelCollision.HavokCollisionShapes[0], ref translation, ref rotation, MyMechanicalConnectionBlockBase.m_penetrations, 15);
				for (int i = 0; i < MyMechanicalConnectionBlockBase.m_penetrations.Count; i++)
				{
					VRage.ModAPI.IMyEntity collisionEntity = MyMechanicalConnectionBlockBase.m_penetrations[i].GetCollisionEntity();
					if (!collisionEntity.Physics.IsPhantom)
					{
						MyCubeGrid myCubeGrid = collisionEntity.GetTopMostParent() as MyCubeGrid;
						if (myCubeGrid == null || myCubeGrid != base.CubeGrid)
						{
							if (builtBy == MySession.Static.LocalPlayerId)
							{
								MyHud.Notifications.Add(MyNotificationSingletons.HeadNotPlaced);
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		public override void UpdateOnceBeforeFrame()
		{
			LoadSubparts();
			UpdateAnimation();
			if (!base.CubeGrid.IsPreview)
			{
				UpdatePosition(forceUpdate: true);
				UpdatePhysicsShape();
			}
			base.UpdateOnceBeforeFrame();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
		}

		public override void OnRemovedFromScene(object source)
		{
			DisposeSubpartsPhysics();
			base.OnRemovedFromScene(source);
			base.CubeGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
			if (base.CubeGrid.Physics != null)
			{
				MyGridPhysics physics = base.CubeGrid.Physics;
				physics.EnabledChanged = (Action)Delegate.Remove(physics.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
		}

		protected override void BeforeDelete()
		{
			DisposeSubpartsPhysics();
			base.BeforeDelete();
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyAttachableConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
		}

		private void UpdateSoundState()
		{
			if (!MySandboxGame.IsGameReady || m_soundEmitter == null || !base.IsWorking)
			{
				return;
			}
			if (base.TopGrid == null || base.TopGrid.Physics == null)
			{
				m_soundEmitter.StopSound(forced: true);
				return;
			}
			if (base.IsWorking && !m_lastPosition.Equals(m_currentPos))
			{
				m_soundEmitter.PlaySingleSound(BlockDefinition.PrimarySound, stopPrevious: true);
			}
			else
			{
				m_soundEmitter.StopSound(forced: false);
			}
			m_lastPosition = m_currentPos;
		}

		public override void ComputeTopQueryBox(out Vector3D pos, out Vector3 halfExtents, out Quaternion orientation)
		{
			MatrixD worldMatrix = base.WorldMatrix;
			orientation = Quaternion.CreateFromRotationMatrix(worldMatrix);
			halfExtents = Vector3.One * base.CubeGrid.GridSize * 0.35f;
			halfExtents.Y = base.CubeGrid.GridSize * 0.25f;
			pos = Subpart3.WorldMatrix.Translation + Subpart3.PositionComp.WorldVolume.Radius * base.WorldMatrix.Up + 0.5f * base.CubeGrid.GridSize * base.WorldMatrix.Up;
		}

		protected override void DisposeConstraint(MyCubeGrid topGrid)
		{
			if (m_constraint != null)
			{
				m_fixedData = null;
				base.CubeGrid.Physics.RemoveConstraint(m_constraint);
				m_constraint.Dispose();
				m_constraint = null;
			}
			base.DisposeConstraint(topGrid);
		}

		protected override void Detach(MyCubeGrid grid, bool updateGroup = true)
		{
			if (base.TopBlock != null && updateGroup)
			{
				MyPistonTop myPistonTop = base.TopBlock as MyPistonTop;
				if (myPistonTop != null)
				{
					m_conveyorEndpoint.Detach(myPistonTop.ConveyorEndpoint as MyAttachableConveyorEndpoint);
				}
			}
			base.Detach(grid, updateGroup);
		}

		protected override bool HasUnsafeSettingsCollector()
		{
			float unsafeImpulseThreshold = BlockDefinition.UnsafeImpulseThreshold;
			if (!(MaxImpulseAxis.Value > unsafeImpulseThreshold) && !(MaxImpulseNonAxis.Value > unsafeImpulseThreshold))
			{
				return base.HasUnsafeSettingsCollector();
			}
			return true;
		}

		public void SetCurrentPosByTopGridMatrix()
		{
			Vector3 vector = m_subpart1LocPos + m_subpart2LocPos + m_subpart3LocPos + m_constraintBasePos;
			Vector3D vector3D = Vector3D.Transform(base.TopBlock.WorldMatrix.Translation, MatrixD.Invert(base.PositionComp.WorldMatrix));
			m_currentPos = (float)vector3D.Y - vector.Length();
			UpdatePosition(forceUpdate: true);
			UpdatePhysicsShape();
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

		private Action<MyMechanicalConnectionBlockBase> GetDelegate(Action<Sandbox.ModAPI.IMyPistonBase> value)
		{
			return (Action<MyMechanicalConnectionBlockBase>)Delegate.CreateDelegate(typeof(Action<MyMechanicalConnectionBlockBase>), value.Target, value.Method);
		}

		void Sandbox.ModAPI.IMyPistonBase.Attach(Sandbox.ModAPI.IMyPistonTop top, bool updateGroup)
		{
			((Sandbox.ModAPI.IMyMechanicalConnectionBlock)this).Attach((Sandbox.ModAPI.IMyAttachableTopBlock)top, updateGroup);
		}

		void Sandbox.ModAPI.Ingame.IMyPistonBase.Extend()
		{
			if (base.IsFunctional && (float)Velocity < 0f)
			{
				Velocity.Value = 0f - (float)Velocity;
			}
		}

		void Sandbox.ModAPI.Ingame.IMyPistonBase.Retract()
		{
			if (base.IsFunctional && (float)Velocity > 0f)
			{
				Velocity.Value = 0f - (float)Velocity;
			}
		}

		void Sandbox.ModAPI.Ingame.IMyPistonBase.Reverse()
		{
			if (base.IsFunctional)
			{
				Velocity.Value = 0f - (float)Velocity;
			}
		}
	}
}
