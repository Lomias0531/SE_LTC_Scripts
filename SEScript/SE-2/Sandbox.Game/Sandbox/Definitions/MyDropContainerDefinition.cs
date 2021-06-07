using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DropContainerDefinition), null)]
	public class MyDropContainerDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyDropContainerDefinition_003C_003EActor : IActivator, IActivator<MyDropContainerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDropContainerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDropContainerDefinition CreateInstance()
			{
				return new MyDropContainerDefinition();
			}

			MyDropContainerDefinition IActivator<MyDropContainerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyPrefabDefinition Prefab;

		public MyContainerSpawnRules SpawnRules;

		public float Priority;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_DropContainerDefinition myObjectBuilder_DropContainerDefinition = builder as MyObjectBuilder_DropContainerDefinition;
			SpawnRules = myObjectBuilder_DropContainerDefinition.SpawnRules;
			Prefab = MyDefinitionManager.Static.GetPrefabDefinition(myObjectBuilder_DropContainerDefinition.Prefab);
			Priority = myObjectBuilder_DropContainerDefinition.Priority;
		}
	}
}
