using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using VRage.Network;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Kitchen))]
	public class MyKitchen : MyCubeBlock
	{
		private class Sandbox_Game_Entities_MyKitchen_003C_003EActor : IActivator, IActivator<MyKitchen>
		{
			private sealed override object CreateInstance()
			{
				return new MyKitchen();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyKitchen CreateInstance()
			{
				return new MyKitchen();
			}

			MyKitchen IActivator<MyKitchen>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
