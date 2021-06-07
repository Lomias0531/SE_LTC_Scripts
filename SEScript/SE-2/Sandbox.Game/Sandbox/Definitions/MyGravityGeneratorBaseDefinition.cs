using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GravityGeneratorBaseDefinition), null)]
	public class MyGravityGeneratorBaseDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyGravityGeneratorBaseDefinition_003C_003EActor : IActivator, IActivator<MyGravityGeneratorBaseDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGravityGeneratorBaseDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGravityGeneratorBaseDefinition CreateInstance()
			{
				return new MyGravityGeneratorBaseDefinition();
			}

			MyGravityGeneratorBaseDefinition IActivator<MyGravityGeneratorBaseDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float MinGravityAcceleration;

		public float MaxGravityAcceleration;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GravityGeneratorBaseDefinition myObjectBuilder_GravityGeneratorBaseDefinition = builder as MyObjectBuilder_GravityGeneratorBaseDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_GravityGeneratorBaseDefinition.ResourceSinkGroup);
			MinGravityAcceleration = myObjectBuilder_GravityGeneratorBaseDefinition.MinGravityAcceleration;
			MaxGravityAcceleration = myObjectBuilder_GravityGeneratorBaseDefinition.MaxGravityAcceleration;
		}
	}
}
