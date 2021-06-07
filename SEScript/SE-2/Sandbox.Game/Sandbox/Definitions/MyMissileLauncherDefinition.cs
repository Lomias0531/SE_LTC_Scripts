using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MissileLauncherDefinition), null)]
	public class MyMissileLauncherDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyMissileLauncherDefinition_003C_003EActor : IActivator, IActivator<MyMissileLauncherDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMissileLauncherDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMissileLauncherDefinition CreateInstance()
			{
				return new MyMissileLauncherDefinition();
			}

			MyMissileLauncherDefinition IActivator<MyMissileLauncherDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ProjectileMissile;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MissileLauncherDefinition myObjectBuilder_MissileLauncherDefinition = (MyObjectBuilder_MissileLauncherDefinition)builder;
			ProjectileMissile = myObjectBuilder_MissileLauncherDefinition.ProjectileMissile;
		}
	}
}
