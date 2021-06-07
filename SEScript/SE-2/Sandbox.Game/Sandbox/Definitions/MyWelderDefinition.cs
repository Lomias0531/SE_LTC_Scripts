using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WelderDefinition), null)]
	internal class MyWelderDefinition : MyEngineerToolBaseDefinition
	{
		private class Sandbox_Definitions_MyWelderDefinition_003C_003EActor : IActivator, IActivator<MyWelderDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWelderDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWelderDefinition CreateInstance()
			{
				return new MyWelderDefinition();
			}

			MyWelderDefinition IActivator<MyWelderDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string FlameEffect;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WelderDefinition myObjectBuilder_WelderDefinition = builder as MyObjectBuilder_WelderDefinition;
			FlameEffect = myObjectBuilder_WelderDefinition.FlameEffect;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_WelderDefinition obj = (MyObjectBuilder_WelderDefinition)base.GetObjectBuilder();
			obj.FlameEffect = FlameEffect;
			return obj;
		}
	}
}
