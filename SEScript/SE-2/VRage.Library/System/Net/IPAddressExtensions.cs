namespace System.Net
{
	public static class IPAddressExtensions
	{
		public static uint ToIPv4NetworkOrder(this IPAddress ip)
		{
			return (uint)IPAddress.HostToNetworkOrder((int)ip.Address);
		}

		public static IPAddress FromIPv4NetworkOrder(uint ip)
		{
			return new IPAddress((uint)IPAddress.NetworkToHostOrder((int)ip));
		}

		public static IPAddress ParseOrAny(string ip)
		{
			if (!IPAddress.TryParse(ip, out IPAddress address))
			{
				return IPAddress.Any;
			}
			return address;
		}

		/// <summary>
		/// Parses IP Endpoint from string in format x.x.x.x:port
		/// </summary>
		public static bool TryParseEndpoint(string ipAndPort, out IPEndPoint result)
		{
			try
			{
				string[] array = ipAndPort.Replace(" ", string.Empty).Split(new string[1]
				{
					":"
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 2 && IPAddress.TryParse(array[0], out IPAddress address) && int.TryParse(array[1], out int result2))
				{
					result = new IPEndPoint(address, result2);
					return true;
				}
			}
			catch
			{
			}
			result = null;
			return false;
		}
	}
}
