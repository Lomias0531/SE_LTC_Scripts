using ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PackageDefinition), null)]
	public class MyPackageDefinition : MyPhysicalItemDefinition
	{
		private class Sandbox_Definitions_MyPackageDefinition_003C_003EActor : IActivator, IActivator<MyPackageDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPackageDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPackageDefinition CreateInstance()
			{
				return new MyPackageDefinition();
			}

			MyPackageDefinition IActivator<MyPackageDefinition>.CreateInstance()
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
