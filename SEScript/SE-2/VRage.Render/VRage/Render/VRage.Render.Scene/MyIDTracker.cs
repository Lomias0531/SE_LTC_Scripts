using System.Collections.Generic;

namespace VRage.Render.Scene
{
	public class MyIDTracker<T> where T : class
	{
		private uint m_ID;

		private T m_value;

		private static readonly Dictionary<uint, MyIDTracker<T>> m_dict = new Dictionary<uint, MyIDTracker<T>>();

		internal uint ID => m_ID;

		internal T Value => m_value;

		public static int Count => m_dict.Count;

		public MyIDTracker()
		{
			m_ID = uint.MaxValue;
		}

		public static T FindByID(uint id)
		{
			if (m_dict.TryGetValue(id, out MyIDTracker<T> value))
			{
				return value.m_value;
			}
			return null;
		}

		internal void Register(uint id, T val)
		{
			m_ID = id;
			m_value = val;
			m_dict[id] = this;
		}

		internal void Deregister()
		{
			m_dict.Remove(m_ID);
			m_ID = uint.MaxValue;
			m_value = null;
		}

		internal void Clear()
		{
			Deregister();
		}
	}
}
