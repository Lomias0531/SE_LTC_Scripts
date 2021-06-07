using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.Render.Image;
using VRage.Utils;

namespace Sandbox.Engine.Voxels.Planet
{
	[MyPlanetMapProvider(typeof(MyObjectBuilder_PlanetTextureMapProvider), true)]
	public class MyPlanetTextureMapProvider : MyPlanetMapProviderBase
	{
		public static string PlanetDataFilesPath = "Data/PlanetDataFiles";

		private string m_path;

		private MyModContext m_mod;

		private MyHeightMapLoadingSystem MapCache => MySession.Static.GetComponent<MyHeightMapLoadingSystem>();

		public override void Init(long seed, MyPlanetGeneratorDefinition generator, MyObjectBuilder_PlanetMapProvider builder)
		{
			MyObjectBuilder_PlanetTextureMapProvider myObjectBuilder_PlanetTextureMapProvider = (MyObjectBuilder_PlanetTextureMapProvider)builder;
			m_path = myObjectBuilder_PlanetTextureMapProvider.Path;
			m_mod = generator.Context;
		}

		public override MyCubemap[] GetMaps(MyPlanetMapTypeSet types)
		{
			if (!MapCache.TryGet(m_path, out MyCubemap[] materialMaps))
			{
				GetPlanetMaps(m_path, m_mod, types, out materialMaps);
				MapCache.Cache(m_path, ref materialMaps);
			}
			return materialMaps;
		}

		public override MyHeightCubemap GetHeightmap()
		{
			if (!MapCache.TryGet(m_path, out MyHeightCubemap heightmap))
			{
				heightmap = GetHeightMap(m_path, m_mod);
				MapCache.Cache(m_path, ref heightmap);
			}
			return heightmap;
		}

		private static string FindTexture(string basePath)
		{
			if (TestPath(".png", out string pathOut2))
			{
				return pathOut2;
			}
			if (TestPath(".dds", out pathOut2))
			{
				return pathOut2;
			}
			return null;
			bool TestPath(string extension, out string pathOut)
			{
				string text = basePath + extension;
				pathOut = (MyFileSystem.FileExists(text) ? text : null);
				return pathOut != null;
			}
		}

		private static string GetPath(string folder, string name, MyModContext context)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			if (!context.IsBaseGame)
			{
				text2 = Path.Combine(Path.Combine(context.ModPath, PlanetDataFilesPath), folder, name);
				text = FindTexture(text2);
			}
			if (text == null)
			{
				text3 = Path.Combine(MyFileSystem.ContentPath, PlanetDataFilesPath, folder, name);
				text = FindTexture(text3);
			}
			return text ?? text2 ?? text3;
		}

		private IMyImage LoadTexture(string path)
		{
			if (!MyFileSystem.FileExists(path))
			{
				return null;
			}
			return MyImage.Load(path, oneChannel: true);
		}

		public MyHeightCubemap GetHeightMap(string folderName, MyModContext context)
		{
			bool flag = false;
			int num = 0;
			MyHeightmapFace[] array = new MyHeightmapFace[6];
			for (int i = 0; i < 6; i++)
			{
				array[i] = GetHeightMap(folderName, MyCubemapHelpers.GetNameForFace(i), context);
				if (array[i] == null)
				{
					flag = true;
				}
				else if (array[i].Resolution != num && num != 0)
				{
					flag = true;
					MyLog.Default.Error("Cubemap faces must be all the same size!");
				}
				else
				{
					num = array[i].Resolution;
				}
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				MyLog.Default.WriteLine("Error loading heightmap " + folderName + ", using fallback instead. See rest of log for details.");
				for (int j = 0; j < 6; j++)
				{
					array[j] = MyHeightmapFace.Default;
					num = array[j].Resolution;
				}
			}
			return new MyHeightCubemap((context.ModId ?? "BaseGame") + ":" + folderName, array, num);
		}

