using ProtoBuf;
using System;
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
	public class SerializableDictionary<T, U>
	{
		public struct Entry
		{
			public T Key;

			public U Value;
		}

		protected class VRage_Serialization_SerializableDictionary_00602_003C_003Em_dictionary_003C_003EAccessor : IMemberAccessor<SerializableDictionary<T, U>, Dictionary<T, U>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDictionary<T, U> owner, in Dictionary<T, U> value)
			{
				owner.m_dictionary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDictionary<T, U> owner, out Dictionary<T, U> value)
			{
				value = owner.m_dictionary;
			}
		}

		protected class VRage_Serialization_SerializableDictionary_00602_003C_003EDictionary_003C_003EAccessor : IMemberAccessor<SerializableDictionary<T, U>, Dictionary<T, U>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDictionary<T, U> owner, in Dictionary<T, U> value)
			{
				owner.Dictionary = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDictionary<T, U> owner, out Dictionary<T, U> value)
			{
				value = owner.Dictionary;
			}
		}

		protected class VRage_Serialization_SerializableDictionary_00602_003C_003EDictionaryEntryProp_003C_003EAccessor : IMemberAccessor<SerializableDictionary<T, U>, Entry[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDictionary<T, U> owner, in Entry[] value)
			{
				owner.DictionaryEntryProp = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDictionary<T, U> owner, out Entry[] value)
			{
				value = owner.DictionaryEntryProp;
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
		[XmlArrayItem("item")]
		[NoSerialize]
		public Entry[] DictionaryEntryProp
		{
			get
			{
				Entry[] array = new Entry[Dictionary.Count];
				int num = 0;
				foreach (KeyValuePair<T, U> item in Dictionary)
				{
					Entry entry = default(Entry);
					entry.Key = item.Key;
					entry.Value = item.Value;
					array[num] = entry;
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
						Dictionary.Add(value[i].Key, value[i].Value);
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

		public SerializableDictionary()
		{
		}

		public SerializableDictionary(Dictionary<T, U> dict)
		{
			Dictionary = dict;
		}
	}
}
