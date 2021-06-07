using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_LightingBlockDefinition), null)]
	public class MyLightingBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyLightingBlockDefinition_003C_003EActor : IActivator, IActivator<MyLightingBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyLightingBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLightingBlockDefinition CreateInstance()
			{
				return new MyLightingBlockDefinition();
			}

			MyLightingBlockDefinition IActivator<MyLightingBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyBounds LightRadius;

		public MyBounds LightReflectorRadius;

		public MyBounds LightFalloff;

		public MyBounds LightIntensity;

		public MyBounds LightOffset;

		public MyBounds BlinkIntervalSeconds;

		public MyBounds BlinkLenght;

		public MyBounds BlinkOffset;

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public string Flare;

		public float ReflectorConeDegrees;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_LightingBlockDefinition myObjectBuilder_LightingBlockDefinition = (MyObjectBuilder_LightingBlockDefinition)builder;
			BlinkIntervalSeconds = myObjectBuilder_LightingBlockDefinition.LightBlinkIntervalSeconds;
			BlinkLenght = myObjectBuilder_LightingBlockDefinition.LightBlinkLenght;
			BlinkOffset = myObjectBuilder_LightingBlockDefinition.LightBlinkOffset;
			LightRadius = myObjectBuilder_LightingBlockDefinition.LightRadius;
			LightReflectorRadius = myObjectBuilder_LightingBlockDefinition.LightReflectorRadius;
			LightFalloff = myObjectBuilder_LightingBlockDefinition.LightFalloff;
			LightIntensity = myObjectBuilder_LightingBlockDefinition.LightIntensity;
			LightOffset = myObjectBuilder_LightingBlockDefinition.LightOffset;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_LightingBlockDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_LightingBlockDefinition.RequiredPowerInput;
			Flare = myObjectBuilder_LightingBlockDefinition.Flare;
		}
	}
}
