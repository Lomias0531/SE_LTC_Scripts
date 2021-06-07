using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AirtightHangarDoorDefinition), null)]
	public class MyAirtightHangarDoorDefinition : MyAirtightDoorGenericDefinition
	{
		private class Sandbox_Definitions_MyAirtightHangarDoorDefinition_003C_003EActor : IActivator, IActivator<MyAirtightHangarDoorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAirtightHangarDoorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAirtightHangarDoorDefinition CreateInstance()
			{
				return new MyAirtightHangarDoorDefinition();
			}

			MyAirtightHangarDoorDefinition IActivator<MyAirtightHangarDoorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
