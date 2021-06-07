using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace VRage.Utils
{
	public class MyConsolePipeWriter : TextWriter
	{
		private static object lockObject = new object();

		private NamedPipeClientStream m_pipeStream;

		private StreamWriter m_writer;

		private bool isConnecting;

		public override Encoding Encoding => Encoding.UTF8;

		public MyConsolePipeWriter(string name)
		{
			m_pipeStream = new NamedPipeClientStream(name);
			m_writer = new StreamWriter(m_pipeStream);
			StartConnectThread();
		}

		public override void Write(string value)
		{
			if (m_pipeStream.IsConnected)
			{
				try
				{
					m_writer.Write(value);
					m_writer.Flush();
				}
				catch (IOException)
				{
					StartConnectThread();
				}
			}
			else
			{
				StartConnectThread();
			}
		}

		public override void WriteLine(string value)
		{
			if (m_pipeStream.IsConnected)
			{
				try
				{
					m_writer.WriteLine(value);
					m_writer.Flush();
				}
				catch (IOException)
				{
					StartConnectThread();
				}
			}
			else
			{
				StartConnectThread();
			}
		}

		private void StartConnectThread()
		{
			lock (lockObject)
			{
				if (isConnecting)
				{
					return;
				}
				isConnecting = true;
			}
			Task.Run(delegate
			{
				m_pipeStream.Connect();
				lock (lockObject)
				{
					isConnecting = false;
				}
			});
		}

		public override void Close()
		{
			base.Close();
			try
			{
				if (m_pipeStream.IsConnected)
				{
					m_pipeStream.WaitForPipeDrain();
					m_writer.Close();
					m_writer.Dispose();
					m_pipeStream.Close();
					m_pipeStream.Dispose();
				}
			}
			catch
			{
			}
		}
	}
}
