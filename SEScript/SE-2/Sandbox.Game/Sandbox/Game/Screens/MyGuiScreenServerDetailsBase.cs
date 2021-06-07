using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public abstract class MyGuiScreenServerDetailsBase : MyGuiScreenBase
	{
		protected enum DetailsPageEnum
		{
			Settings,
			Mods,
			Players
		}

		private class LoadPlayersResult : IMyAsyncResult
		{
			public Dictionary<string, float> Players
			{
				get;
				private set;
			}

			public bool IsCompleted
			{
				get;
				private set;
			}

			public Task Task
			{
				get;
				private set;
			}

			public LoadPlayersResult(MyCachedServerItem server)
			{
				MyGameService.GetPlayerDetails(server.Server.NetAdr.Address.ToIPv4NetworkOrder(), (ushort)server.Server.NetAdr.Port, LoadCompleted, delegate
				{
					LoadCompleted(null);
				});
			}

			private void LoadCompleted(Dictionary<string, float> players)
			{
				Players = players;
				IsCompleted = true;
			}
		}

		private class LoadModsResult : IMyAsyncResult
		{
			public bool IsCompleted => Task.IsComplete;

			public Task Task
			{
				get;
				private set;
			}

			public List<MyWorkshopItem> ServerMods
			{
				get;
				private set;
			}

			public LoadModsResult(MyCachedServerItem server)
			{
				ServerMods = new List<MyWorkshopItem>();
				Task = Parallel.Start(delegate
				{
					if (MyGameService.IsOnline && server.Mods != null && server.Mods.Count > 0)
					{
						MyWorkshop.GetItemsBlockingUGC(server.Mods, ServerMods);
					}
				});
			}
		}

		protected DetailsPageEnum CurrentPage;

		protected Vector2 CurrentPosition;

		protected List<MyWorkshopItem> Mods;

		protected float Padding = 0.02f;

		protected Dictionary<string, float> Players;

		protected MyCachedServerItem Server;

		private MyGuiControlButton BT_Settings;

		private MyGuiControlButton BT_Mods;

		private MyGuiControlButton BT_Players;

		private MyGuiControlRotatingWheel m_loadingWheel;

		private bool serverIsFavorited;

		protected MyObjectBuilder_SessionSettings Settings => Server.Settings;

		protected MyGuiScreenServerDetailsBase(MyCachedServerItem server)
			: base(new Vector2(0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.9398855f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			Server = server;
			CreateScreen();
			TestFavoritesGameList();
		}

		private void CreateScreen()
		{
			base.CanHideOthers = true;
			base.CanBeHidden = true;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.JoinGame_ServerDetails, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.15f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList3);
			MyGuiControlSeparatorList myGuiControlSeparatorList4 = new MyGuiControlSeparatorList();
			float num = 0.303f;
			if (Server.ExperimentalMode)
			{
				num = 0.34f;
			}
			myGuiControlSeparatorList4.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - num), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList4);
			CurrentPosition = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f - 0.003f, m_size.Value.Y / 2f - 0.116f);
			DrawButtons();
			m_loadingWheel = new MyGuiControlRotatingWheel(BT_Players.Position + new Vector2(0.137f, -0.004f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
			Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			if (!serverIsFavorited)
			{
				MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(-0.003f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerDetails_AddFavorite));
				myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_AddFavorite));
				myGuiControlButton.ButtonClicked += FavoriteButtonClick;
				Controls.Add(myGuiControlButton);
			}
			else
			{
				MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(-0.003f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerDetails_RemoveFavorite));
				myGuiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_RemoveFavorite));
				myGuiControlButton2.ButtonClicked += UnFavoriteButtonClick;
				Controls.Add(myGuiControlButton2);
			}
			MyGuiControlButton myGuiControlButton3 = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(0.18f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.JoinGame_Title));
			myGuiControlButton3.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGame_JoinWorld));
			myGuiControlButton3.ButtonClicked += ConnectButtonClick;
			Controls.Add(myGuiControlButton3);
			CurrentPosition.Y += 0.012f;
			AddLabel(MyCommonTexts.ServerDetails_Server, Server.Server.Name);
			AddLabel(MyCommonTexts.ServerDetails_Map, Server.Server.Map);
			AddLabel(MyCommonTexts.ServerDetails_Version, new MyVersion((int)Server.Server.GetGameTagByPrefixUlong("version")).FormattedText.ToString().Replace("_", "."));
			AddLabel(MyCommonTexts.ServerDetails_IPAddress, Server.Server.NetAdr);
			if (Server.ExperimentalMode)
			{
				AddLabel(MyCommonTexts.ServerIsExperimental);
			}
			CurrentPosition.Y += 0.028f;
			switch (CurrentPage)
			{
			case DetailsPageEnum.Settings:
				base.FocusedControl = BT_Settings;
				BT_Settings.HighlightType = MyGuiControlHighlightType.FORCED;
				BT_Settings.HasHighlight = true;
				BT_Settings.Selected = true;
				DrawSettings();
				break;
			case DetailsPageEnum.Mods:
				base.FocusedControl = BT_Mods;
				BT_Mods.HighlightType = MyGuiControlHighlightType.FORCED;
				BT_Mods.HasHighlight = true;
				BT_Mods.Selected = true;
				DrawMods();
				break;
			case DetailsPageEnum.Players:
				base.FocusedControl = BT_Players;
				BT_Players.HighlightType = MyGuiControlHighlightType.FORCED;
				BT_Players.HasHighlight = true;
				BT_Players.Selected = true;
				DrawPlayers();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		protected abstract void DrawSettings();

		private void DrawMods()
		{
			if (Mods != null && Mods.Count > 0)
			{
				double byteSize = Mods.Sum((MyWorkshopItem m) => (long)m.Size);
				string str = MyUtils.FormatByteSizePrefix(ref byteSize);
				AddLabel(MyCommonTexts.ServerDetails_ModDownloadSize, byteSize.ToString("0.") + " " + str + "B");
			}
			AddLabel(MyCommonTexts.WorldSettings_Mods, null);
			if (Mods == null)
			{
				Controls.Add(new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_ModError), null, 0.8f, "Red"));
				return;
			}
			if (Mods.Count == 0)
			{
				Controls.Add(new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_NoMods)));
				return;
			}
			Mods.Sort((MyWorkshopItem a, MyWorkshopItem b) => string.Compare(a.Title, b.Title, StringComparison.CurrentCultureIgnoreCase));
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlParent);
			myGuiControlScrollablePanel.ScrollbarVEnabled = true;
			myGuiControlScrollablePanel.Position = CurrentPosition;
			myGuiControlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlScrollablePanel.Size = new Vector2(base.Size.Value.X - 0.112f, base.Size.Value.Y / 2f - CurrentPosition.Y - 0.145f);
			myGuiControlScrollablePanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
			myGuiControlScrollablePanel.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
			Controls.Add(myGuiControlScrollablePanel);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Close);
			myGuiControlParent.Size = new Vector2(myGuiControlScrollablePanel.Size.X, (float)Mods.Count * (myGuiControlButton.Size.Y / 2f + Padding) + myGuiControlButton.Size.Y / 2f);
			Vector2 value = new Vector2((0f - myGuiControlScrollablePanel.Size.X) / 2f, (0f - myGuiControlParent.Size.Y) / 2f);
			foreach (MyWorkshopItem mod in Mods)
			{
				MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.ClickableText, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, new StringBuilder(mod.Title));
				myGuiControlButton2.UserData = mod.Id;
				int num = Math.Min(mod.Description.Length, 128);
				int num2 = mod.Description.IndexOf("\n");
				if (num2 > 0)
				{
					num = Math.Min(num, num2 - 1);
				}
				myGuiControlButton2.SetToolTip(mod.Description.Substring(0, num));
				myGuiControlButton2.ButtonClicked += ModURLClick;
				value.Y += myGuiControlButton2.Size.Y / 2f + Padding;
				myGuiControlParent.Controls.Add(myGuiControlButton2);
			}
		}

		private void ModURLClick(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrl(MyGameService.WorkshopService.GetItemUrl((ulong)button.UserData), UrlOpenMode.SteamOrExternalWithConfirm);
		}

		private void DrawPlayers()
		{
			MyGuiControlLabel myGuiControlLabel = AddLabel(MyCommonTexts.ScreenCaptionPlayers, null);
			if (Players == null)
			{
				Controls.Add(new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_PlayerError), null, 0.8f, "Red"));
				return;
			}
			if (Players.Count == 0)
			{
				Controls.Add(new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_ServerEmpty)));
				return;
			}
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlParent);
			myGuiControlScrollablePanel.ScrollbarVEnabled = true;
			myGuiControlScrollablePanel.Position = CurrentPosition;
			myGuiControlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlScrollablePanel.Size = new Vector2(base.Size.Value.X - 0.112f, base.Size.Value.Y / 2f - CurrentPosition.Y - 0.145f);
			myGuiControlScrollablePanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
			myGuiControlScrollablePanel.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
			Controls.Add(myGuiControlScrollablePanel);
			myGuiControlParent.Size = new Vector2(myGuiControlScrollablePanel.Size.X, (float)Players.Count * (myGuiControlLabel.Size.Y / 2f + Padding) + myGuiControlLabel.Size.Y / 2f);
			Vector2 value = new Vector2((0f - myGuiControlScrollablePanel.Size.X) / 2f, (0f - myGuiControlParent.Size.Y) / 2f + myGuiControlLabel.Size.Y / 2f);
			foreach (KeyValuePair<string, float> player in Players)
			{
				StringBuilder stringBuilder = new StringBuilder(player.Key);
				stringBuilder.Append(": ");
				MyValueFormatter.AppendTimeInBestUnit((int)player.Value, stringBuilder);
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(value, null, stringBuilder.ToString());
				value.Y += myGuiControlLabel2.Size.Y / 2f + Padding;
				myGuiControlParent.Controls.Add(myGuiControlLabel2);
			}
		}

		protected void DrawButtons()
		{
			float x = CurrentPosition.X;
			BT_Settings = new MyGuiControlButton(CurrentPosition, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerDetails_Settings));
			BT_Settings.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Settings));
			BT_Settings.PositionX += BT_Settings.Size.X / 2f;
			CurrentPosition.X += BT_Settings.Size.X + Padding / 4f;
			BT_Settings.ButtonClicked += SettingButtonClick;
			Controls.Add(BT_Settings);
			BT_Mods = new MyGuiControlButton(CurrentPosition, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.WorldSettings_Mods));
			BT_Mods.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Mods));
			BT_Mods.PositionX += BT_Mods.Size.X / 2f;
			CurrentPosition.X += BT_Mods.Size.X + Padding / 4f;
			BT_Mods.ButtonClicked += ModsButtonClick;
			Controls.Add(BT_Mods);
			BT_Players = new MyGuiControlButton(CurrentPosition, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers));
			BT_Players.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Players));
			BT_Players.PositionX += BT_Players.Size.X / 2f;
			CurrentPosition.X += BT_Players.Size.X + Padding / 4f;
			BT_Players.ButtonClicked += PlayersButtonClick;
			Controls.Add(BT_Players);
			CurrentPosition.X = x;
			CurrentPosition.Y += BT_Settings.Size.Y + Padding / 2f;
		}

		public override string GetFriendlyName()
		{
			return "ServerDetails";
		}

		protected SortedList<string, object> LoadSessionSettings(VRage.Game.Game game)
		{
			if (Settings == null)
			{
				return null;
			}
			SortedList<string, object> result = new SortedList<string, object>();
			FieldInfo[] fields = typeof(MyObjectBuilder_SessionSettings).GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				GameRelationAttribute customAttribute = fieldInfo.GetCustomAttribute<GameRelationAttribute>();
				if (customAttribute == null || (customAttribute.RelatedTo != 0 && customAttribute.RelatedTo != game))
				{
					continue;
				}
				DisplayAttribute customAttribute2 = fieldInfo.GetCustomAttribute<DisplayAttribute>();
				if (customAttribute2 != null && !string.IsNullOrEmpty(customAttribute2.Name))
				{
					string text = "ServerDetails_" + fieldInfo.Name;
					if (!(MyTexts.GetString(text) == text))
					{
						result.Add(text, fieldInfo.GetValue(Settings));
					}
				}
			}
			AddAdditionalSettings(ref result);
			return result;
		}

		private void AddAdditionalSettings(ref SortedList<string, object> result)
		{
			if (Settings != null)
			{
				result.Add(MyTexts.GetString(MyCommonTexts.ServerDetails_PCU_Initial), MyObjectBuilder_SessionSettings.GetInitialPCU(Settings));
			}
		}

		private void ConnectButtonClick(MyGuiControlButton obj)
		{
			ParseIPAndConnect();
		}

		private void ParseIPAndConnect()
		{
			try
			{
				string hostNameOrAddress = Server.Server.NetAdr.Address.MapToIPv4().ToString();
				ushort port = ushort.Parse(Server.Server.NetAdr.Port.ToString());
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostNameOrAddress);
				MyGameService.OnPingServerResponded += MySandboxGame.Static.ServerResponded;
				MyGameService.OnPingServerFailedToRespond += MySandboxGame.Static.ServerFailedToRespond;
				MyGameService.PingServer(hostAddresses[0].ToIPv4NetworkOrder(), port);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
			}
		}

		private void CloseButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void TestFavoritesGameList()
		{
			MyGameService.OnFavoritesServerListResponded += OnFavoritesServerListResponded;
			MyGameService.OnFavoritesServersCompleteResponse += OnFavoritesServersCompleteResponse;
			MyGameService.RequestFavoritesServerList($"gamedir:{MyPerGameSettings.SteamGameServerGameDir};secure:1");
			m_loadingWheel.Visible = true;
		}

		private void OnFavoritesServerListResponded(object sender, int server)
		{
			MyCachedServerItem myCachedServerItem = new MyCachedServerItem(MyGameService.GetFavoritesServerDetails(server));
			MyCachedServerItem server2 = Server;
			if (myCachedServerItem.Server.NetAdr.Address.ToString() == server2.Server.NetAdr.Address.ToString() && myCachedServerItem.Server.NetAdr.Port.ToString() == server2.Server.NetAdr.Port.ToString())
			{
				serverIsFavorited = true;
				RecreateControls(constructor: false);
				m_loadingWheel.Visible = false;
			}
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

		private void FavoriteButtonClick(MyGuiControlButton myGuiControlButton)
		{
			MyGameServerItem server = Server.Server;
			MyGameService.AddFavoriteGame(server.NetAdr.Address.ToIPv4NetworkOrder(), (ushort)server.NetAdr.Port, (ushort)server.NetAdr.Port);
			serverIsFavorited = true;
			RecreateControls(constructor: false);
		}

		private void UnFavoriteButtonClick(MyGuiControlButton myGuiControlButton)
		{
			MyGameServerItem server = Server.Server;
			MyGameService.RemoveFavoriteGame(server.NetAdr.Address.ToIPv4NetworkOrder(), (ushort)server.NetAdr.Port, (ushort)server.NetAdr.Port);
			MyGuiScreenJoinGame firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenJoinGame>();
			if (firstScreenOfType.m_selectedPage.Name == "PageFavoritesPanel")
			{
				firstScreenOfType.RemoveFavoriteServer(Server);
			}
			serverIsFavorited = false;
			RecreateControls(constructor: false);
		}

		private void JoinButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void SettingButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = DetailsPageEnum.Settings;
			RecreateControls(constructor: false);
		}

		private void ModsButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = DetailsPageEnum.Mods;
			if (Server.Mods != null && Server.Mods.Count > 0)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, BeginModResultAction, EndModResultAction));
			}
			else if (Server.Mods != null && Server.Mods.Count == 0)
			{
				Mods = new List<MyWorkshopItem>();
				RecreateControls(constructor: false);
			}
			else
			{
				RecreateControls(constructor: false);
			}
		}

		private void PlayersButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = DetailsPageEnum.Players;
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, BeginPlayerResultAction, EndPlayerResultAction));
		}

		private IMyAsyncResult BeginPlayerResultAction()
		{
			return new LoadPlayersResult(Server);
		}

		private void EndPlayerResultAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			LoadPlayersResult loadPlayersResult = (LoadPlayersResult)result;
			Players = loadPlayersResult.Players;
			screen.CloseScreen();
			m_loadingWheel.Visible = false;
			RecreateControls(constructor: false);
		}

		private IMyAsyncResult BeginModResultAction()
		{
			return new LoadModsResult(Server);
		}

		private void EndModResultAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			LoadModsResult loadModsResult = (LoadModsResult)result;
			Mods = loadModsResult.ServerMods;
			screen.CloseScreen();
			m_loadingWheel.Visible = false;
			RecreateControls(constructor: false);
		}

		protected void AddSeparator(MyGuiControlParent parent, Vector2 localPos, float size = 1f)
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.Size = new Vector2(1f, 0.01f);
			myGuiControlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			myGuiControlSeparatorList.AddHorizontal(Vector2.Zero, size);
			myGuiControlSeparatorList.Position = new Vector2(localPos.X, localPos.Y - 0.02f);
			myGuiControlSeparatorList.Alpha = 0.4f;
			parent.Controls.Add(myGuiControlSeparatorList);
		}

		protected MyGuiControlLabel AddLabel(MyStringId description, object value)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(CurrentPosition, null, $"{MyTexts.GetString(description)}: {value}");
			CurrentPosition.Y += myGuiControlLabel.Size.Y / 2f + Padding;
			Controls.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		protected MyGuiControlLabel AddLabel(MyStringId description)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(description));
			CurrentPosition.Y += myGuiControlLabel.Size.Y / 2f + Padding;
			Controls.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		protected MyGuiControlMultilineText AddMultilineText(string text, float size)
		{
			return AddMultilineText(new StringBuilder(text), size);
		}

		protected MyGuiControlMultilineText AddMultilineText(StringBuilder text, float size)
		{
			CurrentPosition.Y -= Padding / 2f;
			CurrentPosition.X += 0.003f;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(CurrentPosition, new Vector2(base.Size.Value.X - 0.112f, size), null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			CurrentPosition.X -= 0.003f;
			myGuiControlMultilineText.Text = text;
			myGuiControlMultilineText.Position += myGuiControlMultilineText.Size / 2f;
			MyGuiControlCompositePanel control = new MyGuiControlCompositePanel
			{
				Position = CurrentPosition,
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER,
				Size = myGuiControlMultilineText.Size,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			myGuiControlMultilineText.Size = new Vector2(myGuiControlMultilineText.Size.X / 1.01f, myGuiControlMultilineText.Size.Y / 1.09f);
			CurrentPosition.Y += myGuiControlMultilineText.Size.Y + Padding * 1.5f;
			Controls.Add(control);
			Controls.Add(myGuiControlMultilineText);
			return myGuiControlMultilineText;
		}

		protected MyGuiControlCheckbox AddCheckbox(MyStringId text, Action<MyGuiControlCheckbox> onClick, MyStringId? tooltip = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(CurrentPosition, null, tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : string.Empty);
			myGuiControlCheckbox.PositionX += myGuiControlCheckbox.Size.X / 2f;
			Controls.Add(myGuiControlCheckbox);
			if (onClick != null)
			{
				myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onClick);
			}
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(text));
			myGuiControlLabel.PositionX += myGuiControlCheckbox.Size.X + Padding;
			Controls.Add(myGuiControlLabel);
			CurrentPosition.Y += myGuiControlCheckbox.Size.Y;
			return myGuiControlCheckbox;
		}
	}
}
