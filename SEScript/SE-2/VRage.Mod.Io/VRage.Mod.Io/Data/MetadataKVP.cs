using System;
using System.Collections.Generic;

namespace VRage.Mod.Io.Data
{
	[Serializable]
	internal class MetadataKVP
	{
		public string metakey;

		public string metavalue;

		public static Dictionary<string, string> ArrayToDictionary(MetadataKVP[] kvpArray)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(kvpArray.Length);
			foreach (MetadataKVP metadataKVP in kvpArray)
			{
				dictionary[metadataKVP.metakey] = metadataKVP.metavalue;
			}
			return dictionary;
		}

		public static MetadataKVP[] DictionaryToArray(Dictionary<string, string> metaDictionary)
		{
			MetadataKVP[] array = new MetadataKVP[metaDictionary.Count];
			int num = 0;
			foreach (KeyValuePair<string, string> item in metaDictionary)
			{
				MetadataKVP metadataKVP = new MetadataKVP
				{
					metakey = item.Key,
					metavalue = item.Value
				};
				array[num++] = metadataKVP;
			}
			return array;
		}
	}
}
