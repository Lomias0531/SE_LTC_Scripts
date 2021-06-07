using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_DoorBase))]
	public abstract class MyDoorBase : MyFunctionalBlock
	{
		protected sealed class OpenRequest_003C_003ESystem_Boolean_0023System_Int64 : ICallSite<MyDoorBase, bool, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyDoorBase @this, in bool open, in long identityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OpenRequest(open, identityId);
			}
		}

		protected class m_open_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType open;
				ISyncType result = open = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyDoorBase)P_0).m_open = (Sync<bool, SyncDirection.BothWays>)open;
				return result;
			}
		}

		protected class m_anyoneCanUse_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType anyoneCanUse;
				ISyncType result = anyoneCanUse = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyDoorBase)P_0).m_anyoneCanUse = (Sync<bool, SyncDirection.BothWays>)anyoneCanUse;
				return result;
			}
		}

		protected readonly Sync<bool, SyncDirection.BothWays> m_open;

		private readonly Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;

		public bool Open
		{
			get
			{
				return m_open;
			}
			set
			{
				if ((bool)m_open != value && base.Enabled && base.IsWorking && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
				{
					m_open.Value = value;
				}
			}
		}

		public bool AnyoneCanUse
		{
			get
			{
				return m_anyoneCanUse;
			}
			set
			{
				m_anyoneCanUse.Value = value;
			}
		}

		public override MyCubeBlockHighlightModes HighlightMode
		{
			get
			{
				if (AnyoneCanUse)
				{
					return MyCubeBlockHighlightModes.AlwaysCanUse;
				}
				return MyCubeBlockHighlightModes.Default;
			}
		}

		public MyDoorBase()
		{
			CreateTerminalControls();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_DoorBase myObjectBuilder_DoorBase = objectBuilder as MyObjectBuilder_DoorBase;
			m_anyoneCanUse.SetLocalValue(myObjectBuilder_DoorBase.AnyoneCanUse);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_DoorBase obj = (MyObjectBuilder_DoorBase)base.GetObjectBuilderCubeBlock(copy);
			obj.AnyoneCanUse = m_anyoneCanUse.Value;
			return obj;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyDoorBase>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyDoorBase> obj = new MyTerminalControlOnOffSwitch<MyDoorBase>("Open", MySpaceTexts.Blank, default(MyStringId), MySpaceTexts.BlockAction_DoorOpen, MySpaceTexts.BlockAction_DoorClosed)
				{
					Getter = ((MyDoorBase x) => x.Open),
					Setter = delegate(MyDoorBase x, bool v)
					{
						x.SetOpenRequest(v, x.OwnerId);
					}
				};
				obj.EnableToggleAction();
				obj.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlCheckbox<MyDoorBase> obj2 = new MyTerminalControlCheckbox<MyDoorBase>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse)
				{
					Getter = ((MyDoorBase x) => x.AnyoneCanUse),
					Setter = delegate(MyDoorBase x, bool v)
					{
						x.AnyoneCanUse = v;
					}
				};
				obj2.EnableAction();
				MyTerminalControlFactory.AddControl(obj2);
			}
		}

		public void SetOpenRequest(bool open, long identityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyDoorBase x) => x.OpenRequest, open, identityId);
		}

		[Event(null, 112)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void OpenRequest(bool open, long identityId)
		{
			bool flag = AnyoneCanUse || HasPlayerAccess(identityId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			MyPlayer myPlayer = (myIdentity != null && myIdentity.Character != null) ? MyPlayer.GetPlayerFromCharacter(myIdentity.Character) : null;
			if (myPlayer != null && !flag && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out AdminSettingsEnum value))
			{
				flag = value.HasFlag(AdminSettingsEnum.UseTerminals);
			}
			if (flag)
			{
				Open = open;
			}
		}

		protected void CreateSubpartConstraint(MyEntity subpart, out HkFixedConstraintData constraintData, out HkConstraint constraint)
		{
			constraintData = null;
			constraint = null;
			if (base.CubeGrid.Physics != null)
			{
				uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(subpart.GetPhysicsBody().RigidBody.Layer, base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, 1, 1);
				subpart.Physics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
				subpart.Physics.Enabled = true;
				constraintData = new HkFixedConstraintData();
				constraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
				constraintData.SetInertiaStabilizationFactor(1f);
				constraint = new HkConstraint((base.CubeGrid.Physics.RigidBody2 != null && base.CubeGrid.Physics.Flags.HasFlag(RigidBodyFlag.RBF_DOUBLED_KINEMATIC)) ? base.CubeGrid.Physics.RigidBody2 : base.CubeGrid.Physics.RigidBody, subpart.Physics.RigidBody, constraintData);
				constraint.WantRuntime = true;
			}
		}

		protected void DisposeSubpartConstraint(ref HkConstraint constraint, ref HkFixedConstraintData constraintData)
		{
			if (!(constraint == null))
			{
				base.CubeGrid.Physics.RemoveConstraint(constraint);
				constraint.Dispose();
				constraint = null;
				constraintData = null;
			}
		}

		protected static void SetupDoorSubpart(MyEntitySubpart subpart, int havokCollisionSystemID, bool refreshInPlace)
		{
			if (subpart != null && subpart.Physics != null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
			{
				uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(subpart.GetPhysicsBody().RigidBody.Layer, havokCollisionSystemID, 1, 1);
				subpart.Physics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
				if (subpart.GetPhysicsBody().HavokWorld != null && refreshInPlace)
				{
					subpart.GetPhysicsBody().HavokWorld.RefreshCollisionFilterOnEntity(subpart.Physics.RigidBody);
				}
			}
		}
	}
}
