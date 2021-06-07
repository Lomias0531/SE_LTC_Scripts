using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_UsableItemDefinition), null)]
	public class MyUsableItemDefinition : MyPhysicalItemDefinition
	{
		private class Sandbox_Definitions_MyUsableItemDefinition_003C_003EActor : IActivator, IActivator<MyUsableItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyUsableItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUsableItemDefinition CreateInstance()
			{
				return new MyUsableItemDefinition();
			}

			MyUsableItemDefinition IActivator<MyUsableItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string UseSound;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_UsableItemDefinition myObjectBuilder_UsableItemDefinition = builder as MyObjectBuilder_UsableItemDefinition;
			UseSound = myObjectBuilder_UsableItemDefinition.UseSound;
		}
	}
}
