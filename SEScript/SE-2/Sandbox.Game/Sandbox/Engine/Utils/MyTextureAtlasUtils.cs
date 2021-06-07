using Sandbox.Graphics;
using System;
using System.Globalization;
using System.IO;
using VRage.FileSystem;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	internal class MyTextureAtlasUtils
	{
		private static MyTextureAtlas LoadTextureAtlas(string textureDir, string atlasFile)
		{
			MyTextureAtlas myTextureAtlas = new MyTextureAtlas(64);
			using (Stream stream = MyFileSystem.OpenRead(Path.Combine(MyFileSystem.ContentPath, atlasFile)))
			{
				using (StreamReader streamReader = new StreamReader(stream))
				{
					while (!streamReader.EndOfStream)
					{
						string text = streamReader.ReadLine();
						if (!text.StartsWith("#") && text.Trim(new char[1]
						{
							' '
						}).Length != 0)
						{
							string[] array = text.Split(new char[3]
							{
								' ',
								'\t',
								','
							}, StringSplitOptions.RemoveEmptyEntries);
							string key = array[0];
							string str = array[1];
							Vector4 uvOffsets = new Vector4(Convert.ToSingle(array[4], CultureInfo.InvariantCulture), Convert.ToSingle(array[5], CultureInfo.InvariantCulture), Convert.ToSingle(array[7], CultureInfo.InvariantCulture), Convert.ToSingle(array[8], CultureInfo.InvariantCulture));
							string atlasTex = textureDir + str;
							MyTextureAtlasItem value = new MyTextureAtlasItem(atlasTex, uvOffsets);
							myTextureAtlas.Add(key, value);
						}
					}
					return myTextureAtlas;
				}
			}
		}

		public static void LoadTextureAtlas(string[] enumsToStrings, string textureDir, string atlasFile, out string texture, out MyAtlasTextureCoordinate[] textureCoords)
		{
			MyTextureAtlas myTextureAtlas = LoadTextureAtlas(textureDir, atlasFile);
			textureCoords = new MyAtlasTextureCoordinate[enumsToStrings.Length];
			texture = null;
			for (int i = 0; i < enumsToStrings.Length; i++)
			{
				MyTextureAtlasItem myTextureAtlasItem = myTextureAtlas[enumsToStrings[i]];
				textureCoords[i] = new MyAtlasTextureCoordinate(new Vector2(myTextureAtlasItem.UVOffsets.X, myTextureAtlasItem.UVOffsets.Y), new Vector2(myTextureAtlasItem.UVOffsets.Z, myTextureAtlasItem.UVOffsets.W));
				if (texture == null)
				{
					texture = myTextureAtlasItem.AtlasTexture;
				}
			}
		}
	}
}
