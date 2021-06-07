using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractBlockDefinition), null)]
	public class MyContractBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyContractBlockDefinition_003C_003EActor : IActivator, IActivator<MyContractBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractBlockDefinition CreateInstance()
			{
				return new MyContractBlockDefinition();
			}

			MyContractBlockDefinition IActivator<MyContractBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContractBlockDefinition myObjectBuilder_ContractBlockDefinition = builder as MyObjectBuilder_ContractBlockDefinition;
			ScreenAreas = ((myObjectBuilder_ContractBlockDefinition.ScreenAreas != null) ? myObjectBuilder_ContractBlockDefinition.ScreenAreas.ToList() : null);
		}
	}
}
