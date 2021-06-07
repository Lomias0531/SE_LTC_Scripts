using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AmmoMagazineDefinition), null)]
	public class MyAmmoMagazineDefinition : MyPhysicalItemDefinition
	{
		private class Sandbox_Definitions_MyAmmoMagazineDefinition_003C_003EActor : IActivator, IActivator<MyAmmoMagazineDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAmmoMagazineDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAmmoMagazineDefinition CreateInstance()
			{
				return new MyAmmoMagazineDefinition();
			}

			MyAmmoMagazineDefinition IActivator<MyAmmoMagazineDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int Capacity;

		public MyAmmoCategoryEnum Category;

		public MyDefinitionId AmmoDefinitionId;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AmmoMagazineDefinition myObjectBuilder_AmmoMagazineDefinition = builder as MyObjectBuilder_AmmoMagazineDefinition;
			Capacity = myObjectBuilder_AmmoMagazineDefinition.Capacity;
			Category = myObjectBuilder_AmmoMagazineDefinition.Category;
			if (myObjectBuilder_AmmoMagazineDefinition.AmmoDefinitionId != null)
			{
				AmmoDefinitionId = new MyDefinitionId(myObjectBuilder_AmmoMagazineDefinition.AmmoDefinitionId.Type, myObjectBuilder_AmmoMagazineDefinition.AmmoDefinitionId.Subtype);
			}
			else
			{
				AmmoDefinitionId = GetAmmoDefinitionIdFromCategory(Category);
			}
		}

		private MyDefinitionId GetAmmoDefinitionIdFromCategory(MyAmmoCategoryEnum category)
		{
			switch (category)
			{
			case MyAmmoCategoryEnum.LargeCaliber:
				return new MyDefinitionId(typeof(MyObjectBuilder_AmmoDefinition), "LargeCaliber");
			case MyAmmoCategoryEnum.SmallCaliber:
				return new MyDefinitionId(typeof(MyObjectBuilder_AmmoDefinition), "SmallCaliber");
			case MyAmmoCategoryEnum.Missile:
				return new MyDefinitionId(typeof(MyObjectBuilder_AmmoDefinition), "Missile");
			default:
				throw new NotImplementedException();
			}
		}
	}
}
