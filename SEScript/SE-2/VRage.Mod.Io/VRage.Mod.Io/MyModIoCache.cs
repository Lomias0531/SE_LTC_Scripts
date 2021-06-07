using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.GameServices;
using VRage.Mod.Io.Data;

namespace VRage.Mod.Io
{
	internal static class MyModIoCache
	{
		private class MyDownloadInfo
		{
			public ulong Id;

			public ulong Size;

			public ulong Position;

			public string FileName;

			public string TempFileName;

			public DateTime DateAdded;

			public event Action<MyGameServiceCallResult> Response;

			public void InvokeResponse(MyGameServiceCallResult result)
			{
				this.Response.InvokeIfNotNull(result);
			}
		}

		private static readonly Dictionary<ulong, MyDownloadInfo> m_downloadInfos = new Dictionary<ulong, MyDownloadInfo>();

		public static MyWorkshopItemState GetItemState(Modfile modFile)
		{
			MyWorkshopItemState myWorkshopItemState = MyWorkshopItemState.None;
			if (m_downloadInfos.ContainsKey((ulong)modFile.mod_id))
			{
				myWorkshopItemState |= (MyWorkshopItemState.Downloading | MyWorkshopItemState.DownloadPending);
			}
			string fileName = GetFileName(modFile);
			if (File.Exists(fileName))
			{
				myWorkshopItemState |= MyWorkshopItemState.Installed;
				if (File.GetCreationTimeUtc(fileName).ToUnixTimestamp() != modFile.date_added || new FileInfo(fileName).Length != modFile.filesize)
				{
					myWorkshopItemState |= MyWorkshopItemState.NeedsUpdate;
				}
			}
			return myWorkshopItemState;
		}

		public static bool GetItemInstallInfo(Modfile modFile, out ulong size, out string folder, out uint timeStamp)
		{
			if (GetItemState(modFile).HasFlag(MyWorkshopItemState.Installed))
			{
				string fileName = GetFileName(modFile);
				FileInfo fileInfo = new FileInfo(fileName);
				size = (ulong)fileInfo.Length;
				folder = fileName;
				timeStamp = fileInfo.CreationTimeUtc.ToUnixTimestamp();
				return true;
			}
			size = 0uL;
			folder = "";
			timeStamp = 0u;
			return false;
		}

		private static string GetFileName(Modfile modfile)
		{
			return $"{MyFileSystem.CachePath}\\{modfile.mod_id}.zip";
		}

		public static void DownloadItem(Modfile modfile, Action<MyGameServiceCallResult> response)
		{
			if (m_downloadInfos.TryGetValue((ulong)modfile.mod_id, out MyDownloadInfo value))
			{
				value.Response += response;
				return;
			}
			string fileName = GetFileName(modfile);
			string text = fileName + ".download";
			try
			{
				Directory.CreateDirectory(MyFileSystem.CachePath);
			}
			catch
			{
				response.InvokeIfNotNull(MyGameServiceCallResult.AccessDenied);
				return;
			}
			MyDownloadInfo info = new MyDownloadInfo
			{
				Id = (ulong)modfile.mod_id,
				Size = (ulong)modfile.filesize,
				TempFileName = text,
				FileName = fileName,
				DateAdded = ((uint)modfile.date_added).ToDateTimeFromUnixTimestamp()
			};
			info.Response += response;
			m_downloadInfos.Add((ulong)modfile.mod_id, info);
			MyModIo.DownloadFile(modfile.download.binary_url, text, delegate(MyGameServiceCallResult x)
			{
				OnDownloadDone(x, info);
			}, delegate(ulong x)
			{
				OnDownloadProgress(x, info);
			});
		}

		private static void OnDownloadProgress(ulong downloadedSize, MyDownloadInfo info)
		{
			info.Position += downloadedSize;
		}

		private static void OnDownloadDone(MyGameServiceCallResult result, MyDownloadInfo info)
		{
			try
			{
				if (File.Exists(info.FileName))
				{
					File.Delete(info.FileName);
				}
				File.Move(info.TempFileName, info.FileName);
				File.SetCreationTimeUtc(info.FileName, info.DateAdded);
			}
			catch
			{
				result = MyGameServiceCallResult.AccessDenied;
			}
			m_downloadInfos.Remove(info.Id);
			info.InvokeResponse(result);
		}

		public static void GetItemDownloadInfo(ulong id, out ulong bytesDownloaded, out ulong bytesTotal)
		{
			if (m_downloadInfos.TryGetValue(id, out MyDownloadInfo value))
			{
				bytesDownloaded = value.Position;
				bytesTotal = value.Size;
			}
			else
			{
				bytesDownloaded = 0uL;
				bytesTotal = 0uL;
			}
		}
	}
}
