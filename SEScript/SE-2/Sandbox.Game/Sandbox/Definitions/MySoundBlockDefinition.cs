using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SoundBlockDefinition), null)]
	public class MySoundBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MySoundBlockDefinition_003C_003EActor : IActivator, IActivator<MySoundBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySoundBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySoundBlockDefinition CreateInstance()
			{
				return new MySoundBlockDefinition();
			}

			MySoundBlockDefinition IActivator<MySoundBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float MinRange;

		public float MaxRange;

		public float MaxLoopPeriod;

		public int EmitterNumber;

		public int LoopUpdateThreshold;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SoundBlockDefinition myObjectBuilder_SoundBlockDefinition = (MyObjectBuilder_SoundBlockDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_SoundBlockDefinition.ResourceSinkGroup);
			MinRange = myObjectBuilder_SoundBlockDefinition.MinRange;
			MaxRange = myObjectBuilder_SoundBlockDefinition.MaxRange;
			MaxLoopPeriod = myObjectBuilder_SoundBlockDefinition.MaxLoopPeriod;
			EmitterNumber = myObjectBuilder_SoundBlockDefinition.EmitterNumber;
			LoopUpdateThreshold = myObjectBuilder_SoundBlockDefinition.LoopUpdateThreshold;
		}
	}
}
