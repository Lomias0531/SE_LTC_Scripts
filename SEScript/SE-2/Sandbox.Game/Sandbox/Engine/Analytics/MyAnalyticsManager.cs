using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;

namespace Sandbox.Engine.Analytics
{
	public class MyAnalyticsManager
	{
		private static MyAnalyticsManager m_instance;

		private List<IMyAnalytics> m_trackers = new List<IMyAnalytics>();

		public static MyAnalyticsManager Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyAnalyticsManager();
				}
				return m_instance;
			}
		}

		public void RegisterAnalyticsTracker(IMyAnalytics tracker)
		{
			if (tracker != null && m_trackers != null && !m_trackers.Contains(tracker))
			{
				m_trackers.Add(tracker);
			}
		}

		public void UnregisterAnalyticsTracker(IMyAnalytics tracker)
		{
			if (tracker != null && m_trackers != null && m_trackers.Contains(tracker))
			{
				m_trackers.Remove(tracker);
			}
		}

		[Conditional("VRAGE")]
		internal void StartSession(Dictionary<string, object> sessionData)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportSessionStart(sessionData);
			}
		}

		[Conditional("VRAGE")]
		internal void EndSession(Dictionary<string, object> sessionData)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportSessionEnd(sessionData);
			}
		}

		[Conditional("VRAGE")]
		internal void IdentifyPlayer(string playerId, string playerName, bool isSteamOnline, Dictionary<string, object> identificationData)
		{
			if (identificationData == null)
			{
				identificationData = new Dictionary<string, object>();
			}
			identificationData["player_name"] = playerName;
			identificationData["is_steam_online"] = isSteamOnline;
			identificationData["last_login"] = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.IdentifyPlayer(playerId, identificationData);
			}
		}

		[Conditional("VRAGE")]
		internal void ReportEvent(MyAnalyticsProgressionStatus status, Dictionary<string, object> eventData, double timestamp = 0.0)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportEvent(status, eventData, timestamp);
			}
		}

		public void FlushAndDispose()
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.FlushAndDispose();
			}
			m_trackers.Clear();
		}

		internal void ReportScreenMouseClick(string screen, float x, float y, uint seconds)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportScreenMouseClick(screen, x, y, seconds);
			}
		}

		internal void ReportUsedNamespace(string namespaceName, int count)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportUsedNamespace(namespaceName, count);
			}
		}

		internal void ReportModLoaded(string name)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportModLoaded(name);
			}
		}

		internal void ReportPlayerDeath(string deathType, string deathCause, string planetName, string planetType, bool campaign, bool official, string gameMode, string modList, int modCount)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportPlayerDeath(deathType, deathCause, planetName, planetType, campaign, official, gameMode, modList, modCount);
			}
		}

		internal void ReportActivityEnd(string activityName, string planetName = "", string planetType = "", float simSpeedPlayer = 1f, float simSpeedServer = 1f)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportActivityEnd(activityName, planetName, planetType, simSpeedPlayer, simSpeedServer);
			}
		}

		internal void ReportActivityStart(string activityName, string activityFocus, string activityType, string activityItemUsage, bool expectActivityEnd, string planetName = "", string planetType = "", float simSpeedPlayer = 1f, float simSpeedServer = 1f)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportActivityStart(activityName, activityFocus, activityType, activityItemUsage, expectActivityEnd, planetName, planetType, simSpeedPlayer, simSpeedServer);
			}
		}

		internal void ReportServerStatus(int playerCount, int maxPlayers, float simSpeedServer, int entitiesCount, int gridCount, int blockCount, int movingGridsCount, string hostName, string worldType, string worldName, uint worldAge)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportServerStatus(playerCount, maxPlayers, simSpeedServer, entitiesCount, gridCount, blockCount, movingGridsCount, hostName, worldType, worldName, worldAge);
			}
		}

		internal void ReportToolbarPageSwitch(int page)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportToolbarPageSwitch(page);
			}
		}

		internal void ReportCampaignProgression(string campaignName, string levelName, string completedState, int timeFromLastCompletion)
		{
			foreach (IMyAnalytics tracker in m_trackers)
			{
				tracker.ReportCampaignProgression(campaignName, levelName, completedState, timeFromLastCompletion);
			}
		}
	}
}
