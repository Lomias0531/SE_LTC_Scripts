using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_DebugSphere1))]
	internal class MyDebugSphere1 : MyFunctionalBlock
	{
		private class Sandbox_Game_Entities_MyDebugSphere1_003C_003EActor : IActivator, IActivator<MyDebugSphere1>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebugSphere1();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebugSphere1 CreateInstance()
			{
				return new MyDebugSphere1();
			}

			MyDebugSphere1 IActivator<MyDebugSphere1>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private new MyDebugSphere1Definition BlockDefinition => (MyDebugSphere1Definition)base.BlockDefinition;
	}
}
