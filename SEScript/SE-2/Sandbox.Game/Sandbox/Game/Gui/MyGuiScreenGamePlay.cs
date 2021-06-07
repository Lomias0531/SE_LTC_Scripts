#define VRAGE
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenGamePlay : MyGuiScreenBase
	{
		private enum MyBuildPlannerAction
		{
			None,
			Withdraw1,
			WithdrawKeep1,
			WithdrawKeep10,
			AddProduction1,
			AddProduction10,
			DepositOre
		}

		private bool audioSet;

		public static MyGuiScreenGamePlay Static;

		private int[] m_lastBeginShootTime;

		private static MyGuiScreenBase m_activeGameplayScreen;

		public static MyGuiScreenBase TmpGameplayScreenHolder;

		public static bool DisableInput;

		private IMyControlMenuInitializer m_controlMenu;

		private bool m_isAnselCameraInit;

		private bool m_reloadSessionNextFrame;

		private bool m_recording;

		public static bool[] DoubleClickDetected
		{
			get;
			private set;
		}

		public static MyGuiScreenBase ActiveGameplayScreen
		{
			get
			{
				return m_activeGameplayScreen;
			}
			set
			{
				m_activeGameplayScreen = value;
			}
		}

		public bool CanSwitchCamera
		{
			get
			{
				if (!MyClipboardComponent.Static.Clipboard.AllowSwitchCameraMode || !MySession.Static.Settings.Enable3rdPersonView)
				{
					return false;
				}
				MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
				if (cameraControllerEnum != MyCameraControllerEnum.Entity)
				{
					return cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator;
				}
				return true;
			}
		}

		public TimeSpan SuppressMovement
		{
			get;
			set;
		}

		public static bool SpectatorEnabled
		{
			get
			{
				if (MySession.Static == null)
				{
					return false;
				}
				if (MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					return true;
				}
				if (!MySession.Static.SurvivalMode)
				{
					return true;
				}
				if (MyMultiplayer.Static != null && MySession.Static.IsUserModerator(Sync.MyId))
				{
					return true;
				}
				if (MyInput.Static.ENABLE_DEVELOPER_KEYS)
				{
					return true;
				}
				return MySession.Static.Settings.EnableSpectator;
			}
		}

		public bool MouseCursorVisible
		{
			get
			{
				return base.DrawMouseCursor;
			}
			set
			{
				base.DrawMouseCursor = value;
			}
		}

		public MyGuiScreenGamePlay()
			: base(Vector2.Zero)
		{
			Static = this;
			base.DrawMouseCursor = false;
			m_closeOnEsc = false;
			m_drawEvenWithoutFocus = true;
			base.EnabledBackgroundFade = false;
			m_canShareInput = false;
			base.CanBeHidden = false;
			m_isAlwaysFirst = true;
			DisableInput = false;
			m_controlMenu = (Activator.CreateInstance(MyPerGameSettings.ControlMenuInitializerType) as IMyControlMenuInitializer);
			MyGuiScreenToolbarConfigBase.ReinitializeBlockScrollbarPosition();
			m_lastBeginShootTime = new int[(uint)(MyEnum<MyShootActionEnum>.Range.Max + 1)];
			DoubleClickDetected = new bool[m_lastBeginShootTime.Length];
		}

		public static void StartLoading(Action loadingAction, string backgroundOverride = null)
		{
			if (MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.StoreLoadingStartTime();
			}
			MyGuiScreenGamePlay myGuiScreenGamePlay = new MyGuiScreenGamePlay();
			myGuiScreenGamePlay.OnLoadingAction = (Action)Delegate.Combine(myGuiScreenGamePlay.OnLoadingAction, loadingAction);
			MyGuiScreenLoading myGuiScreenLoading = new MyGuiScreenLoading(myGuiScreenGamePlay, Static, backgroundOverride);
			myGuiScreenLoading.OnScreenLoadingFinished += delegate
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HUDScreen));
			};
			MyGuiSandbox.AddScreen(myGuiScreenLoading);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenGamePlay";
		}

		public override void LoadData()
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.LoadData - START");
			MySandboxGame.Log.IncreaseIndent();
			base.LoadData();
			MyCharacter.Preload();
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.LoadData - END");
		}

		public override void LoadContent()
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.LoadContent - START");
			MySandboxGame.Log.IncreaseIndent();
			Static = this;
			base.LoadContent();
			MySandboxGame.IsUpdateReady = true;
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.LoadContent - END");
		}

		public override void UnloadData()
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.UnloadData - START");
			MySandboxGame.Log.IncreaseIndent();
			base.UnloadData();
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.UnloadData - END");
		}

		public override void UnloadContent()
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.UnloadContent - START");
			MySandboxGame.Log.IncreaseIndent();
			base.UnloadContent();
			GC.Collect();
			Static = null;
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.UnloadContent - END");
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			bool flag = false;
			if (!MyVRage.Platform.Ansel.IsInitializedSuccessfuly && MyInput.Static.IsKeyPress(MyKeys.F2) && MyInput.Static.IsKeyPress(MyKeys.Alt) && (!MyInput.Static.WasKeyPress(MyKeys.F2) || !MyInput.Static.WasKeyPress(MyKeys.Alt)))
			{
				if (MyVideoSettingsManager.IsCurrentAdapterNvidia())
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextAnselWrongDriverOrCard), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextAnselNotNvidiaGpu), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
				}
			}
			if (MyClipboardComponent.Static != null)
			{
				flag = MyClipboardComponent.Static.HandleGameInput();
			}
			if (!flag && MyCubeBuilder.Static != null)
			{
				flag = MyCubeBuilder.Static.HandleGameInput();
			}
			if (!flag)
			{
				base.HandleInput(receivedFocusInThisUpdate);
			}
		}

		public override void InputLost()
		{
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.InputLost();
			}
		}

		private static void SetAudioVolumes()
		{
			MyAudio.Static.StopMusic();
			MyAudio.Static.ChangeGlobalVolume(1f, 5f);
			if (MyPerGameSettings.UseMusicController && MyFakes.ENABLE_MUSIC_CONTROLLER && MySandboxGame.Config.EnableDynamicMusic && !Sandbox.Engine.Platform.Game.IsDedicated && MyMusicController.Static == null)
			{
				MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
			}
			MyAudio.Static.MusicAllowed = (MyMusicController.Static == null);
			if (MyMusicController.Static != null)
			{
				MyMusicController.Static.Active = true;
			}
			else
			{
				MyAudio.Static.PlayMusic(new MyMusicTrack
				{
					TransitionCategory = MyStringId.GetOrCompute("Default")
				});
			}
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			IMyCameraController cameraController = MySession.Static.CameraController;
			MyStringId context = controlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyStringId auxiliaryContext = controlledEntity?.AuxiliaryContext ?? MySpaceBindingCreator.AX_BASE;
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(context, MyControlsGUI.MAIN_MENU))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
				if (MySessionComponentReplay.Static.IsReplaying || MySessionComponentReplay.Static.IsRecording)
				{
					MySessionComponentReplay.Static.StopRecording();
					MySessionComponentReplay.Static.StopReplay();
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu, !MySandboxGame.IsPaused));
				}
			}
			if (DisableInput)
			{
				if (MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.CANCEL) || MyControllerHelper.IsControl(context, MyControlsSpace.CUTSCENE_SKIPPER)))
				{
					MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneSkip();
				}
				MySession.Static.ControlledEntity.MoveAndRotate(Vector3.Zero, Vector2.Zero, 0f);
				return;
			}
			if (MyInput.Static.ENABLE_DEVELOPER_KEYS || (MySession.Static.LocalHumanPlayer != null && MySession.Static.HasPlayerSpectatorRights(MySession.Static.LocalHumanPlayer.Id.SteamId)))
			{
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_NONE) && MySession.Static.ControlledEntity != null)
				{
					if (!MyInput.Static.ENABLE_DEVELOPER_KEYS)
					{
						SetCameraController();
					}
					else
					{
						MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
						if (cameraControllerEnum != MyCameraControllerEnum.Entity && cameraControllerEnum != MyCameraControllerEnum.ThirdPersonSpectator)
						{
							SetCameraController();
						}
						else if (MySession.Static.VirtualClients.Any() && Sync.Clients.LocalClient != null)
						{
							MyPlayer myPlayer = MySession.Static.VirtualClients.GetNextControlledPlayer(MySession.Static.LocalHumanPlayer) ?? Sync.Clients.LocalClient.GetPlayer(0);
							if (myPlayer != null)
							{
								Sync.Clients.LocalClient.ControlledPlayerSerialId = myPlayer.Id.SerialId;
							}
						}
						else
						{
							long identityId = MySession.Static.LocalHumanPlayer.Identity.IdentityId;
							List<MyEntity> list = new List<MyEntity>();
							foreach (MyEntity entity in MyEntities.GetEntities())
							{
								MyCharacter myCharacter = entity as MyCharacter;
								if (myCharacter != null && !myCharacter.IsDead && myCharacter.GetIdentity() != null && myCharacter.GetIdentity().IdentityId == identityId)
								{
									list.Add(entity);
								}
								MyCubeGrid myCubeGrid = entity as MyCubeGrid;
								if (myCubeGrid != null)
								{
									foreach (MySlimBlock block in myCubeGrid.GetBlocks())
									{
										MyCockpit myCockpit = block.FatBlock as MyCockpit;
										if (myCockpit != null && myCockpit.Pilot != null && myCockpit.Pilot.GetIdentity() != null && myCockpit.Pilot.GetIdentity().IdentityId == identityId)
										{
											list.Add(myCockpit);
										}
									}
								}
							}
							int num = list.IndexOf(MySession.Static.ControlledEntity.Entity);
							List<MyEntity> list2 = new List<MyEntity>();
							if (num + 1 < list.Count)
							{
								list2.AddRange(list.GetRange(num + 1, list.Count - num - 1));
							}
							if (num != -1)
							{
								list2.AddRange(list.GetRange(0, num + 1));
							}
							Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = null;
							for (int i = 0; i < list2.Count; i++)
							{
								if (list2[i] is Sandbox.Game.Entities.IMyControllableEntity)
								{
									myControllableEntity = (list2[i] as Sandbox.Game.Entities.IMyControllableEntity);
									break;
								}
							}
							if (MySession.Static.LocalHumanPlayer != null && myControllableEntity != null)
							{
								MySession.Static.LocalHumanPlayer.Controller.TakeControl(myControllableEntity);
								MyCharacter myCharacter2 = MySession.Static.ControlledEntity as MyCharacter;
								if (myCharacter2 == null && MySession.Static.ControlledEntity is MyCockpit)
								{
									myCharacter2 = (MySession.Static.ControlledEntity as MyCockpit).Pilot;
								}
								if (myCharacter2 != null)
								{
									MySession.Static.LocalHumanPlayer.Identity.ChangeCharacter(myCharacter2);
								}
							}
						}
						if (!(MySession.Static.ControlledEntity is MyCharacter))
						{
							MySession.Static.GameFocusManager.Clear();
						}
					}
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_DELTA))
				{
					if (SpectatorEnabled)
					{
						MySpectatorCameraController.Static.TurnLightOff();
						MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorDelta);
					}
					if (MySession.Static.ControlledEntity != null)
					{
						MySpectator.Static.Position = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
						MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrix.Up);
						MySpectatorCameraController.Static.TrackedEntity = MySession.Static.ControlledEntity.Entity.EntityId;
					}
					else
					{
						MyEntity targetEntity = MyCubeGrid.GetTargetEntity();
						if (targetEntity != null)
						{
							MySpectator.Static.Position = targetEntity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
							MySpectator.Static.SetTarget(targetEntity.PositionComp.GetPosition(), targetEntity.PositionComp.WorldMatrix.Up);
							MySpectatorCameraController.Static.TrackedEntity = targetEntity.EntityId;
						}
					}
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_FREE) && SpectatorEnabled)
				{
					if (MyInput.Static.IsAnyShiftKeyPressed())
					{
						MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorOrbit);
						MySpectatorCameraController.Static.Reset();
					}
					else
					{
						MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed() && MySession.Static.ControlledEntity != null)
					{
						MySpectator.Static.Position = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
						MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrix.Up);
					}
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_STATIC) && MySession.Static.ControlledEntity != null)
				{
					MySpectatorCameraController.Static.TurnLightOff();
					MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFixed);
					if (MyInput.Static.IsAnyCtrlKeyPressed())
					{
						MySpectator.Static.Position = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
						MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrix.Up);
					}
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Space) && MyInput.Static.IsAnyCtrlKeyPressed() && MySession.Static.CameraController == MySpectator.Static && MySession.Static != null && MySession.Static.IsUserSpaceMaster(Sync.MyId))
				{
					MyMultiplayer.TeleportControlledEntity(MySpectator.Static.Position);
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CONSOLE) && MyInput.Static.IsAnyAltKeyPressed())
				{
					MyGuiScreenConsole.Show();
				}
			}
			if (MyInput.Static.ENABLE_DEVELOPER_KEYS && MySession.Static != null && MySession.Static.LocalCharacter != null && MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.B))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
				MyGuiSandbox.AddScreen(ActiveGameplayScreen = new MyGuiScreenAssetModifier(MySession.Static.LocalCharacter));
			}
			if (MyDefinitionErrors.ShouldShowModErrors)
			{
				MyDefinitionErrors.ShouldShowModErrors = false;
				MyGuiSandbox.ShowModErrors();
			}
			if ((MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CAMERA_MODE) || MyControllerHelper.IsControl(MyControllerHelper.CX_CHARACTER, MyControlsSpace.CAMERA_MODE)) && !MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyAltKeyPressed())
			{
				if (CanSwitchCamera)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
					SwitchCamera();
				}
				else if (MySession.Static.CameraController != null && MySession.Static.LocalHumanPlayer != null && MySession.Static.LocalHumanPlayer.Character != null && MySession.Static.ControlledEntity != null)
				{
					MySession.Static.LocalHumanPlayer.Character.ResetHeadRotation();
				}
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.HELP_SCREEN))
			{
				if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					switch (MySandboxGame.Config.DebugComponentsInfo)
					{
					case MyDebugComponent.MyDebugComponentInfoState.NoInfo:
						MySandboxGame.Config.DebugComponentsInfo = MyDebugComponent.MyDebugComponentInfoState.EnabledInfo;
						break;
					case MyDebugComponent.MyDebugComponentInfoState.EnabledInfo:
						MySandboxGame.Config.DebugComponentsInfo = MyDebugComponent.MyDebugComponentInfoState.FullInfo;
						break;
					case MyDebugComponent.MyDebugComponentInfoState.FullInfo:
						MySandboxGame.Config.DebugComponentsInfo = MyDebugComponent.MyDebugComponentInfoState.NoInfo;
						break;
					}
					MySandboxGame.Config.Save();
				}
				else if (MyInput.Static.IsAnyShiftKeyPressed() && MyPerGameSettings.GUI.PerformanceWarningScreen != null)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					MyGuiSandbox.AddScreen(ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PerformanceWarningScreen));
				}
				else if (ActiveGameplayScreen == null)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					MyGuiSandbox.AddScreen(ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HelpScreen));
				}
			}
			if (MyPerGameSettings.SimplePlayerNames && MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.BROADCASTING))
			{
				MyHud.LocationMarkers.Visible = !MyHud.LocationMarkers.Visible;
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.MISSION_SETTINGS) && ActiveGameplayScreen == null && MyPerGameSettings.Game == GameEnum.SE_GAME && MyFakes.ENABLE_MISSION_TRIGGERS)
			{
				if (MySession.Static.Settings.ScenarioEditMode)
				{
					MyGuiSandbox.AddScreen(new MyGuiScreenMissionTriggers());
				}
				else if (MySession.Static.IsScenario)
				{
					MyGuiSandbox.AddScreen(new MyGuiScreenBriefing());
				}
			}
			bool flag = false;
			if (MySession.Static.ControlledEntity is IMyUseObject)
			{
				flag = (MySession.Static.ControlledEntity as IMyUseObject).HandleInput();
			}
			if (controlledEntity != null && !flag)
			{
				if (!MySandboxGame.IsPaused)
				{
					if (MyFakes.ENABLE_NON_PUBLIC_GUI_ELEMENTS && MyInput.Static.IsNewKeyPressed(MyKeys.F2) && MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyAltKeyPressed())
					{
						if (MySession.Static.Settings.GameMode == MyGameModeEnum.Creative)
						{
							MySession.Static.Settings.GameMode = MyGameModeEnum.Survival;
						}
						else
						{
							MySession.Static.Settings.GameMode = MyGameModeEnum.Creative;
						}
					}
					if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.PRIMARY_TOOL_ACTION))
					{
						if (MyToolbarComponent.CurrentToolbar.ShouldActivateSlot)
						{
							MyToolbarComponent.CurrentToolbar.ActivateStagedSelectedItem();
						}
						else
						{
							if (context != MySpaceBindingCreator.CX_SPACESHIP)
							{
								if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastBeginShootTime[0]) < 500f)
								{
									DoubleClickDetected[0] = true;
								}
								else
								{
									DoubleClickDetected[0] = false;
									m_lastBeginShootTime[0] = MySandboxGame.TotalGamePlayTimeInMilliseconds;
								}
							}
							controlledEntity.BeginShoot(MyShootActionEnum.PrimaryAction);
						}
					}
					if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED))
					{
						if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastBeginShootTime[0]) > 500f)
						{
							DoubleClickDetected[0] = false;
						}
						controlledEntity.EndShoot(MyShootActionEnum.PrimaryAction);
						DoubleClickDetected[0] = false;
					}
					if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.SECONDARY_TOOL_ACTION))
					{
						if (context != MySpaceBindingCreator.CX_SPACESHIP)
						{
							if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastBeginShootTime[1]) < 500f)
							{
								DoubleClickDetected[1] = true;
							}
							else
							{
								DoubleClickDetected[1] = false;
								m_lastBeginShootTime[1] = MySandboxGame.TotalGamePlayTimeInMilliseconds;
							}
						}
						controlledEntity.BeginShoot(MyShootActionEnum.SecondaryAction);
					}
					if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED))
					{
						if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastBeginShootTime[1]) > 500f)
						{
							DoubleClickDetected[1] = false;
						}
						controlledEntity.EndShoot(MyShootActionEnum.SecondaryAction);
						DoubleClickDetected[1] = false;
					}
					if (context == MySpaceBindingCreator.CX_CHARACTER || context == MySpaceBindingCreator.CX_JETPACK || context == MySpaceBindingCreator.CX_SPACESHIP)
					{
						if (MyControllerHelper.IsControl(context, MyControlsSpace.USE))
						{
							if (cameraController != null)
							{
								if (!cameraController.HandleUse())
								{
									MakeRecord(controlledEntity, delegate(ref PerFrameData x)
									{
										x.UseData = new UseData
										{
											Use = true
										};
									});
									controlledEntity.Use();
								}
							}
							else
							{
								controlledEntity.Use();
								MakeRecord(controlledEntity, delegate(ref PerFrameData x)
								{
									x.UseData = new UseData
									{
										Use = true
									};
								});
							}
						}
						else if (MyControllerHelper.IsControl(context, MyControlsSpace.USE, MyControlStateType.PRESSED))
						{
							controlledEntity.UseContinues();
							MakeRecord(controlledEntity, delegate(ref PerFrameData x)
							{
								x.UseData = new UseData
								{
									UseContinues = true
								};
							});
						}
						else if (MyControllerHelper.IsControl(context, MyControlsSpace.USE, MyControlStateType.NEW_RELEASED))
						{
							controlledEntity.UseFinished();
							MakeRecord(controlledEntity, delegate(ref PerFrameData x)
							{
								x.UseData = new UseData
								{
									UseFinished = true
								};
							});
						}
						if (MyControllerHelper.IsControl(context, MyControlsSpace.PICK_UP))
						{
							if (cameraController != null)
							{
								if (!cameraController.HandlePickUp())
								{
									controlledEntity.PickUp();
								}
							}
							else
							{
								controlledEntity.PickUp();
							}
						}
						else if (MyControllerHelper.IsControl(context, MyControlsSpace.PICK_UP, MyControlStateType.PRESSED))
						{
							controlledEntity.PickUpContinues();
						}
						else if (MyControllerHelper.IsControl(context, MyControlsSpace.PICK_UP, MyControlStateType.NEW_RELEASED))
						{
							controlledEntity.PickUpFinished();
						}
						if (!MySession.Static.IsCameraUserControlledSpectator())
						{
							string a = (MyInput.Static.GetGameControl(MyControlsSpace.CROUCH) != null) ? MyInput.Static.GetGameControl(MyControlsSpace.CROUCH).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) : null;
							if (!MyInput.Static.IsAnyCtrlKeyPressed() || a == "LCtrl")
							{
								if (MyControllerHelper.IsControl(context, MyControlsSpace.CROUCH))
								{
									controlledEntity.Crouch();
								}
								if (MyControllerHelper.IsControl(context, MyControlsSpace.CROUCH, MyControlStateType.PRESSED))
								{
									controlledEntity.Down();
								}
							}
							controlledEntity.Sprint(MyControllerHelper.IsControl(context, MyControlsSpace.SPRINT, MyControlStateType.PRESSED));
							if (MyControllerHelper.IsControl(context, MyControlsSpace.JUMP))
							{
								controlledEntity.Jump(MyInput.Static.GetPositionDelta());
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.JUMP, MyControlStateType.PRESSED))
							{
								controlledEntity.Up();
							}
							MyShipController myShipController = controlledEntity as MyShipController;
							if (myShipController != null)
							{
								myShipController.WheelJump(MyControllerHelper.IsControl(context, MyControlsSpace.WHEEL_JUMP, MyControlStateType.PRESSED));
								if (MyControllerHelper.IsControl(context, MyControlsSpace.JUMP))
								{
									myShipController.TryEnableBrakes(enable: true);
								}
								else if (MyControllerHelper.IsControl(context, MyControlsSpace.JUMP, MyControlStateType.NEW_RELEASED))
								{
									myShipController.TryEnableBrakes(enable: false);
								}
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.SWITCH_WALK))
							{
								controlledEntity.SwitchWalk();
							}
							if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.BROADCASTING))
							{
								controlledEntity.SwitchBroadcasting();
								MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.HELMET))
							{
								controlledEntity.SwitchHelmet();
								MakeRecord(controlledEntity, delegate(ref PerFrameData x)
								{
									x.ControlSwitchesData = new ControlSwitchesData
									{
										SwitchHelmet = true
									};
								});
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.DAMPING))
							{
								MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
								if (MyInput.Static.IsAnyCtrlKeyPressed())
								{
									if (!controlledEntity.EnabledDamping)
									{
										controlledEntity.SwitchDamping();
									}
									MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyPlayerCollection.SetDampeningEntity, controlledEntity.Entity.EntityId);
								}
								else
								{
									controlledEntity.SwitchDamping();
									MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyPlayerCollection.ClearDampeningEntity, controlledEntity.Entity.EntityId);
								}
								MakeRecord(controlledEntity, delegate(ref PerFrameData x)
								{
									x.ControlSwitchesData = new ControlSwitchesData
									{
										SwitchDamping = true
									};
								});
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.THRUSTS))
							{
								if (!(controlledEntity is MyCharacter))
								{
									MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
								}
								if (!MyInput.Static.IsAnyCtrlKeyPressed())
								{
									controlledEntity.SwitchThrusts();
									MakeRecord(controlledEntity, delegate(ref PerFrameData x)
									{
										x.ControlSwitchesData = new ControlSwitchesData
										{
											SwitchThrusts = true
										};
									});
								}
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.CONSUME_HEALTH))
							{
								ConsumeHealthItem();
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.CONSUME_ENERGY))
							{
								ConsumeEnergyItem();
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.COLOR_TOOL))
							{
								EquipColorTool();
							}
						}
						if (MyControllerHelper.IsControl(context, MyControlsSpace.HEADLIGHTS))
						{
							if (MySession.Static.IsCameraUserControlledSpectator())
							{
								MySpectatorCameraController.Static.SwitchLight();
							}
							else
							{
								MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
								controlledEntity.SwitchLights();
								MakeRecord(controlledEntity, delegate(ref PerFrameData x)
								{
									x.ControlSwitchesData = new ControlSwitchesData
									{
										SwitchLights = true
									};
								});
							}
						}
						if (MyControllerHelper.IsControl(context, MyControlsSpace.TOGGLE_REACTORS))
						{
							MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
							controlledEntity.SwitchReactors();
							MakeRecord(controlledEntity, delegate(ref PerFrameData x)
							{
								x.ControlSwitchesData = new ControlSwitchesData
								{
									SwitchReactors = true
								};
							});
						}
						if (MyControllerHelper.IsControl(context, MyControlsSpace.LANDING_GEAR))
						{
							controlledEntity.SwitchLandingGears();
							MakeRecord(controlledEntity, delegate(ref PerFrameData x)
							{
								x.ControlSwitchesData = new ControlSwitchesData
								{
									SwitchLandingGears = true
								};
							});
						}
						if (MyControllerHelper.IsControl(context, MyControlsSpace.SUICIDE))
						{
							controlledEntity.Die();
						}
						if (controlledEntity is MyCockpit && MyControllerHelper.IsControl(context, MyControlsSpace.CUBE_COLOR_CHANGE))
						{
							(controlledEntity as MyCockpit).SwitchWeaponMode();
						}
						HandleRadialToolbarInput();
						HandleRadialSystemMenuInput();
						HandleRadialVoxelHandInput();
					}
					BuildPlannerControls(context);
				}
				else if (controlledEntity.ShouldEndShootingOnPause(MyShootActionEnum.PrimaryAction) && controlledEntity.ShouldEndShootingOnPause(MyShootActionEnum.SecondaryAction))
				{
					controlledEntity.EndShoot(MyShootActionEnum.PrimaryAction);
					controlledEntity.EndShoot(MyShootActionEnum.SecondaryAction);
				}
				if (!MySandboxGame.IsPaused)
				{
					MySession.Static.GetComponent<MyEmoteSwitcher>();
					if (MyControllerHelper.IsControl(context, MyControlsSpace.TERMINAL) && ActiveGameplayScreen == null)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
						controlledEntity.ShowTerminal();
					}
					if (MyControllerHelper.IsControl(context, MyControlsSpace.INVENTORY) && ActiveGameplayScreen == null)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
						controlledEntity.ShowInventory();
					}
					if (MyControllerHelper.IsControl(context, MyControlsSpace.CONTROL_MENU) && ActiveGameplayScreen == null)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
						m_controlMenu.OpenControlMenu(controlledEntity);
					}
				}
			}
			if (!VRage.Profiler.MyRenderProfiler.ProfilerVisible && MyControllerHelper.IsControl(context, MyControlsSpace.CHAT_SCREEN) && MyGuiScreenChat.Static == null)
			{
				Vector2 hudPos = new Vector2(0.029f, 0.8f);
				hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
				MyGuiSandbox.AddScreen(new MyGuiScreenChat(hudPos));
			}
			if (MyPerGameSettings.VoiceChatEnabled && MyVoiceChatSessionComponent.Static != null)
			{
				if (MyControllerHelper.IsControl(context, MyControlsSpace.VOICE_CHAT))
				{
					MyVoiceChatSessionComponent.Static.StartRecording();
				}
				else if (MyVoiceChatSessionComponent.Static.IsRecording && !MyControllerHelper.IsControl(context, MyControlsSpace.VOICE_CHAT, MyControlStateType.PRESSED))
				{
					MyVoiceChatSessionComponent.Static.StopRecording();
				}
			}
			MoveAndRotatePlayerOrCamera();
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F5) || m_reloadSessionNextFrame)
			{
				m_reloadSessionNextFrame = false;
				if (MySession.Static.Settings.EnableSaving)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					string currentPath = MySession.Static.CurrentPath;
					if (MyInput.Static.IsAnyShiftKeyPressed())
					{
						if (Sync.IsServer)
						{
							if (!MyAsyncSaving.InProgress)
							{
								MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureYouWantToQuickSave), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
								{
									if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
									{
										MyAsyncSaving.Start(delegate
										{
											MySector.ResetEyeAdaptation = true;
										});
									}
								});
								myGuiScreenMessageBox.SkipTransition = true;
								myGuiScreenMessageBox.CloseBeforeCallback = true;
								MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
							}
						}
						else
						{
							MyHud.Notifications.Add(MyNotificationSingletons.ClientCannotSave);
						}
					}
					else if (Sync.IsServer)
					{
						if (MyAsyncSaving.InProgress)
						{
							MyGuiScreenMessageBox myGuiScreenMessageBox2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextSavingInProgress), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
							myGuiScreenMessageBox2.SkipTransition = true;
							myGuiScreenMessageBox2.InstantClose = false;
							MyGuiSandbox.AddScreen(myGuiScreenMessageBox2);
						}
						else
						{
							ShowLoadMessageBox(currentPath);
						}
					}
					else
					{
						ShowReconnectMessageBox();
					}
				}
				else
				{
					MyHud.Notifications.Add(MyNotificationSingletons.CannotSave);
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F3))
			{
				if (Sync.MultiplayerActive)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PlayersScreen));
				}
				else
				{
					MyHud.Notifications.Add(MyNotificationSingletons.MultiplayerDisabled);
				}
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.FACTIONS_MENU) && !MyInput.Static.IsAnyCtrlKeyPressed())
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
				MyScreenManager.AddScreenNow(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.FactionScreen));
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTIVE_CONTRACT_SCREEN) && ActiveGameplayScreen == null)
			{
				MyContractsActiveViewModel viewModel = new MyContractsActiveViewModel();
				ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(viewModel);
			}
			if (!MyInput.Static.IsKeyPress(MyKeys.LeftWindows) && !MyInput.Static.IsKeyPress(MyKeys.RightWindows) && MyInput.Static.IsNewGameControlPressed(MyControlsSpace.BUILD_SCREEN) && !MyInput.Static.IsAnyCtrlKeyPressed() && ActiveGameplayScreen == null && MyPerGameSettings.GUI.EnableToolbarConfigScreen && MyGuiScreenToolbarConfigBase.Static == null && (MySession.Static.ControlledEntity is MyShipController || MySession.Static.ControlledEntity is MyCharacter))
			{
				int num2 = 0;
				if (MyInput.Static.IsAnyShiftKeyPressed())
				{
					num2 += 6;
				}
				if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					num2 += 12;
				}
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
				MyGuiSandbox.AddScreen(ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, num2, MySession.Static.ControlledEntity as MyShipController, null));
			}
			if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.VOXEL_SELECT_SPHERE))
			{
				MySessionComponentVoxelHand.Static.EquipVoxelHand("Sphere");
				MyCubeBuilder.Static.Deactivate();
			}
			if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.PROGRESSION_MENU))
			{
				MyGuiSandbox.AddScreen(ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, MySession.Static.ControlledEntity as MyShipController, "ResearchPage", true, null));
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.PAUSE_GAME) && Sync.Clients.Count < 2)
			{
				MySandboxGame.PauseToggle();
			}
			if (MySession.Static != null)
			{
				if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.ADMIN_MENU))
				{
					if (MySession.Static.IsAdminMenuEnabled && MyPerGameSettings.Game != 0)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
					}
					else
					{
						MyHud.Notifications.Add(MyNotificationSingletons.AdminMenuNotAvailable);
					}
				}
				if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.BLUEPRINTS_MENU))
				{
					if (MyFakes.I_AM_READY_FOR_NEW_BLUEPRINT_SCREEN)
					{
						MyGuiSandbox.AddScreen(MyGuiBlueprintScreen_Reworked.CreateBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL));
					}
					else
					{
						MyGuiSandbox.AddScreen(new MyGuiBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL));
					}
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.F10))
				{
					if (MyInput.Static.IsAnyAltKeyPressed())
					{
						if (MySession.Static.IsAdminMenuEnabled && MyPerGameSettings.Game != 0)
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
						}
						else
						{
							MyHud.Notifications.Add(MyNotificationSingletons.AdminMenuNotAvailable);
						}
					}
					else if (MyPerGameSettings.GUI.VoxelMapEditingScreen != null && (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode) && MyInput.Static.IsAnyShiftKeyPressed())
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.VoxelMapEditingScreen));
					}
					else if (MyFakes.I_AM_READY_FOR_NEW_BLUEPRINT_SCREEN)
					{
						MyGuiSandbox.AddScreen(MyGuiBlueprintScreen_Reworked.CreateBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL));
					}
					else
					{
						MyGuiSandbox.AddScreen(new MyGuiBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL));
					}
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F11) && !MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed())
			{
				MyDX9Gui.SwitchModDebugScreen();
			}
			bool HandleRadialSystemMenuInput()
			{
				if (MyControllerHelper.IsControl(context, MyControlsSpace.SYSTEM_RADIAL_MENU))
				{
					MySession.Static.GetComponent<MyRadialMenuComponent>().ShowSystemRadialMenu(auxiliaryContext, HandleRadialToolbarInput);
					return true;
				}
				return false;
			}
			bool HandleRadialToolbarInput()
			{
				if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.TOOLBAR_RADIAL_MENU))
				{
					MyGuiSandbox.AddScreen(new MyGuiControlRadialMenuBlock(MyDefinitionManager.Static.GetRadialMenuDefinition("Toolbar"), MyControlsSpace.TOOLBAR_RADIAL_MENU, HandleRadialSystemMenuInput));
					return true;
				}
				return false;
			}
			bool HandleRadialVoxelHandInput()
			{
				if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.VOXEL_MATERIAL_SELECT))
				{
					MyGuiSandbox.AddScreen(new MyGuiControlRadialMenuVoxel(MyDefinitionManager.Static.GetRadialMenuDefinition("VoxelHand"), MyControlsSpace.VOXEL_MATERIAL_SELECT, HandleRadialSystemMenuInput));
					return true;
				}
				return false;
			}
		}

		private void EquipColorTool()
		{
			MyCubeBuilder.Static.ActivateColorTool();
		}

		public void BuildPlannerControls(MyStringId context)
		{
			if (MySession.Static == null || MySession.Static.LocalCharacter == null)
			{
				return;
			}
			MyCharacterDetectorComponent myCharacterDetectorComponent = MySession.Static.LocalCharacter.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent == null || myCharacterDetectorComponent.UseObject == null || !myCharacterDetectorComponent.UseObject.SupportedActions.HasFlag(UseActionEnum.BuildPlanner))
			{
				return;
			}
			bool flag = MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER);
			MyUseObjectBase myUseObjectBase = myCharacterDetectorComponent.UseObject as MyUseObjectBase;
			if (myUseObjectBase == null || (myUseObjectBase.PrimaryAction != UseActionEnum.OpenInventory && myUseObjectBase.SecondaryAction != UseActionEnum.OpenInventory))
			{
				return;
			}
			MyCubeBlock myCubeBlock = myUseObjectBase.Owner as MyCubeBlock;
			if (myCubeBlock != null && !myCubeBlock.GetUserRelationToOwner(MySession.Static.LocalCharacter.ControllerInfo.ControllingIdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
			{
				MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				return;
			}
			MyBuildPlannerAction myBuildPlannerAction = MyBuildPlannerAction.None;
			if (MyInput.Static.IsJoystickLastUsed)
			{
				if (MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER_ADD_COMPONNETS))
				{
					myBuildPlannerAction = MyBuildPlannerAction.AddProduction1;
				}
				else if (MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS))
				{
					myBuildPlannerAction = MyBuildPlannerAction.Withdraw1;
				}
				else if (MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER_DEPOSIT_ORE))
				{
					myBuildPlannerAction = MyBuildPlannerAction.DepositOre;
				}
			}
			else if (flag)
			{
				myBuildPlannerAction = MyBuildPlannerAction.Withdraw1;
				if (MyInput.Static.IsAnyShiftKeyPressed())
				{
					myBuildPlannerAction = ((!MyInput.Static.IsAnyCtrlKeyPressed()) ? MyBuildPlannerAction.AddProduction1 : MyBuildPlannerAction.AddProduction10);
				}
				else if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					myBuildPlannerAction = ((!MyInput.Static.IsAnyAltKeyPressed()) ? MyBuildPlannerAction.WithdrawKeep10 : MyBuildPlannerAction.WithdrawKeep1);
				}
				else if (MyInput.Static.IsAnyAltKeyPressed())
				{
					myBuildPlannerAction = MyBuildPlannerAction.DepositOre;
				}
			}
			switch (myBuildPlannerAction)
			{
			case MyBuildPlannerAction.None:
				break;
			case MyBuildPlannerAction.Withdraw1:
				if (MyCubeBuilder.Static.ToolbarBlockDefinition != null && MySession.Static.LocalCharacter.BuildPlanner.Count == 0 && MySession.Static.LocalCharacter.AddToBuildPlanner(MyCubeBuilder.Static.CurrentBlockDefinition))
				{
					MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerComponentsAdded);
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				}
				ProcessWithdraw((MyEntity)myUseObjectBase.Owner, MySession.Static.LocalCharacter.GetInventory(), null);
				break;
			case MyBuildPlannerAction.WithdrawKeep1:
				ProcessWithdraw((MyEntity)myUseObjectBase.Owner, MySession.Static.LocalCharacter.GetInventory(), 1);
				break;
			case MyBuildPlannerAction.WithdrawKeep10:
				ProcessWithdraw((MyEntity)myUseObjectBase.Owner, MySession.Static.LocalCharacter.GetInventory(), 10);
				break;
			case MyBuildPlannerAction.AddProduction1:
				if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
				{
					int num3 = MyTerminalInventoryController.AddComponentsToProduction((MyEntity)myUseObjectBase.Owner, null);
					if (num3 > 0)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionFailed).SetTextFormatArguments(num3);
					}
					else
					{
						MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionSuccessful);
					}
				}
				else
				{
					ShowEmptyBuildPlannerNotification();
				}
				break;
			case MyBuildPlannerAction.AddProduction10:
				if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
				{
					int num2 = MyTerminalInventoryController.AddComponentsToProduction((MyEntity)myUseObjectBase.Owner, 10);
					if (num2 > 0)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionFailed).SetTextFormatArguments(num2);
					}
					else
					{
						MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionSuccessful);
					}
				}
				else
				{
					ShowEmptyBuildPlannerNotification();
				}
				break;
			case MyBuildPlannerAction.DepositOre:
			{
				int num = MyTerminalInventoryController.DepositAll(MySession.Static.LocalCharacter.GetInventory(), (MyEntity)myUseObjectBase.Owner);
				if (num > 0)
				{
					MyHud.Notifications.Add(MyNotificationSingletons.DepositFailed).SetTextFormatArguments(num);
				}
				else
				{
					MyHud.Notifications.Add(MyNotificationSingletons.DepositSuccessful);
				}
				break;
			}
			}
		}

		private void ConsumeEnergyItem()
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), "Powerkit");
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.TryGetInventory(out MyInventory inventory))
			{
				ConsumeItem(inventory, id);
			}
		}

		private void ConsumeHealthItem()
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), "Medkit");
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.TryGetInventory(out MyInventory inventory))
			{
				ConsumeItem(inventory, id);
			}
		}

		private void ConsumeItem(MyInventory inventory, MyDefinitionId id)
		{
			MyFixedPoint a = inventory?.GetItemAmount(id) ?? ((MyFixedPoint)0);
			if (a > 0)
			{
				MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
				a = MyFixedPoint.Min(a, 1);
				if (myCharacter != null && myCharacter.StatComp != null && a > 0)
				{
					inventory.ConsumeItem(id, a, myCharacter.EntityId);
				}
			}
		}

		private static void ProcessWithdraw(MyEntity owner, MyInventory inventory, int? multiplier)
		{
			if (MySession.Static.LocalCharacter.BuildPlanner.Count == 0)
			{
				ShowEmptyBuildPlannerNotification();
				return;
			}
			List<MyIdentity.BuildPlanItem.Component> list = MyTerminalInventoryController.Withdraw(owner, inventory, multiplier);
			if (list.Count == 0)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.WithdrawSuccessful);
				return;
			}
			string missingComponentsText = MyTerminalInventoryController.GetMissingComponentsText(list);
			MyHud.Notifications.Add(MyNotificationSingletons.WithdrawFailed).SetTextFormatArguments(missingComponentsText);
		}

		public static void ShowEmptyBuildPlannerNotification()
		{
			string text = (MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed) ? ("[" + MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.TOOLBAR_RADIAL_MENU) + "]") : ("[" + MyInput.Static.GetGameControl(MyControlsSpace.BUILD_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]");
			MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerEmpty).SetTextFormatArguments(text);
		}

		private void MakeRecord(Sandbox.Game.Entities.IMyControllableEntity controlledObject, MySessionComponentReplay.ActionRef<PerFrameData> action)
		{
			if (MySessionComponentReplay.Static.IsEntityBeingRecorded(controlledObject.Entity.GetTopMostParent().EntityId))
			{
				PerFrameData item = default(PerFrameData);
				action(ref item);
				MySessionComponentReplay.Static.ProvideEntityRecordData(controlledObject.Entity.GetTopMostParent().EntityId, item);
			}
		}

		public void MoveAndRotatePlayerOrCamera()
		{
			MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
			bool flag = cameraControllerEnum == MyCameraControllerEnum.Spectator;
			bool flag2 = flag || (cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator && MyInput.Static.IsAnyAltKeyPressed());
			bool flag3 = MyScreenManager.GetScreenWithFocus() is MyGuiScreenDebugBase && !MyInput.Static.IsAnyAltKeyPressed();
			bool num = !MySessionComponentVoxelHand.Static.BuildMode && !MyCubeBuilder.Static.IsBuildMode;
			bool flag4 = !MySessionComponentVoxelHand.Static.BuildMode && !MyCubeBuilder.Static.IsBuildMode;
			float num2 = num ? MyInput.Static.GetRoll() : 0f;
			Vector2 vector = MyInput.Static.GetRotation();
			Vector3 vector2 = flag4 ? MyInput.Static.GetPositionDelta() : Vector3.Zero;
			if (MySession.Static.ElapsedGameTime < SuppressMovement)
			{
				if (!(vector2 == Vector3.Zero) || !(vector == Vector2.Zero) || num2 != 0f)
				{
					return;
				}
				SuppressMovement = MySession.Static.ElapsedGameTime;
			}
			if (MyPetaInputComponent.MovementDistanceCounter > 0)
			{
				vector2 = Vector3.Forward;
				MyPetaInputComponent.MovementDistanceCounter--;
			}
			if (MySession.Static.ControlledEntity != null)
			{
				if (MySandboxGame.IsPaused)
				{
					if (!flag && !flag2)
					{
						return;
					}
					if (!flag2 || flag3)
					{
						vector = Vector2.Zero;
					}
					num2 = 0f;
				}
				if (MySession.Static.IsCameraUserControlledSpectator())
				{
					MySpectatorCameraController.Static.MoveAndRotate(vector2, vector, num2);
					return;
				}
				if (!MySession.Static.CameraController.IsInFirstPersonView)
				{
					MyThirdPersonSpectator.Static.UpdateZoom();
				}
				if (MySessionComponentReplay.Static.IsEntityBeingReplayed(MySession.Static.ControlledEntity.Entity.GetTopMostParent().EntityId))
				{
					return;
				}
				if (!MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED))
				{
					MySession.Static.ControlledEntity.MoveAndRotate(vector2, vector, num2);
					return;
				}
				if (MySession.Static.ControlledEntity is MyRemoteControl)
				{
					vector = Vector2.Zero;
					num2 = 0f;
				}
				else if (MySession.Static.ControlledEntity is MyCockpit || !MySession.Static.CameraController.IsInFirstPersonView)
				{
					vector = Vector2.Zero;
				}
				else if (MySession.Static.ControlledEntity is MyCharacter)
				{
					vector.X = 0f;
				}
				MySession.Static.ControlledEntity.MoveAndRotate(vector2, vector, num2);
				if (!MySession.Static.CameraController.IsInFirstPersonView)
				{
					MyThirdPersonSpectator.Static.SaveSettings();
				}
			}
			else
			{
				MySpectatorCameraController.Static.MoveAndRotate(vector2, vector, num2);
			}
		}

		public static void SetCameraController()
		{
			if (MySession.Static.ControlledEntity == null)
			{
				return;
			}
			MyRemoteControl myRemoteControl = MySession.Static.ControlledEntity.Entity as MyRemoteControl;
			if (myRemoteControl != null)
			{
				if (myRemoteControl.PreviousControlledEntity is IMyCameraController)
				{
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, myRemoteControl.PreviousControlledEntity.Entity);
				}
			}
			else
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.ControlledEntity.Entity);
			}
		}

		public void SwitchCamera()
		{
			if (MySession.Static.CameraController == null)
			{
				return;
			}
			MySession.Static.CameraController.IsInFirstPersonView = !MySession.Static.CameraController.IsInFirstPersonView;
			if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator)
			{
				MyEntityCameraSettings cameraSettings = null;
				if (MySession.Static.LocalHumanPlayer != null && MySession.Static.ControlledEntity != null)
				{
					MyThirdPersonSpectator.Static.ResetInternalTimers();
					if (MySession.Static.Cameras.TryGetCameraSettings(MySession.Static.LocalHumanPlayer.Id, MySession.Static.ControlledEntity.Entity.EntityId, MySession.Static.ControlledEntity is MyCharacter && MySession.Static.LocalCharacter == MySession.Static.ControlledEntity, out cameraSettings))
					{
						MyThirdPersonSpectator.Static.ResetViewerDistance(cameraSettings.Distance);
					}
					else
					{
						MyThirdPersonSpectator.Static.RecalibrateCameraPosition();
						MySession.Static.ControlledEntity.ControllerInfo.Controller.SaveCamera();
					}
				}
			}
			MySession.Static.SaveControlledEntityCameraSettings(MySession.Static.CameraController.IsInFirstPersonView);
		}

		public void ShowReconnectMessageBox()
		{
			MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureYouWantToReconnect), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					if (MyMultiplayer.Static is MyMultiplayerLobbyClient)
					{
						ulong lobbyId = MyMultiplayer.Static.LobbyId;
						MySessionLoader.UnloadAndExitToMenu();
						MyJoinGameHelper.JoinGame(lobbyId);
					}
					else if (MyMultiplayer.Static is MyMultiplayerClient)
					{
						MyGameServerItem server = (MyMultiplayer.Static as MyMultiplayerClient).Server;
						MySessionLoader.UnloadAndExitToMenu();
						MyJoinGameHelper.JoinGame(server);
					}
				}
			});
			myGuiScreenMessageBox.SkipTransition = true;
			myGuiScreenMessageBox.CloseBeforeCallback = true;
			MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
		}

		public void ShowLoadMessageBox(string currentSession)
		{
			MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureYouWantToQuickLoad), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					MySessionLoader.Unload();
					MySessionLoader.LoadSingleplayerSession(currentSession);
				}
			});
			myGuiScreenMessageBox.SkipTransition = true;
			myGuiScreenMessageBox.CloseBeforeCallback = true;
			MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
		}

		public void RequestSessionReload()
		{
			m_reloadSessionNextFrame = true;
		}

		public override bool Update(bool hasFocus)
		{
			base.Update(hasFocus);
			if (!audioSet && MySandboxGame.IsGameReady && MyAudio.Static != null && MyRenderProxy.VisibleObjectsRead != null && MyRenderProxy.VisibleObjectsRead.Count > 0)
			{
				SetAudioVolumes();
				audioSet = true;
				MyVisualScriptLogicProvider.GameIsReady = true;
				MyHud.MinimalHud = false;
				MyAudio.Static.EnableReverb = (MySandboxGame.Config.EnableReverb && MyFakes.AUDIO_ENABLE_REVERB);
			}
			MySpectator.Static.Update();
			return true;
		}

		public override bool Draw()
		{
			if (MyVRage.Platform.Ansel.IsSessionRunning)
			{
				MyCameraSetup cameraSetup;
				if (!m_isAnselCameraInit)
				{
					MyCameraSetup myCameraSetup = default(MyCameraSetup);
					myCameraSetup.ViewMatrix = MySector.MainCamera.ViewMatrix;
					myCameraSetup.FOV = MySector.MainCamera.FieldOfView;
					myCameraSetup.AspectRatio = MySector.MainCamera.AspectRatio;
					myCameraSetup.NearPlane = MySector.MainCamera.NearPlaneDistance;
					myCameraSetup.FarPlane = MySector.MainCamera.FarFarPlaneDistance;
					myCameraSetup.Position = MySector.MainCamera.Position;
					cameraSetup = myCameraSetup;
					MyVRage.Platform.Ansel.SetCamera(ref cameraSetup);
					m_isAnselCameraInit = true;
				}
				MyVRage.Platform.Ansel.GetCamera(out cameraSetup);
				MyRenderProxy.SetCameraViewMatrix(cameraSetup.ViewMatrix, cameraSetup.ProjectionMatrix, cameraSetup.ProjectionMatrix, cameraSetup.FOV, cameraSetup.FOV, cameraSetup.NearPlane, cameraSetup.FarPlane, cameraSetup.FarPlane, cameraSetup.Position);
				MySector.MainCamera.SetViewMatrix(cameraSetup.ViewMatrix);
				MySector.MainCamera.Update(0f);
			}
			else
			{
				m_isAnselCameraInit = false;
				if (!MyVRage.Platform.Ansel.IsCaptureRunning && MySector.MainCamera != null)
				{
					MySession.Static.CameraController.ControlCamera(MySector.MainCamera);
					MySector.MainCamera.Update(0.0166666675f);
					MySector.MainCamera.UploadViewMatrixToRender();
				}
			}
			MySector.UpdateSunLight();
			MyRenderProxy.UpdateGameplayFrame(MySession.Static.GameplayFrameCounter);
			MyRenderFogSettings myRenderFogSettings = default(MyRenderFogSettings);
			myRenderFogSettings.FogMultiplier = MySector.FogProperties.FogMultiplier;
			myRenderFogSettings.FogColor = MySector.FogProperties.FogColor;
			myRenderFogSettings.FogDensity = MySector.FogProperties.FogDensity;
			MyRenderFogSettings settings = myRenderFogSettings;
			MyRenderProxy.UpdateFogSettings(ref settings);
			MyRenderPlanetSettings myRenderPlanetSettings = default(MyRenderPlanetSettings);
			myRenderPlanetSettings.AtmosphereIntensityMultiplier = MySector.PlanetProperties.AtmosphereIntensityMultiplier;
			myRenderPlanetSettings.AtmosphereIntensityAmbientMultiplier = MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier;
			myRenderPlanetSettings.AtmosphereDesaturationFactorForward = MySector.PlanetProperties.AtmosphereDesaturationFactorForward;
			myRenderPlanetSettings.CloudsIntensityMultiplier = MySector.PlanetProperties.CloudsIntensityMultiplier;
			MyRenderPlanetSettings settings2 = myRenderPlanetSettings;
			MyRenderProxy.UpdatePlanetSettings(ref settings2);
			MyRenderProxy.UpdateSSAOSettings(ref MySector.SSAOSettings);
			MyRenderProxy.UpdateHBAOSettings(ref MySector.HBAOSettings);
			MyEnvironmentData data = MySector.SunProperties.EnvironmentData;
			data.Skybox = ((!string.IsNullOrEmpty(MySession.Static.CustomSkybox)) ? MySession.Static.CustomSkybox : MySector.EnvironmentDefinition.EnvironmentTexture);
			data.SkyboxOrientation = MySector.EnvironmentDefinition.EnvironmentOrientation.ToQuaternion();
			data.EnvironmentLight.SunLightDirection = -MySector.SunProperties.SunDirectionNormalized;
			Vector3D position = MySector.MainCamera.Position;
			MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(position);
			if (closestPlanet != null && closestPlanet.PositionComp.WorldAABB.Contains(position) != 0)
			{
				float airDensity = closestPlanet.GetAirDensity(position);
				if (closestPlanet.AtmosphereSettings.SunColorLinear.HasValue)
				{
					Vector3 value = data.EnvironmentLight.SunColorRaw / MySector.SunProperties.SunIntensity;
					Vector3 value2 = closestPlanet.AtmosphereSettings.SunColorLinear.Value;
					Vector3.Lerp(ref value, ref value2, airDensity, out data.EnvironmentLight.SunColorRaw);
					data.EnvironmentLight.SunColorRaw *= MySector.SunProperties.SunIntensity;
				}
				if (closestPlanet.AtmosphereSettings.SunSpecularColorLinear.HasValue)
				{
					Vector3 value3 = data.EnvironmentLight.SunSpecularColorRaw;
					Vector3 value4 = closestPlanet.AtmosphereSettings.SunSpecularColorLinear.Value;
					Vector3.Lerp(ref value3, ref value4, airDensity, out data.EnvironmentLight.SunSpecularColorRaw);
				}
			}
			MyRenderProxy.UpdateRenderEnvironment(ref data, MySector.ResetEyeAdaptation);
			MySector.ResetEyeAdaptation = false;
			MyRenderProxy.UpdateEnvironmentMap();
			if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled != MyPostprocessSettingsWrapper.AllEnabled || MyPostprocessSettingsWrapper.IsDirty)
			{
				if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled)
				{
					MyPostprocessSettingsWrapper.ReloadSettingsFrom(MySector.EnvironmentDefinition.PostProcessSettings);
				}
				else
				{
					MyPostprocessSettingsWrapper.ReducePostProcessing();
				}
			}
			MyRenderProxy.SwitchPostprocessSettings(ref MyPostprocessSettingsWrapper.Settings);
			if (MyRenderProxy.SettingsDirty)
			{
				MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
			}
			MyRenderProxy.Draw3DScene();
			using (Stats.Generic.Measure("GamePrepareDraw"))
			{
				if (MySession.Static != null)
				{
					MySession.Static.Draw();
				}
			}
			if (MySession.Static.ControlledEntity != null && MySession.Static.CameraController != null)
			{
				MySession.Static.ControlledEntity.DrawHud(MySession.Static.CameraController, MySession.Static.LocalPlayerId);
			}
			if (MySandboxGame.IsPaused && !MyHud.MinimalHud)
			{
				DrawPauseIndicator();
			}
			return true;
		}

		private void DrawPauseIndicator()
		{
			Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
			safeFullscreenRectangle.Height /= 18;
			StringBuilder text = MyTexts.Get(MyCommonTexts.GamePaused);
			MyGuiManager.DrawSpriteBatch(MyGuiConstants.TEXTURE_HUD_BG_MEDIUM_RED2.Texture, safeFullscreenRectangle, Color.White);
			MyGuiManager.DrawString("Blue", text, new Vector2(0.5f, 0.024f), 1f, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
		}
	}
}
