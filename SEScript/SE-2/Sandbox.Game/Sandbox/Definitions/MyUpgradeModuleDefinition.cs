using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_UpgradeModuleDefinition), null)]
	public class MyUpgradeModuleDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyUpgradeModuleDefinition_003C_003EActor : IActivator, IActivator<MyUpgradeModuleDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyUpgradeModuleDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUpgradeModuleDefinition CreateInstance()
			{
				return new MyUpgradeModuleDefinition();
			}

			MyUpgradeModuleDefinition IActivator<MyUpgradeModuleDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyUpgradeModuleInfo[] Upgrades;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_UpgradeModuleDefinition myObjectBuilder_UpgradeModuleDefinition = builder as MyObjectBuilder_UpgradeModuleDefinition;
			Upgrades = myObjectBuilder_UpgradeModuleDefinition.Upgrades;
		}
	}
}
