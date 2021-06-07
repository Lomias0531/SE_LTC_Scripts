using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_KitchenDefinition), null)]
	public class MyKitchenDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyKitchenDefinition_003C_003EActor : IActivator, IActivator<MyKitchenDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyKitchenDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyKitchenDefinition CreateInstance()
			{
				return new MyKitchenDefinition();
			}

			MyKitchenDefinition IActivator<MyKitchenDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
