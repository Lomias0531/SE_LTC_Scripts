using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MechanicalConnectionBlockBaseDefinition), null)]
	public class MyMechanicalConnectionBlockBaseDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyMechanicalConnectionBlockBaseDefinition_003C_003EActor : IActivator, IActivator<MyMechanicalConnectionBlockBaseDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMechanicalConnectionBlockBaseDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMechanicalConnectionBlockBaseDefinition CreateInstance()
			{
				return new MyMechanicalConnectionBlockBaseDefinition();
			}

			MyMechanicalConnectionBlockBaseDefinition IActivator<MyMechanicalConnectionBlockBaseDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string TopPart;

		public float SafetyDetach;

		public float SafetyDetachMin;

		public float SafetyDetachMax;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MechanicalConnectionBlockBaseDefinition myObjectBuilder_MechanicalConnectionBlockBaseDefinition = builder as MyObjectBuilder_MechanicalConnectionBlockBaseDefinition;
			TopPart = (myObjectBuilder_MechanicalConnectionBlockBaseDefinition.TopPart ?? myObjectBuilder_MechanicalConnectionBlockBaseDefinition.RotorPart);
			SafetyDetach = myObjectBuilder_MechanicalConnectionBlockBaseDefinition.SafetyDetach;
			SafetyDetachMin = myObjectBuilder_MechanicalConnectionBlockBaseDefinition.SafetyDetachMin;
			SafetyDetachMax = myObjectBuilder_MechanicalConnectionBlockBaseDefinition.SafetyDetachMax;
		}
	}
}
