using Sandbox.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Definitions;

namespace SpaceEngineers.Game.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DemoComponentDefinition), null)]
	public class MyDemoComponentDefinition : MySessionComponentDefinition
	{
		public float Float;

		public int Int;

		public string String;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_DemoComponentDefinition myObjectBuilder_DemoComponentDefinition = (MyObjectBuilder_DemoComponentDefinition)builder;
			Float = myObjectBuilder_DemoComponentDefinition.Float;
			Int = myObjectBuilder_DemoComponentDefinition.Int;
			String = myObjectBuilder_DemoComponentDefinition.String;
		}
	}
}
