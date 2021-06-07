using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlElementGroup : IEnumerable<MyGuiControlBase>, IEnumerable
	{
		public const int INVALID_INDEX = -1;

		private List<MyGuiControlBase> m_controlElements;

		private int? m_selectedIndex;

		public MyGuiControlBase SelectedElement => TryGetElement(SelectedIndex ?? (-1));

		public int? SelectedIndex
		{
			get
			{
				return m_selectedIndex;
			}
			set
			{
				if (m_selectedIndex != value)
				{
					if (m_selectedIndex.HasValue)
					{
						m_controlElements[m_selectedIndex.Value].HasHighlight = false;
					}
					m_selectedIndex = value;
					if (m_selectedIndex.HasValue)
					{
						m_controlElements[m_selectedIndex.Value].HasHighlight = true;
					}
					if (this.HighlightChanged != null)
					{
						this.HighlightChanged(this);
					}
				}
			}
		}

		public event Action<MyGuiControlElementGroup> HighlightChanged;

		public MyGuiControlElementGroup()
		{
			m_controlElements = new List<MyGuiControlBase>();
			m_selectedIndex = null;
		}

		public void Add(MyGuiControlBase controlElement)
		{
			if (controlElement.CanHaveFocus)
			{
				m_controlElements.Add(controlElement);
				controlElement.HightlightChanged += OnControlElementSelected;
			}
		}

		public void Remove(MyGuiControlBase controlElement)
		{
			controlElement.HightlightChanged -= OnControlElementSelected;
			m_controlElements.Remove(controlElement);
		}

		public void Clear()
		{
			foreach (MyGuiControlBase controlElement in m_controlElements)
			{
				controlElement.HightlightChanged -= OnControlElementSelected;
			}
			m_controlElements.Clear();
			m_selectedIndex = null;
		}

		public void SelectByIndex(int index)
		{
			if (SelectedIndex.HasValue)
			{
				m_controlElements[SelectedIndex.Value].HasHighlight = false;
			}
			MyGuiControlBase myGuiControlBase = m_controlElements[index];
			SelectedIndex = index;
			myGuiControlBase.HasHighlight = true;
		}

		private void OnControlElementSelected(MyGuiControlBase sender)
		{
			if (sender.HasHighlight)
			{
				SelectedIndex = m_controlElements.IndexOf(sender);
			}
		}

		private MyGuiControlBase TryGetElement(int elementIdx)
		{
			if (elementIdx >= m_controlElements.Count || elementIdx < 0)
			{
				return null;
			}
			return m_controlElements[elementIdx];
		}

		public IEnumerator<MyGuiControlBase> GetEnumerator()
		{
			return m_controlElements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
