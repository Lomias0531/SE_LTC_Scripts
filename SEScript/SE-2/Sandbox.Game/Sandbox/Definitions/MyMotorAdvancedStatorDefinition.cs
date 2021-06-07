using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MotorAdvancedStatorDefinition), null)]
	public class MyMotorAdvancedStatorDefinition : MyMotorStatorDefinition
	{
		private class Sandbox_Definitions_MyMotorAdvancedStatorDefinition_003C_003EActor : IActivator, IActivator<MyMotorAdvancedStatorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMotorAdvancedStatorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMotorAdvancedStatorDefinition CreateInstance()
			{
				return new MyMotorAdvancedStatorDefinition();
			}

			MyMotorAdvancedStatorDefinition IActivator<MyMotorAdvancedStatorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
		}
	}
}
