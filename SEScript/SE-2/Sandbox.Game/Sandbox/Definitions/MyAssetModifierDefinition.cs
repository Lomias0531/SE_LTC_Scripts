using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AssetModifierDefinition), null)]
	public class MyAssetModifierDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyAssetModifierDefinition_003C_003EActor : IActivator, IActivator<MyAssetModifierDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAssetModifierDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAssetModifierDefinition CreateInstance()
			{
				return new MyAssetModifierDefinition();
			}

			MyAssetModifierDefinition IActivator<MyAssetModifierDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyObjectBuilder_AssetModifierDefinition.MyAssetTexture> Textures;

		public bool MetalnessColorable;

		public Color? DefaultColor;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AssetModifierDefinition myObjectBuilder_AssetModifierDefinition = builder as MyObjectBuilder_AssetModifierDefinition;
			Textures = myObjectBuilder_AssetModifierDefinition.Textures;
			MetalnessColorable = myObjectBuilder_AssetModifierDefinition.MetalnessColorable;
			DefaultColor = myObjectBuilder_AssetModifierDefinition.DefaultColor;
		}

		public string GetFilepath(string location, MyTextureType type)
		{
			foreach (MyObjectBuilder_AssetModifierDefinition.MyAssetTexture texture in Textures)
			{
				if (texture.Location == location && texture.Type == type)
				{
					return texture.Filepath;
				}
			}
			return null;
		}
	}
}
