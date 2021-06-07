using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_LCDPanelsBlockDefinition), null)]
	public class MyLCDPanelsBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyLCDPanelsBlockDefinition_003C_003EActor : IActivator, IActivator<MyLCDPanelsBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyLCDPanelsBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLCDPanelsBlockDefinition CreateInstance()
			{
				return new MyLCDPanelsBlockDefinition();
			}

			MyLCDPanelsBlockDefinition IActivator<MyLCDPanelsBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_LCDPanelsBlockDefinition myObjectBuilder_LCDPanelsBlockDefinition = (MyObjectBuilder_LCDPanelsBlockDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_LCDPanelsBlockDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_LCDPanelsBlockDefinition.RequiredPowerInput;
			ScreenAreas = ((myObjectBuilder_LCDPanelsBlockDefinition.ScreenAreas != null) ? myObjectBuilder_LCDPanelsBlockDefinition.ScreenAreas.ToList() : null);
		}
	}
}
