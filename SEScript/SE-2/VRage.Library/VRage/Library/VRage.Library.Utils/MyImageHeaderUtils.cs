using System;
using System.IO;
using VRage.FileSystem;

namespace VRage.Library.Utils
{
	public static class MyImageHeaderUtils
	{
		public struct DDS_PIXELFORMAT
		{
			public uint dwSize;

			public uint dwFlags;

			public uint dwFourCC;

			public uint dwRGBBitCount;

			public uint dwRBitMask;

			public uint dwGBitMask;

			public uint dwBBitMask;

			public uint dwABitMask;
		}

		public struct DDS_HEADER
		{
			public uint dwSize;

			public uint dwFlags;

			public uint dwHeight;

			public uint dwWidth;

			public uint dwPitchOrLinearSize;

			public uint dwDepth;

			public uint dwMipMapCount;

			public uint[] dwReserved1;

			public DDS_PIXELFORMAT ddspf;

			public uint dwCaps;

			public uint dwCaps2;

			public uint dwCaps3;

			public uint dwCaps4;

			public uint dwReserved2;
		}

		private const uint DDS_MAGIC = 542327876u;

		private const uint PNG_MAGIC = 1196314761u;

		/// <summary>
		/// Reads the header of standard DDS texture without reading the rest of its contents.
		/// Checks for magic constant.
		/// </summary>
		/// <param name="filePath">Path to dds.</param>
		/// <param name="header">Output header</param>
		/// <returns>Success flag.</returns>
		public static bool Read_DDS_HeaderData(string filePath, out DDS_HEADER header)
		{
			header = new DDS_HEADER
			{
				dwReserved1 = new uint[11]
			};
			if (!MyFileSystem.FileExists(filePath))
			{
				return false;
			}
			using (Stream input = MyFileSystem.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					uint num = binaryReader.ReadUInt32();
					if (num != 542327876)
					{
						return false;
					}
					header.dwSize = binaryReader.ReadUInt32();
					header.dwFlags = binaryReader.ReadUInt32();
					header.dwHeight = binaryReader.ReadUInt32();
					header.dwWidth = binaryReader.ReadUInt32();
					header.dwPitchOrLinearSize = binaryReader.ReadUInt32();
					header.dwDepth = binaryReader.ReadUInt32();
					header.dwMipMapCount = binaryReader.ReadUInt32();
					for (int i = 0; i < 11; i++)
					{
						header.dwReserved1[i] = binaryReader.ReadUInt32();
					}
					header.ddspf.dwSize = binaryReader.ReadUInt32();
					header.ddspf.dwFlags = binaryReader.ReadUInt32();
					header.ddspf.dwFourCC = binaryReader.ReadUInt32();
					header.ddspf.dwRGBBitCount = binaryReader.ReadUInt32();
					header.ddspf.dwRBitMask = binaryReader.ReadUInt32();
					header.ddspf.dwGBitMask = binaryReader.ReadUInt32();
					header.ddspf.dwBBitMask = binaryReader.ReadUInt32();
					header.ddspf.dwABitMask = binaryReader.ReadUInt32();
					header.dwCaps = binaryReader.ReadUInt32();
					header.dwCaps2 = binaryReader.ReadUInt32();
					header.dwCaps3 = binaryReader.ReadUInt32();
					header.dwCaps4 = binaryReader.ReadUInt32();
					header.dwReserved2 = binaryReader.ReadUInt32();
				}
			}
			return true;
		}

		public static bool Read_PNG_Dimensions(string filePath, out int width, out int height)
		{
			width = 0;
			height = 0;
			if (!MyFileSystem.FileExists(filePath))
			{
				return false;
			}
			using (Stream input = MyFileSystem.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					uint num = binaryReader.ReadUInt32();
					if (num != 1196314761)
					{
						return false;
					}
					binaryReader.ReadBytes(12);
					width = binaryReader.ReadLittleEndianInt32();
					height = binaryReader.ReadLittleEndianInt32();
					return true;
				}
			}
		}

		private static int ReadLittleEndianInt32(this BinaryReader binaryReader)
		{
			byte[] array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array[3 - i] = binaryReader.ReadByte();
			}
			return BitConverter.ToInt32(array, 0);
		}
	}
}
