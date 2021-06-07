using Sandbox.Engine.Voxels;
using Sandbox.Engine.Voxels.Planet;
using System;
using System.Collections.Concurrent;
using System.IO;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Render.Image;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyHeightMapLoadingSystem : MySessionComponentBase
	{
		private ConcurrentDictionary<string, MyHeightCubemap> m_heightMaps;

		private ConcurrentDictionary<string, MyCubemap[]> m_planetMaps;

		private ConcurrentDictionary<string, MyTileTexture<byte>> m_ditherTilesets;

		public static MyHeightMapLoadingSystem Static;

		public override void LoadData()
		{
			base.LoadData();
			m_heightMaps = new ConcurrentDictionary<string, MyHeightCubemap>();
			m_planetMaps = new ConcurrentDictionary<string, MyCubemap[]>();
			m_ditherTilesets = new ConcurrentDictionary<string, MyTileTexture<byte>>();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			foreach (MyHeightCubemap value in m_heightMaps.Values)
			{
				value.Dispose();
			}
			foreach (MyCubemap[] value2 in m_planetMaps.Values)
			{
				for (int i = 0; i < value2.Length; i++)
				{
					value2[i]?.Dispose();
				}
			}
			foreach (MyTileTexture<byte> value3 in m_ditherTilesets.Values)
			{
				value3.Dispose();
			}
			m_heightMaps = null;
			m_planetMaps = null;
			m_ditherTilesets = null;
		}

		public bool TryGet(string path, out MyHeightCubemap heightmap)
		{
			return m_heightMaps.TryGetValue(path, out heightmap);
		}

		public bool TryGet(string path, out MyCubemap[] materialMaps)
		{
			return m_planetMaps.TryGetValue(path, out materialMaps);
		}

		public bool TryGet(string path, out MyTileTexture<byte> tilemap)
		{
			return m_ditherTilesets.TryGetValue(path, out tilemap);
		}

		public void Cache(string path, ref MyHeightCubemap heightmap)
		{
			MyHeightCubemap orAdd = m_heightMaps.GetOrAdd(path, heightmap);
			if (orAdd != heightmap)
			{
				heightmap.Dispose();
				heightmap = orAdd;
			}
		}

		public void Cache(string path, ref MyCubemap[] materialMaps)
		{
			MyCubemap[] orAdd = m_planetMaps.GetOrAdd(path, materialMaps);
			if (orAdd != materialMaps)
			{
				MyCubemap[] array = materialMaps;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]?.Dispose();
				}
				materialMaps = orAdd;
			}
		}

		private void Cache(string path, ref MyTileTexture<byte> tilemap)
		{
			MyTileTexture<byte> orAdd = m_ditherTilesets.GetOrAdd(path, tilemap);
			if (orAdd != tilemap)
			{
				tilemap.Dispose();
				tilemap = orAdd;
			}
		}

		public MyTileTexture<byte> GetTerrainBlendTexture(MyPlanetMaterialBlendSettings settings)
		{
			string texture = settings.Texture;
			int cellSize = settings.CellSize;
			if (!TryGet(texture, out MyTileTexture<byte> tilemap))
			{
				string path = Path.Combine(MyFileSystem.ContentPath, texture) + ".png";
				IMyImage myImage = null;
				try
				{
					myImage = MyImage.Load(path, oneChannel: true);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex.Message);
				}
				if (myImage == null || myImage.BitsPerPixel != 8)
				{
					MyLog.Default.WriteLine("Only 8bit texture supported for terrain");
					return MyTileTexture<byte>.Default;
				}
				tilemap = new MyTileTexture<byte>(myImage.Size, myImage.Stride, ((IMyImage<byte>)myImage).Data, cellSize);
				Cache(texture, ref tilemap);
			}
			return tilemap;
		}
	}
}
