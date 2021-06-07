using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyMechanicalConnectionBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock)
	})]
	public abstract class MyMechanicalConnectionBlockBase : MyFunctionalBlock, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock
	{
		[Serializable]
		protected struct State
		{
			protected class Sandbox_Game_Entities_Blocks_MyMechanicalConnectionBlockBase_003C_003EState_003C_003ETopBlockId_003C_003EAccessor : IMemberAccessor<State, long?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in long? value)
				{
					owner.TopBlockId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out long? value)
				{
					value = owner.TopBlockId;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyMechanicalConnectionBlockBase_003C_003EState_003C_003EWelded_003C_003EAccessor : IMemberAccessor<State, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in bool value)
				{
					owner.Welded = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out bool value)
				{
					value = owner.Welded;
				}
			}

			public long? TopBlockId;

			public bool Welded;
		}

		protected sealed class FindAndAttachTopServer_003C_003E : ICallSite<MyMechanicalConnectionBlockBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyMechanicalConnectionBlockBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.FindAndAttachTopServer();
			}
		}

		protected sealed class DoRecreateTop_003C_003ESystem_Int64_0023System_Boolean_0023System_Boolean : ICallSite<MyMechanicalConnectionBlockBase, long, bool, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyMechanicalConnectionBlockBase @this, in long builderId, in bool smallToLarge, in bool instantBuild, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DoRecreateTop(builderId, smallToLarge, instantBuild);
			}
		}

		protected sealed class NotifyTopPartFailed_003C_003ESandbox_Game_World_MySession_003C_003ELimitResult : ICallSite<MyMechanicalConnectionBlockBase, MySession.LimitResult, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyMechanicalConnectionBlockBase @this, in MySession.LimitResult result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.NotifyTopPartFailed(result);
			}
		}

		protected sealed class DetachRequest_003C_003E : ICallSite<MyMechanicalConnectionBlockBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyMechanicalConnectionBlockBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DetachRequest();
			}
		}

		protected class m_connectionState_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType connectionState;
				ISyncType result = connectionState = new Sync<State, SyncDirection.FromServer>(P_1, P_2);
				((MyMechanicalConnectionBlockBase)P_0).m_connectionState = (Sync<State, SyncDirection.FromServer>)connectionState;
				return result;
			}
		}

		protected class m_forceWeld_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType forceWeld;
				ISyncType result = forceWeld = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMechanicalConnectionBlockBase)P_0).m_forceWeld = (Sync<bool, SyncDirection.BothWays>)forceWeld;
				return result;
			}
		}

		protected class m_weldSpeed_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType weldSpeed;
				ISyncType result = weldSpeed = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMechanicalConnectionBlockBase)P_0).m_weldSpeed = (Sync<float, SyncDirection.BothWays>)weldSpeed;
				return result;
			}
		}

		protected class m_shareInertiaTensor_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType shareInertiaTensor;
				ISyncType result = shareInertiaTensor = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyMechanicalConnectionBlockBase)P_0).m_shareInertiaTensor = (Sync<bool, SyncDirection.BothWays>)shareInertiaTensor;
				return result;
			}
		}

		protected class m_safetyDetach_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType safetyDetach;
				ISyncType result = safetyDetach = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyMechanicalConnectionBlockBase)P_0).m_safetyDetach = (Sync<float, SyncDirection.BothWays>)safetyDetach;
				return result;
			}
		}

		protected readonly Sync<State, SyncDirection.FromServer> m_connectionState;

		protected Sync<bool, SyncDirection.BothWays> m_forceWeld;

		protected Sync<float, SyncDirection.BothWays> m_weldSpeed;

		private float m_weldSpeedSq;

		private float m_unweldSpeedSq;

		private Sync<bool, SyncDirection.BothWays> m_shareInertiaTensor;

		private MyAttachableTopBlockBase m_topBlock;

		private readonly Sync<float, SyncDirection.BothWays> m_safetyDetach;

		protected static List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();

		protected static HashSet<MySlimBlock> m_tmpSet = new HashSet<MySlimBlock>();

		protected HkConstraint m_constraint;

		private bool m_needReattach;

		private bool m_updateAttach;

		protected bool SupportsSharedInertiaTensor = MySession.Static != null && MySession.Static.IsSettingsExperimental();

		private bool ShareInertiaTensor
		{
			get
			{
				return m_shareInertiaTensor;
			}
			set
			{
				if (SupportsSharedInertiaTensor)
				{
					m_shareInertiaTensor.Value = value;
				}
			}
		}

		public MyCubeGrid TopGrid
		{
			get
			{
				if (TopBlock == null)
				{
					return null;
				}
				return TopBlock.CubeGrid;
			}
		}

		public MyAttachableTopBlockBase TopBlock => m_topBlock;

		protected bool m_isWelding
		{
			get;
			private set;
		}

		protected bool m_welded
		{
			get;
			private set;
		}

		protected bool m_isAttached
		{
			get;
			private set;
		}

		public float SafetyDetach
		{
			get
			{
				return m_safetyDetach.Value;
			}
			set
			{
				m_safetyDetach.Value = value;
			}
		}

		private new MyMechanicalConnectionBlockBaseDefinition BlockDefinition => (MyMechanicalConnectionBlockBaseDefinition)base.BlockDefinition;

		protected HkConstraint SafeConstraint
		{
			get
			{
				RefreshConstraint();
				return m_constraint;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.IsAttached => m_isAttached;

		float Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.SafetyLockSpeed
		{
			get
			{
				return m_weldSpeed;
			}
			set
			{
				value = MathHelper.Clamp(value, 0f, (base.CubeGrid.GridSizeEnum == MyCubeSize.Large) ? MyGridPhysics.LargeShipMaxLinearVelocity() : MyGridPhysics.SmallShipMaxLinearVelocity());
				m_weldSpeed.Value = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.SafetyLock
		{
			get
			{
				if (!m_isWelding)
				{
					return m_welded;
				}
				return true;
			}
			set
			{
				if ((m_isWelding || m_welded) != value)
				{
					m_forceWeld.Value = value;
				}
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.IsLocked
		{
			get
			{
				if (!m_isWelding)
				{
					return m_welded;
				}
				return true;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.PendingAttachment
		{
			get
			{
				if (m_connectionState.Value.TopBlockId.HasValue)
				{
					return m_connectionState.Value.TopBlockId.Value == 0;
				}
				return false;
			}
		}

		VRage.Game.ModAPI.IMyCubeGrid Sandbox.ModAPI.IMyMechanicalConnectionBlock.TopGrid => TopGrid;

		Sandbox.ModAPI.IMyAttachableTopBlock Sandbox.ModAPI.IMyMechanicalConnectionBlock.Top => TopBlock;

		VRage.Game.ModAPI.Ingame.IMyCubeGrid Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.TopGrid => TopGrid;

		Sandbox.ModAPI.Ingame.IMyAttachableTopBlock Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.Top => TopBlock;

		protected event Action<MyMechanicalConnectionBlockBase> AttachedEntityChanged;

		public MyMechanicalConnectionBlockBase()
		{
			m_connectionState.ValueChanged += delegate
			{
				OnAttachTargetChanged();
			};
			m_connectionState.Validate = ValidateTopBlockId;
			CreateTerminalControls();
			m_updateAttach = true;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyMechanicalConnectionBlockBase>())
			{
				base.CreateTerminalControls();
				MyTerminalControlSlider<MyMechanicalConnectionBlockBase> myTerminalControlSlider = new MyTerminalControlSlider<MyMechanicalConnectionBlockBase>("Weld speed", MySpaceTexts.BlockPropertyTitle_WeldSpeed, MySpaceTexts.Blank);
				myTerminalControlSlider.SetLimits((MyMechanicalConnectionBlockBase block) => 0f, (MyMechanicalConnectionBlockBase block) => MyGridPhysics.SmallShipMaxLinearVelocity());
				myTerminalControlSlider.DefaultValueGetter = ((MyMechanicalConnectionBlockBase block) => MyGridPhysics.LargeShipMaxLinearVelocity() - 5f);
				myTerminalControlSlider.Visible = ((MyMechanicalConnectionBlockBase x) => false);
				myTerminalControlSlider.Getter = ((MyMechanicalConnectionBlockBase x) => x.m_weldSpeed);
				myTerminalControlSlider.Setter = delegate(MyMechanicalConnectionBlockBase x, float v)
				{
					x.m_weldSpeed.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyMechanicalConnectionBlockBase x, StringBuilder res)
				{
					res.AppendDecimal((float)Math.Sqrt(x.m_weldSpeedSq), 1).Append("m/s");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase> obj = new MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase>("Force weld", MySpaceTexts.BlockPropertyTitle_WeldForce, MySpaceTexts.Blank)
				{
					Visible = ((MyMechanicalConnectionBlockBase x) => false),
					Getter = ((MyMechanicalConnectionBlockBase x) => x.m_forceWeld),
					Setter = delegate(MyMechanicalConnectionBlockBase x, bool v)
					{
						x.m_forceWeld.Value = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlSlider<MyMechanicalConnectionBlockBase> myTerminalControlSlider2 = new MyTerminalControlSlider<MyMechanicalConnectionBlockBase>("SafetyDetach", MySpaceTexts.BlockPropertyTitle_SafetyDetach, MySpaceTexts.BlockPropertyTooltip_SafetyDetach);
				myTerminalControlSlider2.Getter = ((MyMechanicalConnectionBlockBase x) => x.SafetyDetach);
				myTerminalControlSlider2.Setter = delegate(MyMechanicalConnectionBlockBase x, float v)
				{
					x.SafetyDetach = v;
				};
				myTerminalControlSlider2.DefaultValueGetter = ((MyMechanicalConnectionBlockBase x) => 5f);
				myTerminalControlSlider2.SetLimits((MyMechanicalConnectionBlockBase x) => x.BlockDefinition.SafetyDetachMin, (MyMechanicalConnectionBlockBase x) => x.BlockDefinition.SafetyDetachMax);
				myTerminalControlSlider2.Writer = delegate(MyMechanicalConnectionBlockBase x, StringBuilder result)
				{
					MyValueFormatter.AppendDistanceInBestUnit(x.SafetyDetach, result);
				};
				myTerminalControlSlider2.Enabled = ((MyMechanicalConnectionBlockBase b) => b.m_isAttached);
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				if (MySandboxGame.Config.ExperimentalMode)
				{
					MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase> sharedInertiaTensor = new MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase>("ShareInertiaTensor", MySpaceTexts.BlockPropertyTitle_ShareTensor, MySpaceTexts.BlockPropertyTooltip_ShareTensor);
					sharedInertiaTensor.Enabled = ((MyMechanicalConnectionBlockBase x) => x.SupportsSharedInertiaTensor);
					sharedInertiaTensor.Visible = ((MyMechanicalConnectionBlockBase x) => x.SupportsSharedInertiaTensor);
					sharedInertiaTensor.Getter = ((MyMechanicalConnectionBlockBase x) => SetTensorUIColor(x, x.ShareInertiaTensor, sharedInertiaTensor));
					sharedInertiaTensor.Setter = delegate(MyMechanicalConnectionBlockBase x, bool v)
					{
						x.ShareInertiaTensor = SetTensorUIColor(x, v, sharedInertiaTensor);
					};
					sharedInertiaTensor.EnableAction();
					MyTerminalControlFactory.AddControl(sharedInertiaTensor);
				}
			}
		}

		private static bool SetTensorUIColor(MyMechanicalConnectionBlockBase block, bool isUnsafeValue, MyTerminalControlCheckbox<MyMechanicalConnectionBlockBase> control)
		{
			if (!Sync.IsDedicated)
			{
				Vector4 colorMask = control.GetGuiControl().ColorMask;
				if (isUnsafeValue)
				{
					colorMask = Color.Red;
				}
				control.GetGuiControl().Elements[0].ColorMask = colorMask;
			}
			return isUnsafeValue;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_MechanicalConnectionBlock myObjectBuilder_MechanicalConnectionBlock = objectBuilder as MyObjectBuilder_MechanicalConnectionBlock;
			m_weldSpeed.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MechanicalConnectionBlock.WeldSpeed, 0f, MyGridPhysics.SmallShipMaxLinearVelocity()));
			m_forceWeld.SetLocalValue(myObjectBuilder_MechanicalConnectionBlock.ForceWeld);
			if (myObjectBuilder_MechanicalConnectionBlock.TopBlockId.HasValue && myObjectBuilder_MechanicalConnectionBlock.TopBlockId.Value != 0L)
			{
				m_connectionState.SetLocalValue(new State
				{
					TopBlockId = myObjectBuilder_MechanicalConnectionBlock.TopBlockId,
					Welded = (myObjectBuilder_MechanicalConnectionBlock.IsWelded || myObjectBuilder_MechanicalConnectionBlock.ForceWeld)
				});
			}
			if (!SupportsSharedInertiaTensor)
			{
				myObjectBuilder_MechanicalConnectionBlock.ShareInertiaTensor = false;
			}
			m_shareInertiaTensor.SetLocalValue(myObjectBuilder_MechanicalConnectionBlock.ShareInertiaTensor);
			m_shareInertiaTensor.ValueChanged += ShareInertiaTensor_ValueChanged;
			m_safetyDetach.SetLocalValue(MathHelper.Clamp(myObjectBuilder_MechanicalConnectionBlock.SafetyDetach ?? BlockDefinition.SafetyDetach, BlockDefinition.SafetyDetachMin, BlockDefinition.SafetyDetachMax + 0.1f));
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
		}

		private void ShareInertiaTensor_ValueChanged(SyncBase obj)
		{
			UpdateSharedTensorState();
			OnUnsafeSettingsChanged();
		}

		private void UpdateSharedTensorState()
		{
			if (ShareInertiaTensor)
			{
				if (TopGrid != null)
				{
					MySharedTensorsGroups.Link(base.CubeGrid, TopGrid, this);
				}
			}
			else
			{
				MySharedTensorsGroups.BreakLinkIfExists(base.CubeGrid, TopGrid, this);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_MechanicalConnectionBlock obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_MechanicalConnectionBlock;
			obj.WeldSpeed = m_weldSpeed;
			obj.ForceWeld = m_forceWeld;
			obj.TopBlockId = m_connectionState.Value.TopBlockId;
			obj.IsWelded = m_connectionState.Value.Welded;
			obj.SafetyDetach = SafetyDetach;
			obj.ShareInertiaTensor = ShareInertiaTensor;
			return obj;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (m_updateAttach)
			{
				UpdateAttachState();
			}
			if (m_needReattach)
			{
				Reattach(TopGrid);
			}
			OnUnsafeSettingsChanged();
			bool flag = base.CubeGrid != null && base.CubeGrid.IsPreview;
			if ((m_updateAttach || m_needReattach) && !flag)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			CheckSafetyDetach();
		}

		private bool ValidateTopBlockId(State newState)
		{
			if (!newState.TopBlockId.HasValue)
			{
				return true;
			}
			if (newState.TopBlockId == 0)
			{
				return !m_connectionState.Value.TopBlockId.HasValue;
			}
			return false;
		}

		private void WeldSpeed_ValueChanged(SyncBase obj)
		{
			m_weldSpeedSq = (float)m_weldSpeed * (float)m_weldSpeed;
			m_unweldSpeedSq = Math.Max((float)m_weldSpeed - 2f, 0f);
			m_unweldSpeedSq *= m_unweldSpeedSq;
		}

		private void OnForceWeldChanged()
		{
			if (!m_isAttached || !Sync.IsServer)
			{
				return;
			}
			if ((bool)m_forceWeld)
			{
				if (!m_welded)
				{
					WeldGroup(force: true);
					SetDetailedInfoDirty();
				}
			}
			else
			{
				RaisePropertiesChanged();
			}
		}

		private void OnAttachTargetChanged()
		{
			m_updateAttach = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected void CheckSafetyDetach()
		{
			if (!Sync.IsServer || m_constraint == null)
			{
				return;
			}
			if (!base.CubeGrid.Physics.IsActive)
			{
				MyGridPhysics physics = TopGrid.Physics;
				if (physics != null && !physics.IsActive)
				{
					return;
				}
			}
			float safetyDetach = SafetyDetach;
			if (GetConstraintDisplacementSq() > safetyDetach * safetyDetach)
			{
				Detach();
			}
		}

		protected virtual float GetConstraintDisplacementSq()
		{
			m_constraint.GetPivotsInWorld(out Vector3 pivotA, out Vector3 pivotB);
			return (pivotA - pivotB).LengthSquared();
		}

		private void WeldGroup(bool force)
		{
			if (MyFakes.WELD_ROTORS)
			{
				m_isWelding = true;
				DisposeConstraint(TopGrid);
				MyWeldingGroups.Static.CreateLink(base.EntityId, base.CubeGrid, TopGrid);
				if (Sync.IsServer)
				{
					_ = TopBlock.CubeGrid.WorldMatrix * MatrixD.Invert(base.WorldMatrix);
					m_connectionState.Value = new State
					{
						TopBlockId = TopBlock.EntityId,
						Welded = true
					};
				}
				m_welded = true;
				m_isWelding = false;
				RaisePropertiesChanged();
			}
		}

		private void UnweldGroup(MyCubeGrid topGrid)
		{
			if (m_welded)
			{
				m_isWelding = true;
				MyWeldingGroups.Static.BreakLink(base.EntityId, base.CubeGrid, topGrid);
				if (Sync.IsServer)
				{
					m_connectionState.Value = new State
					{
						TopBlockId = TopBlock.EntityId,
						Welded = false
					};
				}
				m_welded = false;
				m_isWelding = false;
				RaisePropertiesChanged();
			}
		}

		private void cubeGrid_OnPhysicsChanged(MyEntity obj)
		{
			if (MyEntities.IsClosingAll)
			{
				return;
			}
			cubeGrid_OnPhysicsChanged();
			if (TopGrid == null || base.CubeGrid == null)
			{
				return;
			}
			if (MyCubeGridGroups.Static.Logical.GetGroup(TopBlock.CubeGrid) != MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid))
			{
				m_needReattach = true;
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			else
			{
				if (TopGrid.Physics == null || base.CubeGrid.Physics == null)
				{
					return;
				}
				if (m_constraint != null)
				{
					if ((!(m_constraint.RigidBodyA == base.CubeGrid.Physics.RigidBody) || !(m_constraint.RigidBodyB == TopGrid.Physics.RigidBody)) && (!(m_constraint.RigidBodyA == TopGrid.Physics.RigidBody) || !(m_constraint.RigidBodyB == base.CubeGrid.Physics.RigidBody)))
					{
						m_needReattach = true;
						base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					}
				}
				else if (!m_welded)
				{
					m_needReattach = (TopGrid.Physics.RigidBody != base.CubeGrid.Physics.RigidBody);
					if (m_needReattach)
					{
						base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					}
				}
			}
		}

		protected virtual void cubeGrid_OnPhysicsChanged()
		{
		}

		private void TopBlock_OnClosing(MyEntity obj)
		{
			Detach();
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			if (Sync.IsServer && m_isAttached)
			{
				m_needReattach = false;
				State value = m_connectionState.Value;
				Detach();
				m_connectionState.Value = value;
			}
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			m_updateAttach = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void RaiseAttachedEntityChanged()
		{
			if (this.AttachedEntityChanged != null)
			{
				this.AttachedEntityChanged(this);
			}
		}

		public virtual void OnTopBlockCubeGridChanged(MyCubeGrid oldGrid)
		{
			MyAttachableTopBlockBase topBlock = TopBlock;
			Detach(oldGrid, updateGroups: true);
			m_connectionState.SetLocalValue(new State
			{
				TopBlockId = topBlock.EntityId,
				Welded = m_welded
			});
			MarkForReattach();
		}

		protected virtual void Detach(MyCubeGrid topGrid, bool updateGroups)
		{
			if (base.CubeGrid.Physics != null)
			{
				base.CubeGrid.Physics.AddDirtyBlock(SlimBlock);
			}
			if (m_welded)
			{
				UnweldGroup(topGrid);
			}
			if (updateGroups && !MyEntities.IsClosingAll)
			{
				m_needReattach = false;
				BreakLinks(topGrid, TopBlock);
				if (Sync.IsServer)
				{
					m_connectionState.Value = new State
					{
						TopBlockId = null,
						Welded = false
					};
				}
			}
			DisposeConstraint(topGrid);
			if (TopBlock != null)
			{
				TopBlock.Detach(isWelding: false);
			}
			m_topBlock = null;
			m_isAttached = false;
			if (!MyEntities.IsClosingAll)
			{
				SetDetailedInfoDirty();
			}
			if (updateGroups && !MyEntities.IsClosingAll)
			{
				RaiseAttachedEntityChanged();
			}
		}

		protected virtual void Detach(bool updateGroups = true)
		{
			Detach(TopGrid, updateGroups);
		}

		protected virtual void DisposeConstraint(MyCubeGrid topGrid)
		{
			MySharedTensorsGroups.BreakLinkIfExists(base.CubeGrid, topGrid, this);
		}

		protected virtual bool CreateConstraint(MyAttachableTopBlockBase top)
		{
			if (CanAttach(top) && !m_welded && base.CubeGrid.Physics.RigidBody != top.CubeGrid.Physics.RigidBody)
			{
				UpdateSharedTensorState();
				return true;
			}
			return false;
		}

		protected virtual bool Attach(MyAttachableTopBlockBase topBlock, bool updateGroup = true)
		{
			if (topBlock.CubeGrid.Physics == null)
			{
				return false;
			}
			if (base.CubeGrid.Physics == null || !base.CubeGrid.Physics.Enabled)
			{
				return false;
			}
			m_topBlock = topBlock;
			TopBlock.Attach(this);
			if (updateGroup)
			{
				base.CubeGrid.OnPhysicsChanged += cubeGrid_OnPhysicsChanged;
				TopGrid.OnPhysicsChanged += cubeGrid_OnPhysicsChanged;
				TopBlock.OnClosing += TopBlock_OnClosing;
				if (base.CubeGrid != topBlock.CubeGrid)
				{
					OnConstraintAdded(GridLinkTypeEnum.Physical, TopGrid);
					OnConstraintAdded(GridLinkTypeEnum.Logical, TopGrid);
					OnConstraintAdded(GridLinkTypeEnum.Mechanical, TopGrid);
					MyGridPhysicalHierarchy.Static.CreateLink(base.EntityId, base.CubeGrid, TopGrid);
				}
				RaiseAttachedEntityChanged();
			}
			if (Sync.IsServer)
			{
				_ = TopBlock.CubeGrid.WorldMatrix * MatrixD.Invert(base.WorldMatrix);
				m_connectionState.Value = new State
				{
					TopBlockId = TopBlock.EntityId,
					Welded = m_welded
				};
			}
			m_isAttached = true;
			return true;
		}

		[Event(null, 666)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		protected void FindAndAttachTopServer()
		{
			MyAttachableTopBlockBase myAttachableTopBlockBase = FindMatchingTop();
			if (myAttachableTopBlockBase != null)
			{
				TryAttach(myAttachableTopBlockBase);
			}
		}

		private void UpdateAttachState()
		{
			m_updateAttach = false;
			m_needReattach = false;
			State state = m_connectionState.Value;
			if (!state.TopBlockId.HasValue)
			{
				if (m_isAttached)
				{
					Detach();
				}
			}
			else if (m_connectionState.Value.TopBlockId == 0)
			{
				if (m_isAttached)
				{
					Detach();
				}
				if (Sync.IsServer)
				{
					FindAndAttachTopServer();
				}
			}
			else if (TopBlock == null || TopBlock.EntityId != m_connectionState.Value.TopBlockId)
			{
				state = m_connectionState.Value;
				long value = state.TopBlockId.Value;
				if (TopBlock != null)
				{
					Detach();
				}
				if (!MyEntities.TryGetEntityById(value, out MyAttachableTopBlockBase entity) || !TryAttach(entity))
				{
					if (Sync.IsServer && (entity == null || entity.MarkedForClose))
					{
						state = (m_connectionState.Value = new State
						{
							TopBlockId = null
						});
					}
					else
					{
						m_updateAttach = true;
						base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					}
				}
			}
			else if (m_welded != m_connectionState.Value.Welded)
			{
				if (m_connectionState.Value.Welded)
				{
					WeldGroup(force: true);
				}
				else
				{
					UnweldGroup(TopGrid);
				}
			}
			RefreshConstraint();
		}

		private bool TryAttach(MyAttachableTopBlockBase top, bool updateGroup = true)
		{
			if (CanAttach(top))
			{
				return Attach(top, updateGroup);
			}
			return false;
		}

		private bool CanAttach(MyAttachableTopBlockBase top)
		{
			if (base.MarkedForClose || base.CubeGrid.MarkedForClose)
			{
				return false;
			}
			if (top.MarkedForClose || top.CubeGrid.MarkedForClose)
			{
				return false;
			}
			if (base.CubeGrid.Physics == null || base.CubeGrid.Physics.RigidBody == null || !base.CubeGrid.Physics.RigidBody.InWorld)
			{
				return false;
			}
			if (top.CubeGrid.Physics == null || top.CubeGrid.Physics.RigidBody == null || !top.CubeGrid.Physics.RigidBody.InWorld)
			{
				return false;
			}
			if (top.CubeGrid.Physics.HavokWorld != base.CubeGrid.Physics.HavokWorld)
			{
				return false;
			}
			return true;
		}

		private bool CanPlaceTop()
		{
			ComputeTopQueryBox(out Vector3D pos, out Vector3 halfExtents, out Quaternion orientation);
			using (MyUtils.ReuseCollection(ref m_penetrations))
			{
				MyPhysics.GetPenetrationsBox(ref halfExtents, ref pos, ref orientation, m_penetrations, 15);
				foreach (HkBodyCollision penetration in m_penetrations)
				{
					VRage.ModAPI.IMyEntity collisionEntity = penetration.GetCollisionEntity();
					if (collisionEntity != null && collisionEntity != base.CubeGrid)
					{
						MyCubeGrid myCubeGrid = collisionEntity as MyCubeGrid;
						if (myCubeGrid != null)
						{
							Vector3D vector3D = TransformPosition(ref pos);
							Vector3I? vector3I = myCubeGrid.RayCastBlocks(vector3D, vector3D + base.WorldMatrix.Up);
							if (vector3I.HasValue)
							{
								return myCubeGrid.GetCubeBlock(vector3I.Value) == null;
							}
						}
					}
				}
			}
			return true;
		}

		private MyAttachableTopBlockBase FindMatchingTop()
		{
			ComputeTopQueryBox(out Vector3D pos, out Vector3 halfExtents, out Quaternion orientation);
			using (MyUtils.ReuseCollection(ref m_penetrations))
			{
				MyPhysics.GetPenetrationsBox(ref halfExtents, ref pos, ref orientation, m_penetrations, 15);
				foreach (HkBodyCollision penetration in m_penetrations)
				{
					VRage.ModAPI.IMyEntity collisionEntity = penetration.GetCollisionEntity();
					if (collisionEntity != null && collisionEntity != base.CubeGrid)
					{
						MyAttachableTopBlockBase myAttachableTopBlockBase = FindTopInGrid(collisionEntity, pos);
						if (myAttachableTopBlockBase != null)
						{
							return myAttachableTopBlockBase;
						}
						MyPhysicsBody myPhysicsBody = collisionEntity.Physics as MyPhysicsBody;
						if (myPhysicsBody != null)
						{
							foreach (MyPhysicsBody child in myPhysicsBody.WeldInfo.Children)
							{
								myAttachableTopBlockBase = FindTopInGrid(child.Entity, pos);
								if (myAttachableTopBlockBase != null)
								{
									return myAttachableTopBlockBase;
								}
							}
						}
					}
				}
			}
			return null;
		}

		private MyAttachableTopBlockBase FindTopInGrid(VRage.ModAPI.IMyEntity entity, Vector3D pos)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				Vector3D vector3D = TransformPosition(ref pos);
				Vector3I? vector3I = myCubeGrid.RayCastBlocks(vector3D, vector3D + base.WorldMatrix.Up);
				if (vector3I.HasValue)
				{
					MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(vector3I.Value);
					if (cubeBlock != null && cubeBlock.FatBlock != null)
					{
						return cubeBlock.FatBlock as MyAttachableTopBlockBase;
					}
				}
			}
			return null;
		}

		protected virtual Vector3D TransformPosition(ref Vector3D position)
		{
			return position;
		}

		public abstract void ComputeTopQueryBox(out Vector3D pos, out Vector3 halfExtents, out Quaternion orientation);

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (m_isAttached)
			{
				m_needReattach = false;
				State value = m_connectionState.Value;
				Detach();
				if (Sync.IsServer)
				{
					m_connectionState.Value = value;
				}
			}
			m_shareInertiaTensor.ValueChanged -= ShareInertiaTensor_ValueChanged;
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.OnBuildSuccess(builtBy, instantBuild);
			if (Sync.IsServer)
			{
				CreateTopPartAndAttach(builtBy, smallToLarge: false, instantBuild);
			}
		}

		protected void RecreateTop(long? builderId = null, bool smallToLarge = false, bool instantBuild = false)
		{
			long num = builderId.HasValue ? builderId.Value : MySession.Static.LocalPlayerId;
			if (m_isAttached || !CanPlaceTop())
			{
				if (num == MySession.Static.LocalPlayerId)
				{
					MyHud.Notifications.Add(MyNotificationSingletons.HeadAlreadyExists);
				}
				return;
			}
			MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = MyDefinitionManager.Static.TryGetDefinitionGroup(BlockDefinition.TopPart);
			MyCubeSize myCubeSize = base.CubeGrid.GridSizeEnum;
			if (smallToLarge && myCubeSize == MyCubeSize.Large)
			{
				myCubeSize = MyCubeSize.Small;
			}
			MyCubeBlockDefinition myCubeBlockDefinition = myCubeBlockDefinitionGroup[myCubeSize];
			bool flag = MySession.Static.CreativeToolsEnabled(MySession.Static.Players.TryGetSteamId(num));
			if (MySession.Static.CheckLimitsAndNotify(num, myCubeBlockDefinition.BlockPairName, flag ? myCubeBlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST, 1))
			{
				MyMultiplayer.RaiseEvent(this, (MyMechanicalConnectionBlockBase x) => x.DoRecreateTop, num, smallToLarge, instantBuild);
			}
		}

		[Event(null, 943)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void DoRecreateTop(long builderId, bool smallToLarge, bool instantBuild)
		{
			if (TopBlock == null)
			{
				CreateTopPartAndAttach(builderId, smallToLarge, instantBuild);
			}
		}

		private void Reattach(MyCubeGrid topGrid)
		{
			m_needReattach = false;
			if (TopBlock == null)
			{
				MyLog.Default.WriteLine("TopBlock null in MechanicalConnection.Reatach");
				m_updateAttach = true;
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				return;
			}
			bool flag = MyCubeGridGroups.Static.Logical.GetGroup(topGrid) != MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid);
			MyAttachableTopBlockBase topBlock = TopBlock;
			Detach(topGrid, flag);
			if (TryAttach(topBlock, flag))
			{
				if (topBlock.CubeGrid.Physics != null)
				{
					topBlock.CubeGrid.Physics.ForceActivate();
				}
				return;
			}
			if (!flag)
			{
				BreakLinks(topGrid, topBlock);
				RaiseAttachedEntityChanged();
			}
			if (Sync.IsServer)
			{
				m_connectionState.Value = new State
				{
					TopBlockId = 0L
				};
			}
		}

		[Event(null, 991)]
		[Reliable]
		[Client]
		private void NotifyTopPartFailed(MySession.LimitResult result)
		{
			if (result != 0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				MyHud.Notifications.Add(MySession.GetNotificationForLimitResult(result));
			}
		}

		private void BreakLinks(MyCubeGrid topGrid, MyAttachableTopBlockBase topBlock)
		{
			if (base.CubeGrid != TopGrid)
			{
				OnConstraintRemoved(GridLinkTypeEnum.Physical, topGrid);
				OnConstraintRemoved(GridLinkTypeEnum.Logical, topGrid);
				OnConstraintRemoved(GridLinkTypeEnum.Mechanical, topGrid);
				MyGridPhysicalHierarchy.Static.BreakLink(base.EntityId, base.CubeGrid, topGrid);
			}
			if (base.CubeGrid != null)
			{
				base.CubeGrid.OnPhysicsChanged -= cubeGrid_OnPhysicsChanged;
			}
			if (topGrid != null)
			{
				topGrid.OnPhysicsChanged -= cubeGrid_OnPhysicsChanged;
			}
			if (topBlock != null)
			{
				topBlock.OnClosing -= TopBlock_OnClosing;
			}
		}

		private void CreateTopPartAndAttach(long builtBy, bool smallToLarge, bool instantBuild)
		{
			CreateTopPart(out MyAttachableTopBlockBase topBlock, builtBy, MyDefinitionManager.Static.TryGetDefinitionGroup(BlockDefinition.TopPart), smallToLarge, instantBuild);
			if (topBlock != null)
			{
				Attach(topBlock);
			}
		}

		protected virtual bool CanPlaceRotor(MyAttachableTopBlockBase rotorBlock, long builtBy)
		{
			return true;
		}

		private void RefreshConstraint()
		{
			if (m_welded)
			{
				if (m_constraint != null)
				{
					DisposeConstraint(TopGrid);
				}
				return;
			}
			bool flag = m_constraint == null;
			if (m_constraint != null && !m_constraint.InWorld)
			{
				DisposeConstraint(TopGrid);
				flag = true;
			}
			if (flag && TopBlock != null)
			{
				CreateConstraint(TopBlock);
				RaisePropertiesChanged();
			}
		}

		private void CreateTopPart(out MyAttachableTopBlockBase topBlock, long builtBy, MyCubeBlockDefinitionGroup topGroup, bool smallToLarge, bool instantBuild)
		{
			if (topGroup == null)
			{
				topBlock = null;
				return;
			}
			MyCubeSize myCubeSize = base.CubeGrid.GridSizeEnum;
			if (smallToLarge && myCubeSize == MyCubeSize.Large)
			{
				myCubeSize = MyCubeSize.Small;
			}
			MatrixD topGridMatrix = GetTopGridMatrix();
			MyCubeBlockDefinition myCubeBlockDefinition = topGroup[myCubeSize];
			ulong num = MySession.Static.Players.TryGetSteamId(builtBy);
			bool flag = MySession.Static.CreativeToolsEnabled(num);
			string failedBlockType = string.Empty;
			MySession.LimitResult limitResult = MySession.Static.IsWithinWorldLimits(out failedBlockType, builtBy, myCubeBlockDefinition.BlockPairName, flag ? myCubeBlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST, 1);
			if (limitResult != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyMechanicalConnectionBlockBase x) => x.NotifyTopPartFailed, limitResult, new EndpointId(num));
				topBlock = null;
				return;
			}
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyCubeGrid.CreateBlockObjectBuilder(myCubeBlockDefinition, Vector3I.Zero, MyBlockOrientation.Identity, MyEntityIdentifier.AllocateId(), base.BuiltBy, MySession.Static.CreativeMode || instantBuild);
			if (myCubeBlockDefinition.Center != Vector3.Zero)
			{
				topGridMatrix.Translation = Vector3D.Transform(-myCubeBlockDefinition.Center * MyDefinitionManager.Static.GetCubeSize(myCubeSize), topGridMatrix);
			}
			MyObjectBuilder_AttachableTopBlockBase myObjectBuilder_AttachableTopBlockBase = myObjectBuilder_CubeBlock as MyObjectBuilder_AttachableTopBlockBase;
			if (myObjectBuilder_AttachableTopBlockBase != null)
			{
				myObjectBuilder_AttachableTopBlockBase.YieldLastComponent = false;
			}
			MyObjectBuilder_Wheel myObjectBuilder_Wheel = myObjectBuilder_CubeBlock as MyObjectBuilder_Wheel;
			if (myObjectBuilder_Wheel != null)
			{
				myObjectBuilder_Wheel.YieldLastComponent = false;
			}
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
			myObjectBuilder_CubeGrid.GridSizeEnum = myCubeSize;
			myObjectBuilder_CubeGrid.IsStatic = false;
			myObjectBuilder_CubeGrid.PositionAndOrientation = new MyPositionAndOrientation(topGridMatrix);
			myObjectBuilder_CubeGrid.CubeBlocks.Add(myObjectBuilder_CubeBlock);
			MyCubeGrid myCubeGrid = MyEntityFactory.CreateEntity<MyCubeGrid>(myObjectBuilder_CubeGrid);
			myCubeGrid.Init(myObjectBuilder_CubeGrid);
			topBlock = (MyAttachableTopBlockBase)myCubeGrid.GetCubeBlock(Vector3I.Zero).FatBlock;
			if (!CanPlaceTop(topBlock, builtBy))
			{
				topBlock = null;
				myCubeGrid.Close();
				return;
			}
			MyEntities.Add(myCubeGrid);
			_ = topBlock.CubeGrid.WorldMatrix * MatrixD.Invert(base.WorldMatrix);
			if (Sync.IsServer)
			{
				m_connectionState.Value = new State
				{
					TopBlockId = topBlock.EntityId
				};
			}
		}

		protected abstract MatrixD GetTopGridMatrix();

		protected virtual bool CanPlaceTop(MyAttachableTopBlockBase topBlock, long builtBy)
		{
			return true;
		}

		public MyStringId GetAttachState()
		{
			if (m_welded || m_isWelding)
			{
				return MySpaceTexts.BlockPropertiesText_MotorLocked;
			}
			if (!m_connectionState.Value.TopBlockId.HasValue)
			{
				return MySpaceTexts.BlockPropertiesText_MotorDetached;
			}
			if (m_connectionState.Value.TopBlockId.Value == 0L)
			{
				return MySpaceTexts.BlockPropertiesText_MotorAttachingAny;
			}
			if (m_isAttached)
			{
				return MySpaceTexts.BlockPropertiesText_MotorAttached;
			}
			return MySpaceTexts.BlockPropertiesText_MotorAttachingSpecific;
		}

		protected void CallDetach()
		{
			if (m_isAttached)
			{
				MyMultiplayer.RaiseEvent(this, (MyMechanicalConnectionBlockBase x) => x.DetachRequest);
			}
		}

		protected void CallAttach()
		{
			if (!m_isAttached)
			{
				MyMultiplayer.RaiseEvent(this, (MyMechanicalConnectionBlockBase x) => x.FindAndAttachTopServer);
			}
		}

		public virtual Vector3? GetConstraintPosition(MyCubeGrid grid, bool opposite = false)
		{
			return null;
		}

		public void MarkForReattach()
		{
			m_needReattach = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override bool HasUnsafeSettingsCollector()
		{
			if (!ShareInertiaTensor)
			{
				return base.HasUnsafeSettingsCollector();
			}
			return true;
		}

		[Event(null, 1200)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		protected void DetachRequest()
		{
			Detach();
		}

		void Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.Attach()
		{
			State value = m_connectionState.Value;
			if (!value.TopBlockId.HasValue)
			{
				Sync<State, SyncDirection.FromServer> connectionState = m_connectionState;
				value = new State
				{
					TopBlockId = 0L
				};
				connectionState.Value = value;
			}
		}

		void Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.Detach()
		{
			m_connectionState.Value = new State
			{
				TopBlockId = null
			};
		}

		void Sandbox.ModAPI.IMyMechanicalConnectionBlock.Attach(Sandbox.ModAPI.IMyAttachableTopBlock top, bool updateGroup)
		{
			if (top != null)
			{
				Attach((MyAttachableTopBlockBase)top, updateGroup);
			}
		}
	}
}
