using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_VirtualMassDefinition), null)]
	public class MyVirtualMassDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyVirtualMassDefinition_003C_003EActor : IActivator, IActivator<MyVirtualMassDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVirtualMassDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVirtualMassDefinition CreateInstance()
			{
				return new MyVirtualMassDefinition();
			}

			MyVirtualMassDefinition IActivator<MyVirtualMassDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public float VirtualMass;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_VirtualMassDefinition myObjectBuilder_VirtualMassDefinition = builder as MyObjectBuilder_VirtualMassDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_VirtualMassDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_VirtualMassDefinition.RequiredPowerInput;
			VirtualMass = myObjectBuilder_VirtualMassDefinition.VirtualMass;
		}
	}
}
