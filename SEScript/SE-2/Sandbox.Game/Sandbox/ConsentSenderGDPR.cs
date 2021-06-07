using Sandbox.Engine.Networking;
using Sandbox.Game;
using System;
using System.Net;
using VRage;
using VRage.Http;
using VRage.Utils;

namespace Sandbox
{
	public static class ConsentSenderGDPR
	{
		internal static void TrySendConsent()
		{
			try
			{
				ServicePointManager.SecurityProtocol = (ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
				HttpData[] parameters = new HttpData[5]
				{
					new HttpData("Content-Type", "application/x-www-form-urlencoded", HttpDataType.HttpHeader),
					new HttpData("User-Agent", "Space Engineers Client", HttpDataType.HttpHeader),
					new HttpData("lcvbex", MyPerGameSettings.BasicGameInfo.GameAcronym, HttpDataType.GetOrPost),
					new HttpData("qudfgh", MyGameService.UserId, HttpDataType.GetOrPost),
					new HttpData("praqnf", MySandboxGame.Config.GDPRConsent.Value ? "agree" : "disagree", HttpDataType.GetOrPost)
				};
				MyVRage.Platform.Http.SendRequestAsync("https://gdpr.keenswh.com/consent.php", parameters, HttpMethod.POST, HandleConsentResponse);
			}
			catch (Exception arg)
			{
				MyLog.Default.WriteLine("Cannot confirm GDPR consent: " + arg);
			}
		}

		private static void HandleConsentResponse(HttpStatusCode statusCode, string content)
		{
			bool consent = false;
			try
			{
				if (statusCode == HttpStatusCode.OK)
				{
					content = content.Replace("\r", "");
					content = content.Replace("\n", "");
					consent = (content == "OK");
				}
			}
			catch
			{
			}
			MySandboxGame.Static.Invoke(delegate
			{
				MySandboxGame.Config.GDPRConsentSent = consent;
				MySandboxGame.Config.Save();
			}, "HandleConsentResponse");
		}
	}
}
