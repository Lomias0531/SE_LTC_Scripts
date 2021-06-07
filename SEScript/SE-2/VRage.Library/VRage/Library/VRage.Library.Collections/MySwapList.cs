using System.Collections.Generic;

namespace VRage.Library.Collections
{
	public class MySwapList<T>
	{
		private List<T> m_foreground = new List<T>();

		private List<T> m_background = new List<T>();

		public List<T> BackgroundList => m_background;

		public T this[int index]
		{
			get
			{
				return m_foreground[index];
			}
			set
			{
				m_foreground[index] = value;
			}
		}

		public void Add(T element)
		{
			m_foreground.Add(element);
		}

		public void Remove(T element)
		{
			m_background.Add(element);
		}

		public void Clear()
		{
			m_foreground.Clear();
		}

		public void Swap()
		{
			List<T> foreground = m_foreground;
			m_foreground = m_background;
			m_background = foreground;
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return m_foreground.GetEnumerator();
		}
	}
}
