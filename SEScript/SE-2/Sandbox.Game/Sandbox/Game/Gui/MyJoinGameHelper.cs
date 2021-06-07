#define VRAGE
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Network;
using VRage.Profiler;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public static class MyJoinGameHelper
	{
		private static bool JoinGameTest(IMyLobby lobby)
		{
			if (!lobby.IsValid)
			{
				return false;
			}
			if (!MyMultiplayerLobby.IsLobbyCorrectVersion(lobby))
			{
				string @string = MyTexts.GetString(MyCommonTexts.MultiplayerError_IncorrectVersion);
				string arg = MyBuildNumbers.ConvertBuildNumberFromIntToString(MyFinalBuildConstants.APP_VERSION);
				int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(lobby);
				if (lobbyAppVersion != 0)
				{
					string arg2 = MyBuildNumbers.ConvertBuildNumberFromIntToString(lobbyAppVersion);
					MyGuiSandbox.Show(new StringBuilder(string.Format(@string, arg, arg2)));
				}
				return false;
			}
			if (MyFakes.ENABLE_MP_DATA_HASHES && !MyMultiplayerLobby.HasSameData(lobby))
			{
				MyGuiSandbox.Show(MyCommonTexts.MultiplayerError_DifferentData);
				MySandboxGame.Log.WriteLine("Different game data when connecting to server. Local hash: " + MyDataIntegrityChecker.GetHashBase64() + ", server hash: " + MyMultiplayerLobby.GetDataHash(lobby));
				return false;
			}
			return true;
		}

		public static void JoinGame(IMyLobby lobby, bool requestData = true)
		{
			if (MySession.Static != null)
			{
				MySession.Static.Unload();
				MySession.Static = null;
			}
			if (requestData && string.IsNullOrEmpty(lobby.GetData("appVersion")))
			{
				MyLobbyHelper myLobbyHelper = new MyLobbyHelper(lobby);
				myLobbyHelper.OnSuccess += delegate(IMyLobby l, bool isSuccess)
				{
					if (!isSuccess)
					{
						JoinGame(lobby.LobbyId);
					}
					JoinGame(l, requestData: false);
				};
				if (myLobbyHelper.RequestData())
				{
					return;
				}
			}
			if (JoinGameTest(lobby))
			{
				JoinGame(lobby.LobbyId);
			}
		}

		public static void JoinGame(MyGameServerItem server, bool enableGuiBackgroundFade = true)
		{
			if (MySession.Static != null)
			{
				MySession.Static.Unload();
				MySession.Static = null;
			}
			MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Join);
			if (server.ServerVersion != (int)MyFinalBuildConstants.APP_VERSION)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.MultiplayerError_IncorrectVersion), MyFinalBuildConstants.APP_VERSION, server.ServerVersion);
				MyGuiSandbox.Show(stringBuilder, MyCommonTexts.MessageBoxCaptionError);
				return;
			}
			if (MyFakes.ENABLE_MP_DATA_HASHES)
			{
				string gameTagByPrefix = server.GetGameTagByPrefix("datahash");
				if (gameTagByPrefix != "" && gameTagByPrefix != MyDataIntegrityChecker.GetHashBase64())
				{
					MyGuiSandbox.Show(MyCommonTexts.MultiplayerError_DifferentData);
					MySandboxGame.Log.WriteLine("Different game data when connecting to server. Local hash: " + MyDataIntegrityChecker.GetHashBase64() + ", server hash: " + gameTagByPrefix);
					return;
				}
			}
			MyGameService.AddHistoryGame(server.NetAdr.Address.ToIPv4NetworkOrder(), (ushort)server.NetAdr.Port, (ushort)server.NetAdr.Port);
			MyMultiplayerClient multiplayer = new MyMultiplayerClient(server, new MySyncLayer(new MyTransportLayer(2)));
			multiplayer.ExperimentalMode = MySandboxGame.Config.ExperimentalMode;
			MyMultiplayer.Static = multiplayer;
			multiplayer.SendPlayerData(MyGameService.UserName);
			server.GetGameTagByPrefix("gamemode");
			StringBuilder text = MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld);
			MyGuiScreenProgress progress = new MyGuiScreenProgress(text, MyCommonTexts.Cancel, isTopMostScreen: false, enableGuiBackgroundFade);
			MyGuiSandbox.AddScreen(progress);
			progress.ProgressCancelled += delegate
			{
				multiplayer.Dispose();
				MySessionLoader.UnloadAndExitToMenu();
				if (MyMultiplayer.Static != null)
				{
					MyMultiplayer.Static.Dispose();
				}
			};
			MyMultiplayerClient myMultiplayerClient = multiplayer;
			myMultiplayerClient.OnJoin = (Action)Delegate.Combine(myMultiplayerClient.OnJoin, (Action)delegate
			{
				OnJoin(progress, success: true, null, MyLobbyStatusCode.Success, multiplayer);
			});
			Action<string> onProfilerCommandExecuted = delegate(string desc)
			{
				MyHudNotification notification = new MyHudNotification(MyStringId.GetOrCompute(desc));
				MyHud.Notifications.Add(notification);
				MyLog.Default.WriteLine(desc);
			};
			VRage.Profiler.MyRenderProfiler.GetProfilerFromServer = delegate
			{
				onProfilerCommandExecuted("Command executed: Download profiler");
				MyMultiplayer.Static.ProfilerDone = onProfilerCommandExecuted;
				MyMultiplayer.Static.DownloadProfiler();
			};
			MyRenderProfiler.ServerInvoke = delegate(RenderProfilerCommand cmd, int payload)
			{
				onProfilerCommandExecuted("Command executed: " + cmd);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner _) => MyRenderProfiler.OnCommandReceived, cmd, payload);
			};
		}

		public static void JoinGame(ulong lobbyId)
		{
			StringBuilder text = MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld);
			MyGuiScreenProgress progress = new MyGuiScreenProgress(text, MyCommonTexts.Cancel);
			MyGuiSandbox.AddScreen(progress);
			progress.ProgressCancelled += delegate
			{
				MySessionLoader.UnloadAndExitToMenu();
			};
			MyLog.Default.WriteLine("Joining lobby: " + lobbyId);
			MyMultiplayerJoinResult result = MyMultiplayer.JoinLobby(lobbyId);
			result.JoinDone += delegate(bool success, IMyLobby lobby, MyLobbyStatusCode response, MyMultiplayerBase multiplayer)
			{
				OnJoin(progress, success, lobby, response, multiplayer);
			};
			progress.ProgressCancelled += delegate
			{
				result.Cancel();
			};
		}

		public static void OnJoin(MyGuiScreenProgress progress, bool success, IMyLobby lobby, MyLobbyStatusCode response, MyMultiplayerBase multiplayer)
		{
			MyLog.Default.WriteLine($"Lobby join response: {success}, enter state: {response}");
			if (success && response == MyLobbyStatusCode.Success && multiplayer.GetOwner() != Sync.MyId)
			{
				DownloadWorld(progress, multiplayer);
			}
			else
			{
				OnJoinFailed(progress, multiplayer, response);
			}
		}

		private static void DownloadWorld(MyGuiScreenProgress progress, MyMultiplayerBase multiplayer)
		{
			if (progress.Text != null)
			{
				progress.Text.Clear();
				progress.Text.Append((object)MyTexts.Get(MyCommonTexts.MultiplayerStateConnectingToServer));
			}
			MyLog.Default.WriteLine("World requested");
			Stopwatch worldRequestTime = Stopwatch.StartNew();
			ulong serverId = multiplayer.GetOwner();
			bool connected = false;
			progress.Tick += delegate
			{
				MyP2PSessionState state = default(MyP2PSessionState);
				MyGameService.Peer2Peer.GetSessionState(multiplayer.ServerId, ref state);
				if (!connected && state.ConnectionActive)
				{
					MyLog.Default.WriteLine("World requested - connection alive");
					connected = true;
					if (progress.Text != null)
					{
						progress.Text.Clear();
						progress.Text.Append((object)MyTexts.Get(MyCommonTexts.MultiplayerStateWaitingForServer));
					}
				}
				if (serverId != multiplayer.GetOwner())
				{
					MyLog.Default.WriteLine("World requested - failed, server changed");
					progress.Cancel();
					MyGuiSandbox.Show(MyCommonTexts.MultiplayerErrorServerHasLeft);
					multiplayer.Dispose();
				}
				bool flag = MyScreenManager.IsScreenOnTop(progress);
				if (flag && !worldRequestTime.IsRunning)
				{
					worldRequestTime.Start();
				}
				else if (!flag && worldRequestTime.IsRunning)
				{
					worldRequestTime.Stop();
				}
				if (worldRequestTime.IsRunning && worldRequestTime.Elapsed.TotalSeconds > 40.0)
				{
					MyLog.Default.WriteLine("World requested - failed, server changed");
					progress.Cancel();
					MyGuiSandbox.Show(MyCommonTexts.MultiplaterJoin_ServerIsNotResponding);
					multiplayer.Dispose();
				}
			};
			multiplayer.DownloadWorld();
		}

		public static StringBuilder GetErrorMessage(MyLobbyStatusCode response)
		{
			MyStringId id;
			switch (response)
			{
			case MyLobbyStatusCode.Error:
				id = MyCommonTexts.LobbyError;
				break;
			case MyLobbyStatusCode.Full:
				id = MyCommonTexts.LobbyFull;
				break;
			case MyLobbyStatusCode.Banned:
				id = MyCommonTexts.LobbyBanned;
				break;
			case MyLobbyStatusCode.Cancelled:
				id = MyCommonTexts.LobbyCancelled;
				break;
			case MyLobbyStatusCode.ClanDisabled:
				id = MyCommonTexts.LobbyClanDisabled;
				break;
			case MyLobbyStatusCode.CommunityBan:
				id = MyCommonTexts.LobbyCommunityBan;
				break;
			case MyLobbyStatusCode.ConnectionProblems:
				id = MyCommonTexts.LobbyConnectionProblems;
				break;
			case MyLobbyStatusCode.DoesntExist:
				id = MyCommonTexts.LobbyDoesntExist;
				break;
			case MyLobbyStatusCode.FriendsOnly:
				id = MyCommonTexts.OnlyFriendsCanJoinThisGame;
				break;
			case MyLobbyStatusCode.InvalidPasscode:
				id = MyCommonTexts.LobbyInvalidPasscode;
				break;
			case MyLobbyStatusCode.Limited:
				id = MyCommonTexts.LobbyLimited;
				break;
			case MyLobbyStatusCode.LostInternetConnection:
				id = MyCommonTexts.LobbyLostInternetConnection;
				break;
			case MyLobbyStatusCode.MemberBlockedYou:
				id = MyCommonTexts.LobbyMemberBlockedYou;
				break;
			case MyLobbyStatusCode.NoDirectConnections:
				id = MyCommonTexts.LobbyNoDirectConnections;
				break;
			case MyLobbyStatusCode.NotAllowed:
				id = MyCommonTexts.LobbyNotAllowed;
				break;
			case MyLobbyStatusCode.ServiceUnavailable:
				id = MyCommonTexts.LobbyServiceUnavailable;
				break;
			case MyLobbyStatusCode.UserMultiplayerRestricted:
				id = MyCommonTexts.LobbyUserMultiplayerRestricted;
				break;
			case MyLobbyStatusCode.VersionMismatch:
				id = MyCommonTexts.LobbyVersionMismatch;
				break;
			case MyLobbyStatusCode.YouBlockedMember:
				id = MyCommonTexts.LobbyYouBlockedMember;
				break;
			case MyLobbyStatusCode.NoUser:
				id = MyCommonTexts.LobbyNoUser;
				break;
			default:
				id = MyCommonTexts.LobbyError;
				break;
			}
			return new StringBuilder(string.Format(MyTexts.GetString(id), MySession.GameServiceName));
		}

		private static void OnJoinFailed(MyGuiScreenProgress progress, MyMultiplayerBase multiplayer, MyLobbyStatusCode response)
		{
			multiplayer?.Dispose();
			progress.Cancel();
			if (response != MyLobbyStatusCode.Success)
			{
				MyGuiSandbox.Show(GetErrorMessage(response));
			}
		}

		private static void CheckDx11AndJoin(MyObjectBuilder_World world, MyMultiplayerBase multiplayer)
		{
			if (multiplayer.Scenario)
			{
				MySessionLoader.LoadMultiplayerScenarioWorld(world, multiplayer);
			}
			else
			{
				MySessionLoader.LoadMultiplayerSession(world, multiplayer);
			}
		}

		public static void OnDX11SwitchRequestAnswer(MyGuiScreenMessageBox.ResultEnum result)
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

		public static void WorldReceived(MyObjectBuilder_World world, MyMultiplayerBase multiplayer)
		{
			if (world != null && world.Checkpoint != null && world.Checkpoint.Settings != null && !MySandboxGame.Config.ExperimentalMode && (world.Checkpoint.Settings.IsSettingsExperimental() || (world.Checkpoint.Mods != null && world.Checkpoint.Mods.Count != 0)))
			{
				MySessionLoader.UnloadAndExitToMenu();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(MyCommonTexts.DialogTextJoinWorldFailed, MyTexts.GetString(MyCommonTexts.MultiplayerErrorExperimental));
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, stringBuilder, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			else
			{
				CheckDx11AndJoin(world, multiplayer);
			}
		}
	}
}
