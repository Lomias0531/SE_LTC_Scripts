using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MotorSuspensionDefinition), null)]
	public class MyMotorSuspensionDefinition : MyMotorStatorDefinition
	{
		private class Sandbox_Definitions_MyMotorSuspensionDefinition_003C_003EActor : IActivator, IActivator<MyMotorSuspensionDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMotorSuspensionDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMotorSuspensionDefinition CreateInstance()
			{
				return new MyMotorSuspensionDefinition();
			}

			MyMotorSuspensionDefinition IActivator<MyMotorSuspensionDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float MaxSteer;

		public float SteeringSpeed;

		public float PropulsionForce;

		public float MinHeight;

		public float MaxHeight;

		public float AxleFriction;

		public float AirShockMinSpeed;

		public float AirShockMaxSpeed;

		public int AirShockActivationDelay;

		public float RequiredIdlePowerInput;

		public MyDefinitionId? SoundDefinitionId;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MotorSuspensionDefinition myObjectBuilder_MotorSuspensionDefinition = (MyObjectBuilder_MotorSuspensionDefinition)builder;
			MaxSteer = myObjectBuilder_MotorSuspensionDefinition.MaxSteer;
			SteeringSpeed = myObjectBuilder_MotorSuspensionDefinition.SteeringSpeed;
			PropulsionForce = myObjectBuilder_MotorSuspensionDefinition.PropulsionForce;
			MinHeight = myObjectBuilder_MotorSuspensionDefinition.MinHeight;
			MaxHeight = myObjectBuilder_MotorSuspensionDefinition.MaxHeight;
			AxleFriction = myObjectBuilder_MotorSuspensionDefinition.AxleFriction;
			AirShockMinSpeed = myObjectBuilder_MotorSuspensionDefinition.AirShockMinSpeed;
			AirShockMaxSpeed = myObjectBuilder_MotorSuspensionDefinition.AirShockMaxSpeed;
			AirShockActivationDelay = myObjectBuilder_MotorSuspensionDefinition.AirShockActivationDelay;
			RequiredIdlePowerInput = ((myObjectBuilder_MotorSuspensionDefinition.RequiredIdlePowerInput != 0f) ? myObjectBuilder_MotorSuspensionDefinition.RequiredIdlePowerInput : myObjectBuilder_MotorSuspensionDefinition.RequiredPowerInput);
			if (myObjectBuilder_MotorSuspensionDefinition.SoundDefinitionId != null && MyDefinitionId.TryParse(myObjectBuilder_MotorSuspensionDefinition.SoundDefinitionId.DefinitionTypeName, myObjectBuilder_MotorSuspensionDefinition.SoundDefinitionId.DefinitionSubtypeName, out MyDefinitionId definitionId))
			{
				SoundDefinitionId = definitionId;
			}
			else
			{
				SoundDefinitionId = null;
			}
		}
	}
}
