using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_RealWheel))]
	public class MyRealWheel : MyMotorRotor
	{
		private class Sandbox_Game_Entities_Blocks_MyRealWheel_003C_003EActor : IActivator, IActivator<MyRealWheel>
		{
			private sealed override object CreateInstance()
			{
				return new MyRealWheel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRealWheel CreateInstance()
			{
				return new MyRealWheel();
			}

			MyRealWheel IActivator<MyRealWheel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void ContactPointCallback(ref MyGridContactInfo value)
		{
			HkContactPointProperties contactProperties = value.Event.ContactProperties;
			contactProperties.Friction = 0.85f;
			contactProperties.Restitution = 0.2f;
			value.EnableParticles = false;
			value.RubberDeformation = true;
		}
	}
}
