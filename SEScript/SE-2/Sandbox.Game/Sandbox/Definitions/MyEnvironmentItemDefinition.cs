using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_EnvironmentItemDefinition), null)]
	public class MyEnvironmentItemDefinition : MyPhysicalModelDefinition
	{
		private class Sandbox_Definitions_MyEnvironmentItemDefinition_003C_003EActor : IActivator, IActivator<MyEnvironmentItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEnvironmentItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEnvironmentItemDefinition CreateInstance()
			{
				return new MyEnvironmentItemDefinition();
			}

			MyEnvironmentItemDefinition IActivator<MyEnvironmentItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
		}
	}
}
