using System;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	/// <summary>
	/// Represents the combination of a section and a key in a <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIni" /> structure.
	/// </summary>
	public struct MyIniKey : IEquatable<MyIniKey>
	{
		private static readonly char[] INVALID_SECTION_CHARS = new char[4]
		{
			'\r',
			'\n',
			'[',
			']'
		};

		private static readonly string INVALID_SECTION_CHARS_STR = "\\r, \\n, [, ]";

		private static readonly char[] INVALID_KEY_CHARS = new char[6]
		{
			'\r',
			'\n',
			'|',
			'=',
			'[',
			']'
		};

		private static readonly string INVALID_KEY_CHARS_STR = "\\r, \\n, |, =, [, ]";

		public static readonly MyIniKey EMPTY = default(MyIniKey);

		internal readonly StringSegment SectionSegment;

		internal readonly StringSegment NameSegment;

		/// <summary>
		/// Gets the Section part of this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" />
		/// </summary>
		public string Section
		{
			get
			{
				StringSegment sectionSegment = SectionSegment;
				return sectionSegment.ToString();
			}
		}

		/// <summary>
		/// Gets the Key part of this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" />
		/// </summary>
		public string Name
		{
			get
			{
				StringSegment nameSegment = NameSegment;
				return nameSegment.ToString();
			}
		}

		/// <summary>
		/// Determines whether this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" /> is empty.
		/// </summary>
		public bool IsEmpty => NameSegment.Length == 0;

		internal static string ValidateSection(ref StringSegment segment)
		{
			if (segment.Length == 0)
			{
				return "cannot be empty.";
			}
			if (segment.IndexOfAny(INVALID_KEY_CHARS) >= 0)
			{
				return $"contains illegal characters ({INVALID_KEY_CHARS_STR})";
			}
			return null;
		}

		internal static string ValidateKey(ref StringSegment segment)
		{
			if (segment.Length == 0)
			{
				return "cannot be empty.";
			}
			if (segment.IndexOfAny(INVALID_KEY_CHARS) >= 0)
			{
				return $"contains illegal characters ({INVALID_KEY_CHARS_STR})";
			}
			return null;
		}

		/// <summary>
		/// Checks the two given <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" />s for equality. Note that this is equality in the sense of a configuration key, which means the comparison is implicitly case insensitive.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(MyIniKey a, MyIniKey b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Checks the two given <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" />s for inequality. Note that this is inequality in the sense of a configuration key, which means the comparison is implicitly case insensitive.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(MyIniKey a, MyIniKey b)
		{
			return !a.Equals(b);
		}

		/// <summary>
		/// Parses a string in the form of <c>section/key</c> into a <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" /> object.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool TryParse(string input, out MyIniKey key)
		{
			if (string.IsNullOrEmpty(input))
			{
				key = EMPTY;
				return false;
			}
			int num = input.IndexOf("/", StringComparison.Ordinal);
			if (num == -1)
			{
				key = EMPTY;
				return false;
			}
			string text = input.Substring(0, num).Trim();
			string text2 = input.Substring(num + 2).Trim();
			if (text.Length == 0 || text.IndexOfAny(INVALID_SECTION_CHARS) >= 0 || text2.Length == 0 || text2.IndexOfAny(INVALID_KEY_CHARS) >= 0)
			{
				key = EMPTY;
				return false;
			}
			key = new MyIniKey(text, text2);
			return true;
		}

		/// <summary>
		/// Parses a string in the form of <c>section/key</c> into a <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" /> object.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <exception cref="T:System.ArgumentException">invalid configuration key format</exception>
		public static MyIniKey Parse(string input)
		{
			if (!TryParse(input, out MyIniKey key))
			{
				throw new ArgumentException("Invalid configuration key format", "input");
			}
			return key;
		}

		/// <summary>
		/// Creates a new instance of <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" />
		/// </summary>
		/// <param name="section"></param>
		/// <param name="name"></param>
		/// <exception cref="T:System.ArgumentException">Section cannot be empty.</exception>
		/// <exception cref="T:System.ArgumentException">Section contains illegal characters</exception>
		/// <exception cref="T:System.ArgumentException">Key cannot be empty.</exception>
		/// <exception cref="T:System.ArgumentException">Key contains illegal characters</exception>
		public MyIniKey(string section, string name)
		{
			if (string.IsNullOrWhiteSpace(section))
			{
				throw new ArgumentException("Section cannot be empty.", "section");
			}
			if (section.IndexOfAny(INVALID_SECTION_CHARS) >= 0)
			{
				throw new ArgumentException($"Section contains illegal characters ({INVALID_SECTION_CHARS_STR})", "section");
			}
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Key cannot be null or whitespace.", "name");
			}
			if (name.IndexOfAny(INVALID_KEY_CHARS) >= 0)
			{
				throw new ArgumentException($"Key contains illegal characters ({INVALID_KEY_CHARS_STR})", "name");
			}
			SectionSegment = new StringSegment(section);
			NameSegment = new StringSegment(name);
		}

		internal MyIniKey(StringSegment section, StringSegment name)
		{
			SectionSegment = section;
			NameSegment = name;
		}

		/// <summary>
		/// Compares this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" /> with another.  Note that this is equality in the sense of a configuration key, which means the comparison is implicitly case insensitive.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(MyIniKey other)
		{
			if (!SectionSegment.EqualsIgnoreCase(other.SectionSegment))
			{
				return false;
			}
			if (!NameSegment.EqualsIgnoreCase(other.NameSegment))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Compares this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" /> with another.  Note that this is equality in the sense of a configuration key, which means the comparison is implicitly case insensitive.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is MyIniKey)
			{
				return Equals((MyIniKey)obj);
			}
			return false;
		}

		/// <summary>
		/// Gets the hash code representing this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" />
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (MyUtils.GetHashUpperCase(SectionSegment.Text, SectionSegment.Start, SectionSegment.Length) * 397) ^ MyUtils.GetHashUpperCase(NameSegment.Text, NameSegment.Start, NameSegment.Length);
		}

		/// <summary>
		/// Generates a string representing this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" /> in the form of <c>section/key</c>.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (IsEmpty)
			{
				return "";
			}
			return $"{Section}/{Name}";
		}
	}
}
