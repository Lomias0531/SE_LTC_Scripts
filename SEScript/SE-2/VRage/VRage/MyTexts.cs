using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Utils;

namespace VRage
{
	public static class MyTexts
	{
		public class MyLanguageDescription
		{
			public readonly MyLanguagesEnum Id;

			public readonly string Name;

			public readonly string CultureName;

			public readonly string SubcultureName;

			public readonly string FullCultureName;

			public readonly bool IsCommunityLocalized;

			public readonly float GuiTextScale;

			internal MyLanguageDescription(MyLanguagesEnum id, string displayName, string cultureName, string subcultureName, float guiTextScale, bool isCommunityLocalized)
			{
				Id = id;
				Name = displayName;
				CultureName = cultureName;
				SubcultureName = subcultureName;
				if (string.IsNullOrWhiteSpace(subcultureName))
				{
					FullCultureName = cultureName;
				}
				else
				{
					FullCultureName = $"{cultureName}-{subcultureName}";
				}
				IsCommunityLocalized = isCommunityLocalized;
				GuiTextScale = guiTextScale;
			}
		}

		private class MyGeneralEvaluator : ITextEvaluator
		{
			public string TokenEvaluate(string token, string context)
			{
				StringBuilder stringBuilder = Get(MyStringId.GetOrCompute(token));
				if (stringBuilder != null)
				{
					return stringBuilder.ToString();
				}
				return "";
			}
		}

		private static readonly string LOCALIZATION_TAG_GENERAL;

		public static readonly MyStringId GAMEPAD_VARIANT_ID;

		private static readonly Dictionary<MyLanguagesEnum, MyLanguageDescription> m_languageIdToLanguage;

		private static readonly Dictionary<string, MyLanguagesEnum> m_cultureToLanguageId;

		private static readonly bool m_checkMissingTexts;

		private static MyLocalizationPackage m_package;

		/// <summary>
		/// Current global localization variant selector.
		/// </summary>
		private static MyStringId m_selectedVariant;

		private static Regex m_textReplace;

		private static readonly Dictionary<string, ITextEvaluator> m_evaluators;

		/// <summary>
		/// Global selector for translation variants.
		/// </summary>
		public static MyStringId GlobalVariantSelector => m_selectedVariant;

		public static DictionaryReader<MyLanguagesEnum, MyLanguageDescription> Languages => new DictionaryReader<MyLanguagesEnum, MyLanguageDescription>(m_languageIdToLanguage);

