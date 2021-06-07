using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_ClipboardDefinition), null)]
	public class MyClipboardDefinition : MySessionComponentDefinition
	{
		private class VRage_Game_Definitions_SessionComponents_MyClipboardDefinition_003C_003EActor : IActivator, IActivator<MyClipboardDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyClipboardDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyClipboardDefinition CreateInstance()
			{
				return new MyClipboardDefinition();
			}

			MyClipboardDefinition IActivator<MyClipboardDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Defines settings for pasting.
		/// </summary>
		public MyPlacementSettings PastingSettings;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ClipboardDefinition myObjectBuilder_ClipboardDefinition = (MyObjectBuilder_ClipboardDefinition)builder;
			PastingSettings = myObjectBuilder_ClipboardDefinition.PastingSettings;
		}
	}
}
