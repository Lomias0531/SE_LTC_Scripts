using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Components
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	internal class MySessionComponentThrower : MySessionComponentBase
	{
		protected sealed class OnThrowMessageSuccess_003C_003EVRage_Game_MyObjectBuilder_CubeGrid_0023VRageMath_Vector3D_0023VRageMath_Vector3D_0023System_Single_0023VRage_Audio_MyCueId : ICallSite<IMyEventOwner, MyObjectBuilder_CubeGrid, Vector3D, Vector3D, float, MyCueId, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_CubeGrid grid, in Vector3D position, in Vector3D linearVelocity, in float mass, in MyCueId throwSound, in DBNull arg6)
			{
				OnThrowMessageSuccess(grid, position, linearVelocity, mass, throwSound);
			}
		}

		public static bool USE_SPECTATOR_FOR_THROW;

		private bool m_isActive;

		private int m_startTime;

		public static MySessionComponentThrower Static
		{
			get;
			set;
		}

		public bool Enabled
		{
			get
			{
				return m_isActive;
			}
			set
			{
				m_isActive = value;
			}
		}

		public MyPrefabThrowerDefinition CurrentDefinition
		{
			get;
			set;
		}

		public override bool IsRequiredByGame => false;

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyToolbarComponent)
		};

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
		}

		public override void HandleInput()
		{
			if (!m_isActive || !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay) || (!MyInput.Static.ENABLE_DEVELOPER_KEYS && MySession.Static.SurvivalMode && !MySession.Static.IsUserAdmin(Sync.MyId)))
			{
				return;
			}
			base.HandleInput();
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.PRIMARY_TOOL_ACTION))
			{
				m_startTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED))
			{
				return;
			}
			MyObjectBuilder_CubeGrid[] gridPrefab = MyPrefabManager.Static.GetGridPrefab(CurrentDefinition.PrefabToThrow);
			Vector3D zero = Vector3D.Zero;
			Vector3D zero2 = Vector3D.Zero;
			if (USE_SPECTATOR_FOR_THROW)
			{
				zero = MySpectator.Static.Position;
				zero2 = MySpectator.Static.Orientation.Forward;
			}
			else if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity)
			{
				if (MySession.Static.ControlledEntity == null)
				{
					return;
				}
				zero = MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true).Translation;
				zero2 = MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true).Forward;
			}
			else
			{
				zero = MySector.MainCamera.Position;
				zero2 = MySector.MainCamera.WorldMatrix.Forward;
			}
			Vector3D arg = zero + zero2;
			float value = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_startTime) / 1000f / CurrentDefinition.PushTime * CurrentDefinition.MaxSpeed;
			value = MathHelper.Clamp(value, CurrentDefinition.MinSpeed, CurrentDefinition.MaxSpeed);
			Vector3D arg2 = zero2 * value + MySession.Static.ControlledEntity.Entity.Physics.LinearVelocity;
			float arg3 = 0f;
			if (CurrentDefinition.Mass.HasValue)
			{
				arg3 = MyDestructionHelper.MassToHavok(CurrentDefinition.Mass.Value);
			}
			gridPrefab[0].EntityId = MyEntityIdentifier.AllocateId();
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnThrowMessageSuccess, gridPrefab[0], arg, arg2, arg3, CurrentDefinition.ThrowSound);
			m_startTime = 0;
		}

		public void Throw(MyObjectBuilder_CubeGrid grid, Vector3D position, Vector3D linearVelocity, float mass, MyCueId throwSound)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			MyEntity myEntity = MyEntities.CreateFromObjectBuilder(grid, fadeIn: false);
			if (myEntity == null)
			{
				return;
			}
			myEntity.PositionComp.SetPosition(position);
			myEntity.Physics.LinearVelocity = linearVelocity;
			if (mass > 0f)
			{
				myEntity.Physics.RigidBody.Mass = mass;
			}
			MyEntities.Add(myEntity);
			if (!throwSound.IsNull)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
				if (myEntity3DSoundEmitter != null)
				{
					myEntity3DSoundEmitter.SetPosition(position);
					myEntity3DSoundEmitter.PlaySoundWithDistance(throwSound);
				}
			}
		}

		public void Activate()
		{
			m_isActive = true;
		}

		public void Deactivate()
		{
			m_isActive = false;
		}

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
			MyToolbarComponent.CurrentToolbar.SelectedSlotChanged += CurrentToolbar_SelectedSlotChanged;
			MyToolbarComponent.CurrentToolbar.SlotActivated += CurrentToolbar_SlotActivated;
			MyToolbarComponent.CurrentToolbar.Unselected += CurrentToolbar_Unselected;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			if (MyToolbarComponent.CurrentToolbar != null)
			{
				MyToolbarComponent.CurrentToolbar.SelectedSlotChanged -= CurrentToolbar_SelectedSlotChanged;
				MyToolbarComponent.CurrentToolbar.SlotActivated -= CurrentToolbar_SlotActivated;
				MyToolbarComponent.CurrentToolbar.Unselected -= CurrentToolbar_Unselected;
			}
		}

		private void CurrentToolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
		{
			if (!(toolbar.SelectedItem is MyToolbarItemPrefabThrower))
			{
				Enabled = false;
			}
		}

		private void CurrentToolbar_SlotActivated(MyToolbar toolbar, MyToolbar.SlotArgs args, bool userActivated)
		{
			if (!(toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemPrefabThrower))
			{
				Enabled = false;
			}
		}

		private void CurrentToolbar_Unselected(MyToolbar toolbar)
		{
			Enabled = false;
		}

		[Event(null, 206)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void OnThrowMessageSuccess(MyObjectBuilder_CubeGrid grid, Vector3D position, Vector3D linearVelocity, float mass, MyCueId throwSound)
		{
			Static.Throw(grid, position, linearVelocity, mass, throwSound);
		}
	}
}
