using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_OxygenContainerDefinition), null)]
	public class MyOxygenContainerDefinition : MyPhysicalItemDefinition
	{
		private class Sandbox_Definitions_MyOxygenContainerDefinition_003C_003EActor : IActivator, IActivator<MyOxygenContainerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyOxygenContainerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyOxygenContainerDefinition CreateInstance()
			{
				return new MyOxygenContainerDefinition();
			}

			MyOxygenContainerDefinition IActivator<MyOxygenContainerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Capacity;

		public MyDefinitionId StoredGasId;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_OxygenContainerDefinition myObjectBuilder_OxygenContainerDefinition = builder as MyObjectBuilder_OxygenContainerDefinition;
			Capacity = myObjectBuilder_OxygenContainerDefinition.Capacity;
			MyDefinitionId myDefinitionId = StoredGasId = ((!myObjectBuilder_OxygenContainerDefinition.StoredGasId.IsNull()) ? ((MyDefinitionId)myObjectBuilder_OxygenContainerDefinition.StoredGasId) : new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen"));
		}
	}
}
