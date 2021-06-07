using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_DebugSphere3))]
	internal class MyDebugSphere3 : MyFunctionalBlock
	{
		private class Sandbox_Game_Entities_MyDebugSphere3_003C_003EActor : IActivator, IActivator<MyDebugSphere3>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebugSphere3();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebugSphere3 CreateInstance()
			{
				return new MyDebugSphere3();
			}

			MyDebugSphere3 IActivator<MyDebugSphere3>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private new MyDebugSphere3Definition BlockDefinition => (MyDebugSphere3Definition)base.BlockDefinition;
	}
}
