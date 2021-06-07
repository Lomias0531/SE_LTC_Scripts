using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AdvancedDoorDefinition), null)]
	public class MyAdvancedDoorDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyAdvancedDoorDefinition_003C_003EActor : IActivator, IActivator<MyAdvancedDoorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAdvancedDoorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAdvancedDoorDefinition CreateInstance()
			{
				return new MyAdvancedDoorDefinition();
			}

			MyAdvancedDoorDefinition IActivator<MyAdvancedDoorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float PowerConsumptionIdle;

		public float PowerConsumptionMoving;

		public MyObjectBuilder_AdvancedDoorDefinition.SubpartDefinition[] Subparts;

		public MyObjectBuilder_AdvancedDoorDefinition.Opening[] OpeningSequence;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AdvancedDoorDefinition myObjectBuilder_AdvancedDoorDefinition = builder as MyObjectBuilder_AdvancedDoorDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_AdvancedDoorDefinition.ResourceSinkGroup);
			PowerConsumptionIdle = myObjectBuilder_AdvancedDoorDefinition.PowerConsumptionIdle;
			PowerConsumptionMoving = myObjectBuilder_AdvancedDoorDefinition.PowerConsumptionMoving;
			Subparts = myObjectBuilder_AdvancedDoorDefinition.Subparts;
			OpeningSequence = myObjectBuilder_AdvancedDoorDefinition.OpeningSequence;
		}
	}
}
