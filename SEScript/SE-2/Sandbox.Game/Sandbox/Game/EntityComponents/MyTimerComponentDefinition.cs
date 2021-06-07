using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_TimerComponentDefinition), null)]
	public class MyTimerComponentDefinition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyTimerComponentDefinition_003C_003EActor : IActivator, IActivator<MyTimerComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTimerComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTimerComponentDefinition CreateInstance()
			{
				return new MyTimerComponentDefinition();
			}

			MyTimerComponentDefinition IActivator<MyTimerComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float TimeToRemoveMin;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TimerComponentDefinition myObjectBuilder_TimerComponentDefinition = builder as MyObjectBuilder_TimerComponentDefinition;
			TimeToRemoveMin = myObjectBuilder_TimerComponentDefinition.TimeToRemoveMin;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_TimerComponentDefinition obj = base.GetObjectBuilder() as MyObjectBuilder_TimerComponentDefinition;
			obj.TimeToRemoveMin = TimeToRemoveMin;
			return obj;
		}
	}
}
