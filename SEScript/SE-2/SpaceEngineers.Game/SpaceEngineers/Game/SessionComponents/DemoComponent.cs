using SpaceEngineers.Game.Definitions;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;

namespace SpaceEngineers.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class DemoComponent : MySessionComponentBase
	{
		public override bool IsRequiredByGame => false;

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			_ = (MyDemoComponentDefinition)definition;
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
		}
	}
}
