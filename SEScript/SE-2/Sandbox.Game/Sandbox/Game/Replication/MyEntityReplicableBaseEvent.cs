using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System.Diagnostics;
using VRage.Game.Entity;
using VRage.Network;

namespace Sandbox.Game.Replication
{
	public abstract class MyEntityReplicableBaseEvent<T> : MyEntityReplicableBase<T>, IMyProxyTarget, IMyNetObject, IMyEventOwner where T : MyEntity, IMyEventProxy
	{
		private IMyEventProxy m_proxy;

		IMyEventProxy IMyProxyTarget.Target => m_proxy;

		protected override void OnHook()
		{
			base.OnHook();
			m_proxy = base.Instance;
		}

		[Conditional("DEBUG")]
		private void RegisterAsserts()
		{
			if (!Sync.IsServer)
			{
				base.Instance.OnMarkForClose += OnMarkForCloseOnClient;
				base.Instance.OnClose += OnMarkForCloseOnClient;
			}
		}

		private void OnMarkForCloseOnClient(MyEntity entity)
		{
			if (MyMultiplayer.Static != null)
			{
				IMyProxyTarget proxyTarget = MyMultiplayer.Static.ReplicationLayer.GetProxyTarget(m_proxy);
				if (MySession.Static.Ready && proxyTarget != null)
				{
					MyMultiplayer.Static.ReplicationLayer.TryGetNetworkIdByObject(proxyTarget, out NetworkId _);
				}
			}
		}
	}
}
