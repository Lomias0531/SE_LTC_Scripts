using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DebugSphere2Definition), null)]
	public class MyDebugSphere2Definition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyDebugSphere2Definition_003C_003EActor : IActivator, IActivator<MyDebugSphere2Definition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebugSphere2Definition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebugSphere2Definition CreateInstance()
			{
				return new MyDebugSphere2Definition();
			}

			MyDebugSphere2Definition IActivator<MyDebugSphere2Definition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
