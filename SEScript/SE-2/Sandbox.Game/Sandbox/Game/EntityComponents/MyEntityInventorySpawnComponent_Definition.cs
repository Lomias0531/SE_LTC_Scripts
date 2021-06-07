using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_InventorySpawnComponent_Definition), null)]
	public class MyEntityInventorySpawnComponent_Definition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyEntityInventorySpawnComponent_Definition_003C_003EActor : IActivator, IActivator<MyEntityInventorySpawnComponent_Definition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityInventorySpawnComponent_Definition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityInventorySpawnComponent_Definition CreateInstance()
			{
				return new MyEntityInventorySpawnComponent_Definition();
			}

			MyEntityInventorySpawnComponent_Definition IActivator<MyEntityInventorySpawnComponent_Definition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId ContainerDefinition;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_InventorySpawnComponent_Definition myObjectBuilder_InventorySpawnComponent_Definition = builder as MyObjectBuilder_InventorySpawnComponent_Definition;
			ContainerDefinition = myObjectBuilder_InventorySpawnComponent_Definition.ContainerDefinition;
		}
	}
}
