using ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipConnectorDefinition), null)]
	public class MyShipConnectorDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyShipConnectorDefinition_003C_003EActor : IActivator, IActivator<MyShipConnectorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipConnectorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipConnectorDefinition CreateInstance()
			{
				return new MyShipConnectorDefinition();
			}

			MyShipConnectorDefinition IActivator<MyShipConnectorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float AutoUnlockTime_Min;

		public float AutoUnlockTime_Max;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ShipConnectorDefinition myObjectBuilder_ShipConnectorDefinition = builder as MyObjectBuilder_ShipConnectorDefinition;
			if (myObjectBuilder_ShipConnectorDefinition != null)
			{
				AutoUnlockTime_Min = myObjectBuilder_ShipConnectorDefinition.AutoUnlockTime_Min;
				AutoUnlockTime_Max = myObjectBuilder_ShipConnectorDefinition.AutoUnlockTime_Max;
			}
		}
	}
}
