using System;
using System.Globalization;
using System.Threading;

namespace Sandbox.Engine.Networking
{
	internal static class MyNetworkMonitor
	{
		private static Thread m_workerThread;

		private static bool m_sessionEnabled;

		private static bool m_running;

		public static event Action OnTick;

		public static void Init()
		{
			m_running = true;
			m_workerThread = new Thread(Worker)
			{
				CurrentCulture = CultureInfo.InvariantCulture,
				CurrentUICulture = CultureInfo.InvariantCulture,
				Name = "Network Monitor"
			};
			m_workerThread.Start();
		}

		public static void Done()
		{
			m_running = false;
			m_workerThread.Join();
		}

		public static void StartSession()
		{
			m_sessionEnabled = true;
		}

		public static void EndSession()
		{
			m_sessionEnabled = false;
		}

		private static void Tick()
		{
			MyNetworkWriter.SendAll();
			MyGameService.ServerUpdate();
			MyGameService.Peer2Peer.BeginFrameProcessing();
			try
			{
				if (m_sessionEnabled)
				{
					MyNetworkReader.ReceiveAll();
				}
			}
			finally
			{
				MyGameService.Peer2Peer.EndFrameProcessing();
			}
		}

		private static void Worker()
		{
			while (m_running)
			{
				Thread.Sleep(8);
				Tick();
				MyNetworkMonitor.OnTick?.Invoke();
			}
		}
	}
}
