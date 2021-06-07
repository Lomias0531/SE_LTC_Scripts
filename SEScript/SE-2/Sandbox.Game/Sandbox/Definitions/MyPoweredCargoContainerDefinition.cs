using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PoweredCargoContainerDefinition), null)]
	public class MyPoweredCargoContainerDefinition : MyCargoContainerDefinition
	{
		private class Sandbox_Definitions_MyPoweredCargoContainerDefinition_003C_003EActor : IActivator, IActivator<MyPoweredCargoContainerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPoweredCargoContainerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPoweredCargoContainerDefinition CreateInstance()
			{
				return new MyPoweredCargoContainerDefinition();
			}

			MyPoweredCargoContainerDefinition IActivator<MyPoweredCargoContainerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ResourceSinkGroup;

		public float RequiredPowerInput;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PoweredCargoContainerDefinition myObjectBuilder_PoweredCargoContainerDefinition = builder as MyObjectBuilder_PoweredCargoContainerDefinition;
			ResourceSinkGroup = myObjectBuilder_PoweredCargoContainerDefinition.ResourceSinkGroup;
			RequiredPowerInput = myObjectBuilder_PoweredCargoContainerDefinition.RequiredPowerInput;
		}
	}
}
