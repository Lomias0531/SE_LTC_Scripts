using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipToolDefinition), null)]
	public class MyShipToolDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyShipToolDefinition_003C_003EActor : IActivator, IActivator<MyShipToolDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipToolDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipToolDefinition CreateInstance()
			{
				return new MyShipToolDefinition();
			}

			MyShipToolDefinition IActivator<MyShipToolDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Flare;

		public float SensorRadius;

		public float SensorOffset;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ShipToolDefinition myObjectBuilder_ShipToolDefinition = builder as MyObjectBuilder_ShipToolDefinition;
			SensorRadius = myObjectBuilder_ShipToolDefinition.SensorRadius;
			SensorOffset = myObjectBuilder_ShipToolDefinition.SensorOffset;
			Flare = myObjectBuilder_ShipToolDefinition.Flare;
		}
	}
}
