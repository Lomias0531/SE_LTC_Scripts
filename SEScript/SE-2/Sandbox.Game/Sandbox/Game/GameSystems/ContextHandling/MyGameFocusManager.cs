namespace Sandbox.Game.GameSystems.ContextHandling
{
	public class MyGameFocusManager
	{
		private IMyFocusHolder m_currentFocusHolder;

		public void Register(IMyFocusHolder newFocusHolder)
		{
			if (m_currentFocusHolder != null && newFocusHolder != m_currentFocusHolder)
			{
				m_currentFocusHolder.OnLostFocus();
			}
			m_currentFocusHolder = newFocusHolder;
		}

		public void Unregister(IMyFocusHolder focusHolder)
		{
			if (m_currentFocusHolder == focusHolder)
			{
				m_currentFocusHolder = null;
			}
		}

		public void Clear()
		{
			if (m_currentFocusHolder != null)
			{
				m_currentFocusHolder.OnLostFocus();
			}
			m_currentFocusHolder = null;
		}
	}
}
