using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage.Library.Utils;

namespace VRage.Stats
{
	public class MyStats
	{
		public enum SortEnum
		{
			None,
			Name,
			Priority
		}

		public volatile SortEnum Sort = SortEnum.Priority;

		private static Comparer<KeyValuePair<string, MyStat>> m_nameComparer = new MyNameComparer();

		private static Comparer<KeyValuePair<string, MyStat>> m_priorityComparer = new MyPriorityComparer();

		private MyGameTimer m_timer = new MyGameTimer();

		private NumberFormatInfo m_format = new NumberFormatInfo
		{
			NumberDecimalSeparator = ".",
			NumberGroupSeparator = " "
		};

		private FastResourceLock m_lock = new FastResourceLock();

		private Dictionary<string, MyStat> m_stats = new Dictionary<string, MyStat>(1024);

		private List<KeyValuePair<string, MyStat>> m_tmpWriteList = new List<KeyValuePair<string, MyStat>>(1024);

		private MyStat GetStat(string name)
		{
			MyStat value;
			using (m_lock.AcquireSharedUsing())
			{
				if (m_stats.TryGetValue(name, out value))
				{
					return value;
				}
			}
			using (m_lock.AcquireExclusiveUsing())
			{
				if (m_stats.TryGetValue(name, out value))
				{
					return value;
				}
				value = new MyStat(0);
				m_stats[name] = value;
				return value;
			}
		}

		private MyStat GetStat(MyStatKeys.StatKeysEnum key)
		{
			MyStatKeys.GetNameAndPriority(key, out string name, out int priority);
			MyStat value;
			using (m_lock.AcquireSharedUsing())
			{
				if (m_stats.TryGetValue(name, out value))
				{
					return value;
				}
			}
			using (m_lock.AcquireExclusiveUsing())
			{
				if (m_stats.TryGetValue(name, out value))
				{
					return value;
				}
				value = new MyStat(priority);
				m_stats[name] = value;
				return value;
			}
		}

		/// <summary>
		/// Clears all stats (doesn't remove them)
		/// </summary>
		public void Clear()
		{
			using (m_lock.AcquireSharedUsing())
			{
				foreach (KeyValuePair<string, MyStat> stat in m_stats)
				{
					stat.Value.Clear();
				}
			}
		}

