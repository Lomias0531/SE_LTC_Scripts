using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
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
using VRage.Game.Localization;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenNewWorkshopGame : MyGuiScreenBase
	{
		public class MyWorkshopItemComparer : IComparer<MyWorkshopItem>
		{
			private Func<MyWorkshopItem, MyWorkshopItem, int> comparator;

			public MyWorkshopItemComparer(Func<MyWorkshopItem, MyWorkshopItem, int> comp)
			{
				comparator = comp;
			}

			public int Compare(MyWorkshopItem x, MyWorkshopItem y)
			{
				if (comparator != null)
				{
					return comparator(x, y);
				}
				return 0;
			}
		}

		private class LoadListResult : IMyAsyncResult
		{
			public bool Success;

			public List<MyWorkshopItem> SubscribedWorlds;

			public bool IsCompleted => Task.IsComplete;

			public Task Task
			{
				get;
				private set;
			}

			public LoadListResult()
			{
				Task = Parallel.Start(delegate
				{
					LoadListAsync(out SubscribedWorlds);
				});
			}

			private void LoadListAsync(out List<MyWorkshopItem> list)
			{
				List<MyWorkshopItem> list2 = new List<MyWorkshopItem>();
				if (MyWorkshop.GetSubscribedWorldsBlocking(list2))
				{
					list = list2;
					List<MyWorkshopItem> list3 = new List<MyWorkshopItem>();
					if (MyWorkshop.GetSubscribedScenariosBlocking(list3) && list3.Count > 0)
					{
						list.InsertRange(list.Count, list3);
					}
				}
				else
				{
					list = null;
				}
				SubscribedWorlds = list;
				Success = MyWorkshop.TryUpdateWorldsBlocking(SubscribedWorlds, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo());
			}
		}

		private MyGuiControlScreenSwitchPanel m_screenSwitchPanel;

		public List<MyWorkshopItem> SubscribedWorlds;

		private MyGuiBlueprintScreen_Reworked.SortOption m_sort;

		private bool m_showThumbnails = true;

		private MyGuiControlButton m_buttonRefresh;

		private MyGuiControlButton m_buttonSorting;

		private MyGuiControlButton m_buttonOpenWorkshop;

		private MyGuiControlButton m_buttonToggleThumbnails;

		private MyGuiControlImage m_iconRefresh;

		private MyGuiControlImage m_iconSorting;

		private MyGuiControlImage m_iconOpenWorkshop;

		private MyGuiControlImage m_iconToggleThumbnails;

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlList m_worldList;

		private MyGuiControlRadioButtonGroup m_worldTypesGroup;

		private MyObjectBuilder_Checkpoint m_selectedWorld;

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

		private MyGuiControlMultilineText m_noSubscribedItemsText;

		private MyGuiControlPanel m_noSubscribedItemsPanel;

		private MyGuiControlMultilineText m_descriptionMultilineText;

		private MyGuiControlPanel m_descriptionPanel;

		private MyGuiControlRotatingWheel m_asyncLoadingWheel;

		private float MARGIN_TOP = 0.22f;

		private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;

		private float MARGIN_LEFT_INFO = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private bool m_displayTabScenario;

		private bool m_displayTabWorkshop;

		private bool m_displayTabCustom;

		public MyGuiScreenNewWorkshopGame(bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
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
			return "New Workshop Game";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.38f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.625f);
			Controls.Add(myGuiControlSeparatorList);
			m_screenSwitchPanel = new MyGuiControlScreenSwitchPanel(this, MyTexts.Get(MyCommonTexts.WorkshopScreen_Description), m_displayTabScenario, m_displayTabWorkshop, m_displayTabCustom);
			InitWorldList();
			InitRightSide();
			FillList();
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		private void InitWorldList()
		{
			float num = 0.31f;
			float x = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			Vector2 position = -m_size.Value / 2f + new Vector2(x, num);
			Vector2 vector = new Vector2(-0.366f, -0.261f);
			m_buttonRefresh = CreateToolbarButton(vector, MyCommonTexts.WorldSettings_Tooltip_Refresh, OnRefreshClicked);
			m_buttonSorting = CreateToolbarButton(vector + new Vector2(m_buttonRefresh.Size.X, 0f), MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButSort, OnSortingClicked);
			m_buttonToggleThumbnails = CreateToolbarButton(vector + new Vector2(m_buttonRefresh.Size.X * 2f, 0f), MyCommonTexts.WorldSettings_Tooltip_ToggleThumbnails, OnToggleThumbnailsClicked);
			m_buttonOpenWorkshop = CreateToolbarButton(vector + new Vector2(m_buttonRefresh.Size.X * 3f, 0f), MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenWorkshop, OnOpenWorkshopClicked);
			m_iconRefresh = CreateToolbarButtonIcon(m_buttonRefresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
			m_iconSorting = CreateToolbarButtonIcon(m_buttonSorting, "");
			SetIconForSorting();
			m_iconToggleThumbnails = CreateToolbarButtonIcon(m_buttonToggleThumbnails, "");
			m_iconOpenWorkshop = CreateToolbarButtonIcon(m_buttonOpenWorkshop, "Textures\\GUI\\Icons\\Blueprints\\Steam.png");
			SetIconForHideThumbnails();
			m_worldTypesGroup = new MyGuiControlRadioButtonGroup();
			m_worldTypesGroup.SelectedChanged += WorldSelectionChanged;
			m_worldTypesGroup.MouseDoubleClick += WorldDoubleClick;
			m_worldList = new MyGuiControlList
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, m_size.Value.Y - num - 0.048f)
			};
			Controls.Add(m_worldList);
			m_searchBox = new MyGuiControlSearchBox(new Vector2(-0.382f, -0.22f), new Vector2(m_worldList.Size.X, 0.032f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_searchBox.OnTextChanged += OnSearchTextChange;
			Controls.Add(m_searchBox);
		}

		private MyGuiControlButton CreateToolbarButton(Vector2 position, MyStringId tooltip, Action<MyGuiControlButton> onClick)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.ShowTooltipWhenDisabled = true;
			myGuiControlButton.SetToolTip(tooltip);
			myGuiControlButton.Size = new Vector2(0.029f, 0.03358333f);
			Controls.Add(myGuiControlButton);
			return myGuiControlButton;
		}

		private MyGuiControlImage CreateToolbarButtonIcon(MyGuiControlButton button, string texture)
		{
			button.Size = new Vector2(button.Size.X, button.Size.X * 4f / 3f);
			float num = 0.95f * Math.Min(button.Size.X, button.Size.Y);
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(size: new Vector2(num * 0.75f, num), position: button.Position + new Vector2(-0.0016f, 0.018f), backgroundColor: null, backgroundTexture: null, textures: new string[1]
			{
				texture
			});
			Controls.Add(myGuiControlImage);
			return myGuiControlImage;
		}

		private void SetIconForSorting()
		{
			switch (m_sort)
			{
			case MyGuiBlueprintScreen_Reworked.SortOption.None:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\Alphabetical.png");
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByCreationDate.png");
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByUpdateDate.png");
				break;
			default:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
				break;
			}
		}

		private void SetIconForHideThumbnails()
		{
			m_iconToggleThumbnails.SetTexture(m_showThumbnails ? "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.png" : "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsOFF.png");
		}

		private void WorldSelectionChanged(MyGuiControlRadioButtonGroup args)
		{
			MyGuiControlContentButton myGuiControlContentButton = args.SelectedButton as MyGuiControlContentButton;
			if (myGuiControlContentButton == null || myGuiControlContentButton.UserData == null)
			{
				return;
			}
			MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> myTuple = (MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>)myGuiControlContentButton.UserData;
			string path = Path.Combine(myTuple.Item2.Folder + "\\Sandbox.sbc");
			string text = "";
			bool flag = false;
			if (MyFileSystem.FileExists(path))
			{
				if (MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_Checkpoint checkpoint))
				{
					MyObjectBuilder_Identity myObjectBuilder_Identity = checkpoint.Identities.Find((MyObjectBuilder_Identity x) => x.CharacterEntityId == checkpoint.ControlledObject);
					if (myObjectBuilder_Identity != null)
					{
						text = myObjectBuilder_Identity.DisplayName;
					}
					if (string.IsNullOrEmpty(myTuple.Item1.Description))
					{
						myTuple.Item1.Description = checkpoint.Briefing;
					}
					flag = (checkpoint.OnlineMode != MyOnlineModeEnum.OFFLINE);
				}
			}
			string text2 = "";
			MyLocalizationContext myLocalizationContext = MyLocalization.Static[myTuple.Item1.SessionName];
			if (myLocalizationContext != null)
			{
				StringBuilder stringBuilder = myLocalizationContext["Name"];
				if (stringBuilder != null)
				{
					text2 = stringBuilder.ToString();
				}
				m_descriptionMultilineText.Text = myLocalizationContext["Description"];
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = myTuple.Item1.SessionName;
				m_descriptionMultilineText.Text = new StringBuilder(myTuple.Item1.Description);
			}
			string text3 = text2;
			if (text3.Length > 40)
			{
				text3 = text3.Remove(32) + "...";
			}
			m_nameText.SetToolTip(text2);
			m_nameLabel.SetToolTip(text2);
			m_nameText.Text = text3;
			if (flag)
			{
				m_onlineMode.Enabled = true;
			}
			else
			{
				m_onlineMode.Enabled = false;
				m_onlineMode.SelectItemByIndex(0);
			}
			m_authorText.Text = text;
			m_ratingDisplay.Value = (int)Math.Round(myTuple.Item2.Score * 10f);
			m_maxPlayersSlider.Enabled = (m_onlineMode.Enabled && m_onlineMode.GetSelectedIndex() > 0);
			m_selectedWorld = myTuple.Item1;
			m_descriptionMultilineText.SetScrollbarPageV(0f);
			m_descriptionMultilineText.SetScrollbarPageH(0f);
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
			Vector2 topLeft = -m_size.Value / 2f + new Vector2(MARGIN_LEFT_LIST + m_worldList.Size.X + MARGIN_LEFT_INFO + 0.012f, MARGIN_TOP - 0.011f);
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
			m_ratingDisplay = new MyGuiControlRating(8)
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
			Vector2 value2 = m_size.Value / 2f;
			value2.X -= MARGIN_RIGHT + 0.004f;
			value2.Y -= MARGIN_BOTTOM + 0.004f;
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(value2, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Start), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClicked);
			myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
			m_descriptionPanel.Size = new Vector2(m_descriptionPanel.Size.X, m_descriptionPanel.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
			m_descriptionMultilineText.Size = new Vector2(m_descriptionMultilineText.Size.X, m_descriptionMultilineText.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
			Controls.Add(myGuiControlButton);
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(myGuiControlButton.Position - new Vector2(myGuiControlButton.Size.X + 0.01f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOpenInWorkshopClicked);
			myGuiControlButton2.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorkshopOpenInWorkshop), MyGameService.Service.ServiceName));
			Controls.Add(myGuiControlButton2);
			base.CloseButtonEnabled = true;
			m_noSubscribedItemsPanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			m_tableLayout.AddWithSize(m_noSubscribedItemsPanel, MyAlignH.Left, MyAlignV.Top, 0, 0, 6, 2);
			m_noSubscribedItemsPanel.Position = new Vector2(m_descriptionPanel.Position.X, m_worldList.Position.Y);
			m_noSubscribedItemsPanel.Size = new Vector2(m_descriptionPanel.Size.X, m_worldList.Size.Y - 1.63f * MyGuiConstants.BACK_BUTTON_SIZE.Y);
			m_noSubscribedItemsText = new MyGuiControlMultilineText(null, null, null, "Blue", 684f / 925f)
			{
				Size = new Vector2(m_descriptionMultilineText.Size.X, m_descriptionMultilineText.Size.Y * 2f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_tableLayout.AddWithSize(m_noSubscribedItemsText, MyAlignH.Left, MyAlignV.Top, 0, 0, 6, 2);
			m_noSubscribedItemsText.Position = m_noSubscribedItemsPanel.Position + new Vector2(num6);
			m_noSubscribedItemsText.Size = m_noSubscribedItemsPanel.Size - new Vector2(2f * num6);
			m_noSubscribedItemsText.Clear();
			m_noSubscribedItemsText.AppendText(string.Format(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_NoWorkshopWorld), MyGameService.Service.ServiceName), "Blue", m_noSubscribedItemsText.TextScale, Vector4.One);
			m_noSubscribedItemsText.AppendLine();
			m_noSubscribedItemsText.AppendLink(MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_WORLDS), "Space Engineers " + MyGameService.WorkshopService.ServiceName + " Workshop");
			m_noSubscribedItemsText.AppendLine();
			m_noSubscribedItemsText.OnLinkClicked += OnLinkClicked;
			m_noSubscribedItemsText.ScrollbarOffsetV = 1f;
			m_noSubscribedItemsPanel.Visible = false;
			m_noSubscribedItemsText.Visible = false;
		}

		private void OnLinkClicked(MyGuiControlBase sender, string url)
		{
			MyGuiSandbox.OpenUrlWithFallback(url, "Space Engineers Steam Workshop");
		}

		private void OnOpenInWorkshopClicked(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrlWithFallback((m_selectedWorld != null) ? MyGameService.WorkshopService.GetItemUrl(m_selectedWorld.WorkshopId.Value) : MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_SCENARIOS), MyGameService.WorkshopService.ServiceName + " Workshop");
		}

		private void m_onlineMode_ItemSelected()
		{
			m_maxPlayersSlider.Enabled = (m_onlineMode.Enabled && m_onlineMode.GetSelectedIndex() > 0);
		}

		private void OnRefreshClicked(MyGuiControlButton button)
		{
			m_selectedWorld = null;
			m_worldList.Clear();
			m_worldTypesGroup.Clear();
			FillList();
		}

		private void OnSortingClicked(MyGuiControlButton button)
		{
			switch (m_sort)
			{
			case MyGuiBlueprintScreen_Reworked.SortOption.None:
				m_sort = MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical;
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
				m_sort = MyGuiBlueprintScreen_Reworked.SortOption.CreationDate;
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
				m_sort = MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate;
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
				m_sort = MyGuiBlueprintScreen_Reworked.SortOption.None;
				break;
			}
			SetIconForSorting();
			m_selectedWorld = null;
			m_worldList.Clear();
			m_worldTypesGroup.Clear();
			OnSuccess();
		}

		private void OnOpenWorkshopClicked(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_SCENARIOS), MyGameService.WorkshopService.ServiceName + " Workshop");
		}

		private void OnToggleThumbnailsClicked(MyGuiControlButton button)
		{
			m_showThumbnails = !m_showThumbnails;
			SetIconForHideThumbnails();
			foreach (MyGuiControlBase control in m_worldList.Controls)
			{
				(control as MyGuiControlContentButton)?.SetPreviewVisibility(m_showThumbnails);
			}
			m_worldList.Recalculate();
		}

		private void OnSearchTextChange(string message)
		{
			ApplyFiltering();
			if (m_worldTypesGroup.Count > 0)
			{
				m_worldTypesGroup.SelectByIndex(0);
			}
		}

		private void OnOkButtonClicked(MyGuiControlButton myGuiControlButton)
		{
			StartSelectedWorld();
		}

		private void WorldDoubleClick(MyGuiControlRadioButton obj)
		{
			StartSelectedWorld();
		}

		private void StartSelectedWorld()
		{
			if (m_selectedWorld != null && m_worldTypesGroup.SelectedButton != null && m_worldTypesGroup.SelectedButton.UserData != null)
			{
				MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> myTuple = (MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>)m_worldTypesGroup.SelectedButton.UserData;
				MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginActionLoadSaves, endActionLoadSaves, myTuple));
			}
		}

		private IMyAsyncResult beginActionLoadSaves()
		{
			return new MyLoadWorldInfoListResult();
		}

		private void endActionLoadSaves(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			screen.CloseScreen();
			MyWorkshopItem item = ((MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>)screen.UserData).Item2;
			if (Directory.Exists(MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(item.Title), contentFolder: false, createIfNotExists: false)))
			{
				OverwriteWorldDialog(item);
			}
			else
			{
				MyWorkshop.CreateWorldInstanceAsync(item, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), overwrite: false, delegate(bool success, string sessionPath)
				{
					if (success)
					{
						OnSuccessStart(sessionPath);
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
					}
				});
			}
		}

		private void OverwriteWorldDialog(MyWorkshopItem world)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextWorldExistsDownloadOverwrite), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					MyWorkshop.CreateWorldInstanceAsync(world, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), overwrite: true, delegate(bool success, string sessionPath)
					{
						if (success)
						{
							OnSuccessStart(sessionPath);
						}
						else
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
						}
					});
				}
			}));
		}

		private void OnSuccessStart(string sessionPath)
		{
			MySessionLoader.LoadSingleplayerSession(sessionPath);
		}

		private void OnCancelButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void AddWorldButton(MyObjectBuilder_Checkpoint world, MyWorkshopItem workshopItem, bool isLocalMod = false, bool isWorkshopMod = false)
		{
			string text = world.SessionName;
			MyLocalizationContext myLocalizationContext = MyLocalization.Static[world.SessionName];
			if (myLocalizationContext != null)
			{
				StringBuilder stringBuilder = myLocalizationContext["Name"];
				if (stringBuilder != null)
				{
					text = stringBuilder.ToString();
				}
			}
			MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(text, GetImagePath(world))
			{
				UserData = new MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>(world, workshopItem),
				IsLocalMod = isLocalMod,
				IsWorkshopMod = isWorkshopMod,
				Key = m_worldTypesGroup.Count
			};
			myGuiControlContentButton.SetPreviewVisibility(m_showThumbnails);
			myGuiControlContentButton.SetTooltip(text);
			m_worldTypesGroup.Add(myGuiControlContentButton);
			m_worldList.Controls.Add(myGuiControlContentButton);
		}

		private string GetImagePath(MyObjectBuilder_Checkpoint world)
		{
			string briefingVideo = world.BriefingVideo;
			if (string.IsNullOrEmpty(world.BriefingVideo))
			{
				return string.Empty;
			}
			return briefingVideo;
		}

		private void SortItems(List<MyWorkshopItem> list)
		{
			MyWorkshopItemComparer myWorkshopItemComparer = null;
			switch (m_sort)
			{
			case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
				myWorkshopItemComparer = new MyWorkshopItemComparer((MyWorkshopItem x, MyWorkshopItem y) => x.Title.CompareTo(y.Title));
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
				myWorkshopItemComparer = new MyWorkshopItemComparer((MyWorkshopItem x, MyWorkshopItem y) => x.TimeCreated.CompareTo(y.TimeCreated));
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
				myWorkshopItemComparer = new MyWorkshopItemComparer((MyWorkshopItem x, MyWorkshopItem y) => x.TimeUpdated.CompareTo(y.TimeUpdated));
				break;
			}
			if (myWorkshopItemComparer != null)
			{
				list.Sort(myWorkshopItemComparer);
			}
		}

		private void ApplyFiltering()
		{
			bool flag = false;
			string[] array = new string[0];
			if (m_searchBox != null)
			{
				flag = (m_searchBox.SearchText != "");
				array = m_searchBox.SearchText.Split(new char[1]
				{
					' '
				});
			}
			foreach (MyGuiControlBase control in m_worldList.Controls)
			{
				MyGuiControlContentButton myGuiControlContentButton = control as MyGuiControlContentButton;
				if (myGuiControlContentButton != null)
				{
					bool visible = true;
					if (flag)
					{
						string text = myGuiControlContentButton.Title.ToLower();
						string[] array2 = array;
						foreach (string text2 in array2)
						{
							if (!text.Contains(text2.ToLower()))
							{
								visible = false;
								break;
							}
						}
					}
					control.Visible = visible;
				}
			}
			m_worldList.SetScrollBarPage();
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
				Size = new Vector2(m_worldList.Size.X, myGuiControlLabel.Size.Y),
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
			m_worldList.Controls.Add(myGuiControlParent);
		}

		private IMyAsyncResult beginAction()
		{
			return new LoadListResult();
		}

		private void endAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			LoadListResult loadListResult = (LoadListResult)result;
			SubscribedWorlds = loadListResult.SubscribedWorlds;
			if (loadListResult.Success)
			{
				OnSuccess();
			}
			screen.CloseScreen();
			if (SubscribedWorlds != null)
			{
				m_noSubscribedItemsPanel.Visible = (SubscribedWorlds.Count == 0);
				m_noSubscribedItemsText.Visible = (SubscribedWorlds.Count == 0);
			}
			else
			{
				m_noSubscribedItemsPanel.Visible = true;
				m_noSubscribedItemsText.Visible = true;
			}
		}

		private void OnSuccess()
		{
			if (SubscribedWorlds != null)
			{
				SortItems(SubscribedWorlds);
				List<string> list = new List<string>();
				foreach (MyWorkshopItem subscribedWorld in SubscribedWorlds)
				{
					MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = new MyObjectBuilder_Checkpoint();
					myObjectBuilder_Checkpoint.SessionName = subscribedWorld.Title;
					string text = Path.Combine(subscribedWorld.Folder + "\\thumb.jpg");
					if (MyFileSystem.FileExists(text))
					{
						myObjectBuilder_Checkpoint.BriefingVideo = text;
						list.Add(text);
					}
					myObjectBuilder_Checkpoint.Description = subscribedWorld.Description;
					myObjectBuilder_Checkpoint.WorkshopId = subscribedWorld.Id;
					AddWorldButton(myObjectBuilder_Checkpoint, subscribedWorld);
				}
				MyRenderProxy.PreloadTextures(list, TextureType.GUIWithoutPremultiplyAlpha);
			}
			if (!m_worldTypesGroup.SelectedIndex.HasValue && m_worldList.Controls.Count > 0)
			{
				m_worldTypesGroup.SelectByIndex(0);
			}
		}

		private void FillList()
		{
			m_worldList.Clear();
			m_worldTypesGroup.Clear();
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginAction, endAction));
		}
	}
}
