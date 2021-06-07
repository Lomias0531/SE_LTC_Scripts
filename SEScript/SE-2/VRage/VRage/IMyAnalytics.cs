using System.Collections.Generic;

namespace VRage
{
	/// <summary>
	/// Generic interface for all analytics library integrations
	/// </summary>
	public interface IMyAnalytics
	{
		void ReportSessionStart(Dictionary<string, object> sessionData);

		void ReportSessionEnd(Dictionary<string, object> sessionData);

		void IdentifyPlayer(string playerId, Dictionary<string, object> identificationData);

		void ReportEvent(MyAnalyticsProgressionStatus status, Dictionary<string, object> eventData, double timestamp = 0.0);

		void FlushAndDispose();

		void ReportActivityStart(string activityName, string activityFocus, string activityType, string activityItemUsage, bool expectActivityEnd = true, string planetName = "", string planetType = "", float simSpeedPlayer = 1f, float simSpeedServer = 1f);

		void ReportActivityEnd(string activityName, string planetName = "", string planetType = "", float simSpeedPlayer = 1f, float simSpeedServer = 1f);

		void ReportPlayerDeath(string deathType, string deathCause, string planetName = "", string planetType = "", bool campaign = false, bool official = false, string gameMode = "Survival", string modList = "", int modCount = 0);

		void ReportServerStatus(int playerCount, int maxPlayers, float simSpeedServer, int entitiesCount, int gridsCount, int blocksCount, int movingGridsCount, string hostName, string worldType, string worldName, uint worldAge);

		void ReportCampaignProgression(string campaignName, string levelName, string completedState, int timeFromLastCompletion);

		void ReportToolbarPageSwitch(int page);

		void ReportScreenMouseClick(string screen, float positionX, float positionY, uint frame);

		void ReportModLoaded(string name);

		void ReportUsedNamespace(string namespaceName, int count);
	}
}
