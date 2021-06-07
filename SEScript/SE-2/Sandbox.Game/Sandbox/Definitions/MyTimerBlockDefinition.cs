using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_TimerBlockDefinition), null)]
	public class MyTimerBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyTimerBlockDefinition_003C_003EActor : IActivator, IActivator<MyTimerBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTimerBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTimerBlockDefinition CreateInstance()
			{
				return new MyTimerBlockDefinition();
			}

			MyTimerBlockDefinition IActivator<MyTimerBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public string TimerSoundStart;

		public string TimerSoundMid;

		public string TimerSoundEnd;

		public int MinDelay;

		public int MaxDelay;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TimerBlockDefinition myObjectBuilder_TimerBlockDefinition = (MyObjectBuilder_TimerBlockDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_TimerBlockDefinition.ResourceSinkGroup);
			TimerSoundStart = myObjectBuilder_TimerBlockDefinition.TimerSoundStart;
			TimerSoundMid = myObjectBuilder_TimerBlockDefinition.TimerSoundMid;
			TimerSoundEnd = myObjectBuilder_TimerBlockDefinition.TimerSoundEnd;
			MinDelay = myObjectBuilder_TimerBlockDefinition.MinDelay;
			MaxDelay = myObjectBuilder_TimerBlockDefinition.MaxDelay;
		}
	}
}
