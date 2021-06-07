using Havok;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_LandingGear))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyLandingGear),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear)
	})]
	public class MyLandingGear : MyFunctionalBlock, Sandbox.Game.Entities.Interfaces.IMyLandingGear, SpaceEngineers.Game.ModAPI.IMyLandingGear, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear
	{
		[Serializable]
		protected struct State
		{
			protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear_003C_003EState_003C_003EForce_003C_003EAccessor : IMemberAccessor<State, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in bool value)
				{
					owner.Force = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out bool value)
				{
					value = owner.Force;
				}
			}

			protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear_003C_003EState_003C_003EOtherEntityId_003C_003EAccessor : IMemberAccessor<State, long?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in long? value)
				{
					owner.OtherEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out long? value)
				{
					value = owner.OtherEntityId;
				}
			}

			protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear_003C_003EState_003C_003EMasterToSlave_003C_003EAccessor : IMemberAccessor<State, MyDeltaTransform?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in MyDeltaTransform? value)
				{
					owner.MasterToSlave = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out MyDeltaTransform? value)
				{
					value = owner.MasterToSlave;
				}
			}

			protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear_003C_003EState_003C_003EGearPivotPosition_003C_003EAccessor : IMemberAccessor<State, Vector3?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in Vector3? value)
				{
					owner.GearPivotPosition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out Vector3? value)
				{
					value = owner.GearPivotPosition;
				}
			}

			protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear_003C_003EState_003C_003EOtherPivot_003C_003EAccessor : IMemberAccessor<State, CompressedPositionOrientation?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in CompressedPositionOrientation? value)
				{
					owner.OtherPivot = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out CompressedPositionOrientation? value)
				{
					value = owner.OtherPivot;
				}
			}

			public bool Force;

			public long? OtherEntityId;

			public MyDeltaTransform? MasterToSlave;

			public Vector3? GearPivotPosition;

			public CompressedPositionOrientation? OtherPivot;
		}

		protected sealed class AttachFailed_003C_003E : ICallSite<MyLandingGear, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLandingGear @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AttachFailed();
			}
		}

		protected sealed class ResetLockConstraint_003C_003ESystem_Boolean_0023System_Boolean : ICallSite<MyLandingGear, bool, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLandingGear @this, in bool locked, in bool force, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ResetLockConstraint(locked, force);
			}
		}

		protected sealed class AttachRequest_003C_003ESystem_Boolean : ICallSite<MyLandingGear, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLandingGear @this, in bool enable, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AttachRequest(enable);
			}
		}

		protected class m_lockModeSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lockModeSync;
				ISyncType result = lockModeSync = new Sync<LandingGearMode, SyncDirection.FromServer>(P_1, P_2);
				((MyLandingGear)P_0).m_lockModeSync = (Sync<LandingGearMode, SyncDirection.FromServer>)lockModeSync;
				return result;
			}
		}

		protected class m_autoLock_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autoLock;
				ISyncType result = autoLock = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyLandingGear)P_0).m_autoLock = (Sync<bool, SyncDirection.BothWays>)autoLock;
				return result;
			}
		}

		protected class m_attachedState_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType attachedState;
				ISyncType result = attachedState = new Sync<State, SyncDirection.FromServer>(P_1, P_2);
				((MyLandingGear)P_0).m_attachedState = (Sync<State, SyncDirection.FromServer>)attachedState;
				return result;
			}
		}

		protected class m_breakForceSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType breakForceSync;
				ISyncType result = breakForceSync = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLandingGear)P_0).m_breakForceSync = (Sync<float, SyncDirection.BothWays>)breakForceSync;
				return result;
			}
		}

		private MySoundPair m_lockSound;

		private MySoundPair m_unlockSound;

		private MySoundPair m_failedAttachSound;

		private bool m_firstlockAttempt;

		private int m_firstLockTimer;

		private bool m_converted;

		private const int FIRST_LOCK_TIME = 100;

		private static List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();

		private Matrix[] m_lockPositions;

		private HkConstraint m_constraint;

		private readonly Sync<LandingGearMode, SyncDirection.FromServer> m_lockModeSync;

		private LandingGearMode m_lockMode;

		private Action<VRage.ModAPI.IMyEntity> m_physicsChangedHandler;

		private VRage.ModAPI.IMyEntity m_attachedTo;

		private bool m_needsToRetryLock;

		private int m_autolockTimer;

		private float m_breakForce;

		private readonly Sync<bool, SyncDirection.BothWays> m_autoLock;

		private readonly Sync<State, SyncDirection.FromServer> m_attachedState;

		private readonly Sync<float, SyncDirection.BothWays> m_breakForceSync;

		private long? m_attachedEntityId;

		private int m_retryCounter;

		public Matrix[] LockPositions => m_lockPositions;

		public bool LockedToStatic => m_converted;

		private HkConstraint SafeConstraint
		{
			get
			{
				if (m_constraint != null && !m_constraint.InWorld)
				{
					Detach();
				}
				return m_constraint;
			}
		}

		public LandingGearMode LockMode
		{
			get
			{
				return m_lockMode;
			}
			private set
			{
				if (m_lockMode != value)
				{
					LandingGearMode lockMode = m_lockMode;
					m_lockMode = value;
					SetEmissiveStateWorking();
					SetDetailedInfoDirty();
					RaisePropertiesChanged();
					this.LockModeChanged?.Invoke(this, lockMode);
				}
			}
		}

		public bool IsLocked => LockMode == LandingGearMode.Locked;

		public bool IsBreakable => BreakForce < 1E+08f;

		public float BreakForce
		{
			get
			{
				return m_breakForce;
			}
			set
			{
				if (m_breakForce == value)
				{
					return;
				}
				bool isBreakable = IsBreakable;
				m_breakForce = value;
				if (isBreakable != IsBreakable)
				{
					m_breakForce = value;
					if (Sync.IsServer)
					{
						ResetLockConstraint(LockMode == LandingGearMode.Locked);
					}
					RaisePropertiesChanged();
				}
				else if (IsBreakable)
				{
					UpdateBrakeThreshold();
					RaisePropertiesChanged();
				}
			}
		}

		private bool CanAutoLock
		{
			get
			{
				if ((bool)m_autoLock)
				{
					return m_autolockTimer == 0;
				}
				return false;
			}
		}

		public bool AutoLock
		{
			get
			{
				return m_autoLock;
			}
			set
			{
				m_autoLock.Value = value;
			}
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.IsBreakable => IsBreakable;

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.IsLocked => IsLocked;

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.AutoLock
		{
			get
			{
				return AutoLock;
			}
			set
			{
				AutoLock = value;
			}
		}

		LandingGearMode SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.LockMode => LockMode;

		public event LockModeChangedHandler LockModeChanged;

		private event Action<bool> StateChanged;

		event Action<SpaceEngineers.Game.ModAPI.IMyLandingGear, LandingGearMode> SpaceEngineers.Game.ModAPI.IMyLandingGear.LockModeChanged
		{
			add
			{
				LockModeChanged += GetDelegate(value);
			}
			remove
			{
				LockModeChanged -= GetDelegate(value);
			}
		}

		event Action<bool> SpaceEngineers.Game.ModAPI.IMyLandingGear.StateChanged
		{
			add
			{
				StateChanged += value;
			}
			remove
			{
				StateChanged -= value;
			}
		}

		public MyLandingGear()
		{
			CreateTerminalControls();
			m_physicsChangedHandler = PhysicsChanged;
			m_attachedState.AlwaysReject();
			m_attachedState.ValueChanged += delegate
			{
				AttachedValueChanged();
			};
			m_autoLock.ValueChanged += delegate
			{
				AutolockChanged();
			};
			m_lockModeSync.AlwaysReject();
			m_lockModeSync.ValueChanged += delegate
			{
				OnLockModeChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyLandingGear>())
			{
				base.CreateTerminalControls();
				MyTerminalControl<MyLandingGear>.WriterDelegate writer = delegate(MyLandingGear b, StringBuilder sb)
				{
					b.WriteLockStateValue(sb);
				};
				MyTerminalControlButton<MyLandingGear> obj = new MyTerminalControlButton<MyLandingGear>("Lock", MySpaceTexts.BlockActionTitle_Lock, MySpaceTexts.Blank, delegate(MyLandingGear b)
				{
					b.RequestLandingGearLock();
				})
				{
					Enabled = ((MyLandingGear b) => b.IsWorking)
				};
				MyTerminalControlExtensions.EnableAction(obj, MyTerminalActionIcons.TOGGLE, null, writer);
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlButton<MyLandingGear> obj2 = new MyTerminalControlButton<MyLandingGear>("Unlock", MySpaceTexts.BlockActionTitle_Unlock, MySpaceTexts.Blank, delegate(MyLandingGear b)
				{
					b.RequestLandingGearUnlock();
				})
				{
					Enabled = ((MyLandingGear b) => b.IsWorking)
				};
				MyTerminalControlExtensions.EnableAction(obj2, MyTerminalActionIcons.TOGGLE, null, writer);
				MyTerminalControlFactory.AddControl(obj2);
				StringBuilder name = MyTexts.Get(MySpaceTexts.BlockActionTitle_SwitchLock);
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyLandingGear>("SwitchLock", name, MyTerminalActionIcons.TOGGLE)
				{
					Action = delegate(MyLandingGear b)
					{
						b.RequestLandingGearSwitch();
					},
					Writer = writer
				});
				MyTerminalControlCheckbox<MyLandingGear> obj3 = new MyTerminalControlCheckbox<MyLandingGear>("Autolock", MySpaceTexts.BlockPropertyTitle_LandGearAutoLock, MySpaceTexts.Blank)
				{
					Getter = ((MyLandingGear b) => b.m_autoLock),
					Setter = delegate(MyLandingGear b, bool v)
					{
						b.m_autoLock.Value = v;
					}
				};
				obj3.EnableAction();
				MyTerminalControlFactory.AddControl(obj3);
				if (MyFakes.LANDING_GEAR_BREAKABLE)
				{
					MyTerminalControlSlider<MyLandingGear> obj4 = new MyTerminalControlSlider<MyLandingGear>("BreakForce", MySpaceTexts.BlockPropertyTitle_BreakForce, MySpaceTexts.BlockPropertyDescription_BreakForce)
					{
						Getter = ((MyLandingGear x) => x.BreakForce),
						Setter = delegate(MyLandingGear x, float v)
						{
							x.m_breakForceSync.Value = v;
						},
						DefaultValue = 1f,
						Writer = delegate(MyLandingGear x, StringBuilder result)
						{
							if (x.BreakForce >= 1E+08f)
							{
								result.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_MotorAngleUnlimited));
							}
							else
							{
								MyValueFormatter.AppendForceInBestUnit(x.BreakForce, result);
							}
						},
						Normalizer = ((MyLandingGear b, float v) => ThresholdToRatio(v)),
						Denormalizer = ((MyLandingGear b, float v) => RatioToThreshold(v))
					};
					obj4.EnableActions();
					MyTerminalControlFactory.AddControl(obj4);
				}
			}
		}

		private void OnLockModeChanged()
		{
			LockMode = m_lockModeSync;
		}

		private void BreakForceChanged()
		{
			BreakForce = m_breakForceSync;
		}

		private void AutolockChanged()
		{
			m_autolockTimer = 0;
			SetEmissiveStateWorking();
			RaisePropertiesChanged();
		}

		public void RequestLandingGearSwitch()
		{
			if (LockMode == LandingGearMode.Locked)
			{
				RequestLandingGearUnlock();
			}
			else
			{
				RequestLandingGearLock();
			}
		}

		public void RequestLandingGearLock()
		{
			if (LockMode == LandingGearMode.ReadyToLock)
			{
				RequestLock(enable: true);
			}
		}

		public void RequestLandingGearUnlock()
		{
			if (LockMode == LandingGearMode.Locked)
			{
				RequestLock(enable: false);
			}
		}

		private static float RatioToThreshold(float ratio)
		{
			if (!(ratio >= 1f))
			{
				return MathHelper.InterpLog(ratio, 500f, 1E+08f);
			}
			return 1E+08f;
		}

		private static float ThresholdToRatio(float threshold)
		{
			if (!(threshold >= 1E+08f))
			{
				return MathHelper.InterpLogInv(threshold, 500f, 1E+08f);
			}
			return 1f;
		}

		private void UpdateBrakeThreshold()
		{
			if (SafeConstraint != null && m_constraint.ConstraintData is HkBreakableConstraintData)
			{
				((HkBreakableConstraintData)m_constraint.ConstraintData).Threshold = BreakForce;
				if (m_attachedTo != null && m_attachedTo.Physics != null)
				{
					((MyPhysicsBody)m_attachedTo.Physics).RigidBody.Activate();
				}
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
			if (base.BlockDefinition is MyLandingGearDefinition)
			{
				MyLandingGearDefinition myLandingGearDefinition = (MyLandingGearDefinition)base.BlockDefinition;
				m_lockSound = new MySoundPair(myLandingGearDefinition.LockSound);
				m_unlockSound = new MySoundPair(myLandingGearDefinition.UnlockSound);
				m_failedAttachSound = new MySoundPair(myLandingGearDefinition.FailedAttachSound);
				if (myLandingGearDefinition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
				{
					myLandingGearDefinition.EmissiveColorPreset = MyStringHash.GetOrCompute("ConnectBlock");
				}
			}
			else
			{
				m_lockSound = new MySoundPair("ShipLandGearOn");
				m_unlockSound = new MySoundPair("ShipLandGearOff");
				m_failedAttachSound = new MySoundPair("ShipLandGearNothing01");
			}
			base.Flags |= (EntityFlags.NeedsUpdate | EntityFlags.NeedsUpdate10 | EntityFlags.NeedsUpdateBeforeNextFrame);
			LoadDummies();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			MyObjectBuilder_LandingGear myObjectBuilder_LandingGear = objectBuilder as MyObjectBuilder_LandingGear;
			if (myObjectBuilder_LandingGear.IsLocked)
			{
				LockMode = LandingGearMode.Locked;
				m_needsToRetryLock = true;
				if (Sync.IsServer)
				{
					m_attachedEntityId = myObjectBuilder_LandingGear.AttachedEntityId;
					m_attachedState.Value = new State
					{
						OtherEntityId = myObjectBuilder_LandingGear.AttachedEntityId,
						GearPivotPosition = myObjectBuilder_LandingGear.GearPivotPosition,
						OtherPivot = myObjectBuilder_LandingGear.OtherPivot,
						MasterToSlave = myObjectBuilder_LandingGear.MasterToSlave
					};
				}
			}
			if (MyFakes.LANDING_GEAR_BREAKABLE)
			{
				m_breakForceSync.Value = RatioToThreshold(myObjectBuilder_LandingGear.BrakeForce);
			}
			else
			{
				m_breakForceSync.Value = RatioToThreshold(1E+08f);
			}
			m_breakForceSync.ValueChanged += delegate
			{
				BreakForceChanged();
			};
			AutoLock = myObjectBuilder_LandingGear.AutoLock;
			m_lockModeSync.SetLocalValue(myObjectBuilder_LandingGear.LockMode);
			m_firstlockAttempt = myObjectBuilder_LandingGear.FirstLockAttempt;
			m_firstLockTimer = 0;
			base.IsWorkingChanged += MyLandingGear_IsWorkingChanged;
			SetDetailedInfoDirty();
			AddDebugRenderComponent(new MyDebugRenderComponentLandingGear(this));
		}

		private void MyLandingGear_IsWorkingChanged(MyCubeBlock obj)
		{
			RaisePropertiesChanged();
		}

		public override void ContactPointCallback(ref MyGridContactInfo info)
		{
			if (info.CollidingEntity != null && m_attachedTo == info.CollidingEntity)
			{
				info.EnableDeformation = false;
				info.EnableParticles = false;
			}
		}

		private void LoadDummies()
		{
			m_lockPositions = (from s in base.Model.Dummies
				where s.Key.ToLower().Contains("gear_lock")
				select s.Value.Matrix).ToArray();
		}

		public void GetBoxFromMatrix(MatrixD m, out Vector3 halfExtents, out Vector3D position, out Quaternion orientation)
		{
			MatrixD matrix = MatrixD.Normalize(m) * base.WorldMatrix;
			orientation = Quaternion.CreateFromRotationMatrix(matrix);
			halfExtents = Vector3.Abs(m.Scale) / 2f;
			position = matrix.Translation;
		}

		private MyEntity FindBody(out Vector3D pivot)
		{
			pivot = Vector3D.Zero;
			if (base.CubeGrid.Physics == null)
			{
				return null;
			}
			Matrix[] lockPositions = m_lockPositions;
			foreach (Matrix m in lockPositions)
			{
				GetBoxFromMatrix(m, out Vector3 halfExtents, out pivot, out Quaternion orientation);
				HkBoxShape? hkBoxShape = null;
				try
				{
					halfExtents *= new Vector3(2f, 1f, 2f);
					orientation.Normalize();
					MyPhysics.GetPenetrationsBox(ref halfExtents, ref pivot, ref orientation, m_penetrations, 15);
					hkBoxShape = new HkBoxShape(halfExtents);
					Matrix transform = Matrix.CreateFromQuaternion(orientation);
					foreach (HkBodyCollision penetration in m_penetrations)
					{
						MyEntity myEntity = penetration.GetCollisionEntity() as MyEntity;
						if (myEntity != null)
						{
							myEntity = myEntity.GetTopMostParent();
							if (myEntity.GetPhysicsBody() != null)
							{
								if (myEntity.GetPhysicsBody().WeldInfo.Children.Count > 0)
								{
									Matrix transform2;
									foreach (MyPhysicsBody child in myEntity.GetPhysicsBody().WeldInfo.Children)
									{
										transform2 = (child.Entity as MyEntity).GetPhysicsBody().WeldInfo.Transform * myEntity.WorldMatrix;
										transform2.Translation -= pivot;
										if (MyPhysics.IsPenetratingShapeShape(hkBoxShape.Value, ref transform, child.WeldedRigidBody.GetShape(), ref transform2) && CanAttachTo(penetration, child.Entity as MyEntity, transform2.Translation))
										{
											return child.Entity as MyEntity;
										}
									}
									transform2 = myEntity.WorldMatrix;
									transform2.Translation -= pivot;
									if (MyPhysics.IsPenetratingShapeShape(hkBoxShape.Value, ref transform, myEntity.GetPhysicsBody().GetShape(), ref transform2) && CanAttachTo(penetration, myEntity, transform2.Translation))
									{
										return myEntity;
									}
								}
								else if (CanAttachTo(penetration, myEntity, pivot))
								{
									return myEntity;
								}
							}
						}
					}
				}
				finally
				{
					hkBoxShape?.Base.RemoveReference();
					m_penetrations.Clear();
				}
			}
			return null;
		}

		private bool CanAttachTo(HkBodyCollision obj, MyEntity entity, Vector3D worldPos)
		{
			MyCubeGrid cubeGrid = base.CubeGrid;
			if (entity == cubeGrid || entity.Parent == cubeGrid)
			{
				return false;
			}
			MyGridPhysics physics = cubeGrid.Physics;
			MyPhysicsComponentBase physics2 = entity.Physics;
			if (physics != null && physics2 != null)
			{
				MyLandingGearDefinition myLandingGearDefinition = base.BlockDefinition as MyLandingGearDefinition;
				if (myLandingGearDefinition != null)
				{
					float maxLockSeparatingVelocity = myLandingGearDefinition.MaxLockSeparatingVelocity;
					Vector3 velocityAtPoint = physics2.GetVelocityAtPoint(worldPos);
					if ((physics.LinearVelocity - velocityAtPoint).LengthSquared() > maxLockSeparatingVelocity * maxLockSeparatingVelocity)
					{
						return false;
					}
				}
			}
			if (!obj.Body.IsFixed && obj.Body.IsFixedOrKeyframed)
			{
				return false;
			}
			if (entity is MyCharacter)
			{
				return false;
			}
			return true;
		}

		public override bool SetEmissiveStateWorking()
		{
			if (!base.IsWorking)
			{
				return false;
			}
			switch (LockMode)
			{
			case LandingGearMode.Locked:
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Locked, base.Render.RenderObjectIDs[0]);
			case LandingGearMode.ReadyToLock:
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Constraint, base.Render.RenderObjectIDs[0]);
			case LandingGearMode.Unlocked:
				if (CanAutoLock && base.Enabled)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Autolock, base.Render.RenderObjectIDs[0]);
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			default:
				return false;
			}
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(base.BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_LockState));
			WriteLockStateValue(detailedInfo);
		}

		private void WriteLockStateValue(StringBuilder sb)
		{
			if (LockMode == LandingGearMode.Locked)
			{
				sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_Locked));
			}
			else if (LockMode == LandingGearMode.ReadyToLock)
			{
				sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_ReadyToLock));
			}
			else
			{
				sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_Unlocked));
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_LandingGear myObjectBuilder_LandingGear = (MyObjectBuilder_LandingGear)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_LandingGear.IsLocked = (LockMode == LandingGearMode.Locked);
			myObjectBuilder_LandingGear.BrakeForce = ThresholdToRatio(BreakForce);
			myObjectBuilder_LandingGear.AutoLock = AutoLock;
			myObjectBuilder_LandingGear.LockSound = m_lockSound.ToString();
			myObjectBuilder_LandingGear.FirstLockAttempt = m_firstlockAttempt;
			myObjectBuilder_LandingGear.UnlockSound = m_unlockSound.ToString();
			myObjectBuilder_LandingGear.FailedAttachSound = m_failedAttachSound.ToString();
			myObjectBuilder_LandingGear.AttachedEntityId = m_attachedEntityId;
			if (m_attachedEntityId.HasValue)
			{
				myObjectBuilder_LandingGear.MasterToSlave = m_attachedState.Value.MasterToSlave;
				myObjectBuilder_LandingGear.GearPivotPosition = m_attachedState.Value.GearPivotPosition;
				myObjectBuilder_LandingGear.OtherPivot = m_attachedState.Value.OtherPivot;
			}
			myObjectBuilder_LandingGear.LockMode = m_lockModeSync.Value;
			return myObjectBuilder_LandingGear;
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			EnqueueRetryLock();
			Detach();
		}

		public override void UpdateBeforeSimulation()
		{
			RetryLock();
			base.UpdateBeforeSimulation();
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			CheckEmissiveState();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (!base.IsWorking && !m_firstlockAttempt)
			{
				return;
			}
			if (Sync.IsServer)
			{
				if (m_firstlockAttempt)
				{
					if (m_firstLockTimer >= 100)
					{
						m_firstlockAttempt = false;
					}
					m_firstLockTimer++;
				}
				if (LockMode != LandingGearMode.Locked)
				{
					if (FindBody(out Vector3D _) != null)
					{
						if (CanAutoLock && Sync.IsServer)
						{
							AttachRequest(enable: true);
						}
						else
						{
							m_lockModeSync.Value = LandingGearMode.ReadyToLock;
						}
					}
					else
					{
						m_lockModeSync.Value = LandingGearMode.Unlocked;
					}
				}
			}
			if (m_autolockTimer != 0 && MySandboxGame.TotalGamePlayTimeInMilliseconds - m_autolockTimer > 3000)
			{
				AutoLock = true;
				AutolockChanged();
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_needsToRetryLock = true;
			RetryLock();
		}

		private void RetryLock()
		{
			if (m_needsToRetryLock)
			{
				if (Sync.IsServer)
				{
					RetryLockServer();
				}
				else
				{
					RetryLockClient();
				}
			}
		}

		private void RetryLockServer()
		{
			Vector3D pivot;
			MyEntity myEntity = FindBody(out pivot);
			if (myEntity == null && m_retryCounter < 3)
			{
				m_retryCounter++;
				return;
			}
			m_retryCounter = 0;
			if (myEntity != null && ((m_attachedEntityId.HasValue && myEntity.EntityId == m_attachedEntityId.Value) || !m_attachedEntityId.HasValue))
			{
				if (m_attachedTo == null || m_attachedTo.Physics == null || myEntity.Physics.RigidBody != ((MyPhysicsBody)m_attachedTo.Physics).RigidBody)
				{
					ResetLockConstraint(locked: true);
				}
			}
			else
			{
				long? attachedEntityId = m_attachedEntityId;
				ResetLockConstraint(locked: false);
				m_attachedEntityId = attachedEntityId;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
			m_needsToRetryLock = false;
		}

		private void RetryLockClient()
		{
			State value = m_attachedState.Value;
			if (!value.OtherEntityId.HasValue)
			{
				m_needsToRetryLock = false;
				return;
			}
			long value2 = value.OtherEntityId.Value;
			MyEntities.TryGetEntityById(value2, out MyEntity entity);
			if (entity == null)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
				return;
			}
			if (m_attachedEntityId.HasValue && (m_attachedEntityId.Value != value2 || value.Force))
			{
				Detach();
			}
			m_attachedEntityId = value2;
			if (value.MasterToSlave.HasValue && value.GearPivotPosition.HasValue && value.OtherPivot.HasValue)
			{
				if (!(entity is MyFloatingObject))
				{
					base.CubeGrid.WorldMatrix = MatrixD.Multiply(value.MasterToSlave.Value, entity.WorldMatrix);
				}
				Attach(entity, value.GearPivotPosition.Value, value.OtherPivot.Value.Matrix);
			}
			m_needsToRetryLock = false;
		}

		protected override void Closing()
		{
			Detach();
			base.Closing();
		}

		public void ResetAutolock()
		{
			m_autolockTimer = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			SetEmissiveStateWorking();
		}

		[Event(null, 744)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private void AttachFailed()
		{
			StartSound(m_failedAttachSound);
		}

		private void Attach(long entityID, Vector3 gearSpacePivot, CompressedPositionOrientation otherBodySpacePivot, bool force = false)
		{
			m_attachedEntityId = entityID;
			if (MyEntities.TryGetEntityById(entityID, out MyEntity entity))
			{
				Attach(entity, gearSpacePivot, otherBodySpacePivot.Matrix);
			}
		}

		private void Attach(MyEntity entity, Vector3 gearSpacePivot, Matrix otherBodySpacePivot)
		{
			if (entity == null)
			{
				return;
			}
			m_attachedEntityId = entity.EntityId;
			if (base.CubeGrid?.Physics == null || !base.CubeGrid.Physics.Enabled || m_attachedTo != null || m_constraint != null)
			{
				return;
			}
			MyEntity topMostParent = entity.GetTopMostParent();
			if (topMostParent?.Physics == null)
			{
				return;
			}
			if (entity.Physics == null)
			{
				MyLog.Default.WriteLine(string.Concat("entity ", entity, " has no physics, topmost parent ", entity.GetTopMostParent(), " has physics"));
			}
			Action<bool> stateChanged = this.StateChanged;
			MyCubeGrid myCubeGrid;
			if (Sync.IsServer && (myCubeGrid = (entity as MyCubeGrid)) != null)
			{
				myCubeGrid.OnGridSplit += CubeGrid_OnGridSplit;
			}
			if (MyFakes.WELD_LANDING_GEARS && CanWeldTo(entity, ref otherBodySpacePivot))
			{
				MyCubeGrid childNode;
				if (entity is MyVoxelBase)
				{
					if (base.CubeGrid.Physics.RigidBody == null)
					{
						MyLog.Default.WriteLine("Rigid body null for CubeGrid " + base.CubeGrid);
					}
					if (!m_converted && AnyGearLockedToStatic())
					{
						m_converted = true;
					}
					if (!m_converted)
					{
						HkRigidBody rigidBody = base.CubeGrid.Physics.RigidBody;
						if ((object)rigidBody != null && !rigidBody.IsFixed && !base.CubeGrid.IsStatic)
						{
							MySandboxGame.Static.Invoke(delegate
							{
								if (base.CubeGrid.Physics != null)
								{
									base.CubeGrid.Physics.ConvertToStatic();
								}
							}, "MyLandingGear / Convert to static");
							m_converted = true;
						}
					}
				}
				else if ((childNode = (topMostParent as MyCubeGrid)) != null)
				{
					MyGridPhysicalHierarchy.Static.CreateLink(base.EntityId, base.CubeGrid, childNode);
				}
				if (Sync.IsServer)
				{
					m_lockModeSync.Value = LandingGearMode.Locked;
				}
				m_attachedTo = entity;
				m_attachedTo.OnPhysicsChanged += m_physicsChangedHandler;
				base.OnPhysicsChanged += m_physicsChangedHandler;
				if (CanAutoLock)
				{
					ResetAutolock();
				}
				OnConstraintAdded(GridLinkTypeEnum.Physical, entity);
				if (!m_needsToRetryLock)
				{
					StartSound(m_lockSound);
				}
				stateChanged?.Invoke(obj: true);
				return;
			}
			HkRigidBody rigidBody2 = topMostParent.Physics.RigidBody;
			if (rigidBody2 == null)
			{
				MyLog.Default.WriteLine("Rigid body null for Entity " + entity);
				return;
			}
			rigidBody2.Activate();
			base.CubeGrid.Physics.RigidBody.Activate();
			m_attachedTo = entity;
			m_attachedTo.OnPhysicsChanged += m_physicsChangedHandler;
			base.OnPhysicsChanged += m_physicsChangedHandler;
			Matrix identity = Matrix.Identity;
			identity.Translation = gearSpacePivot;
			if (Sync.IsServer)
			{
				HkFixedConstraintData hkFixedConstraintData = new HkFixedConstraintData();
				if (MyFakes.OVERRIDE_LANDING_GEAR_INERTIA)
				{
					hkFixedConstraintData.SetInertiaStabilizationFactor(MyFakes.LANDING_GEAR_INTERTIA);
				}
				hkFixedConstraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
				hkFixedConstraintData.SetInBodySpace(identity, otherBodySpacePivot, base.CubeGrid.Physics, entity.Physics as MyPhysicsBody);
				HkConstraintData constraintData = hkFixedConstraintData;
				if (MyFakes.LANDING_GEAR_BREAKABLE && BreakForce < 1E+08f)
				{
					HkBreakableConstraintData hkBreakableConstraintData = new HkBreakableConstraintData(hkFixedConstraintData);
					hkFixedConstraintData.Dispose();
					hkBreakableConstraintData.Threshold = BreakForce;
					hkBreakableConstraintData.ReapplyVelocityOnBreak = true;
					hkBreakableConstraintData.RemoveFromWorldOnBrake = true;
					constraintData = hkBreakableConstraintData;
				}
				m_constraint = new HkConstraint(base.CubeGrid.Physics.RigidBody, rigidBody2, constraintData);
				base.CubeGrid.Physics.AddConstraint(m_constraint);
				m_constraint.Enabled = true;
			}
			if (!m_needsToRetryLock)
			{
				StartSound(m_lockSound);
			}
			if (Sync.IsServer)
			{
				m_lockModeSync.Value = LandingGearMode.Locked;
			}
			if (CanAutoLock)
			{
				ResetAutolock();
			}
			OnConstraintAdded(GridLinkTypeEnum.Physical, entity);
			MyCubeGrid myCubeGrid2 = topMostParent as MyCubeGrid;
			if (myCubeGrid2 != null)
			{
				MyFixedGrids.Link(base.CubeGrid, myCubeGrid2, this);
				MyGridPhysicalHierarchy.Static.CreateLink(base.EntityId, base.CubeGrid, myCubeGrid2);
			}
			stateChanged.InvokeIfNotNull(arg1: true);
			if (entity is MyVoxelBase)
			{
				MyFixedGrids.MarkGridRoot(base.CubeGrid);
				MyGridPhysicalHierarchy.Static.UpdateRoot(base.CubeGrid);
				m_converted = true;
			}
		}

		private bool CanWeldTo(MyEntity entity, ref Matrix otherBodySpacePivot)
		{
			if (Sync.IsServer && BreakForce < 1E+08f)
			{
				return false;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				MyCubeBlock myCubeBlock = entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeGrid = myCubeBlock.CubeGrid;
				}
			}
			if (myCubeGrid != null)
			{
				myCubeGrid.FixTargetCube(out Vector3I cube, otherBodySpacePivot.Translation * myCubeGrid.GridSizeR);
				MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(cube);
				if (cubeBlock != null && cubeBlock.FatBlock is MyAirtightHangarDoor)
				{
					return false;
				}
			}
			else if (entity.Parent != null)
			{
				return false;
			}
			return true;
		}

		private bool AnyGearLockedToStatic()
		{
			foreach (MyCubeGrid groupNode in MyGridPhysicalHierarchy.Static.GetGroupNodes(base.CubeGrid))
			{
				foreach (MyLandingGear fatBlock in groupNode.GetFatBlocks<MyLandingGear>())
				{
					if (fatBlock.LockedToStatic)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void Detach()
		{
			if (base.CubeGrid.Physics == null || m_attachedTo == null)
			{
				return;
			}
			if (Sync.IsServer && m_attachedTo is MyCubeGrid)
			{
				(m_attachedTo as MyCubeGrid).OnGridSplit -= CubeGrid_OnGridSplit;
			}
			if (Sync.IsServer)
			{
				m_lockModeSync.Value = LandingGearMode.Unlocked;
			}
			VRage.ModAPI.IMyEntity attachedTo = m_attachedTo;
			if (m_attachedTo != null)
			{
				m_attachedTo.OnPhysicsChanged -= m_physicsChangedHandler;
			}
			base.OnPhysicsChanged -= m_physicsChangedHandler;
			m_attachedTo = null;
			m_attachedEntityId = null;
			if (MyFakes.WELD_LANDING_GEARS && MyWeldingGroups.Static.LinkExists(base.EntityId, base.CubeGrid, (MyEntity)attachedTo))
			{
				MyWeldingGroups.Static.BreakLink(base.EntityId, base.CubeGrid, (MyEntity)attachedTo);
			}
			else
			{
				if (m_constraint != null)
				{
					base.CubeGrid.Physics.RemoveConstraint(m_constraint);
					m_constraint.Dispose();
					m_constraint = null;
				}
				OnConstraintRemoved(GridLinkTypeEnum.NoContactDamage, attachedTo);
			}
			if (attachedTo is MyCubeGrid)
			{
				MyCubeGrid myCubeGrid = (MyCubeGrid)attachedTo;
				MyFixedGrids.BreakLink(base.CubeGrid, myCubeGrid, this);
				MyGridPhysicalHierarchy.Static.BreakLink(base.EntityId, base.CubeGrid, myCubeGrid);
				if (!myCubeGrid.IsStatic && myCubeGrid.Physics != null)
				{
					myCubeGrid.Physics.ForceActivate();
				}
			}
			if (!m_needsToRetryLock && !base.MarkedForClose)
			{
				StartSound(m_unlockSound);
			}
			OnConstraintRemoved(GridLinkTypeEnum.Physical, attachedTo);
			if (Sync.IsServer)
			{
				m_attachedState.Value = new State
				{
					OtherEntityId = null
				};
			}
			this.StateChanged.InvokeIfNotNull(arg1: false);
			if (attachedTo is MyVoxelBase && !base.CubeGrid.IsStatic)
			{
				m_converted = false;
				bool flag = false;
				foreach (MyLandingGear fatBlock in base.CubeGrid.GetFatBlocks<MyLandingGear>())
				{
					if (fatBlock.LockedToStatic)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					MyFixedGrids.UnmarkGridRoot(base.CubeGrid);
					MyGridPhysicalHierarchy.Static.UpdateRoot(base.CubeGrid);
				}
			}
		}

		private void PhysicsChanged(VRage.ModAPI.IMyEntity entity)
		{
			if (entity is MyVoxelBase && entity.Physics == null)
			{
				return;
			}
			if (entity.Physics == null)
			{
				if (LockMode == LandingGearMode.Locked && Sync.IsServer)
				{
					m_needsToRetryLock = true;
				}
				Detach();
			}
			else if (LockMode == LandingGearMode.Locked && Sync.IsServer)
			{
				m_needsToRetryLock = true;
			}
		}

		public void EnqueueRetryLock()
		{
			if (!m_needsToRetryLock && LockMode == LandingGearMode.Locked && Sync.IsServer)
			{
				m_needsToRetryLock = true;
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
		}

		[Event(null, 1111)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void ResetLockConstraint(bool locked, bool force = false)
		{
			if (base.CubeGrid == null || base.CubeGrid.Physics == null)
			{
				return;
			}
			Detach();
			if (locked)
			{
				Vector3D pivot;
				MyEntity myEntity = FindBody(out pivot);
				if (myEntity != null)
				{
					AttachEntity(pivot, myEntity, force);
				}
			}
			else if (Sync.IsServer)
			{
				m_lockModeSync.Value = LandingGearMode.Unlocked;
			}
			m_needsToRetryLock = false;
		}

		public void RequestLock(bool enable)
		{
			if (base.IsWorking)
			{
				MyMultiplayer.RaiseEvent(this, (MyLandingGear x) => x.AttachRequest, enable);
			}
		}

		private void StartSound(MySoundPair cueEnum)
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.PlaySound(cueEnum, stopPrevious: true);
			}
		}

		VRage.ModAPI.IMyEntity SpaceEngineers.Game.ModAPI.IMyLandingGear.GetAttachedEntity()
		{
			return m_attachedTo;
		}

		[Event(null, 1160)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void AttachRequest(bool enable)
		{
			if (enable)
			{
				if (MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.LandingGearLock, 0L, 0uL))
				{
					Vector3D pivot;
					MyEntity myEntity = FindBody(out pivot);
					if (myEntity != null)
					{
						AttachEntity(pivot, myEntity);
					}
					else
					{
						MyMultiplayer.RaiseEvent(this, (MyLandingGear x) => x.AttachFailed);
					}
				}
				return;
			}
			m_attachedState.Value = new State
			{
				OtherEntityId = null
			};
			ResetLockConstraint(locked: false);
			if (CanAutoLock)
			{
				ResetAutolock();
			}
			if (MyVisualScriptLogicProvider.LandingGearUnlocked != null)
			{
				MyVisualScriptLogicProvider.LandingGearUnlocked(base.EntityId, base.CubeGrid.EntityId, Name, base.CubeGrid.Name);
			}
		}

		private void AttachEntity(Vector3D pivot, MyEntity otherEntity, bool force = false)
		{
			Matrix rigidBodyMatrix = base.CubeGrid.Physics.RigidBody.GetRigidBodyMatrix();
			Matrix rigidBodyMatrix2 = otherEntity.Physics.RigidBody.GetRigidBodyMatrix();
			Matrix matrix = rigidBodyMatrix;
			matrix.Translation = base.CubeGrid.Physics.WorldToCluster(pivot);
			Vector3 translation = (matrix * Matrix.Invert(rigidBodyMatrix)).Translation;
			Matrix matrix2 = matrix * Matrix.Invert(rigidBodyMatrix2);
			CompressedPositionOrientation value = new CompressedPositionOrientation(ref matrix2);
			long entityId = otherEntity.EntityId;
			MatrixD matrix3 = base.CubeGrid.WorldMatrix * MatrixD.Invert(otherEntity.WorldMatrix);
			m_attachedState.Value = new State
			{
				Force = force,
				OtherEntityId = entityId,
				GearPivotPosition = translation,
				OtherPivot = value,
				MasterToSlave = matrix3
			};
			Attach(otherEntity, translation, matrix2);
		}

		private void AttachedValueChanged()
		{
			if (Sync.IsServer)
			{
				return;
			}
			State value = m_attachedState.Value;
			if (value.OtherEntityId.HasValue)
			{
				if (value.OtherEntityId.Value == m_attachedEntityId && !value.Force)
				{
					return;
				}
				m_needsToRetryLock = true;
				Detach();
				m_attachedEntityId = value.OtherEntityId.Value;
				if (MyEntities.TryGetEntityById(value.OtherEntityId.Value, out MyEntity entity))
				{
					if (entity is MyFloatingObject || base.CubeGrid.IsStatic)
					{
						entity.WorldMatrix = MatrixD.Multiply(MatrixD.Invert(value.MasterToSlave.Value), base.CubeGrid.WorldMatrix);
					}
					else
					{
						base.CubeGrid.WorldMatrix = MatrixD.Multiply(value.MasterToSlave.Value, entity.WorldMatrix);
					}
					Attach(entity, value.GearPivotPosition.Value, value.OtherPivot.Value.Matrix);
					m_needsToRetryLock = false;
				}
			}
			else
			{
				ResetLockConstraint(locked: false);
				if (CanAutoLock)
				{
					ResetAutolock();
				}
			}
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			if (Sync.IsServer)
			{
				base.CubeGrid.OnGridSplit -= CubeGrid_OnGridSplit;
				base.CubeGrid.OnIsStaticChanged -= CubeGrid_OnIsStaticChanged;
			}
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			if (!Sync.IsServer)
			{
				return;
			}
			if (m_attachedState.Value.OtherEntityId.HasValue)
			{
				if (base.CubeGrid.Physics == null)
				{
					m_needsToRetryLock = true;
				}
				else
				{
					RetryLockServer();
					State value = m_attachedState.Value;
					value.Force = true;
					m_attachedState.Value = value;
				}
			}
			base.CubeGrid.OnGridSplit += CubeGrid_OnGridSplit;
			base.CubeGrid.OnIsStaticChanged += CubeGrid_OnIsStaticChanged;
		}

		protected void CubeGrid_OnGridSplit(MyCubeGrid grid1, MyCubeGrid grid2)
		{
			ResetLockConstraint(locked: true, force: true);
		}

		protected void CubeGrid_OnIsStaticChanged(bool isStatic)
		{
			if (!isStatic && m_attachedTo is MyVoxelBase)
			{
				ResetLockConstraint(locked: false, force: true);
				m_needsToRetryLock = true;
			}
		}

		public VRage.ModAPI.IMyEntity GetAttachedEntity()
		{
			return m_attachedTo;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			LoadDummies();
		}

		private LockModeChangedHandler GetDelegate(Action<SpaceEngineers.Game.ModAPI.IMyLandingGear, LandingGearMode> value)
		{
			return (LockModeChangedHandler)Delegate.CreateDelegate(typeof(LockModeChangedHandler), value.Target, value.Method);
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.ToggleLock()
		{
			RequestLandingGearSwitch();
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.Lock()
		{
			RequestLandingGearLock();
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.Unlock()
		{
			RequestLandingGearUnlock();
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.ResetAutoLock()
		{
			if (CanAutoLock)
			{
				ResetAutolock();
			}
		}
	}
}
