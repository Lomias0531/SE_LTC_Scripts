using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Localization
{
	/// <summary>
	/// Class designed around an idea of localization contexts.
	/// Context can be game, gui screen, mission, campaign or a task.
	/// Consists of a multitude of files stored in content folder.
	/// Each context can be modded, same way as created.
	/// </summary>
	public class MyLocalizationContext
	{
		internal struct LocalizationFileInfo
		{
			public readonly MyObjectBuilder_Localization Header;

			public readonly MyStringId Bundle;

			public readonly string HeaderPath;

			public LocalizationFileInfo(string headerFilePath, MyObjectBuilder_Localization header, MyStringId bundle)
			{
				Bundle = bundle;
				Header = header;
				HeaderPath = headerFilePath;
			}
		}

		protected readonly MyStringId m_contextName;

		protected readonly List<string> m_languagesHelper = new List<string>();

		private readonly List<LocalizationFileInfo> m_localizationFileInfos = new List<LocalizationFileInfo>();

		protected readonly Dictionary<MyStringId, MyObjectBuilder_Localization> m_loadedFiles = new Dictionary<MyStringId, MyObjectBuilder_Localization>(MyStringId.Comparer);

		private MyLocalizationPackage m_package = new MyLocalizationPackage();

		private MyLocalizationContext m_twinContext;

		private readonly HashSet<ulong> m_switchHelper = new HashSet<ulong>();

		/// <summary>
		/// Defined languages.
		/// </summary>
		public ListReader<string> Languages => m_languagesHelper;

		/// <summary>
		/// All accessible ids from context.
		/// </summary>
		public IEnumerable<MyStringId> Ids => m_package.Keys;

		public int IdsCount => m_package.Keys.Count;

		/// <summary>
		/// Name of this context.
		/// </summary>
		public MyStringId Name => m_contextName;

		/// <summary>
		/// Currently selected language.
		/// </summary>
		public string CurrentLanguage
		{
			get;
			private set;
		}

		/// <summary>
		/// Context of same name. Basically connection between
		/// non disposable and disposable contexts.
		/// </summary>
		internal MyLocalizationContext TwinContext
		{
			get
			{
				return m_twinContext;
			}
			set
			{
				m_twinContext = value;
			}
		}

		/// <summary>
		/// Simplified accessor.
		/// </summary>
		/// <param name="id">Tag to localize.</param>
		/// <returns>Localized String Builder.</returns>
		public StringBuilder this[MyStringId id] => Localize(id);

		/// <summary>
		/// Simplified accessor. Preferably use the string id version.
		/// </summary>
		/// <param name="nameId">Name identifier. (will be converted to MyStringId)</param>
		/// <returns>Localized String Builder.</returns>
		public StringBuilder this[string nameId] => Localize(MyStringId.GetOrCompute(nameId));

		/// <summary>
		/// Clears all data before shutting down context.
		/// </summary>
		public void Dispose()
		{
			m_languagesHelper.Clear();
			m_package.Clear();
			m_loadedFiles.Clear();
			m_switchHelper.Clear();
			m_localizationFileInfos.Clear();
		}

		internal MyLocalizationContext(MyStringId name)
		{
			m_contextName = name;
		}

		internal void UnloadBundle(MyStringId bundleId)
		{
			int num = 0;
			while (num < m_localizationFileInfos.Count)
			{
				LocalizationFileInfo localizationFileInfo = m_localizationFileInfos[num];
				if ((bundleId == MyStringId.NullOrEmpty && localizationFileInfo.Bundle != MyStringId.NullOrEmpty) || (localizationFileInfo.Bundle == bundleId && localizationFileInfo.Bundle != MyStringId.NullOrEmpty))
				{
					m_loadedFiles.Remove(MyStringId.GetOrCompute(localizationFileInfo.HeaderPath));
					m_localizationFileInfos.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			Switch(CurrentLanguage);
		}

		internal void InsertFileInfo(LocalizationFileInfo info)
		{
			m_localizationFileInfos.Add(info);
			if (!m_languagesHelper.Contains(info.Header.Language))
			{
				m_languagesHelper.Add(info.Header.Language);
			}
		}

		private void Load(LocalizationFileInfo fileInfo)
		{
			if (!string.IsNullOrEmpty(fileInfo.Header.ResXName) && fileInfo.Header.Entries.Count <= 0)
			{
				string directoryName = Path.GetDirectoryName(fileInfo.HeaderPath);
				if (!string.IsNullOrEmpty(directoryName))
				{
					using (Stream inStream = MyFileSystem.OpenRead(Path.Combine(directoryName, fileInfo.Header.ResXName)))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(inStream);
						foreach (XmlNode item in xmlDocument.SelectNodes("/root/data"))
						{
							string value = item.Attributes["name"].Value;
							string value2 = null;
							foreach (XmlNode childNode in item.ChildNodes)
							{
								if (childNode.Name.Equals("value", StringComparison.InvariantCultureIgnoreCase))
								{
									XmlNodeReader xmlNodeReader = new XmlNodeReader(childNode);
									if (xmlNodeReader.Read())
									{
										value2 = xmlNodeReader.ReadString();
									}
								}
							}
							fileInfo.Header.Entries.Add(new MyObjectBuilder_Localization.KeyEntry
							{
								Key = value,
								Value = value2
							});
						}
					}
				}
			}
		}

		/// <summary>
		/// Tries to switch context to provided language.
		/// </summary>
		/// <param name="language"></param>
		public void Switch(string language)
		{
			CurrentLanguage = language;
			m_package.Clear();
			m_switchHelper.Clear();
			foreach (LocalizationFileInfo localizationFileInfo in m_localizationFileInfos)
			{
				if (!(localizationFileInfo.Header.Language != language) && !(localizationFileInfo.Bundle != MyStringId.NullOrEmpty))
				{
					m_switchHelper.Add(localizationFileInfo.Header.Id);
					Load(localizationFileInfo);
					LoadLocalizationFileData(localizationFileInfo.Header);
				}
			}
			foreach (LocalizationFileInfo localizationFileInfo2 in m_localizationFileInfos)
			{
				if (!(localizationFileInfo2.Header.Language != language) && !(localizationFileInfo2.Bundle == MyStringId.NullOrEmpty))
				{
					m_switchHelper.Add(localizationFileInfo2.Header.Id);
					Load(localizationFileInfo2);
					LoadLocalizationFileData(localizationFileInfo2.Header, overrideExisting: true);
				}
			}
			foreach (LocalizationFileInfo localizationFileInfo3 in m_localizationFileInfos)
			{
				if (localizationFileInfo3.Header.Default)
				{
					Load(localizationFileInfo3);
					LoadLocalizationFileData(localizationFileInfo3.Header, overrideExisting: false, suppressError: true);
				}
			}
		}

		private void LoadLocalizationFileData(MyObjectBuilder_Localization localization, bool overrideExisting = false, bool suppressError = false)
		{
			if (localization != null)
			{
				foreach (MyObjectBuilder_Localization.KeyEntry entry in localization.Entries)
				{
					if (!m_package.AddMessage(entry.Key, entry.Value, overrideExisting) && !overrideExisting && !suppressError)
					{
						string msg = "LocalizationContext: Context " + m_contextName.String + " already contains id " + entry.Key + " conflicting entry won't be overwritten.";
						MyLog.Default.WriteLine(msg);
					}
				}
			}
		}

		/// <summary>
		/// Retrieves the localized content from entry with provided id.
		/// </summary>
		/// <param name="id">Unique identifier.</param>
		/// <returns>Localized builder.</returns>
		public StringBuilder Localize(MyStringId id)
		{
			if (m_package.TryGetStringBuilder(id, MyTexts.GlobalVariantSelector, out StringBuilder messageSb))
			{
				return MyTexts.SubstituteTexts(messageSb);
			}
			return TwinContext?.Localize(id);
		}

		public override int GetHashCode()
		{
			return m_contextName.Id;
		}

		protected bool Equals(MyLocalizationContext other)
		{
			return m_contextName.Equals(other.m_contextName);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj is MyStringId)
			{
				return m_contextName.Equals((MyStringId)obj);
			}
			return Equals((MyLocalizationContext)obj);
		}
	}
}
