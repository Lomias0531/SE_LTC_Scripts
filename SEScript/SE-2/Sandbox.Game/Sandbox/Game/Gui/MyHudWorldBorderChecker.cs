using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage;
using VRage.Game.Gui;

namespace Sandbox.Game.Gui
{
	public class MyHudWorldBorderChecker
	{
		private static readonly float WARNING_DISTANCE = 600f;

		private MyHudNotification m_notification = new MyHudNotification(MyCommonTexts.NotificationLeavingWorld, MyHudNotificationBase.INFINITE, "Red");

		private MyHudNotification m_notificationCreative = new MyHudNotification(MyCommonTexts.NotificationLeavingWorld_Creative, MyHudNotificationBase.INFINITE, "Red");

		internal static MyHudEntityParams HudEntityParams = new MyHudEntityParams(MyTexts.Get(MyCommonTexts.HudMarker_ReturnToWorld), 0L, MyHudIndicatorFlagsEnum.SHOW_TEXT | MyHudIndicatorFlagsEnum.SHOW_BORDER_INDICATORS);

		public bool WorldCenterHintVisible
		{
			get;
			private set;
		}

		public void Update()
		{
			if (MySession.Static.ControlledEntity == null)
			{
				return;
			}
			float num = MyEntities.WorldHalfExtent();
			double num2 = (MySession.Static.ControlledEntity.Entity != null) ? MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition().AbsMax() : 0.0;
			if (num != 0f && MySession.Static.ControlledEntity.Entity != null && (double)num - num2 < (double)WARNING_DISTANCE)
			{
				double num3 = ((double)num - num2 > 0.0) ? ((double)num - num2) : 0.0;
				if (MySession.Static.SurvivalMode)
				{
					m_notification.SetTextFormatArguments(num3);
					MyHud.Notifications.Add(m_notification);
				}
				else
				{
					m_notificationCreative.SetTextFormatArguments(num3);
					MyHud.Notifications.Add(m_notificationCreative);
				}
				WorldCenterHintVisible = true;
			}
			else
			{
				MyHud.Notifications.Remove(m_notification);
				MyHud.Notifications.Remove(m_notificationCreative);
				WorldCenterHintVisible = false;
			}
		}
	}
}
