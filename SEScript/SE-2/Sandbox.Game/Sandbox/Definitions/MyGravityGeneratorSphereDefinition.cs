using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GravityGeneratorSphereDefinition), null)]
	public class MyGravityGeneratorSphereDefinition : MyGravityGeneratorBaseDefinition
	{
		private class Sandbox_Definitions_MyGravityGeneratorSphereDefinition_003C_003EActor : IActivator, IActivator<MyGravityGeneratorSphereDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGravityGeneratorSphereDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGravityGeneratorSphereDefinition CreateInstance()
			{
				return new MyGravityGeneratorSphereDefinition();
			}

			MyGravityGeneratorSphereDefinition IActivator<MyGravityGeneratorSphereDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float MinRadius;

		public float MaxRadius;

		public float BasePowerInput;

		public float ConsumptionPower;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GravityGeneratorSphereDefinition myObjectBuilder_GravityGeneratorSphereDefinition = builder as MyObjectBuilder_GravityGeneratorSphereDefinition;
			MinRadius = myObjectBuilder_GravityGeneratorSphereDefinition.MinRadius;
			MaxRadius = myObjectBuilder_GravityGeneratorSphereDefinition.MaxRadius;
			BasePowerInput = myObjectBuilder_GravityGeneratorSphereDefinition.BasePowerInput;
			ConsumptionPower = myObjectBuilder_GravityGeneratorSphereDefinition.ConsumptionPower;
		}
	}
}
