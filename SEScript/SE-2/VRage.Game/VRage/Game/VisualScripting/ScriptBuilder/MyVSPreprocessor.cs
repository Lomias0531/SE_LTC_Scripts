using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	public class MyVSPreprocessor
	{
		private readonly HashSet<string> m_filePaths = new HashSet<string>();

		private readonly HashSet<string> m_classNames = new HashSet<string>();

		public string[] ClassNames => m_classNames.ToArray();

		public string[] FileSet
		{
			get
			{
				string[] array = new string[m_filePaths.Count];
				int num = 0;
				foreach (string filePath in m_filePaths)
				{
					array[num++] = filePath;
				}
				return array;
			}
		}

		public void AddFile(string filePath, string localModPath)
		{
			if (filePath == null || !m_filePaths.Add(filePath))
			{
				return;
			}
			MyObjectBuilder_VSFiles objectBuilder = null;
			new MyContentPath(filePath, localModPath);
			using (Stream stream = MyFileSystem.OpenRead(filePath))
			{
				if (stream == null)
				{
					MyLog.Default.WriteLine("VisualScripting Preprocessor: " + filePath + " is Missing.");
				}
				if (!MyObjectBuilderSerializer.DeserializeXML(stream, out objectBuilder))
				{
					m_filePaths.Remove(filePath);
					return;
				}
			}
			if (objectBuilder.VisualScript != null)
			{
				if (m_classNames.Add(objectBuilder.VisualScript.Name))
				{
					foreach (string dependencyFilePath in objectBuilder.VisualScript.DependencyFilePaths)
					{
						AddFile(new MyContentPath(dependencyFilePath, localModPath).GetExitingFilePath(), localModPath);
					}
				}
				else
				{
					m_filePaths.Remove(filePath);
				}
			}
			if (objectBuilder.StateMachine != null)
			{
				MyObjectBuilder_ScriptSMNode[] nodes = objectBuilder.StateMachine.Nodes;
				foreach (MyObjectBuilder_ScriptSMNode myObjectBuilder_ScriptSMNode in nodes)
				{
					if (!(myObjectBuilder_ScriptSMNode is MyObjectBuilder_ScriptSMSpreadNode) && !(myObjectBuilder_ScriptSMNode is MyObjectBuilder_ScriptSMBarrierNode) && !string.IsNullOrEmpty(myObjectBuilder_ScriptSMNode.ScriptFilePath))
					{
						AddFile(new MyContentPath(myObjectBuilder_ScriptSMNode.ScriptFilePath, localModPath).GetExitingFilePath(), localModPath);
					}
				}
				m_filePaths.Remove(filePath);
			}
			if (objectBuilder.LevelScript != null)
			{
				if (m_classNames.Add(objectBuilder.LevelScript.Name))
				{
					foreach (string dependencyFilePath2 in objectBuilder.LevelScript.DependencyFilePaths)
					{
						AddFile(new MyContentPath(dependencyFilePath2, localModPath).GetExitingFilePath(), localModPath);
					}
				}
				else
				{
					m_filePaths.Remove(filePath);
				}
			}
		}

		public void Clear()
		{
			m_filePaths.Clear();
			m_classNames.Clear();
		}
	}
}
