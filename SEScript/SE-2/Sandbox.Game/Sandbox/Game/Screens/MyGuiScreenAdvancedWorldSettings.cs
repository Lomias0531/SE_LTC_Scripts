using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	internal class MyGuiScreenAdvancedWorldSettings : MyGuiScreenBase
	{
		private enum AsteroidAmountEnum
		{
			None = 0,
			Normal = 4,
			More = 7,
			Many = 0x10,
			ProceduralLowest = -5,
			ProceduralLow = -1,
			ProceduralNormal = -2,
			ProceduralHigh = -3,
			ProceduralNone = -4
		}

		private enum MyFloraDensityEnum
		{
			NONE = 0,
			LOW = 10,
			MEDIUM = 20,
			HIGH = 30,
			EXTREME = 40
		}

		private enum MySoundModeEnum
		{
			Arcade,
			Realistic
		}

		private enum MyWorldSizeEnum
		{
			TEN_KM,
			TWENTY_KM,
			FIFTY_KM,
			HUNDRED_KM,
			UNLIMITED,
			CUSTOM
		}

		private enum MyViewDistanceEnum
		{
			CUSTOM = 0,
			FIVE_KM = 5000,
			SEVEN_KM = 7000,
			TEN_KM = 10000,
			FIFTEEN_KM = 15000,
			TWENTY_KM = 20000,
			THIRTY_KM = 30000,
			FORTY_KM = 40000,
			FIFTY_KM = 50000
		}

		private const int MIN_DAY_TIME_MINUTES = 1;

		private const int MAX_DAY_TIME_MINUTES = 1440;

		private readonly float MIN_SAFE_TIME_FOR_SUN = 0.4668752f;

		private MyGuiScreenWorldSettings m_parent;

		private bool m_isNewGame;

		private bool m_isConfirmed;

		private bool m_showWarningForOxygen;

		private bool m_recreating_control;

		private bool m_isHostilityChanged;

		private MyGuiControlTextbox m_passwordTextbox;

		private MyGuiControlCombobox m_onlineMode;

		private MyGuiControlCombobox m_worldSizeCombo;

		private MyGuiControlCombobox m_spawnShipTimeCombo;

		private MyGuiControlCombobox m_viewDistanceCombo;

		private MyGuiControlCombobox m_physicsOptionsCombo;

		private MyGuiControlCombobox m_assembler;

		private MyGuiControlCombobox m_charactersInventory;

		private MyGuiControlCombobox m_refinery;

		private MyGuiControlCombobox m_welder;

		private MyGuiControlCombobox m_grinder;

		private MyGuiControlCombobox m_soundModeCombo;

		private MyGuiControlCombobox m_asteroidAmountCombo;

		private MyGuiControlCombobox m_environment;

		private MyGuiControlCombobox m_blocksInventory;

		private MyGuiControlCheckbox m_autoHealing;

		private MyGuiControlCheckbox m_enableCopyPaste;

		private MyGuiControlCheckbox m_weaponsEnabled;

		private MyGuiControlCheckbox m_showPlayerNamesOnHud;

		private MyGuiControlCheckbox m_thrusterDamage;

		private MyGuiControlCheckbox m_cargoShipsEnabled;

		private MyGuiControlCheckbox m_enableSpectator;

		private MyGuiControlCheckbox m_respawnShipDelete;

		private MyGuiControlCheckbox m_resetOwnership;

		private MyGuiControlCheckbox m_permanentDeath;

		private MyGuiControlCheckbox m_destructibleBlocks;

		private MyGuiControlCheckbox m_enableIngameScripts;

		private MyGuiControlCheckbox m_enableToolShake;

		private MyGuiControlCheckbox m_enableOxygen;

		private MyGuiControlCheckbox m_enableOxygenPressurization;

		private MyGuiControlCheckbox m_enable3rdPersonCamera;

		private MyGuiControlCheckbox m_enableEncounters;

		private MyGuiControlCheckbox m_enableRespawnShips;

		private MyGuiControlCheckbox m_scenarioEditMode;

		private MyGuiControlCheckbox m_enableConvertToStation;

		private MyGuiControlCheckbox m_enableStationVoxelSupport;

		private MyGuiControlCheckbox m_enableSunRotation;

		private MyGuiControlCheckbox m_enableJetpack;

		private MyGuiControlCheckbox m_spawnWithTools;

		private MyGuiControlCheckbox m_enableVoxelDestruction;

		private MyGuiControlCheckbox m_enableDrones;

		private MyGuiControlCheckbox m_enableWolfs;

		private MyGuiControlCheckbox m_enableSpiders;

		private MyGuiControlCheckbox m_enableRemoteBlockRemoval;

		private MyGuiControlCheckbox m_enableContainerDrops;

		private MyGuiControlCheckbox m_blockLimits;

		private MyGuiControlCheckbox m_enableTurretsFriendlyFire;

		private MyGuiControlCheckbox m_enableSubGridDamage;

		private MyGuiControlCheckbox m_enableRealisticDampeners;

		private MyGuiControlCheckbox m_enableAdaptiveSimulationQuality;

		private MyGuiControlCheckbox m_enableVoxelHand;

		private MyGuiControlCheckbox m_enableResearch;

		private MyGuiControlCheckbox m_enableAutoRespawn;

		private MyGuiControlCheckbox m_enableSupergridding;

		private MyGuiControlCheckbox m_enableEconomy;

		private MyGuiControlCheckbox m_enableBountyContracts;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlButton m_survivalModeButton;

		private MyGuiControlButton m_creativeModeButton;

		private MyGuiControlSlider m_maxPlayersSlider;

		private MyGuiControlSlider m_sunRotationIntervalSlider;

		private MyGuiControlLabel m_enableCopyPasteLabel;

		private MyGuiControlLabel m_maxPlayersLabel;

		private MyGuiControlLabel m_maxFloatingObjectsLabel;

		private MyGuiControlLabel m_maxBackupSavesLabel;

		private MyGuiControlLabel m_sunRotationPeriod;

		private MyGuiControlLabel m_sunRotationPeriodValue;

		private MyGuiControlLabel m_enableWolfsLabel;

		private MyGuiControlLabel m_enableSpidersLabel;

		private MyGuiControlLabel m_maxGridSizeValue;

		private MyGuiControlLabel m_maxBlocksPerPlayerValue;

		private MyGuiControlLabel m_totalPCUValue;

		private MyGuiControlLabel m_maxBackupSavesValue;

		private MyGuiControlLabel m_maxFloatingObjectsValue;

		private MyGuiControlLabel m_enableContainerDropsLabel;

		private MyGuiControlLabel m_optimalSpawnDistanceValue;

		private MyGuiControlSlider m_maxFloatingObjectsSlider;

		private MyGuiControlSlider m_maxGridSizeSlider;

		private MyGuiControlSlider m_maxBlocksPerPlayerSlider;

		private MyGuiControlSlider m_totalPCUSlider;

		private MyGuiControlSlider m_optimalSpawnDistanceSlider;

		private MyGuiControlSlider m_maxBackupSavesSlider;

		private StringBuilder m_tempBuilder = new StringBuilder();

		private int m_customWorldSize;

		private int m_customViewDistance = 20000;

		private int? m_asteroidAmount;

		public int AsteroidAmount
		{
			get
			{
				if (!m_asteroidAmount.HasValue)
				{
					return -1;
				}
				return m_asteroidAmount.Value;
			}
			set
			{
				m_asteroidAmount = value;
				switch (value)
				{
				case 0:
					m_asteroidAmountCombo.SelectItemByKey(0L);
					break;
				case 4:
					m_asteroidAmountCombo.SelectItemByKey(4L);
					break;
				case 7:
					m_asteroidAmountCombo.SelectItemByKey(7L);
					break;
				case 16:
					m_asteroidAmountCombo.SelectItemByKey(16L);
					break;
				case -4:
					m_asteroidAmountCombo.SelectItemByKey(-4L);
					break;
				case -1:
					m_asteroidAmountCombo.SelectItemByKey(-1L);
					break;
				case -2:
					m_asteroidAmountCombo.SelectItemByKey(-2L);
					break;
				case -3:
					m_asteroidAmountCombo.SelectItemByKey(-3L);
					break;
				}
			}
		}

		public string Password => m_passwordTextbox.Text;

		public bool IsConfirmed => m_isConfirmed;

		public event Action OnOkButtonClicked;

		public MyGuiScreenAdvancedWorldSettings(MyGuiScreenWorldSettings parent)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, CalcSize(), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenAdvancedWorldSettings.ctor START");
			m_parent = parent;
			base.EnabledBackgroundFade = true;
			m_isNewGame = (parent.Checkpoint == null);
			m_isConfirmed = false;
			RecreateControls(constructor: true);
			m_isHostilityChanged = !m_isNewGame;
			MySandboxGame.Log.WriteLine("MyGuiScreenAdvancedWorldSettings.ctor END");
		}

		public static Vector2 CalcSize()
		{
			return new Vector2(183f / 280f, 0.9398855f);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_recreating_control = true;
			BuildControls();
			LoadValues();
			m_recreating_control = false;
		}

		public void BuildControls()
		{
			Vector2 value = new Vector2(50f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent(null, new Vector2(base.Size.Value.X - value.X * 2f, 2.138f));
			if (!m_isNewGame)
			{
				myGuiControlParent.Size = new Vector2(myGuiControlParent.Size.X, 2.0855f);
			}
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlParent);
			myGuiControlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlScrollablePanel.ScrollbarVEnabled = true;
			myGuiControlScrollablePanel.Size = new Vector2(base.Size.Value.X - value.X * 2f - 0.035f, 0.74f);
			myGuiControlScrollablePanel.Position = new Vector2(-0.27f, -0.394f);
			AddCaption(MyCommonTexts.ScreenCaptionAdvancedSettings, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList2);
			int num = 0;
			MakeLabel(MySpaceTexts.WorldSettings_Password);
			MakeLabel(MyCommonTexts.WorldSettings_OnlineMode);
			m_maxPlayersLabel = MakeLabel(MyCommonTexts.MaxPlayers);
			m_maxFloatingObjectsLabel = MakeLabel(MySpaceTexts.MaxFloatingObjects);
			m_maxBackupSavesLabel = MakeLabel(MySpaceTexts.MaxBackupSaves);
			MyGuiControlLabel control2 = MakeLabel(MySpaceTexts.WorldSettings_EnvironmentHostility);
			MyGuiControlLabel control3 = MakeLabel(MySpaceTexts.WorldSettings_MaxGridSize);
			m_maxGridSizeValue = MakeLabel(MyCommonTexts.Disabled);
			m_maxGridSizeValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			MyGuiControlLabel control4 = MakeLabel(MySpaceTexts.WorldSettings_MaxBlocksPerPlayer);
			m_maxBlocksPerPlayerValue = MakeLabel(MyCommonTexts.Disabled);
			m_maxBlocksPerPlayerValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			MyGuiControlLabel control5 = MakeLabel(MySpaceTexts.WorldSettings_TotalPCU);
			m_totalPCUValue = MakeLabel(MyCommonTexts.Disabled);
			m_totalPCUValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			MyGuiControlLabel control6 = MakeLabel(MySpaceTexts.WorldSettings_OptimalSpawnDistance);
			m_optimalSpawnDistanceValue = MakeLabel(MyCommonTexts.Disabled);
			m_optimalSpawnDistanceValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_maxFloatingObjectsValue = MakeLabel(MyCommonTexts.Disabled);
			m_maxFloatingObjectsValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_maxBackupSavesValue = MakeLabel(MyCommonTexts.Disabled);
			m_maxBackupSavesValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_sunRotationPeriod = MakeLabel(MySpaceTexts.SunRotationPeriod);
			m_sunRotationPeriodValue = MakeLabel(MySpaceTexts.SunRotationPeriod);
			m_sunRotationPeriodValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_sunRotationPeriod = MakeLabel(MySpaceTexts.SunRotationPeriod);
			MakeLabel(MyCommonTexts.WorldSettings_GameMode);
			MakeLabel(MySpaceTexts.WorldSettings_GameStyle);
			MakeLabel(MySpaceTexts.WorldSettings_Scenario);
			MyGuiControlLabel myGuiControlLabel = MakeLabel(MySpaceTexts.WorldSettings_AutoHealing);
			MyGuiControlLabel control7 = MakeLabel(MySpaceTexts.WorldSettings_ThrusterDamage);
			MyGuiControlLabel myGuiControlLabel2 = MakeLabel(MySpaceTexts.WorldSettings_EnableSpectator);
			MyGuiControlLabel myGuiControlLabel3 = MakeLabel(MySpaceTexts.WorldSettings_ResetOwnership);
			MyGuiControlLabel myGuiControlLabel4 = MakeLabel(MySpaceTexts.WorldSettings_PermanentDeath);
			MyGuiControlLabel control8 = MakeLabel(MySpaceTexts.WorldSettings_DestructibleBlocks);
			MyGuiControlLabel myGuiControlLabel5 = MakeLabel(MySpaceTexts.WorldSettings_EnableIngameScripts);
			MyGuiControlLabel control9 = MakeLabel(MySpaceTexts.WorldSettings_Enable3rdPersonCamera);
			MyGuiControlLabel control10 = MakeLabel(MySpaceTexts.WorldSettings_Encounters);
			MyGuiControlLabel control11 = MakeLabel(MySpaceTexts.WorldSettings_EnableToolShake);
			MyGuiControlLabel myGuiControlLabel6 = MakeLabel(MySpaceTexts.WorldSettings_EnableAdaptiveSimulationQuality);
			MyGuiControlLabel control12 = MakeLabel(MySpaceTexts.WorldSettings_EnableVoxelHand);
			MyGuiControlLabel control13 = MakeLabel(MySpaceTexts.WorldSettings_EnableCargoShips);
			m_enableCopyPasteLabel = MakeLabel(MySpaceTexts.WorldSettings_EnableCopyPaste);
			MyGuiControlLabel control14 = MakeLabel(MySpaceTexts.WorldSettings_EnableWeapons);
			MyGuiControlLabel control15 = MakeLabel(MySpaceTexts.WorldSettings_ShowPlayerNamesOnHud);
			MyGuiControlLabel control16 = MakeLabel(MySpaceTexts.WorldSettings_CharactersInventorySize);
			MyGuiControlLabel control17 = MakeLabel(MySpaceTexts.WorldSettings_BlocksInventorySize);
			MyGuiControlLabel control18 = MakeLabel(MySpaceTexts.WorldSettings_RefinerySpeed);
			MyGuiControlLabel control19 = MakeLabel(MySpaceTexts.WorldSettings_AssemblerEfficiency);
			MyGuiControlLabel control20 = MakeLabel(MySpaceTexts.World_Settings_EnableOxygen);
			MyGuiControlLabel oxygenPressurizationLabel = MakeLabel(MySpaceTexts.World_Settings_EnableOxygenPressurization);
			MyGuiControlLabel control21 = MakeLabel(MySpaceTexts.WorldSettings_EnableRespawnShips);
			MyGuiControlLabel control22 = MakeLabel(MySpaceTexts.WorldSettings_RespawnShipDelete);
			MyGuiControlLabel control23 = MakeLabel(MySpaceTexts.WorldSettings_LimitWorldSize);
			MyGuiControlLabel control24 = MakeLabel(MySpaceTexts.WorldSettings_WelderSpeed);
			MyGuiControlLabel control25 = MakeLabel(MySpaceTexts.WorldSettings_GrinderSpeed);
			MyGuiControlLabel control26 = MakeLabel(MySpaceTexts.WorldSettings_RespawnShipCooldown);
			MyGuiControlLabel control27 = MakeLabel(MySpaceTexts.WorldSettings_ViewDistance);
			MyGuiControlLabel control28 = MakeLabel(MyCommonTexts.WorldSettings_Physics);
			MyGuiControlLabel myGuiControlLabel7 = MakeLabel(MyCommonTexts.WorldSettings_BlockLimits);
			MyGuiControlLabel control29 = MakeLabel(MySpaceTexts.WorldSettings_EnableConvertToStation);
			MyGuiControlLabel myGuiControlLabel8 = MakeLabel(MySpaceTexts.WorldSettings_StationVoxelSupport);
			MyGuiControlLabel control30 = MakeLabel(MySpaceTexts.WorldSettings_EnableSunRotation);
			MyGuiControlLabel control31 = MakeLabel(MySpaceTexts.WorldSettings_EnableTurrerFriendlyDamage);
			MyGuiControlLabel myGuiControlLabel9 = MakeLabel(MySpaceTexts.WorldSettings_EnableSubGridDamage);
			MakeLabel(MySpaceTexts.WorldSettings_EnableRealisticDampeners);
			MyGuiControlLabel control32 = MakeLabel(MySpaceTexts.WorldSettings_EnableJetpack);
			MyGuiControlLabel control33 = MakeLabel(MySpaceTexts.WorldSettings_SpawnWithTools);
			MyGuiControlLabel control34 = MakeLabel(MySpaceTexts.WorldSettings_EnableDrones);
			m_enableWolfsLabel = MakeLabel(MySpaceTexts.WorldSettings_EnableWolfs);
			m_enableSpidersLabel = MakeLabel(MySpaceTexts.WorldSettings_EnableSpiders);
			MyGuiControlLabel control35 = MakeLabel(MySpaceTexts.WorldSettings_EnableRemoteBlockRemoval);
			MyGuiControlLabel control36 = MakeLabel(MySpaceTexts.WorldSettings_EnableResearch);
			MyGuiControlLabel control37 = MakeLabel(MySpaceTexts.WorldSettings_EnableAutorespawn);
			MyGuiControlLabel myGuiControlLabel10 = MakeLabel(MySpaceTexts.WorldSettings_EnableSupergridding);
			MyGuiControlLabel control38 = MakeLabel(MySpaceTexts.WorldSettings_EnableBountyContracts);
			MyGuiControlLabel control39 = MakeLabel(MySpaceTexts.WorldSettings_EnableEconomy);
			m_enableContainerDropsLabel = MakeLabel(MySpaceTexts.WorldSettings_EnableContainerDrops);
			MyGuiControlLabel control40 = MakeLabel(MySpaceTexts.WorldSettings_EnableVoxelDestruction);
			MyGuiControlLabel control41 = MakeLabel(MySpaceTexts.WorldSettings_SoundMode);
			MyGuiControlLabel control42 = MakeLabel(MySpaceTexts.Asteroid_Amount);
			float x2 = 0.309375018f;
			m_passwordTextbox = new MyGuiControlTextbox(null, null, 256);
			m_onlineMode = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_environment = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_environment.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnvironment));
			m_environment.ItemSelected += HostilityChanged;
			m_autoHealing = new MyGuiControlCheckbox();
			m_thrusterDamage = new MyGuiControlCheckbox();
			m_cargoShipsEnabled = new MyGuiControlCheckbox();
			m_enableSpectator = new MyGuiControlCheckbox();
			m_resetOwnership = new MyGuiControlCheckbox();
			m_permanentDeath = new MyGuiControlCheckbox();
			m_destructibleBlocks = new MyGuiControlCheckbox();
			m_enableIngameScripts = new MyGuiControlCheckbox();
			m_enable3rdPersonCamera = new MyGuiControlCheckbox();
			m_enableEncounters = new MyGuiControlCheckbox();
			m_enableRespawnShips = new MyGuiControlCheckbox();
			m_enableToolShake = new MyGuiControlCheckbox();
			m_enableAdaptiveSimulationQuality = new MyGuiControlCheckbox();
			m_enableVoxelHand = new MyGuiControlCheckbox();
			m_enableOxygen = new MyGuiControlCheckbox();
			m_enableOxygen.IsCheckedChanged = delegate(MyGuiControlCheckbox x)
			{
				if (m_showWarningForOxygen && x.IsChecked)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.MessageBoxTextAreYouSureEnableOxygen), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum v)
					{
						if (v == MyGuiScreenMessageBox.ResultEnum.NO)
						{
							x.IsChecked = false;
						}
					}));
				}
				if (!x.IsChecked)
				{
					m_enableOxygenPressurization.IsChecked = false;
					m_enableOxygenPressurization.Enabled = false;
					oxygenPressurizationLabel.Enabled = false;
				}
				else
				{
					m_enableOxygenPressurization.Enabled = true;
					oxygenPressurizationLabel.Enabled = true;
				}
			};
			m_enableOxygenPressurization = new MyGuiControlCheckbox();
			if (!m_enableOxygen.IsChecked)
			{
				m_enableOxygenPressurization.Enabled = false;
				oxygenPressurizationLabel.Enabled = false;
			}
			m_enableCopyPaste = new MyGuiControlCheckbox();
			m_weaponsEnabled = new MyGuiControlCheckbox();
			m_showPlayerNamesOnHud = new MyGuiControlCheckbox();
			m_enableSunRotation = new MyGuiControlCheckbox();
			m_enableSunRotation.IsCheckedChanged = delegate(MyGuiControlCheckbox control)
			{
				m_sunRotationIntervalSlider.Enabled = control.IsChecked;
				m_sunRotationPeriodValue.Visible = control.IsChecked;
			};
			m_enableJetpack = new MyGuiControlCheckbox();
			m_spawnWithTools = new MyGuiControlCheckbox();
			m_enableAutoRespawn = new MyGuiControlCheckbox();
			m_enableSupergridding = new MyGuiControlCheckbox();
			m_enableBountyContracts = new MyGuiControlCheckbox();
			m_enableEconomy = new MyGuiControlCheckbox();
			m_enableConvertToStation = new MyGuiControlCheckbox();
			m_enableStationVoxelSupport = new MyGuiControlCheckbox();
			m_maxPlayersSlider = new MyGuiControlSlider(Vector2.Zero, 2f, width: m_onlineMode.Size.X * 0.95f, maxValue: MyMultiplayerLobby.MAX_PLAYERS, defaultValue: null, color: null, labelText: new StringBuilder("{0}").ToString(), labelDecimalPlaces: 0, labelScale: 0.8f, labelSpaceWidth: 0.05f, labelFont: "White", toolTip: null, visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true);
			m_maxFloatingObjectsSlider = new MyGuiControlSlider(Vector2.Zero, 16f, 56f, m_onlineMode.Size.X * 0.95f, null, null, null, 0, 0.7f, 0.05f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true);
			m_maxFloatingObjectsSlider.Value = 0f;
			if (MySandboxGame.Config.ExperimentalMode && MyFakes.IsExperimentalAllowed)
			{
				m_maxFloatingObjectsSlider.MaxValue = 1024f;
			}
			m_maxGridSizeSlider = new MyGuiControlSlider(Vector2.Zero, 0f, 50000f, m_onlineMode.Size.X * 0.95f, null, null, null, 1, 0.8f, 0.05f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true);
			m_maxBlocksPerPlayerSlider = new MyGuiControlSlider(Vector2.Zero, 0f, 100000f, m_onlineMode.Size.X * 0.95f, null, null, null, 1, 0.8f, 0.05f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true);
			m_totalPCUSlider = new MyGuiControlSlider(Vector2.Zero, 100f, 100000f, m_onlineMode.Size.X * 0.95f, null, null, null, 1, 0.8f, 0.05f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true);
			m_totalPCUSlider.Value = 0f;
			if (MySandboxGame.Config.ExperimentalMode && MyFakes.IsExperimentalAllowed)
			{
				m_totalPCUSlider.MinValue = 0f;
				m_totalPCUSlider.MaxValue = 1000000f;
			}
			m_maxBackupSavesSlider = new MyGuiControlSlider(Vector2.Zero, 0f, 1000f, m_onlineMode.Size.X * 0.95f, null, null, null, 0, 0.8f, 0.05f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true);
			m_optimalSpawnDistanceSlider = new MyGuiControlSlider(Vector2.Zero, 900f, 25000f, m_onlineMode.Size.X * 0.95f, null, null, null, 1, 0.8f, 0.05f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true);
			m_enableVoxelDestruction = new MyGuiControlCheckbox();
			m_enableDrones = new MyGuiControlCheckbox();
			m_enableWolfs = new MyGuiControlCheckbox();
			m_enableSpiders = new MyGuiControlCheckbox();
			m_enableRemoteBlockRemoval = new MyGuiControlCheckbox();
			m_enableContainerDrops = new MyGuiControlCheckbox();
			m_enableTurretsFriendlyFire = new MyGuiControlCheckbox();
			m_enableSubGridDamage = new MyGuiControlCheckbox();
			m_enableRealisticDampeners = new MyGuiControlCheckbox();
			m_enableResearch = new MyGuiControlCheckbox();
			m_respawnShipDelete = new MyGuiControlCheckbox();
			m_worldSizeCombo = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_spawnShipTimeCombo = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_soundModeCombo = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_soundModeCombo.ItemSelected += m_soundModeCombo_ItemSelected;
			m_asteroidAmountCombo = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_assembler = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_charactersInventory = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_blocksInventory = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_refinery = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_welder = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_grinder = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_viewDistanceCombo = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_physicsOptionsCombo = new MyGuiControlCombobox(null, new Vector2(x2, 0.04f));
			m_creativeModeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Small, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, CreativeClicked);
			m_creativeModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeCreative);
			m_survivalModeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Small, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, SurvivalClicked);
			m_survivalModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeSurvival);
			if (MyFakes.ENABLE_ASTEROID_FIELDS)
			{
				m_asteroidAmountCombo.ItemSelected += m_asteroidAmountCombo_ItemSelected;
				m_asteroidAmountCombo.AddItem(-4L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralNone);
				m_asteroidAmountCombo.AddItem(-5L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralLowest);
				m_asteroidAmountCombo.AddItem(-1L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralLow);
				m_asteroidAmountCombo.AddItem(-2L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralNormal);
				if (MySandboxGame.Config.ExperimentalMode && MyFakes.IsExperimentalAllowed)
				{
					m_asteroidAmountCombo.AddItem(-3L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralHigh);
				}
				m_asteroidAmountCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsAsteroidAmount));
			}
			m_soundModeCombo.AddItem(0L, MySpaceTexts.WorldSettings_ArcadeSound);
			m_soundModeCombo.AddItem(1L, MySpaceTexts.WorldSettings_RealisticSound);
			m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
			m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
			m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
			m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
			m_environment.AddItem(0L, MySpaceTexts.WorldSettings_EnvironmentHostilitySafe);
			m_environment.AddItem(1L, MySpaceTexts.WorldSettings_EnvironmentHostilityNormal);
			if (MySandboxGame.Config.ExperimentalMode && MyFakes.IsExperimentalAllowed)
			{
				m_environment.AddItem(2L, MySpaceTexts.WorldSettings_EnvironmentHostilityCataclysm);
				m_environment.AddItem(3L, MySpaceTexts.WorldSettings_EnvironmentHostilityCataclysmUnreal);
			}
			m_worldSizeCombo.AddItem(0L, MySpaceTexts.WorldSettings_WorldSize10Km);
			m_worldSizeCombo.AddItem(1L, MySpaceTexts.WorldSettings_WorldSize20Km);
			m_worldSizeCombo.AddItem(2L, MySpaceTexts.WorldSettings_WorldSize50Km);
			m_worldSizeCombo.AddItem(3L, MySpaceTexts.WorldSettings_WorldSize100Km);
			m_worldSizeCombo.AddItem(4L, MySpaceTexts.WorldSettings_WorldSizeUnlimited);
			m_spawnShipTimeCombo.AddItem(0L, MySpaceTexts.WorldSettings_RespawnShip_CooldownsDisabled);
			m_spawnShipTimeCombo.AddItem(1L, MySpaceTexts.WorldSettings_RespawnShip_x01);
			m_spawnShipTimeCombo.AddItem(2L, MySpaceTexts.WorldSettings_RespawnShip_x02);
			m_spawnShipTimeCombo.AddItem(5L, MySpaceTexts.WorldSettings_RespawnShip_x05);
			m_spawnShipTimeCombo.AddItem(10L, MySpaceTexts.WorldSettings_RespawnShip_Default);
			m_spawnShipTimeCombo.AddItem(20L, MySpaceTexts.WorldSettings_RespawnShip_x2);
			m_spawnShipTimeCombo.AddItem(50L, MySpaceTexts.WorldSettings_RespawnShip_x5);
			m_spawnShipTimeCombo.AddItem(100L, MySpaceTexts.WorldSettings_RespawnShip_x10);
			m_spawnShipTimeCombo.AddItem(200L, MySpaceTexts.WorldSettings_RespawnShip_x20);
			m_spawnShipTimeCombo.AddItem(500L, MySpaceTexts.WorldSettings_RespawnShip_x50);
			m_spawnShipTimeCombo.AddItem(1000L, MySpaceTexts.WorldSettings_RespawnShip_x100);
			m_assembler.AddItem(1L, MySpaceTexts.WorldSettings_Realistic);
			m_assembler.AddItem(3L, MySpaceTexts.WorldSettings_Realistic_x3);
			m_assembler.AddItem(10L, MySpaceTexts.WorldSettings_Realistic_x10);
			m_charactersInventory.AddItem(1L, MySpaceTexts.WorldSettings_Realistic);
			m_charactersInventory.AddItem(3L, MySpaceTexts.WorldSettings_Realistic_x3);
			m_charactersInventory.AddItem(5L, MySpaceTexts.WorldSettings_Realistic_x5);
			m_charactersInventory.AddItem(10L, MySpaceTexts.WorldSettings_Realistic_x10);
			m_blocksInventory.AddItem(1L, MySpaceTexts.WorldSettings_Realistic);
			m_blocksInventory.AddItem(3L, MySpaceTexts.WorldSettings_Realistic_x3);
			m_blocksInventory.AddItem(5L, MySpaceTexts.WorldSettings_Realistic_x5);
			m_blocksInventory.AddItem(10L, MySpaceTexts.WorldSettings_Realistic_x10);
			m_refinery.AddItem(1L, MySpaceTexts.WorldSettings_Realistic);
			m_refinery.AddItem(3L, MySpaceTexts.WorldSettings_Realistic_x3);
			m_refinery.AddItem(10L, MySpaceTexts.WorldSettings_Realistic_x10);
			m_welder.AddItem(5L, MySpaceTexts.WorldSettings_Realistic_half);
			m_welder.AddItem(10L, MySpaceTexts.WorldSettings_Realistic);
			m_welder.AddItem(20L, MySpaceTexts.WorldSettings_Realistic_x2);
			m_welder.AddItem(50L, MySpaceTexts.WorldSettings_Realistic_x5);
			m_grinder.AddItem(5L, MySpaceTexts.WorldSettings_Realistic_half);
			m_grinder.AddItem(10L, MySpaceTexts.WorldSettings_Realistic);
			m_grinder.AddItem(20L, MySpaceTexts.WorldSettings_Realistic_x2);
			m_grinder.AddItem(50L, MySpaceTexts.WorldSettings_Realistic_x5);
			m_viewDistanceCombo.AddItem(5000L, MySpaceTexts.WorldSettings_ViewDistance_5_Km);
			m_viewDistanceCombo.AddItem(7000L, MySpaceTexts.WorldSettings_ViewDistance_7_Km);
			m_viewDistanceCombo.AddItem(10000L, MySpaceTexts.WorldSettings_ViewDistance_10_Km);
			m_viewDistanceCombo.AddItem(15000L, MySpaceTexts.WorldSettings_ViewDistance_15_Km);
			if (MySandboxGame.Config.ExperimentalMode && MyFakes.IsExperimentalAllowed)
			{
				m_viewDistanceCombo.AddItem(20000L, MySpaceTexts.WorldSettings_ViewDistance_20_Km);
				m_viewDistanceCombo.AddItem(30000L, MySpaceTexts.WorldSettings_ViewDistance_30_Km);
				m_viewDistanceCombo.AddItem(40000L, MySpaceTexts.WorldSettings_ViewDistance_40_Km);
				m_viewDistanceCombo.AddItem(50000L, MySpaceTexts.WorldSettings_ViewDistance_50_Km);
			}
			m_physicsOptionsCombo.SetToolTip(MyCommonTexts.WorldSettings_Physics_Tooltip);
			m_physicsOptionsCombo.AddItem(4L, MyCommonTexts.WorldSettings_Physics_Fast);
			m_physicsOptionsCombo.AddItem(8L, MyCommonTexts.WorldSettings_Physics_Normal);
			m_physicsOptionsCombo.AddItem(32L, MyCommonTexts.WorldSettings_Physics_Precise);
			m_blockLimits = new MyGuiControlCheckbox();
			CheckExperimental(m_blockLimits, myGuiControlLabel7, MyCommonTexts.ToolTipWorldSettingsBlockLimits, enabled: false);
			m_soundModeCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsSoundMode));
			m_autoHealing.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsAutoHealing), (int)(MySpaceStatEffect.MAX_REGEN_HEALTH_RATIO * 100f)));
			m_thrusterDamage.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsThrusterDamage));
			m_cargoShipsEnabled.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnableCargoShips));
			CheckExperimental(m_enableSpectator, myGuiControlLabel2, MySpaceTexts.ToolTipWorldSettingsEnableSpectator);
			CheckExperimental(m_resetOwnership, myGuiControlLabel3, MySpaceTexts.ToolTipWorldSettingsResetOwnership);
			CheckExperimental(m_permanentDeath, myGuiControlLabel4, MySpaceTexts.ToolTipWorldSettingsPermanentDeath);
			m_destructibleBlocks.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsDestructibleBlocks));
			m_environment.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnvironment));
			m_onlineMode.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsOnlineMode));
			m_enableCopyPaste.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnableCopyPaste));
			m_showPlayerNamesOnHud.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsShowPlayerNamesOnHud));
			m_maxFloatingObjectsSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxFloatingObjects));
			m_maxBackupSavesSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxBackupSaves));
			m_maxGridSizeSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxGridSize));
			m_maxBlocksPerPlayerSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxBlocksPerPlayer));
			m_totalPCUSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsTotalPCU));
			m_optimalSpawnDistanceSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsOptimalSpawnDistance));
			m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
			m_weaponsEnabled.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsWeapons));
			m_worldSizeCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsLimitWorldSize));
			m_viewDistanceCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsViewDistance));
			m_respawnShipDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.TooltipWorldSettingsRespawnShipDelete));
			m_enableToolShake.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_ToolShake));
			CheckExperimental(m_enableAdaptiveSimulationQuality, myGuiControlLabel6, MySpaceTexts.ToolTipWorldSettings_AdaptiveSimulationQuality, enabled: false);
			m_enableOxygen.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableOxygen));
			m_enableOxygenPressurization.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableOxygenPressurization));
			m_enableJetpack.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableJetpack));
			m_enableAutoRespawn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsAutorespawn));
			CheckExperimental(m_enableSupergridding, myGuiControlLabel10, MySpaceTexts.ToolTipWorldSettingsSupergridding);
			m_enableBountyContracts.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsBountyContracts));
			m_enableEconomy.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEconomy));
			m_spawnWithTools.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_SpawnWithTools));
			m_enableEncounters.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableEncounters));
			m_enableSunRotation.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableSunRotation));
			m_enable3rdPersonCamera.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_Enable3rdPersonCamera));
			CheckExperimental(m_enableIngameScripts, myGuiControlLabel5, MySpaceTexts.ToolTipWorldSettings_EnableIngameScripts);
			m_cargoShipsEnabled.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_CargoShipsEnabled));
			CheckExperimental(m_enableWolfs, m_enableWolfsLabel, MySpaceTexts.ToolTipWorldSettings_EnableWolfs);
			CheckExperimental(m_enableSpiders, m_enableSpidersLabel, MySpaceTexts.ToolTipWorldSettings_EnableSpiders);
			m_enableRemoteBlockRemoval.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableRemoteBlockRemoval));
			m_enableContainerDrops.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableContainerDrops));
			m_enableConvertToStation.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableConvertToStation));
			CheckExperimental(m_enableStationVoxelSupport, myGuiControlLabel8, MySpaceTexts.ToolTipWorldSettings_StationVoxelSupport);
			m_enableRespawnShips.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableRespawnShips));
			m_enableVoxelDestruction.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableVoxelDestruction));
			m_enableTurretsFriendlyFire.SetToolTip(MyTexts.GetString(MySpaceTexts.TooltipWorldSettings_EnableTurrerFriendlyDamage));
			CheckExperimental(m_enableSubGridDamage, myGuiControlLabel9, MySpaceTexts.TooltipWorldSettings_EnableSubGridDamage);
			m_enableRealisticDampeners.SetToolTip(MyTexts.GetString(MySpaceTexts.TooltipWorldSettings_EnableRealisticDampeners));
			m_enableResearch.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableResearch));
			m_charactersInventory.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_InventorySize));
			m_blocksInventory.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_BlocksInventorySize));
			m_assembler.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_AssemblerEfficiency));
			m_refinery.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_RefinerySpeed));
			m_welder.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_WeldingSpeed));
			m_grinder.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_GrindingSpeed));
			m_spawnShipTimeCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_RespawnShipCooldown));
			myGuiControlParent.Controls.Add(control16);
			myGuiControlParent.Controls.Add(m_charactersInventory);
			myGuiControlParent.Controls.Add(control17);
			myGuiControlParent.Controls.Add(m_blocksInventory);
			myGuiControlParent.Controls.Add(control19);
			myGuiControlParent.Controls.Add(m_assembler);
			myGuiControlParent.Controls.Add(control18);
			myGuiControlParent.Controls.Add(m_refinery);
			myGuiControlParent.Controls.Add(control24);
			myGuiControlParent.Controls.Add(m_welder);
			myGuiControlParent.Controls.Add(control25);
			myGuiControlParent.Controls.Add(m_grinder);
			myGuiControlParent.Controls.Add(control2);
			myGuiControlParent.Controls.Add(m_environment);
			if (m_isNewGame)
			{
				myGuiControlParent.Controls.Add(control42);
				myGuiControlParent.Controls.Add(m_asteroidAmountCombo);
			}
			if (MyFakes.ENABLE_NEW_SOUNDS)
			{
				myGuiControlParent.Controls.Add(control41);
				myGuiControlParent.Controls.Add(m_soundModeCombo);
			}
			myGuiControlParent.Controls.Add(control23);
			myGuiControlParent.Controls.Add(m_worldSizeCombo);
			myGuiControlParent.Controls.Add(control27);
			myGuiControlParent.Controls.Add(m_viewDistanceCombo);
			myGuiControlParent.Controls.Add(control26);
			myGuiControlParent.Controls.Add(m_spawnShipTimeCombo);
			m_sunRotationIntervalSlider = new MyGuiControlSlider(Vector2.Zero, 0f, 1f, m_onlineMode.Size.X * 0.95f, null, null, null, 1, 0.8f, 0.05f);
			if (MySandboxGame.Config.ExperimentalMode && MyFakes.IsExperimentalAllowed)
			{
				m_sunRotationIntervalSlider.MinValue = 0f;
			}
			else
			{
				m_sunRotationIntervalSlider.MinValue = MIN_SAFE_TIME_FOR_SUN;
			}
			m_sunRotationIntervalSlider.MaxValue = 1f;
			m_sunRotationIntervalSlider.DefaultValue = 0f;
			MyGuiControlSlider sunRotationIntervalSlider = m_sunRotationIntervalSlider;
			sunRotationIntervalSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sunRotationIntervalSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				m_tempBuilder.Clear();
				MyValueFormatter.AppendTimeInBestUnit(MathHelper.Clamp(MathHelper.InterpLog(s.Value, 1f, 1440f), 1f, 1440f) * 60f, m_tempBuilder);
				m_sunRotationPeriodValue.Text = m_tempBuilder.ToString();
			});
			m_sunRotationIntervalSlider.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_DayDuration));
			myGuiControlParent.Controls.Add(control30);
			myGuiControlParent.Controls.Add(m_enableSunRotation);
			myGuiControlParent.Controls.Add(m_sunRotationPeriod);
			myGuiControlParent.Controls.Add(m_sunRotationIntervalSlider);
			myGuiControlParent.Controls.Add(m_maxFloatingObjectsLabel);
			myGuiControlParent.Controls.Add(m_maxFloatingObjectsSlider);
			if (MyFakes.IsExperimentalAllowed)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel7);
				myGuiControlParent.Controls.Add(m_blockLimits);
			}
			myGuiControlParent.Controls.Add(control3);
			myGuiControlParent.Controls.Add(m_maxGridSizeSlider);
			myGuiControlParent.Controls.Add(control4);
			myGuiControlParent.Controls.Add(m_maxBlocksPerPlayerSlider);
			myGuiControlParent.Controls.Add(control5);
			myGuiControlParent.Controls.Add(m_totalPCUSlider);
			myGuiControlParent.Controls.Add(m_maxBackupSavesLabel);
			myGuiControlParent.Controls.Add(m_maxBackupSavesSlider);
			myGuiControlParent.Controls.Add(control6);
			myGuiControlParent.Controls.Add(m_optimalSpawnDistanceSlider);
			if (MyFakes.ENABLE_PHYSICS_SETTINGS)
			{
				myGuiControlParent.Controls.Add(control28);
				myGuiControlParent.Controls.Add(m_physicsOptionsCombo);
			}
			float num2 = 0.21f;
			Vector2 value2 = new Vector2(0f, 0.052f);
			Vector2 value3 = -myGuiControlParent.Size / 2f + new Vector2(0f, m_creativeModeButton.Size.Y / 2f + value2.Y / 3f);
			Vector2 value4 = value3 + new Vector2(num2, 0f);
			_ = m_onlineMode.Size;
			foreach (MyGuiControlBase control43 in myGuiControlParent.Controls)
			{
				control43.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				if (control43 is MyGuiControlLabel)
				{
					control43.Position = value3 + value2 * num;
				}
				else
				{
					control43.Position = value4 + value2 * num++;
					if (num == 5 || num == 9 || num == 17 || num == 19)
					{
						value3.Y += value2.Y / 5f;
						value4.Y += value2.Y / 5f;
					}
				}
			}
			m_survivalModeButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_survivalModeButton.Position = m_creativeModeButton.Position + new Vector2(m_onlineMode.Size.X, 0f);
			MyGuiControlSlider maxBackupSavesSlider = m_maxBackupSavesSlider;
			maxBackupSavesSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(maxBackupSavesSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				m_maxBackupSavesValue.Text = s.Value.ToString();
			});
			MyGuiControlSlider maxFloatingObjectsSlider = m_maxFloatingObjectsSlider;
			maxFloatingObjectsSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(maxFloatingObjectsSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				m_maxFloatingObjectsValue.Text = s.Value.ToString();
			});
			MyGuiControlSlider maxGridSizeSlider = m_maxGridSizeSlider;
			maxGridSizeSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(maxGridSizeSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				if (s.Value >= 100f)
				{
					m_maxGridSizeValue.Text = (s.Value - s.Value % 100f).ToString();
				}
				else
				{
					m_maxGridSizeValue.Text = MyTexts.GetString(MyCommonTexts.Disabled);
				}
			});
			MyGuiControlSlider maxBlocksPerPlayerSlider = m_maxBlocksPerPlayerSlider;
			maxBlocksPerPlayerSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(maxBlocksPerPlayerSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				if (s.Value >= 100f)
				{
					m_maxBlocksPerPlayerValue.Text = (s.Value - s.Value % 100f).ToString();
				}
				else
				{
					m_maxBlocksPerPlayerValue.Text = MyTexts.GetString(MyCommonTexts.Disabled);
				}
			});
			MyGuiControlSlider totalPCUSlider = m_totalPCUSlider;
			totalPCUSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(totalPCUSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				if (s.Value >= 100f)
				{
					m_totalPCUValue.Text = (s.Value - s.Value % 100f).ToString();
				}
				else
				{
					m_totalPCUValue.Text = MyTexts.GetString(MyCommonTexts.Disabled);
				}
			});
			MyGuiControlSlider optimalSpawnDistanceSlider = m_optimalSpawnDistanceSlider;
			optimalSpawnDistanceSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(optimalSpawnDistanceSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				if (s.Value >= 1000f)
				{
					m_optimalSpawnDistanceValue.Text = (s.Value - s.Value % 100f).ToString();
				}
				else
				{
					m_optimalSpawnDistanceValue.Text = MyTexts.GetString(MyCommonTexts.Disabled);
				}
			});
			m_maxGridSizeValue.Position = new Vector2(m_sunRotationIntervalSlider.Position.X + 0.31f, m_maxGridSizeSlider.Position.Y);
			m_maxBlocksPerPlayerValue.Position = new Vector2(m_sunRotationIntervalSlider.Position.X + 0.31f, m_maxBlocksPerPlayerSlider.Position.Y);
			m_totalPCUValue.Position = new Vector2(m_sunRotationIntervalSlider.Position.X + 0.31f, m_totalPCUSlider.Position.Y);
			m_optimalSpawnDistanceValue.Position = new Vector2(m_sunRotationIntervalSlider.Position.X + 0.31f, m_optimalSpawnDistanceSlider.Position.Y);
			m_maxFloatingObjectsValue.Position = new Vector2(m_sunRotationIntervalSlider.Position.X + 0.31f, m_maxFloatingObjectsSlider.Position.Y);
			m_maxBackupSavesValue.Position = new Vector2(m_sunRotationIntervalSlider.Position.X + 0.31f, m_maxBackupSavesSlider.Position.Y);
			myGuiControlParent.Controls.Add(m_maxGridSizeValue);
			myGuiControlParent.Controls.Add(m_maxBlocksPerPlayerValue);
			myGuiControlParent.Controls.Add(m_totalPCUValue);
			myGuiControlParent.Controls.Add(m_optimalSpawnDistanceValue);
			myGuiControlParent.Controls.Add(m_maxFloatingObjectsValue);
			myGuiControlParent.Controls.Add(m_maxBackupSavesValue);
			float num3 = 0.055f;
			myGuiControlLabel.Position = new Vector2(myGuiControlLabel.Position.X - num2 / 2f, myGuiControlLabel.Position.Y + num3);
			m_autoHealing.Position = new Vector2(m_autoHealing.Position.X - num2 / 2f, m_autoHealing.Position.Y + num3);
			m_sunRotationPeriodValue.Position = new Vector2(m_sunRotationIntervalSlider.Position.X + 0.31f, m_sunRotationIntervalSlider.Position.Y);
			myGuiControlParent.Controls.Add(m_sunRotationPeriodValue);
			int count = myGuiControlParent.Controls.Count;
			myGuiControlParent.Controls.Add(myGuiControlLabel);
			myGuiControlParent.Controls.Add(m_autoHealing);
			myGuiControlParent.Controls.Add(control22);
			myGuiControlParent.Controls.Add(m_respawnShipDelete);
			bool flag = MyFakes.IsExperimentalAllowed || MyInput.Static.ENABLE_DEVELOPER_KEYS;
			if (flag)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel2);
				myGuiControlParent.Controls.Add(m_enableSpectator);
			}
			myGuiControlParent.Controls.Add(m_enableCopyPasteLabel);
			myGuiControlParent.Controls.Add(m_enableCopyPaste);
			myGuiControlParent.Controls.Add(control15);
			myGuiControlParent.Controls.Add(m_showPlayerNamesOnHud);
			if (flag)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel3);
				myGuiControlParent.Controls.Add(m_resetOwnership);
			}
			myGuiControlParent.Controls.Add(control7);
			myGuiControlParent.Controls.Add(m_thrusterDamage);
			if (flag)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel4);
				myGuiControlParent.Controls.Add(m_permanentDeath);
			}
			myGuiControlParent.Controls.Add(control14);
			myGuiControlParent.Controls.Add(m_weaponsEnabled);
			if (flag)
			{
				myGuiControlParent.Controls.Add(control13);
				myGuiControlParent.Controls.Add(m_cargoShipsEnabled);
			}
			myGuiControlParent.Controls.Add(control8);
			myGuiControlParent.Controls.Add(m_destructibleBlocks);
			if (MyFakes.ENABLE_PROGRAMMABLE_BLOCK && flag)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel5);
				myGuiControlParent.Controls.Add(m_enableIngameScripts);
			}
			if (flag)
			{
				myGuiControlParent.Controls.Add(control11);
				myGuiControlParent.Controls.Add(m_enableToolShake);
			}
			if (flag)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel6);
				myGuiControlParent.Controls.Add(m_enableAdaptiveSimulationQuality);
			}
			myGuiControlParent.Controls.Add(control12);
			myGuiControlParent.Controls.Add(m_enableVoxelHand);
			myGuiControlParent.Controls.Add(control10);
			myGuiControlParent.Controls.Add(m_enableEncounters);
			myGuiControlParent.Controls.Add(control9);
			myGuiControlParent.Controls.Add(m_enable3rdPersonCamera);
			myGuiControlParent.Controls.Add(control20);
			myGuiControlParent.Controls.Add(m_enableOxygen);
			myGuiControlParent.Controls.Add(oxygenPressurizationLabel);
			myGuiControlParent.Controls.Add(m_enableOxygenPressurization);
			myGuiControlParent.Controls.Add(control29);
			myGuiControlParent.Controls.Add(m_enableConvertToStation);
			if (flag)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel8);
				myGuiControlParent.Controls.Add(m_enableStationVoxelSupport);
			}
			myGuiControlParent.Controls.Add(control32);
			myGuiControlParent.Controls.Add(m_enableJetpack);
			myGuiControlParent.Controls.Add(control33);
			myGuiControlParent.Controls.Add(m_spawnWithTools);
			myGuiControlParent.Controls.Add(control40);
			myGuiControlParent.Controls.Add(m_enableVoxelDestruction);
			myGuiControlParent.Controls.Add(control34);
			myGuiControlParent.Controls.Add(m_enableDrones);
			if (flag)
			{
				myGuiControlParent.Controls.Add(m_enableWolfsLabel);
				myGuiControlParent.Controls.Add(m_enableWolfs);
			}
			if (flag)
			{
				myGuiControlParent.Controls.Add(m_enableSpidersLabel);
				myGuiControlParent.Controls.Add(m_enableSpiders);
			}
			myGuiControlParent.Controls.Add(control35);
			myGuiControlParent.Controls.Add(m_enableRemoteBlockRemoval);
			if (flag)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel9);
				myGuiControlParent.Controls.Add(m_enableSubGridDamage);
			}
			myGuiControlParent.Controls.Add(control31);
			myGuiControlParent.Controls.Add(m_enableTurretsFriendlyFire);
			myGuiControlParent.Controls.Add(m_enableContainerDropsLabel);
			myGuiControlParent.Controls.Add(m_enableContainerDrops);
			myGuiControlParent.Controls.Add(control21);
			myGuiControlParent.Controls.Add(m_enableRespawnShips);
			myGuiControlParent.Controls.Add(control36);
			myGuiControlParent.Controls.Add(m_enableResearch);
			myGuiControlParent.Controls.Add(control37);
			myGuiControlParent.Controls.Add(m_enableAutoRespawn);
			if (flag)
			{
				myGuiControlParent.Controls.Add(myGuiControlLabel10);
				myGuiControlParent.Controls.Add(m_enableSupergridding);
			}
			myGuiControlParent.Controls.Add(control39);
			myGuiControlParent.Controls.Add(m_enableEconomy);
			myGuiControlParent.Controls.Add(control38);
			myGuiControlParent.Controls.Add(m_enableBountyContracts);
			float num4 = 0.018f;
			Vector2 value5 = new Vector2(num2 + num4 + 0.05f, 0f);
			int num5 = 2;
			_ = (value5.X * (float)num5 - 0.05f) / 2f;
			value4.X += num4;
			for (int i = count; i < myGuiControlParent.Controls.Count; i++)
			{
				MyGuiControlBase myGuiControlBase = myGuiControlParent.Controls[i];
				int num6 = (i - count) % 2;
				int num7 = (i - count) / 2 % num5;
				if (num6 == 0)
				{
					myGuiControlBase.Position = value3 + num7 * value5 + value2 * num;
					continue;
				}
				myGuiControlBase.Position = value4 + num7 * value5 + value2 * num;
				if (num7 == num5 - 1)
				{
					num++;
				}
			}
			Vector2 value6 = (m_size.Value / 2f - value) * new Vector2(0f, 1f);
			float num8 = 25f;
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OkButtonClicked);
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, CancelButtonClicked);
			m_okButton.Position = value6 + new Vector2(0f - num8, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_okButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_cancelButton.Position = value6 + new Vector2(num8, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_cancelButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			Controls.Add(myGuiControlScrollablePanel);
			base.CloseButtonEnabled = true;
		}

		private void CheckExperimental(MyGuiControlBase control, MyGuiControlLabel label, MyStringId toolTip, bool enabled = true)
		{
			if (MySandboxGame.Config.ExperimentalMode && MyFakes.IsExperimentalAllowed)
			{
				control.SetToolTip(MyTexts.GetString(toolTip));
			}
			else if (enabled)
			{
				control.SetEnabledByExperimental();
				label.SetEnabledByExperimental();
			}
			else
			{
				control.SetDisabledByExperimental();
				label.SetDisabledByExperimental();
			}
		}

		private MyGuiControlLabel MakeLabel(MyStringId textEnum)
		{
			return new MyGuiControlLabel(null, null, MyTexts.GetString(textEnum));
		}

		private void LoadValues()
		{
			if (m_isNewGame)
			{
				m_passwordTextbox.Text = "";
			}
			else
			{
				m_passwordTextbox.Text = m_parent.Checkpoint.Password;
			}
			SetSettings(m_parent.Settings);
		}

		private void CheckButton(float value, params MyGuiControlButton[] allButtons)
		{
			bool flag = false;
			foreach (MyGuiControlButton myGuiControlButton in allButtons)
			{
				if (myGuiControlButton.UserData is float)
				{
					if ((float)myGuiControlButton.UserData == value && !myGuiControlButton.Checked)
					{
						flag = true;
						myGuiControlButton.Checked = true;
					}
					else if ((float)myGuiControlButton.UserData != value && myGuiControlButton.Checked)
					{
						myGuiControlButton.Checked = false;
					}
				}
			}
			if (!flag)
			{
				allButtons[0].Checked = true;
			}
		}

		private void CheckButton(MyGuiControlButton active, params MyGuiControlButton[] allButtons)
		{
			foreach (MyGuiControlButton myGuiControlButton in allButtons)
			{
				if (myGuiControlButton == active && !myGuiControlButton.Checked)
				{
					myGuiControlButton.Checked = true;
				}
				else if (myGuiControlButton != active && myGuiControlButton.Checked)
				{
					myGuiControlButton.Checked = false;
				}
			}
		}

		public void UpdateSurvivalState(bool survivalEnabled)
		{
			m_creativeModeButton.Checked = !survivalEnabled;
			m_survivalModeButton.Checked = survivalEnabled;
			m_enableCopyPaste.Enabled = !survivalEnabled;
			m_enableCopyPasteLabel.Enabled = !survivalEnabled;
			m_enableContainerDrops.Enabled = survivalEnabled;
			m_enableContainerDropsLabel.Enabled = survivalEnabled;
		}

		private MyGameModeEnum GetGameMode()
		{
			if (!m_survivalModeButton.Checked)
			{
				return MyGameModeEnum.Creative;
			}
			return MyGameModeEnum.Survival;
		}

		private float GetMultiplier(params MyGuiControlButton[] buttons)
		{
			foreach (MyGuiControlButton myGuiControlButton in buttons)
			{
				if (myGuiControlButton.Checked && myGuiControlButton.UserData is float)
				{
					return (float)myGuiControlButton.UserData;
				}
			}
			return 1f;
		}

		private float GetInventoryMultiplier()
		{
			return m_charactersInventory.GetSelectedKey();
		}

		private float GetBlocksInventoryMultiplier()
		{
			return m_blocksInventory.GetSelectedKey();
		}

		private float GetRefineryMultiplier()
		{
			return m_refinery.GetSelectedKey();
		}

		private float GetAssemblerMultiplier()
		{
			return m_assembler.GetSelectedKey();
		}

		private float GetWelderMultiplier()
		{
			return (float)m_welder.GetSelectedKey() / 10f;
		}

		private float GetGrinderMultiplier()
		{
			return (float)m_grinder.GetSelectedKey() / 10f;
		}

		private float GetSpawnShipTimeMultiplier()
		{
			return (float)m_spawnShipTimeCombo.GetSelectedKey() / 10f;
		}

		public int GetWorldSize()
		{
			_ = m_parent.Settings.WorldSizeKm;
			long selectedKey = m_worldSizeCombo.GetSelectedKey();
			if ((ulong)selectedKey <= 5uL)
			{
				switch (selectedKey)
				{
				case 0L:
					return 10;
				case 1L:
					return 20;
				case 2L:
					return 50;
				case 3L:
					return 100;
				case 4L:
					return 0;
				case 5L:
					return m_customWorldSize;
				}
			}
			return 0;
		}

		private MyWorldSizeEnum WorldSizeEnumKey(int worldSize)
		{
			switch (worldSize)
			{
			case 0:
				return MyWorldSizeEnum.UNLIMITED;
			case 10:
				return MyWorldSizeEnum.TEN_KM;
			case 20:
				return MyWorldSizeEnum.TWENTY_KM;
			case 50:
				return MyWorldSizeEnum.FIFTY_KM;
			case 100:
				return MyWorldSizeEnum.HUNDRED_KM;
			default:
				m_worldSizeCombo.AddItem(5L, MySpaceTexts.WorldSettings_WorldSizeCustom);
				m_customWorldSize = worldSize;
				return MyWorldSizeEnum.CUSTOM;
			}
		}

		public int GetViewDistance()
		{
			long selectedKey = m_viewDistanceCombo.GetSelectedKey();
			if (selectedKey == 0L)
			{
				return m_customViewDistance;
			}
			return (int)selectedKey;
		}

		private MyViewDistanceEnum ViewDistanceEnumKey(int viewDistance)
		{
			if (viewDistance != 0 && Enum.IsDefined(typeof(MyViewDistanceEnum), (MyViewDistanceEnum)viewDistance))
			{
				return (MyViewDistanceEnum)viewDistance;
			}
			m_viewDistanceCombo.AddItem(5L, MySpaceTexts.WorldSettings_ViewDistance_Custom);
			m_viewDistanceCombo.SelectItemByKey(5L);
			m_customViewDistance = viewDistance;
			return MyViewDistanceEnum.CUSTOM;
		}

		public void GetSettings(MyObjectBuilder_SessionSettings output)
		{
			output.OnlineMode = (MyOnlineModeEnum)m_onlineMode.GetSelectedKey();
			output.EnvironmentHostility = (MyEnvironmentHostilityEnum)m_environment.GetSelectedKey();
			switch (AsteroidAmount)
			{
			case -5:
				output.ProceduralDensity = 0.1f;
				break;
			case -1:
				output.ProceduralDensity = 0.25f;
				break;
			case -2:
				output.ProceduralDensity = 0.35f;
				break;
			case -3:
				output.ProceduralDensity = 0.5f;
				break;
			case -4:
				output.ProceduralDensity = 0f;
				break;
			default:
				throw new InvalidBranchException();
			}
			output.AutoHealing = m_autoHealing.IsChecked;
			output.CargoShipsEnabled = m_cargoShipsEnabled.IsChecked;
			output.EnableCopyPaste = m_enableCopyPaste.IsChecked;
			output.EnableSpectator = m_enableSpectator.IsChecked;
			output.ResetOwnership = m_resetOwnership.IsChecked;
			output.PermanentDeath = m_permanentDeath.IsChecked;
			output.DestructibleBlocks = m_destructibleBlocks.IsChecked;
			output.EnableIngameScripts = m_enableIngameScripts.IsChecked;
			output.Enable3rdPersonView = m_enable3rdPersonCamera.IsChecked;
			output.EnableEncounters = m_enableEncounters.IsChecked;
			output.EnableToolShake = m_enableToolShake.IsChecked;
			output.AdaptiveSimulationQuality = m_enableAdaptiveSimulationQuality.IsChecked;
			output.EnableVoxelHand = m_enableVoxelHand.IsChecked;
			output.ShowPlayerNamesOnHud = m_showPlayerNamesOnHud.IsChecked;
			output.ThrusterDamage = m_thrusterDamage.IsChecked;
			output.WeaponsEnabled = m_weaponsEnabled.IsChecked;
			output.EnableOxygen = m_enableOxygen.IsChecked;
			if (output.EnableOxygen && output.VoxelGeneratorVersion < 1)
			{
				output.VoxelGeneratorVersion = 1;
			}
			output.EnableOxygenPressurization = m_enableOxygenPressurization.IsChecked;
			output.RespawnShipDelete = m_respawnShipDelete.IsChecked;
			output.EnableConvertToStation = m_enableConvertToStation.IsChecked;
			output.StationVoxelSupport = m_enableStationVoxelSupport.IsChecked;
			output.EnableAutorespawn = m_enableAutoRespawn.IsChecked;
			output.EnableSupergridding = m_enableSupergridding.IsChecked;
			output.EnableEconomy = m_enableEconomy.IsChecked;
			output.EnableBountyContracts = m_enableBountyContracts.IsChecked;
			output.EnableRespawnShips = m_enableRespawnShips.IsChecked;
			output.EnableWolfs = m_enableWolfs.IsChecked;
			output.EnableSunRotation = m_enableSunRotation.IsChecked;
			output.EnableJetpack = m_enableJetpack.IsChecked;
			output.SpawnWithTools = m_spawnWithTools.IsChecked;
			output.EnableVoxelDestruction = m_enableVoxelDestruction.IsChecked;
			output.EnableDrones = m_enableDrones.IsChecked;
			output.EnableTurretsFriendlyFire = m_enableTurretsFriendlyFire.IsChecked;
			output.EnableSubgridDamage = m_enableSubGridDamage.IsChecked;
			output.EnableSpiders = m_enableSpiders.IsChecked;
			output.EnableRemoteBlockRemoval = m_enableRemoteBlockRemoval.IsChecked;
			output.EnableResearch = m_enableResearch.IsChecked;
			output.MaxPlayers = (short)m_maxPlayersSlider.Value;
			output.MaxFloatingObjects = (short)m_maxFloatingObjectsSlider.Value;
			output.MaxBackupSaves = (short)m_maxBackupSavesSlider.Value;
			output.MaxGridSize = (int)(m_maxGridSizeSlider.Value - m_maxGridSizeSlider.Value % 100f);
			output.MaxBlocksPerPlayer = (int)(m_maxBlocksPerPlayerSlider.Value - m_maxBlocksPerPlayerSlider.Value % 100f);
			output.TotalPCU = (int)(m_totalPCUSlider.Value - m_totalPCUSlider.Value % 100f);
			output.OptimalSpawnDistance = (int)(m_optimalSpawnDistanceSlider.Value - m_optimalSpawnDistanceSlider.Value % 100f);
			output.BlockLimitsEnabled = (m_blockLimits.IsChecked ? MyBlockLimitsEnabledEnum.GLOBALLY : MyBlockLimitsEnabledEnum.NONE);
			output.SunRotationIntervalMinutes = MathHelper.Clamp(MathHelper.InterpLog(m_sunRotationIntervalSlider.Value, 1f, 1440f), 1f, 1440f);
			output.AssemblerEfficiencyMultiplier = GetAssemblerMultiplier();
			output.AssemblerSpeedMultiplier = GetAssemblerMultiplier();
			output.InventorySizeMultiplier = GetInventoryMultiplier();
			output.BlocksInventorySizeMultiplier = GetBlocksInventoryMultiplier();
			output.RefinerySpeedMultiplier = GetRefineryMultiplier();
			output.WelderSpeedMultiplier = GetWelderMultiplier();
			output.GrinderSpeedMultiplier = GetGrinderMultiplier();
			output.SpawnShipTimeMultiplier = GetSpawnShipTimeMultiplier();
			output.RealisticSound = ((int)m_soundModeCombo.GetSelectedKey() == 1);
			output.EnvironmentHostility = (MyEnvironmentHostilityEnum)m_environment.GetSelectedKey();
			output.WorldSizeKm = GetWorldSize();
			output.ViewDistance = GetViewDistance();
			output.PhysicsIterations = (int)m_physicsOptionsCombo.GetSelectedKey();
			output.GameMode = GetGameMode();
			if (output.GameMode != 0)
			{
				output.EnableContainerDrops = m_enableContainerDrops.IsChecked;
			}
		}

		public void SetSettings(MyObjectBuilder_SessionSettings settings)
		{
			m_onlineMode.SelectItemByKey((long)settings.OnlineMode);
			m_environment.SelectItemByKey((long)settings.EnvironmentHostility);
			m_worldSizeCombo.SelectItemByKey((long)WorldSizeEnumKey(settings.WorldSizeKm));
			m_spawnShipTimeCombo.SelectItemByKey((int)(settings.SpawnShipTimeMultiplier * 10f));
			m_viewDistanceCombo.SelectItemByKey((long)ViewDistanceEnumKey(settings.ViewDistance));
			m_soundModeCombo.SelectItemByKey(settings.RealisticSound ? 1 : 0);
			m_asteroidAmountCombo.SelectItemByKey((int)settings.ProceduralDensity);
			switch ((int)(settings.ProceduralDensity * 100f))
			{
			case 10:
				m_asteroidAmountCombo.SelectItemByKey(-5L);
				break;
			case 25:
				m_asteroidAmountCombo.SelectItemByKey(-1L);
				break;
			case 35:
				m_asteroidAmountCombo.SelectItemByKey(-2L);
				break;
			case 50:
				m_asteroidAmountCombo.SelectItemByKey(-3L);
				break;
			case 0:
				m_asteroidAmountCombo.SelectItemByKey(-4L);
				break;
			default:
				m_asteroidAmountCombo.SelectItemByKey(-1L);
				break;
			}
			m_environment.SelectItemByKey((long)settings.EnvironmentHostility);
			if (m_physicsOptionsCombo.TryGetItemByKey(settings.PhysicsIterations) != null)
			{
				m_physicsOptionsCombo.SelectItemByKey(settings.PhysicsIterations);
			}
			else
			{
				m_physicsOptionsCombo.SelectItemByKey(4L);
			}
			m_autoHealing.IsChecked = settings.AutoHealing;
			m_cargoShipsEnabled.IsChecked = settings.CargoShipsEnabled;
			m_enableCopyPaste.IsChecked = settings.EnableCopyPaste;
			m_enableSpectator.IsChecked = settings.EnableSpectator;
			m_resetOwnership.IsChecked = settings.ResetOwnership;
			m_permanentDeath.IsChecked = settings.PermanentDeath.Value;
			m_destructibleBlocks.IsChecked = settings.DestructibleBlocks;
			m_enableEncounters.IsChecked = settings.EnableEncounters;
			m_enable3rdPersonCamera.IsChecked = settings.Enable3rdPersonView;
			m_enableIngameScripts.IsChecked = settings.EnableIngameScripts;
			m_enableToolShake.IsChecked = settings.EnableToolShake;
			m_enableAdaptiveSimulationQuality.IsChecked = settings.AdaptiveSimulationQuality;
			m_enableVoxelHand.IsChecked = settings.EnableVoxelHand;
			m_showPlayerNamesOnHud.IsChecked = settings.ShowPlayerNamesOnHud;
			m_thrusterDamage.IsChecked = settings.ThrusterDamage;
			m_weaponsEnabled.IsChecked = settings.WeaponsEnabled;
			m_enableOxygen.IsChecked = settings.EnableOxygen;
			if (settings.VoxelGeneratorVersion < 1)
			{
				m_showWarningForOxygen = true;
			}
			m_enableOxygenPressurization.IsChecked = settings.EnableOxygenPressurization;
			m_enableRespawnShips.IsChecked = settings.EnableRespawnShips;
			m_respawnShipDelete.IsChecked = settings.RespawnShipDelete;
			m_enableConvertToStation.IsChecked = settings.EnableConvertToStation;
			m_enableStationVoxelSupport.IsChecked = settings.StationVoxelSupport;
			m_enableSunRotation.IsChecked = settings.EnableSunRotation;
			m_enableJetpack.IsChecked = settings.EnableJetpack;
			m_spawnWithTools.IsChecked = settings.SpawnWithTools;
			m_enableAutoRespawn.IsChecked = settings.EnableAutorespawn;
			m_enableSupergridding.IsChecked = settings.EnableSupergridding;
			m_enableEconomy.IsChecked = settings.EnableEconomy;
			m_enableBountyContracts.IsChecked = settings.EnableBountyContracts;
			m_sunRotationIntervalSlider.Enabled = m_enableSunRotation.IsChecked;
			m_sunRotationPeriodValue.Visible = m_enableSunRotation.IsChecked;
			m_sunRotationIntervalSlider.Value = 0.03f;
			m_sunRotationIntervalSlider.Value = MathHelper.Clamp(MathHelper.InterpLogInv(settings.SunRotationIntervalMinutes, 1f, 1440f), 0f, 1f);
			m_maxPlayersSlider.Value = settings.MaxPlayers;
			m_maxFloatingObjectsSlider.Value = settings.MaxFloatingObjects;
			m_maxGridSizeSlider.Value = settings.MaxGridSize;
			m_maxBlocksPerPlayerSlider.Value = settings.MaxBlocksPerPlayer;
			m_blockLimits.IsChecked = (settings.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.NONE);
			m_blockLimits.IsCheckedChanged = blockLimits_CheckedChanged;
			blockLimits_CheckedChanged(m_blockLimits);
			if (MySandboxGame.Config.ExperimentalMode && MyFakes.IsExperimentalAllowed)
			{
				m_totalPCUSlider.MinValue = 0f;
				m_totalPCUSlider.MaxValue = 1000000f;
			}
			else
			{
				m_totalPCUSlider.MinValue = 100f;
				if (settings.OnlineMode != 0)
				{
					m_totalPCUSlider.MaxValue = 50000f;
				}
				else
				{
					m_totalPCUSlider.MaxValue = 100000f;
				}
			}
			m_totalPCUSlider.Value = settings.TotalPCU;
			MyGuiControlSlider optimalSpawnDistanceSlider = m_optimalSpawnDistanceSlider;
			float value = m_optimalSpawnDistanceSlider.Value = settings.OptimalSpawnDistance;
			optimalSpawnDistanceSlider.DefaultValue = value;
			m_onlineMode.ItemSelected += OnOnlineModeItemSelected;
			OnOnlineModeItemSelected();
			m_maxBackupSavesSlider.Value = settings.MaxBackupSaves;
			m_enableSubGridDamage.IsChecked = settings.EnableSubgridDamage;
			m_enableTurretsFriendlyFire.IsChecked = settings.EnableTurretsFriendlyFire;
			m_enableVoxelDestruction.IsChecked = settings.EnableVoxelDestruction;
			m_enableDrones.IsChecked = settings.EnableDrones;
			m_enableWolfs.IsChecked = settings.EnableWolfs;
			m_enableSpiders.IsChecked = settings.EnableSpiders;
			m_enableRemoteBlockRemoval.IsChecked = settings.EnableRemoteBlockRemoval;
			m_enableResearch.IsChecked = settings.EnableResearch;
			if (settings.GameMode == MyGameModeEnum.Creative)
			{
				m_enableContainerDrops.IsChecked = false;
			}
			else
			{
				m_enableContainerDrops.IsChecked = settings.EnableContainerDrops;
			}
			m_assembler.SelectItemByKey((int)settings.AssemblerEfficiencyMultiplier);
			m_charactersInventory.SelectItemByKey((int)settings.InventorySizeMultiplier);
			m_blocksInventory.SelectItemByKey((int)settings.BlocksInventorySizeMultiplier);
			m_refinery.SelectItemByKey((int)settings.RefinerySpeedMultiplier);
			m_welder.SelectItemByKey((int)(settings.WelderSpeedMultiplier * 10f));
			m_grinder.SelectItemByKey((int)(settings.GrinderSpeedMultiplier * 10f));
			UpdateSurvivalState(settings.GameMode == MyGameModeEnum.Survival);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenAdvancedWorldSettings";
		}

		private void CancelButtonClicked(object sender)
		{
			CloseScreen();
		}

		private void OkButtonClicked(object sender)
		{
			m_isConfirmed = true;
			if (this.OnOkButtonClicked != null)
			{
				this.OnOkButtonClicked();
			}
			CloseScreen();
		}

		private void CreativeClicked(object sender)
		{
			UpdateSurvivalState(survivalEnabled: false);
			m_enableContainerDrops.IsChecked = false;
		}

		private void SurvivalClicked(object sender)
		{
			UpdateSurvivalState(survivalEnabled: true);
			m_enableContainerDrops.IsChecked = m_survivalModeButton.Checked;
		}

		private void m_soundModeCombo_ItemSelected()
		{
			if (m_soundModeCombo.GetSelectedIndex() == 1)
			{
				m_parent.Settings.EnableOxygenPressurization = true;
			}
		}

		private void m_asteroidAmountCombo_ItemSelected()
		{
			m_asteroidAmount = (int)m_asteroidAmountCombo.GetSelectedKey();
		}

		private void HostilityChanged()
		{
			m_isHostilityChanged = true;
		}

		private void blockLimits_CheckedChanged(MyGuiControlCheckbox checkbox)
		{
			if (!checkbox.IsChecked)
			{
				if (!m_recreating_control)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextBlockLimitDisableWarning), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
				}
				m_maxGridSizeSlider.Value = 0f;
				m_maxGridSizeSlider.Enabled = false;
				m_maxBlocksPerPlayerSlider.Value = 0f;
				m_maxBlocksPerPlayerSlider.Enabled = false;
				m_totalPCUSlider.Value = 0f;
				m_totalPCUSlider.Enabled = false;
			}
			else
			{
				m_maxBlocksPerPlayerSlider.Value = 100000f;
				m_maxBlocksPerPlayerSlider.Enabled = true;
				m_maxGridSizeSlider.Value = 50000f;
				m_maxGridSizeSlider.Enabled = true;
				m_totalPCUSlider.Value = 100000f;
				m_totalPCUSlider.Enabled = true;
			}
		}

		private void OnOnlineModeItemSelected()
		{
			if ((int)m_onlineMode.GetSelectedKey() == 0)
			{
				m_optimalSpawnDistanceSlider.Value = m_optimalSpawnDistanceSlider.MinValue;
				m_optimalSpawnDistanceSlider.Enabled = false;
			}
			else
			{
				m_optimalSpawnDistanceSlider.Enabled = true;
				m_optimalSpawnDistanceSlider.Value = m_optimalSpawnDistanceSlider.DefaultValue.GetValueOrDefault(4000f);
			}
		}
	}
}
