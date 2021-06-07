using Sandbox.Game.Localization;
using System;
using System.Text;
using VRage;

namespace Sandbox.Game.Gui
{
	public class MyHudScenarioInfo
	{
		private enum LineEnum
		{
			TimeLeft,
			LivesLeft
		}

		private int m_livesLeft = -1;

		private int m_timeLeftMin = -1;

		private int m_timeLeftSec = -1;

		private bool m_needsRefresh = true;

		private MyHudNameValueData m_data;

		private bool m_visible;

		public int LivesLeft
		{
			get
			{
				return m_livesLeft;
			}
			set
			{
				if (m_livesLeft != value)
				{
					m_livesLeft = value;
					m_needsRefresh = true;
					Visible = true;
				}
			}
		}

		public int TimeLeftMin
		{
			get
			{
				return m_timeLeftMin;
			}
			set
			{
				if (m_timeLeftMin != value)
				{
					m_timeLeftMin = value;
					m_needsRefresh = true;
					Visible = true;
				}
			}
		}

		public int TimeLeftSec
		{
			get
			{
				return m_timeLeftSec;
			}
			set
			{
				if (m_timeLeftSec != value)
				{
					m_timeLeftSec = value;
					m_needsRefresh = true;
					Visible = true;
				}
			}
		}

		public MyHudNameValueData Data
		{
			get
			{
				if (m_needsRefresh)
				{
					Refresh();
				}
				return m_data;
			}
		}

		public bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				m_visible = value;
			}
		}

		public MyHudScenarioInfo()
		{
			m_data = new MyHudNameValueData(typeof(LineEnum).GetEnumValues().Length);
			Reload();
		}

		public void Reload()
		{
			MyHudNameValueData data = m_data;
			data[1].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudScenarioInfoLivesLeft));
			data[0].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudScenarioInfoTimeLeft));
			m_livesLeft = -1;
			m_timeLeftMin = -1;
			m_timeLeftSec = -1;
			m_needsRefresh = true;
		}

		public void Refresh()
		{
			m_needsRefresh = false;
			if (LivesLeft >= 0)
			{
				Data[1].Value.Clear().AppendInt32(LivesLeft);
				Data[1].Visible = true;
			}
			else
			{
				Data[1].Visible = false;
			}
			if (TimeLeftMin > 0 || TimeLeftSec >= 0)
			{
				Data[0].Value.Clear().AppendInt32(TimeLeftMin).Append(":")
					.AppendFormat("{0:D2}", TimeLeftSec);
				Data[0].Visible = true;
			}
			else
			{
				Data[0].Visible = false;
			}
			if (Data.GetVisibleCount() == 0)
			{
				Visible = false;
			}
			else
			{
				Visible = true;
			}
		}

		public void Show(Action<MyHudScenarioInfo> propertiesInit)
		{
			Refresh();
			propertiesInit?.Invoke(this);
		}

		public void Hide()
		{
			Visible = false;
		}
	}
}
