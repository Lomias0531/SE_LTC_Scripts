using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MotorStatorDefinition), null)]
	public class MyMotorStatorDefinition : MyMechanicalConnectionBlockBaseDefinition
	{
		private class Sandbox_Definitions_MyMotorStatorDefinition_003C_003EActor : IActivator, IActivator<MyMotorStatorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMotorStatorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMotorStatorDefinition CreateInstance()
			{
				return new MyMotorStatorDefinition();
			}

			MyMotorStatorDefinition IActivator<MyMotorStatorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public float MaxForceMagnitude;

		public float RotorDisplacementMin;

		public float RotorDisplacementMax;

		public float RotorDisplacementMinSmall;

		public float RotorDisplacementMaxSmall;

		public float RotorDisplacementInModel;

		public float UnsafeTorqueThreshold;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MotorStatorDefinition myObjectBuilder_MotorStatorDefinition = (MyObjectBuilder_MotorStatorDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_MotorStatorDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_MotorStatorDefinition.RequiredPowerInput;
			MaxForceMagnitude = myObjectBuilder_MotorStatorDefinition.MaxForceMagnitude;
			RotorDisplacementMin = myObjectBuilder_MotorStatorDefinition.RotorDisplacementMin;
			RotorDisplacementMax = myObjectBuilder_MotorStatorDefinition.RotorDisplacementMax;
			RotorDisplacementMinSmall = myObjectBuilder_MotorStatorDefinition.RotorDisplacementMinSmall;
			RotorDisplacementMaxSmall = myObjectBuilder_MotorStatorDefinition.RotorDisplacementMaxSmall;
			RotorDisplacementInModel = myObjectBuilder_MotorStatorDefinition.RotorDisplacementInModel;
			UnsafeTorqueThreshold = myObjectBuilder_MotorStatorDefinition.DangerousTorqueThreshold;
		}
	}
}
