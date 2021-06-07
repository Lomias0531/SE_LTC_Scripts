using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI.HudViewers
{
	public class MyHudControlChat : MyGuiControlMultilineText
	{
		public enum MyChatVisibilityEnum
		{
			Fade,
			AlwaysVisible,
			AlwaysHidden
		}

		public static readonly float FADE_OUT_TIME = 10000f;

		public static float SCROLL_SPEED = 0.03f;

		private int m_displayedMessageCount;

		private MyHudChat m_chat;

		private int m_lastTimestamp;

		private bool m_forceUpdate;

		private MyChatVisibilityEnum m_visibility;

		private float m_fadeOut = 1f;

		private float m_scrollPosition = 1f;

		public MyChatVisibilityEnum Visibility
		{
			get
			{
				return m_visibility;
			}
			set
			{
				m_visibility = value;
				m_forceUpdate = true;
				UpdateText();
				switch (value)
				{
				case MyChatVisibilityEnum.Fade:
				case MyChatVisibilityEnum.AlwaysVisible:
				case MyChatVisibilityEnum.AlwaysHidden:
					return;
				}
				throw new ArgumentOutOfRangeException();
			}
		}

		public void ScrollDown()
		{
			m_scrollPosition += SCROLL_SPEED;
			float num = m_scrollbarV.MaxSize - m_scrollbarV.PageSize;
			if (m_scrollPosition >= num)
			{
				m_scrollPosition = num;
			}
			base.SetScrollbarValueV = m_scrollPosition;
		}

		public void ScrollUp()
		{
			m_scrollPosition -= SCROLL_SPEED;
			if (m_scrollPosition <= 0f)
			{
				m_scrollPosition = 0f;
			}
			base.SetScrollbarValueV = m_scrollPosition;
		}

		public MyHudControlChat(MyHudChat chat, Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, string font = "White", float textScale = 0.5f, MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, StringBuilder contents = null, MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, int? visibleLinesCount = null, bool selectable = false)
			: base(position, size, backgroundColor, font, textScale, textAlign, contents, drawScrollbarV: true, drawScrollbarH: false, textBoxAlign, null, selectable, showTextShadow: true)
		{
			m_forceUpdate = true;
			m_chat = chat;
			m_chat.ChatControl = this;
			base.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			base.VisibleChanged += MyHudControlChat_VisibleChanged;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			UpdateText();
			base.Draw(transitionAlpha * m_fadeOut, backgroundTransitionAlpha * m_fadeOut);
		}

		private void MyHudControlChat_VisibleChanged(object sender, bool isVisible)
		{
			if (!isVisible)
			{
				m_forceUpdate = true;
			}
		}

		private void UpdateText()
		{
			if ((float)m_chat.TimeSinceLastUpdate > FADE_OUT_TIME)
			{
				m_fadeOut -= 0.01f;
				if (m_fadeOut < 0f)
				{
					m_fadeOut = 0f;
				}
			}
			else
			{
				m_fadeOut = 1f;
			}
			if (!m_forceUpdate && m_lastTimestamp == m_chat.Timestamp)
			{
				return;
			}
			float value = m_scrollbarV.Value;
			bool flag = true;
			float num = m_scrollbarV.MaxSize - m_scrollbarV.PageSize;
			if (num > 0f && m_scrollbarV.Value < num)
			{
				flag = false;
			}
			Clear();
			bool showChatTimestamp = MySandboxGame.Config.ShowChatTimestamp;
			for (int i = 0; i < m_chat.MessageHistory.Count; i++)
			{
				MyChatItem myChatItem = m_chat.MessageHistory[i];
				StringBuilder stringBuilder = new StringBuilder(myChatItem.Sender);
				if (showChatTimestamp)
				{
					StringBuilder text = new StringBuilder("[").Append(myChatItem.Timestamp.ToLongTimeString()).Append("] ");
					AppendText(text, myChatItem.Font, base.TextScale, Color.LightGray);
				}
				stringBuilder.Append(": ");
				AppendText(stringBuilder, myChatItem.Font, base.TextScale, myChatItem.SenderColor);
				AppendText(new StringBuilder(myChatItem.Message), "White", base.TextScale, myChatItem.MessageColor);
				AppendLine();
			}
			m_displayedMessageCount = m_chat.MessageHistory.Count;
			m_forceUpdate = false;
			m_lastTimestamp = m_chat.Timestamp;
			RecalculateScrollBar();
			if (flag)
			{
				m_scrollbarV.Value = m_scrollbarV.MaxSize - m_scrollbarV.PageSize;
			}
			else
			{
				m_scrollbarV.Value = value;
			}
		}
	}
}
