using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using VRage;

namespace Sandbox.Game.GameSystems
{
	public class MyGridRadioSystem
	{
		private HashSet<MyDataBroadcaster> m_broadcasters;

		private HashSet<MyDataReceiver> m_receivers;

		private HashSet<MyRadioBroadcaster> m_radioBroadcasters;

		private HashSet<MyRadioReceiver> m_radioReceivers;

		private HashSet<MyLaserBroadcaster> m_laserBroadcasters;

		private HashSet<MyLaserReceiver> m_laserReceivers;

		public bool IsClosing;

		private MyMultipleEnabledEnum m_antennasBroadcasterEnabled;

		private bool m_antennasBroadcasterEnabledNeedsRefresh;

		private MyCubeGrid m_grid;

		public MyMultipleEnabledEnum AntennasBroadcasterEnabled
		{
			get
			{
				if (m_antennasBroadcasterEnabledNeedsRefresh)
				{
					RefreshAntennasBroadcasterEnabled();
				}
				return m_antennasBroadcasterEnabled;
			}
			set
			{
				if (m_antennasBroadcasterEnabled != value && m_antennasBroadcasterEnabled != 0 && !IsClosing)
				{
					BroadcasterStateChanged(value);
				}
			}
		}

		public HashSet<MyDataBroadcaster> Broadcasters => m_broadcasters;

		public HashSet<MyDataReceiver> Receivers => m_receivers;

		public HashSet<MyLaserBroadcaster> LaserBroadcasters => m_laserBroadcasters;

		public HashSet<MyLaserReceiver> LaserReceivers => m_laserReceivers;

		public MyGridRadioSystem(MyCubeGrid grid)
		{
			m_broadcasters = new HashSet<MyDataBroadcaster>();
			m_receivers = new HashSet<MyDataReceiver>();
			m_radioBroadcasters = new HashSet<MyRadioBroadcaster>();
			m_radioReceivers = new HashSet<MyRadioReceiver>();
			m_laserBroadcasters = new HashSet<MyLaserBroadcaster>();
			m_laserReceivers = new HashSet<MyLaserReceiver>();
			m_antennasBroadcasterEnabled = MyMultipleEnabledEnum.NoObjects;
			m_grid = grid;
		}

		public void BroadcasterStateChanged(MyMultipleEnabledEnum enabledState)
		{
			m_antennasBroadcasterEnabled = enabledState;
			bool value = enabledState == MyMultipleEnabledEnum.AllEnabled;
			if (Sync.IsServer)
			{
				foreach (MyRadioBroadcaster radioBroadcaster in m_radioBroadcasters)
				{
					if (radioBroadcaster.Entity is MyRadioAntenna)
					{
						(radioBroadcaster.Entity as MyRadioAntenna).EnableBroadcasting.Value = value;
					}
				}
			}
			m_antennasBroadcasterEnabledNeedsRefresh = false;
		}

		public void Register(MyDataBroadcaster broadcaster)
		{
			m_broadcasters.Add(broadcaster);
			MyLaserBroadcaster myLaserBroadcaster = broadcaster as MyLaserBroadcaster;
			if (myLaserBroadcaster != null)
			{
				m_laserBroadcasters.Add(myLaserBroadcaster);
				return;
			}
			MyRadioBroadcaster myRadioBroadcaster = broadcaster as MyRadioBroadcaster;
			if (myRadioBroadcaster != null && broadcaster.Entity is MyRadioAntenna)
			{
				m_radioBroadcasters.Add(myRadioBroadcaster);
				(broadcaster.Entity as MyRadioAntenna).EnableBroadcasting.ValueChanged += delegate
				{
					broadcaster_EnabledChanged();
				};
				if (m_radioBroadcasters.Count == 1)
				{
					m_antennasBroadcasterEnabled = ((!myRadioBroadcaster.Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
				}
				else if ((AntennasBroadcasterEnabled == MyMultipleEnabledEnum.AllEnabled && !myRadioBroadcaster.Enabled) || (AntennasBroadcasterEnabled == MyMultipleEnabledEnum.AllDisabled && myRadioBroadcaster.Enabled))
				{
					m_antennasBroadcasterEnabled = MyMultipleEnabledEnum.Mixed;
				}
			}
		}

		public void Register(MyDataReceiver reciever)
		{
			m_receivers.Add(reciever);
			MyLaserReceiver myLaserReceiver = reciever as MyLaserReceiver;
			if (myLaserReceiver != null)
			{
				m_laserReceivers.Add(myLaserReceiver);
				return;
			}
			MyRadioReceiver myRadioReceiver = reciever as MyRadioReceiver;
			if (myRadioReceiver != null)
			{
				m_radioReceivers.Add(myRadioReceiver);
			}
		}

		public void Unregister(MyDataBroadcaster broadcaster)
		{
			m_broadcasters.Remove(broadcaster);
			MyLaserBroadcaster myLaserBroadcaster = broadcaster as MyLaserBroadcaster;
			if (myLaserBroadcaster != null)
			{
				m_laserBroadcasters.Remove(myLaserBroadcaster);
				return;
			}
			MyRadioBroadcaster myRadioBroadcaster = broadcaster as MyRadioBroadcaster;
			if (myRadioBroadcaster != null && broadcaster.Entity is MyRadioAntenna)
			{
				m_radioBroadcasters.Remove(myRadioBroadcaster);
				(broadcaster.Entity as MyRadioAntenna).EnableBroadcasting.ValueChanged -= delegate
				{
					broadcaster_EnabledChanged();
				};
				if (m_radioBroadcasters.Count == 0)
				{
					m_antennasBroadcasterEnabled = MyMultipleEnabledEnum.NoObjects;
				}
				else if (m_radioBroadcasters.Count == 1)
				{
					AntennasBroadcasterEnabled = ((!m_radioBroadcasters.First().Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
				}
				else if (AntennasBroadcasterEnabled == MyMultipleEnabledEnum.Mixed)
				{
					m_antennasBroadcasterEnabledNeedsRefresh = true;
				}
			}
		}

		public void Unregister(MyDataReceiver reciever)
		{
			m_receivers.Remove(reciever);
			MyLaserReceiver myLaserReceiver = reciever as MyLaserReceiver;
			if (myLaserReceiver != null)
			{
				m_laserReceivers.Remove(myLaserReceiver);
				return;
			}
			MyRadioReceiver myRadioReceiver = reciever as MyRadioReceiver;
			if (myRadioReceiver != null)
			{
				m_radioReceivers.Remove(myRadioReceiver);
			}
		}

		private void RefreshAntennasBroadcasterEnabled()
		{
			m_antennasBroadcasterEnabledNeedsRefresh = false;
			bool flag = true;
			bool flag2 = true;
			foreach (MyRadioBroadcaster radioBroadcaster in m_radioBroadcasters)
			{
				if (radioBroadcaster.Entity is MyRadioAntenna)
				{
					flag = (flag && radioBroadcaster.Enabled);
					flag2 = (flag2 && !radioBroadcaster.Enabled);
					if (!flag && !flag2)
					{
						m_antennasBroadcasterEnabled = MyMultipleEnabledEnum.Mixed;
						return;
					}
				}
			}
			AntennasBroadcasterEnabled = ((!flag) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
		}

		private void broadcaster_EnabledChanged()
		{
			m_antennasBroadcasterEnabledNeedsRefresh = true;
		}

		public void UpdateRemoteControlInfo()
		{
			foreach (MyDataBroadcaster broadcaster in m_broadcasters)
			{
				broadcaster.UpdateRemoteControlInfo();
			}
		}
	}
}
