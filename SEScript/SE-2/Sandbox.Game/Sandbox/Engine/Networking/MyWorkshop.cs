using ParallelTasks;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
	public class MyWorkshop
	{
		public struct Category
		{
			public string Id;

			public MyStringId LocalizableName;

			public bool IsVisibleForFilter;
		}

		public struct MyWorkshopPathInfo
		{
			public string Path;

			public string Suffix;

			public string NamePrefix;

			public static MyWorkshopPathInfo CreateWorldInfo()
			{
				MyWorkshopPathInfo result = default(MyWorkshopPathInfo);
				result.Path = m_workshopWorldsPath;
				result.Suffix = m_workshopWorldSuffix;
				result.NamePrefix = "Workshop";
				return result;
			}

			public static MyWorkshopPathInfo CreateScenarioInfo()
			{
				MyWorkshopPathInfo result = default(MyWorkshopPathInfo);
				result.Path = m_workshopScenariosPath;
				result.Suffix = m_workshopScenariosSuffix;
				result.NamePrefix = "Scenario";
				return result;
			}
		}

		private class CreateWorldResult : IMyAsyncResult
		{
			public string m_createdSessionPath;

			public Task Task
			{
				get;
				private set;
			}

			public bool Success
			{
				get;
				private set;
			}

			public Action<bool, string> Callback
			{
				get;
				private set;
			}

			public bool IsCompleted => Task.IsComplete;

			public CreateWorldResult(MyWorkshopItem world, MyWorkshopPathInfo pathInfo, Action<bool, string> callback, bool overwrite)
			{
				Callback = callback;
				Task = Parallel.Start(delegate
				{
					Success = TryCreateWorldInstanceBlocking(world, pathInfo, out m_createdSessionPath, overwrite);
				});
			}
		}

		private class UpdateWorldsResult : IMyAsyncResult
		{
			public Task Task
			{
				get;
				private set;
			}

			public bool Success
			{
				get;
				private set;
			}

			public Action<bool> Callback
			{
				get;
				private set;
			}

			public bool IsCompleted => Task.IsComplete;

			public UpdateWorldsResult(List<MyWorkshopItem> worlds, MyWorkshopPathInfo pathInfo, Action<bool> callback)
			{
				Callback = callback;
				Task = Parallel.Start(delegate
				{
					Success = TryUpdateWorldsBlocking(worlds, pathInfo);
				});
			}
		}

		private class PublishItemResult : IMyAsyncResult
		{
			public Task Task
			{
				get;
				private set;
			}

			public bool IsCompleted => Task.IsComplete;

			public PublishItemResult(string localFolder, string publishedTitle, string publishedDescription, ulong? publishedFileId, MyPublishedFileVisibility visibility, string[] tags, HashSet<string> ignoredExtensions, HashSet<string> ignoredPaths = null, uint[] requiredDLCs = null)
			{
				Task = Parallel.Start(delegate
				{
					m_publishedItem = PublishItemBlocking(localFolder, publishedTitle, publishedDescription, publishedFileId, visibility, tags, ignoredExtensions, ignoredPaths, requiredDLCs);
				});
			}
		}

		public struct ResultData
		{
			public bool Success;

			public bool Cancel;

			public List<MyWorkshopItem> Mods;

			public List<MyWorkshopItem> MismatchMods;

			public ResultData(bool success, bool cancel)
			{
				Success = success;
				Cancel = cancel;
				Mods = new List<MyWorkshopItem>();
				MismatchMods = new List<MyWorkshopItem>();
			}
		}

		private class DownloadModsResult : IMyAsyncResult
		{
			public ResultData Result;

			public Action<bool> Callback;

			public Task Task
			{
				get;
				private set;
			}

			public bool IsCompleted => Task.IsComplete;

			public DownloadModsResult(List<MyObjectBuilder_Checkpoint.ModItem> mods, Action<bool> onFinishedCallback, CancelToken cancelToken)
			{
				Callback = onFinishedCallback;
				Task = Parallel.Start(delegate
				{
					Result = DownloadWorldModsBlocking(mods, cancelToken);
					if (!Result.Cancel)
					{
						MySandboxGame.Static.Invoke(endActionDownloadMods, "DownloadModsResult::endActionDownloadMods");
					}
				});
			}
		}

		public class CancelToken
		{
			public bool Cancel;
		}

		private const int MOD_NAME_LIMIT = 25;

		private static MyGuiScreenMessageBox m_downloadScreen;

		private static DownloadModsResult m_downloadResult;

		private static readonly int m_dependenciesRequestTimeout = 30000;

		private static readonly string m_workshopWorldsDir = "WorkshopWorlds";

		private static readonly string m_workshopWorldsPath = Path.Combine(MyFileSystem.UserDataPath, m_workshopWorldsDir);

		private static readonly string m_workshopWorldSuffix = ".sbw";

		private static readonly string m_workshopBlueprintsPath = Path.Combine(MyFileSystem.UserDataPath, "Blueprints", "workshop");

		private static readonly string m_workshopBlueprintSuffix = ".sbb";

		private static readonly string m_workshopScriptPath = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "workshop");

		private static readonly string m_workshopModsPath = MyFileSystem.ModsPath;

		public static readonly string WorkshopModSuffix = "_legacy.bin";

		private static readonly string m_workshopScenariosPath = Path.Combine(MyFileSystem.UserDataPath, "Scenarios", "workshop");

		private static readonly string m_workshopScenariosSuffix = ".sbs";

		private static readonly string[] m_previewFileNames = new string[2]
		{
			"thumb.png",
			MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION
		};

		private const string ModMetadataFileName = "metadata.mod";

		private static readonly HashSet<string> m_ignoredExecutableExtensions = new HashSet<string>(new string[48]
		{
			".action",
			".apk",
			".app",
			".bat",
			".bin",
			".cmd",
			".com",
			".command",
			".cpl",
			".csh",
			".dll",
			".exe",
			".gadget",
			".inf1",
			".ins",
			".inx",
			".ipa",
			".isu",
			".job",
			".jse",
			".ksh",
			".lnk",
			".msc",
			".msi",
			".msp",
			".mst",
			".osx",
			".out",
			".pif",
			".paf",
			".prg",
			".ps1",
			".reg",
			".rgs",
			".run",
			".sct",
			".shb",
			".shs",
			".so",
			".u3p",
			".vb",
			".vbe",
			".vbs",
			".vbscript",
			".workflow",
			".ws",
			".wsf",
			".suo"
		});

		private static readonly int m_bufferSize = 1048576;

		private static byte[] buffer = new byte[m_bufferSize];

		private static Category[] m_modCategories;

		private static Category[] m_worldCategories;

		private static Category[] m_blueprintCategories;

		private static Category[] m_scenarioCategories;

		public const string WORKSHOP_DEVELOPMENT_TAG = "development";

		public const string WORKSHOP_WORLD_TAG = "world";

		public const string WORKSHOP_CAMPAIGN_TAG = "campaign";

		public const string WORKSHOP_MOD_TAG = "mod";

		public const string WORKSHOP_BLUEPRINT_TAG = "blueprint";

		public const string WORKSHOP_SCENARIO_TAG = "scenario";

		private const string WORKSHOP_INGAMESCRIPT_TAG = "ingameScript";

		private static FastResourceLock m_modLock = new FastResourceLock();

		private static Action<bool, MyGameServiceCallResult, MyWorkshopItemPublisher> m_onPublishingFinished;

		private static bool m_publishSuccess;

		private static MyWorkshopItemPublisher m_publishedItem;

		private static MyGameServiceCallResult m_publishResult;

		private static MyGuiScreenProgressAsync m_asyncPublishScreen;

		private static CancelToken m_cancelTokenDownloadMods;

		public static Category[] ModCategories => m_modCategories;

		public static Category[] WorldCategories => m_worldCategories;

		public static Category[] BlueprintCategories => m_blueprintCategories;

		public static Category[] ScenarioCategories => m_scenarioCategories;

		public static void Init(Category[] modCategories, Category[] worldCategories, Category[] blueprintCategories, Category[] scenarioCategories)
		{
			m_modCategories = modCategories;
			m_worldCategories = worldCategories;
			m_blueprintCategories = blueprintCategories;
			m_scenarioCategories = scenarioCategories;
		}

		public static void PublishModAsync(string localModFolder, string publishedTitle, string publishedDescription, ulong publishedFileId, string[] tags, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, MyWorkshopItemPublisher> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
			m_publishSuccess = false;
			m_publishedItem = null;
			m_publishResult = MyGameServiceCallResult.Fail;
			HashSet<string> ignoredPaths = new HashSet<string>();
			ignoredPaths.Add("modinfo.sbmi");
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localModFolder, publishedTitle, publishedDescription, publishedFileId, visibility, tags, m_ignoredExecutableExtensions, ignoredPaths), endActionPublish));
		}

		public static ulong GetWorkshopIdFromLocalMod(string localModFolder)
		{
			string path = Path.Combine(MyFileSystem.ModsPath, localModFolder, "modinfo.sbmi");
			if (File.Exists(path) && MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_ModInfo objectBuilder))
			{
				return objectBuilder.WorkshopId;
			}
			return 0uL;
		}

		public static void PublishWorldAsync(string localWorldFolder, string publishedTitle, string publishedDescription, ulong? publishedFileId, string[] tags, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, MyWorkshopItemPublisher> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
			m_publishSuccess = false;
			m_publishedItem = null;
			m_publishResult = MyGameServiceCallResult.Fail;
			HashSet<string> ignoredExtensions = new HashSet<string>(m_ignoredExecutableExtensions);
			ignoredExtensions.Add(".xmlcache");
			ignoredExtensions.Add(".png");
			HashSet<string> ignoredPaths = new HashSet<string>();
			ignoredPaths.Add("Backup");
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, publishedFileId, visibility, tags, ignoredExtensions, ignoredPaths), endActionPublish));
		}

		public static void PublishBlueprintAsync(string localWorldFolder, string publishedTitle, string publishedDescription, ulong? publishedFileId, string[] tags, uint[] requiredDLCs, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, MyWorkshopItemPublisher> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
			m_publishSuccess = false;
			m_publishedItem = null;
			m_publishResult = MyGameServiceCallResult.Fail;
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, publishedFileId, visibility, tags, m_ignoredExecutableExtensions, null, requiredDLCs), endActionPublish));
		}

		public static void PublishScenarioAsync(string localWorldFolder, string publishedTitle, string publishedDescription, ulong? publishedFileId, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, MyWorkshopItemPublisher> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
			m_publishSuccess = false;
			m_publishedItem = null;
			m_publishResult = MyGameServiceCallResult.Fail;
			string[] tags = new string[1]
			{
				"scenario"
			};
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, publishedFileId, visibility, tags, m_ignoredExecutableExtensions), endActionPublish));
		}

		public static void PublishIngameScriptAsync(string localWorldFolder, string publishedTitle, string publishedDescription, ulong? publishedFileId, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, MyWorkshopItemPublisher> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
			m_publishSuccess = false;
			m_publishedItem = null;
			m_publishResult = MyGameServiceCallResult.Fail;
			string[] tags = new string[1]
			{
				"ingameScript"
			};
			HashSet<string> ignoredExtensions = new HashSet<string>(m_ignoredExecutableExtensions);
			ignoredExtensions.Add(".sbmi");
			ignoredExtensions.Add(".png");
			ignoredExtensions.Add(".jpg");
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, publishedFileId, visibility, tags, ignoredExtensions), endActionPublish));
		}

		private static MyWorkshopItemPublisher PublishItemBlocking(string localFolder, string publishedTitle, string publishedDescription, ulong? workshopId, MyPublishedFileVisibility visibility, string[] tags, HashSet<string> ignoredExtensions = null, HashSet<string> ignoredPaths = null, uint[] requiredDLCs = null)
		{
			MySandboxGame.Log.WriteLine("PublishItemBlocking - START");
			MySandboxGame.Log.IncreaseIndent();
			if (tags.Length == 0)
			{
				MySandboxGame.Log.WriteLine("Error: Can not publish with no tags!");
				MySandboxGame.Log.DecreaseIndent();
				MySandboxGame.Log.WriteLine("PublishItemBlocking - END");
				return null;
			}
			if (!MyGameService.IsActive && !MyGameService.IsOnline)
			{
				return null;
			}
			MyWorkshopItemPublisher workshopItem = MyGameService.CreateWorkshopPublisher();
			if (workshopItem == null)
			{
				return null;
			}
			if (workshopId.HasValue)
			{
				List<MyWorkshopItem> list = new List<MyWorkshopItem>();
				if (GetItemsBlockingUGC(new ulong[1]
				{
					workshopId.Value
				}, list))
				{
					MyWorkshopItem myWorkshopItem = list.FirstOrDefault((MyWorkshopItem wi) => wi.Id == workshopId.Value);
					if (myWorkshopItem != null)
					{
						publishedTitle = myWorkshopItem.Title;
					}
				}
			}
			workshopItem.Title = publishedTitle;
			workshopItem.Description = publishedDescription;
			workshopItem.Visibility = visibility;
			workshopItem.Tags = new List<string>(tags);
			workshopItem.Id = (workshopId ?? 0);
			string filename = Path.Combine(localFolder, "metadata.mod");
			MyModMetadata mod = MyModMetadataLoader.Load(filename);
			CheckAndFixModMetadata(ref mod);
			MyModMetadataLoader.Save(filename, mod);
			bool flag = CheckModFolder(ref localFolder, ignoredExtensions, ignoredPaths);
			workshopItem.Folder = localFolder;
			workshopItem.Metadata = mod;
			string[] previewFileNames = m_previewFileNames;
			foreach (string path in previewFileNames)
			{
				string text = Path.Combine(localFolder, path);
				if (File.Exists(text))
				{
					workshopItem.Thumbnail = text;
					break;
				}
			}
			try
			{
				AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
				try
				{
					workshopItem.ItemPublished += delegate(MyGameServiceCallResult result, ulong id)
					{
						m_publishResult = result;
						if (result == MyGameServiceCallResult.OK)
						{
							MySandboxGame.Log.WriteLine("Published file update successful");
						}
						else
						{
							MySandboxGame.Log.WriteLine($"Error during publishing: {result}");
						}
						m_publishSuccess = (result == MyGameServiceCallResult.OK);
						m_publishedItem = workshopItem;
						resetEvent.Set();
					};
					if (requiredDLCs != null)
					{
						foreach (uint item in requiredDLCs)
						{
							workshopItem.DLCs.Add(item);
						}
					}
					workshopItem.Publish();
					if (!resetEvent.WaitOne())
					{
						return null;
					}
				}
				finally
				{
					if (resetEvent != null)
					{
						((IDisposable)resetEvent).Dispose();
					}
				}
			}
			finally
			{
				if (flag && localFolder.StartsWith(Path.GetTempPath()))
				{
					Directory.Delete(localFolder, recursive: true);
				}
			}
			return workshopItem;
		}

		private static bool CheckModFolder(ref string localFolder, HashSet<string> ignoredExtensions, HashSet<string> ignoredPaths)
		{
			if ((ignoredExtensions == null || ignoredExtensions.Count == 0) && (ignoredPaths == null || ignoredPaths.Count == 0))
			{
				return false;
			}
			string tempPath = Path.GetTempPath();
			string path = $"{Process.GetCurrentProcess().Id}-{Path.GetFileName(localFolder)}";
			string text = Path.Combine(tempPath, path);
			if (Directory.Exists(text))
			{
				Directory.Delete(text, recursive: true);
			}
			localFolder = MyFileSystem.TerminatePath(localFolder);
			int sourcePathLength = localFolder.Length;
			MyFileSystem.CopyAll(localFolder, text, delegate(string s)
			{
				if (ignoredExtensions != null)
				{
					string extension = Path.GetExtension(s);
					if (extension != null && ignoredExtensions.Contains(extension))
					{
						return false;
					}
				}
				if (ignoredPaths != null)
				{
					string item = s.Substring(sourcePathLength);
					if (ignoredPaths.Contains(item))
					{
						return false;
					}
				}
				return true;
			});
			localFolder = text;
			return true;
		}

		public static MyModCompatibility CheckModCompatibility(string localFullPath)
		{
			if (string.IsNullOrWhiteSpace(localFullPath))
			{
				return MyModCompatibility.Unknown;
			}
			string text = Path.Combine(localFullPath, "metadata.mod");
			if (!MyFileSystem.FileExists(text))
			{
				return MyModCompatibility.Unknown;
			}
			return CheckModCompatibility(MyModMetadataLoader.Load(text));
		}

		public static MyModCompatibility CheckModCompatibility(MyModMetadata mod)
		{
			return MyModCompatibility.Ok;
		}

		private static void CheckAndFixModMetadata(ref MyModMetadata mod)
		{
			if (mod == null)
			{
				mod = new MyModMetadata();
			}
			if (mod.ModVersion == null)
			{
				mod.ModVersion = new Version(1, 0);
			}
		}

		private static void endActionPublish(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			screen.CloseScreenNow();
			if (m_onPublishingFinished != null)
			{
				m_onPublishingFinished(m_publishSuccess, m_publishResult, m_publishedItem);
			}
			m_publishSuccess = false;
			m_publishResult = MyGameServiceCallResult.Fail;
			m_onPublishingFinished = null;
			m_asyncPublishScreen = null;
		}

		public static bool GetSubscribedWorldsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1]
				{
					"world"
				});
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - END");
			}
		}

		public static bool GetSubscribedCampaignsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1]
				{
					"campaign"
				});
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - END");
			}
		}

		public static bool GetSubscribedModsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1]
				{
					"mod"
				});
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
			}
		}

		public static bool GetSubscribedScenariosBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedScenariosBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1]
				{
					"scenario"
				});
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedScenariosBlocking - END");
			}
		}

		public static bool GetSubscribedBlueprintsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1]
				{
					"blueprint"
				});
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
			}
		}

		public static bool GetSubscribedIngameScriptsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1]
				{
					"ingameScript"
				});
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
			}
		}

		public static bool GetItemsBlockingUGC(IEnumerable<ulong> publishedFileIds, List<MyWorkshopItem> resultDestination)
		{
			ulong[] array = publishedFileIds.Distinct().ToArray();
			MySandboxGame.Log.WriteLine($"MyWorkshop.GetItemsBlocking: getting {array.Length} items");
			resultDestination.Clear();
			if (!MyGameService.IsOnline && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return false;
			}
			if (array.Length == 0)
			{
				return true;
			}
			MyWorkshopQuery myWorkshopQuery = MyGameService.CreateWorkshopQuery();
			myWorkshopQuery.ItemIds = new List<ulong>(array);
			AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
			try
			{
				myWorkshopQuery.QueryCompleted += delegate(MyGameServiceCallResult result)
				{
					if (result == MyGameServiceCallResult.OK)
					{
						MySandboxGame.Log.WriteLine("Mod query successful");
					}
					else
					{
						MySandboxGame.Log.WriteLine($"Error during mod query: {result}");
					}
					resetEvent.Set();
				};
				myWorkshopQuery.Run();
				if (!resetEvent.WaitOne())
				{
					return false;
				}
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			if (myWorkshopQuery.Items != null)
			{
				resultDestination.AddRange(myWorkshopQuery.Items);
			}
			return true;
		}

		private static bool GetSubscribedItemsBlockingUGC(List<MyWorkshopItem> results, IEnumerable<string> tags)
		{
			results.Clear();
			if (!MyGameService.IsActive)
			{
				return false;
			}
			MyWorkshopQuery myWorkshopQuery = MyGameService.CreateWorkshopQuery();
			myWorkshopQuery.UserId = Sync.MyId;
			if (tags != null)
			{
				if (myWorkshopQuery.RequiredTags == null)
				{
					myWorkshopQuery.RequiredTags = new List<string>();
				}
				myWorkshopQuery.RequiredTags.AddRange(tags);
			}
			AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
			try
			{
				myWorkshopQuery.QueryCompleted += delegate(MyGameServiceCallResult result)
				{
					if (result == MyGameServiceCallResult.OK)
					{
						MySandboxGame.Log.WriteLine("Query successful.");
					}
					else
					{
						MySandboxGame.Log.WriteLine($"Error during UGC query: {result}");
					}
					resetEvent.Set();
				};
				myWorkshopQuery.Run();
				if (!resetEvent.WaitOne())
				{
					return false;
				}
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			if (myWorkshopQuery.Items != null)
			{
				results.AddRange(myWorkshopQuery.Items);
			}
			return true;
		}

		public static void DownloadModsAsync(List<MyObjectBuilder_Checkpoint.ModItem> mods, Action<bool> onFinishedCallback, Action onCancelledCallback = null)
		{
			if (mods == null || mods.Count == 0)
			{
				onFinishedCallback(obj: true);
				return;
			}
			if (!Directory.Exists(m_workshopModsPath))
			{
				Directory.CreateDirectory(m_workshopModsPath);
			}
			m_cancelTokenDownloadMods = new CancelToken();
			m_downloadScreen = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(MyTexts.GetString(MyCommonTexts.ProgressTextCheckingMods)), new StringBuilder(MyTexts.GetString(MyCommonTexts.DownloadingMods)), MyCommonTexts.Cancel, null, null, null, delegate
			{
				m_cancelTokenDownloadMods.Cancel = true;
				if (onCancelledCallback != null)
				{
					onCancelledCallback();
				}
			});
			m_downloadScreen.Closed += OnDownloadScreenClosed;
			m_downloadResult = new DownloadModsResult(mods, onFinishedCallback, m_cancelTokenDownloadMods);
			MyGuiSandbox.AddScreen(m_downloadScreen);
		}

		private static void OnDownloadScreenClosed(MyGuiScreenBase source)
		{
			if (m_cancelTokenDownloadMods != null)
			{
				m_cancelTokenDownloadMods.Cancel = true;
			}
		}

		private static void endActionDownloadMods()
		{
			m_downloadScreen.CloseScreen();
			if (!m_downloadResult.Result.Success)
			{
				MySandboxGame.Log.WriteLine($"Error downloading mods");
			}
			m_downloadResult.Callback(m_downloadResult.Result.Success);
		}

		public static ResultData DownloadModsBlockingUGC(List<MyWorkshopItem> mods, CancelToken cancelToken)
		{
			int num = 0;
			string numMods = mods.Count.ToString();
			CachingList<MyWorkshopItem> cachingList = new CachingList<MyWorkshopItem>();
			CachingList<MyWorkshopItem> cachingList2 = new CachingList<MyWorkshopItem>();
			List<KeyValuePair<MyWorkshopItem, string>> list = new List<KeyValuePair<MyWorkshopItem, string>>();
			bool flag = false;
			long timestamp = Stopwatch.GetTimestamp();
			double byteSize = 0.0;
			for (int i = 0; i < mods.Count; i++)
			{
				byteSize += (double)mods[i].Size;
			}
			string str = MyUtils.FormatByteSizePrefix(ref byteSize);
			str = byteSize.ToString("N1") + str + "B";
			double num2 = 0.0;
			foreach (MyWorkshopItem mod in mods)
			{
				if (!MyGameService.IsOnline)
				{
					flag = true;
				}
				else if (cancelToken != null && cancelToken.Cancel)
				{
					flag = true;
				}
				else
				{
					UpdateDownloadScreen(num, numMods, list, str, num2, mod);
					if (!UpdateMod(mod))
					{
						MySandboxGame.Log.WriteLineAndConsole($"Mod failed: Id = {mod.Id}, title = '{mod.Title}'");
						cachingList.Add(mod);
						flag = true;
						if (cancelToken != null)
						{
							cancelToken.Cancel = true;
						}
					}
					else
					{
						MySandboxGame.Log.WriteLineAndConsole($"Up to date mod: Id = {mod.Id}, title = '{mod.Title}'");
						if (m_downloadScreen != null)
						{
							using (m_modLock.AcquireExclusiveUsing())
							{
								num2 += (double)mod.Size;
								num++;
								list.RemoveAll((KeyValuePair<MyWorkshopItem, string> e) => e.Key == mod);
							}
						}
					}
				}
			}
			long timestamp2 = Stopwatch.GetTimestamp();
			ResultData resultData;
			if (flag)
			{
				cachingList.ApplyChanges();
				if (cachingList.Count > 0)
				{
					foreach (MyWorkshopItem item in cachingList)
					{
						MySandboxGame.Log.WriteLineAndConsole($"Failed to download mod: Id = {item.Id}, title = '{item.Title}'");
					}
				}
				else if (cancelToken == null || !cancelToken.Cancel)
				{
					MySandboxGame.Log.WriteLineAndConsole($"Failed to download mods because Steam is not in Online Mode.");
				}
				else
				{
					MySandboxGame.Log.WriteLineAndConsole($"Failed to download mods because download was stopped.");
				}
				resultData = default(ResultData);
				return resultData;
			}
			cachingList2.ApplyChanges();
			resultData = default(ResultData);
			resultData.Success = true;
			resultData.MismatchMods = new List<MyWorkshopItem>(cachingList2);
			resultData.Mods = new List<MyWorkshopItem>(mods);
			ResultData result = resultData;
			double num3 = (double)(timestamp2 - timestamp) / (double)Stopwatch.Frequency;
			MySandboxGame.Log.WriteLineAndConsole($"Mod download time: {num3:0.00} seconds");
			return result;
		}

		private static void UpdateDownloadScreen(int counter, string numMods, List<KeyValuePair<MyWorkshopItem, string>> currentMods, string sizeStr, double runningTotal, MyWorkshopItem mod)
		{
			if (m_downloadScreen == null)
			{
				return;
			}
			StringBuilder loadingText = new StringBuilder();
			string text;
			if (mod.Title.Length <= 25)
			{
				text = mod.Title;
			}
			else
			{
				text = mod.Title.Substring(0, 25);
				int num = text.LastIndexOf(' ');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				text += "...";
			}
			using (m_modLock.AcquireExclusiveUsing())
			{
				double byteSize = runningTotal;
				string text2 = MyUtils.FormatByteSizePrefix(ref byteSize);
				double byteSize2 = mod.Size;
				string text3 = MyUtils.FormatByteSizePrefix(ref byteSize2);
				currentMods.Add(new KeyValuePair<MyWorkshopItem, string>(mod, text + " " + byteSize2.ToString("N1") + text3 + "B"));
				loadingText.Clear();
				loadingText.AppendLine();
				foreach (KeyValuePair<MyWorkshopItem, string> currentMod in currentMods)
				{
					loadingText.AppendLine(currentMod.Value);
				}
				loadingText.AppendLine(MyTexts.GetString(MyCommonTexts.DownloadingMods_Completed) + counter + "/" + numMods + " : " + byteSize.ToString("N1") + text2 + "B/" + sizeStr);
				MySandboxGame.Static.Invoke(delegate
				{
					using (m_modLock.AcquireExclusiveUsing())
					{
						m_downloadScreen.MessageText = loadingText;
					}
				}, "MySteamWorkshop::set loading text");
			}
		}

		private static void StartDownloadingMod(MyWorkshopItem mod)
		{
			mod.UpdateState();
			if (!mod.IsUpToDate())
			{
				mod.Download();
			}
		}

		public static bool DownloadScriptBlocking(MyWorkshopItem item)
		{
			if (!MyGameService.IsOnline)
			{
				return false;
			}
			if (!item.IsUpToDate())
			{
				if (!UpdateMod(item))
				{
					return false;
				}
			}
			else
			{
				MySandboxGame.Log.WriteLineAndConsole($"Up to date mod: Id = {item.Id}, title = '{item.Title}'");
			}
			return true;
		}

		public static bool DownloadBlueprintBlockingUGC(MyWorkshopItem item, bool check = true)
		{
			if (!check || !item.IsUpToDate())
			{
				if (!UpdateMod(item))
				{
					return false;
				}
			}
			else
			{
				MySandboxGame.Log.WriteLineAndConsole($"Up to date mod: Id = {item.Id}, title = '{item.Title}'");
			}
			return true;
		}

		public static bool IsUpToDate(MyWorkshopItem item)
		{
			if (!MyGameService.IsOnline)
			{
				return false;
			}
			item.UpdateState();
			return item.IsUpToDate();
		}

		public static ResultData DownloadWorldModsBlocking(List<MyObjectBuilder_Checkpoint.ModItem> mods, CancelToken cancelToken)
		{
			ResultData resultData = default(ResultData);
			resultData.Success = true;
			if (!MyFakes.ENABLE_WORKSHOP_MODS)
			{
				if (cancelToken != null)
				{
					resultData.Cancel = cancelToken.Cancel;
				}
				return resultData;
			}
			MySandboxGame.Log.WriteLineAndConsole("Downloading world mods - START");
			MySandboxGame.Log.IncreaseIndent();
			if (mods != null && mods.Count > 0)
			{
				List<ulong> list = new List<ulong>();
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
				{
					if (mod.PublishedFileId != 0L)
					{
						if (!list.Contains(mod.PublishedFileId))
						{
							list.Add(mod.PublishedFileId);
						}
					}
					else if (Sandbox.Engine.Platform.Game.IsDedicated)
					{
						MySandboxGame.Log.WriteLineAndConsole("Local mods are not allowed in multiplayer.");
						MySandboxGame.Log.DecreaseIndent();
						return default(ResultData);
					}
				}
				list.Sort();
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					if (MySandboxGame.ConfigDedicated.AutodetectDependencies)
					{
						CheckModDependencies(mods, list);
					}
					MyGameService.SetServerModTemporaryDirectory();
					resultData = DownloadModsBlocking(mods, resultData, list, cancelToken);
				}
				else
				{
					if (Sync.IsServer)
					{
						CheckModDependencies(mods, list);
					}
					resultData = DownloadModsBlocking(mods, resultData, list, cancelToken);
				}
			}
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLineAndConsole("Downloading world mods - END");
			if (cancelToken != null)
			{
				resultData.Cancel |= cancelToken.Cancel;
			}
			return resultData;
		}

		private static void CheckModDependencies(List<MyObjectBuilder_Checkpoint.ModItem> mods, List<ulong> publishedFileIds)
		{
			List<MyObjectBuilder_Checkpoint.ModItem> list = new List<MyObjectBuilder_Checkpoint.ModItem>();
			HashSet<ulong> hashSet = new HashSet<ulong>();
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
			{
				if (!mod.IsDependency)
				{
					if (mod.PublishedFileId == 0L)
					{
						list.Add(mod);
					}
					else
					{
						hashSet.Add(mod.PublishedFileId);
					}
				}
			}
			bool hasReferenceIssue;
			foreach (MyWorkshopItem item2 in GetModsDependencyHiearchy(hashSet, out hasReferenceIssue))
			{
				bool isDependency = !hashSet.Contains(item2.Id);
				MyObjectBuilder_Checkpoint.ModItem item = new MyObjectBuilder_Checkpoint.ModItem(item2.Id, isDependency);
				item.FriendlyName = item2.Title;
				list.Add(item);
				if (!publishedFileIds.Contains(item2.Id))
				{
					publishedFileIds.Add(item2.Id);
				}
			}
			mods.Clear();
			mods.AddRange(list);
		}

		public static List<MyWorkshopItem> GetModsDependencyHiearchy(HashSet<ulong> publishedFileIds, out bool hasReferenceIssue)
		{
			hasReferenceIssue = false;
			List<MyWorkshopItem> list = new List<MyWorkshopItem>();
			HashSet<ulong> hashSet = new HashSet<ulong>();
			List<ulong> list2 = new List<ulong>();
			Stack<ulong> stack = new Stack<ulong>();
			foreach (ulong publishedFileId in publishedFileIds)
			{
				stack.Push(publishedFileId);
			}
			while (stack.Count > 0)
			{
				while (stack.Count > 0)
				{
					ulong num = stack.Pop();
					if (!hashSet.Contains(num))
					{
						hashSet.Add(num);
						list2.Add(num);
					}
					else
					{
						hasReferenceIssue = true;
						MyLog.Default.WriteLineAndConsole("Reference issue detected (circular reference or wrong order) for mod " + num);
					}
				}
				if (list2.Count != 0)
				{
					List<MyWorkshopItem> modsInfo = GetModsInfo(list2);
					if (modsInfo != null)
					{
						foreach (MyWorkshopItem item2 in modsInfo)
						{
							list.Insert(0, item2);
							for (int num2 = item2.Dependencies.Count - 1; num2 >= 0; num2--)
							{
								ulong item = item2.Dependencies[num2];
								stack.Push(item);
							}
						}
					}
					list2.Clear();
				}
			}
			return list;
		}

		public static List<MyWorkshopItem> GetModsInfo(List<ulong> publishedFileIds)
		{
			MyWorkshopQuery myWorkshopQuery = MyGameService.CreateWorkshopQuery();
			myWorkshopQuery.ItemIds = publishedFileIds;
			AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
			try
			{
				myWorkshopQuery.QueryCompleted += delegate(MyGameServiceCallResult result)
				{
					if (result == MyGameServiceCallResult.OK)
					{
						MySandboxGame.Log.WriteLine("Mod dependencies query successful");
					}
					else
					{
						MySandboxGame.Log.WriteLine($"Error during mod dependencies query: {result}");
					}
					resetEvent.Set();
				};
				myWorkshopQuery.Run();
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					int num = 0;
					while (!resetEvent.WaitOne(1))
					{
						MyGameService.Update();
						num++;
						if (num > m_dependenciesRequestTimeout)
						{
							myWorkshopQuery.Dispose();
							return null;
						}
					}
				}
				else if (!resetEvent.WaitOne(m_dependenciesRequestTimeout))
				{
					myWorkshopQuery.Dispose();
					return null;
				}
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			List<MyWorkshopItem> items = myWorkshopQuery.Items;
			myWorkshopQuery.Dispose();
			return items;
		}

		private static ResultData DownloadModsBlocking(List<MyObjectBuilder_Checkpoint.ModItem> mods, ResultData ret, List<ulong> publishedFileIds, CancelToken cancelToken)
		{
			List<MyWorkshopItem> list = new List<MyWorkshopItem>(publishedFileIds.Count);
			if (!GetItemsBlockingUGC(publishedFileIds, list))
			{
				MySandboxGame.Log.WriteLine("Could not obtain workshop item details");
				ret.Success = false;
			}
			else if (publishedFileIds.Count != list.Count)
			{
				MySandboxGame.Log.WriteLine($"Could not obtain all workshop item details, expected {publishedFileIds.Count}, got {list.Count}");
				ret.Success = false;
			}
			else
			{
				if (m_downloadScreen != null)
				{
					m_downloadScreen.MessageText = new StringBuilder(MyTexts.GetString(MyCommonTexts.ProgressTextDownloadingMods) + " 0 of " + list.Count);
				}
				ret = DownloadModsBlockingUGC(list, cancelToken);
				if (!ret.Success)
				{
					MySandboxGame.Log.WriteLine("Downloading mods failed");
				}
				else
				{
					MyObjectBuilder_Checkpoint.ModItem[] array = mods.ToArray();
					int i = 0;
					while (i < array.Length)
					{
						MyWorkshopItem myWorkshopItem = list.Find((MyWorkshopItem x) => x.Id == array[i].PublishedFileId);
						if (myWorkshopItem != null)
						{
							array[i].FriendlyName = myWorkshopItem.Title;
							array[i].SetModData(myWorkshopItem);
						}
						else
						{
							array[i].FriendlyName = array[i].Name;
						}
						int num = ++i;
					}
					mods.Clear();
					mods.AddRange(array);
				}
			}
			return ret;
		}

		private static bool UpdateMod(MyWorkshopItem mod)
		{
			mod.UpdateState();
			if (mod.IsUpToDate())
			{
				return true;
			}
			AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
			try
			{
				MyWorkshopItem.DownloadItemResult value = delegate(MyGameServiceCallResult result, ulong id)
				{
					switch (result)
					{
					case MyGameServiceCallResult.Pending:
						return;
					case MyGameServiceCallResult.OK:
						MySandboxGame.Log.WriteLine("Mod download successful.");
						break;
					default:
						MySandboxGame.Log.WriteLine($"Error during downloading: {result}");
						break;
					}
					resetEvent.Set();
				};
				mod.ItemDownloaded += value;
				mod.Download();
				resetEvent.WaitOne();
				mod.ItemDownloaded -= value;
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			return true;
		}

		public static bool GenerateModInfo(string modPath, ulong publishedFileId, ulong steamIDOwner)
		{
			MyObjectBuilder_ModInfo myObjectBuilder_ModInfo = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ModInfo>();
			myObjectBuilder_ModInfo.WorkshopId = publishedFileId;
			myObjectBuilder_ModInfo.SteamIDOwner = steamIDOwner;
			if (!MyObjectBuilderSerializer.SerializeXML(Path.Combine(modPath, "modinfo.sbmi"), compress: false, myObjectBuilder_ModInfo))
			{
				MySandboxGame.Log.WriteLine($"Error creating modinfo: workshopID={publishedFileId}, mod='{modPath}'");
				return false;
			}
			return true;
		}

		public static void CreateWorldInstanceAsync(MyWorkshopItem world, MyWorkshopPathInfo pathInfo, bool overwrite, Action<bool, string> callbackOnFinished = null)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextCreatingWorld, null, () => new CreateWorldResult(world, pathInfo, callbackOnFinished, overwrite), endActionCreateWorldInstance));
		}

		private static void endActionCreateWorldInstance(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			screen.CloseScreen();
			CreateWorldResult createWorldResult = (CreateWorldResult)result;
			createWorldResult.Callback?.Invoke(createWorldResult.Success, createWorldResult.m_createdSessionPath);
		}

		public static void UpdateWorldsAsync(List<MyWorkshopItem> worlds, MyWorkshopPathInfo pathInfo, Action<bool> callbackOnFinished = null)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, () => new UpdateWorldsResult(worlds, pathInfo, callbackOnFinished), endActionUpdateWorld));
		}

		private static void endActionUpdateWorld(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			screen.CloseScreen();
			UpdateWorldsResult updateWorldsResult = (UpdateWorldsResult)result;
			updateWorldsResult.Callback?.Invoke(updateWorldsResult.Success);
		}

		public static bool TryUpdateWorldsBlocking(List<MyWorkshopItem> worlds, MyWorkshopPathInfo pathInfo)
		{
			if (!Directory.Exists(pathInfo.Path))
			{
				Directory.CreateDirectory(pathInfo.Path);
			}
			if (!MyGameService.IsOnline)
			{
				return false;
			}
			bool flag = true;
			foreach (MyWorkshopItem world in worlds)
			{
				flag &= UpdateMod(world);
			}
			return flag;
		}

		public static bool TryCreateWorldInstanceBlocking(MyWorkshopItem world, MyWorkshopPathInfo pathInfo, out string sessionPath, bool overwrite)
		{
			if (!Directory.Exists(pathInfo.Path))
			{
				Directory.CreateDirectory(pathInfo.Path);
			}
			string text = MyUtils.StripInvalidChars(world.Title);
			sessionPath = null;
			Path.Combine(pathInfo.Path, world.Id + pathInfo.Suffix);
			if (!MyGameService.IsOnline)
			{
				return false;
			}
			if (!UpdateMod(world))
			{
				return false;
			}
			sessionPath = MyLocalCache.GetSessionSavesPath(text, contentFolder: false, createIfNotExists: false);
			if (overwrite && Directory.Exists(sessionPath))
			{
				Directory.Delete(sessionPath, recursive: true);
			}
			while (Directory.Exists(sessionPath))
			{
				sessionPath = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
			}
			if (MyFileSystem.IsDirectory(world.Folder))
			{
				MyFileSystem.CopyAll(world.Folder, sessionPath);
			}
			else
			{
				MyZipArchive.ExtractToDirectory(world.Folder, sessionPath);
			}
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
			if (myObjectBuilder_Checkpoint == null)
			{
				return false;
			}
			myObjectBuilder_Checkpoint.SessionName = $"({pathInfo.NamePrefix}) {world.Title}";
			myObjectBuilder_Checkpoint.LastSaveTime = DateTime.Now;
			myObjectBuilder_Checkpoint.WorkshopId = null;
			MyLocalCache.SaveCheckpoint(myObjectBuilder_Checkpoint, sessionPath);
			return true;
		}

		private static string GetErrorString(bool ioFailure, MyGameServiceCallResult result)
		{
			if (!ioFailure)
			{
				return result.ToString();
			}
			return "IO Failure";
		}

		public static bool CheckLocalModsAllowed(List<MyObjectBuilder_Checkpoint.ModItem> mods, bool allowLocalMods)
		{
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
			{
				if (mod.PublishedFileId == 0L && !allowLocalMods)
				{
					return false;
				}
			}
			return true;
		}

		public static bool CanRunOffline(List<MyObjectBuilder_Checkpoint.ModItem> mods)
		{
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
			{
				if (mod.PublishedFileId != 0L)
				{
					string path = Path.Combine(MyFileSystem.ModsPath, mod.Name);
					if (!Directory.Exists(path) && !File.Exists(path))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
