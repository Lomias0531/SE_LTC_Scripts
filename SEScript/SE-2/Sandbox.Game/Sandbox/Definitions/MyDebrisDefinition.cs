using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DebrisDefinition), null)]
	public class MyDebrisDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyDebrisDefinition_003C_003EActor : IActivator, IActivator<MyDebrisDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebrisDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebrisDefinition CreateInstance()
			{
				return new MyDebrisDefinition();
			}

			MyDebrisDefinition IActivator<MyDebrisDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Model;

		public MyDebrisType Type;

		public float MinAmount;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_DebrisDefinition myObjectBuilder_DebrisDefinition = builder as MyObjectBuilder_DebrisDefinition;
			Model = myObjectBuilder_DebrisDefinition.Model;
			Type = myObjectBuilder_DebrisDefinition.Type;
			MinAmount = myObjectBuilder_DebrisDefinition.MinAmount;
		}
	}
}
