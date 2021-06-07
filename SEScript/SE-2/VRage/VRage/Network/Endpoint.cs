namespace VRage.Network
{
	public struct Endpoint
	{
		public readonly EndpointId Id;

		public readonly byte Index;

		public Endpoint(EndpointId id, byte index)
		{
			Id = id;
			Index = index;
		}

		public Endpoint(ulong id, byte index)
		{
			Id = new EndpointId(id);
			Index = index;
		}

		public static bool operator ==(Endpoint a, Endpoint b)
		{
			if (a.Id == b.Id)
			{
				return a.Index == b.Index;
			}
			return false;
		}

		public static bool operator !=(Endpoint a, Endpoint b)
		{
			return !(a == b);
		}

		private bool Equals(Endpoint other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (obj is Endpoint)
			{
				return Equals((Endpoint)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			EndpointId id = Id;
			return id.GetHashCode() ^ Index.GetHashCode();
		}
	}
}
