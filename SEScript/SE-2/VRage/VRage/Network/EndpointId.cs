namespace VRage.Network
{
	/// <summary>
	/// Id of network endpoint, opaque struct, internal value should not be accessed outside VRage.Network.
	/// EndpointId is not guid and can change when client reconnects to server.
	/// Internally it's SteamId or RakNetGUID.
	/// </summary>
	public struct EndpointId
	{
		public readonly ulong Value;

		public static EndpointId Null = new EndpointId(0uL);

		public bool IsNull => Value == 0;

		public bool IsValid => !IsNull;

		public EndpointId(ulong value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static bool operator ==(EndpointId a, EndpointId b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(EndpointId a, EndpointId b)
		{
			return a.Value != b.Value;
		}

		public bool Equals(EndpointId other)
		{
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (obj is EndpointId)
			{
				return Equals((EndpointId)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}
