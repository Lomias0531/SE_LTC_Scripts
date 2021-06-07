using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ResourceDistributionGroup), null)]
	public class MyResourceDistributionGroupDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyResourceDistributionGroupDefinition_003C_003EActor : IActivator, IActivator<MyResourceDistributionGroupDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyResourceDistributionGroupDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyResourceDistributionGroupDefinition CreateInstance()
			{
				return new MyResourceDistributionGroupDefinition();
			}

			MyResourceDistributionGroupDefinition IActivator<MyResourceDistributionGroupDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int Priority;

		public bool IsSource;

		public bool IsAdaptible;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ResourceDistributionGroup myObjectBuilder_ResourceDistributionGroup = builder as MyObjectBuilder_ResourceDistributionGroup;
			IsSource = myObjectBuilder_ResourceDistributionGroup.IsSource;
			Priority = myObjectBuilder_ResourceDistributionGroup.Priority;
			IsAdaptible = myObjectBuilder_ResourceDistributionGroup.IsAdaptible;
		}
	}
}
