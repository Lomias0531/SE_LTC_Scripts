using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Gui.DirectoryBrowser
{
	public class MyGuiControlDirectoryBrowser : MyGuiControlTable
	{
		protected readonly MyDirectoryChangeCancelEventArgs m_cancelEvent;

		protected DirectoryInfo m_topMostDir;

		protected DirectoryInfo m_currentDir;

		protected Row m_backRow;

		public Color? FolderCellColor
		{
			get;
			set;
		}

		public MyGuiHighlightTexture FolderCellIconTexture
		{
			get;
			set;
		}

		public MyGuiDrawAlignEnum FolderCellIconAlign
		{
			get;
			set;
		}

		public Color? FileCellColor
		{
			get;
			set;
		}

		public MyGuiHighlightTexture FileCellIconTexture
		{
			get;
			set;
		}

		public MyGuiDrawAlignEnum FileCellIconAlign
		{
			get;
			set;
		}

		public Predicate<DirectoryInfo> DirPredicate
		{
			get;
			private set;
		}

		public Predicate<FileInfo> FilePredicate
		{
			get;
			private set;
		}

		public string CurrentDirectory
		{
			get
			{
				return m_currentDir.FullName;
			}
			set
			{
				TraverseToDirectory(value);
			}
		}

		public event Action<MyDirectoryChangeCancelEventArgs> DirectoryChanging;

		public event Action<MyGuiControlDirectoryBrowser, string> DirectoryChanged;

		public event Action<MyGuiControlDirectoryBrowser, string> FileDoubleClick;

		public event Action<MyGuiControlDirectoryBrowser, string> DirectoryDoubleclick;

		public event Action<MyDirectoryChangeCancelEventArgs> DirectoryDoubleclicking;

		public MyGuiControlDirectoryBrowser(string topMostDirectory = null, string initialDirectory = null, Predicate<DirectoryInfo> dirPredicate = null, Predicate<FileInfo> filePredicate = null)
		{
			if (!string.IsNullOrEmpty(topMostDirectory))
			{
				m_topMostDir = new DirectoryInfo(topMostDirectory);
			}
			if (!string.IsNullOrEmpty(initialDirectory))
			{
				m_currentDir = new DirectoryInfo(initialDirectory);
			}
			else
			{
				m_currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
			}
			DirPredicate = dirPredicate;
			FilePredicate = filePredicate;
			FolderCellIconTexture = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL;
			FolderCellIconAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			base.ItemDoubleClicked += OnItemDoubleClicked;
			base.ItemConfirmed += OnItemDoubleClicked;
			base.ColumnsCount = 2;
			SetCustomColumnWidths(new float[2]
			{
				0.65f,
				0.35f
			});
			SetColumnName(0, MyTexts.Get(MyCommonTexts.Name));
			SetColumnName(1, MyTexts.Get(MyCommonTexts.Created));
			SetColumnComparison(0, (Cell cellA, Cell cellB) => cellA.Text.CompareToIgnoreCase(cellB.Text));
			SetColumnComparison(1, (Cell cellA, Cell cellB) => cellB.Text.CompareToIgnoreCase(cellA.Text));
			m_cancelEvent = new MyDirectoryChangeCancelEventArgs(null, null, this);
			Refresh();
		}

		public virtual void Refresh()
		{
			Clear();
			DirectoryInfo[] directories = m_currentDir.GetDirectories();
			FileInfo[] files = m_currentDir.GetFiles();
			if (!m_topMostDir.FullName.TrimEnd(new char[1]
			{
				Path.DirectorySeparatorChar
			}).Equals(m_currentDir.FullName, StringComparison.OrdinalIgnoreCase))
			{
				AddBackRow();
			}
			DirectoryInfo[] array = directories;
			foreach (DirectoryInfo directoryInfo in array)
			{
				if (DirPredicate == null || DirPredicate(directoryInfo))
				{
					AddFolderRow(directoryInfo);
				}
			}
			FileInfo[] array2 = files;
			foreach (FileInfo fileInfo in array2)
			{
				if (FilePredicate == null || FilePredicate(fileInfo))
				{
					AddFileRow(fileInfo);
				}
			}
			ScrollToSelection();
		}

		protected virtual void AddFileRow(FileInfo file)
		{
			Row row = new Row(file);
			row.AddCell(new Cell(file.Name, file, null, null, FileCellIconTexture, FileCellIconAlign));
			row.AddCell(new Cell(file.CreationTime.ToString()));
			Add(row);
		}

		protected virtual void AddFolderRow(DirectoryInfo dir)
		{
			Row row = new Row(dir);
			row.AddCell(new Cell(dir.Name, dir, null, null, FolderCellIconTexture, FolderCellIconAlign));
			row.AddCell(new Cell(string.Empty));
			Add(row);
		}

		protected virtual void AddBackRow()
		{
			if (m_backRow == null)
			{
				m_backRow = new Row();
				m_backRow.AddCell(new Cell("..", null, null, null, MyGuiConstants.TEXTURE_BLUEPRINTS_ARROW, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			Add(m_backRow);
			base.IgnoreFirstRowForSort = true;
		}

		private void TraverseToDirectory(string path)
		{
			if (!(path == m_currentDir.FullName) && (m_topMostDir == null || m_topMostDir.IsParentOf(path)) && !NotifyDirectoryChanging(m_currentDir.FullName, path))
			{
				m_currentDir = new DirectoryInfo(path);
				Refresh();
				if (this.DirectoryChanged != null)
				{
					this.DirectoryChanged(this, m_currentDir.FullName);
				}
			}
		}

		private void OnItemDoubleClicked(MyGuiControlTable myGuiControlTable, EventArgs eventArgs)
		{
			if (eventArgs.RowIndex >= base.RowsCount)
			{
				return;
			}
			Row row = GetRow(eventArgs.RowIndex);
			if (row == null)
			{
				return;
			}
			if (row == m_backRow)
			{
				OnBackDoubleclicked();
				return;
			}
			DirectoryInfo directoryInfo = row.UserData as DirectoryInfo;
			if (directoryInfo != null)
			{
				OnDirectoryDoubleclicked(directoryInfo);
				return;
			}
			FileInfo fileInfo = row.UserData as FileInfo;
			if (fileInfo != null)
			{
				OnFileDoubleclicked(fileInfo);
			}
		}

		protected virtual void OnDirectoryDoubleclicked(DirectoryInfo info)
		{
			if (!NotifyDirectoryChanging(m_currentDir.FullName, info.FullName))
			{
				TraverseToDirectory(info.FullName);
			}
		}

		protected virtual void OnFileDoubleclicked(FileInfo info)
		{
			if (this.FileDoubleClick != null)
			{
				this.FileDoubleClick(this, info.FullName);
			}
		}

		protected virtual void OnBackDoubleclicked()
		{
			if (m_currentDir.Parent != null)
			{
				string fullName = m_currentDir.Parent.FullName;
				if (!NotifyDirectoryChanging(m_currentDir.FullName, fullName))
				{
					TraverseToDirectory(fullName);
				}
			}
		}

		protected bool NotifyDirectoryChanging(string from, string to)
		{
			if (this.DirectoryChanging != null)
			{
				m_cancelEvent.From = from;
				m_cancelEvent.To = to;
				m_cancelEvent.Cancel = false;
				this.DirectoryChanging(m_cancelEvent);
				return m_cancelEvent.Cancel;
			}
			return false;
		}

		public override MyGuiControlBase HandleInput()
		{
			if (MyInput.Static.IsNewXButton1MousePressed() || MyInput.Static.IsNewKeyPressed(MyKeys.Back))
			{
				OnBackDoubleclicked();
			}
			return base.HandleInput();
		}

		public bool SetTopMostAndCurrentDir(string directory)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directory);
			if (directoryInfo.Exists)
			{
				m_topMostDir = directoryInfo;
				m_currentDir = directoryInfo;
				return true;
			}
			return false;
		}
	}
}
