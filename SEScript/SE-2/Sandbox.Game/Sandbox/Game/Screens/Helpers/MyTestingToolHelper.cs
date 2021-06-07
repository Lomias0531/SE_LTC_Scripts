using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Screens.Helpers.InputRecording;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.Components;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyTestingToolHelper
	{
		public enum EnumTestingToolHelperStateOuter
		{
			Disabled,
			Idle,
			Action_1
		}

		protected class MyBlockTestGenerationState
		{
			public int SessionOrder;

			public int SessionOrder_Max;

			public List<string> UsedKeys = new List<string>();

			public DateTime TestStart = DateTime.UtcNow;

			public string CurrentBlockName = string.Empty;

			public string BasePath = string.Empty;

			public string ResultSaveName = string.Empty;

			public string ResultTestCaseName = string.Empty;

			public string SourceSaveWithBlockPath = string.Empty;

			public string ResultTestCaseSavePath = string.Empty;
		}

		private static MyTestingToolHelper m_instance;

		private MyBlockTestGenerationState BTGS;

		private bool m_syncRendering;

		private bool m_smallBlock;

		private bool m_isSaving;

		private bool m_savingFinished;

		private bool m_isLoading;

		private bool m_loadingFinished;

		private EnumTestingToolHelperStateOuter m_stateOuter;

		private int m_stateInner;

		private int m_stateMicro;

		private int m_timer;

		private readonly int m_timer_Max = 100;

		public static MyTestingToolHelper Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyTestingToolHelper();
				}
				return m_instance;
			}
		}

		public EnumTestingToolHelperStateOuter StateOuter
		{
			get
			{
				return m_stateOuter;
			}
			set
			{
				if (CanChangeOuterStateTo(value))
				{
					m_stateOuter = value;
					m_stateInner = 0;
					m_stateMicro = 0;
					OnStateOuterUpdate();
				}
			}
		}

		public bool IsEnabled
		{
			get;
			private set;
		}

		public bool IsIdle
		{
			get;
			private set;
		}

		public bool NeedsUpdate
		{
			get;
			private set;
		}

		public static string ScreenshotsDir
		{
			get
			{
				string path = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path, "Screenshots");
			}
		}

		private static string UserSaveFolder
		{
			get
			{
				bool flag = false;
				string str = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), str + (flag ? "Dedicated" : ""), "Saves");
			}
		}

		private static string TestCasesDir
		{
			get
			{
				bool flag = false;
				string str = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), str + (flag ? "Dedicated" : ""), "TestCases");
			}
		}

		public static string GameLogPath
		{
			get
			{
				bool flag = false;
				string str = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), str + (flag ? "Dedicated" : ""), str + (flag ? "Dedicated" : "") + ".log");
			}
		}

		public static string ConfigFile
		{
			get
			{
				string text = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), text, text + ".cfg");
			}
		}

		public static string TestResultFilename => "result";

		public static string LastTestRunResultFilename => "last_test_run";

		public bool CanChangeOuterStateTo(EnumTestingToolHelperStateOuter value)
		{
			if (m_stateOuter == value)
			{
				return false;
			}
			switch (m_stateOuter)
			{
			case EnumTestingToolHelperStateOuter.Disabled:
				return false;
			case EnumTestingToolHelperStateOuter.Idle:
				return true;
			case EnumTestingToolHelperStateOuter.Action_1:
				if (value == EnumTestingToolHelperStateOuter.Idle || value == EnumTestingToolHelperStateOuter.Disabled)
				{
					return true;
				}
				return false;
			default:
				return true;
			}
		}

		private void ChangeStateOuter_Force(EnumTestingToolHelperStateOuter value)
		{
			if (m_stateOuter != value)
			{
				m_stateOuter = value;
				OnStateOuterUpdate();
			}
		}

		private MyTestingToolHelper()
		{
			ChangeStateOuter_Force(EnumTestingToolHelperStateOuter.Idle);
		}

		public void Update()
		{
			if (!NeedsUpdate)
			{
				return;
			}
			m_timer--;
			if (m_timer < 0)
			{
				m_timer = m_timer_Max;
				switch (StateOuter)
				{
				case EnumTestingToolHelperStateOuter.Disabled:
				case EnumTestingToolHelperStateOuter.Idle:
					break;
				case EnumTestingToolHelperStateOuter.Action_1:
					Update_Action1();
					break;
				}
			}
		}

		private void Update_Action1()
		{
			switch (m_stateInner)
			{
			case 0:
				Action1_State0_PrepareBase();
				break;
			case 1:
				Action1_State1_SpawningCyclePreparation();
				break;
			case 2:
				Action1_State2_SpawningCycle();
				break;
			case 3:
				Action1_State3_Finish();
				break;
			}
		}

		public void OnStateOuterUpdate()
		{
			IsEnabled = (m_stateOuter != EnumTestingToolHelperStateOuter.Disabled);
			IsIdle = (m_stateOuter == EnumTestingToolHelperStateOuter.Idle);
			NeedsUpdate = (IsEnabled && !IsIdle);
		}

		public void Action_SpawnBlockSaveTestReload()
		{
			StateOuter = EnumTestingToolHelperStateOuter.Action_1;
		}

		public void Action_Test()
		{
			MyCubeBlockDefinition large = MyDefinitionManager.Static.GetDefinitionGroup("Monolith").Large;
			MyCubeBuilder component = MySession.Static.GetComponent<MyCubeBuilder>();
			if (component != null)
			{
				MatrixD worldMatrixAdd = MatrixD.Identity;
				component.AddBlocksToBuildQueueOrSpawn(large, ref worldMatrixAdd, new Vector3I(0, 0, 0), new Vector3I(0, 0, 0), new Vector3I(0, 0, 0), Quaternion.Identity);
			}
		}

		private bool SpawnBlockAtCenter(MyCubeBlockDefinition def)
		{
			return MySession.Static.GetComponent<MyCubeBuilder>()?.AddBlocksToBuildQueueOrSpawn(def, ref MatrixD.Identity, new Vector3I(0, 0, 0), new Vector3I(0, 0, 0), new Vector3I(0, 0, 0), Quaternion.Identity) ?? false;
		}

		private void Action1_State0_PrepareBase()
		{
			MySession.Static.Name = MySession.Static.Name.Replace(":", "-") + "_BlockTests";
			SaveAs(MySession.Static.Name);
			m_stateInner = 1;
		}

		private void Action1_State1_SpawningCyclePreparation()
		{
			if (!m_isSaving && m_savingFinished)
			{
				BTGS = new MyBlockTestGenerationState();
				BTGS.BasePath = MySession.Static.CurrentPath;
				BTGS.SessionOrder = -1;
				BTGS.SessionOrder_Max = MyDefinitionManager.Static.GetDefinitionPairNames().Count * 2;
				BTGS.UsedKeys.Clear();
				BTGS.CurrentBlockName = string.Empty;
				m_smallBlock = false;
				m_stateInner = 2;
			}
		}

		private void Action1_State2_SpawningCycle()
		{
			switch (m_stateMicro)
			{
			case 0:
				BTGS.SessionOrder++;
				BTGS.TestStart = DateTime.UtcNow;
				Load(BTGS.BasePath);
				BTGS.ResultTestCaseName = string.Empty;
				m_stateMicro = 1;
				break;
			case 1:
			{
				if (!ConsumeLoadingCompletion())
				{
					break;
				}
				if (!m_smallBlock)
				{
					bool flag = false;
					foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
					{
						if (!BTGS.UsedKeys.Contains(definitionPairName))
						{
							BTGS.CurrentBlockName = definitionPairName;
							m_smallBlock = false;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						m_stateInner = 3;
						break;
					}
				}
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(BTGS.CurrentBlockName);
				MyCubeBlockDefinition myCubeBlockDefinition = null;
				if (definitionGroup != null)
				{
					if (m_smallBlock)
					{
						myCubeBlockDefinition = definitionGroup.Small;
					}
					else
					{
						myCubeBlockDefinition = definitionGroup.Large;
						BTGS.UsedKeys.Add(BTGS.CurrentBlockName);
					}
				}
				if (myCubeBlockDefinition != null)
				{
					SpawnBlockAtCenter(myCubeBlockDefinition);
					BTGS.ResultSaveName = MySession.Static.Name + "_" + BTGS.CurrentBlockName + (m_smallBlock ? "_S" : "_L");
					BTGS.ResultTestCaseName = BTGS.ResultSaveName + (m_syncRendering ? "-sync" : "-async");
					m_stateMicro = 2;
				}
				else
				{
					m_stateMicro = 0;
				}
				m_smallBlock = !m_smallBlock;
				break;
			}
			case 2:
				SaveAs(BTGS.ResultSaveName);
				m_stateMicro = 3;
				break;
			case 3:
				if (ConsumeSavingCompletion())
				{
					if (m_syncRendering)
					{
						AddSyncRenderingToCfg(m_syncRendering);
					}
					BTGS.SourceSaveWithBlockPath = MySession.Static.CurrentPath;
					BTGS.ResultTestCaseSavePath = Path.Combine(TestCasesDir, BTGS.ResultTestCaseName, Path.GetFileName(BTGS.SourceSaveWithBlockPath));
					Directory.CreateDirectory(Path.Combine(TestCasesDir, BTGS.ResultTestCaseName));
					DirectoryCopy(MySession.Static.CurrentPath, BTGS.ResultTestCaseSavePath, copySubDirs: true);
					File.Copy(ConfigFile, Path.Combine(TestCasesDir, BTGS.ResultTestCaseName, "SpaceEngineers.cfg"), overwrite: true);
					m_stateMicro = 4;
				}
				break;
			case 4:
				MySessionComponentReplay.Static.StartReplay();
				m_stateMicro = 5;
				break;
			case 5:
			{
				PerFrameData perFrameData;
				if (MySession.Static == null)
				{
					ClearTimer();
					m_stateInner = 3;
				}
				else if (MySessionComponentReplay.Static.IsEntityBeingReplayed(MySession.Static.ControlledEntity.Entity.GetTopMostParent().EntityId, out perFrameData))
				{
					ClearTimer();
				}
				else
				{
					MySessionComponentReplay.Static.StopReplay();
					m_stateMicro = 6;
				}
				break;
			}
			case 6:
				SaveAs(Path.Combine(MyFileSystem.SavesPath, "..", "TestingToolSave"));
				MyGuiSandbox.TakeScreenshot(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y, Path.Combine(Path.Combine(MyFileSystem.UserDataPath, "Screenshots"), "TestingToolResult.png"), ignoreSprites: true, showNotification: false);
				m_stateMicro = 7;
				break;
			case 7:
				if (ConsumeSavingCompletion())
				{
					CopyScreenshots(BTGS.ResultTestCaseName, BTGS.TestStart, isAddCase: true);
					CopyLastGamelog(BTGS.ResultTestCaseName, "result.log");
					CopyLastSave(BTGS.ResultTestCaseName, "result");
					MyInputRecording myInputRecording = new MyInputRecording();
					myInputRecording.Name = Path.Combine(TestCasesDir, BTGS.ResultTestCaseName);
					myInputRecording.Description = BTGS.SourceSaveWithBlockPath;
					myInputRecording.Session = MyInputRecordingSession.Specific;
					myInputRecording.SetStartingScreenDimensions(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
					myInputRecording.UseReplayInstead = true;
					myInputRecording.Save();
					m_stateMicro = 0;
				}
				break;
			}
		}

		private void Action1_State3_Finish()
		{
			BTGS = null;
			StateOuter = EnumTestingToolHelperStateOuter.Idle;
		}

		private void ClearTimer()
		{
			m_timer = 0;
		}

		private bool SaveAs(string name)
		{
			if (m_isSaving)
			{
				return false;
			}
			m_isSaving = true;
			MyAsyncSaving.Start(delegate
			{
				OnSaveAsComplete();
			}, name);
			return true;
		}

		public void OnSaveAsComplete()
		{
			m_savingFinished = true;
			m_isSaving = false;
		}

		private bool FakeSaveCompletion()
		{
			if (m_isSaving)
			{
				return false;
			}
			m_savingFinished = true;
			return true;
		}

		private bool ConsumeSavingCompletion()
		{
			if (m_isSaving || !m_savingFinished)
			{
				return false;
			}
			m_savingFinished = false;
			return true;
		}

		private bool Load(string path)
		{
			if (m_isLoading)
			{
				return false;
			}
			m_isLoading = true;
			MySessionLoader.LoadSingleplayerSession(path, delegate
			{
				OnLoadComplete();
			});
			return true;
		}

		public void OnLoadComplete()
		{
			m_loadingFinished = true;
			m_isLoading = false;
		}

		private bool FakeLoadCompletion()
		{
			if (m_isLoading)
			{
				return false;
			}
			m_loadingFinished = true;
			return true;
		}

		private bool ConsumeLoadingCompletion()
		{
			if (m_isLoading || !m_loadingFinished)
			{
				return false;
			}
			m_loadingFinished = false;
			return true;
		}

		public static void CopyScreenshots(string testFolder, DateTime startTime, bool isAddCase = false)
		{
			FileInfo[] files = new DirectoryInfo(ScreenshotsDir).GetFiles();
			List<string> list = new List<string>();
			int num = 0;
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				if (fileInfo.LastWriteTime >= startTime)
				{
					list.Add(fileInfo.FullName);
					File.Copy(fileInfo.FullName, Path.Combine(TestCasesDir, testFolder, LastTestRunResultFilename + num + ".png"), overwrite: true);
					if (isAddCase)
					{
						File.Copy(fileInfo.FullName, Path.Combine(TestCasesDir, testFolder, TestResultFilename + num + ".png"), overwrite: true);
					}
					File.Delete(fileInfo.FullName);
					num++;
				}
			}
			string text = Path.Combine(ScreenshotsDir, "TestingToolResult.png");
			if (File.Exists(text))
			{
				File.Copy(text, Path.Combine(TestCasesDir, testFolder, LastTestRunResultFilename + ".png"), overwrite: true);
				File.Delete(text);
			}
		}

		public static void CopyLastGamelog(string testFolder, string resultType)
		{
			File.Copy(GameLogPath, Path.Combine(TestCasesDir, testFolder, resultType), overwrite: true);
		}

		public void CopyLastSave(string testCasePath, string resultName)
		{
			string text = Path.Combine(UserSaveFolder, "TestingToolSave");
			string path = Path.Combine(TestCasesDir, testCasePath);
			if (File.Exists(Path.Combine(text, "Sandbox.sbc")))
			{
				File.Copy(Path.Combine(text, "Sandbox.sbc"), Path.Combine(path, resultName + ".sbc"), overwrite: true);
				File.Copy(Path.Combine(text, "SANDBOX_0_0_0_.sbs"), Path.Combine(path, resultName + ".sbs"), overwrite: true);
			}
			if (Directory.Exists(Path.Combine(text)))
			{
				new DirectoryInfo(Path.Combine(text)).Delete(recursive: true);
			}
		}

		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				string destFileName = Path.Combine(destDirName, fileInfo.Name);
				fileInfo.CopyTo(destFileName, overwrite: true);
			}
			if (copySubDirs)
			{
				DirectoryInfo[] array = directories;
				foreach (DirectoryInfo directoryInfo2 in array)
				{
					string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
					DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs);
				}
			}
		}

		public static void AddSyncRenderingToCfg(bool value, string cfgPath = null)
		{
			string[] array = File.ReadAllLines(ConfigFile);
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				flag = array[i].Contains("SyncRendering");
				if (flag)
				{
					array[i + 1] = "      <Value xsi:type=\"xsd:string\">" + value + "</Value>";
					break;
				}
			}
			if (!flag)
			{
				List<string> list = array.ToList();
				list.Insert(list.Count - 2, "    <item>");
				list.Insert(list.Count - 2, "      <Key xsi:type=\"xsd:string\">SyncRendering</Key>");
				list.Insert(list.Count - 2, "      <Value xsi:type=\"xsd:string\">" + value + "</Value>");
				list.Insert(list.Count - 2, "    </item>");
				array = list.ToArray();
			}
			File.Delete((cfgPath != null) ? cfgPath : ConfigFile);
			File.WriteAllLines((cfgPath != null) ? cfgPath : ConfigFile, array);
		}
	}
}
