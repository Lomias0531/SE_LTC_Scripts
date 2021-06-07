using System;
using System.Collections.Generic;

namespace VRage.Utils
{
	/// <summary>
	/// From http://www.wintellect.com/Resources CLR Via C# by Jeffrey Richter
	/// </summary>
	public sealed class MyEventSet
	{
		private readonly Dictionary<MyStringId, Delegate> m_events = new Dictionary<MyStringId, Delegate>(MyStringId.Comparer);

		public void Add(MyStringId eventKey, Delegate handler)
		{
			m_events.TryGetValue(eventKey, out Delegate value);
			m_events[eventKey] = Delegate.Combine(value, handler);
		}

		public void Remove(MyStringId eventKey, Delegate handler)
		{
			if (m_events.TryGetValue(eventKey, out Delegate value))
			{
				value = Delegate.Remove(value, handler);
				if ((object)value != null)
				{
					m_events[eventKey] = value;
				}
				else
				{
					m_events.Remove(eventKey);
				}
			}
		}

		public void Raise(MyStringId eventKey, object sender, EventArgs e)
		{
			m_events.TryGetValue(eventKey, out Delegate value);
			value?.DynamicInvoke(sender, e);
		}
	}
}
