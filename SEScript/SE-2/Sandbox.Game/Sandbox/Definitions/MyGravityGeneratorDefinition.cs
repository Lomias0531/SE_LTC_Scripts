using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GravityGeneratorDefinition), null)]
	public class MyGravityGeneratorDefinition : MyGravityGeneratorBaseDefinition
	{
		private class Sandbox_Definitions_MyGravityGeneratorDefinition_003C_003EActor : IActivator, IActivator<MyGravityGeneratorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGravityGeneratorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGravityGeneratorDefinition CreateInstance()
			{
				return new MyGravityGeneratorDefinition();
			}

			MyGravityGeneratorDefinition IActivator<MyGravityGeneratorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float RequiredPowerInput;

		public Vector3 MinFieldSize;

		public Vector3 MaxFieldSize;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GravityGeneratorDefinition myObjectBuilder_GravityGeneratorDefinition = builder as MyObjectBuilder_GravityGeneratorDefinition;
			RequiredPowerInput = myObjectBuilder_GravityGeneratorDefinition.RequiredPowerInput;
			MinFieldSize = myObjectBuilder_GravityGeneratorDefinition.MinFieldSize;
			MaxFieldSize = myObjectBuilder_GravityGeneratorDefinition.MaxFieldSize;
		}
	}
}
