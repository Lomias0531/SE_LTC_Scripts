using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_JumpDrive))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyJumpDrive),
		typeof(Sandbox.ModAPI.Ingame.IMyJumpDrive)
	})]
	public class MyJumpDrive : MyFunctionalBlock, Sandbox.ModAPI.IMyJumpDrive, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyJumpDrive
	{
		protected class m_storedPower_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType storedPower;
				ISyncType result = storedPower = new Sync<float, SyncDirection.FromServer>(P_1, P_2);
				((MyJumpDrive)P_0).m_storedPower = (Sync<float, SyncDirection.FromServer>)storedPower;
				return result;
			}
		}

		protected class m_targetSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetSync;
				ISyncType result = targetSync = new Sync<int?, SyncDirection.BothWays>(P_1, P_2);
				((MyJumpDrive)P_0).m_targetSync = (Sync<int?, SyncDirection.BothWays>)targetSync;
				return result;
			}
		}

		protected class m_jumpDistanceRatio_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType jumpDistanceRatio;
				ISyncType result = jumpDistanceRatio = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyJumpDrive)P_0).m_jumpDistanceRatio = (Sync<float, SyncDirection.BothWays>)jumpDistanceRatio;
				return result;
			}
		}

		protected class m_isRecharging_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isRecharging;
				ISyncType result = isRecharging = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyJumpDrive)P_0).m_isRecharging = (Sync<bool, SyncDirection.BothWays>)isRecharging;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyJumpDrive_003C_003EActor : IActivator, IActivator<MyJumpDrive>
		{
			private sealed override object CreateInstance()
			{
				return new MyJumpDrive();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyJumpDrive CreateInstance()
			{
				return new MyJumpDrive();
			}

			MyJumpDrive IActivator<MyJumpDrive>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly Sync<float, SyncDirection.FromServer> m_storedPower;

		private IMyGps m_selectedGps;

		private IMyGps m_jumpTarget;

		private readonly Sync<int?, SyncDirection.BothWays> m_targetSync;

		private readonly Sync<float, SyncDirection.BothWays> m_jumpDistanceRatio;

		private int? m_storedJumpTarget;

		private float m_timeRemaining;

		private readonly Sync<bool, SyncDirection.BothWays> m_isRecharging;

		public bool IsJumping;

		private static MyGuiControlListbox m_gpsGuiControl;

		private static readonly string[] m_emissiveTextureNames = new string[4]
		{
			"Emissive0",
			"Emissive1",
			"Emissive2",
			"Emissive3"
		};

		private Color m_prevColor = Color.White;

		private int m_prevFillCount = -1;

		public new MyJumpDriveDefinition BlockDefinition => (MyJumpDriveDefinition)base.BlockDefinition;

		public float CurrentStoredPower
		{
			get
			{
				return m_storedPower;
			}
			set
			{
				if (m_storedPower.Value != value)
				{
					m_storedPower.Value = value;
					UpdateEmissivity();
				}
			}
		}

		public bool CanJump
		{
			get
			{
				if (base.IsWorking && base.IsFunctional && IsFull)
				{
					return true;
				}
				return false;
			}
		}

		public bool IsFull => (float)m_storedPower >= BlockDefinition.PowerNeededForJump;

		float Sandbox.ModAPI.Ingame.IMyJumpDrive.CurrentStoredPower => m_storedPower;

		float Sandbox.ModAPI.Ingame.IMyJumpDrive.MaxStoredPower => BlockDefinition.PowerNeededForJump;

		float Sandbox.ModAPI.IMyJumpDrive.CurrentStoredPower
		{
			get
			{
				return CurrentStoredPower;
			}
			set
			{
				CurrentStoredPower = value;
			}
		}

		MyJumpDriveStatus Sandbox.ModAPI.Ingame.IMyJumpDrive.Status
		{
			get
			{
				if (IsJumping)
				{
					return MyJumpDriveStatus.Jumping;
				}
				if (CanJump)
				{
					return MyJumpDriveStatus.Ready;
				}
				return MyJumpDriveStatus.Charging;
			}
		}

		public bool CanJumpAndHasAccess(long userId)
		{
			if (!CanJump)
			{
				return false;
			}
			return base.IDModule.GetUserRelationToOwner(userId).IsFriendly();
		}

		void Sandbox.ModAPI.IMyJumpDrive.Jump(bool usePilot)
		{
			RequestJump(usePilot);
		}

		public MyJumpDrive()
		{
			CreateTerminalControls();
			m_isRecharging.ValueChanged += delegate
			{
				RaisePropertiesChanged();
			};
			m_targetSync.ValueChanged += delegate
			{
				TargetChanged();
			};
			m_storedPower.AlwaysReject();
		}

		private void TargetChanged()
		{
			if (m_targetSync.Value.HasValue)
			{
				m_jumpTarget = MySession.Static.Gpss.GetGps(m_targetSync.Value.Value);
			}
			else
			{
				m_jumpTarget = null;
			}
			RaisePropertiesChanged();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyJumpDrive>())
			{
				base.CreateTerminalControls();
				MyTerminalControlButton<MyJumpDrive> obj = new MyTerminalControlButton<MyJumpDrive>("Jump", MySpaceTexts.BlockActionTitle_Jump, MySpaceTexts.Blank, delegate(MyJumpDrive x)
				{
					x.RequestJump();
				})
				{
					Enabled = ((MyJumpDrive x) => x.CanJump),
					SupportsMultipleBlocks = false,
					Visible = ((MyJumpDrive x) => false)
				};
				MyTerminalAction<MyJumpDrive> myTerminalAction = obj.EnableAction(MyTerminalActionIcons.TOGGLE);
				if (myTerminalAction != null)
				{
					myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType>
					{
						MyToolbarType.ButtonPanel,
						MyToolbarType.Character,
						MyToolbarType.Seat
					};
					myTerminalAction.ValidForGroups = false;
				}
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlOnOffSwitch<MyJumpDrive> obj2 = new MyTerminalControlOnOffSwitch<MyJumpDrive>("Recharge", MySpaceTexts.BlockPropertyTitle_Recharge, MySpaceTexts.Blank)
				{
					Getter = ((MyJumpDrive x) => x.m_isRecharging),
					Setter = delegate(MyJumpDrive x, bool v)
					{
						x.m_isRecharging.Value = v;
					}
				};
				obj2.EnableToggleAction();
				obj2.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlSlider<MyJumpDrive> myTerminalControlSlider = new MyTerminalControlSlider<MyJumpDrive>("JumpDistance", MySpaceTexts.BlockPropertyTitle_JumpDistance, MySpaceTexts.Blank);
				myTerminalControlSlider.SetLimits(0f, 100f);
				myTerminalControlSlider.DefaultValue = 100f;
				myTerminalControlSlider.Enabled = ((MyJumpDrive x) => x.m_jumpTarget == null);
				myTerminalControlSlider.Getter = ((MyJumpDrive x) => (float)x.m_jumpDistanceRatio * 100f);
				myTerminalControlSlider.Setter = delegate(MyJumpDrive x, float v)
				{
					x.m_jumpDistanceRatio.Value = v * 0.01f;
				};
				myTerminalControlSlider.Writer = delegate(MyJumpDrive x, StringBuilder v)
				{
					v.AppendFormatedDecimal(MathHelper.RoundOn2(x.m_jumpDistanceRatio) * 100f + "% (", (float)x.ComputeMaxDistance() / 1000f, 0, " km").Append(")");
				};
				myTerminalControlSlider.EnableActions(0.01f);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MyJumpDrive>("SelectedTarget", MySpaceTexts.BlockPropertyTitle_DestinationGPS, MySpaceTexts.Blank, multiSelect: false, 1)
				{
					ListContent = delegate(MyJumpDrive x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
					{
						x.FillSelectedTarget(list1, list2);
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyJumpDrive>("RemoveBtn", MySpaceTexts.RemoveProjectionButton, MySpaceTexts.Blank, delegate(MyJumpDrive x)
				{
					x.RemoveSelected();
				})
				{
					Enabled = ((MyJumpDrive x) => x.CanRemove())
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyJumpDrive>("SelectBtn", MyCommonTexts.SelectBlueprint, MySpaceTexts.Blank, delegate(MyJumpDrive x)
				{
					x.SelectTarget();
				})
				{
					Enabled = ((MyJumpDrive x) => x.CanSelect())
				});
				MyTerminalControlListbox<MyJumpDrive> myTerminalControlListbox = new MyTerminalControlListbox<MyJumpDrive>("GpsList", MySpaceTexts.BlockPropertyTitle_GpsLocations, MySpaceTexts.Blank, multiSelect: true);
				myTerminalControlListbox.ListContent = delegate(MyJumpDrive x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
				{
					x.FillGpsList(list1, list2);
				};
				myTerminalControlListbox.ItemSelected = delegate(MyJumpDrive x, List<MyGuiControlListbox.Item> y)
				{
					x.SelectGps(y);
				};
				MyTerminalControlFactory.AddControl(myTerminalControlListbox);
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					m_gpsGuiControl = (MyGuiControlListbox)((MyGuiControlBlockProperty)myTerminalControlListbox.GetGuiControl()).PropertyControl;
				}
			}
		}

		private bool CanSelect()
		{
			return m_selectedGps != null;
		}

		private void SelectTarget()
		{
			if (CanSelect())
			{
				m_targetSync.Value = m_selectedGps.Hash;
			}
		}

		private bool CanRemove()
		{
			return m_jumpTarget != null;
		}

		private void RemoveSelected()
		{
			if (CanRemove())
			{
				m_targetSync.Value = null;
			}
		}

		private void RequestJump(bool usePlayer = true)
		{
			if (CanJump)
			{
				if (usePlayer && MySession.Static.LocalCharacter != null)
				{
					MyShipController myShipController = MySession.Static.LocalCharacter.Parent as MyShipController;
					if (myShipController == null && MySession.Static.ControlledEntity != null)
					{
						myShipController = (MySession.Static.ControlledEntity.Entity as MyShipController);
					}
					RequestJumpInternal(myShipController);
				}
				else if (!usePlayer)
				{
					MyShipController shipController = base.CubeGrid.GridSystems.ControlSystem.GetShipController();
					if (shipController != null)
					{
						RequestJumpInternal(shipController);
					}
				}
			}
			else if (!IsJumping && !IsFull)
			{
				MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationJumpDriveNotFullyCharged, 1500);
				myHudNotification.SetTextFormatArguments(((float)m_storedPower / BlockDefinition.PowerNeededForJump).ToString("P"));
				MyHud.Notifications.Add(myHudNotification);
			}
		}

		private void RequestJumpInternal(Sandbox.ModAPI.Ingame.IMyShipController shipController)
		{
			if (m_jumpTarget != null)
			{
				base.CubeGrid.GridSystems.JumpSystem.RequestJump(m_jumpTarget.Name, m_jumpTarget.Coords, shipController.OwnerId);
				return;
			}
			Vector3D value = Vector3D.Transform(Base6Directions.GetVector(shipController.Orientation.Forward), shipController.CubeGrid.WorldMatrix.GetOrientation());
			value.Normalize();
			Vector3D destination = base.CubeGrid.WorldMatrix.Translation + value * ComputeMaxDistance();
			base.CubeGrid.GridSystems.JumpSystem.RequestJump(MyTexts.Get(MySpaceTexts.Jump_Blind).ToString(), destination, shipController.OwnerId);
		}

		private double ComputeMaxDistance()
		{
			double maxJumpDistance = base.CubeGrid.GridSystems.JumpSystem.GetMaxJumpDistance(base.IDModule.Owner);
			if (maxJumpDistance < 5000.0)
			{
				return 5000.0;
			}
			return 5001.0 + (maxJumpDistance - 5000.0) * (double)(float)m_jumpDistanceRatio;
		}

		private void FillGpsList(ICollection<MyGuiControlListbox.Item> gpsItemList, ICollection<MyGuiControlListbox.Item> selectedGpsItemList)
		{
			List<IMyGps> list = new List<IMyGps>();
			MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, list);
			foreach (IMyGps item2 in list)
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.Name), null, null, item2);
				gpsItemList.Add(item);
				if (m_selectedGps == item2)
				{
					selectedGpsItemList.Add(item);
				}
			}
		}

		private void FillSelectedTarget(ICollection<MyGuiControlListbox.Item> selectedTargetList, ICollection<MyGuiControlListbox.Item> emptyList)
		{
			if (m_jumpTarget != null)
			{
				selectedTargetList.Add(new MyGuiControlListbox.Item(new StringBuilder(m_jumpTarget.Name), MyTexts.GetString(MySpaceTexts.BlockActionTooltip_SelectedJumpTarget), null, m_jumpTarget));
			}
			else
			{
				selectedTargetList.Add(new MyGuiControlListbox.Item(MyTexts.Get(MySpaceTexts.BlindJump), MyTexts.GetString(MySpaceTexts.BlockActionTooltip_SelectedJumpTarget)));
			}
		}

		private void SelectGps(List<MyGuiControlListbox.Item> selection)
		{
			if (selection.Count > 0)
			{
				m_selectedGps = (IMyGps)selection[0].UserData;
				RaisePropertiesChanged();
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, ComputeRequiredPower);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_JumpDrive myObjectBuilder_JumpDrive = objectBuilder as MyObjectBuilder_JumpDrive;
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
			if (Sync.IsServer)
			{
				m_storedPower.Value = Math.Min(myObjectBuilder_JumpDrive.StoredPower, BlockDefinition.PowerNeededForJump);
			}
			m_storedJumpTarget = myObjectBuilder_JumpDrive.JumpTarget;
			if (myObjectBuilder_JumpDrive.JumpTarget.HasValue)
			{
				m_jumpTarget = MySession.Static.Gpss.GetGps(myObjectBuilder_JumpDrive.JumpTarget.Value);
			}
			m_jumpDistanceRatio.SetLocalValue(MathHelper.Clamp(myObjectBuilder_JumpDrive.JumpRatio, 0f, 1f));
			m_isRecharging.SetLocalValue(myObjectBuilder_JumpDrive.Recharging);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyJumpDrive_IsWorkingChanged;
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		private void MyJumpDrive_IsWorkingChanged(MyCubeBlock obj)
		{
			CheckForAbort();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			CheckForAbort();
		}

		private void CheckForAbort()
		{
			if (Sync.IsServer && IsJumping && (!base.IsWorking || !base.IsFunctional))
			{
				IsJumping = false;
				base.CubeGrid.GridSystems.JumpSystem.RequestAbort();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_JumpDrive myObjectBuilder_JumpDrive = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_JumpDrive;
			myObjectBuilder_JumpDrive.StoredPower = m_storedPower;
			if (m_jumpTarget != null)
			{
				myObjectBuilder_JumpDrive.JumpTarget = m_jumpTarget.Hash;
			}
			myObjectBuilder_JumpDrive.JumpRatio = m_jumpDistanceRatio;
			myObjectBuilder_JumpDrive.Recharging = m_isRecharging;
			return myObjectBuilder_JumpDrive;
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			base.CubeGrid.GridSystems.JumpSystem.RegisterJumpDrive(this);
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			base.CubeGrid.GridSystems.JumpSystem.AbortJump(MyGridJumpDriveSystem.MyJumpFailReason.None);
			base.CubeGrid.GridSystems.JumpSystem.UnregisterJumpDrive(this);
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (Sync.IsServer && m_storedJumpTarget.HasValue)
			{
				m_jumpTarget = MySession.Static.Gpss.GetGps(m_storedJumpTarget.Value);
				if (m_jumpTarget != null)
				{
					m_targetSync.Value = m_jumpTarget.Hash;
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			base.ResourceSink.Update();
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (base.IsFunctional && !IsFull && (bool)m_isRecharging)
			{
				StorePower(1600f, base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId));
			}
			UpdateEmissivity();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(BlockDefinition.RequiredPowerInput, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxStoredPower));
			MyValueFormatter.AppendWorkHoursInBestUnit(BlockDefinition.PowerNeededForJump, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_StoredPower));
			MyValueFormatter.AppendWorkHoursInBestUnit(m_storedPower, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RechargedIn));
			MyValueFormatter.AppendTimeInBestUnit(m_timeRemaining, detailedInfo);
			detailedInfo.Append("\n");
			int num = (int)(base.CubeGrid.GridSystems.JumpSystem.GetMaxJumpDistance(base.OwnerId) / 1000.0);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxJump));
			detailedInfo.Append(num).Append(" km");
			if (m_jumpTarget != null)
			{
				detailedInfo.Append("\n");
				double num2 = (m_jumpTarget.Coords - base.CubeGrid.WorldMatrix.Translation).Length();
				float num3 = Math.Min(1f, (float)((double)num / num2));
				detailedInfo.Append(MyTexts.Get(MySpaceTexts.BlockPropertiesText_CurrentJump).ToString() + (num3 * 100f).ToString("F2") + "%");
			}
		}

		private float ComputeRequiredPower()
		{
			if (base.IsFunctional && base.IsWorking && (bool)m_isRecharging)
			{
				if (IsFull)
				{
					return 0f;
				}
				return BlockDefinition.RequiredPowerInput;
			}
			return 0f;
		}

		private void StorePower(float deltaTime, float input)
		{
			float num = input / 3600000f;
			float num2 = deltaTime * num * 0.8f;
			if (Sync.IsServer)
			{
				m_storedPower.Value += num2;
			}
			deltaTime /= 1000f;
			if (Sync.IsServer && (float)m_storedPower > BlockDefinition.PowerNeededForJump)
			{
				m_storedPower.Value = BlockDefinition.PowerNeededForJump;
			}
			if (num2 > 0f)
			{
				m_timeRemaining = (BlockDefinition.PowerNeededForJump - (float)m_storedPower) * deltaTime / num2;
			}
			else
			{
				m_timeRemaining = 0f;
			}
		}

		public void SetStoredPower(float filledRatio)
		{
			if (filledRatio < 0f)
			{
				filledRatio = 0f;
			}
			if (filledRatio >= 1f)
			{
				filledRatio = 1f;
				m_timeRemaining = 0f;
			}
			if (Sync.IsServer)
			{
				CurrentStoredPower = filledRatio * BlockDefinition.PowerNeededForJump;
			}
			UpdateEmissivity();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			m_prevFillCount = -1;
			UpdateEmissivity();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			UpdateEmissivity(force: true);
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		private void UpdateEmissivity(bool force = false)
		{
			Color red = Color.Red;
			float fill = 1f;
			float emissivity = 1f;
			MyEmissiveColorStateResult result;
			if (base.IsWorking)
			{
				if (IsFull)
				{
					red = Color.Green;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
					{
						red = result.EmissiveColor;
					}
				}
				else if (!m_isRecharging)
				{
					fill = (float)m_storedPower / BlockDefinition.PowerNeededForJump;
					red = Color.Red;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
					{
						red = result.EmissiveColor;
					}
				}
				else if (base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) > 0f)
				{
					fill = (float)m_storedPower / BlockDefinition.PowerNeededForJump;
					red = Color.Yellow;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
					{
						red = result.EmissiveColor;
					}
				}
				else
				{
					fill = (float)m_storedPower / BlockDefinition.PowerNeededForJump;
					red = Color.Red;
					emissivity = 1f;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
					{
						red = result.EmissiveColor;
					}
				}
			}
			else if (base.IsFunctional)
			{
				fill = 0f;
				red = Color.Red;
				emissivity = 1f;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
				{
					red = result.EmissiveColor;
				}
			}
			else
			{
				fill = 0f;
				red = Color.Black;
				emissivity = 0f;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
				{
					red = result.EmissiveColor;
				}
			}
			SetEmissive(red, fill, emissivity, force);
		}

		private void SetEmissive(Color color, float fill, float emissivity, bool force)
		{
			int num = (int)(fill * (float)m_emissiveTextureNames.Length);
			if (!force && (base.Render.RenderObjectIDs[0] == uint.MaxValue || (!(color != m_prevColor) && num == m_prevFillCount)))
			{
				return;
			}
			for (int i = 0; i < m_emissiveTextureNames.Length; i++)
			{
				if (i <= num)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], color, emissivity);
				}
				else
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], Color.Black, 0f);
				}
			}
			MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], "Emissive", color, emissivity);
			m_prevColor = color;
			m_prevFillCount = num;
		}
	}
}
