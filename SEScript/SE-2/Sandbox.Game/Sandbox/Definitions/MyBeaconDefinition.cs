using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BeaconDefinition), null)]
	public class MyBeaconDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyBeaconDefinition_003C_003EActor : IActivator, IActivator<MyBeaconDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBeaconDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBeaconDefinition CreateInstance()
			{
				return new MyBeaconDefinition();
			}

			MyBeaconDefinition IActivator<MyBeaconDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ResourceSinkGroup;

		public string Flare;

		public float MaxBroadcastRadius;

		public float MaxBroadcastPowerDrainkW;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BeaconDefinition myObjectBuilder_BeaconDefinition = (MyObjectBuilder_BeaconDefinition)builder;
			ResourceSinkGroup = myObjectBuilder_BeaconDefinition.ResourceSinkGroup;
			MaxBroadcastRadius = myObjectBuilder_BeaconDefinition.MaxBroadcastRadius;
			Flare = myObjectBuilder_BeaconDefinition.Flare;
			MaxBroadcastPowerDrainkW = myObjectBuilder_BeaconDefinition.MaxBroadcastPowerDrainkW;
		}
	}
}
