using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BatteryBlockDefinition), null)]
	public class MyBatteryBlockDefinition : MyPowerProducerDefinition
	{
		private class Sandbox_Definitions_MyBatteryBlockDefinition_003C_003EActor : IActivator, IActivator<MyBatteryBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBatteryBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBatteryBlockDefinition CreateInstance()
			{
				return new MyBatteryBlockDefinition();
			}

			MyBatteryBlockDefinition IActivator<MyBatteryBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float MaxStoredPower;

		public float InitialStoredPowerRatio;

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public bool AdaptibleInput;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BatteryBlockDefinition myObjectBuilder_BatteryBlockDefinition = builder as MyObjectBuilder_BatteryBlockDefinition;
			if (myObjectBuilder_BatteryBlockDefinition != null)
			{
				MaxStoredPower = myObjectBuilder_BatteryBlockDefinition.MaxStoredPower;
				InitialStoredPowerRatio = myObjectBuilder_BatteryBlockDefinition.InitialStoredPowerRatio;
				ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_BatteryBlockDefinition.ResourceSinkGroup);
				RequiredPowerInput = myObjectBuilder_BatteryBlockDefinition.RequiredPowerInput;
				AdaptibleInput = myObjectBuilder_BatteryBlockDefinition.AdaptibleInput;
			}
		}
	}
}
