using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipControllerDefinition), null)]
	public class MyShipControllerDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyShipControllerDefinition_003C_003EActor : IActivator, IActivator<MyShipControllerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipControllerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipControllerDefinition CreateInstance()
			{
				return new MyShipControllerDefinition();
			}

			MyShipControllerDefinition IActivator<MyShipControllerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool EnableFirstPerson;

		public bool EnableShipControl;

		public bool EnableBuilderCockpit;

		public string GlassModel;

		public string InteriorModel;

		public string CharacterAnimation;

		public string GetInSound;

		public string GetOutSound;

		public Vector3D RaycastOffset = Vector3D.Zero;

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ShipControllerDefinition myObjectBuilder_ShipControllerDefinition = builder as MyObjectBuilder_ShipControllerDefinition;
			EnableFirstPerson = myObjectBuilder_ShipControllerDefinition.EnableFirstPerson;
			EnableShipControl = myObjectBuilder_ShipControllerDefinition.EnableShipControl;
			EnableBuilderCockpit = myObjectBuilder_ShipControllerDefinition.EnableBuilderCockpit;
			GetInSound = myObjectBuilder_ShipControllerDefinition.GetInSound;
			GetOutSound = myObjectBuilder_ShipControllerDefinition.GetOutSound;
			RaycastOffset = myObjectBuilder_ShipControllerDefinition.RaycastOffset;
		}
	}
}
