using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using VRage;

namespace Sandbox.Game.GameSystems
{
	public class MyGridReflectorLightSystem
	{
		private HashSet<MyReflectorLight> m_reflectors;

		public bool IsClosing;

		private MyMultipleEnabledEnum m_reflectorsEnabled;

		private bool m_reflectorsEnabledNeedsRefresh;

		private MyCubeGrid m_grid;

		public int ReflectorCount => m_reflectors.Count;

		public MyMultipleEnabledEnum ReflectorsEnabled
		{
			get
			{
				if (m_reflectorsEnabledNeedsRefresh)
				{
					RefreshReflectorsEnabled();
				}
				return m_reflectorsEnabled;
			}
			set
			{
				if (m_reflectorsEnabled != value && m_reflectorsEnabled != 0 && !IsClosing)
				{
					m_grid.SendReflectorState(value);
				}
			}
		}

		public MyGridReflectorLightSystem(MyCubeGrid grid)
		{
			m_reflectors = new HashSet<MyReflectorLight>();
			m_reflectorsEnabled = MyMultipleEnabledEnum.NoObjects;
			m_grid = grid;
		}

		public void ReflectorStateChanged(MyMultipleEnabledEnum enabledState)
		{
			m_reflectorsEnabled = enabledState;
			if (Sync.IsServer)
			{
				bool enabled = enabledState == MyMultipleEnabledEnum.AllEnabled;
				foreach (MyReflectorLight reflector in m_reflectors)
				{
					reflector.EnabledChanged -= reflector_EnabledChanged;
					reflector.Enabled = enabled;
					reflector.EnabledChanged += reflector_EnabledChanged;
				}
				m_reflectorsEnabledNeedsRefresh = false;
			}
		}

		public void Register(MyReflectorLight reflector)
		{
			m_reflectors.Add(reflector);
			reflector.EnabledChanged += reflector_EnabledChanged;
			if (m_reflectors.Count == 1)
			{
				m_reflectorsEnabled = ((!reflector.Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
			}
			else if ((ReflectorsEnabled == MyMultipleEnabledEnum.AllEnabled && !reflector.Enabled) || (ReflectorsEnabled == MyMultipleEnabledEnum.AllDisabled && reflector.Enabled))
			{
				m_reflectorsEnabled = MyMultipleEnabledEnum.Mixed;
			}
		}

		public void Unregister(MyReflectorLight reflector)
		{
			m_reflectors.Remove(reflector);
			reflector.EnabledChanged -= reflector_EnabledChanged;
			if (m_reflectors.Count == 0)
			{
				m_reflectorsEnabled = MyMultipleEnabledEnum.NoObjects;
			}
			else if (m_reflectors.Count == 1)
			{
				m_reflectorsEnabled = ((!m_reflectors.First().Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
			}
			else if (ReflectorsEnabled == MyMultipleEnabledEnum.Mixed)
			{
				m_reflectorsEnabledNeedsRefresh = true;
			}
		}

		private void RefreshReflectorsEnabled()
		{
			m_reflectorsEnabledNeedsRefresh = false;
			if (Sync.IsServer)
			{
				bool flag = true;
				bool flag2 = true;
				foreach (MyReflectorLight reflector in m_reflectors)
				{
					flag = (flag && reflector.Enabled);
					flag2 = (flag2 && !reflector.Enabled);
					if (!flag && !flag2)
					{
						m_reflectorsEnabled = MyMultipleEnabledEnum.Mixed;
						return;
					}
				}
				ReflectorsEnabled = ((!flag) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
			}
		}

		private void reflector_EnabledChanged(MyTerminalBlock obj)
		{
			m_reflectorsEnabledNeedsRefresh = true;
		}
	}
}
