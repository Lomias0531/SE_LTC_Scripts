using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractTypeDeliverDefinition), null)]
	public class MyContractTypeDeliverDefinition : MyContractTypeDefinition
	{
		private class Sandbox_Definitions_MyContractTypeDeliverDefinition_003C_003EActor : IActivator, IActivator<MyContractTypeDeliverDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractTypeDeliverDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractTypeDeliverDefinition CreateInstance()
			{
				return new MyContractTypeDeliverDefinition();
			}

			MyContractTypeDeliverDefinition IActivator<MyContractTypeDeliverDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public double Duration_BaseTime;

		public double Duration_TimePerJumpDist;

		public double Duration_TimePerMeter;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractTypeDeliverDefinition myObjectBuilder_ContractTypeDeliverDefinition = builder as MyObjectBuilder_ContractTypeDeliverDefinition;
			if (myObjectBuilder_ContractTypeDeliverDefinition != null)
			{
				Duration_BaseTime = myObjectBuilder_ContractTypeDeliverDefinition.Duration_BaseTime;
				Duration_TimePerJumpDist = myObjectBuilder_ContractTypeDeliverDefinition.Duration_TimePerJumpDist;
				Duration_TimePerMeter = myObjectBuilder_ContractTypeDeliverDefinition.Duration_TimePerMeter;
			}
		}
	}
}
