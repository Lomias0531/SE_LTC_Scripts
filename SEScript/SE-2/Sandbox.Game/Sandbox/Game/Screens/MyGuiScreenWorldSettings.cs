using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenWorldSettings : MyGuiScreenBase
	{
		public static MyGuiScreenWorldSettings Static;

		internal MyGuiScreenAdvancedWorldSettings Advanced;

		internal MyGuiScreenWorldGeneratorSettings WorldGenerator;

		internal MyGuiScreenMods ModsScreen;

		protected bool m_isNewGame;

		private string m_sessionPath;

		protected MyObjectBuilder_SessionSettings m_settings;

		private MyGuiScreenWorldSettings m_parent;

		private List<MyObjectBuilder_Checkpoint.ModItem> m_mods;

		private MyObjectBuilder_Checkpoint m_checkpoint;

		private MyGuiControlTextbox m_nameTextbox;

		private MyGuiControlTextbox m_descriptionTextbox;

		private MyGuiControlCombobox m_onlineMode;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlButton m_survivalModeButton;

		private MyGuiControlButton m_creativeModeButton;

		private MyGuiControlButton m_worldGeneratorButton;

		private MyGuiControlSlider m_maxPlayersSlider;

		private MyGuiControlCheckbox m_autoSave;

		private MyGuiControlLabel m_maxPlayersLabel;

		private MyGuiControlLabel m_autoSaveLabel;

		private MyGuiControlList m_scenarioTypesList;

		private MyGuiControlRadioButtonGroup m_scenarioTypesGroup;

		private MyGuiControlRotatingWheel m_asyncLoadingWheel;

		private IMyAsyncResult m_loadingTask;

		private MyGuiControlRadioButton m_selectedButton;

		private bool m_displayTabScenario;

		private bool m_displayTabWorkshop;

		private bool m_displayTabCustom;

		private float MARGIN_TOP = 0.22f;

		private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;

		private float MARGIN_LEFT_INFO = 29.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private bool m_descriptionChanged;

		public MyObjectBuilder_SessionSettings Settings
		{
			get
			{
				GetSettingsFromControls();
				return m_settings;
			}
		}

		public MyObjectBuilder_Checkpoint Checkpoint => m_checkpoint;

		public MyGuiScreenWorldSettings(bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
			: this(null, null, displayTabScenario, displayTabWorkshop, displayTabCustom)
		{
		}

		public MyGuiScreenWorldSettings(MyObjectBuilder_Checkpoint checkpoint, string path, bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, CalcSize(checkpoint), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_displayTabScenario = displayTabScenario;
			m_displayTabWorkshop = displayTabWorkshop;
			m_displayTabCustom = displayTabCustom;
			MySandboxGame.Log.WriteLine("MyGuiScreenWorldSettings.ctor START");
			base.EnabledBackgroundFade = true;
			Static = this;
			m_checkpoint = checkpoint;
			if (checkpoint == null || checkpoint.Mods == null)
			{
				m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();
			}
			else
			{
				m_mods = checkpoint.Mods;
			}
			m_sessionPath = path;
			m_isNewGame = (checkpoint == null);
			RecreateControls(constructor: true);
			MySandboxGame.Log.WriteLine("MyGuiScreenWorldSettings.ctor END");
		}

		public static Vector2 CalcSize(MyObjectBuilder_Checkpoint checkpoint)
		{
			float x = (checkpoint == null) ? 0.878f : (183f / 280f);
			float y = (checkpoint == null) ? 0.97f : 0.9398855f;
			return new Vector2(x, y);
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool CloseScreen()
		{
			if (WorldGenerator != null)
			{
				WorldGenerator.CloseScreen();
			}
			WorldGenerator = null;
			if (Advanced != null)
			{
				Advanced.CloseScreen();
			}
			Advanced = null;
			if (ModsScreen != null)
			{
				ModsScreen.CloseScreen();
			}
			ModsScreen = null;
			Static = null;
			return base.CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWorldSettings";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			BuildControls();
			if (m_isNewGame)
			{
				SetDefaultValues();
				new MyGuiControlScreenSwitchPanel(this, MyTexts.Get(MyCommonTexts.WorldSettingsScreen_Description), m_displayTabScenario, m_displayTabWorkshop, m_displayTabCustom);
			}
			else
			{
				LoadValues();
				m_nameTextbox.MoveCarriageToEnd();
				m_descriptionTextbox.MoveCarriageToEnd();
			}
		}

		private void InitCampaignList()
		{
			if (MyDefinitionManager.Static.GetScenarioDefinitions().Count == 0)
			{
				MyDefinitionManager.Static.LoadScenarios();
			}
			Vector2 position = -m_size.Value / 2f + new Vector2(MARGIN_LEFT_LIST, MARGIN_TOP);
			m_scenarioTypesGroup = new MyGuiControlRadioButtonGroup();
			m_scenarioTypesGroup.SelectedChanged += scenario_SelectedChanged;
			m_scenarioTypesGroup.MouseDoubleClick += delegate
			{
				OnOkButtonClick(null);
			};
			m_scenarioTypesList = new MyGuiControlList
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, m_size.Value.Y - MARGIN_TOP - 0.048f)
			};
		}

		protected virtual void BuildControls()
		{
			if (m_isNewGame)
			{
				AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
			}
			else
			{
				AddCaption(MyCommonTexts.ScreenCaptionEditSettings);
			}
			int num = 0;
			if (m_isNewGame)
			{
				InitCampaignList();
			}
			Vector2 value = new Vector2(0f, 0.052f);
			Vector2 vector = -m_size.Value / 2f + new Vector2(m_isNewGame ? (MARGIN_LEFT_LIST + m_scenarioTypesList.Size.X + MARGIN_LEFT_INFO) : MARGIN_LEFT_LIST, m_isNewGame ? (MARGIN_TOP + 0.015f) : (MARGIN_TOP - 0.105f));
			Vector2 vector2 = m_size.Value / 2f - vector;
			vector2.X -= MARGIN_RIGHT + 0.005f;
			vector2.Y -= MARGIN_BOTTOM;
			Vector2 value2 = vector2 * (m_isNewGame ? 0.339f : 0.329f);
			Vector2 value3 = vector + new Vector2(value2.X, 0f);
			Vector2 vector3 = vector2 - value2;
			MyGuiControlLabel control = MakeLabel(MyCommonTexts.Name);
			MyGuiControlLabel control2 = MakeLabel(MyCommonTexts.Description);
			MyGuiControlLabel control3 = MakeLabel(MyCommonTexts.WorldSettings_GameMode);
			MyGuiControlLabel control4 = MakeLabel(MyCommonTexts.WorldSettings_OnlineMode);
			m_maxPlayersLabel = MakeLabel(MyCommonTexts.MaxPlayers);
			m_autoSaveLabel = MakeLabel(MyCommonTexts.WorldSettings_AutoSave);
			m_nameTextbox = new MyGuiControlTextbox(null, null, 128);
			m_descriptionTextbox = new MyGuiControlTextbox(null, null, 7999);
			m_descriptionTextbox.TextChanged += OnDescriptionChanged;
			m_onlineMode = new MyGuiControlCombobox(null, new Vector2(vector3.X, 0.04f));
			m_maxPlayersSlider = new MyGuiControlSlider(Vector2.Zero, 2f, width: m_onlineMode.Size.X, maxValue: MyMultiplayerLobby.MAX_PLAYERS, defaultValue: null, color: null, labelText: new StringBuilder("{0}").ToString(), labelDecimalPlaces: 0, labelScale: 0.8f, labelSpaceWidth: 0.028f, labelFont: "White", toolTip: null, visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true, showLabel: true);
			m_autoSave = new MyGuiControlCheckbox();
			m_autoSave.SetToolTip(new StringBuilder().AppendFormat(MyCommonTexts.ToolTipWorldSettingsAutoSave, 5u).ToString());
			m_creativeModeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Small, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCreativeClick);
			m_creativeModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeCreative);
			m_survivalModeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Small, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnSurvivalClick);
			m_survivalModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeSurvival);
			m_onlineMode.ItemSelected += OnOnlineModeSelect;
			m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
			m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
			m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
			m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
			if (m_isNewGame)
			{
				if (MyDefinitionManager.Static.GetScenarioDefinitions().Count == 0)
				{
					MyDefinitionManager.Static.LoadScenarios();
				}
				m_scenarioTypesGroup = new MyGuiControlRadioButtonGroup();
				m_scenarioTypesGroup.SelectedChanged += scenario_SelectedChanged;
				m_scenarioTypesGroup.MouseDoubleClick += OnOkButtonClick;
				m_asyncLoadingWheel = new MyGuiControlRotatingWheel(new Vector2(m_size.Value.X / 2f - 0.077f, (0f - m_size.Value.Y) / 2f + 0.108f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
				m_loadingTask = StartLoadingWorldInfos();
			}
			m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), 5, 128));
			m_nameTextbox.FocusChanged += NameFocusChanged;
			m_descriptionTextbox.SetToolTip(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsDescription));
			m_descriptionTextbox.FocusChanged += DescriptionFocusChanged;
			m_onlineMode.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsOnlineMode), MySession.GameServiceName));
			m_onlineMode.HideToolTip();
			m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
			m_worldGeneratorButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.WorldSettings_WorldGenerator), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnWorldGeneratorClick);
			Controls.Add(control);
			Controls.Add(m_nameTextbox);
			Controls.Add(control2);
			Controls.Add(m_descriptionTextbox);
			Controls.Add(control3);
			Controls.Add(m_creativeModeButton);
			Controls.Add(control4);
			Controls.Add(m_onlineMode);
			Controls.Add(m_maxPlayersLabel);
			Controls.Add(m_maxPlayersSlider);
			Controls.Add(m_autoSaveLabel);
			Controls.Add(m_autoSave);
			Vector2 vector4 = m_size.Value / 2f;
			vector4.X -= MARGIN_RIGHT + 0.004f;
			vector4.Y -= MARGIN_BOTTOM + 0.004f;
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			MyGuiControlButton myGuiControlButton = null;
			MyGuiControlButton myGuiControlButton2 = null;
			myGuiControlButton = new MyGuiControlButton(vector4 - new Vector2(bACK_BUTTON_SIZE.X + 0.0245f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MySpaceTexts.WorldSettings_Advanced), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGameCustomGame_Advanced), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnAdvancedClick);
			myGuiControlButton2 = new MyGuiControlButton(vector4 - new Vector2(bACK_BUTTON_SIZE.X * 2f + 0.049f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.WorldSettings_Mods), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGameCustomGame_Mods), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnModsClick);
			if (MyFakes.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
			{
				Controls.Add(myGuiControlButton2);
			}
			Controls.Add(myGuiControlButton);
			myGuiControlButton2.SetEnabledByExperimental();
			foreach (MyGuiControlBase control5 in Controls)
			{
				control5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				if (control5 is MyGuiControlLabel)
				{
					control5.Position = vector + value * num;
				}
				else
				{
					control5.Position = value3 + value * num++;
				}
			}
			if (m_isNewGame)
			{
				Controls.Add(m_scenarioTypesList);
			}
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			if (m_isNewGame)
			{
				myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.38f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.625f);
			}
			else
			{
				myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.835f);
				myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			}
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlButton myGuiControlButton3;
			if (m_isNewGame)
			{
				myGuiControlButton3 = new MyGuiControlButton(vector4, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Start), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClick);
				myGuiControlButton3.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
			}
			else
			{
				myGuiControlButton3 = new MyGuiControlButton(vector4, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClick);
				myGuiControlButton3.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			}
			Controls.Add(myGuiControlButton3);
			Controls.Add(m_survivalModeButton);
			m_survivalModeButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_creativeModeButton.PositionX += 0.0025f;
			m_creativeModeButton.PositionY += 0.005f;
			m_survivalModeButton.Position = m_creativeModeButton.Position + new Vector2(m_onlineMode.Size.X + 0.0005f, 0f);
			m_nameTextbox.Size = m_onlineMode.Size;
			m_descriptionTextbox.Size = m_nameTextbox.Size;
			m_maxPlayersSlider.PositionX = m_nameTextbox.PositionX - 0.001f;
			m_autoSave.PositionX = m_maxPlayersSlider.PositionX;
			float num2 = 0.007f;
			if (myGuiControlButton2 != null)
			{
				myGuiControlButton2.PositionX = m_maxPlayersSlider.PositionX + 0.003f;
				myGuiControlButton2.PositionY += num2;
			}
			if (myGuiControlButton != null)
			{
				myGuiControlButton.PositionX += 0.0045f + myGuiControlButton2.Size.X + 0.01f;
				if (MyFakes.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
				{
					myGuiControlButton.PositionY = myGuiControlButton2.Position.Y;
				}
				else
				{
					myGuiControlButton.PositionY += num2;
				}
			}
			if (m_isNewGame)
			{
				Controls.Add(m_asyncLoadingWheel);
			}
			base.CloseButtonEnabled = true;
		}

		private void OnDescriptionChanged(MyGuiControlTextbox obj)
		{
			m_descriptionChanged = true;
		}

		private void NameFocusChanged(MyGuiControlBase obj, bool focused)
		{
			if (focused && !m_nameTextbox.IsImeActive)
			{
				m_nameTextbox.SelectAll();
				m_nameTextbox.MoveCarriageToEnd();
			}
		}

		private void DescriptionFocusChanged(MyGuiControlBase obj, bool focused)
		{
			if (focused && !m_descriptionTextbox.IsImeActive)
			{
				m_descriptionTextbox.SelectAll();
				m_descriptionTextbox.MoveCarriageToEnd();
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (m_loadingTask != null && m_loadingTask.IsCompleted)
			{
				OnLoadingFinished(m_loadingTask, null);
				m_loadingTask = null;
				m_asyncLoadingWheel.Visible = false;
			}
			return base.Update(hasFocus);
		}

		private void scenario_SelectedChanged(MyGuiControlRadioButtonGroup group)
		{
			SetDefaultName();
			if (MyFakes.ENABLE_PLANETS || MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				m_worldGeneratorButton.Enabled = true;
				if (m_worldGeneratorButton.Enabled && WorldGenerator != null)
				{
					WorldGenerator.GetSettings(m_settings);
				}
			}
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(group.SelectedButton.UserData as string, out sizeInBytes);
			if (myObjectBuilder_Checkpoint != null)
			{
				m_settings = CopySettings(myObjectBuilder_Checkpoint.Settings);
				SetSettingsToControls();
			}
			if (m_selectedButton != null)
			{
				m_selectedButton.HighlightType = MyGuiControlHighlightType.WHEN_ACTIVE;
			}
			m_selectedButton = group.SelectedButton;
			m_selectedButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
			m_selectedButton.HasHighlight = true;
		}

		private MyGuiControlLabel MakeLabel(MyStringId textEnum)
		{
			return new MyGuiControlLabel(null, null, MyTexts.GetString(textEnum));
		}

		private void SetDefaultName()
		{
			if (m_scenarioTypesGroup.SelectedButton != null)
			{
				string title = ((MyGuiControlContentButton)m_scenarioTypesGroup.SelectedButton).Title;
				m_nameTextbox.Text = title.ToString() + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
				m_descriptionTextbox.Text = string.Empty;
			}
		}

		private void LoadValues()
		{
			m_nameTextbox.Text = (m_checkpoint.SessionName ?? "");
			m_descriptionTextbox.TextChanged -= OnDescriptionChanged;
			m_descriptionTextbox.Text = (string.IsNullOrEmpty(m_checkpoint.Description) ? "" : MyTexts.SubstituteTexts(m_checkpoint.Description));
			m_descriptionTextbox.TextChanged += OnDescriptionChanged;
			m_descriptionChanged = false;
			m_settings = CopySettings(m_checkpoint.Settings);
			m_mods = m_checkpoint.Mods;
			SetSettingsToControls();
		}

		private void SetDefaultValues()
		{
			m_settings = GetDefaultSettings();
			m_settings.EnableToolShake = true;
			m_settings.EnableSunRotation = (MyPerGameSettings.Game == GameEnum.SE_GAME);
			m_settings.VoxelGeneratorVersion = 4;
			m_settings.EnableOxygen = true;
			m_settings.CargoShipsEnabled = true;
			m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();
			SetSettingsToControls();
			SetDefaultName();
		}

		protected virtual MyObjectBuilder_SessionSettings GetDefaultSettings()
		{
			return MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
		}

		protected virtual MyObjectBuilder_SessionSettings CopySettings(MyObjectBuilder_SessionSettings source)
		{
			return source.Clone() as MyObjectBuilder_SessionSettings;
		}

		private void OnOnlineModeSelect()
		{
			bool flag = m_onlineMode.GetSelectedKey() == 0;
			m_maxPlayersSlider.Enabled = !flag;
			m_maxPlayersLabel.Enabled = !flag;
			if (!flag && !MySandboxGame.Config.ExperimentalMode)
			{
				m_settings.TotalPCU = Math.Min(m_settings.TotalPCU, 50000);
			}
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

		private void OnCreativeClick(object sender)
		{
			UpdateSurvivalState(survivalEnabled: false);
			Settings.EnableCopyPaste = true;
		}

		private void OnSurvivalClick(object sender)
		{
			UpdateSurvivalState(survivalEnabled: true);
			Settings.EnableCopyPaste = false;
		}

		private void OnAdvancedClick(object sender)
		{
			Advanced = new MyGuiScreenAdvancedWorldSettings(this);
			Advanced.UpdateSurvivalState(GetGameMode() == MyGameModeEnum.Survival);
			Advanced.OnOkButtonClicked += Advanced_OnOkButtonClicked;
			MyGuiSandbox.AddScreen(Advanced);
		}

		private void OnWorldGeneratorClick(object sender)
		{
			WorldGenerator = new MyGuiScreenWorldGeneratorSettings(this);
			WorldGenerator.OnOkButtonClicked += WorldGenerator_OnOkButtonClicked;
			MyGuiSandbox.AddScreen(WorldGenerator);
		}

		private void WorldGenerator_OnOkButtonClicked()
		{
			WorldGenerator.GetSettings(m_settings);
			SetSettingsToControls();
		}

		private void OnModsClick(object sender)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenMods(m_mods));
		}

		private void UpdateSurvivalState(bool survivalEnabled)
		{
			m_creativeModeButton.Checked = !survivalEnabled;
			m_survivalModeButton.Checked = survivalEnabled;
		}

		private void Advanced_OnOkButtonClicked()
		{
			Advanced.GetSettings(m_settings);
			SetSettingsToControls();
		}

		private void OnOkButtonClick(object sender)
		{
			bool flag = m_nameTextbox.Text.ToString().Replace(':', '-').IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
			if (flag || m_nameTextbox.Text.Length < 5 || m_nameTextbox.Text.Length > 128)
			{
				MyStringId id = flag ? MyCommonTexts.ErrorNameInvalid : ((m_nameTextbox.Text.Length >= 5) ? MyCommonTexts.ErrorNameTooLong : MyCommonTexts.ErrorNameTooShort);
				MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(id), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox.SkipTransition = true;
				myGuiScreenMessageBox.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
				return;
			}
			if (m_descriptionTextbox.Text.Length > 7999)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.ErrorDescriptionTooLong), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox2.SkipTransition = true;
				myGuiScreenMessageBox2.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox2);
				return;
			}
			GetSettingsFromControls();
			if (m_settings.OnlineMode != 0 && !MyGameService.IsActive)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox3 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.ErrorStartSessionNoUser), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox3.SkipTransition = true;
				myGuiScreenMessageBox3.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox3);
			}
			else if (m_isNewGame)
			{
				StartNewSandbox();
			}
			else
			{
				OnOkButtonClickQuestions(0);
			}
		}

		private void OnOkButtonClickQuestions(int skipQuestions)
		{
			if (skipQuestions <= 0)
			{
				bool num = m_checkpoint.Settings.GameMode == MyGameModeEnum.Creative && GetGameMode() == MyGameModeEnum.Survival;
				bool flag = m_checkpoint.Settings.GameMode == MyGameModeEnum.Survival && GetGameMode() == MyGameModeEnum.Creative;
				if (num || (!flag && m_checkpoint.Settings.InventorySizeMultiplier > m_settings.InventorySizeMultiplier))
				{
					MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.HarvestingWarningInventoryMightBeTruncatedAreYouSure), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum x)
					{
						OnOkButtonClickAnswer(x, 1);
					});
					myGuiScreenMessageBox.SkipTransition = true;
					myGuiScreenMessageBox.InstantClose = false;
					MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
					return;
				}
			}
			if (skipQuestions <= 1 && (m_checkpoint.Settings.WorldSizeKm == 0 || m_checkpoint.Settings.WorldSizeKm > m_settings.WorldSizeKm) && m_settings.WorldSizeKm != 0)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.WorldSettings_WarningChangingWorldSize), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum x)
				{
					OnOkButtonClickAnswer(x, 2);
				});
				myGuiScreenMessageBox2.SkipTransition = true;
				myGuiScreenMessageBox2.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox2);
			}
			else
			{
				ChangeWorldSettings();
			}
		}

		private void OnOkButtonClickAnswer(MyGuiScreenMessageBox.ResultEnum answer, int skipQuestions)
		{
			if (answer == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				OnOkButtonClickQuestions(skipQuestions);
			}
		}

		private MyGameModeEnum GetGameMode()
		{
			if (!m_survivalModeButton.Checked)
			{
				return MyGameModeEnum.Creative;
			}
			return MyGameModeEnum.Survival;
		}

		protected virtual void GetSettingsFromControls()
		{
			m_settings.OnlineMode = (MyOnlineModeEnum)m_onlineMode.GetSelectedKey();
			if (m_checkpoint != null)
			{
				m_checkpoint.PreviousEnvironmentHostility = m_settings.EnvironmentHostility;
			}
			m_settings.MaxPlayers = (short)m_maxPlayersSlider.Value;
			m_settings.GameMode = GetGameMode();
			m_settings.ScenarioEditMode = false;
			m_settings.AutoSaveInMinutes = (m_autoSave.IsChecked ? 5u : 0u);
		}

		protected virtual void SetSettingsToControls()
		{
			m_onlineMode.SelectItemByKey((long)m_settings.OnlineMode);
			m_maxPlayersSlider.Value = m_settings.MaxPlayers;
			UpdateSurvivalState(m_settings.GameMode == MyGameModeEnum.Survival);
			m_autoSave.IsChecked = (m_settings.AutoSaveInMinutes != 0);
		}

		private string GetPassword()
		{
			if (Advanced != null && Advanced.IsConfirmed)
			{
				return Advanced.Password;
			}
			if (m_checkpoint != null)
			{
				return m_checkpoint.Password;
			}
			return "";
		}

		private string GetDescription()
		{
			if (m_checkpoint != null)
			{
				return m_checkpoint.Description;
			}
			return m_descriptionTextbox.Text;
		}

		private bool DescriptionChanged()
		{
			return m_descriptionChanged;
		}

		private void ChangeWorldSettings()
		{
			if (m_nameTextbox.Text != m_checkpoint.SessionName)
			{
				MyUtils.StripInvalidChars(m_nameTextbox.Text);
				string sessionPath = m_sessionPath;
				string text = m_sessionPath.Replace(m_checkpoint.SessionName, m_nameTextbox.Text);
				if (text == m_sessionPath)
				{
					text = Path.Combine(MyFileSystem.SavesPath, m_nameTextbox.Text);
				}
				if (Directory.Exists(text))
				{
					MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.WorldSettings_Error_NameExists), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
					myGuiScreenMessageBox.SkipTransition = true;
					myGuiScreenMessageBox.InstantClose = false;
					MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
					return;
				}
				try
				{
					Directory.CreateDirectory(Path.GetDirectoryName(text));
					Directory.Move(sessionPath, text);
					m_sessionPath = text;
				}
				catch
				{
					MyGuiScreenMessageBox myGuiScreenMessageBox2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.WorldSettings_Error_SavingFailed), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
					myGuiScreenMessageBox2.SkipTransition = true;
					myGuiScreenMessageBox2.InstantClose = false;
					MyGuiSandbox.AddScreen(myGuiScreenMessageBox2);
					return;
				}
			}
			m_checkpoint.SessionName = m_nameTextbox.Text;
			if (DescriptionChanged())
			{
				m_checkpoint.Description = m_descriptionTextbox.Text;
				m_descriptionChanged = false;
			}
			GetSettingsFromControls();
			m_checkpoint.Settings = m_settings;
			m_checkpoint.Mods = m_mods;
			MyLocalCache.SaveCheckpoint(m_checkpoint, m_sessionPath);
			if (MySession.Static != null && MySession.Static.Name == m_checkpoint.SessionName && m_sessionPath == MySession.Static.CurrentPath)
			{
				MySession @static = MySession.Static;
				@static.Password = GetPassword();
				@static.Description = GetDescription();
				@static.Settings = m_checkpoint.Settings;
				@static.Mods = m_checkpoint.Mods;
			}
			CloseScreen();
		}

		private void OnCancelButtonClick(object sender)
		{
			CloseScreen();
		}

		private void OnSwitchAnswer(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MySandboxGame.Config.GraphicsRenderer = MySandboxGame.DirectX11RendererKey;
				MySandboxGame.Config.Save();
				MyGuiSandbox.BackToMainMenu();
				StringBuilder messageText = MyTexts.Get(MySpaceTexts.QuickstartDX11PleaseRestartGame);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			else
			{
				StringBuilder messageText2 = MyTexts.Get(MySpaceTexts.QuickstartSelectDifferent);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText2, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
		}

		private void StartNewSandbox()
		{
			MyLog.Default.WriteLine("StartNewSandbox - Start");
			GetSettingsFromControls();
			string sesionPath = m_scenarioTypesGroup.SelectedButton.UserData as string;
			ulong checkpointSizeInBytes;
			MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sesionPath, out checkpointSizeInBytes);
			if (checkpoint != null)
			{
				GetSettingsFromControls();
				checkpoint.Settings = m_settings;
				checkpoint.SessionName = m_nameTextbox.Text;
				checkpoint.Password = GetPassword();
				checkpoint.Description = GetDescription();
				checkpoint.Mods = m_mods;
				checkpoint.Settings.VoxelGeneratorVersion = MyFakes.DEFAULT_PROCEDURAL_ASTEROID_GENERATOR;
				SetupWorldGeneratorSettings(checkpoint);
				if (checkpoint.Settings.OnlineMode != 0)
				{
					MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: true, delegate(bool granted)
					{
						if (granted)
						{
							MySessionLoader.LoadSingleplayerSession(checkpoint, sesionPath, checkpointSizeInBytes, delegate
							{
								MyAsyncSaving.Start(null, Path.Combine(MyFileSystem.SavesPath, checkpoint.SessionName.Replace(':', '-')));
							});
						}
					});
				}
				else
				{
					MySessionLoader.LoadSingleplayerSession(checkpoint, sesionPath, checkpointSizeInBytes, delegate
					{
						MyAsyncSaving.Start(null, Path.Combine(MyFileSystem.SavesPath, checkpoint.SessionName.Replace(':', '-')));
					});
				}
			}
		}

		private void SetupWorldGeneratorSettings(MyObjectBuilder_Checkpoint checkpoint)
		{
		}

		private IMyAsyncResult StartLoadingWorldInfos()
		{
			string path = "CustomWorlds";
			string customPath = Path.Combine(MyFileSystem.ContentPath, path);
			if (m_isNewGame)
			{
				return new MyNewCustomWorldInfoListResult(customPath);
			}
			return new MyLoadWorldInfoListResult(customPath);
		}

		private void OnLoadingFinished(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			MyLoadListResult myLoadListResult = (MyLoadListResult)result;
			m_scenarioTypesGroup.Clear();
			m_scenarioTypesList.Clear();
			if (myLoadListResult.AvailableSaves.Count != 0)
			{
				myLoadListResult.AvailableSaves.Sort((Tuple<string, MyWorldInfo> a, Tuple<string, MyWorldInfo> b) => a.Item2.SessionName.CompareTo(b.Item2.SessionName));
			}
			foreach (Tuple<string, MyWorldInfo> availableSafe in myLoadListResult.AvailableSaves)
			{
				if ((MySandboxGame.Config.ExperimentalMode || !availableSafe.Item2.IsExperimental) && (MyFakes.ENABLE_PLANETS || MyInput.Static.ENABLE_DEVELOPER_KEYS || !availableSafe.Item2.HasPlanets))
				{
					MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(availableSafe.Item2.SessionName, Path.Combine(availableSafe.Item1, "thumb.jpg"))
					{
						UserData = availableSafe.Item1,
						Key = m_scenarioTypesGroup.Count
					};
					m_scenarioTypesGroup.Add(myGuiControlContentButton);
					m_scenarioTypesList.Controls.Add(myGuiControlContentButton);
				}
			}
			if (m_scenarioTypesList.Controls.Count > 0)
			{
				m_scenarioTypesGroup.SelectByIndex(0);
			}
			else
			{
				SetDefaultValues();
			}
		}
	}
}
