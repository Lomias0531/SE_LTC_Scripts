using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlRadioButtonGroup : IEnumerable<MyGuiControlRadioButton>, IEnumerable
	{
		public const int INVALID_INDEX = -1;

		private List<MyGuiControlRadioButton> m_radioButtons;

		private int? m_selectedIndex;

		public MyGuiControlRadioButton SelectedButton => TryGetButton(SelectedIndex ?? (-1));

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
						m_radioButtons[m_selectedIndex.Value].Selected = false;
					}
					m_selectedIndex = value;
					if (m_selectedIndex.HasValue)
					{
						m_radioButtons[m_selectedIndex.Value].Selected = true;
					}
					if (this.SelectedChanged != null)
					{
						this.SelectedChanged(this);
					}
				}
			}
		}

		public int Count => m_radioButtons.Count;

		public event Action<MyGuiControlRadioButtonGroup> SelectedChanged;

		public event Action<MyGuiControlRadioButton> MouseDoubleClick;

		public MyGuiControlRadioButtonGroup()
		{
			m_radioButtons = new List<MyGuiControlRadioButton>();
			m_selectedIndex = null;
		}

		public void Add(MyGuiControlRadioButton radioButton)
		{
			m_radioButtons.Add(radioButton);
			radioButton.SelectedChanged += OnRadioButtonSelected;
			radioButton.MouseDoubleClick += OnRadioButtonMouseDoubleClick;
		}

		private void OnRadioButtonMouseDoubleClick(MyGuiControlRadioButton button)
		{
			this.MouseDoubleClick.InvokeIfNotNull(button);
		}

		public void Remove(MyGuiControlRadioButton radioButton)
		{
			radioButton.SelectedChanged -= OnRadioButtonSelected;
			radioButton.MouseDoubleClick -= OnRadioButtonMouseDoubleClick;
			m_radioButtons.Remove(radioButton);
		}

		public void Clear()
		{
			foreach (MyGuiControlRadioButton radioButton in m_radioButtons)
			{
				radioButton.SelectedChanged -= OnRadioButtonSelected;
				radioButton.MouseDoubleClick -= OnRadioButtonMouseDoubleClick;
			}
			m_radioButtons.Clear();
			m_selectedIndex = null;
		}

		public void SelectByKey(int key)
		{
			for (int i = 0; i < m_radioButtons.Count; i++)
			{
				MyGuiControlRadioButton myGuiControlRadioButton = m_radioButtons[i];
				if (myGuiControlRadioButton.Key == key)
				{
					SelectedIndex = i;
					myGuiControlRadioButton.Selected = true;
				}
				else
				{
					myGuiControlRadioButton.Selected = false;
				}
			}
		}

		public void SelectByIndex(int index)
		{
			if (SelectedIndex.HasValue)
			{
				m_radioButtons[SelectedIndex.Value].Selected = false;
			}
			if (index < m_radioButtons.Count)
			{
				MyGuiControlRadioButton myGuiControlRadioButton = m_radioButtons[index];
				SelectedIndex = index;
				myGuiControlRadioButton.Selected = true;
			}
		}

		private void OnRadioButtonSelected(MyGuiControlRadioButton sender)
		{
			SelectedIndex = m_radioButtons.IndexOf(sender);
		}

		private MyGuiControlRadioButton TryGetButton(int buttonIdx)
		{
			if (buttonIdx >= m_radioButtons.Count || buttonIdx < 0)
			{
				return null;
			}
			return m_radioButtons[buttonIdx];
		}

		public IEnumerator<MyGuiControlRadioButton> GetEnumerator()
		{
			return m_radioButtons.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
