using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WarheadDefinition), null)]
	public class MyWarheadDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyWarheadDefinition_003C_003EActor : IActivator, IActivator<MyWarheadDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWarheadDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWarheadDefinition CreateInstance()
			{
				return new MyWarheadDefinition();
			}

			MyWarheadDefinition IActivator<MyWarheadDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float ExplosionRadius;

		public float WarheadExplosionDamage;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WarheadDefinition myObjectBuilder_WarheadDefinition = (MyObjectBuilder_WarheadDefinition)builder;
			ExplosionRadius = myObjectBuilder_WarheadDefinition.ExplosionRadius;
			WarheadExplosionDamage = myObjectBuilder_WarheadDefinition.WarheadExplosionDamage;
		}
	}
}
