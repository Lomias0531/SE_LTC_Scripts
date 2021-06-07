using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PlanetPrefabDefinition), null)]
	public class MyPlanetPrefabDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyPlanetPrefabDefinition_003C_003EActor : IActivator, IActivator<MyPlanetPrefabDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetPrefabDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetPrefabDefinition CreateInstance()
			{
				return new MyPlanetPrefabDefinition();
			}

			MyPlanetPrefabDefinition IActivator<MyPlanetPrefabDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyObjectBuilder_Planet PlanetBuilder;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PlanetPrefabDefinition myObjectBuilder_PlanetPrefabDefinition = builder as MyObjectBuilder_PlanetPrefabDefinition;
			PlanetBuilder = myObjectBuilder_PlanetPrefabDefinition.PlanetBuilder;
		}
	}
}
