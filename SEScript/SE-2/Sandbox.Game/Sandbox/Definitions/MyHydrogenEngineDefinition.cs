using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_HydrogenEngineDefinition), null)]
	public class MyHydrogenEngineDefinition : MyGasFueledPowerProducerDefinition
	{
		private class Sandbox_Definitions_MyHydrogenEngineDefinition_003C_003EActor : IActivator, IActivator<MyHydrogenEngineDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyHydrogenEngineDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHydrogenEngineDefinition CreateInstance()
			{
				return new MyHydrogenEngineDefinition();
			}

			MyHydrogenEngineDefinition IActivator<MyHydrogenEngineDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float AnimationSpeed;

		public float PistonAnimationMin;

		public float PistonAnimationMax;

		public float AnimationSpinUpSpeed;

		public float AnimationSpinDownSpeed;

		public float[] PistonAnimationOffsets;

		public float AnimationVisibilityDistanceSq;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_HydrogenEngineDefinition myObjectBuilder_HydrogenEngineDefinition = (MyObjectBuilder_HydrogenEngineDefinition)builder;
			AnimationSpeed = myObjectBuilder_HydrogenEngineDefinition.AnimationSpeed;
			PistonAnimationMin = myObjectBuilder_HydrogenEngineDefinition.PistonAnimationMin;
			PistonAnimationMax = myObjectBuilder_HydrogenEngineDefinition.PistonAnimationMax;
			AnimationSpinUpSpeed = myObjectBuilder_HydrogenEngineDefinition.AnimationSpinUpSpeed;
			AnimationSpinDownSpeed = myObjectBuilder_HydrogenEngineDefinition.AnimationSpinDownSpeed;
			PistonAnimationOffsets = myObjectBuilder_HydrogenEngineDefinition.PistonAnimationOffsets;
			AnimationVisibilityDistanceSq = myObjectBuilder_HydrogenEngineDefinition.AnimationVisibilityDistance * myObjectBuilder_HydrogenEngineDefinition.AnimationVisibilityDistance;
		}
	}
}
