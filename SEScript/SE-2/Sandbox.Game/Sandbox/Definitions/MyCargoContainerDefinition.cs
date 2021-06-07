using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_CargoContainerDefinition), null)]
	public class MyCargoContainerDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyCargoContainerDefinition_003C_003EActor : IActivator, IActivator<MyCargoContainerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCargoContainerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCargoContainerDefinition CreateInstance()
			{
				return new MyCargoContainerDefinition();
			}

			MyCargoContainerDefinition IActivator<MyCargoContainerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector3 InventorySize;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CargoContainerDefinition myObjectBuilder_CargoContainerDefinition = builder as MyObjectBuilder_CargoContainerDefinition;
			InventorySize = myObjectBuilder_CargoContainerDefinition.InventorySize;
		}
	}
}
