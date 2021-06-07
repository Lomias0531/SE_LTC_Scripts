using LitJson;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Net;
using VRage;
using VRage.Http;
using VRage.Utils;

namespace Sandbox.Engine.Analytics
{
	public class MyOpickaAnalytics : IMyAnalytics
	{
		private class NewSessionRequest
		{
			public class ProductPurchases
			{
				public string ProductID;

				public uint? PurchaseTimestamp;
			}

			public string UniqueUserIdentifier;

			public string GameID;

			public string GameVersion;

			public List<ProductPurchases> Purchases;
		}

		public class SessionResponse
		{
			public string UniqueUserIdentifier;

			public long SessionId;
		}

		public class NewEventRequest
		{
			public class EventValue
			{
				public string DataKey;

				public string DataValue;
			}

			public List<EventValue> Values
			{
				get;
				set;
			}

			public string UniqueUserIdentifier
			{
				get;
				set;
			}

			public long SessionId
			{
				get;
				set;
			}

			public string EventType
			{
				get;
				set;
			}
		}

		private static readonly object m_singletonGuard = new object();

		private static volatile MyOpickaAnalytics m_instance;

		private const string GameProgression01 = "Game";

		private string m_uniqueUserIdentifier = string.Empty;

		private long m_sessionId;

		private string m_gameId = string.Empty;

		private string m_gameVersion = string.Empty;

		public static MyOpickaAnalytics GetOrCreateInstance(string gameId, string gameVersion)
		{
			lock (m_singletonGuard)
			{
				if (m_instance == null)
				{
					m_instance = new MyOpickaAnalytics(gameId, gameVersion);
				}
			}
			return m_instance;
		}

		public static ulong GetTestValue()
		{
			if (MyGameService.IsActive)
			{
				return MyGameService.UserId % 2uL;
			}
			return 0uL;
		}

		private MyOpickaAnalytics(string gameId, string gameVersion)
		{
			m_gameId = gameId;
			m_gameVersion = gameVersion;
		}

		public void FlushAndDispose()
		{
		}

		public void ReportSessionStart(Dictionary<string, object> sessionData)
		{
			NewSessionRequest newSessionRequest = new NewSessionRequest
			{
				UniqueUserIdentifier = m_uniqueUserIdentifier,
				GameID = m_gameId,
				GameVersion = m_gameVersion
			};
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				newSessionRequest.Purchases = new List<NewSessionRequest.ProductPurchases>();
				if (MyGameService.IsProductOwned(MyGameService.AppId, out DateTime? purchaseTime))
				{
					newSessionRequest.Purchases.Add(new NewSessionRequest.ProductPurchases
					{
						ProductID = MyGameService.AppId.ToString(),
						PurchaseTimestamp = purchaseTime.Value.ToUnixTimestamp()
					});
				}
				foreach (KeyValuePair<uint, MyDLCs.MyDLC> dLC in MyDLCs.DLCs)
				{
					if (MyGameService.IsProductOwned(dLC.Key, out DateTime? purchaseTime2))
					{
						newSessionRequest.Purchases.Add(new NewSessionRequest.ProductPurchases
						{
							ProductID = dLC.Key.ToString(),
							PurchaseTimestamp = purchaseTime2.Value.ToUnixTimestamp()
						});
					}
				}
			}
			try
			{
				string value = JsonMapper.ToJson(newSessionRequest);
				string url = "https://crashlogs.keenswh.com/api/session";
				HttpData[] parameters = new HttpData[2]
				{
					new HttpData("application/json", value, HttpDataType.RequestBody),
					new HttpData("Content-Type", "application/json", HttpDataType.HttpHeader)
				};
				MyVRage.Platform.Http.SendRequestAsync(url, parameters, HttpMethod.POST, OnSessionStartResponse);
			}
			catch
			{
			}
		}

