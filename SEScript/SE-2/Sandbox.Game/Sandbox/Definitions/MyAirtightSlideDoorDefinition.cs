using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AirtightSlideDoorDefinition), null)]
	public class MyAirtightSlideDoorDefinition : MyAirtightDoorGenericDefinition
	{
		private class Sandbox_Definitions_MyAirtightSlideDoorDefinition_003C_003EActor : IActivator, IActivator<MyAirtightSlideDoorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAirtightSlideDoorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAirtightSlideDoorDefinition CreateInstance()
			{
				return new MyAirtightSlideDoorDefinition();
			}

			MyAirtightSlideDoorDefinition IActivator<MyAirtightSlideDoorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
