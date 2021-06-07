using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_LandingGearDefinition), null)]
	public class MyLandingGearDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyLandingGearDefinition_003C_003EActor : IActivator, IActivator<MyLandingGearDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyLandingGearDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLandingGearDefinition CreateInstance()
			{
				return new MyLandingGearDefinition();
			}

			MyLandingGearDefinition IActivator<MyLandingGearDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string LockSound;

		public string UnlockSound;

		public string FailedAttachSound;

		public float MaxLockSeparatingVelocity;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_LandingGearDefinition myObjectBuilder_LandingGearDefinition = builder as MyObjectBuilder_LandingGearDefinition;
			LockSound = myObjectBuilder_LandingGearDefinition.LockSound;
			UnlockSound = myObjectBuilder_LandingGearDefinition.UnlockSound;
			FailedAttachSound = myObjectBuilder_LandingGearDefinition.FailedAttachSound;
			MaxLockSeparatingVelocity = myObjectBuilder_LandingGearDefinition.MaxLockSeparatingVelocity;
		}
	}
}
