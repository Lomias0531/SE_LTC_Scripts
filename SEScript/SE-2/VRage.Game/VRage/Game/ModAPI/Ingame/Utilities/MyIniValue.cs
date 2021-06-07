using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	/// <summary>
	/// Represents the value of a single configuration item.
	/// </summary>
	public struct MyIniValue
	{
		/// <summary>
		/// Represents an empty <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniValue" />
		/// </summary>
		public static readonly MyIniValue EMPTY = default(MyIniValue);

		private static readonly char[] NEWLINE_CHARS = new char[2]
		{
			'\r',
			'\n'
		};

		private readonly string m_value;

		/// <summary>
		/// Gets the <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniKey" /> this value was retrieved from
		/// </summary>
		public readonly MyIniKey Key;

		/// <summary>
		/// Determines whether this value is empty. Be aware that an empty string is not considered to be an empty value.
		/// </summary>
		public bool IsEmpty => m_value == null;

		/// <summary>
		/// Creates a new <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniValue" />
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <exception cref="T:System.ArgumentException">Configuration value cannot use an empty key</exception>
		public MyIniValue(MyIniKey key, string value)
		{
			if (key.IsEmpty)
			{
				throw new ArgumentException("Configuration value cannot use an empty key", "key");
			}
			m_value = (value ?? "");
			Key = key;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Boolean" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public bool ToBoolean(bool defaultValue = false)
		{
			if (!TryGetBoolean(out bool value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Boolean" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetBoolean(out bool value)
		{
			string value2 = m_value;
			if (value2 == null)
			{
				value = false;
				return false;
			}
			if (string.Equals("true", value2, StringComparison.OrdinalIgnoreCase) || string.Equals("yes", value2, StringComparison.OrdinalIgnoreCase) || string.Equals("1", value2, StringComparison.OrdinalIgnoreCase) || string.Equals("on", value2, StringComparison.OrdinalIgnoreCase))
			{
				value = true;
				return true;
			}
			if (string.Equals("false", value2, StringComparison.OrdinalIgnoreCase) || string.Equals("no", value2, StringComparison.OrdinalIgnoreCase) || string.Equals("0", value2, StringComparison.OrdinalIgnoreCase) || string.Equals("off", value2, StringComparison.OrdinalIgnoreCase))
			{
				value = false;
				return true;
			}
			value = false;
			return false;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Char" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public char ToChar(char defaultValue = '\0')
		{
			if (TryGetChar(out char value))
			{
				return value;
			}
			return defaultValue;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Char" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetChar(out char value)
		{
			if (m_value == null)
			{
				value = '\0';
				return false;
			}
			if (m_value.Length == 1)
			{
				value = m_value[0];
				return true;
			}
			value = '\0';
			return false;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.SByte" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public sbyte ToSByte(sbyte defaultValue = 0)
		{
			if (!TryGetSByte(out sbyte value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.SByte" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetSByte(out sbyte value)
		{
			if (m_value == null)
			{
				value = 0;
				return false;
			}
			return sbyte.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Byte" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public byte ToByte(byte defaultValue = 0)
		{
			if (!TryGetByte(out byte value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Byte" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetByte(out byte value)
		{
			if (m_value == null)
			{
				value = 0;
				return false;
			}
			return byte.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.UInt16" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public ushort ToUInt16(ushort defaultValue = 0)
		{
			if (!TryGetUInt16(out ushort value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.UInt16" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetUInt16(out ushort value)
		{
			if (m_value == null)
			{
				value = 0;
				return false;
			}
			return ushort.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Int16" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public short ToInt16(short defaultValue = 0)
		{
			if (!TryGetInt16(out short value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Int16" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetInt16(out short value)
		{
			if (m_value == null)
			{
				value = 0;
				return false;
			}
			return short.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.UInt32" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public uint ToUInt32(uint defaultValue = 0u)
		{
			if (!TryGetUInt32(out uint value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.UInt32" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetUInt32(out uint value)
		{
			if (m_value == null)
			{
				value = 0u;
				return false;
			}
			return uint.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Int32" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public int ToInt32(int defaultValue = 0)
		{
			if (!TryGetInt32(out int value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Int32" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetInt32(out int value)
		{
			if (m_value == null)
			{
				value = 0;
				return false;
			}
			return int.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.UInt64" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public ulong ToUInt64(ulong defaultValue = 0uL)
		{
			if (!TryGetUInt64(out ulong value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.UInt64" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetUInt64(out ulong value)
		{
			if (m_value == null)
			{
				value = 0uL;
				return false;
			}
			return ulong.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Int64" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public long ToInt64(long defaultValue = 0L)
		{
			if (!TryGetInt64(out long value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Int64" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetInt64(out long value)
		{
			if (m_value == null)
			{
				value = 0L;
				return false;
			}
			return long.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Single" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public float ToSingle(float defaultValue = 0f)
		{
			if (TryGetSingle(out float value))
			{
				return value;
			}
			return defaultValue;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Single" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetSingle(out float value)
		{
			if (m_value == null)
			{
				value = 0f;
				return false;
			}
			return float.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Double" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public double ToDouble(double defaultValue = 0.0)
		{
			if (!TryGetDouble(out double value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Double" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetDouble(out double value)
		{
			if (m_value == null)
			{
				value = 0.0;
				return false;
			}
			return double.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Decimal" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public decimal ToDecimal(decimal defaultValue = 0m)
		{
			if (!TryGetDecimal(out decimal value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.Decimal" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetDecimal(out decimal value)
		{
			if (m_value == null)
			{
				value = default(decimal);
				return false;
			}
			return decimal.TryParse(m_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Retrieves each individual line of this value into the provided list.
		/// </summary>
		/// <param name="lines"></param>
		public void GetLines(List<string> lines)
		{
			if (lines == null)
			{
				return;
			}
			string value = m_value;
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			lines.Clear();
			int i = 0;
			int num = 0;
			while (true)
			{
				if (num < value.Length)
				{
					num = value.IndexOfAny(NEWLINE_CHARS, i);
					if (num < 0)
					{
						break;
					}
					lines.Add(value.Substring(i, num - i));
					for (i = num + 1; i < value.Length && Array.IndexOf(NEWLINE_CHARS, value[i]) >= 0; i++)
					{
					}
					continue;
				}
				return;
			}
			lines.Add(value.Substring(i, value.Length - i));
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.String" />. If the value is empty or cannot be understood as this data type, an empty string will be returned instead.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return m_value ?? "";
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.String" />. If the value is empty or cannot be understood as this data type, the <see cref="!:defaultValue" /> will be returned instead.
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public string ToString(string defaultValue)
		{
			return m_value ?? defaultValue ?? "";
		}

		/// <summary>
		/// Attempts to get this value as a <see cref="T:System.String" />. Fills the <c>value</c> on success.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the value could be understood as this data type; <c>false</c> otherwise</returns>
		public bool TryGetString(out string value)
		{
			value = m_value;
			return value != null;
		}

		/// <summary>
		/// Writes this value as a string to the given string builder.
		/// </summary>
		/// <param name="stringBuilder"></param>
		public void Write(StringBuilder stringBuilder)
		{
			if (stringBuilder == null)
			{
				throw new ArgumentNullException("stringBuilder");
			}
			stringBuilder.Append(m_value ?? "");
		}
	}
}
