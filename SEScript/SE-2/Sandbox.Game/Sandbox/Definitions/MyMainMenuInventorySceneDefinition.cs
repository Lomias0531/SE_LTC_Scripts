using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MainMenuInventorySceneDefinition), null)]
	public class MyMainMenuInventorySceneDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyMainMenuInventorySceneDefinition_003C_003EActor : IActivator, IActivator<MyMainMenuInventorySceneDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMainMenuInventorySceneDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMainMenuInventorySceneDefinition CreateInstance()
			{
				return new MyMainMenuInventorySceneDefinition();
			}

			MyMainMenuInventorySceneDefinition IActivator<MyMainMenuInventorySceneDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string SceneDirectory;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MainMenuInventorySceneDefinition myObjectBuilder_MainMenuInventorySceneDefinition = builder as MyObjectBuilder_MainMenuInventorySceneDefinition;
			SceneDirectory = myObjectBuilder_MainMenuInventorySceneDefinition.SceneDirectory;
		}
	}
}
