using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ConveyorSorterDefinition), null)]
	public class MyConveyorSorterDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyConveyorSorterDefinition_003C_003EActor : IActivator, IActivator<MyConveyorSorterDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyConveyorSorterDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyConveyorSorterDefinition CreateInstance()
			{
				return new MyConveyorSorterDefinition();
			}

			MyConveyorSorterDefinition IActivator<MyConveyorSorterDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float PowerInput;

		public Vector3 InventorySize;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ConveyorSorterDefinition myObjectBuilder_ConveyorSorterDefinition = (MyObjectBuilder_ConveyorSorterDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ConveyorSorterDefinition.ResourceSinkGroup);
			PowerInput = myObjectBuilder_ConveyorSorterDefinition.PowerInput;
			InventorySize = myObjectBuilder_ConveyorSorterDefinition.InventorySize;
		}
	}
}
