using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	public class MyProceduralEnvironmentDefinitionPostprocessor : MyDefinitionPostprocessor
	{
		public override void AfterLoaded(ref Bundle definitions)
		{
		}

		public override void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions)
		{
			foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in definitions)
			{
				((MyProceduralEnvironmentDefinition)definition.Value).Prepare();
			}
		}
	}
}
