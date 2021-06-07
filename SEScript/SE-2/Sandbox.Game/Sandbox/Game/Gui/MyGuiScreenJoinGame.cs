using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenJoinGame : MyGuiScreenBase
	{
		private class CellRemainingTime : MyGuiControlTable.Cell
		{
			private readonly DateTime m_timeEstimatedEnd;

			public CellRemainingTime(float remainingTime)
				: base("")
			{
				m_timeEstimatedEnd = DateTime.UtcNow + TimeSpan.FromSeconds(remainingTime);
				FillText();
			}

			public override void Update()
			{
				base.Update();
				FillText();
			}

			private void FillText()
			{
				TimeSpan t = m_timeEstimatedEnd - DateTime.UtcNow;
				if (t < TimeSpan.Zero)
				{
					t = TimeSpan.Zero;
				}
				Text.Clear().Append(t.ToString("mm\\:ss"));
			}
		}

		private enum RefreshStateEnum
		{
			Pause,
			Resume,
			Refresh
		}

		private enum ContextMenuFavoriteAction
		{
			Add,
			Remove
		}

		private struct ContextMenuFavoriteActionItem
		{
			public MyGameServerItem Server;

			public ContextMenuFavoriteAction _Action;
		}

		private MyGuiControlTabControl m_joinGameTabs;

		private MyGuiControlContextMenu m_contextMenu;

		private readonly StringBuilder m_textCache = new StringBuilder();

		private readonly StringBuilder m_gameTypeText = new StringBuilder();

		private readonly StringBuilder m_gameTypeToolTip = new StringBuilder();

		private MyGuiControlTable m_gamesTable;

		private MyGuiControlButton m_joinButton;

		private MyGuiControlButton m_refreshButton;

		private MyGuiControlButton m_detailsButton;

		private MyGuiControlButton m_directConnectButton;

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlButton m_advancedSearchButton;

		private MyGuiControlRotatingWheel m_loadingWheel;

		private readonly string m_dataHash;

		private bool m_searchChanged;

		private DateTime m_searchLastChanged = DateTime.Now;

		private Action m_searchChangedFunc;

		private MyRankedServers m_rankedServers;

		public MyGuiControlTabPage m_selectedPage;

		private int m_remainingTimeUpdateFrame;

		public MyServerFilterOptions FilterOptions;

		public bool EnableAdvancedSearch;

		public bool refresh_favorites;

		private MyGuiControlTabPage m_serversPage;

		private readonly HashSet<MyCachedServerItem> m_dedicatedServers = new HashSet<MyCachedServerItem>();

		private RefreshStateEnum m_nextState;

		private bool m_refreshPaused;

		private bool m_dedicatedResponding;

		private bool m_lastVersionCheck;

		private readonly List<IMyLobby> m_lobbies = new List<IMyLobby>();

		private MyGuiControlTabPage m_lobbyPage;

		private MyGuiControlTabPage m_favoritesPage;

		private HashSet<MyCachedServerItem> m_favoriteServers = new HashSet<MyCachedServerItem>();

		private bool m_favoritesResponding;

		private MyGuiControlTabPage m_historyPage;

		private HashSet<MyCachedServerItem> m_historyServers = new HashSet<MyCachedServerItem>();

		private bool m_historyResponding;

		private MyGuiControlTabPage m_LANPage;

		private HashSet<MyCachedServerItem> m_lanServers = new HashSet<MyCachedServerItem>();

		private bool m_lanResponding;

		private MyGuiControlTabPage m_friendsPage;

		private HashSet<ulong> m_friendIds;

		private HashSet<string> m_friendNames;

		private event Action<MyGuiControlButton> RefreshRequest;

		public MyGuiScreenJoinGame()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(1f, 0.9f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			m_dataHash = MyDataIntegrityChecker.GetHashBase64();
			MyObjectBuilder_ServerFilterOptions serverSearchSettings = MySandboxGame.Config.ServerSearchSettings;
			if (serverSearchSettings != null)
			{
				FilterOptions = new MySpaceServerFilterOptions(serverSearchSettings);
			}
			else
			{
				FilterOptions = new MySpaceServerFilterOptions();
			}
			RecreateControls(constructor: true);
			if (MyFakes.LIMITED_MAIN_MENU && !MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				m_joinGameTabs.SetPage(1);
			}
			m_selectedPage = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageFavoritesPanel");
			joinGameTabs_OnPageChanged();
			MyRankedServers.LoadAsync(MyPerGameSettings.RankedServersUrl, OnRankedServersLoaded);
		}

		private void OnRankedServersLoaded(MyRankedServers rankedServers)
		{
			m_rankedServers = rankedServers;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenJoinGame";
		}

		protected override void OnClosed()
		{
			CloseRequest();
			MySandboxGame.Config.ServerSearchSettings = FilterOptions.GetObjectBuilder();
			MySandboxGame.Config.Save();
			base.OnClosed();
		}

		private int PlayerCountComparison(MyGuiControlTable.Cell b, MyGuiControlTable.Cell a)
		{
			List<StringBuilder> list = a.Text.Split('/');
			List<StringBuilder> list2 = b.Text.Split('/');
			int result = 0;
			int result2 = 0;
			int result3 = 0;
			int result4 = 0;
			bool flag = true;
			if (list.Count >= 2 && list2.Count >= 2)
			{
				flag &= int.TryParse(list[0].ToString(), out result);
				flag &= int.TryParse(list2[0].ToString(), out result2);
				flag &= int.TryParse(list[1].ToString(), out result3);
				flag &= int.TryParse(list2[1].ToString(), out result4);
			}
			else
			{
				flag = false;
			}
			if (result == result2 || !flag)
			{
				if (result3 == result4 || !flag)
				{
					IMyMultiplayerGame myMultiplayerGame = a.Row.UserData as IMyMultiplayerGame;
					if (myMultiplayerGame != null)
					{
						ulong gameID = myMultiplayerGame.GameID;
						IMyMultiplayerGame myMultiplayerGame2 = b.Row.UserData as IMyMultiplayerGame;
						if (myMultiplayerGame2 != null)
						{
							ulong gameID2 = myMultiplayerGame2.GameID;
							return gameID.CompareTo(gameID2);
						}
						return 0;
					}
					return 0;
				}
				return result3.CompareTo(result4);
			}
			return result.CompareTo(result2);
		}

		private int TextComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			int num = a.Text.CompareToIgnoreCase(b.Text);
			if (num == 0)
			{
				IMyMultiplayerGame myMultiplayerGame = a.Row.UserData as IMyMultiplayerGame;
				if (myMultiplayerGame != null)
				{
					ulong gameID = myMultiplayerGame.GameID;
					IMyMultiplayerGame myMultiplayerGame2 = b.Row.UserData as IMyMultiplayerGame;
					if (myMultiplayerGame2 != null)
					{
						ulong gameID2 = myMultiplayerGame2.GameID;
						return gameID.CompareTo(gameID2);
					}
					return 0;
				}
				return 0;
			}
			return num;
		}

		private int PingComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			if (!int.TryParse(a.Text.ToString(), out int result))
			{
				result = -1;
			}
			if (!int.TryParse(b.Text.ToString(), out int result2))
			{
				result2 = -1;
			}
			if (result == result2)
			{
				IMyMultiplayerGame myMultiplayerGame = a.Row.UserData as IMyMultiplayerGame;
				if (myMultiplayerGame != null)
				{
					ulong gameID = myMultiplayerGame.GameID;
					IMyMultiplayerGame myMultiplayerGame2 = b.Row.UserData as IMyMultiplayerGame;
					if (myMultiplayerGame2 != null)
					{
						ulong gameID2 = myMultiplayerGame2.GameID;
						return gameID.CompareTo(gameID2);
					}
					return 0;
				}
				return 0;
			}
			return result.CompareTo(result2);
		}

		private int ModsComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			int result = 0;
			int.TryParse(a.Text.ToString(), out result);
			int result2 = 0;
			int.TryParse(b.Text.ToString(), out result2);
			if (result == result2)
			{
				IMyMultiplayerGame myMultiplayerGame = a.Row.UserData as IMyMultiplayerGame;
				if (myMultiplayerGame != null)
				{
					ulong gameID = myMultiplayerGame.GameID;
					IMyMultiplayerGame myMultiplayerGame2 = b.Row.UserData as IMyMultiplayerGame;
					if (myMultiplayerGame2 != null)
					{
						ulong gameID2 = myMultiplayerGame2.GameID;
						return gameID.CompareTo(gameID2);
					}
					return 0;
				}
				return 0;
			}
			return result.CompareTo(result2);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			string path = MyGuiScreenBase.MakeScreenFilepath("JoinScreen");
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, path), out MyObjectBuilder_GuiScreen objectBuilder);
			Init(objectBuilder);
			m_joinGameTabs = (Controls.GetControlByName("JoinGameTabs") as MyGuiControlTabControl);
			m_joinGameTabs.PositionY -= 0.018f;
			m_searchBox = new MyGuiControlSearchBox(new Vector2(-0.453f, -0.3f), new Vector2(0.754f, 0.02f));
			m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_searchBox.OnTextChanged += OnBlockSearchTextChanged;
			Controls.Add(m_searchBox);
			m_advancedSearchButton = new MyGuiControlButton
			{
				Position = m_searchBox.Position + new Vector2(m_searchBox.Size.X + 0.155f, 0.006f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				VisualStyle = MyGuiControlButtonStyleEnum.ComboBoxButton,
				Text = MyTexts.GetString(MyCommonTexts.Advanced)
			};
			m_advancedSearchButton.ButtonClicked += AdvancedSearchButtonClicked;
			m_advancedSearchButton.SetToolTip(MySpaceTexts.ToolTipJoinGame_Advanced);
			Controls.Add(m_advancedSearchButton);
			m_joinGameTabs.TabButtonScale = 0.86f;
			m_joinGameTabs.OnPageChanged += joinGameTabs_OnPageChanged;
			joinGameTabs_OnPageChanged();
			AddCaption(MyCommonTexts.ScreenMenuButtonJoinGame, null, new Vector2(0f, 0.003f));
			base.CloseButtonEnabled = true;
			if ((float)MySandboxGame.ScreenSize.X / (float)MySandboxGame.ScreenSize.Y == 1.25f)
			{
				SetCloseButtonOffset_5_to_4();
			}
			else
			{
				SetDefaultCloseButtonOffset();
			}
			string centerTexture = "Textures\\GUI\\gtxlogobigger.png";
			string centerTexture2 = "Textures\\GUI\\gtxlogobiggerHighlight.png";
			string userData = "https://www.gtxgaming.co.uk/server-hosting/space-engineers-server-hosting/";
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture2)
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture)
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture)
				}
			};
			Vector2 value = new Vector2(0.2375f, 0.13f) * 0.7f;
			MyGuiControlImageButton myGuiControlImageButton = new MyGuiControlImageButton("Button", new Vector2(base.Size.Value.X / 2f - 0.04f, (0f - base.Size.Value.Y) / 2f - 0.01f), value, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onBannerClick);
			myGuiControlImageButton.BackgroundTexture = new MyGuiCompositeTexture(centerTexture);
			myGuiControlImageButton.ApplyStyle(style);
			myGuiControlImageButton.CanHaveFocus = false;
			myGuiControlImageButton.UserData = userData;
			myGuiControlImageButton.SetToolTip(MyTexts.GetString(MySpaceTexts.JoinScreen_GTXGamingBanner));
			Controls.Add(myGuiControlImageButton);
		}

		private void onBannerClick(MyGuiControlImageButton button)
		{
			MyGuiSandbox.OpenUrl((string)button.UserData, UrlOpenMode.ExternalBrowser);
		}

		private void joinGameTabs_OnPageChanged()
		{
			MyGuiControlTabPage myGuiControlTabPage = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageServersPanel");
			myGuiControlTabPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Servers));
			MyGuiControlTabPage myGuiControlTabPage2 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageLobbiesPanel");
			myGuiControlTabPage2.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Lobbies));
			MyGuiControlTabPage myGuiControlTabPage3 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageFavoritesPanel");
			myGuiControlTabPage3.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Favorites));
			MyGuiControlTabPage myGuiControlTabPage4 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageHistoryPanel");
			myGuiControlTabPage4.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_History));
			MyGuiControlTabPage myGuiControlTabPage5 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageLANPanel");
			myGuiControlTabPage5.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_LAN));
			MyGuiControlTabPage myGuiControlTabPage6 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageFriendsPanel");
			myGuiControlTabPage6.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Friends));
			if (m_selectedPage == myGuiControlTabPage)
			{
				CloseServersPage();
			}
			else if (m_selectedPage == myGuiControlTabPage2)
			{
				CloseLobbyPage();
			}
			else if (m_selectedPage == myGuiControlTabPage3)
			{
				CloseFavoritesPage();
			}
			else if (m_selectedPage == myGuiControlTabPage5)
			{
				CloseLANPage();
			}
			else if (m_selectedPage == myGuiControlTabPage4)
			{
				CloseHistoryPage();
			}
			else if (m_selectedPage == myGuiControlTabPage6)
			{
				CloseFriendsPage();
			}
			m_selectedPage = m_joinGameTabs.GetTabSubControl(m_joinGameTabs.SelectedPage);
			InitPageControls(m_selectedPage);
			if (m_selectedPage == myGuiControlTabPage)
			{
				InitServersPage();
				EnableAdvancedSearch = true;
			}
			else if (m_selectedPage == myGuiControlTabPage2)
			{
				InitLobbyPage();
				EnableAdvancedSearch = false;
			}
			else if (m_selectedPage == myGuiControlTabPage3)
			{
				InitFavoritesPage();
				EnableAdvancedSearch = true;
			}
			else if (m_selectedPage == myGuiControlTabPage4)
			{
				InitHistoryPage();
				EnableAdvancedSearch = true;
			}
			else if (m_selectedPage == myGuiControlTabPage5)
			{
				InitLANPage();
				EnableAdvancedSearch = true;
			}
			else if (m_selectedPage == myGuiControlTabPage6)
			{
				InitFriendsPage();
				EnableAdvancedSearch = false;
			}
			if (m_contextMenu != null)
			{
				m_contextMenu.Deactivate();
				m_contextMenu = null;
			}
			m_contextMenu = new MyGuiControlContextMenu();
			m_contextMenu.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			m_contextMenu.Deactivate();
			m_contextMenu.ItemClicked += OnContextMenu_ItemClicked;
			Controls.Add(m_contextMenu);
			if (MyFakes.LIMITED_MAIN_MENU && !MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				myGuiControlTabPage.IsTabVisible = false;
				myGuiControlTabPage2.IsTabVisible = false;
				myGuiControlTabPage3.IsTabVisible = false;
				myGuiControlTabPage4.IsTabVisible = false;
				myGuiControlTabPage5.IsTabVisible = false;
				myGuiControlTabPage6.IsTabVisible = false;
			}
		}

		private void OnContextMenu_ItemClicked(MyGuiControlContextMenu sender, MyGuiControlContextMenu.EventArgs eventArgs)
		{
			ContextMenuFavoriteActionItem contextMenuFavoriteActionItem = (ContextMenuFavoriteActionItem)eventArgs.UserData;
			MyGameServerItem server = contextMenuFavoriteActionItem.Server;
			if (server != null)
			{
				switch (contextMenuFavoriteActionItem._Action)
				{
				case ContextMenuFavoriteAction.Add:
					MyGameService.AddFavoriteGame(server.NetAdr.Address.ToIPv4NetworkOrder(), (ushort)server.NetAdr.Port, (ushort)server.NetAdr.Port);
					break;
				case ContextMenuFavoriteAction.Remove:
					MyGameService.RemoveFavoriteGame(server.NetAdr.Address.ToIPv4NetworkOrder(), (ushort)server.NetAdr.Port, (ushort)server.NetAdr.Port);
					m_gamesTable.RemoveSelectedRow();
					m_favoritesPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (").Append(m_gamesTable.RowsCount)
						.Append(")");
					break;
				default:
					throw new InvalidBranchException();
				}
			}
		}

		private void InitPageControls(MyGuiControlTabPage page)
		{
			page.Controls.Clear();
			if (m_joinButton != null)
			{
				Controls.Remove(m_joinButton);
			}
			if (m_detailsButton != null)
			{
				Controls.Remove(m_detailsButton);
			}
			if (m_directConnectButton != null)
			{
				Controls.Remove(m_directConnectButton);
			}
			if (m_refreshButton != null)
			{
				Controls.Remove(m_refreshButton);
			}
			Vector2 value = new Vector2(-0.676f, -0.352f);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			m_gamesTable = new MyGuiControlTable();
			m_gamesTable.Position = value + new Vector2(minSizeGui.X, 0.067f);
			m_gamesTable.Size = new Vector2(1450f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 1f);
			m_gamesTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_gamesTable.VisibleRowsCount = 17;
			page.Controls.Add(m_gamesTable);
			Vector2 value2 = new Vector2(value.X, 0f) - new Vector2(-0.3137f, (0f - m_size.Value.Y) / 2f + 0.071f);
			Vector2 value3 = new Vector2(0.1825f, 0f);
			int num = 0;
			Controls.Add(m_detailsButton = MakeButton(value2 + value3 * num++, MyCommonTexts.JoinGame_ServerDetails, MySpaceTexts.ToolTipJoinGame_ServerDetails, ServerDetailsClick));
			Controls.Add(m_directConnectButton = MakeButton(value2 + value3 * num++, MyCommonTexts.JoinGame_DirectConnect, MySpaceTexts.ToolTipJoinGame_DirectConnect, DirectConnectClick));
			Controls.Add(m_refreshButton = MakeButton(value2 + value3 * num++, MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MySpaceTexts.ToolTipJoinGame_Refresh, null));
			Controls.Add(m_joinButton = MakeButton(value2 + value3 * num++, MyCommonTexts.ScreenMenuButtonJoinWorld, MySpaceTexts.ToolTipJoinGame_JoinWorld, null));
			m_joinButton.Enabled = false;
			m_detailsButton.Enabled = false;
			m_loadingWheel = new MyGuiControlRotatingWheel(m_joinButton.Position + new Vector2(0.2f, -0.026f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.22f);
			page.Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.895f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.895f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.895f / 2f, m_size.Value.Y / 2f - 0.152f), m_size.Value.X * 0.895f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.895f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.895f);
			Controls.Add(myGuiControlSeparatorList2);
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, MyStringId toolTip, Action<MyGuiControlButton> onClick)
		{
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(text), toolTip: MyTexts.GetString(toolTip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: onClick);
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool Update(bool hasFocus)
		{
			if (refresh_favorites && hasFocus)
			{
				refresh_favorites = false;
				m_joinButton.Enabled = false;
				m_detailsButton.Enabled = false;
				RebuildFavoritesList();
			}
			if (m_searchChanged && DateTime.Now.Subtract(m_searchLastChanged).Milliseconds > 500)
			{
				m_searchChanged = false;
				m_searchChangedFunc();
			}
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_remainingTimeUpdateFrame++;
				if (m_remainingTimeUpdateFrame % 50 == 0)
				{
					for (int i = 0; i < m_gamesTable.RowsCount; i++)
					{
						m_gamesTable.GetRow(i).Update();
					}
					m_remainingTimeUpdateFrame = 0;
				}
			}
			return base.Update(hasFocus);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F5))
			{
				this.RefreshRequest.InvokeIfNotNull(m_refreshButton);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT) && m_gamesTable == base.FocusedControl)
			{
				OnJoinServer(m_joinButton);
			}
		}

		private bool FilterSimple(MyCachedServerItem item, string searchText = null)
		{
			MyGameServerItem server = item.Server;
			if (server.AppID != MyGameService.AppId)
			{
				return false;
			}
			string map = server.Map;
			int serverVersion = server.ServerVersion;
			if (string.IsNullOrEmpty(map))
			{
				return false;
			}
			if (searchText != null && !StringExtensions.Contains(server.Name, searchText, StringComparison.CurrentCultureIgnoreCase) && !StringExtensions.Contains(server.Map, searchText, StringComparison.CurrentCultureIgnoreCase))
			{
				return false;
			}
			if (FilterOptions.AllowedGroups && !item.AllowedInGroup)
			{
				return false;
			}
			if (FilterOptions.SameVersion && serverVersion != (int)MyFinalBuildConstants.APP_VERSION)
			{
				return false;
			}
			if (FilterOptions.HasPassword.HasValue && FilterOptions.HasPassword.Value != server.Password)
			{
				return false;
			}
			if (MyFakes.ENABLE_MP_DATA_HASHES && FilterOptions.SameData)
			{
				string gameTagByPrefix = server.GetGameTagByPrefix("datahash");
				if (gameTagByPrefix != "" && gameTagByPrefix != m_dataHash)
				{
					return false;
				}
			}
			string gameTagByPrefix2 = server.GetGameTagByPrefix("gamemode");
			if (gameTagByPrefix2 == "C" && !FilterOptions.CreativeMode)
			{
				return false;
			}
			if (gameTagByPrefix2.StartsWith("S") && !FilterOptions.SurvivalMode)
			{
				return false;
			}
			ulong gameTagByPrefixUlong = server.GetGameTagByPrefixUlong("mods");
			if (FilterOptions.CheckMod && !FilterOptions.ModCount.ValueBetween(gameTagByPrefixUlong))
			{
				return false;
			}
			if (FilterOptions.CheckPlayer && !FilterOptions.PlayerCount.ValueBetween(server.Players))
			{
				return false;
			}
			if (FilterOptions.Ping > -1 && server.Ping > FilterOptions.Ping)
			{
				return false;
			}
			if (float.TryParse(server.GetGameTagByPrefix("view"), out float result) && FilterOptions.CheckDistance && !FilterOptions.ViewDistance.ValueBetween(result))
			{
				return false;
			}
			return true;
		}

		private bool FilterAdvanced(MyCachedServerItem item, string searchText = null)
		{
			if (!FilterSimple(item, searchText))
			{
				return false;
			}
			if (item.Rules == null || !item.Rules.Any())
			{
				return false;
			}
			if (!FilterOptions.FilterServer(item))
			{
				return false;
			}
			if (FilterOptions.Mods != null && FilterOptions.Mods.Any() && FilterOptions.AdvancedFilter)
			{
				if (FilterOptions.ModsExclusive)
				{
					if (!FilterOptions.Mods.All((ulong modId) => item.Mods.Contains(modId)))
					{
						return false;
					}
				}
				else if (item.Mods == null || !item.Mods.Any((ulong modId) => FilterOptions.Mods.Contains(modId)))
				{
					return false;
				}
			}
			m_loadingWheel.Visible = false;
			return true;
		}

		private void AdvancedSearchButtonClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_detailsButton != null && m_joinButton != null)
			{
				m_detailsButton.Enabled = false;
				m_joinButton.Enabled = false;
			}
			MyGuiScreenServerSearchSpace myGuiScreenServerSearchSpace = new MyGuiScreenServerSearchSpace(this);
			myGuiScreenServerSearchSpace.Closed += delegate
			{
				m_searchChangedFunc();
			};
			m_loadingWheel.Visible = false;
			MyGuiSandbox.AddScreen(myGuiScreenServerSearchSpace);
		}

		private void ServerDetailsClick(MyGuiControlButton detailButton)
		{
			if (m_gamesTable.SelectedRow == null)
			{
				return;
			}
			MyGameServerItem ser = m_gamesTable.SelectedRow.UserData as MyGameServerItem;
			if (ser != null)
			{
				MyCachedServerItem myCachedServerItem = (detailButton.UserData as HashSet<MyCachedServerItem>).FirstOrDefault((MyCachedServerItem x) => x.Server.NetAdr.Equals(ser.NetAdr));
				if (myCachedServerItem != null)
				{
					m_loadingWheel.Visible = false;
					MyGuiSandbox.AddScreen(new MyGuiScreenServerDetailsSpace(myCachedServerItem));
				}
			}
		}

		private void DirectConnectClick(MyGuiControlButton button)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenServerConnect());
		}

		private void OnBlockSearchTextChanged(string text)
		{
			if (m_detailsButton != null && m_joinButton != null)
			{
				m_detailsButton.Enabled = false;
				m_joinButton.Enabled = false;
			}
			m_searchChanged = true;
			m_searchLastChanged = DateTime.Now;
		}

		private void InitServersPage()
		{
			InitServersTable();
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshServersClick;
			this.RefreshRequest = OnRefreshServersClick;
			m_detailsButton.UserData = m_dedicatedServers;
			m_dedicatedResponding = true;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, (Action)delegate
			{
				RefreshServerGameList(resetSteamQuery: false);
			});
			m_serversPage = m_selectedPage;
			m_serversPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Servers));
			RefreshServerGameList(resetSteamQuery: true);
		}

		private void CloseServersPage()
		{
			CloseRequest();
			m_dedicatedResponding = false;
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, (Action)delegate
			{
				RefreshServerGameList(resetSteamQuery: false);
			});
		}

		private void InitServersTable()
		{
			m_gamesTable.ColumnsCount = (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME ? 10 : 9);
			m_gamesTable.ItemSelected += OnTableItemSelected;
			m_gamesTable.ItemSelected += OnServerTableItemSelected;
			m_gamesTable.ItemDoubleClicked += OnTableItemDoubleClick;
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetCustomColumnWidths(new float[10]
				{
					0.024f,
					0.024f,
					0.024f,
					0.24f,
					0.17f,
					0.19f,
					0.15f,
					0.09f,
					0.05f,
					0.07f
				});
				m_gamesTable.SetHeaderColumnMargin(8, new Thickness(0.01f, 0.01f, 0.005f, 0.01f));
			}
			else
			{
				m_gamesTable.SetCustomColumnWidths(new float[9]
				{
					0.024f,
					0.024f,
					0.024f,
					0.26f,
					0.17f,
					0.29f,
					0.09f,
					0.06f,
					0.07f
				});
				m_gamesTable.SetHeaderColumnMargin(7, new Thickness(0.01f, 0.01f, 0.005f, 0.01f));
			}
			int num = 3;
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetColumnComparison(num, TextComparison);
				m_gamesTable.SetColumnAlign(num);
				m_gamesTable.SetHeaderColumnAlign(num);
				num++;
			}
			m_gamesTable.SetColumnComparison(num, PlayerCountComparison);
			m_gamesTable.SetColumnAlign(num);
			m_gamesTable.SetHeaderColumnAlign(num);
			num++;
			int columnIdx = num;
			m_gamesTable.SetColumnComparison(num, PingComparison);
			m_gamesTable.SetColumnAlign(num);
			m_gamesTable.SetHeaderColumnAlign(num);
			num++;
			m_gamesTable.SetColumnComparison(num, ModsComparison);
			m_gamesTable.SetColumnAlign(num);
			m_gamesTable.SetHeaderColumnAlign(num);
			num++;
			m_gamesTable.SortByColumn(columnIdx);
		}

		private void OnJoinServer(MyGuiControlButton obj)
		{
			JoinSelectedServer();
		}

		private void OnServerTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			if (sender.SelectedRow == null)
			{
				return;
			}
			MyGameServerItem myGameServerItem = sender.SelectedRow.UserData as MyGameServerItem;
			if (myGameServerItem == null || myGameServerItem.NetAdr == null)
			{
				return;
			}
			MyGuiControlTable.Cell cell = sender.SelectedRow.GetCell(5);
			if (cell == null || cell.ToolTip == null)
			{
				return;
			}
			if (eventArgs.MouseButton == MyMouseButtonsEnum.Right)
			{
				m_contextMenu.CreateNewContextMenu();
				ContextMenuFavoriteAction contextMenuFavoriteAction = (m_selectedPage == m_favoritesPage) ? ContextMenuFavoriteAction.Remove : ContextMenuFavoriteAction.Add;
				MyStringId id = MyCommonTexts.JoinGame_Favorites_Remove;
				if (contextMenuFavoriteAction == ContextMenuFavoriteAction.Add)
				{
					id = MyCommonTexts.JoinGame_Favorites_Add;
				}
				m_contextMenu.AddItem(MyTexts.Get(id), "", "", new ContextMenuFavoriteActionItem
				{
					Server = myGameServerItem,
					_Action = contextMenuFavoriteAction
				});
				m_contextMenu.Activate();
			}
			else
			{
				m_contextMenu.Deactivate();
			}
		}

		private void OnTableItemDoubleClick(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			JoinSelectedServer();
		}

		private void JoinSelectedServer(bool checkPing = true)
		{
			MyGuiControlTable.Row selectedRow = m_gamesTable.SelectedRow;
			if (selectedRow == null)
			{
				return;
			}
			MyGameServerItem myGameServerItem = selectedRow.UserData as MyGameServerItem;
			if (myGameServerItem != null)
			{
				if (!MySandboxGame.Config.ExperimentalMode && myGameServerItem.Experimental)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorExperimental)));
					return;
				}
				if (checkPing && myGameServerItem.Ping > 150)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MyCommonTexts.MultiplayerWarningPing), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							JoinSelectedServer(checkPing: false);
						}
					}));
					return;
				}
				MyJoinGameHelper.JoinGame(myGameServerItem);
				MyLocalCache.SaveLastSessionInfo(null, isOnline: true, isLobby: false, myGameServerItem.Name, myGameServerItem.NetAdr.Address.ToString(), myGameServerItem.NetAdr.Port);
			}
			else
			{
				IMyLobby myLobby = selectedRow.UserData as IMyLobby;
				if (myLobby != null)
				{
					MyJoinGameHelper.JoinGame(myLobby);
					MyLocalCache.SaveLastSessionInfo(null, isOnline: true, isLobby: true, selectedRow.GetCell(0).Text.ToString(), myLobby.LobbyId.ToString(), 0);
				}
			}
		}

		private void OnRefreshServersClick(MyGuiControlButton obj)
		{
			if (m_detailsButton != null && m_joinButton != null)
			{
				m_detailsButton.Enabled = false;
				m_joinButton.Enabled = false;
			}
			switch (m_nextState)
			{
			case RefreshStateEnum.Pause:
				m_refreshPaused = true;
				m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldResume);
				m_nextState = RefreshStateEnum.Resume;
				m_loadingWheel.Visible = false;
				break;
			case RefreshStateEnum.Resume:
				m_refreshPaused = false;
				if (m_loadingWheel.Visible)
				{
					m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
					m_nextState = RefreshStateEnum.Pause;
					m_loadingWheel.Visible = true;
				}
				else
				{
					m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldRefresh);
					m_nextState = RefreshStateEnum.Refresh;
					m_loadingWheel.Visible = false;
				}
				RefreshServerGameList(resetSteamQuery: false);
				break;
			case RefreshStateEnum.Refresh:
				m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
				m_nextState = RefreshStateEnum.Pause;
				m_dedicatedServers.Clear();
				RefreshServerGameList(resetSteamQuery: true);
				m_loadingWheel.Visible = true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void OnServerCheckboxCheckChanged(MyGuiControlCheckbox checkbox)
		{
			RefreshServerGameList(resetSteamQuery: false);
		}

		private void OnDedicatedServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(MyGameService.GetDedicatedServerDetails(server));
			IPEndPoint netAdr = serverItem.Server.NetAdr;
			MyGameService.GetServerRules(netAdr.Address.ToIPv4NetworkOrder(), (ushort)netAdr.Port, delegate(Dictionary<string, string> rules)
			{
				DedicatedRulesResponse(rules, serverItem);
			}, delegate
			{
				DedicatedRulesResponse(null, serverItem);
			});
		}

		private void DedicatedRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
			if (server.Server.NetAdr == null || m_dedicatedServers.Any((MyCachedServerItem x) => server.Server.NetAdr.Equals(x.Server.NetAdr)))
			{
				return;
			}
			server.Rules = rules;
			if (rules != null)
			{
				server.DeserializeSettings();
			}
			m_dedicatedServers.Add(server);
			if (m_dedicatedResponding)
			{
				server.Server.IsRanked = IsRanked(server);
				AddServerItem(server);
				if (!m_refreshPaused)
				{
					m_serversPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Servers)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private bool IsRanked(MyCachedServerItem server)
		{
			if (m_rankedServers == null)
			{
				return false;
			}
			string address = server.Server.NetAdr.ToString();
			return m_rankedServers.Servers.Exists((MyRankServer r) => r.Address == address);
		}

		private void OnDedicatedServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseRequest();
		}

		private void CloseRequest()
		{
			m_loadingWheel.Visible = false;
			MyGameService.OnDedicatedServerListResponded -= OnDedicatedServerListResponded;
			MyGameService.OnDedicatedServersCompleteResponse -= OnDedicatedServersCompleteResponse;
			MyGameService.CancelInternetServersRequest();
			if (m_nextState == RefreshStateEnum.Pause)
			{
				m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldRefresh);
				m_nextState = RefreshStateEnum.Refresh;
				m_refreshPaused = false;
			}
		}

		private void AddServerHeaders()
		{
			int num = 0;
			int columnsCount = m_gamesTable.ColumnsCount;
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, new StringBuilder());
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, new StringBuilder());
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, new StringBuilder());
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_World));
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_GameMode));
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Server));
			}
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME && num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_RemainingTime));
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Players));
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Ping));
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Mods));
			}
		}

		private void RefreshServerGameList(bool resetSteamQuery)
		{
			if (m_lastVersionCheck != FilterOptions.SameVersion || FilterOptions.AdvancedFilter)
			{
				resetSteamQuery = true;
			}
			m_lastVersionCheck = FilterOptions.SameVersion;
			m_detailsButton.Enabled = false;
			m_joinButton.Enabled = false;
			m_gamesTable.Clear();
			AddServerHeaders();
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_serversPage.TextEnum = MyCommonTexts.JoinGame_TabTitle_Servers;
			if (resetSteamQuery)
			{
				m_dedicatedServers.Clear();
				CloseRequest();
				m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
				m_nextState = RefreshStateEnum.Pause;
				m_refreshPaused = false;
				string text = $"gamedir:{MyPerGameSettings.SteamGameServerGameDir};secure:1";
				if (FilterOptions.SameVersion)
				{
					text = text + ";gamedataand:" + MyFinalBuildConstants.APP_VERSION;
				}
				MySandboxGame.Log.WriteLine("Requesting dedicated servers, filterOps: " + text);
				MyGameService.OnDedicatedServerListResponded += OnDedicatedServerListResponded;
				MyGameService.OnDedicatedServersCompleteResponse += OnDedicatedServersCompleteResponse;
				MyGameService.RequestInternetServerList(text);
				m_loadingWheel.Visible = true;
			}
			m_gamesTable.SelectedRowIndex = null;
			RebuildServerList();
		}

		private void RebuildServerList()
		{
			string text = m_searchBox.SearchText;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = null;
			}
			m_detailsButton.Enabled = false;
			m_joinButton.Enabled = false;
			m_gamesTable.Clear();
			foreach (MyCachedServerItem dedicatedServer in m_dedicatedServers)
			{
				if (FilterOptions.AdvancedFilter)
				{
					if (!FilterAdvanced(dedicatedServer, text))
					{
						continue;
					}
				}
				else if (!FilterSimple(dedicatedServer, text))
				{
					continue;
				}
				MyGameServerItem server = dedicatedServer.Server;
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				string gameTagByPrefix = server.GetGameTagByPrefix("gamemode");
				if (gameTagByPrefix == "C")
				{
					stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
					stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
				}
				else if (!string.IsNullOrWhiteSpace(gameTagByPrefix))
				{
					string text2 = gameTagByPrefix.Substring(1);
					string[] array = text2.Split(new char[1]
					{
						'-'
					});
					if (array.Length == 4)
					{
						stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival)).Append(" ").Append(text2);
						stringBuilder2.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat), array[0], array[1], array[2], array[3]);
					}
					else
					{
						stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
						stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
					}
				}
				AddServerItem(server, server.Map, stringBuilder, stringBuilder2, sort: false, dedicatedServer.Settings);
			}
			m_gamesTable.Sort(switchSort: false);
			m_serversPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Servers)).Append(" (")
				.Append(m_gamesTable.RowsCount)
				.Append(")");
		}

		private bool AddServerItem(MyCachedServerItem item)
		{
			MyGameServerItem server = item.Server;
			server.Experimental = item.ExperimentalMode;
			if (FilterOptions.AdvancedFilter && item.Rules != null)
			{
				if (!FilterAdvanced(item, m_searchBox.SearchText))
				{
					return false;
				}
			}
			else if (!FilterSimple(item, m_searchBox.SearchText))
			{
				return false;
			}
			string map = server.Map;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			string gameTagByPrefix = server.GetGameTagByPrefix("gamemode");
			if (gameTagByPrefix == "C")
			{
				stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
				stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
			}
			else if (!string.IsNullOrWhiteSpace(gameTagByPrefix))
			{
				string text = gameTagByPrefix.Substring(1);
				string[] array = text.Split(new char[1]
				{
					'-'
				});
				if (array.Length == 4)
				{
					stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival)).Append(" ").Append(text);
					stringBuilder2.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat), array[0], array[1], array[2], array[3]);
				}
				else
				{
					stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
					stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
				}
			}
			if (!m_refreshPaused)
			{
				AddServerItem(server, map, stringBuilder, stringBuilder2, sort: true, item.Settings);
			}
			return true;
		}

		private void AddServerItem(MyGameServerItem server, string sessionName, StringBuilder gamemodeSB, StringBuilder gamemodeToolTipSB, bool sort = true, MyObjectBuilder_SessionSettings settings = null)
		{
			ulong gameTagByPrefixUlong = server.GetGameTagByPrefixUlong("mods");
			string arg = server.MaxPlayers.ToString();
			StringBuilder stringBuilder = new StringBuilder(server.Players + "/" + arg);
			string gameTagByPrefix = server.GetGameTagByPrefix("view");
			if (!string.IsNullOrEmpty(gameTagByPrefix))
			{
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance), gameTagByPrefix);
			}
			if (settings != null)
			{
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Max), settings.TotalPCU);
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Settings), settings.BlockLimitsEnabled);
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Initial), MyObjectBuilder_SessionSettings.GetInitialPCU(settings));
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_Airtightness), settings.EnableOxygenPressurization ? MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_ON) : MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_OFF));
			}
			Color? textColor = Color.White;
			if (server.Experimental && !MySandboxGame.Config.ExperimentalMode)
			{
				textColor = Color.DarkGray;
			}
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(server);
			string toolTip = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Rank).ToString();
			if (server.IsRanked)
			{
				StringBuilder text = new StringBuilder();
				MyGuiHighlightTexture? icon = MyGuiConstants.TEXTURE_ICON_STAR;
				row.AddCell(new MyGuiControlTable.Cell(text, null, toolTip, textColor, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			else
			{
				row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, toolTip, textColor, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			string toolTip2 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Passworded).ToString();
			if (server.Password)
			{
				StringBuilder text2 = new StringBuilder();
				MyGuiHighlightTexture? icon = MyGuiConstants.TEXTURE_ICON_LOCK;
				row.AddCell(new MyGuiControlTable.Cell(text2, null, toolTip2, textColor, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			else
			{
				row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, null, textColor, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			if (server.Experimental)
			{
				StringBuilder text3 = new StringBuilder();
				string @string = MyTexts.GetString(MyCommonTexts.ServerIsExperimental);
				MyGuiHighlightTexture? icon = MyGuiConstants.TEXTURE_ICON_EXPERIMENTAL;
				row.AddCell(new MyGuiControlTable.Cell(text3, null, @string, textColor, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			else
			{
				row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, null, textColor, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			m_textCache.Clear().Append(sessionName);
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendLine(sessionName);
			if (server.Experimental)
			{
				stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.ServerIsExperimental));
			}
			row.AddCell(new MyGuiControlTable.Cell(m_textCache, server.GameID, stringBuilder2.ToString(), textColor));
			row.AddCell(new MyGuiControlTable.Cell(gamemodeSB, null, gamemodeToolTipSB.ToString(), textColor));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(server.Name), null, m_gameTypeToolTip.Clear().AppendLine(server.Name).Append(server.NetAdr)
				.ToString(), textColor));
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder, null, stringBuilder.ToString(), textColor));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(server.Ping), null, m_textCache.ToString(), textColor));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append((gameTagByPrefixUlong == 0L) ? "---" : gameTagByPrefixUlong.ToString()), null, null, textColor));
			if (server.IsRanked)
			{
				row.IsGlobalSortEnabled = false;
				m_gamesTable.Insert(0, row);
			}
			else
			{
				m_gamesTable.Add(row);
			}
			if (sort && !server.IsRanked)
			{
				MyGuiControlTable.Row selectedRow = m_gamesTable.SelectedRow;
				m_gamesTable.Sort(switchSort: false);
				m_gamesTable.SelectedRowIndex = m_gamesTable.FindRow(selectedRow);
			}
		}

		private void InitLobbyPage()
		{
			InitLobbyTable();
			m_detailsButton.Enabled = false;
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshLobbiesClick;
			this.RefreshRequest = OnRefreshLobbiesClick;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshGameList));
			m_lobbyPage = m_selectedPage;
			m_lobbyPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Lobbies));
			LoadPublicLobbies();
		}

		private void CloseLobbyPage()
		{
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(LoadPublicLobbies));
		}

		private void InitLobbyTable()
		{
			m_gamesTable.ColumnsCount = (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME ? 6 : 5);
			m_gamesTable.ItemSelected += OnTableItemSelected;
			m_gamesTable.ItemDoubleClicked += OnTableItemDoubleClick;
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetCustomColumnWidths(new float[6]
				{
					0.3f,
					0.18f,
					0.2f,
					0.16f,
					0.08f,
					0.07f
				});
			}
			else
			{
				m_gamesTable.SetCustomColumnWidths(new float[5]
				{
					0.29f,
					0.19f,
					0.37f,
					0.08f,
					0.07f
				});
			}
			int num = 0;
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetColumnComparison(num, TextComparison);
				m_gamesTable.SetColumnAlign(num);
				m_gamesTable.SetHeaderColumnAlign(num);
				num++;
			}
			m_gamesTable.SetColumnComparison(num, PlayerCountComparison);
			m_gamesTable.SetColumnAlign(num);
			m_gamesTable.SetHeaderColumnAlign(num);
			num++;
			m_gamesTable.SetColumnComparison(num, ModsComparison);
			m_gamesTable.SetColumnAlign(num);
			m_gamesTable.SetHeaderColumnAlign(num);
			num++;
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			sender.CanHaveFocus = true;
			base.FocusedControl = sender;
			if (m_gamesTable.SelectedRow != null)
			{
				m_joinButton.Enabled = true;
				if (m_gamesTable.SelectedRow.UserData is MyGameServerItem)
				{
					m_detailsButton.Enabled = true;
				}
			}
			else
			{
				m_joinButton.Enabled = false;
				m_detailsButton.Enabled = false;
			}
		}

		private void OnRefreshLobbiesClick(MyGuiControlButton obj)
		{
			LoadPublicLobbies();
		}

		private void OnShowCompatibleCheckChanged(MyGuiControlCheckbox checkbox)
		{
			LoadPublicLobbies();
		}

		private void OnShowOnlyFriendsCheckChanged(MyGuiControlCheckbox checkbox)
		{
			LoadPublicLobbies();
		}

		private void PublicLobbiesCallback(bool success)
		{
			if (m_selectedPage == m_lobbyPage)
			{
				if (!success)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder("Cannot enumerate worlds")));
					return;
				}
				m_lobbies.Clear();
				MyGameService.AddPublicLobbies(m_lobbies);
				RefreshGameList();
				m_loadingWheel.Visible = false;
			}
		}

		private void LoadPublicLobbies()
		{
			m_loadingWheel.Visible = true;
			MySandboxGame.Log.WriteLine("Requesting lobbies");
			if (FilterOptions.SameVersion)
			{
				MyGameService.AddLobbyFilter("appVersion", MyFinalBuildConstants.APP_VERSION.ToString());
			}
			MySandboxGame.Log.WriteLine("Requesting worlds, only compatible: " + FilterOptions.SameVersion);
			MyGameService.RequestLobbyList(PublicLobbiesCallback);
		}

		private void AddHeaders()
		{
			int num = 0;
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_World));
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_GameMode));
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Username));
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_RemainingTime));
			}
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Players));
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Mods));
		}

		private void RefreshGameList()
		{
			m_gamesTable.Clear();
			AddHeaders();
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_lobbyPage.Text = MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Lobbies);
			if (m_lobbies != null)
			{
				int num = 0;
				for (int i = 0; i < m_lobbies.Count; i++)
				{
					IMyLobby myLobby = m_lobbies[i];
					MyGuiControlTable.Row row = new MyGuiControlTable.Row(myLobby);
					if (FilterOptions.AdvancedFilter && !FilterOptions.FilterLobby(myLobby))
					{
						continue;
					}
					string lobbyWorldName = MyMultiplayerLobby.GetLobbyWorldName(myLobby);
					MyMultiplayerLobby.GetLobbyWorldSize(myLobby);
					int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(myLobby);
					int lobbyModCount = MyMultiplayerLobby.GetLobbyModCount(myLobby);
					string text = null;
					float? num2 = null;
					string text2 = m_searchBox.SearchText.Trim();
					if (!string.IsNullOrWhiteSpace(text2) && !lobbyWorldName.ToLower().Contains(text2.ToLower()))
					{
						continue;
					}
					m_gameTypeText.Clear();
					m_gameTypeToolTip.Clear();
					float lobbyFloat = MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", myLobby, 1f);
					float lobbyFloat2 = MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", myLobby, 1f);
					float lobbyFloat3 = MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", myLobby, 1f);
					float lobbyFloat4 = MyMultiplayerLobby.GetLobbyFloat("blocksInventoryMultiplier", myLobby, 1f);
					MyGameModeEnum lobbyGameMode = MyMultiplayerLobby.GetLobbyGameMode(myLobby);
					if (MyMultiplayerLobby.GetLobbyScenario(myLobby))
					{
						m_gameTypeText.AppendStringBuilder(MyTexts.Get(MySpaceTexts.WorldSettings_GameScenario));
						DateTime lobbyDateTime = MyMultiplayerLobby.GetLobbyDateTime("scenarioStartTime", myLobby, DateTime.MinValue);
						if (lobbyDateTime > DateTime.MinValue)
						{
							TimeSpan timeSpan = DateTime.UtcNow - lobbyDateTime;
							double num3 = Math.Truncate(timeSpan.TotalHours);
							int num4 = (int)((timeSpan.TotalHours - num3) * 60.0);
							m_gameTypeText.Append(" ").Append(num3).Append(":")
								.Append(num4.ToString("D2"));
						}
						else
						{
							m_gameTypeText.Append(" Lobby");
						}
					}
					else
					{
						switch (lobbyGameMode)
						{
						case MyGameModeEnum.Creative:
							if (!FilterOptions.CreativeMode)
							{
								continue;
							}
							m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative));
							break;
						case MyGameModeEnum.Survival:
							if (!FilterOptions.SurvivalMode)
							{
								continue;
							}
							m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival));
							m_gameTypeText.Append($" {lobbyFloat}-{lobbyFloat4}-{lobbyFloat3}-{lobbyFloat2}");
							break;
						}
					}
					m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat).ToString(), lobbyFloat, lobbyFloat4, lobbyFloat3, lobbyFloat2);
					int lobbyViewDistance = MyMultiplayerLobby.GetLobbyViewDistance(myLobby);
					m_gameTypeToolTip.AppendLine();
					m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance).ToString(), lobbyViewDistance);
					if (string.IsNullOrEmpty(lobbyWorldName) || (FilterOptions.SameVersion && lobbyAppVersion != (int)MyFinalBuildConstants.APP_VERSION) || (FilterOptions.SameData && MyFakes.ENABLE_MP_DATA_HASHES && !MyMultiplayerLobby.HasSameData(myLobby)))
					{
						continue;
					}
					string lobbyHostName = MyMultiplayerLobby.GetLobbyHostName(myLobby);
					string arg = myLobby.MemberLimit.ToString();
					string value = myLobby.MemberCount + "/" + arg;
					if ((FilterOptions.CheckDistance && !FilterOptions.ViewDistance.ValueBetween(MyMultiplayerLobby.GetLobbyViewDistance(myLobby))) || (FilterOptions.CheckPlayer && !FilterOptions.PlayerCount.ValueBetween(myLobby.MemberCount)) || (FilterOptions.CheckMod && !FilterOptions.ModCount.ValueBetween(lobbyModCount)))
					{
						continue;
					}
					List<MyObjectBuilder_Checkpoint.ModItem> lobbyMods = MyMultiplayerLobby.GetLobbyMods(myLobby);
					if (FilterOptions.Mods != null && FilterOptions.Mods.Any() && FilterOptions.AdvancedFilter)
					{
						if (FilterOptions.ModsExclusive)
						{
							bool flag = false;
							foreach (ulong modId in FilterOptions.Mods)
							{
								if (lobbyMods == null || !lobbyMods.Any((MyObjectBuilder_Checkpoint.ModItem m) => m.PublishedFileId == modId))
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								continue;
							}
						}
						else if (lobbyMods == null || !lobbyMods.Any((MyObjectBuilder_Checkpoint.ModItem m) => FilterOptions.Mods.Contains(m.PublishedFileId)))
						{
							continue;
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					int val = 15;
					int num5 = Math.Min(val, lobbyModCount - 1);
					foreach (MyObjectBuilder_Checkpoint.ModItem item in lobbyMods)
					{
						if (val-- <= 0)
						{
							stringBuilder.Append("...");
							break;
						}
						if (num5-- <= 0)
						{
							stringBuilder.Append(item.FriendlyName);
						}
						else
						{
							stringBuilder.AppendLine(item.FriendlyName);
						}
					}
					row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(lobbyWorldName), myLobby.LobbyId, m_textCache.ToString()));
					row.AddCell(new MyGuiControlTable.Cell(m_gameTypeText, null, (m_gameTypeToolTip.Length > 0) ? m_gameTypeToolTip.ToString() : null));
					row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(lobbyHostName), null, m_textCache.ToString()));
					if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
					{
						if (text != null)
						{
							row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(text)));
						}
						else if (num2.HasValue)
						{
							row.AddCell(new CellRemainingTime(num2.Value));
						}
						else
						{
							row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear()));
						}
					}
					row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(value)));
					row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append((lobbyModCount == 0) ? "---" : lobbyModCount.ToString()), null, stringBuilder.ToString()));
					m_gamesTable.Add(row);
					num++;
				}
				m_lobbyPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Lobbies)).Append(" (").Append(num)
					.Append(")");
			}
			m_gamesTable.SelectedRowIndex = null;
		}

		public void RemoveFavoriteServer(MyCachedServerItem server)
		{
			m_favoriteServers.Remove(server);
			refresh_favorites = true;
		}

		private void InitFavoritesPage()
		{
			InitServersTable();
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshFavoritesServersClick;
			this.RefreshRequest = OnRefreshFavoritesServersClick;
			m_detailsButton.UserData = m_favoriteServers;
			m_favoritesResponding = true;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshFavoritesGameList));
			m_favoritesPage = m_selectedPage;
			m_favoritesPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Favorites));
			RefreshFavoritesGameList();
		}

		private void CloseFavoritesPage()
		{
			CloseFavoritesRequest();
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(RefreshFavoritesGameList));
			m_favoritesResponding = false;
		}

		private void OnRefreshFavoritesServersClick(MyGuiControlButton obj)
		{
			RefreshFavoritesGameList();
		}

		private void OnFavoritesCheckboxCheckChanged(MyGuiControlCheckbox checkbox)
		{
			RefreshFavoritesGameList();
		}

		private void RefreshFavoritesGameList()
		{
			CloseFavoritesRequest();
			m_gamesTable.Clear();
			AddServerHeaders();
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_favoriteServers.Clear();
			m_favoritesPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Favorites));
			MySandboxGame.Log.WriteLine("Requesting dedicated servers");
			MyGameService.OnFavoritesServerListResponded += OnFavoritesServerListResponded;
			MyGameService.OnFavoritesServersCompleteResponse += OnFavoritesServersCompleteResponse;
			string text = $"gamedir:{MyPerGameSettings.SteamGameServerGameDir};secure:1";
			MySandboxGame.Log.WriteLine("Requesting favorite servers, filterOps: " + text);
			MyGameService.RequestFavoritesServerList(text);
			m_loadingWheel.Visible = true;
			m_gamesTable.SelectedRowIndex = null;
		}

		private void OnFavoritesServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(MyGameService.GetFavoritesServerDetails(server));
			IPEndPoint netAdr = serverItem.Server.NetAdr;
			MyGameService.GetServerRules(netAdr.Address.ToIPv4NetworkOrder(), (ushort)netAdr.Port, delegate(Dictionary<string, string> rules)
			{
				FavoritesRulesResponse(rules, serverItem);
			}, delegate
			{
				FavoritesRulesResponse(null, serverItem);
			});
		}

		private void FavoritesRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
			if (server.Server.NetAdr == null || m_favoriteServers.Any((MyCachedServerItem x) => server.Server.NetAdr.Equals(x.Server.NetAdr)))
			{
				return;
			}
			server.Rules = rules;
			if (rules != null)
			{
				server.DeserializeSettings();
			}
			m_favoriteServers.Add(server);
			if (m_favoritesResponding)
			{
				server.Server.IsRanked = IsRanked(server);
				AddServerItem(server);
				if (!m_refreshPaused)
				{
					m_favoritesPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private void RebuildFavoritesList()
		{
			m_detailsButton.Enabled = false;
			m_joinButton.Enabled = false;
			m_gamesTable.Clear();
			foreach (MyCachedServerItem favoriteServer in m_favoriteServers)
			{
				AddServerItem(favoriteServer);
			}
			m_favoritesPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (")
				.Append(m_gamesTable.RowsCount)
				.Append(")");
		}

		private void OnFavoritesServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseFavoritesRequest();
		}

		private void CloseFavoritesRequest()
		{
			MyGameService.OnFavoritesServerListResponded -= OnFavoritesServerListResponded;
			MyGameService.OnFavoritesServersCompleteResponse -= OnFavoritesServersCompleteResponse;
			MyGameService.CancelFavoritesServersRequest();
			m_loadingWheel.Visible = false;
		}

		private void InitHistoryPage()
		{
			InitServersTable();
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshHistoryServersClick;
			this.RefreshRequest = OnRefreshHistoryServersClick;
			m_historyResponding = true;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshHistoryGameList));
			m_historyPage = m_selectedPage;
			m_historyPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_History));
			m_detailsButton.UserData = m_historyServers;
			RefreshHistoryGameList();
		}

		private void CloseHistoryPage()
		{
			CloseHistoryRequest();
			m_historyResponding = false;
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(RefreshHistoryGameList));
		}

		private void OnRefreshHistoryServersClick(MyGuiControlButton obj)
		{
			RefreshHistoryGameList();
		}

		private void OnHistoryCheckboxCheckChanged(MyGuiControlCheckbox checkbox)
		{
			RefreshHistoryGameList();
		}

		private void RefreshHistoryGameList()
		{
			CloseHistoryRequest();
			m_gamesTable.Clear();
			AddServerHeaders();
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_historyServers.Clear();
			m_historyPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_History));
			MySandboxGame.Log.WriteLine("Requesting dedicated servers");
			MyGameService.OnHistoryServerListResponded += OnHistoryServerListResponded;
			MyGameService.OnHistoryServersCompleteResponse += OnHistoryServersCompleteResponse;
			string text = $"gamedir:{MyPerGameSettings.SteamGameServerGameDir};secure:1";
			MySandboxGame.Log.WriteLine("Requesting history servers, filterOps: " + text);
			MyGameService.RequestHistoryServerList(text);
			m_loadingWheel.Visible = true;
			m_gamesTable.SelectedRowIndex = null;
		}

		private void OnHistoryServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(MyGameService.GetHistoryServerDetails(server));
			IPEndPoint netAdr = serverItem.Server.NetAdr;
			MyGameService.GetServerRules(netAdr.Address.ToIPv4NetworkOrder(), (ushort)netAdr.Port, delegate(Dictionary<string, string> rules)
			{
				HistoryRulesResponse(rules, serverItem);
			}, delegate
			{
				HistoryRulesResponse(null, serverItem);
			});
		}

		private void HistoryRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
			if (server.Server.NetAdr == null || m_historyServers.Any((MyCachedServerItem x) => server.Server.NetAdr.Equals(x.Server.NetAdr)))
			{
				return;
			}
			server.Rules = rules;
			if (rules != null)
			{
				server.DeserializeSettings();
			}
			m_historyServers.Add(server);
			if (m_historyResponding)
			{
				server.Server.IsRanked = IsRanked(server);
				AddServerItem(server);
				if (!m_refreshPaused)
				{
					m_historyPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_History)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private void OnHistoryServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseHistoryRequest();
		}

		private void CloseHistoryRequest()
		{
			MyGameService.OnHistoryServerListResponded -= OnHistoryServerListResponded;
			MyGameService.OnHistoryServersCompleteResponse -= OnHistoryServersCompleteResponse;
			MyGameService.CancelHistoryServersRequest();
			m_loadingWheel.Visible = false;
		}

		private void InitLANPage()
		{
			InitServersTable();
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshLANServersClick;
			this.RefreshRequest = OnRefreshLANServersClick;
			m_detailsButton.UserData = m_lanServers;
			m_lanResponding = true;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshLANGameList));
			m_LANPage = m_selectedPage;
			m_LANPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_LAN));
			RefreshLANGameList();
		}

		private void CloseLANPage()
		{
			CloseLANRequest();
			m_lanResponding = false;
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(RefreshLANGameList));
		}

		private void OnRefreshLANServersClick(MyGuiControlButton obj)
		{
			RefreshLANGameList();
		}

		private void OnLANCheckboxCheckChanged(MyGuiControlCheckbox checkbox)
		{
			RefreshLANGameList();
		}

		private void RefreshLANGameList()
		{
			CloseLANRequest();
			m_gamesTable.Clear();
			AddServerHeaders();
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_lanServers.Clear();
			m_LANPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_LAN));
			MySandboxGame.Log.WriteLine("Requesting dedicated servers");
			MyGameService.OnLANServerListResponded += OnLANServerListResponded;
			MyGameService.OnLANServersCompleteResponse += OnLANServersCompleteResponse;
			MyGameService.RequestLANServerList();
			m_loadingWheel.Visible = true;
			m_gamesTable.SelectedRowIndex = null;
		}

		private void OnLANServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(MyGameService.GetLANServerDetails(server));
			IPEndPoint netAdr = serverItem.Server.NetAdr;
			MyGameService.GetServerRules(netAdr.Address.ToIPv4NetworkOrder(), (ushort)netAdr.Port, delegate(Dictionary<string, string> rules)
			{
				LanRulesResponse(rules, serverItem);
			}, delegate
			{
				LanRulesResponse(null, serverItem);
			});
		}

		private void LanRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
			if (server.Server.NetAdr == null || m_lanServers.Any((MyCachedServerItem x) => server.Server.NetAdr.Equals(x.Server.NetAdr)))
			{
				return;
			}
			server.Rules = rules;
			if (rules != null)
			{
				server.DeserializeSettings();
			}
			m_lanServers.Add(server);
			if (m_lanResponding)
			{
				server.Server.IsRanked = IsRanked(server);
				AddServerItem(server);
				if (!m_refreshPaused)
				{
					m_LANPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_LAN)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private void OnLANServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseLANRequest();
		}

		private void CloseLANRequest()
		{
			MyGameService.OnLANServerListResponded -= OnLANServerListResponded;
			MyGameService.OnLANServersCompleteResponse -= OnLANServersCompleteResponse;
			MyGameService.CancelLANServersRequest();
			m_loadingWheel.Visible = false;
		}

		private void InitFriendsPage()
		{
			InitServersTable();
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshFriendsServersClick;
			this.RefreshRequest = OnRefreshFriendsServersClick;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshFriendsGameList));
			m_detailsButton.UserData = m_dedicatedServers;
			m_friendsPage = m_selectedPage;
			m_friendsPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Friends));
			if (m_friendIds == null)
			{
				m_friendIds = new HashSet<ulong>();
				m_friendNames = new HashSet<string>();
				RequestFriendsList();
			}
			RefreshFriendsGameList();
		}

		private void InitFriendsTable()
		{
			m_gamesTable.ColumnsCount = (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME ? 7 : 6);
			m_gamesTable.ItemSelected += OnTableItemSelected;
			m_gamesTable.ItemSelected += OnServerTableItemSelected;
			m_gamesTable.ItemDoubleClicked += OnTableItemDoubleClick;
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetCustomColumnWidths(new float[7]
				{
					0.26f,
					0.18f,
					0.2f,
					0.16f,
					0.08f,
					0.05f,
					0.07f
				});
			}
			else
			{
				m_gamesTable.SetCustomColumnWidths(new float[6]
				{
					0.3f,
					0.19f,
					0.31f,
					0.08f,
					0.05f,
					0.07f
				});
			}
			int num = 0;
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetColumnComparison(num, TextComparison);
				m_gamesTable.SetColumnAlign(num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				m_gamesTable.SetHeaderColumnAlign(num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				num++;
			}
			m_gamesTable.SetColumnComparison(num, PlayerCountComparison);
			m_gamesTable.SetColumnAlign(num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_gamesTable.SetHeaderColumnAlign(num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			num++;
			int columnIdx = num;
			m_gamesTable.SetColumnComparison(num, PingComparison);
			m_gamesTable.SetColumnAlign(num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_gamesTable.SetHeaderColumnAlign(num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			num++;
			m_gamesTable.SetColumnComparison(num, ModsComparison);
			m_gamesTable.SetColumnAlign(num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_gamesTable.SetHeaderColumnAlign(num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			num++;
			m_gamesTable.SortByColumn(columnIdx);
		}

		private void CloseFriendsPage()
		{
			CloseFriendsRequest();
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(RefreshFriendsGameList));
		}

		private void OnRefreshFriendsServersClick(MyGuiControlButton obj)
		{
			RefreshFriendsGameList();
		}

		private void RequestFriendsList()
		{
			_ = DateTime.Now;
			int friendsCount = MyGameService.GetFriendsCount();
			for (int i = 0; i < friendsCount; i++)
			{
				ulong friendIdByIndex = MyGameService.GetFriendIdByIndex(i);
				string friendNameByIndex = MyGameService.GetFriendNameByIndex(i);
				m_friendIds.Add(friendIdByIndex);
				m_friendNames.Add(friendNameByIndex);
			}
		}

		private void RefreshFriendsGameList()
		{
			CloseFriendsRequest();
			m_gamesTable.Clear();
			AddServerHeaders();
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_friendsPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Friends));
			MySandboxGame.Log.WriteLine("Requesting dedicated servers");
			CloseFriendsRequest();
			if (FilterOptions.SameVersion)
			{
				MyGameService.AddLobbyFilter("appVersion", MyFinalBuildConstants.APP_VERSION.ToString());
			}
			MyGameService.RequestLobbyList(FriendsLobbyResponse);
			m_dedicatedServers.Clear();
			m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.Refresh);
			m_nextState = RefreshStateEnum.Pause;
			m_refreshPaused = false;
			string text = $"gamedir:{MyPerGameSettings.SteamGameServerGameDir};secure:1";
			if (FilterOptions.SameVersion)
			{
				text = text + ";gamedataand:" + MyFinalBuildConstants.APP_VERSION;
			}
			MySandboxGame.Log.WriteLine("Requesting dedicated servers, filterOps: " + text);
			MyGameService.OnDedicatedServerListResponded += OnFriendsServerListResponded;
			MyGameService.OnDedicatedServersCompleteResponse += OnFriendsServersCompleteResponse;
			MyGameService.RequestInternetServerList(text);
			m_loadingWheel.Visible = true;
			m_loadingWheel.Visible = true;
			m_gamesTable.SelectedRowIndex = null;
		}

		private void FriendsLobbyResponse(bool success)
		{
			if (!success)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder("Cannot enumerate worlds")));
			}
			m_lobbies.Clear();
			MyGameService.AddFriendLobbies(m_lobbies);
			MyGameService.AddPublicLobbies(m_lobbies);
			foreach (IMyLobby lobby in m_lobbies)
			{
				if (m_friendIds.Contains(lobby.OwnerId) || m_friendIds.Contains(MyMultiplayerLobby.GetLobbyHostSteamId(lobby)) || m_friendIds.Contains(lobby.LobbyId) || (lobby.MemberList != null && lobby.MemberList.Any((ulong m) => m_friendIds.Contains(m))))
				{
					lock (m_friendsPage)
					{
						AddLobby(lobby);
					}
				}
			}
		}

		private void OnFriendsServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(MyGameService.GetDedicatedServerDetails(server));
			if (serverItem.Server.Players > 0)
			{
				MyGameService.GetPlayerDetails(serverItem.Server.NetAdr.Address.ToIPv4NetworkOrder(), (ushort)serverItem.Server.NetAdr.Port, delegate(Dictionary<string, float> players)
				{
					LoadPlayersCompleted(players, serverItem);
				}, delegate
				{
					LoadPlayersCompleted(null, serverItem);
				});
			}
		}

		private void LoadPlayersCompleted(Dictionary<string, float> players, MyCachedServerItem serverItem)
		{
			if (players != null && players.Keys.Any((string n) => m_friendNames.Contains(n)))
			{
				IPEndPoint netAdr = serverItem.Server.NetAdr;
				MyGameService.GetServerRules(netAdr.Address.ToIPv4NetworkOrder(), (ushort)netAdr.Port, delegate(Dictionary<string, string> rules)
				{
					FriendRulesResponse(rules, serverItem);
				}, delegate
				{
					FriendRulesResponse(null, serverItem);
				});
			}
		}

		private void FriendRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
			if (server.Server.NetAdr != null && !m_dedicatedServers.Any((MyCachedServerItem x) => server.Server.NetAdr.Equals(x.Server.NetAdr)))
			{
				server.Rules = rules;
				if (rules != null)
				{
					server.DeserializeSettings();
				}
				m_dedicatedServers.Add(server);
				server.Server.IsRanked = IsRanked(server);
				lock (m_friendsPage)
				{
					AddServerItem(server);
					m_friendsPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Friends)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private void OnFriendsServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseFriendsRequest();
		}

		private void CloseFriendsRequest()
		{
			MyGameService.OnDedicatedServerListResponded -= OnFriendsServerListResponded;
			MyGameService.OnDedicatedServersCompleteResponse -= OnFriendsServersCompleteResponse;
			m_loadingWheel.Visible = false;
		}

		private void AddLobby(IMyLobby lobby)
		{
			if (FilterOptions.AdvancedFilter && !FilterOptions.FilterLobby(lobby))
			{
				return;
			}
			string lobbyWorldName = MyMultiplayerLobby.GetLobbyWorldName(lobby);
			MyMultiplayerLobby.GetLobbyWorldSize(lobby);
			int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(lobby);
			int lobbyModCount = MyMultiplayerLobby.GetLobbyModCount(lobby);
			string text = m_searchBox.SearchText.Trim();
			if (!string.IsNullOrWhiteSpace(text) && !lobbyWorldName.ToLower().Contains(text.ToLower()))
			{
				return;
			}
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			float lobbyFloat = MyMultiplayerLobby.GetLobbyFloat("blocksInventoryMultiplier", lobby, 1f);
			float lobbyFloat2 = MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", lobby, 1f);
			float lobbyFloat3 = MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", lobby, 1f);
			float lobbyFloat4 = MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", lobby, 1f);
			MyGameModeEnum lobbyGameMode = MyMultiplayerLobby.GetLobbyGameMode(lobby);
			if (MyMultiplayerLobby.GetLobbyScenario(lobby))
			{
				m_gameTypeText.AppendStringBuilder(MyTexts.Get(MySpaceTexts.WorldSettings_GameScenario));
				DateTime lobbyDateTime = MyMultiplayerLobby.GetLobbyDateTime("scenarioStartTime", lobby, DateTime.MinValue);
				if (lobbyDateTime > DateTime.MinValue)
				{
					TimeSpan timeSpan = DateTime.UtcNow - lobbyDateTime;
					double num = Math.Truncate(timeSpan.TotalHours);
					int num2 = (int)((timeSpan.TotalHours - num) * 60.0);
					m_gameTypeText.Append(" ").Append(num).Append(":")
						.Append(num2.ToString("D2"));
				}
				else
				{
					m_gameTypeText.Append(" Lobby");
				}
			}
			else
			{
				switch (lobbyGameMode)
				{
				case MyGameModeEnum.Creative:
					if (!FilterOptions.CreativeMode)
					{
						return;
					}
					m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative));
					break;
				case MyGameModeEnum.Survival:
					if (!FilterOptions.SurvivalMode)
					{
						return;
					}
					m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival));
					m_gameTypeText.Append($" {lobbyFloat2}-{lobbyFloat}-{lobbyFloat4}-{lobbyFloat3}");
					break;
				}
			}
			m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat).ToString(), lobbyFloat2, lobbyFloat, lobbyFloat4, lobbyFloat3);
			int lobbyViewDistance = MyMultiplayerLobby.GetLobbyViewDistance(lobby);
			m_gameTypeToolTip.AppendLine();
			m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance).ToString(), lobbyViewDistance);
			if (string.IsNullOrEmpty(lobbyWorldName) || (FilterOptions.SameVersion && lobbyAppVersion != (int)MyFinalBuildConstants.APP_VERSION) || (FilterOptions.SameData && MyFakes.ENABLE_MP_DATA_HASHES && !MyMultiplayerLobby.HasSameData(lobby)))
			{
				return;
			}
			string lobbyHostName = MyMultiplayerLobby.GetLobbyHostName(lobby);
			string arg = lobby.MemberLimit.ToString();
			string value = lobby.MemberCount + "/" + arg;
			if ((FilterOptions.CheckDistance && !FilterOptions.ViewDistance.ValueBetween(MyMultiplayerLobby.GetLobbyViewDistance(lobby))) || (FilterOptions.CheckPlayer && !FilterOptions.PlayerCount.ValueBetween(lobby.MemberCount)) || (FilterOptions.CheckMod && !FilterOptions.ModCount.ValueBetween(lobbyModCount)))
			{
				return;
			}
			List<MyObjectBuilder_Checkpoint.ModItem> lobbyMods = MyMultiplayerLobby.GetLobbyMods(lobby);
			if (FilterOptions.Mods != null && FilterOptions.Mods.Any() && FilterOptions.AdvancedFilter)
			{
				if (FilterOptions.ModsExclusive)
				{
					bool flag = false;
					foreach (ulong modId in FilterOptions.Mods)
					{
						if (lobbyMods == null || !lobbyMods.Any((MyObjectBuilder_Checkpoint.ModItem m) => m.PublishedFileId == modId))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						return;
					}
				}
				else if (lobbyMods == null || !lobbyMods.Any((MyObjectBuilder_Checkpoint.ModItem m) => FilterOptions.Mods.Contains(m.PublishedFileId)))
				{
					return;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			int val = 15;
			int num3 = Math.Min(val, lobbyModCount - 1);
			foreach (MyObjectBuilder_Checkpoint.ModItem item in lobbyMods)
			{
				if (val-- <= 0)
				{
					stringBuilder.Append("...");
					break;
				}
				if (num3-- <= 0)
				{
					stringBuilder.Append(item.FriendlyName);
				}
				else
				{
					stringBuilder.AppendLine(item.FriendlyName);
				}
			}
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(lobby);
			string toolTip = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Rank).ToString();
			row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, toolTip, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(lobbyWorldName), lobby.LobbyId, m_textCache.ToString()));
			row.AddCell(new MyGuiControlTable.Cell(m_gameTypeText, null, (m_gameTypeToolTip.Length > 0) ? m_gameTypeToolTip.ToString() : null));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(lobbyHostName), null, m_textCache.ToString()));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(value)));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append("---")));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append((lobbyModCount == 0) ? "---" : lobbyModCount.ToString()), null, stringBuilder.ToString()));
			m_gamesTable.Add(row);
			m_friendsPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Friends)).Append(" (")
				.Append(m_gamesTable.RowsCount)
				.Append(")");
		}
	}
}
