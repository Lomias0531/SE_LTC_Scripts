using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders.Components;
using VRage.Utils;

namespace VRage.Game.Components.Session
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666, typeof(MyObjectBuilder_LocalizationSessionComponent), null)]
	public class MyLocalizationSessionComponent : MySessionComponentBase
	{
		public static readonly string MOD_BUNDLE_NAME = "MySession - Mod Bundle";

		public static readonly string CAMPAIGN_BUNDLE_NAME = "MySession - Campaing Bundle";

		private string m_language;

		private MyContentPath m_campaignModFolder;

		private MyLocalization.MyBundle m_modBundle;

		private MyLocalization.MyBundle m_campaignBundle;

		private readonly HashSet<MyLocalizationContext> m_influencedContexts = new HashSet<MyLocalizationContext>();

		public string Language => m_language;

		public MyLocalizationSessionComponent()
		{
			m_modBundle.BundleId = MyStringId.GetOrCompute(MOD_BUNDLE_NAME);
			m_campaignBundle.BundleId = MyStringId.GetOrCompute(CAMPAIGN_BUNDLE_NAME);
			m_campaignBundle.FilePaths = new List<string>();
			m_modBundle.FilePaths = new List<string>();
		}

		public void LoadCampaignLocalization(IEnumerable<string> paths, string campaignModFolderPath = null)
		{
			MyContentPath myContentPath = new MyContentPath(campaignModFolderPath ?? MyFileSystem.ContentPath);
			m_campaignModFolder = campaignModFolderPath;
			m_campaignBundle.FilePaths.Clear();
			if (!string.IsNullOrEmpty(campaignModFolderPath) && MyFileSystem.IsDirectory(campaignModFolderPath))
			{
				m_campaignBundle.FilePaths.Add(campaignModFolderPath);
			}
			string path = string.IsNullOrEmpty(myContentPath.Path) ? myContentPath.RootFolder : myContentPath.Path;
			foreach (string path2 in paths)
			{
				try
				{
					MyContentPath myContentPath2 = new MyContentPath(Path.Combine(path, path2));
					if (myContentPath2.AbsoluteFileExists)
					{
						m_campaignBundle.FilePaths.Add(myContentPath2.Absolute);
					}
					else if (myContentPath2.AlternateFileExists)
					{
						m_campaignBundle.FilePaths.Add(myContentPath2.AlternatePath);
					}
					else
					{
						foreach (string file in MyFileSystem.GetFiles(myContentPath2.AbsoluteDirExists ? myContentPath2.Absolute : myContentPath2.AlternatePath, "*.sbl", MySearchOption.AllDirectories))
						{
							m_campaignBundle.FilePaths.Add(file);
						}
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine("Wrong Path for localization component: " + path2);
					MyLog.Default.WriteLine(ex.Message);
				}
			}
			if (m_campaignBundle.FilePaths.Count > 0)
			{
				MyLocalization.Static.LoadBundle(m_campaignBundle, m_influencedContexts);
			}
		}

		public void SwitchLanguage(string language)
		{
			m_language = language;
			foreach (MyLocalizationContext influencedContext in m_influencedContexts)
			{
				influencedContext.Switch(language);
			}
		}

		public override void BeforeStart()
		{
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in Session.Mods)
			{
				string path = mod.GetPath();
				try
				{
					foreach (string file in MyFileSystem.GetFiles(path, "*.sbl", MySearchOption.AllDirectories))
					{
						m_modBundle.FilePaths.Add(file);
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine("MyLocalizationSessionComponent: Problem deserializing " + path + "\n" + ex);
				}
			}
			MyLocalization.Static.LoadBundle(m_modBundle, m_influencedContexts);
			SwitchLanguage(m_language);
		}

		protected override void UnloadData()
		{
			MyLocalization.Static.DisposeAll();
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_LocalizationSessionComponent myObjectBuilder_LocalizationSessionComponent = sessionComponent as MyObjectBuilder_LocalizationSessionComponent;
			if (myObjectBuilder_LocalizationSessionComponent != null)
			{
				m_language = myObjectBuilder_LocalizationSessionComponent.Language;
				m_campaignModFolder = myObjectBuilder_LocalizationSessionComponent.CampaignModFolderName;
				LoadCampaignLocalization(myObjectBuilder_LocalizationSessionComponent.CampaignPaths, m_campaignModFolder.Absolute);
			}
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_LocalizationSessionComponent myObjectBuilder_LocalizationSessionComponent = base.GetObjectBuilder() as MyObjectBuilder_LocalizationSessionComponent;
			if (myObjectBuilder_LocalizationSessionComponent != null)
			{
				myObjectBuilder_LocalizationSessionComponent.Language = m_language;
				myObjectBuilder_LocalizationSessionComponent.CampaignModFolderName = m_campaignModFolder.ModFolder;
				{
					foreach (string filePath in m_campaignBundle.FilePaths)
					{
						myObjectBuilder_LocalizationSessionComponent.CampaignPaths.Add(filePath.Replace(MyFileSystem.ContentPath + "\\", ""));
					}
					return myObjectBuilder_LocalizationSessionComponent;
				}
			}
			return myObjectBuilder_LocalizationSessionComponent;
		}
	}
}