		/// <summary>
		/// Removes all stats
		/// </summary>
		public void RemoveAll()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_stats.Clear();
			}
		}

		/// <summary>
		/// Remove a stat
		/// </summary>
		public void Remove(string name)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_stats.Remove(name);
			}
		}

		public void Clear(string name)
		{
			GetStat(name).Clear();
		}

		/// <summary>
		/// Increments an internal counter with given name and sets it to refresh after given time has passed.
		/// </summary>
		public void Increment(string name, int refreshMs = 0, int clearRateMs = -1)
		{
			Write(name, 0L, MyStatTypeEnum.Counter, refreshMs, 0, clearRateMs);
		}

		public void Increment(MyStatKeys.StatKeysEnum key, int refreshMs = 0, int clearRateMs = -1)
		{
			Write(key, 0f, MyStatTypeEnum.Counter, refreshMs, 0, clearRateMs);
		}

		public MyStatToken Measure(string name, MyStatTypeEnum type, int refreshMs = 200, int numDecimals = 1, int clearRateMs = -1)
		{
			MyStat stat = GetStat(name);
			if (stat.DrawText == null)
			{
				stat.DrawText = GetMeasureText(name, type);
			}
			stat.ChangeSettings((type | MyStatTypeEnum.FormatFlag) & (MyStatTypeEnum)191, refreshMs, numDecimals, clearRateMs);
			return new MyStatToken(m_timer, stat);
		}

		public MyStatToken Measure(string name)
		{
			return Measure(name, MyStatTypeEnum.Avg);
		}

		private string GetMeasureText(string name, MyStatTypeEnum type)
		{
			switch (type & (MyStatTypeEnum)15)
			{
			case MyStatTypeEnum.Counter:
				return name + ": {0}x";
			case MyStatTypeEnum.CounterSum:
				return name + ": {0}x / {1}ms";
			case MyStatTypeEnum.MinMax:
				return name + ": {0}ms / {1}ms";
			case MyStatTypeEnum.MinMaxAvg:
				return name + ": {0}ms / {1}ms / {2}ms";
			default:
				return name + ": {0}ms";
			}
		}

		/// <summary>
		/// Write stat, colon and space is added automatically
		/// </summary>
		public void Write(string name, float value, MyStatTypeEnum type, int refreshMs, int numDecimals, int clearRateMs = -1)
		{
			GetStat(name).Write(value, type, refreshMs, numDecimals, clearRateMs);
		}

		public void Write(MyStatKeys.StatKeysEnum key, float value, MyStatTypeEnum type, int refreshMs, int numDecimals, int clearRateMs = -1)
		{
			GetStat(key).Write(value, type, refreshMs, numDecimals, clearRateMs);
		}

		/// <summary>
		/// Write stat, colon and space is added automatically
		/// </summary>
		public void Write(string name, long value, MyStatTypeEnum type, int refreshMs, int numDecimals, int clearRateMs = -1)
		{
			GetStat(name).Write(value, type, refreshMs, numDecimals, clearRateMs);
		}

		/// <summary>
		/// Write stat using format string
		/// Number of arguments in format string:
		/// MinMaxAvg - three
		/// MinMax - two
		/// Other - one
		/// </summary>
		public void WriteFormat(string name, float value, MyStatTypeEnum type, int refreshMs, int numDecimals, int clearRateMs = -1)
		{
			GetStat(name).Write(value, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs);
		}

		public void WriteFormat(string name, float value1, float value2, MyStatTypeEnum type, int refreshMs, int numDecimals, int clearRateMs = -1)
		{
			GetStat(name).Write(value1, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs, value2);
		}

		public void WriteFormat(MyStatKeys.StatKeysEnum key, float value, MyStatTypeEnum type, int refreshMs, int numDecimals, int clearRateMs = -1)
		{
			GetStat(key).Write(value, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs);
		}

		public void WriteFormat(MyStatKeys.StatKeysEnum key, float value1, float value2, MyStatTypeEnum type, int refreshMs, int numDecimals, int clearRateMs = -1)
		{
			GetStat(key).Write(value1, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs, value2);
		}

		/// <summary>
		/// Write stat using format string
		/// Number of arguments in format string:
		/// MinMaxAvg - three
		/// MinMax - two
		/// Other - one
		/// </summary>
		public void WriteFormat(string name, long value, MyStatTypeEnum type, int refreshMs, int numDecimals, int clearRateMs = -1)
		{
			GetStat(name).Write(value, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs);
		}

		public void WriteTo(StringBuilder writeTo)
		{
			lock (m_tmpWriteList)
			{
				try
				{
					using (m_lock.AcquireSharedUsing())
					{
						foreach (KeyValuePair<string, MyStat> stat in m_stats)
						{
							m_tmpWriteList.Add(stat);
						}
					}
					switch (Sort)
					{
					case SortEnum.Name:
						m_tmpWriteList.Sort(m_nameComparer);
						break;
					case SortEnum.Priority:
						m_tmpWriteList.Sort(m_priorityComparer);
						break;
					}
					foreach (KeyValuePair<string, MyStat> tmpWrite in m_tmpWriteList)
					{
						AppendStat(writeTo, tmpWrite.Key, tmpWrite.Value);
					}
				}
				finally
				{
					m_tmpWriteList.Clear();
				}
			}
		}

		private void AppendStatLine<A, B, C, D>(StringBuilder text, string statName, A arg0, B arg1, C arg2, D arg3, NumberFormatInfo format, string formatString) where A : IConvertible where B : IConvertible where C : IConvertible where D : IConvertible
		{
			if (formatString == null)
			{
				text.ConcatFormat(statName, arg0, arg1, arg2, arg3, format);
			}
			else
			{
				text.ConcatFormat(formatString, statName, arg0, arg1, arg2, arg3, format);
			}
			text.AppendLine();
		}

		private MyTimeSpan RequiredInactivity(MyStatTypeEnum type)
		{
			if ((type & MyStatTypeEnum.DontDisappearFlag) == MyStatTypeEnum.DontDisappearFlag)
			{
				return MyTimeSpan.MaxValue;
			}
			if ((type & MyStatTypeEnum.KeepInactiveLongerFlag) == MyStatTypeEnum.KeepInactiveLongerFlag)
			{
				return MyTimeSpan.FromSeconds(30.0);
			}
			return MyTimeSpan.FromSeconds(3.0);
		}

		private void AppendStat(StringBuilder text, string statKey, MyStat stat)
		{
			stat.ReadAndClear(m_timer.Elapsed, out MyStat.Value sum, out int count, out MyStat.Value min, out MyStat.Value max, out MyStat.Value last, out MyStatTypeEnum type, out int decimals, out MyTimeSpan inactivityMs, out MyStat.Value last2);
			if (inactivityMs > RequiredInactivity(type))
			{
				Remove(statKey);
				return;
			}
			string statName = stat.DrawText ?? statKey;
			bool flag = (type & MyStatTypeEnum.LongFlag) == MyStatTypeEnum.LongFlag;
			float num = (float)((flag ? ((double)sum.AsLong) : ((double)sum.AsFloat)) / (double)count);
			m_format.NumberDecimalDigits = decimals;
			m_format.NumberGroupSeparator = ((decimals == 0) ? "," : string.Empty);
			bool flag2 = (type & MyStatTypeEnum.FormatFlag) == MyStatTypeEnum.FormatFlag;
			switch (type & (MyStatTypeEnum)15)
			{
			case MyStatTypeEnum.Avg:
				AppendStatLine(text, statName, num, 0, 0, 0, m_format, flag2 ? null : "{0}: {1}");
				break;
			case MyStatTypeEnum.Counter:
				AppendStatLine(text, statName, count, 0, 0, 0, m_format, flag2 ? null : "{0}: {1}");
				break;
			case MyStatTypeEnum.CurrentValue:
				if (flag)
				{
					AppendStatLine(text, statName, last.AsLong, 0, 0, last2.AsLong, m_format, flag2 ? null : "{0}: {1}");
				}
				else
				{
					AppendStatLine(text, statName, last.AsFloat, 0, 0, last2.AsFloat, m_format, flag2 ? null : "{0}: {1}");
				}
				break;
			case MyStatTypeEnum.Max:
				if (flag)
				{
					AppendStatLine(text, statName, max.AsLong, 0, 0, 0, m_format, flag2 ? null : "{0}: {1}");
				}
				else
				{
					AppendStatLine(text, statName, max.AsFloat, 0, 0, 0, m_format, flag2 ? null : "{0}: {1}");
				}
				break;
			case MyStatTypeEnum.Min:
				if (flag)
				{
					AppendStatLine(text, statName, min.AsLong, 0, 0, 0, m_format, flag2 ? null : "{0}: {1}");
				}
				else
				{
					AppendStatLine(text, statName, min.AsFloat, 0, 0, 0, m_format, flag2 ? null : "{0}: {1}");
				}
				break;
			case MyStatTypeEnum.MinMax:
				if (flag)
				{
					AppendStatLine(text, statName, min.AsLong, max.AsLong, 0, 0, m_format, flag2 ? null : "{0}: {1} / {2}");
				}
				else
				{
					AppendStatLine(text, statName, min.AsFloat, max.AsFloat, 0, 0, m_format, flag2 ? null : "{0}: {1} / {2}");
				}
				break;
			case MyStatTypeEnum.MinMaxAvg:
				if (flag)
				{
					AppendStatLine(text, statName, min.AsLong, max.AsLong, num, 0, m_format, flag2 ? null : "{0}: {1} / {2} / {3}");
				}
				else
				{
					AppendStatLine(text, statName, min.AsFloat, max.AsFloat, num, 0, m_format, flag2 ? null : "{0}: {1} / {2} / {3}");
				}
				break;
			case MyStatTypeEnum.Sum:
				if (flag)
				{
					AppendStatLine(text, statName, sum.AsLong, 0, 0, 0, m_format, flag2 ? null : "{0}: {1}");
				}
				else
				{
					AppendStatLine(text, statName, sum.AsFloat, 0, 0, 0, m_format, flag2 ? null : "{0}: {1}");
				}
				break;
			case MyStatTypeEnum.CounterSum:
				if (flag)
				{
					AppendStatLine(text, statName, count, sum.AsLong, 0, 0, m_format, flag2 ? null : "{0}: {1} / {2}");
				}
				else
				{
					AppendStatLine(text, statName, count, sum.AsFloat, 0, 0, m_format, flag2 ? null : "{0}: {1} / {2}");
				}
				break;
			}
		}
	}
}
