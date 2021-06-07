using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VRage.Http;
using VRage.Utils;

namespace VRage.Game
{
	public class MyRankedServers
	{
		private static XmlSerializer m_serializer = new XmlSerializer(typeof(MyRankedServers));

		public List<MyRankServer> Servers
		{
			get;
			set;
		}

		public MyRankedServers()
		{
			Servers = new List<MyRankServer>();
		}

		public static void LoadAsync(string url, Action<MyRankedServers> completedCallback)
		{
			Task.Run(delegate
			{
				DownloadChangelog(url, completedCallback);
			});
		}

		private static void DownloadChangelog(string url, Action<MyRankedServers> completedCallback)
		{
			MyRankedServers obj = null;
			try
			{
				if (MyVRage.Platform.Http.SendRequest(url, null, HttpMethod.GET, out string content) == HttpStatusCode.OK)
				{
					using (StringReader textReader = new StringReader(content))
					{
						obj = (m_serializer.Deserialize(textReader) as MyRankedServers);
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Error while downloading ranked servers: " + ex.ToString());
				return;
			}
			completedCallback?.Invoke(obj);
		}

		public static void SaveTestData()
		{
			MyRankedServers myRankedServers = new MyRankedServers();
			myRankedServers.Servers.Add(new MyRankServer
			{
				Address = "10.20.0.26:27016",
				Rank = 1
			});
			using (FileStream stream = File.OpenWrite("rankedServers.xml"))
			{
				m_serializer.Serialize(stream, myRankedServers);
			}
		}
	}
}
