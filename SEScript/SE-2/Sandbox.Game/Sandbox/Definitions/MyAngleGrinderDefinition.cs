using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AngleGrinderDefinition), null)]
	internal class MyAngleGrinderDefinition : MyEngineerToolBaseDefinition
	{
		private class Sandbox_Definitions_MyAngleGrinderDefinition_003C_003EActor : IActivator, IActivator<MyAngleGrinderDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAngleGrinderDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAngleGrinderDefinition CreateInstance()
			{
				return new MyAngleGrinderDefinition();
			}

			MyAngleGrinderDefinition IActivator<MyAngleGrinderDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
