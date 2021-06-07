using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.EnvironmentItems
{
	[MyEntityType(typeof(MyObjectBuilder_Bushes), false)]
	[MyEntityType(typeof(MyObjectBuilder_DestroyableItems), true)]
	public class MyDestroyableItems : MyEnvironmentItems
	{
		private class Sandbox_Game_Entities_EnvironmentItems_MyDestroyableItems_003C_003EActor : IActivator, IActivator<MyDestroyableItems>
		{
			private sealed override object CreateInstance()
			{
				return new MyDestroyableItems();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDestroyableItems CreateInstance()
			{
				return new MyDestroyableItems();
			}

			MyDestroyableItems IActivator<MyDestroyableItems>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void DoDamage(float damage, int instanceId, Vector3D position, Vector3 normal, MyStringHash type)
		{
			if (Sync.IsServer)
			{
				RemoveItem(instanceId, sync: true);
			}
		}

		protected override MyEntity DestroyItem(int itemInstanceId)
		{
			RemoveItem(itemInstanceId, sync: true);
			return null;
		}
	}
}