		private void OnSessionStartResponse(HttpStatusCode code, string content)
		{
			if (code == HttpStatusCode.OK)
			{
				try
				{
					SessionResponse sessionResponse = JsonMapper.ToObject<SessionResponse>(content);
					m_sessionId = sessionResponse.SessionId;
					MyLog.Default.WriteLine($"Analytics session: {m_sessionId}");
				}
				catch (Exception arg)
				{
					MyLog.Default.WriteLine($"Opicka reponse error: {arg}\n{content}");
				}
			}
			else
			{
				MyLog.Default.WriteLine("Analytics session error");
			}
		}

		public void ReportSessionEnd(Dictionary<string, object> sessionData)
		{
			try
			{
				string url = $"https://crashlogs.keenswh.com/api/session?userId={m_uniqueUserIdentifier}&sessionId={m_sessionId}";
				MyVRage.Platform.Http.SendRequestAsync(url, null, HttpMethod.DELETE, null);
			}
			catch
			{
			}
			m_sessionId = 0L;
		}

		public void IdentifyPlayer(string playerId, Dictionary<string, object> identificationData)
		{
			m_uniqueUserIdentifier = playerId;
			MyLog.Default.WriteLine("Analytics uuid: " + m_uniqueUserIdentifier);
		}

		public void UpdatePlayerIdentity(Dictionary<string, object> identificationData)
		{
		}

		public void ReportEvent(MyAnalyticsProgressionStatus status, Dictionary<string, object> eventData, double timestamp = 0.0)
		{
			NewEventRequest newEventRequest = new NewEventRequest
			{
				UniqueUserIdentifier = m_uniqueUserIdentifier,
				SessionId = m_sessionId,
				EventType = status.ToString(),
				Values = new List<NewEventRequest.EventValue>()
			};
			if (eventData != null)
			{
				foreach (KeyValuePair<string, object> eventDatum in eventData)
				{
					NewEventRequest.EventValue item = new NewEventRequest.EventValue
					{
						DataKey = eventDatum.Key,
						DataValue = eventDatum.Value.ToString()
					};
					newEventRequest.Values.Add(item);
				}
			}
			try
			{
				string value = JsonMapper.ToJson(newEventRequest);
				string url = "https://crashlogs.keenswh.com/api/event";
				HttpData[] parameters = new HttpData[2]
				{
					new HttpData("application/json", value, HttpDataType.RequestBody),
					new HttpData("Content-Type", "application/json", HttpDataType.HttpHeader)
				};
				MyVRage.Platform.Http.SendRequestAsync(url, parameters, HttpMethod.POST, null);
			}
			catch
			{
			}
		}

		public void ReportActivityStart(string activityName, string activityFocus, string activityType, string activityItemUsage, bool expectActivityEnd = true, string planetName = "", string planetType = "", float simSpeedPlayer = 1f, float simSpeedServer = 1f)
		{
		}

		public void ReportActivityEnd(string activityName, string planetName = "", string planetType = "", float simSpeedPlayer = 1f, float simSpeedServer = 1f)
		{
		}

		public void ReportPlayerDeath(string deathType, string deathCause, string planetName = "", string planetType = "", bool campaign = false, bool official = false, string gameMode = "Survival", string modList = "", int modCount = 0)
		{
		}

		public void ReportServerStatus(int playerCount, int maxPlayers, float simSpeedServer, int entitiesCount, int gridsCount, int blocksCount, int movingGridsCount, string hostName, string worldType, string worldName, uint worldAge)
		{
		}

		public void ReportCampaignProgression(string campaignName, string levelName, string completedState, int timeFromLastCompletion)
		{
		}

		public void ReportToolbarPageSwitch(int page)
		{
		}

		public void ReportScreenMouseClick(string screen, float positionX, float positionY, uint frame)
		{
		}

		public void ReportModLoaded(string name)
		{
		}

		public void ReportUsedNamespace(string namespaceName, int count)
		{
		}
	}
}
