using System;
using System.Net;

namespace VRage.Http
{
	public interface IVRageHttp
	{
		HttpStatusCode SendRequest(string url, HttpData[] parameters, HttpMethod method, out string content);

		void SendRequestAsync(string url, HttpData[] parameters, HttpMethod method, Action<HttpStatusCode, string> onDone);

		void DownloadAsync(string url, string filename, Action<ulong> onProgress, Action<HttpStatusCode> action);
	}
}
