using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_JumpDriveDefinition), null)]
	public class MyJumpDriveDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyJumpDriveDefinition_003C_003EActor : IActivator, IActivator<MyJumpDriveDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyJumpDriveDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyJumpDriveDefinition CreateInstance()
			{
				return new MyJumpDriveDefinition();
			}

			MyJumpDriveDefinition IActivator<MyJumpDriveDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public float PowerNeededForJump;

		public double MaxJumpDistance;

		public double MaxJumpMass;

		public float JumpDelay;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_JumpDriveDefinition myObjectBuilder_JumpDriveDefinition = builder as MyObjectBuilder_JumpDriveDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_JumpDriveDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_JumpDriveDefinition.RequiredPowerInput;
			PowerNeededForJump = myObjectBuilder_JumpDriveDefinition.PowerNeededForJump;
			MaxJumpDistance = myObjectBuilder_JumpDriveDefinition.MaxJumpDistance;
			MaxJumpMass = myObjectBuilder_JumpDriveDefinition.MaxJumpMass;
			JumpDelay = myObjectBuilder_JumpDriveDefinition.JumpDelay;
		}
	}
}
