#define VRAGE
using ParallelTasks;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenNewGame : MyGuiScreenBase
	{
		private class AsyncCampaingLoader : IMyAsyncResult
		{
			public bool IsCompleted
			{
				get
				{
					if (!Task.IsComplete)
					{
						return !Task.valid;
					}
					return true;
				}
			}

			public Task Task
			{
				get;
				private set;
			}

			public AsyncCampaingLoader()
			{
				Task = MyCampaignManager.Static.RefreshModData();
			}
		}

		private MyGuiControlScreenSwitchPanel m_screenSwitchPanel;

		private MyGuiControlList m_campaignList;

		private MyGuiControlRadioButtonGroup m_campaignTypesGroup;

		private MyObjectBuilder_Campaign m_selectedCampaign;

		private MyGuiControlContentButton m_selectedButton;

		private MyLayoutTable m_tableLayout;

		private MyGuiControlLabel m_nameLabel;

		private MyGuiControlLabel m_nameText;

		private MyGuiControlLabel m_onlineModeLabel;

		private MyGuiControlCombobox m_onlineMode;

		private MyGuiControlSlider m_maxPlayersSlider;

		private MyGuiControlLabel m_maxPlayersLabel;

		private MyGuiControlLabel m_authorLabel;

		private MyGuiControlLabel m_authorText;

		private MyGuiControlLabel m_ratingLabel;

		private MyGuiControlRating m_ratingDisplay;

		private MyGuiControlMultilineText m_descriptionMultilineText;

		private MyGuiControlPanel m_descriptionPanel;

		private MyGuiControlButton m_publishButton;

		private MyGuiControlRotatingWheel m_asyncLoadingWheel;

		private Task m_refreshTask;

		private float MARGIN_TOP = 0.22f;

		private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;

		private float MARGIN_LEFT_INFO = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private bool m_displayTabScenario;

		private bool m_displayTabWorkshop;

		private bool m_displayTabCustom;

		public MyGuiScreenNewGame(bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.878f, 0.97f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_displayTabScenario = displayTabScenario;
			m_displayTabWorkshop = displayTabWorkshop;
			m_displayTabCustom = displayTabCustom;
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "New Game";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.38f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.625f);
			Controls.Add(myGuiControlSeparatorList);
			m_screenSwitchPanel = new MyGuiControlScreenSwitchPanel(this, MyTexts.Get(MyCommonTexts.NewGameScreen_Description), m_displayTabScenario, m_displayTabWorkshop, m_displayTabCustom);
			InitCampaignList();
			InitRightSide();
			RefreshCampaignList();
			m_refreshTask = MyCampaignManager.Static.RefreshModData();
			m_asyncLoadingWheel = new MyGuiControlRotatingWheel(new Vector2(m_size.Value.X / 2f - 0.077f, (0f - m_size.Value.Y) / 2f + 0.108f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
			Controls.Add(m_asyncLoadingWheel);
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool Update(bool hasFocus)
		{
			m_publishButton.Visible = (m_selectedCampaign != null && m_selectedCampaign.IsLocalMod);
			if (m_refreshTask.valid && m_refreshTask.IsComplete)
			{
				m_refreshTask.valid = false;
				m_asyncLoadingWheel.Visible = false;
				RefreshCampaignList();
			}
			else if (base.FocusedControl == null)
			{
				MyGuiControlButton myGuiControlButton = (MyGuiControlButton)(base.FocusedControl = (MyGuiControlButton)m_screenSwitchPanel.Controls.GetControlByName("CampaignButton"));
			}
			return base.Update(hasFocus);
		}

		private void InitCampaignList()
		{
			Vector2 position = -m_size.Value / 2f + new Vector2(MARGIN_LEFT_LIST, MARGIN_TOP);
			m_campaignTypesGroup = new MyGuiControlRadioButtonGroup();
			m_campaignTypesGroup.SelectedChanged += CampaignSelectionChanged;
			m_campaignTypesGroup.MouseDoubleClick += CampaignDoubleClick;
			m_campaignList = new MyGuiControlList
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, m_size.Value.Y - MARGIN_TOP - 0.048f)
			};
			Controls.Add(m_campaignList);
		}

		private void CampaignSelectionChanged(MyGuiControlRadioButtonGroup args)
		{
			MyGuiControlContentButton myGuiControlContentButton = args.SelectedButton as MyGuiControlContentButton;
			if (myGuiControlContentButton == null)
			{
				return;
			}
			MyObjectBuilder_Campaign myObjectBuilder_Campaign = myGuiControlContentButton.UserData as MyObjectBuilder_Campaign;
			if (myObjectBuilder_Campaign == null)
			{
				return;
			}
			string name = string.IsNullOrEmpty(myObjectBuilder_Campaign.ModFolderPath) ? myObjectBuilder_Campaign.Name : Path.Combine(myObjectBuilder_Campaign.ModFolderPath, myObjectBuilder_Campaign.Name);
			MyCampaignManager.Static.ReloadMenuLocalization(name);
			string text = null;
			MyLocalizationContext myLocalizationContext = null;
			if (string.IsNullOrEmpty(myObjectBuilder_Campaign.DescriptionLocalizationFile))
			{
				text = myObjectBuilder_Campaign.Name;
				myLocalizationContext = MyLocalization.Static[text];
			}
			else
			{
				Dictionary<string, string> pathToContextTranslator = MyLocalization.Static.PathToContextTranslator;
				string key = string.IsNullOrEmpty(myObjectBuilder_Campaign.ModFolderPath) ? Path.Combine(MyFileSystem.ContentPath, myObjectBuilder_Campaign.DescriptionLocalizationFile) : Path.Combine(myObjectBuilder_Campaign.ModFolderPath, myObjectBuilder_Campaign.DescriptionLocalizationFile);
				if (pathToContextTranslator.ContainsKey(key))
				{
					text = pathToContextTranslator[key];
				}
				if (!string.IsNullOrEmpty(text))
				{
					myLocalizationContext = MyLocalization.Static[text];
				}
			}
			if (myLocalizationContext != null)
			{
				StringBuilder stringBuilder = myLocalizationContext["Name"];
				if (stringBuilder != null)
				{
					m_nameText.Text = stringBuilder.ToString();
				}
				else
				{
					m_nameText.Text = "name";
				}
				m_descriptionMultilineText.Text = myLocalizationContext["Description"];
			}
			else
			{
				m_nameText.Text = myObjectBuilder_Campaign.Name;
				m_descriptionMultilineText.Text = new StringBuilder(myObjectBuilder_Campaign.Description);
			}
			if (myObjectBuilder_Campaign != null && myObjectBuilder_Campaign.IsMultiplayer)
			{
				m_onlineMode.Enabled = true;
				m_maxPlayersSlider.MaxValue = MathHelper.Min(MyMultiplayerLobby.MAX_PLAYERS, myObjectBuilder_Campaign.MaxPlayers);
				m_maxPlayersSlider.Value = m_maxPlayersSlider.MaxValue;
			}
			else
			{
				m_onlineMode.Enabled = false;
				m_onlineMode.SelectItemByIndex(0);
			}
			m_authorText.Text = myObjectBuilder_Campaign.Author;
			m_maxPlayersSlider.Enabled = (m_onlineMode.Enabled && m_onlineMode.GetSelectedIndex() > 0);
			m_selectedCampaign = myObjectBuilder_Campaign;
			if (m_selectedButton != null)
			{
				m_selectedButton.HighlightType = MyGuiControlHighlightType.WHEN_ACTIVE;
			}
			m_selectedButton = myGuiControlContentButton;
			m_selectedButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
			m_selectedButton.HasHighlight = true;
		}

		private void InitRightSide()
		{
			int num = 5;
			Vector2 topLeft = -m_size.Value / 2f + new Vector2(MARGIN_LEFT_LIST + m_campaignList.Size.X + MARGIN_LEFT_INFO + 0.012f, MARGIN_TOP - 0.011f);
			Vector2 value = m_size.Value;
			Vector2 size = new Vector2(value.X / 2f - topLeft.X, value.Y - MARGIN_TOP - MARGIN_BOTTOM - 0.0345f) - new Vector2(MARGIN_RIGHT, 0.12f);
			float num2 = size.X * 0.6f;
			float num3 = size.X - num2;
			float num4 = 0.052f;
			float num5 = size.Y - (float)num * num4;
			m_tableLayout = new MyLayoutTable(this, topLeft, size);
			m_tableLayout.SetColumnWidthsNormalized(num2 - 0.055f, num3 + 0.055f);
			m_tableLayout.SetRowHeightsNormalized(num4, num4, num4, num4, num4, num5);
			m_nameLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.Name),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_nameText = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_authorLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.WorldSettings_Author),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			m_authorText = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_ratingLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.WorldSettings_Rating),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			m_ratingDisplay = new MyGuiControlRating(10)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_onlineModeLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.WorldSettings_OnlineMode),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			m_onlineMode = new MyGuiControlCombobox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
			m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
			m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
			m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
			m_onlineMode.SelectItemByIndex(0);
			m_onlineMode.ItemSelected += m_onlineMode_ItemSelected;
			m_onlineMode.Enabled = false;
			m_maxPlayersSlider = new MyGuiControlSlider(Vector2.Zero, 2f, width: m_onlineMode.Size.X, maxValue: MyMultiplayerLobby.MAX_PLAYERS, defaultValue: null, color: null, labelText: new StringBuilder("{0}").ToString(), labelDecimalPlaces: 0, labelScale: 0.8f, labelSpaceWidth: 0.028f, labelFont: "White", toolTip: null, visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true, showLabel: true);
			m_maxPlayersLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.MaxPlayers));
			m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
			m_descriptionMultilineText = new MyGuiControlMultilineText(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, selectable: false, showTextShadow: false, null, null)
			{
				Name = "BriefingMultilineText",
				Position = new Vector2(-0.009f, -0.115f),
				Size = new Vector2(0.419f, 0.412f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_descriptionPanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			m_tableLayout.Add(m_nameLabel, MyAlignH.Left, MyAlignV.Center, 0, 0);
			m_tableLayout.Add(m_authorLabel, MyAlignH.Left, MyAlignV.Center, 1, 0);
			m_tableLayout.Add(m_onlineModeLabel, MyAlignH.Left, MyAlignV.Center, 2, 0);
			m_tableLayout.Add(m_maxPlayersLabel, MyAlignH.Left, MyAlignV.Center, 3, 0);
			m_tableLayout.Add(m_ratingLabel, MyAlignH.Left, MyAlignV.Center, 4, 0);
			m_nameLabel.PositionX -= 0.003f;
			m_authorLabel.PositionX -= 0.003f;
			m_onlineModeLabel.PositionX -= 0.003f;
			m_maxPlayersLabel.PositionX -= 0.003f;
			m_ratingLabel.PositionX -= 0.003f;
			m_tableLayout.AddWithSize(m_nameText, MyAlignH.Left, MyAlignV.Center, 0, 1);
			m_tableLayout.AddWithSize(m_authorText, MyAlignH.Left, MyAlignV.Center, 1, 1);
			m_tableLayout.AddWithSize(m_onlineMode, MyAlignH.Left, MyAlignV.Center, 2, 1);
			m_tableLayout.AddWithSize(m_maxPlayersSlider, MyAlignH.Left, MyAlignV.Center, 3, 1);
			m_tableLayout.AddWithSize(m_ratingDisplay, MyAlignH.Left, MyAlignV.Center, 4, 1);
			m_nameText.PositionX -= 0.001f;
			m_nameText.Size += new Vector2(0.002f, 0f);
			m_onlineMode.PositionX -= 0.002f;
			m_onlineMode.PositionY -= 0.005f;
			m_maxPlayersSlider.PositionX -= 0.003f;
			m_tableLayout.AddWithSize(m_descriptionPanel, MyAlignH.Left, MyAlignV.Top, 5, 0, 1, 2);
			m_tableLayout.AddWithSize(m_descriptionMultilineText, MyAlignH.Left, MyAlignV.Top, 5, 0, 1, 2);
			m_descriptionMultilineText.PositionY += 0.012f;
			float num6 = 0.01f;
			m_descriptionPanel.Position = new Vector2(m_descriptionPanel.PositionX - num6, m_descriptionPanel.PositionY - num6 + 0.012f);
			m_descriptionPanel.Size = new Vector2(m_descriptionPanel.Size.X + num6, m_descriptionPanel.Size.Y + num6 * 2f - 0.012f);
			Vector2 vector = m_size.Value / 2f;
			vector.X -= MARGIN_RIGHT + 0.004f;
			vector.Y -= MARGIN_BOTTOM + 0.004f;
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(vector, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Start), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClicked);
			myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
			m_publishButton = new MyGuiControlButton(vector - new Vector2(bACK_BUTTON_SIZE.X + 0.0245f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnPublishButtonOnClick);
			m_publishButton.Visible = true;
			m_publishButton.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH;
			m_descriptionPanel.Size = new Vector2(m_descriptionPanel.Size.X, m_descriptionPanel.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
			m_descriptionMultilineText.Size = new Vector2(m_descriptionMultilineText.Size.X, m_descriptionMultilineText.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
			Controls.Add(m_publishButton);
			Controls.Add(myGuiControlButton);
			base.CloseButtonEnabled = true;
		}

		private void m_onlineMode_ItemSelected()
		{
			m_maxPlayersSlider.Enabled = (m_onlineMode.Enabled && m_onlineMode.GetSelectedIndex() > 0);
		}

		private void OnPublishButtonOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_selectedCampaign != null)
			{
				MyCampaignManager.Static.SwitchCampaign(m_selectedCampaign.Name, m_selectedCampaign.IsVanilla, m_selectedCampaign.PublishedFileId, m_selectedCampaign.ModFolderPath);
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MessageBoxTextDoYouWishToPublishCampaign), MySession.GameServiceName, MySession.PlatformLinkAgreement)), MyTexts.Get(MyCommonTexts.MessageBoxCaptionDoYouWishToPublishCampaign), null, null, null, null, delegate
				{
					MyCampaignManager.Static.PublishActive();
				}));
			}
		}

		private void OnOkButtonClicked(MyGuiControlButton myGuiControlButton)
		{
			StartSelectedWorld();
		}

		private void CampaignDoubleClick(MyGuiControlRadioButton obj)
		{
			StartSelectedWorld();
		}

		private void StartSelectedWorld()
		{
			if (m_selectedCampaign != null)
			{
				MyCampaignManager.Static.SwitchCampaign(m_selectedCampaign.Name, m_selectedCampaign.IsVanilla, m_selectedCampaign.PublishedFileId, m_selectedCampaign.ModFolderPath);
				MySpaceAnalytics.Instance.ReportScenarioStart(m_selectedCampaign.Name);
				MyOnlineModeEnum onlineMode = (MyOnlineModeEnum)m_onlineMode.GetSelectedKey();
				int maxPlayers = (int)m_maxPlayersSlider.Value;
				MyCampaignManager.Static.RunNewCampaign(m_selectedCampaign.Name, onlineMode, maxPlayers);
			}
		}

		private void OnCancelButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void RefreshCampaignList()
		{
			List<MyObjectBuilder_Campaign> source = MyCampaignManager.Static.Campaigns.ToList();
			List<MyObjectBuilder_Campaign> list = new List<MyObjectBuilder_Campaign>();
			List<MyObjectBuilder_Campaign> list2 = new List<MyObjectBuilder_Campaign>();
			List<MyObjectBuilder_Campaign> list3 = new List<MyObjectBuilder_Campaign>();
			List<MyObjectBuilder_Campaign> list4 = new List<MyObjectBuilder_Campaign>();
			foreach (MyObjectBuilder_Campaign item in source.OrderBy((MyObjectBuilder_Campaign x) => x.Order).ToList())
			{
				if (item.IsVanilla && !item.IsDebug)
				{
					list.Add(item);
				}
				else if (item.IsLocalMod)
				{
					list2.Add(item);
				}
				else if (item.IsVanilla && item.IsDebug)
				{
					list4.Add(item);
				}
				else
				{
					list3.Add(item);
				}
			}
			m_campaignList.Controls.Clear();
			m_campaignTypesGroup.Clear();
			if (!MyFakes.LIMITED_MAIN_MENU || MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				foreach (MyObjectBuilder_Campaign item2 in list)
				{
					AddCampaignButton(item2);
				}
				if (MySandboxGame.Config.ExperimentalMode)
				{
					if (list3.Count > 0)
					{
						AddSeparator(MyTexts.Get(MyCommonTexts.Workshop).ToString());
					}
					foreach (MyObjectBuilder_Campaign item3 in list3)
					{
						AddCampaignButton(item3, isLocalMod: false, isWorkshopMod: true);
					}
				}
				if (MySandboxGame.Config.ExperimentalMode)
				{
					if (list2.Count > 0)
					{
						AddSeparator(MyTexts.Get(MyCommonTexts.Local).ToString());
					}
					foreach (MyObjectBuilder_Campaign item4 in list2)
					{
						AddCampaignButton(item4, isLocalMod: true);
					}
				}
				if (MyInput.Static.ENABLE_DEVELOPER_KEYS)
				{
					AddSeparator(MyTexts.Get(MyCommonTexts.Debug).ToString());
					foreach (MyObjectBuilder_Campaign item5 in list4)
					{
						AddCampaignButton(item5);
					}
				}
			}
			if (m_campaignList.Controls.Count > 0)
			{
				m_campaignTypesGroup.SelectByIndex(0);
			}
		}

		private void AddCampaignButton(MyObjectBuilder_Campaign campaign, bool isLocalMod = false, bool isWorkshopMod = false)
		{
			string title = campaign.Name;
			MyLocalizationContext myLocalizationContext = MyLocalization.Static[campaign.Name];
			if (myLocalizationContext != null)
			{
				StringBuilder stringBuilder = myLocalizationContext["Name"];
				if (stringBuilder != null)
				{
					title = stringBuilder.ToString();
				}
			}
			MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(title, GetImagePath(campaign))
			{
				UserData = campaign,
				IsLocalMod = isLocalMod,
				IsWorkshopMod = isWorkshopMod,
				Key = m_campaignTypesGroup.Count
			};
			m_campaignTypesGroup.Add(myGuiControlContentButton);
			m_campaignList.Controls.Add(myGuiControlContentButton);
		}

		private string GetImagePath(MyObjectBuilder_Campaign campaign)
		{
			string text = campaign.ImagePath;
			if (string.IsNullOrEmpty(campaign.ImagePath))
			{
				return string.Empty;
			}
			if (!campaign.IsVanilla)
			{
				text = ((campaign.ModFolderPath != null) ? Path.Combine(campaign.ModFolderPath, campaign.ImagePath) : string.Empty);
				if (!MyFileSystem.FileExists(text))
				{
					text = Path.Combine(MyFileSystem.ContentPath, campaign.ImagePath);
				}
			}
			return text;
		}

		private void AddSeparator(string sectionName)
		{
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = Vector2.Zero
			};
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = sectionName,
				Font = "Blue",
				PositionX = 0.005f
			};
			float num = 0.003f;
			Color tHEMED_GUI_LINE_COLOR = MyGuiConstants.THEMED_GUI_LINE_COLOR;
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, null, null, null, new string[1]
			{
				"Textures\\GUI\\FogSmall3.dds"
			})
			{
				Size = new Vector2(myGuiControlLabel.Size.X + num * 10f, 0.007f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				ColorMask = tHEMED_GUI_LINE_COLOR.ToVector4(),
				Position = new Vector2(0f - num, myGuiControlLabel.Size.Y)
			};
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent
			{
				Size = new Vector2(m_campaignList.Size.X, myGuiControlLabel.Size.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = Vector2.Zero
			};
			myGuiControlCompositePanel.Size = myGuiControlParent.Size + new Vector2(-0.035f, 0.01f);
			myGuiControlCompositePanel.Position -= myGuiControlParent.Size / 2f - new Vector2(-0.01f, 0f);
			myGuiControlLabel.Position -= myGuiControlParent.Size / 2f;
			myGuiControlImage.Position -= myGuiControlParent.Size / 2f;
			myGuiControlParent.Controls.Add(myGuiControlCompositePanel);
			myGuiControlParent.Controls.Add(myGuiControlImage);
			myGuiControlParent.Controls.Add(myGuiControlLabel);
			m_campaignList.Controls.Add(myGuiControlParent);
		}

		private AsyncCampaingLoader RunRefreshAsync()
		{
			return new AsyncCampaingLoader();
		}
	}
}
