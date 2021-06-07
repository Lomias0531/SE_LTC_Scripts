using System.Xml.Serialization;
using VRage.Game.Definitions;
using VRage.Network;
using VRageRender;

namespace VRage.Game
{
	/// <summary>
	/// Stripped environment definition with only visual settings
	/// </summary>
	[MyDefinitionType(typeof(MyObjectBuilder_VisualSettingsDefinition), null)]
	public class MyVisualSettingsDefinition : MyDefinitionBase
	{
		private class VRage_Game_MyVisualSettingsDefinition_003C_003EActor : IActivator, IActivator<MyVisualSettingsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVisualSettingsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVisualSettingsDefinition CreateInstance()
			{
				return new MyVisualSettingsDefinition();
			}

			MyVisualSettingsDefinition IActivator<MyVisualSettingsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyFogProperties FogProperties = MyFogProperties.Default;

		public MySunProperties SunProperties = MySunProperties.Default;

		public MyPostprocessSettings PostProcessSettings = MyPostprocessSettings.Default;

		[XmlIgnore]
		public MyShadowsSettings ShadowSettings
		{
			get;
			private set;
		}

		public MyVisualSettingsDefinition()
		{
			ShadowSettings = new MyShadowsSettings();
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_VisualSettingsDefinition myObjectBuilder_VisualSettingsDefinition = (MyObjectBuilder_VisualSettingsDefinition)builder;
			FogProperties = myObjectBuilder_VisualSettingsDefinition.FogProperties;
			SunProperties = myObjectBuilder_VisualSettingsDefinition.SunProperties;
			PostProcessSettings = myObjectBuilder_VisualSettingsDefinition.PostProcessSettings;
			ShadowSettings.CopyFrom(myObjectBuilder_VisualSettingsDefinition.ShadowSettings);
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_VisualSettingsDefinition myObjectBuilder_VisualSettingsDefinition = new MyObjectBuilder_VisualSettingsDefinition();
			myObjectBuilder_VisualSettingsDefinition.FogProperties = FogProperties;
			myObjectBuilder_VisualSettingsDefinition.SunProperties = SunProperties;
			myObjectBuilder_VisualSettingsDefinition.PostProcessSettings = PostProcessSettings;
			myObjectBuilder_VisualSettingsDefinition.ShadowSettings.CopyFrom(ShadowSettings);
			return myObjectBuilder_VisualSettingsDefinition;
		}
	}
}
