using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GyroDefinition), null)]
	public class MyGyroDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyGyroDefinition_003C_003EActor : IActivator, IActivator<MyGyroDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGyroDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGyroDefinition CreateInstance()
			{
				return new MyGyroDefinition();
			}

			MyGyroDefinition IActivator<MyGyroDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ResourceSinkGroup;

		public float ForceMagnitude;

		public float RequiredPowerInput;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GyroDefinition myObjectBuilder_GyroDefinition = (MyObjectBuilder_GyroDefinition)builder;
			ResourceSinkGroup = myObjectBuilder_GyroDefinition.ResourceSinkGroup;
			ForceMagnitude = myObjectBuilder_GyroDefinition.ForceMagnitude;
			RequiredPowerInput = myObjectBuilder_GyroDefinition.RequiredPowerInput;
		}
	}
}
