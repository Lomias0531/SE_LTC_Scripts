using Sandbox.Game.Weapons;
using VRage.Network;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentSmallGatlingGun : MyRenderComponentCubeBlock
	{
		private class Sandbox_Game_Components_MyRenderComponentSmallGatlingGun_003C_003EActor : IActivator, IActivator<MyRenderComponentSmallGatlingGun>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentSmallGatlingGun();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentSmallGatlingGun CreateInstance()
			{
				return new MyRenderComponentSmallGatlingGun();
			}

			MyRenderComponentSmallGatlingGun IActivator<MyRenderComponentSmallGatlingGun>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MySmallGatlingGun m_gatlingGun;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_gatlingGun = (base.Container.Entity as MySmallGatlingGun);
		}

		public override void Draw()
		{
			base.Draw();
		}
	}
}
