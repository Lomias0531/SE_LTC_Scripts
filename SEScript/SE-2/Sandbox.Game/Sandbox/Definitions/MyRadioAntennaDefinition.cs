using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_RadioAntennaDefinition), null)]
	public class MyRadioAntennaDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyRadioAntennaDefinition_003C_003EActor : IActivator, IActivator<MyRadioAntennaDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyRadioAntennaDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRadioAntennaDefinition CreateInstance()
			{
				return new MyRadioAntennaDefinition();
			}

			MyRadioAntennaDefinition IActivator<MyRadioAntennaDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float MaxBroadcastRadius;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_RadioAntennaDefinition myObjectBuilder_RadioAntennaDefinition = (MyObjectBuilder_RadioAntennaDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_RadioAntennaDefinition.ResourceSinkGroup);
			MaxBroadcastRadius = myObjectBuilder_RadioAntennaDefinition.MaxBroadcastRadius;
		}
	}
}
