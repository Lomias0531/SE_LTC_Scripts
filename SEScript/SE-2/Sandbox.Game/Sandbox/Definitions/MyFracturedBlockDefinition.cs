using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FracturedBlockDefinition), null)]
	internal class MyFracturedBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyFracturedBlockDefinition_003C_003EActor : IActivator, IActivator<MyFracturedBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFracturedBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFracturedBlockDefinition CreateInstance()
			{
				return new MyFracturedBlockDefinition();
			}

			MyFracturedBlockDefinition IActivator<MyFracturedBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
