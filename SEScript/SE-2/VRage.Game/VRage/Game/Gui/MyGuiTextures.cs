using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game.GUI
{
	public class MyGuiTextures
	{
		private readonly Dictionary<MyStringHash, MyObjectBuilder_GuiTexture> m_textures = new Dictionary<MyStringHash, MyObjectBuilder_GuiTexture>();

		private readonly Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture> m_compositeTextures = new Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture>();

		private static MyGuiTextures m_instance;

		public static MyGuiTextures Static => m_instance ?? (m_instance = new MyGuiTextures());

		public void Reload()
		{
			m_textures.Clear();
			m_compositeTextures.Clear();
			if (MyRenderProxy.RenderThread != null)
			{
				IEnumerable<MyGuiTextureAtlasDefinition> allDefinitions = MyDefinitionManagerBase.Static.GetAllDefinitions<MyGuiTextureAtlasDefinition>();
				List<string> list = new List<string>();
				if (allDefinitions != null)
				{
					foreach (MyGuiTextureAtlasDefinition item in allDefinitions)
					{
						foreach (KeyValuePair<MyStringHash, MyObjectBuilder_GuiTexture> texture in item.Textures)
						{
							m_textures[texture.Key] = texture.Value;
							list.Add(texture.Value.Path);
						}
						foreach (KeyValuePair<MyStringHash, MyObjectBuilder_CompositeTexture> compositeTexture in item.CompositeTextures)
						{
							m_compositeTextures[compositeTexture.Key] = compositeTexture.Value;
						}
					}
				}
				IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons"), "*", MySearchOption.TopDirectoryOnly);
				list.AddRange(files);
				files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons\\cubes"), "*", MySearchOption.TopDirectoryOnly);
				list.AddRange(files);
				files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons\\component"), "*", MySearchOption.TopDirectoryOnly);
				list.AddRange(files);
				files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons\\skins"), "*", MySearchOption.AllDirectories);
				list.AddRange(files);
				MyRenderProxy.PreloadTextures(list, TextureType.GUI);
				list.Clear();
				files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "customworlds"), "*.jpg", MySearchOption.AllDirectories);
				list.AddRange(files);
				files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "scenarios"), "*.png", MySearchOption.AllDirectories);
				list.AddRange(files);
				MyRenderProxy.PreloadTextures(list, TextureType.GUIWithoutPremultiplyAlpha);
			}
		}

		public MyObjectBuilder_GuiTexture GetTexture(MyStringHash hash)
		{
			MyObjectBuilder_GuiTexture value = null;
			m_textures.TryGetValue(hash, out value);
			return value;
		}

		public MyObjectBuilder_CompositeTexture GetCompositeTexture(MyStringHash hash)
		{
			MyObjectBuilder_CompositeTexture value = null;
			m_compositeTextures.TryGetValue(hash, out value);
			return value;
		}

		public bool TryGetTexture(MyStringHash hash, out MyObjectBuilder_GuiTexture texture)
		{
			return m_textures.TryGetValue(hash, out texture);
		}

		public bool TryGetCompositeTexture(MyStringHash hash, out MyObjectBuilder_CompositeTexture texture)
		{
			return m_compositeTextures.TryGetValue(hash, out texture);
		}
	}
}
