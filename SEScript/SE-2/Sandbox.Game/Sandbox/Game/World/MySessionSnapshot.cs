using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Voxels;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MySessionSnapshot
	{
		private static FastResourceLock m_savingLock = new FastResourceLock();

		public string TargetDir;

		public string SavingDir;

		public MyObjectBuilder_Checkpoint CheckpointSnapshot;

		public MyObjectBuilder_Sector SectorSnapshot;

		public const int MAX_WINDOWS_PATH = 260;

		public Dictionary<string, byte[]> CompressedVoxelSnapshots
		{
			get;
			set;
		}

		public Dictionary<string, byte[]> VoxelSnapshots
		{
			get;
			set;
		}

		public Dictionary<string, IMyStorage> VoxelStorageNameCache
		{
			get;
			set;
		}

		public ulong SavedSizeInBytes
		{
			get;
			private set;
		}

		public bool SavingSuccess
		{
			get;
			private set;
		}

		public bool Save(Func<bool> screenshotTaken)
		{
			bool flag = true;
			using (m_savingLock.AcquireExclusiveUsing())
			{
				MySandboxGame.Log.WriteLine("Session snapshot save - START");
				using (MySandboxGame.Log.IndentUsing())
				{
					Directory.CreateDirectory(TargetDir);
					MySandboxGame.Log.WriteLine("Checking file access for files in target dir.");
					if (!CheckAccessToFiles())
					{
						SavingSuccess = false;
						return false;
					}
					string savingDir = SavingDir;
					if (Directory.Exists(savingDir))
					{
						Directory.Delete(savingDir, recursive: true);
					}
					Directory.CreateDirectory(savingDir);
					try
					{
						ulong sizeInBytes = 0uL;
						ulong sizeInBytes2 = 0uL;
						ulong num = 0uL;
						flag = (MyLocalCache.SaveSector(SectorSnapshot, SavingDir, Vector3I.Zero, out sizeInBytes) && MyLocalCache.SaveCheckpoint(CheckpointSnapshot, SavingDir, out sizeInBytes2));
						if (flag)
						{
							foreach (KeyValuePair<string, byte[]> voxelSnapshot in VoxelSnapshots)
							{
								ulong size = 0uL;
								flag = (flag && SaveVoxelSnapshot(voxelSnapshot.Key, voxelSnapshot.Value, compress: true, out size));
								if (flag)
								{
									num += size;
								}
							}
							VoxelSnapshots.Clear();
							VoxelStorageNameCache.Clear();
							foreach (KeyValuePair<string, byte[]> compressedVoxelSnapshot in CompressedVoxelSnapshots)
							{
								ulong size2 = 0uL;
								flag = (flag && SaveVoxelSnapshot(compressedVoxelSnapshot.Key, compressedVoxelSnapshot.Value, compress: false, out size2));
								if (flag)
								{
									num += size2;
								}
							}
							CompressedVoxelSnapshots.Clear();
						}
						if (flag && Sync.IsServer)
						{
							flag = MyLocalCache.SaveLastSessionInfo(TargetDir, isOnline: false, isLobby: false, MySession.Static.Name, null, 0);
						}
						if (flag)
						{
							SavedSizeInBytes = sizeInBytes + sizeInBytes2 + num;
						}
					}
					catch (Exception ex)
					{
						MySandboxGame.Log.WriteLine("There was an error while saving snapshot.");
						MySandboxGame.Log.WriteLine(ex);
						flag = false;
					}
					if (flag)
					{
						HashSet<string> hashSet = new HashSet<string>();
						string[] files = Directory.GetFiles(savingDir);
						foreach (string obj in files)
						{
							string fileName = Path.GetFileName(obj);
							string text = Path.Combine(TargetDir, fileName);
							if (File.Exists(text))
							{
								File.Delete(text);
							}
							File.Move(obj, text);
							hashSet.Add(fileName);
						}
						files = Directory.GetFiles(TargetDir);
						foreach (string path in files)
						{
							string fileName2 = Path.GetFileName(path);
							if (!hashSet.Contains(fileName2) && !(fileName2 == MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION))
							{
								File.Delete(path);
							}
						}
						Directory.Delete(savingDir);
						if (screenshotTaken != null)
						{
							while (!screenshotTaken())
							{
								Thread.Sleep(10);
							}
						}
						Backup();
					}
					else if (Directory.Exists(savingDir))
					{
						Directory.Delete(savingDir, recursive: true);
					}
				}
				MySandboxGame.Log.WriteLine("Session snapshot save - END");
			}
			SavingSuccess = flag;
			return flag;
		}

		private void Backup()
		{
			if (MySession.Static.MaxBackupSaves > 0)
			{
				string path = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
				string text = Path.Combine(TargetDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER, path);
				Directory.CreateDirectory(text);
				string[] files = Directory.GetFiles(TargetDir);
				foreach (string text2 in files)
				{
					string text3 = Path.Combine(text, Path.GetFileName(text2));
					if (text3.Length < 260 && text2.Length < 260)
					{
						File.Copy(text2, text3, overwrite: true);
					}
				}
				string[] directories = Directory.GetDirectories(Path.Combine(TargetDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER));
				if (!IsSorted(directories))
				{
					Array.Sort(directories);
				}
				if (directories.Length > MySession.Static.MaxBackupSaves)
				{
					int num = directories.Length - MySession.Static.MaxBackupSaves;
					for (int j = 0; j < num; j++)
					{
						Directory.Delete(directories[j], recursive: true);
					}
				}
			}
			else if (MySession.Static.MaxBackupSaves == 0 && Directory.Exists(Path.Combine(TargetDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER)))
			{
				Directory.Delete(Path.Combine(TargetDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER), recursive: true);
			}
		}

		public static bool IsSorted(string[] arr)
		{
			for (int i = 1; i < arr.Length; i++)
			{
				if (arr[i - 1].CompareTo(arr[i]) > 0)
				{
					return false;
				}
			}
			return true;
		}

		private bool SaveVoxelSnapshot(string storageName, byte[] snapshotData, bool compress, out ulong size)
		{
			string text = Path.Combine(SavingDir, storageName + ".vx2");
			try
			{
				if (compress)
				{
					using (MemoryStream memoryStream = new MemoryStream(16384))
					{
						using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
						{
							gZipStream.Write(snapshotData, 0, snapshotData.Length);
						}
						byte[] array = memoryStream.ToArray();
						File.WriteAllBytes(text, array);
						size = (ulong)array.Length;
						if (VoxelStorageNameCache != null)
						{
							IMyStorage value = null;
							if (VoxelStorageNameCache.TryGetValue(storageName, out value) && !value.Closed)
							{
								value.SetCompressedDataCache(array);
							}
						}
					}
				}
				else
				{
					File.WriteAllBytes(text, snapshotData);
					size = (ulong)snapshotData.Length;
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine($"Failed to write voxel file '{text}'");
				MySandboxGame.Log.WriteLine(ex);
				size = 0uL;
				return false;
			}
			return true;
		}

		private bool CheckAccessToFiles()
		{
			string[] files = Directory.GetFiles(TargetDir, "*", SearchOption.TopDirectoryOnly);
			foreach (string text in files)
			{
				if (!(text == MySession.Static.ThumbPath) && !MyFileSystem.CheckFileWriteAccess(text))
				{
					MySandboxGame.Log.WriteLine($"Couldn't access file '{Path.GetFileName(text)}'.");
					return false;
				}
			}
			return true;
		}

		public void SaveParallel(Func<bool> screenshotTaken, Action completionCallback = null)
		{
			Action action = delegate
			{
				Save(screenshotTaken);
			};
			if (completionCallback != null)
			{
				Parallel.Start(action, completionCallback);
			}
			else
			{
				Parallel.Start(action);
			}
		}

		public static void WaitForSaving()
		{
			int num = 0;
			do
			{
				using (m_savingLock.AcquireExclusiveUsing())
				{
					num = m_savingLock.ExclusiveWaiters;
				}
			}
			while (num > 0);
		}
	}
}
