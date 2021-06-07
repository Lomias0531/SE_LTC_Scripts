using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;

namespace VRage
{
	/// <summary>
	/// Lowest level storage for localization key-value pairs with support for message variants.
	/// </summary>
	public sealed class MyLocalizationPackage
	{
		private struct Entry
		{
			public string Message;

			public Variant[] Variants;
		}

		private struct Variant
		{
			public MyStringId VariantId;

			public string Message;

			public Variant(MyStringId variantId, string message)
			{
				VariantId = variantId;
				Message = message;
			}
		}

		/// <summary>
		/// Localization entries.
		/// </summary>
		private readonly Dictionary<MyStringId, Entry> m_entries = new Dictionary<MyStringId, Entry>(MyStringId.Comparer);

		/// <summary>
		/// Cache of string builders per message.
		/// </summary>
		private readonly ConcurrentDictionary<string, StringBuilder> m_stringBuilderCache = new ConcurrentDictionary<string, StringBuilder>();

		/// <summary>
		/// Callback that can be set to control the validation of variant names when registering messages.
		/// </summary>
		public static Func<string, bool> ValidateVariantName;

		/// <summary>
		/// Set of all localization keys in this package.
		/// </summary>
		public ICollection<MyStringId> Keys => m_entries.Keys;

		/// <summary>
		/// Add a message to the package.
		/// </summary>
		/// <param name="key">The key for the message</param>
		/// <param name="message">The message.</param>
		/// <param name="overwrite">Whether to overwrite existing messages with the same key.</param>
		/// <returns><c>true</c> when there was no collision with existing keys, <c>false</c> otherwise.</returns>
		public bool AddMessage(string key, string message, bool overwrite = false)
		{
			int num = key.IndexOf(":", StringComparison.Ordinal);
			MyStringId orCompute;
			MyStringId myStringId;
			if (num > 0)
			{
				orCompute = MyStringId.GetOrCompute(key.Substring(0, num));
				string text = key.Substring(num + 1);
				Func<string, bool> validateVariantName = ValidateVariantName;
				if (validateVariantName != null && !validateVariantName(text))
				{
					throw new ArgumentException("Variant name '" + text + "' is not valid", "key");
				}
				myStringId = MyStringId.GetOrCompute(text);
			}
			else
			{
				orCompute = MyStringId.GetOrCompute(key);
				myStringId = MyStringId.NullOrEmpty;
			}
			Entry value;
			bool flag = m_entries.TryGetValue(orCompute, out value);
			if (!flag)
			{
				value.Variants = Array.Empty<Variant>();
			}
			if (myStringId == MyStringId.NullOrEmpty)
			{
				if (!flag || overwrite || value.Message == null)
				{
					value.Message = message;
				}
			}
			else
			{
				flag = false;
				for (int i = 0; i < value.Variants.Length; i++)
				{
					if (myStringId == value.Variants[i].VariantId)
					{
						if (overwrite)
						{
							value.Variants[i].Message = message;
						}
						flag = true;
					}
				}
				if (!flag)
				{
					int num2 = value.Variants.Length;
					Array.Resize(ref value.Variants, num2 + 1);
					value.Variants[num2] = new Variant(myStringId, message);
				}
			}
			m_entries[orCompute] = value;
			return !flag;
		}

		/// <summary>
		/// Clear the contents of this package.
		/// </summary>
		public void Clear()
		{
			m_entries.Clear();
			m_stringBuilderCache.Clear();
		}

		/// <summary>
		/// Whether this package contains a message with the provided key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(MyStringId key)
		{
			return m_entries.ContainsKey(key);
		}

		/// <summary>
		/// Try to get a localized message.
		/// </summary>
		/// <param name="key">The message key.</param>
		/// <param name="variant">The message variant.</param>
		/// <param name="message">The resulting string if the message was found.</param>
		/// <returns>Whether the message was localized.</returns>
		public bool TryGet(MyStringId key, MyStringId variant, out string message)
		{
			if (m_entries.TryGetValue(key, out Entry value))
			{
				if (variant != MyStringId.NullOrEmpty)
				{
					for (int i = 0; i < value.Variants.Length; i++)
					{
						if (variant == value.Variants[i].VariantId)
						{
							message = value.Variants[i].Message;
							return true;
						}
					}
				}
				message = value.Message;
				return true;
			}
			message = null;
			return false;
		}

		/// <summary>
		/// Try to get a localized message as a string builder.
		/// </summary>
		/// <param name="key">The message key.</param>
		/// <param name="variant">The message variant.</param>
		/// <param name="messageSb">The resulting string builder if the message was found.</param>
		/// <returns>Whether the message was localized.</returns>
		public bool TryGetStringBuilder(MyStringId key, MyStringId variant, out StringBuilder messageSb)
		{
			if (TryGet(key, variant, out string message))
			{
				if (!m_stringBuilderCache.TryGetValue(message, out messageSb))
				{
					m_stringBuilderCache.TryAdd(message, messageSb = new StringBuilder(message));
				}
				return true;
			}
			messageSb = null;
			return false;
		}
	}
}
