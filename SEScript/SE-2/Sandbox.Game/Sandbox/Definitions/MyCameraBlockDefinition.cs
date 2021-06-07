using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_CameraBlockDefinition), null)]
	public class MyCameraBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyCameraBlockDefinition_003C_003EActor : IActivator, IActivator<MyCameraBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCameraBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCameraBlockDefinition CreateInstance()
			{
				return new MyCameraBlockDefinition();
			}

			MyCameraBlockDefinition IActivator<MyCameraBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ResourceSinkGroup;

		public float RequiredPowerInput;

		public float RequiredChargingInput;

		public string OverlayTexture;

		public float MinFov;

		public float MaxFov;

		public float RaycastConeLimit;

		public double RaycastDistanceLimit;

		public float RaycastTimeMultiplier;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CameraBlockDefinition myObjectBuilder_CameraBlockDefinition = builder as MyObjectBuilder_CameraBlockDefinition;
			ResourceSinkGroup = myObjectBuilder_CameraBlockDefinition.ResourceSinkGroup;
			RequiredPowerInput = myObjectBuilder_CameraBlockDefinition.RequiredPowerInput;
			RequiredChargingInput = myObjectBuilder_CameraBlockDefinition.RequiredChargingInput;
			OverlayTexture = myObjectBuilder_CameraBlockDefinition.OverlayTexture;
			MinFov = myObjectBuilder_CameraBlockDefinition.MinFov;
			MaxFov = myObjectBuilder_CameraBlockDefinition.MaxFov;
			RaycastConeLimit = myObjectBuilder_CameraBlockDefinition.RaycastConeLimit;
			RaycastDistanceLimit = myObjectBuilder_CameraBlockDefinition.RaycastDistanceLimit;
			RaycastTimeMultiplier = myObjectBuilder_CameraBlockDefinition.RaycastTimeMultiplier;
		}
	}
}
