using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PlanterDefinition), null)]
	public class MyPlanterDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyPlanterDefinition_003C_003EActor : IActivator, IActivator<MyPlanterDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanterDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanterDefinition CreateInstance()
			{
				return new MyPlanterDefinition();
			}

			MyPlanterDefinition IActivator<MyPlanterDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
