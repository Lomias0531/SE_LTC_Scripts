using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipGrinderDefinition), null)]
	public class MyShipGrinderDefinition : MyShipToolDefinition
	{
		private class Sandbox_Definitions_MyShipGrinderDefinition_003C_003EActor : IActivator, IActivator<MyShipGrinderDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipGrinderDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipGrinderDefinition CreateInstance()
			{
				return new MyShipGrinderDefinition();
			}

			MyShipGrinderDefinition IActivator<MyShipGrinderDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			_ = (MyObjectBuilder_ShipGrinderDefinition)builder;
		}
	}
}
