using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	/// <summary>
	///     <para>
	///         A configuration class to parse and create a text string resembling the old fashioned INI format, but with
	///         support for multiline
	///         values.
	///     </para>
	///     <para>
	///         Do not forget that parsing is a time-consuming task. Keep your parsing to a minimum.
	///     </para>
	/// </summary>
	/// <example>
	///     <para>
	///         Using MyIni to deal with CustomData end-user configuration:
	///     </para>
	///     <para>
	///         The CustomData:
	///     </para>
	///     <code>
	/// [kernel]
	/// output=DebugTextPanel
	/// bootText=
	/// |-- HAL9000 --
	/// |Good morning, Dave.
	/// </code>
	///     <para>
	///         The code:
	///     </para>
	///     <code>
	/// MyIni _ini = new MyIni();
	/// IMyTextPanel _outputTextPanel;
	///
	/// public Program() 
	/// {
	///     MyIniParseResult result;
	///     if (!_ini.TryParse(Me.CustomData, out result) 
	///     {
	///         Echo($"CustomData error:\nLine {result}");
	///     }
	///
	///     // Get the kernel section's output value. If this value is set, the system attempts
	///     // to retrieve a text panel with the value set. Otherwise output is ignored.
	///     var name = _ini.Get("kernel", "output").ToString();
	///     if (name != null) 
	///     {
	///         _outputTextPanel = GridTerminalSystem.GetBlockWithName&lt;IMyTextPanel&gt;(name);
	///         if (_outputTextPanel == null)
	///             Echo($"No text panel named {name}");
	///     }
	///
	///     // Get the kernel section's boottext value. If no value is given, a default value will be returned.
	///     var bootText = _ini.Get("kernel", "bootText").ToString("Kernel is starting up...");
	///     _outputTextPanel?.WritePublicText(bootText);
	/// }
	///
	/// public void Main() {
	///     // Do your stuff. Only parse the configuration when you have to.
	/// }
	/// </code>
	///     <para>
	///         Using MyIni to deal with internal storage:
	///     </para>
	///     <code>
	/// MyIni _storage = new MyIni();
	/// Vector3D _startupPosition;
	/// bool _hasTarget;
	/// Vector3D _currentTarget;
	///
	/// public Program() 
	/// {
	///     // You only need to parse here in the constructor.
	///     if (_ini.TryParse(Storage) 
	///     {
	///         var str = _ini.Get("state", "startupPosition").ToString();
	///         Vector3D.TryParse(str, out _startupPosition);
	///         str = _ini.Get("state", "currentTarget").ToString();
	///         Vector3D.TryParse(str, out _currentTarget);
	///         _hasTarget = _ini.Get("state", "hasTarget").ToBoolean();
	///     } 
	///     else 
	///     {
	///         // Set up defaults, the storage is nonexistent or corrupt
	///         _startupPosition = Me.CubeGrid.Position;
	///     }
	/// }
	///
	/// public void Save()
	/// {
	///     // You only need to update Storage when the Save method is called.
	///     _ini.Set("state", "startupPosition", _startupPosition);
	///     _ini.Set("state", "currentTarget", _currentTarget);
	///     Storage = _ini.ToString();
	/// }
	///
	/// public void Main() {
	///     // Do your stuff
	/// }
	/// </code>
	/// </example>
	/// <remarks>
	///     This class is NOT THREAD SAFE as it's optimized for programmable block use.
	/// </remarks>
	public class MyIni
	{
		private class MyIniKeyComparer : IEqualityComparer<MyIniKey>
		{
			public static readonly MyIniKeyComparer DEFAULT = new MyIniKeyComparer();

			public bool Equals(MyIniKey x, MyIniKey y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(MyIniKey obj)
			{
				return obj.GetHashCode();
			}
		}

		private readonly Dictionary<MyIniKey, StringSegment> m_items = new Dictionary<MyIniKey, StringSegment>(MyIniKeyComparer.DEFAULT);

		private readonly Dictionary<StringSegment, int> m_sections = new Dictionary<StringSegment, int>(StringSegmentIgnoreCaseComparer.DEFAULT);

		private readonly Dictionary<MyIniKey, StringSegment> m_itemComments = new Dictionary<MyIniKey, StringSegment>(MyIniKeyComparer.DEFAULT);

		private readonly Dictionary<StringSegment, StringSegment> m_sectionComments = new Dictionary<StringSegment, StringSegment>(StringSegmentIgnoreCaseComparer.DEFAULT);

		private string m_content;

		private int m_sectionCounter;

		private StringBuilder m_tmpContentBuilder;

		private StringBuilder m_tmpValueBuilder;

		private List<MyIniKey> m_tmpKeyList;

		private List<string> m_tmpStringList;

		private StringSegment m_endComment;

		private StringSegment m_endContent;

		private StringBuilder TmpContentBuilder
		{
			get
			{
				if (m_tmpContentBuilder == null)
				{
					m_tmpContentBuilder = new StringBuilder();
				}
				return m_tmpContentBuilder;
			}
		}

		private StringBuilder TmpValueBuilder
		{
			get
			{
				if (m_tmpValueBuilder == null)
				{
					m_tmpValueBuilder = new StringBuilder();
				}
				return m_tmpValueBuilder;
			}
		}

		private List<MyIniKey> TmpKeyList
		{
			get
			{
				if (m_tmpKeyList == null)
				{
					m_tmpKeyList = new List<MyIniKey>();
				}
				return m_tmpKeyList;
			}
		}

		private List<string> TmpStringList
		{
			get
			{
				if (m_tmpStringList == null)
				{
					m_tmpStringList = new List<string>();
				}
				return m_tmpStringList;
			}
		}

		/// <summary>
		/// You can terminate a configuration stream by entering "---" on a separate line. This property
		/// will contain all the content after this line.
		/// </summary>
		public string EndContent
		{
			get
			{
				return m_endContent.ToString();
			}
			set
			{
				m_endContent = ((value == null) ? default(StringSegment) : new StringSegment(value));
				m_content = null;
			}
		}

		/// <summary>
		/// Get or set a comment to be placed after the last section or item.
		/// Is <c>null</c> if the section does not exist or has no comment.
		/// </summary>
		/// <returns></returns>
		public string EndComment
		{
			get
			{
				StringSegment endCommentSegment = GetEndCommentSegment();
				if (endCommentSegment.IsEmpty)
				{
					return null;
				}
				return endCommentSegment.ToString();
			}
			set
			{
				if (value == null)
				{
					m_endComment = default(StringSegment);
					return;
				}
				m_endComment = new StringSegment(value);
				m_content = null;
			}
		}

		/// <summary>
		///     Determines if the given configuration contains what looks like the given section.
		///     It does not verify that the content is actually in a valid format, just if there's
		///     a line starting with [section].
		/// </summary>
		/// <param name="config"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public static bool HasSection(string config, string section)
		{
			return FindSection(config, section) >= 0;
		}

		private static int FindSection(string config, string section)
		{
			TextPtr ptr = new TextPtr(config);
			if (MatchesSection(ref ptr, section))
			{
				return ptr.Index;
			}
			while (!ptr.IsOutOfBounds())
			{
				ptr = ptr.Find("\n");
				++ptr;
				if (ptr.Char == '[')
				{
					if (MatchesSection(ref ptr, section))
					{
						return ptr.Index;
					}
				}
				else if (ptr.StartsWith("---"))
				{
					ptr = (ptr + 3).SkipWhitespace();
					if (ptr.IsEndOfLine())
					{
						break;
					}
				}
			}
			return -1;
		}

		private static bool MatchesSection(ref TextPtr ptr, string section)
		{
			if (!ptr.StartsWith("["))
			{
				return false;
			}
			TextPtr ptr2 = ptr + 1;
			if (!ptr2.StartsWithCaseInsensitive(section))
			{
				return false;
			}
			if (!(ptr2 + section.Length).StartsWith("]"))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		///     Determines whether a section of a given name exists in the currently parsed configuration.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public bool ContainsSection(string section)
		{
			return m_sections.ContainsKey(new StringSegment(section));
		}

		/// <summary>
		///     Determines whether a configuration key (section/key) exists in the currently parsed configuration.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool ContainsKey(string section, string name)
		{
			return ContainsKey(new MyIniKey(section, name));
		}

		/// <summary>
		///     Determines whether a configuration key (section/key) exists in the currently parsed configuration.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(MyIniKey key)
		{
			return m_items.ContainsKey(key);
		}

		/// <summary>
		///     Fills the provided list with the configuration keys within the given section.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="keys"></param>
		public void GetKeys(string section, List<MyIniKey> keys)
		{
			if (keys != null)
			{
				GetKeys(new StringSegment(section), keys);
			}
		}

		private void GetKeys(StringSegment section, List<MyIniKey> keys)
		{
			if (keys != null)
			{
				keys.Clear();
				foreach (MyIniKey key in m_items.Keys)
				{
					if (key.SectionSegment.EqualsIgnoreCase(section))
					{
						keys.Add(key);
					}
				}
			}
		}

		/// <summary>
		///     Fills the provided list with all configuration keys within the currently parsed configuration.
		/// </summary>
		/// <param name="keys"></param>
		public void GetKeys(List<MyIniKey> keys)
		{
			if (keys != null)
			{
				keys.Clear();
				foreach (MyIniKey key in m_items.Keys)
				{
					keys.Add(key);
				}
			}
		}

		/// <summary>
		///     Fills the provided list with the names of all the sections in the currently parsed configuration.
		/// </summary>
		/// <param name="names"></param>
		public void GetSections(List<string> names)
		{
			if (names != null)
			{
				names.Clear();
				foreach (StringSegment key in m_sections.Keys)
				{
					names.Add(key.ToString());
				}
			}
		}

		private StringSegment GetEndCommentSegment()
		{
			StringSegment comment = m_endComment;
			if (!comment.IsCached)
			{
				RealizeComment(ref comment);
				m_endComment = comment;
			}
			return comment;
		}

		/// <summary>
		/// Sets a comment to be placed after the last section or item. Set the comment to <c>null</c> to remove it.
		/// </summary>
		/// <param name="comment"></param>
		public void SetEndComment(string comment)
		{
			if (comment == null)
			{
				m_endComment = default(StringSegment);
				return;
			}
			m_endComment = new StringSegment(comment);
			m_content = null;
		}

		/// <summary>
		/// Get any comment that might be associated with the given section.
		/// Returns <c>null</c> if the section does not exist or has no comment.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public string GetSectionComment(string section)
		{
			StringSegment key = new StringSegment(section);
			StringSegment sectionCommentSegment = GetSectionCommentSegment(key);
			if (sectionCommentSegment.IsEmpty)
			{
				return null;
			}
			return sectionCommentSegment.ToString();
		}

		private StringSegment GetSectionCommentSegment(StringSegment key)
		{
			if (!m_sectionComments.TryGetValue(key, out StringSegment value))
			{
				return default(StringSegment);
			}
			if (!value.IsCached)
			{
				RealizeComment(ref value);
				m_sectionComments[key] = value;
			}
			return value;
		}

		/// <summary>
		/// Sets a comment on a given section. The section must already exist. Set the comment to <c>null</c> to remove it.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="comment"></param>
		public void SetSectionComment(string section, string comment)
		{
			StringSegment key = new StringSegment(section);
			if (!m_sections.ContainsKey(key))
			{
				throw new ArgumentException("No section named " + section);
			}
			if (comment == null)
			{
				m_sectionComments.Remove(key);
				return;
			}
			StringSegment value = new StringSegment(comment);
			m_sectionComments[key] = value;
			m_content = null;
		}

		/// <summary>
		/// Gets any comment that might be associated with the given key.
		/// Returns <c>null</c> if the key does not exist or has no comment.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetComment(string section, string name)
		{
			return GetComment(new MyIniKey(section, name));
		}

		/// <summary>
		/// Gets any comment that might be associated with the given key.
		/// Returns <c>null</c> if the key does not exist or has no comment.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetComment(MyIniKey key)
		{
			StringSegment commentSegment = GetCommentSegment(key);
			if (commentSegment.IsEmpty)
			{
				return null;
			}
			return commentSegment.ToString();
		}

		private StringSegment GetCommentSegment(MyIniKey key)
		{
			if (!m_itemComments.TryGetValue(key, out StringSegment value))
			{
				return default(StringSegment);
			}
			if (!value.IsCached)
			{
				RealizeComment(ref value);
				m_itemComments[key] = value;
			}
			return value;
		}

		/// <summary>
		/// Sets a comment on a given item. The item must already exist. Set the comment to <c>null</c> to remove it.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="comment"></param>
		public void SetComment(string section, string name, string comment)
		{
			SetComment(new MyIniKey(section, name), comment);
		}

		/// <summary>
		/// Sets a comment on a given item. The item must already exist. Set the comment to <c>null</c> to remove it.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="comment"></param>
		public void SetComment(MyIniKey key, string comment)
		{
			if (!m_items.ContainsKey(key))
			{
				throw new ArgumentException("No item named " + key);
			}
			if (comment == null)
			{
				m_itemComments.Remove(key);
				return;
			}
			StringSegment value = new StringSegment(comment);
			m_itemComments[key] = value;
			m_content = null;
		}

		/// <summary>
		///     Gets the <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniValue" /> of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public MyIniValue Get(string section, string name)
		{
			return Get(new MyIniKey(section, name));
		}

		/// <summary>
		///     Gets the <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniValue" /> of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public MyIniValue Get(MyIniKey key)
		{
			if (m_items.TryGetValue(key, out StringSegment value))
			{
				Realize(ref key, ref value);
				return new MyIniValue(key, value.ToString());
			}
			return MyIniValue.EMPTY;
		}

		private void RealizeComment(ref StringSegment comment)
		{
			if (!comment.IsCached)
			{
				string text = comment.Text;
				TextPtr ptr = new TextPtr(text, comment.Start);
				if (comment.Length > 0)
				{
					StringBuilder tmpValueBuilder = TmpValueBuilder;
					try
					{
						TextPtr ptr2 = ptr + comment.Length;
						bool flag = false;
						while ((int)ptr < (int)ptr2)
						{
							if (flag)
							{
								tmpValueBuilder.Append('\n');
							}
							if (ptr.Char == ';')
							{
								++ptr;
								TextPtr textPtr = ptr.FindEndOfLine();
								int count = textPtr.Index - ptr.Index;
								tmpValueBuilder.Append(ptr.Content, ptr.Index, count);
								ptr = textPtr.SkipWhitespace();
								if (ptr.IsEndOfLine())
								{
									if (ptr.Char == '\r')
									{
										ptr += 2;
									}
									else
									{
										++ptr;
									}
								}
							}
							else
							{
								ptr = ptr.SkipWhitespace();
								if (!ptr.IsEndOfLine())
								{
									break;
								}
								if (ptr.Char == '\r')
								{
									ptr += 2;
								}
								else
								{
									++ptr;
								}
							}
							flag = true;
						}
						comment = new StringSegment(tmpValueBuilder.ToString());
					}
					finally
					{
						tmpValueBuilder.Clear();
					}
				}
			}
		}

		private void Realize(ref MyIniKey key, ref StringSegment value)
		{
			if (!value.IsCached)
			{
				string text = value.Text;
				TextPtr textPtr = new TextPtr(text, value.Start);
				if (value.Length > 0 && textPtr.IsNewLine())
				{
					StringBuilder tmpValueBuilder = TmpValueBuilder;
					try
					{
						textPtr = textPtr.FindEndOfLine(skipNewline: true);
						++textPtr;
						int count = value.Start + value.Length - textPtr.Index;
						tmpValueBuilder.Append(text, textPtr.Index, count);
						tmpValueBuilder.Replace("\n|", "\n");
						m_items[key] = (value = new StringSegment(tmpValueBuilder.ToString()));
					}
					finally
					{
						tmpValueBuilder.Clear();
					}
				}
				else
				{
					m_items[key] = (value = new StringSegment(value.ToString()));
				}
			}
		}

		/// <summary>
		///     Deletes the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		public void Delete(string section, string name)
		{
			Delete(new MyIniKey(section, name));
			m_content = null;
		}

		/// <summary>
		///     Deletes the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		public void Delete(MyIniKey key)
		{
			if (key.IsEmpty)
			{
				throw new ArgumentException("Key cannot be empty", "key");
			}
			m_items.Remove(key);
			m_content = null;
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, string value)
		{
			Set(new MyIniKey(section, name), value);
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, string value)
		{
			if (key.IsEmpty)
			{
				throw new ArgumentException("Key cannot be empty", "key");
			}
			if (value == null)
			{
				Delete(key);
				return;
			}
			StringSegment section = key.SectionSegment;
			AddSection(ref section);
			m_items[key] = new StringSegment(value);
			m_content = null;
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, bool value)
		{
			Set(section, name, value ? "true" : "false");
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, bool value)
		{
			Set(key, value ? "true" : "false");
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, byte value)
		{
			Set(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, byte value)
		{
			Set(key, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, sbyte value)
		{
			Set(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, sbyte value)
		{
			Set(key, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, ushort value)
		{
			Set(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, ushort value)
		{
			Set(key, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, short value)
		{
			Set(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, short value)
		{
			Set(key, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, uint value)
		{
			Set(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, uint value)
		{
			Set(key, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public unsafe void Set(string section, string name, int value)
		{
			Set(section, name, ((int*)(&value))->ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public unsafe void Set(MyIniKey key, int value)
		{
			Set(key, ((int*)(&value))->ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, ulong value)
		{
			Set(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, ulong value)
		{
			Set(key, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, long value)
		{
			Set(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, long value)
		{
			Set(key, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, float value)
		{
			Set(section, name, value.ToString("R", CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, float value)
		{
			Set(key, value.ToString("R", CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, double value)
		{
			Set(section, name, value.ToString("R", CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, double value)
		{
			Set(key, value.ToString("R", CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string section, string name, decimal value)
		{
			Set(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Sets the value of the given configuration key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(MyIniKey key, decimal value)
		{
			Set(key, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Empties this configuration
		/// </summary>
		public void Clear()
		{
			m_items.Clear();
			m_sections.Clear();
			m_content = null;
			m_sectionCounter = 0;
			m_endContent = default(StringSegment);
			if (m_tmpContentBuilder != null)
			{
				m_tmpContentBuilder.Clear();
			}
			if (m_tmpValueBuilder != null)
			{
				m_tmpValueBuilder.Clear();
			}
			if (m_tmpKeyList != null)
			{
				m_tmpKeyList.Clear();
			}
			if (m_tmpStringList != null)
			{
				m_tmpStringList.Clear();
			}
		}

		/// <summary>
		///     Attempts to parse the given content as a configuration file.
		/// </summary>
		/// <param name="content"></param>
		/// <returns><c>true</c> if the parse succeeds, <c>false</c> otherwise</returns>
		public bool TryParse(string content)
		{
			MyIniParseResult result = default(MyIniParseResult);
			return TryParseCore(content, null, ref result);
		}

		/// <summary>
		///     Attempts to parse the given content as a configuration file.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="result">If unsuccessful, this value contains information about why the parse failed</param>
		/// <returns><c>true</c> if the parse succeeds, <c>false</c> otherwise</returns>
		public bool TryParse(string content, out MyIniParseResult result)
		{
			return TryParse(content, null, out result);
		}

		/// <summary>
		///     Attempts to parse the given content as a configuration file. OBSERVE: Use only for read-only operations. 
		///     If you parse a single section and run <see cref="M:VRage.Game.ModAPI.Ingame.Utilities.MyIni.ToString" />, you will only get the parsed section, 
		///     the rest will be discarded. 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="section">The specific section to parse. Any other section in the content will be ignored.</param>
		/// <param name="result">If unsuccessful, this value contains information about why the parse failed</param>
		/// <returns><c>true</c> if the parse succeeds, <c>false</c> otherwise</returns>
		public bool TryParse(string content, string section, out MyIniParseResult result)
		{
			result = new MyIniParseResult(new TextPtr(content), null);
			return TryParseCore(content, section, ref result);
		}

		/// <summary>
		///     Attempts to parse the given content as a configuration file. OBSERVE: Use only for read-only operations. 
		///     If you parse a single section and run <see cref="M:VRage.Game.ModAPI.Ingame.Utilities.MyIni.ToString" />, you will only get the parsed section, 
		///     the rest will be discarded. 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="section">The specific section to parse. Any other section in the content will be ignored.</param>
		/// <returns><c>true</c> if the parse succeeds, <c>false</c> otherwise</returns>
		public bool TryParse(string content, string section)
		{
			MyIniParseResult result = default(MyIniParseResult);
			return TryParseCore(content, section, ref result);
		}

		private bool TryParseCore(string content, string section, ref MyIniParseResult result)
		{
			content = (content ?? "");
			if (string.Equals(m_content, content, StringComparison.Ordinal))
			{
				return true;
			}
			Clear();
			TextPtr ptr = new TextPtr(content);
			if (section != null)
			{
				int num = FindSection(content, section);
				if (num == -1)
				{
					if (result.IsDefined)
					{
						result = new MyIniParseResult(new TextPtr(content), $"Cannot find section \"{section}\"");
					}
					return false;
				}
				ptr += num;
			}
			while (!ptr.IsOutOfBounds())
			{
				if (!TryParseSection(ref ptr, ref result, section == null))
				{
					if (!result.IsDefined || result.Success)
					{
						break;
					}
					return false;
				}
				if (section != null)
				{
					m_content = null;
					return true;
				}
			}
			m_content = content;
			return true;
		}

		/// <summary>
		///     Forces regeneration of the ini content. Only really useful if you want to reformat the configuration file.
		/// </summary>
		public void Invalidate()
		{
			m_content = null;
		}

		private void ReadPrefix(ref TextPtr ptr, out StringSegment prefix)
		{
			bool flag = false;
			TextPtr ptr2 = ptr;
			while (!ptr.IsOutOfBounds())
			{
				if (ptr.IsStartOfLine() && ptr.Char == ';')
				{
					if (!flag)
					{
						flag = true;
						ptr2 = ptr;
					}
					ptr = ptr.FindEndOfLine();
				}
				TextPtr ptr3 = ptr.SkipWhitespace();
				if (!ptr3.IsNewLine())
				{
					break;
				}
				if (ptr3.Char == '\r')
				{
					ptr = ptr3 + 2;
				}
				else
				{
					ptr = ptr3 + 1;
				}
			}
			if (flag)
			{
				TextPtr ptr4 = ptr;
				while (char.IsWhiteSpace(ptr4.Char) && (int)ptr4 > (int)ptr2)
				{
					--ptr4;
				}
				int num = ptr4.Index - ptr2.Index;
				if (num > 0)
				{
					prefix = new StringSegment(ptr.Content, ptr2.Index, num);
					return;
				}
			}
			prefix = default(StringSegment);
		}

		private bool TryParseSection(ref TextPtr ptr, ref MyIniParseResult result, bool parseEndContent)
		{
			TextPtr ptr2 = ptr;
			ReadPrefix(ref ptr2, out StringSegment prefix);
			m_endComment = prefix;
			if (ptr2.Char != '[')
			{
				if (result.IsDefined)
				{
					result = new MyIniParseResult(ptr, "Expected [section] definition");
				}
				return false;
			}
			TextPtr ptr3 = ptr2.FindEndOfLine();
			while (ptr3.Index > ptr2.Index && ptr3.Char != ']')
			{
				--ptr3;
			}
			if (ptr3.Char != ']')
			{
				if (result.IsDefined)
				{
					result = new MyIniParseResult(ptr, "Expected [section] definition");
				}
				return false;
			}
			++ptr2;
			StringSegment segment = new StringSegment(ptr2.Content, ptr2.Index, ptr3.Index - ptr2.Index);
			string text = MyIniKey.ValidateSection(ref segment);
			if (text != null)
			{
				if (result.IsDefined)
				{
					result = new MyIniParseResult(ptr2, $"Section {text}");
				}
				return false;
			}
			ptr2 = (ptr3 + 1).SkipWhitespace();
			if (!ptr2.IsEndOfLine())
			{
				if (result.IsDefined)
				{
					result = new MyIniParseResult(ptr2, "Expected newline");
				}
				return false;
			}
			ptr2 = ptr2.FindEndOfLine(skipNewline: true);
			AddSection(ref segment);
			if (!prefix.IsEmpty)
			{
				m_sectionComments[segment] = prefix;
				m_endComment = default(StringSegment);
			}
			while (TryParseItem(ref segment, ref ptr2, ref result, parseEndContent))
			{
			}
			if (result.IsDefined && !result.Success)
			{
				return false;
			}
			ptr = ptr2;
			return true;
		}

		private void AddSection(ref StringSegment section)
		{
			if (!m_sections.ContainsKey(section))
			{
				m_sections[section] = m_sectionCounter;
				m_sectionCounter++;
			}
		}

		private bool TryParseItem(ref StringSegment section, ref TextPtr ptr, ref MyIniParseResult result, bool parseEndContent)
		{
			TextPtr ptr2 = ptr;
			ReadPrefix(ref ptr2, out StringSegment prefix);
			m_endComment = prefix;
			TextPtr textPtr;
			if (ptr2.StartsWith("---"))
			{
				textPtr = (ptr2 + 3).SkipWhitespace();
				if (textPtr.IsEndOfLine())
				{
					m_endComment = prefix;
					ptr2 = textPtr;
					textPtr = (ptr = new TextPtr(ptr2.Content, ptr2.Content.Length));
					if (parseEndContent)
					{
						ptr2 = ptr2.FindEndOfLine(skipNewline: true);
						m_endContent = new StringSegment(ptr2.Content, ptr2.Index, textPtr.Index - ptr2.Index);
					}
					return false;
				}
			}
			ptr2 = ptr2.TrimStart();
			if (ptr2.IsOutOfBounds() || ptr2.Char == '[')
			{
				return false;
			}
			textPtr = ptr2.FindInLine('=');
			if (textPtr.IsOutOfBounds())
			{
				if (result.IsDefined)
				{
					result = new MyIniParseResult(ptr2, "Expected key=value definition");
				}
				return false;
			}
			StringSegment segment = new StringSegment(ptr2.Content, ptr2.Index, textPtr.TrimEnd().Index - ptr2.Index);
			string text = MyIniKey.ValidateKey(ref segment);
			if (text != null)
			{
				if (result.IsDefined)
				{
					result = new MyIniParseResult(ptr2, $"Key {text}");
				}
				return false;
			}
			ptr2 = (textPtr + 1).TrimStart();
			textPtr = ptr2.FindEndOfLine();
			StringSegment value = new StringSegment(ptr2.Content, ptr2.Index, textPtr.TrimEnd().Index - ptr2.Index);
			if (value.Length == 0)
			{
				TextPtr textPtr2 = textPtr.FindEndOfLine(skipNewline: true);
				if (textPtr2.Char == '|')
				{
					TextPtr textPtr3 = textPtr2;
					do
					{
						textPtr2 = textPtr3.FindEndOfLine();
						textPtr3 = textPtr2.FindEndOfLine(skipNewline: true);
					}
					while (textPtr3.Char == '|');
					textPtr = textPtr2;
				}
				value = new StringSegment(ptr2.Content, ptr2.Index, textPtr.Index - ptr2.Index);
			}
			MyIniKey myIniKey = new MyIniKey(section, segment);
			if (m_items.ContainsKey(myIniKey))
			{
				if (result.IsDefined)
				{
					result = new MyIniParseResult(new TextPtr(segment.Text, segment.Start), $"Duplicate key {myIniKey}");
				}
				return false;
			}
			m_items[myIniKey] = value;
			if (!prefix.IsEmpty)
			{
				m_itemComments[myIniKey] = prefix;
				m_endComment = default(StringSegment);
			}
			ptr = textPtr.FindEndOfLine(skipNewline: true);
			return true;
		}

		/// <summary>
		///     Generates a configuration file from the currently parsed configuration
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (m_content == null)
			{
				m_content = GenerateContent();
			}
			return m_content;
		}

		private static bool NeedsMultilineFormat(ref StringSegment str)
		{
			if (str.Length > 0)
			{
				if (!char.IsWhiteSpace(str[0]) && !char.IsWhiteSpace(str[str.Length - 1]))
				{
					return str.IndexOf('\n') >= 0;
				}
				return true;
			}
			return false;
		}

		private string GenerateContent()
		{
			StringBuilder tmpContentBuilder = TmpContentBuilder;
			List<MyIniKey> tmpKeyList = TmpKeyList;
			List<string> tmpStringList = TmpStringList;
			try
			{
				bool flag = false;
				StringSegment sectionCommentSegment;
				foreach (StringSegment item in m_sections.Keys.OrderBy((StringSegment s) => m_sections[s]))
				{
					if (flag)
					{
						tmpContentBuilder.Append('\n');
					}
					flag = true;
					sectionCommentSegment = GetSectionCommentSegment(item);
					if (!sectionCommentSegment.IsEmpty)
					{
						sectionCommentSegment.GetLines(tmpStringList);
						foreach (string item2 in tmpStringList)
						{
							tmpContentBuilder.Append(";");
							tmpContentBuilder.Append(item2);
							tmpContentBuilder.Append('\n');
						}
					}
					tmpContentBuilder.Append("[");
					tmpContentBuilder.Append(item);
					tmpContentBuilder.Append("]\n");
					GetKeys(item, tmpKeyList);
					for (int i = 0; i < tmpKeyList.Count; i++)
					{
						MyIniKey key = tmpKeyList[i];
						StringSegment nameSegment = key.NameSegment;
						sectionCommentSegment = GetCommentSegment(key);
						if (!sectionCommentSegment.IsEmpty)
						{
							sectionCommentSegment.GetLines(tmpStringList);
							foreach (string item3 in tmpStringList)
							{
								tmpContentBuilder.Append(";");
								tmpContentBuilder.Append(item3);
								tmpContentBuilder.Append('\n');
							}
						}
						tmpContentBuilder.Append(nameSegment.Text, nameSegment.Start, nameSegment.Length);
						tmpContentBuilder.Append('=');
						StringSegment str = m_items[key];
						if (NeedsMultilineFormat(ref str))
						{
							Realize(ref key, ref str);
							str.GetLines(tmpStringList);
							tmpContentBuilder.Append('\n');
							foreach (string item4 in tmpStringList)
							{
								tmpContentBuilder.Append("|");
								tmpContentBuilder.Append(item4);
								tmpContentBuilder.Append('\n');
							}
						}
						else
						{
							tmpContentBuilder.Append(str.Text, str.Start, str.Length);
							tmpContentBuilder.Append('\n');
						}
					}
				}
				sectionCommentSegment = GetEndCommentSegment();
				if (!sectionCommentSegment.IsEmpty)
				{
					tmpContentBuilder.Append('\n');
					sectionCommentSegment.GetLines(tmpStringList);
					foreach (string item5 in tmpStringList)
					{
						tmpContentBuilder.Append(";");
						tmpContentBuilder.Append(item5);
						tmpContentBuilder.Append('\n');
					}
				}
				if (m_endContent.Length > 0)
				{
					tmpContentBuilder.Append('\n');
					tmpContentBuilder.Append("---\n");
					tmpContentBuilder.Append(m_endContent);
				}
				string result = tmpContentBuilder.ToString();
				tmpContentBuilder.Clear();
				tmpKeyList.Clear();
				return result;
			}
			finally
			{
				tmpContentBuilder.Clear();
				tmpKeyList.Clear();
			}
		}
	}
}
