using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PistonBaseDefinition), null)]
	public class MyPistonBaseDefinition : MyMechanicalConnectionBlockBaseDefinition
	{
		private class Sandbox_Definitions_MyPistonBaseDefinition_003C_003EActor : IActivator, IActivator<MyPistonBaseDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPistonBaseDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPistonBaseDefinition CreateInstance()
			{
				return new MyPistonBaseDefinition();
			}

			MyPistonBaseDefinition IActivator<MyPistonBaseDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Minimum;

		public float Maximum;

		public float MaxVelocity;

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public float MaxImpulse;

		public float DefaultMaxImpulseAxis;

		public float DefaultMaxImpulseNonAxis;

		public float UnsafeImpulseThreshold;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PistonBaseDefinition myObjectBuilder_PistonBaseDefinition = (MyObjectBuilder_PistonBaseDefinition)builder;
			Minimum = myObjectBuilder_PistonBaseDefinition.Minimum;
			Maximum = myObjectBuilder_PistonBaseDefinition.Maximum;
			MaxVelocity = myObjectBuilder_PistonBaseDefinition.MaxVelocity;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_PistonBaseDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_PistonBaseDefinition.RequiredPowerInput;
			MaxImpulse = myObjectBuilder_PistonBaseDefinition.MaxImpulse;
			DefaultMaxImpulseAxis = myObjectBuilder_PistonBaseDefinition.DefaultMaxImpulseAxis;
			DefaultMaxImpulseNonAxis = myObjectBuilder_PistonBaseDefinition.DefaultMaxImpulseNonAxis;
			UnsafeImpulseThreshold = myObjectBuilder_PistonBaseDefinition.DangerousImpulseThreshold;
		}
	}
}
