using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_DebugSphere2))]
	internal class MyDebugSphere2 : MyFunctionalBlock
	{
		private class Sandbox_Game_Entities_MyDebugSphere2_003C_003EActor : IActivator, IActivator<MyDebugSphere2>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebugSphere2();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebugSphere2 CreateInstance()
			{
				return new MyDebugSphere2();
			}

			MyDebugSphere2 IActivator<MyDebugSphere2>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private new MyDebugSphere2Definition BlockDefinition => (MyDebugSphere2Definition)base.BlockDefinition;
	}
}
