using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AirtightDoorGenericDefinition), null)]
	public class MyAirtightDoorGenericDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyAirtightDoorGenericDefinition_003C_003EActor : IActivator, IActivator<MyAirtightDoorGenericDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAirtightDoorGenericDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAirtightDoorGenericDefinition CreateInstance()
			{
				return new MyAirtightDoorGenericDefinition();
			}

			MyAirtightDoorGenericDefinition IActivator<MyAirtightDoorGenericDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ResourceSinkGroup;

		public float PowerConsumptionIdle;

		public float PowerConsumptionMoving;

		public float OpeningSpeed;

		public string Sound;

		public string OpenSound;

		public string CloseSound;

		public float SubpartMovementDistance = 2.5f;

		protected override void Init(MyObjectBuilder_DefinitionBase builderBase)
		{
			base.Init(builderBase);
			MyObjectBuilder_AirtightDoorGenericDefinition myObjectBuilder_AirtightDoorGenericDefinition = builderBase as MyObjectBuilder_AirtightDoorGenericDefinition;
			ResourceSinkGroup = myObjectBuilder_AirtightDoorGenericDefinition.ResourceSinkGroup;
			PowerConsumptionIdle = myObjectBuilder_AirtightDoorGenericDefinition.PowerConsumptionIdle;
			PowerConsumptionMoving = myObjectBuilder_AirtightDoorGenericDefinition.PowerConsumptionMoving;
			OpeningSpeed = myObjectBuilder_AirtightDoorGenericDefinition.OpeningSpeed;
			Sound = myObjectBuilder_AirtightDoorGenericDefinition.Sound;
			OpenSound = myObjectBuilder_AirtightDoorGenericDefinition.OpenSound;
			CloseSound = myObjectBuilder_AirtightDoorGenericDefinition.CloseSound;
			SubpartMovementDistance = myObjectBuilder_AirtightDoorGenericDefinition.SubpartMovementDistance;
		}
	}
}
