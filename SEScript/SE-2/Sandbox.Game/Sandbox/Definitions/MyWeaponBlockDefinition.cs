using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WeaponBlockDefinition), null)]
	public class MyWeaponBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyWeaponBlockDefinition_003C_003EActor : IActivator, IActivator<MyWeaponBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWeaponBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWeaponBlockDefinition CreateInstance()
			{
				return new MyWeaponBlockDefinition();
			}

			MyWeaponBlockDefinition IActivator<MyWeaponBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId WeaponDefinitionId;

		public MyStringHash ResourceSinkGroup;

		public float InventoryMaxVolume;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WeaponBlockDefinition myObjectBuilder_WeaponBlockDefinition = builder as MyObjectBuilder_WeaponBlockDefinition;
			WeaponDefinitionId = new MyDefinitionId(myObjectBuilder_WeaponBlockDefinition.WeaponDefinitionId.Type, myObjectBuilder_WeaponBlockDefinition.WeaponDefinitionId.Subtype);
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_WeaponBlockDefinition.ResourceSinkGroup);
			InventoryMaxVolume = myObjectBuilder_WeaponBlockDefinition.InventoryMaxVolume;
		}
	}
}
