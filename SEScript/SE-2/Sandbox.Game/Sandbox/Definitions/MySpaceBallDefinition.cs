using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SpaceBallDefinition), null)]
	public class MySpaceBallDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MySpaceBallDefinition_003C_003EActor : IActivator, IActivator<MySpaceBallDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySpaceBallDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySpaceBallDefinition CreateInstance()
			{
				return new MySpaceBallDefinition();
			}

			MySpaceBallDefinition IActivator<MySpaceBallDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float MaxVirtualMass;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SpaceBallDefinition myObjectBuilder_SpaceBallDefinition = builder as MyObjectBuilder_SpaceBallDefinition;
			MaxVirtualMass = myObjectBuilder_SpaceBallDefinition.MaxVirtualMass;
		}
	}
}
