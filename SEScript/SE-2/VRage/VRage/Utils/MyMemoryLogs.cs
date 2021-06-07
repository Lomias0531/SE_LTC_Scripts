using System;
using System.Collections.Generic;

namespace VRage.Utils
{
	public class MyMemoryLogs
	{
		public class MyMemoryEvent
		{
			public string Name;

			public bool HasStart;

			public bool HasEnd;

			public float ManagedStartSize;

			public float ManagedEndSize;

			public float ProcessStartSize;

			public float ProcessEndSize;

			public float DeltaTime;

			public int Id;

			public bool Selected;

			public DateTime StartTime;

			public DateTime EndTime;

			private List<MyMemoryEvent> m_childs = new List<MyMemoryEvent>();

			public float ManagedDelta => ManagedEndSize - ManagedStartSize;

			public float ProcessDelta => ProcessEndSize - ProcessStartSize;
		}

		private class MyManagedComparer : IComparer<MyMemoryEvent>
		{
			public int Compare(MyMemoryEvent x, MyMemoryEvent y)
			{
				return -1 * x.ManagedDelta.CompareTo(y.ManagedDelta);
			}
		}

		private class MyNativeComparer : IComparer<MyMemoryEvent>
		{
			public int Compare(MyMemoryEvent x, MyMemoryEvent y)
			{
				return -1 * x.ProcessDelta.CompareTo(y.ProcessDelta);
			}
		}

		private class MyTimedeltaComparer : IComparer<MyMemoryEvent>
		{
			public int Compare(MyMemoryEvent x, MyMemoryEvent y)
			{
				return -1 * x.DeltaTime.CompareTo(y.DeltaTime);
			}
		}

		private static MyManagedComparer m_managedComparer = new MyManagedComparer();

		private static MyNativeComparer m_nativeComparer = new MyNativeComparer();

		private static MyTimedeltaComparer m_timeComparer = new MyTimedeltaComparer();

		private static List<MyMemoryEvent> m_events = new List<MyMemoryEvent>();

		private static List<string> m_consoleLogSTART = new List<string>();

		private static List<string> m_consoleLogEND = new List<string>();

		private static Stack<MyMemoryEvent> m_stack = new Stack<MyMemoryEvent>();

		private static int IdCounter = 1;

		public static List<MyMemoryEvent> GetManaged()
		{
			List<MyMemoryEvent> list = new List<MyMemoryEvent>(m_events);
			list.Sort(m_managedComparer);
			return list;
		}

		public static List<MyMemoryEvent> GetNative()
		{
			List<MyMemoryEvent> list = new List<MyMemoryEvent>(m_events);
			list.Sort(m_nativeComparer);
			return list;
		}

		public static List<MyMemoryEvent> GetTimed()
		{
			List<MyMemoryEvent> list = new List<MyMemoryEvent>(m_events);
			list.Sort(m_timeComparer);
			return list;
		}

		public static List<MyMemoryEvent> GetEvents()
		{
			return m_events;
		}

		public static void StartEvent()
		{
			MyMemoryEvent myMemoryEvent = new MyMemoryEvent();
			if (m_consoleLogSTART.Count > 0)
			{
				myMemoryEvent.Name = m_consoleLogSTART[m_consoleLogSTART.Count - 1];
				myMemoryEvent.Id = IdCounter++;
				myMemoryEvent.StartTime = DateTime.Now;
				m_consoleLogSTART.Clear();
				m_stack.Push(myMemoryEvent);
			}
		}

		public static void EndEvent(MyMemoryEvent ev)
		{
			if (m_stack.Count > 0)
			{
				MyMemoryEvent myMemoryEvent = m_stack.Peek();
				ev.Name = myMemoryEvent.Name;
				ev.Id = myMemoryEvent.Id;
				ev.StartTime = myMemoryEvent.StartTime;
				ev.EndTime = DateTime.Now;
				m_events.Add(ev);
				m_stack.Pop();
			}
		}

		public static void AddConsoleLine(string line)
		{
			if (line.EndsWith("START"))
			{
				m_consoleLogEND.Clear();
				line = line.Substring(0, line.Length - 5);
				if (m_stack.Count > 0 && m_stack.Peek().HasStart)
				{
					m_events[m_events.Count].HasStart = true;
					m_events[m_events.Count].Name = line;
				}
				else
				{
					m_consoleLogSTART.Add(line);
				}
			}
			else if (line.EndsWith("END"))
			{
				line = line.Substring(0, line.Length - 5);
				m_consoleLogEND.Add(line);
				m_consoleLogSTART.Clear();
			}
		}

		public static void DumpMemoryUsage()
		{
			m_events.Sort(m_managedComparer);
			MyLog.Default.WriteLine("\n\n");
			MyLog.Default.WriteLine("Managed MemoryUsage: \n");
			float num = 0f;
			for (int i = 0; i < m_events.Count && i < 30; i++)
			{
				float num2 = m_events[i].ManagedDelta * 1f / 1048576f;
				num += num2;
				MyLog.Default.WriteLine(m_events[i].Name + num2);
			}
			MyLog.Default.WriteLine("Total Managed MemoryUsage: " + num + " [MB]");
			m_events.Sort(m_nativeComparer);
			MyLog.Default.WriteLine("\n\n");
			MyLog.Default.WriteLine("Process MemoryUsage: \n");
			num = 0f;
			for (int j = 0; j < m_events.Count && j < 30; j++)
			{
				float num3 = m_events[j].ProcessDelta * 1f / 1048576f;
				num += num3;
				MyLog.Default.WriteLine(m_events[j].Name + num3);
			}
			MyLog.Default.WriteLine("Total Process MemoryUsage: " + num + " [MB]");
			m_events.Sort(m_timeComparer);
			MyLog.Default.WriteLine("\n\n");
			MyLog.Default.WriteLine("Load time comparison: \n");
			float num4 = 0f;
			for (int k = 0; k < m_events.Count && k < 30; k++)
			{
				float deltaTime = m_events[k].DeltaTime;
				num4 += deltaTime;
				MyLog.Default.WriteLine(m_events[k].Name + " " + deltaTime);
			}
			MyLog.Default.WriteLine("Total load time: " + num4 + " [s]");
		}
	}
}
