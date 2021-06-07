using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Filesystem;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GuiTextureAtlasDefinition), null)]
	public class MyGuiTextureAtlasDefinition : MyDefinitionBase
	{
		private class VRage_Game_Definitions_MyGuiTextureAtlasDefinition_003C_003EActor : IActivator, IActivator<MyGuiTextureAtlasDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGuiTextureAtlasDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGuiTextureAtlasDefinition CreateInstance()
			{
				return new MyGuiTextureAtlasDefinition();
			}

			MyGuiTextureAtlasDefinition IActivator<MyGuiTextureAtlasDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public readonly Dictionary<MyStringHash, MyObjectBuilder_GuiTexture> Textures = new Dictionary<MyStringHash, MyObjectBuilder_GuiTexture>(MyStringHash.Comparer);

		public readonly Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture> CompositeTextures = new Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture>(MyStringHash.Comparer);

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GuiTextureAtlasDefinition myObjectBuilder_GuiTextureAtlasDefinition = builder as MyObjectBuilder_GuiTextureAtlasDefinition;
			Textures.Clear();
			CompositeTextures.Clear();
			MyObjectBuilder_GuiTexture[] textures = myObjectBuilder_GuiTextureAtlasDefinition.Textures;
			foreach (MyObjectBuilder_GuiTexture myObjectBuilder_GuiTexture in textures)
			{
				bool flag = true;
				if (Context.IsBaseGame && ContentIndex.TryGetImageSize(myObjectBuilder_GuiTexture.Path, out myObjectBuilder_GuiTexture.SizePx.X, out myObjectBuilder_GuiTexture.SizePx.Y))
				{
					myObjectBuilder_GuiTexture.Path = Path.Combine(MyFileSystem.ContentPath, myObjectBuilder_GuiTexture.Path);
					Textures.Add(myObjectBuilder_GuiTexture.SubtypeId, myObjectBuilder_GuiTexture);
					flag = false;
				}
				else
				{
					if (Context.IsBaseGame)
					{
						myObjectBuilder_GuiTexture.Path = Path.Combine(MyFileSystem.ContentPath, myObjectBuilder_GuiTexture.Path);
					}
					string text = myObjectBuilder_GuiTexture.Path.ToLower();
					if (text.EndsWith(".dds"))
					{
						if (MyImageHeaderUtils.Read_DDS_HeaderData(myObjectBuilder_GuiTexture.Path, out MyImageHeaderUtils.DDS_HEADER header))
						{
							myObjectBuilder_GuiTexture.SizePx.X = (int)header.dwWidth;
							myObjectBuilder_GuiTexture.SizePx.Y = (int)header.dwHeight;
							Textures.Add(myObjectBuilder_GuiTexture.SubtypeId, myObjectBuilder_GuiTexture);
							flag = false;
						}
					}
					else if (text.EndsWith(".png"))
					{
						if (MyImageHeaderUtils.Read_PNG_Dimensions(myObjectBuilder_GuiTexture.Path, out myObjectBuilder_GuiTexture.SizePx.X, out myObjectBuilder_GuiTexture.SizePx.Y))
						{
							Textures.Add(myObjectBuilder_GuiTexture.SubtypeId, myObjectBuilder_GuiTexture);
							flag = false;
						}
					}
					else
					{
						MyLog.Default.WriteLine("GuiTextures.sbc");
						MyLog.Default.WriteLine("Unsupported texture format! Texture: " + myObjectBuilder_GuiTexture.Path);
					}
				}
				if (flag)
				{
					MyLog.Default.WriteLine("GuiTextures.sbc");
					MyLog.Default.WriteLine("Failed to parse texture header! Texture: " + myObjectBuilder_GuiTexture.Path);
				}
			}
			MyObjectBuilder_CompositeTexture[] compositeTextures = myObjectBuilder_GuiTextureAtlasDefinition.CompositeTextures;
			foreach (MyObjectBuilder_CompositeTexture myObjectBuilder_CompositeTexture in compositeTextures)
			{
				CompositeTextures.Add(myObjectBuilder_CompositeTexture.SubtypeId, myObjectBuilder_CompositeTexture);
			}
		}
	}
}
