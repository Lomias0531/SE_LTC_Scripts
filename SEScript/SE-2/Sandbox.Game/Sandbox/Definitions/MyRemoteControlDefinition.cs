using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_RemoteControlDefinition), null)]
	public class MyRemoteControlDefinition : MyShipControllerDefinition
	{
		private class Sandbox_Definitions_MyRemoteControlDefinition_003C_003EActor : IActivator, IActivator<MyRemoteControlDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyRemoteControlDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRemoteControlDefinition CreateInstance()
			{
				return new MyRemoteControlDefinition();
			}

			MyRemoteControlDefinition IActivator<MyRemoteControlDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_RemoteControlDefinition myObjectBuilder_RemoteControlDefinition = builder as MyObjectBuilder_RemoteControlDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_RemoteControlDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_RemoteControlDefinition.RequiredPowerInput;
		}
	}
}
