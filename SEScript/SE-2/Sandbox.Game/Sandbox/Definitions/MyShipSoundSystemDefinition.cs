using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipSoundSystemDefinition), null)]
	public class MyShipSoundSystemDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyShipSoundSystemDefinition_003C_003EActor : IActivator, IActivator<MyShipSoundSystemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipSoundSystemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipSoundSystemDefinition CreateInstance()
			{
				return new MyShipSoundSystemDefinition();
			}

			MyShipSoundSystemDefinition IActivator<MyShipSoundSystemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float MaxUpdateRange = 2000f;

		public float MaxUpdateRange_sq = 4000000f;

		public float WheelsCallbackRangeCreate_sq = 250000f;

		public float WheelsCallbackRangeRemove_sq = 562500f;

		public float FullSpeed = 96f;

		public float FullSpeed_sq = 9216f;

		public float SpeedThreshold1 = 32f;

		public float SpeedThreshold2 = 64f;

		public float LargeShipDetectionRadius = 15f;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ShipSoundSystemDefinition myObjectBuilder_ShipSoundSystemDefinition = builder as MyObjectBuilder_ShipSoundSystemDefinition;
			MaxUpdateRange = myObjectBuilder_ShipSoundSystemDefinition.MaxUpdateRange;
			FullSpeed = myObjectBuilder_ShipSoundSystemDefinition.FullSpeed;
			FullSpeed_sq = myObjectBuilder_ShipSoundSystemDefinition.FullSpeed * myObjectBuilder_ShipSoundSystemDefinition.FullSpeed;
			SpeedThreshold1 = myObjectBuilder_ShipSoundSystemDefinition.FullSpeed * 0.33f;
			SpeedThreshold2 = myObjectBuilder_ShipSoundSystemDefinition.FullSpeed * 0.66f;
			LargeShipDetectionRadius = myObjectBuilder_ShipSoundSystemDefinition.LargeShipDetectionRadius;
			MaxUpdateRange_sq = myObjectBuilder_ShipSoundSystemDefinition.MaxUpdateRange * myObjectBuilder_ShipSoundSystemDefinition.MaxUpdateRange;
			WheelsCallbackRangeCreate_sq = myObjectBuilder_ShipSoundSystemDefinition.WheelStartUpdateRange * myObjectBuilder_ShipSoundSystemDefinition.WheelStartUpdateRange;
			WheelsCallbackRangeRemove_sq = myObjectBuilder_ShipSoundSystemDefinition.WheelStopUpdateRange * myObjectBuilder_ShipSoundSystemDefinition.WheelStopUpdateRange;
		}
	}
}