		static MyTexts()
		{
			LOCALIZATION_TAG_GENERAL = "LOCG";
			GAMEPAD_VARIANT_ID = MyStringId.GetOrCompute("Gamepad");
			m_languageIdToLanguage = new Dictionary<MyLanguagesEnum, MyLanguageDescription>();
			m_cultureToLanguageId = new Dictionary<string, MyLanguagesEnum>();
			m_checkMissingTexts = false;
			m_package = new MyLocalizationPackage();
			m_selectedVariant = MyStringId.NullOrEmpty;
			m_evaluators = new Dictionary<string, ITextEvaluator>();
			AddLanguage(MyLanguagesEnum.English, "en", null, "English", 1f, isCommunityLocalized: false);
			AddLanguage(MyLanguagesEnum.Czech, "cs", "CZ", "Česky", 0.95f);
			AddLanguage(MyLanguagesEnum.Slovak, "sk", "SK", "Slovenčina", 0.95f);
			AddLanguage(MyLanguagesEnum.German, "de", null, "Deutsch", 0.85f);
			AddLanguage(MyLanguagesEnum.Russian, "ru", null, "Русский", 1f, isCommunityLocalized: false);
			AddLanguage(MyLanguagesEnum.Spanish_Spain, "es", null, "Español (España)");
			AddLanguage(MyLanguagesEnum.French, "fr", null, "Français");
			AddLanguage(MyLanguagesEnum.Italian, "it", null, "Italiano");
			AddLanguage(MyLanguagesEnum.Danish, "da", null, "Dansk");
			AddLanguage(MyLanguagesEnum.Dutch, "nl", null, "Nederlands");
			AddLanguage(MyLanguagesEnum.Icelandic, "is", "IS", "Íslenska");
			AddLanguage(MyLanguagesEnum.Polish, "pl", "PL", "Polski");
			AddLanguage(MyLanguagesEnum.Finnish, "fi", null, "Suomi");
			AddLanguage(MyLanguagesEnum.Hungarian, "hu", "HU", "Magyar", 0.85f);
			AddLanguage(MyLanguagesEnum.Portuguese_Brazil, "pt", "BR", "Português (Brasileiro)");
			AddLanguage(MyLanguagesEnum.Estonian, "et", "EE", "Eesti");
			AddLanguage(MyLanguagesEnum.Norwegian, "no", null, "Norsk");
			AddLanguage(MyLanguagesEnum.Spanish_HispanicAmerica, "es", "419", "Español (Latinoamerica)");
			AddLanguage(MyLanguagesEnum.Swedish, "sv", null, "Svenska", 0.9f);
			AddLanguage(MyLanguagesEnum.Catalan, "ca", "AD", "Català", 0.85f);
			AddLanguage(MyLanguagesEnum.Croatian, "hr", "HR", "Hrvatski", 0.9f);
			AddLanguage(MyLanguagesEnum.Romanian, "ro", null, "Română", 0.85f);
			AddLanguage(MyLanguagesEnum.Ukrainian, "uk", null, "Українська");
			AddLanguage(MyLanguagesEnum.Turkish, "tr", "TR", "Türkçe");
			AddLanguage(MyLanguagesEnum.Latvian, "lv", null, "Latviešu", 0.87f);
			AddLanguage(MyLanguagesEnum.ChineseChina, "zh", "CN", "Chinese", 1f, isCommunityLocalized: false);
			RegisterEvaluator(LOCALIZATION_TAG_GENERAL, new MyGeneralEvaluator());
		}

		private static void AddLanguage(MyLanguagesEnum id, string cultureName, string subcultureName = null, string displayName = null, float guiTextScale = 1f, bool isCommunityLocalized = true)
		{
			MyLanguageDescription myLanguageDescription = new MyLanguageDescription(id, displayName, cultureName, subcultureName, guiTextScale, isCommunityLocalized);
			m_languageIdToLanguage.Add(id, myLanguageDescription);
			m_cultureToLanguageId.Add(myLanguageDescription.FullCultureName, id);
		}

		public static MyLanguagesEnum GetBestSuitableLanguage(string culture)
		{
			MyLanguagesEnum value = MyLanguagesEnum.English;
			if (!m_cultureToLanguageId.TryGetValue(culture, out value))
			{
				string[] array = culture.Split(new char[1]
				{
					'-'
				});
				string b = array[0];
				_ = array[1];
				{
					foreach (KeyValuePair<MyLanguagesEnum, MyLanguageDescription> item in m_languageIdToLanguage)
					{
						if (item.Value.FullCultureName == b)
						{
							return item.Key;
						}
					}
					return value;
				}
			}
			return value;
		}

		public static string GetSystemLanguage()
		{
			return CultureInfo.InstalledUICulture.Name;
		}

		public static void LoadSupportedLanguages(string rootDirectory, HashSet<MyLanguagesEnum> outSupportedLanguages)
		{
			outSupportedLanguages.Add(MyLanguagesEnum.English);
			IEnumerable<string> files = MyFileSystem.GetFiles(rootDirectory, "*.resx", MySearchOption.TopDirectoryOnly);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string item in files)
			{
				string[] array = Path.GetFileNameWithoutExtension(item).Split(new char[1]
				{
					'.'
				});
				if (array.Length > 1)
				{
					hashSet.Add(array[1]);
				}
			}
			foreach (string item2 in hashSet)
			{
				if (m_cultureToLanguageId.TryGetValue(item2, out MyLanguagesEnum value))
				{
					outSupportedLanguages.Add(value);
				}
			}
		}

