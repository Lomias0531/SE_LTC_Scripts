using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_TextPanelDefinition), null)]
	public class MyTextPanelDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyTextPanelDefinition_003C_003EActor : IActivator, IActivator<MyTextPanelDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTextPanelDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTextPanelDefinition CreateInstance()
			{
				return new MyTextPanelDefinition();
			}

			MyTextPanelDefinition IActivator<MyTextPanelDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public string PanelMaterialName;

		public float RequiredPowerInput;

		public int TextureResolution;

		public int ScreenWidth;

		public int ScreenHeight;

		public float MinFontSize;

		public float MaxFontSize;

		public float MaxChangingSpeed;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TextPanelDefinition myObjectBuilder_TextPanelDefinition = (MyObjectBuilder_TextPanelDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_TextPanelDefinition.ResourceSinkGroup);
			if (!string.IsNullOrEmpty(myObjectBuilder_TextPanelDefinition.PanelMaterialName))
			{
				PanelMaterialName = myObjectBuilder_TextPanelDefinition.PanelMaterialName;
			}
			RequiredPowerInput = myObjectBuilder_TextPanelDefinition.RequiredPowerInput;
			TextureResolution = myObjectBuilder_TextPanelDefinition.TextureResolution;
			ScreenWidth = myObjectBuilder_TextPanelDefinition.ScreenWidth;
			ScreenHeight = myObjectBuilder_TextPanelDefinition.ScreenHeight;
			MinFontSize = myObjectBuilder_TextPanelDefinition.MinFontSize;
			MaxFontSize = myObjectBuilder_TextPanelDefinition.MaxFontSize;
			MaxChangingSpeed = myObjectBuilder_TextPanelDefinition.MaxChangingSpeed;
		}
	}
}