		private MyHeightmapFace GetHeightMap(string folderName, string faceName, MyModContext context)
		{
			string path = GetPath(folderName, faceName, context);
			MyHeightmapFace myHeightmapFace = null;
			try
			{
				IMyImage myImage = LoadTexture(path);
				if (myImage != null)
				{
					if (myImage.BitsPerPixel != 8 && myImage.BitsPerPixel != 16)
					{
						MyLog.Default.Error($"Heighmap texture {path}: Invalid format {myImage.BitsPerPixel} (expecting 8 or 16bit).");
					}
					myHeightmapFace = new MyHeightmapFace(myImage.Size.Y);
					PrepareHeightMap(myHeightmapFace, myImage);
					return myHeightmapFace;
				}
				MyLog.Default.Error("Could not load texture {0}, no suitable format found. ", path);
				return myHeightmapFace;
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex.Message);
				return myHeightmapFace;
			}
		}

		private unsafe static void PrepareHeightMap(MyHeightmapFace map, IMyImage image)
		{
			int num = 0;
			int stride = image.Stride;
			if (image.BitsPerPixel == 8)
			{
				byte[] data = ((IMyImage<byte>)image).Data;
				for (int i = 0; i < map.Resolution; i++)
				{
					int rowStart = map.GetRowStart(i);
					for (int j = 0; j < image.Size.X; j++)
					{
						map.Data[rowStart++] = (ushort)(data[num++] * 256);
					}
					num += stride - image.Size.X;
				}
			}
			else if (image.BitsPerPixel == 16)
			{
				fixed (ushort* ptr = ((IMyImage<ushort>)image).Data)
				{
					for (int k = 0; k < map.Resolution; k++)
					{
						Unsafe.CopyBlockUnaligned(map.Data + map.GetRowStart(k), ptr + num, (uint)(image.Size.X * 2));
						num += stride;
					}
				}
			}
		}

		public MyHeightDetailTexture GetDetailMap(string path)
		{
			MyHeightDetailTexture myHeightDetailTexture = null;
			string text = Path.Combine(MyFileSystem.ContentPath, path);
			text = (FindTexture(text) ?? text);
			try
			{
				IMyImage<byte> myImage;
				if ((myImage = (LoadTexture(text) as IMyImage<byte>)) != null)
				{
					if (myImage.BitsPerPixel != 8)
					{
						MyLog.Default.Error("Detail map '{0}' could not be loaded, expected 8bit format, got {1}bit instead.", text, myImage.BitsPerPixel);
					}
					else
					{
						myHeightDetailTexture = new MyHeightDetailTexture(myImage.Data, (uint)myImage.Size.Y);
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.Error(ex.ToString());
			}
			return myHeightDetailTexture ?? new MyHeightDetailTexture(new byte[1], 1u);
		}

		private void ClearMatValues(MyCubemapData<byte>[] maps)
		{
			for (int i = 0; i < 6; i++)
			{
				maps[i * 4] = null;
				maps[i * 4 + 1] = null;
				maps[i * 4 + 2] = null;
			}
		}

		public unsafe void GetPlanetMaps(string folder, MyModContext context, MyPlanetMapTypeSet mapsToUse, out MyCubemap[] maps)
		{
			maps = new MyCubemap[4];
			MyCubemapData<byte>[] tmpMaps = new MyCubemapData<byte>[24];
			if (mapsToUse != 0)
			{
				int i;
				IMyImage texture;
				byte** streams;
				int num;
				for (i = 0; i < 6; num = ++i)
				{
					string name = Path.Combine(folder, MyCubemapHelpers.GetNameForFace(i));
					try
					{
						texture = TryGetPlanetTexture(name, context, "_mat", out string fullPath);
						if (texture == null)
						{
							ClearMatValues(tmpMaps);
						}
						else
						{
							if (texture.Size.X == texture.Size.Y)
							{
								streams = stackalloc byte*[3];
								SetStream(0, MyPlanetMapTypeSet.Material);
								SetStream(1, MyPlanetMapTypeSet.Biome);
								SetStream(2, MyPlanetMapTypeSet.Ore);
								ReadChannelsFromImage(streams, texture);
								continue;
							}
							MyLog.Default.Error("While loading maps from {0}: Width and height must be the same.", fullPath);
						}
						void SetStream(int streamIndex, MyPlanetMapTypeSet typeSet)
						{
							int num5 = i * 4 + streamIndex;
							MyCubemapData<byte> myCubemapData = ((mapsToUse & typeSet) == 0) ? null : new MyCubemapData<byte>(texture.Size.X);
							tmpMaps[num5] = myCubemapData;
							streams[streamIndex] = (byte*)(long)((myCubemapData == null) ? ((IntPtr)(void*)null) : ((IntPtr)myCubemapData.Data));
						}
					}
					catch (Exception ex)
					{
						MyLog.Default.Error(ex.ToString());
					}
					break;
				}
			}
			for (int j = 0; j < 4; j++)
			{
				if (tmpMaps[j] != null)
				{
					MyCubemapData<byte>[] array = new MyCubemapData<byte>[6];
					for (int k = 0; k < 6; k++)
					{
						array[k] = tmpMaps[j + k * 4];
					}
					maps[j] = new MyCubemap(array);
				}
			}
			void ReadChannelsFromImage(byte** dataStreams, IMyImage image)
			{
				int x = image.Size.X;
				uint[] data = ((MyImage<uint>)image).Data;
				int num2 = 0;
				int num3 = x + 3;
				for (int l = 0; l < x; l++)
				{
					for (int m = 0; m < x; m++)
					{
						uint num4 = data[num2];
						Set(0, num3, (byte)num4);
						Set(1, num3, (byte)(num4 >> 8));
						Set(2, num3, (byte)(num4 >> 16));
						num2++;
						num3++;
					}
					num3 += 2;
				}
				void Set(int stream, int offset, byte value)
				{
					if (dataStreams[stream] != null)
					{
						dataStreams[stream][offset] = value;
					}
				}
			}
		}

		private IMyImage TryGetPlanetTexture(string name, MyModContext context, string p, out string fullPath)
		{
			bool flag = false;
			name += p;
			fullPath = Path.Combine(context.ModPathData, "PlanetDataFiles", name) + ".png";
			if (!context.IsBaseGame)
			{
				if (!MyFileSystem.FileExists(fullPath))
				{
					fullPath = Path.Combine(context.ModPathData, "PlanetDataFiles", name) + ".dds";
					if (MyFileSystem.FileExists(fullPath))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
			}
			if (!flag)
			{
				string path = Path.Combine(MyFileSystem.ContentPath, PlanetDataFilesPath);
				fullPath = Path.Combine(path, name) + ".png";
				if (!MyFileSystem.FileExists(fullPath))
				{
					fullPath = Path.Combine(path, name) + ".dds";
					if (!MyFileSystem.FileExists(fullPath))
					{
						return null;
					}
				}
			}
			if (fullPath.Contains(MyWorkshop.WorkshopModSuffix))
			{
				string text = fullPath.Substring(0, fullPath.IndexOf(MyWorkshop.WorkshopModSuffix) + MyWorkshop.WorkshopModSuffix.Length);
				string name2 = fullPath.Replace(text + "\\", "");
				using (MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(text))
				{
					try
					{
						return MyImage.Load(myZipArchive.GetFile(name2).GetStream(), oneChannel: false);
					}
					catch (Exception)
					{
						MyLog.Default.Error("Failed to load existing " + p + " file mod archive. " + fullPath);
						return null;
					}
				}
			}
			return MyImage.Load(fullPath, oneChannel: false);
		}
	}
}
