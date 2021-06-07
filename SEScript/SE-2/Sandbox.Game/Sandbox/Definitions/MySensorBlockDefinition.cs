using Sandbox.Common.ObjectBuilders.Definitions;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SensorBlockDefinition), null)]
	public class MySensorBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MySensorBlockDefinition_003C_003EActor : IActivator, IActivator<MySensorBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySensorBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySensorBlockDefinition CreateInstance()
			{
				return new MySensorBlockDefinition();
			}

			MySensorBlockDefinition IActivator<MySensorBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public float MaxRange;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SensorBlockDefinition myObjectBuilder_SensorBlockDefinition = builder as MyObjectBuilder_SensorBlockDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_SensorBlockDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_SensorBlockDefinition.RequiredPowerInput;
			MaxRange = Math.Max(myObjectBuilder_SensorBlockDefinition.MaxRange, 1f);
		}
	}
}
