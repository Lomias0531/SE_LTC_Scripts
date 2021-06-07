using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GlobalEventDefinition), null)]
	public class MyGlobalEventDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyGlobalEventDefinition_003C_003EActor : IActivator, IActivator<MyGlobalEventDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGlobalEventDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGlobalEventDefinition CreateInstance()
			{
				return new MyGlobalEventDefinition();
			}

			MyGlobalEventDefinition IActivator<MyGlobalEventDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public TimeSpan? MinActivationTime;

		public TimeSpan? MaxActivationTime;

		public TimeSpan? FirstActivationTime;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			if (builder.Id.TypeId == typeof(MyObjectBuilder_GlobalEventDefinition))
			{
				builder.Id = new SerializableDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), builder.Id.SubtypeName);
			}
			base.Init(builder);
			MyObjectBuilder_GlobalEventDefinition myObjectBuilder_GlobalEventDefinition = builder as MyObjectBuilder_GlobalEventDefinition;
			if (myObjectBuilder_GlobalEventDefinition.MinActivationTimeMs.HasValue && !myObjectBuilder_GlobalEventDefinition.MaxActivationTimeMs.HasValue)
			{
				myObjectBuilder_GlobalEventDefinition.MaxActivationTimeMs = myObjectBuilder_GlobalEventDefinition.MinActivationTimeMs;
			}
			if (myObjectBuilder_GlobalEventDefinition.MaxActivationTimeMs.HasValue && !myObjectBuilder_GlobalEventDefinition.MinActivationTimeMs.HasValue)
			{
				myObjectBuilder_GlobalEventDefinition.MinActivationTimeMs = myObjectBuilder_GlobalEventDefinition.MaxActivationTimeMs;
			}
			if (myObjectBuilder_GlobalEventDefinition.MinActivationTimeMs.HasValue)
			{
				MinActivationTime = TimeSpan.FromTicks(myObjectBuilder_GlobalEventDefinition.MinActivationTimeMs.Value * 10000);
			}
			if (myObjectBuilder_GlobalEventDefinition.MaxActivationTimeMs.HasValue)
			{
				MaxActivationTime = TimeSpan.FromTicks(myObjectBuilder_GlobalEventDefinition.MaxActivationTimeMs.Value * 10000);
			}
			if (myObjectBuilder_GlobalEventDefinition.FirstActivationTimeMs.HasValue)
			{
				FirstActivationTime = TimeSpan.FromTicks(myObjectBuilder_GlobalEventDefinition.FirstActivationTimeMs.Value * 10000);
			}
		}
	}
}
