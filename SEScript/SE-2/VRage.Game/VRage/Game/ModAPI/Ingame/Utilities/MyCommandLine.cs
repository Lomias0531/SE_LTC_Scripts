using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	/// <summary>
	/// A utility class to parse arguments from a command line string. Switches are defined with hyphen (-switch). Quotes can be
	/// placed around an argument to parse verbatim.
	/// </summary>
	public class MyCommandLine
	{
		/// <summary>
		/// A collection of arguments
		/// </summary>
		public class ItemCollection : IReadOnlyList<string>, IEnumerable<string>, IEnumerable, IReadOnlyCollection<string>
		{
			private readonly List<StringSegment> m_items;

			/// <summary>
			/// Gets the number of parameters
			/// </summary>
			public int Count => m_items.Count;

			/// <summary>
			/// Gets the parameter at the given index
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public string this[int index]
			{
				get
				{
					if (index < 0 || index >= m_items.Count)
					{
						return null;
					}
					return m_items[index].ToString();
				}
			}

			internal ItemCollection(List<StringSegment> items)
			{
				m_items = items;
			}

			/// <summary>
			/// Gets an enumerator to step through the content of this list
			/// </summary>
			/// <returns></returns>
			public Enumerator GetEnumerator()
			{
				return new Enumerator(m_items.GetEnumerator());
			}

			/// <internalonly />
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				return new Enumerator(m_items.GetEnumerator());
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(m_items.GetEnumerator());
			}
		}

		/// <summary>
		/// A collection of set switches
		/// </summary>
		public class SwitchCollection : IReadOnlyCollection<string>, IEnumerable<string>, IEnumerable
		{
			private readonly Dictionary<StringSegment, int> m_switches;

			/// <summary>
			/// Returns the number of set switches
			/// </summary>
			public int Count => m_switches.Count;

			internal SwitchCollection(Dictionary<StringSegment, int> switches)
			{
				m_switches = switches;
			}

			/// <summary>
			/// Gets an enumerator to step through the content of this list
			/// </summary>
			/// <returns></returns>
			public SwitchEnumerator GetEnumerator()
			{
				return new SwitchEnumerator(m_switches.Keys.GetEnumerator());
			}

			/// <internalonly />
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				return new SwitchEnumerator(m_switches.Keys.GetEnumerator());
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SwitchEnumerator(m_switches.Keys.GetEnumerator());
			}
		}

		[Serializable]
		public struct SwitchEnumerator : IEnumerator<string>, IEnumerator, IDisposable
		{
			protected class VRage_Game_ModAPI_Ingame_Utilities_MyCommandLine_003C_003ESwitchEnumerator_003C_003Em_enumerator_003C_003EAccessor : IMemberAccessor<SwitchEnumerator, Dictionary<StringSegment, int>.KeyCollection.Enumerator>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SwitchEnumerator owner, in Dictionary<StringSegment, int>.KeyCollection.Enumerator value)
				{
					owner.m_enumerator = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SwitchEnumerator owner, out Dictionary<StringSegment, int>.KeyCollection.Enumerator value)
				{
					value = owner.m_enumerator;
				}
			}

			private Dictionary<StringSegment, int>.KeyCollection.Enumerator m_enumerator;

			public string Current => m_enumerator.Current.ToString();

			object IEnumerator.Current => Current;

			internal SwitchEnumerator(Dictionary<StringSegment, int>.KeyCollection.Enumerator enumerator)
			{
				m_enumerator = enumerator;
			}

			public void Dispose()
			{
				m_enumerator.Dispose();
			}

			public bool MoveNext()
			{
				return m_enumerator.MoveNext();
			}

			void IEnumerator.Reset()
			{
				((IEnumerator)m_enumerator).Reset();
			}
		}

		[Serializable]
		public struct Enumerator : IEnumerator<string>, IEnumerator, IDisposable
		{
			protected class VRage_Game_ModAPI_Ingame_Utilities_MyCommandLine_003C_003EEnumerator_003C_003Em_enumerator_003C_003EAccessor : IMemberAccessor<Enumerator, List<StringSegment>.Enumerator>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Enumerator owner, in List<StringSegment>.Enumerator value)
				{
					owner.m_enumerator = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Enumerator owner, out List<StringSegment>.Enumerator value)
				{
					value = owner.m_enumerator;
				}
			}

			private List<StringSegment>.Enumerator m_enumerator;

			public string Current => m_enumerator.Current.ToString();

			object IEnumerator.Current => Current;

			internal Enumerator(List<StringSegment>.Enumerator enumerator)
			{
				m_enumerator = enumerator;
			}

			public void Dispose()
			{
				m_enumerator.Dispose();
			}

			public bool MoveNext()
			{
				return m_enumerator.MoveNext();
			}

			void IEnumerator.Reset()
			{
				((IEnumerator)m_enumerator).Reset();
			}
		}

		private readonly List<StringSegment> m_items = new List<StringSegment>();

		private readonly Dictionary<StringSegment, int> m_switchIndexes = new Dictionary<StringSegment, int>(StringSegmentIgnoreCaseComparer.DEFAULT);

		private readonly List<int> m_argumentIndexes = new List<int>();

		/// <summary>
		/// Contains all items, both arguments and switches
		/// </summary>
		public ItemCollection Items
		{
			get;
			private set;
		}

		/// <summary>
		/// Contains a list of all detected switches
		/// </summary>
		public SwitchCollection Switches
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns the number of non-switch arguments
		/// </summary>
		public int ArgumentCount => m_argumentIndexes.Count;

		/// <summary>
		/// Creates a new instance of <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyCommandLine" />
		/// </summary>
		public MyCommandLine()
		{
			Items = new ItemCollection(m_items);
			Switches = new SwitchCollection(m_switchIndexes);
		}

		/// <summary>
		///     Attempts to parse the given string as a command line
		/// </summary>
		/// <param name="argument"></param>
		/// <returns><c>true</c> if there were arguments in the string, <c>false</c> otherwise</returns>
		public bool TryParse(string argument)
		{
			Clear();
			if (string.IsNullOrEmpty(argument))
			{
				return false;
			}
			TextPtr ptr = new TextPtr(argument);
			while (true)
			{
				ptr = ptr.SkipWhitespace();
				if (ptr.IsOutOfBounds())
				{
					break;
				}
				if (!TryParseSwitch(ref ptr))
				{
					ParseParameter(ref ptr);
				}
			}
			return Items.Count > 0;
		}

		/// <summary>
		/// Returns the argument at the given index. Switches are not counted.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string Argument(int index)
		{
			if (index < 0 || index >= m_argumentIndexes.Count)
			{
				return null;
			}
			return Items[m_argumentIndexes[index]];
		}

		/// <summary>
		/// Determines whether the given switch is set. Switches are specified without their prefixed hyphen.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool Switch(string name)
		{
			return m_switchIndexes.ContainsKey(new StringSegment(name));
		}

		/// <summary>
		/// <para>
		/// Gets an argument of a switch.
		/// For example, using <c>Switch("key", 0)</c> on the command line <c>someOtherArgument -key value</c> will return <c>value</c>.
		/// </para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="relativeArgument"></param>
		/// <returns></returns>
		public string Switch(string name, int relativeArgument)
		{
			StringSegment key = (name.Length > 0 && name[0] == '-') ? new StringSegment(name, 1, name.Length - 1) : new StringSegment(name);
			if (!m_switchIndexes.TryGetValue(key, out int value))
			{
				return null;
			}
			relativeArgument += 1 + value;
			for (int num = relativeArgument; num > value; num--)
			{
				if (!m_argumentIndexes.Contains(num))
				{
					return null;
				}
			}
			return m_items[relativeArgument].ToString();
		}

		/// <summary>
		///     Clears all arguments
		/// </summary>
		public void Clear()
		{
			m_items.Clear();
			m_switchIndexes.Clear();
			m_argumentIndexes.Clear();
		}

		private bool TryParseSwitch(ref TextPtr ptr)
		{
			if (ptr.Char != '-')
			{
				return false;
			}
			StringSegment item = ParseQuoted(ref ptr);
			int count = Items.Count;
			m_items.Add(item);
			m_switchIndexes.Add(new StringSegment(item.Text, item.Start + 1, item.Length - 1), count);
			return true;
		}

		private void ParseParameter(ref TextPtr ptr)
		{
			StringSegment item = ParseQuoted(ref ptr);
			int count = Items.Count;
			m_items.Add(item);
			m_argumentIndexes.Add(count);
		}

		private StringSegment ParseQuoted(ref TextPtr ptr)
		{
			TextPtr textPtr = ptr;
			bool flag = textPtr.Char == '"';
			if (flag)
			{
				++textPtr;
			}
			TextPtr textPtr2 = textPtr;
			TextPtr textPtr3;
			while (!textPtr2.IsOutOfBounds())
			{
				if (textPtr2.Char == '"')
				{
					flag = !flag;
				}
				if (!flag && char.IsWhiteSpace(textPtr2.Char))
				{
					ptr = textPtr2;
					textPtr3 = textPtr2 - 1;
					if (textPtr3.Char == '"')
					{
						textPtr2 = textPtr3;
					}
					return new StringSegment(textPtr.Content, textPtr.Index, textPtr2.Index - textPtr.Index);
				}
				++textPtr2;
			}
			textPtr2 = (ptr = new TextPtr(ptr.Content, ptr.Content.Length));
			textPtr3 = textPtr2 - 1;
			if (textPtr3.Char == '"')
			{
				textPtr2 = textPtr3;
			}
			return new StringSegment(textPtr.Content, textPtr.Index, textPtr2.Index - textPtr.Index);
		}
	}
}
