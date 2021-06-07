using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui.DirectoryBrowser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage;
using VRage.FileSystem;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlSaveBrowser : MyGuiControlDirectoryBrowser
	{
		private readonly List<FileInfo> m_saveEntriesToCreate = new List<FileInfo>();

		private readonly Dictionary<string, MyWorldInfo> m_loadedWorldsByFilePaths = new Dictionary<string, MyWorldInfo>();

		private readonly HashSet<string> m_loadedDirectories = new HashSet<string>();

		public string SearchTextFilter;

		public bool InBackupsFolder
		{
			get;
			private set;
		}

		public MyGuiControlSaveBrowser()
			: base(MyFileSystem.SavesPath, MyFileSystem.SavesPath, null, (FileInfo info) => false)
		{
			SetColumnName(1, MyTexts.Get(MyCommonTexts.Date));
			SetColumnComparison(1, delegate(Cell cellA, Cell cellB)
			{
				if (cellA == null)
				{
					return -1;
				}
				if (cellB == null)
				{
					return -1;
				}
				FileInfo fileInfo = cellA.UserData as FileInfo;
				FileInfo fileInfo2 = cellB.UserData as FileInfo;
				if (fileInfo == fileInfo2)
				{
					if (fileInfo == null)
					{
						return 0;
					}
				}
				else
				{
					if (fileInfo == null)
					{
						return -1;
					}
					if (fileInfo2 == null)
					{
						return 1;
					}
				}
				return m_loadedWorldsByFilePaths[fileInfo.DirectoryName].LastSaveTime.CompareTo(m_loadedWorldsByFilePaths[fileInfo2.DirectoryName].LastSaveTime);
			});
		}

		public DirectoryInfo GetDirectory(Row row)
		{
			if (row == null)
			{
				return null;
			}
			return row.UserData as DirectoryInfo;
		}

		public Tuple<string, MyWorldInfo> GetSave(Row row)
		{
			if (row == null)
			{
				return null;
			}
			FileInfo fileInfo = row.UserData as FileInfo;
			if (fileInfo == null)
			{
				return null;
			}
			string directoryName = Path.GetDirectoryName(fileInfo.FullName);
			MyWorldInfo item = m_loadedWorldsByFilePaths[Path.GetDirectoryName(fileInfo.FullName)];
			return new Tuple<string, MyWorldInfo>(directoryName, item);
		}

		public void AccessBackups()
		{
			if (base.SelectedRow != null)
			{
				DirectoryInfo directoryInfo = (base.SelectedRow.UserData as FileInfo).Directory.GetDirectories().FirstOrDefault((DirectoryInfo dir) => dir.Name.StartsWith("Backup"));
				if (directoryInfo == null)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SaveBrowserMissingBackup), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
					return;
				}
				InBackupsFolder = true;
				base.CurrentDirectory = directoryInfo.FullName;
			}
		}

		protected override void AddFolderRow(DirectoryInfo dir)
		{
			if (!SearchFilterTest(dir.Name))
			{
				return;
			}
			FileInfo[] files = dir.GetFiles();
			bool flag = false;
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				if (fileInfo.Name == "Sandbox.sbc")
				{
					if (m_loadedWorldsByFilePaths.ContainsKey(fileInfo.DirectoryName))
					{
						m_saveEntriesToCreate.Add(fileInfo);
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				base.AddFolderRow(dir);
			}
		}

		public override void Refresh()
		{
			RefreshTheWorldInfos();
		}

		public void ForceRefresh()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, StartLoadingWorldInfos, OnLoadingFinished));
		}

		private void RefreshAfterLoaded()
		{
			base.Refresh();
			m_saveEntriesToCreate.Sort((FileInfo fileA, FileInfo fileB) => m_loadedWorldsByFilePaths[fileB.DirectoryName].LastSaveTime.CompareTo(m_loadedWorldsByFilePaths[fileA.DirectoryName].LastSaveTime));
			foreach (FileInfo item in m_saveEntriesToCreate)
			{
				AddSavedGame(item);
			}
			m_saveEntriesToCreate.Clear();
		}

		private void AddSavedGame(FileInfo fileInfo)
		{
			MyWorldInfo myWorldInfo = m_loadedWorldsByFilePaths[fileInfo.DirectoryName];
			if (SearchFilterTest(myWorldInfo.SessionName))
			{
				Row row = new Row(fileInfo);
				Cell cell = new Cell(myWorldInfo.SessionName, fileInfo, null, null, base.FileCellIconTexture, base.FileCellIconAlign);
				if (myWorldInfo.IsCorrupted)
				{
					cell.TextColor = Color.Red;
				}
				row.AddCell(cell);
				row.AddCell(new Cell(myWorldInfo.LastSaveTime.ToString(), fileInfo));
				Add(row);
			}
		}

		private void RefreshTheWorldInfos()
		{
			if (!m_loadedDirectories.Contains(base.CurrentDirectory))
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, StartLoadingWorldInfos, OnLoadingFinished));
			}
			else
			{
				RefreshAfterLoaded();
			}
		}

		private bool SearchFilterTest(string testString)
		{
			if (SearchTextFilter != null && SearchTextFilter.Length != 0)
			{
				string[] array = SearchTextFilter.Split(new char[1]
				{
					' '
				});
				string text = testString.ToLower();
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!text.Contains(text2.ToLower()))
					{
						return false;
					}
				}
			}
			return true;
		}

		private IMyAsyncResult StartLoadingWorldInfos()
		{
			return new MyLoadWorldInfoListResult(base.CurrentDirectory);
		}

		private void OnLoadingFinished(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			MyLoadListResult myLoadListResult = (MyLoadListResult)result;
			m_loadedDirectories.Add(base.CurrentDirectory);
			foreach (Tuple<string, MyWorldInfo> availableSafe in myLoadListResult.AvailableSaves)
			{
				m_loadedWorldsByFilePaths[availableSafe.Item1] = availableSafe.Item2;
			}
			if (myLoadListResult.ContainsCorruptedWorlds)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SomeWorldFilesCouldNotBeLoaded), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			RefreshAfterLoaded();
			screen.CloseScreen();
		}

		protected override void OnBackDoubleclicked()
		{
			if (m_currentDir.Name.StartsWith("Backup"))
			{
				base.CurrentDirectory = m_currentDir.Parent.Parent.FullName;
				InBackupsFolder = false;
				base.IgnoreFirstRowForSort = false;
			}
			else
			{
				base.OnBackDoubleclicked();
			}
		}
	}
}
