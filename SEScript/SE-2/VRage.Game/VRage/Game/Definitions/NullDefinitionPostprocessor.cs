using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.Definitions
{
	public class NullDefinitionPostprocessor : MyDefinitionPostprocessor
	{
		public override void AfterLoaded(ref Bundle definitions)
		{
		}

		public override void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions)
		{
		}
	}
}
