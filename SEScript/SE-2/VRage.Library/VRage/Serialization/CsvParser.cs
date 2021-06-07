using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VRage.Serialization
{
	public static class CsvParser
	{
		private static Tuple<T, IEnumerable<T>> HeadAndTail<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IEnumerator<T> enumerator = source.GetEnumerator();
			enumerator.MoveNext();
			return Tuple.Create(enumerator.Current, EnumerateTail(enumerator));
		}

		private static IEnumerable<T> EnumerateTail<T>(IEnumerator<T> en)
		{
			while (en.MoveNext())
			{
				yield return en.Current;
			}
		}

		public static IEnumerable<IList<string>> Parse(string content, char delimiter, char qualifier)
		{
			using (StringReader reader = new StringReader(content))
			{
				return Parse(reader, delimiter, qualifier);
			}
		}

		public static Tuple<IList<string>, IEnumerable<IList<string>>> ParseHeadAndTail(TextReader reader, char delimiter, char qualifier)
		{
			return Parse(reader, delimiter, qualifier).HeadAndTail();
		}

		public static IEnumerable<IList<string>> Parse(TextReader reader, char delimiter, char qualifier)
		{
			bool inQuote = false;
			List<string> record = new List<string>();
			StringBuilder sb = new StringBuilder();
			while (reader.Peek() != -1)
			{
				char c = (char)reader.Read();
				if (c == '\n' || (c == '\r' && (ushort)reader.Peek() == 10))
				{
					if (c == '\r')
					{
						reader.Read();
					}
					if (inQuote)
					{
						if (c == '\r')
						{
							sb.Append('\r');
						}
						sb.Append('\n');
						continue;
					}
					if (record.Count > 0 || sb.Length > 0)
					{
						record.Add(sb.ToString());
						sb.Clear();
					}
					if (record.Count > 0)
					{
						yield return record;
					}
					record = new List<string>(record.Count);
				}
				else if (sb.Length == 0 && !inQuote)
				{
					if (c == qualifier)
					{
						inQuote = true;
					}
					else if (c == delimiter)
					{
						record.Add(sb.ToString());
						sb.Clear();
					}
					else if (!char.IsWhiteSpace(c))
					{
						sb.Append(c);
					}
				}
				else if (c == delimiter)
				{
					if (inQuote)
					{
						sb.Append(delimiter);
						continue;
					}
					record.Add(sb.ToString());
					sb.Clear();
				}
				else if (c == qualifier)
				{
					if (inQuote)
					{
						if ((ushort)reader.Peek() == qualifier)
						{
							reader.Read();
							sb.Append(qualifier);
						}
						else
						{
							inQuote = false;
						}
					}
					else
					{
						sb.Append(c);
					}
				}
				else
				{
					sb.Append(c);
				}
			}
			if (record.Count > 0 || sb.Length > 0)
			{
				record.Add(sb.ToString());
			}
			if (record.Count > 0)
			{
				yield return record;
			}
		}
	}
}
