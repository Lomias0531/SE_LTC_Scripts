using VRage;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyHudNotification : MyHudNotificationBase, IMyHudNotification
	{
		private MyStringId m_originalText;

		public MyStringId Text
		{
			get
			{
				return m_originalText;
			}
			set
			{
				if (m_originalText != value)
				{
					m_originalText = value;
					SetTextDirty();
				}
			}
		}

		string IMyHudNotification.Text
		{
			get
			{
				return GetText();
			}
			set
			{
				SetTextFormatArguments(value);
			}
		}

		int IMyHudNotification.AliveTime
		{
			get
			{
				return m_lifespanMs;
			}
			set
			{
				m_lifespanMs = value;
				ResetAliveTime();
			}
		}

		string IMyHudNotification.Font
		{
			get
			{
				return Font;
			}
			set
			{
				Font = value;
			}
		}

		public MyHudNotification(MyStringId text = default(MyStringId), int disappearTimeMs = 2500, string font = "Blue", MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, int priority = 0, MyNotificationLevel level = MyNotificationLevel.Normal)
			: base(disappearTimeMs, font, textAlign, priority, level)
		{
			m_originalText = text;
		}

		protected override string GetOriginalText()
		{
			return MyTexts.Get(m_originalText).ToString();
		}

		void IMyHudNotification.Show()
		{
			MyHud.Notifications.Add(this);
		}

		void IMyHudNotification.Hide()
		{
			MyHud.Notifications.Remove(this);
		}

		void IMyHudNotification.ResetAliveTime()
		{
			ResetAliveTime();
		}
	}
}
