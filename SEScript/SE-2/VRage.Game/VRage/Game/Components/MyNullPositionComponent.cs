using VRage.Network;

namespace VRage.Game.Components
{
	public class MyNullPositionComponent : MyPositionComponentBase
	{
		private class VRage_Game_Components_MyNullPositionComponent_003C_003EActor : IActivator, IActivator<MyNullPositionComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyNullPositionComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyNullPositionComponent CreateInstance()
			{
				return new MyNullPositionComponent();
			}

			MyNullPositionComponent IActivator<MyNullPositionComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
