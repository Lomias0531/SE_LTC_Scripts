using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_OxygenTankDefinition), null)]
	public class MyOxygenTankDefinition : MyGasTankDefinition
	{
		private class Sandbox_Definitions_MyOxygenTankDefinition_003C_003EActor : IActivator, IActivator<MyOxygenTankDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyOxygenTankDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyOxygenTankDefinition CreateInstance()
			{
				return new MyOxygenTankDefinition();
			}

			MyOxygenTankDefinition IActivator<MyOxygenTankDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
