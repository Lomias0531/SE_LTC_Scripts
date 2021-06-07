using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components.Trading;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.GameSystems.Trading
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof(MyObjectBuilder_TradingManager), null)]
	[StaticEventOwner]
	public class MyTradingManager : MySessionComponentBase
	{
		private struct MyTradeOfferState
		{
			internal ulong RequestedId;

			internal bool OfferAccepted;

			internal MyObjectBuilder_SubmitOffer Offer;
		}

		protected sealed class TradeRequest_Server_003C_003ESystem_UInt64_0023System_UInt64 : ICallSite<IMyEventOwner, ulong, ulong, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong requestingId, in ulong requestedId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TradeRequest_Server(requestingId, requestedId);
			}
		}

		protected sealed class TradeRequest_StartTrade_003C_003ESystem_UInt64 : ICallSite<IMyEventOwner, ulong, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong requestingId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TradeRequest_StartTrade(requestingId);
			}
		}

		protected sealed class TradeRequest_StartTrade_Server_003C_003ESandbox_Game_GameSystems_Trading_MyTradeResponseReason : ICallSite<IMyEventOwner, MyTradeResponseReason, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyTradeResponseReason reason, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TradeRequest_StartTrade_Server(reason);
			}
		}

		protected sealed class TradeRequest_Response_003C_003ESystem_UInt64_0023Sandbox_Game_GameSystems_Trading_MyTradeResponseReason : ICallSite<IMyEventOwner, ulong, MyTradeResponseReason, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong otherPlayerId, in MyTradeResponseReason reason, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TradeRequest_Response(otherPlayerId, reason);
			}
		}

		protected sealed class TradeRequest_Cancel_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TradeRequest_Cancel();
			}
		}

		protected sealed class SubmitTradingOffer_Server_003C_003EVRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer : ICallSite<IMyEventOwner, MyObjectBuilder_SubmitOffer, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_SubmitOffer obOffer, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SubmitTradingOffer_Server(obOffer);
			}
		}

		protected sealed class SubmitTradingOffer_ClientRecieve_003C_003EVRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer : ICallSite<IMyEventOwner, MyObjectBuilder_SubmitOffer, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_SubmitOffer obOffer, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SubmitTradingOffer_ClientRecieve(obOffer);
			}
		}

		protected sealed class SubmitTradingOffer_Abort_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SubmitTradingOffer_Abort();
			}
		}

		protected sealed class AcceptOffer_Server_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool isAccepted, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AcceptOffer_Server(isAccepted);
			}
		}

		protected sealed class AcceptOffer_ClientRecieve_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool isAccepted, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AcceptOffer_ClientRecieve(isAccepted);
			}
		}

		private const int TIMEOUT_SEC = 30;

		private const float MAXIMUM_TRADE_DISTANCE_SQUARED = 25f;

		public static MyTradingManager Static;

		private Action<MyTradeResponseReason> m_onAnswerRecieved;

		private MyPlayerTradeViewModel m_activeTradeView;

		private MyGuiScreenMessageBox m_activeTradeReqMsgBox;

		private Dictionary<ulong, MyTradeOfferState> m_activePlayerTrades = new Dictionary<ulong, MyTradeOfferState>();

		public override bool IsRequiredByGame => true;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			Static = this;
			if (Sync.IsServer)
			{
				MySession.Static.Players.PlayerRemoved += OnPlayerRemovedFromGame;
			}
		}

		private void OnPlayerRemovedFromGame(MyPlayer.PlayerId player)
		{
			if (Static.m_activePlayerTrades.TryGetValue(player.SteamId, out MyTradeOfferState value))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, 0uL, MyTradeResponseReason.Offline, new EndpointId(value.RequestedId));
				Static.m_activePlayerTrades.Remove(player.SteamId);
				Static.m_activePlayerTrades.Remove(value.RequestedId);
			}
		}

		public void TradeRequest_Client(ulong requestingId, ulong requestedId, Action<MyTradeResponseReason> AnswerRecievedDelegate)
		{
			m_onAnswerRecieved = AnswerRecievedDelegate;
			MyPlayer outPlayer;
			MyPlayer outPlayer2;
			MyTradeResponseReason myTradeResponseReason = ValidateTradeProssible(requestingId, requestedId, out outPlayer, out outPlayer2);
			if (myTradeResponseReason != MyTradeResponseReason.Ok)
			{
				TradeRequest_Response(0uL, myTradeResponseReason);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Server, requestingId, requestedId);
			}
		}

		[Event(null, 88)]
		[Reliable]
		[Server]
		private static void TradeRequest_Server(ulong requestingId, ulong requestedId)
		{
			MyPlayer outPlayer;
			MyPlayer outPlayer2;
			MyTradeResponseReason myTradeResponseReason = ValidateTradeProssible(requestingId, requestedId, out outPlayer, out outPlayer2);
			if (myTradeResponseReason != MyTradeResponseReason.Ok)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, requestedId, myTradeResponseReason, new EndpointId(requestingId));
			}
			else if (Static.m_activePlayerTrades.ContainsKey(requestedId))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, requestedId, MyTradeResponseReason.AlreadyTrading, new EndpointId(requestingId));
			}
			else if (!Static.m_activePlayerTrades.ContainsKey(requestingId))
			{
				MyTradeOfferState myTradeOfferState = default(MyTradeOfferState);
				myTradeOfferState.RequestedId = requestedId;
				MyTradeOfferState value = myTradeOfferState;
				Static.m_activePlayerTrades.Add(requestingId, value);
				myTradeOfferState = default(MyTradeOfferState);
				myTradeOfferState.RequestedId = requestingId;
				MyTradeOfferState value2 = myTradeOfferState;
				Static.m_activePlayerTrades.Add(requestedId, value2);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_StartTrade, requestingId, new EndpointId(requestedId));
			}
		}

		[Event(null, 117)]
		[Reliable]
		[Client]
		private static void TradeRequest_StartTrade(ulong requestingId)
		{
			if (MySession.Static.Players.TryGetPlayerById(new MyPlayer.PlayerId(requestingId), out MyPlayer player))
			{
				if (MySandboxGame.Config.EnableTrading)
				{
					StringBuilder stringBuilder = new StringBuilder();
					Static.m_activeTradeReqMsgBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, stringBuilder.AppendFormat(MySpaceTexts.TradeScreenPopupAcceptTrade, player.DisplayName), MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum res)
					{
						OnTradeRequestCallback(res, requestingId);
					});
					MyGuiSandbox.AddScreen(Static.m_activeTradeReqMsgBox);
				}
				else
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_StartTrade_Server, MyTradeResponseReason.Cancel);
				}
			}
		}

		private static void OnTradeRequestCallback(MyGuiScreenMessageBox.ResultEnum res, ulong requestingId)
		{
			if (res == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_StartTrade_Server, MyTradeResponseReason.Ok);
				Static.StartTrading(requestingId);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_StartTrade_Server, MyTradeResponseReason.Cancel);
			}
			Static.m_activeTradeReqMsgBox = null;
		}

		[Event(null, 156)]
		[Reliable]
		[Server]
		private static void TradeRequest_StartTrade_Server(MyTradeResponseReason reason)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (!Static.m_activePlayerTrades.TryGetValue(value, out MyTradeOfferState value2))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, 0uL, MyTradeResponseReason.Cancel, new EndpointId(value));
			}
			else if (reason == MyTradeResponseReason.Ok)
			{
				if (Static.m_activePlayerTrades.ContainsKey(value2.RequestedId))
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, value, MyTradeResponseReason.Ok, new EndpointId(value2.RequestedId));
				}
			}
			else
			{
				Static.m_activePlayerTrades.Remove(value);
				Static.m_activePlayerTrades.Remove(value2.RequestedId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, value, MyTradeResponseReason.Cancel, new EndpointId(value2.RequestedId));
			}
		}

		[Event(null, 183)]
		[Reliable]
		[Client]
		private static void TradeRequest_Response(ulong otherPlayerId, MyTradeResponseReason reason)
		{
			if (Static.m_onAnswerRecieved != null)
			{
				Static.m_onAnswerRecieved(reason);
				Static.m_onAnswerRecieved = null;
			}
			switch (reason)
			{
			case MyTradeResponseReason.AlreadyTrading:
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.TradeScreenPopupAlreadyTrading), MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
				break;
			case MyTradeResponseReason.Offline:
				CloseTradeWindows();
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.TradeScreenPopupOffline), MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
				break;
			case MyTradeResponseReason.Dead:
				CloseTradeWindows();
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.TradeScreenPopupDead), MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
				break;
			case MyTradeResponseReason.Cancel:
				CloseTradeWindows();
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.TradeScreenPopupCancel), MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
				break;
			case MyTradeResponseReason.Abort:
				CloseTradeWindows();
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.TradeScreenPopupError), MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
				break;
			case MyTradeResponseReason.Ok:
				if (otherPlayerId == 0L)
				{
					MyLog.Default.Error("Requested id for trade cannot be 0");
				}
				else
				{
					Static.StartTrading(otherPlayerId);
				}
				break;
			case MyTradeResponseReason.Complete:
				if (Static.m_activeTradeView != null)
				{
					Static.m_activeTradeView.CloseScreenLocal();
					Static.m_activeTradeView = null;
				}
				break;
			}
		}

		private static void CloseTradeWindows()
		{
			if (Static.m_activeTradeView != null)
			{
				Static.m_activeTradeView.CloseScreenLocal();
				Static.m_activeTradeView = null;
			}
			if (Static.m_activeTradeReqMsgBox != null)
			{
				Static.m_activeTradeReqMsgBox = null;
				MyScreenManager.GetScreenWithFocus().CloseScreen();
			}
		}

		private void StartTrading(ulong otherPlayerId)
		{
			MyScreenManager.CloseNowAllBelow(MyGuiScreenHudSpace.Static);
			MyPlayerTradeViewModel myPlayerTradeViewModel = new MyPlayerTradeViewModel(otherPlayerId);
			Static.m_activeTradeView = myPlayerTradeViewModel;
			ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(myPlayerTradeViewModel);
		}

		public void TradeCancel_Client()
		{
			m_onAnswerRecieved = null;
			m_activeTradeView = null;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Cancel);
		}

		[Event(null, 313)]
		[Reliable]
		[Server]
		private static void TradeRequest_Cancel()
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (!Static.m_activePlayerTrades.TryGetValue(value, out MyTradeOfferState value2))
			{
				MyLog.Default.Error($"Player with id: {value} that is not trading is trying to cancel offer.");
				return;
			}
			Static.m_activePlayerTrades.Remove(value2.RequestedId);
			Static.m_activePlayerTrades.Remove(value);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, value, MyTradeResponseReason.Cancel, new EndpointId(value2.RequestedId));
		}

		internal void SubmitTradingOffer_Client(MyObjectBuilder_SubmitOffer obOffer)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SubmitTradingOffer_Server, obOffer);
		}

		[Event(null, 339)]
		[Reliable]
		[Server]
		private static void SubmitTradingOffer_Server(MyObjectBuilder_SubmitOffer obOffer)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (!Static.m_activePlayerTrades.TryGetValue(value, out MyTradeOfferState value2))
			{
				MyLog.Default.Error($"Player with id: {value} that is not trading is trying to submit offer.");
				return;
			}
			MyPlayer outPlayer;
			MyPlayer outPlayer2;
			MyTradeResponseReason myTradeResponseReason = ValidateTradeProssible(value, value2.RequestedId, out outPlayer, out outPlayer2);
			if (myTradeResponseReason == MyTradeResponseReason.Ok)
			{
				MyInventory myInventory = outPlayer2.Identity.Character.GetInventoryBase() as MyInventory;
				MyFixedPoint b = 0;
				foreach (MyObjectBuilder_InventoryItem inventoryItem in obOffer.InventoryItems)
				{
					MyInventory.GetItemVolumeAndMass(inventoryItem.PhysicalContent.GetId(), out float _, out float itemVolume);
					b += itemVolume * inventoryItem.Amount;
				}
				if (myInventory.CurrentVolume + b > myInventory.MaxVolume)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SubmitTradingOffer_Abort, new EndpointId(MyEventContext.Current.Sender.Value));
					return;
				}
			}
			if (myTradeResponseReason == MyTradeResponseReason.Ok)
			{
				AcceptOffer_ServerInternal(value2.RequestedId, isAccepted: false);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SubmitTradingOffer_ClientRecieve, obOffer, new EndpointId(value2.RequestedId));
				value2.Offer = obOffer;
				Static.m_activePlayerTrades[value] = value2;
			}
		}

		[Event(null, 391)]
		[Reliable]
		[Client]
		private static void SubmitTradingOffer_ClientRecieve(MyObjectBuilder_SubmitOffer obOffer)
		{
			if (Static.m_activeTradeView != null)
			{
				Static.m_activeTradeView.OnOfferRecieved(obOffer);
			}
		}

		[Event(null, 402)]
		[Reliable]
		[Client]
		private static void SubmitTradingOffer_Abort()
		{
			if (Static.m_activeTradeView != null)
			{
				Static.m_activeTradeView.OnOfferAbortedRecieved();
			}
		}

		internal void AcceptOffer_Client(bool isAccepted)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AcceptOffer_Server, isAccepted);
		}

		[Event(null, 422)]
		[Reliable]
		[Server]
		private static void AcceptOffer_Server(bool isAccepted)
		{
			AcceptOffer_ServerInternal(MyEventContext.Current.Sender.Value, isAccepted);
		}

		private static void AcceptOffer_ServerInternal(ulong offerFromId, bool isAccepted)
		{
			MyTradeOfferState value2;
			if (!Static.m_activePlayerTrades.TryGetValue(offerFromId, out MyTradeOfferState value))
			{
				MyLog.Default.Error($"Player with id: {offerFromId} that is not trading is trying to submit offer.");
			}
			else if (!Static.m_activePlayerTrades.TryGetValue(value.RequestedId, out value2))
			{
				MyLog.Default.Error($"Player with id: {value.RequestedId} that is not trading is trying to submit offer.");
			}
			else if (isAccepted && value2.OfferAccepted)
			{
				ulong requestedId = value.RequestedId;
				if (ValidateTradeProssible(offerFromId, requestedId, out MyPlayer outPlayer, out MyPlayer outPlayer2) == MyTradeResponseReason.Ok)
				{
					long identityId = outPlayer.Identity.IdentityId;
					long identityId2 = outPlayer2.Identity.IdentityId;
					MyObjectBuilder_SubmitOffer offer = value.Offer;
					MyObjectBuilder_SubmitOffer offer2 = value2.Offer;
					TransferCurrency(identityId, identityId2, offer, offer2);
					TransferPCU(outPlayer, outPlayer2, offer, offer2);
					TransferInventoryItems(outPlayer, outPlayer2, offer, offer2);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, 0uL, MyTradeResponseReason.Complete, new EndpointId(offerFromId));
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TradeRequest_Response, 0uL, MyTradeResponseReason.Complete, new EndpointId(requestedId));
					Static.m_activePlayerTrades.Remove(offerFromId);
					Static.m_activePlayerTrades.Remove(requestedId);
				}
			}
			else
			{
				Static.m_activePlayerTrades[offerFromId] = new MyTradeOfferState
				{
					RequestedId = value.RequestedId,
					OfferAccepted = isAccepted,
					Offer = value.Offer
				};
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AcceptOffer_ClientRecieve, isAccepted, new EndpointId(value.RequestedId));
			}
		}

		private static void TransferPCU(MyPlayer player1, MyPlayer player2, MyObjectBuilder_SubmitOffer player1ToPlayer2Offer, MyObjectBuilder_SubmitOffer player2ToPlayer1Offer)
		{
			if (player1ToPlayer2Offer != null && player1ToPlayer2Offer.PCUAmount >= 0)
			{
				TransferPCU_Internal(player1, player2, player1ToPlayer2Offer);
			}
			if (player2ToPlayer1Offer != null && player2ToPlayer1Offer.PCUAmount >= 0)
			{
				TransferPCU_Internal(player2, player1, player2ToPlayer1Offer);
			}
		}

		private static void TransferPCU_Internal(MyPlayer player1, MyPlayer player2, MyObjectBuilder_SubmitOffer player1ToPlayer2Offer)
		{
			switch (MySession.Static.BlockLimitsEnabled)
			{
			case MyBlockLimitsEnabledEnum.NONE:
			case MyBlockLimitsEnabledEnum.GLOBALLY:
				break;
			case MyBlockLimitsEnabledEnum.PER_FACTION:
				TransferFactionPCU(player1, player2, player1ToPlayer2Offer);
				break;
			case MyBlockLimitsEnabledEnum.PER_PLAYER:
				TransferPlayerPCU(player1, player2, player1ToPlayer2Offer);
				break;
			default:
				MyLog.Default.Error("PCU TRANSFER - Case missing.");
				break;
			}
		}

		private static void TransferPlayerPCU(MyPlayer player1, MyPlayer player2, MyObjectBuilder_SubmitOffer player1ToPlayer2Offer)
		{
			if (player1.Identity.BlockLimits.PCU - player1ToPlayer2Offer.PCUAmount > 0)
			{
				player1.Identity.BlockLimits.AddPCU(-player1ToPlayer2Offer.PCUAmount);
				player2.Identity.BlockLimits.AddPCU(player1ToPlayer2Offer.PCUAmount);
			}
		}

		private static void TransferFactionPCU(MyPlayer player1, MyPlayer player2, MyObjectBuilder_SubmitOffer player1ToPlayer2Offer)
		{
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(player1.Identity.IdentityId);
			MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(player2.Identity.IdentityId);
			bool num = playerFaction?.IsLeader(player1.Identity.IdentityId) ?? false;
			bool flag = playerFaction2?.IsLeader(player2.Identity.IdentityId) ?? false;
			if (num && flag && playerFaction.BlockLimits.PCU - player1ToPlayer2Offer.PCUAmount > 0)
			{
				playerFaction.BlockLimits.AddPCU(-player1ToPlayer2Offer.PCUAmount);
				playerFaction2.BlockLimits.AddPCU(player1ToPlayer2Offer.PCUAmount);
			}
		}

		private static void TransferInventoryItems(MyPlayer player1, MyPlayer player2, MyObjectBuilder_SubmitOffer player1ToPlayer2Offer, MyObjectBuilder_SubmitOffer player2ToPlayer1Offer)
		{
			MyInventory myInventory = player1.Identity.Character.GetInventoryBase() as MyInventory;
			MyInventory myInventory2 = player2.Identity.Character.GetInventoryBase() as MyInventory;
			if (player1ToPlayer2Offer != null)
			{
				foreach (MyObjectBuilder_InventoryItem inventoryItem in player1ToPlayer2Offer.InventoryItems)
				{
					MyInventory.Transfer(myInventory, myInventory2, inventoryItem.PhysicalContent.GetId(), MyItemFlags.None, inventoryItem.Amount);
				}
			}
			if (player2ToPlayer1Offer != null)
			{
				foreach (MyObjectBuilder_InventoryItem inventoryItem2 in player2ToPlayer1Offer.InventoryItems)
				{
					MyInventory.Transfer(myInventory2, myInventory, inventoryItem2.PhysicalContent.GetId(), MyItemFlags.None, inventoryItem2.Amount);
				}
			}
		}

		private static void TransferCurrency(long player1IdentityId, long player2IdentityId, MyObjectBuilder_SubmitOffer player1ToPlayer2Offer, MyObjectBuilder_SubmitOffer player2ToPlayer1Offer)
		{
			if (player1ToPlayer2Offer != null && player1ToPlayer2Offer.CurrencyAmount > 0)
			{
				MyBankingSystem.Static.Transfer_Server(player1IdentityId, player2IdentityId, player1ToPlayer2Offer.CurrencyAmount);
			}
			if (player2ToPlayer1Offer != null && player2ToPlayer1Offer.CurrencyAmount > 0)
			{
				MyBankingSystem.Static.Transfer_Server(player2IdentityId, player1IdentityId, player2ToPlayer1Offer.CurrencyAmount);
			}
		}

		[Event(null, 574)]
		[Reliable]
		[Client]
		private static void AcceptOffer_ClientRecieve(bool isAccepted)
		{
			if (Static.m_activeTradeView != null)
			{
				Static.m_activeTradeView.OnOfferAcceptStateChange(isAccepted);
			}
		}

		public static MyTradeResponseReason ValidateTradeProssible(ulong playerId1, ulong playerId2, out MyPlayer outPlayer1, out MyPlayer outPlayer2)
		{
			MyTradeResponseReason result = MyTradeResponseReason.Abort;
			outPlayer1 = (outPlayer2 = null);
			if (MySession.Static.Players.TryGetPlayerById(new MyPlayer.PlayerId(playerId1), out MyPlayer player))
			{
				MyPlayer.PlayerId playerId3 = player.Id;
				if (!Sync.Players.IsPlayerOnline(ref playerId3))
				{
					return MyTradeResponseReason.Offline;
				}
				IMyControllableEntity controlledEntity = player.Controller.ControlledEntity;
				if (MySession.Static.Players.TryGetPlayerById(new MyPlayer.PlayerId(playerId2), out MyPlayer player2))
				{
					MyPlayer.PlayerId playerId4 = player2.Id;
					if (!Sync.Players.IsPlayerOnline(ref playerId4))
					{
						return MyTradeResponseReason.Offline;
					}
					IMyControllableEntity controlledEntity2 = player2.Controller.ControlledEntity;
					if (controlledEntity != null && controlledEntity2 != null)
					{
						double num = (controlledEntity.Entity.PositionComp.GetPosition() - controlledEntity2.Entity.PositionComp.GetPosition()).LengthSquared();
						outPlayer1 = player;
						outPlayer2 = player2;
						if (num > 25.0)
						{
							return MyTradeResponseReason.Abort;
						}
						if (player.Identity.IsDead || player.Character.IsDead || player2.Identity.IsDead || player2.Character.IsDead)
						{
							return MyTradeResponseReason.Dead;
						}
						return MyTradeResponseReason.Ok;
					}
					return MyTradeResponseReason.Dead;
				}
			}
			return result;
		}
	}
}
