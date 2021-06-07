using System.IO;
using System.IO.Compression;

namespace Sandbox.Game.Replication.StateGroups
{
	internal static class MemoryCompressor
	{
		private static void CopyTo(Stream src, Stream dest)
		{
			byte[] array = new byte[4096];
			int count;
			while ((count = src.Read(array, 0, array.Length)) != 0)
			{
				dest.Write(array, 0, count);
			}
		}

		public static byte[] Compress(byte[] bytes)
		{
			using (MemoryStream src = new MemoryStream(bytes))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (GZipStream dest = new GZipStream(memoryStream, CompressionMode.Compress))
					{
						CopyTo(src, dest);
					}
					return memoryStream.ToArray();
				}
			}
		}

		public static byte[] Decompress(byte[] bytes)
		{
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (GZipStream src = new GZipStream(stream, CompressionMode.Decompress))
					{
						CopyTo(src, memoryStream);
					}
					return memoryStream.ToArray();
				}
			}
		}
	}
}
