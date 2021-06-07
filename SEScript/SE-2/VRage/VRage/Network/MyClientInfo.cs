namespace VRage.Network
{
	public struct MyClientInfo
	{
		private readonly MyClient m_clients;

		public MyClientStateBase State => m_clients.State;

		public Endpoint EndpointId => m_clients.State.EndpointId;

		public float PriorityMultiplier => m_clients.PriorityMultiplier;

		internal MyClientInfo(MyClient client)
		{
			m_clients = client;
		}

		public bool HasReplicable(IMyReplicable replicable)
		{
			return m_clients.Replicables.ContainsKey(replicable);
		}

		public bool IsReplicableReady(IMyReplicable replicable)
		{
			return m_clients.IsReplicableReady(replicable);
		}
	}
}