		/// <summary>
		/// Set the global variant to be selected for each translation.
		/// </summary>
		/// <param name="variantName"></param>
		public static void SetGlobalVariantSelector(MyStringId variantName)
		{
			m_selectedVariant = variantName;
		}

		public static string SubstituteTexts(string text, string context = null)
		{
			if (text != null)
			{
				return m_textReplace.Replace(text, (Match match) => ReplaceEvaluator(match, context));
			}
			return null;
		}

		public static StringBuilder SubstituteTexts(StringBuilder text)
		{
			if (text == null)
			{
				return null;
			}
			string text2 = text.ToString();
			string text3 = m_textReplace.Replace(text2, ReplaceEvaluator);
			if (!(text2 == text3))
			{
				return new StringBuilder(text3);
			}
			return text;
		}

		public static StringBuilder Get(MyStringId id)
		{
			if (!m_package.TryGetStringBuilder(id, m_selectedVariant, out StringBuilder messageSb))
			{
				messageSb = ((!m_checkMissingTexts) ? new StringBuilder(id.ToString()) : new StringBuilder("X_" + id.ToString()));
			}
			if (m_checkMissingTexts)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("T_");
				messageSb = stringBuilder.Append((object)messageSb);
			}
			string text = messageSb.ToString();
			string text2 = m_textReplace.Replace(text, ReplaceEvaluator);
			if (!(text == text2))
			{
				return new StringBuilder(text2);
			}
			return messageSb;
		}

		public static string TrySubstitute(string input)
		{
			MyStringId orCompute = MyStringId.GetOrCompute(input);
			if (!m_package.TryGet(orCompute, m_selectedVariant, out string message))
			{
				return input;
			}
			return m_textReplace.Replace(message.ToString(), ReplaceEvaluator);
		}

		public static void RegisterEvaluator(string prefix, ITextEvaluator eval)
		{
			m_evaluators.Add(prefix, eval);
			InitReplace();
		}

		private static void InitReplace()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			stringBuilder.Append("{(");
			foreach (KeyValuePair<string, ITextEvaluator> evaluator in m_evaluators)
			{
				if (num != 0)
				{
					stringBuilder.Append("|");
				}
				stringBuilder.AppendFormat(evaluator.Key);
				num++;
			}
			stringBuilder.Append("):((?:\\w|:)*)}");
			m_textReplace = new Regex(stringBuilder.ToString());
		}

		private static string ReplaceEvaluator(Match match)
		{
			return ReplaceEvaluator(match, null);
		}

		private static string ReplaceEvaluator(Match match, string context)
		{
			if (match.Groups.Count != 3)
			{
				return string.Empty;
			}
			if (m_evaluators.TryGetValue(match.Groups[1].Value, out ITextEvaluator value))
			{
				return value.TokenEvaluate(match.Groups[2].Value, context);
			}
			return string.Empty;
		}

		public static string GetString(MyStringId id)
		{
			if (!m_package.TryGet(id, m_selectedVariant, out string message))
			{
				message = ((!m_checkMissingTexts) ? id.ToString() : ("X_" + id.ToString()));
			}
			if (m_checkMissingTexts)
			{
				message = "T_" + message;
			}
			return m_textReplace.Replace(message, ReplaceEvaluator);
		}

		public static string GetString(string keyString)
		{
			return GetString(MyStringId.GetOrCompute(keyString));
		}

		public static bool Exists(MyStringId id)
		{
			return m_package.ContainsKey(id);
		}

		public static void Clear()
		{
			m_package.Clear();
			m_package.AddMessage("", "");
		}

		private static string GetPathWithFile(string file, List<string> allFiles)
		{
			foreach (string allFile in allFiles)
			{
				if (allFile.Contains(file))
				{
					return allFile;
				}
			}
			return null;
		}

		public static bool IsTagged(string text, int position, string tag)
		{
			for (int i = 0; i < tag.Length; i++)
			{
				if (text[position + i] != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		public static void LoadTexts(string rootDirectory, string cultureName = null, string subcultureName = null)
		{
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			HashSet<string> hashSet3 = new HashSet<string>();
			IEnumerable<string> files = MyFileSystem.GetFiles(rootDirectory, "*.resx", MySearchOption.AllDirectories);
			List<string> list = new List<string>();
			foreach (string item in files)
			{
				if (item.Contains("MyCommonTexts"))
				{
					hashSet.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1]
					{
						'.'
					})[0]);
				}
				else if (item.Contains("MyTexts"))
				{
					hashSet2.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1]
					{
						'.'
					})[0]);
				}
				else
				{
					if (!item.Contains("MyCoreTexts"))
					{
						continue;
					}
					hashSet3.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1]
					{
						'.'
					})[0]);
				}
				list.Add(item);
			}
			foreach (string item2 in hashSet)
			{
				PatchTexts(GetPathWithFile($"{item2}.resx", list));
			}
			foreach (string item3 in hashSet2)
			{
				PatchTexts(GetPathWithFile($"{item3}.resx", list));
			}
			foreach (string item4 in hashSet3)
			{
				PatchTexts(GetPathWithFile($"{item4}.resx", list));
			}
			if (cultureName != null)
			{
				foreach (string item5 in hashSet)
				{
					PatchTexts(GetPathWithFile($"{item5}.{cultureName}.resx", list));
				}
				foreach (string item6 in hashSet2)
				{
					PatchTexts(GetPathWithFile($"{item6}.{cultureName}.resx", list));
				}
				foreach (string item7 in hashSet3)
				{
					PatchTexts(GetPathWithFile($"{item7}.{cultureName}.resx", list));
				}
				if (subcultureName != null)
				{
					foreach (string item8 in hashSet)
					{
						PatchTexts(GetPathWithFile($"{item8}.{cultureName}-{subcultureName}.resx", list));
					}
					foreach (string item9 in hashSet2)
					{
						PatchTexts(GetPathWithFile($"{item9}.{cultureName}-{subcultureName}.resx", list));
					}
					foreach (string item10 in hashSet3)
					{
						PatchTexts(GetPathWithFile($"{item10}.{cultureName}-{subcultureName}.resx", list));
					}
				}
			}
		}

		private static void PatchTexts(string resourceFile)
		{
			if (MyFileSystem.FileExists(resourceFile))
			{
				using (Stream inStream = MyFileSystem.OpenRead(resourceFile))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(inStream);
					foreach (XmlNode item in xmlDocument.SelectNodes("/root/data"))
					{
						string value = item.Attributes["name"].Value;
						string text = null;
						foreach (XmlNode childNode in item.ChildNodes)
						{
							if (childNode.Name.Equals("value", StringComparison.InvariantCultureIgnoreCase))
							{
								XmlNodeReader xmlNodeReader = new XmlNodeReader(childNode);
								if (xmlNodeReader.Read())
								{
									text = xmlNodeReader.ReadString();
								}
							}
						}
						if (!string.IsNullOrEmpty(value) && text != null)
						{
							m_package.AddMessage(value, text, overwrite: true);
						}
					}
				}
			}
		}

		public static StringBuilder AppendFormat(this StringBuilder stringBuilder, MyStringId textEnum, object arg0)
		{
			return stringBuilder.AppendFormat(GetString(textEnum), arg0);
		}

		public static StringBuilder AppendFormat(this StringBuilder stringBuilder, MyStringId textEnum, params object[] arg)
		{
			return stringBuilder.AppendFormat(GetString(textEnum), arg);
		}

		public static StringBuilder AppendFormat(this StringBuilder stringBuilder, MyStringId textEnum, MyStringId arg0)
		{
			return stringBuilder.AppendFormat(GetString(textEnum), GetString(arg0));
		}
	}
}
