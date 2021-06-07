using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PowerProducerDefinition), null)]
	public class MyPowerProducerDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyPowerProducerDefinition_003C_003EActor : IActivator, IActivator<MyPowerProducerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPowerProducerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPowerProducerDefinition CreateInstance()
			{
				return new MyPowerProducerDefinition();
			}

			MyPowerProducerDefinition IActivator<MyPowerProducerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSourceGroup;

		public float MaxPowerOutput;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PowerProducerDefinition myObjectBuilder_PowerProducerDefinition = builder as MyObjectBuilder_PowerProducerDefinition;
			if (myObjectBuilder_PowerProducerDefinition != null)
			{
				ResourceSourceGroup = MyStringHash.GetOrCompute(myObjectBuilder_PowerProducerDefinition.ResourceSourceGroup);
				MaxPowerOutput = myObjectBuilder_PowerProducerDefinition.MaxPowerOutput;
			}
		}
	}
}
