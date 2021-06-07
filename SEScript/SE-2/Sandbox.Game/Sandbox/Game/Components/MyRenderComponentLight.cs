using VRage.Network;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentLight : MyRenderComponentCubeBlock
	{
		private class Sandbox_Game_Components_MyRenderComponentLight_003C_003EActor : IActivator, IActivator<MyRenderComponentLight>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentLight();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentLight CreateInstance()
			{
				return new MyRenderComponentLight();
			}

			MyRenderComponentLight IActivator<MyRenderComponentLight>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
