#define VRAGE
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenChat : MyGuiScreenBase
	{
		private enum MyNameFillState
		{
			Disabled,
			Inactive,
			Active
		}

		private readonly MyGuiControlTextbox m_chatTextbox;

		private readonly MyGuiControlLabel m_channelInfo;

		private readonly MyGuiControlLabel m_chatUnavailable;

		public static MyGuiScreenChat Static;

		private const int MESSAGE_HISTORY_SIZE = 20;

		private static StringBuilder[] m_messageHistory;

		private static int m_messageHistoryPushTo;

		private static int m_messageHistoryShown;

		private MyNameFillState m_currentNameFillState = MyNameFillState.Inactive;

		private string[] NAMEFILL_BASES = new string[2]
		{
			"/w \"",
			"/w "
		};

		private string m_namefillPrefix_completeBefore = string.Empty;

		private string m_namefillPrefix_completeNew = string.Empty;

		private string m_namefillPrefix_name = string.Empty;

		private string m_namefillPrefix_command = string.Empty;

		private int m_currentNamefillIndex = int.MaxValue;

		private List<MyPlayer> m_currentPlayerList;

		public MyGuiControlTextbox ChatTextbox => m_chatTextbox;

		static MyGuiScreenChat()
		{
			Static = null;
			m_messageHistory = new StringBuilder[20];
			m_messageHistoryPushTo = 0;
			m_messageHistoryShown = 0;
			for (int i = 0; i < 20; i++)
			{
				m_messageHistory[i] = new StringBuilder();
			}
		}

		public MyGuiScreenChat(Vector2 position)
			: base(position, MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenChat.ctor START");
			base.EnabledBackgroundFade = false;
			m_isTopMostScreen = true;
			base.CanHideOthers = false;
			base.CanBeHidden = false;
			base.DrawMouseCursor = false;
			m_closeOnEsc = true;
			m_chatTextbox = new MyGuiControlTextbox(Vector2.Zero, null, MyGameService.GetChatMaxMessageSize());
			m_chatTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_chatTextbox.Size = new Vector2(0.27f, 0.05f);
			m_chatTextbox.TextScale = 0.8f;
			m_chatTextbox.VisualStyle = MyGuiControlTextboxStyleEnum.Default;
			m_chatTextbox.EnterPressed += OnInputFieldActivated;
			ChatChannel currentChannel = MySession.Static.ChatSystem.CurrentChannel;
			string empty = string.Empty;
			Color channelColor = MyChatSystem.GetChannelColor(currentChannel);
			switch (currentChannel)
			{
			case ChatChannel.Global:
				empty = MyTexts.GetString(MyCommonTexts.Chat_NameModifier_Global);
				break;
			case ChatChannel.Faction:
			{
				string arg2 = "faction";
				IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(MySession.Static.ChatSystem.CurrentTarget);
				if (myFaction != null)
				{
					arg2 = myFaction.Tag;
				}
				empty = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_ToBracketed), arg2);
				break;
			}
			case ChatChannel.Private:
			{
				string arg = "player";
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(MySession.Static.ChatSystem.CurrentTarget);
				if (myIdentity != null)
				{
					arg = ((myIdentity.DisplayName.Length > 9) ? myIdentity.DisplayName.Substring(0, 9) : myIdentity.DisplayName);
				}
				empty = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_ToBracketed), arg);
				break;
			}
			default:
				empty = MyTexts.GetString(MyCommonTexts.Chat_NameModifier_ReportThis);
				break;
			}
			m_channelInfo = new MyGuiControlLabel(new Vector2(-0.016f, -0.042f), null, empty);
			m_channelInfo.ColorMask = channelColor;
			m_chatTextbox.Size = new Vector2(0.3215f - m_channelInfo.Size.X, 0.032f);
			m_chatTextbox.Position = new Vector2(-0.01f, -0.06f) + new Vector2(m_channelInfo.Size.X, 0f);
			MyMultiplayerBase @static = MyMultiplayer.Static;
			if (@static != null && !@static.IsChatAvailable)
			{
				m_chatUnavailable = new MyGuiControlLabel(new Vector2(-0.016f, -0.042f - m_chatTextbox.Size.Y));
				m_chatUnavailable.ColorMask = Color.Red;
				m_chatUnavailable.TextEnum = MyCommonTexts.ChatRestricted;
				Controls.Add(m_chatUnavailable);
			}
			Controls.Add(m_chatTextbox);
			Controls.Add(m_channelInfo);
			MySandboxGame.Log.WriteLine("MyGuiScreenChat.ctor END");
			MyHud.Chat.ChatOpened();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Down))
			{
				HardResetFill();
				HistoryUp();
			}
			else if (MyInput.Static.IsNewKeyPressed(MyKeys.Up))
			{
				HardResetFill();
				HistoryDown();
			}
			else if (MyInput.Static.IsKeyPress(MyKeys.PageUp))
			{
				HardResetFill();
				MyHud.Chat.ChatControl.ScrollUp();
			}
			else if (MyInput.Static.IsKeyPress(MyKeys.PageDown))
			{
				HardResetFill();
				MyHud.Chat.ChatControl.ScrollDown();
			}
			else if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) && m_currentNameFillState == MyNameFillState.Active)
			{
				SoftResetFill();
			}
			else if (MyInput.Static.IsNewKeyPressed(MyKeys.Tab))
			{
				switch (m_currentNameFillState)
				{
				case MyNameFillState.Disabled:
					break;
				case MyNameFillState.Inactive:
					if (InitiateNamefill())
					{
						CycleNamefill();
					}
					break;
				case MyNameFillState.Active:
					if (m_chatTextbox.Text.Equals(m_namefillPrefix_completeNew))
					{
						CycleNamefill();
						break;
					}
					SoftResetFill();
					if (InitiateNamefill())
					{
						CycleNamefill();
					}
					break;
				}
			}
			else
			{
				base.HandleInput(receivedFocusInThisUpdate);
			}
		}

		public void OnInputFieldActivated(MyGuiControlTextbox textBox)
		{
			string text = textBox.Text;
			PushHistory(text);
			if (MySession.Static.ChatSystem.CommandSystem.CanHandle(text))
			{
				MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer.DisplayName, text);
				MySession.Static.ChatSystem.CommandSystem.Handle(text);
			}
			else if (!string.IsNullOrWhiteSpace(text))
			{
				if (MySession.Static.ChatBot != null && MySession.Static.ChatBot.FilterMessage(text, OnChatBotResponse))
				{
					MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(text, ChatChannel.ChatBot, MySession.Static.LocalPlayerId, -1L, DateTime.UtcNow);
					MyHud.Chat.ShowMessage((MySession.Static.LocalHumanPlayer == null) ? "Player" : MySession.Static.LocalHumanPlayer.DisplayName, text);
					MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.GoodBot);
				}
				else
				{
					bool sendToOthers = true;
					MyAPIUtilities.Static.EnterMessage(text, ref sendToOthers);
					if (sendToOthers)
					{
						SendChatMessage(text);
					}
				}
			}
			textBox.FocusEnded();
			CloseScreenNow();
		}

		public static void SendChatMessage(string message)
		{
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.SendChatMessage(message, MySession.Static.ChatSystem.CurrentChannel, MySession.Static.ChatSystem.CurrentTarget);
			}
			else if (MyGameService.IsActive)
			{
				MyHud.Chat.ShowMessageColoredSP(message, MySession.Static.ChatSystem.CurrentChannel, MySession.Static.ChatSystem.CurrentTarget);
			}
			else
			{
				MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer.DisplayName, message);
			}
		}

		private bool InitiateNamefill()
		{
			string text = m_chatTextbox.Text;
			string command = string.Empty;
			string nameStump = string.Empty;
			if (!TestNamefillPrefixse(text, out command, out nameStump))
			{
				return false;
			}
			m_currentNameFillState = MyNameFillState.Active;
			m_namefillPrefix_completeBefore = (m_namefillPrefix_completeNew = text);
			m_namefillPrefix_command = command;
			m_namefillPrefix_name = nameStump;
			m_currentPlayerList = MySession.Static.Players.GetPlayersStartingNameWith(nameStump);
			m_currentNamefillIndex = m_currentPlayerList.Count - 1;
			return true;
		}

		private void CycleNamefill()
		{
			if (m_currentPlayerList.Count != 0)
			{
				m_currentNamefillIndex++;
				if (m_currentNamefillIndex >= m_currentPlayerList.Count)
				{
					m_currentNamefillIndex = 0;
				}
				m_namefillPrefix_completeNew = m_namefillPrefix_command + m_currentPlayerList[m_currentNamefillIndex].DisplayName;
				m_chatTextbox.Text = m_namefillPrefix_completeNew;
			}
		}

		private bool TestNamefillPrefixse(string complete, out string command, out string nameStump)
		{
			command = string.Empty;
			nameStump = string.Empty;
			string[] nAMEFILL_BASES = NAMEFILL_BASES;
			foreach (string text in nAMEFILL_BASES)
			{
				if (complete.Length >= text.Length && text.Equals(complete.Substring(0, text.Length)))
				{
					command = text;
					nameStump = complete.Substring(text.Length, complete.Length - text.Length);
					return true;
				}
			}
			return false;
		}

		private void HardResetFill()
		{
			if (m_currentNameFillState == MyNameFillState.Active)
			{
				m_chatTextbox.Text = m_namefillPrefix_completeBefore;
			}
			SoftResetFill();
		}

		private void SoftResetFill()
		{
			if (m_currentNameFillState == MyNameFillState.Active)
			{
				m_currentNameFillState = MyNameFillState.Inactive;
				m_namefillPrefix_completeBefore = (m_namefillPrefix_completeNew = (m_namefillPrefix_command = (m_namefillPrefix_name = string.Empty)));
				m_currentNamefillIndex = int.MaxValue;
				m_currentPlayerList = null;
			}
		}

		private void OnChatBotResponse(string text)
		{
			if (MySession.Static != null)
			{
				MyUnifiedChatItem item = MyUnifiedChatItem.CreateChatbotMessage(text, DateTime.UtcNow, 0L, MySession.Static.LocalPlayerId, MyTexts.GetString(MySpaceTexts.ChatBotName));
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(ref item);
				MyHud.Chat.ShowMessage(MyTexts.GetString(MySpaceTexts.ChatBotName), text);
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			Vector2 hudPos = m_position;
			hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			return true;
		}

		public override bool Draw()
		{
			return base.Draw();
		}

		private static void PushHistory(string message)
		{
			m_messageHistory[m_messageHistoryPushTo].Clear().Append(message);
			m_messageHistoryPushTo = HistoryIndexUp(m_messageHistoryPushTo);
			m_messageHistoryShown = m_messageHistoryPushTo;
			m_messageHistory[m_messageHistoryPushTo].Clear();
		}

		private void HistoryDown()
		{
			int num = HistoryIndexDown(m_messageHistoryShown);
			if (num != m_messageHistoryPushTo)
			{
				m_messageHistoryShown = num;
				m_chatTextbox.Text = (m_messageHistory[m_messageHistoryShown].ToString() ?? "");
			}
		}

		private void HistoryUp()
		{
			if (m_messageHistoryShown != m_messageHistoryPushTo)
			{
				m_messageHistoryShown = HistoryIndexUp(m_messageHistoryShown);
				m_chatTextbox.Text = (m_messageHistory[m_messageHistoryShown].ToString() ?? "");
			}
		}

		private static int HistoryIndexUp(int index)
		{
			index++;
			if (index >= 20)
			{
				return 0;
			}
			return index;
		}

		private static int HistoryIndexDown(int index)
		{
			index--;
			if (index < 0)
			{
				return 19;
			}
			return index;
		}

		private void Process(string message)
		{
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenChat";
		}

		public override void LoadContent()
		{
			base.LoadContent();
			Static = this;
		}

		public override void UnloadContent()
		{
			if (m_chatTextbox != null)
			{
				m_chatTextbox.FocusEnded();
			}
			Static = null;
			base.UnloadContent();
		}

		public override bool HideScreen()
		{
			UnloadContent();
			return base.HideScreen();
		}

		protected override void OnClosed()
		{
			MyHud.Chat.ChatClosed();
			base.OnClosed();
		}
	}
}
