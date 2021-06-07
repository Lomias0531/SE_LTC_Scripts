using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Sync
{
	public class SyncType
	{
		private List<SyncBase> m_properties;

		private Action<SyncBase> m_registeredHandlers;

		public ListReader<SyncBase> Properties => new ListReader<SyncBase>(m_properties);

		public event Action<SyncBase> PropertyChanged
		{
			add
			{
				m_registeredHandlers = (Action<SyncBase>)Delegate.Combine(m_registeredHandlers, value);
				foreach (SyncBase property in m_properties)
				{
					property.ValueChanged += value;
				}
			}
			remove
			{
				foreach (SyncBase property in m_properties)
				{
					property.ValueChanged -= value;
				}
				m_registeredHandlers = (Action<SyncBase>)Delegate.Remove(m_registeredHandlers, value);
			}
		}

		public event Action<SyncBase> PropertyChangedNotify
		{
			add
			{
				foreach (SyncBase property in m_properties)
				{
					property.ValueChangedNotify += value;
				}
			}
			remove
			{
				foreach (SyncBase property in m_properties)
				{
					property.ValueChangedNotify -= value;
				}
			}
		}

		public event Action PropertyCountChanged;

		public SyncType(List<SyncBase> properties)
		{
			m_properties = properties;
		}

		public void Append(object obj)
		{
			int count = m_properties.Count;
			SyncHelpers.Compose(obj, m_properties.Count, m_properties);
			for (int i = count; i < m_properties.Count; i++)
			{
				m_properties[i].ValueChanged += m_registeredHandlers;
			}
			this.PropertyCountChanged.InvokeIfNotNull();
		}
	}
}
