using VRage.Network;

namespace Sandbox.Game.Replication
{
	public abstract class MyExternalReplicableEvent<T> : MyExternalReplicable<T>, IMyProxyTarget, IMyNetObject, IMyEventOwner where T : IMyEventProxy
	{
		private IMyEventProxy m_proxy;

		IMyEventProxy IMyProxyTarget.Target => m_proxy;

		protected override void OnHook()
		{
			m_proxy = base.Instance;
		}
	}
}
