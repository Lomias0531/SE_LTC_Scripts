using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	internal class MyMemoryProfiler
	{
		private static List<MyMemoryLogs.MyMemoryEvent> m_managed;

		private static List<MyMemoryLogs.MyMemoryEvent> m_native;

		private static List<MyMemoryLogs.MyMemoryEvent> m_timed;

		private static List<MyMemoryLogs.MyMemoryEvent> m_events;

		private static bool m_initialized = false;

		private static bool Enabled = false;

		private static Vector2 GraphOffset = new Vector2(0.1f, 0.5f);

		private static Vector2 GraphSize = new Vector2(0.8f, -0.3f);

		private static void SaveSnapshot()
		{
		}

		private static MyMemoryLogs.MyMemoryEvent GetEventFromCursor(Vector2 screenPosition)
		{
			Vector2 vector = screenPosition;
			for (int i = 0; i < m_events.Count; i++)
			{
				float num = (float)(m_events[i].StartTime - m_events[0].StartTime).TotalSeconds;
				float num2 = (float)(m_events[i].EndTime - m_events[0].StartTime).TotalSeconds;
				if (vector.X >= num && vector.X <= num2 && vector.Y >= 0f && vector.Y <= m_events[i].ProcessEndSize)
				{
					return m_events[i];
				}
			}
			return null;
		}

		public static void Draw()
		{
			if (!m_initialized)
			{
				m_managed = MyMemoryLogs.GetManaged();
				m_native = MyMemoryLogs.GetNative();
				m_timed = MyMemoryLogs.GetTimed();
				m_events = MyMemoryLogs.GetEvents();
				m_initialized = true;
			}
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			if (m_events.Count > 0)
			{
				num = (float)(m_events[m_events.Count - 1].EndTime - m_events[0].StartTime).TotalSeconds;
			}
			for (int i = 0; i < m_events.Count; i++)
			{
				num3 += m_events[i].ProcessDelta;
				num2 = Math.Max(num2, m_events[i].ProcessStartSize);
				num2 = Math.Max(num2, m_events[i].ProcessEndSize);
			}
			MyMemoryLogs.MyMemoryEvent eventFromCursor = GetEventFromCursor((MyGuiSandbox.MouseCursorPosition - GraphOffset) * new Vector2(GraphSize.X, GraphSize.Y) * new Vector2(num, num2));
			if (eventFromCursor != null)
			{
				new StringBuilder(100).Append(eventFromCursor.Name);
			}
			float num4 = (num2 > 0f) ? (1f / num2) : 0f;
			float num5 = (num > 0f) ? (1f / num) : 0f;
			int num6 = 0;
			foreach (MyMemoryLogs.MyMemoryEvent @event in m_events)
			{
				float num7 = (float)(@event.StartTime - m_events[0].StartTime).TotalSeconds;
				_ = (float)(@event.EndTime - m_events[0].StartTime).TotalSeconds;
				_ = @event.ManagedStartSize;
				_ = @event.ManagedEndSize;
				_ = @event.ProcessStartSize;
				_ = @event.ProcessEndSize;
				if (num6 % 2 != 1)
				{
					_ = Color.LightGreen;
				}
				else
				{
					_ = Color.Green;
				}
				if (num6++ % 2 != 1)
				{
					_ = Color.LightBlue;
				}
				else
				{
					_ = Color.Blue;
				}
				if (@event == eventFromCursor)
				{
					_ = Color.Yellow;
					_ = Color.Orange;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			Vector2 vector = new Vector2(100f, 500f);
			for (int j = 0; j < 50 && j < m_native.Count; j++)
			{
				stringBuilder.Clear();
				stringBuilder.Append(m_native[j].Name);
				stringBuilder.Append((9.536743E-07f * m_native[j].ManagedDelta).ToString("GC: 0.0 MB "));
				stringBuilder.Clear();
				stringBuilder.Append((9.536743E-07f * m_native[j].ProcessDelta).ToString("Process: 0.0 MB "));
				vector.Y += 13f;
			}
			vector = new Vector2(1000f, 500f);
			vector.Y += 10f;
			for (int k = 0; k < 50 && k < m_timed.Count; k++)
			{
				stringBuilder.Clear();
				stringBuilder.Append(m_native[k].Name);
				stringBuilder.Append(m_timed[k].DeltaTime.ToString(" 0.000 s"));
				vector.Y += 13f;
			}
		}
	}
}
