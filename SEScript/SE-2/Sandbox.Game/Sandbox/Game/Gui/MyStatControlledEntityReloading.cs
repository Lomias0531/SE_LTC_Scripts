using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityReloading : MyStatBase
	{
		private MyUserControllableGun m_lastConnected;

		private int m_reloadCompletionTime;

		private int m_reloadInterval;

		public MyStatControlledEntityReloading()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_reloading");
		}

		public override void Update()
		{
			MyUserControllableGun myUserControllableGun = MySession.Static.ControlledEntity as MyUserControllableGun;
			if (myUserControllableGun != m_lastConnected)
			{
				if (m_lastConnected != null)
				{
					m_lastConnected.ReloadStarted -= OnReloading;
				}
				m_lastConnected = myUserControllableGun;
				if (myUserControllableGun != null)
				{
					myUserControllableGun.ReloadStarted += OnReloading;
				}
				m_reloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			int num = m_reloadCompletionTime - MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (num > 0)
			{
				base.CurrentValue = 1f - (float)num / (float)m_reloadInterval;
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}

		private void OnReloading(int reloadTime)
		{
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (m_reloadCompletionTime <= totalGamePlayTimeInMilliseconds)
			{
				m_reloadCompletionTime = totalGamePlayTimeInMilliseconds + reloadTime;
				m_reloadInterval = reloadTime;
			}
		}
	}
}
