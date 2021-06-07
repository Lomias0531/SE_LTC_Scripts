using Sandbox.Game.Multiplayer;
using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.World
{
	public class MyControllerInfo : IMyControllerInfo
	{
		private MyEntityController m_controller;

		public MyEntityController Controller
		{
			get
			{
				return m_controller;
			}
			set
			{
				if (m_controller != value)
				{
					ReleaseControls();
					m_controller = value;
					AcquireControls();
				}
			}
		}

		public long ControllingIdentityId
		{
			get
			{
				if (Controller == null)
				{
					return 0L;
				}
				return Controller.Player.Identity.IdentityId;
			}
		}

		IMyEntityController IMyControllerInfo.Controller => Controller;

		long IMyControllerInfo.ControllingIdentityId => ControllingIdentityId;

		public event Action<MyEntityController> ControlAcquired;

		public event Action<MyEntityController> ControlReleased;

		event Action<IMyEntityController> IMyControllerInfo.ControlAcquired
		{
			add
			{
				ControlAcquired += GetDelegate(value);
			}
			remove
			{
				ControlAcquired -= GetDelegate(value);
			}
		}

		event Action<IMyEntityController> IMyControllerInfo.ControlReleased
		{
			add
			{
				ControlReleased += GetDelegate(value);
			}
			remove
			{
				ControlReleased -= GetDelegate(value);
			}
		}

		public bool IsLocallyControlled()
		{
			if (Controller != null && Sync.Clients != null)
			{
				return Controller.Player.Client == Sync.Clients.LocalClient;
			}
			return false;
		}

		public bool IsLocallyHumanControlled()
		{
			if (Controller != null && Sync.Clients != null && Sync.Clients.LocalClient != null)
			{
				return Controller.Player == Sync.Clients.LocalClient.FirstPlayer;
			}
			return false;
		}

		public bool IsRemotelyControlled()
		{
			if (Controller != null)
			{
				return Controller.Player.Client != Sync.Clients.LocalClient;
			}
			return false;
		}

		private Action<MyEntityController> GetDelegate(Action<IMyEntityController> value)
		{
			return (Action<MyEntityController>)Delegate.CreateDelegate(typeof(Action<MyEntityController>), value.Target, value.Method);
		}

		bool IMyControllerInfo.IsLocallyControlled()
		{
			return IsLocallyControlled();
		}

		bool IMyControllerInfo.IsLocallyHumanControlled()
		{
			return IsLocallyHumanControlled();
		}

		bool IMyControllerInfo.IsRemotelyControlled()
		{
			return IsRemotelyControlled();
		}

		public void ReleaseControls()
		{
			if (m_controller != null)
			{
				this.ControlReleased?.Invoke(m_controller);
			}
		}

		public void AcquireControls()
		{
			if (m_controller != null)
			{
				this.ControlAcquired?.Invoke(m_controller);
			}
		}
	}
}
