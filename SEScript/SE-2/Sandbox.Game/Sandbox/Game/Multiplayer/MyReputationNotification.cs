using Sandbox.Game.Gui;

namespace Sandbox.Game.Multiplayer
{
	internal class MyReputationNotification
	{
		private string m_notificationTag = string.Empty;

		private int m_notificationValue;

		private MyHudNotification m_notification;

		internal MyReputationNotification(MyHudNotification notification)
		{
			m_notification = notification;
		}

		internal void UpdateReputationNotification(in string newTag, in int valueChange)
		{
			if (!m_notification.Alive)
			{
				MyHud.Notifications.Add(m_notification);
				m_notificationTag = newTag;
				m_notificationValue = valueChange;
			}
			else if (m_notificationTag == newTag)
			{
				m_notificationValue += valueChange;
				m_notification.ResetAliveTime();
			}
			else
			{
				m_notificationTag = newTag;
				m_notificationValue = valueChange;
			}
			m_notification.SetTextFormatArguments(m_notificationTag, m_notificationValue);
		}
	}
}
