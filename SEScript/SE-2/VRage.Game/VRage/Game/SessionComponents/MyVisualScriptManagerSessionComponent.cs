using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting;
using VRage.Game.VisualScripting.ScriptBuilder;
using VRage.Utils;

namespace VRage.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 1000, typeof(MyObjectBuilder_VisualScriptManagerSessionComponent), null)]
	public class MyVisualScriptManagerSessionComponent : MySessionComponentBase
	{
		private static bool m_firstUpdate = true;

		private CachingList<IMyLevelScript> m_levelScripts;

		private MyObjectBuilder_VisualScriptManagerSessionComponent m_objectBuilder;

		private MyVSStateMachineManager m_smManager;

		private readonly Dictionary<string, string> m_relativePathsToAbsolute = new Dictionary<string, string>();

		private readonly List<string> m_stateMachineDefinitionFilePaths = new List<string>();

		private string[] m_runningLevelScriptNames;

		private string[] m_failedLevelScriptExceptionTexts;

		public CachingList<IMyLevelScript> LevelScripts => m_levelScripts;

		public MyVSStateMachineManager SMManager => m_smManager;

		public MyObjectBuilder_Questlog QuestlogData
		{
			get
			{
				if (m_objectBuilder != null)
				{
					return m_objectBuilder.Questlog;
				}
				return null;
			}
			set
			{
				if (m_objectBuilder != null)
				{
					m_objectBuilder.Questlog = value;
				}
			}
		}

		/// <summary>
		/// Array of running level script names.
		/// </summary>
		public string[] RunningLevelScriptNames => m_runningLevelScriptNames;

		/// <summary>
		/// Array of exceptions raised when the scripts were running.
		/// The name of the script is at the same index. (RunningLevelScriptNames)
		/// </summary>
		public string[] FailedLevelScriptExceptionTexts => m_failedLevelScriptExceptionTexts;

		/// <summary>
		/// Path a campaign repository in case of modded campaign.
		/// </summary>
		public string CampaignModPath
		{
			get;
			set;
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			if (Session.IsServer)
			{
				m_firstUpdate = (m_objectBuilder = (MyObjectBuilder_VisualScriptManagerSessionComponent)sessionComponent).FirstRun;
			}
		}

		public override void BeforeStart()
		{
			if (m_objectBuilder == null)
			{
				return;
			}
			m_relativePathsToAbsolute.Clear();
			m_stateMachineDefinitionFilePaths.Clear();
			if (m_objectBuilder.LevelScriptFiles != null)
			{
				string[] levelScriptFiles = m_objectBuilder.LevelScriptFiles;
				foreach (string text in levelScriptFiles)
				{
					MyContentPath myContentPath = Path.Combine(CampaignModPath ?? MyFileSystem.ContentPath, text);
					if (myContentPath.GetExitingFilePath() != null)
					{
						m_relativePathsToAbsolute.Add(text, myContentPath.GetExitingFilePath());
					}
					else
					{
						MyLog.Default.WriteLine(text + " Level Script was not found.");
					}
				}
			}
			if (m_objectBuilder.StateMachines != null)
			{
				string[] levelScriptFiles = m_objectBuilder.StateMachines;
				foreach (string text2 in levelScriptFiles)
				{
					MyContentPath myContentPath2 = Path.Combine(CampaignModPath ?? MyFileSystem.ContentPath, text2);
					if (myContentPath2.GetExitingFilePath() != null)
					{
						if (!m_relativePathsToAbsolute.ContainsKey(text2))
						{
							m_stateMachineDefinitionFilePaths.Add(myContentPath2.GetExitingFilePath());
						}
						m_relativePathsToAbsolute.Add(text2, myContentPath2.GetExitingFilePath());
					}
					else
					{
						MyLog.Default.WriteLine(text2 + " Mission File was not found.");
					}
				}
			}
			bool flag = false;
			if (Session.Mods != null)
			{
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in Session.Mods)
				{
					string text3 = mod.GetPath();
					if (!MyFileSystem.DirectoryExists(text3))
					{
						text3 = Path.Combine(MyFileSystem.ModsPath, mod.Name);
						if (!MyFileSystem.DirectoryExists(text3))
						{
							text3 = null;
						}
					}
					if (!string.IsNullOrEmpty(text3))
					{
						foreach (string file in MyFileSystem.GetFiles(text3, "*", MySearchOption.AllDirectories))
						{
							string extension = Path.GetExtension(file);
							string key = MyFileSystem.MakeRelativePath(Path.Combine(text3, "VisualScripts"), file);
							if (extension == ".vs" || extension == ".vsc")
							{
								if (m_relativePathsToAbsolute.ContainsKey(key))
								{
									m_relativePathsToAbsolute[key] = file;
									flag = true;
								}
								else
								{
									m_relativePathsToAbsolute.Add(key, file);
								}
							}
						}
					}
				}
			}
			bool isScriptCompilationSupported = MyVRage.Platform.IsScriptCompilationSupported;
			bool flag2 = false;
			if (!flag)
			{
				try
				{
					flag2 = MyVSAssemblyProvider.TryLoad("VisualScripts", isScriptCompilationSupported);
				}
				catch (FileNotFoundException)
				{
				}
				catch (Exception ex2)
				{
					MyLog.Default.Error("Cannot load visual script assembly: " + ex2.Message);
				}
			}
			if ((flag || !flag2) && MyVRage.Platform.IsScriptCompilationSupported)
			{
				MyVSAssemblyProvider.Init(m_relativePathsToAbsolute.Values, CampaignModPath);
			}
			else if (!flag2)
			{
				MyLog.Default.Error("Precompiled mod scripts assembly was not loaded. Scripts will not function.");
				return;
			}
			m_levelScripts = new CachingList<IMyLevelScript>();
			MyVSAssemblyProvider.GetLevelScriptInstances(new HashSet<string>(m_objectBuilder.LevelScriptFiles?.Select(Path.GetFileNameWithoutExtension) ?? Enumerable.Empty<string>())).ForEach(delegate(IMyLevelScript script)
			{
				m_levelScripts.Add(script);
			});
			m_levelScripts.ApplyAdditions();
			m_runningLevelScriptNames = m_levelScripts.Select((IMyLevelScript x) => x.GetType().Name).ToArray();
			m_failedLevelScriptExceptionTexts = new string[m_runningLevelScriptNames.Length];
			m_smManager = new MyVSStateMachineManager();
			foreach (string stateMachineDefinitionFilePath in m_stateMachineDefinitionFilePaths)
			{
				m_smManager.AddMachine(stateMachineDefinitionFilePath);
			}
			if (m_objectBuilder != null && m_objectBuilder.ScriptStateMachineManager != null)
			{
				foreach (MyObjectBuilder_ScriptStateMachineManager.CursorStruct activeStateMachine in m_objectBuilder.ScriptStateMachineManager.ActiveStateMachines)
				{
					m_smManager.Restore(activeStateMachine.StateMachineName, activeStateMachine.Cursors);
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			if (Session.IsServer)
			{
				if (m_smManager != null)
				{
					m_smManager.Update();
				}
				if (m_levelScripts != null)
				{
					foreach (IMyLevelScript levelScript in m_levelScripts)
					{
						try
						{
							if (m_firstUpdate)
							{
								levelScript.GameStarted();
							}
							else
							{
								levelScript.Update();
							}
						}
						catch (Exception ex)
						{
							string name = levelScript.GetType().Name;
							for (int i = 0; i < m_runningLevelScriptNames.Length; i++)
							{
								if (m_runningLevelScriptNames[i] == name)
								{
									m_runningLevelScriptNames[i] += " - failed";
									m_failedLevelScriptExceptionTexts[i] = ex.ToString();
								}
							}
							m_levelScripts.Remove(levelScript);
						}
					}
					m_levelScripts.ApplyRemovals();
					m_firstUpdate = false;
				}
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			DisposeRunningScripts();
		}

		private void DisposeRunningScripts()
		{
			if (m_levelScripts != null)
			{
				foreach (IMyLevelScript levelScript in m_levelScripts)
				{
					levelScript.GameFinished();
					levelScript.Dispose();
				}
				m_smManager.Dispose();
				m_smManager = null;
				m_levelScripts.Clear();
				m_levelScripts.ApplyRemovals();
			}
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			if (!Session.IsServer)
			{
				return null;
			}
			m_objectBuilder.ScriptStateMachineManager = m_smManager?.GetObjectBuilder();
			m_objectBuilder.FirstRun = m_firstUpdate;
			return m_objectBuilder;
		}

		public void Reset()
		{
			if (m_smManager != null)
			{
				m_smManager.Dispose();
			}
			m_firstUpdate = true;
		}
	}
}
