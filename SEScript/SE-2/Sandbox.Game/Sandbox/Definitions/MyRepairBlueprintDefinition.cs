using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_RepairBlueprintDefinition), null)]
	public class MyRepairBlueprintDefinition : MyBlueprintDefinition
	{
		private class Sandbox_Definitions_MyRepairBlueprintDefinition_003C_003EActor : IActivator, IActivator<MyRepairBlueprintDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyRepairBlueprintDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRepairBlueprintDefinition CreateInstance()
			{
				return new MyRepairBlueprintDefinition();
			}

			MyRepairBlueprintDefinition IActivator<MyRepairBlueprintDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float RepairAmount;

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
			MyObjectBuilder_RepairBlueprintDefinition myObjectBuilder_RepairBlueprintDefinition = ob as MyObjectBuilder_RepairBlueprintDefinition;
			RepairAmount = myObjectBuilder_RepairBlueprintDefinition.RepairAmount;
		}
	}
}
