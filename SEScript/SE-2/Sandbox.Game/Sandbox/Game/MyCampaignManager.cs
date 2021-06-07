using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game
{
	public class MyCampaignManager
	{
		private const string CAMPAIGN_CONTENT_RELATIVE_PATH = "Campaigns";

		private readonly string m_scenariosContentRelativePath = "Scenarios";

		private readonly string m_scenarioFileExtension = "*.scf";

		private const string CAMPAIGN_DEBUG_RELATIVE_PATH = "Worlds\\Campaigns";

		private static MyCampaignManager m_instance;

		private string m_activeCampaignName;

		private MyObjectBuilder_Campaign m_activeCampaign;

		private readonly Dictionary<string, List<MyObjectBuilder_Campaign>> m_campaignsByNames = new Dictionary<string, List<MyObjectBuilder_Campaign>>();

		private readonly List<string> m_activeCampaignLevelNames = new List<string>();

		private Dictionary<string, MyLocalization.MyBundle> m_campaignMenuLocalizationBundle = new Dictionary<string, MyLocalization.MyBundle>();

		private readonly HashSet<MyLocalizationContext> m_campaignLocContexts = new HashSet<MyLocalizationContext>();

		private MyLocalization.MyBundle? m_currentMenuBundle;

		private readonly List<MyWorkshopItem> m_subscribedCampaignItems = new List<MyWorkshopItem>();

		private Task m_refreshTask;

		public static Action AfterCampaignLocalizationsLoaded;

		public static MyCampaignManager Static
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyCampaignManager();
				}
				return m_instance;
			}
		}

		public IEnumerable<MyObjectBuilder_Campaign> Campaigns
		{
			get
			{
				List<MyObjectBuilder_Campaign> list = new List<MyObjectBuilder_Campaign>();
				foreach (List<MyObjectBuilder_Campaign> value in m_campaignsByNames.Values)
				{
					list.AddRange(value);
				}
				return list;
			}
		}

		public IEnumerable<string> CampaignNames => m_campaignsByNames.Keys;

		public IEnumerable<string> ActiveCampaignLevels => m_activeCampaignLevelNames;

		public string ActiveCampaignName => m_activeCampaignName;

		public MyObjectBuilder_Campaign ActiveCampaign => m_activeCampaign;

		public bool IsCampaignRunning
		{
			get
			{
				if (MySession.Static == null)
				{
					return false;
				}
				return MySession.Static.GetComponent<MyCampaignSessionComponent>()?.Running ?? false;
			}
		}

		public IEnumerable<string> LocalizationLanguages
		{
			get
			{
				if (m_activeCampaign == null)
				{
					return null;
				}
				return m_activeCampaign.LocalizationLanguages;
			}
		}

		public bool IsNewCampaignLevelLoading
		{
			get;
			private set;
		}

		public event Action OnCampaignFinished;

		public void Init()
		{
			MyLocalization.Static.InitLoader(LoadCampaignLocalization);
			MySandboxGame.Log.WriteLine("MyCampaignManager.Constructor() - START");
			foreach (string file in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, m_scenariosContentRelativePath), m_scenarioFileExtension, MySearchOption.AllDirectories))
			{
				if (MyObjectBuilderSerializer.DeserializeXML(file, out MyObjectBuilder_VSFiles objectBuilder) && objectBuilder.Campaign != null)
				{
					objectBuilder.Campaign.IsVanilla = true;
					objectBuilder.Campaign.IsLocalMod = false;
					LoadCampaignData(objectBuilder.Campaign);
				}
			}
			foreach (string file2 in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Worlds\\Campaigns"), "*.vs", MySearchOption.TopDirectoryOnly))
			{
				if (MyObjectBuilderSerializer.DeserializeXML(file2, out MyObjectBuilder_VSFiles objectBuilder2) && objectBuilder2.Campaign != null)
				{
					objectBuilder2.Campaign.IsVanilla = true;
					objectBuilder2.Campaign.IsLocalMod = false;
					objectBuilder2.Campaign.IsDebug = true;
					LoadCampaignData(objectBuilder2.Campaign);
				}
			}
			MySandboxGame.Log.WriteLine("MyCampaignManager.Constructor() - END");
		}

		public Task RefreshModData()
		{
			if (!m_refreshTask.IsComplete)
			{
				return m_refreshTask;
			}
			return m_refreshTask = Parallel.Start(delegate
			{
				RefreshLocalModData();
				RefreshSubscribedModData();
			});
		}

		private void RefreshLocalModData()
		{
			string[] directories = Directory.GetDirectories(MyFileSystem.ModsPath);
			foreach (List<MyObjectBuilder_Campaign> value in m_campaignsByNames.Values)
			{
				value.RemoveAll((MyObjectBuilder_Campaign campaign) => campaign.IsLocalMod);
			}
			string[] array = directories;
			foreach (string localModPath in array)
			{
				RegisterLocalModData(localModPath);
			}
		}

		private void RegisterLocalModData(string localModPath)
		{
			foreach (string file in MyFileSystem.GetFiles(Path.Combine(localModPath, "Campaigns"), "*.vs", MySearchOption.TopDirectoryOnly))
			{
				LoadScenarioFile(file);
			}
			foreach (string file2 in MyFileSystem.GetFiles(Path.Combine(localModPath, m_scenariosContentRelativePath), m_scenarioFileExtension, MySearchOption.AllDirectories))
			{
				LoadScenarioFile(file2);
			}
		}

		private void LoadScenarioFile(string modFile)
		{
			if (MyObjectBuilderSerializer.DeserializeXML(modFile, out MyObjectBuilder_VSFiles objectBuilder) && objectBuilder.Campaign != null)
			{
				objectBuilder.Campaign.IsVanilla = false;
				objectBuilder.Campaign.IsLocalMod = true;
				objectBuilder.Campaign.ModFolderPath = GetModFolderPath(modFile);
				LoadCampaignData(objectBuilder.Campaign);
			}
		}

		private void RefreshSubscribedModData()
		{
			if (MyWorkshop.GetSubscribedCampaignsBlocking(m_subscribedCampaignItems))
			{
				List<MyObjectBuilder_Campaign> list = new List<MyObjectBuilder_Campaign>();
				foreach (List<MyObjectBuilder_Campaign> campaignList in m_campaignsByNames.Values)
				{
					foreach (MyObjectBuilder_Campaign item in campaignList)
					{
						if (item.PublishedFileId != 0L)
						{
							bool flag = false;
							for (int i = 0; i < m_subscribedCampaignItems.Count; i++)
							{
								if (m_subscribedCampaignItems[i].Id == item.PublishedFileId)
								{
									m_subscribedCampaignItems.RemoveAtFast(i);
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								list.Add(item);
							}
						}
					}
					list.ForEach(delegate(MyObjectBuilder_Campaign campaignToRemove)
					{
						campaignList.Remove(campaignToRemove);
					});
					list.Clear();
				}
				MyWorkshop.DownloadModsBlockingUGC(m_subscribedCampaignItems, null);
				foreach (MyWorkshopItem subscribedCampaignItem in m_subscribedCampaignItems)
				{
					RegisterWorshopModDataUGC(subscribedCampaignItem);
				}
			}
		}

		private void RegisterWorshopModDataUGC(MyWorkshopItem mod)
		{
			string folder = mod.Folder;
			IEnumerable<string> files = MyFileSystem.GetFiles(folder, "*.vs", MySearchOption.AllDirectories);
			LoadScenarioMod(mod, files);
			IEnumerable<string> files2 = MyFileSystem.GetFiles(folder, m_scenarioFileExtension, MySearchOption.AllDirectories);
			LoadScenarioMod(mod, files2);
		}

		private void LoadScenarioMod(MyWorkshopItem mod, IEnumerable<string> visualScriptingFiles)
		{
			foreach (string visualScriptingFile in visualScriptingFiles)
			{
				if (MyObjectBuilderSerializer.DeserializeXML(visualScriptingFile, out MyObjectBuilder_VSFiles objectBuilder) && objectBuilder.Campaign != null)
				{
					objectBuilder.Campaign.IsVanilla = false;
					objectBuilder.Campaign.IsLocalMod = false;
					objectBuilder.Campaign.PublishedFileId = mod.Id;
					objectBuilder.Campaign.ModFolderPath = GetModFolderPath(visualScriptingFile);
					LoadCampaignData(objectBuilder.Campaign);
				}
			}
		}

		public void PublishActive()
		{
			ulong workshopIdFromLocalMod = MyWorkshop.GetWorkshopIdFromLocalMod(m_activeCampaign.ModFolderPath);
			MyWorkshop.PublishModAsync(m_activeCampaign.ModFolderPath, m_activeCampaign.Name, m_activeCampaign.Description, workshopIdFromLocalMod, new string[1]
			{
				"campaign"
			}, MyPublishedFileVisibility.Public, OnPublishFinished);
		}

		private void OnPublishFinished(bool publishSuccess, MyGameServiceCallResult publishResult, MyWorkshopItemPublisher publishedFile)
		{
			if (publishSuccess)
			{
				MyWorkshop.GenerateModInfo(m_activeCampaign.ModFolderPath, publishedFile.Id, Sync.MyId);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MessageBoxTextCampaignPublished), MySession.GameServiceName)), MyTexts.Get(MyCommonTexts.MessageBoxCaptionCampaignPublished), null, null, null, null, delegate
				{
					MyGameService.OpenOverlayUrl(MyGameService.WorkshopService.GetItemUrl(publishedFile.Id));
				}));
			}
			else
			{
				StringBuilder messageText = (publishResult != MyGameServiceCallResult.AccessDenied) ? new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublishFailed), MySession.WorkshopServiceName, MySession.GameServiceName) : MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText, MyTexts.Get(MyCommonTexts.MessageBoxCaptionModCampaignPublishFailed)));
			}
		}

		private string GetModFolderPath(string path)
		{
			int num = path.IndexOf("Campaigns", StringComparison.InvariantCulture);
			if (num == -1)
			{
				num = path.IndexOf(m_scenariosContentRelativePath, StringComparison.InvariantCulture);
			}
			return path.Remove(num - 1);
		}

		private void LoadCampaignData(MyObjectBuilder_Campaign campaignOb)
		{
			if (m_campaignsByNames.ContainsKey(campaignOb.Name))
			{
				List<MyObjectBuilder_Campaign> list = m_campaignsByNames[campaignOb.Name];
				foreach (MyObjectBuilder_Campaign item2 in list)
				{
					if (item2.IsLocalMod == campaignOb.IsLocalMod && item2.IsMultiplayer == campaignOb.IsMultiplayer && item2.IsVanilla == campaignOb.IsVanilla && item2.PublishedFileId == campaignOb.PublishedFileId)
					{
						return;
					}
				}
				list.Add(campaignOb);
			}
			else
			{
				m_campaignsByNames.Add(campaignOb.Name, new List<MyObjectBuilder_Campaign>());
				m_campaignsByNames[campaignOb.Name].Add(campaignOb);
			}
			if (string.IsNullOrEmpty(campaignOb.DescriptionLocalizationFile))
			{
				return;
			}
			FileInfo fileInfo = new FileInfo(Path.Combine(campaignOb.ModFolderPath ?? MyFileSystem.ContentPath, campaignOb.DescriptionLocalizationFile));
			if (!fileInfo.Exists)
			{
				return;
			}
			string[] files = Directory.GetFiles(fileInfo.Directory.FullName, Path.GetFileNameWithoutExtension(fileInfo.Name) + "*.sbl", SearchOption.TopDirectoryOnly);
			string text = string.IsNullOrEmpty(campaignOb.ModFolderPath) ? campaignOb.Name : Path.Combine(campaignOb.ModFolderPath, campaignOb.Name);
			MyLocalization.MyBundle value = new MyLocalization.MyBundle
			{
				BundleId = MyStringId.GetOrCompute(text),
				FilePaths = new List<string>()
			};
			string[] array = files;
			foreach (string item in array)
			{
				if (!value.FilePaths.Contains(item))
				{
					value.FilePaths.Add(item);
				}
			}
			if (m_campaignMenuLocalizationBundle.ContainsKey(text))
			{
				m_campaignMenuLocalizationBundle[text] = value;
			}
			else
			{
				m_campaignMenuLocalizationBundle.Add(text, value);
			}
		}

		public void LoadSessionFromActiveCampaign(string relativePath, Action afterLoad = null, string campaignDirectoryName = null, string campaignName = null, MyOnlineModeEnum onlineMode = MyOnlineModeEnum.OFFLINE, int maxPlayers = 0)
		{
			string path;
			if (m_activeCampaign.IsVanilla || m_activeCampaign.IsDebug)
			{
				path = Path.Combine(MyFileSystem.ContentPath, relativePath);
				if (!MyFileSystem.FileExists(path))
				{
					MySandboxGame.Log.WriteLine("ERROR: Missing vanilla world file in campaign: " + m_activeCampaignName);
					return;
				}
			}
			else
			{
				path = Path.Combine(m_activeCampaign.ModFolderPath, relativePath);
				if (!MyFileSystem.FileExists(path))
				{
					path = Path.Combine(MyFileSystem.ContentPath, relativePath);
					if (!MyFileSystem.FileExists(path))
					{
						MySandboxGame.Log.WriteLine("ERROR: Missing world file in campaign: " + m_activeCampaignName);
						return;
					}
				}
			}
			if (string.IsNullOrEmpty(campaignDirectoryName))
			{
				campaignDirectoryName = ActiveCampaignName + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
			string text = Path.Combine(MyFileSystem.SavesPath, campaignDirectoryName, directoryInfo.Name);
			while (MyFileSystem.DirectoryExists(text))
			{
				text = Path.Combine(MyFileSystem.SavesPath, campaignDirectoryName, directoryInfo.Name + " " + MyUtils.GetRandomInt(int.MaxValue).ToString("########"));
			}
			if (File.Exists(path))
			{
				MyUtils.CopyDirectory(directoryInfo.FullName, text);
				if (m_activeCampaign != null)
				{
					string path2 = Path.Combine(text, Path.GetFileName(path));
					if (MyFileSystem.FileExists(path2) && MyObjectBuilderSerializer.DeserializeXML(path2, out MyObjectBuilder_Checkpoint objectBuilder))
					{
						objectBuilder.Settings.VoxelGeneratorVersion = MyFakes.DEFAULT_PROCEDURAL_ASTEROID_GENERATOR;
						foreach (MyObjectBuilder_SessionComponent sessionComponent in objectBuilder.SessionComponents)
						{
							MyObjectBuilder_LocalizationSessionComponent myObjectBuilder_LocalizationSessionComponent;
							if ((myObjectBuilder_LocalizationSessionComponent = (sessionComponent as MyObjectBuilder_LocalizationSessionComponent)) != null)
							{
								myObjectBuilder_LocalizationSessionComponent.CampaignModFolderName = m_activeCampaign.ModFolderPath;
								break;
							}
						}
						MyObjectBuilderSerializer.SerializeXML(path2, compress: false, objectBuilder);
					}
				}
			}
			else
			{
				string text2 = Path.Combine(Path.GetTempPath(), "TMP_CAMPAIGN_MOD_FOLDER");
				string source = Path.Combine(text2, Path.GetDirectoryName(relativePath));
				MyZipArchive.ExtractToDirectory(m_activeCampaign.ModFolderPath, text2);
				MyUtils.CopyDirectory(source, text);
				Directory.Delete(text2, recursive: true);
			}
			string currentLanguage = MyLanguage.CurrentCultureName;
			if (!string.IsNullOrEmpty(currentLanguage))
			{
				afterLoad = (Action)Delegate.Combine(afterLoad, (Action)delegate
				{
					MyLocalizationSessionComponent component = MySession.Static.GetComponent<MyLocalizationSessionComponent>();
					component.LoadCampaignLocalization(m_activeCampaign.LocalizationPaths, m_activeCampaign.ModFolderPath);
					component.SwitchLanguage(currentLanguage);
					if (AfterCampaignLocalizationsLoaded != null)
					{
						AfterCampaignLocalizationsLoaded();
					}
				});
			}
			afterLoad = (Action)Delegate.Combine(afterLoad, (Action)delegate
			{
				MySession.Static.Save();
				IsNewCampaignLevelLoading = false;
			});
			IsNewCampaignLevelLoading = true;
			if (MyLocalization.Static != null)
			{
				MyLocalization.Static.DisposeAll();
			}
			MySessionLoader.LoadSingleplayerSession(text, afterLoad, campaignName, onlineMode, maxPlayers);
		}

		public void LoadCampaignLocalization()
		{
			string currentCultureName = MyLanguage.CurrentCultureName;
			if (MySession.Static != null)
			{
				MyLocalizationSessionComponent component = MySession.Static.GetComponent<MyLocalizationSessionComponent>();
				if (component != null && m_activeCampaign != null)
				{
					component.LoadCampaignLocalization(m_activeCampaign.LocalizationPaths, m_activeCampaign.ModFolderPath);
					component.SwitchLanguage(currentCultureName);
				}
			}
		}

		public bool SwitchCampaign(string name, bool isVanilla = true, ulong publisherFileId = 0uL, string localModFolder = null)
		{
			if (m_campaignsByNames.ContainsKey(name))
			{
				foreach (MyObjectBuilder_Campaign item in m_campaignsByNames[name])
				{
					if (item.IsVanilla == isVanilla && item.IsLocalMod == (localModFolder != null && publisherFileId == 0) && item.PublishedFileId == publisherFileId)
					{
						m_activeCampaign = item;
						m_activeCampaignName = name;
						m_activeCampaignLevelNames.Clear();
						MyObjectBuilder_CampaignSMNode[] nodes = m_activeCampaign.StateMachine.Nodes;
						foreach (MyObjectBuilder_CampaignSMNode myObjectBuilder_CampaignSMNode in nodes)
						{
							m_activeCampaignLevelNames.Add(myObjectBuilder_CampaignSMNode.Name);
						}
						return true;
					}
					if (publisherFileId == 0L && item.PublishedFileId != 0L)
					{
						publisherFileId = item.PublishedFileId;
						return true;
					}
				}
			}
			if (publisherFileId != 0L)
			{
				if (DownloadCampaign(publisherFileId))
				{
					return SwitchCampaign(name, isVanilla, publisherFileId, localModFolder);
				}
			}
			else if (!isVanilla && localModFolder != null && MyFileSystem.DirectoryExists(localModFolder))
			{
				RegisterLocalModData(localModFolder);
				return SwitchCampaign(name, isVanilla, publisherFileId, localModFolder);
			}
			return false;
		}

		public bool DownloadCampaign(ulong publisherFileId)
		{
			MyWorkshop.ResultData resultData = default(MyWorkshop.ResultData);
			resultData.Success = false;
			MyWorkshop.ResultData resultData2 = resultData;
			resultData2 = MyWorkshop.DownloadModsBlockingUGC(new List<MyWorkshopItem>(new MyWorkshopItem[1]
			{
				new MyWorkshopItem
				{
					Id = publisherFileId
				}
			}), null);
			if (resultData2.Success && resultData2.Mods.Count != 0)
			{
				RegisterWorshopModDataUGC(resultData2.Mods[0]);
				return true;
			}
			return false;
		}

		public void ReloadMenuLocalization(string name)
		{
			if (m_currentMenuBundle.HasValue)
			{
				MyLocalization.Static.UnloadBundle(m_currentMenuBundle.Value.BundleId);
				m_campaignLocContexts.Clear();
			}
			if (m_campaignMenuLocalizationBundle.ContainsKey(name))
			{
				m_currentMenuBundle = m_campaignMenuLocalizationBundle[name];
				if (m_currentMenuBundle.HasValue)
				{
					MyLocalization.Static.LoadBundle(m_currentMenuBundle.Value, m_campaignLocContexts, disposableContexts: false);
					foreach (MyLocalizationContext campaignLocContext in m_campaignLocContexts)
					{
						campaignLocContext.Switch(MyLanguage.CurrentCultureName);
					}
				}
			}
		}

		public void RunNewCampaign(string campaignName, MyOnlineModeEnum onlineMode, int maxPlayers)
		{
			MyObjectBuilder_CampaignSMNode myObjectBuilder_CampaignSMNode = FindStartingState();
			if (myObjectBuilder_CampaignSMNode != null)
			{
				LoadSessionFromActiveCampaign(myObjectBuilder_CampaignSMNode.SaveFilePath, null, null, campaignName, onlineMode, maxPlayers);
			}
		}

		private MyObjectBuilder_CampaignSMNode FindStartingState()
		{
			if (m_activeCampaign == null)
			{
				return null;
			}
			bool flag = false;
			MyObjectBuilder_CampaignSMNode[] nodes = m_activeCampaign.StateMachine.Nodes;
			foreach (MyObjectBuilder_CampaignSMNode myObjectBuilder_CampaignSMNode in nodes)
			{
				MyObjectBuilder_CampaignSMTransition[] transitions = m_activeCampaign.StateMachine.Transitions;
				for (int j = 0; j < transitions.Length; j++)
				{
					if (transitions[j].To == myObjectBuilder_CampaignSMNode.Name)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					flag = false;
					continue;
				}
				return myObjectBuilder_CampaignSMNode;
			}
			return null;
		}

		public void NotifyCampaignFinished()
		{
			this.OnCampaignFinished?.Invoke();
		}
	}
}
