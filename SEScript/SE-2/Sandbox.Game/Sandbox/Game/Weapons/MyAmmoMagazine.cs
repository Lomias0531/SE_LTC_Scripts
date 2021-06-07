using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Weapons
{
	[MyEntityType(typeof(MyObjectBuilder_AmmoMagazine), true)]
	public class MyAmmoMagazine : MyBaseInventoryItemEntity
	{
		private class Sandbox_Game_Weapons_MyAmmoMagazine_003C_003EActor : IActivator, IActivator<MyAmmoMagazine>
		{
			private sealed override object CreateInstance()
			{
				return new MyAmmoMagazine();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAmmoMagazine CreateInstance()
			{
				return new MyAmmoMagazine();
			}

			MyAmmoMagazine IActivator<MyAmmoMagazine>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
		}
	}
}
