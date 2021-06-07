using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DebugSphere1Definition), null)]
	public class MyDebugSphere1Definition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyDebugSphere1Definition_003C_003EActor : IActivator, IActivator<MyDebugSphere1Definition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebugSphere1Definition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebugSphere1Definition CreateInstance()
			{
				return new MyDebugSphere1Definition();
			}

			MyDebugSphere1Definition IActivator<MyDebugSphere1Definition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
