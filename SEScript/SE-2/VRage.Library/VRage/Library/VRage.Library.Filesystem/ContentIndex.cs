using System;
using System.Collections.Generic;
using System.IO;

namespace VRage.Library.Filesystem
{
	public static class ContentIndex
	{
		private readonly struct FileData
		{
			public readonly int Width;

			public readonly int Height;

			public readonly bool IsImage;

			public FileData(int width, int height, bool isImage)
			{
				Width = width;
				Height = height;
				IsImage = isImage;
			}
		}

		private static readonly Dictionary<string, FileData> Files = new Dictionary<string, FileData>(StringComparer.InvariantCultureIgnoreCase);

		public static bool IsLoaded
		{
			get;
			private set;
		}

		public static bool TryGetImageSize(string path, out int width, out int height)
		{
			if (Files.TryGetValue(path, out FileData value) && value.IsImage)
			{
				width = value.Width;
				height = value.Height;
				return true;
			}
			width = (height = 0);
			return false;
		}

		public static void Load(string indexFile)
		{
			using (FileStream stream = File.OpenRead(indexFile))
			{
				using (StreamReader streamReader = new StreamReader(stream))
				{
					while (!streamReader.EndOfStream)
					{
						string text = streamReader.ReadLine();
						char c = text[0];
						FileData data;
						string filePath;
						if (c != 'G')
						{
							if (c != 'I')
							{
								continue;
							}
							ReadImageFile(text, out data, out filePath);
						}
						else
						{
							ReadGenericFile(text, out data, out filePath);
						}
						Files.Add(filePath, data);
					}
				}
			}
			IsLoaded = true;
		}

		private static void ReadGenericFile(string line, out FileData data, out string filePath)
		{
			data = default(FileData);
			filePath = line.Substring(2);
		}

		private static void ReadImageFile(string line, out FileData data, out string filePath)
		{
			int num = line.IndexOf(' ', 2);
			int num2 = line.IndexOf(' ', num + 1);
			int width = int.Parse(line.Substring(2, num - 2));
			int height = int.Parse(line.Substring(num + 1, num2 - num - 1));
			filePath = line.Substring(num2 + 1);
			data = new FileData(width, height, isImage: true);
		}

		/// <summary>
		/// Whether a file is expected to exist in the content directory.
		/// </summary>
		/// <param name="contentPath"></param>
		/// <returns></returns>
		public static bool FileExists(string contentPath)
		{
			return Files.ContainsKey(contentPath);
		}
	}
}
