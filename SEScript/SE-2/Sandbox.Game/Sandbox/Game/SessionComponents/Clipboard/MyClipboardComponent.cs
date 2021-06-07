using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.ContextHandling;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.Voxels;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents.Clipboard
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyClipboardComponent : MySessionComponentBase, IMyFocusHolder
	{
		public static MyClipboardComponent Static;

		protected static readonly MyStringId[] m_rotationControls = new MyStringId[6]
		{
			MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE,
			MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE,
			MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE,
			MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE,
			MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE,
			MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE
		};

		protected static readonly int[] m_rotationDirections = new int[6]
		{
			-1,
			1,
			1,
			-1,
			1,
			-1
		};

		private static MyClipboardDefinition m_definition;

		private static MyGridClipboard m_clipboard;

		private MyFloatingObjectClipboard m_floatingObjectClipboard = new MyFloatingObjectClipboard();

		private MyVoxelClipboard m_voxelClipboard = new MyVoxelClipboard();

		private MyHudNotification m_symmetryNotification;

		private MyHudNotification m_pasteNotification;

		private MyHudNotification m_blueprintNotification;

		private float IntersectionDistance = 20f;

		private float BLOCK_ROTATION_SPEED = 0.002f;

		private MyCubeBlockDefinitionWithVariants m_definitionWithVariants;

		protected MyBlockBuilderRotationHints m_rotationHints = new MyBlockBuilderRotationHints();

		private List<Vector3D> m_collisionTestPoints = new List<Vector3D>(12);

		private int m_lastInputHandleTime;

		protected bool m_rotationHintRotating;

		private bool m_activated;

		private MyHudNotification m_stationRotationNotification;

		private MyHudNotification m_stationRotationNotificationOff;

		public static MyClipboardDefinition ClipboardDefinition => m_definition;

		public MyGridClipboard Clipboard => m_clipboard;

		internal MyFloatingObjectClipboard FloatingObjectClipboard => m_floatingObjectClipboard;

		internal MyVoxelClipboard VoxelClipboard => m_voxelClipboard;

		public Vector3D FreePlacementTarget => MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * IntersectionDistance;

		public bool IsActive => m_activated;

		private static bool DeveloperSpectatorIsBuilding
		{
			get
			{
				if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator)
				{
					if (MySession.Static.SurvivalMode)
					{
						return MyInput.Static.ENABLE_DEVELOPER_KEYS;
					}
					return true;
				}
				return false;
			}
		}

		public static bool SpectatorIsBuilding
		{
			get
			{
				if (!DeveloperSpectatorIsBuilding)
				{
					return AdminSpectatorIsBuilding;
				}
				return true;
			}
		}

		private static bool AdminSpectatorIsBuilding
		{
			get
			{
				if (MyFakes.ENABLE_ADMIN_SPECTATOR_BUILDING && MySession.Static != null && MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator && MyMultiplayer.Static != null)
				{
					return MySession.Static.IsUserAdmin(Sync.MyId);
				}
				return false;
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			MyClipboardDefinition definition2 = definition as MyClipboardDefinition;
			if (m_clipboard == null)
			{
				m_definition = definition2;
				m_clipboard = new MyGridClipboard(m_definition.PastingSettings);
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			if (m_clipboard != null)
			{
				m_clipboard.Deactivate();
			}
			if (m_floatingObjectClipboard != null)
			{
				m_floatingObjectClipboard.Deactivate();
			}
			if (m_voxelClipboard != null)
			{
				m_voxelClipboard.Deactivate();
			}
			Static = null;
		}

		private void RotateAxis(int index, int sign, bool newlyPressed, int frameDt)
		{
			float angleDelta = (float)frameDt * BLOCK_ROTATION_SPEED;
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				if (!newlyPressed)
				{
					return;
				}
				angleDelta = MathF.E * 449f / 777f;
			}
			if (MyInput.Static.IsAnyAltKeyPressed())
			{
				if (!newlyPressed)
				{
					return;
				}
				angleDelta = MathHelper.ToRadians(1f);
			}
			if (m_clipboard.IsActive)
			{
				m_clipboard.RotateAroundAxis(index, sign, newlyPressed, angleDelta);
			}
			if (m_floatingObjectClipboard.IsActive)
			{
				m_floatingObjectClipboard.RotateAroundAxis(index, sign, newlyPressed, angleDelta);
			}
		}

		private bool CheckCopyPasteAllowed()
		{
			if (MySession.Static.ControlledEntity != null && !MySessionComponentSafeZones.IsActionAllowed((MyEntity)MySession.Static.ControlledEntity, MySafeZoneAction.Building, 0L, 0uL))
			{
				return false;
			}
			if (!MySession.Static.IsCopyPastingEnabled && !MySession.Static.CreativeMode)
			{
				_ = SpectatorIsBuilding;
			}
			if (MySession.Static.ControlledEntity is MyShipController)
			{
				return false;
			}
			return true;
		}

		public bool HandleGameInput()
		{
			m_rotationHintRotating = false;
			if (MyGuiScreenGamePlay.DisableInput)
			{
				return false;
			}
			MyStringId context = (m_activated && MySession.Static.ControlledEntity is MyCharacter) ? MySession.Static.ControlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
			bool flag = true;
			if (MySession.Static.ControlledEntity != null)
			{
				flag &= MySessionComponentSafeZones.IsActionAllowed((MyEntity)MySession.Static.ControlledEntity, MySafeZoneAction.Building, 0L, 0uL);
			}
			if (!MySession.Static.IsCopyPastingEnabled && !MySession.Static.CreativeMode)
			{
				if (!SpectatorIsBuilding)
				{
				}
			}
			else
			{
				if (MySession.Static.IsCopyPastingEnabled && !(MySession.Static.ControlledEntity is MyShipController))
				{
					if (HandleCopyInput())
					{
						return true;
					}
					if (flag)
					{
						if (HandleDeleteInput())
						{
							return true;
						}
						if (HandleCutInput())
						{
							return true;
						}
						if (HandlePasteInput())
						{
							return true;
						}
						if (HandleMouseScrollInput(context))
						{
							return true;
						}
					}
				}
				if (HandleEscape(context))
				{
					return true;
				}
				if (!flag)
				{
					return false;
				}
				if (HandleLeftMouseButton(context))
				{
					return true;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (HandleBlueprintInput())
			{
				return true;
			}
			if (!MySession.Static.IsCopyPastingEnabled && !(MySession.Static.ControlledEntity is MyShipController) && MyInput.Static.IsNewKeyPressed(MyKeys.V) && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed())
			{
				ShowCannotPasteWarning();
			}
			if (m_clipboard != null && m_clipboard.IsActive && (MyControllerHelper.IsControl(context, MyControlsSpace.FREE_ROTATION) || MyControllerHelper.IsControl(context, MyControlsSpace.SWITCH_BUILDING_MODE)))
			{
				ChangeStationRotation();
			}
			if (HandleRotationInput(context))
			{
				return true;
			}
			return false;
		}

		public void ChangeStationRotation()
		{
			m_clipboard.EnableStationRotation = !m_clipboard.EnableStationRotation;
			m_floatingObjectClipboard.EnableStationRotation = !m_floatingObjectClipboard.EnableStationRotation;
		}

		public static void ShowCannotPasteWarning()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MyCommonTexts.Blueprints_NoCreativeRightsMessage)));
		}

		private bool HandleLeftMouseButton(MyStringId context)
		{
			if (MyInput.Static.IsNewLeftMousePressed() || MyControllerHelper.IsControl(context, MyControlsSpace.COPY_PASTE_ACTION) || MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION))
			{
				bool flag = false;
				if (m_clipboard.IsActive && m_clipboard.PasteGrid())
				{
					UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
					flag = true;
				}
				if (m_floatingObjectClipboard.IsActive && m_floatingObjectClipboard.PasteFloatingObject())
				{
					UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
					flag = true;
				}
				if (m_voxelClipboard.IsActive && m_voxelClipboard.PasteVoxelMap())
				{
					UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
					flag = true;
				}
				if (flag)
				{
					Deactivate();
					return true;
				}
			}
			return false;
		}

		private bool HandleEscape(MyStringId context)
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(context, MyControlsSpace.COPY_PASTE_CANCEL) || MyControllerHelper.IsControl(context, MyControlsSpace.SLOT0))
			{
				bool flag = false;
				if (m_clipboard.IsActive)
				{
					m_clipboard.Deactivate();
					UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
					flag = true;
				}
				if (m_floatingObjectClipboard.IsActive)
				{
					m_floatingObjectClipboard.Deactivate();
					UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
					flag = true;
				}
				if (m_voxelClipboard.IsActive)
				{
					m_voxelClipboard.Deactivate();
					UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
					flag = true;
				}
				if (flag)
				{
					Deactivate();
					return true;
				}
			}
			return false;
		}

		public bool HandlePasteInput()
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.V) && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed())
			{
				return Paste();
			}
			return false;
		}

		public bool Paste()
		{
			if (!CheckCopyPasteAllowed())
			{
				return false;
			}
			bool flag = false;
			MySession.Static.GameFocusManager.Clear();
			if (m_clipboard.PasteGrid(deactivate: true, !m_floatingObjectClipboard.HasCopiedFloatingObjects()))
			{
				MySessionComponentVoxelHand.Static.Enabled = false;
				UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
				flag = true;
			}
			else if (m_floatingObjectClipboard.PasteFloatingObject())
			{
				MySessionComponentVoxelHand.Static.Enabled = false;
				UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
				flag = true;
			}
			if (flag)
			{
				if (m_activated)
				{
					Deactivate();
				}
				else
				{
					Activate();
				}
				return true;
			}
			return false;
		}

		private bool HandleDeleteInput()
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Delete) && MyInput.Static.IsAnyCtrlKeyPressed())
			{
				MyEntity entity = MyCubeGrid.GetTargetEntity();
				if (entity == null)
				{
					return false;
				}
				if (!MySessionComponentSafeZones.IsActionAllowed(entity, MySafeZoneAction.Building, MySession.Static.LocalCharacterEntityId, Sync.MyId))
				{
					return false;
				}
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (myCubeGrid != null)
				{
					MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
					if (localHumanPlayer == null)
					{
						return false;
					}
					long identityId = localHumanPlayer.Identity.IdentityId;
					bool flag = false;
					bool flag2 = false;
					IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(identityId);
					if (myFaction != null)
					{
						flag2 = myFaction.IsLeader(identityId);
					}
					if (MySession.Static.IsUserAdmin(localHumanPlayer.Id.SteamId))
					{
						flag = true;
					}
					else if (myCubeGrid.BigOwners.Count != 0)
					{
						foreach (long bigOwner in myCubeGrid.BigOwners)
						{
							if (bigOwner == identityId)
							{
								flag = true;
								break;
							}
							if (MySession.Static.Players.TryGetIdentity(bigOwner) != null && flag2)
							{
								IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(bigOwner);
								if (myFaction2 != null && myFaction.FactionId == myFaction2.FactionId)
								{
									flag = true;
									break;
								}
							}
						}
					}
					else
					{
						flag = true;
					}
					if (!flag)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.DeletePermissionFailed);
						return false;
					}
				}
				bool flag3 = false;
				if (myCubeGrid != null)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
					bool cutGroup = !MyInput.Static.IsAnyShiftKeyPressed();
					bool cutOverLg = MyInput.Static.IsAnyAltKeyPressed();
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToDeleteGrid), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum v)
					{
						if (v == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							OnDeleteConfirm(entity as MyCubeGrid, cutGroup, cutOverLg);
						}
						MyEntities.EnableEntityBoundingBoxDraw(entity, enable: false);
					}));
					flag3 = true;
				}
				else if (entity is MyVoxelMap)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToDeleteGrid), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum v)
					{
						if (v == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							OnDeleteAsteroidConfirm(entity as MyVoxelMap);
						}
						MyEntities.EnableEntityBoundingBoxDraw(entity, enable: false);
					}));
					flag3 = true;
				}
				else if (entity is MyFloatingObject)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToDeleteGrid), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum v)
					{
						if (v == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							OnCutFloatingObjectConfirm(entity as MyFloatingObject);
						}
						MyEntities.EnableEntityBoundingBoxDraw(entity, enable: false);
					}));
					flag3 = true;
				}
				if (flag3)
				{
					return true;
				}
			}
			return false;
		}

		private bool HandleCutInput()
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.X) && MyInput.Static.IsAnyCtrlKeyPressed())
			{
				Cut();
			}
			return false;
		}

		public bool Cut()
		{
			if (!CheckCopyPasteAllowed())
			{
				return false;
			}
			MyEntity entity = MyCubeGrid.GetTargetEntity();
			if (entity == null)
			{
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(entity, MySafeZoneAction.Building, MySession.Static.LocalCharacterEntityId, Sync.MyId))
			{
				return false;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
				if (localHumanPlayer == null)
				{
					return false;
				}
				long identityId = localHumanPlayer.Identity.IdentityId;
				bool flag = false;
				bool flag2 = false;
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(identityId);
				if (myFaction != null)
				{
					flag2 = myFaction.IsLeader(identityId);
				}
				if (MySession.Static.IsUserAdmin(localHumanPlayer.Id.SteamId))
				{
					flag = true;
				}
				else if (myCubeGrid.BigOwners.Count != 0)
				{
					foreach (long bigOwner in myCubeGrid.BigOwners)
					{
						if (bigOwner == identityId)
						{
							flag = true;
							break;
						}
						if (MySession.Static.Players.TryGetIdentity(bigOwner) != null && flag2)
						{
							IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(bigOwner);
							if (myFaction2 != null && myFaction.FactionId == myFaction2.FactionId)
							{
								flag = true;
								break;
							}
						}
					}
				}
				else
				{
					flag = true;
				}
				if (!flag)
				{
					MyHud.Notifications.Add(MyNotificationSingletons.CutPermissionFailed);
					return false;
				}
			}
			bool handled = false;
			if (myCubeGrid != null && !m_clipboard.IsActive)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				bool cutGroup = !MyInput.Static.IsAnyShiftKeyPressed();
				bool cutOverLg = MyInput.Static.IsAnyAltKeyPressed();
				MyEntities.EnableEntityBoundingBoxDraw(entity, enable: true);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToMoveGridToClipboard), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum v)
				{
					if (v == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						OnCutConfirm(entity as MyCubeGrid, cutGroup, cutOverLg);
					}
					MyEntities.EnableEntityBoundingBoxDraw(entity, enable: false);
				}));
				handled = true;
			}
			else if (entity is MyVoxelMap && !m_voxelClipboard.IsActive && MyPerGameSettings.GUI.VoxelMapEditingScreen == typeof(MyGuiScreenDebugSpawnMenu))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.MessageBoxTextAreYouSureToRemoveAsteroid), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum v)
				{
					if (v == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						OnCutAsteroidConfirm(entity as MyVoxelMap);
					}
					MyEntities.EnableEntityBoundingBoxDraw(entity, enable: false);
				}));
				handled = true;
			}
			else if (entity is MyFloatingObject && !m_floatingObjectClipboard.IsActive)
			{
				MyEntities.EnableEntityBoundingBoxDraw(entity, enable: true);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureToMoveGridToClipboard), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum v)
				{
					if (v == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						OnCutFloatingObjectConfirm(entity as MyFloatingObject);
						handled = true;
					}
					MyEntities.EnableEntityBoundingBoxDraw(entity, enable: false);
				}));
				handled = true;
			}
			if (handled)
			{
				return true;
			}
			return false;
		}

		private bool HandleCopyInput()
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.C) && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyMousePressed())
			{
				Copy();
			}
			return false;
		}

		public bool Copy()
		{
			if (!CheckCopyPasteAllowed())
			{
				return false;
			}
			if (m_clipboard.IsBeingAdded)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.CopyFailed);
				return false;
			}
			if (MySession.Static.CameraController is MyCharacter || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator)
			{
				bool flag = false;
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				MyEntity targetEntity = MyCubeGrid.GetTargetEntity();
				if (!m_clipboard.IsActive && targetEntity is MyCubeGrid)
				{
					MyCubeGrid myCubeGrid = targetEntity as MyCubeGrid;
					MySessionComponentVoxelHand.Static.Enabled = false;
					DeactivateCopyPasteFloatingObject(clear: true);
					if (!MyInput.Static.IsAnyShiftKeyPressed())
					{
						m_clipboard.CopyGroup(myCubeGrid, MyInput.Static.IsAnyAltKeyPressed() ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
						m_clipboard.Activate();
					}
					else
					{
						m_clipboard.CopyGrid(myCubeGrid);
						m_clipboard.Activate();
					}
					UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
					flag = true;
				}
				else if (!m_floatingObjectClipboard.IsActive && targetEntity is MyFloatingObject)
				{
					MySessionComponentVoxelHand.Static.Enabled = false;
					DeactivateCopyPaste(clear: true);
					m_floatingObjectClipboard.CopyfloatingObject(targetEntity as MyFloatingObject);
					UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
					flag = true;
				}
				if (flag)
				{
					Activate();
					MyHud.Notifications.Add(MyNotificationSingletons.CopySucceeded);
					return true;
				}
				MyHud.Notifications.Add(MyNotificationSingletons.CopyFailed);
			}
			return false;
		}

		private bool HandleRotationInput(MyStringId context)
		{
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastInputHandleTime;
			m_lastInputHandleTime += num;
			if (m_activated)
			{
				for (int i = 0; i < 6; i++)
				{
					if (!MyControllerHelper.IsControl(context, m_rotationControls[i], MyControlStateType.PRESSED))
					{
						continue;
					}
					bool flag = MyControllerHelper.IsControl(context, m_rotationControls[i]);
					int num2 = -1;
					int num3 = m_rotationDirections[i];
					if (MyFakes.ENABLE_STANDARD_AXES_ROTATION)
					{
						int[] array = new int[6]
						{
							1,
							1,
							0,
							0,
							2,
							2
						};
						if (m_rotationHints.RotationUpAxis != array[i])
						{
							return true;
						}
					}
					if (i < 2)
					{
						num2 = m_rotationHints.RotationUpAxis;
						num3 *= m_rotationHints.RotationUpDirection;
					}
					if (i >= 2 && i < 4)
					{
						num2 = m_rotationHints.RotationRightAxis;
						num3 *= m_rotationHints.RotationRightDirection;
					}
					if (i >= 4)
					{
						num2 = m_rotationHints.RotationForwardAxis;
						num3 *= m_rotationHints.RotationForwardDirection;
					}
					if (num2 != -1)
					{
						m_rotationHintRotating |= !flag;
						RotateAxis(num2, num3, flag, num);
						return true;
					}
				}
			}
			return false;
		}

		private bool HandleBlueprintInput()
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.B) && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyMousePressed())
			{
				return CreateBlueprint();
			}
			return false;
		}

		public bool CreateBlueprint()
		{
			if (!m_clipboard.IsActive)
			{
				MySessionComponentVoxelHand.Static.Enabled = false;
				MyCubeGrid targetGrid = MyCubeGrid.GetTargetGrid();
				if (targetGrid == null)
				{
					return true;
				}
				if (!MySessionComponentSafeZones.IsActionAllowed(targetGrid, MySafeZoneAction.Building, MySession.Static.LocalCharacterEntityId, Sync.MyId))
				{
					return false;
				}
				if (!MySession.Static.CreativeMode && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					List<MyCubeGrid> list = new List<MyCubeGrid>();
					if (MyInput.Static.IsAnyShiftKeyPressed())
					{
						list.Add(targetGrid);
					}
					else
					{
						MyCubeGridGroups.Static.GetGroups(MyInput.Static.IsAnyAltKeyPressed() ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical).GetGroupNodes(targetGrid, list);
					}
					bool flag = true;
					foreach (MyCubeGrid item in list)
					{
						if (item.BigOwners.Count != 0 && !item.BigOwners.Contains(MySession.Static.LocalPlayerId))
						{
							MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId);
							if (playerFaction == null)
							{
								flag = false;
								break;
							}
							bool flag2 = false;
							foreach (long bigOwner in item.BigOwners)
							{
								if (MySession.Static.Factions.GetPlayerFaction(bigOwner) == playerFaction)
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								flag = false;
								break;
							}
						}
					}
					if (!flag)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
						UpdateBlueprintNotification(MyCommonTexts.CubeBuilderNoBlueprintPermission);
						return true;
					}
				}
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				if (MyInput.Static.IsAnyShiftKeyPressed())
				{
					m_clipboard.CopyGrid(targetGrid);
				}
				else
				{
					m_clipboard.CopyGroup(targetGrid, MyInput.Static.IsAnyAltKeyPressed() ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
				}
				UpdatePasteNotification(MyCommonTexts.CubeBuilderPasteNotification);
				if (MyFakes.I_AM_READY_FOR_NEW_BLUEPRINT_SCREEN)
				{
					MyGuiBlueprintScreen_Reworked myGuiBlueprintScreen_Reworked = MyGuiBlueprintScreen_Reworked.CreateBlueprintScreen(m_clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL);
					if (targetGrid != null)
					{
						myGuiBlueprintScreen_Reworked.CreateBlueprintFromClipboard(withScreenshot: true);
					}
					m_clipboard.Deactivate();
					MyGuiSandbox.AddScreen(myGuiBlueprintScreen_Reworked);
				}
				else
				{
					MyGuiBlueprintScreen myGuiBlueprintScreen = new MyGuiBlueprintScreen(m_clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL);
					if (targetGrid != null)
					{
						myGuiBlueprintScreen.CreateFromClipboard(withScreenshot: true);
					}
					m_clipboard.Deactivate();
					MyGuiSandbox.AddScreen(myGuiBlueprintScreen);
				}
			}
			return true;
		}

		private void OnCutConfirm(MyCubeGrid targetGrid, bool cutGroup, bool cutOverLgs)
		{
			if (MyEntities.EntityExists(targetGrid.EntityId))
			{
				DeactivateCopyPasteVoxel(clear: true);
				DeactivateCopyPasteFloatingObject(clear: true);
				if (cutGroup)
				{
					m_clipboard.CutGroup(targetGrid, cutOverLgs ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
				}
				else
				{
					m_clipboard.CutGrid(targetGrid);
				}
			}
		}

		private void OnDeleteConfirm(MyCubeGrid targetGrid, bool cutGroup, bool cutOverLgs)
		{
			if (MyEntities.EntityExists(targetGrid.EntityId))
			{
				DeactivateCopyPasteVoxel(clear: true);
				DeactivateCopyPasteFloatingObject(clear: true);
				if (cutGroup)
				{
					m_clipboard.DeleteGroup(targetGrid, cutOverLgs ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
				}
				else
				{
					m_clipboard.DeleteGrid(targetGrid);
				}
			}
		}

		private void OnDeleteAsteroidConfirm(MyVoxelMap targetVoxelMap)
		{
			if (MyEntities.EntityExists(targetVoxelMap.EntityId))
			{
				DeactivateCopyPaste(clear: true);
				DeactivateCopyPasteFloatingObject(clear: true);
				MyEntities.SendCloseRequest(targetVoxelMap);
			}
		}

		private void OnCutAsteroidConfirm(MyVoxelMap targetVoxelMap)
		{
			if (MyEntities.EntityExists(targetVoxelMap.EntityId))
			{
				DeactivateCopyPaste(clear: true);
				DeactivateCopyPasteFloatingObject(clear: true);
				MyEntities.SendCloseRequest(targetVoxelMap);
			}
		}

		private void OnDeleteFloatingObjectConfirm(MyFloatingObject floatingObj)
		{
			if (MyEntities.Exist(floatingObj))
			{
				DeactivateCopyPasteVoxel(clear: true);
				DeactivateCopyPaste(clear: true);
				m_floatingObjectClipboard.DeleteFloatingObject(floatingObj);
			}
		}

		private void OnCutFloatingObjectConfirm(MyFloatingObject floatingObj)
		{
			if (MyEntities.Exist(floatingObj))
			{
				DeactivateCopyPasteVoxel(clear: true);
				DeactivateCopyPaste(clear: true);
				m_floatingObjectClipboard.CutFloatingObject(floatingObj);
			}
		}

		public void OnLostFocus()
		{
			Deactivate();
		}

		public void DeactivateCopyPasteVoxel(bool clear = false)
		{
			if (m_voxelClipboard.IsActive)
			{
				m_voxelClipboard.Deactivate();
			}
			RemovePasteNotification();
			if (clear)
			{
				m_voxelClipboard.ClearClipboard();
			}
		}

		public void DeactivateCopyPasteFloatingObject(bool clear = false)
		{
			if (m_floatingObjectClipboard.IsActive)
			{
				m_floatingObjectClipboard.Deactivate();
			}
			RemovePasteNotification();
			if (clear)
			{
				m_floatingObjectClipboard.ClearClipboard();
			}
		}

		public void DeactivateCopyPaste(bool clear = false)
		{
			if (m_clipboard.IsActive)
			{
				m_clipboard.Deactivate();
			}
			RemovePasteNotification();
			if (clear)
			{
				m_clipboard.ClearClipboard();
			}
		}

		private void UpdatePasteNotification(MyStringId myTextsWrapperEnum)
		{
			RemovePasteNotification();
			if (m_clipboard.IsActive)
			{
				m_pasteNotification = new MyHudNotification(myTextsWrapperEnum, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Control);
				MyHud.Notifications.Add(m_pasteNotification);
			}
		}

		private void RemovePasteNotification()
		{
			if (m_pasteNotification != null)
			{
				MyHud.Notifications.Remove(m_pasteNotification);
				m_pasteNotification = null;
			}
		}

		private void UpdateBlueprintNotification(MyStringId text)
		{
			if (m_blueprintNotification != null)
			{
				MyHud.Notifications.Remove(m_blueprintNotification);
			}
			m_blueprintNotification = new MyHudNotification(text, 2500, "Red");
			MyHud.Notifications.Add(m_blueprintNotification);
		}

		private bool HandleMouseScrollInput(MyStringId context)
		{
			bool flag = MyInput.Static.IsAnyCtrlKeyPressed();
			if ((flag && MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue()) || MyControllerHelper.IsControl(context, MyControlsSpace.MOVE_FURTHER, MyControlStateType.PRESSED))
			{
				bool result = false;
				if (m_clipboard.IsActive)
				{
					m_clipboard.MoveEntityFurther();
					result = true;
				}
				if (m_floatingObjectClipboard.IsActive)
				{
					m_floatingObjectClipboard.MoveEntityFurther();
					result = true;
				}
				if (m_voxelClipboard.IsActive)
				{
					m_voxelClipboard.MoveEntityFurther();
					result = true;
				}
				return result;
			}
			if ((flag && MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue()) || MyControllerHelper.IsControl(context, MyControlsSpace.MOVE_CLOSER, MyControlStateType.PRESSED))
			{
				bool result2 = false;
				if (m_clipboard.IsActive)
				{
					m_clipboard.MoveEntityCloser();
					result2 = true;
				}
				if (m_floatingObjectClipboard.IsActive)
				{
					m_floatingObjectClipboard.MoveEntityCloser();
					result2 = true;
				}
				if (m_voxelClipboard.IsActive)
				{
					m_voxelClipboard.MoveEntityCloser();
					result2 = true;
				}
				return result2;
			}
			return false;
		}

		public static void PrepareCharacterCollisionPoints(List<Vector3D> outList)
		{
			MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
			if (myCharacter == null)
			{
				return;
			}
			float scaleFactor = myCharacter.Definition.CharacterCollisionHeight * 0.7f;
			float scaleFactor2 = myCharacter.Definition.CharacterCollisionWidth * 0.2f;
			if (myCharacter != null)
			{
				if (myCharacter.IsCrouching)
				{
					scaleFactor = myCharacter.Definition.CharacterCollisionCrouchHeight;
				}
				Vector3 value = myCharacter.PositionComp.LocalMatrix.Up * scaleFactor;
				Vector3 value2 = myCharacter.PositionComp.LocalMatrix.Forward * scaleFactor2;
				Vector3 value3 = myCharacter.PositionComp.LocalMatrix.Right * scaleFactor2;
				Vector3D value4 = myCharacter.Entity.PositionComp.GetPosition() + myCharacter.PositionComp.LocalMatrix.Up * 0.2f;
				float num = 0f;
				for (int i = 0; i < 6; i++)
				{
					float scaleFactor3 = (float)Math.Sin(num);
					float scaleFactor4 = (float)Math.Cos(num);
					Vector3D vector3D = value4 + scaleFactor3 * value3 + scaleFactor4 * value2;
					outList.Add(vector3D);
					outList.Add(vector3D + value);
					num += MathF.PI / 3f;
				}
			}
		}

		private void Activate()
		{
			MySession.Static.GameFocusManager.Register(this);
			m_activated = true;
		}

		private void Deactivate()
		{
			MySession.Static.GameFocusManager.Unregister(this);
			m_activated = false;
			m_rotationHints.ReleaseRenderData();
			DeactivateCopyPasteVoxel();
			DeactivateCopyPasteFloatingObject();
			DeactivateCopyPaste();
		}

		public void ActivateVoxelClipboard(MyObjectBuilder_EntityBase voxelMap, IMyStorage storage, Vector3 centerDeltaDirection, float dragVectorLength)
		{
			MySessionComponentVoxelHand.Static.Enabled = false;
			m_voxelClipboard.SetVoxelMapFromBuilder(voxelMap, storage, centerDeltaDirection, dragVectorLength);
			Activate();
		}

		public void ActivateFloatingObjectClipboard(MyObjectBuilder_FloatingObject floatingObject, Vector3D centerDeltaDirection, float dragVectorLength)
		{
			MySessionComponentVoxelHand.Static.Enabled = false;
			m_floatingObjectClipboard.SetFloatingObjectFromBuilder(floatingObject, centerDeltaDirection, dragVectorLength);
			Activate();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (!m_activated)
			{
				return;
			}
			m_clipboard.Update();
			m_floatingObjectClipboard.Update();
			m_voxelClipboard.Update();
			if (m_clipboard.IsActive || m_floatingObjectClipboard.IsActive || m_voxelClipboard.IsActive)
			{
				m_collisionTestPoints.Clear();
				PrepareCharacterCollisionPoints(m_collisionTestPoints);
				if (m_clipboard.IsActive)
				{
					m_clipboard.Show();
				}
				else
				{
					m_clipboard.Hide();
				}
				if (m_floatingObjectClipboard.IsActive)
				{
					m_floatingObjectClipboard.Show();
					m_floatingObjectClipboard.HideWhenColliding(m_collisionTestPoints);
				}
				else
				{
					m_floatingObjectClipboard.Hide();
				}
				if (m_voxelClipboard.IsActive)
				{
					m_voxelClipboard.Show();
				}
				else
				{
					m_voxelClipboard.Hide();
				}
			}
			UpdateClipboards();
		}

		private void UpdateClipboards()
		{
			if (m_clipboard.IsActive)
			{
				m_clipboard.CalculateRotationHints(m_rotationHints, m_rotationHintRotating);
			}
			else if (m_floatingObjectClipboard.IsActive)
			{
				m_floatingObjectClipboard.CalculateRotationHints(m_rotationHints, m_rotationHintRotating);
			}
		}
	}
}
