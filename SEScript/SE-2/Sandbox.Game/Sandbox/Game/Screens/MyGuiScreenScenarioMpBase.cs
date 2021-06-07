using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public abstract class MyGuiScreenScenarioMpBase : MyGuiScreenBase
	{
		public static MyGuiScreenScenarioMpBase Static;

		private MyGuiControlMultilineText m_descriptionBox;

		protected MyGuiControlButton m_kickPlayerButton;

		protected MyGuiControlTable m_connectedPlayers;

		private MyGuiControlLabel m_timeoutLabel;

		private MyGuiControlLabel m_canJoinRunningLabel;

		protected MyGuiControlCheckbox m_canJoinRunning;

		protected MyHudControlChat m_chatControl;

		protected MyGuiControlTextbox m_chatTextbox;

		protected MyGuiControlButton m_sendChatButton;

		protected MyGuiControlButton m_startButton;

		private bool m_update;

		private StringBuilder m_editBoxStringBuilder = new StringBuilder();

		protected static HashSet<ulong> m_readyPlayers = new HashSet<ulong>();

		public MyGuiControlCombobox TimeoutCombo
		{
			get;
			protected set;
		}

		public string Briefing
		{
			set
			{
				m_descriptionBox.Text = new StringBuilder(value);
			}
		}

		public MyGuiScreenScenarioMpBase()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(1620f, 1125f) / MyGuiConstants.GUI_OPTIMAL_SIZE)
		{
			RecreateControls(constructor: true);
			base.CanHideOthers = false;
			MySyncScenario.PlayerReadyToStartScenario += MySyncScenario_PlayerReady;
			MySyncScenario.TimeoutReceived += MySyncScenario_SetTimeout;
			MySyncScenario.CanJoinRunningReceived += MySyncScenario_SetCanJoinRunning;
			MyGuiControlCheckbox canJoinRunning = m_canJoinRunning;
			canJoinRunning.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(canJoinRunning.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnJoinRunningChecked));
			Static = this;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			MyLayoutTable myLayoutTable = new MyLayoutTable(this);
			myLayoutTable.SetColumnWidthsNormalized(50f, 300f, 300f, 300f, 300f, 300f, 50f);
			myLayoutTable.SetRowHeightsNormalized(50f, 450f, 70f, 70f, 70f, 400f, 70f, 70f, 50f);
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlParent)
			{
				Name = "BriefingScrollableArea",
				ScrollbarVEnabled = true,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ScrolledAreaPadding = new MyGuiBorderThickness(0.005f)
			};
			myLayoutTable.AddWithSize(myGuiControlScrollablePanel, MyAlignH.Left, MyAlignV.Top, 1, 1, 4, 3);
			m_descriptionBox = new MyGuiControlMultilineText(new Vector2(-0.227f, 5f), new Vector2(myGuiControlScrollablePanel.Size.X - 0.02f, 11f), null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlParent.Controls.Add(m_descriptionBox);
			m_connectedPlayers = new MyGuiControlTable();
			m_connectedPlayers.Size = new Vector2(490f, 150f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_connectedPlayers.VisibleRowsCount = 8;
			m_connectedPlayers.ColumnsCount = 2;
			m_connectedPlayers.SetCustomColumnWidths(new float[2]
			{
				0.7f,
				0.3f
			});
			m_connectedPlayers.SetColumnName(0, MyTexts.Get(MySpaceTexts.GuiScenarioPlayerName));
			m_connectedPlayers.SetColumnName(1, MyTexts.Get(MySpaceTexts.GuiScenarioPlayerStatus));
			m_kickPlayerButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: MyTexts.Get(MyCommonTexts.Kick), size: new Vector2(190f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnKick2Clicked);
			m_kickPlayerButton.Enabled = CanKick();
			m_timeoutLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.GuiScenarioTimeout));
			TimeoutCombo = new MyGuiControlCombobox();
			TimeoutCombo.ItemSelected += OnTimeoutSelected;
			TimeoutCombo.AddItem(3L, MyTexts.Get(MySpaceTexts.GuiScenarioTimeout3min));
			TimeoutCombo.AddItem(5L, MyTexts.Get(MySpaceTexts.GuiScenarioTimeout5min));
			TimeoutCombo.AddItem(10L, MyTexts.Get(MySpaceTexts.GuiScenarioTimeout10min));
			TimeoutCombo.AddItem(-1L, MyTexts.Get(MySpaceTexts.GuiScenarioTimeoutUnlimited));
			TimeoutCombo.SelectItemByIndex(0);
			TimeoutCombo.Enabled = Sync.IsServer;
			m_canJoinRunningLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.ScenarioSettings_CanJoinRunningShort));
			m_canJoinRunning = new MyGuiControlCheckbox();
			m_canJoinRunningLabel.Enabled = false;
			m_canJoinRunning.Enabled = false;
			m_startButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: MyTexts.Get(MySpaceTexts.GuiScenarioStart), size: new Vector2(200f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnStartClicked);
			m_startButton.Enabled = Sync.IsServer;
			m_chatControl = new MyHudControlChat(MyHud.Chat, null, new Vector2(1400f, 300f) / MyGuiConstants.GUI_OPTIMAL_SIZE, MyGuiConstants.THEMED_GUI_BACKGROUND_COLOR, "DarkBlue", 0.7f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
			m_chatControl.BorderEnabled = true;
			m_chatControl.BorderColor = Color.CornflowerBlue;
			m_chatTextbox = new MyGuiControlTextbox(null, null, MyGameService.GetChatMaxMessageSize());
			m_chatTextbox.Size = new Vector2(1400f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_chatTextbox.TextScale = 0.8f;
			m_chatTextbox.VisualStyle = MyGuiControlTextboxStyleEnum.Default;
			m_chatTextbox.EnterPressed += ChatTextbox_EnterPressed;
			m_sendChatButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: MyTexts.Get(MySpaceTexts.GuiScenarioSend), size: new Vector2(190f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnSendChatClicked);
			myLayoutTable.AddWithSize(m_connectedPlayers, MyAlignH.Left, MyAlignV.Top, 1, 4, 2, 2);
			myLayoutTable.AddWithSize(m_kickPlayerButton, MyAlignH.Left, MyAlignV.Center, 2, 5);
			myLayoutTable.AddWithSize(m_timeoutLabel, MyAlignH.Left, MyAlignV.Center, 3, 4);
			myLayoutTable.AddWithSize(TimeoutCombo, MyAlignH.Left, MyAlignV.Center, 3, 5);
			myLayoutTable.AddWithSize(m_canJoinRunningLabel, MyAlignH.Left, MyAlignV.Center, 4, 4);
			myLayoutTable.AddWithSize(m_canJoinRunning, MyAlignH.Right, MyAlignV.Center, 4, 5);
			myLayoutTable.AddWithSize(m_chatControl, MyAlignH.Left, MyAlignV.Top, 5, 1, 1, 5);
			myLayoutTable.AddWithSize(m_chatTextbox, MyAlignH.Left, MyAlignV.Top, 6, 1, 1, 4);
			myLayoutTable.AddWithSize(m_sendChatButton, MyAlignH.Right, MyAlignV.Top, 6, 5);
			myLayoutTable.AddWithSize(m_startButton, MyAlignH.Left, MyAlignV.Top, 7, 2);
		}

		private void ChatTextbox_EnterPressed(MyGuiControlTextbox textBox)
		{
			SendMessageFromChatTextBox();
		}

		private void SendMessageFromChatTextBox()
		{
			m_chatTextbox.GetText(m_editBoxStringBuilder.Clear());
			string message = m_editBoxStringBuilder.ToString();
			SendChatMessage(message);
			m_chatTextbox.SetText(m_editBoxStringBuilder.Clear());
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenMpScenario";
		}

		public override bool Update(bool hasFocus)
		{
			m_update = true;
			UpdateControls();
			m_update = false;
			return base.Update(hasFocus);
		}

		protected virtual void OnStartClicked(MyGuiControlButton sender)
		{
		}

		protected override void OnClosed()
		{
			MySyncScenario.PlayerReadyToStartScenario -= MySyncScenario_PlayerReady;
			MySyncScenario.TimeoutReceived -= MySyncScenario_SetTimeout;
			MySyncScenario.CanJoinRunningReceived -= MySyncScenario_SetCanJoinRunning;
			MyGuiControlCheckbox canJoinRunning = m_canJoinRunning;
			canJoinRunning.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(canJoinRunning.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnJoinRunningChecked));
			m_readyPlayers.Clear();
			base.OnClosed();
			if (base.Cancelled)
			{
				MySessionLoader.UnloadAndExitToMenu();
			}
		}

		public void MySyncScenario_PlayerReady(ulong Id)
		{
			m_readyPlayers.Add(Id);
		}

		private void OnSendChatClicked(MyGuiControlButton sender)
		{
			SendMessageFromChatTextBox();
		}

		private void SendChatMessage(string message)
		{
		}

		protected bool CanKick()
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			MyPlayer myPlayer = (m_connectedPlayers.SelectedRow != null) ? (m_connectedPlayers.SelectedRow.UserData as MyPlayer) : null;
			if (myPlayer == null || myPlayer.Identity.IdentityId == MySession.Static.LocalPlayerId)
			{
				return false;
			}
			return true;
		}

		private void OnKick2Clicked(MyGuiControlButton sender)
		{
			MyPlayer myPlayer = (m_connectedPlayers.SelectedRow != null) ? (m_connectedPlayers.SelectedRow.UserData as MyPlayer) : null;
			if (myPlayer != null && myPlayer.Identity.IdentityId != MySession.Static.LocalPlayerId)
			{
				MyMultiplayer.Static.KickClient(myPlayer.Id.SteamId);
			}
		}

		private void OnTimeoutSelected()
		{
			MyScenarioSystem.LoadTimeout = 60 * (int)TimeoutCombo.GetSelectedKey();
			if (Sync.IsServer)
			{
				MySyncScenario.SetTimeout(TimeoutCombo.GetSelectedIndex());
			}
		}

		public void MySyncScenario_SetTimeout(int index)
		{
			TimeoutCombo.SelectItemByIndex(index);
		}

		private void OnJoinRunningChecked(MyGuiControlCheckbox source)
		{
			MySession.Static.Settings.CanJoinRunning = source.IsChecked;
			MySyncScenario.SetJoinRunning(source.IsChecked);
		}

		public void MySyncScenario_SetCanJoinRunning(bool canJoin)
		{
			m_canJoinRunning.IsChecked = canJoin;
		}

		private void UpdateControls()
		{
			if (MyMultiplayer.Static != null && MySession.Static != null)
			{
				m_kickPlayerButton.Enabled = CanKick();
				UpdatePlayerList(m_connectedPlayers);
			}
		}

		private static void UpdatePlayerList(MyGuiControlTable table)
		{
			MyPlayer myPlayer = (table.SelectedRow != null) ? (table.SelectedRow.UserData as MyPlayer) : null;
			table.Clear();
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				string displayName = onlinePlayer.DisplayName;
				MyGuiControlTable.Row row = new MyGuiControlTable.Row(onlinePlayer);
				row.AddCell(new MyGuiControlTable.Cell(displayName));
				if (Sync.ServerId == onlinePlayer.Id.SteamId)
				{
					row.AddCell(new MyGuiControlTable.Cell("SERVER"));
				}
				else if (m_readyPlayers.Contains(onlinePlayer.Id.SteamId))
				{
					row.AddCell(new MyGuiControlTable.Cell("ready"));
				}
				else
				{
					row.AddCell(new MyGuiControlTable.Cell(""));
				}
				table.Add(row);
				if (onlinePlayer == myPlayer)
				{
					table.SelectedRow = row;
				}
			}
		}
	}
}
