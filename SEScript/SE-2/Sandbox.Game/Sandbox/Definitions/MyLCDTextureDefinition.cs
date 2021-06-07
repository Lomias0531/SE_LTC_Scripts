using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_LCDTextureDefinition), null)]
	public class MyLCDTextureDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyLCDTextureDefinition_003C_003EActor : IActivator, IActivator<MyLCDTextureDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyLCDTextureDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLCDTextureDefinition CreateInstance()
			{
				return new MyLCDTextureDefinition();
			}

			MyLCDTextureDefinition IActivator<MyLCDTextureDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string TexturePath;

		public string SpritePath;

		public string LocalizationId;

		public bool Selectable;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_LCDTextureDefinition myObjectBuilder_LCDTextureDefinition = builder as MyObjectBuilder_LCDTextureDefinition;
			if (myObjectBuilder_LCDTextureDefinition != null)
			{
				TexturePath = myObjectBuilder_LCDTextureDefinition.TexturePath;
				SpritePath = myObjectBuilder_LCDTextureDefinition.SpritePath;
				LocalizationId = myObjectBuilder_LCDTextureDefinition.LocalizationId;
				Selectable = myObjectBuilder_LCDTextureDefinition.Selectable;
			}
		}
	}
}
