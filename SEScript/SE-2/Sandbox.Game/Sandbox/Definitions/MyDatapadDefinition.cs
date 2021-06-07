using Sandbox.Common.ObjectBuilders.Definitions;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DatapadDefinition), null)]
	public class MyDatapadDefinition : MyPhysicalItemDefinition
	{
		private class Sandbox_Definitions_MyDatapadDefinition_003C_003EActor : IActivator, IActivator<MyDatapadDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDatapadDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDatapadDefinition CreateInstance()
			{
				return new MyDatapadDefinition();
			}

			MyDatapadDefinition IActivator<MyDatapadDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
		}

		internal override string GetTooltipDisplayName(MyObjectBuilder_PhysicalObject content)
		{
			MyObjectBuilder_Datapad myObjectBuilder_Datapad = content as MyObjectBuilder_Datapad;
			if (string.IsNullOrEmpty(myObjectBuilder_Datapad.Name))
			{
				return base.GetTooltipDisplayName(content) + "\n" + MyTexts.GetString(MyCommonTexts.Datapad_InventoryItem_TTIP);
			}
			return myObjectBuilder_Datapad.Name + "\n" + MyTexts.GetString(MyCommonTexts.Datapad_InventoryItem_TTIP);
		}
	}
}
