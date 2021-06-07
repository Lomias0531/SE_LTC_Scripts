using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DoorDefinition), null)]
	public class MyDoorDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyDoorDefinition_003C_003EActor : IActivator, IActivator<MyDoorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDoorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDoorDefinition CreateInstance()
			{
				return new MyDoorDefinition();
			}

			MyDoorDefinition IActivator<MyDoorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ResourceSinkGroup;

		public float MaxOpen;

		public string OpenSound;

		public string CloseSound;

		public float OpeningSpeed;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_DoorDefinition myObjectBuilder_DoorDefinition = builder as MyObjectBuilder_DoorDefinition;
			ResourceSinkGroup = myObjectBuilder_DoorDefinition.ResourceSinkGroup;
			MaxOpen = myObjectBuilder_DoorDefinition.MaxOpen;
			OpenSound = myObjectBuilder_DoorDefinition.OpenSound;
			CloseSound = myObjectBuilder_DoorDefinition.CloseSound;
			OpeningSpeed = myObjectBuilder_DoorDefinition.OpeningSpeed;
		}
	}
}
