using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractTypeObtainAndDeliverDefinition), null)]
	public class MyContractTypeObtainAndDeliverDefinition : MyContractTypeDefinition
	{
		private class Sandbox_Definitions_MyContractTypeObtainAndDeliverDefinition_003C_003EActor : IActivator, IActivator<MyContractTypeObtainAndDeliverDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractTypeObtainAndDeliverDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractTypeObtainAndDeliverDefinition CreateInstance()
			{
				return new MyContractTypeObtainAndDeliverDefinition();
			}

			MyContractTypeObtainAndDeliverDefinition IActivator<MyContractTypeObtainAndDeliverDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<SerializableDefinitionId> AvailableItems;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractTypeObtainAndDeliverDefinition myObjectBuilder_ContractTypeObtainAndDeliverDefinition = builder as MyObjectBuilder_ContractTypeObtainAndDeliverDefinition;
			if (myObjectBuilder_ContractTypeObtainAndDeliverDefinition != null)
			{
				AvailableItems = myObjectBuilder_ContractTypeObtainAndDeliverDefinition.AvailableItems;
			}
		}
	}
}
