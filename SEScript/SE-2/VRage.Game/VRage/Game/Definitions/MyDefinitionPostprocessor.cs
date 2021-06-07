using System;
using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.Definitions
{
	public abstract class MyDefinitionPostprocessor
	{
		public struct Bundle
		{
			public MyModContext Context;

			public MyDefinitionSet Set;

			public Dictionary<MyStringHash, MyDefinitionBase> Definitions;
		}

		public class PostprocessorComparer : IComparer<MyDefinitionPostprocessor>
		{
			public int Compare(MyDefinitionPostprocessor x, MyDefinitionPostprocessor y)
			{
				return y.Priority - x.Priority;
			}
		}

		public Type DefinitionType;

		public static PostprocessorComparer Comparer = new PostprocessorComparer();

		public virtual int Priority => 500;

		public abstract void AfterLoaded(ref Bundle definitions);

		public abstract void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions);

		public virtual void OverrideBy(ref Bundle currentDefinitions, ref Bundle overrideBySet)
		{
			foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in overrideBySet.Definitions)
			{
				if (definition.Value.Enabled)
				{
					currentDefinitions.Definitions[definition.Key] = definition.Value;
				}
				else
				{
					currentDefinitions.Definitions.Remove(definition.Key);
				}
			}
		}
	}
}
