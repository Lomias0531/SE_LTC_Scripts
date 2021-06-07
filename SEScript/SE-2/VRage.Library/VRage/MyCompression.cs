using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
	public static class MyCompression
	{
		private static byte[] m_buffer = new byte[16384];

		public static byte[] Compress(byte[] buffer)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
				{
					gZipStream.Write(buffer, 0, buffer.Length);
					gZipStream.Close();
					memoryStream.Position = 0L;
					byte[] array = new byte[memoryStream.Length + 4];
					memoryStream.Read(array, 4, (int)memoryStream.Length);
					Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, array, 0, 4);
					return array;
				}
			}
		}

		public static void CompressFile(string fileName)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				FileInfo fileInfo = new FileInfo(fileName);
				Buffer.BlockCopy(BitConverter.GetBytes(fileInfo.Length), 0, m_buffer, 0, 4);
				memoryStream.Write(m_buffer, 0, 4);
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
				{
					using (FileStream fileStream = File.OpenRead(fileName))
					{
						for (int num = fileStream.Read(m_buffer, 0, m_buffer.Length); num > 0; num = fileStream.Read(m_buffer, 0, m_buffer.Length))
						{
							gZipStream.Write(m_buffer, 0, num);
						}
					}
					gZipStream.Close();
					memoryStream.Position = 0L;
					using (FileStream fileStream2 = File.Create(fileName))
					{
						for (int num2 = memoryStream.Read(m_buffer, 0, m_buffer.Length); num2 > 0; num2 = memoryStream.Read(m_buffer, 0, m_buffer.Length))
						{
							fileStream2.Write(m_buffer, 0, num2);
							fileStream2.Flush();
						}
					}
				}
			}
		}

		public static byte[] Decompress(byte[] gzBuffer)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = BitConverter.ToInt32(gzBuffer, 0);
				memoryStream.Write(gzBuffer, 4, gzBuffer.Length - 4);
				memoryStream.Position = 0L;
				byte[] array = new byte[num];
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					gZipStream.Read(array, 0, array.Length);
					return array;
				}
			}
		}

		public static void DecompressFile(string fileName)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (FileStream fileStream = File.OpenRead(fileName))
				{
					fileStream.Read(m_buffer, 0, 4);
					using (GZipStream gZipStream = new GZipStream(fileStream, CompressionMode.Decompress))
					{
						for (int num = gZipStream.Read(m_buffer, 0, m_buffer.Length); num > 0; num = gZipStream.Read(m_buffer, 0, m_buffer.Length))
						{
							memoryStream.Write(m_buffer, 0, num);
						}
					}
				}
				memoryStream.Position = 0L;
				using (FileStream fileStream2 = File.Create(fileName))
				{
					for (int num2 = memoryStream.Read(m_buffer, 0, m_buffer.Length); num2 > 0; num2 = memoryStream.Read(m_buffer, 0, m_buffer.Length))
					{
						fileStream2.Write(m_buffer, 0, num2);
						fileStream2.Flush();
					}
				}
			}
		}
	}
}
