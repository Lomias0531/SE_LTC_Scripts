using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Serialization
{
	[ProtoContract]
	[XmlRoot("Dictionary")]
	[Obfuscation(Feature = "cw symbol renaming", Exclude = true, ApplyToMembers = true)]
	public class SerializableDictionaryHack<T, U>
	{
		protected class VRage_Serialization_SerializableDictionaryHack_00602_003C_003Em_dictionary_003C_003EAccessor : IMemberAccessor<SerializableDictionaryHack<T, U>, Dictionary<T, U>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDictionaryHack<T, U> owner, in Dictionary<T, U> value)
			{
				owner.m_dictionary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDictionaryHack<T, U> owner, out Dictionary<T, U> value)
			{
				value = owner.m_dictionary;
			}
		}

		protected class VRage_Serialization_SerializableDictionaryHack_00602_003C_003EDictionary_003C_003EAccessor : IMemberAccessor<SerializableDictionaryHack<T, U>, Dictionary<T, U>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDictionaryHack<T, U> owner, in Dictionary<T, U> value)
			{
				owner.Dictionary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDictionaryHack<T, U> owner, out Dictionary<T, U> value)
			{
				value = owner.Dictionary;
			}
		}

		protected class VRage_Serialization_SerializableDictionaryHack_00602_003C_003EDictionaryEntryProp_003C_003EAccessor : IMemberAccessor<SerializableDictionaryHack<T, U>, DictionaryEntry[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDictionaryHack<T, U> owner, in DictionaryEntry[] value)
			{
				((SerializableDictionaryHack<, >)(object)owner).DictionaryEntryProp = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDictionaryHack<T, U> owner, out DictionaryEntry[] value)
			{
				value = ((SerializableDictionaryHack<, >)(object)owner).DictionaryEntryProp;
			}
		}

		private Dictionary<T, U> m_dictionary = new Dictionary<T, U>();

		/// <summary>
		/// Public stuff dictionary.
		/// </summary>
		/// <remarks>
		/// Note the XmlIgnore attribute.
		/// </remarks>
		[ProtoMember(1)]
		[XmlIgnore]
		public Dictionary<T, U> Dictionary
		{
			get
			{
				return m_dictionary;
			}
			set
			{
				m_dictionary = value;
			}
		}

		/// <summary>
		/// Property created expressly for the XmlSerializer
		/// </summary>
		/// <remarks>
		/// Note the XML Serialiazation attributes; they control what elements are named when this object is serialized.
		/// </remarks>
		[XmlArray("dictionary")]
		[XmlArrayItem("item", Type = typeof(DictionaryEntry))]
		public DictionaryEntry[] DictionaryEntryProp
		{
			get
			{
				DictionaryEntry[] array = new DictionaryEntry[Dictionary.Count];
				int num = 0;
				foreach (KeyValuePair<T, U> item in Dictionary)
				{
					DictionaryEntry dictionaryEntry = default(DictionaryEntry);
					dictionaryEntry.Key = item.Key;
					dictionaryEntry.Value = item.Value;
					array[num] = dictionaryEntry;
					num++;
				}
				return array;
			}
			set
			{
				Dictionary.Clear();
				for (int i = 0; i < value.Length; i++)
				{
					try
					{
						Dictionary.Add((T)value[i].Key, (U)value[i].Value);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		public U this[T key]
		{
			get
			{
				return Dictionary[key];
			}
			set
			{
				Dictionary[key] = value;
			}
		}

		public SerializableDictionaryHack()
		{
		}

		public SerializableDictionaryHack(Dictionary<T, U> dict)
		{
			Dictionary = dict;
		}
	}
}
