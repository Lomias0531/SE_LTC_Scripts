using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_SessionComponentContractSystemDefinition), null)]
	public class MySessionComponentContractSystemDefinition : MySessionComponentDefinition
	{
		private class VRage_Game_Definitions_SessionComponents_MySessionComponentContractSystemDefinition_003C_003EActor : IActivator, IActivator<MySessionComponentContractSystemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySessionComponentContractSystemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySessionComponentContractSystemDefinition CreateInstance()
			{
				return new MySessionComponentContractSystemDefinition();
			}

			MySessionComponentContractSystemDefinition IActivator<MySessionComponentContractSystemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const int ActiveContractsLimitPerPlayer = 20;

		public const int ContractCreationLimitPerPlayer = 20;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			_ = (MyObjectBuilder_SessionComponentContractSystemDefinition)builder;
		}
	}
}
