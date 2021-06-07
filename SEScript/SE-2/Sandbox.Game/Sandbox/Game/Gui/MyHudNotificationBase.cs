using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public abstract class MyHudNotificationBase
	{
		public static readonly int INFINITE;

		private int m_formatArgsCount;

		private object[] m_textFormatArguments = new object[20];

		private MyGuiDrawAlignEnum m_actualTextAlign;

		private int m_aliveTime;

		private string m_notificationText;

		private bool m_isTextDirty;

		public int m_lifespanMs;

		public MyNotificationLevel Level;

		public readonly int Priority;

		public string Font;

		public bool HasFog;

		public bool IsControlsHint => Level == MyNotificationLevel.Control;

		public bool Alive
		{
			get;
			private set;
		}

		public MyHudNotificationBase(int disapearTimeMs, string font = "Blue", MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, int priority = 0, MyNotificationLevel level = MyNotificationLevel.Normal)
		{
			Font = font;
			Priority = priority;
			HasFog = false;
			m_isTextDirty = true;
			m_actualTextAlign = textAlign;
			AssignFormatArgs(null);
			Level = level;
			m_lifespanMs = disapearTimeMs;
			m_aliveTime = 0;
			Alive = false;
		}

		public void SetTextDirty()
		{
			m_isTextDirty = true;
		}

		public string GetText()
		{
			if (string.IsNullOrEmpty(m_notificationText) || m_isTextDirty)
			{
				if (m_formatArgsCount > 0)
				{
					m_notificationText = string.Format(GetOriginalText(), m_textFormatArguments);
				}
				else
				{
					m_notificationText = GetOriginalText();
				}
				m_isTextDirty = false;
			}
			return m_notificationText;
		}

		public object[] GetTextFormatArguments()
		{
			return m_textFormatArguments;
		}

		public void SetTextFormatArguments(params object[] arguments)
		{
			AssignFormatArgs(arguments);
			m_notificationText = null;
			GetText();
		}

		public void AddAliveTime(int timeStep)
		{
			m_aliveTime += timeStep;
			RefreshAlive();
		}

		public void ResetAliveTime()
		{
			m_aliveTime = 0;
			RefreshAlive();
		}

		protected abstract string GetOriginalText();

		private void RefreshAlive()
		{
			Alive = (m_lifespanMs == INFINITE || m_aliveTime < m_lifespanMs);
		}

		private void AssignFormatArgs(object[] args)
		{
			int i = 0;
			m_formatArgsCount = 0;
			if (args != null)
			{
				if (m_textFormatArguments.Length < args.Length)
				{
					m_textFormatArguments = new object[args.Length];
				}
				for (; i < args.Length; i++)
				{
					m_textFormatArguments[i] = args[i];
				}
				m_formatArgsCount = args.Length;
			}
			for (; i < m_textFormatArguments.Length; i++)
			{
				m_textFormatArguments[i] = "<missing>";
			}
		}

		public virtual void BeforeAdd()
		{
		}

		public virtual void BeforeRemove()
		{
		}
	}
}
