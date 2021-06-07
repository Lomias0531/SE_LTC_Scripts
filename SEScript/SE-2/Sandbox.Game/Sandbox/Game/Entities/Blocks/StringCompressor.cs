using System.IO;
using System.IO.Compression;
using System.Text;

namespace Sandbox.Game.Entities.Blocks
{
	internal static class StringCompressor
	{
		public static void CopyTo(Stream src, Stream dest)
		{
			byte[] array = new byte[4096];
			int count;
			while ((count = src.Read(array, 0, array.Length)) != 0)
			{
				dest.Write(array, 0, count);
			}
		}

		public static byte[] CompressString(string str)
		{
			using (MemoryStream src = new MemoryStream(Encoding.UTF8.GetBytes(str)))
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

		public static string DecompressString(byte[] bytes)
		{
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (GZipStream src = new GZipStream(stream, CompressionMode.Decompress))
					{
						CopyTo(src, memoryStream);
					}
					return Encoding.UTF8.GetString(memoryStream.ToArray());
				}
			}
		}
	}
}
