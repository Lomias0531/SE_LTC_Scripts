using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ExtendedPistonBaseDefinition), null)]
	public class MyExtendedPistonBaseDefinition : MyPistonBaseDefinition
	{
		private class Sandbox_Definitions_MyExtendedPistonBaseDefinition_003C_003EActor : IActivator, IActivator<MyExtendedPistonBaseDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyExtendedPistonBaseDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyExtendedPistonBaseDefinition CreateInstance()
			{
				return new MyExtendedPistonBaseDefinition();
			}

			MyExtendedPistonBaseDefinition IActivator<MyExtendedPistonBaseDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
