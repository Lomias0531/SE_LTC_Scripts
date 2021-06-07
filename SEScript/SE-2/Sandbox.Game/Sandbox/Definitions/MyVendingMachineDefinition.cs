using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_VendingMachineDefinition), null)]
	public class MyVendingMachineDefinition : MyStoreBlockDefinition
	{
		private class Sandbox_Definitions_MyVendingMachineDefinition_003C_003EActor : IActivator, IActivator<MyVendingMachineDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVendingMachineDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVendingMachineDefinition CreateInstance()
			{
				return new MyVendingMachineDefinition();
			}

			MyVendingMachineDefinition IActivator<MyVendingMachineDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<string> AdditionalEmissiveMaterials;

		public List<MyObjectBuilder_StoreItem> DefaultItems;

		public string ThrowOutDummy;

		public Dictionary<string, float> ThrowOutItems;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_VendingMachineDefinition myObjectBuilder_VendingMachineDefinition = (MyObjectBuilder_VendingMachineDefinition)builder;
			AdditionalEmissiveMaterials = myObjectBuilder_VendingMachineDefinition.AdditionalEmissiveMaterials;
			DefaultItems = myObjectBuilder_VendingMachineDefinition.DefaultItems;
			ThrowOutDummy = myObjectBuilder_VendingMachineDefinition.ThrowOutDummy;
			if (myObjectBuilder_VendingMachineDefinition.ThrowOutItems != null)
			{
				ThrowOutItems = myObjectBuilder_VendingMachineDefinition.ThrowOutItems.Dictionary;
			}
		}
	}
}
