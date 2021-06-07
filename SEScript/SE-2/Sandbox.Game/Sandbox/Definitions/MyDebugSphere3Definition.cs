using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DebugSphere3Definition), null)]
	public class MyDebugSphere3Definition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyDebugSphere3Definition_003C_003EActor : IActivator, IActivator<MyDebugSphere3Definition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebugSphere3Definition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebugSphere3Definition CreateInstance()
			{
				return new MyDebugSphere3Definition();
			}

			MyDebugSphere3Definition IActivator<MyDebugSphere3Definition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
