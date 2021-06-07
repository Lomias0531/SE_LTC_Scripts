using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using VRage.Network;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Planter))]
	public class MyPlanter : MyCubeBlock
	{
		private class Sandbox_Game_Entities_MyPlanter_003C_003EActor : IActivator, IActivator<MyPlanter>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanter();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanter CreateInstance()
			{
				return new MyPlanter();
			}

			MyPlanter IActivator<MyPlanter>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
