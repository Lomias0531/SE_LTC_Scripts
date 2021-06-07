using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipWelderDefinition), null)]
	public class MyShipWelderDefinition : MyShipToolDefinition
	{
		private class Sandbox_Definitions_MyShipWelderDefinition_003C_003EActor : IActivator, IActivator<MyShipWelderDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipWelderDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipWelderDefinition CreateInstance()
			{
				return new MyShipWelderDefinition();
			}

			MyShipWelderDefinition IActivator<MyShipWelderDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			_ = (MyObjectBuilder_ShipWelderDefinition)builder;
		}
	}
}
