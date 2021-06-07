using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ReflectorBlockDefinition), null)]
	public class MyReflectorBlockDefinition : MyLightingBlockDefinition
	{
		private class Sandbox_Definitions_MyReflectorBlockDefinition_003C_003EActor : IActivator, IActivator<MyReflectorBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyReflectorBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyReflectorBlockDefinition CreateInstance()
			{
				return new MyReflectorBlockDefinition();
			}

			MyReflectorBlockDefinition IActivator<MyReflectorBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ReflectorTexture;

		public string ReflectorConeMaterial;

		public float ReflectorThickness;

		public MyBounds RotationSpeedBounds;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ReflectorBlockDefinition myObjectBuilder_ReflectorBlockDefinition = (MyObjectBuilder_ReflectorBlockDefinition)builder;
			ReflectorTexture = myObjectBuilder_ReflectorBlockDefinition.ReflectorTexture;
			ReflectorConeMaterial = myObjectBuilder_ReflectorBlockDefinition.ReflectorConeMaterial;
			ReflectorThickness = myObjectBuilder_ReflectorBlockDefinition.ReflectorThickness;
			ReflectorConeDegrees = myObjectBuilder_ReflectorBlockDefinition.ReflectorConeDegrees;
			RotationSpeedBounds = myObjectBuilder_ReflectorBlockDefinition.RotationSpeedBounds;
		}
	}
}
