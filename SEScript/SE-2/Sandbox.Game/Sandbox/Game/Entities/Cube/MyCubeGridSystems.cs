using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Groups;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	public class MyCubeGridSystems
	{
		private readonly MyCubeGrid m_cubeGrid;

		private Action<MyBlockGroup> m_terminalSystem_GroupAdded;

		private Action<MyBlockGroup> m_terminalSystem_GroupRemoved;

		private bool m_blocksRegistered;

		private readonly HashSet<MyResourceSinkComponent> m_tmpSinks = new HashSet<MyResourceSinkComponent>();

		public Action<long, bool, string> GridPowerStateChanged;

		public const int PowerStateRequestPlayerId_Trash = -1;

		public const int PowerStateRequestPlayerId_SpecialContent = -2;

		public MyResourceDistributorComponent ResourceDistributor
		{
			get;
			private set;
		}

		public MyGridTerminalSystem TerminalSystem
		{
			get;
			private set;
		}

		public MyGridConveyorSystem ConveyorSystem
		{
			get;
			private set;
		}

		public MyGridGyroSystem GyroSystem
		{
			get;
			private set;
		}

		public MyGridWeaponSystem WeaponSystem
		{
			get;
			private set;
		}

		public MyGridReflectorLightSystem ReflectorLightSystem
		{
			get;
			private set;
		}

		public MyGridRadioSystem RadioSystem
		{
			get;
			private set;
		}

		public MyGridWheelSystem WheelSystem
		{
			get;
			private set;
		}

		public MyGridLandingSystem LandingSystem
		{
			get;
			private set;
		}

		public MyGroupControlSystem ControlSystem
		{
			get;
			private set;
		}

		public MyGridCameraSystem CameraSystem
		{
			get;
			private set;
		}

		public MyShipSoundComponent ShipSoundComponent
		{
			get;
			private set;
		}

		public MyGridOreDetectorSystem OreDetectorSystem
		{
			get;
			private set;
		}

		public MyGridGasSystem GasSystem
		{
			get;
			private set;
		}

		public MyGridJumpDriveSystem JumpSystem
		{
			get;
			private set;
		}

		protected MyCubeGrid CubeGrid => m_cubeGrid;

		public bool NeedsPerFrameUpdate
		{
			get
			{
				if (!TerminalSystem.NeedsHudUpdate && !ConveyorSystem.NeedsUpdateLines && !GyroSystem.IsDirty && !CameraSystem.NeedsPerFrameUpdate && !ControlSystem.NeedsPerFrameUpdate && (!MyPerGameSettings.EnableJumpDrive || !JumpSystem.NeedsPerFrameUpdate) && (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT || !WheelSystem.NeedsPerFrameUpdate) && (GasSystem == null || !MySession.Static.Settings.EnableOxygen || !MySession.Static.Settings.EnableOxygenPressurization || !GasSystem.NeedsPerFrameUpdate) && (ShipSoundComponent == null || !ShipSoundComponent.NeedsPerFrameUpdate) && !GyroSystem.NeedsPerFrameUpdate && (!CubeGrid.Components.TryGet(out MyEntityThrustComponent component) || component.ThrustCount <= 0 || !component.HasPower))
				{
					if (ResourceDistributor == null)
					{
						return false;
					}
					return ResourceDistributor.NeedsPerFrameUpdate;
				}
				return true;
			}
		}

		public bool NeedsPerFrameDraw => (byte)(0 | (CameraSystem.NeedsPerFrameUpdate ? 1 : 0)) != 0;

		public MyCubeGridSystems(MyCubeGrid grid)
		{
			m_cubeGrid = grid;
			m_terminalSystem_GroupAdded = TerminalSystem_GroupAdded;
			m_terminalSystem_GroupRemoved = TerminalSystem_GroupRemoved;
			GyroSystem = new MyGridGyroSystem(m_cubeGrid);
			WeaponSystem = new MyGridWeaponSystem();
			ReflectorLightSystem = new MyGridReflectorLightSystem(m_cubeGrid);
			RadioSystem = new MyGridRadioSystem(m_cubeGrid);
			if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
			{
				WheelSystem = new MyGridWheelSystem(m_cubeGrid);
			}
			ConveyorSystem = new MyGridConveyorSystem(m_cubeGrid);
			LandingSystem = new MyGridLandingSystem();
			ControlSystem = new MyGroupControlSystem();
			CameraSystem = new MyGridCameraSystem(m_cubeGrid);
			OreDetectorSystem = new MyGridOreDetectorSystem(m_cubeGrid);
			if (Sync.IsServer && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
			{
				GasSystem = new MyGridGasSystem(m_cubeGrid);
			}
			if (MyPerGameSettings.EnableJumpDrive)
			{
				JumpSystem = new MyGridJumpDriveSystem(m_cubeGrid);
			}
			if (MyPerGameSettings.EnableShipSoundSystem && (MyFakes.ENABLE_NEW_SMALL_SHIP_SOUNDS || MyFakes.ENABLE_NEW_LARGE_SHIP_SOUNDS) && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				ShipSoundComponent = new MyShipSoundComponent();
			}
			m_blocksRegistered = true;
		}

		public virtual void Init(MyObjectBuilder_CubeGrid builder)
		{
			MyEntityThrustComponent myEntityThrustComponent = CubeGrid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent != null)
			{
				myEntityThrustComponent.DampenersEnabled = builder.DampenersEnabled;
			}
			if (WheelSystem != null)
			{
				m_cubeGrid.SetHandbrakeRequest(builder.Handbrake);
			}
			if (GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
			{
				GasSystem.Init(builder.OxygenRooms);
			}
			if (ShipSoundComponent != null && !ShipSoundComponent.InitComponent(m_cubeGrid))
			{
				ShipSoundComponent.DestroyComponent();
				ShipSoundComponent = null;
			}
			if (MyPerGameSettings.EnableJumpDrive)
			{
				JumpSystem.Init(builder.JumpDriveDirection, builder.JumpRemainingTime);
			}
			CubeGrid.Components.Get<MyEntityThrustComponent>()?.MergeAllGroupsDirty();
		}

		public virtual void BeforeBlockDeserialization(MyObjectBuilder_CubeGrid builder)
		{
			ConveyorSystem.BeforeBlockDeserialization(builder.ConveyorLines);
		}

		public virtual void AfterBlockDeserialization()
		{
			ConveyorSystem.AfterBlockDeserialization();
			ConveyorSystem.ResourceSink.Update();
		}

		public void UpdateBeforeSimulation()
		{
			ControlSystem.UpdateBeforeSimulation();
			if (MyPerGameSettings.EnableJumpDrive)
			{
				JumpSystem.UpdateBeforeSimulation();
			}
			if (CubeGrid.Components.TryGet(out MyEntityThrustComponent component))
			{
				if (CubeGrid.Physics != null && !Sync.IsServer && CubeGrid.Physics.LinearVelocity.LengthSquared() < 1E-05f && CubeGrid.Physics.LastLinearVelocity.LengthSquared() >= 1E-05f)
				{
					component.MarkDirty();
				}
				component.UpdateBeforeSimulation(updateDampeners: true, ControlSystem.RelativeDampeningEntity);
			}
			if (GyroSystem.IsDirty || GyroSystem.NeedsPerFrameUpdate)
			{
				GyroSystem.UpdateBeforeSimulation();
			}
			if (TerminalSystem.NeedsHudUpdate)
			{
				TerminalSystem.UpdateHud();
			}
			else
			{
				TerminalSystem.IncrementHudLastUpdated();
			}
			if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
			{
				WheelSystem.UpdateBeforeSimulation();
			}
			CameraSystem.UpdateBeforeSimulation();
			if (GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
			{
				GasSystem.UpdateBeforeSimulation();
			}
			if (ShipSoundComponent != null)
			{
				ShipSoundComponent.Update();
			}
			UpdatePower();
		}

		public virtual void PrepareForDraw()
		{
			CameraSystem.PrepareForDraw();
		}

		public void UpdatePower()
		{
			if (ResourceDistributor != null)
			{
				ResourceDistributor.UpdateBeforeSimulation();
			}
		}

		public virtual void UpdateOnceBeforeFrame()
		{
		}

		public virtual void UpdateBeforeSimulation10()
		{
			CameraSystem.UpdateBeforeSimulation10();
			ConveyorSystem.UpdateBeforeSimulation10();
		}

		public virtual void UpdateBeforeSimulation100()
		{
			if (ControlSystem != null)
			{
				ControlSystem.UpdateBeforeSimulation100();
			}
			if (GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
			{
				GasSystem.UpdateBeforeSimulation100();
			}
			if (ShipSoundComponent != null)
			{
				ShipSoundComponent.Update100();
			}
			if (ResourceDistributor != null)
			{
				ResourceDistributor.UpdateBeforeSimulation100();
			}
		}

		public virtual void UpdateAfterSimulation()
		{
			ConveyorSystem.UpdateAfterSimulation();
		}

		public virtual void UpdateAfterSimulation100()
		{
			ConveyorSystem.UpdateAfterSimulation100();
		}

		public virtual void GetObjectBuilder(MyObjectBuilder_CubeGrid ob)
		{
			ob.DampenersEnabled = (CubeGrid.Components.Get<MyEntityThrustComponent>()?.DampenersEnabled ?? true);
			ConveyorSystem.SerializeLines(ob.ConveyorLines);
			if (ob.ConveyorLines.Count == 0)
			{
				ob.ConveyorLines = null;
			}
			if (WheelSystem != null)
			{
				ob.Handbrake = WheelSystem.HandBrake;
			}
			if (GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
			{
				ob.OxygenRooms = GasSystem.GetOxygenAmount();
			}
			if (MyPerGameSettings.EnableJumpDrive)
			{
				ob.JumpDriveDirection = JumpSystem.GetJumpDriveDirection();
				ob.JumpRemainingTime = JumpSystem.GetRemainingJumpTime();
			}
		}

		public virtual void AddGroup(MyBlockGroup group)
		{
			if (TerminalSystem != null)
			{
				TerminalSystem.AddUpdateGroup(group, fireEvent: false);
			}
		}

		public virtual void RemoveGroup(MyBlockGroup group)
		{
			if (TerminalSystem != null)
			{
				TerminalSystem.RemoveGroup(group, fireEvent: false);
			}
		}

		public virtual void OnAddedToGroup(MyGridLogicalGroupData group)
		{
			TerminalSystem = group.TerminalSystem;
			ResourceDistributor = group.ResourceDistributor;
			WeaponSystem = group.WeaponSystem;
			if (string.IsNullOrEmpty(ResourceDistributor.DebugName))
			{
				ResourceDistributor.DebugName = m_cubeGrid.ToString();
			}
			m_cubeGrid.OnFatBlockAdded += ResourceDistributor.CubeGrid_OnFatBlockAddedOrRemoved;
			m_cubeGrid.OnFatBlockRemoved += ResourceDistributor.CubeGrid_OnFatBlockAddedOrRemoved;
			ResourceDistributor.AddSink(GyroSystem.ResourceSink);
			ResourceDistributor.AddSink(ConveyorSystem.ResourceSink);
			ConveyorSystem.ResourceSink.IsPoweredChanged += ResourceDistributor.ConveyorSystem_OnPoweredChanged;
			foreach (MyBlockGroup blockGroup in m_cubeGrid.BlockGroups)
			{
				TerminalSystem.AddUpdateGroup(blockGroup, fireEvent: false);
			}
			TerminalSystem.GroupAdded += m_terminalSystem_GroupAdded;
			TerminalSystem.GroupRemoved += m_terminalSystem_GroupRemoved;
			foreach (MyCubeBlock fatBlock in m_cubeGrid.GetFatBlocks())
			{
				if (!fatBlock.MarkedForClose)
				{
					MyTerminalBlock myTerminalBlock = fatBlock as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						TerminalSystem.Add(myTerminalBlock);
					}
					MyResourceSourceComponent myResourceSourceComponent = fatBlock.Components.Get<MyResourceSourceComponent>();
					if (myResourceSourceComponent != null)
					{
						ResourceDistributor.AddSource(myResourceSourceComponent);
					}
					MyResourceSinkComponent myResourceSinkComponent = fatBlock.Components.Get<MyResourceSinkComponent>();
					if (myResourceSinkComponent != null)
					{
						ResourceDistributor.AddSink(myResourceSinkComponent);
					}
					IMyRechargeSocketOwner myRechargeSocketOwner = fatBlock as IMyRechargeSocketOwner;
					if (myRechargeSocketOwner != null)
					{
						myRechargeSocketOwner.RechargeSocket.ResourceDistributor = group.ResourceDistributor;
					}
					IMyGunObject<MyDeviceBase> myGunObject = fatBlock as IMyGunObject<MyDeviceBase>;
					if (myGunObject != null)
					{
						WeaponSystem.Register(myGunObject);
					}
				}
			}
			MyResourceDistributorComponent resourceDistributor = ResourceDistributor;
			resourceDistributor.OnPowerGenerationChanged = (Action<bool>)Delegate.Combine(resourceDistributor.OnPowerGenerationChanged, new Action<bool>(OnPowerGenerationChanged));
			TerminalSystem.BlockManipulationFinishedFunction();
			ResourceDistributor.UpdateBeforeSimulation();
		}

		public virtual void OnRemovedFromGroup(MyGridLogicalGroupData group)
		{
			if (m_blocksRegistered)
			{
				TerminalSystem.GroupAdded -= m_terminalSystem_GroupAdded;
				TerminalSystem.GroupRemoved -= m_terminalSystem_GroupRemoved;
				foreach (MyBlockGroup blockGroup in m_cubeGrid.BlockGroups)
				{
					TerminalSystem.RemoveGroup(blockGroup, fireEvent: false);
				}
				foreach (MyCubeBlock fatBlock in m_cubeGrid.GetFatBlocks())
				{
					MyTerminalBlock myTerminalBlock = fatBlock as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						TerminalSystem.Remove(myTerminalBlock);
					}
					MyResourceSourceComponent myResourceSourceComponent = fatBlock.Components.Get<MyResourceSourceComponent>();
					if (myResourceSourceComponent != null)
					{
						ResourceDistributor.RemoveSource(myResourceSourceComponent);
					}
					MyResourceSinkComponent myResourceSinkComponent = fatBlock.Components.Get<MyResourceSinkComponent>();
					if (myResourceSinkComponent != null)
					{
						ResourceDistributor.RemoveSink(myResourceSinkComponent, resetSinkInput: false, fatBlock.MarkedForClose);
					}
					IMyRechargeSocketOwner myRechargeSocketOwner = fatBlock as IMyRechargeSocketOwner;
					if (myRechargeSocketOwner != null)
					{
						myRechargeSocketOwner.RechargeSocket.ResourceDistributor = null;
					}
					IMyGunObject<MyDeviceBase> myGunObject = fatBlock as IMyGunObject<MyDeviceBase>;
					if (myGunObject != null)
					{
						WeaponSystem.Unregister(myGunObject);
					}
				}
				TerminalSystem.BlockManipulationFinishedFunction();
			}
			ConveyorSystem.ResourceSink.IsPoweredChanged -= ResourceDistributor.ConveyorSystem_OnPoweredChanged;
			group.ResourceDistributor.RemoveSink(ConveyorSystem.ResourceSink, resetSinkInput: false);
			group.ResourceDistributor.RemoveSink(GyroSystem.ResourceSink, resetSinkInput: false);
			group.ResourceDistributor.UpdateBeforeSimulation();
			m_cubeGrid.OnFatBlockAdded -= ResourceDistributor.CubeGrid_OnFatBlockAddedOrRemoved;
			m_cubeGrid.OnFatBlockRemoved -= ResourceDistributor.CubeGrid_OnFatBlockAddedOrRemoved;
			MyResourceDistributorComponent resourceDistributor = ResourceDistributor;
			resourceDistributor.OnPowerGenerationChanged = (Action<bool>)Delegate.Remove(resourceDistributor.OnPowerGenerationChanged, new Action<bool>(OnPowerGenerationChanged));
			ResourceDistributor = null;
			TerminalSystem = null;
			WeaponSystem = null;
		}

		private void OnPowerGenerationChanged(bool powerIsGenerated)
		{
			if (MyVisualScriptLogicProvider.GridPowerGenerationStateChanged != null)
			{
				MyVisualScriptLogicProvider.GridPowerGenerationStateChanged(CubeGrid.EntityId, CubeGrid.Name, powerIsGenerated);
			}
			if (GridPowerStateChanged != null)
			{
				GridPowerStateChanged(CubeGrid.EntityId, powerIsGenerated, CubeGrid.Name);
			}
		}

		public void OnAddedToGroup(MyGridPhysicalGroupData group)
		{
			ControlSystem = group.ControlSystem;
			foreach (MyShipController fatBlock in m_cubeGrid.GetFatBlocks<MyShipController>())
			{
				if (fatBlock != null && (fatBlock.ControllerInfo.Controller != null || (fatBlock.Pilot != null && MySessionComponentReplay.Static.HasEntityReplayData(CubeGrid.EntityId))) && fatBlock.EnableShipControl && (!(fatBlock is MyCockpit) || fatBlock.IsMainCockpit || !fatBlock.CubeGrid.HasMainCockpit()))
				{
					ControlSystem.AddControllerBlock(fatBlock);
				}
			}
			ControlSystem.AddGrid(CubeGrid);
		}

		public void OnRemovedFromGroup(MyGridPhysicalGroupData group)
		{
			ControlSystem.RemoveGrid(CubeGrid);
			if (m_blocksRegistered)
			{
				foreach (MyShipController fatBlock in m_cubeGrid.GetFatBlocks<MyShipController>())
				{
					if (fatBlock != null && fatBlock.ControllerInfo.Controller != null && fatBlock.EnableShipControl && (!(fatBlock is MyCockpit) || fatBlock.IsMainCockpit || !fatBlock.CubeGrid.HasMainCockpit()))
					{
						ControlSystem.RemoveControllerBlock(fatBlock);
					}
				}
			}
			ControlSystem = null;
		}

		public virtual void BeforeGridClose()
		{
			ConveyorSystem.IsClosing = true;
			ReflectorLightSystem.IsClosing = true;
			RadioSystem.IsClosing = true;
			if (ShipSoundComponent != null)
			{
				ShipSoundComponent.DestroyComponent();
				ShipSoundComponent = null;
			}
			if (GasSystem != null)
			{
				GasSystem.OnGridClosing();
			}
		}

		public virtual void AfterGridClose()
		{
			ConveyorSystem.AfterGridClose();
			if (MyPerGameSettings.EnableJumpDrive)
			{
				JumpSystem.AfterGridClose();
			}
			m_blocksRegistered = false;
			GasSystem = null;
		}

		public virtual void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_TERMINAL_SYSTEMS)
			{
				MyRenderProxy.DebugDrawText3D(m_cubeGrid.WorldMatrix.Translation, TerminalSystem.GetHashCode().ToString(), Color.NavajoWhite, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS)
			{
				ConveyorSystem.DebugDraw(m_cubeGrid);
				ConveyorSystem.DebugDrawLinePackets();
			}
			if (GyroSystem != null && MyDebugDrawSettings.DEBUG_DRAW_GYROS)
			{
				GyroSystem.DebugDraw();
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS && ResourceDistributor != null)
			{
				ResourceDistributor.DebugDraw(m_cubeGrid);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS && TerminalSystem != null)
			{
				TerminalSystem.DebugDraw(m_cubeGrid);
			}
			if (MySession.Static != null && GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization && MyDebugDrawSettings.DEBUG_DRAW_OXYGEN)
			{
				GasSystem.DebugDraw();
			}
		}

		public virtual bool IsTrash()
		{
			if (ResourceDistributor.ResourceState != MyResourceStateEnum.NoPower)
			{
				return false;
			}
			if (ControlSystem.IsControlled)
			{
				return false;
			}
			return true;
		}

		public virtual void RegisterInSystems(MyCubeBlock block)
		{
			if (block.GetType() != typeof(MyCubeBlock))
			{
				if (ResourceDistributor != null)
				{
					MyResourceSourceComponent myResourceSourceComponent = block.Components.Get<MyResourceSourceComponent>();
					if (myResourceSourceComponent != null)
					{
						ResourceDistributor.AddSource(myResourceSourceComponent);
					}
					MyResourceSinkComponent myResourceSinkComponent = block.Components.Get<MyResourceSinkComponent>();
					if (!(block is MyThrust) && myResourceSinkComponent != null)
					{
						ResourceDistributor.AddSink(myResourceSinkComponent);
					}
					IMyRechargeSocketOwner myRechargeSocketOwner = block as IMyRechargeSocketOwner;
					if (myRechargeSocketOwner != null)
					{
						myRechargeSocketOwner.RechargeSocket.ResourceDistributor = ResourceDistributor;
					}
				}
				if (WeaponSystem != null)
				{
					IMyGunObject<MyDeviceBase> myGunObject = block as IMyGunObject<MyDeviceBase>;
					if (myGunObject != null)
					{
						WeaponSystem.Register(myGunObject);
					}
				}
				if (TerminalSystem != null)
				{
					MyTerminalBlock myTerminalBlock = block as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						TerminalSystem.Add(myTerminalBlock);
					}
				}
				MyCubeBlock myCubeBlock = (block != null && block.HasInventory) ? block : null;
				if (myCubeBlock != null)
				{
					ConveyorSystem.Add(myCubeBlock);
				}
				IMyConveyorEndpointBlock myConveyorEndpointBlock = block as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock != null)
				{
					myConveyorEndpointBlock.InitializeConveyorEndpoint();
					ConveyorSystem.AddConveyorBlock(myConveyorEndpointBlock);
				}
				IMyConveyorSegmentBlock myConveyorSegmentBlock = block as IMyConveyorSegmentBlock;
				if (myConveyorSegmentBlock != null)
				{
					myConveyorSegmentBlock.InitializeConveyorSegment();
					ConveyorSystem.AddSegmentBlock(myConveyorSegmentBlock);
				}
				MyReflectorLight myReflectorLight = block as MyReflectorLight;
				if (myReflectorLight != null)
				{
					ReflectorLightSystem.Register(myReflectorLight);
				}
				if (block.Components.Contains(typeof(MyDataBroadcaster)))
				{
					MyDataBroadcaster broadcaster = block.Components.Get<MyDataBroadcaster>();
					RadioSystem.Register(broadcaster);
				}
				if (block.Components.Contains(typeof(MyDataReceiver)))
				{
					MyDataReceiver reciever = block.Components.Get<MyDataReceiver>();
					RadioSystem.Register(reciever);
				}
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
				{
					MyMotorSuspension myMotorSuspension = block as MyMotorSuspension;
					if (myMotorSuspension != null)
					{
						WheelSystem.Register(myMotorSuspension);
					}
				}
				IMyLandingGear myLandingGear = block as IMyLandingGear;
				if (myLandingGear != null)
				{
					LandingSystem.Register(myLandingGear);
				}
				MyGyro myGyro = block as MyGyro;
				if (myGyro != null)
				{
					GyroSystem.Register(myGyro);
				}
				MyCameraBlock myCameraBlock = block as MyCameraBlock;
				if (myCameraBlock != null)
				{
					CameraSystem.Register(myCameraBlock);
				}
			}
			block.OnRegisteredToGridSystems();
		}

		public virtual void UnregisterFromSystems(MyCubeBlock block)
		{
			if (block.GetType() != typeof(MyCubeBlock))
			{
				if (ResourceDistributor != null)
				{
					MyResourceSourceComponent myResourceSourceComponent = block.Components.Get<MyResourceSourceComponent>();
					if (myResourceSourceComponent != null)
					{
						ResourceDistributor.RemoveSource(myResourceSourceComponent);
					}
					MyResourceSinkComponent myResourceSinkComponent = block.Components.Get<MyResourceSinkComponent>();
					if (myResourceSinkComponent != null)
					{
						ResourceDistributor.RemoveSink(myResourceSinkComponent);
					}
					IMyRechargeSocketOwner myRechargeSocketOwner = block as IMyRechargeSocketOwner;
					if (myRechargeSocketOwner != null)
					{
						myRechargeSocketOwner.RechargeSocket.ResourceDistributor = null;
					}
				}
				if (WeaponSystem != null)
				{
					IMyGunObject<MyDeviceBase> myGunObject = block as IMyGunObject<MyDeviceBase>;
					if (myGunObject != null)
					{
						WeaponSystem.Unregister(myGunObject);
					}
				}
				if (TerminalSystem != null)
				{
					MyTerminalBlock myTerminalBlock = block as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						TerminalSystem.Remove(myTerminalBlock);
					}
				}
				if (block.HasInventory)
				{
					ConveyorSystem.Remove(block);
				}
				IMyConveyorEndpointBlock myConveyorEndpointBlock = block as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock != null)
				{
					ConveyorSystem.RemoveConveyorBlock(myConveyorEndpointBlock);
				}
				IMyConveyorSegmentBlock myConveyorSegmentBlock = block as IMyConveyorSegmentBlock;
				if (myConveyorSegmentBlock != null)
				{
					ConveyorSystem.RemoveSegmentBlock(myConveyorSegmentBlock);
				}
				MyReflectorLight myReflectorLight = block as MyReflectorLight;
				if (myReflectorLight != null)
				{
					ReflectorLightSystem.Unregister(myReflectorLight);
				}
				MyDataBroadcaster myDataBroadcaster = block.Components.Get<MyDataBroadcaster>();
				if (myDataBroadcaster != null)
				{
					RadioSystem.Unregister(myDataBroadcaster);
				}
				MyDataReceiver myDataReceiver = block.Components.Get<MyDataReceiver>();
				if (myDataReceiver != null)
				{
					RadioSystem.Unregister(myDataReceiver);
				}
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
				{
					MyMotorSuspension myMotorSuspension = block as MyMotorSuspension;
					if (myMotorSuspension != null)
					{
						WheelSystem.Unregister(myMotorSuspension);
					}
				}
				IMyLandingGear myLandingGear = block as IMyLandingGear;
				if (myLandingGear != null)
				{
					LandingSystem.Unregister(myLandingGear);
				}
				MyGyro myGyro = block as MyGyro;
				if (myGyro != null)
				{
					GyroSystem.Unregister(myGyro);
				}
				MyCameraBlock myCameraBlock = block as MyCameraBlock;
				if (myCameraBlock != null)
				{
					CameraSystem.Unregister(myCameraBlock);
				}
			}
			block.OnUnregisteredFromGridSystems();
		}

		public void SyncObject_PowerProducerStateChanged(MyMultipleEnabledEnum enabledState, long playerId)
		{
			if (Sync.IsServer)
			{
				MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(CubeGrid);
				if (group != null)
				{
					foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
					{
						MyCubeGrid nodeData = node.NodeData;
						if (nodeData != null && nodeData.Physics != null && nodeData.Physics.Shape != null)
						{
							foreach (MyCubeBlock fatBlock in nodeData.GetFatBlocks())
							{
								IMyPowerProducer myPowerProducer = fatBlock as IMyPowerProducer;
								if (myPowerProducer != null)
								{
									bool flag = false;
									if (playerId >= 0)
									{
										MyFunctionalBlock myFunctionalBlock = fatBlock as MyFunctionalBlock;
										if (myFunctionalBlock != null && myFunctionalBlock.HasPlayerAccess(playerId))
										{
											flag = true;
										}
									}
									else
									{
										switch (playerId)
										{
										case -1L:
											flag = true;
											break;
										case -2L:
										{
											string a = (fatBlock as MyTerminalBlock).CustomName.ToString();
											if (a == "Special Content Power" || a == "Special Content")
											{
												flag = true;
											}
											break;
										}
										}
									}
									if (flag)
									{
										myPowerProducer.Enabled = (enabledState == MyMultipleEnabledEnum.AllEnabled);
									}
								}
							}
						}
					}
				}
			}
			if (ResourceDistributor != null)
			{
				ResourceDistributor.ChangeSourcesState(MyResourceDistributorComponent.ElectricityId, enabledState, playerId);
			}
			CubeGrid.ActivatePhysics();
		}

		private void TerminalSystem_GroupRemoved(MyBlockGroup group)
		{
			MyBlockGroup myBlockGroup = m_cubeGrid.BlockGroups.Find((MyBlockGroup x) => x.Name.CompareTo(group.Name) == 0);
			if (myBlockGroup != null)
			{
				myBlockGroup.Blocks.Clear();
				m_cubeGrid.BlockGroups.Remove(myBlockGroup);
				m_cubeGrid.ModifyGroup(myBlockGroup);
			}
		}

		private void TerminalSystem_GroupAdded(MyBlockGroup group)
		{
			MyBlockGroup myBlockGroup = m_cubeGrid.BlockGroups.Find((MyBlockGroup x) => x.Name.CompareTo(group.Name) == 0);
			if (group.Blocks.FirstOrDefault((MyTerminalBlock x) => m_cubeGrid.GetFatBlocks().IndexOf(x) != -1) != null)
			{
				if (myBlockGroup == null)
				{
					myBlockGroup = new MyBlockGroup();
					myBlockGroup.Name.AppendStringBuilder(group.Name);
					m_cubeGrid.BlockGroups.Add(myBlockGroup);
				}
				myBlockGroup.Blocks.Clear();
				foreach (MyTerminalBlock block in group.Blocks)
				{
					if (block.CubeGrid == m_cubeGrid)
					{
						myBlockGroup.Blocks.Add(block);
					}
				}
				m_cubeGrid.ModifyGroup(myBlockGroup);
			}
		}

		public virtual void OnBlockAdded(MySlimBlock block)
		{
			if (ShipSoundComponent != null && block.FatBlock is MyThrust)
			{
				ShipSoundComponent.ShipHasChanged = true;
			}
			if (ConveyorSystem != null)
			{
				ConveyorSystem.UpdateLines();
			}
		}

		public virtual void OnBlockRemoved(MySlimBlock block)
		{
			if (ShipSoundComponent != null && block.FatBlock is MyThrust)
			{
				ShipSoundComponent.ShipHasChanged = true;
			}
			if (ConveyorSystem != null)
			{
				ConveyorSystem.UpdateLines();
			}
		}

		public virtual void OnBlockIntegrityChanged(MySlimBlock block)
		{
		}

		public virtual void OnBlockOwnershipChanged(MyCubeGrid cubeGrid)
		{
			ConveyorSystem.FlagForRecomputation();
		}
	}
}
